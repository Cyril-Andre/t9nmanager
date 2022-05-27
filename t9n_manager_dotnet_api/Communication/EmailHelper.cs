using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Communication
{
    public static class EmailHelper
    {
        public static void SendEmail(string to, string from, string subject, string body)
        {
            MailMessage mail = new MailMessage(from: from, to: to, subject: subject, body: body);
            mail.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Port = 587;
            smtp.Host = "smtp.gmail.com";
            smtp.EnableSsl = true;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials=new NetworkCredential("kalangp@gmail.com","Ja30aejva1f");
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;

            smtp.Send(mail);
        }
    }
}
