using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// DateTime型に関する機能を提供します。
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// 2つの日付の期間を取得します。
        /// </summary>
        /// <param name="fromDate">開始日付</param>
        /// <param name="toDate">終了日付</param>
        /// <param name="addFirstDay">初日を含めるかどうか（true：含める）</param>
        /// <returns>期間</returns>
        public static int GetPeriod(DateTime fromDate, DateTime toDate, bool addFirstDay)
        {
            if (fromDate == DateTime.MinValue || toDate == DateTime.MinValue)
            {
                return 0;
            }

            TimeSpan ts = toDate - fromDate;

            int day = ts.Days;

            //初日も含める場合
            if (addFirstDay)
            {
                day++;
            }

            return day;
        }
    }
}
