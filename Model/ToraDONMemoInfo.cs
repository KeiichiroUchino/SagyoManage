using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON備考テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONMemoInfo
    {
        /// <summary>
        /// 備考IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONMemoId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONMemoCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String ToraDONMemoName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public bool DisableFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 備考検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class MemoSearchParameter
    {
        /// <summary>
        /// 備考IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONMemoId { get; set; }
        /// <summary>
        /// 備考コードを取得・設定します。
        /// </summary>
        public Int32? ToraDONMemoCd { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
