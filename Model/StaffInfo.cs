using System;
using System.Collections.Generic;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 社員情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class StaffInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 社員IDを取得・設定します。
        /// </summary>
        public Decimal StaffId { get; set; }
        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32 StaffCode { get; set; }
        /// <summary>
        /// 社員名称を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
        /// <summary>
        /// 社員区分IDを取得・設定します。
        /// </summary>
        public Decimal StaffKbnId { get; set; }
        /// <summary>
        /// 電話番号を取得・設定します。
        /// </summary>
        public String StaffTel { get; set; }
        /// <summary>
        /// メールアドレスを取得・設定します。
        /// </summary>
        public String MailAddress { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

        /// <summary>
        /// 社員区分コードを取得・設定します。
        /// </summary>
        public Decimal StaffKbnCode { get; set; }
        /// <summary>
        /// 社員区分名称を取得・設定します。
        /// </summary>
        public String StaffKbnName { get; set; }

        #endregion

        /// <summary>
        /// 使用停止を取得します。
        /// </summary>
        public string ShiyoTeishi
        {
            get { return (this.DisableFlag) ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 社員情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class StaffSearchParameter
    {
        /// <summary>
        /// 社員IDを取得・設定します。
        /// </summary>
        public Decimal? StaffId { get; set; }
        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32? StaffCode { get; set; }
        /// <summary>
        /// 社員名称を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }

        #region 一括検索用

        /// <summary>
        /// 社員ID一覧情報
        /// </summary>
        public IList<decimal> StaffList { get; set; }

        /// <summary>
        /// チェックされたIdをカンマ区切りで取得します。
        /// </summary>
        public String StaffCheckList
        {
            get
            {
                if (this.StaffList == null) return string.Empty;
                 StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (decimal id in this.StaffList)
                {
                    if (id > 0)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(id.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion
    }
}
