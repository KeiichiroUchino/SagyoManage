using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// 値型全般に適用される拡張メソッドを提供します。 
    /// </summary>
    public static class ValueTypeExtensions
    {
        /// <summary>
        /// デフォルト値であればnullに変換します。
        /// 戻り値はNull許容型になります。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T? NullIfDefault<T>(this T value) where T : struct
        {
            if (default(T).Equals(value))
            {
                return null;
            }
            else
            {
                return value;
            }
        }

    }
}
