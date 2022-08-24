using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 名称情報クラスのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class MeishoInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 名称IDを取得・設定します。
        /// </summary>
        public Decimal MeishoId { get; set; }
        /// <summary>
        /// 名称区分を取得・設定します。
        /// </summary>
        public Int32 MeishoKbn { get; set; }
        /// <summary>
        /// 名称コードを取得・設定します。
        /// </summary>
        public Int32 MeishoCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String Meisho { get; set; }
        /// <summary>
        /// 名称カナを取得・設定します。
        /// </summary>
        public String MeishoKana { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        /// <summary>
        /// 使用停止を取得します。
        /// </summary>
        public string ShiyoTeishi
        {
            get { return (this.DisableFlag) ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 名称抽出情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class MeishoSearchParameter
    {
        /// <summary>
        /// 名称区分を取得・設定します。
        /// </summary>
        public Int32? MeishoKbn { get; set; }
        /// <summary>
        /// 名称コードを取得・設定します。
        /// </summary>
        public Int32? MeishoCode { get; set; }
        /// <summary>
        /// 名称IDを取得・設定します。
        /// </summary>
        public Decimal? MeishoId { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public string Meisho { get; set; }

        /// <summary>
        /// 名称IDリストを取得・設定します。
        /// </summary>
        public List<decimal> MeishoIdList { get; set; }
    }
}
