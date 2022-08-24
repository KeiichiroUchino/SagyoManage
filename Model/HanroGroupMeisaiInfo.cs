using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 販路グループ明細マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroGroupMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 販路グループIDを取得・設定します。
        /// </summary>
        public Decimal HanroGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32 Gyo { get; set; }
        /// <summary>
        /// トラDON販路IDを取得・設定します。
        /// </summary>
        public Decimal HanroId { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// トラDON販路名を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
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
    /// 販路グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroGroupMeisaiSearchParameter
    {
        /// <summary>
        /// 販路グループIDを取得・設定します。
        /// </summary>
        public Decimal? HanroGroupId { get; set; }
        /// <summary>
        /// 明細行を取得・設定します。
        /// </summary>
        public Int32? Gyo { get; set; }
        /// <summary>
        /// トラDON販路IDを取得・設定します。
        /// </summary>
        public Decimal? HanroId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
