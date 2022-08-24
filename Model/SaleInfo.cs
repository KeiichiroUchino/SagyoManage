using Jpsys.HaishaManageV10.BizProperty;
using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 売上情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SaleInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 売上IDを取得・設定します。
        /// </summary>
        public decimal SaleId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// 日報IDを取得・設定します。
        /// </summary>
        public Decimal DailyReportId { get; set; }
        /// <summary>
        /// 売上伝票番号を取得・設定します。
        /// </summary>
        public Decimal SaleSlipNo { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }
        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal ContractId { get; set; }
        /// <summary>
        /// 発地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 発地名を取得・設定します。
        /// </summary>
        public String StartPointName { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public Decimal EndPointId { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointName { get; set; }
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal ItemId { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        /// 荷主IDを取得・設定します。
        /// </summary>
        public Decimal OwnerId { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerName { get; set; }
        /// <summary>
        /// 作業開始日付を取得・設定します。
        /// </summary>
        public DateTime TaskStartDate { get; set; }
        /// <summary>
        /// 作業開始時刻を取得・設定します。
        /// </summary>
        public TimeSpan TaskStartTime { get; set; }
        /// <summary>
        /// 作業終了日付を取得・設定します。
        /// </summary>
        public DateTime TaskEndDate { get; set; }
        /// <summary>
        /// 作業終了時刻を取得・設定します。
        /// </summary>
        public TimeSpan TaskEndTime { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal AtPrice { get; set; }
        /// <summary>
        /// 金額を取得・設定します。
        /// </summary>
        public Decimal Price { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 外税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceOutTaxCalc { get; set; }
        /// <summary>
        /// 外税額を取得・設定します。
        /// </summary>
        public Decimal PriceOutTax { get; set; }
        /// <summary>
        /// 内税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceInTaxCalc { get; set; }
        /// <summary>
        /// 内税額を取得・設定します。
        /// </summary>
        public Decimal PriceInTax { get; set; }
        /// <summary>
        /// 非課税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceNoTaxCalc { get; set; }
        /// <summary>
        /// 税区分を取得・設定します。
        /// </summary>
        public Int32 TaxDispKbn { get; set; }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpDate { get; set; }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Memo { get; set; }
        /// <summary>
        /// 請求締切日を取得・設定します。
        /// </summary>
        public DateTime ClmFixDate { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal CarOfChartererId { get; set; }
        /// <summary>
        /// 傭車金額を取得・設定します。
        /// </summary>
        public Decimal CharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車外税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceOutTaxCalc { get; set; }
        /// <summary>
        /// 傭車外税額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceOutTax { get; set; }
        /// <summary>
        /// 傭車内税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceInTaxCalc { get; set; }
        /// <summary>
        /// 傭車内税額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceInTax { get; set; }
        /// <summary>
        /// 傭車非課税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceNoTaxCalc { get; set; }
        /// <summary>
        /// 傭車税区分を取得・設定します。
        /// </summary>
        public Int32 CharterTaxDispKbn { get; set; }
        /// <summary>
        /// 傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpDate { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 傭車支払締切日を取得・設定します。
        /// </summary>
        public DateTime CharterPayFixDate { get; set; }
        /// <summary>
        /// 運賃を取得・設定します。
        /// </summary>
        public Decimal Fee { get; set; }
        /// <summary>
        /// 運賃_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInFee { get; set; }
        /// <summary>
        /// 運賃_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInFee { get; set; }
        /// <summary>
        /// 運賃外税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeOutTaxCalc { get; set; }
        /// <summary>
        /// 運賃外税額を取得・設定します。
        /// </summary>
        public Decimal FeeOutTax { get; set; }
        /// <summary>
        /// 運賃内税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeInTaxCalc { get; set; }
        /// <summary>
        /// 運賃内税額を取得・設定します。
        /// </summary>
        public Decimal FeeInTax { get; set; }
        /// <summary>
        /// 運賃非課税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeNoTaxCalc { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal Weight { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }
        /// <summary>
        /// 待機時間数量を取得・設定します。
        /// </summary>
        public Decimal TaikijikanNumber { get; set; }
        /// <summary>
        /// 待機時間単位IDを取得・設定します。
        /// </summary>
        public Decimal TaikijikanFigId { get; set; }
        /// <summary>
        /// 待機時間単価を取得・設定します。
        /// </summary>
        public Decimal TaikijikanAtPrice { get; set; }
        /// <summary>
        /// 金額_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInPrice { get; set; }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 運賃_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInFee { get; set; }
        /// <summary>
        /// 運賃_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInFee { get; set; }
        /// <summary>
        /// 荷積み時間数量を取得・設定します。
        /// </summary>
        public Decimal NizumijikanNumber { get; set; }
        /// <summary>
        /// 荷積み時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NizumijikanFigId { get; set; }
        /// <summary>
        /// 荷積み時間単価を取得・設定します。
        /// </summary>
        public Decimal NizumijikanAtPrice { get; set; }
        /// <summary>
        /// 荷卸し時間数量を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanNumber { get; set; }
        /// <summary>
        /// 荷卸し時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NioroshijikanFigId { get; set; }
        /// <summary>
        /// 荷卸し時間単価を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanAtPrice { get; set; }
        /// <summary>
        /// 附帯業務時間数量を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanNumber { get; set; }
        /// <summary>
        /// 附帯業務時間単位IDを取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanFigId { get; set; }
        /// <summary>
        /// 附帯業務時間単価を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanAtPrice { get; set; }

        #region 関連項目

        ///// <summary>
        ///// トラDON社員コードを取得・設定します。
        ///// </summary>
        //public Int32 ToraDONStaffCode { get; set; }
        ///// <summary>
        ///// トラDON社員名称を取得・設定します。
        ///// </summary>
        //public String ToraDONStaffName { get; set; }
        ///// <summary>
        ///// トラDON社員カナを取得・設定します。
        ///// </summary>
        //public String ToraDONStaffNameKana { get; set; }
        ///// <summary>
        ///// トラDON非表示フラグを取得・設定します。
        ///// </summary>
        //public Boolean ToraDONDisableFlag { get; set; }
        ///// <summary>
        ///// 所属グループコードを取得・設定します。
        ///// </summary>
        //public Int32 ShozokuGroupCode { get; set; }
        ///// <summary>
        ///// 所属グループ名称を取得・設定します。
        ///// </summary>
        //public String ShozokuGroupName { get; set; }

        #endregion
    }

    /// <summary>
    /// 売上検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SaleSearchParameter
    {
        /// <summary>
        /// 売上IDを取得・設定します。
        /// </summary>
        public Decimal? SaleId { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal? HaishaId { get; set; }
        /// <summary>
        /// 請求データ作成の抽出条件を取得・設定します。
        /// </summary>
        public SeikyuDataSakuseiConditionInfo SeikyuDataSakuseiConditionInfo { get; set; }
        /// <summary>
        /// 連携データのみフラグを取得・設定します。
        /// </summary>
        public Boolean OnlyLinkDataFlag { get; set; }
    }
}
