using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    public class RangeHelper
    {
        /// <summary>
        /// 開始~終了の整合性（値の大小）が取れているかどうか
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool StartEndIsConsistent<T>(T start, T end) where T : IComparable
        {
            return (start.CompareTo(end) <= 0);
        }

        /// <summary>
        /// 開始~終了の整合性（値の大小）が取れているかどうかを判断します
        /// 開始または終了がnullの場合はnullを返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        public static bool? StartEndIsConsistent<T>(T? start, T? end) where T : struct, IComparable
        {
            if (!start.HasValue)
                return null;

            if (!end.HasValue)
                return null;

            return StartEndIsConsistent(start.Value, end.Value);
        }

        /// <summary>
        /// 指定した日付範囲のコレクションが期間の重複を含まないどうかを返します。(含まない場合：true)
        /// 指定したコレクションの要素数が0, または1つの場合は true を返します。
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static bool DateRangeIsNonOverlaping(IEnumerable<Range<DateTime>> ranges)
        {
            return CompareToPreviousItemAll(ranges, (previous, current) =>
            {
                //  小 ⇦ - - - - - - - - - - - ➡ 大
                //前要素   |-------|
                //現在要素            |--------|
                //の場合の判断
                if (previous.End.Date <= current.Start.Date)
                {
                    return true;
                }

                //前要素               |-------|
                //現在要素  |--------|
                //の場合の判断
                if (previous.Start.Date >= current.End.Date)
                {
                    return true;
                }

                return false;
            });
        }

        /// <summary>
        /// 指定した日付範囲のコレクションが連続しているかどうか
        /// 前要素の終了日と現在要素の開始日が同じ場合も許容します。
        /// 指定したコレクションの要素数が0, または1つの場合は true を返します。
        /// </summary>
        /// <param name="ranges"></param>
        /// <returns></returns>
        public static bool DateRangeIsConcatSameDayOK(IEnumerable<Range<DateTime>> ranges)
        {
            return CompareToPreviousItemAll(ranges, (previous, current) => 
            {    
                DateTime previousEnd = previous.End.Date;
                DateTime currentStart = current.Start.Date;

                if (previousEnd == currentStart)
                {
                    return true;
                }

                //前要素の終了日付がMAXの場合はNextDayで例外を吐くので、ここで処理する。
                if (previousEnd.IsMaxDate())
                {
                    return false;
                }

                if (previousEnd.NextDay() == currentStart)
                {
                    return true;
                }

                return false;
            });
        }

        /// <summary>
        /// すべての要素が前要素と比較する述語を満たすかどうかを返します。
        /// 指定した範囲の要素数が0, または1つの場合は true を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="ranges">レンジのコレクション</param>
        /// <param name="predicate">前要素と現在要素を引数にする述語</param>
        /// <returns></returns>
        private static bool CompareToPreviousItemAll<T>(IEnumerable<Range<T>> ranges, ComparePreviousItemPredicate<T> predicate) where T : IComparable
        {
            bool first = true;
            Range<T> previousItem = null;

            foreach (var item in ranges)
            {
                if (!first)
                {
                    if (!predicate(previousItem, item)) { return false; }
                }

                previousItem = item;
                first = false;
            }

            return true;
        }

        /// <summary>
        /// 前要素と現在要素を引数にする述語を表します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="previousItem"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private delegate bool ComparePreviousItemPredicate<T>(Range<T> previousItem, Range<T> item) where T : IComparable;
    }
}
