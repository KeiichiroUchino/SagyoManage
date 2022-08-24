using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
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
    public class BootInitializer
    {

        /// <summary>
        /// ClickOnceからのアプリケーション展開時に、最新バージョンがないか
        /// どうか確認し、存在していた場合バージョンアップします。
        /// バージョンアップ時はTrueを返却します。それ以外の場合はFalseを
        /// 返却します。ClickOnceを使用していない場合もFalseを返却します。
        /// </summary>
        /// <returns></returns>
        private static bool NetworkDeployedUpdatableCheck()
        {
            bool rt_val = false;

            //ClickOnceかどうかチェックする
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                try
                {
                    //現在のClickOnce
                    ApplicationDeployment curdp = ApplicationDeployment.CurrentDeployment;
                    //最新バージョンが利用可能かどうかチェックする
                    if (curdp.CheckForUpdate())
                    {
                        //最新バージョンが存在する場合は強制アップデート
                        curdp.Update();

                        //バージョンアップを行ったためTrue
                        rt_val = true;
                    }

                }
                catch (DeploymentDownloadException)
                {
                    //配置場所からダウンロードできない
                    MessageBox.Show(
                        "最新バージョンの有無を確認できませんでした。OKボタンをクリックするとアプリケーションを起動します。",
                        "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (InvalidDeploymentException)
                {
                    //配置場所のClickOnceが壊れてる
                    MessageBox.Show(
                        "配置先に問題が発生しています。サポートへご連絡ください。OKボタンをクリックするとアプリケーションを起動します。",
                        "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }


            }

            return rt_val;

        }

        public static void BootStart()
        {
            //初期設定
            //--製品のIDを指定する
            NSKUtilSetting.SeihinId = "SagyoManage";
            NSKUtilSetting.ProductName = "受注配車管理システム";
            NSKUtilSetting.EditionName = "Base";

            //アプリケーション構成ファイルからシステム名を取得する
            var app_product_name = ConfigurationManager.AppSettings["DisplaySystemName"].Trim();
            if (app_product_name.Length != 0)
            {
                //構成ファイルが空白のときはそのまま使う
                NSKUtilSetting.ProductName = app_product_name;
            }

            //アプリケーション構成ファイルからエディション名を取得する
            var app_edition_name = ConfigurationManager.AppSettings["EditionName"].Trim();
            if (app_edition_name.Length != 0)
            {
                //構成ファイルが空白のときはそのまま使う
                NSKUtilSetting.EditionName = app_edition_name;
            }

            //追加 T.Kuroki 20090929 ClickOnceの起動時のバージョンチェックを
            // ネットワークの都合やキャンセルボタンの押下ですり抜けた場合でも
            // 強制的にチェックを行いアップデートするようにする。
            if (BootInitializer.NetworkDeployedUpdatableCheck())
            {
                MessageBox.Show("最新バージョンにアップデートしました。" + "\r\n" +
                    "OKボタンをクリックした後、再度アプリケーションを起動してください。",
                    NSKUtilSetting.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                //バージョンアップ後は再起動させるため、アプリケーションを終了させる
                return;
            }

            #region メソッド内ユーザ定義

            //--読み込み済みのメニュー一覧
            IList<MenuItemInfo> loadedMenuItemList =
                    (IList<MenuItemInfo>)(new List<MenuItemInfo>());

            //--起動不可のメニューのメニュー番号を格納する。
            List<decimal> ignorAuthMenuList = new List<decimal>();

            ////--お知らせ機能の監視間隔を格納する（分単位）
            //decimal informationCheckIntervalMinutes = 0;

            #endregion

            using (LoginFrame f = new LoginFrame())
            {
                f.InitLoginFrame();
                Application.Run(f);
                DialogResult d_result = f.DialogResult;
                if (d_result == DialogResult.OK)
                {
                    Console.WriteLine("ログイン成功");
                }
                else
                {
                    Application.Exit();
                    return;
                }
            }

            //一度Application.Runを実行するとなぜか、ThreadExceptionイベントが発生しないので
            //ここで、再度ThreadExceptionのイベントを定義しておく
            Application.ThreadException += new System.Threading.ThreadExceptionEventHandler(Application_ThreadException);
            //念のためUnhandledExceptionイベントも再定義
            System.Threading.Thread.GetDomain().UnhandledException += new UnhandledExceptionEventHandler(BootInitializer_UnhandledException);

            try
            {

                using (LoadingFrame f = new LoadingFrame())
                {
                    f.Show();
                    f.DrowLoadingText(string.Empty);
                    f.Refresh();
                    System.Threading.Thread.Sleep(1);

                    //追加 T.Kuroki 20090929 ClickOnceの起動時のバージョンチェックを
                    // ネットワークの都合やキャンセルボタンの押下ですり抜けた場合でも
                    // 強制的にチェックを行いアップデートするようにする。
                    // 起動直後にもチェックをするがログイン前にも確認する。
                    #region ログイン後のバージョンチェック

                    f.DrowLoadingText("バージョンチェック中...");
                    f.Refresh();
                    if (BootInitializer.NetworkDeployedUpdatableCheck())
                    {
                        MessageBox.Show("最新バージョンにアップデートしました。" + "\r\n" +
                            "OKボタンをクリックした後、再度アプリケーションを起動してください。",
                            NSKUtilSetting.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        //バージョンアップ後は再起動させるため、アプリケーションを終了させる
                        return;
                    }

                    #endregion

                    #region 権限別メニュー使用不可情報の取得

                    //修正 T.kuroki 20111109 NSK管理アカウントのときは権限情報の
                    // 読み込みは飛ばす
                    //if (!UserProperty.GetInstance().NSKAdminLoginFlag)
                    //{
                    //    //権限情報のビジネスロジックのインスタンスを作成
                    //    MenuAuthSetting ma_bll = new MenuAuthSetting();
                    //    //権限情報（起動不可メニューの一覧）を取得
                    //    IList<MenuAuthSettingInfo> ma_list =
                    //        ma_bll.GetMenuAuthSettingList(UserProperty.GetInstance().AuthorityId);
                    //    f.DrowLoadingText("権限情報読み込み中...");
                    //    f.Refresh();
                    //    //大回転
                    //    foreach (MenuAuthSettingInfo item in ma_list)
                    //    {
                    //        //起動不可のメニューのメニューIDを設定する。
                    //        if (!item.AllowUseFlag)
                    //        {
                    //            ignorAuthMenuList.Add(item.MenuId);
                    //        }

                    //    }
                    //}

                    #endregion

                    #region "Frame読み込み処理"
                    AssemblyName asm_nm = new AssemblyName("Jpsys.SagyoManage.Frame");
                    Assembly asm = Assembly.Load(asm_nm);

                    f.Show();

                    Type[] types = asm.GetTypes();

                    string iframebase_nams = typeof(IFrameBase).Name;

                    foreach (Type t in types)
                    {
                        //アセンブリ内のすべての型でIFrameBaseインタフェースを取っているかチェック
                        if (t.IsClass && t.IsPublic && !t.IsAbstract &&
                            t.GetInterface(iframebase_nams) != null)
                        {

                            //IFrameBaseを取っている時は、インスタンス化したうえで初期化する
                            IFrameBase frame_base =
                                (IFrameBase)asm.CreateInstance(t.FullName);

#if DEBUG
                            Console.WriteLine(t.FullName);
#endif
                            f.DrowLoadingText(frame_base.FrameName + " ロード中...");

                            //*** s.arimura 2009/09/09 Init時にMrow等の操作をしていると
                            //***                       Mrowの参照が無くNullRef例外が帰ってくるのでコメントアウト
                            //frame_base.InitFrame();
                        }
                    }
                    #endregion

                    #region メニュー読み込み処理
                    f.DrowLoadingText("メニューロード中...");
                    f.Refresh();
                    SQLServerDAL.MenuItem menubll = new SQLServerDAL.MenuItem();
                    loadedMenuItemList = menubll.GetMenuItemList();



                    #endregion

                }

                //構成ファイルのプロパティチェック
                if (ConfigurationManager.AppSettings["IsSidMenu"] == "True")
                {
                    using (MenuFrame f = new MenuFrame(loadedMenuItemList, ignorAuthMenuList))
                    {
                        //Mdiメニュを作る
                        f.StartPosition = FormStartPosition.CenterScreen;
                        f.InitMenuFrame();

                        //メニューにMDIメニューの参照を渡す
                        f.Text = NSKUtilSetting.ProductName;
                        Application.Run(f);
                    }
                }
                else
                {
                    using (MdiBaseMenuFrame f = new MdiBaseMenuFrame())
                    {
                        //Mdiメニュを作る
                        MenuFrame mdiMenu = new MenuFrame(loadedMenuItemList, ignorAuthMenuList);
                        mdiMenu.MdiParent = f;
                        mdiMenu.StartPosition = FormStartPosition.CenterScreen;
                        mdiMenu.InitMenuFrame();

                        //メニューにMDIメニューの参照を渡す
                        f.InitMainMenu(mdiMenu, loadedMenuItemList, ignorAuthMenuList);
                        f.WindowState = FormWindowState.Maximized;
                        f.Text = NSKUtilSetting.ProductName;
                        Application.Run(f);
                    }
                }

                Application.Exit();
                return;
            }
            catch (Exception err)
            {
                //例外ハンドラ
                //例外メッセージを作る
                string err_msg = err.Message;
                if (err.StackTrace != null)
                {
                    err_msg += err.StackTrace;
                }

                ShowErrorMessage(err, "エラー");
            }
        }

        // 未処理例外をキャッチするイベント・ハンドラ
        // （Windowsアプリケーション用）
        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            ShowErrorMessage(e.Exception, "Application_ThreadExceptionによる例外通知");
        }

        // 未処理例外をキャッチするイベント・ハンドラ
        // （主にコンソール・アプリケーション用）
        static void BootInitializer_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            Exception ex = e.ExceptionObject as Exception;
            if (ex != null)
            {
                ShowErrorMessage(ex, "Application_UnhandledExceptionによる例外通知です。");
                //例外メッセージを出した後、アプリケーションを終了させる
                Environment.Exit(-1);
            }
            else
            {
                //例外オブジェクトがnullだった場合でも、強制的に終了させる
                Environment.Exit(-1);
            }

        }

        // ユーザー・フレンドリなダイアログを表示するメソッド
        public static void ShowErrorMessage(Exception ex, string extraMessage)
        {
            string err_msg = string.Empty;

            //例外メッセージを作成するメソッドに投げる
            err_msg = CreateExceptionMessage(ex);

            LogWriter.DebugLogWriter(typeof(BootInitializer).FullName, err_msg);

            ErrorMessageUtil.ShowErrorMessageDialog(err_msg);
        }

        #region 例外のメッセージ作成

        /// <summary>
        /// 例外発生時のメッセージを作成します。
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        private static string CreateExceptionMessage(Exception err)
        {
            //例外のメッセージのリストを取得
            List<string> exMsgList = GetExceptionMessageList(err);

            //メッセージを連結してエラー内容を作成
            StringBuilder sbErrorContent = new StringBuilder();
            foreach (string exMsg in exMsgList)
            {
                sbErrorContent.AppendLine(string.Format("・{0}", exMsg));
            }

            string errorContent = sbErrorContent.ToString();

            //メッセージを作成
            string err_msg =
                "致命的なエラーが発生しました。システム担当者または開発元にお知らせください\r\n\r\n" +
                "【エラー内容】\r\n" + errorContent + "\r\n\r\n" +
                "【スタックトレース】\r\n" + err.StackTrace;

            return err_msg;
        }

        /// <summary>
        /// 例外からメッセージの一覧を取得します。
        /// </summary>
        /// <param name="err"></param>
        /// <returns></returns>
        private static List<string> GetExceptionMessageList(Exception err)
        {
            List<string> rt_list = new List<string>();

            //リストに例外のメッセージを追加
            AddMessageList(err, rt_list);

            return rt_list;
        }

        /// <summary>
        /// リストに例外のメッセージを追加します。
        /// </summary>
        /// <param name="err">例外</param>
        /// <param name="list">追加するメッセージのリスト</param>
        private static void AddMessageList(Exception err, List<string> list)
        {
            //例外があるか？
            if (err == null)
            {
                //無ければそのまま抜ける
                return;
            }
            else
            {
                //有ればメッセージがあるか判断
                if (err.Message != null && err.Message.Trim().Length > 0)
                {
                    //メッセージが有ればリストに追加する
                    list.Add(err.Message);
                }

                //InnerExceptionを指定して再帰呼び出し
                AddMessageList(err.InnerException, list);
            }
        }

        #endregion

    }
}
