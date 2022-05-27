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
                TemplatesPath = "templates",
                CertSerialNumber= "418F0A50E3FB2C804FBF41F6BC82EE67"
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
            user.UserTenants.Add(new Tenant() { Id = Guid.NewGuid(), Name = "Public" });
            user.UserTenants.Add(new Tenant() { Id = Guid.NewGuid(), Name = "MyTenant" });
            var token = TokenHelper.CreateToken(appSettings, user);
            Assert.AreEqual("ey", token.Substring(0, 2));
        }

        [Test]
        public void ValidateTokenFailedExpired()
        {            
            string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkZDQTczN0E1OEYzMTY1REMzQUNCQjlFMjI4REJEMkZFODIzQjA4Q0YiLCJ4NXQiOiJfS2MzcFk4eFpkdzZ5N25pS052U19vSTdDTTgiLCJ0eXAiOiJKV1QifQ.eyJ1bmlxdWVfbmFtZSI6IkN5cmlsQW5kcmUiLCJlbWFpbCI6InRoZS5jeXJpbC5hbmRyZUBnbWFpbC5jb20iLCJUZW5hbnRzIjoiNjMxMzUyNTEtYjU4NC00MDZlLWI5NjctZTczODhiNTZlMGZlLDFiNzkyMWM3LWM4MzQtNGJkYy1iMjJiLTNiYmM5ODNiMzJiYiIsIm5iZiI6MTY1MTgzMjAzOSwiZXhwIjoxNjUxODYwODM4LCJpYXQiOjE2NTE4MzIwMzksImlzcyI6InQ5bk1hbmFnZXIifQ.LFRC_k_6H9lujk3oqhPW81Bar_bY42X_x37yvxqdIJw1 - rMDoFTsx8kDElDlpUfONXaRgtuozKAbAJmUQxCzVUC1lIYIAICUJ6vDsQkycgU3yAYXqMjFtoXxkBfMbWkGf0wswDADHWG7OQNmen9cogjTLyYOPyIFPH - T9QFgZzy3jWxgE0GtVArDZDklDB4fpK - jPkvrvns226D - R5yxktAIlGJQ3YY0_msefogN6Sf69jAB7I1m8XDQecgbB4BUOQdWSXNEATca_azlNfsk1YDMFa78gZpet3Fiy - SG36caw4YwSmQzrihXOD1ov1xKdLckFq5mpZUvf6ceLbOFsA";
            var claims = TokenHelper.ValidateToken(appSettings, token, out var reason);
            Assert.IsNull(claims);
            Assert.IsNotEmpty(reason);
        }
        [Test]
        public void ValidateTokenFailedInvalidSignature()
        {            
            string token = "eyJhbGciOiJSUzI1NiIsImtpZCI6IkZDQTczN0E1OEYzMTY1REMzQUNCQjlFMjI4REJEMkZFODIzQjA4Q0YiLCJ4NXQiOiJfS2MzcFk4eFpkdzZ5N25pS052U19vSTdDTTgiLCJ0eXAiOiJKV1QifQ.eyJ1amlxdWVfbmFtZSI6IkN5cmlsQW5kcmUiLCJlbWFpbCI6InRoZS5jeXJpbC5hbmRyZUBnbWFpbC5jb20iLCJUZW5hbnRzIjoiNjMxMzUyNTEtYjU4NC00MDZlLWI5NjctZTczODhiNTZlMGZlLDFiNzkyMWM3LWM4MzQtNGJkYy1iMjJiLTNiYmM5ODNiMzJiYiIsIm5iZiI6MTY1MTgzMjAzOSwiZXhwIjoxNjUxODYwODM4LCJpYXQiOjE2NTE4MzIwMzksImlzcyI6InQ5bk1hbmFnZXIifQ.LFRC_k_6H9lujk3oqhPW81Bar_bY42X_x37yvxqdIJw1 - rMDoFTsx8kDElDlpUfONXaRgtuozKAbAJmUQxCzVUC1lIYIAICUJ6vDsQkycgU3yAYXqMjFtoXxkBfMbWkGf0wswDADHWG7OQNmen9cogjTLyYOPyIFPH - T9QFgZzy3jWxgE0GtVArDZDklDB4fpK - jPkvrvns226D - R5yxktAIlGJQ3YY0_msefogN6Sf69jAB7I1m8XDQecgbB4BUOQdWSXNEATca_azlNfsk1YDMFa78gZpet3Fiy - SG36caw4YwSmQzrihXOD1ov1xKdLckFq5mpZUvf6ceLbOFsA";
            var claims = TokenHelper.ValidateToken(appSettings, token, out var reason);
            Assert.IsNull(claims);
            Assert.IsNotEmpty(reason);
        }
    [Test]
        public void ValidateTokenSuccess()
        {
            User user = new User()
            {
                UserEmail = "the.cyril.andre@gmail.com",
                UserBirthDate = DateTime.Parse("1973-04-28"),
                UserName = "CyrilAndre",
                UserTenants = new List<Tenant>()
            };
            user.UserTenants.Add(new Tenant() { Id = Guid.NewGuid(), Name = "Public" });
            user.UserTenants.Add(new Tenant() { Id = Guid.NewGuid(), Name = "MyTenant" });
            var token = TokenHelper.CreateToken(appSettings, user); 
            var claims = TokenHelper.ValidateToken(appSettings, token, out var reason);
            Assert.IsNotNull(claims);
            Assert.AreEqual("CyrilAndre", claims?.Identity?.Name);
        }
    }
}