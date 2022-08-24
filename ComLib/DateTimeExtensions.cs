using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// DateTime型の拡張メソッドを提供します。
    /// </summary>
    public static class DateTimeExtensions
    {
         /// <summary>
        /// 1日付の日付を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(this DateTime value)
        {
            return ChangeDay(value, 1);
        }

        /// <summary>
        /// 末日付の日付を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(this DateTime value)
        {
            return ChangeDay(value, DateTime.DaysInMonth(value.Year, value.Month));
        }

        /// <summary>
        /// 末日付の日付かどうかを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsLastDayOfMonth(this DateTime value)
        {
            return value.Date == LastDayOfMonth(value);
        }


        /// <summary>
        /// 日(dd)の部分を変更して新たな日付を作成します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="day"></param>
        /// <returns></returns>
        public static DateTime ChangeDay(this DateTime value, int day)
        {
            return new DateTime(value.Year, value.Month, day);
        }

        /// <summary>
        /// 時分秒の部分を変更して新たな日付を作成します。
        /// </summary>
        /// <param name="hour"></param>
        /// <param name="minute"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static DateTime ChangeTime(this DateTime value, int hour, int minute, int second)
        {
            return new DateTime(value.Year, value.Month, value.Day, hour, minute, second);
        }

        /// <summary>
        /// 元年からの月数を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int MonthCountFromTheFirstYear(this DateTime value)
        {
            return (value.Year * 12) + value.Month;
        }

        /// <summary>
        /// 前日を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime PreviousDay(this DateTime value)
        {
            return value.AddDays(-1);
        }

        /// <summary>
        /// 前日が存在するかどうかを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ExistsPreviousDay(this DateTime value)
        {
            try
            {
                value.PreviousDay();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 翌日を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime NextDay(this DateTime value)
        {
            return value.AddDays(1);
        }

        /// <summary>
        /// 翌日が存在するかどうかを取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ExistsNextDay(this DateTime value)
        {
            try
            {
                value.NextDay();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// 前月を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime PreviousMonth(this DateTime value)
        {
            return value.AddMonths(-1);
        }

        /// <summary>
        /// 翌月を取得します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime NextMonth(this DateTime value)
        {
            return value.AddMonths(1);
        }

        /// <summary>
        /// 23時59分59秒の日付を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime EndOfDay(this DateTime value)
        {
            return value.Date.AddHours(23).AddMinutes(59).AddSeconds(59);
        }

        /// <summary>
        /// DateTime.MinValueの場合は空文字を返して、
        /// そうでなければ、指定した書式を使用してToString()メソッドを実行した結果を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringIfMinValueThenEmpty(this DateTime value, string format)
        {
            if (value == DateTime.MinValue)
            {
                return string.Empty;
            }

            return value.ToString(format);
        }


        /// <summary>
        /// 指定した範囲に含まれるかどうかを返します。
        /// </summary>
        /// <param name="value">対象のDateTime値</param>
        /// <param name="start">範囲の開始</param>
        /// <param name="end">範囲の終了</param>
        /// <returns></returns>
        public static bool IsInRange(this DateTime value, DateTime? start, DateTime? end)
        {
            //両方なければ常にfalse。
            if (!start.HasValue && !end.HasValue)
            {
                return false;
            }
        
            //開始の判定。値なしの場合はチェックしない
            if (start.HasValue)
            {
                if (value < start.Value)
                    return false;
            }

            //終了の判定。値なしの場合はチェックしない
            if (end.HasValue)
            {
                if (value > end.Value)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 時刻部分を取り出して TimeSpan型に変換します。
        /// </summary>
        /// <returns></returns>
        public static TimeSpan ToTimeSpan(this DateTime value)
        {
            return new TimeSpan(value.Hour, value.Minute, value.Second);
        }

        /// <summary>
        /// 日付が最大かどうか（時分秒を除く）
        /// </summary>
        /// <returns></returns>
        public static bool IsMaxDate(this DateTime value)
        {
            return (value.Date == DateTime.MaxValue.Date);
        }

        /// <summary>
        /// 日付を指定した書式の和暦の文字列に変換して返します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static string ToStringWareki(this DateTime value, string format)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("ja-JP", true);
            culture.DateTimeFormat.Calendar = new System.Globalization.JapaneseCalendar();

            if (value == DateTime.MinValue)
            {
                return string.Empty;
            }

            return value.ToString(format,culture);
        }

        #region Null許容型

        /// <summary>
        /// 指定した書式を指定して文字列に変換します。
        /// nullの場合は空白を返します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="format"></param>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static string ToString(this DateTime? value, string format, IFormatProvider provider = null)
        {
            if (value.HasValue)
            {
                if (provider != null)
                {
                    return value.Value.ToString(format, provider);
                }
                else
                {
                    return value.Value.ToString(format);
                }
            }
            else
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// 日付部分を取得します。
        /// インスタンスがnullの場合はnullを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime? GetDate(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value.Date;
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 時刻部分を取り出して TimeSpan型に変換します。
        /// </summary>
        /// <returns></returns>
        public static TimeSpan? ToTimeSpan(this DateTime? value)
        {
            if (value.HasValue)
            {
                return ToTimeSpan(value.Value);
            }
            else
            {
                return null;
            }
        }


        /// <summary>
        /// 値がnullの場合はMaxValueを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime MaxValueIfNull(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                return DateTime.MaxValue;
            }
        }

        /// <summary>
        /// 値がnullの場合はMinValueを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static DateTime MinValueIfNull(this DateTime? value)
        {
            if (value.HasValue)
            {
                return value.Value;
            }
            else
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// NullまたはDateTime.MinValueの場合はtrueを返します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrMinValue(this DateTime? value)
        {
            if (!value.HasValue)
                return true;

            return (value.Value == DateTime.MinValue);
        }

        #endregion
    }
}
