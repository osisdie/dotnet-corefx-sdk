using System;
using CoreFX.Abstractions.Configurations;
using CoreFX.Abstractions.Consts;
using CoreFX.Abstractions.Enums;
using CoreFX.Abstractions.Utils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CoreFX.Abstractions.App_Start
{
    public class SdkRuntime
    {
        private static string _version;
        public static string Version
        {
            get => _version ?? SysConfigs.Version ?? new Version(0, 0, 0, 1).ToString();
            set => _version = value;
        }

        private static string _machineName;
        public static string MachineName
        {
            get
            {
                if (null != _machineName)
                {
                    return _machineName;
                }

                try
                {
                    _machineName = Environment.GetEnvironmentVariable(EnvConst.MachineName) ?? Environment.MachineName;
                }
                catch { _machineName = ""; }

                return _machineName;
            }
        }

        private static string _deployName;
        public static string DeploymentName
        {
            get => _deployName ?? ApiName;
            set => _deployName = value;
        }

        private static string _svcBaseUrl;
        public static string BaseUrl
        {
            get => _svcBaseUrl ?? SysConfigs.BaseUrl;
            set => _svcBaseUrl = value;
        }

        private static string _svcApiKey;
        public static string ApiKey
        {
            get => _svcApiKey ?? SysConfigs.ApiKey;
            set => _svcApiKey = value;
        }

        private static string _svcApiName;
        public static string ApiName
        {
            get => _svcApiName ?? SysConfigs.ApiName;
            set => _svcApiName = value;
        }

        private static IConfiguration _configuration;
        public static IConfiguration Configuration
        {
            get => _configuration;
            set
            {
                _configuration = value;
                SysConfigs = _configuration?.GetSection(SvcConst.DefaultSDKSectionName)?.Get<SvcContextConfiguration>() ?? new SvcContextConfiguration();
            }
        }

        // Default: Debug
        public static string SdkEnv = EnvironmentEnum.Debug.ToString();

        // Align ASPNETCORE_ENVIRONMENT value
        public static IHostEnvironment HostingEnv;

        public static readonly DateTime _ts = DateTime.UtcNow;
        public static string LocalIP => NetworkUtil.LocalIP;

        public static SvcContextConfiguration SysConfigs = new SvcContextConfiguration();

        protected static bool _isInitialized = false;
        protected static readonly object _lock = new object();
    }
}
