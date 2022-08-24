using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// Rangeに関するstaticメソッドを提供します。
    /// </summary>
    public static class Range
    {
        public static Range<T> Create<T>(T start, T end) where T : IComparable
        {
            return new Range<T>(start, end);
        }
    }

    /// <summary>
    /// 2つの値の範囲を表します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Range<T> where T : IComparable
    {
        /// <summary>
        /// 範囲の開始と終了を指定して値を初期化します。
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        public Range(T start, T end)
        {
            if (!RangeHelper.StartEndIsConsistent(start, end))
            {
                throw new ArgumentException("endはstart以上の値を指定してください。");
            }

            this._start = start;
            this._end = end;
        }

        T _start;
        T _end;

        /// <summary>
        /// 範囲の開始
        /// </summary>
        public T Start
        {
            get
            {
                return this._start;
            }
        }

        /// <summary>
        /// 範囲の終了
        /// </summary>
        public T End
        {
            get
            {
                return this._end;
            }
        }

        /// <summary>
        /// 指定した値が範囲に含まれているかどうか
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            if (this.Start.CompareTo(value) > 0)
            {
                return false;
            }

            if (this.End.CompareTo(value) < 0)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 指定した範囲オブジェクトと重なる部分が存在するかどうかを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool OverlapOn(Range<T> value)
        {
            if (value == null)
                return false;

            return
                (this.Start.CompareTo(value.End) <= 0)          //A.Start <= B.End
                && (this.End.CompareTo(value.Start) >= 0);      //A.End >= B.Start
        }

        public override string ToString()
        {
            return string.Format("Start：{0}　End：{1}", this.Start, this.End);
        }
    }
}
