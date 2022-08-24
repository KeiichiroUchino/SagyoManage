using GrapeCity.Win.MultiRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Jpsys.SagyoManage.FrameLib.MultiRow;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// コントロールのEnter, Leaveを監視します。
    /// コントロールのEnter, Leaveを検知した場合はイベント経由で通知を行います。
    /// </summary>
    public class ControlEnteringObserver
    {
        //***なんとなくだけど、後々画面のオブジェクトが必要になりそうなので...
        private Form _frame;



        //***編集中のセルは各グリッドコントロール毎に保持した方がいいかも...
        /// <summary>
        /// 編集中のセル
        /// </summary>
        private string _edintingCellName;


        /// <summary>
        /// 監視対象のリスト
        /// </summary>
        private List<ISubject> _subjects = new List<ISubject>();

        /// <summary>
        /// </summary>
        /// <param name="frame"></param>
        public ControlEnteringObserver(Form frame)
        {
            this._frame = frame;
        }

        #region イベント

        /// <summary>
        /// 監視対象がEnterしたときのイベントです。
        /// </summary>
        public event EventHandler<SubjectEventArgs> SubjectEnter;
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSubjectEnter(SubjectEventArgs e)
        {
            if (this.SubjectEnter != null)
                this.SubjectEnter(this, e);
        }

        /// <summary>
        /// 監視対象がLeaveしたときのイベントです。
        /// </summary>
        public event EventHandler<SubjectEventArgs> SubjectLeave;
        /// <summary>
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnSubjectLeave(SubjectEventArgs e)
        {
            if (this.SubjectLeave != null)
                this.SubjectLeave(this, e);
        }

        #endregion

        #region 通常のコントロール

        /// <summary>
        /// 監視対象のコントロールを登録します。
        /// </summary>
        /// <param name="control"></param>
        public void Register(Control control)
        {
            control.Enter += control_Enter;
            control.Leave += control_Leave;


            this.Add(new SubjectControl(control));
        }

        void control_Enter(object sender, EventArgs e)
        {
            OnSubjectEnter(new SubjectEventArgs(((Control)sender).Name));
        }

        void control_Leave(object sender, EventArgs e)
        {
            OnSubjectLeave(new SubjectEventArgs(((Control)sender).Name));
        }

        #endregion

        #region MultirowCel

        /// <summary>
        /// 監視対象の列を登録します。
        /// </summary>
        /// <param name="multiRow"></param>
        /// <param name="cellName"></param>
        public void Register(GcMultiRow multiRow, string cellName)
        {
            this.Add(new SubjectMultirowCell(multiRow, cellName));

            //***イベントをハンドルする条件を限定したい or イベントが発生したときに一定の条件の場合だけ処理したい
            //***Rxならできそう（笑）

            multiRow.Enter += (sender, e) =>
            {
                var mrow = (GcMultiRow)sender;

                if (mrow.HasCurrentCell() && mrow.CurrentCell.Name == cellName)
                    this.OnSubjectEnter(new SubjectEventArgs(mrow.Name + ":" + cellName));
            };

            multiRow.Leave += (sender, e) =>
            {
                var mrow = (GcMultiRow)sender;

                if (mrow.HasCurrentCell() && mrow.CurrentCell.Name == cellName)
                    this.OnSubjectLeave(new SubjectEventArgs(mrow.Name + ":" + cellName));
            };

            multiRow.CellEnter += (sender, e) =>
            {
                var mrow = (GcMultiRow)sender;

                if (mrow.HasCurrentCell() && mrow.Focused && e.CellName == cellName)
                    this.OnSubjectEnter(new SubjectEventArgs(mrow.Name + ":" + cellName));
            };

            //***なぜフォーカスがnullの場合には判断を変えているのか？元々、この分岐はなかった。
            //***しかし、その時に「セルの編集中にほかのセルに移動すると、OnSubjectLeaveが発行されない」現象があり、修正して今に至る。

            //***【なぜその現象が起きたか？】
            //***MultiRowはセルが編集中の場合、フォーカスを取らない。
            //***また、セルの編集中にほかのセルに移動すると
            //*** CellLeave → CellBeginEnd → CellEnter  となり、Focusedプロパティは
            //*** false     → true         → true となる。
            //*** CellLeave時、Focused がfalseになるためにOnSubectLeaveを発行する条件が満たされない。
    
            multiRow.CellLeave += (sender, e) =>
            {
                 var mrow = (GcMultiRow)sender;

                 if (mrow.HasCurrentCell())
                 {
                     if (mrow.Focused)
                     {
                         if (e.CellName == cellName)
                            this.OnSubjectLeave(new SubjectEventArgs(mrow.Name + ":" + cellName));
                     }
                     else
                     {  
                         //***フォーカスを持っていない場合は、編集中のセルかどうかを判断する。
                         if (_edintingCellName == cellName)
                             this.OnSubjectLeave(new SubjectEventArgs(mrow.Name + ":" + cellName));
                     }
                 }
                    
            };

            //***BeginEndで編集中のセルを管理する
            multiRow.CellBeginEdit += (sender, e) =>
            {
                if (e.CellName == cellName)
                    _edintingCellName = cellName;   
            };

            multiRow.CellEndEdit += (sender, e) =>
            {
                if (e.CellName == cellName)
                    _edintingCellName = null;
            };
        }

        #endregion

        private void Add(ISubject subjectControl)
        {
            _subjects.Add(subjectControl);
        }
    }

    interface ISubject
    {
    }

    class SubjectControl : ISubject
    {
        public SubjectControl(Control control)
        {

        }
    }

    class SubjectMultirowCell : ISubject
    {
        public SubjectMultirowCell(GrapeCity.Win.MultiRow.GcMultiRow multiRow, string cellName)
        {

        }
    }

}
