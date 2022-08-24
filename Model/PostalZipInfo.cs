using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 郵便番号に対応する住所の情報を表すエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class PostalZipInfo
    {
        /// <summary>
        /// 郵便番号を取得・設定します。
        /// </summary>
        public string ZipCode { get; set; }
        /// <summary>
        /// 都道府県名を取得・設定します。
        /// </summary>
        public string PrefName { get; set; }
        /// <summary>
        /// 市区町村名を取得・設定します。
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 町域名を取得・設定します。
        /// </summary>
        public string TownName { get; set; }

        /// <summary>
        /// 住所を表す文字列をすべて結合した文字列を取得します。
        /// </summary>
        /// <returns>住所</returns>
        public override string ToString()
        {
            string rt_val = string.Empty;

            rt_val = this.PrefName + this.CityName + TownName;

            return rt_val;
        }
    }
}
