using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// ControlOvseverの監視対象となるオブジェクトのイベントデータです。
    /// </summary>
    public class SubjectEventArgs : EventArgs
    {
        /// <summary>
        /// </summary>
        /// <param name="sourceDescription"></param>
        public SubjectEventArgs(string sourceDescription)
        {
            this.SourceDescription = sourceDescription;
        }

        /// <summary>
        /// 発生源になったコントロールの説明（デバッグ用）
        /// </summary>
        public string SourceDescription { get; private set; }
    }
}
