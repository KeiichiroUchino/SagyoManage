using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 得意先グループ明細マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiGroupMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 得意先グループIDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32 Gyo { get; set; }
        /// <summary>
        /// トラDON得意先IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONTokuisakiId { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON得意先コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONTokuisakiCode { get; set; }
        /// <summary>
        /// トラDON得意先名を取得・設定します。
        /// </summary>
        public String ToraDONTokuisakiName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }

        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean SelectFlag { get; set; }

        #endregion
    }

    /// <summary>
    /// 得意先グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiGroupMeisaiSearchParameter
    {
        /// <summary>
        /// 得意先グループIDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32? Gyo { get; set; }
        /// <summary>
        /// トラDON得意先IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONTokuisakiId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? ToraDONDisableFlag { get; set; }
    }
}
