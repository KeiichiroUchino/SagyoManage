using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.Boot
{
    public class ErrorMessageUtil
    {
        public static DialogResult ShowErrorMessageDialog(string message)
        {
            var emf = new ErrorMessgeFrame(message);
            emf.StartPosition = FormStartPosition.CenterScreen;
            return emf.ShowDialog();
        }
    }
}

