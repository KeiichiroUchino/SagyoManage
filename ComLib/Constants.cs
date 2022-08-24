using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// 定数の集まり
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// SQLのエラー番号
        /// </summary>
        public static class SQLErrors
        {
            /// <summary>
            /// デッドロック
            /// </summary>
            public static readonly int Deadlock = 1205;

            /// <summary>
            /// 一意制約エラー
            /// </summary>
            public static readonly int UniqueConstraintViolation = 2627;
        }
    }
}
