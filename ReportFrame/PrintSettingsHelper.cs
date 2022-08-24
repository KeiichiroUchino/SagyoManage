using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ReportFrame
{
    public static class PrintSettingsHelper
    {
        /// <summary>
        /// 設定ファイルが存在するかを返します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool SettingFileExists(string name)
        {
            string path = CreatePrintSettingFilePath(name);
            return System.IO.File.Exists(path);
        }

        /// <summary>
        /// 設定ファイルのパスを生成して返します。
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CreatePrintSettingFilePath(string name)
        {
            //印刷設定ファイルのファイル名までのパスを取得する
            string path =
                System.IO.Path.Combine(
                    Property.SystemProperty.GetInstance().ReportPrinterSettingPath,
                        name) + ".xml";

            return path;
        }
    }
}
