using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CoreFX.Abstractions.App_Start;
using CoreFX.Abstractions.Notification.Interfaces;
using CoreFX.Abstractions.Notification.Models;
using CoreFX.Notification.Extensions;
using CoreFX.Notification.Interfaces;
using CoreFX.Notification.Utils;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Hello.MediatR.Endpoint.Services.NotifyServices
{
    public class SvcSchedule_ReportService<T> : ISvcSchedule_ReportService<T>
        where T : class, IReportRecordDto
    {
        protected readonly ILogger _logger;
        protected readonly ReportConfiguration _config;
        protected readonly IEmailService _svcEmail;

        public SvcSchedule_ReportService(ILogger<SvcSchedule_ReportService<T>> logger, IOptions<ReportConfiguration> config, IEmailService svcEmail = null)
        {
            _logger = logger;
            _config = config?.Value ?? new ReportConfiguration();
            _svcEmail = svcEmail;

            if (_config.CooldownSecs > 0)
            {
                _cooldownSecs = _config.CooldownSecs;
            }

            if (_config.MaxRecords > 0)
            {
                _maxRecords = _config.MaxRecords > 100 ? 100 : _config.MaxRecords;
            }

            if (_config.Enabled)
            {
                StartTimer();
            }
        }

        public void AddRecord(T rec)
        {
            _records.Push(rec);
        }

        public int CountRecords() => _records.Count;

        public void StartTimer()
        {
            _timer = new Timer(TimerCallback,
                null,
                _consumeInterval,
                Timeout.Infinite
            );
        }

        public void RestartTimer()
        {
            _timer?.Change(_consumeInterval, Timeout.Infinite);
        }

        private void TimerCallback(object state)
        {
            try
            {
                if (_svcEmail == null)
                {
                    throw new ArgumentNullException(nameof(IEmailService));
                }

                if ((DateTime.UtcNow - _lastSentTime).TotalSeconds > _cooldownSecs && !_records.IsEmpty)
                {
                    var count = _records.Count;

                    var aryRecs = new T[_records.Count];
                    //var recs = _records.TryDequeue(_records.Count);
                    _records.TryPopRange(aryRecs, 0, _records.Count);

                    var subject = $"summary events: {SdkRuntime.ApiName}";
                    var sb = new StringBuilder();

                    sb.Append("<p><ul>");
                    sb.Append($"Summary during UTC {DateTime.UtcNow.ToString("s")} ~ {_lastSentTime.ToString("s")})");
                    sb.Append($"<li>Total events: {aryRecs.Length}</li>");
                    foreach (var grp in aryRecs.GroupBy(x => $"[{x.IsSuccess.ToResultColor()}] {x.Category}"))
                    {
                        sb.Append($"<li>Total {grp.Key} events: {grp.Count()}</li>");
                    }
                    sb.Append("</ul></p>");

                    var sn = count;
                    for (var idx = 0; idx < count; ++idx)
                    {
                        var item = aryRecs[idx];
                        item.Sn = sn--;
                        if (idx < _maxRecords)
                        {
                            item.What ??= item.IsSuccess.ToResultColor();
                            item.When ??= $"UTC {item._ts.ToString("s")}";
                        }
                    }

                    var table = aryRecs.Take(_maxRecords).ToList().ToHtmlTable();
                    if (!string.IsNullOrEmpty(table))
                    {
                        sb.Append(table);
                    }

                    var body = sb.ToString();
                    Task.Run(async () =>
                    {
                        await _svcEmail.SendAsync(subject: subject, html: body);
                    });

                    _lastSentTime = DateTime.UtcNow;
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex.ToString());
            }

            RestartTimer();
        }

        public const int DefaultConsumeInterval = 30000; //ms
        public const int DefaultCoolDownSecs = 10 * 60; //10 mins
        public const int DefaultMaxRecord = 50; //10 mins

        // (identifier, object)
        private readonly ConcurrentStack<T> _records = new ConcurrentStack<T>();

        private readonly int _consumeInterval = DefaultConsumeInterval;
        private readonly int _cooldownSecs = DefaultCoolDownSecs;
        private readonly int _maxRecords = DefaultMaxRecord;
        private DateTime _lastSentTime = DateTime.UtcNow;
        private Timer _timer;
    }
}
