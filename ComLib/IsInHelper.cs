using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// IsInメソッドを提供します。
    /// </summary>
    public static class IsInHelper
    {
        /// <summary>
        /// 指定した値のいずれかに合致するかどうかを返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsIn<T>(this T source, params T[] values)
        {
            return source.IsIn((IEnumerable<T>)values);
        }

        /// <summary>
        /// 指定した値のいずれかに合致するかどうかを返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        public static bool IsIn<T>(this T source, IEnumerable<T> values)
        {
            return values.Contains(source);
        }
    }
}
