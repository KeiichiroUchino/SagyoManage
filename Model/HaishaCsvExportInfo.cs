using Jpsys.HaishaManageV10.ComLib.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配車情報CSV出力のエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class HaishaCsvExportInfo
    {
        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        [Field("営業所コード", Order = 1)]
        public Int32 BranchOfficeCd { get; set; }
        /// <summary>
        /// 営業所略称を取得・設定します。
        /// </summary>
        [Field("営業所略称", Order = 2)]
        public String BranchOfficeSNM { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        [Field("車両区分", Order = 3)]
        public Int32 CarKbn { get; set; }
        /// <summary>
        /// 車両区分名を取得・設定します。
        /// </summary>
        [Field("車両区分名", Order = 4)]
        public String CarKbnName { get; set; }
        /// <summary>
        /// 計上日を取得・設定します。
        /// </summary>
        public DateTime AddUpYMD { get; set; }
        /// <summary>
        /// 計上日表示文字列を取得・設定します。
        /// </summary>
        [Field("計上日", Order = 5)]
        public String AddUpYMDString
        {
            get
            {
                return
                    this.AddUpYMD == DateTime.MinValue
                    ? string.Empty
                    : this.AddUpYMD.ToString("yyyy/MM/dd");
            }
        }
        /// <summary>
        /// 傭車計上日を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpYMD { get; set; }
        /// <summary>
        /// 傭車計上日表示文字列を取得・設定します。
        /// </summary>
        [Field("傭車計上日", Order = 6)]
        public String CharterAddUpYMDString
        {
            get
            {
                return
                    this.CharterAddUpYMD == DateTime.MinValue
                    ? string.Empty
                    : this.CharterAddUpYMD.ToString("yyyy/MM/dd");
            }
        }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        [Field("得意先コード", Order = 7)]
        public Int32 TokuisakiCd { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        [Field("得意先名称", Order = 8)]
        public String TokuisakiNM { get; set; }
        /// <summary>
        /// 請求部門コードを取得・設定します。
        /// </summary>
        [Field("請求部門コード", Order = 9)]
        public Int32 ClmClassCd { get; set; }
        /// <summary>
        /// 請求部門名称を取得・設定します。
        /// </summary>
        [Field("請求部門名称", Order = 10)]
        public String ClmClassNM { get; set; }
        /// <summary>
        /// 請負コードを取得・設定します。
        /// </summary>
        [Field("請負コード", Order = 11)]
        public Int32 ContractCd { get; set; }
        /// <summary>
        /// 請負名称を取得・設定します。
        /// </summary>
        [Field("請負名称", Order = 12)]
        public String ContractNM { get; set; }
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        [Field("販路コード", Order = 13)]
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 販路名称を取得・設定します。
        /// </summary>
        [Field("販路名称", Order = 14)]
        public String HanroName { get; set; }
        /// <summary>
        /// 積日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 積日時表示文字列を取得・設定します。
        /// </summary>
        [Field("積日時", Order = 15)]
        public String TaskStartDateTimeString
        {
            get
            {
                return
                    this.TaskStartDateTime == DateTime.MinValue
                    ? string.Empty
                    : this.TaskStartDateTime.ToString("yyyy/MM/dd HH:mm");
            }
        }
        /// <summary>
        /// 発日時を取得・設定します。
        /// </summary>
        public DateTime StartYMD { get; set; }
        /// <summary>
        /// 発日時表示文字列を取得・設定します。
        /// </summary>
        [Field("発日時", Order = 16)]
        public String StartYMDString
        {
            get
            {
                return
                    this.StartYMD == DateTime.MinValue
                    ? string.Empty
                    : this.StartYMD.ToString("yyyy/MM/dd HH:mm");
            }
        }
        /// <summary>
        /// 着日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        /// <summary>
        /// 着日時表示文字列を取得・設定します。
        /// </summary>
        [Field("着日時", Order = 17)]
        public String TaskEndDateTimeString
        {
            get
            {
                return
                    this.TaskEndDateTime == DateTime.MinValue
                    ? string.Empty
                    : this.TaskEndDateTime.ToString("yyyy/MM/dd HH:mm");
            }
        }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 再使用可能日日時表示文字列を取得・設定します。
        /// </summary>
        [Field("再使用可能日時", Order = 18)]
        public String ReuseYMDString
        {
            get
            {
                return
                    this.ReuseYMD == DateTime.MinValue
                    ? string.Empty
                    : this.ReuseYMD.ToString("yyyy/MM/dd HH:mm");
            }
        }
        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        [Field("積地コード", Order = 19)]
        public Int32 StartPointCd { get; set; }
        /// <summary>
        /// 積地名称を取得・設定します。
        /// </summary>
        [Field("積地名称", Order = 20)]
        public String StartPointNM { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        [Field("着地コード", Order = 21)]
        public Int32 EndPointCd { get; set; }
        /// <summary>
        /// 着地名称を取得・設定します。
        /// </summary>
        [Field("積地名称", Order = 22)]
        public String EndPointNM { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        [Field("品目コード", Order = 23)]
        public Int32 ItemCd { get; set; }
        /// <summary>
        /// 品目名称を取得・設定します。
        /// </summary>
        [Field("品目名称", Order = 24)]
        public String ItemNM { get; set; }
        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        [Field("荷主コード", Order = 25)]
        public Int32 OwnerCd { get; set; }
        /// <summary>
        /// 荷主名称を取得・設定します。
        /// </summary>
        [Field("荷主名称", Order = 26)]
        public String OwnerNM { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        [Field("車両コード", Order = 27)]
        public Int32 CarCd { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        [Field("車番", Order = 28)]
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        [Field("車種コード", Order = 29)]
        public Int32 CarKindCd { get; set; }
        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        [Field("車種名称", Order = 30)]
        public String CarKindNM { get; set; }
        /// <summary>
        /// 乗務員コードを取得・設定します。
        /// </summary>
        [Field("乗務員コード", Order = 31)]
        public Int32 DriverCd { get; set; }
        /// <summary>
        /// 乗務員名称を取得・設定します。
        /// </summary>
        [Field("乗務員名称", Order = 32)]
        public String DriverNm { get; set; }
        /// <summary>
        /// 傭車先コードを取得・設定します。
        /// </summary>
        [Field("傭車先コード", Order = 33)]
        public Int32 TorihikiCd { get; set; }
        /// <summary>
        /// 傭車先名称を取得・設定します。
        /// </summary>
        [Field("傭車先名称", Order = 34)]
        public String TorihikiNm { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        [Field("単価", Order = 35)]
        public Decimal AtPrice { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        [Field("重量", Order = 36)]
        public Decimal Weight { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        [Field("数量", Order = 37)]
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        [Field("単位名称", Order = 38)]
        public String FigNm { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        [Field("金額", Order = 39)]
        public Decimal Price { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        [Field("金額（金額）", Order = 40)]
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        [Field("金額（通行料）", Order = 41)]
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        [Field("金額（附帯業務料）", Order = 42)]
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 外税対象額を取得・設定します。
        /// </summary>
        [Field("外税対象額", Order = 43)]
        public Decimal PriceOutTaxCalc { get; set; }
        /// <summary>
        /// 外税額を取得・設定します。
        /// </summary>
        [Field("外税額", Order = 44)]
        public Decimal PriceOutTax { get; set; }
        /// <summary>
        /// 内税対象額を取得・設定します。
        /// </summary>
        [Field("内税対象額", Order = 45)]
        public Decimal PriceInTaxCalc { get; set; }
        /// <summary>
        /// 内税額を取得・設定します。
        /// </summary>
        [Field("内税額", Order = 46)]
        public Decimal PriceInTax { get; set; }
        /// <summary>
        /// 非課税対象額を取得・設定します。
        /// </summary>
        [Field("非課税対象額", Order = 47)]
        public Decimal PriceNoTaxCalc { get; set; }
        /// <summary>
        /// 税区分を取得・設定します。
        /// </summary>
        [Field("税区分", Order = 48)]
        public Int32 TaxDispKbn { get; set; }
        /// <summary>
        /// 税区分名称を取得・設定します。
        /// </summary>
        public String TaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 税区分名称表示文字を取得・設定します。
        /// </summary>
        [Field("税区分名称", Order = 49)]
        public String TaxDispKbnShortNmString
        {
            get
            {
                return (this.FixFlag ? this.TaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        [Field("確定フラグ", Order = 50)]
        public Int32 FixFlagInt { get; set; }
        /// <summary>
        /// 傭車金額を取得・設定します。
        /// </summary>
        [Field("傭車金額", Order = 51)]
        public Decimal CharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        [Field("傭車金額（金額）", Order = 52)]
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        [Field("傭車金額（通行料）", Order = 53)]
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車外税対象額を取得・設定します。
        /// </summary>
        [Field("傭車外税対象額", Order = 54)]
        public Decimal CharterPriceOutTaxCalc { get; set; }
        /// <summary>
        /// 傭車外税額を取得・設定します。
        /// </summary>
        [Field("傭車外税額", Order = 55)]
        public Decimal CharterPriceOutTax { get; set; }
        /// <summary>
        /// 傭車内税対象額を取得・設定します。
        /// </summary>
        [Field("傭車内税対象額", Order = 56)]
        public Decimal CharterPriceInTaxCalc { get; set; }
        /// <summary>
        /// 傭車内税額を取得・設定します。
        /// </summary>
        [Field("傭車内税額", Order = 57)]
        public Decimal CharterPriceInTax { get; set; }
        /// <summary>
        /// 傭車非課税対象額を取得・設定します。
        /// </summary>
        [Field("傭車非課税対象額", Order = 58)]
        public Decimal CharterPriceNoTaxCalc { get; set; }
        /// <summary>
        /// 傭車税区分を取得・設定します。
        /// </summary>
        [Field("傭車税区分", Order = 59)]
        public Int32 CharterTaxDispKbn { get; set; }
        /// <summary>
        /// 傭車税区分名称を取得・設定します。
        /// </summary>
        public String CharterTaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 傭車税区分名称表示文字を取得・設定します。
        /// </summary>
        [Field("傭車税区分名称", Order = 60)]
        public String CharterTaxDispKbnShortNmString
        {
            get
            {
                return (this.CharterFixFlag ? this.CharterTaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        [Field("傭車確定フラグ", Order = 61)]
        public Int32 CharterFixFlagInt { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        [Field("乗務員売上金額", Order = 62)]
        public Decimal JomuinUriageKingaku { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        [Field("孫傭車先", Order = 63)]
        public String MagoYoshasaki { get; set; }
        /// <summary>
        /// 受領済フラグを取得・設定します。
        /// </summary>
        public Boolean ReceivedFlag { get; set; }
        /// <summary>
        /// 受領済フラグを取得・設定します。
        /// </summary>
        [Field("受領済フラグ", Order = 64)]
        public Int32 ReceivedFlagInt { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        [Field("備考", Order = 65)]
        public String Memo { get; set; }
        /// <summary>
        /// コメントを取得・設定します。
        /// </summary>
        [Field("コメント", Order = 66)]
        public String Comment { get; set; }
    }

    /// <summary>
    /// 配車情報CSV出力検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaCsvExportSearchParameter
    {
        /// <summary>
        /// 請求先指定区分を表す列挙体です。
        /// </summary>
        public enum SeikyusakiShiteiKbnItem : int
        {
            /// <summary>
            /// 1:締日
            /// </summary>
            Shimebi = 1,
            /// <summary>
            /// 2:個別
            /// </summary>
            Kobetsu = 2,
        }

        /// <summary>
        /// 日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDFrom { get; set; }
        /// <summary>
        /// 日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDTo { get; set; }
        /// <summary>
        /// 請求先指定区分を取得・設定します。
        /// </summary>
        public SeikyusakiShiteiKbnItem SeikyusakiShiteiKbn { get; set; }
        /// <summary>
        /// 締日を取得・設定します。
        /// </summary>
        public Int32 Shimebi { get; set; }
        /// <summary>
        /// 請求先IDリストを取得・設定します。
        /// </summary>
        public List<Decimal> SeikyusakiIdList { get; set; }

        /// <summary>
        /// 請求連携日報区分を取得・設定します。
        /// </summary>
        public Int32 SeikyuRenkeiDailyReportKbn { get; set; }
        /// <summary>
        /// 請求連携上書区分を取得・設定します。
        /// </summary>
        public Int32 SeikyuRenkeiUwagakiKbn { get; set; }

        #region 日報情報検索条件

        /// <summary>
        /// 日報日付を取得・設定します。
        /// </summary>
        public DateTime? DailyReportDate { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public decimal? CarId { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public decimal? CarOfChartererId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public decimal? DriverId { get; set; }

        #endregion
    }
}
