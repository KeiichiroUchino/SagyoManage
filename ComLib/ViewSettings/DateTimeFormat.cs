using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.ViewSettings
{
    /// <summary>
    /// 日付時刻セルの書式を表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class DateTimeFormat : System.Attribute
    {
        public DateTimeFormat()
        {
            this.Format = "yyyy/MM/dd";
        }

        public string Format { get; set; }
    }
}
