using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.BizProperty.SettingsModel;
using System.IO;

namespace Jpsys.SagyoManage.BizProperty
{
    /// <summary>
    /// 
    /// </summary>
    public class BizCommon
    {
        #region 設定情報の基礎

        /// <summary>
        /// //アプリケーション設定フォルダのパス
        /// </summary>
        private static readonly string ApplicationSettingsPath =
                Path.Combine(
                    System.Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    @"jpsys.co.jp\" + NSKUtilSetting.SeihinId + "_"
                    + NSKUtilSetting.EditionName);

        /// <summary>
        /// 設定情報をファイルに保存します。
        /// </summary>
        /// <param name="info"></param>
        /// <param name="fileName"></param>
        private static void SaveSettings(object info, string fileName)
        {

            string forderpath = ApplicationSettingsPath + @"\Settings";

            if (!Directory.Exists(forderpath))
            {
                Directory.CreateDirectory(forderpath);
            }

            string text = ComLib.DataContractSerializeHelper.SerializeToString(info);

            string path = Path.Combine(forderpath, fileName);

            File.WriteAllText(path, text);
        }

        /// <summary>
        /// 設定情報を取得します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private static T GetSettings<T>(string fileName)
        {
            string forderpath = ApplicationSettingsPath + @"\Settings";
            string path = Path.Combine(forderpath, fileName);

            if (File.Exists(path))
            {
                string text = File.ReadAllText(path);
                return ComLib.DataContractSerializeHelper.DeserializeFromString<T>(text);
            }
            else
            {
                return default(T);
            }
        }

        #endregion

        #region KEN_ALL_CSV情報

        public static void SaveKenAllCsvSettingInfo(KenAllCsvSettingInfo info)
        {
            SaveSettings(info, "KenAllCsvSettings.xml");
        }

        public static KenAllCsvSettingInfo GetKenAllCsvSettingInfo()
        {
            return GetSettings<KenAllCsvSettingInfo>("KenAllCsvSettings.xml");
        }

        #endregion

        #region 配車情報CSV情報

        public static void SaveHaishaCsvSettingInfo(HaishaCsvSettingInfo info)
        {
            SaveSettings(info, "HaishaCsvSettings.xml");
        }

        public static HaishaCsvSettingInfo GetHaishaCsvSettingInfo()
        {
            return GetSettings<HaishaCsvSettingInfo>("HaishaCsvSettings.xml");
        }

        #endregion
    }
}
