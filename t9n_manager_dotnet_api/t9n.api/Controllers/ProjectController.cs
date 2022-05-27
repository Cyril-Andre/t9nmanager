using System;
using System.Collections.Generic;
using System.Linq;
using arb;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NLog;
using t9n.api.model;
using t9n.api.model.extension;
using t9n.DAL;
using userManagement;

namespace t9n.api.Controllers
{
    [Authorize]
    [Route( "api/project" )]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly IOptions<AppSettings> _appSettings;
        private readonly t9nDbContext _dbContext;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public ProjectController( IOptions<AppSettings> appSettings, t9nDbContext dbContext )
        {
            _appSettings = appSettings;
            _dbContext = dbContext;
        }

        [HttpGet]
        public IActionResult GetAllProjects( string tenantKey )
        {
            Guid tenantGuid = Guid.Parse( tenantKey );
            var claims = User.Claims;
            var userName = claims.FirstOrDefault( c => c.Type.Contains( "name" ) )?.Value;
            var dbTenant = _dbContext.Tenants.Include( t => t.Projects ).FirstOrDefault( ten => ten.InternalId == tenantGuid );
            if ( dbTenant == null )
            {
                return NotFound( new ApiMessage( 404, $"Tenant {tenantKey} has not been found." ));
            }
            if ( dbTenant.Projects.Count() == 0 )
            {
                return Ok( new ApiMessage( 200, "This tenant has no projects" ) { Value = new List<Project>() } );
            }
            return Ok( new ApiMessage( 200, "Success" ) { Value = dbTenant.Projects.ToProjectsList() } );
        }
        [HttpPost( "create" )]
        public IActionResult CreateProject( CreateProjectRequest createProjectRequest )
        {
            var claims = User.Claims;
            var userName = claims.FirstOrDefault( c => c.Type.Contains( "name" ) )?.Value;
            DbUser dbUser = _dbContext.Users.FirstOrDefault( u => u.UserName.ToLower() == userName.ToLower() );
            if ( dbUser == null )
            {
                return NotFound( new ApiMessage( 404, $"User {userName} not found." ) );
            }
            DbTenant dbTenant = null;
            if (createProjectRequest == null)
            {
                return BadRequest( new ApiMessage( 400, "Create project request is compulsery.", "" ) );
            }
            bool isPublicTenant = ( createProjectRequest.tenant == null ) ||
                                    String.Equals( "public", createProjectRequest.tenant.Name, StringComparison.OrdinalIgnoreCase );
            if ( isPublicTenant )
            {
                dbTenant = _dbContext.Tenants.First( t => t.Name.ToLower() == "public" );
            }
            else
            {
                dbTenant = _dbContext.Tenants.Include(t=>t.Projects).Include(t=>t.Users).FirstOrDefault( t => t.InternalId == createProjectRequest.tenant.Id );
                if ( dbTenant == null )
                {
                    return NotFound( new ApiMessage( 404, $"Tenant ${createProjectRequest.tenant.AdminUserName} not found" ) );
                }
                if ( dbTenant.Name.ToLower() != createProjectRequest.tenant.Name.ToLower() )
                {
                    return Conflict( new ApiMessage( 409, $"Tenant {createProjectRequest.tenant.Id} is not named {createProjectRequest.tenant.Name}. You must be mistaken." ) );
                }
                if ( !dbTenant.Users.Contains( dbUser ) )
                {
                    return Unauthorized( new ApiMessage( 401, $"User {userName} is not part of Tenant {dbTenant.Name}" ));
                }
                if ( dbTenant.Projects.FirstOrDefault( t => t.Name.ToLower() == createProjectRequest.projectName.ToLower() )!=null )
                {
                    return Conflict( new ApiMessage( 409, $"Project {createProjectRequest.projectName} is already existing for this tenant.." ) );
                }
            }
            var dbProject = new DbProject
            {
                Name = createProjectRequest.projectName,
                Tenant = dbTenant,
                Users = new List<DbUser> { dbUser }
            };
            dbTenant.Projects.Add( dbProject );
            dbUser.Projects.Add( dbProject );
            _dbContext.Projects.Add(dbProject );
            _dbContext.SaveChanges();
            var project = dbProject.ToProject();
            return Ok(new ApiMessage( 200, "Success" ) { Value=project} );
        }
    }
}
