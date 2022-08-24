using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON請負テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONContractInfo
    {
        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal ContractId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }
        /// <summary>
        /// 請負番号を取得・設定します。
        /// </summary>
        public Int32 ContractCode { get; set; }
        /// <summary>
        /// 請負名を取得・設定します。
        /// </summary>
        public String ContractName { get; set; }
        /// <summary>
        /// 請負開始を取得・設定します。
        /// </summary>
        public DateTime ContractStartDate { get; set; }
        /// <summary>
        /// 請負終了を取得・設定します。
        /// </summary>
        public DateTime ContractEndDate { get; set; }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpDate { get; set; }
        /// <summary>
        /// 請負金額を取得・設定します。
        /// </summary>
        public Decimal ContractPrice { get; set; }
        /// <summary>
        /// 請負金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInContractPrice { get; set; }
        /// <summary>
        /// 請負金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInContractPrice { get; set; }
        /// <summary>
        /// 税区分を取得・設定します。
        /// </summary>
        public Int32 TaxDispKbn { get; set; }
        /// <summary>
        /// 完了区分を取得・設定します。
        /// </summary>
        public Int32 EndKbn { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Memo { get; set; }
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
        /// 請求締切日を取得・設定します。
        /// </summary>
        public DateTime ClmFixDate { get; set; }

        #region 関連項目

        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 請求部門コードを取得・設定します。
        /// </summary>
        public Int32 ClmClassCode { get; set; }
        /// <summary>
        /// 請求部門名称を取得・設定します。
        /// </summary>
        public String ClmClassName { get; set; }
        /// <summary>
        /// 税区分名称を取得・設定します。
        /// </summary>
        public String TaxDispKbnShortName { get; set; }

        #endregion
    }

    /// <summary>
    /// 請負検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ToraDONContractSearchParameter
    {
        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal? ContractId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiId { get; set; }
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal? ClmClassId { get; set; }
        /// <summary>
        /// 請負番号を取得・設定します。
        /// </summary>
        public Int32? ContractCode { get; set; }
    }
}
