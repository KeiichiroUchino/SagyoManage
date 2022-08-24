using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using GrapeCity.Win.Editors;
using Jpsys.HaishaManageV10.ComLib;
using System.ComponentModel;
using System.Linq;
using GrapeCity.Win.Pickers;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishokuExFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishokuExFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region 配色定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配色登録";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 現在の画面の編集モードを保持する領域
        /// </summary>
        private FrameEditModeEx currentMode = FrameEditModeEx.Default;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 配色クラス
        /// </summary>
        private Haishoku _Haishoku;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList;

        /// <summary>
        /// 現在編集中のテーブル情報を保持する領域
        /// </summary>
        private SystemNameInfo _TableKeyInfo = null;

        /// <summary>
        /// 現在編集中の機能情報を保持する領域
        /// </summary>
        private SystemNameInfo _FunctionKeyInfo = null;

        /// <summary>
        /// 現在編集中の対象情報を保持する領域
        /// </summary>
        private TargetKeyInfo _TargetKeyInfo = null;

        /// <summary>
        /// 現在編集中の配色情報を保持する領域
        /// </summary>
        private HaishokuExInfo _HaishokuInfo = null;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region テーブル一覧

        //--Spread列定義

        /// <summary>
        /// テーブル名列番号
        /// </summary>
        private const int COL_TABLEKEY_NAME = 0;

        /// <summary>
        /// テーブル情報リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TABLEKEYLIST = 1;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialTableKeyStyleInfoArr;

        /// <summary>
        /// テーブル情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> _TableKeyList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowTableKeyidx_Result = 0;

        #endregion

        #region 機能一覧

        //--Spread列定義

        /// <summary>
        /// 機能名列番号
        /// </summary>
        private const int COL_FUNCTIONKEY_NAME = 0;

        /// <summary>
        /// 機能情報リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_FUNCTIONKEYLIST = 1;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialFunctionKeyStyleInfoArr;

        /// <summary>
        /// テーブル情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> _FunctionKeyList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowFunctionKeyidx_Result = 0;

        #endregion

        #region 対象一覧

        //--Spread列定義

        /// <summary>
        /// 対象コード列番号
        /// </summary>
        private const int COL_TARGETKEY_CODE = 0;

        /// <summary>
        /// 対象名列番号
        /// </summary>
        private const int COL_TARGETKEY_NAME = 1;

        /// <summary>
        /// 対象情報リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TARGETKEYLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialTargetKeyStyleInfoArr;

        /// <summary>
        /// 対象情報のリストを保持する領域
        /// </summary>
        private IList<TargetKeyInfo> _TargetKeyList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowTargetKeyidx_Result = 0;

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitHaishokuExFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
            this.InitFrameColor();

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
            this.InitMenuHaishoku();

            //バインドの設定
            this.SettingCommands();
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //配色クラスインスタンス作成
            this._Haishoku = new Haishoku(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //入力項目のクリア
            this.ClearInputs();

            //検索条件入力項目のクリア
            this.ClearSearchInputs();

            //Spread関連のクリア
            this.InitSheet();

            //テーブル名リストのセット
            this.SetTableKeyListSheet();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditModeEx.Default);
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
        /// メニュー関連の初期化をします。
        /// </summary>
        private void InitMenuHaishoku()
        {
            // 操作メニュー
            this.InitActionMenuHaishoku();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuHaishoku()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //テーブルリストの初期化
            this.InitTableKeyListSheet();

            //機能リストの初期化
            this.InitFunctionKeyListSheet();

            //対象リストの初期化
            this.InitTargetKeyListSheet();
        }

        /// <summary>
        /// テーブルリストを初期化します。
        /// </summary>
        private void InitTableKeyListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTableKeyListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_TABLEKEYLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 機能リストを初期化します。
        /// </summary>
        private void InitFunctionKeyListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpFunctionKeyListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_FUNCTIONKEYLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 対象リストを初期化します。
        /// </summary>
        private void InitTargetKeyListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTargetKeyListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_TARGETKEYLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //テーブル一覧のクリア
            this.ClearInputsTableKeyList();

            //機能一覧のクリア
            this.ClearInputsFunctionKeyList();

            //対象一覧のクリア
            this.ClearInputsTargetKeyList();

            //配色情報のクリア
            this.ClearInputsHaishokuInfo();
        }

        /// <summary>
        /// 検索条件入力項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.edtSearch.Text = string.Empty;
            this.chkAllFlag.Checked = false;
        }

        /// <summary>
        /// テーブル一覧をクリアします。
        /// </summary>
        private void ClearInputsTableKeyList()
        {
            //テーブルリストの初期化
            this.InitTableKeyListSheet();
        }

        /// <summary>
        ///機能一覧をクリアします。
        /// </summary>
        private void ClearInputsFunctionKeyList()
        {
            //機能一覧リストの初期化
            this.InitFunctionKeyListSheet();
        }

        /// <summary>
        ///対象一覧をクリアします。
        /// </summary>
        private void ClearInputsTargetKeyList()
        {
            //対象一覧リストの初期化
            this.InitTargetKeyListSheet();
        }

        /// <summary>
        /// 配色情報をクリアします。
        /// </summary>
        private void ClearInputsHaishokuInfo()
        {
            this.clpForeColor.ResetText();
            this.clpBackColor.ResetText();

            this.chkDisableFlag.Checked = false;

            //メンバをクリア
            this.isConfirmClose = true;
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
            //テーブル一覧スタイル情報初期化
            this.InitTableKeyListStyleInfo();

            //機能一覧スタイル情報初期化
            this.InitFunctionKeyListStyleInfo();

            //対象一覧スタイル情報初期化
            this.InitTargetKeyListStyleInfo();
        }

        /// <summary>
        /// テーブル一覧のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitTableKeyListStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTableKeyListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialTableKeyStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TABLEKEYLIST];

            for (int i = 0; i < COL_MAXCOLNUM_TABLEKEYLIST; i++)
            {
                this.initialTableKeyStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 機能一覧のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitFunctionKeyListStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpFunctionKeyListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialFunctionKeyStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_FUNCTIONKEYLIST];

            for (int i = 0; i < COL_MAXCOLNUM_FUNCTIONKEYLIST; i++)
            {
                this.initialFunctionKeyStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 対象一覧のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitTargetKeyListStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTargetKeyListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialTargetKeyStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TARGETKEYLIST];

            for (int i = 0; i < COL_MAXCOLNUM_TARGETKEYLIST; i++)
            {
                this.initialTargetKeyStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***編集取消
            _commandSet.EditCancel.Execute += EditCancel_Execute;
            _commandSet.Bind(
                _commandSet.EditCancel, this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.EditCancel), this.toolStripCancel);

            //***保存
            _commandSet.Save.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.Save, this.btnSave, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            //***削除
            _commandSet.Delete.Execute += Delete_Execute;
            _commandSet.Bind(
                _commandSet.Delete, this.btnDelete, actionMenuItems.GetMenuItemBy(ActionMenuItems.Delete), this.toolStripRemove);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.Editable:
                    this.DoClearTableKey();
                    break;
                case FrameEditModeEx.Editable2:
                    this.DoClearFunctionKey();
                    break;
                case FrameEditModeEx.Editable3:
                    this.DoClearTargetKey(true);
                    break;
                default:
                    break;
            }
        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
        }

        void Delete_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishokuExFrame();
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインターフェース経由で
        /// 取得・設定します。
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
        /// 本インスタンスのTextプロパティをインターフェース経由で
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

        #region プライベートメソッド

        /// <summary>
        /// 画面モードを表す列挙体を指定して、現在の画面の
        /// 表示モードを切り替えます。
        /// </summary>
        /// <param name="mode">変更したい画面モード</param>
        private void ChangeMode(FrameEditModeEx mode)
        {
            switch (mode)
            {
                case FrameEditModeEx.Default:
                    //初期状態
                    //--コントロールの使用可否
                    this.pnl01.Enabled = true;
                    this.pnl02.Enabled = false;
                    this.pnl03.Enabled = false;
                    this.pnl04.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.Editable:
                    //編集モード
                    this.pnl01.Enabled = false;
                    this.pnl02.Enabled = true;
                    this.pnl03.Enabled = false;
                    this.pnl04.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.Editable2:
                    //編集モード
                    this.pnl01.Enabled = false;
                    this.pnl02.Enabled = false;
                    this.pnl03.Enabled = true;
                    this.pnl04.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.Editable3:
                    //編集モード
                    this.pnl01.Enabled = false;
                    this.pnl02.Enabled = false;
                    this.pnl03.Enabled = false;
                    this.pnl04.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = this._HaishokuInfo != null && this._HaishokuInfo.IsPersisted;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// テーブルリストに値を設定します。
        /// </summary>
        private void SetTableKeyListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitTableKeyListSheet();

                //検索結果一覧の取得を指示
                this._TableKeyList =
                    this.systemNameList
                    .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishokuTableKbn)
                        && x.IntegerValue01 > 0)
                    .ToList();

                if (this._TableKeyList == null)
                {
                    this._TableKeyList = new List<SystemNameInfo>();
                }

                IList<SystemNameInfo> wk_list = this._TableKeyList;

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputs();

                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_TABLEKEYLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_TABLEKEYLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    SystemNameInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TABLEKEYLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialTableKeyStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_TABLEKEY_NAME, wk_info.SystemName);

                    datamodel.SetTag(j, COL_TABLEKEY_NAME, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpTableKeyListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpTableKeyListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpTableKeyListGrid.Sheets[0].SetActiveCell(selectrowTableKeyidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpTableKeyListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                this.fpTableKeyListGrid.Sheets[0].AddSelection(selectrowTableKeyidx_Result, -1, 1,
                    this.fpTableKeyListGrid.Sheets[0].ColumnCount - 1);

                this.DoGetDataTableKey(false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 機能リストに値を設定します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void SetFunctionKeyListSheet(bool changeMode = true)
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitFunctionKeyListSheet();

                //検索結果一覧の取得を指示
                this._FunctionKeyList =
                    this.systemNameList
                    .Where(x => x.SystemNameKbn == this._TableKeyInfo.IntegerValue01)
                    .ToList();

                if (this._FunctionKeyList == null)
                {
                    this._FunctionKeyList = new List<SystemNameInfo>();
                }

                IList<SystemNameInfo> wk_list = this._FunctionKeyList;

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputsFunctionKeyList();

                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_FUNCTIONKEYLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_FUNCTIONKEYLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    SystemNameInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_FUNCTIONKEYLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialFunctionKeyStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_FUNCTIONKEY_NAME, wk_info.SystemName);

                    datamodel.SetTag(j, COL_FUNCTIONKEY_NAME, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpFunctionKeyListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpFunctionKeyListGrid.Sheets[0].Models.Style = stylemodel;

                if (changeMode)
                {
                    //選択行にフォーカスを合わせて選択状態にする
                    this.fpFunctionKeyListGrid.Sheets[0].SetActiveCell(selectrowFunctionKeyidx_Result, 0, true);

                    //選択行にスクロールバーを移動させる
                    this.fpFunctionKeyListGrid.ShowActiveCell(
                        FarPoint.Win.Spread.VerticalPosition.Top,
                        FarPoint.Win.Spread.HorizontalPosition.Left);

                    this.fpFunctionKeyListGrid.Sheets[0].AddSelection(selectrowFunctionKeyidx_Result, -1, 1,
                        this.fpFunctionKeyListGrid.Sheets[0].ColumnCount - 1);
                }
                else
                {    //選択範囲をクリアします
                    this.fpFunctionKeyListGrid.Sheets[0].ClearSelection();

                    this.selectrowFunctionKeyidx_Result = 0;
                }

                this.DoGetDataFunctionKey(false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 対象リストに値を設定します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void SetTargetKeyListSheet(bool changeMode = true)
        {
            if (this._TableKeyInfo == null || this._TableKeyInfo.SystemNameCode == 0
                || this._FunctionKeyInfo == null || this._FunctionKeyInfo.SystemNameCode == 0)
            {
                return;
            }

            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧の取得を指示
                this._TargetKeyList = this._Haishoku.GetTargetKeyList(this._TableKeyInfo, this._FunctionKeyInfo);

                if (this._TargetKeyList == null)
                {
                    this._TargetKeyList = new List<TargetKeyInfo>();
                }

                IList<TargetKeyInfo> wk_list = this.GetMatchedList(this._TargetKeyList);

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputsTargetKeyList();

                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_TARGETKEYLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_TARGETKEYLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    TargetKeyInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TARGETKEYLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialTargetKeyStyleInfoArr[k]);

                        //[使用しない]データの場合は、背景色をグレーに変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_TARGETKEY_CODE, wk_info.TargetKeyCode);
                    datamodel.SetValue(j, COL_TARGETKEY_NAME, wk_info.TargetKeyName);

                    datamodel.SetTag(j, COL_TARGETKEY_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpTargetKeyListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpTargetKeyListGrid.Sheets[0].Models.Style = stylemodel;

                if (changeMode)
                {
                    //選択行にフォーカスを合わせて選択状態にする
                    this.fpTargetKeyListGrid.Sheets[0].SetActiveCell(selectrowTargetKeyidx_Result, 0, true);

                    //選択行にスクロールバーを移動させる
                    this.fpTargetKeyListGrid.ShowActiveCell(
                        FarPoint.Win.Spread.VerticalPosition.Top,
                        FarPoint.Win.Spread.HorizontalPosition.Left);

                    this.fpTargetKeyListGrid.Sheets[0].AddSelection(selectrowTargetKeyidx_Result, -1, 1,
                        this.fpTargetKeyListGrid.Sheets[0].ColumnCount - 1);
                }
                else
                {    //選択範囲をクリアします
                    this.fpTargetKeyListGrid.Sheets[0].ClearSelection();

                    this.selectrowTargetKeyidx_Result = 0;
                }

                this.DoGetDataTargetKey(false);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 検索テーブル情報の一覧を指定して、抽出条件に合致する検索テーブル情報の一覧を取得します。
        /// </summary>
        /// <param name="list">検索テーブル情報の一覧</param>
        private List<TargetKeyInfo> GetMatchedList(IList<TargetKeyInfo> list)
        {
            List<TargetKeyInfo> rt_list = new List<TargetKeyInfo>();

            //検索条件「名称」
            string joken_Mei = this.edtSearch.Text.Trim().ToLower();

            //全角に変換
            joken_Mei =
                Microsoft.VisualBasic.Strings.StrConv(
                    joken_Mei, Microsoft.VisualBasic.VbStrConv.Wide, 0x411);

            //カナの一致のタイプをチェックボックスから列挙値に変換する
            FrameLib.SpecialStringContaintType kana_type;

            //包含一致
            kana_type = SpecialStringContaintType.Contains;

            IList<TargetKeyInfo> wk_list = list;

            if (!this.chkAllFlag.Checked)
            {
                var var_list = list.Where(x => x.DisableFlag == false);
                if (var_list != null && 0 < var_list.Count())
                {
                    wk_list = var_list.ToList();
                }
            }

            foreach (TargetKeyInfo item in wk_list)
            {
                //「コード」+「名称」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.TargetKeyCode) + Environment.NewLine
                    + item.TargetKeyName.Trim() + Environment.NewLine;

                //半角を全角に変換する
                string zenkaku =
                    Microsoft.VisualBasic.Strings.StrConv(
                        item_mei, Microsoft.VisualBasic.VbStrConv.Wide, 0x411);

                //検索条件に合致しているかどうかの値(true:一致)
                bool match = true;

                //名称のチェック
                if (match)
                {
                    if (joken_Mei.Length > 0 &&
                        !FrameUtilites.SpecialContaintString(zenkaku.ToLower(), joken_Mei, kana_type))
                    {
                        match = false;
                    }
                }

                //全ての抽出条件に合致していればリストに追加
                if (match)
                {
                    rt_list.Add(item);
                }
            }

            return rt_list;
        }

        /// <summary>
        /// 配色情報を設定します。
        /// </summary>
        private void SetHaishokuInfo()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //配色情報を初期化
                this.ClearInputsHaishokuInfo();

                //検索結果一覧の取得を指示
                this._HaishokuInfo = this._Haishoku.GetInfoEx(
                    new HaishokuExSearchParameter()
                    {
                        TableKey = this._TargetKeyInfo.TableKey,
                        FunctionKey = this._TargetKeyInfo.FunctionKey,
                        TargetKey = this._TargetKeyInfo.TargetKey,
                        GetDisableDataFlag = true
                    });

                if (this._HaishokuInfo == null)
                {
                    this._HaishokuInfo = new HaishokuExInfo(); ;
                }
                else
                {
                    if (this._HaishokuInfo.ForeColor == null)
                    {
                        this.clpForeColor.SelectedColor = Color.Empty;
                    }
                    else
                    {
                        this.clpForeColor.SelectedColor = ColorTranslator.FromOle(this._HaishokuInfo.ForeColor.Value);
                    }
                    if (this._HaishokuInfo.BackColor == null)
                    {
                        this.clpBackColor.SelectedColor = Color.Empty;
                    }
                    else
                    {
                        this.clpBackColor.SelectedColor = ColorTranslator.FromOle(this._HaishokuInfo.BackColor.Value);
                    }

                    this.chkDisableFlag.Checked = _HaishokuInfo.DisableFlag;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetDataTableKey(bool changeMode = true)
        {
            //選択テーブル情報をリストから取得して設定
            SystemNameInfo info = this.GetTableKeyByTableKeyListOnSelection();

            if (info == null || info.IntegerValue01 == 0)
            {
                return;
            }

            try
            {
                //テーブル情報取得
                this._TableKeyInfo = info;

                //画面設定
                this.SetScreenTableKey(changeMode);

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditModeEx.Editable);

                    //フォーカス設定
                    this.ActiveControl = this.fpFunctionKeyListGrid;
                    this.fpFunctionKeyListGrid.Focus();
                }
            }
            catch (CanRetryException ex)
            {
                //データがない場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 発着地大分類リストにて選択中の発着地大分類情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の発着地大分類情報</returns>
        private SystemNameInfo GetTableKeyByTableKeyListOnSelection()
        {
            //返却用
            SystemNameInfo rt_info = new SystemNameInfo();

            SheetView sheet0 = this.fpTableKeyListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowTableKeyidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから地区情報を取得
                    rt_info = (SystemNameInfo)sheet0.Cells[select_row, COL_TABLEKEY_NAME].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetDataFunctionKey(bool changeMode = true)
        {
            //選択機能情報をリストから取得して設定
            SystemNameInfo info = this.GetFunctionKeyByFunctionKeyListOnSelection();

            if (info == null || info.SystemNameKbn == 0)
            {
                return;
            }

            try
            {
                //機能情報取得
                this._FunctionKeyInfo = info;

                //画面設定
                this.SetScreenFunctionKey(changeMode);

                if (changeMode)
                {
                    //検索条件入力項目のクリア
                    this.ClearSearchInputs();

                    //モード設定
                    this.ChangeMode(FrameEditModeEx.Editable2);

                    //フォーカス設定
                    this.ActiveControl = this.fpTargetKeyListGrid;
                    this.fpTargetKeyListGrid.Focus();
                }
            }
            catch (CanRetryException ex)
            {
                //データがない場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 機能リストにて選択中の機能情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の機能情報</returns>
        private SystemNameInfo GetFunctionKeyByFunctionKeyListOnSelection()
        {
            //返却用
            SystemNameInfo rt_info = new SystemNameInfo();

            SheetView sheet0 = this.fpFunctionKeyListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowFunctionKeyidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから機能情報を取得
                    rt_info = (SystemNameInfo)sheet0.Cells[select_row, COL_FUNCTIONKEY_NAME].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetDataTargetKey(bool changeMode = true)
        {
            //選択機能情報をリストから取得して設定
            this._TargetKeyInfo = this.GetTargetKeyByTargetKeyListOnSelection();

            if (this._TargetKeyInfo == null || this._TargetKeyInfo.TargetKeyCode == 0)
            {
                return;
            }

            try
            {
                //画面設定
                this.SetScreenTargetKey();

                //配色コントロールの活性／非活性
                this.SetColorEnabled();

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditModeEx.Editable3);

                    //フォーカス設定
                    this.ActiveControl = this.fpTargetKeyListGrid;
                    this.fpTargetKeyListGrid.Focus();
                }
            }
            catch (CanRetryException ex)
            {
                //データがない場合の例外ハンドラ
                FrameUtilites.ShowExceptionMessage(ex, this);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 機能リストにて選択中の機能情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の機能情報</returns>
        private TargetKeyInfo GetTargetKeyByTargetKeyListOnSelection()
        {
            //返却用
            TargetKeyInfo rt_info = new TargetKeyInfo();

            SheetView sheet0 = this.fpTargetKeyListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowTargetKeyidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから機能情報を取得
                    rt_info = (TargetKeyInfo)sheet0.Cells[select_row, COL_TARGETKEY_CODE].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void SetScreenTableKey(bool changeMode = true)
        {
            //機能リストを設定
            this.SetFunctionKeyListSheet(changeMode);
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void SetScreenFunctionKey(bool changeMode = true)
        {
            //検索条件入力項目のクリア
            this.ClearSearchInputs();

            //対象リストを設定
            this.SetTargetKeyListSheet(changeMode);
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreenTargetKey()
        {
            //配色情報を設定
            this.SetHaishokuInfo();
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetColorEnabled()
        {
            bool backForeEnabled = this._FunctionKeyInfo != null
                && this._FunctionKeyInfo.IntegerValue01 != (int)DefaultProperty.HaishokuTaishoKbn.OnlyBackColor;
            this.lblForeColor.Enabled = backForeEnabled;
            this.clpForeColor.Enabled = backForeEnabled;

            bool backColorEnabled = this._FunctionKeyInfo != null
                && this._FunctionKeyInfo.IntegerValue01 != (int)DefaultProperty.HaishokuTaishoKbn.OnlyForeColor;
            this.lblBackColor.Enabled = backColorEnabled;
            this.clpBackColor.Enabled = backColorEnabled;
        }

        /// <summary>
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdate()
        {
            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2102007"),
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
            {
                //画面から値を取得
                this.GetScreen();
                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._Haishoku.SaveEx(tx, this._HaishokuInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "配色情報", this._TableKeyInfo.SystemName + " "
                                + this._FunctionKeyInfo.SystemName + " "
                                + this._TargetKeyInfo.TargetKeyCode.ToString()
                                + this._TargetKeyInfo.TargetKeyName });

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //操作ログ出力
                    FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                        FrameLogWriter.LoggingOperateKind.UpdateItem,
                            log_jyoken);

                    //更新完了のメッセージ
                    msg =
                        FrameUtilites.GetDefineMessage("MI2001003");

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //初期状態へ移行
                    this.DoClearTargetKey(false);
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    this.clpBackColor.Focus();
                }
                catch (MustCloseFormException ex)
                {
                    //画面の終了が要求される例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //画面を閉じます
                    this.DoClose();
                }
                catch (Model.DALExceptions.UniqueConstraintException ex)
                {
                    //同一コードが存在する場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    this.clpBackColor.Focus();
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            this._HaishokuInfo.TableKey = this._TableKeyInfo.SystemNameCode;
            this._HaishokuInfo.FunctionKey = this._FunctionKeyInfo.SystemNameCode;
            this._HaishokuInfo.TargetKey = this._TargetKeyInfo.TargetKey;
            if (this.clpForeColor.SelectedColor == Color.Empty)
            {
                this._HaishokuInfo.ForeColor = null;
            }
            else
            {
                this._HaishokuInfo.ForeColor = ColorTranslator.ToOle(this.clpForeColor.SelectedColor);
            }
            if (this.clpBackColor.SelectedColor == Color.Empty)
            {
                this._HaishokuInfo.BackColor = null;
            }
            else
            {
                this._HaishokuInfo.BackColor = ColorTranslator.ToOle(this.clpBackColor.SelectedColor);
            }

            this._HaishokuInfo.DisableFlag = this.chkDisableFlag.Checked;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// 画面情報からデータを削除します。
        /// </summary>
        private void DoDelData()
        {
            //削除確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2101004"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2);

            //Yesだったら
            if (d_result == DialogResult.Yes)
            {
                //削除確認画面を表示
                using (DeleteMessage f = new DeleteMessage())
                {
                    f.InitFrame();

                    DialogResult wk_d_result = f.ShowDialog(this);

                    //削除OK
                    if (wk_d_result == DialogResult.OK)
                    {
                        try
                        {
                            //削除
                            SQLHelper.ActionWithTransaction(tx =>
                            {
                                this._Haishoku.DeleteEx(tx, this._HaishokuInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "配色情報", this._TableKeyInfo.SystemName + " "
                                + this._FunctionKeyInfo.SystemName + " "
                                + this._TargetKeyInfo.TargetKeyCode.ToString()
                                + this._TargetKeyInfo.TargetKeyName });

                            //操作ログ出力（削除）
                            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                                FrameLogWriter.LoggingOperateKind.DelItem,
                                log_jyoken);

                            //削除完了メッセージ
                            MessageBox.Show(
                                FrameUtilites.GetDefineMessage("MI2001004"),
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            //初期状態へ移行
                            this.DoClearTargetKey(false);
                        }
                        catch (CanRetryException ex)
                        {
                            //他から更新されている場合の例外ハンドラ
                            FrameUtilites.ShowExceptionMessage(ex, this);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        private void DoClearTableKey()
        {
            //入力項目をクリア
            this.ClearInputs();

            //Spread関連をクリア
            this.InitSheet();

            //テーブルリストをセット
            this.SetTableKeyListSheet();

            //モード変更
            this.ChangeMode(FrameEditModeEx.Default);

            //フォーカス設定
            this.fpTableKeyListGrid.Focus();
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        private void DoClearFunctionKey()
        {
            //機能リストの初期化
            this.InitFunctionKeyListSheet();

            //機能リストをセット
            this.SetFunctionKeyListSheet();

            //モード変更
            this.ChangeMode(FrameEditModeEx.Editable);

            //フォーカス設定
            this.fpFunctionKeyListGrid.Focus();
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        /// <param name="showCancelConfirm">確認画面を表示するかどうか(true:表示する)</param>
        private void DoClearTargetKey(bool showCancelConfirm)
        {
            if (showCancelConfirm)
            {
                //取消確認の実行確認ダイアログ
                DialogResult d_result =
                    MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MQ2102008"),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }
            }

            //対象リストの初期化
            this.InitTargetKeyListSheet();

            //対象リストをセット
            this.SetTargetKeyListSheet();

            //モード変更
            this.ChangeMode(FrameEditModeEx.Editable2);

            this.fpTargetKeyListGrid.Focus();
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

        #endregion

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.currentMode == FrameEditModeEx.Editable3)
            {
                if (this.isConfirmClose)
                {
                    DialogResult result =
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MQ2102001"),
                            this.Text,
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        //Yesの場合は閉じる
                        e.Cancel = false;
                    }
                    else if (result == DialogResult.No)
                    {
                        //Noの場合は終了をキャンセル
                        e.Cancel = true;
                    }
                }
            }
        }

        /// <summary>
        /// 登録済リスト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpTableKeyListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetDataTableKey();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 登録済リスト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpFunctionKeyListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetDataFunctionKey();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 登録済リスト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpTargetKeyListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetDataTargetKey();
                    break;
                case Keys.F12:
                    SendKeys.Send("+{TAB}");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

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

        private void HaishokuExFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void HaishokuExFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpTableKeyListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //テーブルリスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetDataTableKey();
            }
        }

        private void FpFunctionKeyListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //機能リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetDataFunctionKey();
            }
        }

        private void FpTargetKeyListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //対象リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetDataTargetKey();
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpTableKeyListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpTableKeyListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpFunctionKeyListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpFunctionKeyListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpTargetKeyListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpTargetKeyListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpTableKeyListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpTableKeyListGridPreviewKeyDown(e);
        }

        private void fpFunctionKeyListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpFunctionKeyListGridPreviewKeyDown(e);
        }

        private void fpTargetKeyListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpTargetKeyListGridPreviewKeyDown(e);
        }

        private void HaishokuExFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void fpTableKeyListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetDataTableKey(false);
        }

        private void fpFunctionKeyListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetDataFunctionKey(false);
        }

        private void fpTargetKeyListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetDataTargetKey(false);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowTargetKeyidx_Result = 0;

            //対象リスト再描画
            this.SetTargetKeyListSheet();
        }

        private void chkAllFlag_CheckedChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowTargetKeyidx_Result = 0;

            //対象リスト再描画
            this.SetTargetKeyListSheet();
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void clp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{F4}");
        }

        private void clp_TextChanged(object sender, EventArgs e)
        {
            //色を削除した場合
            if (String.IsNullOrEmpty(((GcColorPicker)sender).Text))
            {
                ((GcColorPicker)sender).SelectedColor = Color.Empty;
            }
        }
    }
}
