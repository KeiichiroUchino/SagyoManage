using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// ICollectionインタフェースの拡張メソッド
    /// </summary>
    public static class ICollectionExtension
    {
        /// <summary>
        /// 指定したインデックスがコレクションに含まれるかどうかを返します。
        /// </summary>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static bool ContainsIndex(this ICollection list, int index)
        {
            if (index < 0)
            {
                throw new ArgumentException("indexは負数を指定できません。", "index");
            }

            return (index < list.Count);
        }
    }
}
