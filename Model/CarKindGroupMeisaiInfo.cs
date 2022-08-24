using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 車種グループ明細マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindGroupMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 車種グループIDを取得・設定します。
        /// </summary>
        public Decimal CarKindGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32 Gyo { get; set; }
        /// <summary>
        /// トラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONCarKindId { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON車種コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONCarKindCode { get; set; }
        /// <summary>
        /// トラDON車種名を取得・設定します。
        /// </summary>
        public String ToraDONCarKindName { get; set; }
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
    /// 車種グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindGroupMeisaiSearchParameter
    {
        /// <summary>
        /// 車種グループIDを取得・設定します。
        /// </summary>
        public Decimal? CarKindGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32? Gyo { get; set; }
        /// <summary>
        /// トラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONCarKindId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? ToraDONDisableFlag { get; set; }
    }
}
