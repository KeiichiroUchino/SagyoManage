using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.Spread.Cells
{
    /// <summary>
    /// </summary>
    public class CustomDateTimeCellType : FarPoint.Win.Spread.CellType.DateTimeCellType
    {
        /// <summary>
        /// </summary>
        public override string Format(object o)
        {
            DateTime value = System.Convert.ToDateTime(o);

            //日付がMinValueの場合は空白を返す
            if (value == DateTime.MinValue)
            {
                return string.Empty;
            }
            else
            {
                return base.Format(o);
            }
        }
    }
}
