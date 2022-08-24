using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 操作メニューの項目を定義した列挙体です。
    /// </summary>
    public enum ActionMenuItems : int
    {
        /// <summary>
        /// コード変更
        /// </summary>
        ChangeCode,
        /// <summary>
        /// 新規作成 [F2]
        /// </summary>
        New,
        /// <summary>
        /// CSV出力 [F8]
        /// </summary>
        ExportCsv,
        /// <summary>
        /// CSV取込 [F8]
        /// </summary>
        ImportCsv,
        /// <summary>
        /// 印刷 [F7]
        /// </summary>
        PrintToPrinter,
        /// <summary>
        /// プレビュー [F8]
        /// </summary>
        PrintToScreen,
        /// <summary>
        /// 編集取消 [F6]
        /// </summary>
        EditCancel,
        /// <summary>
        /// 削除 [Shift+F8]
        /// </summary>
        Delete,
        /// <summary>
        /// 選択 [F7]
        /// </summary>
        Select,
        /// <summary>
        /// 終了 [F1]
        /// </summary>
        Close,
        /// <summary>
        /// 保存 [F7]
        /// </summary>
        Update,
        /// <summary>
        /// セパレータ
        /// </summary>
        Separator,
        /// <summary>
        /// 全明細削除 [Shift+F8]
        /// </summary>
        DeleteAllDtl,
        /// <summary>
        /// 分割 [F3]
        /// </summary>
        Copy,
        /// <summary>
        /// 送信 [F7]
        /// </summary>
        Send,
        /// <summary>
        /// 印刷設定の削除
        /// </summary>
        DelPrintSetting,
    }
    
    /// <summary>
    /// 印刷操作メニューの項目を定義した列挙体です。
    /// 主に印刷指示画面で使用するメニュー項目です。
    /// </summary>
    public enum PrintActionMenuItems : int
    {
        /// <summary>
        /// 終了 [F1]
        /// </summary>
        Close,
        /// <summary>
        /// 印刷プレビュー [F7]
        /// </summary>
        PrintToScreen,
        /// <summary>
        /// セパレータ
        /// </summary>
        Separator,
        /// <summary>
        /// 印刷設定の削除
        /// </summary>
        DelPrintSetting,
        /// <summary>
        /// 実行 [F7]
        /// </summary>
        ExecProc,
    }

    /// <summary>
    /// 操作メニューのクリックイベントのイベントデータを格納するクラスです。
    /// </summary>
    public class ActionMenuClickEventArgs : EventArgs
    {
        private ActionMenuItems _clickedMenuItem;

        /// <summary>
        /// 操作メニューの項目を表す列挙体を指定して、本イベントデータクラスの
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="clickedMenuItem">操作メニューの項目を表す列挙体</param>
        public ActionMenuClickEventArgs(ActionMenuItems clickedMenuItem)
        {
            this._clickedMenuItem = clickedMenuItem;
        }

        /// <summary>
        /// クリックされた項目に該当する操作メニューの列挙体の値を
        /// 取得します。
        /// </summary>
        public ActionMenuItems ClickedMenuItem
        {
            get { return this._clickedMenuItem; }
        }
    }

    /// <summary>
    /// 印刷操作メニューのクリックイベントのイベントデータを格納するクラスです。
    /// </summary>
    public class PrintActionMenuClickEventArgs : EventArgs
    {
        private PrintActionMenuItems _clickedMenuItem;

        /// <summary>
        /// 印刷操作メニューの項目を表す列挙体を指定して、本イベントデータクラスの
        /// インスタンスを初期化します。
        /// </summary>
        /// <param name="clickedMenuItem">印刷操作メニューの項目を表す列挙体</param>
        public PrintActionMenuClickEventArgs(PrintActionMenuItems clickedMenuItem)
        {
            this._clickedMenuItem = clickedMenuItem;
        }

        /// <summary>
        /// クリックされた項目に該当する印刷操作メニューの列挙体の値を
        /// 取得します。
        /// </summary>
        public PrintActionMenuItems ClickedMenuItem
        {
            get { return this._clickedMenuItem; }
        }
    }

    /// <summary>
    /// フォームのMenuStripに追加する操作メニューを保持し、必要な
    /// MenuStripに操作メニューを追加および追加したメニューを制御
    /// する一連の機能を提供するクラスです。
    /// </summary>
    public class AddActionMenuItem
    {
        /// <summary>
        /// メニューのクリックイベント発生用のイベントデリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void ActionMenuClickDelagate(object sender, ActionMenuClickEventArgs e);

        /// <summary>
        /// 操作メニューのいずれかの項目がクリックされたときに発生するイベントです。
        /// </summary>
        public event ActionMenuClickDelagate ActionMenuClick;

        /// <summary>
        /// 操作メニューのいずれかの項目がクリックされたときのイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        public void OnActionMenuClick(ActionMenuClickEventArgs e)
        {
            if (ActionMenuClick != null)
                ActionMenuClick(this, e);

        }

        /// <summary>
        /// 表示対象のメニューをメニューオブジェクトをキーにして、そのメニューを
        /// 表す列挙体の値を格納します。
        /// （クリックイベントのイベント発生時に使用）
        /// </summary>
        private Dictionary<ToolStripItem, ActionMenuItems> _menuItemTableByMenu =
            new Dictionary<ToolStripItem, ActionMenuItems>();

        /// <summary>
        /// 表示対象のメニューをそのメニューを表す列挙体の値をキーにして、メニュー
        /// オブジェクトを格納します。
        /// （メニューの項目を指定した処理に使用）
        /// </summary>
        private Dictionary<ActionMenuItems, ToolStripItem> _menuItemTableByEnums =
            new Dictionary<ActionMenuItems, ToolStripItem>();

        /// <summary>
        /// 表示対象のメニューのメニューオブジェクトを格納します。
        /// （メインのメニューに追加するときに使用）
        /// </summary>
        private List<ToolStripItem> _menuItemList = new List<ToolStripItem>();

        /// <summary>
        /// 操作メニューに表示したいメニュー項目の列挙体を指定して、本クラスのインスタンス
        /// にメニューの作成を指示します。
        /// </summary>
        /// <param name="selectItems">表示したいメニュー項目を表す列挙体</param>
        public void SetCreatingItem(ActionMenuItems selectItems)
        {
            ToolStripItem m_item;

            #region メニュー項目で分岐

            switch (selectItems)
            {
                case ActionMenuItems.New:
                    m_item = new ToolStripMenuItem("新規作成", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F2;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.ExportCsv:
                    m_item = new ToolStripMenuItem("CSV出力", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys =
                        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.ImportCsv:
                    m_item = new ToolStripMenuItem("CSV取込", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F8;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.ChangeCode:
                    m_item = new ToolStripMenuItem("コード変更", null, new EventHandler(m_item_Click));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.PrintToScreen:
                    m_item = new ToolStripMenuItem("プレビュー", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F8;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.PrintToPrinter:
                    m_item = new ToolStripMenuItem("印刷", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.EditCancel:
                    m_item = new ToolStripMenuItem("編集取消", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys =
                        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F6)));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Delete:
                    m_item = new ToolStripMenuItem("削除", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = 
                        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Close:
                    m_item = new ToolStripMenuItem("終了", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F1;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Update:
                    m_item = new ToolStripMenuItem("保存", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Select:
                    m_item = new ToolStripMenuItem("選択", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Separator:
                    m_item = new System.Windows.Forms.ToolStripSeparator();
                    // セパレータは、有効無効を切り替えたりイベントを取得する必要が
                    // ないので、_menuItemListにのみ、追加。
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.DeleteAllDtl:
                    m_item = new ToolStripMenuItem("全明細削除", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys =
                        ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Shift | System.Windows.Forms.Keys.F8)));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Copy:
                    m_item = new ToolStripMenuItem("分割", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F3;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.Send:
                    m_item = new ToolStripMenuItem("送信", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);
                    break;
                case ActionMenuItems.DelPrintSetting:
                    m_item = new ToolStripMenuItem("印刷設定の削除", null, new EventHandler(m_item_Click));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);

                    break;
                default:
                    break;
            }

            #endregion
        }

        /// <summary>
        /// 操作メニューを追加するMenuStripを指定して、指定した
        /// MenuStripに操作メニューを追加します。
        /// </summary>
        /// <param name="topMenu"></param>
        public void AddMenu(MenuStrip topMenu)
        {
            //操作メニューのトップメニューを作る
            ToolStripMenuItem wk_menu =
                new ToolStripMenuItem("操作(&A)", null);

            wk_menu.MergeAction = MergeAction.MatchOnly;


            //メニューの作成指示順に格納したリストを大回転
            foreach (ToolStripItem item in this._menuItemList)
            {
                //操作メニューのトップメニューに追加
                //topMenu.Items.Add(item);
                wk_menu.DropDownItems.Add(item);
            }
            //指定されたメニューに追加する。
            topMenu.Items.Add(wk_menu);
        }

        /// <summary>
        /// 指定したメニュー項目を取得します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ToolStripItem GetMenuItemBy(ActionMenuItems item)
        {
            return _menuItemTableByEnums[item];
        }

        /// <summary>
        /// 操作メニュー内のメニューの有効無効を切り替えます。
        /// </summary>
        /// <param name="selectItems">有効無効を切り替えるメニュー項目を表す列挙体</param>
        /// <param name="enableVal">有効無効の値</param>
        public void ActionMenuItemEnable(ActionMenuItems selectItems, bool enableVal)
        {
            this._menuItemTableByEnums[selectItems].Enabled = enableVal;
        }

        /// <summary>
        /// 動的に作成した操作メニューの各項目のクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_item_Click(object sender, EventArgs e)
        {
            //イベントの発生元のToolStripMenuItemオブジェクトをキーにして、
            //リストから該当する操作メニューの項目を表す列挙体の値を
            //取得して、イベント発生関数を起動する。
            if (this._menuItemTableByMenu.ContainsKey((ToolStripMenuItem)sender))
            {
                ToolStripMenuItem key = (ToolStripMenuItem)sender;
                ActionMenuItems wk_enum = this._menuItemTableByMenu[key];
                ActionMenuClickEventArgs eArgs = new ActionMenuClickEventArgs(wk_enum);
                //イベント起動
                this.OnActionMenuClick(eArgs);
            }
        }
    }

    /// <summary>
    /// フォームのMenuStripに追加する印刷操作メニューを保持し、必要な
    /// MenuStripに印刷操作メニューを追加および、追加したメニューを制
    /// 御する一連の機能を提供するクラスです。
    /// </summary>
    public class AddPrintActionMenuItem
    {
        /// <summary>
        /// メニューのクリックイベント発生用のイベントデリゲート
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public delegate void PrintActionMenuClickDelagate(object sender, PrintActionMenuClickEventArgs e);

        /// <summary>
        /// 印刷操作メニューのいずれかの項目がクリックされたときに発生するイベントです。
        /// </summary>
        public event PrintActionMenuClickDelagate PrintActionMenuClick;

        /// <summary>
        /// 印刷操作メニューのいずれかの項目がクリックされたときのイベントを発生させます。
        /// </summary>
        /// <param name="e">イベントデータ</param>
        public void OnPrintActionMenuClick(PrintActionMenuClickEventArgs e)
        {
            if (PrintActionMenuClick != null)
                PrintActionMenuClick(this, e);
        }

        /// <summary>
        /// 表示対象のメニューをメニューオブジェクトをキーにして、そのメニューを
        /// 表す列挙体の値を格納します。
        /// （クリックイベントのイベント発生時に使用）
        /// </summary>
        private Dictionary<ToolStripItem, PrintActionMenuItems> _menuItemTableByMenu =
            new Dictionary<ToolStripItem, PrintActionMenuItems>();

        /// <summary>
        /// 表示対象のメニューをそのメニューを表す列挙体の値をキーにして、メニュー
        /// オブジェクトを格納します。
        /// （メニューの項目を指定した処理に使用）
        /// </summary>
        private Dictionary<PrintActionMenuItems, ToolStripItem> _menuItemTableByEnums =
            new Dictionary<PrintActionMenuItems, ToolStripItem>();

        /// <summary>
        /// 表示対象のメニューのメニューオブジェクトを格納します。
        /// （メインのメニューに追加するときに使用）
        /// </summary>
        private List<ToolStripItem> _menuItemList = new List<ToolStripItem>();

        /// <summary>
        /// 印刷操作メニューに表示したいメニュー項目の列挙体を指定して、本クラスのインスタンス
        /// にメニューの作成を指示します。
        /// </summary>
        /// <param name="selectItems">表示したいメニュー項目を表す列挙体</param>
        public void SetCreatingItem(PrintActionMenuItems selectItems)
        {
            ToolStripItem m_item;

            #region メニュー項目で分岐

            switch (selectItems)
            {
                case PrintActionMenuItems.Close:
                    m_item = new ToolStripMenuItem("終了", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F1;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);

                    break;
                case PrintActionMenuItems.PrintToScreen:
                    m_item = new ToolStripMenuItem("印刷プレビュー", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);

                    break;
                case PrintActionMenuItems.DelPrintSetting:
                    m_item = new ToolStripMenuItem("印刷設定の削除", null, new EventHandler(m_item_Click));
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);

                    break;
                case PrintActionMenuItems.ExecProc:
                    m_item = new ToolStripMenuItem("実行", null, new EventHandler(m_item_Click));
                    ((ToolStripMenuItem)m_item).ShortcutKeys = System.Windows.Forms.Keys.F7;
                    this._menuItemTableByMenu.Add(m_item, selectItems);
                    this._menuItemTableByEnums.Add(selectItems, m_item);
                    this._menuItemList.Add(m_item);

                    break;
                case PrintActionMenuItems.Separator:
                    m_item = new System.Windows.Forms.ToolStripSeparator();
                    // セパレータは、有効無効を切り替えたりイベントを取得する必要が
                    // ないので、_menuItemListにのみ、追加。
                    this._menuItemList.Add(m_item);

                    break;
                default:
                    break;
            }

            #endregion
        }

        /// <summary>
        /// 印刷操作メニューを追加するMenuStripを指定して、指定した
        /// MenuStripに操作メニューを追加します。
        /// </summary>
        /// <param name="topMenu"></param>
        public void AddMenu(MenuStrip topMenu)
        {
            // 印刷操作メニューのトップメニューを作る
            ToolStripMenuItem wk_menu =
                new ToolStripMenuItem("操作(&A)", null);

            wk_menu.MergeAction = MergeAction.MatchOnly;


            // メニューの作成指示順に格納したリストを大回転
            foreach (ToolStripItem item in this._menuItemList)
            {
                // 印刷操作メニューのトップメニューに追加
                wk_menu.DropDownItems.Add(item);
            }

            // 指定されたメニューに追加する
            topMenu.Items.Add(wk_menu);
        }

        /// <summary>
        /// 指定したメニュー項目を取得します。
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public ToolStripItem GetMenuItemBy(PrintActionMenuItems item)
        {
            return _menuItemTableByEnums[item];
        }

        /// <summary>
        /// 印刷操作メニュー内のメニューの有効無効を切り替えます。
        /// </summary>
        /// <param name="selectItems">有効無効を切り替えるメニュー項目を表す列挙体</param>
        /// <param name="enableVal">有効無効の値</param>
        public void PrintActionMenuItemEnable(PrintActionMenuItems selectItems, bool enableVal)
        {
            this._menuItemTableByEnums[selectItems].Enabled = enableVal;
        }

        /// <summary>
        /// 動的に作成した印刷操作メニューの各項目のクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void m_item_Click(object sender, EventArgs e)
        {
            // イベントの発生元のToolStripMenuItemオブジェクトをキーにして、
            // リストから該当する印刷操作メニューの項目を表す列挙体の値を
            // 取得して、イベント発生関数を起動する。
            if (this._menuItemTableByMenu.ContainsKey((ToolStripMenuItem)sender))
            {
                ToolStripMenuItem key = (ToolStripMenuItem)sender;
                PrintActionMenuItems wk_enum = this._menuItemTableByMenu[key];
                PrintActionMenuClickEventArgs eArgs = new PrintActionMenuClickEventArgs(wk_enum);
                //イベント起動
                this.OnPrintActionMenuClick(eArgs);
            }
        }
    }
}
