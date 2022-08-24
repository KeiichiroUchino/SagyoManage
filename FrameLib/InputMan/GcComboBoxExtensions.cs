using GrapeCity.Win.Editors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.InputMan
{
    /// <summary>
    /// GcComboBoxの拡張メソッドを提供します。
    /// </summary>
    public static class GcComboBoxExtensions
    {
        /// <summary>
        /// 先頭のアイテムを選択します。
        /// </summary>
        /// <param name="source"></param>
        public static void SelectFirstItem(this GcComboBox source)
        {
            if (source.Items.Count > 0)
            {
                source.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// 指定した桁になるように 左に0（ゼロ）埋めした文字列を返します。
        /// Null の場合は string.Empty を返します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="digit">桁数</param>
        /// <returns>0（ゼロ）埋めした文字列</returns>
        public static string ZeroPadding(this GcComboBox source, int digit)
        {
            return source.SelectedValue == null ? string.Empty : Convert.ToInt32(source.SelectedValue).ToString("D" + digit.ToString());
        }
    }
}
