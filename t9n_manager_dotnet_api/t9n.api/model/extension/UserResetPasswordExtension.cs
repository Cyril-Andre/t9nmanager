using System.Linq;
using t9n.DAL;
using userManagement;
using Security;

namespace t9n.api.model.extension
{
    public static class UserResetPasswordExtension
    {
        public static DbUser ToDatabase(this UserResetPasswordModel userResetPasswordModel, t9nDbContext context)
        {
            var passwordHashContainer = Security.PasswordHashProvider.CreateHash(userResetPasswordModel.UserPassword);
            var dbUser = context.Users.FirstOrDefault(u => userResetPasswordModel.UserEmail.ToLower() == u.UserEmail.ToLower());
            dbUser.UserPasswordHash = ByteConverter.GetHexString(passwordHashContainer.HashedPassword);
            dbUser.Salt = ByteConverter.GetHexString(passwordHashContainer.Salt);
            dbUser.ResetPasswordOtp = null;
            return dbUser;
        }
    }
}
