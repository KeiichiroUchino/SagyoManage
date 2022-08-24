using GrapeCity.Win.MultiRow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.MultiRow
{
    /// <summary>
    /// 初期設定をサポートします。
    /// </summary>
    public static class InitialSettingHelper
    {
        /// <summary>
        /// ショートカットキーの初期設定を行います。
        /// MultiRowコントロール内で完結する設定だけ行われます。
        /// MultiRow外のコンテキストを参照する設定は各画面で対応してください。
        /// </summary>
        /// <param name="multirRow"></param>
        public static void InitialShortcutKeySetting(this GcMultiRow multirRow)
        {
            //F2にセルの編集のショートカット割り当てるように変更
            //---規定値でF2は編集モードになっています T.Kuroki@NSK

            //--Escのショートカットキーを無効に設定
            multirRow.ShortcutKeyManager.Unregister(Keys.Escape);

            //--セルの編集の制御を追加
            //---Enter制御を追加
            multirRow.ShortcutKeyManager.Unregister(Keys.Enter);
            multirRow.ShortcutKeyManager.Register(
                SelectionActions.MoveToNextCellThenControl, Keys.Enter);

            //--Tab制御を追加
            multirRow.ShortcutKeyManager.Unregister(Keys.Tab);
            multirRow.ShortcutKeyManager.Register(
                SelectionActions.MoveToNextCellThenControl, Keys.Tab);

            //--Shift+Tab制御を追加
            multirRow.ShortcutKeyManager.Unregister(Keys.Shift | Keys.Tab);
            multirRow.ShortcutKeyManager.Register(
                SelectionActions.MoveToPreviousCellThenControl, Keys.Shift | Keys.Tab);

            //--上キー制御を追加（Shift+Tabと同じ動作）
            multirRow.ShortcutKeyManager.Unregister(Keys.Up);
            multirRow.ShortcutKeyManager.Register(
                SelectionActions.MoveToPreviousCellThenControl, Keys.Up);

            //--下キー制御を追加（Enter、Tabキーと同じ動作）
            multirRow.ShortcutKeyManager.Unregister(Keys.Down);
            multirRow.ShortcutKeyManager.Register(
                SelectionActions.MoveToNextCellThenControl, Keys.Down);

            //--左キー制御を追加
            multirRow.ShortcutKeyManager.Unregister(Keys.Left);
            multirRow.ShortcutKeyManager.Register(
                EditingActions.BeginEdit, Keys.Left);

            //--右キー制御を追加
            multirRow.ShortcutKeyManager.Unregister(Keys.Right);
            multirRow.ShortcutKeyManager.Register(
                EditingActions.BeginEdit, Keys.Right);
            
            //--貼り付けを削除
            multirRow.ShortcutKeyManager.Unregister(Keys.Control | Keys.V);

            //--切り取りを削除
            multirRow.ShortcutKeyManager.Unregister(Keys.Control | Keys.X);
            multirRow.ShortcutKeyManager.Unregister(Keys.Shift | Keys.Delete);

            //--DeleteキーのClearアクションを再設定
            multirRow.ShortcutKeyManager.Unregister(Keys.Delete);
            multirRow.ShortcutKeyManager.Register(new ClearEditAction(), Keys.Delete);

            //--切り取り、貼り付け制御を削除。
            multirRow.ShortcutKeyManager.Unregister(EditingActions.Cut);
            multirRow.ShortcutKeyManager.Unregister(EditingActions.Paste);
        }
    }

    /// <summary>
    /// CellClear処理を行うユーザ定義アクションクラスです。
    /// </summary>
    internal class ClearEditAction : IAction
    {
        #region IAction メンバ

        public bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        public string DisplayName
        {
            get { return this.ToString(); }
        }

        public void Execute(GcMultiRow target)
        {
            //Deleteキーが押された場合の処理          
            //セルが非編集モードの場合
            if (target.CurrentCell.IsInEditMode == false)
            {
                //セルの編集を開始する
                EditingActions.BeginEdit.Execute(target);
                if (target.CurrentCell.IsInEditMode)
                {
                    EditingActions.ClearEdit.Execute(target);

                }
            }
        }

        #endregion
    }
}
