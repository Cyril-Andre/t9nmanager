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
using Security;
using t9n.api.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace t9n.api.Controllers
{
    [AllowAnonymous]
    [Route("api/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly t9nDbContext _dbContext;
        private readonly IOptions<AppSettings> _appSettings;

        public UserController(t9nDbContext dbContext, IOptions<AppSettings> appSettings)
        {
            _dbContext = dbContext;
            _appSettings = appSettings;
        }
        [HttpPost("register")]
        public IActionResult Register(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                if (userRegistrationModel.Exists(_dbContext, out var moreMessage))
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
                if (userRegistrationModel.TenantId != null)
                {
                    var dbTenant = _dbContext.Tenants.FirstOrDefault(t => t.InternalId == userRegistrationModel.TenantId);
                    if (dbTenant!=null)
                        dbUser.Tenants.Add(dbTenant);
                }
                _dbContext.Users.Add(dbUser);
                //Check if an invitation is pending
                var invitations = _dbContext.Invitations.Where(i=>i.UserEmail.ToLower()==dbUser.Email.ToLower()).ToList();
                foreach(DbInvitation invit in invitations)
                {
                    var tenant = _dbContext.Tenants.FirstOrDefault(t => t.InternalId == invit.TenantInternalId);
                    if (tenant != null)
                    {
                        dbUser.Tenants.Add(tenant);
                        //tenant.Users.Add(dbUser);
                        _dbContext.Invitations.Remove(invit);
                    }
                }
                _dbContext.SaveChanges();
                CommunicationHelper.SendConfirmationMail(userRegistrationModel.UserEmail,$"{_appSettings.Value.ConfirmationEmailUrl}?o={dbUser.InternalId:D}",_appSettings.Value.TemplatesPath,"en");
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
                var user = userLogin.ValidateCredentials(_dbContext, out var reason);
                if (user!=null)
                {
                    string token = TokenHelper.CreateToken(_appSettings.Value, user);
                    return Ok(new ApiMessage(httpStatus:200,message:token));
                }
                else
                {
                    if (!string.IsNullOrEmpty(reason)) // when password is wrong, reason remains empty
                    {
                        return Unauthorized(new ApiMessage(httpStatus: 401, message: "Login failed", moreInfo: reason));
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
            try
            {
                Guid reference = Guid.Parse(o);
                var user = _dbContext.Users.FirstOrDefault(u => u.InternalId == reference);
                if (user == null) return NotFound(new ApiMessage(httpStatus: 404, message: "User unknown."));
                user.UserEmailValidated = true;
                _dbContext.SaveChanges();
                return Ok(new ApiMessage(httpStatus: 200, message: "Account activated."));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(httpStatus: 500, message: "User cannot ne activated", moreInfo: $"{ex.Message}"));
            }
        }
    
    
        [HttpPost("startresetpassword")]
        public IActionResult StartResetPassword(UserResetPasswordModel userResetPasswordModel)
        {
            try
            {
                if (userResetPasswordModel == null || String.IsNullOrEmpty(userResetPasswordModel.UserEmail))
                    return BadRequest(new ApiMessage(httpStatus: 400, message: "User email is not valid"));
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == userResetPasswordModel.UserEmail);
                if (user == null)
                {
                    return NotFound(new ApiMessage(httpStatus: 404, message: $"Cannot find user with email {userResetPasswordModel.UserEmail}"));
                }
                string otp = OtpProvider.GenerateOtp(6, true);
                user.ResetPasswordOtp = otp;
                _dbContext.SaveChanges();
                CommunicationHelper.SendResetPasswordMail(user.Email,user.UserName, otp, _appSettings.Value.TemplatesPath, "en");
                return Ok(new ApiMessage(httpStatus: 200, message: $"Reset password OTP sent"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(httpStatus: 500, message: "User cannot reset password", moreInfo: $"{ex.Message}"));
            }

        }


        [HttpPost("finalizeresetpassword")]
        public IActionResult FinalizeResetPassword(UserResetPasswordModel userResetPasswordModel)
        {
            try
            {
                if (userResetPasswordModel == null || String.IsNullOrEmpty(userResetPasswordModel.UserEmail))
                    return BadRequest(new ApiMessage(httpStatus: 400, message: "User email is not valid"));
                if (!userResetPasswordModel.Validate(out var reason))
                {
                    return BadRequest(new ApiMessage(httpStatus: 400, message: "User email is not valid",moreInfo:reason));
                }
                var user = _dbContext.Users.FirstOrDefault(u => u.Email == userResetPasswordModel.UserEmail);
                if (user == null)
                {
                    return NotFound(new ApiMessage(httpStatus: 404, message: $"Cannot find user with email {userResetPasswordModel.UserEmail}"));
                }
                if (!string.Equals(userResetPasswordModel.Otp, user.ResetPasswordOtp, StringComparison.Ordinal))
                {
                    return Unauthorized(new ApiMessage(httpStatus: 401, message: $"User password cannot be reintialized"));
                }
                var dbUser = userResetPasswordModel.ToDatabase(_dbContext);
                _dbContext.SaveChanges();
                return Ok(new ApiMessage(httpStatus: 200, message: $"Password reset"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(httpStatus: 500, message: "Password cannot be reset", moreInfo: $"{ex.Message}"));
            }

        }
    }
}
