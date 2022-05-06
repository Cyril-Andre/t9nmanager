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

            var cert = certStore.Certificates.Find(X509FindType.FindBySerialNumber, "418F0A50E3FB2C804FBF41F6BC82EE67", true).FirstOrDefault();// new X509Certificate2(appSettings.CertFullPath);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,user.UserName),
                    new Claim(ClaimTypes.Email,user.UserEmail),
                    new Claim("Tenants",string.Join(",",user.UserTenants.Select(x=>x.TenantKey).ToList()))
                }),
                Issuer = "t9nManager",
                Expires = DateTime.Now.AddHours(8),
                SigningCredentials=new X509SigningCredentials(cert)
            };
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            string jwt = tokenHandler.WriteToken(token);
            return jwt;
        }
    }
}
