using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配車排他管理情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaExclusiveManageInfo : AbstractModelBase
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 作業中IDを取得・設定します。
        /// </summary>
        public Decimal WorkId { get; set; }
    }

    /// <summary>
    /// 配車排他管理検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaExclusiveManageSearchParameter
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal? OperatorId { get; set; }
    }
}
