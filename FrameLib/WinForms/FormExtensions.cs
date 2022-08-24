using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.WinForms
{
    /// <summary>
    /// </summary>
    public static class FormExtensions
    {
        /// <summary>
        /// 共通検索が完了したときに呼び出します。
        /// </summary>
        public static void OnCmnSearchComplete(this Form source)
        {
            SendKeys.Send("{TAB}");
        }
    }
}
