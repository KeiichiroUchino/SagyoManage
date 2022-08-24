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
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame.Command;
using Jpsys.HaishaManageV10.FrameLib.WinForms;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class CmnSearchContractFrame : Form, IFrameBase
    {
        #region ユーザ定義

        /// <summary>
        /// 画面タイトルの静的な部分
        /// </summary>
        private const string WINDOW_TITLE = "請負検索";

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示しているファイルメニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを
        /// 利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil dalUtil;

        /// <summary>
        /// 検索テーブルのクラス領域
        /// </summary>
        private ToraDONContract searchTblClass;

        /// <summary>
        /// トラDON請負情報のリストを保持する領域
        /// </summary>
        private IList<ToraDONContractInfo> ContractList = null;

        /// <summary>
        /// 起動パラメータの得意先コードを保持
        /// </summary>
        private int _paramTokuisakiCode = 0;

        /// <summary>
        /// 起動パラメータの請求部門コードを保持
        /// </summary>
        private int _paramClmClassCode = 0;

        /// <summary>
        /// 行の複数選択を許可するかどうかの値を保持
        /// </summary>
        private bool _allowExtendedSelectRow = false;

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

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        #region Spread_リスト関係

        /// <summary>
        /// コード列番号
        /// </summary>
        private const int COL_CODE = 0;
        /// <summary>
        /// 名称列番号
        /// </summary>
        private const int COL_NAME = 1;
        /// <summary>
        /// 請負期間列番号
        /// </summary>
        private const int COL_SPAN = 2;
        /// <summary>
        /// 請負金額列番号
        /// </summary>
        private const int COL_KINGAKU = 3;
        /// <summary>
        /// 税列番号
        /// </summary>
        private const int COL_ZEI = 4;
        /// <summary>
        /// 通行料列番号
        /// </summary>
        private const int COL_TSUKORYO = 5;
        /// <summary>
        /// 検索一覧最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LIST = 6;

        #endregion

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public CmnSearchContractFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        private void InitCmnSearchContractFrame()
        {
            //画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
            this.InitFrameColor();

            //親フォームがあるときは、そのフォームを中心に表示する
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
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを
            // 利用するためのファサードクラスのインスタンス作成
            this.dalUtil = new CommonDALUtil(this.appAuth);

            //検索テーブルのクラスインスタンス作成
            this.searchTblClass = new ToraDONContract();

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numSearchTokuisakiCode, this.ShowCmnSearchTokuisaki},
                {this.numSearchClmClassCode, this.ShowCmnSearchClmClass},
            };

            //Spread関連の初期化
            this.InitSheet();

            //入力項目をクリア
            this.ClearInputs();

            //登録済み一覧を取得
            this.DoGetData();

            //リストのセット
            this.SetListSheet();

            this.ActiveControl = this.fpListGrid;
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
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //リストの初期化
            this.InitListSheet();
        }

        /// <summary>
        /// リストの初期化します。
        /// </summary>
        private void InitListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            //行数の初期化
            sheet0.Models.Data.RowCount = 0;

            //複数行選択の許可
            if (this._allowExtendedSelectRow)
            {
                sheet0.OperationMode = OperationMode.ExtendedSelect;
            }

            for (int i = 0; i < COL_MAXCOLNUM_LIST; i++)
            {
                sheet0.Columns[i].ResetSortIndicator();
            }
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
            //メニューを
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Select);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***OK（選択）
            this.commandSet.Select.Execute += OkCommand_Execute;
            this.commandSet.Bind(this.commandSet.Select,
               this.btnOk, this.actionMenuItems.GetMenuItemBy(ActionMenuItems.Select), this.toolStripSelect);

            //***キャンセル（終了）
            commandSet.Close.Execute += CancelCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
                this.numSearchTokuisakiCode,
                this.numSearchClmClassCode
                );

            this.searchStateBinder.AddStateObject(this.toolStripSearch);
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
                ControlValidatingEventRaiser.Create(this.numSearchClmClassCode, ctl => ctl.Text, this.numSearchClmClassCode_Validating));
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.numSearchTokuisakiCode.Tag = null;
            this.numSearchTokuisakiCode.Value = null;
            this.edtSearchTokuisakiName.Text = string.Empty;
            if (0 < this.ParamTokuisakiCode)
            {
                this.numSearchTokuisakiCode.Value = this.ParamTokuisakiCode;
                this.ValidateSearchTokuisakiCode(new System.ComponentModel.CancelEventArgs());
            }
            else
            {
                this.numSearchTokuisakiCode.Tag = null;
                this.numSearchTokuisakiCode.Value = null;
                this.edtSearchTokuisakiName.Text = string.Empty;
            }
            this.numSearchClmClassCode.Tag = null;
            this.numSearchClmClassCode.Value = null;
            this.edtSearchClmClassName.Text = string.Empty;
            if (0 < this.ParamClmClassCode)
            {
                this.numSearchClmClassCode.Value = this.ParamClmClassCode;
                this.ValidateSearchClmClassCode(new System.ComponentModel.CancelEventArgs());
            }
            else
            {
                this.numSearchClmClassCode.Tag = null;
                this.numSearchClmClassCode.Value = null;
                this.edtSearchClmClassName.Text = string.Empty;
            }

            this.edtHyojiKensu.Text = "0件";

            this.btnOk.Enabled = false;
            this.commandSet.Select.Enabled = false;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// 請負一覧を取得します。
        /// </summary>
        private void DoGetData()
        {
            //請負一覧を取得
            this.ContractList =
                this.searchTblClass.GetList(new ToraDONContractSearchParameter() { });
        }

        /// <summary>
        /// 検索一覧に値を設定します。
        /// 検索条件が指定されている場合は、該当するデータのみを設定します。
        /// </summary>
        private void SetListSheet()
        {
            SheetView sheet = this.fpListGrid.ActiveSheet;

            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索一覧を初期化
                this.InitListSheet();

                if (this.ContractList == null)
                {
                    return;
                }

                //抽出後のリストを取得（同時に並べ替え）
                List<ToraDONContractInfo> wk_list =
                    this.GetMatchedList(this.ContractList).OrderBy(x => x.ContractCode).ToList();

                // 件数取得
                int rowcount = wk_list.Count;

                this.edtHyojiKensu.Text = rowcount.ToString("#,##0件");

                if (rowcount == 0)
                {
                    //OKボタンを無効にして処理から抜ける
                    this.btnOk.Enabled = false;
                    this.commandSet.Select.Enabled = false;
                    return;
                }

                //Spreadのデータモデルを抜き出す
                var datamodel = sheet.Models.Data;
                //Spreadのスタイルモデルを抜き出す
                var stylemodel = sheet.Models.Style;

                //行数の設定
                datamodel.RowCount = wk_list.Count;

                //ループしてモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    ToraDONContractInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(stylemodel.GetDirectInfo(j, -1, null));

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    string contractTime = wk_info.ContractStartDate.ToString("yyyy/MM/dd") + " ～ " +
                                           wk_info.ContractEndDate.ToString("yyyy/MM/dd");

                    //データモデルのセット
                    datamodel.SetValue(j, COL_CODE, wk_info.ContractCode);
                    datamodel.SetValue(j, COL_NAME, wk_info.ContractName);
                    datamodel.SetValue(j, COL_SPAN, contractTime);
                    datamodel.SetValue(j, COL_KINGAKU, wk_info.ContractPrice - wk_info.TollFeeInContractPrice);
                    datamodel.SetValue(j, COL_ZEI, wk_info.TaxDispKbnShortName);
                    datamodel.SetValue(j, COL_TSUKORYO, wk_info.TollFeeInContractPrice);

                    // Tagへ1件分のデータを退避（後で使用）
                    datamodel.SetTag(j, COL_CODE, wk_info);
                }

                //検索結果の1行目を選択状態にする
                sheet.SetActiveCell(0, 0, true);
                sheet.AddSelection(0, -1, 1, sheet.ColumnCount - 1);

                // 明細がない場合はOKボタンをロック
                if (sheet.RowCount == 0)
                {
                    this.btnOk.Enabled = false;
                    this.commandSet.Select.Enabled = false;
                }
                else
                {
                    this.btnOk.Enabled = true;
                    this.commandSet.Select.Enabled = true;
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //デフォルトに戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 検索テーブル情報の一覧を指定して、抽出条件に合致する検索テーブル情報の一覧を取得します。
        /// </summary>
        /// <param name="list">検索テーブル情報の一覧</param>
        private List<ToraDONContractInfo> GetMatchedList(IList<ToraDONContractInfo> list)
        {
            List<ToraDONContractInfo> rt_list = new List<ToraDONContractInfo>();

            //使用停止表示フラグによってデータ再抽出
            IList<ToraDONContractInfo> wk_list = list;

            //検索用得意先ID取得
            decimal tokuisakiid = Convert.ToDecimal(this.numSearchTokuisakiCode.Tag);

            //検索用請求部門ID取得
            decimal clmclassid = Convert.ToDecimal(this.numSearchClmClassCode.Tag);

            foreach (ToraDONContractInfo item in wk_list)
            {
                //検索用得意先IDが存在しない、もしくは、検索用得意先IDと一致
                //もしくは、
                //検索用請求部門IDが存在しない、もしくは、検索用請求部門IDと一致
                if ((0 == tokuisakiid || tokuisakiid == item.TokuisakiId)
                    || (0 == clmclassid || clmclassid == item.ClmClassId))
                {
                    rt_list.Add(item);
                }
            }

            return rt_list;
        }

        /// <summary>
        /// 選択している請負情報をメンバにセットします。
        /// </summary>
        private void SetSelectedInfo()
        {
            List<ToraDONContractInfo> wk_val = this.GetInfoByListOnSelection();

            //選択していない場合は警告を表示する
            if (wk_val.Count == 0)
            {
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2203012", new string[] { "データ" }),
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                this.fpListGrid.Focus();
            }
            else
            {
                if (this._allowExtendedSelectRow)
                {
                    //複数行の選択を許可している場合はリストのプロパティに格納
                    this.SelectedInfoList = wk_val;
                }
                else
                {
                    //複数行の選択を許可していない場合は検索テーブル情報のプロパティに格納
                    this.SelectedInfo = wk_val[0];
                }

                //OKで画面を閉じる
                this.DoClose(DialogResult.OK);
            }
        }

        /// <summary>
        /// 一覧にて選択中の請負情報を取得します。
        /// </summary>
        /// <returns>請負情報</returns>
        private List<ToraDONContractInfo> GetInfoByListOnSelection()
        {
            List<ToraDONContractInfo> rt_list = new List<ToraDONContractInfo>();

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行をループで回してコードを取得
                foreach (CellRange item in sheet0.GetSelections())
                {
                    //ブロックの中のセルを取り出してコードをリストに追加
                    for (int i = 0; i < item.RowCount; i++)
                    {
                        ToraDONContractInfo info =
                            ((ToraDONContractInfo)sheet0.Cells[item.Row + i, COL_CODE].Tag);

                        rt_list.Add(info);
                    }
                }
            }

            return rt_list;
        }

        /// <summary>
        /// DialogResultを指定して画面を閉じます。
        /// </summary>
        /// <param name="dialogResult"></param>
        private void DoClose(DialogResult dialogResult)
        {
            this.DialogResult = dialogResult;
        }

        /// <summary>
        /// 結果一覧上でのPreviewKeyDownイベントを処理します。
        /// </summary>
        private void ProcessFpListGirdPreviewKeyDown(PreviewKeyDownEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Enter:
                    this.SetSelectedInfo();
                    break;
                default:
                    break;
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
        /// 請求部門検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchClmClass()
        {
            using (CmnSearchClmClassFrame f = new CmnSearchClmClassFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ClmClassCode);

                    this.OnCmnSearchComplete();
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
                    this.dalUtil.Tokuisaki.GetInfo(this.SearchTokuisakiCode);

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

                // 検索処理
                this.SetListSheet();
            }
        }

        /// <summary>
        /// 検索用請求部門コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchClmClassCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchClmClassCode == 0)
                {
                    is_clear = true;
                    return;
                }

                ToraDONClmClassInfo info =
                    this.dalUtil.ToraDONClmClass.GetInfo(this.SearchClmClassCode);

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
                        this.numSearchClmClassCode.Tag = info.ClmClassId;
                        this.numSearchClmClassCode.Value = info.ClmClassCode;
                        this.edtSearchClmClassName.Text = info.ClmClassName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchClmClassCode.Tag = null;
                    this.numSearchClmClassCode.Value = null;
                    this.edtSearchClmClassName.Text = string.Empty;
                }

                // 検索処理
                this.SetListSheet();
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 検索用得意先コードの値を取得します。
        /// </summary>
        private int SearchTokuisakiCode
        {
            get { return Convert.ToInt32(this.numSearchTokuisakiCode.Value); }
        }

        /// <summary>
        /// 検索用請求部門コードの値を取得します。
        /// </summary>
        private int SearchClmClassCode
        {
            get { return Convert.ToInt32(this.numSearchClmClassCode.Value); }
        }

        /// <summary>
        /// 選択された検索テーブル情報を取得します。
        /// </summary>
        public ToraDONContractInfo SelectedInfo { get; private set; }

        /// <summary>
        /// 選択されたトラDON請求部門情報を一覧で取得します。
        /// </summary>
        public List<ToraDONContractInfo> SelectedInfoList { get; private set; }

        /// <summary>
        /// 起動パラメータの得意先コードを取得・設定します
        /// </summary>
        public int ParamTokuisakiCode
        {
            get { return this._paramTokuisakiCode; }
            set { this._paramTokuisakiCode = value; }
        }

        /// <summary>
        /// 起動パラメータの請求部門コードを取得・設定します
        /// </summary>
        public int ParamClmClassCode
        {
            get { return this._paramClmClassCode; }
            set { this._paramClmClassCode = value; }
        }

        /// <summary>
        /// 複数行、複数ブロックの行選択を許可するかどうかの値を取得・設定します(true:許可する)。
        /// </summary>
        public bool AllowExtendedSelectRow
        {
            get { return this._allowExtendedSelectRow; }
            set { this._allowExtendedSelectRow = value; }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitCmnSearchContractFrame();
        }

        /// <summary>
        /// 本インスタンスのNameプロパティをインターフェース経由で取得・設定します。
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
        /// 本インスタンスのNameプロパティをインターフェース経由で取得・設定します。
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

        private void numSearchTokuisakiCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidateSearchTokuisakiCode(e);
        }

        private void numSearchClmClassCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidateSearchClmClassCode(e);
        }

        private void CmnSearchContractFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void OkCommand_Execute(object sender, EventArgs e)
        {
            // 選択処理
            this.SetSelectedInfo();
        }

        private void CancelCommand_Execute(object sender, EventArgs e)
        {
            // キャンセル処理
            this.DoClose(DialogResult.Cancel);
        }

        private void edtSearchText_TextChanged(object sender, EventArgs e)
        {
            // 検索処理
            this.SetListSheet();
        }

        private void chkAllFlag_CheckedChanged(object sender, EventArgs e)
        {
            // 検索処理
            this.SetListSheet();
        }

        private void fpListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            //結果一覧 - CellClick
            if (e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダ上の場合はその列の自動ソートを実行
                this.fpListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //結果一覧をダブルクリック
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                this.SetSelectedInfo();
            }
        }

        private void fpListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //結果一覧 - PreviewKeyDown
            this.ProcessFpListGirdPreviewKeyDown(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            //共通検索画面
            this.ShowCmnSearch();
        }
    }
}
