using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// 期間クラス
    /// </summary>
    [Serializable]
    public class Period
    {
        /// <summary>
        /// 期間の開始
        /// </summary>
        public DateTime StartDateTime { get; private set; }

        /// <summary>
        /// 期間の終了
        /// </summary>
        public DateTime EndDateTime { get; private set; }

        /// <summary>
        /// 期間の開始と終了を指定して値を初期化します。
        /// </summary>
        /// <param name="StartDateTime"></param>
        /// <param name="eEndDateTimend"></param>
        public Period(DateTime StartDateTime, DateTime EndDateTime)
        {
            if (StartDateTime > EndDateTime)
                throw new ArgumentException("endはstart以上の値を指定してください。");
            this.StartDateTime = StartDateTime;
            this.EndDateTime = EndDateTime;
        }

        /// <summary>
        /// 指定した範囲オブジェクトと重なる部分が存在するかどうかを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool Overlaps(Period anotherPeriod){
            return (this.StartDateTime < anotherPeriod.EndDateTime && anotherPeriod.StartDateTime < this.EndDateTime);
        }

        /// <summary>
        /// 指定した範囲オブジェクトと重なり方を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IntersectionType GetIntersectionType(Period anotherPeriod)
        {
            if (anotherPeriod == null)
                return IntersectionType.None;

            if (this == anotherPeriod)
            {
                return IntersectionType.RangesEqauled;
            }
            else if (IsInRange(anotherPeriod.StartDateTime) && IsInRange(anotherPeriod.EndDateTime))
            {
                return IntersectionType.ContainedInRange;
            }
            else if (IsInRange(anotherPeriod.StartDateTime))
            {
                return IntersectionType.StartsInRange;
            }
            else if (IsInRange(anotherPeriod.EndDateTime))
            {
                return IntersectionType.EndsInRange;
            }
            else if (anotherPeriod.IsInRange(this.StartDateTime) && anotherPeriod.IsInRange(this.EndDateTime))
            {
                return IntersectionType.ContainsRange;
            }
            return IntersectionType.None;
        }

        /// <summary>
        /// 指定した期間オブジェクトとの重複間隔を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public TimeSpan GetIntersection(Period anotherPeriod)
        {
            var type = this.GetIntersectionType(anotherPeriod);
            if (type == IntersectionType.RangesEqauled || type == IntersectionType.ContainedInRange)
            {
                return anotherPeriod.EndDateTime - anotherPeriod.StartDateTime;
            }
            else if (type == IntersectionType.StartsInRange)
            {
                return this.EndDateTime - anotherPeriod.StartDateTime;
            }
            else if (type == IntersectionType.EndsInRange)
            {
                return anotherPeriod.EndDateTime - this.StartDateTime;
            }
            else if (type == IntersectionType.ContainsRange)
            {
                return this.EndDateTime - this.StartDateTime;
            }
            else
            {
                return new TimeSpan();
            }
        }

        /// <summary>
        /// 指定した日付が期間内に存在するか返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>判定結果（true：存在する、false：存在しない）</returns>
        public bool IsInRange(DateTime date)
        {
            return (date >= this.StartDateTime) && (date <= this.EndDateTime);
        }

        /// <summary>
        /// 期間間隔を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns>期間間隔</returns>
        public TimeSpan GetDuration()
        {
            return EndDateTime - StartDateTime;
        }

        /// <summary>
        /// 期間の重複パターンタイプを表す列挙体です。
        /// </summary>
        public enum IntersectionType
        {
            /// <summary>
            /// 重複しない
            /// </summary>
            None = -1,
            /// <summary>
            /// 後方重複
            ///     |--------|       ：ベース期間
            /// |--------|           ：対象期間
            /// </summary>
            EndsInRange,
            /// <summary>
            /// 前方重複
            /// |--------|           ：ベース期間
            ///     |--------|       ：対象期間
            StartsInRange,
            /// <summary>
            /// 期間一致
            /// |--------|           ：ベース期間
            /// |--------|           ：対象期間
            RangesEqauled,
            /// <summary>
            /// 範囲内に含まれる
            /// |------------|       ：ベース期間
            ///   |--------|         ：対象期間
            ContainedInRange,
            /// <summary>
            /// 範囲を含む
            ///   |--------|         ：ベース期間
            /// |------------|       ：対象期間
            ContainsRange,
        }
    }
}
