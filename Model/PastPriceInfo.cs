using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 過去受注履歴検索画面の出力情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PastPriceInfo
    {
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 販路名称を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// 販路非表示フラグを取得・設定します。
        /// </summary>
        public Boolean HanroDisableFlag { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 得意先略称を取得・設定します。
        /// </summary>
        public String TokuisakiShortName { get; set; }
        /// <summary>
        /// 得意先非表示フラグを取得・設定します。
        /// </summary>
        public Boolean TokuisakiDisableFlag { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32 CarKindCode { get; set; }
        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 車種略称を取得・設定します。
        /// </summary>
        public String CarKindShortName { get; set; }
        /// <summary>
        /// 車種非表示フラグを取得・設定します。
        /// </summary>
        public Boolean CarKindDisableFlag { get; set; }
        /// <summary>
        /// 積地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCode { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
        /// </summary>
        public String StartPointName { get; set; }
        /// <summary>
        /// 積地非表示フラグを取得・設定します。
        /// </summary>
        public Boolean StartPointDisableFlag { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public Decimal EndPointId { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCode { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointName { get; set; }
        /// <summary>
        /// 着地非表示フラグを取得・設定します。
        /// </summary>
        public Boolean EndPointDisableFlag { get; set; }
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal ItemId { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCode { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        /// 品目非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ItemDisableFlag { get; set; }
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }
        /// <summary>
        /// 単位非表示フラグを取得・設定します。
        /// </summary>
        public Boolean FigDisableFlag { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal TorihikiId { get; set; }
        /// <summary>
        /// 傭車先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCode { get; set; }
        /// <summary>
        /// 傭車先名を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }
        /// <summary>
        /// 傭車先略名を取得・設定します。
        /// </summary>
        public String TorihikiShortName { get; set; }
        /// <summary>
        /// 傭車先非表示フラグを取得・設定します。
        /// </summary>
        public Boolean TorihikiDisableFlag { get; set; }
        /// <summary>
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal AtPrice { get; set; }
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
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }

    }

    /// <summary>
    /// 過去受注履歴検索画面初期パラメータ情報を作成します。
    /// </summary>
    [Serializable]
    public class PastPriceDefaultParameters
    {
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32? HanroCode { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32? TokuisakiCode { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32? StartPointCode { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32? EndPointCode { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32? ItemCode { get; set; }
        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32? FigCode { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32? CarKindCode { get; set; }
        /// <summary>
        /// 傭車先コードを取得・設定します。
        /// </summary>
        public Int32? TorihikiCode { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32 CarKbn { get; set; }
    }

    /// <summary>
    /// 過去受注履歴検索画面初期パラメータ検索情報を作成します。
    /// </summary>
    [Serializable]
    public class PastPriceDefaultSearchParametersInfo
    {
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
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
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public int? CarKindCode { get; set; }
        /// <summary>
        /// 庸車先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCode { get; set; }

        #region 関連項目

        /// <summary>
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId { get; set; }
        /// <summary>
        /// 販路名を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 積地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
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
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal TorihikiId { get; set; }
        /// <summary>
        /// 庸車先名を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }

        #endregion
    }

    /// <summary>
    /// 過去受注履歴検索画面検索条件パラメータを作成します。
    /// </summary>
    [Serializable]
    public class PastPriceSearchParametersInfo
    {
        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 積地コードを取得・設定します。
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
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public int? CarKindCode { get; set; }
        /// <summary>
        /// 庸車先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCode { get; set; }
        /// <summary>
        /// 対象日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime TaishoYMDFrom { get; set; }
        /// <summary>
        /// 対象日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime TaishoYMDTo { get; set; }
        /// <summary>
        /// 配車Ace選択区分を取得・設定します。
        /// </summary>
        public Boolean HaishaAceChecked { get; set; }

        #region 関連項目

        /// <summary>
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId { get; set; }
        /// <summary>
        /// 販路名を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 積地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
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
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal TorihikiId { get; set; }
        /// <summary>
        /// 庸車先名を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }

        /// <summary>
        /// トラDONバーション区分を取得・設定します。
        /// </summary>
        public int TraDonVersionKbn { get; set; }

        #endregion
    }

}
