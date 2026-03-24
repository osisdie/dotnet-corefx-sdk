namespace CoreFX.Abstractions.Notification.Models
{
    public class NotifyConfiguration
    {
        public int CooldownSecs { get; set; }
        public ReportConfiguration ReportConfig { get; set; }
        public EmailConfiguration EmailConfig { get; set; }
    }
}
