using Hangfire;
using Hangfire.Server;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using NotificationService.Entities.NotificationEntities;
using NotificationService.Models;
using NotificationService.Models.AppSettings;
using NotificationService.Repositories;
using NotificationService.Services;

namespace NotificationService.Hangfire.Jobs;

public sealed class EmailDeliveryProcessingJob
{
    private readonly ILogger<EmailDeliveryProcessingJob> _logger;
    private readonly IEmailsRepository _repo;
    private readonly SMTPSettings _config;

    public EmailDeliveryProcessingJob(
        ILogger<EmailDeliveryProcessingJob> logger,
        IOptions<SMTPSettings> config,
        IEmailsRepository repository

        )
    {
        _logger = logger;
        _repo = repository;
        _config = config.Value;
    }

    [AutomaticRetry(Attempts = 0)]
    [JobDisplayName("EmailDeliveryProcessingJob")]
    [Queue(HangfireQueues.DEFAULT)]
    public async Task Send(
        EmailNotification email,
        Recipient recipient,
        PerformContext context,
        CancellationToken cancellationToken)
    {
        _logger.LogInformation("EmailDeliveryProcessingJob invoked");

        var mailMessage = new MimeMessage();
        mailMessage.From.Add(new MailboxAddress(_config.SenderName, _config.SenderEmail));
        mailMessage.To.Add(new MailboxAddress("Client", recipient.Email));
        mailMessage.Subject = email.Subject;
        mailMessage.Body = new TextPart("html")
        {
            Text = email.Content
        };

        var smtpClient = new SmtpClient();
        try
        {
            await smtpClient.ConnectAsync(_config.Host, _config.Port, _config.UseSSL, cancellationToken);
            await smtpClient.AuthenticateAsync(_config.UserName, _config.Password, cancellationToken);
            await smtpClient.SendAsync(mailMessage, cancellationToken);
            await _repo.ChangeEmailStatusAsync(email.Id, EStatus.Send);
            Console.WriteLine($"Email {email.Id} send successfully");
        }
        catch (Exception ex)
        {
            await _repo.ChangeEmailStatusAsync(email.Id, EStatus.HasErrors);
            _logger.LogError(ex.Message);
            return;
        }

        await smtpClient.DisconnectAsync(true, cancellationToken);
    }
}