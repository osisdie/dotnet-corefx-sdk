using System;
using CoreFX.Abstractions.Notification.Models;
using CoreFX.Notification.Interfaces;
using CoreFX.Notification.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CoreFX.Notification.Extensions
{
    public static class AddEmailService_Extension
    {
        public static IServiceCollection AddEmailService(
            this IServiceCollection serviceCollection,
            Action<EmailConfiguration> options,
            bool optional = false)
        {
            if (options == null && !optional)
            {
                throw new ArgumentNullException(nameof(options),
                    $"Please provide options for {typeof(IEmailService).Name}.");
            }

            if (options != null)
            {
                serviceCollection.AddSingleton<IEmailService, EmailService>();
                serviceCollection.Configure(options);
            }

            return serviceCollection;
        }
    }
}
