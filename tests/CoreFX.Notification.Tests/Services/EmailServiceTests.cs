using System.Threading.Tasks;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Notification.Models;
using CoreFX.Notification.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace CoreFX.Notification.Tests.Services
{
    public class EmailServiceTests
    {
        private readonly EmailService _service;

        public EmailServiceTests()
        {
            var logger = new Mock<ILogger<EmailService>>();
            var options = Options.Create(new EmailConfiguration
            {
                From = null,
                To = null,
                SmtpConfig = new SmtpCofiguration
                {
                    Host = "smtp.test.com",
                    Port = 587,
                    Username = "test@test.com",
                }
            });
            _service = new EmailService(logger.Object, options);
        }

        [Fact]
        public async Task SendAsync_MissingFrom_ReturnsInvalidContract()
        {
            var result = await _service.SendAsync("subject", "<p>body</p>", from: null, to: "test@test.com");
            // from defaults to config.From which is null
            Assert.False(result.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.InvalidContract, result.Code);
        }

        [Fact]
        public async Task SendAsync_MissingTo_ReturnsInvalidContract()
        {
            var result = await _service.SendAsync("subject", "<p>body</p>", from: "sender@test.com", to: null);
            Assert.False(result.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.InvalidContract, result.Code);
        }

        [Fact]
        public async Task SendAsync_MissingSubject_ReturnsInvalidContract()
        {
            var result = await _service.SendAsync(null, "<p>body</p>", from: "a@b.com", to: "c@d.com");
            Assert.False(result.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.InvalidContract, result.Code);
        }

        [Fact]
        public async Task SendAsync_MissingBody_ReturnsInvalidContract()
        {
            var result = await _service.SendAsync("subject", null, from: "a@b.com", to: "c@d.com");
            Assert.False(result.IsSuccess);
            Assert.Equal((int)SvcCodeEnum.InvalidContract, result.Code);
        }

        [Fact]
        public void Constructor_NullMailSettings_UsesDefaults()
        {
            var logger = new Mock<ILogger<EmailService>>();
            var exception = Record.Exception(() => new EmailService(logger.Object, null));
            Assert.Null(exception);
        }
    }
}
