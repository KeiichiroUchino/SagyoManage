using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.Threading;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;

namespace Jpsys.SagyoManage.Deploy
{
    static class StartExecute
    {
        /// <summary>
        /// アプリケーションのメイン エントリ ポイントです。
        /// </summary>
        [STAThread]
        static void Main()
        {
            SetProcessDPIAware();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Boot.BootInitializer.BootStart();

            Application.Exit();

        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public extern static IntPtr SetProcessDPIAware();
    }
}
