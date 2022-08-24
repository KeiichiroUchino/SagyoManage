using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 請求データのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SeikyuDataInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 請求データIDを取得・設定します。
        /// </summary>
        public Decimal SeikyuDataId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// 営業所IDリストを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
    }

    /// <summary>
    /// 請求データ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SeikyuDataSearchParameter
    {
        /// <summary>
        /// 営業所IDリストを取得・設定します。
        /// </summary>
        public Decimal? BranchOfficeId { get; set; }
    }
}
