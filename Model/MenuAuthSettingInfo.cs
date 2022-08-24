using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 権限別メニュー設定情報を表す
    /// ビジネスエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class MenuAuthSettingInfo
    {
        /// <summary>
        /// 権限IDを取得・設定します。
        /// </summary>
        public decimal AuthId { get; set; }

        /// <summary>
        /// メニューIDを取得・設定します。
        /// </summary>
        public decimal MenuId { get; set; }

        /// <summary>
        /// 大分類コード
        /// </summary>
        public int TBunruiCode { get; set; }

        /// <summary>
        /// 大分類名
        /// </summary>
        public string TBunruiName { get; set; }

        /// <summary>
        /// 中分類コード
        /// </summary>
        public int MBunruiCode { get; set; }

        /// <summary>
        /// 中分類名
        /// </summary>
        public string MBunruiName { get; set; }

        /// <summary>
        /// 使用許可フラグ
        /// </summary>
        public bool AllowUseFlag { get; set; }
    }
}
