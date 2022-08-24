using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// 現在行の変更をハンドルして通知します。
    /// シートが複数のSPREADは未対応です。
    /// </summary>
    public class CurrentRowChangedEventRaiser
    {
        /// <summary>
        /// 現在行
        /// </summary>
        private int _currentRowIndex;
        /// <summary>
        /// 対象のSPREAD
        /// </summary>
        private FarPoint.Win.Spread.FpSpread _fpSpread;

        /// <summary>
        /// </summary>
        public CurrentRowChangedEventRaiser(FarPoint.Win.Spread.FpSpread fpSpread)
        {
            this._fpSpread = fpSpread;
            this._fpSpread.SelectionChanged += fpSpread_SelectionChanged;
            _currentRowIndex = -1;
        }

        /// <summary>
        /// 手動で行変更を通知するときに呼び出します。
        /// 例えば、AddSelectionでセルの選択を設定した場合はSelectionChangedイベントが発生しないので
        /// このメソッドを呼び出してください.
        /// </summary>
        public void NotifySelectionChanged()
        {
            RaiseEventIfSelectionRowIsChanged();
        }

        /// <summary>
        /// 現在行のインデックスを返します。
        /// </summary>
        public int CurrentRowIndex
        {
            get { return _currentRowIndex; }
        }

        /// <summary>
        /// 現在行が変更されていれば、行変更選択イベントを通知します。
        /// </summary>
        private void RaiseEventIfSelectionRowIsChanged()
        {
            this.RaiseEventIfSelectionRowIsChanged(this._fpSpread.ActiveSheet.ActiveRowIndex);
        }

        /// <summary>
        /// 現在行が変更されていれば、行変更選択イベントを通知します。
        /// </summary>
        private void RaiseEventIfSelectionRowIsChanged(int newRowIndex)
        {
            if (_currentRowIndex != newRowIndex)
            {
                this._currentRowIndex = newRowIndex;

                this.OnCurrentRowChanged(new EventArgs());
            }
        }

        private void fpSpread_SelectionChanged(object sender, FarPoint.Win.Spread.SelectionChangedEventArgs e)
        {
            this.RaiseEventIfSelectionRowIsChanged(e.Range.Row);
        }

        #region イベント

        /// <summary>
        /// </summary>
        public event EventHandler CurrentRowChanged;

        /// <summary>
        /// </summary>
        protected void OnCurrentRowChanged(EventArgs eventArgs)
        {
            if (this.CurrentRowChanged != null)
                this.CurrentRowChanged(this, eventArgs);
        }

        #endregion
    }
}
