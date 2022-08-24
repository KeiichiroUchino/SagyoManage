using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 操作履歴情報を表すビジネスエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class OperateHistoryInfo
    {
        /// <summary>
        /// IDを取得・設定します。
        /// </summary>
        public Decimal OperateHistoryId { get; set; }
        /// <summary>
        /// 処理日時を取得・設定します。
        /// </summary>
        public DateTime TransactTime { get; set; }
        /// <summary>
        /// オペレータ名を取得・設定します。
        /// </summary>
        public String OperatorName { get; set; }
        /// <summary>
        /// 端末IDを取得・設定します。
        /// </summary>
        public String TerminalId { get; set; }
        /// <summary>
        /// 処理IDを取得・設定します。
        /// </summary>
        public String ProcessId { get; set; }
        /// <summary>
        /// 区分を取得・設定します。
        /// </summary>
        public String OperateKbn { get; set; }
        /// <summary>
        /// 摘要を取得・設定します。
        /// </summary>
        public String Remark { get; set; }
    }
}
