using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.ViewSettings
{
    /// <summary>
    /// 数値セルの書式を表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class NumberFormat : System.Attribute
    {
        public NumberFormat()
        {
            this.DecimalPlaces = 0;
            this.DecimalSeparator = string.Empty;
            this.Separator = string.Empty;
            this.HideZero = false;
        }

        /// <summary>
        /// 少数桁数
        /// </summary>
        public int DecimalPlaces { get; set; }
        /// <summary>
        /// 少数区切りの文字
        /// </summary>
        public string DecimalSeparator { get; set; }
        /// <summary>
        /// 3桁区切りの文字
        /// </summary>
        public string Separator { get; set; }
        /// <summary>
        /// 3桁区切り文字を表示するか
        /// </summary>
        public bool ShowSeprator { get { return !string.IsNullOrWhiteSpace(this.Separator); } }
        /// <summary>
        /// ゼロを非表示するか
        /// </summary>
        public bool HideZero { get; set; }
    }
}
