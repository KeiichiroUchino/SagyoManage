using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Frame.Command;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.FrameLib.ViewSupport;
using Jpsys.SagyoManage.FrameLib.MultiRow;
using Jpsys.SagyoManage.FrameLib.WinForms;
using GrapeCity.Win.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System.ComponentModel;
using jp.co.jpsys.util;
using System.Drawing;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;

namespace Jpsys.SagyoManage.Frame
{
    public partial class KeiyakuViewFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public KeiyakuViewFrame()
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

        #region 検索可能状態

        private SearchStateBinder _searchStateBinder;

        #endregion


        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "契約一覧";

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
        /// 契約一覧クラス
        /// </summary>
        private Keiyaku _Keiyaku;

        /// <summary>
        /// 検索した契約情報リストを保持する領域
        /// </summary>
        private List<KeiyakuInfo> _KeiyakuInfoList = new List<KeiyakuInfo>();

        /// <summary>
        /// 登録済みリストがダブルクリックされたかどうかを表す値を保持します。
        /// </summary>
        private bool isDoubleClickedListSheet = false;

        /// <summary>
        /// 登録済みの一覧の選択行を保持する領域
        /// </summary>
        private int selectrowidx = 0;

        /// <summary>
        /// 再表示する際、データがない場合メッセージを表示するかしないかの値を保持します。
        /// 初期値は、true:メッセージを表示。
        /// </summary>
        private bool _ReloadMessageFlg = true;

        #region Spread列定義

        #region 契約一覧

        /// <summary>
        /// 契約一覧の列名を表します。
        /// </summary>
        private enum SpreadColKeys
        {
            KeiyakuId,
            KeiyakuCode,
            KeiyakuName,
            KeiyakuStartYMD,
            KeiyakuEndYMD,
            TokuisakiCode,
            TokuisakiName,
            SagyoBashoCode,
            SagyoBashoName,
            SagyoDaiBunruiName,
            SagyoChuBunruiName,
            SagyoShoBunruiName,
            ShelterRenkeiFlg,
        }

        /// <summary>
        /// 契約一覧最大列数
        /// </summary>
        private int COL_MAXCOLUM_LIST = Enum.GetNames(typeof(SpreadColKeys)).Length;

        /// <summary>
        /// フィルタ情報を保持する領域
        /// </summary>
        private FarPoint.Win.Spread.HideRowFilter hrFilter;
        private string[] filterStrings;
        private FarPoint.Win.Spread.Model.SortIndicator[] sortIndicators;

        #endregion

        #endregion

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// 検索状態かどうかの値を保持する領域
        /// (true:検索状態)
        /// </summary>
        private bool isSearch = true;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitJuchuIchiranFrame()
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
            this.SettingSearchStateBinder();
            this.SettingCommands();
            this.SetupValidatingEventRaiser();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.edtTokuisakiCd, this.ShowCmnSearchTokuisaki},
                {this.edtSagyoBashoCd, this.ShowCmnSearchSagyobasho},
            };

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //契約一覧クラスインスタンス作成
            this._Keiyaku = new Keiyaku(this.appAuth);

            //コンボボックスの初期化
            this.InitCombo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            ////データを取得
            //this.DoGetData();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SettingSearchStateBinder()
        {
            this._searchStateBinder = new SearchStateBinder(this);
            this._searchStateBinder.AddSearchableControls(
                this.edtTokuisakiCd,
                this.edtSagyoBashoCd);
            this._searchStateBinder.AddStateObject(this.toolStripSearch);
        }

        /// <summary>
        /// 入力画面より再表示を指示
        /// </summary>
        /// <param name="id"></param>
        public void ReSetScreen(decimal id, bool adjustWidth = false, bool dtlFlg = true)
        {
            if (this.isSearch)
            {
                //入力画面より再表示を指示、件数が0件の場合でもメッセージを出さない
                this._ReloadMessageFlg = false;
                //フィルタ情報を保持
                this.GetFilterInfo();
                this.DoGetData(id);
                //フィルタ情報を復元
                this.SetFilterInfo();
                this._ReloadMessageFlg = true;
                //フォーカスを設定
                this.setDtlFocus(id, dtlFlg);
            }
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
            //SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            //行数の初期化
            sheet0.Models.Data.RowCount = 0;

            //フィルター・ソートをクリアする
            this.fpListGrid.ActiveSheet.RowFilter.ResetFilter();
            FrameUtilites.ResetSortInfo(this.fpListGrid.ActiveSheet);

            //マウスポインタを矢印キーにする
            this.fpListGrid.SetCursor(FarPoint.Win.Spread.CursorType.Normal, System.Windows.Forms.Cursors.Arrow);

            this.selectrowidx = 0;

            //件数表示
            this.grpDetail.Text = "検索結果(0件)";

            this.isSearch = false;
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.edtTokuisakiCd.Text = string.Empty;
            this.edtTokuisakiCd.Tag = null;
            this.edtTokuisakiNm.Text = string.Empty;

            this.edtSagyoBashoCd.Text = string.Empty;
            this.edtSagyoBashoCd.Tag = null;
            this.edtSagyoBashoNm.Text = string.Empty;

            this.edtKeiyakuCode.Text = string.Empty;
            this.edtKeiyakuName.Text = string.Empty;

            this.chkAllFlag.Checked = false;

            this.dteKeiyakuYMDFrom.Value = DateTime.Today.Date.AddMonths(-1);
            this.dteKeiyakuYMDTo.Value = DateTime.Today.Date.AddMonths(1);
            this.isSearch = false;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            validatingEventRaiserCollection.Add(
                   ControlValidatingEventRaiser.Create(this.edtTokuisakiCd, ctl => ctl.Text, this.edtTokuisakiCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbFilterDateKbns, ctl => ctl.Text, this.cmbFilterDateKbns_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteKeiyakuYMDFrom, ctl => ctl.Text, this.dteKeiyakuYMDFrom_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteKeiyakuYMDTo, ctl => ctl.Text, this.dteKeiyakuYMDTo_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtKeiyakuCode, ctl => ctl.Text, this.edtKeiyakuCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtKeiyakuName, ctl => ctl.Text, this.edtKeiyakuName_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.edtSagyoBashoCd, ctl => ctl.Text, this.edtSagyoBashoCd_Validating));
        }

        #endregion


        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitNyukinMeisaiKbnCombo();
            this.SagyoDaiBunruiCombo();
        }

        /// <summary>
        /// 日付指定区分コンボボックスを初期化します。
        /// </summary>
        private void InitNyukinMeisaiKbnCombo()
        {
            // 日付指定区分コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<SystemNameInfo> list = this._DalUtil.SystemGlobalName.GetList((Int32)DefaultProperty.SystemNameKbn.FilterDateKbns);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.SystemNameCode)
                    .ToList();
            }
            else
            {
                list = new List<SystemNameInfo>();
            }

            String key = "";
            String value = "";

            foreach (SystemNameInfo item in list)
            {
                key = item.SystemNameCode.ToString();
                value = item.SystemName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbFilterDateKbns, datasource, false, null, false);
            this.cmbFilterDateKbns.SelectedIndex = 0;
        }

        /// <summary>
        /// 作業大分類コンボボックスを初期化します。
        /// </summary>
        private void SagyoDaiBunruiCombo()
        {
            // 作業大分類コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<SagyoDaiBunruiInfo> list = this._DalUtil.SagyoDaiBunrui.GetList(null);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.SagyoDaiBunruiCode)
                    .ToList();
            }
            else
            {
                list = new List<SagyoDaiBunruiInfo>();
            }

            String key = "";
            String value = "";

            foreach (SagyoDaiBunruiInfo item in list)
            {
                key = item.SagyoDaiBunruiId.ToString();
                value = item.SagyoDaiBunruiCode.ToString() + "." + item.SagyoDaiBunruiName.ToString();

                datasource.Add(key, value);
            }

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

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        void New_Execute(object sender, EventArgs e)
        {
            //登録画面を開く
            this.ShowMitsumoriFrame();
        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        private void ymd_Down(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, -1);
        }

        private void ymd_Up(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, 1);
        }

        private void AddDayForNskDateTime(Control c, int days)
        {
            if (c.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)c).Value = Convert.ToDateTime(((NskDateTime)c).Value).AddDays(days);
                }
                catch
                {
                    ;
                }
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitJuchuIchiranFrame();
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
                    this.pnlTop.Enabled = true;
                    this.pnlMid.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;
                case FrameEditMode.Editable:
                    //編集モード
                    this.pnlTop.Enabled = false;
                    this.pnlMid.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    //--ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;
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
        /// 契約情報一覧の検索条件を取得します
        /// </summary>
        /// <returns>契約情報一覧の検索条件</returns>
        private KeiyakuSearchParameter GetSearchParameter()
        {
            KeiyakuSearchParameter para = new KeiyakuSearchParameter();

            // 日付指定区分
            para.FilterDateKbns = Convert.ToInt32(this.cmbFilterDateKbns.SelectedValue);

            // 契約日付（From）
            if (this.dteKeiyakuYMDFrom.Value != null)
            {
                para.StartYMD = new DateTime(
                    this.dteKeiyakuYMDFrom.Value.Value.Year
                    , this.dteKeiyakuYMDFrom.Value.Value.Month
                    , this.dteKeiyakuYMDFrom.Value.Value.Day
                    , 0
                    , 0
                    , 0);
            }
            else
            {
                para.StartYMD = DateTime.MinValue;
            }

            // 契約日付（To）
            if (this.dteKeiyakuYMDTo.Value != null)
            {
                para.EndYMD = new DateTime(
                    this.dteKeiyakuYMDTo.Value.Value.Year
                    , this.dteKeiyakuYMDTo.Value.Value.Month
                    , this.dteKeiyakuYMDTo.Value.Value.Day
                    , 23
                    , 59
                    , 59);
            }
            else
            {
                para.EndYMD = DateTime.MaxValue;
            }

            //契約コード（あいまい検索）
            para.KeiyakuCodeAmbiguous = this.edtKeiyakuCode.Text;
            //契約名
            para.KeiyakuName = this.edtKeiyakuName.Text;

            //得意先
            para.TokuisakiId = Convert.ToDecimal(this.edtTokuisakiCd.Tag);
            //作業場所
            para.SagyoBashoId = Convert.ToDecimal(this.edtSagyoBashoCd.Tag);

            return para;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen(decimal id)
        {
            this.SetListMrow(id);
        }

        /// <summary>
        /// 画面コントローラのメンバに読み込んだ旅券売上情報を画面にセットします。
        /// </summary>
        private void SetListMrow(decimal id)
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            var list = this._KeiyakuInfoList
                        .OrderBy(x => x.KeiyakuCode)
                        .ToList();

            //使用停止表示フラグによってデータ再抽出
            IList<KeiyakuInfo> wk_list =
                this.chkAllFlag.Checked ? list : list.Where(x => !x.DisableFlag).ToList();

            try
            {
                //登録済みの一覧を初期化
                this.InitSheet();

                //件数取得
                int rowCount = wk_list.Count;

                //件数表示
                this.grpDetail.Text = "検索結果(" + rowCount.ToString() + "件)";

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを抜き出す
                var datamodel = sheet.Models.Data;
                //Spreadのスタイルモデルを抜き出す
                var stylemodel = sheet.Models.Style;

                datamodel.RowCount = rowCount;

                int row = 0;

                foreach (var item in wk_list)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLUM_LIST; k++)
                    {
                        //ヘッダーのスタイルを取得
                        StyleInfo sInfo = new StyleInfo(stylemodel.GetDirectInfo(-1, k, null));

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (item.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(row, k, sInfo);
                    }

                    //画面に値を設定していく
                    if (item.KeiyakuId == id && id != 0)
                    {
                        selectrowidx = row;
                    }

                    datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuId, item.KeiyakuId);
                    datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuCode, item.KeiyakuCode);

                    if (item.KeiyakuStartDate != DateTime.MinValue)
                    {
                        datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuStartYMD, item.KeiyakuStartDate);
                    }
                    else
                    {
                        datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuStartYMD, null);
                    }
                    if (item.KeiyakuEndDate != DateTime.MinValue)
                    {
                        datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuEndYMD, item.KeiyakuEndDate);
                    }
                    else
                    {
                        datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuEndYMD, null);
                    }

                    datamodel.SetValue(row, (int)SpreadColKeys.KeiyakuName, item.KeiyakuName);

                    datamodel.SetValue(row, (int)SpreadColKeys.TokuisakiCode, item.TokuisakiCode);
                    datamodel.SetValue(row, (int)SpreadColKeys.SagyoBashoCode, item.SagyoBashoCode);
                    datamodel.SetValue(row, (int)SpreadColKeys.TokuisakiName, item.TokuisakiName);
                    datamodel.SetValue(row, (int)SpreadColKeys.SagyoBashoName, item.SagyoBashoName);

                    datamodel.SetValue(row, (int)SpreadColKeys.SagyoDaiBunruiName, item.SagyoDaiBunruiName);
                    datamodel.SetValue(row, (int)SpreadColKeys.SagyoChuBunruiName, item.SagyoChuBunruiName);
                    datamodel.SetValue(row, (int)SpreadColKeys.SagyoShoBunruiName, item.SagyoShoBunruiName);

                    if (item.ShelterId != decimal.Zero)
                    { 
                        datamodel.SetValue(row, (int)SpreadColKeys.ShelterRenkeiFlg, "〇");
                    }
                    else
                    {
                        datamodel.SetValue(row, (int)SpreadColKeys.ShelterRenkeiFlg, string.Empty);
                    }

                    //タグに情報を保持しておく
                    datamodel.SetTag(row, (int)SpreadColKeys.KeiyakuId, item);

                    row++;

                }

                //選択行にフォーカスを合わせて選択状態にする
                sheet.SetActiveCell(selectrowidx, 0, true);
                sheet.AddSelection(selectrowidx, -1, 1, sheet.ColumnCount - 1);

                //フォーカスを移動
                this.fpListGrid.Focus();

                //選択行にスクロールバーを移動させる
                this.fpListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                //デフォルトに戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 検索条件項目をチェックします。
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

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            //フォーカス移動
            this.edtTokuisakiCd.Focus();
        }

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
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        /// <summary>
        /// データを取得し、画面上にセットします。
        /// </summary>
        private void DoGetData(decimal id = 0)
        {
            try
            {
                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
                {

                    //契約情報検索条件取得
                    KeiyakuSearchParameter para = this.GetSearchParameter();

                    // 画面コントローラに取得を指示
                    // 契約一覧取得
                    this._KeiyakuInfoList = this._Keiyaku.GetList(para).ToList();

                    // 0件なら処理しない
                    if (this._KeiyakuInfoList.Count == 0 && this._ReloadMessageFlg)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201015"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        this.edtTokuisakiCd.Focus();
                        this.isSearch = false;
                    }
                    else
                    {
                        //取得したデータを画面にセット
                        this.SetScreen(id);
                        this.isSearch = true;
                    }
                }
            }
            finally
            {
                //カーソを元に戻す
                this.Cursor = Cursors.Default;
            }
        }

        private void fpListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //セルをダブルクリックしたときのイベントハンドラ
            if (!e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //クリックイベントをキャンセルしてメンバに判断する値を持たせる
                e.Cancel = true;
                //ダブルクリックされた
                this.isDoubleClickedListSheet = true;
            }
        }

        private void fpListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            //登録済み一覧 - CellClick
            if (e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダ上の場合はその列の自動ソートを実行
                this.fpListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpListGrid_MouseUp(object sender, MouseEventArgs e)
        {
            //ダブルクリックされていれば...
            if (e.Button == MouseButtons.Left && this.isDoubleClickedListSheet)
            {
                //登録画面を開く
                this.ShowTorokuFrameAsSyuusei();
            }
            //フラグをクリア
            this.isDoubleClickedListSheet = false;
        }

        /// <summary>
        /// 登録画面を修正で表示します。
        /// </summary>
        private void ShowTorokuFrameAsSyuusei()
        {
            //選択している契約IDを取得
            decimal id = this.GetIdByListOnSelection();

            //フィルタ情報を保持
            this.GetFilterInfo();
            //一覧を再表示
            this.DoGetData(id);
            //フィルタ情報を復元
            this.SetFilterInfo();

            //登録画面を開く
            this.ShowMitsumoriFrame(id);
        }

        /// <summary>
        /// 処理モード、Idを指定して、契約入力画面を開きます。
        /// </summary>
        /// <param name="processmode">処理モード</param>
        /// <param name="id">Id</param>
        private void ShowMitsumoriFrame(decimal id = 0)
        {

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行の行Indexを取得
                int select_row = sheet0.GetSelection(0).Row;

                //選択行indexを設定
                this.selectrowidx = select_row;
            }

            KeiyakuTorokuFrame f = null;

            if (id.CompareTo(decimal.Zero) > 0)
            {
                f = new KeiyakuTorokuFrame(id);
            }
            else
            {
                f = new KeiyakuTorokuFrame();
            }

            f.InitFrame();
            f.KeiyakuIchiran = this;
            f.ShowDialog(this);

            //自身のMdiParentプロパティを入力画面のMdiParentプロパティに
            //セットする
            if (this.MdiParent != null)
            {
                ((Form)f).MdiParent = this.MdiParent;
            }
        }

        /// <summary>
        /// 一覧にて選択中のIDを取得します。
        /// 未選択の場合は"0"を返却します。
        /// </summary>
        private decimal GetIdByListOnSelection()
        {
            decimal rt_val = 0;

            //選択行の行Indexを取得
            int currowidx = this.GetListSelectionRowIndex();

            if (currowidx >= 0)
            {
                SheetView sheet0 = this.fpListGrid.Sheets[0];

                //選択行の契約IDを取得
                rt_val =
                    ((decimal)sheet0.Models.Data.GetValue(
                        sheet0.GetModelRowFromViewRow(currowidx),
                        (int)SpreadColKeys.KeiyakuId));
            }

            return rt_val;
        }

        /// <summary>
        /// 登録済みリストにて現在選択中の行のindexを取得します。
        /// 未選択の場合は-1を返却します。
        /// </summary>
        /// <returns>現在選択中の行</returns>
        private int GetListSelectionRowIndex()
        {
            //返却値
            int rt_val = -1;

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                rt_val = sheet0.GetSelection(0).Row;
            }

            return rt_val;
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
                // F5は共通検索画面
                case Keys.F5:
                    //キーボードイベントの抑止
                    e.Handled = true;

                    int rows = fpListGrid.ActiveSheet.Models.Selection.LeadRow - fpListGrid.ActiveSheet.Models.Selection.AnchorRow + 1;

                    if (this.fpListGrid.ContainsFocus && 1 == rows)
                    {
                        this.ShowTorokuFrameAsSyuusei();
                    }
                    else
                    {
                        this.ShowCmnSearch();
                    }
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
        /// 作業場所検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchSagyoBasho()
        {
            using (CmnSearchSagyoBashoFrame f = new CmnSearchSagyoBashoFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.SagyoBashoCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        #endregion

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



        private void JuchuIchiranFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void JuchuIchiranFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void JuchuIchiranFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            this.DoGetData();
        }

        /// <summary>
        /// フィルタ情報を保持します。
        /// </summary>
        private void GetFilterInfo()
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            // フィルタ設定の保存
            hrFilter = sheet.RowFilter as FarPoint.Win.Spread.HideRowFilter;
            filterStrings = FrameUtilites.GetFilterInfo(sheet);

            // ソート設定の保存
            sortIndicators = FrameUtilites.GetSortInfo(sheet);
        }

        /// <summary>
        /// フィルタ情報を復元します。
        /// </summary>
        private void SetFilterInfo()
        {
            //SheetView変数定義
            SheetView sheet = this.fpListGrid.ActiveSheet;

            // 保存したフィルタ設定の復元
            FrameUtilites.SetFilterInfo(sheet, filterStrings, hrFilter);

            // 保存したソート設定の復元
            FrameUtilites.SetSortInfo(sheet, sortIndicators);
        }

        /// <summary>
        /// 明細IDを元に明細部にフォーカスを設定します。
        /// </summary>
        private void setDtlFocus(decimal id, bool dtlFlg)
        {
            var sheet = this.fpListGrid.ActiveSheet;

            int row = 0;

            //ループしてモデルにセット
            for (int i = 0; i < this.fpListGrid.ActiveSheet.RowCount; i++)
            {
                var dtlId = Convert.ToDecimal(sheet.Models.Data.GetValue(sheet.GetModelRowFromViewRow(i), (int)SpreadColKeys.KeiyakuId));

                if (id == dtlId && dtlFlg)
                {
                    row = i;
                    break;
                }
            }

            //選択行にフォーカスを合わせて選択状態にする
            sheet.SetActiveCell(row, 0, true);
            sheet.AddSelection(row, -1, 1, sheet.ColumnCount - 1);

            //選択行にスクロールバーを移動させる
            this.fpListGrid.ShowActiveCell(
                FarPoint.Win.Spread.VerticalPosition.Top,
                FarPoint.Win.Spread.HorizontalPosition.Left);

        }

        /// <summary>
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcTextBox)this.ActiveControl).Text =
                        f.SelectedInfo.TokuisakiCode;

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 得意先コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void edtTokuisakiCd_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
            this.ValidateTokuisakiCd(e);
        }

        /// <summary>
        /// 得意先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (string.IsNullOrWhiteSpace(this.edtTokuisakiCd.Text))
                {
                    isClear = true;
                    return;
                }

                //得意先情報を取得
                TokuisakiSearchParameter para = new TokuisakiSearchParameter();
                para.TokuisakiCode = this.edtTokuisakiCd.Text;
                TokuisakiInfo info = _DalUtil.Tokuisaki.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.edtTokuisakiCd.Tag = info.TokuisakiId;
                    this.edtTokuisakiNm.Text = info.TokuisakiName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.edtTokuisakiCd.Tag = null;
                    this.edtTokuisakiCd.Text = string.Empty;
                    this.edtTokuisakiNm.Text = string.Empty;
                }
            }
        }


        /// <summary>
        /// 作業場所検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchSagyobasho()
        {
            using (CmnSearchSagyoBashoFrame f = new CmnSearchSagyoBashoFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcTextBox)this.ActiveControl).Text =
                        f.SelectedInfo.SagyoBashoCode;

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 作業場所コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void edtSagyoBashoCd_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
            this.ValidateSagyoBashoCd(e);
        }

        /// <summary>
        /// 作業場所コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSagyoBashoCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (string.IsNullOrWhiteSpace(this.edtSagyoBashoCd.Text))
                {
                    isClear = true;
                    return;
                }

                //得意先情報を取得
                SagyoBashoSearchParameter para = new SagyoBashoSearchParameter();
                para.SagyoBashoCode = this.edtSagyoBashoCd.Text;
                SagyoBashoInfo info = _DalUtil.SagyoBasho.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;
                }
                else
                {
                    this.edtSagyoBashoCd.Tag = info.SagyoBashoId;
                    this.edtSagyoBashoNm.Text = info.SagyoBashoName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.edtSagyoBashoCd.Tag = null;
                    this.edtSagyoBashoCd.Text = string.Empty;
                    this.edtSagyoBashoNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 日付区分のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbFilterDateKbns_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
        }

        /// <summary>
        /// 開始日のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteKeiyakuYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
        }

        /// <summary>
        /// 終了日のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteKeiyakuYMDTo_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
        }

        /// <summary>
        /// 契約コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void edtKeiyakuCode_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
        }

        /// <summary>
        /// 契約名のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void edtKeiyakuName_Validating(object sender, CancelEventArgs e)
        {
            this.InitSheet();
        }

    }
}
