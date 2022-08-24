using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jpsys.SagyoManage.ComLib;
using GrapeCity.Win.Editors;

namespace Jpsys.SagyoManage.FrameLib.InputMan
{
    /// <summary>
    /// GcDateTimeの拡張クラス
    /// </summary>
    public static class GcDateTimeExtensions
    {
        /// <summary>
        /// 年の入力だけで当月、当日を補完し、年月日を形成するFieldLeaveイベント
        /// </summary>
        public static void GcDateTime_FieldLeave(object sender, GrapeCity.Win.Editors.FieldEventArgs e)
        {
            GrapeCity.Win.Editors.GcDateTime gcDateTime = (GrapeCity.Win.Editors.GcDateTime)sender;

            if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField)
                && gcDateTime.Value == null)
            {
                DateTime today = DateTime.Today;

                string yearPartText = e.Field.Text;
                string monthPartText = gcDateTime.Fields.Count >= 3 ? gcDateTime.Fields[2].Text : string.Empty;
                string dayPartText = gcDateTime.Fields.Count >= 5 ? gcDateTime.Fields[4].Text : string.Empty;

                int yearPartInt = yearPartText.Trim('_').ToInt32();
                int monthPartInt = monthPartText.Trim('_').ToInt32();
                int dayPartInt = dayPartText.Trim('_').ToInt32();

                int year = 0;
                int month = 0;
                int day = 0;

                if (yearPartInt != 0)
                {
                    if (yearPartInt < 1000)
                    {
                        year = yearPartInt + 2000;
                    }
                    else
                    {
                        year = yearPartInt;
                    }

                    if (monthPartInt == 0)
                    {
                        month = today.Month;
                    }
                    else
                    {
                        month = monthPartInt;
                    }

                    if (dayPartInt == 0)
                    {
                        day = today.Day;
                    }
                    else
                    {
                        day = dayPartInt;
                    }

                    DateTime dt = new DateTime(year, month, day);

                    //MaxMinBehavior.AdjustToMaxMinと同じ動きをさせる（ABEND回避） 2020/12/10 mawatari
                    //（その他のMaxMinBehaviorの動きをさせたい場合、都度実装してください）
                    if (gcDateTime.MaxDate < dt)
                    {
                        dt = gcDateTime.MaxDate;
                    }
                    else if (dt < gcDateTime.MinDate)
                    {
                        dt = gcDateTime.MinDate;
                    }

                    // ※ Value へセットしたら SelectionStart が 0 になってしまう（ActiveField が DateMonthField → DateYearField）
                    gcDateTime.Value = dt;

                    // ※ Value へセットして SelectionStart が変わってしまったので、強制的に本来の値へ戻す
                    //gcDateTime.SelectionStart = 5;

                    SendKeys.Send("{TAB}");
                }
            }

        }

        /// <summary>
        /// 年、月、日の入力だけで未入力の当年、当月、当日を補完し、
        /// 年月日を形成するFieldLeaveイベント
        /// </summary>
        //GcDateTime_FieldLeaveを修正 2018/08/27 mawatari 月、日でも当日補完するように修正 STERT
        public static void GcDateTime_FieldLeaveEx(object sender, GrapeCity.Win.Editors.FieldEventArgs e)
        {
            GrapeCity.Win.Editors.GcDateTime gcDateTime = (GrapeCity.Win.Editors.GcDateTime)sender;

            if ((
                e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField)
                || e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField)
                || e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField)
                )
                && gcDateTime.Value == null)
            {
                DateTime today = DateTime.Today;

                string yearPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 1
                        ? gcDateTime.Fields[0].Text
                        : string.Empty; ;
                string monthPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 3 
                        ? gcDateTime.Fields[2].Text 
                        : string.Empty;
                string dayPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 5 
                        ? gcDateTime.Fields[4].Text 
                        : string.Empty;

                int yearPartInt = yearPartText.Trim('_').ToInt32();
                int monthPartInt = monthPartText.Trim('_').ToInt32();
                int dayPartInt = dayPartText.Trim('_').ToInt32();

                if (yearPartInt == 0 && monthPartInt == 0 && dayPartInt == 0)
                {
                    return;
                }

                int year = 0;
                int month = 0;
                int day = 0;

                if (yearPartInt < 1000)
                {
                    if (yearPartInt == 0)
                    {
                        year = today.Year;
                    }
                    else
                    {
                        year = yearPartInt + 2000;
                    }
                }
                else
                {
                    year = yearPartInt;
                }

                if (monthPartInt == 0)
                {
                    month = today.Month;
                }
                else
                {
                    month = monthPartInt;
                }

                if (dayPartInt == 0)
                {
                    day = today.Day;
                }
                else
                {
                    day = dayPartInt;
                }

                try
                {
                    gcDateTime.Value = new DateTime(year, month, day);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //2018/02/31とかになった場合、月末を設定
                    gcDateTime.Value = DateTimeExtensions.LastDayOfMonth(new DateTime(year, month, 1));
                }

                // ※ Value へセットして SelectionStart が変わってしまったので、強制的に本来の値へ戻す
                if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField))
                {
                    gcDateTime.SelectionStart = 0;
                }
                else if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField))
                {
                    gcDateTime.SelectionStart = 3;
                }
                else if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField))
                {
                    gcDateTime.SelectionStart = 5;
                }

                SendKeys.Send("{TAB}");
            }
        }
        //GcDateTime_FieldLeaveを修正 2018/08/27 mawatari 月、日でも当日補完するように修正 END

        /// <summary>
        /// 年、月、日の入力だけで未入力の当年、当月、当日を補完し、
        /// 年月日を形成するFieldLeaveイベント
        /// </summary>
        //GcDateTime_FieldLeaveExを修正 2018/10/01 mawatari
        //日の月、日でも当日補完するように修正 STERT
        public static void GcDateTime_FieldLeaveDefaultToday(object sender, GrapeCity.Win.Editors.FieldEventArgs e)
        {
            GrapeCity.Win.Editors.GcDateTime gcDateTime = (GrapeCity.Win.Editors.GcDateTime)sender;

            if ((
                e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField)
                || e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField)
                || e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField)
                )
                && gcDateTime.Value == null)
            {
                DateTime today = DateTime.Today;

                string yearPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 1
                        ? gcDateTime.Fields[0].Text
                        : string.Empty; ;
                string monthPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 3 
                        ? gcDateTime.Fields[2].Text 
                        : string.Empty;
                string dayPartText = e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField)
                    ? e.Field.Text
                    : gcDateTime.Fields.Count >= 5 
                        ? gcDateTime.Fields[4].Text 
                        : string.Empty;

                int yearPartInt = yearPartText.Trim('_').ToInt32();
                int monthPartInt = monthPartText.Trim('_').ToInt32();
                int dayPartInt = dayPartText.Trim('_').ToInt32();

                if (yearPartInt == 0 && monthPartInt == 0 && dayPartInt == 0)
                {
                    return;
                }

                int year = 0;
                int month = 0;
                int day = 0;

                if (yearPartInt < 1000)
                {
                    if (yearPartInt == 0)
                    {
                        year = today.Year;
                    }
                    else
                    {
                        year = yearPartInt + 2000;
                    }
                }
                else
                {
                    year = yearPartInt;
                }

                if (monthPartInt == 0)
                {
                    month = today.Month;
                }
                else
                {
                    month = monthPartInt;
                }

                if (dayPartInt == 0)
                {
                    day = today.Day;
                }
                else
                {
                    day = dayPartInt;
                }

                try
                {
                    gcDateTime.Value = new DateTime(year, month, day);
                }
                catch (ArgumentOutOfRangeException)
                {
                    //2018/02/31とかになった場合、月末を設定
                    gcDateTime.Value = DateTimeExtensions.LastDayOfMonth(new DateTime(year, month, 1));
                }

                // ※ Value へセットして SelectionStart が変わってしまったので、強制的に本来の値へ戻す
                if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateYearField))
                {
                    gcDateTime.SelectionStart = 0;
                }
                else if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateMonthField))
                {
                    gcDateTime.SelectionStart = 3;
                }
                else if (e.Field.GetType() == typeof(GrapeCity.Win.Editors.Fields.DateDayField))
                {
                    gcDateTime.SelectionStart = 5;
                }

                SendKeys.Send("{TAB}");
            }
        }
        //GcDateTime_FieldLeaveを修正 2018/08/27 mawatari 月、日でも当日補完するように修正 END
    }
}
