using EventMangerBLL.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace EventMangerBLL.Infrastructure
{
    class Emailer
    {
        public void SendConfirmationEmail(UserDTO user)
        {
            MailAddress from = new MailAddress("eventmanagerkbip@gmail.com", "Tom");
            MailAddress to = new MailAddress(user.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Подтверждение почты";
            m.Body = string.Format(@"Перейдите по ссылке для поддтверждения Email <a href=http://localhost:51572/User/ConfirmEmail/{0} > Email Confirm</a>", user.Id);
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("eventmanagerkbip@gmail.com", "XBmaj7TNP8QNjzE");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
        public void SendNotification(UserDTO user,EventDTO updatedEvent)//TO DO email
        {
            MailAddress from = new MailAddress("eventmanagerkbip@gmail.com", "Tom");
            MailAddress to = new MailAddress(user.Email);
            MailMessage m = new MailMessage(from, to);
            m.Subject = "Уведомление об изменении события";
            m.Body = string.Format(@"Перейдите по ссылке для поддтверждения Email <a href=http://localhost:51572/User/ConfirmEmail/{0} > Email Confirm</a>", user.Id);
            m.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587);
            smtp.Credentials = new NetworkCredential("eventmanagerkbip@gmail.com", "XBmaj7TNP8QNjzE");
            smtp.EnableSsl = true;
            smtp.Send(m);
        }
    }
}
