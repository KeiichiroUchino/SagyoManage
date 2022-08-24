using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 作業大分類情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoDaiBunruiInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 作業大分類IDを取得・設定します。
        /// </summary>
        public Decimal SagyoDaiBunruiId { get; set; }
        /// <summary>
        /// 作業大分類コードを取得・設定します。
        /// </summary>
        public Int32 SagyoDaiBunruiCode { get; set; }
        /// <summary>
        /// 作業大分類名称を取得・設定します。
        /// </summary>
        public String SagyoDaiBunruiName { get; set; }
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
    /// 作業大分類検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoDaiBunruiSearchParameter
    {
        /// <summary>
        /// 作業大分類IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoDaiBunruiId { get; set; }
        /// <summary>
        /// 作業大分類コードを取得・設定します。
        /// </summary>
        public Int32? SagyoDaiBunruiCode { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
