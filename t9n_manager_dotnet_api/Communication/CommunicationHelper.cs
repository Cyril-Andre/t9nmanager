using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Communication
{
    public static class CommunicationHelper
    {
        public static void SendInvitationMail(string email, string t9nManagerUrl, string firstname, string lastname,string tenant, string templatePath, string smtpUsername, string smtpPassword, string locale = "en")
        {
            List<TemplateParameter> parameters = new List<TemplateParameter>
            {
                new TemplateParameter
                {
                    Key="firstname",
                    Value = firstname
                },
                new TemplateParameter
                {
                    Key="lastname",
                    Value = lastname
                },
                new TemplateParameter
                {
                    Key="tenant",
                    Value = tenant
                },
                new TemplateParameter
                {
                    Key="t9nManagerUrl",
                    Value = t9nManagerUrl
                }
            };
            string content = HtmlHelper.loadHtmlTemplate(templatePath, "emailinvitation", locale, parameters);
            EmailHelper.SendEmail(email, "emailinvitation@genomemaster.com", $"{firstname} {lastname} invites you", content,smtpUsername, smtpPassword);

        }
        public static void SendConfirmationMail(string email,string confirmationEmailUrl,  string templatePath,string smtpUsername, string smtpPassword, string locale="en")
        {
            List<TemplateParameter> parameters = new List<TemplateParameter>
            {
                new TemplateParameter
                {
                    Key="url",
                    Value = confirmationEmailUrl
                }
            };
            string content = HtmlHelper.loadHtmlTemplate(templatePath, "emailconfirmation", locale, parameters);
            EmailHelper.SendEmail(email,"emailconfirmation@genomemaster.com","Genome Master email confirmation",content, smtpUsername, smtpPassword);
        }

        public static void SendResetPasswordMail(string email,string userName, string resetOTP, string templatePath,string smtpUsername,string smtpPassword, string locale = "en")
        {
            List<TemplateParameter> parameters = new List<TemplateParameter>
            {
                new TemplateParameter
                {
                    Key="otp",
                    Value=resetOTP
                },
                new TemplateParameter {
                    Key="userName",
                    Value=userName
                }
            };
            string content = HtmlHelper.loadHtmlTemplate(templatePath, "emailresetpassword", locale, parameters);
            EmailHelper.SendEmail(email, "resetpassword@genomemaster.com","Genome Master - Reset Password", content, smtpUsername, smtpPassword);
        }
    }
}
