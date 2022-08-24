using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.BizProperty.SettingsModel
{
    /// <summary>
    /// 配車情報CSV出力ファイルパス情報
    /// </summary>
    [DataContract]
    public class HaishaCsvSettingInfo
    {
        /// <summary>
        /// 配車情報CSV出力ファイルパスを取得・設定します。
        /// </summary>
        [DataMember(Order = 1)]
        public String HaishaCsvExportFilePath { get; set; }
    }
}
