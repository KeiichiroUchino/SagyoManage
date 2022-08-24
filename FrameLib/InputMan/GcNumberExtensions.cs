using GrapeCity.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.InputMan
{
    /// <summary>
    /// GcNumberの拡張メソッドを提供します。
    /// </summary>
    public static class GcNumberExtensions
    {
        /// <summary>
        /// 指定した桁になるように 左に0（ゼロ）埋めした文字列を返します。
        /// Null の場合は string.Empty を返します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="digit">桁数</param>
        /// <returns>0（ゼロ）埋めした文字列</returns>
        public static string ZeroPadding(this GcNumber source, int digit)
        {
            return source.Value == null ? string.Empty : Convert.ToInt64(source.Value).ToString("D" + digit.ToString());
        }
    }
}
