using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 販路情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// フリガナを取得・設定します。
        /// </summary>
        public String HanroNameKana { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String HanroSName { get; set; }
        /// <summary>
        /// 略略称を取得・設定します。
        /// </summary>
        public String HanroSSName { get; set; }
        /// <summary>
        /// トラDON得意先IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONTokuisakiId { get; set; }
        /// <summary>
        /// トラDON積地IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONHatchiId { get; set; }
        /// <summary>
        /// トラDON着地IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONChakuchiId { get; set; }
        /// <summary>
        /// トラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONItemId { get; set; }
        /// <summary>
        /// トラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONCarKindId { get; set; }
        /// <summary>
        /// トラDON車両IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONCarId { get; set; }
        /// <summary>
        /// トラDON傭車先IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONTorihikiId { get; set; }
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
        /// <summary>
        /// 再利用可能日数を取得・設定します。
        /// </summary>
        public Int32 ReuseNissu { get; set; }
        /// <summary>
        /// 再利用可能時間を取得・設定します。
        /// </summary>
        public Int32 ReuseJikan { get; set; }
        /// <summary>
        /// 請求単価を取得・設定します。
        /// </summary>
        public Decimal SeikyuTanka { get; set; }
        /// <summary>
        /// 傭車金額を取得・設定します。
        /// </summary>
        public Decimal YoshaKingaku { get; set; }
        /// <summary>
        /// 附帯業務料を取得・設定します。
        /// </summary>
        public Decimal Futaigyomuryo { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

        #region 関連項目

        /// <summary>
        /// JOINトラDON得意先IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONTokuisakiId { get; set; }
        /// <summary>
        /// トラDON得意先コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiCode { get; set; }
        /// <summary>
        /// トラDON得意先名称を取得・設定します。
        /// </summary>
        public String ToraDONTokuisakiName { get; set; }
        /// <summary>
        /// トラDON得意先略称を取得・設定します。
        /// </summary>
        public String ToraDONTokuisakiShortName { get; set; }
        /// <summary>
        /// トラDON得意先諸口区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiMemoAccount { get; set; }
        /// <summary>
        /// トラDON得意先部門管理区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiClmClassUseKbn { get; set; }
        /// <summary>
        /// トラDON得意先金額丸め区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiGakCutKbn { get; set; }
        /// <summary>
        /// トラDON得意先計上日区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiSaleSlipToClmDayKbn { get; set; }
        /// <summary>
        /// トラDON得意先非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONTokuisakiDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON積地IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONHatchiId { get; set; }
        /// <summary>
        /// トラDON積地コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONHatchiCode { get; set; }
        /// <summary>
        /// トラDON積地名称を取得・設定します。
        /// </summary>
        public String ToraDONHatchiName { get; set; }
        /// <summary>
        /// トラDON積地非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONHatchiDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON着地IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONChakuchiId { get; set; }
        /// <summary>
        /// トラDON着地コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONChakuchiCode { get; set; }
        /// <summary>
        /// トラDON着地名称を取得・設定します。
        /// </summary>
        public String ToraDONChakuchiName { get; set; }
        /// <summary>
        /// トラDON着地非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONChakuchiDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONItemId { get; set; }
        /// <summary>
        /// トラDON品目コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONItemCode { get; set; }
        /// <summary>
        /// トラDON品目名称を取得・設定します。
        /// </summary>
        public String ToraDONItemName { get; set; }
        /// <summary>
        /// トラDON品目税区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONItemItemTaxKbn { get; set; }
        /// <summary>
        /// トラDON品目非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONItemDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON単位IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONFigId { get; set; }
        /// <summary>
        /// トラDON単位コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONFigCode { get; set; }
        /// <summary>
        /// トラDON単位名称を取得・設定します。
        /// </summary>
        public String ToraDONFigName { get; set; }
        /// <summary>
        /// JOINトラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONCarKindId { get; set; }
        /// <summary>
        /// トラDON車種コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONCarKindCode { get; set; }
        /// <summary>
        /// トラDON車種名称を取得・設定します。
        /// </summary>
        public String ToraDONCarKindName { get; set; }
        /// <summary>
        /// トラDON車種非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONCarKindDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON車両IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONCarId { get; set; }
        /// <summary>
        /// トラDON車両コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONCarCode { get; set; }
        /// <summary>
        /// トラDON車両名称を取得・設定します。
        /// </summary>
        public String ToraDONCarName { get; set; }
        /// <summary>
        /// トラDON車両区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONCarKbn { get; set; }
        /// <summary>
        /// トラDON車両非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONCarDisableFlag { get; set; }
        /// <summary>
        /// JOINトラDON傭車先IDを取得・設定します。
        /// </summary>
        public Decimal JoinToraDONTorihikiId { get; set; }
        /// <summary>
        /// トラDON傭車先コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONTorihikiCode { get; set; }
        /// <summary>
        /// トラDON傭車先名称を取得・設定します。
        /// </summary>
        public String ToraDONTorihikiName { get; set; }
        /// <summary>
        /// トラDON傭車先略称を取得・設定します。
        /// </summary>
        public String ToraDONTorihikiShortName { get; set; }
        /// <summary>
        /// トラDON傭車先非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONTorihikiDisableFlag { get; set; }
        /// <summary>
        /// 往復区分名称を取得・設定します。
        /// </summary>
        public String OfukuKbnName { get; set; }

        #endregion
    }

    /// <summary>
    /// 販路検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroSearchParameter
    {
        /// <summary>
        /// 販路IDを取得・設定します。
        /// </summary>
        public Decimal? HanroId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32? HanroCode { get; set; }
        /// <summary>
        /// トラDON得意先IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONTokuisakiId { get; set; }
    }
}
