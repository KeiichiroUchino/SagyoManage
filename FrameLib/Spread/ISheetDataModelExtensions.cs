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
    /// ISheetDataModel の拡張メソッドを定義します。
    /// </summary>
    public static class ISheetDataModelExtensions
    {
        /// <summary>
        /// 指定した行と列のセルにデータを設定します。
        /// </summary>
        /// <param name="datamodel"></param>
        /// <param name="row">行</param>
        /// <param name="column">列</param>
        /// <param name="cellValue">セルの値</param>
        /// <param name="tagValue">タグの値</param>
        public static void SetValue(this ISheetDataModel datamodel, int row, int column, object cellValue, object tagValue)
        {
            datamodel.SetValue(row, column, cellValue);
            datamodel.SetTag(row, column, tagValue);
        }
    }
}
