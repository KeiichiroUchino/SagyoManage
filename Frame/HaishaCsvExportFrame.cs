using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.BizProperty.SettingsModel;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaCsvExportFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル初期値
        /// </summary>
        private const string WINDOW_TITLE = "配車情報CSV出力";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している操作メニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 配車情報SSV出力クラス
        /// </summary>
        private HaishaCsvExport _HaishaCsvExport;

        /// <summary>
        /// 得意先クラス
        /// </summary>
        private Tokuisaki _Tokuisaki;

        /// <summary>
        /// 管理情報クラス
        /// </summary>
        private KanriInfo _KanriInfo;

        #region 請求先リスト

        //--Spread列定義

        /// <summary>
        /// 請求先コード列番号
        /// </summary>
        private const int COL_SEIKYUSAKI_CODE = 0;

        /// <summary>
        /// 請求先名列番号
        /// </summary>
        private const int COL_SEIKYUSAKI_NAME = 1;

        /// <summary>
        /// 請求先カナ名列番号
        /// </summary>
        private const int COL_SEIKYUSAKI_KNAME = 2;

        /// <summary>
        /// 締日列番号
        /// </summary>
        private const int COL_SEIKYUSAKI_SHIMEBI = 3;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_SELECT_CHECKBOX = 4;

        /// <summary>
        /// 請求先Id列番号
        /// </summary>
        private const int COL_SEIKYUSAKI_ID = 5;

        /// <summary>
        /// 請求先リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_SEIKYUSAKILIST = 6;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialSeikyusakiStyleInfoArr;

        /// <summary>
        /// 請求先情報のリストを保持する領域
        /// </summary>
        private IList<TokuisakiInfo> _SeikyusakiInfoList = null;

        #endregion

        #region コマンド

        private CommandSet commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #endregion

        #region コンストラクタ

        //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, [MarshalAs(UnmanagedType.Bool)] bool pvParam, UInt32 fWinIni);
        [Flags]
        public enum SystemParametersInfoActionFlag : uint
        {
            SPI_SETKEYBOARDCUES = 0x100B,
        }
        [Flags]
        public enum SystemParametersInfoFlag : uint
        {
            SPIF_UPDATEINIFILE = 0x0001,
            SPIF_SENDWININICHANGE = 0x0002,
        }

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public HaishaCsvExportFrame()
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0,
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitSeikyuDataSakuseiFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            // 画面配色の初期化
            this.InitFrameColor();

            // 親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 請求データ作成クラスインスタンス作成
            this._HaishaCsvExport = new HaishaCsvExport(this.appAuth);

            // 得意先クラスインスタンス作成
            this._Tokuisaki = new Tokuisaki(this.appAuth);

            // 管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //コンボボックスの初期化
            this.InitCombo();

            // Spread 関連の初期化
            this.InitSheet();

            // 入力項目のクリア
            this.ClearInputs();

            // 請求先リストのセット
            this.SetSeikyusakiListSheet();

            // コントロール活性化
            this.InitControlsEnabled();

            // フォーカス設定
            this.SetFirstFocus();
        }

        /// <summary>
        /// 画面配色を初期化します。
        /// </summary>
        private void InitFrameColor()
        {
            //表のヘッダー背景色、表の選択行背景色の設定
            FrameUtilites.SetFrameGridBackColor(this);

            //ステータスバーの背景色設定
            this.statusStrip1.BackColor = FrameUtilites.GetFrameFooterBackColor();
        }

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            this.actionMenuItems = new AddActionMenuItem();

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.ExportCsv);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***CSV出力
            this.commandSet.ExportCsv.Execute += ExportCsvCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.ExportCsv,
                this.btnExportCsv,
                this.actionMenuItems.GetMenuItemBy(ActionMenuItems.ExportCsv),
                this.toolStripExportCsv
                );

            //***終了
            this.commandSet.Close.Execute += CloseCommand_Execute;
            this.commandSet.Bind(
                this.commandSet.Close,
                this.btnClose,
                this.actionMenuItems.GetMenuItemBy(ActionMenuItems.Close),
                this.toolStripEnd
                );
        }

        private void ExportCsvCommand_Execute(object sender, EventArgs e)
        {
            // CSV出力
            this.DoExportCsv();
        }

        private void CloseCommand_Execute(object sender, EventArgs e)
        {
            // 終了
            this.DoClose();
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteHizukeYMDFrom, ctl => ctl.Text, this.dteHizukeYMDFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteHizukeYMDTo, ctl => ctl.Text, this.dteHizukeYMDTo_Validating));
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStyleInfo()
        {
            //SpreadのStyle情報を格納するメンバ変数の初期化
            this.InitSpreadStyleInfo();
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSpreadStyleInfo()
        {
            // 請求先リストスタイル情報初期化
            this.InitSeikyusakiStyleInfo();
        }

        /// <summary>
        /// 請求先リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSeikyusakiStyleInfo()
        {
            // Id列非表示
            this.fpSeikyusakiListGrid.Sheets[0].Columns[COL_SEIKYUSAKI_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpSeikyusakiListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialSeikyusakiStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_SEIKYUSAKILIST];

            for (int i = 0; i < COL_MAXCOLNUM_SEIKYUSAKILIST; i++)
            {
                this.initialSeikyusakiStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitShimebiCombo();
        }

        /// <summary>
        /// 締日コンボボックスを初期化します。
        /// </summary>
        private void InitShimebiCombo()
        {
            // 締日コンボ設定
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbShimebi,
                this._Tokuisaki.GetShimebiComboData(), false, null, false);
            this.cmbShimebi.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
        }

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //請求先リストの初期化
            this.InitSeikyusakiListSheet();
        }

        /// <summary>
        /// 請求先リストを初期化します。
        /// </summary>
        private void InitSeikyusakiListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpSeikyusakiListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_SEIKYUSAKILIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.dteHizukeYMDFrom.Value = DateTime.Today.FirstDayOfMonth();
            this.dteHizukeYMDTo.Value = DateTime.Today.LastDayOfMonth();
            this.radShimebiShitei.Checked = true;
            this.cmbShimebi.SelectedIndex = (0 < this.cmbShimebi.Items.Count) ? 0 : -1;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #endregion

        #region プライベート メソッド

        /// <summary>コントロールの活性／非活性を初期化します。
        /// </summary>
        private void InitControlsEnabled()
        {
            this.ChangeSeikyusakiCondition();
        }

        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            this.dteHizukeYMDFrom.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 請求先指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeSeikyusakiCondition()
        {
            this.cmbShimebi.Enabled = this.radShimebiShitei.Checked;
            this.fpSeikyusakiListGrid.Enabled = this.radKobetsuShitei.Checked;

            // 請求先リストが非活性の場合
            if (!this.fpSeikyusakiListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpSeikyusakiListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpSeikyusakiListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpSeikyusakiListGrid.Sheets[0].ClearSelection();

                //ボタン非活性
                this.btnSelectAll.Enabled = false;
                this.btnCancelSelected.Enabled = false;
            }
            else
            {
                //ボタン非活性
                this.btnSelectAll.Enabled = true;
                this.btnCancelSelected.Enabled = true;
            }
        }

        /// <summary>
        /// 請求先リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckSeikyusakiList(int rowIndex)
        {
            this.fpSeikyusakiListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpSeikyusakiListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 請求先リストに値を設定します。
        /// </summary>
        private void SetSeikyusakiListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._SeikyusakiInfoList =
                    this._Tokuisaki.GetList(
                    new TokuisakiSearchParameter()
                    {
                        ToraDONDisableFlag = false
                    }
                    );

                //件数
                int rowCount = this._SeikyusakiInfoList.Count;

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_SEIKYUSAKILIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_SEIKYUSAKILIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_SEIKYUSAKILIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialSeikyusakiStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    TokuisakiInfo wk_info = this._SeikyusakiInfoList[i];

                    datamodel.SetValue(i, COL_SEIKYUSAKI_CODE, wk_info.ToraDONTokuisakiCode);
                    datamodel.SetValue(i, COL_SEIKYUSAKI_NAME, wk_info.ToraDONTokuisakiName);
                    datamodel.SetValue(i, COL_SEIKYUSAKI_KNAME, wk_info.ToraDONTokuisakiNameKana);
                    datamodel.SetValue(i, COL_SEIKYUSAKI_SHIMEBI, wk_info.ToraDONFixDayMeisho1);
                    datamodel.SetValue(i, COL_SEIKYUSAKI_ID, wk_info.ToraDONTokuisakiId);

                    datamodel.SetTag(i, COL_SEIKYUSAKI_CODE, wk_info);
                }

                //Spreadにデータモデルをセット
                this.fpSeikyusakiListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpSeikyusakiListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpSeikyusakiListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpSeikyusakiListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// CSV出力を実行します。
        /// </summary>
        /// <param name="printDestType"></param>
        private void DoExportCsv()
        {
            if (!this.ValidateChildren())
            {
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                return;
            }

            if (!this.CheckInputs())
                return;

            //画面からパラメーターを取得
            HaishaCsvExportSearchParameter param =
                this.CreateParameterFromScreen();

            //配車情報CSV出力リスト取得
            IList<HaishaCsvExportInfo> list = this._HaishaCsvExport.GetList(param);

            if (list == null || list.Count == 0)
            {
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2201015"),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                return;
            }

            //ファイルダイアログからCSV出力パスを取得
            string path = this.ShowSaveFileDialog();

            // CSV出力パスが未設定の場合
            if (path.Equals(string.Empty))
            {
                return;
            }

            //画面を使用不可に
            this.ControlEnableChangeForPrinting(false);
            //画面タイトルを変更する
            this.Text = WINDOW_TITLE + "　出力中...";
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                #region CSV出力処理

                //CSVに出力するstring配列リスト
                List<string[]> export_list = ComLib.Text.CsvHelper.CreateTextAsLines(list);

                //出力ディレクトリが存在する場合
                if (Directory.Exists(Path.GetDirectoryName(path)))
                {
                    try
                    {
                        //CSV出力
                        FrameUtilites.CreateCSVDataByStringArrListEx(export_list, path, true);

                        //操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.FileOutPut,
                            this.CreateLog(path));

                        //出力終了確認メッセージを表示
                        DialogResult r_result =
                            MessageBox.Show(
                                FrameUtilites.GetDefineMessage("MW2203028", new string[] { list.Count.ToString() }),
                                this.Text,
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Question,
                                MessageBoxDefaultButton.Button2);

                        if (r_result == DialogResult.Yes)
                        {
                            //出力したCSVファイルを開く
                            System.Diagnostics.Process.Start(path);
                        }
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

                #endregion
            }
            catch (System.Exception e)
            {
                // CSV出力に失敗したとき
                MessageBox.Show(
                        FrameUtilites.GetDefineMessage("ME2302008", new string[] { "CSV出力" }) +
                            "\r\n" + e.Message,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                    );
            }
            finally
            {
                //画面を使用可能に
                this.ControlEnableChangeForPrinting(true);
                //画面タイトルを変更する
                this.Text = WINDOW_TITLE;
                //待機状態へ
                this.Cursor = Cursors.Default;
                //画面の最初の項目にフォーカスを移動
                this.SetFirstFocus();
            }
        }

        /// <summary>
        /// 詳細の入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;

            string msg = string.Empty;
            Control ctl = null;

            // 日付（範囲開始）
            if (rt_val && this.HizukeYMDFrom == DateTime.MinValue)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "日付（範囲開始）" });
                ctl = this.dteHizukeYMDFrom;
            }
            else
            {
                // 日付（範囲終了）
                if (rt_val && this.HizukeYMDTo == DateTime.MinValue)
                {
                    this.dteHizukeYMDTo.Value = this.HizukeYMDFrom;
                }

                if (this.HizukeYMDFrom > this.HizukeYMDTo)
                {
                    rt_val = false;
                    msg = "開始＞終了です。";
                    ctl = this.dteHizukeYMDTo;
                }
                else
                {
                    if (this.HizukeYMDFrom.AddMonths(13) < this.HizukeYMDTo)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage("MW2202026", new string[] { "13" });
                        ctl = this.dteHizukeYMDTo;
                    }
                }
            }

            // 締日指定の場合
            if (this.radShimebiShitei.Checked)
            {
                // 日付（範囲開始）
                if (rt_val && this.cmbShimebi.SelectedIndex < 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { "締日" });
                    ctl = this.cmbShimebi;
                }
            }

            // 個別指定の場合
            if (this.radKobetsuShitei.Checked)
            {
                int cnt = 0;
                //チェック件数取得
                for (int i = 0; i < this.fpSeikyusakiListGrid.Sheets[0].RowCount; i++)
                {
                    if ((bool)this.fpSeikyusakiListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value)
                        cnt++;
                }
                //チェックが0件の場合
                if (rt_val && cnt == 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { "請求先" });
                    ctl = this.fpSeikyusakiListGrid;
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(
                    msg,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                if (ctl != null)
                {
                    ctl.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 画面からパラメーターを取得します。
        /// </summary>
        private HaishaCsvExportSearchParameter CreateParameterFromScreen()
        {
            return new HaishaCsvExportSearchParameter()
            {
                HizukeYMDFrom = this.HizukeYMDFrom,
                HizukeYMDTo = this.HizukeYMDTo,
                SeikyusakiShiteiKbn = this.radShimebiShitei.Checked
                    ? HaishaCsvExportSearchParameter.SeikyusakiShiteiKbnItem.Shimebi
                    : HaishaCsvExportSearchParameter.SeikyusakiShiteiKbnItem.Kobetsu,
                Shimebi = Convert.ToInt32(this.cmbShimebi.SelectedValue),
                SeikyusakiIdList = this.GetSelectSeikyusakiList().Select(x => x.SeikyusakiId).ToList(),
                SeikyuRenkeiDailyReportKbn = UserProperty.GetInstance().SystemSettingsInfo.SeikyuRenkeiDailyReportKbn,
                SeikyuRenkeiUwagakiKbn = UserProperty.GetInstance().SystemSettingsInfo.SeikyuRenkeiUwagakiKbn,
            };
        }

        /// <summary>
        ///ファイル選択ダイアログを表示します。
        /// </summary>
        private string ShowSaveFileDialog()
        {
            string rt_str = string.Empty;

            //初期フォルダパスの初期値を設定
            this.saveFileDialog1.InitialDirectory = System.Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);

            //初期フォルダパスを設定
            HaishaCsvSettingInfo info = BizCommon.GetHaishaCsvSettingInfo();
            if (info != null)
            {
                //ディレクトリが存在する場合
                if (Directory.Exists(Path.GetDirectoryName(info.HaishaCsvExportFilePath)))
                {
                    this.saveFileDialog1.InitialDirectory = info.HaishaCsvExportFilePath;
                }
            }

            //初期ファイル名を設定
            this.saveFileDialog1.FileName = WINDOW_TITLE + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";

            //ファイルダイアログを開く
            if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                //ファイルパスを取得
                rt_str = this.saveFileDialog1.FileName;

                //CSVファイルパスを設定ファイルに保存
                this.SaveCsvPath(System.IO.Path.GetDirectoryName(this.saveFileDialog1.FileName));
            }

            return rt_str;
        }

        /// <summary>
        /// 配車情報CSVファイルパス情報を保存します。
        /// </summary>
        private void SaveCsvPath(string dir)
        {
            HaishaCsvSettingInfo info = new HaishaCsvSettingInfo();
            info.HaishaCsvExportFilePath = dir;
            BizCommon.SaveHaishaCsvSettingInfo(info);
        }

        /// <summary>
        /// 印刷中の処理においてコントロールの有効無効を切り替えます。
        /// 印刷のための処理中に不用意にコントロールが操作されることを
        /// 防ぐためです。
        /// </summary>
        /// <param name="val">コントロールの有効・無効(true:有効）</param>
        private void ControlEnableChangeForPrinting(bool val)
        {
            this.menuStripTop.Enabled = val;
            this.pnlMain.Enabled = val;
            this.pnlBottom.Enabled = val;

            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.ExportCsv, val);
            this.actionMenuItems.ActionMenuItemEnable(ActionMenuItems.Close, val);
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLog(string path)
        {
            //操作ログ(保存)の条件取得
            string log_jyoken =
                "[日付] " + HizukeYMDString +
                "\r\n[範囲指定] " + SeikyusakiKbnString;

            if (this.radShimebiShitei.Checked)
            {
                log_jyoken = log_jyoken +
                    "\r\n[締日] " + ShimebiString;
            }
            else
            {
                log_jyoken = log_jyoken +
                    "\r\n[請求先]" + SeikyusakiListString;
            }

            log_jyoken = log_jyoken +
                "\r\n[出力先]：" + path;

            return log_jyoken;
        }

        /// <summary>
        /// 画面「請求先一覧」で選択されている請求先情報リストを取得します。
        /// </summary>
        /// <returns>選択されている請求先情報リスト</returns>
        private List<SelectSeikyusakiResult> GetSelectSeikyusakiList()
        {
            SheetView sheet0 = this.fpSeikyusakiListGrid.ActiveSheet;

            List<SelectSeikyusakiResult> selectList = new List<SelectSeikyusakiResult>();
            for (int i = 0; i < sheet0.RowCount; i++)
            {
                //チェックがされているか判断します。
                if (Convert.ToBoolean(sheet0.GetValue(i, COL_SELECT_CHECKBOX)))
                {
                    //タグから情報を取得
                    TokuisakiInfo info = (TokuisakiInfo)sheet0.Cells[i, COL_SEIKYUSAKI_CODE].Tag;
                    selectList.Add(new SelectSeikyusakiResult() { SeikyusakiCode = info.ToraDONTokuisakiCode, SeikyusakiId = info.ToraDONTokuisakiId });
                }
            }
            return selectList;
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// リスト一括選択
        /// </summary>
        /// <param name="checkedflag">選択可否（true：選択、false：解除）</param>
        private void SetSelectCheckBox(bool checkedflag)
        {
            for (int i = 0; i < this.fpSeikyusakiListGrid.Sheets[0].RowCount; i++)
            {
                this.fpSeikyusakiListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = checkedflag;
            }
        }

        #endregion

        #region 検索処理

        /// <summary>
        /// キーイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessKeyEvent(KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                default:
                    break;
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 日付（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHizukeYMDFrom(CancelEventArgs e)
        {
            //日付（範囲開始）が設定された場合
            if (this.dteHizukeYMDFrom.Value != null)
            {
                //月初日が設定された場合
                if (this.dteHizukeYMDFrom.Value.Value.ToString("dd").Equals("01"))
                {
                    //日付（範囲終了）に月末日を設定
                    this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.AddMonths(1).PreviousDay();
                }
                else
                {
                    //日付（範囲終了）に一か月後の日付を設定
                    this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.AddMonths(1);
                }
            }
            //日付（範囲開始）が空の場合
            else
            {
                //システム日付を設定
                this.dteHizukeYMDFrom.Value = DateTime.Today.FirstDayOfMonth();
                this.dteHizukeYMDTo.Value = DateTime.Today.LastDayOfMonth();
            }
        }

        /// <summary>
        /// 日付（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHizukeYMDTo(CancelEventArgs e)
        {
            //日付（範囲終了）が空の場合
            if (this.dteHizukeYMDTo.Value == null)
            {
                //月初日が設定された場合
                if (this.dteHizukeYMDFrom.Value.Value.ToString("dd").Equals("01"))
                {
                    //日付（範囲終了）に月末日を設定
                    this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.AddMonths(1).PreviousDay();
                }
                else
                {
                    //日付（範囲終了）に一か月後の日付を設定
                    this.dteHizukeYMDTo.Value = this.dteHizukeYMDFrom.Value.Value.AddMonths(1);
                }
            }

            //日付（範囲開始）が存在する場合
            if (this.dteHizukeYMDFrom.Value != null)
            {
                if (this.dteHizukeYMDFrom.Value.Value > this.dteHizukeYMDTo.Value.Value)
                {
                    MessageBox.Show(
                        "開始＞終了です。",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );
                    this.dteHizukeYMDTo.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 日付（範囲開始）の値を取得します。
        /// </summary>
        public DateTime HizukeYMDFrom
        {
            get { return Convert.ToDateTime(this.dteHizukeYMDFrom.Value).Date; }
        }

        /// <summary>
        /// 日付（範囲開始）の値を取得します。
        /// </summary>
        public DateTime HizukeYMDTo
        {
            get { return Convert.ToDateTime(this.dteHizukeYMDTo.Value).Date; }
        }

        /// <summary>
        /// 画面「日付」の条件指定を文字型で取得します。
        /// </summary>
        private String HizukeYMDString
        {
            get { return string.Format("{0}", string.Format("{0}～{1}", Convert.ToDateTime(this.dteHizukeYMDFrom.Value).ToString("yyyy/MM/dd"), Convert.ToDateTime(this.dteHizukeYMDTo.Value).ToString("yyyy/MM/dd"))); }
        }

        /// <summary>
        /// 画面「請求先」の条件指定を文字型で取得します。
        /// </summary>
        private String SeikyusakiKbnString
        {
            get { return this.radShimebiShitei.Checked ? "締日指定" : "個別指定"; }
        }

        /// <summary>
        /// 画面「締日」の条件指定を文字型で取得します。
        /// </summary>
        private String ShimebiString
        {
            get { return this.radShimebiShitei.Checked ? this.cmbShimebi.Text + "締め" : "個別指定"; }
        }

        /// <summary>
        /// 画面「選択請求先」の条件指定を文字型で取得します。
        /// </summary>
        private String SeikyusakiListString
        {
            get { return string.Join(",", this.GetSelectSeikyusakiList().Select(x => x.SeikyusakiCode).ToList()); }
        }

        #endregion

        #region プライベートクラス

        /// <summary>
        /// 選択請求先情報
        /// </summary>
        private class SelectSeikyusakiResult
        {
            public Int32 SeikyusakiCode { get; set; }
            public Decimal SeikyusakiId { get; set; }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitSeikyuDataSakuseiFrame();
        }

        /// <summary>
        /// 本クラスの Name プロパティを取得・設定します。
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
        /// 本クラスの Text プロパティを取得・設定します。
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

        private void HatchuShoFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void HatchuShoFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void dteHizukeYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            // 日付（範囲開始）
            this.ValidateHizukeYMDFrom(e);
        }

        private void dteHizukeYMDTo_Validating(object sender, CancelEventArgs e)
        {
            // 日付（範囲終了）
            this.ValidateHizukeYMDTo(e);
        }

        private void radSeikyusaki_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeSeikyusakiCondition();
        }

        private void fpSeikyusakiListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckSeikyusakiList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpSeikyusakiListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void fpSeikyusakiListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.F12:
                    SendKeys.SendWait("+{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckSeikyusakiList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void ymd_Down(object sender, EventArgs e)
        {
            if (this.ActiveControl.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)this.ActiveControl).Value = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(-1);
                }
                catch
                {
                    ;
                }
            }
        }

        private void ymd_Up(object sender, EventArgs e)
        {
            if (this.ActiveControl.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)this.ActiveControl).Value = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(1);
                }
                catch
                {
                    ;
                }
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            this.SetSelectCheckBox(true);
        }

        private void btnCancelSelected_Click(object sender, EventArgs e)
        {
            this.SetSelectCheckBox(false);
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
