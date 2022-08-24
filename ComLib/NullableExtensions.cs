using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// Null許容型の値の拡張メソッドを定義します。
    /// </summary>
    public static class NullableExtensions
    {
        /// <summary>
        /// 自身の型を引数および戻りにする関数を実行して結果を返します。
        /// オブジェクトがnullの場合は null を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="func"></param>
        /// <returns></returns>
        public static T? FuncOrNull<T>(this T? obj, Func<T, T> func) where T : struct
        {
            if (!obj.HasValue)
            {
                return null;
            }

            return func(obj.Value);
        }
    }
}
