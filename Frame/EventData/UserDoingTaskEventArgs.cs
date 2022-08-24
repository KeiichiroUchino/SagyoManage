using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Frame.EventData
{
    public class UserDoingTaskEventArgs : System.EventArgs
    {
        /// <summary>
        /// 処理結果を指定して、イベントデータのインスタンスを初期化します。
        /// </summary>
        /// <param name="taskResult">処理結果</param>
        public UserDoingTaskEventArgs(bool taskResult)
        {
            this.TaskResult = taskResult;
        }

        /// <summary>
        /// 処理結果と処理中に発生した例外の例外オブジェクトを指定して
        /// イベントデータのインスタンスを初期化します。
        /// このコンストラクタは、例外が発生したときのみ使用します。
        /// </summary>
        /// <param name="taskResult">処理結果</param>
        /// <param name="err">処理中に発生した例外</param>
        public UserDoingTaskEventArgs(bool taskResult, Exception err)
        {
            this.TaskResult = taskResult;
            this.TaskHandledError = err;
        }

        /// <summary>
        /// 処理結果を取得します。
        /// </summary>
        public bool TaskResult { get; private set; }

        /// <summary>
        /// 処理中に例外が発生した場合、その例外を取得します。
        /// </summary>
        public Exception TaskHandledError { get; private set; }
    }
}
