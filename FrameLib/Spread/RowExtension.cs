using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarPoint.Win.Spread;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// Rowクラスの拡張メソッドを定義します。
    /// </summary>
    public static class RowExtension
    {
        #region 拡張情報

        /// <summary>
        /// 拡張情報を取得します。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static RowExtendedInfo GetExtendedInfo(this Row row)
        {
            if (row.Tag == null)
            {
                RowExtendedInfo info = new RowExtendedInfo();
                row.Tag = info;

                return info;
            }
            else
            {
                RowExtendedInfo info = row.Tag as RowExtendedInfo;

                if (info == null)
                {
                    throw new InvalidOperationException("RowのTagに別のデータが埋め込まれています。");
                }

                return info;
            }
        }

        /// <summary>
        /// 編集状態を取得します。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool GetItemIsEdited(this Row row)
        {
            return row.GetExtendedInfo().ItemIsEdited;
        }
        
        /// <summary>
        /// 編集状態にします。
        /// </summary>
        /// <param name="row"></param>
        public static void ToItemEdited(this Row row)
        {
            row.GetExtendedInfo().ItemIsEdited = true;

            System.Diagnostics.Debug.WriteLine(
                string.Format("ToItemEdited rowIndex：{0}", row.Index));
        }

        /// <summary>
        /// 削除済みかどうかを取得します。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static bool GetItemIsDeleted(this Row row)
        {
            return row.GetExtendedInfo().ItemIsDeleted;
        }

        /// <summary>
        /// 削除済みにします。
        /// </summary>
        /// <param name="row"></param>
        public static void ToItemDeleted(this Row row)
        {
            row.GetExtendedInfo().ItemIsDeleted = true;
        }

        /// <summary>
        /// 元データを取得します。
        /// </summary>
        /// <param name="row"></param>
        /// <returns></returns>
        public static object GetSourceObject(this Row row)
        {
            return row.GetExtendedInfo().SourceObject;
        }

        /// <summary>
        /// 元データを設定します。
        /// </summary>
        /// <param name="row"></param>
        /// <param name="value"></param>
        public static void SetSourceObject(this Row row, object value)
        {
            row.GetExtendedInfo().SourceObject = value;
        }

        #endregion

        /// <summary>
        /// 削除済みにマーキングして、削除済みのスタイルに変更します。
        /// </summary>
        /// <param name="row"></param>
        public static void MarkDeleted(this Row row)
        {
            row.ToItemDeleted();
            row.SetDeletedStyle();
        }

        /// <summary>
        /// 指定した行を、業務データが削除された状態のスタイルに設定します。
        /// </summary>
        /// <param name="row">設定対象の行</param>
        public static void SetDeletedStyle(this Row row)
        {
            row.Locked = true;
            row.BackColor = System.Drawing.Color.Gray;
        }
    }
}
