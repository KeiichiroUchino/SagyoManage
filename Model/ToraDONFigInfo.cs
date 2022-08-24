using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON単位テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONFigInfo
    {
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal FigId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }
        /// <summary>
        /// 時間区分を取得・設定します。
        /// </summary>
        public Int32 TimeKbn { get; set; }
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
    /// 単位検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class FigSearchParameter
    {
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal? FigId { get; set; }
        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32? FigCd { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
