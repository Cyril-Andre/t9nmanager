using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using userManagement;
using System.Linq;
using t9n.DAL;
using t9n.api.model.extension;
using t9n.api.model;
using Microsoft.Extensions.Options;
using System;
using Microsoft.EntityFrameworkCore;
using Communication;
using NLog;
using Microsoft.Extensions.Logging;

namespace t9n.api.Controllers
{
    [Authorize]
    [Route("api/tenant")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private readonly AppSettings _appSettings;
        private readonly t9nDbContext _dbContext;

        public TenantController(IOptions<AppSettings> appSettings, t9nDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(ApiMessage), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult GetAllUserTenants()
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
                if (String.IsNullOrEmpty(userName))
            {
               return BadRequest(new ApiMessage(400, message: "Token invalid"));
            }

            var user = _dbContext.Users.Include(user => user.Tenants).FirstOrDefault(u => u.UserName == userName)?.ToUser();
                if (user == null)
            {
               return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));
            }

            ApiMessage result = new ApiMessage(httpStatus:200, message:"Success"){Value = user.UserTenants };
                _logger.Trace($"Get Tenant ==> {user.UserTenants.Count}");
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiMessage(500,"Unexpected error",ex.Message));
            }
        }

        [HttpPost("create/{tenantName}")]
        [ProducesResponseType( typeof( ApiMessage ), 200 )]
        [ProducesResponseType( typeof( ApiMessage ), 400 )]
        [ProducesResponseType( typeof( ApiMessage ), 404 )]
        [ProducesResponseType( typeof( ApiMessage ), 409 )]
        [ProducesResponseType( typeof( ApiMessage ), 500 )]
        public IActionResult CreateTenant(string tenantName)
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
                if ( String.IsNullOrEmpty(userName))
            {
               return BadRequest(new ApiMessage(400, message: "Token invalid"));
            }

            var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
                if (user == null)
            {
               return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));
            }
                if ( _dbContext.Tenants.FirstOrDefault( t => t.Name.ToLower() == tenantName.ToLower() ) != null )
                {
                    return Conflict( new ApiMessage( 409, $"Tenant {tenantName} cannot be created.", "Duplicate" ) );
                }
            DbTenant dbTenant = new DbTenant
                {
                    InternalId = Guid.NewGuid(),
                    Name = tenantName,
                    AdminUserName = userName,
                };
                _dbContext.Tenants.Add(dbTenant);
                user.Tenants.Add(dbTenant);
              
                _dbContext.SaveChanges();
                ApiMessage result = new ApiMessage(httpStatus: 200, message: "Success", moreInfo: "") { Value = dbTenant.ToTenant() };
                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(500, "Unexpected error", ex.Message));
            }
        }

        [HttpDelete( "{tenantKey}" )]
        [ProducesResponseType( typeof( ApiMessage ), 200 )]
        [ProducesResponseType( typeof( ApiMessage ), 404 )]
        [ProducesResponseType( typeof( ApiMessage ), 400 )]
        [ProducesResponseType( typeof( ApiMessage ), 500 )]
        public IActionResult DeleteUserTenants( string tenantKey )
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault( c => c.Type.Contains( "name" ) )?.Value;
                if ( String.IsNullOrEmpty( userName ) )
                {
                    return BadRequest( new ApiMessage( 400, message: "Token invalid" ) );
                }

                var user = _dbContext.Users.Include( user => user.Tenants ).FirstOrDefault( u => u.UserName == userName );
                if ( user == null )
                {
                    return NotFound( new ApiMessage( 404, message: $"Cannot find user {userName}" ) );
                }

                var tenant = user.Tenants.Find( t => t.InternalId.ToString( "D" ) == tenantKey );
                if ( tenant == null )
                {
                    return NotFound( new ApiMessage( 404, message: $"Cannot find tenant {tenantKey} in the tenants' list of {user.UserName}" ) );
                }

                if ( tenant.AdminUserName == user.UserName )
                {
                    var allUsers = _dbContext.Users.Include( u => u.Tenants ).Where( u => u.Tenants.Contains( tenant ) ).ToList();
                    foreach ( var dbUser in allUsers )
                    {
                        dbUser.Tenants.Remove( tenant );
                    }
                    _dbContext.Tenants.Remove( tenant );
                }
                else
                {
                    user.Tenants.Remove( tenant );
                }
                _dbContext.SaveChanges();
                return Ok( new ApiMessage( 200, "Success", "Tenant successfuly removed" ) );
            }
            catch ( Exception ex )
            {
                return StatusCode( 500, new ApiMessage( 500, "Unexpected error", ex.Message ) );
            }
        }

        [HttpPost("invite")]
        [ProducesResponseType(typeof(ApiMessage), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 409)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult InviteUser([FromBody]Invitation invitation)
        {
            string tenantKey = invitation.tenant.Id.ToString();
            string userEmail = invitation.userEmail;
            Guid tenantGuid = invitation.tenant.Id;

            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.InternalId == tenantGuid);
            if (tenant == null)
            {
                return NotFound(new ApiMessage(404, $"Tenant with Id = {tenantKey} is unknown"));
            }
            var claims = User.Claims;
            var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
            if (String.IsNullOrEmpty(userName))
         {
            return BadRequest(new ApiMessage(400, message: "Token invalid"));
         }

         var currentUser = _dbContext.Users.Include(user => user.Tenants).FirstOrDefault(u => u.UserName == userName);
            if (currentUser == null)
         {
            return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));
         }

         var invitedUser = _dbContext.Users.FirstOrDefault(u=>u.Email.ToLower()==userEmail.ToLower());
            if (invitedUser == null)
            {
                var invit = _dbContext.Invitations.FirstOrDefault(i=>i.UserEmail==userEmail && i.TenantInternalId==tenantGuid);
                if (invit == null)
                {
                    invit= new DbInvitation { TenantInternalId=tenantGuid, UserEmail=userEmail };
                    _dbContext.Invitations.Add(invit);
                    _dbContext.SaveChanges();
                    CommunicationHelper.SendInvitationMail(userEmail, _appSettings.t9nManagerUrl, currentUser.Firstname, currentUser.Lastname, tenant.Name, _appSettings.TemplatesPath, locale: "en");
                    ApiMessage result = new ApiMessage(200, $"{userEmail} invited to tenant {tenant.Name}.");
                    result.Value = tenant.ToTenant();
                    return Ok(result);
                }
                return Conflict(new ApiMessage(409, $"User {invitedUser.UserName} has already been invited to tenant {tenant.Name}"));
            }
            else
            {
                if (invitedUser.Tenants.Contains(tenant))
                {
                    return Conflict(new ApiMessage(409, $"User {invitedUser.UserName} is already part of tenant {tenant.Name}."));
                }
                invitedUser.Tenants.Add(tenant);
                _dbContext.SaveChanges();
                ApiMessage result = new ApiMessage(200, $"{invitedUser.UserName} added to tenant {tenant.Name}.");
                result.Value = tenant.ToTenant();
                return Ok(result);
            }
        }
    }
}
