using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 受注入力画面の出力情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuNyuryokuInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 日報IDを取得・設定します。
        /// </summary>
        public Decimal DailyReportId { get; set; }
        /// <summary>
        /// 受注伝票番号を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNo { get; set; }
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
        public String TokuisakiNM { get; set; }
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }
        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal ContractId { get; set; }
        /// <summary>
        /// 積地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public String StartPointNM { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public Decimal EndPointId { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointNM { get; set; }
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal ItemId { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemNM { get; set; }
        /// <summary>
        /// 荷主IDを取得・設定します。
        /// </summary>
        public Decimal OwnerId { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerNM { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
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
        public DateTime AddUpYMD { get; set; }
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
        public DateTime ClmFixYMD { get; set; }
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
        public DateTime CharterAddUpYMD { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 傭車支払締切日を取得・設定します。
        /// </summary>
        public DateTime CharterPayFixYMD { get; set; }
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
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId { get; set; }
        /// <summary>
        /// 受注担当IDを取得・設定します。
        /// </summary>
        public Decimal JuchuTantoId { get; set; }
        /// <summary>
        /// 往復区分コードを取得・設定します。
        /// </summary>
        public Int32 OfukuKbn { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }
        /// <summary>
        /// 受領済フラグを取得・設定します。
        /// </summary>
        public Boolean ReceivedFlag { get; set; }
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
        /// <summary>
        /// 乗務員売上同額フラグを取得・設定します。
        /// </summary>
        public Boolean JomuinUriageDogakuFlag { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }

        #region 関連項目

        /// <summary>
        /// 営業先コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCd { get; set; }

        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCd { get; set; }

        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCd { get; set; }

        /// <summary>
        /// 販路名を取得・設定します。
        /// </summary>
        public String HanroNm { get; set; }

        /// <summary>
        /// 諸口区分を取得・設定します。
        /// </summary>
        public Int32 MemoAccount { get; set; }

        /// <summary>
        /// 部門管理区分を取得・設定します。
        /// </summary>
        public Int32 ClmClassUseKbn { get; set; }

        /// <summary>
        /// 請求部門コードを取得・設定します。
        /// </summary>
        public Int32 ClmClassCd { get; set; }

        /// <summary>
        /// 請求部門略称を取得・設定します。
        /// </summary>
        public String ClmClassSNM { get; set; }

        /// <summary>
        /// 請負コードを取得・設定します。
        /// </summary>
        public Int32 ContractCd { get; set; }

        /// <summary>
        /// 請負名を取得・設定します。
        /// </summary>
        public String ContractNm { get; set; }

        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCd { get; set; }

        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCd { get; set; }

        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCd { get; set; }

        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCd { get; set; }

        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigNm { get; set; }

        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCd { get; set; }

        /// <summary>
        /// 車両営業所略称を取得・設定します。
        /// </summary>
        public String CarBranchOfficeSNM { get; set; }

        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32 CarKbn { get; set; }

        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32 CarKindCd { get; set; }

        /// <summary>
        /// 車種略称を取得・設定します。
        /// </summary>
        public String CarKindSNM { get; set; }

        /// <summary>
        /// 乗務員コードを取得・設定します。
        /// </summary>
        public Int32 DriverCd { get; set; }

        /// <summary>
        /// 乗務員名称を取得・設定します。
        /// </summary>
        public String DriverNm { get; set; }

        /// <summary>
        /// 庸車先コードを取得・設定します。
        /// </summary>
        public Int32 CarOfChartererCd { get; set; }

        /// <summary>
        /// 庸車先名を取得・設定します。
        /// </summary>
        public String CarOfChartererShortNm { get; set; }

        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        public Int32 OwnerCd { get; set; }

        /// <summary>
        /// 受注担当コードを取得・設定します。
        /// </summary>
        public Int32 JuchuTantoCd { get; set; }

        /// <summary>
        /// 受注担当名称を取得・設定します。
        /// </summary>
        public String JuchuTantoNm { get; set; }

        /// <summary>
        /// 往復区分名を取得・設定します。
        /// </summary>
        public String OfukuKbnNm { get; set; }
        //TODO 性能問題のためコメント化中 START
        ///// <summary>
        ///// トラDON請求締切日を取得・設定します。
        ///// </summary>
        //public DateTime MinClmFixDate { get; set; }
        ///// <summary>
        ///// トラDON傭車支払締切日を取得・設定します。
        ///// </summary>
        //public DateTime MinCharterPayFixDate { get; set; }
        ///// <summary>
        ///// トラDON計上日を取得・設定します。
        ///// </summary>
        //public DateTime MinAddUpDate { get; set; }
        ///// <summary>
        ///// トラDON傭車計上日を取得・設定します。
        ///// </summary>
        //public DateTime MinCharterAddUpDate { get; set; }
        ///// <summary>
        ///// トラDON確定フラグを取得・設定します。
        ///// </summary>
        //public Boolean MaxFixFlag { get; set; }
        ///// <summary>
        ///// トラDON傭車確定フラグを取得・設定します。
        ///// </summary>
        //public Boolean MaxCharterFixFlag { get; set; }
        //TODO 性能問題のためコメント化中 END
        /// <summary>
        /// 不変更フラグを取得・設定します。
        /// </summary>
        public Boolean UnChange { get; set; }
        
        #endregion
    }

    /// <summary>
    /// 受注入力画面の検索条件のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuNyuryokuSearchParameter
    {
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNo { get; set; }
    }

    /// <summary>
    /// 税区分および端数区分に応じて計算された消費税とその基になる金額など
    /// を保持するビジネスエンティティオブジェクトです。
    /// </summary>
    [Serializable]
    public class CalcTaxResultStructInfo
    {
        /// <summary>
        /// 税計算を行う基になる、または基になった金額を取得・設定します。
        /// </summary>
        public decimal BaseAmount { get; set; }

        /// <summary>
        /// 税計算を行う際に使用するまたは使用した税率を取得・設定します。値は100分率値です。
        /// （ex:5%の場合5）
        /// </summary>
        public decimal TaxRate { get; set; }

        /// <summary>
        /// 税計算を行い基になる金額から税額を引いたもの（税抜き額）を取得・設定
        /// します。外税または無税・非課税の場合には基になる金額と同じになります。
        /// </summary>
        public decimal WithoutTaxAmount { get; set; }

        /// <summary>
        /// 税計算を行い基になる金額から計算された税額を取得・設定します。
        /// 無税・非課税の場合には0となります。いわゆる税額です。
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// 税計算を行い、基になる金額と税額を合算した金額を取得・設定します。
        /// 内税・無税・非課税の場合は基になる金額と同じになります。いわゆる税込み額です。
        /// </summary>
        public decimal InTaxAmount { get; set; }

        /// <summary>
        /// 税計算に使用する税区分を取得・設定します。
        /// </summary>
        public BizProperty.DefaultProperty.ZeiKbn CurrentTaxDispKbn { get; set; }

        /// <summary>
        /// 税計算に使用する端数丸め区分を取得・設定します。
        /// </summary>
        public BizProperty.DefaultProperty.HasuMArumeKbn CurrentCutKbn { get; set; }
    }

    /// <summary>
    /// 単価マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class AtPriceInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 単価IDを取得・設定します。
        /// </summary>
        public decimal AtPriceId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }

        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public int TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public string TokuisakiName { get; set; }
        /// <summary>
        /// 得意先名の略称を取得・設定します。
        /// </summary>
        public string TokuisakiShortName { get; set; }
        /// <summary>
        /// 作業品名IDを取得・設定します。
        /// </summary>
        public decimal ItemId { get; set; }
        /// <summary>
        /// 作業品名コードを取得・設定します。
        /// </summary>
        public int ItemCode { get; set; }
        /// <summary>
        /// 作業品名を取得・設定します。
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 積地IDを取得・設定します。
        /// </summary>
        public decimal StartPointId { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public int StartPointCode { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public string StartPointName { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public decimal EndPointId { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public int EndPointCode { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public string EndPointName { get; set; }
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public decimal FigId { get; set; }
        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public int FigCode { get; set; }
        /// <summary>
        /// 単位名を取得・設定します。
        /// </summary>
        public string FigName { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public decimal CarKindId { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public int CarKindCode { get; set; }
        /// <summary>
        /// 車種名を取得・設定します。
        /// </summary>
        public string CarKindName { get; set; }
        /// <summary>
        /// 車種名の略称を取得・設定します。
        /// </summary>
        public string CarKindShortName { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public decimal AtPrice { get; set; }
        /// <summary>
        /// 設定フラグを取得・設定します。
        /// </summary>
        public string EstablishmentFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public bool DelFlag { get; set; }
    }

}
