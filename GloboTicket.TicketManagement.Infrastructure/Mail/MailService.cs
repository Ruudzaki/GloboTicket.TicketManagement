using System.Net;
using GloboTicket.TicketManagement.Application.Contracts.Infrastructure;
using GloboTicket.TicketManagement.Application.Models.Mail;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GloboTicket.TicketManagement.Infrastructure.Mail;

public class EmailService : IEmailService
{
    public EmailService(IOptions<EmailSettings> mailSettings)
    {
        EmailSettings = mailSettings.Value;
    }

    public EmailSettings EmailSettings { get; }

    public async Task<bool> SendEmail(Email email)
    {
        var client = new SendGridClient(EmailSettings.ApiKey);

        var subject = email.Subject;
        var to = new EmailAddress(email.To);
        var emailBody = email.Body;

        var from = new EmailAddress
        {
            Email = EmailSettings.FromAddress,
            Name = EmailSettings.FromName
        };

        var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, emailBody, emailBody);
        var response = await client.SendEmailAsync(sendGridMessage);

        return response.StatusCode is HttpStatusCode.Accepted or HttpStatusCode.OK;
    }
}