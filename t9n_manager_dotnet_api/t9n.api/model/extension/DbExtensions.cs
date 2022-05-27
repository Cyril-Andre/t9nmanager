using arb;
using System.Collections.Generic;
using t9n.DAL;
using userManagement;

namespace t9n.api.model.extension
{
    public static class DbExtensions
    {
        public static User ToUser(this DbUser dbUser)
        {
            if ( dbUser == null )
            {
                return null;
            }

            var user = new User
            {
                FirstName = dbUser.Firstname,
                LastName = dbUser.Lastname,
                UserName = dbUser.UserName,
                UserEmail = dbUser.Email,
                UserBirthDate = dbUser.Birthdate,
                UserTenants = dbUser.Tenants.ToTenantsList()
            };
            return user;
        }
        public static List<User> ToUsersList(this List<DbUser> listDbUser)
        {
            if ( listDbUser == null )
            {
                return null;
            }

            List<User> list = new List<User>();
            foreach ( DbUser dbUser in listDbUser )
            {
                var user = new User
                {
                    FirstName = dbUser.Firstname,
                    LastName = dbUser.Lastname,
                    UserName = dbUser.UserName,
                    UserEmail = dbUser.Email,
                    UserBirthDate = dbUser.Birthdate,
                    UserTenants = dbUser.Tenants.ToTenantsList()
                };
                list.Add( user );
            }
            return list;
        }
        public static Tenant ToTenant(this DbTenant dbTenant)
        {
            if ( dbTenant == null )
            {
                return null;
            }

            var tenant = new Tenant
            {
                Id = dbTenant.InternalId,
                Name = dbTenant.Name,
                AdminUserName = dbTenant.AdminUserName
            };
            return tenant;
        }

        public static List<Tenant> ToTenantsList(this List<DbTenant> listDbTenant)
        {
            if ( listDbTenant == null )
            {
                return null;
            }

            List<Tenant> list = new List<Tenant>();
            foreach ( DbTenant t in listDbTenant )
            {
                Tenant tenant = new Tenant
                {
                    Id = t.InternalId,
                    Name = t.Name,
                    AdminUserName = t.AdminUserName
                };
                list.Add( tenant );

            }
            return list;
        }

        public static Project ToProject(this DbProject dbProject)
        {
            if ( dbProject == null )
            {
                return null;
            }

            var project = new Project
            {
                Id = dbProject.InternalId,
                Name = dbProject.Name,
                Tenant = dbProject.Tenant.ToTenant(),
                Users = dbProject.Users.ToUsersList()
            };
            return project;
        }

        public static List<Project> ToProjectsList(this List<DbProject> listDbProjects)
        {
            if ( listDbProjects == null )
            {
                return null;
            }

            List<Project> list = new List<Project>();
            foreach ( DbProject dbProject in listDbProjects )
            {
                var project = new Project
                {
                    Id = dbProject.InternalId,
                    Name = dbProject.Name,
                    Tenant = dbProject.Tenant.ToTenant(),
                    Users = dbProject.Users.ToUsersList()
                };
                list.Add( project );
            }
            return list;
        }

    }
}
