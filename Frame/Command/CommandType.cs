using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Frame.Command
{
    public enum CommandType
    {
        ChangeCode,
        New,
        ExportCsv,
        ImportCsv,
        PrintToScreen,
        PrintToPrinter,
        EditCancel,
        Delete,
        Select,
        ExecProc,
        Close,
        Update,
        Separator,
        DeleteAllDtl,
        Copy,
        Send,
        DelPrintSetting,
    }
}
