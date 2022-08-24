using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON請求部門テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONClmClassInfo
    {
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 ClmClassCode { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String ClmClassName { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String ClmClassShortName { get; set; }
        /// <summary>
        /// カナを取得・設定します。
        /// </summary>
        public String ClmClassNameKana { get; set; }
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
        /// 請求先印字区分を取得・設定します。
        /// </summary>
        public Int32 ClmPRTKbn { get; set; }
        /// <summary>
        /// 請求先住所印字区分を取得・設定します。
        /// </summary>
        public Int32 ClmAddressPRTKbn { get; set; }
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public bool DisableFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return (this.DisableFlag) ? "停止中" : string.Empty; }
        }

        #region 関連項目

        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }

        #endregion
    }

    /// <summary>
    /// 請求部門検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ToraDONClmClassSearchParameter
    {
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal? ClmClassId { get; set; }
        /// <summary>
        /// 請求部門コードを取得・設定します。
        /// </summary>
        public Int32? ClmClassCode { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
