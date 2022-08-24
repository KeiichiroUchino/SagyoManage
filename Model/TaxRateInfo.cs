using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 消費税率テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class TaxRateInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 適用開始日を取得・設定します。
        /// </summary>
        public DateTime? StartDate { get; set; }
        /// <summary>
        /// 税率を取得・設定します。
        /// </summary>
        public decimal TaxRate { get; set; }
    }
}
