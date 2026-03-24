using CoreFX.Abstractions.Notification.Interfaces;

namespace Hello.MediatR.Endpoint.Services.NotifyServices
{
    public interface ISvcSchedule_ReportService<T>
        where T : class, IReportRecordDto
    {
        void StartTimer();
        void RestartTimer();
        void AddRecord(T rec);
        int CountRecords();
    }
}
