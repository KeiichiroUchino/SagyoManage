using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 権限名称テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class KengenNameInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 権限を取得・設定します。
        /// </summary>
        public decimal Kengen { get; set; }
        /// <summary>
        /// 権限名称を取得・設定します。
        /// </summary>
        public string KengenName { get; set; }
    }
}
