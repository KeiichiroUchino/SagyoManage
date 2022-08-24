using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.ComLib.ViewSettings
{
    /// <summary>
    /// TimeSpanの書式を表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class TimeSpanFormat : System.Attribute
    {
        public TimeSpanFormat()
        {
            this.Format = "HH:mm";
        }

        public string Format { get; set; }
    }
}
