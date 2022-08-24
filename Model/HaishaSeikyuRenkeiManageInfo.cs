using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配車請求連携管理のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaSeikyuRenkeiManageInfo : AbstractModelBase
    {
        /// <summary>
        /// 配車請求連携IDを取得・設定します。
        /// </summary>
        public Decimal HaishaSeikyuRenkeiId { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal HaishaId { get; set; }
    }
}
