using GrapeCity.Win.MultiRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.MultiRow
{
    /// <summary>
    /// テンプレートの初期化を提供します。
    /// </summary>
    public class TemplateInitializer
    {
        /// <summary>
        /// </summary>
        public GcMultiRow _mrow;

        /// <summary>
        /// </summary>
        /// <param name="mrow"></param>
        public TemplateInitializer(GcMultiRow mrow)
        {
            this._mrow = mrow;
        }

        /// <summary>
        /// </summary>
        public void Initialize()
        {
            foreach (var cell in this._mrow.Template.Row.Cells)
            {
                this.InitValue(cell);
            }
        }

        private void InitValue(GrapeCity.Win.MultiRow.Cell cell)
        {
            if (cell.GetType() == typeof(GrapeCity.Win.MultiRow.InputMan.GcTextBoxCell))
            {
                cell.Value = string.Empty;
            }
            else if (cell.GetType() == typeof(GrapeCity.Win.MultiRow.InputMan.GcNumberCell))
            {
                cell.Value = 0;
            }
            else if (cell.GetType() == typeof(GrapeCity.Win.MultiRow.InputMan.GcDateTimeCell))
            {
                cell.Value = null;
            }
            else if (cell.GetType() == typeof(GrapeCity.Win.MultiRow.CheckBoxCell))
            {
                cell.Value = false;
            }
            else
            {
                //Console.WriteLine(cell.GetType());
            }
        }
    }
}
