using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON管理情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ToraDonSystemPropertyInfo : AbstractModelBase
    {
        /// <summary>
        /// 管理マスタのIDを取得・設定します。
        /// </summary>
        public decimal SystemPropertyId { get; set; }
        /// <summary>
        /// 会社名を取得・設定します。
        /// </summary>
        public string CompanyName { get; set; }
        /// <summary>
        /// 代表者名を取得・設定します。
        /// </summary>
        public string ChiefName { get; set; }
        /// <summary>
        /// 会社の郵便番号を取得・設定します。
        /// </summary>
        public string Postal { get; set; }
        /// <summary>
        /// 会社の住所1を取得・設定します。
        /// </summary>
        public string FirstAddress { get; set; }
        /// <summary>
        /// 会社の住所2を取得・設定します。
        /// </summary>
        public string SecondAddress { get; set; }
        /// <summary>
        /// 会社のTELを取得・設定します。
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 会社のFAXを取得・設定します。
        /// </summary>
        public string Fax { get; set; }
        /// <summary>
        /// 会社のURLを取得・設定します。
        /// </summary>
        public string Url { get; set; }
        /// <summary>
        /// 会社のメールアドレスを取得・設定します。
        /// </summary>
        public string MailAddress { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public string Memo { get; set; }
        /// <summary>
        /// 口座1を取得・設定します。
        /// </summary>
        public string FirstAccount { get; set; }
        /// <summary>
        /// 口座2を取得・設定します。
        /// </summary>
        public string SecondAccount { get; set; }
        /// <summary>
        /// 口座3を取得・設定します。
        /// </summary>
        public string ThirdAccount { get; set; }
        /// <summary>
        /// 口座番号印字区分を取得・設定します。
        /// </summary>
        public int AccountPrintKbn { get; set; }
        /// <summary>
        /// 既定の消費税率を取得・設定します。
        /// </summary>
        public decimal TaxRate { get; set; }
        /// <summary>
        /// 新消費税率を取得・設定します。
        /// </summary>
        public decimal NewTaxRate { get; set; }
        /// <summary>
        /// 新消費税率を適用開始する日付を取得・設定します。
        /// </summary>
        public DateTime NewTaxRateStartDate { get; set; }
        /// <summary>
        /// 期首年月を取得・設定します。
        /// </summary>
        public DateTime AccountStartDate { get; set; }
        /// <summary>
        /// 決算年月を取得・設定します。
        /// </summary>
        public DateTime AccountSettlementDate { get; set; }
        /// <summary>
        /// 先行年月を取得・設定します。
        /// </summary>
        public DateTime InputMaxDate { get; set; }
        /// <summary>
        /// ログ保存期間を取得・設定します。
        /// </summary>
        public int LogRemainDaySpan { get; set; }
        /// <summary>
        /// 最終月次集計年月を取得・設定します。
        /// </summary>
        public DateTime LastSummaryOfMonthDate { get; set; }
        /// <summary>
        /// 集計取消可能日を取得・設定します。
        /// </summary>
        public DateTime CancelAccClmEnableDate { get; set; }
        /// <summary>
        /// 給与締日を取得・設定します。
        /// </summary>
        public decimal SalaryFixDay { get; set; }
        /// <summary>
        /// コメントを取得・設定します。
        /// </summary>
        public string BillComments { get; set; }
        /// <summary>
        /// 支払明細書コメントを取得・設定します。
        /// </summary>
        public string PayDtlComments { get; set; }

        /// <summary>
        /// カナ検索モードをを表す列挙体です。
        /// </summary>
        public enum DefaultSearchKanaModeKbnItem : int
        {
            /// <summary>
            /// ～を含む
            /// </summary>
            KanaContain = 0,
            /// <summary>
            /// ～から始まる
            /// </summary>
            KanaFrom = 1,
        }

        /// <summary>
        /// カナ検索モードを取得・設定します。
        /// </summary>
        public DefaultSearchKanaModeKbnItem DefaultSearchKanaMode { get; set; }

        /// <summary>
        /// 運転日報入力数量初期値をを表す列挙体です。
        /// </summary>
        public enum DefaultDailyDriveReportNumberIem : int
        {
            /// <summary>
            /// 数量初期値は0
            /// </summary>
            Zero = 0,
            /// <summary>
            /// 数量初期値は1
            /// </summary>
            One = 1,
        }

        /// <summary>
        /// 運転日報入力数量初期値を取得・設定します。
        /// </summary>
        public DefaultDailyDriveReportNumberIem DefaultDailyDriveReportNumber { get; set; }

        /// <summary>
        /// 運転日報入力発日初期値をを表す列挙体です。
        /// </summary>
        public enum DefaultDailyDriveReportStartDateItem : int
        {
            /// <summary>
            /// 出庫日付
            /// </summary>
            StartDate = 0,
            /// <summary>
            /// 前行の発日
            /// </summary>
            FormerRowStartDate = 1,
            /// <summary>
            /// 前行の着日
            /// </summary>
            FormerRowEndDate = 2,
        }

        /// <summary>
        /// 運転日報入力発日初期値を取得・設定します。
        /// </summary>
        public DefaultDailyDriveReportStartDateItem DefaultDailyDriveReportStartDate { get; set; }
        /// <summary>
        /// 品目必須フラグを取得・設定します。
        /// </summary>
        public bool ItemMustFlag { get; set; }

        /// <summary>
        /// 日報収支経費科目1を取得・設定します。
        /// </summary>
        public decimal DRBalanceCostAccountId1 { get; set; }
        /// <summary>
        /// 日報収支経費科目2を取得・設定します。
        /// </summary>
        public decimal DRBalanceCostAccountId2 { get; set; }
        /// <summary>
        /// 日報収支経費科目3を取得・設定します。
        /// </summary>
        public decimal DRBalanceCostAccountId3 { get; set; }
        /// <summary>
        /// 日報収支経費科目4を取得・設定します。
        /// </summary>
        public decimal DRBalanceCostAccountId4 { get; set; }
        /// <summary>
        /// 日報収支経費科目5を取得・設定します。
        /// </summary>
        public decimal DRBalanceCostAccountId5 { get; set; }

        /// <summary>
        /// 日報収支経費科目1コードを取得・設定します。
        /// </summary>
        public int DRBalanceCostAccountCd1 { get; set; }
        /// <summary>
        /// 日報収支経費科目2コードを取得・設定します。
        /// </summary>
        public int DRBalanceCostAccountCd2 { get; set; }
        /// <summary>
        /// 日報収支経費科目3コードを取得・設定します。
        /// </summary>
        public int DRBalanceCostAccountCd3 { get; set; }
        /// <summary>
        /// 日報収支経費科目4コードを取得・設定します。
        /// </summary>
        public int DRBalanceCostAccountCd4 { get; set; }
        /// <summary>
        /// 日報収支経費科目5コードを取得・設定します。
        /// </summary>
        public int DRBalanceCostAccountCd5 { get; set; }

        /// <summary>
        /// 日報収支経費科目1名称を取得・設定します。
        /// </summary>
        public string DRBalanceCostAccountName1 { get; set; }
        /// <summary>
        /// 日報収支経費科目2コードを取得・設定します。
        /// </summary>
        public string DRBalanceCostAccountName2 { get; set; }
        /// <summary>
        /// 日報収支経費科目3コードを取得・設定します。
        /// </summary>
        public string DRBalanceCostAccountName3 { get; set; }
        /// <summary>
        /// 日報収支経費科目4コードを取得・設定します。
        /// </summary>
        public string DRBalanceCostAccountName4 { get; set; }
        /// <summary>
        /// 日報収支経費科目5コードを取得・設定します。
        /// </summary>
        public string DRBalanceCostAccountName5 { get; set; }

        #region 関連項目
        /// <summary>
        /// 日付を指定して、日付に対応する消費税率を小数倍で取得します。
        /// </summary>
        /// <param name="dt_">日付</param>
        /// <returns>消費税率(5%の場合は0.05)</returns>
        public decimal GetTaxRateByDay(DateTime dt_)
        {
            decimal rt_val = 0;

            //税率取得用
            decimal wk_taxrate = 0;

            if (this.NewTaxRateStartDate == DateTime.MinValue || dt_ < this.NewTaxRateStartDate)
            {
                //新消費税の適用日が無し, または新消費税の適用日よりも前の場合は現行の消費税率を返却する
                wk_taxrate = this.TaxRate;
            }
            else
            {
                //新消費税の適用日以降だったら新消費税率を返却
                wk_taxrate = this.NewTaxRate;
            }

            //少数倍にして返却
            return rt_val = wk_taxrate / 100;
        }

        #endregion

    }
}
