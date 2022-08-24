using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// 画面のコマンドを表します。
    /// </summary>
    public class Command
    {
        #region イベント

        /// <summary>
        /// 有効状態が変更されたときのイベントです。
        /// </summary>
        public event EventHandler EnabledChanged;

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected void OnEnabledChanged(EventArgs e)
        {
            if (EnabledChanged != null)
                this.EnabledChanged(this, e);
        }

        /// <summary>
        /// コマンドが実行されたときのイベントです。
        /// </summary>
        public event EventHandler Execute;

        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected void OnExecute(EventArgs e)
        {
            if (Execute != null)
                this.Execute(this, e);
        }

        #endregion

        /// <summary>
        /// Executeイベントを発生させます。
        /// </summary>
        public void PerformExecute()
        {
            this.OnExecute(new EventArgs());
        }

        private bool _enabled;

        /// <summary>
        /// 有効状態を取得します。
        /// </summary>
        public bool Enabled
        {
            get { return _enabled; }
            set 
            { 
                _enabled = value;
                this.OnEnabledChanged(new EventArgs());
            }
        }
    }
}
