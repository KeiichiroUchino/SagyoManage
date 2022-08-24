using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    /// <summary>
    /// 売上、入金などが請求締処理済で登録, 修正, 削除ができなかった場合にスローされます。
    /// </summary>
    public class ClmFixedException : Exception
    {
    }
}
