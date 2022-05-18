﻿using Microsoft.AspNetCore.Authorization;
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

namespace t9n.api.Controllers
{
    [Authorize]
    [Route("api/tenant")]
    [ApiController]
    public class TenantController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly t9nDbContext _dbContext;

        public TenantController(IOptions<AppSettings> appSettings, t9nDbContext dbContext)
        {
            _appSettings = appSettings.Value;
            _dbContext = dbContext;
        }
        [HttpGet()]
        [ProducesResponseType(typeof(List<Tenant>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult GetAllUserTenants()
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(new ApiMessage(400, message: "Token invalid"));
                var user = _dbContext.Users.Include(user => user.UserTenants).FirstOrDefault(u => u.UserName == userName)?.ToUser();
                if (user == null)
                    return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));

                return Ok(user.UserTenants);
            }
            catch (Exception ex)
            {
                return StatusCode(500,new ApiMessage(500,"Unexpected error",ex.Message));
            }
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(Tenant), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult CreateTenant(string tenantName)
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(new ApiMessage(400, message: "Token invalid"));
                var user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                    return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));
                DbTenant dbTenant = new DbTenant
                {
                    TenantInternalId = Guid.NewGuid(),
                    TenantName = tenantName,
                    AdminUserName = userName,
                };
                _dbContext.Tenants.Add(dbTenant);
                user.UserTenants.Add(dbTenant);
                _dbContext.SaveChanges();
                return Ok(dbTenant.ToTenant());
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(500, "Unexpected error", ex.Message));
            }
        }

        [HttpDelete()]
        [ProducesResponseType(typeof(List<Tenant>), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult DeleteUserTenants(string tenantKey)
        {
            try
            {
                var claims = User.Claims;
                var userName = claims.FirstOrDefault(c => c.Type.Contains("name"))?.Value;
                if (string.IsNullOrEmpty(userName))
                    return BadRequest(new ApiMessage(400, message: "Token invalid"));
                var user = _dbContext.Users.Include(user => user.UserTenants).FirstOrDefault(u => u.UserName == userName);
                if (user == null)
                    return NotFound(new ApiMessage(404, message: $"Cannot find user {userName}"));
                var tenant = user.UserTenants.Find(t => t.TenantInternalId.ToString("D") == tenantKey);
                if (tenant==null)
                    return NotFound(new ApiMessage(404, message: $"Cannot find tenant {tenantKey} in the tenants' list of {user.UserName}"));
                if (tenant.AdminUserName == user.UserName)
                {
                    var allUsers = _dbContext.Users.Where(u => u.UserTenants.Contains(tenant)).ToList();
                    foreach (var dbUser in allUsers)
                    {
                        dbUser.UserTenants.Remove(tenant);
                    }
                }
                user.UserTenants.Remove(tenant);
                //_dbContext.SaveChanges();
                var businessUser = user.ToUser();
                return Ok(businessUser.UserTenants);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiMessage(500, "Unexpected error", ex.Message));
            }
        }

        [HttpGet("invite")]
        [ProducesResponseType(typeof(ApiMessage), 200)]
        [ProducesResponseType(typeof(ApiMessage), 404)]
        [ProducesResponseType(typeof(ApiMessage), 400)]
        [ProducesResponseType(typeof(ApiMessage), 409)]
        [ProducesResponseType(typeof(ApiMessage), 500)]
        public IActionResult InviteUser(string tenantKey, string userEmail)
        {
            Guid tenantGuid = Guid.Parse(tenantKey);
            var tenant = _dbContext.Tenants.FirstOrDefault(t => t.TenantInternalId == tenantGuid);
            if (tenant == null)
            {
                return NotFound(new ApiMessage(404, $"Tenant with Id = {tenantKey} is unknown"));
            }
            var invitedUser = _dbContext.Users.FirstOrDefault(u=>u.UserEmail.ToLower()==userEmail.ToLower());
            if (invitedUser == null)
            {
                var invit = _dbContext.Invations.FirstOrDefault(i=>i.UserEmail==userEmail && i.TenantInternalId==tenantGuid);
                if (invit == null)
                {
                    invit= new DbInvitation { TenantInternalId=tenantGuid, UserEmail=userEmail };
                    _dbContext.Invations.Add(invit);
                    _dbContext.SaveChanges();
                    ApiMessage result = new ApiMessage(200, $"{invitedUser.UserName} invited to tenant {tenant.TenantName}.");
                    result.Value = tenant.ToTenant();
                    return Ok(result);
                }
                return Conflict(new ApiMessage(409, $"User {invitedUser.UserName} has already been invited to tenant {tenant.TenantName}"));
            }
            else
            {
                if (invitedUser.UserTenants.Contains(tenant))
                {
                    return Conflict(new ApiMessage(409, $"User {invitedUser.UserName} is already part of tenant {tenant.TenantName}."));
                }
                invitedUser.UserTenants.Add(tenant);
                _dbContext.SaveChanges();
                ApiMessage result = new ApiMessage(200, $"{invitedUser.UserName} added to tenant {tenant.TenantName}.");
                result.Value = tenant.ToTenant();
                return Ok(result);
            }
        }
    }
}
