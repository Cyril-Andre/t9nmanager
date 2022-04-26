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
        public static void SendConfirmationMail(string email,string confirmationEmailUrl,  string templatePath, string locale="en")
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
            EmailHelper.SendEmail(email,"noreply@genomemaster.com","Email confirmation",content);
        }
    }
}
