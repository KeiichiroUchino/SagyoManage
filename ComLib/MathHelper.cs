using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    public static class MathHelper
    {
        /// <summary>
        /// 除算をして結果を返します。
        /// 除数が0の場合は0を返します。
        /// 値はdecimal型で取り扱います。
        /// </summary>
        /// <param name="a">被除数</param>
        /// <param name="b">除数</param>
        public static decimal DivideOrZeroAsDecimal(decimal a, decimal b)
        {
            if (b == 0)
                return 0M;

            return a / b;
        }

        /// <summary>
        /// 値を負数にして返します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal Nagative(decimal val)
        {
            if (val <= 0)
                return val;

            return val * -1;
        }

        /// <summary>
        /// 数値化範囲内か判定します。
        /// True：範囲内
        /// </summary>
        /// <param name="value"></param>
        /// <param name="maxValue"></param>
        /// <param name="minValue"></param>
        /// <returns>True：範囲内</returns>
        public static bool Between(this decimal value, decimal maxValue, decimal minValue)
        {
            return value >= minValue && maxValue >= value;
        }
    }
}
