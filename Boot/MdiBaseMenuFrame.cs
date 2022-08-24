using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Deployment.Application;
using System.Configuration;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.Frame;
using Jpsys.SagyoManage.FrameLib;

namespace Jpsys.SagyoManage.Boot
{
    public partial class MdiBaseMenuFrame : Form
    {
        #region ユーザ定義

        /// <summary>
        /// MDIフォーム内に表示するメニューのインスタンスを保持
        /// </summary>
        private MenuFrame showingMdiMenu;

        /// <summary>
        /// 現在表示中のメニュー項目のインスタンスを保持
        /// </summary>
        private IList<MenuItemInfo> currentMenuItemList;

        /// <summary>
        /// 表示しないメニューのIDを保持
        /// </summary>
        private IList<decimal> currentIgnorMenuList;

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// お知らせ機能における、お知らせ存在確認の間隔（分単位）
        /// </summary>
        private decimal _informationCheckInterval;


        #endregion

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public MdiBaseMenuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// MainMenuを初期化します。
        /// </summary>
        /// <param name="mdiMenu"></param>
        /// <param name="viewingMenuItemList"></param>
        /// <param name="ignorMenuList"></param>
        public void InitMainMenu(MenuFrame mdiMenu,
                            IList<MenuItemInfo> viewingMenuItemList,
                            IList<decimal> ignorMenuList)
        {
            showingMdiMenu = mdiMenu;
            this.currentMenuItemList = viewingMenuItemList;
            this.currentIgnorMenuList = ignorMenuList;

            //////アイコン
            this.Icon =
                global::Jpsys.SagyoManage.Boot.
                    Properties.Resources.AppIcon;

            //2014/01/23 UEYAMA
            //this.toolStripStatusLabelLoginUserName.Text =
            //    "ログイン:" + Forms.Property.UserProperty.GetInstance().LoginOperatorName;
            this.toolStripStatusLabelLoginUserName.Text =
                Property.UserProperty.GetInstance().LoginOperatorName;
            
            this.toolStripStatusLabelBootDate.Text =
                 DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString();
            this.toolStripStatusLabelVersion.Text = string.Empty;

            //管理メニューはここでは非表示にしておく（後でチェックがあるため）
            NSKMenuToolStripMenuItem.Visible = false;


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
            if (
                Property.UserProperty.GetInstance().NSKAdminLoginFlag)
            {
                
                //2014/01/23 UEYAMA
                this.toolStripStatusLabelLoginUserName.Text =
                    this.toolStripStatusLabelLoginUserName.Text + "(NSK管理用)" ;
                //this.toolStripStatusLabelLoginUserName.Text =
                //    "NSK管理用:" + this.toolStripStatusLabelLoginUserName.Text;

                NSKMenuToolStripMenuItem.Visible = true;
            }


            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;


            ////お知らせチェックのメソッドをFrame側に渡しておく
            ////--例外的に匿名メソッドを使用する(T.Kuroki)
            //FrameUtilites.SetInformationCheckAction(
            //    delegate() { this.DoInformationChecking(); });
        }

        #region パブリックメソッド

        /// <summary>
        /// お知らせ情報のチェックを行います。主にタイマーから呼び出されますが、
        /// お知らせ確認画面からもデリゲートを経由して呼び出されることがあります。
        /// </summary>
        public void DoInformationChecking()
        {
            //---バックグラウンド処理が実行中でないときにかぎる。
            if (!this.bgwInformationCheck.IsBusy)
            {
                this.bgwInformationCheck.RunWorkerAsync();
            }

        }

        #endregion

        #region プライベートメソッド

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
        /// メインメニューの表示処理を行う。
        /// </summary>
        private void ShowMainMenu()
        {
            if (this.showingMdiMenu == null || !this.showingMdiMenu.Visible)
            {
                this.showingMdiMenu = new MenuFrame(this.currentMenuItemList,
                    this.currentIgnorMenuList);

                this.showingMdiMenu.MdiParent = this;
                this.showingMdiMenu.StartPosition = FormStartPosition.CenterScreen;
                this.showingMdiMenu.InitMenuFrame();
                this.showingMdiMenu.Show();

            }
            else
            {
                //表示されているときは、表に出す。
                this.showingMdiMenu.Activate();
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

        /// <summary>
        /// お知らせ情報を取得してお知らせ画面を表示する
        /// </summary>
        private void ShowInformationView()
        {

            //お知らせ機能の各種クラスが未実装のため以下をコメントアウト


            ////MDI内に存在していたら、アクティブにしてメソッドから抜ける
            //foreach (Form item in this.MdiChildren)
            //{
            //    if (item.Name == typeof(InformationListViewFrame).Name)
            //    {
            //        item.Activate();
            //        return;
            //    }
            //}

            ////お知らせ用のフォームを作って表示
            //InformationListViewFrame f = new InformationListViewFrame();
            //f.MdiParent = this;
            //////画面のアイコンを設定
            ////外薗orクマモトでアイコンを切り替える
            //switch (Hokawn.CraneBizManage.Forms.Property.SystemProperty.GetInstance().CurrentRunningCompanyKind)
            //{
            //    case Hokawn.CraneBizManage.Forms.Property.SystemProperty.RunningCompanyKind.HOKAWN:
            //        //外薗のとき
            //        f.Icon =
            //            global::Hokawn.CraneBizManage.Framework.Boot.
            //                Properties.Resources.AppIcon;
            //        break;
            //    case Hokawn.CraneBizManage.Forms.Property.SystemProperty.RunningCompanyKind.KUMAMOTO:
            //        //クマモトのとき
            //        f.Icon =
            //            global::Hokawn.CraneBizManage.Framework.Boot.
            //                Properties.Resources.AppIcon_KUMA;
            //        break;
            //    default:
            //        break;
            //}

            //f.InitFrame();
            ////f.SetInformationListToScreen();
            //f.StartPosition = FormStartPosition.CenterScreen;
            //f.Text = f.Text + " " + "[" + DateTime.Now.ToString("yyyy/MM/dd HH:mm") + "]";
            //f.Show();

            ////お知らせを非表示にする
            //this.toolStripSplitButtonAlartInfo.Visible = false;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// お知らせ機能における、お知らせ確認の時間間隔を分単位で指定します。
        /// </summary>
        public decimal InformationCheckInterval
        {
            get { return this._informationCheckInterval; }
            set { this._informationCheckInterval = value; }
        }

        #endregion


        private void MdiBaseMenuFrame_Shown(object sender, EventArgs e)
        {
            ////表示時
            ////お知らせのチェック開始
            ////--ログインオペレータの「お知らせ使用区分=1:使用する」かつ、
            ////--お知らせ通知間隔が設定されている場合のみチェックを行う
            //if (Forms.Property.UserProperty.GetInstance().InformationUseFlag &&
            //    this._informationCheckInterval != 0)
            //{
            //    //起動時１回目は自力でチェックする
            //    this.bgwInformationCheck.RunWorkerAsync();
            //    //--間隔をセット
            //    this.tmrInformationCheckDriven.Interval =
            //        Convert.ToInt32(((this._informationCheckInterval * 60) * 1000));
            //    //--タイマースタート
            //    this.tmrInformationCheckDriven.Enabled = true;
            //}

            //メニューの表示
            showingMdiMenu.Show();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            this.toolStripStatusLabelBootDate.Text =
                DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString();
        }

        private void ToolStripMenuItemShowMainMenu_Click(object sender, EventArgs e)
        {
            //メインメニューを表示
            this.ShowMainMenu();
        }
        
        private void 終了XToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void DelPrintSettingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //印刷設定の削除を行う
            this.DelPrintSetting();
        }

        private void MdiBaseMenuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //閉じる直前
            string msg = "アプリケーションを終了しますか？";
            DialogResult d_result =
                MessageBox.Show(this, msg, "確認", MessageBoxButtons.YesNo,
                MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (d_result == DialogResult.No)
            {
                e.Cancel = true;
            }

            this.bgwInformationCheck.CancelAsync();
        }

        private void ToolStripMenuItemCascading_Click(object sender, EventArgs e)
        {
            this.LayoutMdi(MdiLayout.Cascade);
        }

        private void tmrInformationCheckDriven_Tick(object sender, EventArgs e)
        {
            //お知らせ情報チェック用のタイマー起動時の処理
            //--お知らせのチェックを行う。
            //---バックグラウンド処理が実行中でないときにかぎる。
            //修正 T.kuroki 20091202 Frame側からの呼び出しを認めるためDelegateに
            // するので、メソッドアウトしておく
            //if (!this.bgwInformationCheck.IsBusy)
            //{
            //    this.bgwInformationCheck.RunWorkerAsync();
            //}
            this.DoInformationChecking();
        }

        private void ShowInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //お知らせ画面を表示する
            this.ShowInformationView();
        }

        private void bgwInformationCheck_DoWork(object sender, DoWorkEventArgs e)
        {
            //お知らせ機能の各種クラスが未実装のため以下をコメントアウト

            ////キャンセルされたかチェック
            //if (this.bgwInformationCheck.CancellationPending)
            //{
            //    return;
            //}

            ////BackGroundWorkerにて実行されるスレッド
            //BizObject.BLL.Information info_bll = new Information(this.appAuth);

            ////キャンセルされたかチェック
            //if (this.bgwInformationCheck.CancellationPending)
            //{
            //    return;
            //}


            ////チェックを実行し、お知らせ件数を取得
            //int result_count = info_bll.InvokeInformationCheck();

            ////キャンセルされたかチェック
            //if (this.bgwInformationCheck.CancellationPending)
            //{
            //    return;
            //}

            ////完了のイベントに値を引き渡す。
            //e.Result = result_count;
        }

        private void bgwInformationCheck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //お知らせ機能の各種クラスが未実装のため以下をコメントアウト

            ////完了したら・・・
            ////--件数を取得
            //int result_count = (int)e.Result;

            ////0件以上あったら・・・
            //if (result_count > 0)
            //{
            //    //お知らせ表示
            //    this.toolStripSplitButtonAlartInfo.Visible = true;
            //}
            //else
            //{
            //    //お知らせ非表示
            //    this.toolStripSplitButtonAlartInfo.Visible = false;
            //}
        }

        private void toolStripSplitButtonAlartInfo_ButtonClick(object sender, EventArgs e)
        {
            this.toolStripSplitButtonAlartInfo.ShowDropDown();
        }

        private void BranchDeployToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //拠点用配布を行います。
            this.BranchDeploingProgramFile2Folder();
        }

        private void ToolStripMenuItemShowMenuButton_Click(object sender, EventArgs e)
        {
            //メインメニューを表示
            this.ShowMainMenu();
        }

        private void ExplodingCurrentFolderToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //現在実行中のフォルダを表示する。
            string start_path = Application.StartupPath;
            System.Diagnostics.Process.Start(start_path);
        }

    }
}
