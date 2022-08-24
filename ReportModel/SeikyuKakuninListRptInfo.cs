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
    /// 請求確認リスト情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class SeikyuKakuninListRptInfo
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 売上IDを取得・設定します。
        /// </summary>
        public Decimal SaleId { get; set; }
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
        public Decimal TaskStartTime { get; set; }
        /// <summary>
        /// 作業終了日付を取得・設定します。
        /// </summary>
        public DateTime TaskEndDate { get; set; }
        /// <summary>
        /// 作業終了時刻を取得・設定します。
        /// </summary>
        public Decimal TaskEndTime { get; set; }
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

        #region 関連情報

        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public int BranchOfficeCode { get; set; }
        /// <summary>
        /// 営業所略名を取得・設定します。
        /// </summary>
        public String BranchOfficeShortName { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public int CarCode { get; set; }

        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public int CarKindCode { get; set; }
        /// <summary>
        /// 車種略名を取得・設定します。
        /// </summary>
        public String CarKindShortName { get; set; }
        /// <summary>
        /// 乗務員コードを取得・設定します。
        /// </summary>
        public int DriverCode { get; set; }
        /// <summary>
        /// 乗務員名を取得・設定します。
        /// </summary>
        public string DriverName { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public int TokuisakiCode { get; set; }
        /// <summary>
        /// 作業品名コードを取得・設定します。
        /// </summary>
        public int ItemCode { get; set; }
        /// <summary>
        /// 単位名を取得・設定します。
        /// </summary>
        public string FigName { get; set; }
        /// <summary>
        /// 傭車先名を取得・設定します。
        /// </summary>
        public string CarOfChartererName { get; set; }
        /// <summary>
        /// 傭車先名の略称を取得・設定します。
        /// </summary>
        public string CarOfChartererShortName { get; set; }
        /// <summary>
        /// 発地コードを取得・設定します。
        /// </summary>
        public int StartPointCode { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public int EndPointCode { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public int CarKbn { get; set; }

        /// <summary>
        /// 帳票グループキーを取得します。
        /// </summary>
        public string GroupKey
        {
            get
            {
                return this.ItemCode.ToString().PadLeft(6, '0') + "_" +
                    this.StartPointCode.ToString().PadLeft(5, '0') + "_" +
                    this.EndPointCode.ToString().PadLeft(5, '0');
            }
        }

        /// <summary>
        /// 税区分（表示項目）を取得します。
        /// </summary>
        public string DispZeiKbnMeisho
        {
            get
            {
                return this.TaxDispKbn == (int)DefaultProperty.ZeiKbn.Hikazei ? "非課税" : string.Empty;
            }
        }

        /// <summary>
        /// 船名（表示項目）を取得します。
        /// </summary>
        public string DispFunaMei
        {
            get
            {
                string ret = string.Empty;

                if (this.Memo != null && 0 < this.Memo.Length)
                {
                    if (0 < this.Memo.Split('\t').Count())
                    {
                        ret = this.Memo.Split('\t')[0];
                    }
                    else
                    {
                        ret = this.Memo;
                    }
                }

                return ret;
            }
        }

        /// <summary>
        /// コンテナNo（表示項目）を取得します。
        /// </summary>
        public string DispContainerNo
        {
            get
            {
                string ret = string.Empty;

                if (this.Memo != null && 0 < this.Memo.Length)
                {
                    if (1 < this.Memo.Split('\t').Count())
                    {
                        ret = this.Memo.Split('\t')[1];
                    }
                }

                return ret;
            }
        }

        #endregion
    }
}
