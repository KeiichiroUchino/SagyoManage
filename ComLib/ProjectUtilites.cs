using Jpsys.SagyoManage.ComLib;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 開発プロジェクトにて共通に使用する、
    /// 各種静的メソッドを定義します。開発プロジェクトごとのシステム共通関数です。
    /// </summary>
    public class ProjectUtilites
    {
        /// <summary>
        /// int型の時分からString型の時分を取得します。
        /// （4桁の入力値に対応、例：1200→「12:00」、0→「00:00」）
        /// </summary>
        /// <param name="hms"></param>
        /// <returns>String型の時分</returns>
        public static String IntToHMSDigit4(int hms)
        {
            return String.Format("{0:D4}", hms).Substring(0, 2) + ":" + String.Format("{0:D4}", hms).Substring(2, 2);
        }

        /// <summary>
        /// int型の時分秒からString型の時分を取得します。
        /// （6桁の入力値に対応、例：120000→「12:00」、0→「00:00」）
        /// </summary>
        /// <param name="hms"></param>
        /// <returns>String型の時分</returns>
        public static String IntToHMSDigit6(int hms)
        {
            return String.Format("{0:D6}", hms).Substring(0, 2) + ":" + String.Format("{0:D6}", hms).Substring(2, 2);
        }

        /// <summary>
        /// String型の時分からInt型の時分秒を取得します。
        /// （6桁の出力値に変換、例：「12:00」→120000、「00:00」→0）
        /// </summary>
        /// <param name="hms"></param>
        /// <returns>Int型の時分秒/returns>
        public static int HMSToIntDigit6(String hms)
        {
            Regex re = new Regex(@"[^0-9]");
            return Convert.ToInt32(re.Replace(hms, "").PadRight(6, '0'));
        }

        /// <summary>
        /// String型の時分が適正か判定します。
        /// （時分変換可能な場合：true、時分変換不可の場合：false）
        /// </summary>
        /// <param name="hms"></param>
        /// <returns>判定結果/returns>
        public static bool IsHMS(String hms)
        {
            try
            {
                Regex re = new Regex(@"[^0-9]");
                Convert.ToInt32(re.Replace(hms, ""));
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 整数部桁数、小数部桁数、ゼロ表示フラグを指定して
        /// フォーマット文字列を取得します。
        /// </summary>
        public static String GetDigitsFormat(int intDigits, int decimalDigits, bool displayZero)
        {
            string rt_str = string.Empty;

            string zeroStr = displayZero ? "0" : "#";

            switch (intDigits)
            {
                case 6:
                    rt_str = "###,##" + zeroStr;
                    break;
                case 7:
                    rt_str = "#,###,##" + zeroStr;
                    break;
                case 8:
                    rt_str = "##,###,##" + zeroStr;
                    break;
                case 9:
                    rt_str = "###,###,##" + zeroStr;
                    break;
                case 10:
                    rt_str = "#,###,###,##" + zeroStr;
                    break;
                case 11:
                    rt_str = "##,###,###,##" + zeroStr;
                    break;
                case 12:
                    rt_str = "###,###,###,##" + zeroStr;
                    break;
                default:
                    break;
            }

            switch (decimalDigits)
            {
                case 0:
                    break;
                case 1:
                    rt_str = rt_str + ".#";
                    break;
                case 2:
                    rt_str = rt_str + ".##";
                    break;
                case 3:
                    rt_str = rt_str + ".###";
                    break;
                case 4:
                    rt_str = rt_str + ".####";
                    break;
                case 5:
                    rt_str = rt_str + ".#####";
                    break;
                default:
                    break;
            }


            return rt_str;
        }

        /// <summary>
        /// SAP Crystal Reportsのバージョン情報を取得します。
        /// </summary>
        /// <returns>SAP Crystal Reportsのバージョン情報</returns>
        public static List<string> GetSAPVersion()
        {
            List<string> ret = new List<string>();

            string uninstall_path = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Uninstall";
            Microsoft.Win32.RegistryKey uninstall = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstall_path, false);
            if (uninstall != null)
            {
                foreach (string subKey in uninstall.GetSubKeyNames())
                {
                    string appName = null;
                    Microsoft.Win32.RegistryKey appkey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(uninstall_path + "\\" + subKey, false);

                    if (appkey.GetValue("DisplayName") != null)
                        appName = appkey.GetValue("DisplayName").ToString();
                    else
                        appName = subKey;

                    if (appName.StartsWith("SAP Crystal Reports runtime engine for .NET Framework"))
                    {
                        ret.Add(appName);
                        ret.Add(appkey.GetValue("DisplayVersion").ToString());

                        return ret;
                    }
                }
            }

            return ret;
        }
    }
}
