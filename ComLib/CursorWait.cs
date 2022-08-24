using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// カーソルを待機カーソルに変更します。
    /// </summary>
    public class CursorWait : IDisposable
    {
        public CursorWait()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor;
        }


        #region IDisposable メンバー

        public void Dispose()
        {
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default;
        }

        #endregion
    }
}
