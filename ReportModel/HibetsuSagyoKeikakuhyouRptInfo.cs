using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ReportModel
{
    /// <summary>
    /// 日別作業計画表情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class HibetsuSagyoKeikakuhyouRptInfo
    {
		/// <summary>
		/// 会社名を取得・設定します。
		/// </summary>
		public String CompanyName { get; set; }
		/// <summary>
		/// 作業日付を取得・設定します。
		/// </summary>
		public String SagyoYMD { get; set; }
		/// <summary>
		/// 号車（車両）コードを取得・設定します。
		/// </summary>
		public Int32 CarCode { get; set; }
		/// <summary>
		/// 車両No（車番）を取得・設定します。
		/// </summary>
		public String LicPlateCarNo { get; set; }
		/// <summary>
		/// 責任者名称を取得・設定します。
		/// </summary>
		public String SekininshaName { get; set; }
		/// <summary>
		/// 施工者（社員名称）を取得・設定します。
		/// </summary>
		public String StaffName { get; set; }
		/// <summary>
		/// 契約番号（契約コードを）取得・設定します。
		/// </summary>
		public String KeiyakuCode { get; set; }
		/// <summary>
		/// 予定開始時間（作業開始日時）取得・設定します。
		/// </summary>
		public String YoteiKaishiDate { get; set; }
		/// <summary>
		/// 予定終了時間（作業終了日時）取得・設定します。
		/// </summary>
		public String YoteiShuryoDate { get; set; }
		/// <summary>
		/// 作業場所名称を取得・設定します。
		/// </summary>
		public String SagyoBashoName { get; set; }
		/// <summary>
		/// 作業コードを取得・設定します。
		/// </summary>
		public String SagyoCode { get; set; }
		/// <summary>
		/// 作業名を取得・設定します。
		/// </summary>
		public String SagyoName { get; set; }
		/// <summary>
		/// 備考（作業内容等）を取得・設定します。
		/// </summary>
		public String Biko { get; set; }
		/// <summary>
		/// 備考バイト数を取得・設定します。
		/// </summary>
		public int Biko_LenB { get; set; }
		/// <summary>
		/// 特記事項を取得・設定します。
		/// </summary>
		public String TokkiJiko { get; set; }
		/// <summary>
		/// 特記事項バイト数を取得・設定します。
		/// </summary>
		public int TokkiJiko_LenB { get; set; }
	}


	/// <summary>
	/// 日別作業計画表（印刷条件）のエンティティコンポーネントを作成します。
	/// </summary>
	[Serializable]
    public class HibetsuSagyoKeikakuhyouRptInfoSearchParameter
    {
        /// <summary>
        /// 日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime SagyoYMDFrom { get; set; }
        /// <summary>
        /// 日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime SagyoYMDTo { get; set; }
        /// <summary>
        /// 社員一覧情報
        /// </summary>
        public IList<decimal> StaffList { get; set; }


        #region 関連項目
        /// <summary>
        /// チェックされた乗務員Idをカンマ区切りで取得します。
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
