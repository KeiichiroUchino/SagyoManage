using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 受注一覧画面の出力情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuIchiranInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 受注伝票番号を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNo { get; set; }
        /// <summary>
        /// 営業所略称を取得・設定します。
        /// </summary>
        public String BranchOfficeSNM { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCd { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種略称を取得・設定します。
        /// </summary>
        public String CarKindSNM { get; set; }
        /// <summary>
        /// 乗務員名称を取得・設定します。
        /// </summary>
        public String DriverNm { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiNM { get; set; }
        /// <summary>
        /// 請求部門名を取得・設定します。
        /// </summary>
        public String ClmClassNM { get; set; }
        /// <summary>
        /// 請負名を取得・設定します。
        /// </summary>
        public String ContractNM { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public String StartPointNM { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointNM { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemNM { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerNM { get; set; }
        /// <summary>
        /// 受注担当名称を取得・設定します。
        /// </summary>
        public String JuchuTantoNm { get; set; }
        /// <summary>
        /// 販路名を取得・設定します。
        /// </summary>
        public String HanroNm { get; set; }
        /// <summary>
        /// 往復区分名を取得・設定します。
        /// </summary>
        public String OfukuKbnNm { get; set; }
        /// <summary>
        /// 庸車先名を取得・設定します。
        /// </summary>
        public String CarOfChartererShortNm { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigNm { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal AtPrice { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal Weight { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 税区分名称を取得・設定します。
        /// </summary>
        public String TaxDispKbnNm { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpYMD { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車税区分名称を取得・設定します。
        /// </summary>
        public String CharterTaxDispKbnNm { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpYMD { get; set; }
        /// <summary>
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Memo { get; set; }
        /// <summary>
        /// 運賃_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 乗務員売上同額フラグを取得・設定します。
        /// </summary>
        public String JomuinUriageDogakuFlag { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }
        /// <summary>
        /// 受領済フラグを取得・設定します。
        /// </summary>
        public Boolean ReceivedFlag { get; set; }
        /// <summary>
        /// 配車存在フラグを取得・設定します。
        /// </summary>
        public String ExistsHaishaFlag { get; set; }
        /// <summary>
        /// 連携存在フラグを取得・設定します。
        /// </summary>
        public String ExistsRenkeiFlag { get; set; }
    }

    /// <summary>
    /// 受注一覧画面の検索条件のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuIchiranSearchParameter
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
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }

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
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }

        /// <summary>
        /// 受注伝票番号（From）を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNoFrom { get; set; }

        /// <summary>
        /// 受注伝票番号（To）を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNoTo { get; set; }

        /// <summary>
        ///未配車のみフラグを取得・設定します。
        /// </summary>
        public Boolean MiHaishaNomiFlag { get; set; }

        /// <summary>
        ///未連携のみフラグを取得・設定します。
        /// </summary>
        public Boolean MiRenkeiNomiFlag { get; set; }

    }

}
