using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 作業中分類情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoChuBunruiInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 作業中分類IDを取得・設定します。
        /// </summary>
        public Decimal SagyoChuBunruiId { get; set; }
        /// <summary>
        /// 作業大分類IDを取得・設定します。
        /// </summary>
        public Decimal SagyoDaiBunruiId { get; set; }
        /// <summary>
        /// 作業中分類コードを取得・設定します。
        /// </summary>
        public Int32 SagyoChuBunruiCode { get; set; }
        /// <summary>
        /// 作業中分類名称を取得・設定します。
        /// </summary>
        public String SagyoChuBunruiName { get; set; }
        /// <summary>
        /// ShelterIDを取得・設定します。
        /// </summary>
        public Decimal ShelterId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 作業中分類検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoChuBunruiSearchParameter
    {
        /// <summary>
        /// 作業中分類IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoChuBunruiId { get; set; }
        /// <summary>
        /// 作業大分類IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoDaiBunruiId { get; set; }
        /// <summary>
        /// 作業中分類コードを取得・設定します。
        /// </summary>
        public Int32? SagyoChuBunruiCode { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
