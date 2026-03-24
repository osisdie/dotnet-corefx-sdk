using System;
using System.Collections.Generic;
using CoreFX.Abstractions.App_Start;

namespace CoreFX.Abstractions.Extensions
{
    public static class Logger_Extension
    {
        public static IDictionary<string, T> BeginCollection<T>() =>
           new Dictionary<string, T>(StringComparer.OrdinalIgnoreCase);

        public static IDictionary<string, T> TryAdd<T>(this IDictionary<string, T> src, string key, T val)
        {
            src[key] = val;
            return src;
        }

        public static IDictionary<string, T> AddDebugData<T>(this IDictionary<string, T> src)
        {
            src["ver"] = (T)Convert.ChangeType(SdkRuntime.Version, typeof(T));
            src["api-name"] = (T)Convert.ChangeType(SdkRuntime.ApiName, typeof(T));
            src["deploy"] = (T)Convert.ChangeType(SdkRuntime.DeploymentName, typeof(T));
            src["env"] = (T)Convert.ChangeType(SdkRuntime.SdkEnv, typeof(T));
            src["_ip"] = (T)Convert.ChangeType(SdkRuntime.LocalIP, typeof(T));
            src["_host"] = (T)Convert.ChangeType(SdkRuntime.MachineName, typeof(T));
            src["_os"] = (T)Convert.ChangeType(Environment.OSVersion.Platform.ToString(), typeof(T));
            src["_ts"] = (T)Convert.ChangeType(DateTime.UtcNow.ToString("s"), typeof(T));
            src["_up"] = (T)Convert.ChangeType(SdkRuntime._ts.ToString("s"), typeof(T));

            return src;
        }
    }
}
