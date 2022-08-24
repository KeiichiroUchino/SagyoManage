using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.Text
{
    public static class CsvHelper
    {
        /// <summary>
        /// 指定したデータをCSV形式のテキストに変換して返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">ダンプ対象のデータ</param>
        public static List<string[]> CreateTextAsLines<T>(IEnumerable<T> source)
        {
            List<string[]> textData = new List<string[]>();

            //隠し列を除いたプロパティ
            var properties_export =
                GetPropertiesOf(typeof(T))
                .Where(prop =>
                {
                    var field = prop.GetCustomAttribute<FieldAttribute>();
                    return (field != null && !field.Disable);
                }).ToArray();

            //並び替える
            var properties = Order(properties_export).ToArray();

            var getDelegates = CreatePropertyGetDelegates(typeof(T));

            #region タイトル行

            List<string> titles = new List<string>();
            foreach (var property in properties)
            {
                var fieldAttr = property.GetCustomAttribute<FieldAttribute>();
                titles.Add((fieldAttr != null) ? fieldAttr.Text : property.Name);
            }
            textData.Add(titles.ToArray());

            #endregion

            #region データ行
            foreach (var item in source)
            {
                List<string> fieldTexts = new List<string>();

                foreach (var property in properties)
                {
                    var fieldAttr = property.GetCustomAttribute<FieldAttribute>();

                    object fieldValue = getDelegates[property.Name](item);
                    string fieldText = ObjToString(fieldValue, fieldAttr);

                    fieldTexts.Add(fieldText);
                }

                textData.Add(fieldTexts.ToArray());
            }
            #endregion

            return textData;
        }
        /// <summary>
        /// 指定したデータをCSV形式のテキストに変換して返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">ダンプ対象のデータ</param>
        public static List<string[]> CreateTextAsLinesEx<T>(IEnumerable<T> source, bool titleFlag = true)
        {
            List<string[]> textData = new List<string[]>();

            //隠し列を除いたプロパティ
            var properties_export =
                GetPropertiesOf(typeof(T))
                .Where(prop =>
                {
                    var field = prop.GetCustomAttribute<FieldAttribute>();
                    return (field != null && !field.Disable);
                }).ToArray();

            //並び替える
            var properties = Order(properties_export).ToArray();

            var getDelegates = CreatePropertyGetDelegates(typeof(T));

            #region タイトル行

            if (titleFlag)
            {
                List<string> titles = new List<string>();
                foreach (var property in properties)
                {
                    var fieldAttr = property.GetCustomAttribute<FieldAttribute>();
                    titles.Add((fieldAttr != null) ? fieldAttr.Text : property.Name);
                }
                textData.Add(titles.ToArray());
            }

            #endregion

            #region データ行
            foreach (var item in source)
            {
                List<string> fieldTexts = new List<string>();

                foreach (var property in properties)
                {
                    var fieldAttr = property.GetCustomAttribute<FieldAttribute>();

                    object fieldValue = getDelegates[property.Name](item);
                    string fieldText = ObjToString(fieldValue, fieldAttr);

                    fieldTexts.Add(fieldText);
                }

                textData.Add(fieldTexts.ToArray());
            }
            #endregion

            return textData;
        }

        //***FrameLibからコピーかつ、テキストファイルにAppendできるようにテキストを返すようにする
        /// <summary> 
        /// 文字列の配列を型とするジェネリックのList型の値とファイルの保存先を指定して、 
        /// CSVファイルを出力します。 
        /// １件目をヘッダとはみなしません。 
        /// </summary> 
        /// <param name="stringArrayList">出力したい値の入ったリスト</param> 
        /// <param name="filePath">ファイルの保存先</param> 
        /// <remarks></remarks> 
        public static string LinesToCsvText(List<string[]> stringArrayList)
        {
            string result = string.Empty;

            try
            {
                //開く 
                System.IO.StringWriter sr = new System.IO.StringWriter();

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

                result = sr.ToString();

                //閉じる 
                sr.Close();
            }
            catch (Exception)
            {
            }

            return result;
        }


        /// <summary>
        /// ColumnOrderの設定を使用して並び替えを行います。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        private static IEnumerable<PropertyInfo> Order(this PropertyInfo[] properties)
        {
            List<PropertySortInfo> propertySortInfos = new List<PropertySortInfo>();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var fieldAttribute = property.GetCustomAttribute<FieldAttribute>();

                //ソートIndexを取得。属性が設定されていない場合は-1
                int sortIndex = (fieldAttribute != null) ? fieldAttribute.Order : -1;

                propertySortInfos.Add(
                    new PropertySortInfo()
                    {
                        PropertyInfoIndex = i,
                        SortIndex = sortIndex,
                        PropertyInfo = property,
                    });
            }

            return
                propertySortInfos
                    .OrderBy(element => element.SortIndex)
                    .ThenBy(element => element.PropertyInfoIndex)
                    .Select(element => element.PropertyInfo);
        }


        private static string ObjToString(object obj, FieldAttribute fieldAttribute)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            if (fieldAttribute == null || string.IsNullOrWhiteSpace(fieldAttribute.Format))
            {
                return obj.ToString();
            }

            try
            {
                //仕方ないので dynamic で処理する。
                return ((dynamic)obj).ToString(fieldAttribute.Format);
            }
            catch (Exception)
            {
                return obj.ToString();
            }
        }


        /// <summary>
        /// 指定したTypeオブジェクトの持つプロパティを返します。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static PropertyInfo[] GetPropertiesOf(Type t)
        {
            return t.GetProperties(BindingFlags.Instance | BindingFlags.Public);
        }

        /// <summary>
        /// 指定したTypeオブジェクトの持つプロパティの値を返すデリゲートを生成します。Key はプロパティ名です。
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static Dictionary<string, Func<object, object>> CreatePropertyGetDelegates(Type t)
        {
            return
                GetPropertiesOf(t)
                .ToDictionary(
                    element => element.Name,
                    element => Utility.CreateGetDelegate(t, element.Name));
        }

        /// <summary>
        /// ヘッダがついたCSVデータを指定したファイルに追記します。
        /// ファイルが存在している場合はヘッダは書き込みません。
        /// ファイルが存在しない場合はヘッダをつけて追記します。
        /// 自動で1要素目をヘッダとみなします。ヘッダがなしやヘッダが複数要素に渡って展開されるCSVファイルは指定しないでください。
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="lines"></param>
        /// <param name="encoding"></param>
        public static void AppendLinesWithHeaderToFile(string filePath, List<string[]> lines, Encoding encoding)
        {
            if (lines.Count == 0)
            {
                return;
            }

            //***ファイルが存在しない場合はヘッダを書き込む
            if (File.Exists(filePath))
            {
                using (FileStream stream = File.Open(filePath, FileMode.Append))
                using (StreamWriter writer = new StreamWriter(stream, encoding))
                {
                    string csvText = CsvHelper.LinesToCsvText(lines.Skip(1).ToList()); //ヘッダを除いて書き込み
                    writer.Write(csvText);
                }
            }
            else
            {
                using (FileStream stream = File.Create(filePath))
                using (StreamWriter writer = new StreamWriter(stream, encoding))
                {
                    string csvText = CsvHelper.LinesToCsvText(lines);
                    writer.Write(csvText);
                }
            }
        }


        /// <summary>
        /// プロパティの並び替えに必要な情報
        /// </summary>
        private struct PropertySortInfo
        {
            /// <summary>
            /// PropertyInfo配列上のインデックス
            /// </summary>
            public int PropertyInfoIndex { get; set; }
            /// <summary>
            /// ソートインデックス
            /// </summary>
            public int SortIndex { get; set; }
            /// <summary>
            /// プロパティ情報
            /// </summary>
            public PropertyInfo PropertyInfo { get; set; }
        }

        #region CSV取込用
        
        /// <summary>
        /// CSVをArrayListに変換
        /// </summary>
        /// <param name="csvText">CSVの内容が入ったString</param>
        /// <returns>変換結果のArrayList</returns>
        public static System.Collections.ArrayList CsvToArrayList(string csvText)
        {
            System.Collections.ArrayList csvRecords =
                new System.Collections.ArrayList();

            //前後の改行を削除しておく
            csvText = csvText.Trim(new char[] { '\r', '\n' });

            //一行取り出すための正規表現
            System.Text.RegularExpressions.Regex regLine =
                new System.Text.RegularExpressions.Regex(
                "^.*(?:\\n|$)",
                System.Text.RegularExpressions.RegexOptions.Multiline);

            //1行のCSVから各フィールドを取得するための正規表現
            System.Text.RegularExpressions.Regex regCsv =
                new System.Text.RegularExpressions.Regex(
                "\\s*(\"(?:[^\"]|\"\")*\"|[^,]*)\\s*,",
                System.Text.RegularExpressions.RegexOptions.None);

            System.Text.RegularExpressions.Match mLine = regLine.Match(csvText);
            while (mLine.Success)
            {
                //一行取り出す
                string line = mLine.Value;
                //改行記号が"で囲まれているか調べる
                while ((CountString(line, "\"") % 2) == 1)
                {
                    mLine = mLine.NextMatch();
                    if (!mLine.Success)
                    {
                        throw new ApplicationException("不正なCSV");
                    }
                    line += mLine.Value;
                }
                //行の最後の改行記号を削除
                line = line.TrimEnd(new char[] { '\r', '\n' });
                //最後に「,」をつける
                line += ",";

                //1つの行からフィールドを取り出す
                System.Collections.ArrayList csvFields =
                    new System.Collections.ArrayList();
                System.Text.RegularExpressions.Match m = regCsv.Match(line);
                while (m.Success)
                {
                    string field = m.Groups[1].Value;
                    //前後の空白を削除
                    field = field.Trim();
                    //"で囲まれている時
                    if (field.StartsWith("\"") && field.EndsWith("\""))
                    {
                        //前後の"を取る
                        field = field.Substring(1, field.Length - 2);
                        //「""」を「"」にする
                        field = field.Replace("\"\"", "\"");
                    }
                    csvFields.Add(field);
                    m = m.NextMatch();
                }

                csvFields.TrimToSize();
                csvRecords.Add(csvFields);

                mLine = mLine.NextMatch();
            }

            csvRecords.TrimToSize();
            return csvRecords;
        }

        /// <summary>
        /// 指定された文字列内にある文字列が幾つあるか数える
        /// </summary>
        /// <param name="strInput">strFindが幾つあるか数える文字列</param>
        /// <param name="strFind">数える文字列</param>
        /// <returns>strInput内にstrFindが幾つあったか</returns>
        public static int CountString(string strInput, string strFind)
        {
            int foundCount = 0;
            int sPos = strInput.IndexOf(strFind);
            while (sPos > -1)
            {
                foundCount++;
                sPos = strInput.IndexOf(strFind, sPos + 1);
            }

            return foundCount;
        }

        #endregion
    }
}
