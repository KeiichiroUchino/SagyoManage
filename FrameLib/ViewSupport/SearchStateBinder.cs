using GrapeCity.Win.MultiRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.CSharp.RuntimeBinder;
using System.ComponentModel;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// 検索対象のEnter,Leaveを監視することによって画面の検索可能状態の変化を検知して、
    /// オブジェクトに反映します。
    /// </summary>
    public class SearchStateBinder
    {
        /// <summary>
        /// 建託対象のEnter,Leaveを監視するオブジェクト
        /// </summary>
        private ControlEnteringObserver _controlEnteringObserver;

        /// <summary>
        /// 検索可能状態を反映するオブジェクト
        /// </summary>
        private List<Component> _statusObjects = new List<Component>();

        /// <summary>
        /// </summary>
        /// <param name="f"></param>
        public SearchStateBinder(Form f)
        {
            _controlEnteringObserver = new ControlEnteringObserver(f);

            _controlEnteringObserver.SubjectEnter += _controlEnteringObserver_SubjectEnter;
            _controlEnteringObserver.SubjectLeave += _controlEnteringObserver_SubjectLeave;
        }

        

        void _controlEnteringObserver_SubjectEnter(object sender, SubjectEventArgs e)
        {
            //***要素をdynamicで取回してダブルディスパッチする
            foreach (dynamic item in _statusObjects)
            {
                try
                {
                    StatusChangeForObject(item, true);
                }
                //ディスパッチ先の記述漏れで例外が発生しても処理止めたくないので、例外を握りつぶす
                catch (RuntimeBinderException)
                {}
              
            }
        }

        void _controlEnteringObserver_SubjectLeave(object sender, SubjectEventArgs e)
        {
            //***要素をdynamicで取回してダブルディスパッチする
            foreach (dynamic item in _statusObjects)
            {
                try
                {
                    StatusChangeForObject(item, false);
                }
                //ディスパッチ先の記述漏れで例外が発生しても処理止めたくないので、例外を握りつぶす
                catch (Exception)
                {}
            }
        }

        //***Enabledの変更にはダブルディスパッチという手法を使う。
        //***実はComponentにはEnabledプロパティがない。あればComponentに対してEnabledを設定すればよかった。
        //***ToolStripStatusLabelのEnabledはComponentから継承しているわけではなく独自実装であり、
        //***ToolStripStatusLabelのEnabledの設定は個別で処理する必要がある。
        //***ダブルディスパッチを使用すれば通常よりもこの手の処理が簡単に記述できるため、今回採用に至った。
        
        #region ダブルディスパッチ先

        private void StatusChangeForObject(Control obj, bool status)
        {
            obj.Enabled = status;
        }

        private void StatusChangeForObject(ToolStripStatusLabel obj, bool status)
        {
            obj.Enabled = status;
        }

        #endregion


        /// <summary>
        /// 検索可能コントロールを追加します。
        /// </summary>
        /// <param name="controls"></param>
        public void AddSearchableControls(params Control[] controls)
        {
            foreach (var item in controls)
            {
                this.AddSearchableControl(item);
            }
        }

        /// <summary>
        /// 検索可能コントロールを追加します。
        /// </summary>
        /// <param name="ctl"></param>
        public void AddSearchableControl(Control ctl)
        {
            _controlEnteringObserver.Register(ctl);
        }

        /// <summary>
        /// 検索可能コントロールを追加します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="cellName"></param>
        public void AddSearchableControl(GcMultiRow multiRow, string cellName)
        {
            _controlEnteringObserver.Register(multiRow, cellName);
        }

        /// <summary>
        /// 検索可能コントロールを追加します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="cellNames"></param>
        public void AddSearchableControls(GcMultiRow multiRow, params string[] cellNames)
        {
            foreach (var cellName in cellNames)
            {
                _controlEnteringObserver.Register(multiRow, cellName);
            }
        }

        /// <summary>
        /// 状態を表示するオブジェクトを追加します。
        /// </summary>
        /// <param name="control"></param>
        public void AddStateDisplayObject(Control control)
        {
            _statusObjects.Add(control);
            //初期はfalse
            control.Enabled = false;
        }

        /// <summary>
        /// 状態を表示するオブジェクトを追加します。
        /// </summary>
        /// <param name="component"></param>
        public void AddStateObject(ToolStripStatusLabel component)
        {
            _statusObjects.Add(component);
            //初期はfalse
            component.Enabled = false;
        }

    }
}
