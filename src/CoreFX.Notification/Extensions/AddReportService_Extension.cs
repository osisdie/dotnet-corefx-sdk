using System;
using CoreFX.Abstractions.Notification.Interfaces;
using CoreFX.Abstractions.Notification.Models;
using Hello.MediatR.Endpoint.Services.NotifyServices;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFX.Notification.Extensions
{
    public static class AddReportService_Extension
    {
        public static IServiceCollection AddReportService<T>(
            this IServiceCollection serviceCollection,
            Action<ReportConfiguration> options,
            bool optional = true) where T : class, IReportRecordDto
        {
            if (options == null && !optional)
            {
                throw new ArgumentNullException(nameof(options),
                    $"Please provide options for {typeof(ISvcSchedule_ReportService<>).Name}.");
            }

            if (options != null)
            {
                serviceCollection.AddSingleton<ISvcSchedule_ReportService<T>, SvcSchedule_ReportService<T>>();
                serviceCollection.Configure(options);
            }

            return serviceCollection;
        }
    }
}
