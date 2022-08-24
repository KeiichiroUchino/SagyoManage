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
using System.Globalization;


namespace Jpsys.SagyoManage.Frame
{
    public partial class MeishoFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public MeishoFrame()
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

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "名称登録";

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

        /// <summary>
        /// 名称クラス
        /// </summary>
        private Meisho _Meisho;

        /// <summary>
        /// 現在編集中の名称情報を保持する領域
        /// </summary>
        private MeishoInfo _MeishoInfo = null;

        /// <summary>
        /// 名称IDを保持する領域
        /// </summary>
        private decimal _MeishoId = 0;

        /// <summary>
        /// カナ取得を行うためのコントロールを保持する領域
        /// </summary>
        private GrapeCity.Win.Editors.GcIme ime;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region 名称一覧

        //--Spread列定義

        /// <summary>
        /// 名称コード列番号
        /// </summary>
        private const int COL_MEISHO_CODE = 0;

        /// <summary>
        /// 名称列番号
        /// </summary>
        private const int COL_MEISHO_NAME = 1;

        /// <summary>
        /// 名称リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_MEISHOLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialMeishoStyleInfoArr;

        /// <summary>
        /// 名称情報のリストを保持する領域
        /// </summary>
        private IList<MeishoInfo> _MeishoInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx_Result = 0;

        /// <summary>
        /// 新規で登録されたコードを保持する領域
        /// </summary>
        private string newcode = string.Empty;

        /// <summary>
        /// 名称区分コンボ作成用の名称区分リスト
        /// </summary>
        private IList<string[]> _NameKbnList = null;

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitMeishoFrame()
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
            this.InitMenuItem();

            //バインドの設定
            this.SettingCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //名称クラスインスタンス作成
            this._Meisho = new Meisho(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //コンボボックスのデータを取得
            this.GetComboList();

            //コンボボックスの初期化
            this.InitCombo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //名称リストのセット
            this.SetMeishoListSheet();

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
            //this.actionMenuItems.SetCreatingItem(ActionMenuItems.ChangeCode);
            //this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
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
        /// コンボボックスで使用する一覧を取得します。
        /// </summary>
        private void GetComboList()
        {
            this.GetNameKbnComboList();
        }

        /// <summary>
        /// 名称区分コンボ用リストを取得してメンバに設定します。
        /// </summary>
        private void GetNameKbnComboList()
        {
            int kbn = 0;
            string name = string.Empty;

            this._NameKbnList = new List<string[]>();

            foreach (BizProperty.DefaultProperty.MeishoKbn item in
                Enum.GetValues(typeof(BizProperty.DefaultProperty.MeishoKbn)))
            {
                kbn = (int)item;
                name = DefaultProperty.GetMeishoKbnString(item);

                this._NameKbnList.Add(new string[] { kbn.ToString(), name });
            }

        }

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitNameKbnCombo();
        }

        /// <summary>
        /// 名称区分コンボを初期化します。
        /// </summary>
        private void InitNameKbnCombo()
        {
            List<NSKComboItem> wk_list = new List<NSKComboItem>();

            foreach (string[] item in this._NameKbnList)
            {
                wk_list.Add(new NSKComboItem(item[1], item[0].ToString()));
            }

            //コンボコントロールを作成
            this.cmbNameKbn.CreateControl();

            this.cmbNameKbn.TextBoxStyle = GrapeCity.Win.Editors.TextBoxStyle.TextOnly;
            this.cmbNameKbn.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbNameKbn.ListDefaultColumn.AutoWidth = true;
            this.cmbNameKbn.ListGridLines.HorizontalLines.Style = GrapeCity.Win.Editors.LineStyle.None;
            this.cmbNameKbn.ListGridLines.VerticalLines.Style = GrapeCity.Win.Editors.LineStyle.None;

            this.cmbNameKbn.DataSource = wk_list;

            this.cmbNameKbn.ListColumns[0].DataPropertyName = NSKComboItem.DisplayMemberString;
            this.cmbNameKbn.ListColumns[1].DataPropertyName = NSKComboItem.ValueMemberString;

            this.cmbNameKbn.TextSubItemIndex = 0;
            this.cmbNameKbn.ValueSubItemIndex = 1;

            //ドロップダウンしたときのリストには表示用の値のみ表示する
            this.cmbNameKbn.ListColumns[0].Visible = true;
            this.cmbNameKbn.ListColumns[1].Visible = false;

            this.cmbNameKbn.ListHeaderPane.Visible = false;
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //名称リストの初期化
            this.InitMeishoListSheet();

            //件数表示
            this.lblKensu.Text = "0件";
        }

        /// <summary>
        /// 名称リストを初期化します。
        /// </summary>
        private void InitMeishoListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMeishoListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_MEISHOLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.cmbNameKbn.TextSubItemIndex = 0;
            this.cmbNameKbn.ValueSubItemIndex = 1;
            this.chkAllFlag.Checked = false;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.numMeishoCode.Value = 0;
            this.edtMeishoName.Text = string.Empty;
            this.edtMeishoNameKana.Text = string.Empty;

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

            //カナ取得用のImeコントロールの初期化
            this.InitGrapeCityIme();

            //カナは無変換にする
            FrameUtilites.SetImeSentenceModeForInputMan(this.edtMeishoNameKana);
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSpreadStyleInfo()
        {
            //名称スタイル情報初期化
            this.InitMeishoStyleInfo();
        }

        /// <summary>
        /// 名称のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitMeishoStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpMeishoListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialMeishoStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_MEISHOLIST];

            for (int i = 0; i < COL_MAXCOLNUM_MEISHOLIST; i++)
            {
                this.initialMeishoStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// カナ取得用のImeコントロールを初期化します。
        /// </summary>
        private void InitGrapeCityIme()
        {
            this.InitKanaGrapeCityIme();
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
        }

        /// <summary>
        /// カナ用のかな文字取得に使用するImeコントロールを初期化します。
        /// </summary>
        private void InitKanaGrapeCityIme()
        {
            //インスタンス作成
            this.ime = new GcIme();
            //カナを取得するコントロールとイベント発生を指示
            this.ime.SetCausesImeEvent(this.edtMeishoName, true);
            this.ime.SetInputScope(this.edtMeishoNameKana, GrapeCity.Win.Editors.InputScopeNameValue.Default);
            //イベントハンドラを作成
            this.ime.ResultString +=
                new EventHandler<GrapeCity.Win.Editors.ResultStringEventArgs>(ime_ResultString);
        }

        private void ime_ResultString(object sender, GrapeCity.Win.Editors.ResultStringEventArgs e)
        {
            //名を入力したときにカナを自動セットする
            this.edtMeishoNameKana.Text += e.ReadString;
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddStateObject(this.toolStripNew);
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
                _commandSet.New, this.btnNew, actionMenuItems.GetMenuItemBy(ActionMenuItems.New), this.toolStripNew);

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
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);

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

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitMeishoFrame();
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
                    this.numMeishoCode.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.New.Enabled = true;
                    _commandSet.EditCancel.Enabled = false;
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
                    this.numMeishoCode.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.New.Enabled = false;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.numMeishoCode.Enabled = false;
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
        /// 名称リストに値を設定します。
        /// </summary>
        private void SetMeishoListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._MeishoInfoList =
                    this._Meisho.GetList();

                IList<MeishoInfo> list = this.GetMatchedList(this._MeishoInfoList);

                //使用停止表示フラグによってデータ再抽出
                IList<MeishoInfo> wk_list =
                    this.chkAllFlag.Checked ? list : list.Where(x => !x.DisableFlag).ToList();

                //件数取得
                int rowCount = wk_list.Count;

                //件数表示
                this.lblKensu.Text = rowCount.ToString() + "件";

                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputs();

                    //0件の場合は何もしない
                    return;
                }

                //抽出後のリストを取得
                wk_list = wk_list
                    .OrderBy(element => element.MeishoCode)
                    .ToList();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_MEISHOLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_MEISHOLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    MeishoInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_MEISHOLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialMeishoStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_MEISHO_CODE, wk_info.MeishoCode);
                    datamodel.SetValue(j, COL_MEISHO_NAME, wk_info.Meisho);

                    datamodel.SetTag(j, COL_MEISHO_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpMeishoListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpMeishoListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpMeishoListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpMeishoListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpMeishoListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpMeishoListGrid.Sheets[0].ColumnCount - 1);

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
        private IList<MeishoInfo> GetMatchedList(IList<MeishoInfo> list)
        {
            int namekbn = 0;

            //コンボボックスのvalueがnullのときは"0"を入れる
            if (cmbNameKbn.SelectedValue != null)
            {
                namekbn = Convert.ToInt32(this.cmbNameKbn.SelectedValue);
            }

            MeishoSearchParameter para = new MeishoSearchParameter
            {
                MeishoKbn = namekbn
            };

            //名称リストを取得
            return _Meisho.GetList(para);
        }

        /// <summary>
        /// 名称の新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //名称区分が選択されているかチェック
            if (Convert.ToInt32(this.cmbNameKbn.SelectedValue) == 0)
            {
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                        "MW2203012", new string[] { "区分" }),
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.cmbNameKbn.Focus();
                return;
            }

            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の名称情報を保持する領域の初期化
            this._MeishoInfo = new MeishoInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.numMeishoCode.Focus();
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //名称区分が選択されているかチェック
            if (Convert.ToInt32(this.cmbNameKbn.SelectedValue) == 0)
            {
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                        "MW2203012", new string[] { "区分" }),
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                this.cmbNameKbn.Focus();
                return;
            }

            //名称コードをリストから取得して設定
            this._MeishoId = this.GetMeishoIdByMeishoListOnSelection();

            if (this._MeishoId == 0)
            {
                this._MeishoInfo = new MeishoInfo();

                this.ChangeMode(FrameEditMode.New);
                this.numMeishoCode.Focus();
            }
            else
            {
                try
                {
                    //名称情報取得
                    this._MeishoInfo =
                        this._Meisho.GetInfoById(_MeishoId);

                    //画面設定
                    this.SetScreen();

                    if (changeMode)
                    {
                        //モード設定
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        this.edtMeishoName.Focus();
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
        /// 名称リストにて選択中の名称の名称コードを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の名称ID</returns>
        private decimal GetMeishoIdByMeishoListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpMeishoListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからMeishoInfoを取り出して、MeishoIdを取得
                    rt_val = ((MeishoInfo)sheet0.Cells[select_row, COL_MEISHO_CODE].Tag).MeishoId;
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
            if (this._MeishoInfo ==null)
            {
                this.ClearInputs();
            }
            else
            {
                this.edtMeishoName.Text = _MeishoInfo.Meisho;
                this.numMeishoCode.Value = Convert.ToDecimal(_MeishoInfo.MeishoCode);
                this.edtMeishoName.Text = _MeishoInfo.Meisho;
                this.edtMeishoNameKana.Text = _MeishoInfo.MeishoKana;

                this.chkDisableFlag.Checked = _MeishoInfo.DisableFlag;
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
                        this._Meisho.Save(tx, this._MeishoInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "名称", _MeishoInfo.MeishoCode.ToString() });

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
                        this.numMeishoCode.Focus();
                    }
                    else
                    {
                        this.edtMeishoName.Focus();
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
                        this.numMeishoCode.Focus();
                    }
                    else
                    {
                        this.edtMeishoName.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _MeishoInfo.MeishoKbn = Convert.ToInt32(this.cmbNameKbn.SelectedValue);

            _MeishoInfo.MeishoCode = Convert.ToInt32(this.numMeishoCode.Value);
            _MeishoInfo.Meisho = this.edtMeishoName.Text.Trim();
            _MeishoInfo.MeishoKana = this.edtMeishoNameKana.Text.Trim();

            _MeishoInfo.DisableFlag = this.chkDisableFlag.Checked;
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

            //コードの必須入力チェック
            if (rt_val && Convert.ToDecimal(this.numMeishoCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numMeishoCode;
            }

            //名称の必須入力チェック
            if (rt_val && (this.edtMeishoName.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "名称" });
                ctl = this.edtMeishoName;
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
                                this._Meisho.Delete(tx, _MeishoInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "名称",_MeishoInfo.MeishoCode.ToString() });

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

            //名称リストをセット
            this.SetMeishoListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpMeishoListGrid.Focus();
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
        private void ProcessFpMeishoListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void MeishoFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void MeishoFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpMeishoListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //名称リスト - CellDoubleClick
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダの場合は実行しない。
                this.DoGetData();
            }
        }

        /// <summary>
        /// 登録済リストのクリックイベントです
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpMeishoListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpMeishoListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpMeishoListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpMeishoListGridPreviewKeyDown(e);
        }

        private void fpMeishoListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void cmbNameKbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetMeishoListSheet();
        }

        private void chkAllFlag_CheckedChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetMeishoListSheet();
        }
    }
}
