using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model.DALExceptions
{
    /// <summary>
    /// 売上、入金, 日報などが月次締め処理済みで登録, 修正, 削除ができなかった場合にスローされます。
    /// </summary>
    public class SettleByMonthClosedException : Exception
    {
    }
}
