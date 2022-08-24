using GrapeCity.Win.Editors;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.MultiRow
{
    /// <summary>
    /// GcMultiRowの拡張クラスです。
    /// </summary>
    public static class GcMultiRowExtensions
    {
        /// <summary>
        /// 現在行のインデックスを取得します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <returns></returns>
        public static int CurrentRowIndex(this GcMultiRow multiRow)
        {
            var currentRow = multiRow.CurrentRow;

            if (currentRow != null)
            {
                return currentRow.Index;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// 指定した行インデックスの行があるかを返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static bool HasRow(this GcMultiRow multiRow, int rowIndex)
        {
            if (rowIndex < 0)
                throw new ArgumentException("行インデックスは0以上をしてください。", "rowIndex");

            return (rowIndex <= multiRow.RowCount - 1);
        }

        /// <summary>
        /// 行を新規行の前に追加します。
        /// 新規行がない場合は最終行に追加します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <returns>追加した行のインデックス</returns>
        public static int AddRowInFrontOfNewRow(this GcMultiRow multiRow)
        {
            if (multiRow.NewRowIndex > -1)
            {
                int addRowIndex = multiRow.NewRowIndex;
                multiRow.Rows.Insert(addRowIndex);
                return addRowIndex;
            }
            else
            {
                return multiRow.Rows.Add();
            }
        }




        #region セルポジションを最初のセルに設定する


        /// <summary>
        /// セルポジションを選択可能な最初のセルに移動します。
        /// セルの順序はTabIndexの値で決定します。
        /// </summary>
        /// <param name="multiRow"></param>
        public static void CellPositionToSelectableFirst(this GcMultiRow multiRow)
        {
            for (int i = 0; i < multiRow.RowCount; i++)
            {
                Cell selectableCell = 
                    multiRow.Rows[i].Cells
                    .OrderBy(cell => cell.TabIndex)  
                    .ThenBy(cell => cell.CellIndex)
                    .FirstOrDefault(element => element.Selectable && !element.ReadOnly);
                    
                if (selectableCell != null)
                {
                    multiRow.CurrentCell = selectableCell;
                    return;
                }
            }
        }

        /// <summary>
        /// セルポジションを選択可能な最初のセルに移動します。
        /// セルの順序はTabIndexの値で決定します。
        /// </summary>
        /// <param name="multiRow">行インデックス</param>
        public static void CellPositionToSelectableFirstOfCurrentRow(this GcMultiRow multiRow)
        {
            if (multiRow.CurrentCellPosition != CellPosition.Empty)
            {
                multiRow.CellPositionToSelectableFirstOrder(multiRow.CurrentCellPosition.RowIndex);
            }
        }


        /// <summary>
        /// セルポジションを選択可能な最初のセルに移動します。
        /// セルの順序はTabIndexの値で決定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex">行インデックス</param>
        public static void CellPositionToSelectableFirstOrder(this GcMultiRow multiRow, int rowIndex)
        {
            Cell selectableCell =
                multiRow.Rows[rowIndex].Cells
                .OrderBy(cell => cell.TabIndex)
                .ThenBy(cell => cell.CellIndex)
                .FirstOrDefault(element => element.Selectable && !element.ReadOnly);

            if (selectableCell != null)
            {
                multiRow.CurrentCell = selectableCell;
                return;
            }   
        }





        #endregion

        #region 選択可能なセルポジションを返却


        /// <summary>
        /// 指定した行で選択可能なセルのポジションを返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="searchOrderType">検索する順序の指定</param>
        /// <returns></returns>
        public static CellPosition GetSelectableCellPositionOfRow(this GcMultiRow multiRow, int rowIndex, SearchOrderType searchOrderType)
        {
            return multiRow.GetSelectableCellPositionOfRow(rowIndex, 0, searchOrderType);
        }

        /// <summary>
        /// 指定した行で選択可能なセルのポジションを返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex">行インデックス</param>
        /// <param name="searchStartCellIndex">検索開始セル</param>
        /// <param name="searchOrderType">検索する順序の指定</param>
        /// <returns></returns>
        public static CellPosition GetSelectableCellPositionOfRow(this GcMultiRow multiRow, int rowIndex, int searchStartCellIndex, SearchOrderType searchOrderType)
        {
            IEnumerable<Cell> cellsOfRow = multiRow.Rows[rowIndex].Cells;
            Cell searchStartCell = multiRow[rowIndex, searchStartCellIndex];

            switch (searchOrderType)
            {
                case SearchOrderType.CellIndex:
                    cellsOfRow = cellsOfRow.Where(c => c.CellIndex >= searchStartCell.CellIndex).OrderBy(c => c.CellIndex);
                    break;
                case SearchOrderType.TabIndex:
                    cellsOfRow = cellsOfRow.Where(c => c.TabIndex >= searchStartCell.TabIndex).OrderBy(c => c.TabIndex);
                    break;
                default:
                    break;
            }

            Cell selectableCellFirst = cellsOfRow.Where(c => c.Selectable).FirstOrDefault();

            if (selectableCellFirst == null)
            {
                return CellPosition.Empty;
            }
            else
            {
                return new CellPosition(selectableCellFirst.RowIndex, selectableCellFirst.CellIndex);
            }
        }


        #endregion

        /// <summary>
        /// 現在セルの位置を設定します。
        /// 指定したセル位置が選択不可の場合は、同じ行で次に選択可能なセルを設定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex"></param>
        /// <param name="cellName"></param>
        public static void SetCurrentCellPosition(this GcMultiRow multiRow, int rowIndex, string cellName)
        {
            if (rowIndex < 0 || string.IsNullOrEmpty(cellName))
                return;

            int cellIndex = multiRow[rowIndex, cellName].CellIndex;

            CellPosition selectableCellPosition =
                multiRow.GetSelectableCellPositionOfRow(rowIndex, cellIndex, SearchOrderType.TabIndex);

            if (selectableCellPosition.IsEmpty)
                return;

            multiRow.CurrentCellPosition = selectableCellPosition;
        }



        /// <summary>
        /// 編集コントロールのインスタンスを保持しているかを返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <returns></returns>
        public static bool HasEditingControl(this GcMultiRow multiRow)
        {
            return multiRow.EditingControl != null;
        }


        #region 編集コントロールとセルの値を透過的に操作する


        /// <summary>
        /// 行インデックス、セル名を指定してセルの値を取得します。
        /// 編集セルと位置が重なった場合は編集セルから値を取得します。
        /// </summary>
        /// <returns>キーのセルの値</returns>
        public static object GetEditingOrCellValue(this GcMultiRow multiRow, int rowIndex, string cellName)
        {
            object rt_val = null;

            if (multiRow.CurrentCell == null)
            {
                return null;
            }

            //Mrow行を取得しておく
            GrapeCity.Win.MultiRow.Row wk_mrow = multiRow.Rows[rowIndex];
            //現在アクティブなセルのキーを取得
            string wk_curcellkey = multiRow.CurrentCell.Name;
            ////現在アクティブな行を取得
            int wk_curmrowidx = multiRow.CurrentRow.Index;

            //指定したセルがアクティブセルと同じかどうかで分岐
            if (wk_curmrowidx == rowIndex && cellName == wk_curcellkey && multiRow.EditingControl != null)
            {
                rt_val = GetEditingControlValue(multiRow.EditingControl);
            }
            else
            {
                //別ならMrowのセルから取得
                rt_val = wk_mrow[cellName].Value;
            }

            return rt_val;
        }

        /// <summary>
        /// MultiRowの編集コントロールからCell.Valueに相当するものを取得します。
        /// </summary>
        /// <param name="editingControl">MultiRowの編集コントロール（IEditingControlを実装するオブジェクト）</param>
        /// <returns>Cell.Valueに相当する値</returns>
        internal static object GetEditingControlValue(Control editingControl)
        {
            if (editingControl == null)
            {
                throw new ArgumentNullException("引数：editingControlがnullです。");
            }

            if (!(editingControl is IEditingControl))
            {
                throw new ArgumentException("引数：editingControlがIEditingControlを実装していません。");
            }

            //GcNumber
            {
                var ec = editingControl as GcNumber;

                if (ec != null)
                {
                    return ec.Value;
                }
            }

            //GcTextBox
            {
                var ec = editingControl as GcTextBox;

                if (ec != null)
                {
                    return ec.Text;
                }
            }

            //GcDate
            {
                var ec = editingControl as GcDate;

                if (ec != null)
                {
                    return ec.Value;
                }
            }

            //GcDateTimeEditingControl
            {
                var ec = editingControl as GcDateTimeEditingControl;

                if (ec != null)
                {
                    return ec.Value;
                }
            }

            //ComboBox
            {
                var ec = editingControl as ComboBoxEditingControl;

                if (ec != null)
                {
                    return ec.SelectedValue;
                }
            }

            //GcComboBox
            {
                var ec = editingControl as GcComboBoxEditingControl;

                if (ec != null)
                {
                    return ec.SelectedValue;
                }
            }

            throw new ArgumentException(editingControl.GetType().Name + " コントロールには対応していません。");
        }




        /// <summary>
        /// 編MultiRowの編集コントロールからCell.Valueに相当するものを取得します。
        /// </summary>
        /// <param name="multiRow"></param>
        public static object GetEditingControlValue(this GcMultiRow multiRow)
        {
            return GetEditingControlValue(multiRow.EditingControl);
        }


        /// <summary>
        /// MultiRowシート、行インデックス、セル名を指定して、値を指定したセルに設定します。
        /// 編集セルと位置が重なった場合は編集セルに値を設定します。
        /// </summary>
        /// <returns>キーのセルの値</returns>
        public static void SetEditingControlValueOrCellValue(this GcMultiRow multiRow, int rowIndex, string cellName, object value)
        {
            if (multiRow.CurrentCell == null)
            {
                return;
            }

            //Mrow行を取得しておく
            GrapeCity.Win.MultiRow.Row wk_mrow = multiRow.Rows[rowIndex];
            //現在アクティブなセルのキーを取得
            string wk_curcellkey = multiRow.CurrentCell.Name;
            ////現在アクティブな行を取得
            int wk_curmrowidx = multiRow.CurrentRow.Index;

            //指定したセルがアクティブセルと同じかどうかで分岐
            if (wk_curmrowidx == rowIndex && cellName == wk_curcellkey && multiRow.EditingControl != null)
            {
                SetEditingControlValue(multiRow.EditingControl, value);
            }
            else
            {
                //別ならMrowのセルから取得
                wk_mrow[cellName].Value = value;
            }
        }

        /// <summary>
        /// 編集コントロールに値を設定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="value"></param>
        public static void SetEditingControlValue(this GcMultiRow multiRow, object value)
        {
            if (multiRow.CurrentCell == null)
                return;

            if (multiRow.EditingControl == null)
                return;

            SetEditingControlValue(multiRow.EditingControl, value);
        }

        /// <summary>
        /// MultiRowの編集コントロールのCell.Valueに相当するものに値を設定します。
        /// </summary>
        /// <param name="editingControl">MultiRowの編集コントロール（IEditingControlを実装するオブジェクト）</param>
        /// <param name="value">値</param>
        internal static void SetEditingControlValue(Control editingControl, object value)
        {
            if (!(editingControl is IEditingControl))
            {
                throw new ArgumentNullException("引数：editingControlがIEditingControlを実装していません。");
            }

            if (editingControl == null)
            {
                throw new ArgumentNullException("引数：editingControlがnullです。");
            }

            //GcNumber
            {
                var ec = editingControl as GcNumber;

                if (ec != null)
                {
                    ec.Value = Convert.ToDecimal(value);
                    return;
                }
            }

            //GcTextBox
            {
                var ec = editingControl as GcTextBox;

                if (ec != null)
                {
                    ec.Text = Convert.ToString(value);
                    return;
                }
            }

            //GcDate
            {
                var ec = editingControl as GcDate;

                if (ec != null)
                {
                    ec.Value = value != null ? Convert.ToDateTime(value) : (DateTime?)null;
                    return;
                }
            }

            //GcDateEditingControl
            {
                var ec = editingControl as GcDateTimeEditingControl;

                if (ec != null)
                {
                    ec.Value = value != null ? Convert.ToDateTime(value) : (DateTime?)null;
                    return;
                }
            }


            //ComboBox
            {
                var ec = editingControl as ComboBoxEditingControl;

                if (ec != null)
                {
                    ec.SelectedValue = value;
                    return;
                }
            }

            //GcComboBox
            {
                var ec = editingControl as GcComboBoxEditingControl;

                if (ec != null)
                {
                    ec.SelectedValue = value;
                    return;
                }
            }

            throw new ArgumentException("引数に対応するeditingControlが存在しません。");
        }

        #endregion

        /// <summary>
        /// セルの編集を開始して、編集セルに指定した値を設定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="value">編集セルに設定する値</param>
        /// <returns></returns>
        public static void BeginEdit(this GcMultiRow multiRow, object value)
        {
            //編集開始する
            multiRow.BeginEdit(true);
            SetEditingControlValue(multiRow.EditingControl, value);
        }

     
        /// <summary>
        /// CurrentCell がnullかどうかを判断してnullでなければ true を返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <returns></returns>
        public static bool HasCurrentCell(this GcMultiRow multiRow)
        {
            return (multiRow.CurrentCell != null);
        }

        /// <summary>
        /// 指定したインデックスの行をRemoveします。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndexes"></param>
        public static void RemoveRows(this GcMultiRow multiRow, IEnumerable<int> rowIndexes)
        {
            foreach (var rowIndex in rowIndexes.OrderByDescending(i => i))
            {
                multiRow.Rows.Remove(rowIndex, 1);
            }
        }

        /// <summary>
        /// 指定した行インデックスの行を取得します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="indexes"></param>
        public static Row[] GetRows(this GcMultiRow multiRow, int[] indexes)
        {
            List<Row> rows = new List<Row>();

            foreach (var index in indexes)
            {
                rows.Add(multiRow.Rows[index]);
            }

            return rows.ToArray();
        }

        /// <summary>
        /// 新規行を除いた行数を取得します。
        /// </summary>
        /// <param name="multiRow"></param>
        public static int GetRowCountWithoutNewRow(this GcMultiRow multiRow)
        {
            int rowCount = multiRow.RowCount;

            if (multiRow.NewRowIndex > -1)
                rowCount--;

            return rowCount;
        }

        /// <summary>
        /// すべての行のVisbleをtrueに設定して表示します。
        /// </summary>
        /// <param name="multiRow"></param>
        public static void ShowAllRows(this GcMultiRow multiRow)
        {
            foreach (var row in multiRow.Rows)
            {
                row.Visible = true;
            }
        }

        /// <summary>
        /// 新規行を除く表示されているすべての行に指定した値を設定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="cellName"></param>
        /// <param name="value"></param>
        public static void SetValueShownRows(this GcMultiRow multiRow, string cellName, object value)
        {
            SetValueRows(multiRow, cellName, value, row => row.Visible);
        }

        /// <summary>
        /// 新規行を除いて、条件を満たす行に指定した値を設定します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="cellName">セル名</param>
        /// <param name="value">値</param>
        /// <param name="rowPredicate">値を設定する行の条件</param>
        public static void SetValueRows(this GcMultiRow multiRow, string cellName, object value, Func<Row, bool> rowPredicate)
        {
            foreach (var row in multiRow.Rows)
            {
                if (row.IsNewRow)
                    continue;

                if (rowPredicate(row))
                    row[cellName].Value = value;
            }
        }

        /// <summary>
        /// 指定した行のタグに格納されているオブジェクトを取得します。
        /// タグに格納されているオブジェクトなければオブジェクトを作成して返します。
        /// </summary>
        /// <typeparam name="T">取得するオブジェクトの型</typeparam>
        /// <param name="multiRow">GcMultiRowオブジェクト</param>
        /// <param name="rowIndex">行インデックス</param>
        /// <returns></returns>
        public static T GetOrNewRowTag<T>(this GcMultiRow multiRow, int rowIndex) where T : new()
        {
            var row = multiRow.Rows[rowIndex];

            if ((row.Tag != null) && !(row.Tag is T))
                throw new InvalidOperationException(typeof(T).Name + "型ではないオブジェクトが既に格納されています。");
            
            if (row.Tag == null)
                row.Tag = new T();
        
            return (T)row.Tag;
        }

        /// <summary>
        /// 選択行を次行に動かします。
        /// 選択行がない場合は先頭行を選択します。
        /// 最終行を選択している場合は選択をなくします。
        /// </summary>
        /// <param name="multiRow"></param>
        public static void MoveCurrentRowToNext(this GcMultiRow multiRow)
        {
            if (multiRow.RowCount == 0)
                return;

            var currentRow = multiRow.CurrentRow;
            //選択がない場合は先頭行を選択
            if (currentRow == null)
            {
                multiRow.CurrentCellPosition = multiRow.GetSelectableCellPositionOfRow(0, SearchOrderType.TabIndex);
                return;
            }
            //最終行の場合は選択をなくす
            if (currentRow.Index >= multiRow.RowCount - 1)
            {
                multiRow.CurrentCellPosition = CellPosition.Empty;
                return;
            }

            //***次行を選択する
            var currentCellPosition = multiRow.CurrentCellPosition;
            int currentRowIndex = currentCellPosition.RowIndex;
            int currentCellIndex = currentCellPosition.CellIndex;

            CellPosition nextCellPosition = multiRow.GetSelectableCellPositionOfRow(currentRowIndex + 1, currentCellIndex, SearchOrderType.TabIndex);
            if (!nextCellPosition.IsEmpty)
            {
                multiRow.CurrentCellPosition = nextCellPosition;
            }
        }

        /// <summary>
        /// 指定したインデックスの行が新規行かどうかを返します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        public static bool IsNewRow(this GcMultiRow multiRow, int rowIndex)
        {
            return (multiRow.AllowUserToAddRows && multiRow.NewRowIndex == rowIndex);
        }

        /// <summary>
        /// 新規行を除いた行を取得します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IEnumerable<Row> GetRowsWithoutNewRow(this GcMultiRow source)
        {
            foreach (var row in source.Rows)
            {
                if (row.IsNewRow)
                    continue;

                yield return row;
            }   
        }
    }
}
