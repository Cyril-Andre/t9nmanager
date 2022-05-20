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
    public static class UserRegistrationModelExtension
    {
        public static DbUser ToDatabase(this UserRegistrationModel user)
        {
            var passwordHashContainer = Security.PasswordHashProvider.CreateHash(user.UserPassword);
            DbUser dbUser = new DbUser
            {
                Firstname = user.Firstname,
                Lastname = user.Lastname,
                UserName = user.UserName,
                UserEmail = user.UserEmail,
                UserPasswordHash = ByteConverter.GetHexString(passwordHashContainer.HashedPassword) ,//user.UserPassword,
                Salt = ByteConverter.GetHexString(passwordHashContainer.Salt),
                UserBirthdate = user.UserBirthDate
            };
            return dbUser;
        }

        public static bool Exists(this UserRegistrationModel user, t9nDbContext context, out string moreInfo)
        {
                var dbUser = context.Users.FirstOrDefault(u =>
                    user.UserName.ToLower() == u.UserName.ToLower() ||
                    user.UserEmail.ToLower() == u.UserEmail.ToLower());
                if (dbUser == null)
                {
                    moreInfo = "";
                    return false;
                }

                if (string.Equals(dbUser.UserName, user.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    moreInfo = $"{user.UserName} already exists.";
                    return true;
                }

                moreInfo = $"{user.UserEmail} email is already assigned to another account.";
                return true;
        }
    }
}
