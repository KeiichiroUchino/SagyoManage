using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 作業員割当情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoinWariateInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 作業員割当IDを取得・設定します。
        /// </summary>
        public Decimal SagyoinWariateId { get; set; }
        /// <summary>
        /// 作業案件IDを取得・設定します。
        /// </summary>
        public Decimal SagyoAnkenId { get; set; }
        /// <summary>
        /// 社員IDを取得・設定します。
        /// </summary>
        public Decimal StaffId { get; set; }
        /// <summary>
        /// 終日フラグを取得・設定します。
        /// </summary>
        public Boolean ShuJitsuFlg { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32 StaffCode { get; set; }
        /// <summary>
        /// 社員名称を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
        /// <summary>
        /// 社員区分コードを取得・設定します。
        /// </summary>
        public Decimal StaffKbnCode { get; set; }
        /// <summary>
        /// 社員区分名称を取得・設定します。
        /// </summary>
        public String StaffKbnName { get; set; }

        #endregion

    }

    /// <summary>
    /// 作業員割当情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoinWariateSearchParameter
    {
        /// <summary>
        /// 検索区分を取得・設定します。
        /// </summary>
        public int SearchKbn { get; set; }
        /// <summary>
        /// 作業員割当IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoinWariateId { get; set; }
        /// <summary>
        /// 作業案件IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoAnkenId { get; set; }
        /// <summary>
        /// 社員IDを取得・設定します。
        /// </summary>
        public Decimal? StaffId { get; set; }
        /// <summary>
        /// 社員IDリストを取得・設定します。
        /// </summary>
        public List<Decimal> StaffIdList { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime? SagyoStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime? SagyoEndDateTime { get; set; }
        /// <summary>
        /// 契約IDを取得・設定します。
        /// </summary>
        public Decimal? KeiyakuId { get; set; }

    }
}
