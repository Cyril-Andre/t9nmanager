using Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace userManagement
{
    public class UserResetPasswordModel
    {
        public string UserEmail { get; set; }
        public string? UserPassword { get; set; }
        public string? Otp { get; set; }

        public bool Validate(out string reason)
        {
            PasswordOptions opts = new PasswordOptions();
            var strength = PasswordStrengthHelper.GetPasswordStrength(UserPassword);
            reason = "";
            if (string.IsNullOrEmpty(UserEmail))
            {
                reason = "User email is compulsory";
                return false;
            }
            if ((int)strength < 2)
            {
                reason = "Password is too weak";
                return false;
            }
            reason = $"{strength.GetDescription()}";
            return true;
        }
    }
}
