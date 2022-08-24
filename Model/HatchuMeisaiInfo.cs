using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 発注情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HatchuInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 発注IDを取得・設定します。
        /// </summary>
        public Decimal HatchuId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// 積込日を取得・設定します。
        /// </summary>
        public DateTime TsumikomiYMD { get; set; }
        /// <summary>
        /// 引取工場IDを取得・設定します。
        /// </summary>
        public Decimal HikitoriKojoId { get; set; }
        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }
        /// <summary>
        /// 餌IDを取得・設定します。
        /// </summary>
        public Decimal EsaId { get; set; }
        /// <summary>
        /// 重さを取得・設定します。
        /// </summary>
        public Decimal Weight { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal SharyoId { get; set; }
        /// <summary>
        /// 積込区分を取得・設定します。
        /// </summary>
        public Int32 TsumikomiKbn { get; set; }
        /// <summary>
        /// 請求先IDを取得・設定します。
        /// </summary>
        public Decimal SeikyusakiId { get; set; }
        /// <summary>
        /// 履歴番号を取得・設定します。
        /// </summary>
        public Int32 RirekiNo { get; set; }
        /// <summary>
        /// FAXを取得・設定します。
        /// </summary>
        public String Fax { get; set; }
        /// <summary>
        /// 工場名01を取得・設定します。
        /// </summary>
        public String HikitoriKojoNM01 { get; set; }
        /// <summary>
        /// 工場名02を取得・設定します。
        /// </summary>
        public String HikitoriKojoNM02 { get; set; }
        /// <summary>
        /// 農場名を取得・設定します。
        /// </summary>
        public String NojoName { get; set; }
        /// <summary>
        /// 餌名を取得・設定します。
        /// </summary>
        public String EsaName { get; set; }
        /// <summary>
        /// 得意先品目コードを取得・設定します。
        /// </summary>
        public String TokuisakiItemCode { get; set; }
        /// <summary>
        /// 特約店IDを取得・設定します。
        /// </summary>
        public Decimal TokuyakutenId { get; set; }
        /// <summary>
        /// 特約店名を取得・設定します。
        /// </summary>
        public String TokuyakutenName { get; set; }
        /// <summary>
        /// 特約店略名を取得・設定します。
        /// </summary>
        public String TokuyakutenShortName { get; set; }
        /// <summary>
        /// 印刷区分を取得・設定します。
        /// </summary>
        public Int32 InsatsuKbn { get; set; }
        /// <summary>
        /// 変更重さフラグを取得・設定します。
        /// </summary>
        public Boolean HenkoWeightFlag { get; set; }
        /// <summary>
        /// 変更車両区分フラグを取得・設定します。
        /// </summary>
        public Boolean HenkoSharyoFlag { get; set; }
        /// <summary>
        /// 変更積込区分フラグを取得・設定します。
        /// </summary>
        public Boolean HenkoTsumikomiKbnFlag { get; set; }
        /// <summary>
        /// 積込区分02を取得・設定します。
        /// </summary>
        public Int32 TsumikomiKbn02 { get; set; }
    }

    /// <summary>
    /// 発注検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HatchuSearchParameter
    {
        /// <summary>
        /// 発注IDを取得・設定します。
        /// </summary>
        public Decimal? HatchuId { get; set; }
        /// <summary>
        /// 積込日を取得・設定します。
        /// </summary>
        public DateTime? TsumikomiYMD { get; set; }
        /// <summary>
        /// 積込日（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime? TsumikomiYMDFrom { get; set; }
        /// <summary>
        /// 積込日（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime? TsumikomiYMDTo { get; set; }
        /// <summary>
        /// 引取工場IDを取得・設定します。
        /// </summary>
        public Decimal? HikitoriKojoId { get; set; }
        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal? NojoId { get; set; }
        /// <summary>
        /// 餌IDを取得・設定します。
        /// </summary>
        public Decimal? EsaId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal? SharyoId { get; set; }
        /// <summary>
        /// 積込区分を取得・設定します。
        /// </summary>
        public Int32? TsumikomiKbn { get; set; }
        /// <summary>
        /// 請求先IDを取得・設定します。
        /// </summary>
        public Decimal? SeikyusakiId { get; set; }
    }
}
