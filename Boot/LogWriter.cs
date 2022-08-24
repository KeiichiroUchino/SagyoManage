using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using jp.co.jpsys.util;
using jp.co.jpsys.util.log;

namespace Jpsys.SagyoManage.Boot
{
    public class LogWriter
    {
        /// <summary>
        /// デバッグログを出力するロガーのインスタンスを保持
        /// </summary>
        private static INSKLogWriter debugLogger = null;

        ///// <summary>
        ///// マシンのイベントログへ出力するロガーのインスタンスを保持
        ///// </summary>
        //private static INSKLogWriter eventLogger = null;

        /// <summary>
        /// ログに出力するタイトルと、内容を指定してアプリケーション動作ログを出力します。
        /// ログは非ローミングユーザのApplication Dataフォルダに出力されます。
        /// </summary>
        /// <param name="priority">ログレベル（ログの重要度）</param>
        /// <param name="title">ログのタイトル</param>
        /// <param name="description">ログの内容</param>
        private static void ApplicationLogWriter(NSKLogPriority priority, string title, string description)
        {
            NSKLogManager.LogFilePrefix = priority.ToString();
            //ログファイルの出力先をSystemPropertyから取得
            //NSKLogManager.LogFileName = @"errlog";/
            //ロガーのインスタンスがnullだった時はインスタンスを作成する。
            if (LogWriter.debugLogger == null)
            {

                ////保存先のフォルダ初期値として、アセンブリのフォルダにしておく
                //System.Reflection.Assembly myasm =
                //    System.Reflection.Assembly.GetEntryAssembly();
                //string log_folder =
                //    System.IO.Path.GetDirectoryName(myasm.Location);

                //ユーザーのアプリケーションフォルダをデフォルトのフォルダとする
                string log_folder =
                    System.IO.Path.Combine(
                        System.Environment.GetFolderPath(
                            Environment.SpecialFolder.LocalApplicationData),
                        @"jpsys.co.jp\" + NSKUtilSetting.SeihinId + "_"
                        + NSKUtilSetting.EditionName + "_MenuLog");

                //アプリケーション構成ファイルでログ出力先が
                //指定されていた場合はそこに出力する。
                //フォルダがない場合は作成する
                if (!(ConfigurationManager.AppSettings["ApplicationLogFolder"] == null ||
                    ConfigurationManager.AppSettings["ApplicationLogFolder"].Trim().Length == 0))
                {
                    string wk_folder =
                        ConfigurationManager.AppSettings["ApplicationLogFolder"].Trim();


                    if (!System.IO.Directory.Exists(wk_folder))
                    {
                        try
                        {
                            //無ければフォルダを作る
                            System.IO.Directory.CreateDirectory(wk_folder);
                            //フォルダを指定
                            log_folder = wk_folder;
                        }
                        catch (System.IO.IOException)
                        {
                            //IOExceptionのときは何もしない
                            //　つまり初期値のまま
                        }
                    }
                    else
                    {
                        //フォルダを指定
                        log_folder = wk_folder;
                    }

                }

                //ログマネージャに出力先フォルダを設定。
                NSKLogManager.LogFileDirectoryName = log_folder;
                //ロガーのインスタンスを取得。
                LogWriter.debugLogger = NSKLogManager.GetLogger(NSKLogWriterType.TYPE_FILE);
            }

            //ログを出力
            LogWriter.debugLogger.WriteLog(
                NSKLogFacility.FACILITY_USER, priority,
                title, description);
        }

        /// <summary>
        /// デバッグログに出力するタイトルと、内容を指定してデバッグログを出力します。
        /// デバッグログは非ローミングユーザのApplication Dataフォルダに出力されます。
        /// </summary>
        /// <param name="title">デバッグログのタイトル</param>
        /// <param name="description">デバッグログの内容</param>
        public static void DebugLogWriter(string title, string description)
        {
            //ログレベルを取得する
            string log_level_string = ConfigurationManager.AppSettings["ApplicationLogLevel"];
            int log_level = 0;
            if (NSKUtil.IsNumeric(log_level_string))
            {
                log_level = Convert.ToInt32(log_level_string);
                //ログレベルが8以上の場合のみデバッグを出力
                if (log_level >= 8)
                {
                    //ログ出力
                    LogWriter.ApplicationLogWriter(NSKLogPriority.PRIORITY_DEBUG,
                        title, description);
                }
            }
        }
    }
}

