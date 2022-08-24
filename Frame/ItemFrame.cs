using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
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
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using System.Globalization;
using GrapeCity.Win.Editors;
using Jpsys.HaishaManageV10.ComLib;


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class ItemFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public ItemFrame()
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
        private const string WINDOW_TITLE = "品目登録";

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
        /// 品目クラス
        /// </summary>
        private Item _Item;

        /// <summary>
        /// 現在編集中の品目情報を保持する領域
        /// </summary>
        private ItemInfo _ItemInfo = null;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        ///// <summary>
        ///// 各GcNumberに対応するSearchFunctionを返します。
        ///// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        ///// </summary>
        //private Dictionary<Control, System.Action> SearchFunctions;

        #region 品目一覧

        //--Spread列定義

        /// <summary>
        /// 品目コード列番号
        /// </summary>
        private const int COL_ITEM_CODE = 0;

        /// <summary>
        /// 品目名列番号
        /// </summary>
        private const int COL_ITEM_NAME = 1;

        /// <summary>
        /// 品目名カナ列番号
        /// </summary>
        private const int COL_ITEM_KANA = 2;

        /// <summary>
        /// 品目リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_ITEM_LIST = 3;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialItemStyleInfoArr;

        /// <summary>
        /// 品目情報のリストを保持する領域
        /// </summary>
        private IList<ItemInfo> _ItemInfoList = null;

        /// <summary>
        /// 検索結果一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx_Result = 0;

        #endregion

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitItemFrame()
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

            //品目クラスインスタンス作成
            this._Item = new Item(this.appAuth);

            ////検索画面を起動するコントロールと検索画面起動メソッドの設定
            //this.SearchFunctions = new Dictionary<Control, System.Action>
            //{
            //    {this.numItemGroupCode, this.ShowCmnSearchItemGroup},
            //};

            //無効フラグはトラDONで制御するため非表示
            this.chkDisableFlag.Visible = false;

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

            //品目リストのセット
            this.SetItemListSheet();

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
            //大分類コンボボックス
            this.InitLBunruiCombo();
        }

        /// <summary>
        /// 大分類コンボボックスを初期化します。
        /// </summary>
        private void InitLBunruiCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            foreach (ItemLBunruiInfo item in this._DalUtil.ItemLBunrui.GetList())
            {
                datasource.Add(item.ItemLBunruiId.ToString(), item.ItemLBunruiCode + "：" + item.ItemLBunruiName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbLBunrui, datasource, true, null, false);
        }

        /// <summary>
        /// 中分類コンボボックスを初期化します。
        /// </summary>
        private void InitMBunruiCombo()
        {
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<ItemMBunruiInfo> list = new List<ItemMBunruiInfo>();

            //大分類コードが設定されている場合
            if (!(Convert.ToDecimal(this.cmbLBunrui.SelectedValue) == 0))
            {
                //中分類コンボを初期化して戻る
                list = this._DalUtil.ItemMBunrui.GetList(
                    new ItemMBunruiSearchParameter()
                    {
                        ItemLBunruiId = Convert.ToDecimal(this.cmbLBunrui.SelectedValue)
                    });
            }

            foreach (ItemMBunruiInfo item in list)
            {
                datasource.Add(item.ItemMBunruiId.ToString(), item.ItemMBunruiCode + "：" + item.ItemMBunruiName);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbMBunrui, datasource, true, null, false);
        }

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //品目リストの初期化
            this.InitItemListSheet();
        }

        /// <summary>
        /// 品目リストを初期化します。
        /// </summary>
        private void InitItemListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpItemListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_ITEM_LIST);
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
            this.numItemCode.Value = 0;
            this.edtItemName.Text = string.Empty;
            this.edtItemNameKana.Text = string.Empty;
            this.edtItemShortName.Text = string.Empty;
            this.edtItemSShortName.Text = string.Empty;
            //this.numItemGroupCode.Tag = null;
            //this.numItemGroupCode.Value = null;
            //this.edtItemGroupName.Text = string.Empty;
            this.cmbLBunrui.SelectedValue = null;
            this.cmbMBunrui.SelectedValue = null;

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
            //品目スタイル情報初期化
            this.InitItemStyleInfo();
        }

        /// <summary>
        /// 品目のスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitItemStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpItemListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialItemStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_ITEM_LIST];

            for (int i = 0; i < COL_MAXCOLNUM_ITEM_LIST; i++)
            {
                this.initialItemStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            //validatingEventRaiserCollection.Add(
            //    ControlValidatingEventRaiser.Create(this.numItemGroupCode, ctl => ctl.Text, this.numItemGroupCode_Validating));
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
            //this.searchStateBinder.AddSearchableControls(
            //    this.numItemGroupCode
            //    );

            //this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        #endregion

        #region プロパティ

        ///// <summary>
        ///// 品目グループコードの値を取得します。
        ///// </summary>
        //private int ItemGroupCode
        //{
        //    get { return Convert.ToInt32(this.numItemGroupCode.Value); }
        //}

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitItemFrame();
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
                    this.numItemCode.Enabled = false;
                    this.edtItemName.Enabled = false;
                    this.edtItemNameKana.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = false;
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlLeft.Enabled = false;
                    this.pnlRight.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.numItemCode.Enabled = false;
                    this.edtItemName.Enabled = false;
                    this.edtItemNameKana.Enabled = false;
                    //--ファンクションの使用可否
                    _commandSet.ChangeCode.Enabled = true;
                    _commandSet.EditCancel.Enabled = true;
                    _commandSet.Delete.Enabled = (this._ItemInfo != null && 0 < this._ItemInfo.ItemId);
                    _commandSet.Save.Enabled = true;
                    _commandSet.Close.Enabled = true;
                    
                    break;
                //case FrameEditMode.ViewOnly:
                //    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// 品目リストに値を設定します。
        /// </summary>
        private void SetItemListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._ItemInfoList =
                    this._Item.GetList();

                IList<ItemInfo> wk_list = this.GetMatchedList(this._ItemInfoList);

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
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_ITEM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_ITEM_LIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    ItemInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_ITEM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialItemStyleInfoArr[k]);

                        //[使用しない]データの場合は、背景色をグレーに変更します
                        if (wk_info.ToraDONDisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_ITEM_CODE, wk_info.ToraDONItemCode);
                    datamodel.SetValue(j, COL_ITEM_NAME, wk_info.ToraDONItemName);
                    datamodel.SetValue(j, COL_ITEM_KANA, wk_info.ToraDONItemNameKana);

                    datamodel.SetTag(j, COL_ITEM_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpItemListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpItemListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpItemListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpItemListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpItemListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpItemListGrid.Sheets[0].ColumnCount - 1);

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
        private List<ItemInfo> GetMatchedList(IList<ItemInfo> list)
        {
            List<ItemInfo> rt_list = new List<ItemInfo>();

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

            foreach (ItemInfo item in list)
            {
                //「コード」+「名称」+「カナ」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.ToraDONItemCode) + Environment.NewLine
                    + item.ToraDONItemName.Trim() + Environment.NewLine
                    + item.ToraDONItemNameKana.Trim() + Environment.NewLine;

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
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //選択品目情報をリストから取得して設定
            ItemInfo info = this.GetItemInfoByItemListOnSelection();

            if (info.ToraDONItemId == 0)
            {
                return;
            }

            try
            {
                //品目情報取得
                this._ItemInfo =
                    this._Item.GetInfoById(info.ToraDONItemId);

                //画面設定
                this.SetScreen();

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditMode.Editable);

                    //フォーカス設定
                    this.edtItemShortName.Focus();
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
        /// 品目リストにて選択中の品目情報を取得します。
        /// 未選択の場合は初期値を返却します。
        /// </summary>
        /// <returns>選択中の品目情報</returns>
        private ItemInfo GetItemInfoByItemListOnSelection()
        {
            //返却用
            ItemInfo rt_info = new ItemInfo();

            SheetView sheet0 = this.fpItemListGrid.Sheets[0];

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
                    //最初の列にセットしたTagから品目情報を取得
                    rt_info = (ItemInfo)sheet0.Cells[select_row, COL_ITEM_CODE].Tag;
                }
            }

            //返却
            return rt_info;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            if (this._ItemInfo ==null)
            {
                this.ClearInputs();

                //選択された品目情報を取得
                ItemInfo info = this.GetItemInfoByItemListOnSelection();
                this._ItemInfo.ToraDONItemId = info.ToraDONItemId;

                //表示項目設定
                this.numItemCode.Value = info.ToraDONItemCode;
                this.edtItemName.Text = StringHelper.ConvertToString(info.ToraDONItemName);
                this.edtItemNameKana.Text = StringHelper.ConvertToString(info.ToraDONItemNameKana);
            }
            else
            {
                this.numItemCode.Value = this._ItemInfo.ToraDONItemCode;
                this.edtItemName.Text = this._ItemInfo.ToraDONItemName;
                this.edtItemNameKana.Text = this._ItemInfo.ToraDONItemNameKana;
                this.edtItemShortName.Text = this._ItemInfo.ItemShortName;
                this.edtItemSShortName.Text = this._ItemInfo.ItemSShortName;
                //this.numItemGroupCode.Tag = this._ItemInfo.ItemGroupId;
                //this.numItemGroupCode.Value = this._ItemInfo.ItemGroupCode;
                //this.edtItemGroupName.Text = this._ItemInfo.ItemGroupName;
                this.cmbLBunrui.SelectedValue = this._ItemInfo.ItemLBunruiId == 0
                    ? (string)null
                    : this._ItemInfo.ItemLBunruiId.ToString();
                this.cmbMBunrui.SelectedValue = this._ItemInfo.ItemMBunruiId == 0
                    ? (string)null
                    : this._ItemInfo.ItemMBunruiId.ToString();

                this.chkDisableFlag.Checked = _ItemInfo.DisableFlag;
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
                        this._Item.Save(tx, this._ItemInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "品目", _ItemInfo.ToraDONItemCode.ToString() });

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
                    this.DoClear(false);
                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);

                    this.edtItemShortName.Focus();
                }
                catch (MustCloseFormException ex)
                {
                    //画面の終了が要求される例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                    //画面を閉じます
                    this.DoClose();
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            this._ItemInfo.ToraDONItemCode = Convert.ToInt32(this.numItemCode.Value);
            this._ItemInfo.ToraDONItemName = this.edtItemName.Text.Trim();
            this._ItemInfo.ToraDONItemNameKana = this.edtItemNameKana.Text.Trim();
            this._ItemInfo.ItemShortName = this.edtItemShortName.Text.Trim();
            this._ItemInfo.ItemSShortName = this.edtItemSShortName.Text.Trim();
            //this._ItemInfo.ItemGroupId = Convert.ToDecimal(this.numItemGroupCode.Tag);
            //this._ItemInfo.ItemGroupCode = Convert.ToInt32(this.numItemGroupCode.Value);
            this._ItemInfo.ItemLBunruiId = Convert.ToDecimal(this.cmbLBunrui.SelectedValue);
            this._ItemInfo.ItemMBunruiId = Convert.ToDecimal(this.cmbMBunrui.SelectedValue);
            this._ItemInfo.DisableFlag = this.chkDisableFlag.Checked;
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
            if (rt_val && Convert.ToDecimal(this.numItemCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numItemCode;
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
                                this._Item.Delete(tx, _ItemInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "品目", _ItemInfo.ToraDONItemCode.ToString() });

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

            //コンボボックスの初期化
            this.InitCombo();

            //入力項目をクリア
            this.ClearInputs();

            //Spread関連をクリア
            this.InitSheet();

            //品目リストをセット
            this.SetItemListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpItemListGrid.Focus();
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
                //case Keys.F5:
                //    // F5は共通検索画面
                //    this.ShowCmnSearch();
                //    break;
                default:
                    break;
            }
        }

        ///// <summary>
        ///// フォーカスしているコントロールに対応する共通検索画面を表示します。
        ///// </summary>
        //private void ShowCmnSearch()
        //{
        //    if (SearchFunctions.ContainsKey(ActiveControl))
        //    {
        //        SearchFunctions[ActiveControl]();
        //    }
        //}

        ///// <summary>
        ///// 品目グループ検索画面を表示して結果を画面に設定します。
        ///// </summary>
        //private void ShowCmnSearchItemGroup()
        //{
        //    using (CmnSearchItemGroupFrame f = new CmnSearchItemGroupFrame())
        //    {
        //        // パラメータをセット
        //        f.ShowAllFlag = false;

        //        f.InitFrame();
        //        f.ShowDialog(this);

        //        if (f.DialogResult == DialogResult.OK)
        //        {
        //            // 画面から値を取得
        //            ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
        //                Convert.ToInt64(f.SelectedInfo.ItemGroupCode);

        //            this.OnCmnSearchComplete();
        //        }
        //    }
        //}

        #endregion

        #region 検証（Validate）処理

        ///// <summary>
        ///// 品目グループコードの値の検証を行います。
        ///// </summary>
        ///// <param name="e"></param>
        //private void ValidateItemGroupCode(CancelEventArgs e)
        //{
        //    bool is_clear = false;

        //    try
        //    {
        //        //コードの入力が無い場合は抜ける
        //        if (this.ItemGroupCode == 0)
        //        {
        //            is_clear = true;
        //            return;
        //        }

        //        ItemGroupInfo info =
        //            this._DalUtil.ItemGroup.GetInfo(this.ItemGroupCode);

        //        if (info == null)
        //        {
        //            //データなし
        //            MessageBox.Show(
        //                FrameUtilites.GetDefineMessage("MW2201003"),
        //                this.Text,
        //                MessageBoxButtons.OK,
        //                MessageBoxIcon.Warning);

        //            is_clear = true;
        //            e.Cancel = true;
        //        }
        //        else
        //        {
        //            if (info.DisableFlag)
        //            {
        //                MessageBox.Show(
        //                    FrameUtilites.GetDefineMessage("MW2201016"),
        //                    this.Text,
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Warning);

        //                is_clear = true;
        //                e.Cancel = true;
        //            }
        //            else
        //            {
        //                this.numItemGroupCode.Tag = info.ItemGroupId;
        //                this.numItemGroupCode.Value = info.ItemGroupCode;
        //                this.edtItemGroupName.Text = info.ItemGroupName;
        //            }
        //        }
        //    }
        //    finally
        //    {
        //        if (is_clear)
        //        {
        //            this.numItemGroupCode.Tag = null;
        //            this.numItemGroupCode.Value = null;
        //            this.edtItemGroupName.Text = string.Empty;
        //        }
        //    }
        //}

        #endregion

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.currentMode == FrameEditMode.Editable)
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
        private void ProcessFpItemListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void ItemFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void ItemFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpItemListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //品目リスト - CellDoubleClick
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
        private void fpItemListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpItemListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpItemListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpItemListGridPreviewKeyDown(e);
        }

        private void ItemFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetItemListSheet();
        }

        //private void numItemGroupCode_Validating(object sender, CancelEventArgs e)
        //{
        //    //品目グループコード
        //    this.ValidateItemGroupCode(e);
        //}

        //private void sbtn_Click(object sender, EventArgs e)
        //{
        //    this.ShowCmnSearch();
        //}

        private void fpItemListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void cmbLBunrui_SelectedValueChanged(object sender, EventArgs e)
        {
            this.InitMBunruiCombo();
        }
    }
}
