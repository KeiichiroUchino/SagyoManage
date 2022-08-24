using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// IEnumerableの拡張メソッドを定義します。
    /// </summary>
    public static class IEnumerableExtensions
    {
        /// <summary>
        /// 重複データを含むかどうかを返します。
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool HasDuplication<T>(this IEnumerable<T> source)
        {
            //※Distinctで重複を除いた件数が、元の件数と異なっていれば重複ありと判断する。
            return source.Count() != source.Distinct().Count();
        }

        /// <summary>
        /// 指定した条件に合致した要素を先頭からすべて削除します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="trimPredicate"></param>
        public static IEnumerable<T> TrimStart<T>(this IEnumerable<T> source, Func<T, bool> trimPredicate)
        {
            //条件に合致する限りはスキップする
            return source.SkipWhile(trimPredicate);
        }

        /// <summary>
        /// 指定した条件に合致した要素を末尾からすべて削除します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="trimPredicate"></param>
        public static IEnumerable<T> TrimEnd<T>(this IEnumerable<T> source, Func<T, bool> trimPredicate)
        {
            //反転
            return source.Reverse()
                //トリム
                .TrimStart(trimPredicate)
                //事前に反転したので元に戻す
                .Reverse();
        }

        /// <summary>
        /// 指定した条件がtrueの場合だけ反転処理をします。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static IEnumerable<T> ReverseIf<T>(this IEnumerable<T> source, bool condition)
        {
            if (condition)
            {
                return source.Reverse();
            }

            return source;
        }

        /// <summary>
        /// 指定したサイズに分割して、分割した塊を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static IEnumerable<IEnumerable<T>> Chunk<T>(this IEnumerable<T> source, int size)
        {
            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (size <= 0)
            {
                throw new ArgumentException("size は0より大きい値を指定してください");
            }

            var enumrator = source.GetEnumerator();

            while (true)
            {
                var result = GetElementsCount(enumrator, size);

                if (result.Any())
                {
                    yield return result;
                }
                else
                {
                    break;
                }
            }
        }

        /// <summary>
        /// 指定した数だけ要素を取得して、同時に列挙子を進めます。
        /// </summary>
        /// <param name="enumrator"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        private static IEnumerable<T> GetElementsCount<T>(IEnumerator<T> enumrator, int size)
        {
            if(!enumrator.MoveNext())
                return Enumerable.Empty<T>();

            List<T> list = new List<T>();

            do
            {
                list.Add(enumrator.Current);
                size--;
            } while (size > 0 && enumrator.MoveNext());

            return list;
        }

        #region IEnumrable<int>

        /// <summary>
        /// 欠番が存在するかどうかを返します。
        /// 要素がない場合は false を返します。
        /// 開始値を指定しない場合は1から始まる連番とします。
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startValue">何から始まる連番とするか。0 を指定した場合は、0から始まる連番（「0,1,2,3...」）と比較して欠番が存在するかをチェックします。 </param>
        /// <returns></returns>
        public static bool HasMissingNumber(this IEnumerable<int> source, int startValue = 1)
        {
            //重複を除いて昇順に並べる
            var sourceIEnumerator = source.Distinct().OrderBy(element => element).GetEnumerator();
            //要素なしは欠番なしとする
            if (!sourceIEnumerator.MoveNext())
            {
                return false;
            }

            //最初の要素が開始値と異なれば欠番とする
            if (sourceIEnumerator.Current != startValue)
            {
                return true;
            }


            //*** ここから下は２つ目以降の要素をループで回して、現在要素と前要素を比較して連番になっているかどうかをチェックする。
            //*** {1,2,3,5} の場合 
            //*** ループ1回目 → 現在要素：2 前要素：1 判定：OK
            //*** ループ2回目 → 現在要素：3 前要素：2 判定：OK
            //*** ループ3回目 → 現在要素：5 前要素：3 判定：NG

            //最初の要素を前回値とする
            int previouseValue = sourceIEnumerator.Current;

            while (sourceIEnumerator.MoveNext())
            {
                int currentValue = sourceIEnumerator.Current;

                //連番の関係が崩れているかどうか？
                if (previouseValue != (currentValue - 1))
                {
                    return true;
                }

                //前回値を入れ替える
                previouseValue = currentValue;
            }

            return false;
        }
    
        #endregion
    }
}
