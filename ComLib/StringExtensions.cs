using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// Stringクラスの拡張メソッド
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// 指定したインデックスから始まる文字列を置き換えます。
        /// </summary>
        /// <param name="startIndex"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceBytesByIndex(this string source, int startIndex, string value, System.Text.Encoding encoding)
        {
            //***バイト配列に変換してリプレースする
            var valueByteCount = encoding.GetByteCount(value);
            var sourceBytes = encoding.GetBytes(source);
            ReplaceBytes(sourceBytes, startIndex, encoding.GetBytes(value));

            return encoding.GetString(sourceBytes);
        }

        /// <summary>
        /// バイト配列の値を指定したIndexから置き換えます。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <param name="replaceValues"></param>
        private static void ReplaceBytes(byte[] bytes, int startIndex, byte[] replaceValues)
        {
            int idx = startIndex;

            foreach (var value in replaceValues)
            {
                bytes[idx] = value;

                idx++;
            }
        }


        /// <summary>
        /// 指定した部分文字列がこの文字列内に存在するかどうかを示す値を返します。
        /// 大小を区別しません。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCase(this string source, string value)
        {
            return source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// 指定したいずれかの文字列を含んでいるかどうかを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static bool ContainsAny(this string str, params string[] param)
        {
            return param.Any(s => str.Contains(s));
        }

        /// <summary>
        /// 文字列がnullまたは空白文字だけで構成されているかを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 文字列の先頭からから指定した文字数分を切り出して返します。
        /// </summary>
        /// <param name="str">対象の文字列</param>
        /// <param name="len">文字数</param>
        /// <returns></returns>
        public static string Left(this string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, len);
        }

        /// <summary>
        /// 指定した文字数分を末尾から切り出して返します。
        /// </summary>
        /// <param name="str">対象の文字列</param>
        /// <param name="len">文字数</param>
        /// <returns></returns>
        public static string Right(this string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(str.Length - len, len);
        }

        /// <summary>
        /// 指定した文字から末尾の文字数を除いて返します。
        /// </summary>
        /// <param name="str">対象の文字列</param>
        /// <param name="len">除外する末尾の文字</param>
        /// <returns></returns>
        public static string CutRight(this string str, int len)
        {
            if (len < 0)
            {
                throw new ArgumentException("引数'len'は0以上でなければなりません。");
            }
            if (str == null)
            {
                throw new ArgumentNullException("str");
            }
            if (str.Length <= len)
            {
                return str;
            }
            return str.Substring(0, str.Length - len);
        }


        /// <summary>
        /// 開始と終了のインデックスの範囲を抜き出して返します。
        /// 文字列が指定した範囲を満たさない場合は範囲に含まれている部分を返します。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string Slice(this string str, int start, int end)
        {
            if (str == string.Empty)
                return string.Empty;

            if (start > end)
                throw new ArgumentException("startはendよりも大きくならないように指定してください。");

            if (start >= str.Length)
	            return string.Empty;
           
            //終了位置を補正する（終了が文字列を超えていれば文字列数に直す）
            int end_actual = (end >= str.Length) ? str.Length - 1 : end; 

            //開始～終了から長さを求める
            int len = (end_actual - start) + 1;

            return str.Substring(start, len);
        }

        /// <summary>
        /// 1から始まるインデックスを使用して、文字列から開始と終了範囲の部分を抜き出して返します。
        /// 文字列が指定した範囲を満たさない場合は範囲に含まれている部分を返します。
        /// </summary>
        /// <param name="str"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static string SliceOneBased(this string str, int start, int end)
        {
            return Slice(str, start - 1, end - 1);
        }

        /// <summary>
        /// Decimal型オブジェクトにConvertします。
        /// nullまたはEmtpyの場合は0を返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            return Convert.ToDecimal(str);
        }

        /// <summary>
        /// Decimal型オブジェクトにConvertします。
        /// nullまたはEmtpyの場合はnullを返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static decimal? ToDecimalOrNull(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return null;

            return Convert.ToDecimal(str);
        }

        /// <summary>
        /// Int32型オブジェクトにConvertします。
        /// nullまたはEmtpyの場合は0を返します。
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt32(this string str)
        {
            if (string.IsNullOrEmpty(str))
                return 0;

            return Convert.ToInt32(str);
        }

        /// <summary>
        /// 指定した部分文字列がこの文字列内に存在するかどうかを示す値を返します。
        /// 全角半角大小を区別しません。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsIgnoreCaseWidth(this string source, string value)
        {
            if (source == null)
            {
                return false;
            }

            System.Globalization.CompareInfo ci = System.Globalization.CultureInfo.CurrentCulture.CompareInfo;
            return
                ci.IndexOf(source, value,
                    System.Globalization.CompareOptions.IgnoreWidth | System.Globalization.CompareOptions.IgnoreCase) >= 0;

        }

        /// <summary>
        /// 指定したエンコードで指定したバイト数以下に文字列を切り出します。
        /// </summary>
        /// <param name="s">文字列</param>
        /// <param name="encoding">エンコーディング</param>
        /// <param name="maxByteCount">最大バイト数</param>
        /// <returns></returns>
        public static string LeftB(this string s, Encoding encoding, int maxByteCount)
        { 
            var bytes = encoding.GetBytes(s); 
            if (bytes.Length <= maxByteCount) return s; 
  
            var result = s.Substring(0, 
                encoding.GetString(bytes, 0, maxByteCount).Length); 
   
                while (encoding.GetByteCount(result) > maxByteCount) 
                { 
                    result = result.Substring(0, result.Length - 1); 
                } 
                return result; 
        }
    }
}
