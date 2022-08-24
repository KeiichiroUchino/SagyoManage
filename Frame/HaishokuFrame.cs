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


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishokuFrame()
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
        private const string WINDOW_TITLE = "配色登録";

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
        /// 配色クラス
        /// </summary>
        private Haishoku _Haishoku;

        /// <summary>
        /// 現在編集中の配色情報を保持する領域
        /// </summary>
        private HaishokuInfo _HaishokuInfo = null;

        /// <summary>
        /// 配色IDを保持する領域
        /// </summary>
        private decimal _HaishokuId = 0;

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

        #region 配色一覧

        //--Spread列定義

        /// <summary>
        /// 配色テーブルキー列番号
        /// </summary>
        private const int COL_HAISHOKU_TABLEKEY = 0;

        /// <summary>
        /// 配色コード列番号
        /// </summary>
        private const int COL_HAISHOKU_CODE = 1;

        /// <summary>
        /// 配色区分列番号
        /// </summary>
        private const int COL_HAISHOKU_KBN = 2;

        /// <summary>
        /// 配色区分値列番号
        /// </summary>
        private const int COL_HAISHOKU_KBN_VALUE = 3;

        /// <summary>
        /// 配色リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_HAISHOKULIST = 4;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialHaishokuStyleInfoArr;

        /// <summary>
        /// 配色情報のリストを保持する領域
        /// </summary>
        private IList<HaishokuInfo> _HaishokuInfoList = null;

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
        private void InitHaishokuFrame()
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

            //配色クラスインスタンス作成
            this._Haishoku = new Haishoku(this.appAuth);

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numTableCode, this.ShowCmnSearchTable},
                {this.numSystemKbn, this.ShowCmnSearchSystemName},
            };

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //コンボボックスの初期化
            this.InitCombo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //配色リストのセット
            this.SetHaishokuListSheet();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);

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
            //配色テーブル区分コンボボックス
            this.InitHaishokuTableKbnCombo();

            //システム名称コンボボックス
            this.InitSystemNameCombo();
        }

        /// <summary>
        /// 配色テーブル区分コンボボックスを初期化します。
        /// </summary>
        private void InitHaishokuTableKbnCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            foreach (SystemNameInfo item in this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishokuTableKbn))
                .ToList())
            {
                datasource.Add(item.SystemNameCode.ToString(), item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbKey, datasource, true, null, false);
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSearchKey, datasource, true, null, false);
        }

        /// <summary>
        /// システム名称コンボボックスを初期化します。
        /// </summary>
        private void InitSystemNameCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            foreach (SystemNameInfo item in this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.HaishokuSystemNameKbn))
                .ToList())
            {
                datasource.Add(item.SystemNameCode.ToString(), item.SystemName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSystemKbn, datasource, true, null, false);
        }

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //配色リストの初期化
            this.InitHaishokuListSheet();
        }

        /// <summary>
        /// 配色リストを初期化します。
        /// </summary>
        private void InitHaishokuListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHaishokuListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_HAISHOKULIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.edtSearch.Text = string.Empty;
            this.cmbSearchKey.SelectedValue = null;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.cmbKey.SelectedValue = null;
            this.numTableCode.Tag = null;
            this.numTableCode.Value = null;
            this.cmbSystemKbn.SelectedValue = null;
            this.numSystemKbn.Value = null;
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
            //配色スタイル情報初期化
            this.InitHaishokuStyleInfo();
        }

        /// <summary>
        /// 配色のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitHaishokuStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHaishokuListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialHaishokuStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_HAISHOKULIST];

            for (int i = 0; i < COL_MAXCOLNUM_HAISHOKULIST; i++)
            {
                this.initialHaishokuStyleInfoArr[i] =
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
                ControlValidatingEventRaiser.Create(this.numTableCode, ctl => ctl.Text, this.numTableCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSystemKbn, ctl => ctl.Text, this.numSystemKbn_Validating));
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
                this.numTableCode,
                this.numSystemKbn
                );
            this.searchStateBinder.AddStateObject(this.toolStripReference);
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

        #endregion

        #region プロパティ

        /// <summary>
        /// テーブルコードの値を取得します。
        /// </summary>
        private int TableCode
        {
            get { return Convert.ToInt32(this.numTableCode.Value); }
        }

        /// <summary>
        /// システム区分の値を取得します。
        /// </summary>
        private int SystemKbn
        {
            get { return Convert.ToInt32(this.numSystemKbn.Value); }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishokuFrame();
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
                    this.cmbKey.Enabled = false;
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = false;
                    this.numSystemKbn.Enabled = false;
                    this.edtSystemKbnName.Enabled = false;
                    //--ファンクションの使用可否
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
                    this.cmbKey.Enabled = true;
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = false;
                    this.numSystemKbn.Enabled = false;
                    this.edtSystemKbnName.Enabled = false;
                    //--ファンクションの使用可否
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
                    this.cmbKey.Enabled = false;
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = false;
                    this.numSystemKbn.Enabled = false;
                    this.edtSystemKbnName.Enabled = false;
                    //--ファンクションの使用可否
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
        /// 配色リストに値を設定します。
        /// </summary>
        private void SetHaishokuListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._HaishokuInfoList =
                    this._Haishoku.GetList();

                IList<HaishokuInfo> wk_list = this.GetMatchedList(this._HaishokuInfoList);

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
                    .OrderBy(element => element.TableKey)
                    .ThenBy(element => element.TableId)
                    .ThenBy(element => element.SystemKbn)
                    .ThenBy(element => element.SystemId)
                    .ToList();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_HAISHOKULIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_HAISHOKULIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    HaishokuInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_HAISHOKULIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialHaishokuStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_HAISHOKU_TABLEKEY, DefaultProperty.GetHaishokuTableKbnMeisho((DefaultProperty.HaishokuTableKbn)(wk_info.TableKey)));
                    datamodel.SetValue(j, COL_HAISHOKU_CODE, wk_info.TableCode == 0 ? string.Empty : wk_info.TableCode.ToString());
                    datamodel.SetValue(j, COL_HAISHOKU_KBN, FrameUtilites.GetSystemNameKbnString((DefaultProperty.SystemNameKbn)(wk_info.SystemKbn)));
                    datamodel.SetValue(j, COL_HAISHOKU_KBN_VALUE, wk_info.SystemId == 0 ? string.Empty : wk_info.SystemId.ToString());

                    datamodel.SetTag(j, COL_HAISHOKU_TABLEKEY, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpHaishokuListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpHaishokuListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpHaishokuListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpHaishokuListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpHaishokuListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpHaishokuListGrid.Sheets[0].ColumnCount - 1);

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
        private List<HaishokuInfo> GetMatchedList(IList<HaishokuInfo> list)
        {
            List<HaishokuInfo> rt_list = new List<HaishokuInfo>();

            //テーブルキー
            int tableKey = Convert.ToInt32(this.cmbSearchKey.SelectedValue);

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

            foreach (HaishokuInfo item in list)
            {
                if (tableKey == 0 || item.TableKey == tableKey)
                {
                    //「キー」+「コード」+「区分」+「区分値」であいまい検索
                    string item_mei = 
                        DefaultProperty.GetHaishokuTableKbnMeisho((DefaultProperty.HaishokuTableKbn)(item.TableKey)) + Environment.NewLine
                        + (item.TableCode == 0 ? string.Empty : item.TableCode.ToString()) + Environment.NewLine
                        + (FrameUtilites.GetSystemNameKbnString((DefaultProperty.SystemNameKbn)(item.SystemKbn))) + Environment.NewLine
                        + (item.SystemId == 0 ? string.Empty : item.SystemId.ToString()) + Environment.NewLine;

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
        /// 配色の新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の配色情報を保持する領域の初期化
            this._HaishokuInfo = new HaishokuInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.tabControl1.Focus();
            this.ActiveControl = this.cmbKey;
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //配色コードをリストから取得して設定
            this._HaishokuId = this.GetHaishokuIdByHaishokuListOnSelection();

            if (this._HaishokuId == 0)
            {
                this._HaishokuInfo = new HaishokuInfo();

                this.ChangeMode(FrameEditMode.New);
                this.tabControl1.Focus();
                this.ActiveControl = this.cmbKey;
            }
            else
            {
                try
                {
                    //配色情報取得
                    this._HaishokuInfo =
                        this._Haishoku.GetInfoById(_HaishokuId);

                    //画面設定
                    this.SetScreen();

                    if (changeMode)
                    {
                        //モード設定
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        this.tabControl1.Focus();
                        this.ActiveControl = this.clpForeColor;
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
        /// 配色リストにて選択中の配色の配色コードを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の配色ID</returns>
        private decimal GetHaishokuIdByHaishokuListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpHaishokuListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからHaishokuInfoを取り出して、HaishokuIdを取得
                    rt_val = ((HaishokuInfo)sheet0.Cells[select_row, COL_HAISHOKU_TABLEKEY].Tag).HaishokuId;
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
            if (this._HaishokuInfo ==null)
            {
                this.ClearInputs();
            }
            else
            {
                this.cmbKey.SelectedValue = this._HaishokuInfo.TableKey.ToString();
                this.numTableCode.Tag = this._HaishokuInfo.TableId;
                this.numTableCode.Value = this._HaishokuInfo.TableCode;
                this.edtTableName.Text = this._HaishokuInfo.TableName;
                this.cmbSystemKbn.SelectedValue = this._HaishokuInfo.SystemKbn.ToString();
                this.numSystemKbn.Value = this._HaishokuInfo.SystemId;
                this.edtSystemKbnName.Text = this._HaishokuInfo.SystemKbnName;
                this.clpForeColor.SelectedColor = ColorTranslator.FromOle(this._HaishokuInfo.ForeColor);
                this.clpBackColor.SelectedColor = ColorTranslator.FromOle(this._HaishokuInfo.BackColor);

                this.chkDisableFlag.Checked = _HaishokuInfo.DisableFlag;
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
                        this._Haishoku.Save(tx, this._HaishokuInfo);
                    });

                    ////操作ログ(保存)の条件取得
                    string log_jyoken = Createlog();

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
                    this.tabControl1.Focus();
                    //フォーカスを移動
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.ActiveControl = this.cmbKey;
                    }
                    else
                    {
                        this.ActiveControl = this.clpForeColor;
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
                    this.tabControl1.Focus();
                    //フォーカスを移動
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.ActiveControl = this.cmbKey;
                    }
                    else
                    {
                        this.ActiveControl = this.clpForeColor;
                    }
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _HaishokuInfo.TableKey = Convert.ToInt32(this.cmbKey.SelectedValue);
            _HaishokuInfo.TableId = Convert.ToDecimal(this.numTableCode.Tag);
            _HaishokuInfo.SystemKbn = Convert.ToInt32(this.cmbSystemKbn.SelectedValue);
            _HaishokuInfo.SystemId = Convert.ToInt32(this.numSystemKbn.Value);
            _HaishokuInfo.ForeColor = ColorTranslator.ToOle(this.clpForeColor.SelectedColor);
            _HaishokuInfo.BackColor = ColorTranslator.ToOle(this.clpBackColor.SelectedColor);

            _HaishokuInfo.DisableFlag = this.chkDisableFlag.Checked;
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

            //キー取得
            int key = Convert.ToInt32(this.cmbKey.SelectedValue);

            //キーの必須入力チェック
            if (rt_val && key == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "キー" });
                ctl = this.cmbKey;
            }

            switch ((DefaultProperty.HaishokuTableKbn)key)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    //区分の必須入力チェック
                    if (rt_val && Convert.ToInt32(this.cmbSystemKbn.SelectedValue) == 0)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                            "MW2203001", new string[] { "区分" });
                        ctl = this.cmbSystemKbn;
                    }
                    //区分値の必須入力チェック
                    if (rt_val && this.SystemKbn == 0)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                            "MW2203001", new string[] { "区分値" });
                        ctl = this.numSystemKbn;
                    }
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    //コードの必須入力チェック
                    if (rt_val && this.TableCode == 0)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                            "MW2203001", new string[] { "コード" });
                        ctl = this.numTableCode;
                    }
                    break;
                default:
                    break;
            }

            //文字色の必須入力チェック
            if (rt_val && this.clpForeColor.Text.Equals("Empty"))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "文字色" });
                ctl = this.clpForeColor;
            }

            //背景色の必須入力チェック
            if (rt_val && this.clpBackColor.Text.Equals("Empty"))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "背景色" });
                ctl = this.clpBackColor;
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
                                this._Haishoku.Delete(tx, _HaishokuInfo);
                            });

                            ////操作ログ(削除)の条件取得
                            string log_jyoken = Createlog();

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

            //配色リストをセット
            this.SetHaishokuListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpHaishokuListGrid.Focus();
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
        private void ShowCmnSearchTable()
        {
            switch ((DefaultProperty.HaishokuTableKbn)(Convert.ToInt32(this.cmbKey.SelectedValue)))
            {
                case DefaultProperty.HaishokuTableKbn.Car:
                    this.ShowCmnSearchCar();
                    break;
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    this.ShowCmnSearchTokuisaki();
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// システム区分検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchSystemName()
        {
            using (CmnSearchSystemNameFrame f = new CmnSearchSystemNameFrame((DefaultProperty.SystemNameKbn)(Convert.ToInt32(this.cmbSystemKbn.SelectedValue))))
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.SystemNameCode);

                    this.OnCmnSearchComplete();
                }
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

        #endregion

        #region 検証（Validate）処理

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
                if (this.TableCode == 0)
                {
                    is_clear = true;
                    return;
                }

                CarInfo info =
                    this._DalUtil.Car.GetInfo(this.TableCode);

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
                        this.numTableCode.Tag = info.ToraDONCarId;
                        this.numTableCode.Value = info.ToraDONCarCode;
                        this.edtTableName.Text = info.LicPlateCarNo;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTableCode.Tag = null;
                    this.numTableCode.Value = null;
                    this.edtTableName.Text = string.Empty;
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
                if (this.TableCode == 0)
                {
                    is_clear = true;
                    return;
                }

                TokuisakiInfo info =
                    this._DalUtil.Tokuisaki.GetInfo(this.TableCode);

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
                        this.numTableCode.Tag = info.ToraDONTokuisakiId;
                        this.numTableCode.Value = info.ToraDONTokuisakiCode;
                        this.edtTableName.Text = info.ToraDONTokuisakiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numTableCode.Tag = null;
                    this.numTableCode.Value = null;
                    this.edtTableName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// システム区分の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSystemKbn(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SystemKbn == 0)
                {
                    is_clear = true;
                    return;
                }

                SystemNameInfo info =
                    this._DalUtil.SystemGlobalName.GetInfo(Convert.ToInt32(this.cmbSystemKbn.SelectedValue), this.SystemKbn);

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
                    this.numSystemKbn.Value = info.SystemNameCode;
                    this.edtSystemKbnName.Text = info.SystemName;
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSystemKbn.Value = null;
                    this.edtSystemKbnName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// コントロールの活性／非活性を制御します。
        /// </summary>
        /// <param name="e"></param>
        private void SetControlEnabled(int val)
        {
            switch ((DefaultProperty.HaishokuTableKbn)val)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = true;
                    this.numSystemKbn.Enabled = true;
                    this.edtSystemKbnName.Enabled = true;
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    this.numTableCode.Enabled = true;
                    this.edtTableName.Enabled = true;
                    this.cmbSystemKbn.Enabled = false;
                    this.numSystemKbn.Enabled = false;
                    this.edtSystemKbnName.Enabled = false;
                    break;
                default:
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = false;
                    this.numSystemKbn.Enabled = false;
                    this.edtSystemKbnName.Enabled = false;
                    break;
            }
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        public string Createlog()
        {
            return this.KeyString + "\r\n" +
                    this.CodeString + "\r\n" +
                    this.KbnString + "\r\n" +
                    this.KbnValueString + "\r\n" +
                    this.ForeColorString + "\r\n" +
                    this.BackColorString + "\r\n" +
                    ""
                    ;
        }

        /// <summary>
        /// 画面「キー」の条件指定を文字型で取得します。
        /// </summary>
        private String KeyString
        {
            get
            {
                return string.Format("[キー] {0}", DefaultProperty.GetHaishokuTableKbnMeisho(
                    (DefaultProperty.HaishokuTableKbn)(Convert.ToInt32(this.cmbKey.SelectedValue))));
            }
        }

        /// <summary>
        /// 画面「コード」の条件指定を文字型で取得します。
        /// </summary>
        private String CodeString
        {
            get
            {
                return string.Format("[コード] {0}", this.TableCode.ToString());
            }
        }

        /// <summary>
        /// 画面「区分」の条件指定を文字型で取得します。
        /// </summary>
        private String KbnString
        {
            get
            {
                return string.Format("[区分] {0}", FrameUtilites.GetSystemNameKbnString(
                    (DefaultProperty.SystemNameKbn)(Convert.ToInt32(this.cmbSystemKbn.SelectedValue == null ? "0" : this.cmbSystemKbn.SelectedValue))));
            }
        }

        /// <summary>
        /// 画面「区分値」の条件指定を文字型で取得します。
        /// </summary>
        private String KbnValueString
        {
            get
            {
                return string.Format("[区分値] {0}", this.SystemKbn.ToString());
            }
        }

        /// <summary>
        /// 画面「文字色」の条件指定を文字型で取得します。
        /// </summary>
        private String ForeColorString
        {
            get
            {
                return string.Format("[文字色] {0}", this.clpForeColor.Text);
            }
        }

        /// <summary>
        /// 画面「背景色」の条件指定を文字型で取得します。
        /// </summary>
        private String BackColorString
        {
            get
            {
                return string.Format("[背景色] {0}", this.clpBackColor.Text);
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
        private void ProcessFpHaishokuListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void HaishokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void HaishokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpHaishokuListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //配色リスト - CellDoubleClick
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
        private void fpHaishokuListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpHaishokuListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpHaishokuListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpHaishokuListGridPreviewKeyDown(e);
        }

        private void HaishokuFrame_KeyDown(object sender, KeyEventArgs e)
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
                this.SetHaishokuListSheet();
            }
        }

        private void numTableCode_Validating(object sender, CancelEventArgs e)
        {
            int kbn = Convert.ToInt32(this.cmbKey.SelectedValue);

            switch ((DefaultProperty.HaishokuTableKbn)kbn)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    this.numTableCode.Enabled = false;
                    this.edtTableName.Enabled = false;
                    this.cmbSystemKbn.Enabled = true;
                    this.numSystemKbn.Enabled = true;
                    this.edtSystemKbnName.Enabled = true;
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                    this.ValidateCarCode(e);
                    break;
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    this.ValidateTokuisakiCode(e);
                    break;
                default:
                    break;
            }
        }

        private void numSystemKbn_Validating(object sender, CancelEventArgs e)
        {
            //システム区分
            this.ValidateSystemKbn(e);
        }

        private void fpHaishokuListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void cmbKey_SelectedValueChanged(object sender, EventArgs e)
        {
            this.numTableCode.Tag = null;
            this.numTableCode.Value = null;
            this.edtTableName.Text = string.Empty;
            this.numSystemKbn.Value = null;
            this.edtSystemKbnName.Text = string.Empty;
            this.SetControlEnabled(Convert.ToInt32((((GcComboBox)sender).SelectedValue)));
        }

        private void cmbSystemKbn_SelectedValueChanged(object sender, EventArgs e)
        {
            this.numSystemKbn.Value = null;
            this.edtSystemKbnName.Text = string.Empty;
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void clp_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SendKeys.Send("{F4}");
        }
    }
}
