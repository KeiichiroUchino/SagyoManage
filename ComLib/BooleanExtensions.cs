using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// Boolean型の拡張メソッドを提供します。
    /// </summary>
    public static class BooleanExtensions
    {
        #region Null許容型

        /// <summary>
        /// 値が true かどうかを返します。
        /// nullの場合はfalseを返します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsTrue(this bool? source)
        {
            if (!source.HasValue)
                return false;

            return source.Value;
        }

        /// <summary>
        /// 値が false かどうかを返します。
        /// nullの場合はfalseを返します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsFalse(this bool? source)
        {
            if (!source.HasValue)
                return false;

            return !source.Value;
        }

        #endregion
    }
}
