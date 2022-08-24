using Jpsys.HaishaManageV10.BizProperty;
using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 請求データ作成（印刷条件）のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SeikyuDataSakuseiConditionInfo
    {
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDFrom { get; set; }
        /// <summary>
        /// 日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDTo { get; set; }
        /// <summary>
        /// 請求連携日付区分を取得・設定します。
        /// </summary>
        public DefaultProperty.SeikyuRenkeiHizukeKbn SeikyuRenkeiHizukeKbn { get; set; }
        /// <summary>
        /// 印刷条件を取得・設定します。
        /// </summary>
        public String PrintJokenString { get; set; }
    }
}
