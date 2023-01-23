using System;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using MimeKit;

namespace CompleteDotNetCore.Utility
{
    public class EmailSender : IEmailSender
    {
        public EmailSender()
        {
        }

        public Task SendEmailAsync(string email,
            string subject, string htmlMessage)
        {
            //MimeMessage emailToSend = new();
            //emailToSend.From.Add(MailboxAddress.Parse(
            //    "hello@dotnetcore.com"));
            //emailToSend.To.Add(MailboxAddress.Parse(email));
            //emailToSend.Subject = subject;
            //emailToSend.Body = new TextPart(MimeKit.Text.TextFormat
            //    .Html)
            //{ Text = htmlMessage };

            //// Send email
            //using (SmtpClient emailClient = new())
            //{
            //    emailClient.Connect("smtp.gmail.com", 587, MailKit.Security
            //        .SecureSocketOptions.StartTls);
            //    //emailClient.Authenticate("dotnetcore@gmail.com", "Dotnet@123");
            //    emailClient.Send(emailToSend);
            //    emailClient.Disconnect(true);
            //}
            return Task.CompletedTask;
        }
    }
}

