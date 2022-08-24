using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// トラDON営業所テーブルのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class ToraDONBranchOfficeInfo
    {
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String BranchOfficeName { get; set; }
        /// <summary>
        /// 略称を取得・設定します。
        /// </summary>
        public String BranchOfficeShortName { get; set; }
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
        /// URLを取得・設定します。
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// メールアドレスを取得・設定します。
        /// </summary>
        public String MailAddress { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Memo { get; set; }
        /// <summary>
        /// 口座1を取得・設定します。
        /// </summary>
        public String Account1 { get; set; }
        /// <summary>
        /// 口座2を取得・設定します。
        /// </summary>
        public String Account2 { get; set; }
        /// <summary>
        /// 口座3を取得・設定します。
        /// </summary>
        public String Account3 { get; set; }
        /// <summary>
        /// 口座予備名1を取得・設定します。
        /// </summary>
        public String AccountSub1 { get; set; }
        /// <summary>
        /// 口座予備名2を取得・設定します。
        /// </summary>
        public String AccountSub2 { get; set; }
        /// <summary>
        /// 口座予備名3を取得・設定します。
        /// </summary>
        public String AccountSub3 { get; set; }
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
    /// 営業所検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ToraDONBranchOfficeSearchParameter
    {
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal? BranchOfficeId { get; set; }
        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32? BranchOfficeCode { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
    }
}
