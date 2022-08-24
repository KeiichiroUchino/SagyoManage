using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    /// <summary>
    /// アプリケーション障害を表す例外です。
    /// アプリケーションを閉じます。
    /// </summary>
    public class ApplicationFailureException : Exception
    {
        /// <summary>
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        public ApplicationFailureException()
            : base("アプリケーションの終了が要求される例外です。")
        { }

        /// <summary>
        /// 例外発生時のメッセージを指定して、
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外発生時のメッセージ</param>
        public ApplicationFailureException(string message)
            : this(message, MessageBoxIcon.Error)
        {
        }

        /// <summary>
        /// 例外発生時のメッセージ、例外発生時のアイコンを指定して、
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外発生時のメッセージ</param>
        /// <param name="magicon">例外発生時のアイコン</param>
        public ApplicationFailureException(string message, MessageBoxIcon magicon)
            : this(message, magicon, null)
        {
        }

        /// <summary>
        /// 例外発生時のメッセージ、例外発生時のアイコン、内部例外を指定して、
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外発生時のメッセージ</param>
        /// <param name="magicon">例外発生時のアイコン</param>
        /// <param name="innerException">内部例外</param>
        public ApplicationFailureException(string message, MessageBoxIcon magicon, Exception innerException)
            : base(message, innerException)
        {
            this.Msgicon = magicon;
        }

        /// <summary>
        /// 例外発生時のアイコン
        /// </summary>
        public MessageBoxIcon Msgicon { get; set; }
    }
}
