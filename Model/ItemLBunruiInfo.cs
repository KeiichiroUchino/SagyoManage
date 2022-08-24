using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 品目大分類情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ItemLBunruiInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 品目大分類IDを取得・設定します。
        /// </summary>
        public Decimal ItemLBunruiId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public String ItemLBunruiCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String ItemLBunruiName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        /// <summary>
        /// 中分類リストを取得・設定します。
        /// </summary>
        public IList<ItemMBunruiInfo> ItemMBunruiList { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 品目大分類検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ItemLBunruiSearchParameter
    {
        /// <summary>
        /// 品目大分類IDを取得・設定します。
        /// </summary>
        public Decimal? ItemLBunruiId { get; set; }
        /// <summary>
        /// 品目大分類コードを取得・設定します。
        /// </summary>
        public String ItemLBunruiCode { get; set; }
    }
}
