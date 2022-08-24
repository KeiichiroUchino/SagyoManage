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
    /// コントロールの値が変更されているときだけValidtingイベントを発生させます。
    /// </summary>
    public class ControlValidatingEventRaiser
    {
        private Control _control;
        private Type _controlType;
        private Func<Control, object> _valueGetter;

        /// <summary>
        /// </summary>
        public ControlValidatingEventRaiser(Control control, Type tpe, Func<Control, object> valueGetter, CancelEventHandler eventHandler)
        {
            _control = control;
            _controlType = tpe;
            _valueGetter = valueGetter;
            if (eventHandler != null)
            {
                this.Validating += eventHandler;
            }

            _control.Enter += _control_Enter;
            _control.Validating += _control_Validating;
            _control.Validated += _control_Validated;
        }

        /// <summary>
        /// コントロールと変更対象の値を取得する関数を指定して、インスタンスを作成します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="control"></param>
        /// <param name="valueGetter"></param>
        /// <param name="eventHandler"></param>
        /// <returns></returns>
        public static ControlValidatingEventRaiser Create<T>(T control, Func<T, object> valueGetter, CancelEventHandler eventHandler) where T : Control
        {
            //Controlを引数にする関数に変換
            Func<Control, object> func = (ctl) => valueGetter((T)ctl);

            return new ControlValidatingEventRaiser(control, typeof(T), func, eventHandler);
        }

        #region イベント

        /// <summary>
        /// </summary>
        public event CancelEventHandler Validating;

        /// <summary>
        /// </summary>
        /// <param name="control"></param>
        /// <param name="e"></param>
        protected void OnValidating(Control control, CancelEventArgs e)
        {
            if (Validating != null)
                Validating(control, e);
        }

        #endregion

        /// <summary>
        /// 前回値
        /// </summary>
        private object _oldValue;
        /// <summary>
        /// 前回値を保持している状態ならtrue
        /// </summary>
        private bool _isHandling;

        private void _control_Enter(object sender, EventArgs e)
        {
            this.PerformWhenEnter();
        }

        private void _control_Validating(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //***ハンドリング中以外はValidatingを発生させない。
            //***こうすることで、保存前のForm.Validate()でValidatingイベントが発生しなくなる
            if (!_isHandling)
                return;

            if (object.Equals(_oldValue, _valueGetter(_control)))
                return;

            this.OnValidating(_control, e);
        }

        void _control_Validated(object sender, EventArgs e)
        {
            _oldValue = null;
            _isHandling = false;
        }

        /// <summary>
        /// コントロールのEnterイベント時と同等の処理を実行します。
        /// </summary>
        public void PerformWhenEnter()
        {
            _oldValue = _valueGetter(_control);
            _isHandling = true;
        }

        /// <summary>
        /// 前回値を保持してる場合、前回値を更新します。
        /// Enterイベント後に画面値の初期化処理がされる場合などに呼び出してください。
        /// </summary>
        public void UpdateOldValue()
        {
            if (_isHandling)
                _oldValue = _valueGetter(_control);
        }

        /// <summary>
        /// 対象のコントロールを取得します。
        /// </summary>
        public Control Control { get { return this._control; } }

        /// <summary>
        /// 編集前の値を取得します。
        /// </summary>
        public Object OldValue { get { return this._oldValue; } }
    }
}
