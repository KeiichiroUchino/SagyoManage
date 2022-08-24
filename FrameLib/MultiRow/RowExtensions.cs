using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;

namespace Jpsys.SagyoManage.FrameLib.MultiRow
{
    /// <summary>
    /// Row の拡張メソッドを提供します。
    /// </summary>
    public static class RowExtensions
    {
        /// <summary>
        /// すべてのセルのスタイルを設定します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="setStyleDelegate">スタイルを設定するデリゲート</param>
        public static void SetCellsStyle(this Row source, Action<CellStyle> setStyleDelegate)
        {
            foreach (var cell in source.Cells)
            {
                setStyleDelegate(cell.Style);
            }
        }

        /// <summary>
        /// 指定したセルの値を取得します。セルが編集中の場合は編集値を取得します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static object GetEditingOrCellValue(this Row source, string cellName)
        {
            return source.GcMultiRow.GetEditingOrCellValue(source.Index, cellName);
        }


        /// <summary>
        /// 指定したセルの値を取得します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cellName"></param>
        /// <returns></returns>
        public static object GetValue(this Row source, string cellName)
        {
            return source.GcMultiRow.GetValue(source.Index, cellName);
        }

        /// <summary>
        /// 指定したセルに値を設定します。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cellName"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static void SetValue(this Row source, string cellName, object value)
        {
            source.GcMultiRow.SetValue(source.Index, cellName, value);
        }

        /// <summary>
        /// 指定したセルの値をコピーします。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="cellName">セル名</param>
        /// <param name="rowIndex">コピー先の行インデックス</param>
        public static void CopyValueTo(this Row source, string cellName, int rowIndex)
        {
            object sourceValue = source.GcMultiRow.GetEditingOrCellValue(source.Index, cellName);
            source.GcMultiRow.SetEditingControlValueOrCellValue(rowIndex, cellName, sourceValue);
        }
    }
}
