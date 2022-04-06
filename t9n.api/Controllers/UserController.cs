using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
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
        public UserController(t9nDbContext context)
        {
            _context = context;
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
                        new ApiMessage(httpStatus: 400, message: "User cannot register", moreInfo: reason));
                }
                var dbUser = userRegistrationModel.ToDatabase();
                _context.Users.Add(dbUser);
                _context.SaveChanges();
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
                if (userLogin.ValidateCredentials(_context))
                {
                    return Ok(new ApiMessage(httpStatus: 200,message:"Login successfully"));
                }
                else
                {
                    return BadRequest(new ApiMessage (httpStatus: 400, message: "Login failed"));
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage (httpStatus: 500, message: "Cannot login", moreInfo: $"{ex.Message}"));
            }
        }
    }
}
