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


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class ItemBunruiFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public ItemBunruiFrame()
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

        #region 品目分類定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "品目分類登録";

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
        /// 品目大分類クラス
        /// </summary>
        private ItemLBunrui _ItemLBunrui;

        /// <summary>
        /// 品目中分類クラス
        /// </summary>
        private ItemMBunrui _ItemMBunrui;

        /// <summary>
        /// 現在編集中の品目大分類情報を保持する領域
        /// </summary>
        private ItemLBunruiInfo _ItemLBunruiInfo = null;

        /// <summary>
        /// 現在編集中の品目中分類情報を保持する領域
        /// </summary>
        private ItemMBunruiInfo _ItemMBunruiInfo = null;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region 品目大分類一覧

        //--Spread列定義

        /// <summary>
        /// 品目大分類コード列番号
        /// </summary>
        private const int COL_LBUNRUI_CODE = 0;

        /// <summary>
        /// 品目大分類名列番号
        /// </summary>
        private const int COL_LBUNRUI_NAME = 1;

        /// <summary>
        /// 品目大分類リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LBUNRUILIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialItemLBunruiStyleInfoArr;

        /// <summary>
        /// 品目大分類情報のリストを保持する領域
        /// </summary>
        private IList<ItemLBunruiInfo> _ItemLBunruiInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowLidx_Result = 0;

        #endregion

        #region 品目中分類一覧

        //--Spread列定義

        /// <summary>
        /// 品目中分類コード列番号
        /// </summary>
        private const int COL_MBUNRUI_CODE = 0;

        /// <summary>
        /// 品目中分類名列番号
        /// </summary>
        private const int COL_MBUNRUI_NAME = 1;

        /// <summary>
        /// 品目中分類リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_MBUNRUILIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialItemMBunruiStyleInfoArr;

        /// <summary>
        /// 品目中分類情報のリストを保持する領域
        /// </summary>
        private IList<ItemMBunruiInfo> _ItemMBunruiInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowMidx_Result = 0;

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitItemBunruiFrame()
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
            this.InitMenuChiku();

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

            //品目大分類クラスインスタンス作成
            this._ItemLBunrui = new ItemLBunrui(this.appAuth);

            //品目中分類クラスインスタンス作成
            this._ItemMBunrui = new ItemMBunrui(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //品目大分類リストのセット
            this.SetLBunruiListSheet();

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
        private void InitMenuChiku()
        {
            // 操作メニュー
            this.InitActionMenuChikus();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuChikus()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.ChangeCode);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.New);
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
            //品目大分類リストの初期化
            this.InitLBunruiListSheet();

            //品目中分類リストの初期化
            this.InitMBunruiListSheet();
        }

        /// <summary>
        /// 品目大分類リストを初期化します。
        /// </summary>
        private void InitLBunruiListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpLBunruiListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_LBUNRUILIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 品目中分類リストを初期化します。
        /// </summary>
        private void InitMBunruiListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMBunruiListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_MBUNRUILIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //品目大分類のクリア
            this.ClearInputsLBunrui();

            //品目中分類のクリア
            this.ClearInputsMBunrui();
        }

        /// <summary>
        /// 品目大分類項目をクリアします。
        /// </summary>
        private void ClearInputsLBunrui()
        {
            this.edtLBunruiCode.Text = string.Empty;
            this.edtLBunruiName.Text = string.Empty;

            this.chkDisableFlag.Checked = false;

            //品目中分類リストの初期化
            this.InitMBunruiListSheet();

            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// 品目中分類項目をクリアします。
        /// </summary>
        private void ClearInputsMBunrui()
        {
            this.edtMBunruiCode.Text = string.Empty;
            this.edtMBunruiName.Text = string.Empty;

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
            //品目大分類スタイル情報初期化
            this.InitLBunruiStyleInfo();

            //品目中分類スタイル情報初期化
            this.InitMBunruiStyleInfo();
        }

        /// <summary>
        /// 品目大分類のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitLBunruiStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpLBunruiListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialItemLBunruiStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_LBUNRUILIST];

            for (int i = 0; i < COL_MAXCOLNUM_LBUNRUILIST; i++)
            {
                this.initialItemLBunruiStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// 品目中分類のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitMBunruiStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMBunruiListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialItemMBunruiStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_MBUNRUILIST];

            for (int i = 0; i < COL_MAXCOLNUM_MBUNRUILIST; i++)
            {
                this.initialItemMBunruiStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtLBunruiCode, ctl => ctl.Text, this.edtLBunruiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtMBunruiCode, ctl => ctl.Text, this.edtMBunruiCode_Validating));
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***新規作成
            _commandSet.New.Execute += New_Execute;
            _commandSet.Bind(
                _commandSet.New, this.btnNew, this.btnNew2, actionMenuItems.GetMenuItemBy(ActionMenuItems.New), this.toolStripNew);

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

            //***コード変更
            _commandSet.ChangeCode.Execute += ChangeCode_Execute;
            _commandSet.Bind(
                _commandSet.ChangeCode, actionMenuItems.GetMenuItemBy(ActionMenuItems.ChangeCode));

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

        }

        void New_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.Default:
                    this.DoStartNewDataLBunrui();
                    break;
                case FrameEditModeEx.Editable:
                    this.DoStartNewDataMBunrui();
                    break;
                default:
                    break;
            }
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.New:
                case FrameEditModeEx.Editable:
                    this.DoClearLBunrui(true);
                    break;
                case FrameEditModeEx.New2:
                case FrameEditModeEx.Editable2:
                    this.DoClearMBunrui(true);
                    break;
                default:
                    break;
            }
        }

        void Save_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.New:
                case FrameEditModeEx.Editable:
                    this.DoUpdateLBunrui();
                    break;
                case FrameEditModeEx.New2:
                case FrameEditModeEx.Editable2:
                    this.DoUpdateMBunrui();
                    break;
                default:
                    break;
            }
        }

        void Delete_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.Editable:
                    this.DoDelDataLBunrui();
                    break;
                case FrameEditModeEx.Editable2:
                    this.DoDelDataMBunrui();
                    break;
                default:
                    break;
            }
        }

        void ChangeCode_Execute(object sender, EventArgs e)
        {
            switch (this.currentMode)
            {
                case FrameEditModeEx.Editable:
                    this.DoChangeCodeLBunrui();
                    break;
                case FrameEditModeEx.Editable2:
                    this.DoChangeCodeMBunrui();
                    break;
                default:
                    break;
            }
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
            this.InitItemBunruiFrame();
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
                    this.pnlLeft.Enabled = true;
                    this.pnlLeft2.Enabled = false;
                    this.pnlRight.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    this.edtLBunruiCode.Enabled = false;
                    this.edtMBunruiCode.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.New.Enabled = true;
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.New:
                    //新規作成モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlLeft2.Enabled = true;
                    this.pnlRight.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    this.edtLBunruiCode.Enabled = true;
                    this.edtMBunruiCode.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.New.Enabled = false;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.Editable:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlLeft2.Enabled = true;
                    this.pnlRight.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    this.edtLBunruiCode.Enabled = false;
                    this.edtMBunruiCode.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = true;
                    _commandSet.New.Enabled = true;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = true;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.New2:
                    //新規作成モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlLeft2.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtLBunruiCode.Enabled = false;
                    this.edtMBunruiCode.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.New.Enabled = false;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditModeEx.Editable2:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlLeft2.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtLBunruiCode.Enabled = false;
                    this.edtMBunruiCode.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = true;
                    _commandSet.New.Enabled = false;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = true;
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
        /// 品目大分類リストに値を設定します。
        /// </summary>
        private void SetLBunruiListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitLBunruiListSheet();

                //検索結果一覧の取得を指示
                this._ItemLBunruiInfoList =
                    this._ItemLBunrui.GetList();

                if (this._ItemLBunruiInfoList == null)
                {
                    this._ItemLBunruiInfoList = new List<ItemLBunruiInfo>();
                }

                //IList<ChikuInfo> wk_list = this.GetMatchedList(this._ChikuInfoList);
                IList<ItemLBunruiInfo> wk_list = this._ItemLBunruiInfoList;

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
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_LBUNRUILIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_LBUNRUILIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    ItemLBunruiInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LBUNRUILIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialItemLBunruiStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_LBUNRUI_CODE, wk_info.ItemLBunruiCode);
                    datamodel.SetValue(j, COL_LBUNRUI_NAME, wk_info.ItemLBunruiName);

                    datamodel.SetTag(j, COL_LBUNRUI_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpLBunruiListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpLBunruiListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpLBunruiListGrid.Sheets[0].SetActiveCell(selectrowLidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpLBunruiListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                this.fpLBunruiListGrid.Sheets[0].AddSelection(selectrowLidx_Result, -1, 1,
                    this.fpLBunruiListGrid.Sheets[0].ColumnCount - 1);

                this.DoGetDataLBunrui(false);
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
        /// 品目中分類リストに値を設定します。
        /// </summary>
        private void SetMBunruiListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitMBunruiListSheet();

                //検索結果一覧の取得を指示
                this._ItemMBunruiInfoList =
                    this._ItemMBunrui.GetList(
                        new ItemMBunruiSearchParameter()
                        {
                            ItemLBunruiId = this._ItemLBunruiInfo.ItemLBunruiId,
                        });

                if (this._ItemMBunruiInfoList == null)
                {
                    this._ItemMBunruiInfoList = new List<ItemMBunruiInfo>();
                }

                IList<ItemMBunruiInfo> wk_list = this._ItemMBunruiInfoList;

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputsMBunrui();

                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_MBUNRUILIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_MBUNRUILIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    ItemMBunruiInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_MBUNRUILIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialItemMBunruiStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_MBUNRUI_CODE, wk_info.ItemMBunruiCode);
                    datamodel.SetValue(j, COL_MBUNRUI_NAME, wk_info.ItemMBunruiName);

                    datamodel.SetTag(j, COL_MBUNRUI_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpMBunruiListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpMBunruiListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpMBunruiListGrid.Sheets[0].SetActiveCell(selectrowMidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpMBunruiListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                this.fpMBunruiListGrid.Sheets[0].AddSelection(selectrowMidx_Result, -1, 1,
                    this.fpMBunruiListGrid.Sheets[0].ColumnCount - 1);

                this.DoGetDataMBunrui(false);
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
        private void DoGetDataLBunrui(bool changeMode = true)
        {
            //選択品目大分類情報をリストから取得して設定
            ItemLBunruiInfo info = this.GetLBunruiInfoByLBunruiListOnSelection();

            if (info.ItemLBunruiId == 0)
            {
                return;
            }

            try
            {
                //品目大分類情報取得
                this._ItemLBunruiInfo =
                    this._ItemLBunrui.GetInfoById(info.ItemLBunruiId);

                //画面設定
                this.SetScreenLBunrui();

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditModeEx.Editable);

                    //フォーカス設定
                    this.edtLBunruiName.Focus();
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
        /// 品目大分類リストにて選択中の品目大分類情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の品目大分類情報</returns>
        private ItemLBunruiInfo GetLBunruiInfoByLBunruiListOnSelection()
        {
            //返却用
            ItemLBunruiInfo rt_info = new ItemLBunruiInfo();

            SheetView sheet0 = this.fpLBunruiListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowLidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから地区情報を取得
                    rt_info = (ItemLBunruiInfo)sheet0.Cells[select_row, COL_LBUNRUI_CODE].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetDataMBunrui(bool changeMode = true)
        {
            //選択品目中分類情報をリストから取得して設定
            ItemMBunruiInfo info = this.GetMBunruiInfoByMBunruiListOnSelection();

            if (info.ItemMBunruiId == 0)
            {
                return;
            }

            try
            {
                //品目中分類情報取得
                this._ItemMBunruiInfo =
                    this._ItemMBunrui.GetInfoById(this._ItemLBunruiInfo.ItemLBunruiId, info.ItemMBunruiId);

                //画面設定
                this.SetScreenMBunrui();

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditModeEx.Editable2);

                    //フォーカス設定
                    this.edtMBunruiName.Focus();
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
        /// 品目中分類リストにて選択中の品目中分類情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の品目中分類情報</returns>
        private ItemMBunruiInfo GetMBunruiInfoByMBunruiListOnSelection()
        {
            //返却用
            ItemMBunruiInfo rt_info = new ItemMBunruiInfo();

            SheetView sheet0 = this.fpMBunruiListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowMidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagから地区情報を取得
                    rt_info = (ItemMBunruiInfo)sheet0.Cells[select_row, COL_MBUNRUI_CODE].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreenLBunrui()
        {
            if (this._ItemLBunruiInfo == null)
            {
                this.ClearInputs();

                //選択された地区情報を取得
                ItemLBunruiInfo info = this.GetLBunruiInfoByLBunruiListOnSelection();
                this._ItemLBunruiInfo.ItemLBunruiId = info.ItemLBunruiId;

                //表示項目設定
                this.edtLBunruiCode.Text = info.ItemLBunruiCode;
                this.edtLBunruiName.Text = Convert.ToString(info.ItemLBunruiName);
            }
            else
            {
                this.edtLBunruiCode.Text = this._ItemLBunruiInfo.ItemLBunruiCode;
                this.edtLBunruiName.Text = Convert.ToString(this._ItemLBunruiInfo.ItemLBunruiName);
                this.chkDisableFlag.Checked = this._ItemLBunruiInfo.DisableFlag;
            }

            //品目中分類リストを設定
            this.SetMBunruiListSheet();
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreenMBunrui()
        {
            if (this._ItemMBunruiInfo == null)
            {
                this.ClearInputsMBunrui();

                //選択された地区情報を取得
                ItemMBunruiInfo info = this.GetMBunruiInfoByMBunruiListOnSelection();
                this._ItemMBunruiInfo.ItemMBunruiId = info.ItemMBunruiId;

                //表示項目設定
                this.edtMBunruiCode.Text = info.ItemMBunruiCode;
                this.edtMBunruiName.Text = Convert.ToString(info.ItemMBunruiName);
            }
            else
            {
                this.edtMBunruiCode.Text = this._ItemMBunruiInfo.ItemMBunruiCode;
                this.edtMBunruiName.Text = Convert.ToString(this._ItemMBunruiInfo.ItemMBunruiName);
                this.chkDisableFlag.Checked = this._ItemMBunruiInfo.DisableFlag;
            }
        }

        /// <summary>
        /// 品目大分類の新規登録を開始します
        /// </summary>
        private void DoStartNewDataLBunrui()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の品目大分類情報を保持する領域の初期化
            this._ItemLBunruiInfo = new ItemLBunruiInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditModeEx.New);

            //フォーカス移動
            this.edtLBunruiCode.Focus();
        }

        /// <summary>
        /// 品目中分類の新規登録を開始します
        /// </summary>
        private void DoStartNewDataMBunrui()
        {
            //入力項目のクリア
            this.ClearInputsMBunrui();

            //現在編集中の品目中分類情報を保持する領域の初期化
            this._ItemMBunruiInfo = new ItemMBunruiInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditModeEx.New2);

            //フォーカス移動
            this.edtMBunruiCode.Focus();
        }
        
        /// <summary>
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdateLBunrui()
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

            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputsLBunrui())
            {
                //画面から値を取得
                this.GetScreenLBunrui();
                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._ItemLBunrui.Save(tx, this._ItemLBunruiInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "品目大分類", _ItemLBunruiInfo.ItemLBunruiCode.ToString() });

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
                    this.DoClearLBunrui(false);
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    if (this.currentMode == FrameEditModeEx.New)
                    {
                        this.edtLBunruiCode.Focus();
                    }
                    else
                    {
                        this.edtLBunruiName.Focus();
                    }
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
                    if (this.currentMode == FrameEditModeEx.New)
                    {
                        this.edtLBunruiCode.Focus();
                    }
                    else
                    {
                        this.edtLBunruiName.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 画面情報からデータを更新します。
        /// </summary>
        private void DoUpdateMBunrui()
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

            if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputsMBunrui())
            {
                //画面から値を取得
                this.GetScreenMBunrui();
                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._ItemMBunrui.Save(tx, this._ItemMBunruiInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "品目中分類", "品目大分類コード:" + _ItemLBunruiInfo.ItemLBunruiCode.ToString()
                            + "、品目中分類コード:" + _ItemMBunruiInfo.ItemMBunruiCode.ToString() });

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
                    this.DoClearMBunrui(false);
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    if (this.currentMode == FrameEditModeEx.New2)
                    {
                        this.edtMBunruiCode.Focus();
                    }
                    else
                    {
                        this.edtMBunruiName.Focus();
                    }
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
                    if (this.currentMode == FrameEditModeEx.New2)
                    {
                        this.edtMBunruiCode.Focus();
                    }
                    else
                    {
                        this.edtMBunruiName.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreenLBunrui()
        {
            this._ItemLBunruiInfo.ItemLBunruiCode = this.edtLBunruiCode.Text.Trim();
            this._ItemLBunruiInfo.ItemLBunruiName = this.edtLBunruiName.Text.Trim();
            this._ItemLBunruiInfo.DisableFlag = this.chkDisableFlag.Checked;
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreenMBunrui()
        {
            this._ItemMBunruiInfo.ItemLBunruiId = this._ItemLBunruiInfo.ItemLBunruiId;
            this._ItemMBunruiInfo.ItemMBunruiCode = this.edtMBunruiCode.Text.Trim();
            this._ItemMBunruiInfo.ItemMBunruiName = this.edtMBunruiName.Text.Trim();
            this._ItemMBunruiInfo.DisableFlag = this.chkDisableFlag.Checked;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputsLBunrui()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            //コードの必須入力チェック
            if (rt_val && String.IsNullOrWhiteSpace(this.edtLBunruiCode.Text.Trim()))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "品目大分類コード" });
                ctl = this.edtLBunruiCode;
            }

            ////名称の必須入力チェック
            //if (rt_val && this.edtLBunruiName.Text.Trim().Length == 0)
            //{
            //    rt_val = false;
            //    msg = FrameUtilites.GetDefineMessage(
            //        "MW2203001", new string[] { "品目大分類名称" });
            //    ctl = this.edtLBunruiName;
            //}

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputsMBunrui()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            //コードの必須入力チェック
            if (rt_val && String.IsNullOrWhiteSpace(this.edtMBunruiCode.Text.Trim()))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "品目中分類コード" });
                ctl = this.edtMBunruiCode;
            }

            ////名称の必須入力チェック
            //if (rt_val && this.edtMBunruiName.Text.Trim().Length == 0)
            //{
            //    rt_val = false;
            //    msg = FrameUtilites.GetDefineMessage(
            //        "MW2203001", new string[] { "品目中分類名称" });
            //    ctl = this.edtMBunruiName;
            //}

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
        private void DoDelDataLBunrui()
        {
            if (0 < this.fpMBunruiListGrid.ActiveSheet.Rows.Count)
            {
                MessageBox.Show(
                    "品目中分類が登録済みのため削除できません。",
                    this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

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
                                this._ItemLBunrui.Delete(tx, _ItemLBunruiInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "品目大分類", _ItemLBunruiInfo.ItemLBunruiCode.ToString() });

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
                            this.DoClearLBunrui(false);
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
        /// 画面情報からデータを削除します。
        /// </summary>
        private void DoDelDataMBunrui()
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
                                this._ItemMBunrui.Delete(tx, _ItemMBunruiInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "品目中分類", "品目大分類コード:" + this._ItemLBunruiInfo.ItemLBunruiCode + "、品目中分類コード:" + _ItemMBunruiInfo.ItemMBunruiCode.ToString() });

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
                            this.DoClearMBunrui(false);
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
        /// <param name="showCancelConfirm">確認画面を表示するかどうか(true:表示する)</param>
        private void DoClearLBunrui(bool showCancelConfirm)
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

            //入力項目をクリア
            this.ClearInputs();

            //Spread関連をクリア
            this.InitSheet();

            //品目大分類リストをセット
            this.SetLBunruiListSheet();

            //モード変更
            this.ChangeMode(FrameEditModeEx.Default);

            this.fpLBunruiListGrid.Focus();
        }

        /// <summary>
        /// 画面情報をクリアして初期状態に変更します。
        /// </summary>
        /// <param name="showCancelConfirm">確認画面を表示するかどうか(true:表示する)</param>
        private void DoClearMBunrui(bool showCancelConfirm)
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

            //入力項目をクリア
            this.ClearInputsMBunrui();

            //品目中分類リストの初期化
            this.InitMBunruiListSheet();

            //品目中分類リストをセット
            this.SetMBunruiListSheet();

            if (this._ItemLBunruiInfo != null && 0 < this._ItemLBunruiInfo.ItemLBunruiId)
            {
                //モード変更
                this.ChangeMode(FrameEditModeEx.Editable);

                this.fpMBunruiListGrid.Focus();
            }
            else
            {
                //モード変更
                this.ChangeMode(FrameEditModeEx.New);

                this.edtMBunruiCode.Focus();
            }
        }

        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCodeLBunrui()
        {
            //コードを取得
            string oldcode = this.edtLBunruiCode.Text.Trim();

            //コードの桁数を取得
            int codedigitcount = this.edtLBunruiCode.MaxLength;

            //コード変更画面を表示
            using (ChangeMasterCodeTextFrame f =
                new ChangeMasterCodeTextFrame(DefaultProperty.MasterCodeKbn.ItemLBunrui, oldcode, codedigitcount))
            {
                f.InitFrame();

                DialogResult wk_result = f.ShowDialog(this);

                //正常に処理されたら初期状態へ変更
                if (wk_result == DialogResult.OK)
                {
                    //画面初期化
                    this.DoClearLBunrui(false);
                }
            }
        }

        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCodeMBunrui()
        {
            //コードを取得
            string oldcode = this.edtMBunruiCode.Text.Trim();

            //コードの桁数を取得
            int codedigitcount = this.edtMBunruiCode.MaxLength;

            //コード変更画面を表示
            using (ChangeMasterCodeTextFrame f =
                new ChangeMasterCodeTextFrame(DefaultProperty.MasterCodeKbn.ItemMBunrui, oldcode, codedigitcount, this._ItemLBunruiInfo.ItemLBunruiId))
            {
                f.InitFrame();

                DialogResult wk_result = f.ShowDialog(this);

                //正常に処理されたら初期状態へ変更
                if (wk_result == DialogResult.OK)
                {
                    //画面初期化
                    this.DoClearMBunrui(false);
                }
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
        /// 品目大分類コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateLBunruiCode(CancelEventArgs e)
        {
            bool isClear = true;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.LBunruiCode.Equals(string.Empty))
                {
                    return;
                }

                this.edtLBunruiCode.Text = this.LBunruiCode.PadLeft(2, '0');

                isClear = false;
            }
            finally
            {
                if (isClear)
                {
                    this.edtLBunruiCode.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 品目中分類コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateMBunruiCode(CancelEventArgs e)
        {
            bool isClear = true;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.MBunruiCode.Equals(string.Empty))
                {
                    return;
                }

                this.edtMBunruiCode.Text = this.MBunruiCode.PadLeft(3, '0');

                isClear = false;
            }
            finally
            {
                if (isClear)
                {
                    this.edtMBunruiCode.Text = string.Empty;
                }
            }
        }

        #endregion

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.currentMode == FrameEditModeEx.Editable
                || this.currentMode == FrameEditModeEx.Editable2)
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
        private void ProcessFpLBunruiListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetDataLBunrui();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 登録済リスト上でPreviewKeyDownイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpMBunruiListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetDataMBunrui();
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

        private String LBunruiCode
        {
            get { return StringHelper.ConvertToString(this.edtLBunruiCode.Text.Trim()); }
        }

        private String MBunruiCode
        {
            get { return StringHelper.ConvertToString(this.edtMBunruiCode.Text.Trim()); }
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

        private void edtLBunruiCode_Validating(object sender, CancelEventArgs e)
        {
            //品目大分類コード
            this.ValidateLBunruiCode(e);
        }

        private void edtMBunruiCode_Validating(object sender, CancelEventArgs e)
        {
            //品目中分類コード
            this.ValidateMBunruiCode(e);
        }

        private void ItemBunruiFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void ItemBunruiFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpLBunruiListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //品目大分類リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetDataLBunrui();
            }
        }

        private void fpMBunruiListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //品目中分類リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetDataMBunrui();
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpLBunruiListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpLBunruiListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpMBunruiListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpMBunruiListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpLBunruiListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpLBunruiListGridPreviewKeyDown(e);
        }

        private void fpMBunruiListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpMBunruiListGridPreviewKeyDown(e);
        }

        private void ItemBunruiFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void fpLBunruiListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetDataLBunrui(false);
        }

        private void fpMBunruiListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetDataMBunrui(false);
        }
    }
}
