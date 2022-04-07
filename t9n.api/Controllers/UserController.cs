using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Communication;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using t9n.api.model;
using t9n.api.model.extension;
using t9n.DAL;
using userManagement;

namespace t9n.api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly t9nDbContext _context;
        private readonly IOptions<AppSettings> _appSettings;

        public UserController(t9nDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }
        [HttpPost("register")]
        public IActionResult Register(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                if (userRegistrationModel.Exists(_context, out var moreMessage))
                {
                    return Conflict(new ApiMessage
                        (httpStatus:409, message: "User cannot register", moreInfo: moreMessage));
                }

                if (!userRegistrationModel.Validate(out var reason))
                {
                    return BadRequest(
                        new ApiMessage(httpStatus: 403, message: "User cannot register", moreInfo: reason));
                }
                var dbUser = userRegistrationModel.ToDatabase();
                _context.Users.Add(dbUser);
                _context.SaveChanges();
                CommunicationHelper.SendConfirmationMail(userRegistrationModel.UserEmail,$"{_appSettings.Value.ConfirmationEmailUrl}?o={dbUser.UserInternalId:D}",_appSettings.Value.TemplatesPath,"en");
                return Ok(new ApiMessage (httpStatus: 200, message: $"User is registered with a {reason} password"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage (httpStatus: 500, message: "User cannot register", moreInfo: $"{ex.Message}"));
            }
        }

        [HttpPost("login")]
        public IActionResult Login(UserLoginModel userLogin)
        {
            try
            {
                if (userLogin.ValidateCredentials(_context,out var reason))
                {
                    return Ok(new ApiMessage(httpStatus: 200,message:"Login successfully"));
                }
                else
                {
                    if (!string.IsNullOrEmpty(reason)) // when password is wrong, reason remains empty
                    {
                        return Unauthorized(new ApiMessage(httpStatus: 403, message: "Login failed", moreInfo: reason));
                    }
                    return BadRequest(new ApiMessage (httpStatus: 400, message: "Login failed"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage (httpStatus: 500, message: "Cannot login", moreInfo: $"{ex.Message}"));
            }
        }

        /// <summary>
        /// ConfirmationEmail call comes from email client. During registration, an email has been sent to the user to validate his email.
        /// The link contains User internal Id (guid).
        /// This method accept a Get (rather than a Put/Post because it comes from simple email.
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        [HttpGet("confirm")]
        public IActionResult ConfirmationEmail(string o)
        {
            Guid reference = Guid.Parse(o);
            var user = _context.Users.FirstOrDefault(u => u.UserInternalId == reference);
            if (user == null) return NotFound(new ApiMessage(httpStatus: 404, message: "User unknown."));
            user.UserEmailValidated = true;
            _context.SaveChanges();
            return Ok(new ApiMessage(httpStatus: 200, message: "Account activated."));
        }
    }
}
