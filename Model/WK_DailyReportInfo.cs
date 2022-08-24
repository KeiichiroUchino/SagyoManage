using Jpsys.HaishaManageV10.BizProperty;
using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 日報ワーク情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class WK_DailyReportInfo : AbstractModelBase
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 日報IDを取得・設定します。
        /// </summary>
        public decimal DailyReportId { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public decimal CarDispatchId { get; set; }
        /// <summary>
        /// 日報日付を取得・設定します。
        /// </summary>
        public DateTime DailyReportDate { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 天候Idを取得・設定します。
        /// </summary>
        public decimal WeatherId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public decimal CarId { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public string LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public decimal CarKindId { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public decimal CarOfChartererId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public decimal DriverId { get; set; }
        /// <summary>
        /// 出庫日付を取得・設定します。
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 出庫時刻を取得・設定します。
        /// </summary>
        public TimeSpan StartTime { get; set; }
        /// <summary>
        /// 出庫メーターを取得・設定します。
        /// </summary>
        public decimal StartMeter { get; set; }
        /// <summary>
        /// 帰庫日付を取得・設定します。
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 帰庫時刻を取得・設定します。
        /// </summary>
        public TimeSpan EndTime { get; set; }
        /// <summary>
        /// 帰庫メーターを取得・設定します。
        /// </summary>
        public decimal EndMeter { get; set; }
        /// <summary>
        /// 稼働日数を取得・設定します。
        /// </summary>
        public decimal WorkDays { get; set; }
        /// <summary>
        /// 輸送回数を取得・設定します。
        /// </summary>
        public decimal TransportTimes { get; set; }
        /// <summary>
        /// 輸送トン数を取得・設定します。
        /// </summary>
        public decimal TransportWeight { get; set; }
        /// <summary>
        /// 走行キロを取得・設定します。
        /// </summary>
        public decimal MileageKm { get; set; }
        /// <summary>
        /// 実車キロを取得・設定します。
        /// </summary>
        public decimal ActualKm { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public bool DelFlag { get; set; }
    }

    /// <summary>
    /// 日報ワーク検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class WK_DailyReportSearchParameter
    {
        /// <summary>
        /// 日報IDを取得・設定します。
        /// </summary>
        public Decimal? DailyReportId { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal? HaishaId { get; set; }
        /// <summary>
        /// 請求データ作成の抽出条件を取得・設定します。
        /// </summary>
        public SeikyuDataSakuseiConditionInfo SeikyuDataSakuseiConditionInfo { get; set; }
    }
}
