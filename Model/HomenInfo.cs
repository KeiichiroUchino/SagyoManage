using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 方面情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 方面IDを取得・設定します。
        /// </summary>
        public Decimal HomenId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 HomenCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String HomenName { get; set; }
        /// <summary>
        /// フリガナを取得・設定します。
        /// </summary>
        public String HomenNameKana { get; set; }
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
    /// 方面検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenSearchParameter
    {
        /// <summary>
        /// 方面IDを取得・設定します。
        /// </summary>
        public Decimal? HomenId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32? HomenCode { get; set; }
    }
}
