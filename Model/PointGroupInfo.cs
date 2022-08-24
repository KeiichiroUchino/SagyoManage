using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 発着地グループ情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointGroupInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 発着地グループIDを取得・設定します。
        /// </summary>
        public Decimal PointGroupId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 PointGroupCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String PointGroupName { get; set; }
        /// <summary>
        /// フリガナを取得・設定します。
        /// </summary>
        public String PointGroupNameKana { get; set; }
        /// <summary>
        /// 発着地グループ分類区分を取得・設定します。
        /// </summary>
        public Int32 PointGroupBunruiKbn { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 発着地グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointGroupSearchParameter
    {
        /// <summary>
        /// 発着地グループIDを取得・設定します。
        /// </summary>
        public Decimal? PointGroupId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32? PointGroupCode { get; set; }
    }
}
