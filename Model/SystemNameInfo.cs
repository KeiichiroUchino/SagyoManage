using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// システム名称マスタのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class SystemNameInfo
    {
        /// <summary>
        /// システム名称区分を取得・設定します。
        /// </summary>
        public int SystemNameKbn { get; set; }

        /// <summary>
        /// システム名称コードを取得・設定します。
        /// </summary>
        public int SystemNameCode { get; set; }

        /// <summary>
        /// システム名称を取得・設定します。
        /// </summary>
        public string SystemName { get; set; }

        /// <summary>
        /// 文字列01を取得・設定します。
        /// </summary>
        public string StringValue01 { get; set; }

        /// <summary>
        /// 整数01を取得・設定します。
        /// </summary>
        public int IntegerValue01 { get; set; }

        /// <summary>
        /// 小数01を取得・設定します。
        /// </summary>
        public decimal DecimalValue01 { get; set; }
    }

    /// <summary>
    /// システム名称検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SystemNameSearchParameter
    {
        /// <summary>
        /// システム名称区分を取得・設定します。
        /// </summary>
        public Int32? SystemNameKbn { get; set; }
        /// <summary>
        /// システム名称コードを取得・設定します。
        /// </summary>
        public Int32? SystemNameCode { get; set; }
    }
}
