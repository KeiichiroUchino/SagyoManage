using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ReportModel
{
    /// <summary>
    /// 受注配車一覧表情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class JuchuHaishaIchiranHyoRptInfo
    {
        /***************
         * キー
         ***************/
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal HaishaId { get; set; }
        /// <summary>
        /// 配車行番号を取得・設定します。
        /// </summary>
        public Int32 HaishaRowIndex { get; set; }

        /***************
         * 受注
         ***************/

        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCd { get; set; }
        /// <summary>
        /// 営業所略名を取得・設定します。
        /// </summary>
        public String BranchOfficeSNM { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCd { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiNM { get; set; }
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCd { get; set; }
        /// <summary>
        /// 販路名を取得・設定します。
        /// </summary>
        public String HanroNm { get; set; }
        /// <summary>
        /// 往復区分名を取得・設定します。
        /// </summary>
        public String OfukuKbnNm { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCd { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public String StartPointNM { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCd { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointNM { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime StartYMD { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCd { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemNM { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCd { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
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
        /// 傭車先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCd { get; set; }
        /// <summary>
        /// 傭車先略称を取得・設定します。
        /// </summary>
        public String TorihikiSNm { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal AtPrice { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal Weight { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigNm { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 税区分名称を取得・設定します。
        /// </summary>
        public String TaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 税区分名称表示文字を取得・設定します。
        /// </summary>
        public String TaxDispKbnShortNmString
        {
            get
            {
                return (this.FixFlag ? this.TaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpYMD { get; set; }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
        /// <summary>
        /// 確定フラグ表示文字を取得・設定します。
        /// </summary>
        public String FixFlagString
        {
            get
            {
                return (this.FixFlag ? "✓" : string.Empty);
            }
        }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車税区分名称を取得・設定します。
        /// </summary>
        public String CharterTaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 傭車税区分名称表示文字を取得・設定します。
        /// </summary>
        public String CharterTaxDispKbnShortNmString
        {
            get
            {
                return (this.CharterFixFlag ? this.CharterTaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpYMD { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 傭車確定フラグ表示文字を取得・設定します。
        /// </summary>
        public String CharterFixFlagString
        {
            get
            {
                return (this.CharterFixFlag ? "✓" : string.Empty);
            }
        }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }
        /// <summary>
        /// 受注担当コードを取得・設定します。
        /// </summary>
        public Int32 JuchuTantoCd { get; set; }
        /// <summary>
        /// 受注担当名称を取得・設定します。
        /// </summary>
        public String JuchuTantoNm { get; set; }
        /// <summary>
        /// 受注伝票番号を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNo { get; set; }
        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        public Int32 OwnerCd { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerNM { get; set; }
        /// <summary>
        /// 請求備考／コメントを取得・設定します。
        /// </summary>
        public String Memo { get; set; }
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
        /// 受領済フラグ表示文字を取得・設定します。
        /// </summary>
        public String ReceivedFlagString
        {
            get
            {
                return (this.ReceivedFlag ? "✓" : string.Empty);
            }
        }

        /***************
         * 配車
         ***************/

        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_StartPointCd { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public String Haisha_StartPointNM { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_EndPointCd { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String Haisha_EndPointNM { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime Haisha_TaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime Haisha_TaskEndDateTime { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime Haisha_StartYMD { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_ItemCd { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String Haisha_ItemNM { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_CarCd { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String Haisha_LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_CarKindCd { get; set; }
        /// <summary>
        /// 車種略称を取得・設定します。
        /// </summary>
        public String Haisha_CarKindSNM { get; set; }
        /// <summary>
        /// 乗務員コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_DriverCd { get; set; }
        /// <summary>
        /// 乗務員名称を取得・設定します。
        /// </summary>
        public String Haisha_DriverNm { get; set; }
        /// <summary>
        /// 傭車先コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_TorihikiCd { get; set; }
        /// <summary>
        /// 傭車先略称を取得・設定します。
        /// </summary>
        public String Haisha_TorihikiSNm { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal Haisha_AtPrice { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal Haisha_Weight { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Haisha_Number { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String Haisha_FigNm { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal Haisha_PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal Haisha_TollFeeInPrice { get; set; }
        /// <summary>
        /// 税区分名称を取得・設定します。
        /// </summary>
        public String Haisha_TaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 税区分名称表示文字を取得・設定します。
        /// </summary>
        public String Haisha_TaxDispKbnShortNmString
        {
            get
            {
                return (this.Haisha_FixFlag ? this.Haisha_TaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime Haisha_AddUpYMD { get; set; }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean Haisha_FixFlag { get; set; }
        /// <summary>
        /// 確定フラグ表示文字を取得・設定します。
        /// </summary>
        public String Haisha_FixFlagString
        {
            get
            {
                return (this.Haisha_FixFlag ? "✓" : string.Empty);
            }
        }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal Haisha_PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal Haisha_TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車税区分名称を取得・設定します。
        /// </summary>
        public String Haisha_CharterTaxDispKbnShortNm { get; set; }
        /// <summary>
        /// 傭車税区分名称表示文字を取得・設定します。
        /// </summary>
        public String Haisha_CharterTaxDispKbnShortNmString
        {
            get
            {
                return (this.Haisha_CharterFixFlag ? this.Haisha_CharterTaxDispKbnShortNm : "●");
            }
        }
        /// <summary>
        /// 傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime Haisha_CharterAddUpYMD { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean Haisha_CharterFixFlag { get; set; }
        /// <summary>
        /// 傭車確定フラグ表示文字を取得・設定します。
        /// </summary>
        public String Haisha_CharterFixFlagString
        {
            get
            {
                return (this.Haisha_CharterFixFlag ? "✓" : string.Empty);
            }
        }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal Haisha_FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal Haisha_JomuinUriageKingaku { get; set; }
        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        public Int32 Haisha_OwnerCd { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String Haisha_OwnerNM { get; set; }
        /// <summary>
        /// 請求備考／コメントを取得・設定します。
        /// </summary>
        public String Haisha_Biko { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime Haisha_ReuseYMD { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String Haisha_MagoYoshasaki { get; set; }
        /// <summary>
        /// トラDONバージョン区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONVersionKbn { get; set; }
    }

    /// <summary>
    /// 受注配車一覧表の検索条件のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuHaishaIchiranHyoSearchParameter
    {
        /// <summary>
        /// 日付指定区分を取得・設定します。
        /// </summary>
        public Int32 FilterDateKbns { get; set; }

        /// <summary>
        /// 対象日（From）を取得・設定します。
        /// </summary>
        public DateTime TaishoYMDFrom { get; set; }

        /// <summary>
        /// 対象日（To）を取得・設定します。
        /// </summary>
        public DateTime TaishoYMDTo { get; set; }

        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }

        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }

        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }

        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }

        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal CarOfChartererId { get; set; }

        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }

        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }

        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal ContractId { get; set; }

        /// <summary>
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId { get; set; }

        /// <summary>
        /// 受注担当IDを取得・設定します。
        /// </summary>
        public Decimal JuchuTantoId { get; set; }

        /// <summary>
        /// 受注伝票番号（From）を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNoFrom { get; set; }

        /// <summary>
        /// 受注伝票番号（To）を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNoTo { get; set; }
        /// <summary>
        /// トラDONバージョン区分を取得・設定します。
        /// </summary>
        public Int32 TraDonVersionKbn { get; set; }
        /// <summary>
        /// 印刷条件を取得・設定します。
        /// </summary>
        public String PrintJokenString { get; set; }
    }

    /// <summary>
    /// 受注配車一覧表の合計情報を作成します。
    /// </summary>
    [Serializable]
    public class JuchuHaishaIchiranHyoSumInfo
    {
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }
    }
}
