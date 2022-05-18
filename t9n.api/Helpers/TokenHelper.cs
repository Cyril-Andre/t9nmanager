using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using userManagement;

namespace t9n.api.Helpers
{
    static public class TokenHelper
    {
        static public string CreateToken(AppSettings appSettings, User user)
        {
            var certStore = new X509Store("My");
            certStore.Open(OpenFlags.ReadOnly);

            var cert = certStore.Certificates.Find(X509FindType.FindBySerialNumber, appSettings.CertSerialNumber, true).FirstOrDefault();// new X509Certificate2(appSettings.CertFullPath);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.Email, user.UserEmail),
                    new Claim("Tenants", string.Join(",", user.UserTenants.Select(x => x.TenantKey).ToList()))
                }),
                Issuer = "t9nManager",
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials=new X509SigningCredentials(cert)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);
            return jwt;
        }

        static public TokenValidationParameters TokenValidationParameters(AppSettings appSettings)
        {
            var certStore = new X509Store("My");
            certStore.Open(OpenFlags.ReadOnly);

            var cert = certStore.Certificates.Find(X509FindType.FindBySerialNumber, appSettings.CertSerialNumber, true).FirstOrDefault();

            var key = new X509SecurityKey(cert);
            var validationParameters = new TokenValidationParameters()
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew =TimeSpan.Zero
            };
            return validationParameters;
        }
        static public ClaimsPrincipal ValidateToken(AppSettings appSettings, string jwt, out string reason)
        {
            reason = "";
            if (string.IsNullOrEmpty(jwt)) throw new ArgumentException("Token empty");
            var certStore = new X509Store("My");
            certStore.Open(OpenFlags.ReadOnly);

            var cert = certStore.Certificates.Find(X509FindType.FindBySerialNumber, appSettings.CertSerialNumber, true).FirstOrDefault();

            var key = new X509SecurityKey(cert);
            var validationParameters = TokenValidationParameters(appSettings);

            var handler = new JwtSecurityTokenHandler();
            try
            {
                var claims = handler.ValidateToken(jwt, validationParameters, out var validatedToken);
                if (claims != null && validatedToken != null)
                {
                    return claims;
                }
                return null;
            }
            catch (SecurityTokenInvalidSignatureException signEx)
            {
                reason = "Invalid signature";
                return null;
            }
            catch (SecurityTokenExpiredException expEx)
            {
                reason = "Token expired";
                return null;
            }
            catch (Exception ex)
            {
                reason = ex.Message;
                return null;
            }

        }
    }
}
