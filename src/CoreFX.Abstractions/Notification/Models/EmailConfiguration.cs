namespace CoreFX.Abstractions.Notification.Models
{
    public class EmailConfiguration
    {
        public SmtpCofiguration SmtpConfig { get; set; }
        public string From { get; set; }
        public string To { get; set; }
    }
}
