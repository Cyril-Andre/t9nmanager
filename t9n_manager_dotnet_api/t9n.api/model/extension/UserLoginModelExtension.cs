using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Security;
using t9n.DAL;
using userManagement;

namespace t9n.api.model.extension
{
    public static class UserLoginModelExtension
    {
        public static User ValidateCredentials(this UserLoginModel login, t9nDbContext context, out string reason)
        {
            reason = "";
            //Look for user with this username
            var dbUser = context.Users.FirstOrDefault(user => user.UserName.ToLower() == login.UserName.ToLower());
            if (dbUser != null)
            {
                if (!dbUser.UserEmailValidated)
                {
                    reason = "Email has not been confirmed. Account not activated.";
                    return null;
                }
                var password = login.Password;
                //Check password validity based on hash & salt 
                if (Security.PasswordHashProvider.ValidatePassword(password, ByteConverter.GetHexBytes(dbUser.Salt),
                    ByteConverter.GetHexBytes(dbUser.UserPasswordHash)))
                {
                    reason = "Login successful";
                    User user = new User { UserName = dbUser.UserName, UserEmail = dbUser.UserEmail, UserBirthDate = dbUser.UserBirthdate, UserTenants = dbUser.UserTenants.ToTenantsList() };
                    return user;
                }
            }

            reason = "User unknown.";
            return null;
        }
    }
}
