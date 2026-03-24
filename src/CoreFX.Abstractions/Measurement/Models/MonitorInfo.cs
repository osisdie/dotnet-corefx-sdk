using System;
using System.Collections.Generic;

namespace CoreFX.Abstractions.Measurement.Models
{
    public class MonitorInfo<T>
    {
        public string Key { get; set; }
        public T Value { get; set; }
        public DateTime LastUsedTime { get; set; } = DateTime.UtcNow;
        public DateTime CreatedTime { get; set; } = DateTime.UtcNow;
        public Dictionary<string, string> ExtMap { get; set; }
    }
}
