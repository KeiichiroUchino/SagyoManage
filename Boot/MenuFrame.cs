using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Reflection;
using System.Configuration;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.Frame;
using Jpsys.SagyoManage.FrameLib;
using Microsoft.Win32;

namespace Jpsys.SagyoManage.Boot
{
    public partial class MenuFrame : Form
    {

        /// <summary>
        /// メニューの情報をリストで保持します
        /// </summary>
        private IList<MenuItemInfo> menuItemList = null;

        /// <summary>
        /// 起動不可メニューをリストで保持します。
        /// </summary>
        private IList<decimal> ignorAuthMenuList = null;

        /// <summary>
        /// サイドメニューをそのコード順で保持します。
        /// </summary>
        private SortedDictionary<int, TopMenuItemInfo> topMenuTable =
                new SortedDictionary<int, TopMenuItemInfo>();

        /// <summary>
        /// メニューをそのコード順で保持します。
        /// </summary>
        private SortedDictionary<int, MiddleMenuItemInfo> middleMenuTabel =
                new SortedDictionary<int, MiddleMenuItemInfo>();

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューのボタンを押した際に行う処理の種類
        /// </summary>
        private enum ButtonItemKind : int
        {
            /// <summary>
            /// 印刷設定の削除
            /// </summary>
            DeletePrintSettings = 0,
            /// <summary>
            /// 拠点配布用のメニュー
            /// </summary>
            BranchDeploingProgramFile = 1,
            /// <summary>
            /// 現在実行中のフォルダの表示
            /// </summary>
            ExplodingCurrentFolder = 2,
            /// <summary>
            /// 郵便番号辞書の最新化の表示
            /// </summary>
            KenAll = 3,
        }

        private List<ToolStripButton> sideBarList = new List<ToolStripButton>();

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public MenuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 事前に読み込んだメニュー情報のリストと権限別で起動させないメニューの
        /// メニューIDを格納した情報を指定して本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="viewingMenuItemList">表示するメニューの情報を格納したリスト</param>
        /// <param name="ignorMenuList">起動させないメニューのメニューIDを格納したリスト</param>
        public MenuFrame(IList<MenuItemInfo> viewingMenuItemList,
                            IList<decimal> ignorMenuList)
        {
            InitializeComponent();

            this.menuItemList = viewingMenuItemList;
            this.ignorAuthMenuList = ignorMenuList;
        }

        #region 初期化処理

        /// <summary>
        /// MenuFrameを初期化します。
        /// </summary>
        public void InitMenuFrame()
        {

            this.InitMenuTable();
            this.CreateSideMenu();

            //最初のメニューを選択状態にしておく
            ToolStripButton first_top_menu = (ToolStripButton)this.sideBarList[0];
            first_top_menu.Checked = true;
            TopMenuItemInfo wk_topmenu = (TopMenuItemInfo)first_top_menu.Tag;
            this.CreateCenterMenu(wk_topmenu.TopMenuCode);

            this.lblCompanyName.Text =
                 ConfigurationManager.AppSettings["DisplayCompanyName"].Trim();

            this.lblSystemName.Text =
                 ConfigurationManager.AppSettings["DisplaySystemName"].Trim();

            this.lblSystemVersion.Text =
                 ConfigurationManager.AppSettings["DisplaySystemVersion"].Trim();

            this.Icon =
                global::Jpsys.SagyoManage.Boot.
                    Properties.Resources.AppIcon;

            this.panelTop.BackColor = FrameUtilites.GetFrameTitleBackColor();

            this.toolStripStatusLabelLoginUserName.Text =
                Property.UserProperty.GetInstance().LoginOperatorName;

            this.toolStripStatusLabelBootDate.Text =
                 DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
            this.toolStripStatusLabelVersion.Text = string.Empty;

            //ClickOnceかどうかチェックする
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                //ClickOnceだったときはステータスバーにバージョンを表示
                //--現在のClickOnce
                ApplicationDeployment curdp = ApplicationDeployment.CurrentDeployment;
                this.Text = this.Text + " " + curdp.CurrentVersion.ToString();

                this.toolStripStatusLabelVersion.Text =
                    "ClickOnce:" + " " + curdp.CurrentVersion.ToString();


            }
            else
            {

                //ClickOnceではないときには、アプリケーション構成ファイルから表示
                if (ConfigurationManager.AppSettings["LocalVersion"] != null)
                {
                    string wk_depver =
                        ConfigurationManager.AppSettings["LocalVersion"].Trim();

                    this.toolStripStatusLabelVersion.Text =
                        "Local:" + " " + wk_depver;

                }
            }

            //NSK管理用アカウントでログインしている場合は、管理メニューを表示して
            //ステータスバーのログインユーザ名を書き換える
            if (Property.UserProperty.GetInstance().NSKAdminLoginFlag)
            {
                //2014/01/23 UEYAMA
                if (this.toolStripStatusLabelLoginUserName.Text != "")
                {
                    this.toolStripStatusLabelLoginUserName.Text = "システム管理者";

                }
                else
                {
                    this.toolStripStatusLabelLoginUserName.Text = this.toolStripStatusLabelLoginUserName.Text + "(システム管理者)";
                }
                //this.toolStripStatusLabelLoginUserName.Text =
                //    "NSK管理用:" + this.toolStripStatusLabelLoginUserName.Text;
            }


            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

        }

        /// <summary>
        /// メニューの情報を格納する各リストに必要な情報をセットします。
        /// </summary>
        private void InitMenuTable()
        {
            foreach (MenuItemInfo item in this.menuItemList)
            {
                //メニュー本体用

                //--ボタンを作る
                Button bt = new Button();
                bt.Text = item.MiddleMenuName;
                bt.Name = "btnShow" + item.ClassName;
                bt.UseVisualStyleBackColor = true;
                bt.Tag = item;  //MenuItemInfoをボタンのTagに割り当てておく
                bt.Click += new EventHandler(centerMenuButton_Click);
                bt.MouseEnter += new EventHandler(centerMenuButton_MouseEnter);

                //メニューマスタから使用可否を設定
                bt.Enabled = item.EnableFlag;

                //権限をチェックして、使用不可になっているメニューはボタンをDisableに。
                if (this.ignorAuthMenuList != null &&
                    this.ignorAuthMenuList.Contains(item.MenuId))
                {
                    bt.Enabled = false;
                    continue;
                }

                //--メニュー本体を表すクラスを作る
                MiddleMenuItemInfo mditem =
                    new MiddleMenuItemInfo()
                    {
                        MenuId = item.MenuId,
                        MiddleMenuCode = item.MiddleMenuCode,
                        MiddleMenuDescriptionString = item.MenuDescription,
                        MiddleMenuString = item.MiddleMenuName,
                        ActivateClassName = item.ClassName,
                        ShowingCommandButton = bt
                    };

                //トップメニュー（分類メニュー）
                if (!this.topMenuTable.ContainsKey(item.TopMenuCode))
                {
                    //キーがないとき
                    this.topMenuTable[item.TopMenuCode] =
                        new TopMenuItemInfo()
                        {
                            TopMenuCode = item.TopMenuCode,
                            TopMenuString = item.TopMenuName,
                            MiddleMenuItemList = new List<MiddleMenuItemInfo>() { mditem }
                        };

                }
                else
                {
                    //キーが存在するときはメニュー本体をTopメニュー内のリストに入れる
                    this.topMenuTable[item.TopMenuCode].MiddleMenuItemList.Add(mditem);
                }
            }


            //--ボタンを作る
            Button bt_pdel = new Button();
            bt_pdel.Text = "印刷設定の削除";
            bt_pdel.Name = "btnShowDeletePrintSettings";
            bt_pdel.UseVisualStyleBackColor = true;
            bt_pdel.Tag = ButtonItemKind.DeletePrintSettings;
            bt_pdel.Click += new EventHandler(centerMenuButtonProc_Click);
            bt_pdel.MouseEnter += new EventHandler(centerMenuButton_MouseEnter);

            //--メニュー本体を表すクラスを作る
            MiddleMenuItemInfo miditem_pdel =
                new MiddleMenuItemInfo()
                {
                    MenuId = 1,
                    MiddleMenuCode = int.MinValue,
                    MiddleMenuDescriptionString = "",
                    MiddleMenuString = "",
                    ActivateClassName = "",
                    ShowingCommandButton = bt_pdel
                };

            //キーがないとき
            this.topMenuTable[int.MaxValue] =
                new TopMenuItemInfo()
                {
                    TopMenuCode = int.MaxValue,
                    TopMenuString = "管理",
                    MiddleMenuItemList = new List<MiddleMenuItemInfo>() { miditem_pdel }
                };


            //NSK管理者の時のみ
            if (Property.UserProperty.GetInstance().NSKAdminLoginFlag)
            {

                //--ボタンを作る
                Button bt_bdep = new Button();
                bt_bdep.Text = "拠点アプリケーション配布";
                bt_bdep.Name = "btnShowBranchDeploingProgramFile";
                bt_bdep.UseVisualStyleBackColor = true;
                bt_bdep.Tag = ButtonItemKind.BranchDeploingProgramFile;
                bt_bdep.Click += new EventHandler(centerMenuButtonProc_Click);
                bt_bdep.MouseEnter += new EventHandler(centerMenuButton_MouseEnter);

                //--メニュー本体を表すクラスを作る
                MiddleMenuItemInfo miditem_bdep =
                    new MiddleMenuItemInfo()
                    {
                        MenuId = 2,
                        MiddleMenuCode = int.MinValue,
                        MiddleMenuDescriptionString = "",
                        MiddleMenuString = "",
                        ActivateClassName = "",
                        ShowingCommandButton = bt_bdep
                    };


                //キーが存在するときはメニュー本体をTopメニュー内のリストに入れる
                this.topMenuTable[int.MaxValue].MiddleMenuItemList.Add(miditem_bdep);



                //--ボタンを作る
                Button bt_ecur = new Button();
                bt_ecur.Text = "実行中のフォルダの表示";
                bt_ecur.Name = "btnShowExplodingCurrentFolder";
                bt_ecur.UseVisualStyleBackColor = true;
                bt_ecur.Tag = ButtonItemKind.ExplodingCurrentFolder;
                bt_ecur.Click += new EventHandler(centerMenuButtonProc_Click);
                bt_ecur.MouseEnter += new EventHandler(centerMenuButton_MouseEnter);

                //--メニュー本体を表すクラスを作る
                MiddleMenuItemInfo mditem_ecur =
                    new MiddleMenuItemInfo()
                    {
                        MenuId = 3,
                        MiddleMenuCode = int.MinValue,
                        MiddleMenuDescriptionString = "",
                        MiddleMenuString = "",
                        ActivateClassName = "",
                        ShowingCommandButton = bt_ecur
                    };

                //キーが存在するときはメニュー本体をTopメニュー内のリストに入れる
                this.topMenuTable[int.MaxValue].MiddleMenuItemList.Add(mditem_ecur);



                //--ボタンを作る
                Button bt_kena = new Button();
                bt_kena.Text = "郵便番号辞書の最新化";
                bt_kena.Name = "btnKenAll";
                bt_kena.UseVisualStyleBackColor = true;
                bt_kena.Tag = ButtonItemKind.KenAll;
                bt_kena.Click += new EventHandler(centerMenuButtonProc_Click);
                bt_kena.MouseEnter += new EventHandler(centerMenuButton_MouseEnter);

                //--メニュー本体を表すクラスを作る
                MiddleMenuItemInfo mditem_kena =
                    new MiddleMenuItemInfo()
                    {
                        MenuId = 4,
                        MiddleMenuCode = int.MinValue,
                        MiddleMenuDescriptionString = "",
                        MiddleMenuString = "",
                        ActivateClassName = "",
                        ShowingCommandButton = bt_kena
                    };

                //キーが存在するときはメニュー本体をTopメニュー内のリストに入れる
                this.topMenuTable[int.MaxValue].MiddleMenuItemList.Add(mditem_kena);
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// サイドメニューを作ります。
        /// </summary>
        private void CreateSideMenu()
        {
            foreach (TopMenuItemInfo item in this.topMenuTable.Values)
            {
                ToolStripButton tsb = new ToolStripButton(item.TopMenuString, null, sideMenuButton_Click);
                tsb.AutoSize = false;
                tsb.DisplayStyle = ToolStripItemDisplayStyle.Text;
                tsb.CheckOnClick = true;
                tsb.Size = new System.Drawing.Size(160, 30);
                tsb.Tag = item;
                tsb.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));

                this.toolStripButtonMenu.Items.Add(tsb);

                //保持用のリストに入れておく
                this.sideBarList.Add(tsb);

            }

            ////メニューを閉じるを追加する
            //ToolStripButton tsb_menuexit =
            //    new ToolStripButton("メニュー終了", null, menuCloseSideMenuButton_Click);
            //tsb_menuexit.AutoSize = false;
            //tsb_menuexit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            //tsb_menuexit.CheckOnClick = true;
            //tsb_menuexit.Size = new System.Drawing.Size(105, 30);
            //tsb_menuexit.Font =
            //    new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            //this.toolStripButtonMenu.Items.Add(tsb_menuexit);
            ////保持用のリストに入れておく
            //this.sideBarList.Add(tsb_menuexit);

            //サイドメニューの最後に終了ボタンを追加する
            ToolStripButton tsb_exit = new ToolStripButton("終了", null, closeSideMenuButton_Click);
            tsb_exit.AutoSize = false;
            tsb_exit.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsb_exit.CheckOnClick = true;
            tsb_exit.Size = new System.Drawing.Size(160, 30);
            tsb_exit.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.toolStripButtonMenu.Items.Add(tsb_exit);
            //保持用のリストに入れておく
            this.sideBarList.Add(tsb_exit);

        }

        private void centerMenuButton_Click(object sender, EventArgs e)
        {
            try
            {
                //中央メニューのボタンクリックのイベントハンドラ

                //senderからボタンと値を取り出す。
                Button o = (Button)sender;
                MenuItemInfo menu_info = (MenuItemInfo)o.Tag;

                //クラス名を半角スペースで分割
                string[] classNames = menu_info.ClassName.Split(' ');

                //外部アプリケーション対応
                //名前空間が空白の場合は、外部アプリケーション
                if (menu_info.PartOfNameSpace.Equals(string.Empty))
                {
                    //外部アプリケーション起動（VBの場合、起動チェックは各画面で行っている）
                    System.Diagnostics.Process.Start(Path.Combine(Application.StartupPath, menu_info.ClassName));
                }
                else
                {

                    //現在起動中のフォームかどうか調べる
                    foreach (Form item in Application.OpenForms)
                    {
                        //配給入力画面と配給入力（抽出条件）画面の場合
                        if (menu_info.ClassName.Equals("HaikyuNyuryokuConditionFrame") ||
                            menu_info.ClassName.Equals("HaikyuNyuryokuFrame"))
                        {
                            if (menu_info.ClassName.Replace("Condition", "") == item.Name.Replace("Condition", ""))
                            {
                                // 最小化されている場合、元のサイズに戻す
                                if (item.WindowState == FormWindowState.Minimized)
                                {
                                    item.WindowState = FormWindowState.Normal;
                                }

                                //どちらかが存在していれば
                                //アクティブにする
                                item.Activate();

                                //戻る
                                return;
                            }
                        }
                        else
                        {
                            if (menu_info.ClassName == item.Name)
                            {
                                // 最小化されている場合、元のサイズに戻す
                                if (item.WindowState == FormWindowState.Minimized)
                                {
                                    item.WindowState = FormWindowState.Normal;
                                }

                                //存在している
                                //アクティブにする
                                item.Activate();

                                //戻る
                                return;
                            }
                        }
                    }

                    //クラス名からフォームのインスタンスを作成する
                    AssemblyName asm_nm = new AssemblyName(menu_info.PartOfNameSpace);
                    Assembly asm = Assembly.Load(asm_nm);
                    Form f = null;

                    if (classNames.Length == 1)
                    {
                        f =
                            (Form)asm.CreateInstance(menu_info.PartOfNameSpace + "." + classNames[0]);
                    }
                    else
                    {
                        //引数がある画面
                        //引数が文字列であることが前提
                        //引数部分を,で分割
                        string[] args = classNames[1].Split(',');

                        f =
                            (Form)asm.CreateInstance(menu_info.PartOfNameSpace + "." + classNames[0], // 名前空間を含めたクラス名
                                 false, // 大文字小文字を無視するかどうか
                                 BindingFlags.CreateInstance, // インスタンスを生成
                                 null, // 通常はnullでOK,
                                 args, // コンストラクタの引数
                                 null, // カルチャ設定（通常はnullでOK）
                                 null // ローカル実行の場合はnullでOK
                               );
                    }

                    //インスタンスが出来ていない=エラー(メニューの設定が間違っている)
                    if (f == null)
                    {
                        MessageBox.Show(this, menu_info.MiddleMenuName + "画面は開発中です。今しばらくお待ちください。",
                            "開発中", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }

                    //待機状態へ
                    this.Cursor = Cursors.WaitCursor;

                    try
                    {
                        ((IFrameBase)f).InitFrame();
                        f.StartPosition = FormStartPosition.CenterScreen;

                        //表示する前にログを流す
                        //--認証アプリケーション情報を設定
                        AppAuthInfo app_auth = new AppAuthInfo();
                        app_auth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
                        app_auth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
                        app_auth.UserProcessId = f.Name;
                        app_auth.UserProcessName = f.Text;
                        //--ログ出力
                        FrameLogWriter.GetLogger(app_auth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.ShownFrame);

                        //画面のアイコンを設定
                        f.Icon =
                            global::Jpsys.SagyoManage.Boot.
                                    Properties.Resources.AppIcon;

                        ////最大化抑制
                        //f.MaximizeBox = true;
                        //サイズ変更抑制
                        f.FormBorderStyle = FormBorderStyle.FixedSingle;

                        ////表示
                        ////--ダイアログ表示の有無で表示方法を切り替える
                        //if (menu_info.ShowingMode == 1)
                        //{
                        //    f.MaximizeBox = false;
                        //    f.MinimizeBox = false;
                        //    if (!f.IsDisposed) f.ShowDialog(this);
                        //}
                        //else if (menu_info.ShowingMode == 0)
                        //{

                        //    //*** s.arimura 2010/04/02 ダイアログ表示で無い場合は最大化しておく
                        //    f.WindowState = FormWindowState.Maximized;

                        //    if (!f.IsDisposed) f.Show();
                        //}
                        //else if (menu_info.ShowingMode == 2)
                        //{
                        //    //ダイアログでなく最大化でもなく
                        //    if (!f.IsDisposed) f.Show();
                        //}

                        //ダイアログでなく最大化でもなく
                        f.Show();
                    }
                    finally
                    {
                        this.Cursor = Cursors.Default;
                    }
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(
                    "致命的なエラーが発生しました。いったんシステムを終了" +
                    "して再度やり直すか、サポートに連絡してください。" + "\r\n" +
                    err.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //管理メニュー用のボタンを作成する
        private void centerMenuButtonProc_Click(object sender, EventArgs e)
        {

            try
            {
                //中央メニューのボタンクリックのイベントハンドラ

                //senderからボタンと値を取り出す。
                Button o = (Button)sender;
                int menu_kind = (int)o.Tag;

                switch (menu_kind)
                {
                    case (int)ButtonItemKind.DeletePrintSettings:
                        this.DelPrintSetting();
                        break;
                    case (int)ButtonItemKind.BranchDeploingProgramFile:
                        this.BranchDeploingProgramFile2Folder();
                        break;
                    case (int)ButtonItemKind.ExplodingCurrentFolder:
                        this.ExplodingCurrentFolder();
                        break;
                    case (int)ButtonItemKind.KenAll:
                        this.KenAll();
                        break;
                    default:
                        break;
                }
            }
            catch (Exception err)
            {
                MessageBox.Show(
                    "致命的なエラーが発生しました。いったんシステムを終了" +
                    "して再度やり直すか、サポートに連絡してください。" + "\r\n" +
                    err.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        /// <summary>
        /// 表示中央のメニューを作ります。
        /// </summary>
        private void CreateCenterMenu(int topMenuCd)
        {
            //TableLayoutPanelをクリアーする
            this.tableLayoutPanel1.Controls.Clear();

            //画面下部の説明文をクリア
            this.lblAppDiscription.Text = string.Empty;

            //該当するサイドメニューから内部に表示するメニューを取り出す
            List<MiddleMenuItemInfo> wk_mdmenu_list =
                this.topMenuTable[topMenuCd].MiddleMenuItemList;

            //メニュー数を取り出す
            int menu_count = wk_mdmenu_list.Count;

            //メニューの数だけループ
            for (int i = 0; i < menu_count; i++)
            {
                //行数をセットする
                int set_row = i;
                //列数をセットする
                int set_col = 0;

                //10行を超えた時点で次の列に移り行数から９を減算する
                if (i > (10 - 1))
                {
                    //２列目に・・・
                    set_col = 1;
                    //Index値から９を引いて、行数を求める
                    set_row = i - 10;
                }

                //メニューを取り出す
                MiddleMenuItemInfo mdm_info = wk_mdmenu_list[i];
                //ボタンを取り出しプロパティを変更する
                Button wk_btn = mdm_info.ShowingCommandButton;
                wk_btn.Dock = DockStyle.Fill;
                wk_btn.Font = new System.Drawing.Font("メイリオ", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
                //TableLayoutPanelにセット
                this.tableLayoutPanel1.Controls.Add(wk_btn, set_col, set_row);
            }
        }

        /// <summary>
        /// 拠点用の配布フォルダへプログラムをコピーします。
        /// </summary>
        private void BranchDeploingProgramFile2Folder()
        {
            //処理に成功したかどうか
            bool result_bool = true;
            //処理完了後に表示するメッセージ
            string result_dialog_msg = string.Empty;

            DialogResult d_result =
                MessageBox.Show(
                this,
                "拠点用の配布フォルダへプログラムをコピーしますか？",
                "確認",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (d_result == DialogResult.No)
            {
                //Noなら処理せずに抜ける。
                return;
            }


            //多段IFでコーディングする・・・ほんとは嫌だけど時間切れ

            //現在実行中のフォルダを取得する。
            string start_path = Application.StartupPath;

            //構成ファイルのプロパティチェック
            if (ConfigurationManager.AppSettings["BranchDeploingFolder"] != null)
            {
                //構成ファイルからコピー先を取得
                string wk_destpath =
                    ConfigurationManager.AppSettings["BranchDeploingFolder"].Trim();
                try
                {
                    //構成ファイルの内容チェック
                    if (wk_destpath.Trim().Length == 0)
                    {

                        //はいってない。
                        throw new NSKArgumentException(
                            "構成ファイルの、BranchDeploingFolderに値が設定されていません。");
                    }

                    Microsoft.VisualBasic.FileIO.FileSystem.CopyDirectory(
                        start_path, wk_destpath,
                        Microsoft.VisualBasic.FileIO.UIOption.AllDialogs,
                        Microsoft.VisualBasic.FileIO.UICancelOption.ThrowException);

                    result_dialog_msg = "コピーに成功しました。";
                }
                catch (OperationCanceledException err)
                {
                    //処理結果をFalseに
                    result_bool = false;
                    result_dialog_msg = err.Message;
                }
                catch (SystemException err)
                {
                    //処理結果をFalseに
                    result_bool = false;
                    result_dialog_msg = err.Message;
                }
                catch (NSKException err)
                {
                    //処理結果をFalseに
                    result_bool = false;
                    result_dialog_msg = err.Message;
                }
                catch (Exception err)
                {
                    throw err;
                }

            }
            else
            {
                //構成ファイルに定義がないとき
                result_bool = false;
                result_dialog_msg = "構成ファイルに、BranchDeploingFolderの定義がありません。";
            }

            if (result_bool)
            {
                MessageBox.Show(this, "拠点用の展開先フォルダへのコピーに成功しました。",
                    "NSK管理用", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show(this,
                    "拠点用の展開先フォルダへのコピーに失敗しました。" + "\r\n" +
                    result_dialog_msg,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 印刷設定の削除を行う。
        /// </summary>
        private void DelPrintSetting()
        {
            //確認メッセージ
            DialogResult d_result =
                MessageBox.Show(this,
                                "印刷設定を削除しますか？" + "\r\n" + "（全ての印刷設定が削除されます)"
                                + "\r\n\r\n"
                                + "※個別に削除する場合は、各印刷指示画面の「印刷」メニュー内にある「印刷設定の削除」"
                                + "を使用してください。"
                                ,
                                "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);
            if (d_result == DialogResult.No)
            {
                return;
            }

            //印刷設定のフォルダを取得する
            string wk_path =
                Property.SystemProperty.GetInstance().ReportPrinterSettingPath;
            try
            {
                //フォルダごとごっそり消す。
                if (System.IO.Directory.Exists(wk_path))
                {
                    System.IO.Directory.Delete(wk_path, true);
                }

                //消し終わったらメッセージを出す
                MessageBox.Show(this,
                    "印刷設定を削除しました。",
                    Property.SystemProperty.GetInstance().ProductName,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception err)
            {
                MessageBox.Show(this,
                    "設定の削除に失敗しました。システムを再起動してもう一度やり直すか。" +
                    "システム管理者に連絡してください。" + "\r\n" + err.Message,
                    "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExplodingCurrentFolder()
        {
            //現在実行中のフォルダを表示する。
            string start_path = Application.StartupPath;
            System.Diagnostics.Process.Start(start_path);

        }

        private void KenAll()
        {
            //using (KenAllCsvImportFrame f = new KenAllCsvImportFrame())
            //{
            //    f.InitFrame();
            //    f.ShowDialog(this);
            //}
        }

        void centerMenuButton_MouseEnter(object sender, EventArgs e)
        {
            //中央メニューのマウスえんたーのイベントハンドら
            //senderからボタンと値を取り出す。
            Button o = (Button)sender;
            //MenuItemInfo menu_info = (MenuItemInfo)o.Tag;

            MenuItemInfo menu_info = o.Tag as MenuItemInfo;
            if (menu_info != null)
            {
                this.lblAppDiscription.Text = menu_info.MenuDescription;
            }
            else
            {
                this.lblAppDiscription.Text = "";
            }
        }

        private void sideMenuButton_Click(object sender, EventArgs e)
        {
            //サイドメニューのボタンのクリックのイベントハンドラ

            //senderから、イベント送信元のToolStripButtonを取り出す
            ToolStripButton wk_tsb = (ToolStripButton)sender;
            //tagに入っているTopMenuItemInfoを取り出して処理を判断する
            TopMenuItemInfo wk_topmenu = (TopMenuItemInfo)wk_tsb.Tag;

            //表示中央のメニューを作る
            this.CreateCenterMenu(wk_topmenu.TopMenuCode);

            //クリックされたボタン以外のボタンのチェック状態は解除
            foreach (ToolStripButton item in this.sideBarList)
            {
                if (!item.Equals(wk_tsb))
                {
                    item.Checked = false;
                }
            }

        }

        private void menuCloseSideMenuButton_Click(object sender, EventArgs e)
        {
            //サイドメニューメニューを閉じるボタンのイベントハンドラ

            //senderから、イベント送信元のToolStripButtonを取り出す
            ToolStripButton wk_tsb = (ToolStripButton)sender;

            //普通のボタンを同じ動きにするため、全部のボタンのチェックは解除
            foreach (ToolStripButton item in this.sideBarList)
            {
                item.Checked = false;
            }

            this.Close();
        }

        private void closeSideMenuButton_Click(object sender, EventArgs e)
        {
            //senderから、イベント送信元のToolStripButtonを取り出す
            ToolStripButton wk_tsb = (ToolStripButton)sender;

            //普通のボタンを同じ動きにするため、全部のボタンのチェックは解除
            foreach (ToolStripButton item in this.sideBarList)
            {
                item.Checked = false;
            }

            ////サイドメニューの終了ボタンのイベントハンドら
            //Application.Exit();
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabelBootDate.Text =
                 DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        #endregion


        #region プライベートクラス

        public class TopMenuItemInfo
        {
            public TopMenuItemInfo()
            {
                this.MiddleMenuItemList = new List<MiddleMenuItemInfo>();
            }

            public int TopMenuCode { get; set; }

            public string TopMenuString { get; set; }

            public List<MiddleMenuItemInfo> MiddleMenuItemList { get; set; }

        }

        public class MiddleMenuItemInfo
        {
            public decimal MenuId { get; set; }

            public int MiddleMenuCode { get; set; }

            public string MiddleMenuString { get; set; }

            public string MiddleMenuDescriptionString { get; set; }

            public string ActivateClassName { get; set; }

            public Button ShowingCommandButton { get; set; }
        }

        #endregion

        private void InMdiMenuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                List<Form> closingList =
                    new List<Form>();

                foreach (Form item in Application.OpenForms)
                {
                    if (item is MenuFrame)
                    {
                        continue;
                    }
                    closingList.Add(item);
                }

                foreach (Form item in closingList)
                {
                    //フォームを閉じる
                    item.Close();
                }

                foreach (Form item in Application.OpenForms)
                {
                    if (!(item is MenuFrame))
                    {
                        //閉じられていないフォームがある場合
                        e.Cancel = true;
                        return;
                    }
                }

                //閉じる前にアラートを出す。
                string msg = "アプリケーションを終了しますか？";
                DialogResult d_result =
                    MessageBox.Show(this, msg, "確認", MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (d_result == DialogResult.No)
                {
                    e.Cancel = true;
                }
                else
                {
                    //利用者ログイン時間初期化
                    this.InitOperatorLoginYMD();
                }
            }
        }

        private void InitOperatorLoginYMD()
        {
            try
            {
                //利用者情報取得
                Operator _Operator = new Operator();
                OperatorInfo op_info = _Operator.GetInfoById(this.appAuth.OperatorId);

                //利用者情報が存在する場合
                if (op_info != null)
                {
                    //ログイン日時初期化
                    op_info.LoginYMD = DateTime.MinValue;

                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        _Operator.SaveLoginYMD(tx, op_info);
                    });
                }
            }
            catch
            {
                //何もしない
            }
        }

        private void MenuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.F1:
                    SendKeys.Send("%{F4}");
                    break;
                default:
                    break;
            }
        }

        private void MenuFrame_Load(object sender, EventArgs e)
        {
            //イベントをイベントハンドラに関連付ける
            SystemEvents.SessionEnding +=
                new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        private void MenuFrame_FormClosed(object sender, FormClosedEventArgs e)
        {
            //イベントを解放する
            SystemEvents.SessionEnding -=
                new SessionEndingEventHandler(SystemEvents_SessionEnding);
        }

        //ログオフ、シャットダウンしようとしているとき
        private void SystemEvents_SessionEnding(object sender, SessionEndingEventArgs e)
        {
            //利用者ログイン時間初期化
            this.InitOperatorLoginYMD();
        }
    }
}
