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
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Frame;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.InputMan;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using System.Globalization;
using System.Text.RegularExpressions;


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HanroFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HanroFrame()
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
        private const string WINDOW_TITLE = "販路登録";

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
        /// 販路クラス
        /// </summary>
        private Hanro _Hanro;

        /// <summary>
        /// 現在編集中の販路情報を保持する領域
        /// </summary>
        private HanroInfo _HanroInfo = null;

        /// <summary>
        /// 販路情報パラメータを保持する領域
        /// </summary>
        private HanroInfo _ParamHanroInfo = null;

        /// <summary>
        /// 販路IDを保持する領域
        /// </summary>
        private decimal _HanroId = 0;

        /// <summary>
        /// カナ取得を行うためのコントロールを保持する領域
        /// </summary>
        private GrapeCity.Win.Editors.GcIme ime;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// 初期化中を判定するフラグ
        /// (true:初期化中)
        /// </summary>
        private bool isInitializing = true;

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

        #region 販路一覧

        //--Spread列定義

        /// <summary>
        /// 販路コード列番号
        /// </summary>
        private const int COL_HANRO_CODE = 0;

        /// <summary>
        /// 販路列番号
        /// </summary>
        private const int COL_HANRO_NAME = 1;

        /// <summary>
        /// 販路カナ列番号
        /// </summary>
        private const int COL_HANRO_KANA = 2;

        /// <summary>
        /// 得意先コード列番号
        /// </summary>
        private const int COL_TOKUISAKI_CODE = 3;

        /// <summary>
        /// 得意先列番号
        /// </summary>
        private const int COL_TOKUISAKI_NAME = 4;

        /// <summary>
        /// 積地コード列番号
        /// </summary>
        private const int COL_HATCHI_CODE = 5;

        /// <summary>
        /// 積地列番号
        /// </summary>
        private const int COL_HATCHI_NAME = 6;

        /// <summary>
        /// 着地コード列番号
        /// </summary>
        private const int COL_CHAKUCHI_CODE = 7;

        /// <summary>
        /// 着地列番号
        /// </summary>
        private const int COL_CHAKUCHI_NAME = 8;

        /// <summary>
        /// 販路リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_HANROLIST = 9;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialHanroStyleInfoArr;

        /// <summary>
        /// 販路情報のリストを保持する領域
        /// </summary>
        private IList<HanroInfo> _HanroInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx_Result = 0;

        /// <summary>
        /// 新規で登録されたコードを保持する領域
        /// </summary>
        private string newcode = string.Empty;

        #endregion

        #region 項目初期値

        /// <summary>
        /// 配給開始時間初期値
        /// </summary>
        private const String STARTHMS_DEFAULT_VALUE = "00:00";

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitHanroFrame()
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

            //販路クラスインスタンス作成
            this._Hanro = new Hanro(this.appAuth);

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numSearchTokuisakiCode, this.ShowCmnSearchTokuisaki},
                {this.numTokuisakiCode, this.ShowCmnSearchTokuisaki},
                {this.numHatchiCode, this.ShowCmnSearchPoint},
                {this.numChakuchiCode, this.ShowCmnSearchPoint},
                {this.numItemCode, this.ShowCmnSearchItem},
                {this.numCarKindCode, this.ShowCmnSearchCarKind},
                {this.numCarCode, this.ShowCmnSearchCar},
                {this.numTorihikiCode, this.ShowCmnSearchTorihiki},
            };

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //請求単価の桁数を受注テーブルの単価の桁数に合わせる
            this.numSeikyuTanka.Fields.DecimalPart.MaxDigits = UserProperty.GetInstance().JuchuAtPriceDecimalDigits;
            this.numSeikyuTanka.Fields.IntegerPart.MaxDigits = UserProperty.GetInstance().JuchuAtPriceIntDigits;

            //コンボボックスの初期化
            this.InitCombo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //販路リストのセット
            this.SetHanroListSheet();

            //販路情報パラメータが設定されている場合
            if (this._ParamHanroInfo != null)
            {
                //新規登録モード
                this.DoStartNewData();

                //画面設定
                this.SetScreen(this._ParamHanroInfo);

                //登録済みコードで最小値コードを取得
                this.numHanroCode.Value = this._Hanro.GetNextCode();
            }
            else
            {
                //現在の画面モードを初期状態に変更
                this.ChangeMode(FrameEditMode.Default);
            }

            //初期化終了
            this.isInitializing = false;
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

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            //往復区分コンボボックス
            this.InitOfukuKbnCombo();
        }

        /// <summary>
        /// 往復区分コンボボックスを初期化します。
        /// </summary>
        private void InitOfukuKbnCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            foreach (SystemNameInfo item in this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.OfukuKbn))
                .ToList())
            {
                datasource.Add(item.SystemNameCode.ToString(), item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbOfukuKbn, datasource, true, null, false);
        }

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //販路リストの初期化
            this.InitHanroListSheet();
        }

        /// <summary>
        /// 販路リストを初期化します。
        /// </summary>
        private void InitHanroListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHanroListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_HANROLIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.numSearchTokuisakiCode.Tag = null;
            this.numSearchTokuisakiCode.Value = null;
            this.edtSearchTokuisakiName.Text = string.Empty;
            this.edtSearch.Text = string.Empty;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.numHanroCode.Value = 0;
            this.edtHanroName.Text = string.Empty;
            this.edtHanroNameKana.Text = string.Empty;
            this.edtHanroSName.Text = string.Empty;
            this.edtHanroSSName.Text = string.Empty;
            this.numTokuisakiCode.Tag = null;
            this.numTokuisakiCode.Value = null;
            this.edtTokuisakiName.Text = string.Empty;
            this.numHatchiCode.Tag = null;
            this.numHatchiCode.Value = null;
            this.edtHatchiName.Text = string.Empty;
            this.numChakuchiCode.Tag = null;
            this.numChakuchiCode.Value = null;
            this.edtChakuchiName.Text = string.Empty;
            this.numItemCode.Tag = null;
            this.numItemCode.Value = null;
            this.edtItemName.Text = string.Empty;
            this.numCarKindCode.Tag = null;
            this.numCarKindCode.Value = null;
            this.edtCarKindName.Text = string.Empty;
            this.numCarCode.Tag = null;
            this.numCarCode.Value = null;
            this.edtCarName.Text = string.Empty;
            this.numTorihikiCode.Tag = null;
            this.numTorihikiCode.Value = null;
            this.numTorihikiCode.ReadOnly = true;
            this.sbtnTorihikiCode.Enabled = false;
            this.edtTorihikiName.Text = string.Empty;
            this.cmbOfukuKbn.SelectedValue = null;
            this.numKoteiNissu.Value = null;
            this.edtKoteiJikan.Text = STARTHMS_DEFAULT_VALUE;
            this.numReuseNissu.Value = null;
            this.edtReuseJikan.Text = STARTHMS_DEFAULT_VALUE;
            this.numSeikyuTanka.Value = null;
            this.numYoshaKingaku.Value = null;
            this.numYoshaKingaku.ReadOnly = true;
            this.numFutaigyomuryo.Value = null;

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
            FrameUtilites.SetImeSentenceModeForInputMan(this.edtHanroNameKana);
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSpreadStyleInfo()
        {
            //販路スタイル情報初期化
            this.InitHanroStyleInfo();
        }

        /// <summary>
        /// 販路のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitHanroStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHanroListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialHanroStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_HANROLIST];

            for (int i = 0; i < COL_MAXCOLNUM_HANROLIST; i++)
            {
                this.initialHanroStyleInfoArr[i] =
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
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchTokuisakiCode, ctl => ctl.Text, this.numSearchTokuisakiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTokuisakiCode, ctl => ctl.Text, this.numTokuisakiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHatchiCode, ctl => ctl.Text, this.numHatchiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numChakuchiCode, ctl => ctl.Text, this.numChakuchiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numItemCode, ctl => ctl.Text, this.numItemCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCode, ctl => ctl.Text, this.numCarKindCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCode, ctl => ctl.Text, this.numCarCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTorihikiCode, ctl => ctl.Text, this.numTorihikiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtKoteiJikan, ctl => ctl.Text, this.edtHMS_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtReuseJikan, ctl => ctl.Text, this.edtHMS_Validating));
        }

        /// <summary>
        /// カナ用のかな文字取得に使用するImeコントロールを初期化します。
        /// </summary>
        private void InitKanaGrapeCityIme()
        {
            //インスタンス作成
            this.ime = new GcIme();
            //カナを取得するコントロールとイベント発生を指示
            this.ime.SetCausesImeEvent(this.edtHanroName, true);
            this.ime.SetInputScope(this.edtHanroName, GrapeCity.Win.Editors.InputScopeNameValue.Default);
            //イベントハンドラを作成
            this.ime.ResultString +=
                new EventHandler<GrapeCity.Win.Editors.ResultStringEventArgs>(ime_ResultString);
        }

        private void ime_ResultString(object sender, GrapeCity.Win.Editors.ResultStringEventArgs e)
        {
            //名を入力したときにカナを自動セットする
            this.edtHanroNameKana.Text += e.ReadString;
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
                this.numSearchTokuisakiCode,
                this.numTokuisakiCode,
                this.numHatchiCode,
                this.numChakuchiCode,
                this.numItemCode,
                this.numCarKindCode,
                this.numCarCode
                );
            this.searchStateBinder.AddStateObject(this.toolStripSearch);
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

        void ChangeCode_Execute(object sender, EventArgs e)
        {
            this.DoChangeCode();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 販路情報パラメータを取得・設定します。
        /// </summary>
        public HanroInfo ParamHanroInfo
        {
            get { return this._ParamHanroInfo; }
            set { this._ParamHanroInfo = value; }
        }

        /// <summary>
        /// 検索用得意先コードの値を取得します。
        /// </summary>
        private int SearchTokuisakiCode
        {
            get { return Convert.ToInt32(this.numSearchTokuisakiCode.Value); }
        }

        /// <summary>
        /// 得意先コードの値を取得します。
        /// </summary>
        private int TokuisakiCode
        {
            get { return Convert.ToInt32(this.numTokuisakiCode.Value); }
        }

        /// <summary>
        /// 積地コードの値を取得します。
        /// </summary>
        private int HatchiCode
        {
            get { return Convert.ToInt32(this.numHatchiCode.Value); }
        }

        /// <summary>
        /// 着地コードの値を取得します。
        /// </summary>
        private int ChakuchiCode
        {
            get { return Convert.ToInt32(this.numChakuchiCode.Value); }
        }

        /// <summary>
        /// 品目コードの値を取得します。
        /// </summary>
        private int ItemCode
        {
            get { return Convert.ToInt32(this.numItemCode.Value); }
        }

        /// <summary>
        /// 車種コードの値を取得します。
        /// </summary>
        private int CarKindCode
        {
            get { return Convert.ToInt32(this.numCarKindCode.Value); }
        }

        /// <summary>
        /// 車両コードの値を取得します。
        /// </summary>
        private int CarCode
        {
            get { return Convert.ToInt32(this.numCarCode.Value); }
        }

        /// <summary>
        /// 車種コードの値を取得します。
        /// </summary>
        private int TorihikiCode
        {
            get { return Convert.ToInt32(this.numTorihikiCode.Value); }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHanroFrame();
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
                    this.numHanroCode.Enabled = false;
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
                    this.numHanroCode.Enabled = true;
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
                    this.numHanroCode.Enabled = false;
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
        /// 販路リストに値を設定します。
        /// </summary>
        private void SetHanroListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._HanroInfoList =
                    this._Hanro.GetList();

                IList<HanroInfo> wk_list = this.GetMatchedList(this._HanroInfoList);

                //件数取得
                int rowCount = wk_list.Count;
                if (rowCount == 0)
                {
                    //入力項目をクリア
                    this.ClearInputs();

                    //0件の場合は何もしない
                    return;
                }

                //抽出後のリストを取得
                wk_list = wk_list
                    .OrderBy(element => element.HanroCode)
                    .ToList();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_HANROLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_HANROLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    HanroInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_HANROLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialHanroStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_HANRO_CODE, wk_info.HanroCode);
                    datamodel.SetValue(j, COL_HANRO_NAME, wk_info.HanroName);
                    datamodel.SetValue(j, COL_HANRO_KANA, wk_info.HanroNameKana);
                    datamodel.SetValue(j, COL_TOKUISAKI_CODE, wk_info.ToraDONTokuisakiCode);
                    datamodel.SetValue(j, COL_TOKUISAKI_NAME, wk_info.ToraDONTokuisakiShortName);
                    datamodel.SetValue(j, COL_HATCHI_CODE, wk_info.ToraDONHatchiCode);
                    datamodel.SetValue(j, COL_HATCHI_NAME, wk_info.ToraDONHatchiName);
                    datamodel.SetValue(j, COL_CHAKUCHI_CODE, wk_info.ToraDONChakuchiCode);
                    datamodel.SetValue(j, COL_CHAKUCHI_NAME, wk_info.ToraDONChakuchiName);

                    datamodel.SetTag(j, COL_HANRO_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpHanroListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpHanroListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpHanroListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpHanroListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpHanroListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpHanroListGrid.Sheets[0].ColumnCount - 1);

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
        private List<HanroInfo> GetMatchedList(IList<HanroInfo> list)
        {
            List<HanroInfo> rt_list = new List<HanroInfo>();

            //得意先ID
            decimal tokuiskiId = Convert.ToDecimal(numSearchTokuisakiCode.Tag);

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

            foreach (HanroInfo item in list)
            {
                if (0 == tokuiskiId || item.ToraDONTokuisakiId == tokuiskiId)
                {
                    //「コード」+「名称」+「カナ」+「得意先」+「積地」+「着地」であいまい検索
                    string item_mei =
                        Convert.ToInt32(item.HanroCode) + Environment.NewLine
                        + item.HanroName.Trim() + Environment.NewLine
                        + item.HanroNameKana.Trim() + Environment.NewLine
                        + item.ToraDONTokuisakiShortName.Trim() + Environment.NewLine
                        + item.ToraDONHatchiName.Trim() + Environment.NewLine
                        + item.ToraDONChakuchiName.Trim() + Environment.NewLine;

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
            }

            return rt_list;
        }

        /// <summary>
        /// 販路の新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の販路情報を保持する領域の初期化
            this._HanroInfo = new HanroInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.numHanroCode.Focus();
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //販路コードをリストから取得して設定
            this._HanroId = this.GetHanroIdByHanroListOnSelection();

            if (this._HanroId == 0)
            {
                this._HanroInfo = new HanroInfo();

                this.ChangeMode(FrameEditMode.New);
                this.numHanroCode.Focus();
            }
            else
            {
                try
                {
                    //販路情報取得
                    this._HanroInfo =
                        this._Hanro.GetInfoById(_HanroId);

                    //画面設定
                    this.SetScreen(this._HanroInfo);

                    if (changeMode)
                    {
                        //モード設定
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        this.edtHanroName.Focus();
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
        /// 販路リストにて選択中の販路の販路コードを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の販路ID</returns>
        private decimal GetHanroIdByHanroListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpHanroListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからHanroInfoを取り出して、HanroIdを取得
                    rt_val = ((HanroInfo)sheet0.Cells[select_row, COL_HANRO_CODE].Tag).HanroId;
                }
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen(HanroInfo hanroInfo)
        {
            if (hanroInfo == null)
            {
                this.ClearInputs();
            }
            else
            {
                this.numHanroCode.Value = hanroInfo.HanroCode;
                this.edtHanroName.Text = hanroInfo.HanroName;
                this.edtHanroNameKana.Text = hanroInfo.HanroNameKana;
                this.edtHanroSName.Text = hanroInfo.HanroSName;
                this.edtHanroSSName.Text = hanroInfo.HanroSSName;
                this.numTokuisakiCode.Tag = hanroInfo.ToraDONTokuisakiId;
                this.numTokuisakiCode.Value = hanroInfo.ToraDONTokuisakiCode;
                this.edtTokuisakiName.Text = hanroInfo.ToraDONTokuisakiName;
                this.numHatchiCode.Tag = hanroInfo.ToraDONHatchiId;
                this.numHatchiCode.Value = hanroInfo.ToraDONHatchiCode;
                this.edtHatchiName.Text = hanroInfo.ToraDONHatchiName;
                this.numChakuchiCode.Tag = hanroInfo.ToraDONChakuchiId;
                this.numChakuchiCode.Value = hanroInfo.ToraDONChakuchiCode;
                this.edtChakuchiName.Text = hanroInfo.ToraDONChakuchiName;
                this.numItemCode.Tag = hanroInfo.ToraDONItemId;
                this.numItemCode.Value = hanroInfo.ToraDONItemCode;
                this.edtItemName.Text = hanroInfo.ToraDONItemName;
                this.numCarKindCode.Tag = hanroInfo.ToraDONCarKindId;
                this.numCarKindCode.Value = hanroInfo.ToraDONCarKindCode;
                this.edtCarKindName.Text = hanroInfo.ToraDONCarKindName;
                this.numCarCode.Tag = hanroInfo.ToraDONCarId;
                this.numCarCode.Value = hanroInfo.ToraDONCarCode;
                this.edtCarName.Text = hanroInfo.ToraDONCarName;
                this.numTorihikiCode.Tag = hanroInfo.ToraDONTorihikiId;
                this.numTorihikiCode.Value = hanroInfo.ToraDONTorihikiCode;
                this.numTorihikiCode.ReadOnly = hanroInfo.ToraDONCarKbn != (int)DefaultProperty.CarKbn.Yosha;
                this.sbtnTorihikiCode.Enabled = hanroInfo.ToraDONCarKbn == (int)DefaultProperty.CarKbn.Yosha;
                this.edtTorihikiName.Text = hanroInfo.ToraDONTorihikiName;
                this.cmbOfukuKbn.SelectedValue = hanroInfo.OfukuKbn.ToString();
                this.numKoteiNissu.Value = hanroInfo.KoteiNissu;
                this.edtKoteiJikan.Text = ProjectUtilites.IntToHMSDigit6(hanroInfo.KoteiJikan);
                this.numReuseNissu.Value = hanroInfo.ReuseNissu;
                this.edtReuseJikan.Text = ProjectUtilites.IntToHMSDigit6(hanroInfo.ReuseJikan);
                this.numSeikyuTanka.Value = hanroInfo.SeikyuTanka;
                this.numYoshaKingaku.Value = hanroInfo.YoshaKingaku;
                this.numYoshaKingaku.ReadOnly = hanroInfo.ToraDONCarKbn != (int)DefaultProperty.CarKbn.Yosha;
                this.numFutaigyomuryo.Value = hanroInfo.Futaigyomuryo;

                this.chkDisableFlag.Checked = hanroInfo.DisableFlag;

                if (this._ParamHanroInfo != null)
                {
                    this.ValidateTokuisakiCode(new System.ComponentModel.CancelEventArgs());
                    this.ValidateHatchiCode(new System.ComponentModel.CancelEventArgs());
                    this.ValidateChakuchiCode(new System.ComponentModel.CancelEventArgs());
                    this.ValidateItemCode(new System.ComponentModel.CancelEventArgs());
                    this.ValidateCarKindCode(new System.ComponentModel.CancelEventArgs());
                    this.numCarCode.Value = hanroInfo.ToraDONCarCode;
                    this.ValidateCarCode(new System.ComponentModel.CancelEventArgs());
                    this.ValidateTorihikiCode(new System.ComponentModel.CancelEventArgs());
                }
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
                        this._Hanro.Save(tx, this._HanroInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "販路", _HanroInfo.HanroCode.ToString() });

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

                    if (this._ParamHanroInfo != null)
                    {
                        //画面を閉じる（確認メッセージなし）
                        this.isConfirmClose = false;
                        this.DoClose();
                    }
                    else
                    {
                        //初期状態へ移行
                        this.DoClear(false);
                    }
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //フォーカスを移動
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.numHanroCode.Focus();
                    }
                    else
                    {
                        this.edtHanroName.Focus();
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
                        this.numHanroCode.Focus();
                    }
                    else
                    {
                        this.edtHanroName.Focus();
                    }
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _HanroInfo.HanroCode = Convert.ToInt32(this.numHanroCode.Value);
            _HanroInfo.HanroName = this.edtHanroName.Text.Trim();
            _HanroInfo.HanroNameKana = this.edtHanroNameKana.Text.Trim();
            _HanroInfo.HanroSName = this.edtHanroSName.Text.Trim();
            _HanroInfo.HanroSSName = this.edtHanroSSName.Text.Trim();
            _HanroInfo.ToraDONTokuisakiId = Convert.ToDecimal(this.numTokuisakiCode.Tag);
            _HanroInfo.ToraDONHatchiId = Convert.ToDecimal(this.numHatchiCode.Tag);
            _HanroInfo.ToraDONChakuchiId = Convert.ToDecimal(this.numChakuchiCode.Tag);
            _HanroInfo.ToraDONItemId = Convert.ToDecimal(this.numItemCode.Tag);
            _HanroInfo.ToraDONCarKindId = Convert.ToDecimal(this.numCarKindCode.Tag);
            _HanroInfo.ToraDONCarId = Convert.ToDecimal(this.numCarCode.Tag);
            _HanroInfo.ToraDONTorihikiId = Convert.ToDecimal(this.numTorihikiCode.Tag);
            _HanroInfo.OfukuKbn = Convert.ToInt32(this.cmbOfukuKbn.SelectedValue);
            _HanroInfo.KoteiNissu = Convert.ToInt32(this.numKoteiNissu.Value);
            _HanroInfo.KoteiJikan = ProjectUtilites.HMSToIntDigit6(this.edtKoteiJikan.Text.Trim());
            _HanroInfo.ReuseNissu = Convert.ToInt32(this.numReuseNissu.Value);
            _HanroInfo.ReuseJikan = ProjectUtilites.HMSToIntDigit6(this.edtReuseJikan.Text.Trim());
            _HanroInfo.SeikyuTanka = Convert.ToDecimal(this.numSeikyuTanka.Value);
            _HanroInfo.YoshaKingaku = Convert.ToDecimal(this.numYoshaKingaku.Value);
            _HanroInfo.Futaigyomuryo = Convert.ToDecimal(this.numFutaigyomuryo.Value);

            _HanroInfo.DisableFlag = this.chkDisableFlag.Checked;
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
            if (rt_val && Convert.ToDecimal(this.numHanroCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numHanroCode;
            }

            //名称の必須入力チェック
            if (rt_val && (this.edtHanroName.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "名称" });
                ctl = this.edtHanroName;
            }

            //得意先の必須入力チェック
            if (rt_val && Convert.ToDecimal(this.numTokuisakiCode.Tag) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "得意先" });
                ctl = this.numTokuisakiCode;
            }

            //行程時間のフォーマットチェック
            if (rt_val && !ProjectUtilites.IsHMS(this.edtKoteiJikan.Text))
            {
                rt_val = false;
                msg = "行程時間の入力が不正です。";
                ctl = this.edtKoteiJikan;
            }

            //再利用可能時間のフォーマットチェック
            if (rt_val && !ProjectUtilites.IsHMS(this.edtReuseJikan.Text))
            {
                rt_val = false;
                msg = "再利用可能時間の入力が不正です。";
                ctl = this.edtReuseJikan;
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
                                this._Hanro.Delete(tx, _HanroInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "販路",_HanroInfo.HanroCode.ToString() });

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

            //販路情報パラメータをクリア
            this._ParamHanroInfo = null;

            //販路リストをセット
            this.SetHanroListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpHanroListGrid.Focus();
        }

        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCode()
        {
            //コードを取得
            int oldcode = Convert.ToInt32(this.numHanroCode.Value);

            //コードの桁数を取得
            int codedigitcount = Convert.ToInt32(this.numHanroCode.MaxValue).ToString().Length;

            //コード変更画面を表示
            using (ChangeMasterCodeFrame f =
                new ChangeMasterCodeFrame(DefaultProperty.MasterCodeKbn.Hanro, oldcode, codedigitcount))
            {
                f.InitFrame();

                DialogResult wk_result = f.ShowDialog(this);

                //正常に処理されたら初期状態へ変更
                if (wk_result == DialogResult.OK)
                {
                    //画面初期化
                    this.DoClear(false);
                }
            }
        }

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
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONTokuisakiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 発着地検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchPoint()
        {
            using (CmnSearchPointFrame f = new CmnSearchPointFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONPointCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 品目検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchItem()
        {
            using (CmnSearchItemFrame f = new CmnSearchItemFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONItemCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 車種検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCarKind()
        {
            using (CmnSearchCarKindFrame f = new CmnSearchCarKindFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONCarKindCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 車両検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCar()
        {
            using (CmnSearchCarFrame f = new CmnSearchCarFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONCarCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 傭車先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTorihiki()
        {
            if (0 < this.CarCode)
            {
                CarInfo info =
                    this._DalUtil.Car.GetInfo(this.CarCode);

                if (info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                {
                    using (CmnSearchTorihikisakiFrame f = new CmnSearchTorihikisakiFrame())
                    {
                        // パラメータをセット
                        f.ShowYoshaFlag = true;
                        f.ShowAllFlag = false;

                        f.InitFrame();
                        f.ShowDialog(this);

                        if (f.DialogResult == DialogResult.OK)
                        {
                            // 画面から値を取得
                            ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                                Convert.ToInt64(f.SelectedInfo.ToraDONTorihikiCode);

                            this.OnCmnSearchComplete();
                        }
                    }
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 検索用得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchTokuisakiCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchTokuisakiCode == 0)
                {
                    is_clear = true;
                    return;
                }

                TokuisakiInfo info =
                    this._DalUtil.Tokuisaki.GetInfo(this.SearchTokuisakiCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.ToraDONDisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numSearchTokuisakiCode.Tag = info.ToraDONTokuisakiId;
                        this.numSearchTokuisakiCode.Value = info.ToraDONTokuisakiCode;
                        this.edtSearchTokuisakiName.Text = info.ToraDONTokuisakiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchTokuisakiCode.Tag = null;
                    this.numSearchTokuisakiCode.Value = null;
                    this.edtSearchTokuisakiName.Text = string.Empty;
                }

                if (!this.isInitializing)
                {
                    //選択中の行番号を初期化
                    selectrowidx_Result = 0;

                    //リスト再描画
                    this.SetHanroListSheet();
                }
            }
        }

        /// <summary>
        /// 得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.TokuisakiCode == 0)
                {
                    is_clear = true;
                    return;
                }

                TokuisakiInfo info =
                    this._DalUtil.Tokuisaki.GetInfo(this.TokuisakiCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.ToraDONDisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numTokuisakiCode.Tag = info.ToraDONTokuisakiId;
                        this.numTokuisakiCode.Value = info.ToraDONTokuisakiCode;
                        this.edtTokuisakiName.Text = info.ToraDONTokuisakiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTokuisakiCode.Tag = null;
                    this.numTokuisakiCode.Value = null;
                    this.edtTokuisakiName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 品目コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHatchiCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.HatchiCode == 0)
                {
                    is_clear = true;
                    return;
                }

                PointInfo info =
                    this._DalUtil.Point.GetInfo(this.HatchiCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numHatchiCode.Tag = info.ToraDONPointId;
                        this.numHatchiCode.Value = info.ToraDONPointCode;
                        this.edtHatchiName.Text = info.ToraDONPointName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numHatchiCode.Tag = null;
                    this.numHatchiCode.Value = null;
                    this.edtHatchiName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 着地コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateChakuchiCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.ChakuchiCode == 0)
                {
                    is_clear = true;
                    return;
                }

                PointInfo info =
                    this._DalUtil.Point.GetInfo(this.ChakuchiCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numChakuchiCode.Tag = info.ToraDONPointId;
                        this.numChakuchiCode.Value = info.ToraDONPointCode;
                        this.edtChakuchiName.Text = info.ToraDONPointName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numChakuchiCode.Tag = null;
                    this.numChakuchiCode.Value = null;
                    this.edtChakuchiName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 品目コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateItemCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.ItemCode == 0)
                {
                    is_clear = true;
                    return;
                }

                ItemInfo info =
                    this._DalUtil.Item.GetInfo(this.ItemCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numItemCode.Tag = info.ToraDONItemId;
                        this.numItemCode.Value = info.ToraDONItemCode;
                        this.edtItemName.Text = info.ToraDONItemName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numItemCode.Tag = null;
                    this.numItemCode.Value = null;
                    this.edtItemName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.CarKindCode == 0)
                {
                    is_clear = true;
                    return;
                }

                CarKindInfo info =
                    this._DalUtil.CarKind.GetInfo(this.CarKindCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.ToraDONDisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numCarKindCode.Tag = info.ToraDONCarKindId;
                        this.numCarKindCode.Value = info.ToraDONCarKindCode;
                        this.edtCarKindName.Text = info.ToraDONCarKindName;
                        this.numCarCode.Tag = null;
                        this.numCarCode.Value = null;
                        this.edtCarName.Text = string.Empty;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarKindCode.Tag = null;
                    this.numCarKindCode.Value = null;
                    this.edtCarKindName.Text = string.Empty;
                    this.numCarCode.Tag = null;
                    this.numCarCode.Value = null;
                    this.edtCarName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.CarCode == 0)
                {
                    is_clear = true;
                    return;
                }

                CarInfo info =
                    this._DalUtil.Car.GetInfo(this.CarCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numCarKindCode.Tag = info.CarKindId;
                        this.numCarKindCode.Value = info.CarKindCode;
                        this.edtCarKindName.Text = info.CarKindName;
                        this.numCarCode.Tag = info.ToraDONCarId;
                        this.numCarCode.Value = info.ToraDONCarCode;
                        this.edtCarName.Text = info.LicPlateCarNo;
                        if (info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                        {
                            this.numTorihikiCode.Tag = info.YoshasakiId;
                            this.numTorihikiCode.Value = info.YoshasakiCode;
                            this.numTorihikiCode.ReadOnly = false;
                            this.sbtnTorihikiCode.Enabled = true;
                            this.edtTorihikiName.Text = info.YoshasakiName;
                            this.numYoshaKingaku.ReadOnly = false;
                        }
                        else
                        {
                            this.numTorihikiCode.Tag = null;
                            this.numTorihikiCode.Value = null;
                            this.numTorihikiCode.ReadOnly = true;
                            this.sbtnTorihikiCode.Enabled = false;
                            this.edtTorihikiName.Text = string.Empty;
                            this.numYoshaKingaku.ReadOnly = true;
                        }
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarCode.Tag = null;
                    this.numCarCode.Value = null;
                    this.edtCarName.Text = string.Empty;
                    this.numTorihikiCode.Tag = null;
                    this.numTorihikiCode.Value = null;
                    this.numTorihikiCode.ReadOnly = true;
                    this.sbtnTorihikiCode.Enabled = false;
                    this.edtTorihikiName.Text = string.Empty;
                    this.numYoshaKingaku.ReadOnly = true;
                }
            }
        }

        /// <summary>
        /// 傭車先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTorihikiCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.TorihikiCode == 0)
                {
                    is_clear = true;
                    return;
                }

                TorihikisakiInfo info =
                    this._DalUtil.Torihikisaki.GetInfo(this.TorihikiCode);

                if (info == null)
                {
                    //データなし
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    is_clear = true;
                    e.Cancel = true;
                }
                else
                {
                    if (info.ToraDONDisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        is_clear = true;
                        e.Cancel = true;
                    }
                    else
                    {
                        this.numTorihikiCode.Tag = info.ToraDONTorihikiId;
                        this.numTorihikiCode.Value = info.ToraDONTorihikiCode;
                        this.edtTorihikiName.Text = info.ToraDONTorihikiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTorihikiCode.Tag = null;
                    this.numTorihikiCode.Value = null;
                    this.edtTorihikiName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 時間の値の検証を行います。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ValidateHMS(object sender, CancelEventArgs e)
        {
            int val = 0;

            try
            {
                //値取得
                Regex re = new Regex(@"[^0-9]");
                val = Convert.ToInt32(re.Replace(((GcTextBox)sender).Text, ""));

                //分取得
                int m = Convert.ToInt32(String.Format("{0:D4}", val).Substring(2, 2));

                if ((val < 0 || 2400 < val)
                    || (m < 0 || 59 < m))
                {
                    //データなし
                    MessageBox.Show(
                        "時刻の入力が不正です。",
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    e.Cancel = true;
                }
            }
            catch
            {
            }
            finally
            {
                ((GcTextBox)sender).Text = ProjectUtilites.IntToHMSDigit4(val);
            }
        }

        #endregion

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
        private void ProcessFpHanroListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void HanroFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void HanroFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpHanroListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //販路リスト - CellDoubleClick
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
        private void fpHanroListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpHanroListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpHanroListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpHanroListGridPreviewKeyDown(e);
        }

        private void HanroFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void Search_Changed(object sender, EventArgs e)
        {
            if (!this.isInitializing)
            {
                //選択中の行番号を初期化
                selectrowidx_Result = 0;

                //リスト再描画
                this.SetHanroListSheet();
            }
        }

        private void numSearchTokuisakiCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidateSearchTokuisakiCode(e);
        }

        private void numTokuisakiCode_Validating(object sender, CancelEventArgs e)
        {
            //得意先コード
            this.ValidateTokuisakiCode(e);
        }

        private void numHatchiCode_Validating(object sender, CancelEventArgs e)
        {
            //積地コード
            this.ValidateHatchiCode(e);
        }

        private void numChakuchiCode_Validating(object sender, CancelEventArgs e)
        {
            //着地コード
            this.ValidateChakuchiCode(e);
        }

        private void numItemCode_Validating(object sender, CancelEventArgs e)
        {
            //品目コード
            this.ValidateItemCode(e);
        }

        private void numCarKindCode_Validating(object sender, CancelEventArgs e)
        {
            //車種コード
            this.ValidateCarKindCode(e);
        }

        private void numCarCode_Validating(object sender, CancelEventArgs e)
        {
            //車両コード
            this.ValidateCarCode(e);
        }

        private void numTorihikiCode_Validating(object sender, CancelEventArgs e)
        {
            //傭車先コード
            this.ValidateTorihikiCode(e);
        }

        private void edtHMS_Validating(object sender, CancelEventArgs e)
        {
            //時間
            this.ValidateHMS(sender, e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void fpHanroListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void hms_Leave(object sender, EventArgs e)
        {
            FrameUtilites.HmsLeave(sender, e);
        }

        private void hms_Enter(object sender, EventArgs e)
        {
            FrameUtilites.HmsEnter(sender, e);
        }
    }
}
