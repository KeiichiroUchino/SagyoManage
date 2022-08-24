using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 休日カレンダ明細情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KyujitsuCalendarMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 休日カレンダIDを取得・設定します。
        /// </summary>
        public Decimal KyujitsuCalendarId { get; set; }
        /// <summary>
        /// 日付を取得・設定します。
        /// </summary>
        public DateTime HizukeYMD { get; set; }
        /// <summary>
        /// 休日区分を取得・設定します。
        /// </summary>
        public Int32 KyujitsuKbn { get; set; }
        /// <summary>
        /// メモを取得・設定します。
        /// </summary>
        public String Memo { get; set; }

        #region 関連項目

        /// <summary>
        /// 休日日数を取得・設定します。
        /// </summary>
        public Decimal KyujitsuNissu { get; set; }

        #endregion
    }

    /// <summary>
    /// 休日カレンダ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KyujitsuCalendarMeisaiSearchParameter
    {
        /// <summary>
        /// 休日カレンダIDを取得・設定します。
        /// </summary>
        public Decimal? KyujitsuCalendarId { get; set; }
        /// <summary>
        /// 年度を取得・設定します。
        /// </summary>
        public Int32? Nendo { get; set; }
        /// <summary>
        /// 除外トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal? ExcludeToraDONStaffId { get; set; }
    }
}
