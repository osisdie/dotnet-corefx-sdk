using System;
using System.Linq;
using System.Threading.Tasks;
using CoreFX.Abstractions.App_Start;
using CoreFX.Abstractions.Bases.Interfaces;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Contracts;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Extensions;
using CoreFX.Abstractions.Notification.Models;
using CoreFX.Notification.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;

namespace CoreFX.Notification.Services
{
    public class EmailService : IEmailService
    {
        protected readonly ILogger _logger;
        protected readonly EmailConfiguration _mailSettings;

        public EmailService(ILogger<EmailService> logger, IOptions<EmailConfiguration> mailSettings)
        {
            _logger = logger;
            _mailSettings = mailSettings?.Value ?? new EmailConfiguration();
            _mailSettings.SmtpConfig ??= new SmtpCofiguration();
        }

        public async Task<ISvcResponseBaseDto> SendAsync(string subject, string html, string from = null, string to = null)
        {
            from ??= _mailSettings.From;
            to ??= _mailSettings.To;

            var res = new SvcResponseDto();
            if (string.IsNullOrEmpty(from) ||
                string.IsNullOrEmpty(to) ||
                string.IsNullOrEmpty(subject) ||
                string.IsNullOrEmpty(html))
            {
                res.Error(SvcCodeEnum.InvalidContract, "Require fields: from, to, subject, body");
                return res;
            }

            try
            {
                _mailSettings.SmtpConfig.Password ??= Environment.GetEnvironmentVariable(EnvConst.SMTP_PWD)
                    ?? throw new ArgumentNullException(EnvConst.SMTP_PWD);

                await ExecuteAsync(from: from, to: to, subject: subject, html: html);
                res.Success();
                _logger.LogInformation($"Successfully sent email to {to}, title={subject}");
            }
            catch (Exception ex)
            {
                res.Error(SvcCodeEnum.Exception, ex.Message);
                _logger.LogWarning($"Failed to send email to {to}, title={subject}, ex={ex}");
            }

            return res;
        }

        private async Task ExecuteAsync(string from, string to, string subject, string html)
        {
            var titlePrefix = SvcContext.IsProduction() ? string.Empty : SdkRuntime.SdkEnv + "-";
            var receivers = to.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(from));
            receivers.ForEach(x =>
            {
                email.To.Add(MailboxAddress.Parse(x));
            });

            email.Subject = titlePrefix + subject;
            email.Body = new TextPart(TextFormat.Html) { Text = html };

            using (var client = new SmtpClient())
            {
                client.Connect(_mailSettings.SmtpConfig.Host, _mailSettings.SmtpConfig.Port, SecureSocketOptions.StartTls);
                //client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_mailSettings.SmtpConfig.Username, _mailSettings.SmtpConfig.Password);
                await client.SendAsync(email);
            }
        }
    }
}
