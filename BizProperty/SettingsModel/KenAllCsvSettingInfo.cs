using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.BizProperty.SettingsModel
{
    /// <summary>
    /// KEN_ALL_CSV取込ファイルパス情報
    /// </summary>
    [DataContract]
    public class KenAllCsvSettingInfo
    {
        /// <summary>
        /// KEN_ALL_CSV取込ファイルパスを取得・設定します。
        /// </summary>
        [DataMember(Order = 1)]
        public String KenAllCsvImportFilePath { get; set; }
    }
}
