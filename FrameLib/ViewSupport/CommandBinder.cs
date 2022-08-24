using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// コマンドとコントロールをバインドします。
    /// </summary>
    public class CommandBinder
    {
         /// <summary>
        /// 監視対象のEnter,Leaveを監視するオブジェクト
        /// </summary>
        private Command _command;

        /// <summary>
        /// 検索可能状態を反映するオブジェクト
        /// </summary>
        private List<Component> _viewComponent = new List<Component>();

        /// <summary>
        /// </summary>
        /// <param name="command"></param>
        public CommandBinder(Command command)
        {
            _command = command;
            _command.EnabledChanged += _command_EnabledChanged;
        }

        void _command_EnabledChanged(object sender, EventArgs e)
        {
            //***SearchStateBinderと同じくダブルディスパッチを使用する。理由はあっちに書いてあります。
            //***要素をdynamicで取回してダブルディスパッチする
            foreach (dynamic item in _viewComponent)
            {
                try
                {
                    StatusChangeForObject(item, _command.Enabled);
                }
                //ディスパッチ先の記述漏れで例外が発生しても処理止めたくないので、例外を握りつぶす
                catch (Exception)
                { }
            }
        }

        #region ダブルディスパッチ先

        private void StatusChangeForObject(Control obj, bool status)
        {
            obj.Enabled = status;
        }

        private void StatusChangeForObject(ToolStripStatusLabel obj, bool status)
        {
            obj.Enabled = status;
        }

        private void StatusChangeForObject(ToolStripItem obj, bool status)
        {
            obj.Enabled = status;
        }

        #endregion

        /// <summary>
        /// </summary>
        public void AddViewComponents(params Component[] components)
        {
            foreach (dynamic item in components)
	        {
                try
                {
                    AddViewComponent(item);
                }
                //ディスパッチ先の記述漏れで例外が発生しても処理止めたくないので、例外を握りつぶす
                catch (Exception)
                { }
	        }
        }

        /// <summary>
        /// </summary>
        /// <param name="control"></param>
        public void AddViewComponent(Control control)
        {
            control.Click += component_Click;
            _viewComponent.Add(control);
        }

        /// <summary>
        /// </summary>
        /// <param name="component"></param>
        public void AddViewComponent(ToolStripStatusLabel component)
        {
            _viewComponent.Add(component);
        }

        /// <summary>
        /// </summary>
        /// <param name="component"></param>
        public void AddViewComponent(ToolStripItem component)
        {
            component.Click += component_Click;
            _viewComponent.Add(component);
        }

        void component_Click(object sender, EventArgs e)
        {
            _command.PerformExecute();
        }
    }
}
