using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON荷主テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONOwnerInfo
    {
        /// <summary>
        /// 荷主IDを取得・設定します。
        /// </summary>
        public Decimal OwnerId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 OwnerCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String OwnerName { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String OwnerShortName { get; set; }
        /// <summary>
        /// カナを取得・設定します。
        /// </summary>
        public String OwnerNameKana { get; set; }
        /// <summary>
        /// 郵便番号を取得・設定します。
        /// </summary>
        public String Postal { get; set; }
        /// <summary>
        /// 住所1を取得・設定します。
        /// </summary>
        public String Address1 { get; set; }
        /// <summary>
        /// 住所2を取得・設定します。
        /// </summary>
        public String Address2 { get; set; }
        /// <summary>
        /// TELを取得・設定します。
        /// </summary>
        public String Tel { get; set; }
        /// <summary>
        /// FAXを取得・設定します。
        /// </summary>
        public String Fax { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public bool DisableFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return (this.DisableFlag) ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 荷主検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ToraDONOwnerSearchParameter
    {
        /// <summary>
        /// 荷主IDを取得・設定します。
        /// </summary>
        public Decimal? OwnerId { get; set; }
        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        public Int32? OwnerCode { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
