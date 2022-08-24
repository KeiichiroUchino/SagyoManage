using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配色情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaShosaiNyuryokuInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /***************
         * 受注
         ***************/

        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal Weight { get; set; }
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
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCode { get; set; }
        /// <summary>
        /// 営業所名称を取得・設定します。
        /// </summary>
        public String BranchOfficeName { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 発地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCode { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCode { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCode { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        /// 作業開始日付を取得・設定します。
        /// </summary>
        public DateTime? TaskStartYMD { get; set; }
        /// <summary>
        /// 作業終了日付を取得・設定します。
        /// </summary>
        public DateTime? TaskEndYMD { get; set; }
        /// <summary>
        /// 削除対象Flgを取得・設定します。
        /// </summary>
        public bool DelTargetFlg { get; set; }

        /***************
         * 単位
         ***************/

        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }

        /***************
         * 販路
         ***************/

        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 販路名称を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// 往復区分を取得・設定します。
        /// </summary>
        public Int32 OfukuKbn { get; set; }
        /// <summary>
        /// 行程日数を取得・設定します。
        /// </summary>
        public Int32 KoteiNissu { get; set; }
        /// <summary>
        /// 行程時間を取得・設定します。
        /// </summary>
        public Int32 KoteiJikan { get; set; }

        /***************
         * 社員
         ***************/

        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32? StaffCd { get; set; }
        /// <summary>
        /// 氏名を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
    }

    /// <summary>
    /// 積載時間帯区分の取得のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SekisaiJikanKbnSearchParameter
    {
        /// <summary>
        /// 配色IDを取得・設定します。
        /// </summary>
        public Decimal? HaishokuId { get; set; }
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32? TableKey { get; set; }
        /// <summary>
        /// テーブルIDを取得・設定します。
        /// </summary>
        public Decimal? TableId { get; set; }
        /// <summary>
        /// システム区分を取得・設定します。
        /// </summary>
        public Int32? SystemKbn { get; set; }
        /// <summary>
        /// システムIDを取得・設定します。
        /// </summary>
        public Int32? SystemId { get; set; }
    }
}
