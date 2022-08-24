using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    //***うまく分類できないメソッドはここに書く...

    /// <summary>
    /// 共通メソッドを定義します。
    /// </summary>
    public static class Utility
    {
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
        /// 全項目にダブルクォーテーションをつける
        /// </summary> 
        /// <param name="stringArrayList">出力したい値の入ったリスト</param> 
        /// <param name="filePath">ファイルの保存先</param> 
        /// <remarks></remarks> 
        public static void CreateCSVDataByStringArrQuoteList(List<string[]> stringArrayList, string filePath)
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

                        field = "\"" + field + "\"";

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
        /// 指定したdecimal型の値を最低限必要な桁数の値に変換します。
        /// たとえば、 10.1000 は 10.1 に変換されます。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal ToMinDecimalDigitsValue(decimal val)
        {
            var intergerPart = Math.Truncate(val);
            var decimalPart = System.Convert.ToDecimal((val % 1).ToString("0.############################"));

            return intergerPart + decimalPart;
        }

        // *** s.arimura 
        // レポート高速化のためにIList<T> → DataTable の変換をしなければならない。
        // その変換時にリフレクションを使用する必要があるが、このメソッドの戻り値を使用すれば
        // 一般的なPropertyInfoを使用した方法と比較して、速度において約10倍弱のパフォーマンスが見込める。
        // http://neue.cc/2011/04/20_317.html から拝借。
        /// <summary>
        /// 型情報とメンバ名を指定して、この型情報の型のもつプロパティ及びフィールドからリフレクションによって
        /// 値を取得するためのデリゲートを返します。
        /// </summary>
        /// <param name="type">値を取得する対象のオブジェクトの型</param>
        /// <param name="memberName">メンバ名</param>
        /// <returns>値を取得するためのデリゲート</returns>
        public static Func<object, object> CreateGetDelegate(Type type, string memberName)
        {
            //引数のパラメータ
            var target = Expression.Parameter(typeof(object), "target");
            //Lambda本体
            var lambda = Expression.Lambda<Func<object, object>>(
            Expression.Convert(
                Expression.PropertyOrField(
                    Expression.Convert(
                        target
                        , type)
                    , memberName)
                , typeof(object))
            , target);

            return lambda.Compile();
        }

        // *** s.arimura
        // http://neue.cc/2011/04/20_317.html から拝借して式木にしたやつ（本家はFuncデリゲート）
        /// <summary>
        /// 型情報とメンバ名を指定して、この型のプロパティおよびフィールドにリフレクションで
        /// 値を設定するためのデリゲートを表す式木を返します。
        /// </summary>
        /// <param name="type">値を設定する対象のオブジェクトの型</param>
        /// <param name="memberName">メンバ名</param>
        /// <returns>値を設定するためのデリゲートを表す式木</returns>
        public static Expression<Action<object, object>> CreateSetDelegateExpression(Type type, string memberName)
        {
            var target = Expression.Parameter(typeof(object), "target");
            var value = Expression.Parameter(typeof(object), "value");

            var left =
                Expression.PropertyOrField(
                    Expression.Convert(target, type), memberName);
            var right = Expression.Convert(value, left.Type);

            var lambda = Expression.Lambda<Action<object, object>>(
                Expression.Assign(left, right), target, value);

            return lambda;
        }

        /// <summary>
        /// メンバにアクセスする関数を表す式木から、アクセスするメンバの名前を取得して返します。
        /// </summary>
        /// <typeparam name="T">メンバを持つクラスの型</typeparam>
        /// <typeparam name="PT">メンバの型</typeparam>
        /// <param name="propFunc">メンバにアクセスする関数を表す式木</param>
        /// <returns>メンバ名</returns>
        public static string GetMemberName<T, PT>(Expression<Func<T, PT>> propFunc)
        {
            return ((MemberExpression)propFunc.Body).Member.Name;
        }

        /// <summary>
        /// 構造体の値をNull許容型の値に変換します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Nullable<T> ToNullable<T>(T value) where T : struct
        {
            return value;
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
        /// 文字列から指定した型への値に変換します。
        /// もし文字列がstring.IsNullOrWhiteSpace()メソッドでtrueが返却される場合は
        /// 指定した型のデフォルト値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        public static T ConvertFromStringIfStringValueIsNullOrWhiteSpaceThenDefaultValue<T>(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return default(T);
            }

            return (T)System.Convert.ChangeType(val, typeof(T));
        }

        /// <summary>
        ///  異なる型のオブジェクトに同じ名前のプロパティ値をセットします。
        /// </summary>
        public static class PropertyCopier
        {
            public static T CopyTo<T>(object src, T dest)
            {
                if (src == null || dest == null) return dest;
                var srcProperties = src.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
                var destProperties = dest.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite);
                var properties = srcProperties.Join(destProperties, p => new { p.Name, p.PropertyType }, p => new { p.Name, p.PropertyType }, (p1, p2) => new { p1, p2 });
                foreach (var property in properties)
                    property.p2.SetValue(dest, property.p1.GetValue(src));
                return dest;
            }
        }

        /// <summary>
        /// 指定したディレクトリとその中身を全て削除する
        /// </summary>
        public static void DeleteDirectory(string targetDirectoryPath)
        {
            if (!Directory.Exists(targetDirectoryPath))
            {
                return;
            }

            //ディレクトリ以外の全ファイルを削除
            string[] filePaths = Directory.GetFiles(targetDirectoryPath);
            foreach (string filePath in filePaths)
            {
                File.SetAttributes(filePath, FileAttributes.Normal);
                File.Delete(filePath);
            }

            //ディレクトリの中のディレクトリも再帰的に削除
            string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
            foreach (string directoryPath in directoryPaths)
            {
                DeleteDirectory(directoryPath);
            }

            //中が空になったらディレクトリ自身も削除
            Directory.Delete(targetDirectoryPath, false);
        }
    }
}
