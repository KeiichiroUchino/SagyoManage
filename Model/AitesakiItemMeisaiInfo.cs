using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 相手先品目明細マスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class AitesakiItemMeisaiInfo : AbstractModelBase
    {
        /// <summary>
        /// 引取工場IDを取得・設定します。
        /// </summary>
        public Decimal HikitoriKojoId { get; set; }
        /// <summary>
        /// トラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONItemId { get; set; }
        /// <summary>
        /// 相手先品目コードを取得・設定します。
        /// </summary>
        public String AitesakiItemCode { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON品目コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONItemCode { get; set; }
        /// <summary>
        /// トラDON品目名を取得・設定します。
        /// </summary>
        public string ToraDONItemName { get; set; }

        #endregion
    }

    /// <summary>
    /// 相手先品目明細検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class AitesakiItemMeisaiSearchParameter
    {
        /// <summary>
        /// 引取工場IDを取得・設定します。
        /// </summary>
        public Decimal? HikitoriKojoId { get; set; }
        /// <summary>
        /// トラDON品目IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONItemId { get; set; }
    }
}
