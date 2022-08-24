using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using jp.co.jpsys.util;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using GrapeCity.Win.Editors;
using GrapeCity.Win.Common;
using GrapeCity.Win.Components;
using GrapeCity.Win.MultiRow;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.SQLServerDAL.CsvExport;
using Jpsys.SagyoManage.Frame.Command;

namespace Jpsys.SagyoManage.Frame
{
    public partial class CsvExportFrame : Form, IFrameBase
    {
        #region ユーザー定義

        /// <summary>
        /// 画面タイトル
        /// </summary>
        private const string WINDOW_TITLE = "外部データ出力";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;
        
        /// <summary>
        /// 各CSV出力のDAL
        /// </summary>
        private List<CsvExportDALBase> csvExportDALs = null;

        #endregion

        #region コマンド

        private CommandSet commandSet;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 外部データ出力画面を初期化します。
        /// </summary>
        private void InitMasutaShutsuryokuFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            //メニューの初期化
            this.InitMenuItem();

            //バインドの設定
            this.SetupCommands();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //入力項目のクリア
            this.ClearInputs();

            //SQLServerDALのロード
            this.LoadCsvDALs();

            //コンボの初期化
            this.InitCombo();
        }

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            //操作メニュー
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.ExportCsv);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目を初期化します。
        /// </summary>
        private void ClearInputs()
        {
            this.cmbExportTask.SelectedValue = "-1";
            this.lbxSelectionTargetItem.Items.Clear();
            this.lbxExportTargetItem.Items.Clear();
            this.chkDisableFlag.Checked = true;
        }

        /// <summary>
        /// 各CSVのDALをロード
        /// </summary>
        private void LoadCsvDALs()
        {
            this.csvExportDALs = new List<CsvExportDALBase>();

            this.csvExportDALs.Add(new ItemExporter()); //品目（トラDON補）
            this.csvExportDALs.Add(new PointExporter()); //発着地（トラDON補）
            this.csvExportDALs.Add(new CarExporter()); //車両（トラDON補）
            this.csvExportDALs.Add(new CarKindExporter()); //車種（トラDON補）
            this.csvExportDALs.Add(new StaffExporter()); //社員（トラDON補）
            this.csvExportDALs.Add(new TokuisakiGroupExporter()); //得意先グループ
            this.csvExportDALs.Add(new HomenGroupExporter()); //方面グループ
            this.csvExportDALs.Add(new CarKindGroupExporter()); //車種グループ
            this.csvExportDALs.Add(new ItemBunruiExporter()); //品目分類
            this.csvExportDALs.Add(new PointBunruiExporter()); //発着地分類
            this.csvExportDALs.Add(new HanroExporter()); //販路
            this.csvExportDALs.Add(new HomenExporter()); //方面
            this.csvExportDALs.Add(new KyujitsuCalendarExporter()); //休日カレンダ
        }

        /// <summary>
        /// コンボ関連を初期化します。
        /// </summary>
        private void InitCombo()
        {
            //出力対象コンボの初期化
            this.InitExportTaskCombo();
        }

        /// <summary>
        /// 出力対象コンボを初期化します。
        /// </summary>
        private void InitExportTaskCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            datasource.Add("-1", "------------未選択------------");

            for (int i = 0; i < this.csvExportDALs.Count; i++)
            {
                var item = this.csvExportDALs[i];
                datasource.Add(i.ToString(), item.GetName());
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbExportTask, datasource, false, addPrefixKey: false);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

            //***CSV出力
            commandSet.ExportCsv.Execute += ExportCsvCommand_Execute;
            commandSet.Bind(
                commandSet.ExportCsv, this.btnExportCsv, actionMenuItems.GetMenuItemBy(ActionMenuItems.ExportCsv), this.toolStripExportCsv);
        }

        private void CloseCommand_Execute(object sender, EventArgs e)
        {
            // 終了
            this.DoClose();
        }

        private void ExportCsvCommand_Execute(object sender, EventArgs e)
        {
            //CSV作成
            this.DoExportCsv();
        }

        #endregion

        #region コンストラクタ

        public CsvExportFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 現在の出力テーブルコンボの列の一覧を選択対象リストボックスにセットして、
        /// 出力対象リストボックスをクリアします。
        /// </summary>
        private void SetSelectionTargetItem()
        {
            //選択対象リストボックスをクリア
            this.lbxSelectionTargetItem.Items.Clear();
            //出力対象リストボックスをクリア
            this.lbxExportTargetItem.Items.Clear();

            try
            {
                var selectedDAL = this.GetSelectedExportDAL();

                //出力対象が選択されているか
                if (selectedDAL != null)
                {
                    string[] colNames = selectedDAL.GetColumnNames();

                    //列の一覧をループで回して選択対象リストボックスにセット
                    foreach (string item in colNames)
                    {
                        this.lbxSelectionTargetItem.Items.Add(item);
                    }
                }
            }
            catch (System.Data.SqlClient.SqlException)
            {
                //DBエラーの場合はthrow
                throw;
            }
            catch (jp.co.jpsys.util.db.NSKDBIOException)
            {
                //DBエラーの場合はthrow
                throw;
            }
        }

        /// <summary>
        /// 選択対象リストボックスのすべての項目を出力対象リストボックスに移動します。
        /// </summary>
        private void SelectAllItem()
        {
            if (this.lbxSelectionTargetItem.Items != null)
            {
                //ループで回して選択対象リストボックスの項目を全て移動
                foreach (object item in this.lbxSelectionTargetItem.Items)
                {
                    this.lbxExportTargetItem.Items.Add(item);
                }

                //選択対象リストボックスの項目を全て削除
                this.lbxSelectionTargetItem.Items.Clear();
            }
        }

        /// <summary>
        /// 選択対象リストボックスの現在選択している項目を出力対象リストボックスに移動します。
        /// </summary>
        private void SelectItems()
        {
            //選択中のインデックスを取得しておく
            int select_idx = this.lbxSelectionTargetItem.SelectedIndex;

            //選択中の項目を格納するリスト
            List<object> wk_list = new List<object>();
            //選択中の項目を取得
            foreach (object item in this.lbxSelectionTargetItem.SelectedItems)
            {
                wk_list.Add(item);
            }

            //選択チェック
            if (wk_list.Count == 0)
            {
                //選択が無ければメッセージを表示
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2203031"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                //項目があれば先頭を選択
                if (lbxSelectionTargetItem.Items.Count > 0)
                {
                    this.lbxSelectionTargetItem.SelectedIndex = 0;
                }
            }
            else
            {
                //現在選択中の項目をループ
                foreach (object item in wk_list)
                {
                    //出力対象リストボックスに項目を追加
                    this.lbxExportTargetItem.Items.Add(item);
                    //選択対象リストボックスの項目を削除
                    this.lbxSelectionTargetItem.Items.Remove(item);
                }

                //出力対象リストボックスの現在の項目数を取得
                int cur_count = this.lbxSelectionTargetItem.Items.Count;

                //移動前のインデックスが選択可能かチェック
                if (select_idx <= cur_count - 1)
                {
                    //選択が可能なら移動前のインデックスの項目を選択
                    this.lbxSelectionTargetItem.SelectedIndex = select_idx;
                }
                else
                {
                    //選択が不可なら最後尾を選択
                    this.lbxSelectionTargetItem.SelectedIndex = cur_count - 1;
                }
            }

            //選択しようがしまいが選択対象リストフォーカスをセットする
            this.lbxSelectionTargetItem.Focus();
        }

        /// <summary>
        /// 出力対象リストボックスのすべての項目を選択対象リストボックスに移動します。
        /// </summary>
        private void CancelSelectionAllItem()
        {
            if (this.lbxExportTargetItem.Items != null)
            {
                //ループで回して出力対象リストボックスの項目を全て移動
                foreach (object item in this.lbxExportTargetItem.Items)
                {
                    this.lbxSelectionTargetItem.Items.Add(item);
                }

                //出力対象リストボックスの項目を全て削除
                this.lbxExportTargetItem.Items.Clear();
            }
        }

        /// <summary>
        /// 出力対象リストボックスの現在選択している項目を選択対象リストボックスに移動します。
        /// </summary>
        private void CancelSelectionItems()
        {
            //選択中のインデックスを取得しておく
            int select_idx = this.lbxExportTargetItem.SelectedIndex;

            //選択中の項目を格納するリスト
            List<object> wk_list = new List<object>();
            //選択中の項目を取得
            foreach (object item in this.lbxExportTargetItem.SelectedItems)
            {
                wk_list.Add(item);
            }

            //選択チェック
            if (wk_list.Count == 0)
            {
                //選択が無ければメッセージを表示
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2203032"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                //項目があれば先頭を選択
                if (lbxExportTargetItem.Items.Count > 0)
                {
                    this.lbxExportTargetItem.SelectedIndex = 0;
                }
            }
            else
            {
                //現在選択中の項目をループ
                foreach (object item in wk_list)
                {
                    //出力対象リストボックスに項目を追加
                    this.lbxSelectionTargetItem.Items.Add(item);
                    //選択対象リストボックスの項目を削除
                    this.lbxExportTargetItem.Items.Remove(item);
                }

                //出力対象リストボックスの現在の項目数を取得
                int cur_count = this.lbxExportTargetItem.Items.Count;

                //移動前のインデックスが選択可能かチェック
                if (select_idx <= cur_count - 1)
                {
                    //選択が可能なら移動前のインデックスの項目を選択
                    this.lbxExportTargetItem.SelectedIndex = select_idx;
                }
                else
                {
                    //選択が不可なら最後尾を選択
                    this.lbxExportTargetItem.SelectedIndex = cur_count - 1;
                }
            }

            //選択しようがしまいが選択対象リストフォーカスをセットする
            this.lbxExportTargetItem.Focus();
        }

        /// <summary>
        /// CSVファイルを作成して保存します。
        /// </summary>
        private void DoExportCsv()
        {
            //出力対象項目を取得
            string[] exportItems = this.GetExportItems();

            //項目の存在チェック
            if (exportItems.Length == 0)
            {
                //項目が無ければメッセージを表示
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2203033"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            else
            {
                //選択があれば「名前を付けて保存ダイアログを表示」
                using (SaveFileDialog sfd = new SaveFileDialog())
                {
                    //選択中のテーブル + .csvを既定のファイル名に
                    sfd.FileName = this.cmbExportTask.Text.Trim() + ".csv";

                    //上書き確認をするか？
                    sfd.OverwritePrompt = true;

                    //はじめに表示されるフォルダを指定する
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                    //デバッグ用。フォルダのパスをデスクトップに上書き
#if DEBUG
                    sfd.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
#endif

                    //[ファイルの種類]に表示される選択肢を指定する
                    sfd.Filter =
                        "CSVファイル(*.csv)|*.csv|すべてのファイル(*.*)|*.*";
                    //[ファイルの種類]の選択をテキストに
                    sfd.FilterIndex = 0;
                    //タイトルを設定する
                    sfd.Title = "保存先のファイルを選択してください";
                    //ダイアログボックスを閉じる前に現在のディレクトリを復元するようにする
                    sfd.RestoreDirectory = true;

                    //ダイアログを表示する
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        //待機状態へ
                        Cursor.Current = Cursors.WaitCursor;

                        try
                        {
                            //現在選択中のタスクを取得
                            var dal = this.GetSelectedExportDAL();

                            //CSVに出力するstring配列のリストを取得する
                            List<string[]> export_list =
                                dal.GetCsvData(exportItems, this.chkDisableFlag.Checked);

                            if (Directory.Exists(Path.GetDirectoryName(sfd.FileName)))
                            {
                                try
                                {
                                    FrameUtilites.CreateCSVDataByStringArrList(export_list, sfd.FileName);

                                    //処理完了メッセージ
                                    MessageBox.Show(
                                        FrameUtilites.GetDefineMessage("MI2001012"),
                                        this.Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Information);

                                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                                        FrameLogWriter.LoggingOperateKind.ProcessTask,
                                        "出力項目"　+FrameUtilites.GetDefineLogMessage(
                                            "C10001", new string[] { this.cmbExportTask.Text.Trim() })
                                    );

                                    this.ClearInputs();
                                    //フォーカスのセット
                                    this.cmbExportTask.Focus();
                                }
                                catch (IOException err)
                                {
                                    //IOExceptionが発生したらメッセージを表示。そうでない場合はthrowする
                                    MessageBox.Show(
                                        FrameUtilites.GetDefineMessage("ME2302008", new string[] { "CSVファイル" }) +
                                            "\r\n" + err.Message,
                                        this.Text,
                                        MessageBoxButtons.OK,
                                        MessageBoxIcon.Error
                                        );
                                }
                                catch (Exception err)
                                {
                                    throw err;
                                }
                            }
                            else
                            {
                                //フォルダが無い場合のメッセージ
                                MessageBox.Show(
                                    FrameUtilites.GetDefineMessage("MW2201007", new string[] { "指定した", "フォルダ" }),
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                        }
                        finally
                        {
                            Cursor.Current = Cursors.Default;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 出力対象の項目をString型の配列で取得します。
        /// </summary>
        /// <returns></returns>
        private string[] GetExportItems()
        {
            //選択中の項目を格納するリスト
            List<string> wk_list = new List<string>();

            foreach (object item in this.lbxExportTargetItem.Items)
            {
                //キャストして追加
                wk_list.Add((string)item);
            }

            //配列に変換
            return wk_list.ToArray();
        }

        /// <summary>
        /// 選択中の出力DALを取得する。
        /// </summary>
        /// <returns></returns>
        private CsvExportDALBase GetSelectedExportDAL()
        {
            int selectedValue = Convert.ToInt32(this.cmbExportTask.SelectedValue);

            if (selectedValue < 0)
            {
                return null;
            }

            if (this.csvExportDALs.Count <= selectedValue)
            {
                return null;
            }

            return this.csvExportDALs[selectedValue];
        }

        /// <summary>
        /// 未選択項目スト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcesslbxSelectionTargetItemPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.SelectItems();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 選択項目スト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcesslbxExportTargetItemPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.CancelSelectionItems();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 本画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        #endregion

        #region IFrameBase メンバー

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitMasutaShutsuryokuFrame();
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインスタンス経由で
        /// 取得･設定します。
        /// </summary>
        public string FrameName
        {
            get
            {
                return this.Name;
            }
            set
            {
                this.Name = value;
            }
        }

        /// <summary>
        /// 本インスタンスのTextプロパティをインスタンス経由で
        /// 取得・設定します。
        /// </summary>
        public string FrameText
        {
            get
            {
                return this.Text;
            }
            set
            {
                this.Text = value;
            }
        }

        #endregion

        /// <summary>
        /// OS規程の操作によりフォームを閉じる操作をした場合に、フォームの
        /// 自動入力検証(AutoValidate)処理を行わないようにするために、Windowsメッセージ
        /// の処理メソッドをオーバーライドして、該当する操作時に自動入力検証を
        /// 無効にします。
        /// 無効後はいずれかの処理で必ず自動入力検証をONにしてください。
        /// </summary>
        /// <param name="m">処理対象のWindows Message</param>
        protected override void WndProc(ref Message m)
        {
            const int WM_CLOSE = 0x10;
            const int WM_SYSCOMMAND = 0x112;
            const int SC_CLOSE = 0xf060;

            switch (m.Msg)
            {
                case WM_SYSCOMMAND:
                    if (m.WParam.ToInt32() == SC_CLOSE)
                    {
                        //Xボタン、コントロールメニューの「閉じる」、 
                        //コントロールボックスのダブルクリック、 
                        //Atl+F4などにより閉じられようとしている 
                        //このときValidatingイベントを発生させない。 
                        this.AutoValidate = AutoValidate.Disable;
                    }


                    break;
                case WM_CLOSE:
                    //Application.Exit以外で閉じられようとしている 
                    //このときValidatingイベントを発生させない。 
                    this.AutoValidate = AutoValidate.Disable;
                    break;
            }

            base.WndProc(ref m);
        }

        private void cmbExportTask_SelectedValueChanged(object sender, EventArgs e)
        {
            //選択対象のリストボックスをセット
            this.SetSelectionTargetItem();
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            //すべて選択ボタンクリック
            this.SelectAllItem();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            //選択ボタンクリック
            this.SelectItems();
        }

        private void btnCancelSelection_Click(object sender, EventArgs e)
        {
            //解除ボタンクリック
            this.CancelSelectionItems();
        }

        private void btnCancelSelectionAll_Click(object sender, EventArgs e)
        {
            //すべて解除ボタンクリック
            this.CancelSelectionAllItem();
        }

        private void lbxSelectionTargetItem_DoubleClick(object sender, EventArgs e)
        {
            //未出力一覧ダブルクリック
            this.SelectItems();
        }

        private void lbxExportTargetItem_DoubleClick(object sender, EventArgs e)
        {
            //出力一覧ダブルクリック
            this.CancelSelectionItems();
        }

        private void lbxSelectionTargetItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //未選択項目リスト - PreviewKeyDown
            this.ProcesslbxSelectionTargetItemPreviewKeyDown(e);
        }

        private void lbxExportTargetItem_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //選択項目リスト - PreviewKeyDown
            this.ProcesslbxExportTargetItemPreviewKeyDown(e);
        }

        private void CsvExportFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }
    }
}
