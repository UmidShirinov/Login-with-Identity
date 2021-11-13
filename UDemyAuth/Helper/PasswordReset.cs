using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace UDemyAuth.Helper
{
    public static class PasswordReset
    {
        public static void sendToMail(string link , string email)
        {
            MailMessage mail = new MailMessage("psbaku99@gmail.com",email);
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            mail.Subject = "Parol sifirlama";
            mail.Body = "<h2>Parolu yenilemek ucun asagidaki linke vurun</h2><hr/>";
            mail.Body += $"<a href='{link}'>Parol yenileme linki</a>";
            mail.IsBodyHtml = true;
            smtp.UseDefaultCredentials = false;
            NetworkCredential net = new NetworkCredential("psbaku99@gmail.com", "tofiqumid1234");
            smtp.Credentials = net;
            smtp.EnableSsl = true;
            smtp.Send(mail);


        }
    }
}
