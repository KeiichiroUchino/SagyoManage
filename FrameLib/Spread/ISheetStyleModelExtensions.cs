using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// ISheetSytleModelの拡張メソッドを定義します。
    /// </summary>
    public static class ISheetStyleModelExtensions
    {
        /// <summary>
        /// </summary>
        public static void SetStyleInfoValue(this ISheetStyleModel styleModel, int rowIndex, int columnIndex, Action<StyleInfo> valueSettingDelegate)
        {
            //指定された場所のStyleInfoを取り出す
            var info = styleModel.GetDirectInfo(-1, columnIndex, null);

            //値設定のデリゲートに渡す
            valueSettingDelegate(info);

            //指定された場所にStyleInfoを設定する。
            styleModel.SetDirectInfo(rowIndex, columnIndex, info);
        }
    }
}
