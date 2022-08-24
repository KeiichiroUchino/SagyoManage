using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic;
using System.Configuration;
using jp.co.jpsys.util;
using jp.co.jpsys.util.log;
using GrapeCity.Win.Editors;
using GrapeCity.Win.Common;
using GrapeCity.Win.Components;
using GrapeCity.Win.MultiRow;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using GrapeCity.Win.Pickers;
using System.Text.RegularExpressions;
using GrapeCity.Win.MultiRow.InputMan;
using FarPoint.Win.Spread;
using System.IO;
using Jpsys.SagyoManage.Model.DALExceptions;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 画面を定義するクラスまたはその操作を定義するクラスにて共通に使用する、
    /// 各種静的メソッドを定義します。いわゆるシステム共通関数です。
    /// </summary>
    public class FrameUtilites
    {
        #region ユーザ定義

        /// <summary>
        /// デバッグログを出力するロガーのインスタンスを保持
        /// </summary>
        private static INSKLogWriter debugLogger = null;

        #endregion

        #region 項目初期値

        /// <summary>
        /// 時間初期値
        /// </summary>
        private const String HMS_DEFAULT_VALUE = "00:00";

        /// <summary>
        /// 配給一覧最大表示行数
        /// </summary>
        public static int COL_MAXROW_TANKLIST = 150;

        #endregion

        /// <summary>
        /// システム名称マスタの名称区分とコードを指定して、
        /// システム名称を取得します。
        /// </summary>
        /// <param name="kbn">システム名称区分</param>
        /// <param name="code">システム名称コード</param>
        /// <returns>システム名称</returns>
        public static string GetSystemName(DefaultProperty.SystemNameKbn kbn, int code)
        {
            var list = UserProperty.GetInstance().SystemNameList.Where(x => x.SystemNameKbn == (int)kbn && x.SystemNameCode == code);

            if (list == null || list.Count() == 0)
            {
                return string.Empty;
            }

            return list.ToList()[0].SystemName;
        }

        /// <summary>
        /// システム名称を表す列挙体を表す列挙体を指定して、システム名称の区分(種類)名を取得します。
        /// </summary>
        /// <param name="kbn">システム名称を表す列挙体</param>
        /// <returns>システム名称名</returns>
        public static string GetSystemNameKbnString(DefaultProperty.SystemNameKbn kbn)
        {
            string rt_val = string.Empty;

            switch (kbn)
            {
                case DefaultProperty.SystemNameKbn.None:
                    rt_val = "";
                    break;
                //case DefaultProperty.SystemNameKbn.KyujitsuKbn:
                //    rt_val = "休日区分";
                //    break;

                default:
                    break;
            }

            return rt_val;
        }

        /// <summary>
        /// 画面の表示モードを表す列挙体を指定して、表示モード名を取得します。
        /// </summary>
        public static string GetDataSpecifyModeNameString(DataSpecifyMode mode)
        {
            string rt_val = string.Empty;

            switch (mode)
            {
                case DataSpecifyMode.All:
                    rt_val = "すべて";
                    break;
                case DataSpecifyMode.Range:
                    rt_val = "範囲指定";
                    break;
                case DataSpecifyMode.Separated:
                    rt_val = "個別指定";
                    break;
                default:
                    break;
            }

            return rt_val;
        }

        /// <summary>
        /// 処理モードに応じて、ToolStripStatusLabelのTextを変更します
        /// </summary>
        /// <param name="label">ToolStripStatusLabelコントロール</param>
        /// <param name="processMode">処理モード</param>
        public static void SetProcessModeStatusLabel(ToolStripStatusLabel label, ProcessMode processMode)
        {
            switch (processMode)
            {
                case ProcessMode.Shinki:
                    label.Text = "新規登録";
                    break;
                case ProcessMode.Syuusei:
                    label.Text = "修正";
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// フォーム用共通メッセージテーブルのキー文字列を指定して、共通メッセージ
        /// を取得します。
        /// </summary>
        /// <param name="key">取得したいメッセージのキー文字列</param>
        /// <returns>共通メッセージ</returns>
        public static string GetDefineMessage(string key)
        {
            string rt_val = string.Empty;

            //リソースファイルからメッセージを取得
            rt_val =
                global::Jpsys.SagyoManage.FrameLib.message.
                        ResourceManager.GetString(key);

            if (rt_val == null)
            {
                throw new ApplicationException(
                    "指定されたキーに該当するメッセージは存在しません。");
            }

            return rt_val;
        }

        /// <summary>
        /// フォーム用共通メッセージテーブルのキー文字列及び書式指定文字列に埋め込む
        /// 文字列の配列を指定して、共通メッセージを取得します。
        /// </summary>
        /// <param name="key">取得したいメッセージのキー文字列</param>
        /// <param name="strArr">メッセージに埋め込む文字列の配列</param>
        /// <returns>メッセージを埋め込まれた共通メッセージ</returns>
        public static string GetDefineMessage(string key, params string[] strArr)
        {
            string rt_val = string.Empty;

            //リソースファイルからメッセージを取得
            string wk_src =
                global::Jpsys.SagyoManage.FrameLib.message.
                        ResourceManager.GetString(key);

            if (wk_src == null)
            {
                throw new ApplicationException(
                    "指定されたキーに該当するメッセージは存在しません。");
            }


            try
            {
                //書式指定文字列を置き換える
                rt_val = string.Format(wk_src, strArr);
            }
            catch (Exception err)
            {
                throw new ApplicationException("メッセージの構築に失敗しました。", err);
            }

            return rt_val;
        }

        /// <summary>
        /// ログ出力用共通メッセージテーブルのキー文字列および書式指定文字列に
        /// 埋め込む文字列の配列を指定して、ログ出力用共通メッセージを
        /// 取得します。
        /// </summary>
        /// <param name="key">取得したいログメッセージのキー文字列</param>
        /// <param name="strArr">メッセージに埋め込む文字列の配列</param>
        /// <returns>メッセージを埋め込まれたログ出力用共通メッセージ</returns>
        public static string GetDefineLogMessage(string key, string[] strArr)
        {
            string rt_val = string.Empty;

            //ログ用リソースファイルからメッセージを取得
            string wk_src =
                global::Jpsys.SagyoManage.FrameLib.loggingmsg.
                    ResourceManager.GetString(key);

            if (wk_src == null)
            {
                throw new ApplicationException(
                    "指定されたキーに該当するメッセージは存在しません。");
            }


            try
            {
                //書式指定文字列を置き換える
                rt_val = string.Format(wk_src, strArr);
            }
            catch (Exception err)
            {
                throw new ApplicationException("メッセージの構築に失敗しました。", err);
            }



            return rt_val;
        }

        /// <summary>
        /// MultiRowの編集コントロールからCell.Valueに相当するものを取得します。
        /// </summary>
        /// <param name="editingControl">MultiRowの編集コントロール（IEditingControlを実装するオブジェクト）</param>
        /// <returns>Cell.Valueに相当する値</returns>
        public static object GetEditingControlValue(Control editingControl)
        {
            return FrameLib.MultiRow.GcMultiRowExtensions.GetEditingControlValue(editingControl);
        }

        /// <summary>
        /// MultiRowの編集コントロールのCell.Valueに相当するものに値を設定します。
        /// </summary>
        /// <param name="editingControl">MultiRowの編集コントロール（IEditingControlを実装するオブジェクト）</param>
        /// <param name="value">値</param>
        public static void SetEditingControlValue(Control editingControl, object value)
        {
            FrameLib.MultiRow.GcMultiRowExtensions.SetEditingControlValue(editingControl, value);
        }


        /// <summary>
        /// CSVをStringのリストを格納するリストに変換
        /// </summary>
        /// <param name="csvText">CSVの内容が入ったString</param>
        /// <returns>変換結果のStringのリストを格納するリスト</returns>
        public static List<List<string>> CsvToList(string csvText)
        {
            //前後の改行を削除しておく
            csvText = csvText.Trim(new char[] { '\r', '\n' });

            List<List<string>> csvRecords = new List<List<string>>();
            List<string> csvFields = new List<string>();

            int csvTextLength = csvText.Length;
            int startPos = 0, endPos = 0;
            string field = "";

            while (true)
            {
                //空白を飛ばす
                while (startPos < csvTextLength &&
                    (csvText[startPos] == ' ' || csvText[startPos] == '\t'))
                {
                    startPos++;
                }

                //データの最後の位置を取得
                if (startPos < csvTextLength && csvText[startPos] == '"')
                {
                    //"で囲まれているとき
                    //最後の"を探す
                    endPos = startPos;
                    while (true)
                    {
                        endPos = csvText.IndexOf('"', endPos + 1);
                        if (endPos < 0)
                        {
                            throw new ApplicationException("\"が不正");
                        }
                        //"が2つ続かない時は終了
                        if (endPos + 1 == csvTextLength || csvText[endPos + 1] != '"')
                        {
                            break;
                        }
                        //"が2つ続く
                        endPos++;
                    }

                    //一つのフィールドを取り出す
                    field = csvText.Substring(startPos, endPos - startPos + 1);
                    //""を"にする
                    field = field.Substring(1, field.Length - 2).Replace("\"\"", "\"");

                    endPos++;
                    //空白を飛ばす
                    while (endPos < csvTextLength &&
                        csvText[endPos] != ',' && csvText[endPos] != '\n')
                    {
                        endPos++;
                    }
                }
                else
                {
                    //"で囲まれていない
                    //カンマか改行の位置
                    endPos = startPos;
                    while (endPos < csvTextLength &&
                        csvText[endPos] != ',' && csvText[endPos] != '\n')
                    {
                        endPos++;
                    }

                    //一つのフィールドを取り出す
                    field = csvText.Substring(startPos, endPos - startPos);
                    //後の空白を削除
                    field = field.TrimEnd();
                }

                //フィールドの追加
                csvFields.Add(field);

                //行の終了か調べる
                if (endPos >= csvTextLength || csvText[endPos] == '\n')
                {
                    //行の終了
                    //レコードの追加
                    csvFields.TrimExcess();
                    csvRecords.Add(csvFields);
                    csvFields = new List<string>(csvFields.Count);

                    if (endPos >= csvTextLength)
                    {
                        //終了
                        break;
                    }
                }

                //次のデータの開始位置
                startPos = endPos + 1;
            }

            csvRecords.TrimExcess();
            return csvRecords;
        }

        /// <summary>
        /// ログに出力するタイトルと、内容を指定してアプリケーション動作ログを出力します。
        /// ログは非ローミングユーザのApplication Dataフォルダに出力されます。
        /// </summary>
        /// <param name="priority">ログレベル（ログの重要度）</param>
        /// <param name="title">ログのタイトル</param>
        /// <param name="description">ログの内容</param>
        public static void ApplicationLogWriter(NSKLogPriority priority, string title, string description)
        {
            NSKLogManager.LogFilePrefix = priority.ToString();
            //ログファイルの出力先をSystemPropertyから取得
            NSKLogManager.LogFileDirectoryName =
                Property.SystemProperty.GetInstance().DebugLoggingPath;
            //NSKLogManager.LogFileName = @"errlog";/
            //ロガーのインスタンスがnullだった時はインスタンスを作成する。
            if (FrameUtilites.debugLogger == null)
            {
                FrameUtilites.debugLogger = NSKLogManager.GetLogger(NSKLogWriterType.TYPE_FILE);
            }

            //ログを出力
            FrameUtilites.debugLogger.WriteLog(
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
                    FrameUtilites.ApplicationLogWriter(NSKLogPriority.PRIORITY_DEBUG,
                        title, description);
                }
            }
        }

        /// <summary>
        /// 印刷画面名を指定して、その印刷設定を削除します。
        /// </summary>
        /// <param name="printToFrameName">印刷画面名</param>
        public static void DelPrintSetting(string printToFrameName)
        {
            //印刷設定ファイルのファイル名までのパスを取得する
            string wk_path =
                System.IO.Path.Combine(
                    Property.SystemProperty.GetInstance().ReportPrinterSettingPath,
                        printToFrameName) + ".xml";

            //指定して印刷画面の印刷設定ファイルを削除する
            if (System.IO.File.Exists(wk_path))
            {
                System.IO.File.Delete(wk_path);
            }
        }

        /// <summary>
        /// 対象の文字列をShiftJISで取扱い、バイト単位で文字列の一部を
        /// 取得します。開始位置及び取得する文字数はバイト数にて指定します。
        /// </summary>
        /// <param name="val">扱う対象の文字列</param>
        /// <param name="start">開始位置（1からです）</param>
        /// <param name="length">取得するバイト数</param>
        /// <returns>
        /// 対象の文字列をShiftJISにて取扱い、指定された開始位置（バイト数）から
        /// 指定したバイト数分文字列を取得します。その際最後の文字列が２バイト文字
        /// だった場合は切り捨てます。
        /// </returns>
        public static string SubstringShiftJIS(string val, int start, int length)
        {

            //空文字に対しては常に空文字を返す
            if (val.Trim().Length == 0)
            {
                return string.Empty;
            }


            //Lengthが0か、Start以降のバイト数をオーバーする場合は
            //Start以降の全バイトが指定されたものとみなす。 
            int rest_length =
                System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(val) - start + 1;

            if (length == 0 || length > rest_length)
            {
                length = rest_length;
            }

            //文字列をShiftJISで切り出す
            System.Text.Encoding SJIS = System.Text.Encoding.GetEncoding("Shift-JIS");
            byte[] B = (byte[])Array.CreateInstance(typeof(byte), length);

            Array.Copy(SJIS.GetBytes(val), start - 1, B, 0, length);

            string st1 = SJIS.GetString(B);

            //最後の１バイトが文字列の半分かどうか判定する
            int result_length =
                System.Text.Encoding.GetEncoding("Shift_JIS").GetByteCount(st1) - start + 1;

            if (Strings.Asc(Strings.Right(st1, 1)) == 0)
            {
                return st1.Substring(0, st1.Length - 1);
            }
            else if (length == result_length - 1)
            {
                return st1.Substring(0, st1.Length - 1);
            }
            else
            {
                //その他の場合 
                return st1;
            }

        }

        /// <summary> 
        /// 文字列の配列を型とするジェネリックのList型の値とファイルの保存先を指定して、 
        /// CSVファイルを出力します。 
        /// １件目をヘッダとはみなしません。 
        /// </summary> 
        /// <param name="stringArrayList">出力したい値の入ったリスト</param> 
        /// <param name="filePath">ファイルの保存先</param> 
        /// <remarks></remarks> 
        public static void CreateCSVDataByStringArrList(List<string[]> stringArrayList, string filePath)
        {
            try
            {
                //保存先のCSVファイルのパス 
                string csvPath = filePath;
                //CSVファイルに書き込むときに使うEncoding 
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                //開く 
                System.IO.StreamWriter sr = new System.IO.StreamWriter(csvPath, false, enc);

                int colCount = stringArrayList[0].Length;
                int lastColIndex = colCount - 1;

                //レコードを書き込む 
                foreach (string[] row in stringArrayList)
                {
                    for (Int32 i = 0; i <= colCount - 1; i++)
                    {
                        //フィールドの取得 
                        string field = row[i].ToString();
                        //"で囲む必要があるか調べる 
                        if (field.IndexOf(ControlChars.Quote) > -1 || field.IndexOf(',') > -1 || field.IndexOf(ControlChars.Cr) > -1 ||
                            field.IndexOf(ControlChars.Lf) > -1 || field.StartsWith(" ") || field.StartsWith(ControlChars.Tab.ToString()) ||
                            field.EndsWith(" ") || field.EndsWith(ControlChars.Tab.ToString()))
                        {
                            if (field.IndexOf(ControlChars.Quote) > -1)
                            {
                                //"を""とする 
                                field = field.Replace("\"", "\"\"");
                            }
                            field = "\"" + field + "\"";
                        }
                        //フィールドを書き込む 
                        sr.Write(field);
                        //カンマを書き込む 
                        if (lastColIndex > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する 
                    sr.Write("\r\n");
                }

                //閉じる 
                sr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary> 
        /// 文字列の配列を型とするジェネリックのList型の値とファイルの保存先を指定して、 
        /// CSVファイルを出力します。 
        /// １件目をヘッダとはみなしません。 
        /// </summary> 
        /// <param name="stringArrayList">出力したい値の入ったリスト</param> 
        /// <param name="filePath">ファイルの保存先</param> 
        /// <param name="QuoteFlag">ダブルクォート必須フラグ（true：囲む、false：囲まない）</param> 
        /// <remarks></remarks> 
        public static void CreateCSVDataByStringArrListEx(List<string[]> stringArrayList, string filePath, bool QuoteFlag)
        {
            try
            {
                //保存先のCSVファイルのパス 
                string csvPath = filePath;
                //CSVファイルに書き込むときに使うEncoding 
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                //開く 
                System.IO.StreamWriter sr = new System.IO.StreamWriter(csvPath, false, enc);

                int colCount = stringArrayList[0].Length;
                int lastColIndex = colCount - 1;

                //レコードを書き込む 
                foreach (string[] row in stringArrayList)
                {
                    for (Int32 i = 0; i <= colCount - 1; i++)
                    {
                        //フィールドの取得 
                        string field = row[i].ToString();
                        //"で囲む必要があるか調べる 
                        if (
                            QuoteFlag
                            || 
                            (
                                field.IndexOf(ControlChars.Quote) > -1 || field.IndexOf(',') > -1 || field.IndexOf(ControlChars.Cr) > -1 ||
                                field.IndexOf(ControlChars.Lf) > -1 || field.StartsWith(" ") || field.StartsWith(ControlChars.Tab.ToString()) ||
                                field.EndsWith(" ") || field.EndsWith(ControlChars.Tab.ToString()))
                            )
                        {
                            if (field.IndexOf(ControlChars.Quote) > -1)
                            {
                                //"を""とする 
                                field = field.Replace("\"", "\"\"");
                            }
                            field = "\"" + field + "\"";
                        }
                        //フィールドを書き込む 
                        sr.Write(field);
                        //カンマを書き込む 
                        if (lastColIndex > i)
                        {
                            sr.Write(',');
                        }
                    }
                    //改行する 
                    sr.Write("\r\n");
                }

                //閉じる 
                sr.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 一致の方法を選択して、文字列に任意の文字列が含まれているかどうか
        /// を判断します。
        /// </summary>
        /// <param name="srcValue">比較対象の文字列</param>
        /// <param name="containtString">比較する文字列</param>
        /// <param name="type">一致方法</param>
        /// <returns>判断結果（true:一致する）</returns>
        public static bool SpecialContaintString(string srcValue, string containtString, SpecialStringContaintType type)
        {
            bool rt_val = false;

            switch (type)
            {
                case SpecialStringContaintType.StartsWith:
                    //前方一致
                    rt_val = srcValue.StartsWith(containtString);
                    break;
                case SpecialStringContaintType.Contains:
                    //包含一致
                    rt_val = srcValue.Contains(containtString);
                    break;
                case SpecialStringContaintType.EndsWith:
                    //後方一致
                    rt_val = srcValue.EndsWith(containtString);
                    break;
                default:
                    break;
            }
            return rt_val;
        }

        /// <summary>
        /// TimeSpan型の値をDateTime型に変換して返します。日付にはシステム日付を使用します。
        /// （指定したTimeSpan値がMinValueの場合、戻り値はDateTime.MinValueを返します。）
        /// </summary>
        /// <param name="ts">DateTime型に変換したいTimeSpan値</param>
        /// <returns>DateTime値</returns>
        public static DateTime ConvertTimeSpanToDateTime(TimeSpan ts)
        {
            DateTime rt_val = DateTime.MinValue;

            if (ts != TimeSpan.MinValue)
            {
                DateTime today = DateTime.Today;

                rt_val =
                    new DateTime(today.Year, today.Month, today.Day,
                        ts.Hours, ts.Minutes, ts.Seconds);
            }

            return rt_val;
        }

        /// <summary>
        /// DateTime型の時分秒(hhmmss)値をTimeSpan型に変換して返します。年月日(yyyyMMdd)は値に影響を及ぼしません。
        /// （指定したDateTime値がMinValueの場合、戻り値はTimeSpan.MinValueを返します。）
        /// </summary>
        /// <param name="dt">TimeSpan型に変換したいDateTime値</param>
        /// <returns>TimeSpan値</returns>
        public static TimeSpan ConvertDateTimeToTimeSpan(DateTime dt)
        {
            TimeSpan rt_val = TimeSpan.MinValue;

            if (dt != DateTime.MinValue)
            {
                rt_val = new TimeSpan(dt.Hour, dt.Minute, dt.Second);
            }

            return rt_val;
        }

        /// <summary>
        /// Imeの変換モードを設定します。
        /// </summary>
        /// <param name="control"></param>
        public static void SetImeSentenceModeForInputMan(GcTextBox control)
        {
            GcIme gcIme1 = new GcIme();

            gcIme1.SetImeSentenceMode(control, GrapeCity.Win.Editors.ImeSentenceMode.Direct);

            // 対象のコントロールにGotFocusイベントを実装します。 
            control.GotFocus += txtHanKana_GotFocus;

        }

        private static void txtHanKana_GotFocus(object sender, EventArgs e)
        {
            // イベント発生元をControlオブジェクトに変換します。 
            Control ctrl = sender as Control;
            if (ctrl == null)
            {
                return;
            }

            // BeginInvokeメソッドを使用して、SetImeSentenceModeメソッドのDirectの設定を有効にします。 
            ctrl.BeginInvoke((MethodInvoker)delegate()
            {
                if (ctrl.ContainsFocus)
                {
                    GcIme gcIme1 = new GcIme();
                    gcIme1.SetInputScope(ctrl, GrapeCity.Win.Editors.InputScopeNameValue.KatakanaHalfWidth);
                    gcIme1.SetImeSentenceMode(ctrl, GrapeCity.Win.Editors.ImeSentenceMode.Direct);
                }
            });
        }

        /// <summary>
        /// GcComboBoxをセットアップします。
        /// 指定したIDictionary型のデータソースはGcCombBoxのデータソースに設定されます。
        /// 要素のKeyはコンボのValueに、ValueはTextに設定されます。
        /// </summary>
        /// <param name="comboBox"></param>
        /// <param name="datasource"></param>
        public static void SetupGcComboBox(GrapeCity.Win.Editors.GcComboBox comboBox, IDictionary datasource)
        {
            comboBox.Items.Clear();
            comboBox.ListColumns.Clear();

            //列の設定
            GrapeCity.Win.Editors.ListColumn column01 = new GrapeCity.Win.Editors.ListColumn("コード");
            column01.Visible = false;
            GrapeCity.Win.Editors.ListColumn column02 = new GrapeCity.Win.Editors.ListColumn("名称");
            column02.Width = comboBox.Width;
            comboBox.ListColumns.AddRange((new GrapeCity.Win.Editors.ListColumn[] { column01, column02 }));

            //データの設定
            foreach (DictionaryEntry comboItem in datasource)
            {
                comboBox.Items.Add(
                    new GrapeCity.Win.Editors.ListItem(
                        new GrapeCity.Win.Editors.SubItem[] 
                        { 
                            new GrapeCity.Win.Editors.SubItem(comboItem.Key), 
                            new GrapeCity.Win.Editors.SubItem(comboItem.Value) 
                        }));
            }

            comboBox.ValueSubItemIndex = 0;
            comboBox.TextSubItemIndex = 1;
            comboBox.ListHeaderPane.Visible = false;
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboBox.DropDown.AllowResize = false;
        }

        /// <summary>
        /// GcComboBoxCellをセットアップします。
        /// 指定したIDictionary型のデータソースはGcCombBoxCellのデータソースに設定されます。
        /// 要素のKeyはコンボのValueに、ValueはTextに設定されます。
        /// </summary>
        /// <param name="comboBoxCell"></param>
        /// <param name="datasource"></param>
        /// <param name="addEmptyItemToFirst">空のItemを追加するかどうか</param>
        /// <param name="emptyItemTextToFirst">空のItemの名称</param>
        public static void SetupGcComboBoxCell(GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell comboBoxCell, IDictionary datasource, bool addEmptyItemToFirst = false, string emptyItemTextToFirst = " ")
        {
            comboBoxCell.Items.Clear();
            comboBoxCell.ListColumns.Clear();

            //列の設定
            GrapeCity.Win.MultiRow.InputMan.ListColumn column01 = new GrapeCity.Win.MultiRow.InputMan.ListColumn("コード");
            column01.Visible = false;
            GrapeCity.Win.MultiRow.InputMan.ListColumn column02 = new GrapeCity.Win.MultiRow.InputMan.ListColumn("名称");

            // 2017/08/09 K.Yamasako
            // 幅を固定で設定（高DPIの場合、切れてしまうため）
            //column02.Width = comboBoxCell.Width;
            column02.Width = 500;
            
            comboBoxCell.ListColumns.AddRange((new GrapeCity.Win.MultiRow.InputMan.ListColumn[] { column01, column02 }));

            if (addEmptyItemToFirst)
            {
                comboBoxCell.Items.Add(
                    new GrapeCity.Win.MultiRow.InputMan.ListItem(
                        new GrapeCity.Win.MultiRow.InputMan.SubItem[] 
                        { 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(null), 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(emptyItemTextToFirst) 
                        }));
            }

            //データの設定
            foreach (DictionaryEntry comboItem in datasource)
            {
                comboBoxCell.Items.Add(
                    new GrapeCity.Win.MultiRow.InputMan.ListItem(
                        new GrapeCity.Win.MultiRow.InputMan.SubItem[] 
                        { 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(comboItem.Key), 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(Convert.ToInt32(comboItem.Key) + "：" + comboItem.Value) 
                        }));
            }

            comboBoxCell.ValueSubItemIndex = 0;
            comboBoxCell.TextSubItemIndex = 1;
            comboBoxCell.ListHeaderPane.Visible = false;
            comboBoxCell.DropDownStyle = GrapeCity.Win.MultiRow.MultiRowComboBoxStyle.DropDownList;
            comboBoxCell.DropDown.AllowResize = false;
        }

        /// <summary>
        /// GcComboBoxCellをセットアップします。
        /// 指定したIDictionary型のデータソースはGcCombBoxCellのデータソースに設定されます。
        /// 要素のKeyはコンボのValueに、ValueはTextに設定されます。
        /// </summary>
        /// <param name="comboBoxCell"></param>
        /// <param name="datasource"></param>
        /// <param name="addEmptyItemToFirst">空のItemを追加するかどうか</param>
        /// <param name="emptyItemTextToFirst">空のItemの名称</param>
        public static void SetupGcComboBoxCellEx(GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell comboBoxCell, IDictionary datasource, bool addEmptyItemToFirst = false, string emptyItemTextToFirst = " ")
        {
            comboBoxCell.Items.Clear();
            comboBoxCell.ListColumns.Clear();

            //列の設定
            GrapeCity.Win.MultiRow.InputMan.ListColumn column01 = new GrapeCity.Win.MultiRow.InputMan.ListColumn("コード");
            column01.Visible = false;
            GrapeCity.Win.MultiRow.InputMan.ListColumn column02 = new GrapeCity.Win.MultiRow.InputMan.ListColumn("名称");

            column02.Width = 500;

            comboBoxCell.ListColumns.AddRange((new GrapeCity.Win.MultiRow.InputMan.ListColumn[] { column01, column02 }));

            if (addEmptyItemToFirst)
            {
                comboBoxCell.Items.Add(
                    new GrapeCity.Win.MultiRow.InputMan.ListItem(
                        new GrapeCity.Win.MultiRow.InputMan.SubItem[] 
                        { 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(null), 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(emptyItemTextToFirst) 
                        }));
            }

            //データの設定
            foreach (DictionaryEntry comboItem in datasource)
            {
                comboBoxCell.Items.Add(
                    new GrapeCity.Win.MultiRow.InputMan.ListItem(
                        new GrapeCity.Win.MultiRow.InputMan.SubItem[] 
                        { 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(comboItem.Key), 
                            new GrapeCity.Win.MultiRow.InputMan.SubItem(Convert.ToString(comboItem.Value)) 
                        }));
            }

            comboBoxCell.ValueSubItemIndex = 0;
            comboBoxCell.TextSubItemIndex = 1;
            comboBoxCell.ListHeaderPane.Visible = false;
            comboBoxCell.DropDownStyle = GrapeCity.Win.MultiRow.MultiRowComboBoxStyle.DropDownList;
            comboBoxCell.DropDown.AllowResize = false;
        }

        /// <summary>
        /// GcComboBoxをセットアップします。
        /// 指定したIDictionary型のデータソースはGcCombBoxのデータソースに設定されます。
        /// 要素のKeyはコンボのValueに、ValueはTextに設定されます。
        /// </summary>
        /// <param name="comboBox">コンボボックス</param>
        /// <param name="datasource">データソース</param>
        /// <param name="addEmptyItemToFirst">空のItemを追加するかどうか</param>
        /// <param name="emptyItemTextToFirst">空のItemの名称</param>
        /// <param name="addPrefixKey">コンボアイテムのプレフィックスにキーとつけるかどうか</param>
        public static void SetupGcComboBoxForValueText(GrapeCity.Win.Editors.GcComboBox comboBox, IDictionary datasource = null, bool addEmptyItemToFirst = false, string emptyItemTextToFirst = null, bool addPrefixKey = true)
        {
            comboBox.Items.Clear();
            comboBox.ListColumns.Clear();

            //列の設定
            GrapeCity.Win.Editors.ListColumn column01 = new GrapeCity.Win.Editors.ListColumn("キー");
            column01.Visible = false;
            GrapeCity.Win.Editors.ListColumn column02 = new GrapeCity.Win.Editors.ListColumn("名称");
            column02.AutoWidth = true;
            column02.Width = comboBox.Width;
            comboBox.ListColumns.AddRange((new GrapeCity.Win.Editors.ListColumn[] { column01, column02 }));
            comboBox.ValueSubItemIndex = 0;
            comboBox.TextSubItemIndex = 1;
            comboBox.ListHeaderPane.Visible = false;
            comboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;

            if (datasource != null)
            comboBox.DropDown.AllowResize = false;
            {
                SetGcComboBoxDataSource(comboBox, datasource, addEmptyItemToFirst, emptyItemTextToFirst, addPrefixKey);
            }
        }

        /// <summary>
        /// 指定したComboBoxにDictinary形式のデータソースを設定します。
        /// DictionaryのKeyはコンボのValueに、ValueはTextに設定されます。
        /// </summary>
        /// <param name="comboBox">コンボボックス</param>
        /// <param name="datasource">データソース</param>
        /// <param name="addEmptyItemToFirst">空のアイテムを追加するかどうか</param>
        /// <param name="emptyItemTextToFirst">空のItemの名称</param>
        /// <param name="addPrefixKey">コンボアイテムのプレフィックスにキーとつけるかどうか</param>
        private static void SetGcComboBoxDataSource(GrapeCity.Win.Editors.GcComboBox comboBox, IDictionary datasource, bool addEmptyItemToFirst = false, string emptyItemTextToFirst = null, bool addPrefixKey = true)
        {
            comboBox.Items.Clear();

            if (addEmptyItemToFirst)
            {
                comboBox.Items.Add(
                    new GrapeCity.Win.Editors.ListItem(
                        new GrapeCity.Win.Editors.SubItem[] 
                        { 
                            new GrapeCity.Win.Editors.SubItem(null), 
                            new GrapeCity.Win.Editors.SubItem(emptyItemTextToFirst) 
                        }));
            }

            //データの設定
            foreach (DictionaryEntry comboItem in datasource)
            {
                if (addPrefixKey)
                {
                    comboBox.Items.Add(
                    new GrapeCity.Win.Editors.ListItem(
                        new GrapeCity.Win.Editors.SubItem[] 
                        { 
                            new GrapeCity.Win.Editors.SubItem(comboItem.Key), 
                            new GrapeCity.Win.Editors.SubItem(Convert.ToInt32(comboItem.Key) + "：" + comboItem.Value) 
                        }));
                }
                else
                {
                    comboBox.Items.Add(
                    new GrapeCity.Win.Editors.ListItem(
                        new GrapeCity.Win.Editors.SubItem[] 
                        { 
                            new GrapeCity.Win.Editors.SubItem(comboItem.Key), 
                            new GrapeCity.Win.Editors.SubItem(comboItem.Value) 
                        }));
                }

                
            }
        }


        /// <summary>
        /// 画面の終了を要求される例外のインスタンスを受け取ったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Exception ex, Form f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                           MessageBoxIcon.Error);
        }

        /// <summary>
        /// リトライ可能例外のインスタンスを受けったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Model.DALExceptions.CanRetryException ex, Form f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                            ex.Msgicon);
        }

        /// <summary>
        /// リトライ可能例外のインスタンスを受けったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Model.DALExceptions.UniqueConstraintException ex, Form f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                            ex.Msgicon);
        }

        /// <summary>
        /// 画面の終了を要求される例外のインスタンスを受け取ったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Model.DALExceptions.MustCloseFormException ex, Form f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                            ex.Msgicon);
        }

        /// <summary>
        /// リトライ可能例外のインスタンスを受けったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Model.DALExceptions.CanRetryException ex, UserControl f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                            ex.Msgicon);
        }

        /// <summary>
        /// 画面の終了を要求される例外のインスタンスを受け取ったときのメッセージを表示します。
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="f"></param>
        public static void ShowExceptionMessage(Model.DALExceptions.MustCloseFormException ex, UserControl f)
        {
            //他から更新されている場合の例外ハンドラ
            MessageBox.Show(
                            f,
                            ex.Message,
                            f.Text,
                            MessageBoxButtons.OK,
                            ex.Msgicon);
        }

        /// <summary>
        /// 指定した値型がデフォルト値（たとえば、DateTime：MinValue, Int32：0など...）の場合はnullに変換する
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T? StructToNullIfValueIsDefault<T>(T val) where T : struct
        {
            return (val.Equals(default(T))) ? null : (T?)val;
        }

        /// <summary>
        /// stringがIsNullOrWhiteSpaceメソッドでtrueを返す場合は、nullにして返す
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static string StringToNullIfValueIsNullOrWhiteSpace(string val)
        {
            return (string.IsNullOrWhiteSpace(val)) ? null : val;
        }

        /// <summary>
        /// GcTime.Valueの値をTimeSpan型にして返却します。
        /// 秒は0を入れて返却する
        /// </summary>
        /// <param name="val">GcTime.Value</param>
        /// <returns>TimeSpan型に変換された値</returns>
        public static TimeSpan GetGcTimeValue(TimeSpan? val)
        {
            TimeSpan rt_val = TimeSpan.MinValue;

            if (val != null && val != TimeSpan.MinValue)
            {
                rt_val = new TimeSpan(val.Value.Hours, val.Value.Minutes, 0);
            }

            return rt_val;
        }

        /// <summary>
        /// GcTime.Valueにセットする値を作成する
        /// </summary>
        /// <param name="val">GcTime.Value</param>
        /// <returns>TimeSpan型に変換された値</returns>
        public static TimeSpan? SetGcTimeValue(TimeSpan val)
        {
            TimeSpan? rt_val = null;

            if (val != TimeSpan.MinValue)
            {
                rt_val = (TimeSpan?)val;
            }

            return rt_val;
        }

        /// <summary>
        ///明細(マルチロウ)の指定した行数目を引数の背景色を変更する
        ///指定した行数が-1だった場合はすべての行の背景色を変更する
        /// </summary>
        /// <param name="mrow">明細(マルチロウ)</param>
        /// <param name="rowIndex">○行目(-1の場合は全行を対象とする)</param>
        /// <param name="color">変更させる色</param>
        public static void ChangeBackColorMrow(GcMultiRow mrow, int rowIndex, Color color)
        {
            GrapeCity.Win.MultiRow.Row wk_curmrow;

            if (rowIndex == -1)
            {
                for (int i = 0; i < mrow.RowCount; i++)
                {
                    wk_curmrow = mrow.Rows[i];

                    wk_curmrow.BackColor = color;
                }
            }
            else
            {
                wk_curmrow = mrow.Rows[rowIndex];

                wk_curmrow.BackColor = color;
            }

        }

        /// <summary>
        ///明細(マルチロウ)の指定した行数目を引数の文字色を変更する
        ///指定した行数が-1だった場合はすべての行の文字色を変更する
        /// </summary>
        /// <param name="mrow">明細(マルチロウ)</param>
        /// <param name="rowIndex">○行目(-1の場合は全行を対象とする)</param>
        /// <param name="color">変更させる色</param>
        public static void ChangeForeColorMrow(GcMultiRow mrow, int rowIndex, Color color)
        {
            GrapeCity.Win.MultiRow.Row wk_curmrow;

            if (rowIndex == -1)
            {
                for (int i = 0; i < mrow.RowCount; i++)
                {
                    wk_curmrow = mrow.Rows[i];

                    for (int j = 0; j < mrow.Columns.Count(); j++)
                    {
                        wk_curmrow[j].Style.ForeColor = color;
                    }
                }
            }
            else
            {
                wk_curmrow = mrow.Rows[rowIndex];

                for (int i = 0; i < mrow.Columns.Count(); i++)
                {
                    wk_curmrow[i].Style.ForeColor = color;
                }
            }

        }

        /// <summary>
        /// 会計年度を計算します。
        /// 戻り値は西暦の下2桁で返します。
        /// </summary>
        /// <returns></returns>
        public static int CalcKaikeiNendo(DateTime date)
        {
            if (date == DateTime.MinValue)
                return 0;

            int year = int.Parse(date.ToString("yy"));

            int month = date.Month;

            //1月～3月なら年に1をマイナスする。
            if (month < 4)
            {
                year -= 1;
            }

            return year;
        }

        /// <summary>
        /// ファイル名を指定して、保存先を選択して、保存先、ダイアログの結果を返します。
        /// </summary>
        /// <param name="fileName">ファイル名</param>
        /// <param name="dialogResult">ダイアログボックスの戻り値</param>
        /// <returns></returns>
        public static string SelectFilePath(string fileName, out DialogResult dialogResult)
        {
            string rt_val = string.Empty;

            SaveFileDialog sfd = new SaveFileDialog();
            //選択中のテーブル + .csvを既定のファイル名に
            sfd.FileName = fileName.Trim() + ".csv";
            //はじめに表示されるフォルダを指定する
            sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //[ファイルの種類]に表示される選択肢を指定する
            sfd.Filter =
                "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
            //[ファイルの種類]の選択をテキストに
            sfd.FilterIndex = 0;
            //タイトルを設定する
            sfd.Title = "保存先のファイルを選択してください";
            //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
            sfd.RestoreDirectory = true;

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                dialogResult = DialogResult.OK;
            }
            else
            {
                dialogResult = DialogResult.Cancel;
            }

            rt_val = sfd.FileName;

            return rt_val;
        }
        
        /// <summary>
        /// 現在稼働中のユーザ毎にレジストリに保存されているファイル出力に
        /// 使用するパスを取得します。
        /// 見つからなかった場合はドキュメントフォルダのパスを返します。
        /// </summary>
        /// <param name="frameName">画面名（レジストリのキー）</param>
        /// <param name="reportfolderpath">帳票フォルダパス</param>
        /// <returns>ファイル出力先のパス</returns>
        public static string GetExportFilePath(string frameName, string reportfolderpath = "")
        {
            string defaultPath = string.Empty;

            if (reportfolderpath == "")
            {
                // 初期値として、ドキュメントを取得しておく
                defaultPath =
                    System.Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\";
            }
            else
            {
                // 初期値として、ドキュメントを取得しておく
                defaultPath = reportfolderpath + @"\";
            }

            // レジストリキーを開く（キーがなければ作る）
            Microsoft.Win32.RegistryKey rkey =
                Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    SystemProperty.GetInstance().SystemDefaultRegistryPath);

            // 値を取得
            return (string)rkey.GetValue(frameName, defaultPath);
        }

        /// <summary>
        /// ファイル出力に使用するパスをレジストリへ保存します。
        /// </summary>
        /// <param name="path">保存する出力先パス</param>
        /// <param name="frameName">画面名（レジストリのキー）</param>
        public static void SetExportFilePath(string path, string frameName)
        {
            // レジストリキーを開く（キーがなければ作る）
            Microsoft.Win32.RegistryKey rkey =
                Microsoft.Win32.Registry.CurrentUser.CreateSubKey(
                    SystemProperty.GetInstance().SystemDefaultRegistryPath);

            // 値を保存
            rkey.SetValue(frameName, path);
        }

        /// <summary>
        /// チェックボックス等をEnterキーでタブ遷移します。
        /// </summary>
        /// <param name="sender">チェックボックス等</param>
        /// <param name="e">キーイベント</param>
        /// <param name="form">フォーム</param>
        public static void SelectNextControlByKeyDown(object sender, KeyEventArgs e, object form)
        {
            if (sender.GetType().Equals(typeof(CheckBox))
                || sender.GetType().Equals(typeof(DateTimePicker))
                || sender.GetType().Equals(typeof(RadioButton))
                || sender.GetType().Equals(typeof(GcColorPicker)))
            {
                switch (e.KeyCode)
                {
                    case Keys.Return:
                        ((Form)form).SelectNextControl(((Form)form).ActiveControl, true, true, true, true);
                        break;
                    case Keys.F12:
                        ((Form)form).SelectNextControl(((Form)form).ActiveControl, false, true, true, true);
                        break;
                }
            }
        }

        /// <summary>
        /// 時刻テキストボックスのEnterイベントの制御を行います。
        /// </summary>
        /// <param name="sender">テキストボックス</param>
        /// <param name="e">キーイベント</param>
        public static void HmsEnter(object sender, EventArgs e)
        {
            if (sender.GetType().Equals(typeof(GcTextBox)))
            {
                ((GcTextBox)sender).Text = ((GcTextBox)sender).Text.Replace(":", string.Empty);
                ((GcTextBox)sender).MaxLength = 4;
                ((GcTextBox)sender).SelectAll();
            }
            else if (sender.GetType().Equals(typeof(GcTextBoxCell)))
            {
                ((GcTextBoxCell)sender).Value = Convert.ToString(((GcTextBoxCell)sender).Value).Replace(":", string.Empty);
                ((GcTextBoxCell)sender).MaxLength = 4;
                //((GcTextBoxCell)sender).SelectAll();
            }
        }

        /// <summary>
        /// 時刻テキストボックスのLeaveイベントの制御を行います。
        /// </summary>
        /// <param name="sender">テキストボックス</param>
        /// <param name="e">キーイベント</param>
        public static void HmsLeave(object sender, EventArgs e)
        {
            String setVal = HMS_DEFAULT_VALUE;
            try
            {
                if (sender.GetType().Equals(typeof(GcTextBox)))
                {
                    Regex re = new Regex(@"[^0-9]");
                    String strVal = re.Replace(((GcTextBox)sender).Text, "");
                    setVal = ProjectUtilites.IntToHMSDigit4(Convert.ToInt32(strVal));
                }
                else if (sender.GetType().Equals(typeof(GcTextBoxCell)))
                {
                    Regex re = new Regex(@"[^0-9]");
                    String strVal = re.Replace(Convert.ToString(((GcTextBoxCell)sender).Value), "");
                    setVal = ProjectUtilites.IntToHMSDigit4(Convert.ToInt32(strVal));
                }
            }
            catch
            {
            }
            finally
            {
                if (sender.GetType().Equals(typeof(GcTextBox)))
                {
                    ((GcTextBox)sender).MaxLength = 5;
                    ((GcTextBox)sender).Text = setVal;
                }
                else if (sender.GetType().Equals(typeof(GcTextBoxCell)))
                {
                    ((GcTextBoxCell)sender).MaxLength = 5;
                    ((GcTextBoxCell)sender).Value = setVal;
                }
            }
        }

        /// <summary>
        /// 表背景色を設定します。
        /// </summary>
        /// <returns>タイトル背景色</returns>
        public static void SetFrameGridBackColor(Control frame)
        {
            try
            {
                Control[] all = FrameUtilites.GetGridControls(frame);
                foreach (Control c in all)
                {
                    if (c.GetType().Equals(typeof(DataGridView)))
                    {
                        FrameUtilites.SetDataGridViewColor((DataGridView)c);
                    }
                    else if (c.GetType().Equals(typeof(FpSpread)))
                    {
                        FrameUtilites.SetSpreadColor((FpSpread)c);
                    }
                    else if (c.GetType().Equals(typeof(GcMultiRow)))
                    {
                        FrameUtilites.SetGcMultiRowColor((GcMultiRow)c);
                    }
                }
            }
            catch
            {
                //何もしない
            }
        }

        /// <summary>
        /// DataGridViewの色を取得します。
        /// </summary>
        /// <param name="dgv">DataGridView</param>
        private static void SetDataGridViewColor(DataGridView dgv)
        {
            //実装が必要な場合に追記ください。
        }

        /// <summary>
        /// Spreadの色を取得します。
        /// </summary>
        /// <param name="sheet">FpSpread</param>
        private static void SetSpreadColor(FpSpread sheet)
        {
            //選択行の背景色を設定
            sheet.ActiveSheet.SelectionBackColor = FrameUtilites.GetFrameGridSelectionBackColor();

            //SheetView変数定義
            SheetView sheet0 = sheet.Sheets[0];

            //列ヘッダーが存在する場合
            if (sheet0.ColumnHeaderVisible)
            {
                for (int i = 0; i < sheet0.ColumnCount; i++)
                {
                    // ヘッダーの背景色を設定
                    sheet0.ColumnHeader.Cells[0, i].BackColor = FrameUtilites.GetFrameGridHeaderBackColor();
                }
            }

            //列ヘッダーが存在する場合
            if (sheet0.RowHeaderVisible)
            {
                for (int i = 0; i < sheet0.RowCount; i++)
                {
                    // ヘッダーの背景色を設定
                    sheet0.RowHeader.Cells[i,0].BackColor = FrameUtilites.GetFrameGridHeaderBackColor();
                }
            }

            //シートコーナーが存在する場合
            if (sheet0.ColumnHeaderVisible && sheet0.RowHeaderVisible)
            {
                sheet0.SheetCorner.Cells[0, 0].BackColor = FrameUtilites.GetFrameGridHeaderBackColor();
            }
        }

        /// <summary>
        /// GcMultiRowの色を取得します。
        /// </summary>
        /// <param name="multiRow">GcMultiRow</param>
        private static void SetGcMultiRowColor(GcMultiRow multiRow)
        {
            //列ヘッダー背景色の設定
            ColumnHeaderSectionCollection secCol = multiRow.Template.ColumnHeaders;
            if (secCol != null && 0 < secCol.Count)
            {
                CellCollection cellCol = secCol[0].Cells;
                if (cellCol != null && 0 < cellCol.Count)
                {
                    for (int i = 0; i < cellCol.Count; i++)
                    {
                        cellCol[i].Style.BackColor = FrameUtilites.GetFrameGridHeaderBackColor();
                    }
                }
            }

            //行ヘッダー背景色の設定
            CellCollection rowCellCol = multiRow.Template.Row.Cells;
            if (rowCellCol != null && 0 < rowCellCol.Count)
            {
                for (int i = 0; i < rowCellCol.Count; i++)
                {
                    if (rowCellCol[i].GetType().Equals(typeof(RowHeaderCell)))
                    {
                        rowCellCol[i].Style.BackColor = FrameUtilites.GetFrameGridHeaderBackColor();
                    }
                }
            }
        }

        /// <summary>
        /// タイトル背景色を取得します。
        /// </summary>
        /// <returns>タイトル背景色</returns>
        public static Color GetFrameTitleBackColor()
        {
            //デフォルトタイトル背景色を取得
            Color rt_color = FrameUtilites.GetColorByRgbString(DefaultProperty.FRAME_TITLE_DEFAULT_BACKCOLOR);

            string configColor = string.Empty;

            if (UserProperty.GetInstance().SystemSettingsInfo == null)
            {
                configColor = ConfigurationManager.AppSettings["FrameTitleBackColor"];

                if (configColor != null && 0 < configColor.Length)
                {
                    //顧客タイトル背景色を取得
                    rt_color = FrameUtilites.GetColorByRgbString(configColor, rt_color);
                }

                return rt_color;
            }

            //顧客タイトル背景色取得
            string strRgb = UserProperty.GetInstance().SystemSettingsInfo.FrameTitleBackColor;

            //顧客タイトル背景色が存在する場合
            if (strRgb != null && 0 < strRgb.Length)
            {
                //顧客タイトル背景色を取得
                rt_color = FrameUtilites.GetColorByRgbString(strRgb, rt_color);
            }

            return rt_color;
        }

        /// <summary>
        /// フッター背景色を取得します。
        /// </summary>
        /// <returns>フッター背景色</returns>
        public static Color GetFrameFooterBackColor()
        {
            //デフォルトフッター背景色を取得
            Color rt_color = FrameUtilites.GetColorByRgbString(DefaultProperty.FRAME_FOOTER_DEFAULT_BACKCOLOR);

            if (UserProperty.GetInstance().SystemSettingsInfo == null)
            {
                return rt_color;
            }

            //顧客フッター背景色取得
            string strRgb = UserProperty.GetInstance().SystemSettingsInfo.FrameFooterBackColor;

            //顧客フッター背景色が存在する場合
            if (strRgb != null && 0 < strRgb.Length)
            {
                //顧客フッター背景色を取得
                rt_color = FrameUtilites.GetColorByRgbString(strRgb, rt_color);
            }

            return rt_color;
        }

        /// <summary>
        /// 表ヘッダー背景色を取得します。
        /// </summary>
        /// <returns>表ヘッダー背景色</returns>
        public static Color GetFrameGridHeaderBackColor()
        {
            //デフォルト表ヘッダー背景色を取得
            Color rt_color = FrameUtilites.GetColorByRgbString(DefaultProperty.FRAME_GRIDHEADER_DEFAULT_BACKCOLOR);

            if (UserProperty.GetInstance().SystemSettingsInfo == null)
            {
                return rt_color;
            }

            //顧客表ヘッダー背景色取得
            string strRgb = UserProperty.GetInstance().SystemSettingsInfo.FrameGridHeaderBackColor;

            //顧客表ヘッダー背景色が存在する場合
            if (strRgb != null && 0 < strRgb.Length)
            {
                //顧客表ヘッダー背景色を取得
                rt_color = FrameUtilites.GetColorByRgbString(strRgb, rt_color);
            }

            return rt_color;
        }

        /// <summary>
        /// 表選択行背景色を取得します。
        /// </summary>
        /// <returns>表選択行背景色</returns>
        public static Color GetFrameGridSelectionBackColor()
        {
            //デフォルト表選択行背景色を取得
            Color rt_color = FrameUtilites.GetColorByRgbString(DefaultProperty.FRAME_GRIDSELECTION_DEFAULT_BACKCOLOR);

            if (UserProperty.GetInstance().SystemSettingsInfo == null)
            {
                return rt_color;
            }

            //顧客表選択行背景色取得
            string strRgb = UserProperty.GetInstance().SystemSettingsInfo.FrameGridSelectionBackColor;

            //顧客表選択行背景色が存在する場合
            if (strRgb != null && 0 < strRgb.Length)
            {
                //顧客表選択行背景色を取得
                rt_color = FrameUtilites.GetColorByRgbString(strRgb, rt_color);
            }

            return rt_color;
        }

        /// <summary>
        /// 指定したコントロール配下のすべてのコントロールを取得します。
        /// </summary>
        /// <returns>タイトル背景色</returns>
        public static Control[] GetGridControls(Control top)
        {
            ArrayList buf = new ArrayList();
            try
            {
                foreach (Control c in top.Controls)
                {
                    if (c.GetType().Equals(typeof(DataGridView))
                        || c.GetType().Equals(typeof(GcMultiRow))
                        || c.GetType().Equals(typeof(FpSpread)))
                    {
                        buf.Add(c);
                    }
                    else
                    {
                        buf.AddRange(FrameUtilites.GetGridControls(c));
                    }
                }
            }
            catch
            {
                //何もしない
            }
            return (Control[])buf.ToArray(typeof(Control));
        }

        /// <summary>
        /// カンマ区切りのRGBカラー文字列から色オブジェクトを取得します。
        /// </summary>
        /// <param name="strRgb">RGBカラー文字列</param>
        /// <param name="defaultColor">デフォルトカラー</param>
        /// <returns>色オブジェクト</returns>
        public static Color GetColorByRgbString(string strRgb, Color defaultColor = new Color())
        {
            Color rt_color = defaultColor;

            if (strRgb != null && 0 < strRgb.Length && 1 < FrameUtilites.CountChar(strRgb, ','))
            {
                string[] arrRgb = strRgb.Split(',');
                rt_color = Color.FromArgb(Convert.ToInt32(arrRgb[0]),
                    Convert.ToInt32(arrRgb[1]), Convert.ToInt32(arrRgb[2]));
            }
            else
            {
                try
                {
                    rt_color = ColorTranslator.FromHtml(strRgb);
                }
                catch
                {
                    //何もしない
                }
            }

            return rt_color;
        }

        /// <summary>
        /// 文字の出現回数をカウントします。
        /// </summary>
        /// <param name="s">対象文字列</param>
        /// <param name="c">対象文字</param>
        /// <returns>出現回数</returns>
        public static int CountChar(string s, char c)
        {
            return s.Length - s.Replace(c.ToString(), "").Length;
        }

        /// <summary>
        /// デバッグログに出力するタイトルと、内容を指定してデバッグログを出力します。
        /// デバッグログは非ローミングユーザのApplication Dataフォルダに出力されます。
        /// </summary>
        /// <returns>配給一覧最大行数</returns>
        public static decimal GetMaxRowNumTankList()
        {
            //戻り値
            decimal rt_val = FrameUtilites.COL_MAXROW_TANKLIST;

            //ログレベルを取得する
            decimal val = Convert.ToDecimal(ConfigurationManager.AppSettings["MaxRowNumTankList"]);

            if (0 < val)
            {
                rt_val = val;
            }

            return rt_val;
        }

        /// <summary>
        /// フィルタ情報を保持します。
        /// </summary>
        public static string[] GetFilterInfo(SheetView sheet)
        {
            // フィルタ設定の保存
            string[] filterStrings = new String[sheet.ColumnCount];
            for (var i = 0; i < sheet.ColumnCount; i++)
            {
                filterStrings[i] = sheet.RowFilter.GetColumnFilterBy(i);
            }

            return filterStrings;
        }

        /// <summary>
        /// ソート情報を保持します。
        /// </summary>
        public static FarPoint.Win.Spread.Model.SortIndicator[]
            GetSortInfo(SheetView sheet)
        {
            // ソート設定の保存
            FarPoint.Win.Spread.Model.SortIndicator[] sortIndicators = new FarPoint.Win.Spread.Model.SortIndicator[sheet.ColumnCount];
            for (var i = 0; i < sheet.ColumnCount; i++)
            {
                sortIndicators[i] = sheet.Columns[i].SortIndicator;
            }

            return sortIndicators;
        }

        /// <summary>
        /// フィルタ情報を復元します。
        /// </summary>
        public static void SetFilterInfo(SheetView sheet, string[] filterStrings,
            FarPoint.Win.Spread.HideRowFilter hrFilter)
        {
            try
            {
                // 保存したフィルタ設定の復元
                sheet.RowFilter = hrFilter;
                foreach (FarPoint.Win.Spread.FilterColumnDefinition fcd in sheet.RowFilter.ColumnDefinitions)
                {
                    if (fcd.Filters.Count > 0)
                    {
                        for (var i = 0; i < fcd.Filters.Count; i++)
                        {
                            if (fcd.Filters[i].DisplayName == filterStrings[fcd.ColumnIndex])
                            {
                                sheet.AutoFilterColumn(fcd.ColumnIndex, fcd.Filters[i].DisplayName, i);
                                break;
                            }
                        }
                    }
                }
            }
            catch
            {
                //フィルター情報に一致するデータがない場合は、そのフィルタを解除
                //フィルター・ソートをクリアする
                sheet.RowFilter.ResetFilter();
                FrameUtilites.ResetSortInfo(sheet);
            }
        }

        /// <summary>
        /// ソート情報を復元します。
        /// </summary>
        public static void SetSortInfo(SheetView sheet, FarPoint.Win.Spread.Model.SortIndicator[] sortIndicators)
        {
            // 保存したソート設定の復元
            for (var i = 0; i < sheet.ColumnCount; i++)
            {
                if (sortIndicators[i] == FarPoint.Win.Spread.Model.SortIndicator.Ascending)
                {
                    sheet.AutoSortColumn(i, true);
                }
                else if (sortIndicators[i] == FarPoint.Win.Spread.Model.SortIndicator.Descending)
                {
                    sheet.AutoSortColumn(i, false);
                }
            }
        }

        /// <summary>
        /// ソート情報をリセットします。
        /// </summary>
        public static void ResetSortInfo(SheetView sheet)
        {
            for (var i = 0; i < sheet.ColumnCount; i++)
            {
                sheet.Columns[i].ResetSortIndicator();
            }
        }

        /// <summary>
        /// CSVの項目の必須チェックを行う。
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="errList"></param>
        /// <returns></returns>
        public static void isNullCheckCsvColumn(List<string> line, int rowIndex, int columnIndex,  List<string> errList)
        {
            string val = line[columnIndex].Replace("/", string.Empty);

            //項目が空白の場合はDateTime.MinValueを返す
            if (string.IsNullOrWhiteSpace(val))
            {
                errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値が設定されておりません。", columnIndex + 1, rowIndex + 1));

                //throw new CanRetryException(
                //    string.Format("CSVファイルの{0}列目、{1}行目の値が設定されておりません。", columnIndex + 1, rowIndex + 1),
                //    MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// CSVの項目を日付に変換する
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="errList"></param>
        /// <returns></returns>
        public static DateTime ConvertToDatetimeFromString(List<string> line, int rowIndex, int columnIndex, List<string> errList)
        {
            DateTime value = DateTime.MinValue;

            string val = line[columnIndex].Replace("/", string.Empty);

            //項目が空白の場合はDateTime.MinValueを返す
            if (val == string.Empty)
                return DateTime.MinValue;

            string content = val.Substring(0, 4) + "/" + val.Substring(4, 2) + "/" + val.Substring(6, 2);

            if (!DateTime.TryParse(content, out value))
            {
                errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値を日付に変換できませんでした。", columnIndex + 1, rowIndex + 1));

                //throw new CanRetryException(
                //    string.Format("CSVファイルの{0}列目、{1}行目の値を日付に変換できませんでした。", columnIndex + 1, rowIndex + 1),
                //    MessageBoxIcon.Error);
            }

            return value;
        }

        /// <summary>
        /// CSVの項目を数値に変換する
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="errList"></param>
        /// <param name="allowEmpty">True:空白を許す(空白の場合は0で返す)</param>
        /// <returns></returns>
        public static decimal ConvertToDecimalFromString(List<string> line, int rowIndex, int columnIndex, List<string> errList, bool allowEmpty = false)
        {
            decimal value = 0;

            string content = line[columnIndex];

            //引数「空白許可」=Trueかつ項目が空白の場合は0を返す
            if (allowEmpty && string.IsNullOrWhiteSpace(content))
                return 0;

            if (!decimal.TryParse(content, out value))
            {
                errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値を数値に変換できませんでした。", columnIndex + 1, rowIndex + 1));

                //throw new CanRetryException(
                //    string.Format("CSVファイルの{0}列目、{1}行目の値を数値に変換できませんでした。", columnIndex + 1, rowIndex + 1),
                //    MessageBoxIcon.Error);
            }

            return value;
        }


        /// <summary>
        /// CSVの項目を数値に変換する
        /// </summary>
        /// <param name="line"></param>
        /// <param name="rowIndex"></param>
        /// <param name="columnIndex"></param>
        /// <param name="errList"></param>
        /// <returns></returns>
        public static int ConvertToIntFromString(List<string> line, int rowIndex, int columnIndex, List<string> errList)
        {
            int value = 0;

            string content = line[columnIndex].Replace("/", string.Empty);

            if (content == string.Empty)
                return 0;

            if (!int.TryParse(content, out value))
            {
                errList.Add(string.Format("CSVファイルの{0}列目、{1}行目の値を数値に変換できませんでした。", columnIndex + 1, rowIndex + 1));

                //throw new CanRetryException(
                //    string.Format("CSVファイルの{0}列目、{1}行目の値を数値に変換できませんでした。", columnIndex + 1, rowIndex + 1),
                //    MessageBoxIcon.Error);
            }

            return value;
        }

        /// <summary> 
        /// エラーファイルを出力します。
        /// </summary> 
        /// <param name="stringArrayList">出力したい値の入ったリスト</param> 
        /// <param name="filePath">ファイルの保存先</param> 
        /// <remarks></remarks> 
        public static string LinesToErrText(List<string> stringArrayList, string filePath)
        {
            string result = string.Empty;

            try
            {
                //保存先のCSVファイルのパス 
                string csvPath = filePath;
                //CSVファイルに書き込むときに使うEncoding 
                System.Text.Encoding enc = System.Text.Encoding.GetEncoding("Shift_JIS");

                //開く 
                System.IO.StreamWriter sr = new System.IO.StreamWriter(csvPath, false, enc);

                int colCount = stringArrayList[0].Length;
                int lastColIndex = colCount - 1;

                //レコードを書き込む 
                foreach (string field in stringArrayList)
                {
                    //出力
                    sr.Write(field);
                    //改行する 
                    sr.Write("\r\n");
                }

                result = sr.ToString();

                //閉じる 
                sr.Close();
            }
            catch (Exception)
            {
            }

            return result;
        }
    }
}
