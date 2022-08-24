using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    public class NoBusinessDataException : NSKException
    {
        /// <summary>
        /// 本例外クラスを初期化します。
        /// </summary>
        public NoBusinessDataException()
            : base()
        {
        }

        /// <summary>
        /// エラーメッセージを指定して、本例外クラスを初期化します。
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public NoBusinessDataException(string message)
            : base(message)
        { }
    }
}
