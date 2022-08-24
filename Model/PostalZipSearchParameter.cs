using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class PostalZipSearchParameter
    {
        /// <summary>
        /// 郵便番号を取得・設定します。
        /// </summary>
        public String ZipCode { get; set; }
        /// <summary>
        /// 住所を取得・設定します。
        /// </summary>
        public String Adress { get; set; }
    }
}
