using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.ComLib
{
    public static class NullSafeValueHelper
    {
        /// <summary>
        /// NullSafeなアクセスを提供します
        /// オブジェクトがnullの場合は指定した nullValue を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PT NullSafe<T, PT>(this T obj, Func<T, PT> func, PT nullValue)
        {
            return GetValueOrDefault(obj, func, nullValue);
        }

        /// <summary>
        /// NullSafeなアクセスを提供します
        /// オブジェクトがnullの場合はデフォルト値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PT NullSafe<T, PT>(this T obj, Func<T, PT> func)
        {
            return GetValueOrDefault(obj, func);
        }

        /// <summary>
        /// NullSafeなアクセスを提供します
        /// オブジェクトがnullの場合は string.Empty を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string NullSafe<T>(this T obj, Func<T, string> func)
        {
            return GetValueOrEmpty(obj, func);
        }

        /// <summary>
        /// 指定したオブジェクトをNullSafeWrapperにラップして返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static NullSafeWrapper<T> NullSafeWrap<T>(T obj)
        {
            return new NullSafeWrapper<T>(obj);
        }


        /// <summary>
        /// オブジェクトとそのオブジェクトから値を返すファンクションを指定して値を取得します。
        /// 指定したオブジェクトがnullの場合はデフォルト値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PT GetValueOrDefault<T, PT>(T obj, Func<T, PT> func)
        {
            return GetValueOrDefault(obj, func, default(PT));
        }

        /// <summary>
        /// オブジェクトとそのオブジェクトから値を返すファンクションを指定して値を取得します。
        /// 指定したオブジェクトがnullの場合は、デフォルト値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <param name="nullValue">null時の値</param>
        /// <returns></returns>
        public static PT GetValueOrDefault<T, PT>(T obj, Func<T, PT> func, PT defaultValue)
        {
            if (obj == null)
            {
                return defaultValue;
            }

            return func(obj);
        }

        /// <summary>
        /// オブジェクトとそのオブジェクトから値を返すファンクションを指定して値を取得します。
        /// 指定したオブジェクトがnullの場合はデフォルト値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="PT"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static string GetValueOrEmpty<T>(T obj, Func<T, string> func)
        {
            if (obj == null)
            {
                return string.Empty;
            }

            return func(obj);
        }

        /// <summary>
        /// オブジェクトとそのオブジェクトから値を返すファンクションを指定して値をNullable型で取得します。戻り値の型は構造体のみ指定できます。
        /// 指定したオブジェクトがnullの場合はNullを返します。
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public static PT? GetValueOrNull<T, PT>(this T obj, Func<T, PT> func) where PT : struct
        {
            if (obj == null)
            {
                return null;
            }

            return func(obj);
        }
    }
}
