using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 休刊カレンダ情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KyujitsuCalendarInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 休刊カレンダIDを取得・設定します。
        /// </summary>
        public Decimal KyujitsuCalendarId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// 年度を取得・設定します。
        /// </summary>
        public Int32 Nendo { get; set; }
        /// <summary>
        /// トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONStaffId { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }
        /// <summary>
        /// 休刊カレンダ明細情報リストを取得・設定します。
        /// </summary>
        public IList<KyujitsuCalendarMeisaiInfo> KyujitsuCalendarMeisaiList { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON社員コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONStaffCode { get; set; }
        /// <summary>
        /// トラDON社員名を取得・設定します。
        /// </summary>
        public String ToraDONStaffName { get; set; }

        #endregion
    }

    /// <summary>
    /// 休刊カレンダ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KyujitsuCalendarSearchParameter
    {
        /// <summary>
        /// 休刊カレンダIDを取得・設定します。
        /// </summary>
        public Decimal? KyujitsuCalendarId { get; set; }
        /// <summary>
        /// 年度を取得・設定します。
        /// </summary>
        public Int32? Nendo { get; set; }
        /// <summary>
        /// トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONStaffId { get; set; }
    }
}
