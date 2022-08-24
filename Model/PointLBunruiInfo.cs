using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 発着地大分類情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointLBunruiInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 発着地大分類IDを取得・設定します。
        /// </summary>
        public Decimal PointLBunruiId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public String PointLBunruiCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String PointLBunruiName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        /// <summary>
        /// 品目中分類リストを取得・設定します。
        /// </summary>
        public IList<PointMBunruiInfo> PointMBunruiList { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 発着地大分類検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointLBunruiSearchParameter
    {
        /// <summary>
        /// 発着地大分類IDを取得・設定します。
        /// </summary>
        public Decimal? PointLBunruiId { get; set; }
        /// <summary>
        /// 発着地大分類コードを取得・設定します。
        /// </summary>
        public String PointLBunruiCode { get; set; }
    }
}
