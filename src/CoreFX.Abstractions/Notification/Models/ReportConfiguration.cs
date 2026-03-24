namespace CoreFX.Abstractions.Notification.Models
{
    public class ReportConfiguration
    {
        public bool Enabled { get; set; }
        public int CooldownSecs { get; set; }
        public int MaxRecords { get; set; }
    }
}
