using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using GrapeCity.Win.Editors;
using System.Linq.Expressions;
using System.ComponentModel;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// コントロールの値の変更をハンドルすることをサポートします。
    /// </summary>
    public class MultiControlValueChangedHandler
    {
        #region staticメンバの初期化

        /// <summary>
        /// ハンドルする対象のイベント
        /// </summary>
        private static readonly Dictionary<Type, string> TypeEventNames = new Dictionary<Type, string>();

        private static void Register<T>(string eventName)
        {
            TypeEventNames.Add(typeof(T), eventName);
        }

        static MultiControlValueChangedHandler()
        {
            //***Grapecity
            Register<GcTextBox>("TextChanged");
            Register<GcDate>("ValueChanged");
            Register<GcDateTime>("ValueChanged");
            Register<GcNumber>("ValueChanged");
            Register<ComboBox>("SelectedIndexChanged");
            //***WinForm
            Register<System.Windows.Forms.CheckBox>("CheckChanged");
            Register<System.Windows.Forms.RadioButton>("CheckChanged");
            //***jp.co.jpsys
            Register<jp.co.jpsys.util.ui.CheckBoxExNSK>("CheckChanged");
            Register<jp.co.jpsys.util.ui.RadioButtonExNSK>("CheckChanged");
        }

        #endregion

        /// <summary>
        /// 対象のコントロールを追加します。
        /// </summary>
        /// <param name="controls"></param>
        public void AddControls(params Control[] controls)
        {
            foreach (var item in controls)
            {
                this.AddControl(item);
            }
        }

        /// <summary>
        /// 検索可能コントロールを追加します。
        /// </summary>
        /// <param name="ctl"></param>
        public void AddControl(Control ctl)
        {
            Type type = ctl.GetType();

            if (!TypeEventNames.ContainsKey(type))
                return;

            string eventName = TypeEventNames[type];

            //***イベントのメタデータ
            EventDescriptorCollection events = TypeDescriptor.GetEvents(type);
            EventDescriptor changedEvent = events.Find(eventName, true);

            if (changedEvent == null)
                return;

            changedEvent.AddEventHandler(ctl, new EventHandler(OnValueChanged));
        }

        /// <summary>
        /// </summary>
        /// <param name="sendar"></param>
        /// <param name="args"></param>
        protected void OnValueChanged(object sendar, EventArgs args)
        {
            if (this.ValueChanged != null)
                this.ValueChanged(sendar, args);
        }

        /// <summary>
        /// </summary>
        public event EventHandler ValueChanged;
    }
}
