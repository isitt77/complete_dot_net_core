using System;
using Microsoft.AspNetCore.Identity.UI.Services;

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
            return Task.CompletedTask;
        }
    }
}

