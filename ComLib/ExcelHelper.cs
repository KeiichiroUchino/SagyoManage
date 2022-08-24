using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    public static class ExcelHelper
    {
        /// <summary>
        /// 数値を指定してExcelの列名に変換して返す
        /// 例：0→A、1→B
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public static string GetColumnName(this int index)
        {
            string str = string.Empty;
            do
            {
                str = Convert.ToChar(index % 26 + 0x41) + str;
            } while ((index = index / 26 - 1) != -1);

            return str;
        }

        /// <summary>
        /// 列インデックス、行インデックスを指定して文字列のセルインデックスに変換して返します。
        /// 例0,10→A10
        /// </summary>
        /// <param name="columIndex">列インデックス</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        public static string GetCellIndexName(int columIndex, int rowIndex)
        {
            return columIndex.GetColumnName() + rowIndex.ToString();
        }

        /// <summary>
        /// 列インデックス、行インデックスを指定して文字列のセルインデックスに変換して返します。
        /// </summary>
        /// <param name="columName">列名</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        public static string GetCellIndexName(string columName, int rowIndex)
        {
            return columName + rowIndex.ToString();
        }

        /// <summary>
        /// セル範囲を文字列で取得します。
        /// </summary>
        /// <param name="cell_begin"></param>
        /// <param name="cell_end"></param>
        /// <returns></returns>
        public static string GetCellRangeName(Tuple<int, int> cell_begin, Tuple<int, int> cell_end)
        {
            return GetCellIndexName(cell_begin.Item1, cell_begin.Item2) + ":" + GetCellIndexName(cell_end.Item1, cell_end.Item2);
        }

        /// <summary>
        /// セル範囲を文字列で取得します。
        /// </summary>
        /// <param name="cell_begin"></param>
        /// <param name="cell_end"></param>
        /// <returns></returns>
        public static string GetCellRangeName(Tuple<string, int> cell_begin, Tuple<string, int> cell_end)
        {
            return GetCellIndexName(cell_begin.Item1, cell_begin.Item2) + ":" + GetCellIndexName(cell_end.Item1, cell_end.Item2);
        }

        /// <summary>
        /// 列、行の始まり終わりを数値で指定して文字列でセル範囲を返す
        /// (0,1,1,2)→A1:B2
        /// A1 形式、変数名、セル名で指定します。
        /// </summary>
        /// <param name="begin_y"></param>
        /// <param name="begin_x"></param>
        /// <param name="end_y"></param>
        /// <param name="end_x"></param>
        /// <returns></returns>
        public static string GetCellRangeName(int begin_y, int begin_x, int end_y, int end_x)
        {
            return GetCellRangeName(
                Tuple.Create<int, int>(begin_y, begin_x),
                Tuple.Create<int, int>(end_y, end_x));
        }

    }
}
