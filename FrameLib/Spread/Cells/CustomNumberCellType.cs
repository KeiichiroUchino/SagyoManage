using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.Spread.Cells
{
    /// <summary>
    /// </summary>
    public class CustomNumberCellType : FarPoint.Win.Spread.CellType.NumberCellType
    {
        /// <summary>
        /// ゼロを非表示にするかどうか
        /// </summary>
        public bool HideZero { get; set; }

        /// <summary>
        /// </summary>
        public override string Format(object o)
        {
            decimal value = System.Convert.ToDecimal(o);

            if (HideZero && value == 0)
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
