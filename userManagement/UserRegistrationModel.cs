using System;
using Security;

namespace userManagement
{
    public class UserRegistrationModel
    {
        public string UserName { get; set; }
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public DateTime UserBirthDate { get; set; }

        public bool Validate(out string reason)
        {
            if (DateTime.Today.Year - UserBirthDate.Year < 13)
            {
                reason = "User must be at least 13 years old.";
                return false;
            }
            PasswordOptions opts = new PasswordOptions();
            var strength = PasswordStrengthHelper.GetPasswordStrength(UserPassword);
            reason = "";
            if (string.IsNullOrEmpty(UserName))
            {
                reason = "Username is compulsory";
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
