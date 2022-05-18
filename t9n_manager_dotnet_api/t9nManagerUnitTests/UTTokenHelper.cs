using NUnit.Framework;
using System;
using System.Collections.Generic;
using t9n.api;
using t9n.api.Helpers;
using userManagement;

namespace t9nManagerUnitTests
{
    public class Tests
    {
        private AppSettings appSettings;
        [SetUp]
        public void Setup()
        {
            appSettings = new AppSettings()
            {
                CertFullPath = "..\\t9n.Api\\Assets\\certificate\\LilariszAuthCert.cer",
                ConfirmationEmailUrl = "https://localhost:44375/api/user/confirm",
                TemplatesPath = "templates"
            };
        }

        [Test]
        public void CreateTokenSuccessfuly()
        {
            User user = new User()
            {
                UserEmail = "the.cyril.andre@gmail.com",
                UserBirthDate = DateTime.Parse("1973-04-28"),
                UserName = "CyrilAndre",
                UserTenants = new List<Tenant>()
            };
            user.UserTenants.Add(new Tenant() { TenantKey = Guid.NewGuid(), TenantName = "Public", TenantUsers = new List<User> { user } });
            user.UserTenants.Add(new Tenant() { TenantKey = Guid.NewGuid(), TenantName = "MyTenant", TenantUsers = new List<User> { user } });
            var token = TokenHelper.CreateToken(appSettings, user);
        }
    }
}