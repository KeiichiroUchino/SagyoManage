using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using jp.co.jpsys.util;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    /// <summary>
    /// 登録しようとするデータとそのデータの更新対象となるデータベース上の
    /// データに不一致など登録できない理由が存在する場合に、発生する例外を表します。
    /// </summary>
    public class DetailDataUnmatchingException : NSKException
    {
        /// <summary>
        /// 本例外クラスを初期化します。
        /// </summary>
        public DetailDataUnmatchingException()
            : base()
        {
        }

        /// <summary>
        /// エラーメッセージを指定して、本例外クラスを初期化します。
        /// </summary>
        /// <param name="message">エラーメッセージ</param>
        public DetailDataUnmatchingException(string message)
            : base(message)
        { }
    }
}
