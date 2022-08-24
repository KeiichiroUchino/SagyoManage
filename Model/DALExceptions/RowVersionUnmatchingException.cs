using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    /// <summary>
    /// 更新時に行バージョンが異なるデータを更新しようとした時に発生する例外です。
    /// </summary>
    public class RowVersionUnmatchingException : NSKException
    {
        /// <summary>
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        public RowVersionUnmatchingException()
            : base()
        { }


        /// <summary>
        /// 例外発生時のメッセージを指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="message">例外発生時のメッセージ</param>
        public RowVersionUnmatchingException(string message)
            : base(message)
        {
        }
    }
}
