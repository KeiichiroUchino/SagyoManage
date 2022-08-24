using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;

namespace Jpsys.SagyoManage.BizProperty
{
    /// <summary>
    /// 数値の端数処理を行うためのクラスです
    /// </summary>
    public class MarumeNumberCalculator
    {
        /// <summary>
        /// 端数処理をします。
        /// </summary>
        /// <param name="kbn"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static decimal Marume(int kbn, decimal val)
        {
            return Marume(kbn, val, 0);
        }

        /// <summary>
        /// 値の端数処理をします。
        /// </summary>
        /// <param name="kbn">端数丸め区分</param>
        /// <param name="val">値</param>
        /// <param name="digits">精度（0 の場合は整数になる）</param>
        /// <returns>値</returns>
        public static decimal Marume(int kbn, decimal val, int digits)
        {
            switch ((DefaultProperty.HasuMArumeKbn)kbn)
            {
                case DefaultProperty.HasuMArumeKbn.RoundOff:
                    val = NSKUtil.RoundOff(val, digits);
                    break;
                case DefaultProperty.HasuMArumeKbn.RoundUp:
                    val = NSKUtil.RoundUp(val, digits);
                    break;
                case DefaultProperty.HasuMArumeKbn.RoundDown:
                    val = NSKUtil.RoundDown(val, digits);
                    break;
                default:
                    break;
            }

            return val;
        }
    }
}
