using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// 文字列操作のヘルパ
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// エンコーディング
        /// </summary>
        /// <param name="val">分割対象</param>
        /// <param name="len">分割単位の長さ　バイトで指定する</param>
        /// <param name="resultMinSize">結果の配列において確保する最低の要素数　既定値はstring.Empty</param>
        /// <returns></returns>
        public static string[] DivideStringByLengthOnShiftJis(string val, int len, int resultMinSize = 0)
        {
            if (len == 0)
            {
                throw new ArgumentException("長さは1以上を指定してください。", "len");
            }

            List<string> list = new List<string>();

            //文字列残り
            string valZan = val;

            //文字がなくなるまで処理
            while (valZan != null && valZan.Length > 0)
            {
                //指定のバイト分切出す
                string partOfVal =SubstringShiftJIS(valZan, 1, len);
                list.Add(partOfVal);

                valZan = valZan.Substring(partOfVal.Length);
            }

            if (resultMinSize > 0)
            {
                //確保すべきサイズの現在のサイズの差を算出
                int dif = resultMinSize - list.Count;

                //差分を追加
                for (int i = 0; i < dif; i++)
                {
                    list.Add(string.Empty);
                }

            }

            return list.ToArray();
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
        /// 文字列配列の要素を連結してひとつの文字列を返します。
        /// 連結対象の要素が string.IsNullOrEmptyメソッドで true を返す場合はその要素は連結の対象になりません。
        /// </summary>
        /// <param name="separator">区切り記号として使用する文字列</param>
        /// <param name="values">連結対象の文字列</param>
        /// <returns></returns>
        public static string JoinExcludeNullOrEmptyValue(string separator, params string[] values)
        {
            //連結対象をフィルタ
            var valuesExcludeNullOrEmpty = values.Where(element => !string.IsNullOrEmpty(element));

            return string.Join(separator, valuesExcludeNullOrEmpty);
        }

        /// <summary>
        /// 指定した配列の要素を探索して、nullまたは、string.Emptyではない最初の要素を返します。
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static string Coalesce(params string[] values)
        {
            return values.Where(val => !string.IsNullOrEmpty(val)).FirstOrDefault();
        }

        /// <summary>
        /// 指定した部分文字列がこの文字列内に存在するかどうかを示す値を返します。
        /// 全角半角大小ひらがなカタカナを区別しません。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCaseWidthKanaType(this string source, string value)
        {
            if (source == null)
            {
                return false;
            }

            System.Globalization.CompareInfo ci = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
            return
                ci.IndexOf(source, value,
                    System.Globalization.CompareOptions.IgnoreWidth | System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreKanaType) >= 0;

        }

        /// <summary>
        /// 指定した文字列がこの文字列と等しいかを示す値を返します。
        /// 全角半角大小ひらがなカタカナを区別しません。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EqualIgnoreCaseWidthKanaType(this string source, string value)
        {
            if (source == null)
            {
                return false;
            }

            System.Globalization.CompareInfo ci = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
            return
                ci.Compare(source, value,
                    System.Globalization.CompareOptions.IgnoreWidth | System.Globalization.CompareOptions.IgnoreCase | System.Globalization.CompareOptions.IgnoreKanaType) == 0;

        }

        /// <summary>
        /// オブジェクト型を文字列で返します。
        /// </summary>
        /// <param name="value">オブジェクト</param>
        public static string ConvertToString(object value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                return value.ToString();
            }
        }
    }
}
