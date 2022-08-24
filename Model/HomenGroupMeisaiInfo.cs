using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 方面グループ明細マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenGroupMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 方面グループIDを取得・設定します。
        /// </summary>
        public Decimal HomenGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32 Gyo { get; set; }
        /// <summary>
        /// トラDON方面IDを取得・設定します。
        /// </summary>
        public Decimal HomenId { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON方面コードを取得・設定します。
        /// </summary>
        public Int32 HomenCode { get; set; }
        /// <summary>
        /// トラDON方面名を取得・設定します。
        /// </summary>
        public String HomenName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }

        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean SelectFlag { get; set; }

        #endregion
    }

    /// <summary>
    /// 方面グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenGroupMeisaiSearchParameter
    {
        /// <summary>
        /// 方面グループIDを取得・設定します。
        /// </summary>
        public Decimal? HomenGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32? Gyo { get; set; }
        /// <summary>
        /// トラDON方面IDを取得・設定します。
        /// </summary>
        public Decimal? HomenId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
