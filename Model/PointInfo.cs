using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 発着地（トラDON補）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 発着地IDを取得・設定します。
        /// </summary>
        public Decimal PointId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// トラDON発着地IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONPointId { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String PointShortName { get; set; }
        /// <summary>
        /// 略略称を取得・設定します。
        /// </summary>
        public String PointSShortName { get; set; }
        /// <summary>
        /// 発着地大分類IDを取得・設定します。
        /// </summary>
        public Decimal PointLBunruiId { get; set; }
        /// <summary>
        /// 発着地中分類IDを取得・設定します。
        /// </summary>
        public Decimal PointMBunruiId { get; set; }
        /// <summary>
        /// 方面IDを取得・設定します。
        /// </summary>
        public Decimal HomenId { get; set; }
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
        /// トラDON発着地コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONPointCode { get; set; }
        /// <summary>
        /// トラDON発着地名称を取得・設定します。
        /// </summary>
        public String ToraDONPointName { get; set; }
        /// <summary>
        /// トラDON発着地カナを取得・設定します。
        /// </summary>
        public String ToraDONPointNameKana { get; set; }
        /// <summary>
        /// トラDON非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }
        /// <summary>
        /// 方面コードを取得・設定します。
        /// </summary>
        public Int32 HomenCode { get; set; }
        /// <summary>
        /// 方面名称を取得・設定します。
        /// </summary>
        public String HomenName { get; set; }

        #endregion

        /// <summary>
        /// 使用停止を取得します。
        /// </summary>
        public string ShiyoTeishi
        {
            get { return this.ToraDONDisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 発着地検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointSearchParameter
    {
        /// <summary>
        /// 発着地IDを取得・設定します。
        /// </summary>
        public Decimal? PointId { get; set; }
        /// <summary>
        /// トラDON発着地IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONPointId { get; set; }
        /// <summary>
        /// トラDON発着地コードを取得・設定します。
        /// </summary>
        public Int32? ToraDONPointCode { get; set; }
    }
}
