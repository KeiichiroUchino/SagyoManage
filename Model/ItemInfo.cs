using System;
using System.Globalization;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 品目（トラDON補）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ItemInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal ItemId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// トラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONItemId { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String ItemShortName { get; set; }
        /// <summary>
        /// 略略称を取得・設定します。
        /// </summary>
        public String ItemSShortName { get; set; }
        ///// <summary>
        ///// 品目グループIDを取得・設定します。
        ///// </summary>
        //public Decimal ItemGroupId { get; set; }
        /// <summary>
        /// 品目大分類IDを取得・設定します。
        /// </summary>
        public Decimal ItemLBunruiId { get; set; }
        /// <summary>
        /// 品目中分類IDを取得・設定します。
        /// </summary>
        public Decimal ItemMBunruiId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON品目コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONItemCode { get; set; }
        /// <summary>
        /// トラDON品目名称を取得・設定します。
        /// </summary>
        public String ToraDONItemName { get; set; }
        /// <summary>
        /// トラDON品目カナを取得・設定します。
        /// </summary>
        public String ToraDONItemNameKana { get; set; }
        /// <summary>
        /// トラDON非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }
        /// <summary>
        /// トラDON重量を取得・設定します。
        /// </summary>
        public Decimal ToraDONWeight { get; set; }
        /// <summary>
        /// トラDON単位IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONFigId { get; set; }
        /// <summary>
        /// トラDON税区分を取得・設定します。
        /// </summary>
        public Int32 ToraDONItemTaxKbn { get; set; }
        ///// <summary>
        ///// 品目グループコードを取得・設定します。
        ///// </summary>
        //public Int32 ItemGroupCode { get; set; }
        ///// <summary>
        ///// 品目グループ名称を取得・設定します。
        ///// </summary>
        //public String ItemGroupName { get; set; }

        #endregion

        /// <summary>
        /// 使用停止を取得します。
        /// </summary>
        public string ShiyoTeishi
        {
            get { return (this.DisableFlag || this.ToraDONDisableFlag) ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 品目検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ItemSearchParameter
    {
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal? ItemId { get; set; }
        /// <summary>
        /// トラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONItemId { get; set; }
        /// <summary>
        /// トラDON品目コードを取得・設定します。
        /// </summary>
        public Int32? ToraDONItemCode { get; set; }
        ///// <summary>
        ///// 品目グループIDを取得・設定します。
        ///// </summary>
        //public Decimal? ItemGroupId { get; set; }
    }
}
