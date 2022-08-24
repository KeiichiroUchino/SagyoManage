using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.Frame.Command
{
    /// <summary>
    /// 使用するコマンドおよびバインダのインスタンスを集約します。
    /// 各画面でインスタンスを作成して、コマンドのショートカットとして使用します。
    /// </summary>
    public class CommandSet
    {
        private Dictionary<CommandType, FrameLib.ViewSupport.Command> _commands;
        private Dictionary<FrameLib.ViewSupport.Command, FrameLib.ViewSupport.CommandBinder> _binders;


        public CommandSet()
        {
            _commands = new Dictionary<CommandType, FrameLib.ViewSupport.Command>();
            _binders = new Dictionary<FrameLib.ViewSupport.Command, FrameLib.ViewSupport.CommandBinder>();

            this.ChangeCode = this.RegisterNew(CommandType.ChangeCode);
            this.New = this.RegisterNew(CommandType.New);
            this.ExportCsv = this.RegisterNew(CommandType.ExportCsv);
            this.ImportCsv = this.RegisterNew(CommandType.ImportCsv);

            this.PrintToScreen = this.RegisterNew(CommandType.PrintToScreen);
            this.PrintToPrinter = this.RegisterNew(CommandType.PrintToPrinter);
            this.EditCancel = this.RegisterNew(CommandType.EditCancel);
            this.Delete = this.RegisterNew(CommandType.Delete);
            this.Select = this.RegisterNew(CommandType.Select);
            this.ExecProc = this.RegisterNew(CommandType.ExecProc);
            this.Close = this.RegisterNew(CommandType.Close);
            this.Save = this.RegisterNew(CommandType.Update);
            this.DeleteAllDtl = this.RegisterNew(CommandType.DeleteAllDtl);
            this.Copy = this.RegisterNew(CommandType.Copy);
            this.Send = this.RegisterNew(CommandType.Send);
            this.DelPrintSetting = this.RegisterNew(CommandType.DelPrintSetting);
        }

        /// <summary>
        /// 指定したコマンドタイプのコマンドを作成して登録します。
        /// </summary>
        /// <param name="commandType"></param>
        /// <returns></returns>
        private FrameLib.ViewSupport.Command RegisterNew(CommandType commandType)
        {
            FrameLib.ViewSupport.Command command = new FrameLib.ViewSupport.Command();
            _commands.Add(commandType, command);

            return command;
        }

        public FrameLib.ViewSupport.Command ChangeCode { get; private set; }
        public FrameLib.ViewSupport.Command New { get; private set; }
        public FrameLib.ViewSupport.Command ExportCsv { get; private set; }
        public FrameLib.ViewSupport.Command ImportCsv { get; private set; }

        public FrameLib.ViewSupport.Command PrintToScreen { get; private set; }
        public FrameLib.ViewSupport.Command PrintToPrinter { get; private set; }
        public FrameLib.ViewSupport.Command EditCancel { get; private set; }
        public FrameLib.ViewSupport.Command Delete { get; private set; }
        public FrameLib.ViewSupport.Command Select { get; private set; }
        public FrameLib.ViewSupport.Command Close { get; private set; }
        public FrameLib.ViewSupport.Command Save { get; private set; }
        public FrameLib.ViewSupport.Command ExecProc { get; private set; }
        public FrameLib.ViewSupport.Command DeleteAllDtl { get; private set; }
        public FrameLib.ViewSupport.Command Copy { get; private set; }
        public FrameLib.ViewSupport.Command Send { get; private set; }
        public FrameLib.ViewSupport.Command DelPrintSetting { get; private set; }

        /// <summary>
        /// コマンドとコンポーネントをバインドします。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="components"></param>
        public void Bind(FrameLib.ViewSupport.Command command, params Component[] components)
        {
            foreach (var item in components)
            {
                this.Bind(command, item);
            }
        }

        /// <summary>
        /// コマンドとコンポーネントをバインドします。
        /// </summary>
        /// <param name="command"></param>
        /// <param name="components"></param>
        public void Bind(FrameLib.ViewSupport.Command command, Component component)
        {
            FrameLib.ViewSupport.CommandBinder binder;

            if (_binders.ContainsKey(command))
            {
                binder = _binders[command];
            }
            else
            {
                binder = new FrameLib.ViewSupport.CommandBinder(command);
                _binders.Add(command, binder);
            }

            binder.AddViewComponents(component);
        }
    }
}
