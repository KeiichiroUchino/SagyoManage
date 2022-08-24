using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using GrapeCity.Win.Editors;
using jp.co.jpsys.util;
using Jpsys.SagyoManage.ComLib;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Frame;
using Jpsys.SagyoManage.Frame.Command;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.FrameLib.ViewSupport;
using Jpsys.SagyoManage.FrameLib.InputMan;
using Jpsys.SagyoManage.FrameLib.WinForms;


namespace Jpsys.SagyoManage.Frame
{
    public partial class OperatorFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public OperatorFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "利用者登録";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// 現在の画面の編集モードを保持する領域
        /// </summary>
        private FrameEditMode currentMode = FrameEditMode.Default;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;
        
        //--ローカル変数
        /// <summary>
        /// オペレータクラス
        /// </summary>
        private Operator _Operator;        

        /// <summary>
        /// 現在編集中のオペレータ情報を保持する領域
        /// </summary>
        private OperatorInfo _OperatorInfo = null;

        /// <summary>
        /// 外部から指定されたオペレータコードを保持する領域
        /// </summary>
        private string _operatorCode = string.Empty;

        /// <summary>
        /// ログインユーザの管理者権限を保持する領域
        /// </summary>
        private bool adminFrag = false;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        #region オペレータ一覧

        //--Spread列定義

        /// <summary>
        /// オペレータ名列番号
        /// </summary>
        private const int COL_OPERATOR_NAME = 0;

        /// <summary>
        /// ログインID列番号
        /// </summary>
        private const int COL_OPERATOR_LOGINID = 1;

        /// <summary>
        /// オペレータリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_OPERATORLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialOperatorStyleInfoArr;

        /// <summary>
        /// オペレータ情報のリストを保持する領域
        /// </summary>
        private IList<OperatorInfo> _OperatorInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx_Result = 0;

        /// <summary>
        /// 新規で登録されたコードを保持する領域
        /// </summary>
        private string newcode = string.Empty;

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitOperatorFrame()
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

            //ログインユーザの管理者権限設定
            this.adminFrag = UserProperty.GetInstance().AdminKbn == 1;

            //メニューの初期化
            this.InitMenuItem();

            //バインドの設定
            this.SettingCommands();
            this.SetupSearchStateBinder();
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //オペレータクラスインスタンス作成
            this._Operator = new Operator(this.appAuth);

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numStaffCode, this.ShowCmnSearchStaff},
            };

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //コンボボックスの初期化
            this.InitCombo();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //オペレーターリストのセット
            this.SetOperatorListSheet();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
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
        private void InitMenuItem()
        {
            // 操作メニュー
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
            if (this.adminFrag)
                this.actionMenuItems.SetCreatingItem(ActionMenuItems.New);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            if (this.adminFrag)
                this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitAuthorityCombo();
            //this.InitMasterSecurityLevelCombo();
        }

        /// <summary>
        /// 権限コンボボックスを初期化します。
        /// </summary>
        private void InitAuthorityCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            //foreach (KengenNameInfo item in this._DalUtil.KengenName.GetList())
            //{
            //    datasource.Add(item.Kengen.ToString(), item.KengenName);
            //}

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbAuthority, datasource, true);
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //オペレータリストの初期化
            this.InitOperatorListSheet();
        }

        /// <summary>
        /// オペレータリストを初期化します。
        /// </summary>
        private void InitOperatorListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpOperatorListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_OPERATORLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.edtSearch.Text = string.Empty;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtOperatorName.Text = string.Empty;
            this.edtOperatorCode.Text = string.Empty;
            this.edtPassword.Text = string.Empty;
            this.edtConfirmPassword.Text = string.Empty;
            this.numStaffCode.Tag = null;
            this.numStaffCode.Value = null;
            this.edtStaffName.Text = string.Empty;
            this.cmbAuthority.SelectedIndex = 0;
            this.dteLoginYMD.Value = null;
            this.chkAdminKbn.Checked = false;
            this.chkDisableFlag.Checked = false;

            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitStyleInfo()
        {
            //オペレータスタイル情報初期化
            this.InitOperatorStyleInfo();
        }

        /// <summary>
        /// オペレータのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitOperatorStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpOperatorListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialOperatorStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_OPERATORLIST];

            for (int i = 0; i < COL_MAXCOLNUM_OPERATORLIST; i++)
            {
                this.initialOperatorStyleInfoArr[i] =
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
                ControlValidatingEventRaiser.Create(this.numStaffCode, ctl => ctl.Text, this.numStaffCode_Validating));

        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            if (this.adminFrag)
            {
                //***新規作成
                _commandSet.New.Execute += New_Execute;
                _commandSet.Bind(
                    _commandSet.New, this.btnNew, actionMenuItems.GetMenuItemBy(ActionMenuItems.New), this.toolStripNew);
            }

            //***編集取消
            _commandSet.EditCancel.Execute += EditCancel_Execute;
            _commandSet.Bind(
                _commandSet.EditCancel, this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.EditCancel), this.toolStripCancel);

            //***保存
            _commandSet.Save.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.Save, this.btnUpdate, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            if (this.adminFrag)
            {
                //***削除
                _commandSet.Delete.Execute += Delete_Execute;
                _commandSet.Bind(
                    _commandSet.Delete, this.btnDel, actionMenuItems.GetMenuItemBy(ActionMenuItems.Delete), this.toolStripRemove);
            }

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

        }

        void New_Execute(object sender, EventArgs e)
        {
            this.DoStartNewData();
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoClear(true);
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

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            this.searchStateBinder.AddSearchableControls(
                this.numStaffCode
                );

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 社員コードの値を取得します。
        /// </summary>
        private int StaffCode
        {
            get { return Convert.ToInt32(this.numStaffCode.Value); }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitOperatorFrame();
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
        private void ChangeMode(FrameEditMode mode)
        {
            switch (mode)
            {
                case FrameEditMode.Default:
                    //初期状態
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = true;
                    this.pnlRight.Enabled = false;
                    this.pnlBottom.Enabled = true;
                    //--入力項目の使用可否
                    this.edtOperatorName.Enabled = false;
                    this.edtOperatorCode.Enabled = false;
                    //--ファンクションの使用可否
                    if (this.adminFrag)
                        _commandSet.New.Enabled = true;
                    _commandSet.EditCancel.Enabled = false;
                    if (this.adminFrag)
                        _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.New:
                    //新規作成モード
                    //--コントロールの使用可否
                    this.pnlLeft.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--入力項目の使用可否
                    this.edtOperatorName.Enabled = true;
                    this.edtOperatorCode.Enabled = true;
                    this.SetVisible(false);
                    //--ファンクションの使用可否
                    if (this.adminFrag)
                        _commandSet.New.Enabled = false;
                    _commandSet.EditCancel.Enabled = true;
                    if (this.adminFrag)
                        _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--入力項目の使用可否
                    this.edtOperatorName.Enabled = this.adminFrag;
                    this.edtOperatorCode.Enabled = false;
                    this.SetVisible(true);
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = true;
                    if (this.adminFrag)
                        _commandSet.New.Enabled = true;
                    _commandSet.EditCancel.Enabled = true;
                    if (this.adminFrag)
                        _commandSet.Delete.Enabled = true;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.ViewOnly:
                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// 権限項目の活性／非活性を設定します。
        /// </summary>
        private void SetVisible(bool editFlag)
        {
            //ログインユーザが管理者の場合、権限コンボボックス、管理者フラグ表示
            this.cmbAuthority.Visible = this.adminFrag;
            this.lblAuthority.Visible = this.adminFrag;
            this.chkAdminKbn.Visible = this.adminFrag;
            this.chkDisableFlag.Visible = this.adminFrag;
            this.btnNew.Visible = this.adminFrag;
            this.btnDel.Visible = this.adminFrag;
            this.toolStripNew.Visible = this.adminFrag;
            this.toolStripRemove.Visible = this.adminFrag;
            this.SetLoginYMDVisible(editFlag);
        }

        /// <summary>
        /// ログイン日時項目の活性／非活性を設定します。
        /// </summary>
        private void SetLoginYMDVisible(bool editFlag)
        {
            //編集の場合
            if (editFlag)
            {
                //ログインユーザが管理者、かつ、自分以外の編集の場合、ログイン日時編集表示
                bool visible = this.adminFrag && this.appAuth.OperatorId != this._OperatorInfo.OperatorId;
                this.lblLoginYMD.Visible = visible;
                this.dteLoginYMD.Visible = visible;
                this.btnInitLoginYMD.Visible = visible;
                //表示、かつ、ログイン日時が存在する場合、ボタン活性化
                this.btnInitLoginYMD.Enabled = visible && this.dteLoginYMD.Value != null;
            }
            else
            {
                this.lblLoginYMD.Visible = false;
                this.dteLoginYMD.Visible = false;
                this.btnInitLoginYMD.Visible = false;
                this.btnInitLoginYMD.Enabled = false;
            }
        }

        /// <summary>
        /// オペレーターリストに値を設定します。
        /// </summary>
        private void SetOperatorListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索条件設定
                OperatorSearchParameter para = new OperatorSearchParameter();

                //管理者以外の場合
                if (!this.adminFrag)
                {
                    //自分のみ検索
                    para.OperatoId = this.appAuth.OperatorId;
                }

                //検索結果一覧の取得を指示
                this._OperatorInfoList =
                    this._Operator.GetList(para);

                IList<OperatorInfo> wk_list = this.GetMatchedList(this._OperatorInfoList);

                //件数取得
                int rowCount = this._OperatorInfoList.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputs();

                    //0件の場合は何もしない
                    return;
                }

                //抽出後のリストを取得
                wk_list = wk_list
                    .OrderBy(element => element.DisableFlag)
                    .ThenBy(element => element.OperatorId)
                    .ToList();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_OPERATORLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_OPERATORLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    OperatorInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_OPERATORLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialOperatorStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_OPERATOR_NAME, wk_list[j].OperatorName);
                    datamodel.SetValue(j, COL_OPERATOR_LOGINID, wk_list[j].OperatorCode);

                    datamodel.SetTag(j, COL_OPERATOR_NAME, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpOperatorListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpOperatorListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpOperatorListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpOperatorListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpOperatorListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpOperatorListGrid.Sheets[0].ColumnCount - 1);

                    this.DoGetData(false);
                }
                else
                {
                    //入力項目をクリア
                    this.ClearInputs();
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
        /// 検索テーブル情報の一覧を指定して、抽出条件に合致する検索テーブル情報の一覧を取得します。
        /// </summary>
        /// <param name="list">検索テーブル情報の一覧</param>
        private List<OperatorInfo> GetMatchedList(IList<OperatorInfo> list)
        {
            List<OperatorInfo> rt_list = new List<OperatorInfo>();

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

            foreach (OperatorInfo item in list)
            {
                //「コード」+「名称」であいまい検索
                string item_mei =
                    item.OperatorCode.Trim() + Environment.NewLine
                    + item.OperatorName.Trim() + Environment.NewLine;

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
        /// オペレータの新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中のオペレータ情報を保持する領域の初期化
            this._OperatorInfo = new OperatorInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            if (this.adminFrag)
            {
                this.edtOperatorName.Focus();
            }
            else
            {
                this.edtPassword.Focus();
            }
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //オペレーターコードをリストから取得して設定
            this._operatorCode = this.GetOperatorCodeByOperatorListOnSelection();

            if (this._operatorCode.Trim() == string.Empty)
            {
                this._OperatorInfo = new OperatorInfo();

                this.ChangeMode(FrameEditMode.New);
                if (this.adminFrag)
                {
                    this.edtOperatorName.Focus();
                }
                else
                {
                    this.edtPassword.Focus();
                }
            }
            else if(this._operatorCode.Trim() !=string.Empty)
            {
                try
                {
                    //利用者情報取得
                    this._OperatorInfo =
                        this._Operator.GetInfo(_operatorCode);

                    //画面設定
                    this.SetScreen();

                    if (changeMode)
                    {
                        //モード設定
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        if (this.adminFrag)
                        {
                            this.edtOperatorName.Focus();
                        }
                        else
                        {
                            this.edtPassword.Focus();
                        }
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
        }

        /// <summary>
        /// オペレータリストにて選択中のオペレーターのオペレーターコードを取得します。
        /// 未選択の場合はString.Emptyを返却します。
        /// </summary>
        /// <returns>選択中のオペレーターのログインID</returns>
        private string GetOperatorCodeByOperatorListOnSelection()
        {
            //返却用
            string rt_val = string.Empty;

            SheetView sheet0 = this.fpOperatorListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowidx_Result = select_row;

                //行数を取得
                int rowcount = sheet0.Rows.Count;
                //一覧が0件のときにSelectionCountが1件になりデータを読み込んでしまうため
                if (rowcount > 0)
                {
                    //最初の列にセットしたTagからOperatorInfoを取り出して、OperatorCodeを取得
                    rt_val = ((OperatorInfo)sheet0.Cells[select_row, COL_OPERATOR_NAME].Tag).OperatorCode;
                }
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            if (this._OperatorInfo ==null)
            {
                this.ClearInputs();
            }
            else
            {
                this.edtOperatorName.Text = _OperatorInfo.OperatorName;
                this.edtOperatorCode.Text = _OperatorInfo.OperatorCode;
                this.edtPassword.Text = _OperatorInfo.Password;
                this.edtConfirmPassword.Text = _OperatorInfo.Password;
                this.numStaffCode.Tag = _OperatorInfo.ToraDONStaffId;
                this.numStaffCode.Value = _OperatorInfo.ToraDONStaffCode;
                this.edtStaffName.Text = _OperatorInfo.ToraDONStaffName;
                this.cmbAuthority.SelectedValue = _OperatorInfo.AuthorityId.ToString();
                this.dteLoginYMD.Value = FrameUtilites.StructToNullIfValueIsDefault(_OperatorInfo.LoginYMD);
                this.chkAdminKbn.Checked = NSKUtil.IntToBool(_OperatorInfo.AdminKbn);
                this.chkDisableFlag.Checked = _OperatorInfo.DisableFlag;

                this.SetLoginYMDVisible(true);
            }
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
                        this._Operator.Save(tx, this._OperatorInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "利用者", _OperatorInfo.OperatorCode });

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //操作ログ出力
                    if (this.currentMode == FrameEditMode.New)
                    {
                        //操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                           FrameLogWriter.LoggingOperateKind.NewItem,
                               log_jyoken);

                        //登録完了のメッセージ
                        msg =
                           FrameUtilites.GetDefineMessage("MI2001002");
                    }
                    else
                    {
                        //操作ログ出力
                        FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                            FrameLogWriter.LoggingOperateKind.UpdateItem,
                               log_jyoken);

                        //更新完了のメッセージ
                        msg =
                           FrameUtilites.GetDefineMessage("MI2001003");
                    }

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //初期状態へ移行
                    this.DoClear(false);
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.edtOperatorCode.Focus();
                    }
                    else
                    {
                        if (this.adminFrag)
                        {
                            this.edtOperatorName.Focus();
                        }
                        else
                        {
                            this.edtPassword.Focus();
                        }
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
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.edtOperatorCode.Focus();
                    }
                    else
                    {
                        if (this.adminFrag)
                        {
                            this.edtOperatorName.Focus();
                        }
                        else
                        {
                            this.edtPassword.Focus();
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _OperatorInfo.OperatorName = this.edtOperatorName.Text;
            _OperatorInfo.OperatorCode = this.edtOperatorCode.Text;
            _OperatorInfo.Password = this.edtPassword.Text;
            _OperatorInfo.ToraDONStaffId = Convert.ToDecimal(this.numStaffCode.Tag);
            _OperatorInfo.ToraDONStaffCode = Convert.ToInt32(this.numStaffCode.Value);
            _OperatorInfo.ToraDONStaffName = this.edtOperatorName.Text.Trim();
            _OperatorInfo.AuthorityId = Convert.ToDecimal(this.cmbAuthority.SelectedValue);
            _OperatorInfo.LoginYMD = Convert.ToDateTime(this.dteLoginYMD.Value);
            _OperatorInfo.AdminKbn = NSKUtil.BoolToInt(this.chkAdminKbn.Checked);
            _OperatorInfo.DisableFlag = this.chkDisableFlag.Checked;
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

            //オペレーター名の必須入力チェック
            if (rt_val && (this.edtOperatorName.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "ユーザー名" });
                ctl = this.edtOperatorName;
            }

            //ログインIDの必須入力チェック
            if (rt_val && (this.edtOperatorCode.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "ログインID" });
                ctl = this.edtOperatorCode;
            }

            //パスワードと確認パスワードの一致チェック
            if (rt_val && this.edtPassword.Text.Trim() != this.edtConfirmPassword.Text.Trim())
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203006");
                ctl = this.edtConfirmPassword;
            }

            //権限の必須入力チェック
            if (rt_val && this.cmbAuthority.SelectedValue == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203012", new string[] { "メニュー制御権限" });
                ctl = this.cmbAuthority;
            }

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
                                this._Operator.Delete(tx, _OperatorInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "利用者",_OperatorInfo.OperatorCode });

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
                            this.DoClear(false);
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
        private void DoClear(bool showCancelConfirm)
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

            //オペレータリストをセット
            this.SetOperatorListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpOperatorListGrid.Focus();
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
                case Keys.F5:
                    // F5は共通検索画面
                    this.ShowCmnSearch();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// フォーカスしているコントロールに対応する共通検索画面を表示します。
        /// </summary>
        private void ShowCmnSearch()
        {
            if (SearchFunctions.ContainsKey(ActiveControl))
            {
                SearchFunctions[ActiveControl]();
            }
        }

        /// <summary>
        /// 社員検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchStaff()
        {
            using (CmnSearchStaffFrame f = new CmnSearchStaffFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    ////画面から値を取得
                    //((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                    //    Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

                    //this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCode(CancelEventArgs e)
        {
            bool is_clear = false;

            //try
            //{
            //    //コードの入力が無い場合は抜ける
            //    if (this.StaffCode == 0)
            //    {
            //        is_clear = true;
            //        return;
            //    }

            //    StaffInfo info =
            //        this._DalUtil.Staff.GetInfo(this.StaffCode);

            //    if (info == null)
            //    {
            //        //データなし
            //        MessageBox.Show(
            //            FrameUtilites.GetDefineMessage("MW2201003"),
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        is_clear = true;
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        if (info.ToraDONDisableFlag)
            //        {
            //            MessageBox.Show(
            //                FrameUtilites.GetDefineMessage("MW2201016"),
            //                this.Text,
            //                MessageBoxButtons.OK,
            //                MessageBoxIcon.Warning);

            //            is_clear = true;
            //            e.Cancel = true;
            //        }
            //        else
            //        {
            //            this.numStaffCode.Tag = info.ToraDONStaffId;
            //            this.numStaffCode.Value = info.ToraDONStaffCode;
            //            this.edtStaffName.Text = info.ToraDONStaffName;
            //        }
            //    }
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        this.numStaffCode.Tag = null;
            //        this.numStaffCode.Value = null;
            //        this.edtStaffName.Text = string.Empty;
            //    }
            //}
       
        }

        #endregion

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.currentMode == FrameEditMode.Editable ||
                   this.currentMode == FrameEditMode.New)
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
        private void ProcessFpOperatorListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.DoGetData();
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

        private void OperatorFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void fpOperatorListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //オペレーターリスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetData();
            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            //新規作成ボタンクリック
            this.DoStartNewData();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            //編集ボタン
            this.DoGetData();
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpOperatorListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpOperatorListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpOperatorListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpOperatorListGridPreviewKeyDown(e);
        }

        private void OperatorFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetOperatorListSheet();
        }

        private void numStaffCode_Validating(object sender, CancelEventArgs e)
        {
            //社員コード
            this.ValidateStaffCode(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void btnInitLoginYMD_Click(object sender, EventArgs e)
        {
            this.dteLoginYMD.Value = null;
            this.btnInitLoginYMD.Enabled = false;
        }

        private void fpOperatorListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }
    }
}
