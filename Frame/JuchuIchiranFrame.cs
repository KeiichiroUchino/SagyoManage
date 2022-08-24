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

namespace Jpsys.SagyoManage.Frame
{
    public partial class JuchuIchiranFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public JuchuIchiranFrame()
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
        private const string WINDOW_TITLE = "受注一覧";

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
        /// 受注一覧クラス
        /// </summary>
        private JuchuIchiran _JuchuIchiran;

        /// <summary>
        /// 検索した受注情報リストを保持する領域
        /// </summary>
        private List<JuchuIchiranInfo> _JuchuIchiranInfoList = new List<JuchuIchiranInfo>();

        #region MultiRow関連


        /// <summary>
        /// 明細の列名を表します。
        /// </summary>
        private enum MrowCellKeys
        {
            JuchuSlipNo,
            BranchOfficeNm,
            CarCd,
            LicPlateCarNo,
            CarKindNM,
            DriverNm,
            CarOfChartererNM,
            TokuisakiNM,
            ClmClassNM,
            ContractNM,
            TaskStartYMD,
            TaskStartHM,
            TaskEndYMD,
            TaskEndHM,
            ReuseYMD,
            ReuseHM,
            StartPointNM,
            EndPointNM,
            ItemNM,
            OwnerNM,
            JuchuTantoNm,
            HanroName,
            OfukuKbnName,
            Number,
            FigNM,
            AtPrice,
            Weight,
            PriceInPrice,
            TaxDispKbn,
            TollFeeInPrice,
            AddUpYMD,
            PriceInCharterPrice,
            CharterTaxDispKbn,
            TollFeeInCharterPrice,
            CharterAddUpYMD,
            Memo,
            FixFlag,
            CharterFixFlag,
            FutaigyomuryoInPrice,
            JomuinUriageDogakuFlag,
            JomuinUriageKingaku,
            MagoYoshasaki,
            ReceivedFlag,
            ExistsHaishaFlag,
            ExistsRenkeiFlag,
        }
        #endregion

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


        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

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

            //受注一覧クラスインスタンス作成
            this._JuchuIchiran = new JuchuIchiran(this.appAuth);

            //検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numCarCd, this.ShowCmnSearchCar},
                {this.numDriverCd, this.ShowCmnSearchDriver},
                {this.numCarOfChartererCd, this.ShowCmnSearchTorihikisaki},
                {this.numTokuisakiCd, this.ShowCmnSearchTokuisaki},
                {this.numClmClassCd, this.ShowCmnSearchClmClass},
                {this.numContractCd, this.ShowCmnSearchContract},
                {this.numHanroCd, this.ShowCmnSearchHanro},
                {this.numJuchuTantoCd, this.ShowCmnSearchJuchuTantosha},
            };

            //コンボボックスの初期化
            this.InitCombo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

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
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.New);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 画面検索条件項目をクリアします。
        /// </summary>
        private void ClearSearchInputs()
        {
            this.dteTaishoYMDFrom.Value = DateTime.Today.AddMonths(-1);
            this.dteTaishoYMDTo.Value = DateTime.Today;
            this.numTokuisakiCd.Tag = null;
            this.numTokuisakiCd.Value = null;
            this.edtTokuisakiNm.Text = string.Empty;
            this.cmbBranchOffice.SelectedValue = null;
            this.cmbBranchOffice.Tag = null;
            this.numJuchuSlipNoFrom.Value = null;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {

            this.cmbFilterDateKbns.SelectedIndex = 0;
            DateTime today = DateTime.Today; ;
            this.dteTaishoYMDFrom.Value = new DateTime(today.Year, today.Month, 1);
            this.dteTaishoYMDTo.Value = NSKUtil.MonthLastDay(today);
            this.numCarCd.Value = 0;
            this.numCarCd.Tag = null;
            this.edtLicPlateCarNo.Text = string.Empty;
            this.edtCarKindNM.Text = string.Empty;
            this.numDriverCd.Value = 0;
            this.numDriverCd.Tag = null;
            this.edtDriverNM.Text = string.Empty;
            this.numCarOfChartererCd.Value = 0;
            this.numCarOfChartererCd.Tag = null;
            this.edtCarOfChartererNm.Text = string.Empty;
            this.numTokuisakiCd.Value = 0;
            this.numTokuisakiCd.Tag = null;
            this.edtTokuisakiNm.Text = string.Empty;
            this.numClmClassCd.Value = 0;
            this.numClmClassCd.Tag = null;
            this.edtClmClassNm.Text = string.Empty;
            this.numContractCd.Value = 0;
            this.numContractCd.Tag = null;
            this.edtContractNm.Text = string.Empty;
            this.numHanroCd.Value = 0;
            this.numHanroCd.Tag = null;
            this.edtHanroNm.Text = string.Empty;
            this.numJuchuTantoCd.Value = 0;
            this.numJuchuTantoCd.Tag = null;
            this.edtJuchuTantoNm.Text = string.Empty;
            this.numJuchuSlipNoFrom.Value = 0;
            this.numJuchuSlipNoTo.Value = 0;
            this.chkMiRenkeiNomiFlag.Checked = false;
            this.chkMiHaishaNomiFlag.Checked = false;


            //売上明細の初期化
            this.InitMrow();

            //メンバをクリア
            this.isConfirmClose = true;
        }

        /// <summary>
        /// Mrow関連を初期化します。
        /// </summary>
        private void InitMrow()
        {
            this.InitJuchuNyuryokuMeisaiListMrow();
        }

        /// <summary>
        /// 売上明細のMrowを初期化します。
        /// </summary>
        private void InitJuchuNyuryokuMeisaiListMrow()
        {
            //描画を停止
            this.mrsJuchuList.SuspendLayout();

            try
            {
                //初期値を設定
                //Mrowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsJuchuList.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                // トラDON_V40の場合
                if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                    == (int)DefaultProperty.TraDonVersionKbn.V40)
                {
                    // 金額_附帯業務料ヘッダーラベル（非表示）
                    tpl.ColumnHeaders[0].Cells["clmHeaderCellFutaigyomuryoInPrice"].Visible = false;
                    // 空白（位置／サイズ変更）
                    tpl.ColumnHeaders[0].Cells["clmHeaderCellBlank"].Location = new System.Drawing.Point(
                        tpl.ColumnHeaders[0].Cells["clmHeaderCellFutaigyomuryoInPrice"].Location.X,
                        tpl.ColumnHeaders[0].Cells["clmHeaderCellBlank"].Location.Y);
                    tpl.ColumnHeaders[0].Cells["clmHeaderCellBlank"].Size = new Size(
                        tpl.ColumnHeaders[0].Cells["clmHeaderCellBlank"].Size.Width + tpl.ColumnHeaders[0].Cells["clmHeaderCellFutaigyomuryoInPrice"].Size.Width,
                        tpl.ColumnHeaders[0].Cells["clmHeaderCellBlank"].Size.Height);

                    // 金額_附帯業務料（非表示）
                    tpl.Row.Cells[MrowCellKeys.FutaigyomuryoInPrice.ToString()].Visible = false;
                    // 空白（位置／サイズ変更）
                    tpl.Row.Cells["Blank"].Location = new System.Drawing.Point(
                        tpl.Row.Cells[MrowCellKeys.FutaigyomuryoInPrice.ToString()].Location.X,
                        tpl.Row.Cells["Blank"].Location.Y);
                    tpl.Row.Cells["Blank"].Size = new Size(
                        tpl.Row.Cells["Blank"].Size.Width + tpl.Row.Cells[MrowCellKeys.FutaigyomuryoInPrice.ToString()].Size.Width,
                        tpl.Row.Cells["Blank"].Size.Height);
                }

                //***値の初期化
                TemplateInitializer initializer = new TemplateInitializer(mrsJuchuList);
                initializer.Initialize();

                //テンプレートを再設定
                this.mrsJuchuList.Template = tpl;

                //***ショートカットキー
                //基本設定
                this.mrsJuchuList.InitialShortcutKeySetting();

                //--上キー制御を変更（前の行へ移動）
                this.mrsJuchuList.ShortcutKeyManager.Unregister(Keys.Up);
                this.mrsJuchuList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousRow, Keys.Up);

                //--下キー制御を変更（次の行へ移動）
                this.mrsJuchuList.ShortcutKeyManager.Unregister(Keys.Down);
                this.mrsJuchuList.ShortcutKeyManager.Register(SelectionActions.MoveToNextRow, Keys.Down);

                //--左キー制御を変更（前のセルへ移動 ※行の折り返しあり）
                this.mrsJuchuList.ShortcutKeyManager.Unregister(Keys.Left);
                this.mrsJuchuList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCell, Keys.Left);

                //--右キー制御を変更（次のセルへ移動 ※行の折り返しあり）
                this.mrsJuchuList.ShortcutKeyManager.Unregister(Keys.Right);
                this.mrsJuchuList.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Right);

                //行をクリア
                this.mrsJuchuList.Rows.Clear();

                //--交互行でスタイルを変更する
                this.mrsJuchuList.AlternatingRowsDefaultCellStyle.BackColor =
                    FrameUtilites.GetColorByRgbString(DefaultProperty.FRAME_GRIDSELECTION_DEFAULT_BACKCOLOR);
            }
            finally
            {
                this.mrsJuchuList.ResumeLayout();
            }
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCd, ctl => ctl.Text, this.numCarCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numDriverCd, ctl => ctl.Text, this.numDriverCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarOfChartererCd, ctl => ctl.Text, this.numCarOfChartererCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTokuisakiCd, ctl => ctl.Text, this.numTokuisakiCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numClmClassCd, ctl => ctl.Text, this.numClmClassCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numContractCd, ctl => ctl.Text, this.numContractCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHanroCd, ctl => ctl.Text, this.numHanroCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numJuchuTantoCd, ctl => ctl.Text, this.numJuchuTantoCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbBranchOffice, ctl => ctl.Text, this.cmbBranchOffice_Validating));

        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);
            this.searchStateBinder.AddSearchableControls(
                this.numCarCd, this.numDriverCd, this.numCarOfChartererCd, this.numTokuisakiCd, this.numJuchuTantoCd, this.numClmClassCd, this.numContractCd, this.numHanroCd);

            string[] SearchCells = new string[] { MrowCellKeys.JuchuSlipNo.ToString(),
                MrowCellKeys.BranchOfficeNm.ToString(),
                MrowCellKeys.CarCd.ToString(),
                MrowCellKeys.LicPlateCarNo.ToString(),
                MrowCellKeys.CarKindNM.ToString(),
                MrowCellKeys.DriverNm.ToString(),
                MrowCellKeys.CarOfChartererNM.ToString(),
                MrowCellKeys.TokuisakiNM.ToString(),
                MrowCellKeys.ClmClassNM.ToString(),
                MrowCellKeys.ContractNM.ToString(),
                MrowCellKeys.TaskStartYMD.ToString(),
                MrowCellKeys.TaskStartHM.ToString(),
                MrowCellKeys.TaskEndYMD.ToString(),
                MrowCellKeys.TaskEndHM.ToString(),
                MrowCellKeys.ReuseYMD.ToString(),
                MrowCellKeys.ReuseHM.ToString(),
                MrowCellKeys.StartPointNM.ToString(),
                MrowCellKeys.EndPointNM.ToString(),
                MrowCellKeys.ItemNM.ToString(),
                MrowCellKeys.OwnerNM.ToString(),
                MrowCellKeys.JuchuTantoNm.ToString(),
                MrowCellKeys.HanroName.ToString(),
                MrowCellKeys.OfukuKbnName.ToString(),
                MrowCellKeys.Number.ToString(),
                MrowCellKeys.FigNM.ToString(),
                MrowCellKeys.AtPrice.ToString(),
                MrowCellKeys.Weight.ToString(),
                MrowCellKeys.PriceInPrice.ToString(),
                MrowCellKeys.TaxDispKbn.ToString(),
                MrowCellKeys.TollFeeInPrice.ToString(),
                MrowCellKeys.AddUpYMD.ToString(),
                MrowCellKeys.PriceInCharterPrice.ToString(),
                MrowCellKeys.CharterTaxDispKbn.ToString(),
                MrowCellKeys.TollFeeInCharterPrice.ToString(),
                MrowCellKeys.CharterAddUpYMD.ToString(),
                MrowCellKeys.Memo.ToString(),
                MrowCellKeys.FixFlag.ToString(),
                MrowCellKeys.CharterFixFlag.ToString(),
                MrowCellKeys.FutaigyomuryoInPrice.ToString(),
                MrowCellKeys.JomuinUriageDogakuFlag.ToString(),
                MrowCellKeys.JomuinUriageKingaku.ToString(),
                MrowCellKeys.MagoYoshasaki.ToString(),
                MrowCellKeys.ReceivedFlag.ToString(),
                MrowCellKeys.ExistsHaishaFlag.ToString()};

            this.searchStateBinder.AddSearchableControls(this.mrsJuchuList, SearchCells);

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        #endregion

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitNyukinMeisaiKbnCombo();
            this.InitBranchOfficeCombo();
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
                value = item.SystemNameCode.ToString() + " " + item.SystemName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbFilterDateKbns, datasource, false, null, false);
            this.cmbFilterDateKbns.SelectedIndex = 0;
        }

        /// <summary>
        /// 営業所コンボボックスを初期化します。
        /// </summary>
        private void InitBranchOfficeCombo()
        {
            //// 営業所コンボ設定
            //Dictionary<int, String> datasource = new Dictionary<int, String>();

            //IList<ToraDONBranchOfficeInfo> list = this._DalUtil.ToraDONBranchOffice.GetList(null);

            //if (list != null && 0 < list.Count())
            //{
            //    list = list
            //        .OrderBy(x => x.BranchOfficeCode)
            //        .ToList();
            //}
            //else
            //{
            //    list = new List<ToraDONBranchOfficeInfo>();
            //}

            //int key = 0;
            //String value = "";

            //foreach (ToraDONBranchOfficeInfo item in list)
            //{
            //    key = item.BranchOfficeCode;
            //    value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

            //    datasource.Add(key, value);
            //}

            //FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, datasource, true, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);
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
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        void New_Execute(object sender, EventArgs e)
        {
            this.DoStartEntry();
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
        /// 受注情報一覧の検索条件を取得します
        /// </summary>
        /// <returns>受注情報一覧の検索条件</returns>
        private JuchuIchiranSearchParameter GetJuchuIchiranSearchParameter()
        {
            JuchuIchiranSearchParameter para = new JuchuIchiranSearchParameter();

            // 日付指定区分
            para.FilterDateKbns = Convert.ToInt32(this.cmbFilterDateKbns.SelectedValue);
            // 対象日付（From）
            para.TaishoYMDFrom = Convert.ToDateTime(this.dteTaishoYMDFrom.Value);
            // 対象日付（To）
            para.TaishoYMDTo = new DateTime(
                this.dteTaishoYMDTo.Value.Value.Year
                , this.dteTaishoYMDTo.Value.Value.Month
                , this.dteTaishoYMDTo.Value.Value.Day
                , 23
                , 59
                , 59);

            // 車両ID
            para.CarId = Convert.ToDecimal(this.numCarCd.Tag);
            // 乗務員ID
            para.DriverId = Convert.ToDecimal(this.numDriverCd.Tag);
            // 傭車先ID
            para.CarOfChartererId = Convert.ToDecimal(this.numCarOfChartererCd.Tag);
            // 得意先ID
            para.TokuisakiId = Convert.ToDecimal(this.numTokuisakiCd.Tag);
            // 請求部門ID
            para.ClmClassId = Convert.ToDecimal(this.numClmClassCd.Tag);
            // 請負ID
            para.ContractId = Convert.ToDecimal(this.numContractCd.Tag);
            // 販路ID
            para.HanroId = Convert.ToDecimal(this.numHanroCd.Tag);
            // 受注担当ID
            para.JuchuTantoId = Convert.ToDecimal(this.numJuchuTantoCd.Tag);
            // 営業所ID
            para.BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag);
            // 伝票№（From）
            para.JuchuSlipNoFrom = Convert.ToDecimal(this.numJuchuSlipNoFrom.Value);
            // 伝票№（To）
            para.JuchuSlipNoTo = Convert.ToDecimal(this.numJuchuSlipNoTo.Value);
            // 未配車のみフラグ
            para.MiHaishaNomiFlag = this.chkMiHaishaNomiFlag.Checked;
            // 未連携のみフラグ
            para.MiRenkeiNomiFlag = this.chkMiRenkeiNomiFlag.Checked;

            return para;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            this.SetJuchuListMrow();
        }

        /// <summary>
        /// 画面コントローラのメンバに読み込んだ旅券売上情報を画面にセットします。
        /// </summary>
        private void SetJuchuListMrow()
        {
            //MultiRowの描画を停止
            this.mrsJuchuList.SuspendLayout();

            try
            {
                //明細をクリア
                this.mrsJuchuList.Rows.Clear();

                if (this._JuchuIchiranInfoList == null
                    || this._JuchuIchiranInfoList.Count == 0)
                {
                    return;
                }

                //件数取得
                int rowcount = this._JuchuIchiranInfoList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //明細の行数を設定（新規行なし）
                this.mrsJuchuList.RowCount = rowcount;

                for (int i = 0; i < rowcount; i++)
                {
                    JuchuIchiranInfo info = this._JuchuIchiranInfoList[i];

                    //Mrow取得
                    GrapeCity.Win.MultiRow.Row row = this.mrsJuchuList.Rows[i];

                    // 伝票№
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.JuchuSlipNo.ToString(), info.JuchuSlipNo);
                    // 営業所名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.BranchOfficeNm.ToString(), info.BranchOfficeSNM);
                    // 車両コード
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.CarCd.ToString(), info.CarCd);
                    // 車番
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.LicPlateCarNo.ToString(), info.LicPlateCarNo);
                    // 車種名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.CarKindNM.ToString(), info.CarKindSNM);
                    // 乗務員名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.DriverNm.ToString(), info.DriverNm);
                    // 得意先名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.TokuisakiNM.ToString(), info.TokuisakiNM);
                    // 請求部門名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ClmClassNM.ToString(), info.ClmClassNM);
                    // 請負名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ContractNM.ToString(), info.ContractNM);

                    // 積日
                    if (info.TaskStartDateTime != DateTime.MinValue)
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskStartYMD.ToString(), info.TaskStartDateTime);
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskStartHM.ToString(), info.TaskStartDateTime);
                    }
                    else
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskStartYMD.ToString(), null);
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskStartHM.ToString(), null);
                    }

                    // 着日
                    if (info.TaskEndDateTime != DateTime.MinValue)
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskEndYMD.ToString(), info.TaskEndDateTime);
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskEndHM.ToString(), info.TaskEndDateTime);
                    }
                    else
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskEndYMD.ToString(), null);   
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.TaskEndHM.ToString(), null);
                    }

                    // 再使用可能日
                    if (info.ReuseYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.ReuseYMD.ToString(), info.ReuseYMD);
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.ReuseHM.ToString(), info.ReuseYMD);
                    }
                    else
                    {
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.ReuseYMD.ToString(), null);
                        this.mrsJuchuList.SetValue(i, MrowCellKeys.ReuseHM.ToString(), null);
                    }


                    // 積地
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.StartPointNM.ToString(), info.StartPointNM);
                    // 着地
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.EndPointNM.ToString(), info.EndPointNM);
                    // 品目名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ItemNM.ToString(), info.ItemNM);
                    // 荷主名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.OwnerNM.ToString(), info.OwnerNM);
                    // 受注担当名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.JuchuTantoNm.ToString(), info.JuchuTantoNm);
                    // 販路名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.HanroName.ToString(), info.HanroNm);
                    // 往復区分名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.OfukuKbnName.ToString(), info.OfukuKbnNm);
                    // 傭車先名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.CarOfChartererNM.ToString(), info.CarOfChartererShortNm);
                    // 数量
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.Number.ToString(), info.Number);
                    // 単位名
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.FigNM.ToString(), info.FigNm);
                    // 単価
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.AtPrice.ToString(), info.AtPrice);
                    // 重量
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.Weight.ToString(), info.Weight);
                    // 売上金額
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.PriceInPrice.ToString(), info.PriceInPrice);
                    // 税区分
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.TaxDispKbn.ToString(), info.TaxDispKbnNm);
                    // 金額_通行料
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.TollFeeInPrice.ToString(), info.TollFeeInPrice);
                    // 計上日
                    if (info.AddUpYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuList.SetValue(
                            i, MrowCellKeys.AddUpYMD.ToString(), info.AddUpYMD);
                    }
                    // 傭車金額
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.PriceInCharterPrice.ToString(), info.PriceInCharterPrice);
                    // 傭車税区分
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.CharterTaxDispKbn.ToString(), info.CharterTaxDispKbnNm);
                    // 傭車金額_通行料
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.TollFeeInCharterPrice.ToString(), info.TollFeeInCharterPrice);
                    // 傭車計上日
                    if (info.CharterAddUpYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuList.SetValue(
                            i, MrowCellKeys.CharterAddUpYMD.ToString(), info.CharterAddUpYMD);
                    }
                    // 確定フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.FixFlag.ToString(), info.FixFlag);
                    // 傭車確定フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.CharterFixFlag.ToString(), info.CharterFixFlag);
                    // 備考
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.Memo.ToString(), info.Memo);
                    // 金額_附帯業務料
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.FutaigyomuryoInPrice.ToString(), info.FutaigyomuryoInPrice);
                    // 乗務員売上同額フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.JomuinUriageDogakuFlag.ToString(), info.JomuinUriageDogakuFlag);
                    // 乗務員売上金額
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.JomuinUriageKingaku.ToString(), info.JomuinUriageKingaku);
                    // 孫傭車先
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.MagoYoshasaki.ToString(), info.MagoYoshasaki);
                    // 確定フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ReceivedFlag.ToString(), info.ReceivedFlag);
                    // 配車存在フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ExistsHaishaFlag.ToString(), info.ExistsHaishaFlag);
                    // 連携存在フラグ
                    this.mrsJuchuList.SetValue(
                        i, MrowCellKeys.ExistsRenkeiFlag.ToString(), info.ExistsRenkeiFlag);

                    //１件分の受注情報をMRowのTagにセットしておく
                    this.mrsJuchuList.Rows[i].Tag = info;

                }
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsJuchuList.ResumeLayout();
            }
        }

        /// <summary>
        /// ログに出力する文字を作成します。
        /// </summary>
        /// <returns></returns>
        private string CreateLogText()
        {
            string result = string.Empty;

            result =
                this.IdsString + "\r\n" +
                ""
                ;

            if (255 < result.Length)
            {
                result = result.Substring(255);
            }

            return result;
        }

        /// <summary>
        /// 伝票番号をカンマ区切りで返却します。
        /// </summary>
        private String IdsString
        {
            get
            {
                string rtval = string.Empty;

                int i = 0;

                foreach (JuchuIchiranInfo info in this._JuchuIchiranInfoList)
                {
                    if (0 < i++)
                    {
                        rtval = rtval + ",";
                    }

                    rtval = rtval + info.JuchuId;
                }

                return string.Format("[伝票番号] {0}", rtval);
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

            //日付（範囲開始）の必須入力チェック
            if (rt_val && DateTime.MinValue == Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "日付（範囲開始）" });
                ctl = this.dteTaishoYMDFrom;
            }

            //日付（範囲終了）の必須入力チェック
            if (rt_val && DateTime.MinValue == Convert.ToDateTime(this.dteTaishoYMDTo.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "日付（範囲終了）" });
                ctl = this.dteTaishoYMDTo;
            }

            //日付の範囲チェック
            if (rt_val && Convert.ToDateTime(this.dteTaishoYMDTo.Value) < Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2202018", new string[] { "日付" });
                ctl = this.dteTaishoYMDTo;
            }


            //伝票№の範囲チェック
            decimal JuchuSlipNoFrom = Convert.ToDecimal(this.numJuchuSlipNoFrom.Value);
            decimal JuchuSlipNoTo = Convert.ToDecimal(this.numJuchuSlipNoTo.Value);

            //どちらも入力されている場合にのみチェックを行う
            if ((0 != JuchuSlipNoFrom) && (0 != JuchuSlipNoTo))
            {
                if (rt_val && Convert.ToDecimal(this.numJuchuSlipNoTo.Value) < Convert.ToDecimal(this.numJuchuSlipNoFrom.Value))
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "伝票№" });
                    ctl = this.numJuchuSlipNoTo;
                }
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
            this.dteTaishoYMDFrom.Focus();
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
        private void DoGetData()
        {
            try
            {
                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
                {
                    //受注情報検索条件取得
                    JuchuIchiranSearchParameter para = this.GetJuchuIchiranSearchParameter();

                    // 画面コントローラに取得を指示
                    // 受注一覧取得
                    this._JuchuIchiranInfoList = this._JuchuIchiran.GetJuchuInfoList(para);

                    // 0件なら処理しない
                    if (this._JuchuIchiranInfoList.Count == 0)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201015"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        this.dteTaishoYMDFrom.Focus();
                    }
                    else
                    {
                        //取得したデータを画面にセット
                        this.SetScreen();
                    }
                }
            }
            finally
            {
                //カーソを元に戻す
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 受注番号を指定して登録画面を表示します。
        /// </summary>
        private void DoUpdateEntry()
        {
            // 選択行のTagから受注情報を取得
            JuchuIchiranInfo currentRowInfo = (JuchuIchiranInfo)mrsJuchuList.CurrentRow.Tag;

            //再検索フラグ
            Boolean SearchFlg = false;

            using (JuchuNyuryokuFrame f = new JuchuNyuryokuFrame(currentRowInfo.JuchuSlipNo, currentRowInfo.JuchuId))
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    SearchFlg = true;
                }
            }

            //「更新」「削除」が行われた場合には明細を再検索する。。
            if (SearchFlg)
            {
                this.DoGetData();
            }
        }

        /// <summary>
        /// 登録画面を表示します。
        /// </summary>
        private void DoStartEntry()
        {
            JuchuNyuryokuFrame f = new JuchuNyuryokuFrame();
            f.InitFrame();
            f.Show();
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

                    if (this.mrsJuchuList.ContainsFocus && 1 == this.mrsJuchuList.SelectedRows.Count)
                    {
                        this.DoUpdateEntry();
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
        /// 車両検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCar()
        {
            using (CmnSearchCarFrame f = new CmnSearchCarFrame())
            {
                ////パラメータをセット
                //f.ShowAllFlag = false;

                //f.InitFrame();
                //f.ShowDialog(this);
                //if (f.DialogResult == DialogResult.OK)
                //{
                //    //画面から値を取得
                //    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                //        Convert.ToInt32(f.SelectedInfo.ToraDONCarCode);

                //    this.OnCmnSearchComplete();
                //}
            }
        }

        /// <summary>
        /// 運転者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchDriver()
        {
            //using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
            //{
            //    //パラメータをセット
            //    f.ShowAllFlag = false;

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        /// <summary>
        /// 受注担当者検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchJuchuTantosha()
        {
            //using (CmnSearchJuchuTantoshaFrame f = new CmnSearchJuchuTantoshaFrame())
            //{
            //    //パラメータをセット
            //    f.ShowAllFlag = false;

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        /// <summary>
        /// 取引先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTorihikisaki()
        {
            //using (CmnSearchTorihikisakiFrame f = new CmnSearchTorihikisakiFrame())
            //{
            //    //パラメータをセット
            //    f.ShowAllFlag = false;
            //    f.ShowYoshaFlag = true;

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.ToraDONTorihikiCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        /// <summary>
        /// 得意先検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
            {
                ////パラメータをセット
                //f.ShowAllFlag = false;

                //f.InitFrame();
                //f.ShowDialog(this);
                //if (f.DialogResult == DialogResult.OK)
                //{
                //    //画面から値を取得
                //    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                //        Convert.ToInt32(f.SelectedInfo.ToraDONTokuisakiCode);

                //    this.OnCmnSearchComplete();
                //}
            }
        }

        /// <summary>
        /// 請求部門検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchClmClass()
        {
            //using (CmnSearchClmClassFrame f = new CmnSearchClmClassFrame())
            //{
            //    //パラメータをセット
            //    f.ShowAllFlag = false;

            //    f.ParamTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.ClmClassCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        /// <summary>
        /// 請負検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchContract()
        {
            //using (CmnSearchContractFrame f = new CmnSearchContractFrame())
            //{
            //    //パラメータをセット
            //    f.ParamTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.ContractCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        /// <summary>
        /// 販路検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHanro()
        {
            //using (CmnSearchHanroFrame f = new CmnSearchHanroFrame())
            //{
            //    //パラメータをセット
            //    f.ShowAllFlag = false;

            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.HanroCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                ////コードの入力が無い場合は抜ける
                //if (0 == this.numCarCd.Value)
                //{
                //    isClear = true;
                //    return;
                //}

                //// 車両情報を取得
                //CarSearchParameter para = new CarSearchParameter();
                //para.ToraDONCarCode = Convert.ToInt32(this.numCarCd.Value);
                //CarInfo info = _DalUtil.Car.GetList(para, null).FirstOrDefault();

                //if (null == info)
                //{
                //    MessageBox.Show(
                //        FrameUtilites.GetDefineMessage("MW2201003"),
                //        this.Text,
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);

                //    isClear = true;
                //    //イベントをキャンセル
                //    e.Cancel = true;
                //}
                //else
                //{
                //    this.numCarCd.Tag = info.ToraDONCarId;
                //    this.numCarCd.Value = info.ToraDONCarCode;
                //    this.edtLicPlateCarNo.Text = info.LicPlateCarNo;
                //    this.edtCarKindNM.Text = info.CarKindName;
                //}
            }
            finally
            {
                if (isClear)
                {
                    this.numCarCd.Tag = null;
                    this.numCarCd.Value = null;
                    this.edtLicPlateCarNo.Text = string.Empty;
                    this.edtCarKindNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 乗務員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateDriverCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                ////コードの入力が無い場合は抜ける
                //if (0 == this.numDriverCd.Value)
                //{
                //    isClear = true;
                //    return;
                //}

                //// 乗務員情報を取得
                //StaffSearchParameter para = new StaffSearchParameter();
                //para.ToraDONStaffCode = Convert.ToInt32(this.numDriverCd.Value);
                //StaffInfo info = _DalUtil.Staff.GetList(para, null).FirstOrDefault();

                //if (null == info)
                //{
                //    MessageBox.Show(
                //        FrameUtilites.GetDefineMessage("MW2201003"),
                //        this.Text,
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);

                //    isClear = true;
                //    //イベントをキャンセル
                //    e.Cancel = true;
                //}
                //else
                //{
                //    this.numDriverCd.Tag = info.ToraDONStaffId;
                //    this.numDriverCd.Value = info.ToraDONStaffCode;
                //    this.edtDriverNM.Text = info.ToraDONStaffName;
                //}
            }
            finally
            {
                if (isClear)
                {
                    this.numDriverCd.Tag = null;
                    this.numDriverCd.Value = null;
                    this.edtDriverNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 傭車先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                ////コードの入力が場合は抜ける
                //if (0 == this.numCarOfChartererCd.Value)
                //{
                //    isClear = true;
                //    return;
                //}

                //// 取引先情報を取得
                //TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
                //para.ToraDONTorihikiCode = Convert.ToInt32(this.numCarOfChartererCd.Value);
                //TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

                //if (null == info)
                //{
                //    MessageBox.Show(
                //        FrameUtilites.GetDefineMessage("MW2201003"),
                //        this.Text,
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);

                //    isClear = true;
                //    //イベントをキャンセル
                //    e.Cancel = true;
                //}
                //else
                //{
                //    this.numCarOfChartererCd.Tag = info.ToraDONTorihikiId;
                //    this.numCarOfChartererCd.Value = info.ToraDONTorihikiCode;
                //    this.edtCarOfChartererNm.Text = info.ToraDONTorihikiName;
                //}
            }
            finally
            {
                if (isClear)
                {
                    this.numCarOfChartererCd.Tag = null;
                    this.numCarOfChartererCd.Value = null;
                    this.edtCarOfChartererNm.Text = string.Empty;
                }
            }
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
                ////コードの入力が無い場合は抜ける
                //if (0 == this.numTokuisakiCd.Value)
                //{
                //    isClear = true;
                //    return;
                //}

                //// 得意先情報を取得
                //TokuisakiSearchParameter para = new TokuisakiSearchParameter();
                //para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
                //TokuisakiInfo info = _DalUtil.Tokuisaki.GetList(para).FirstOrDefault();

                //if (null == info)
                //{
                //    MessageBox.Show(
                //        FrameUtilites.GetDefineMessage("MW2201003"),
                //        this.Text,
                //        MessageBoxButtons.OK,
                //        MessageBoxIcon.Warning);

                //    isClear = true;
                //    //イベントをキャンセル
                //    e.Cancel = true;
                //}
                //else
                //{
                //    this.numTokuisakiCd.Tag = info.ToraDONTokuisakiId;
                //    this.numTokuisakiCd.Value = info.ToraDONTokuisakiCode;
                //    this.edtTokuisakiNm.Text = info.ToraDONTokuisakiShortName;

                //    //関連をクリア
                //    this.numClmClassCd.Tag = null;
                //    this.numClmClassCd.Value = null;
                //    this.edtClmClassNm.Text = string.Empty;
                //    this.numContractCd.Tag = null;
                //    this.numContractCd.Value = null;
                //    this.edtContractNm.Text = string.Empty;
                //}
            }
            finally
            {
                if (isClear)
                {
                    this.numTokuisakiCd.Tag = null;
                    this.numTokuisakiCd.Value = null;
                    this.edtTokuisakiNm.Text = string.Empty;
                    this.numClmClassCd.Tag = null;
                    this.numClmClassCd.Value = null;
                    this.edtClmClassNm.Text = string.Empty;
                    this.numContractCd.Tag = null;
                    this.numContractCd.Value = null;
                    this.edtContractNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 請求部門コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateClmClassCd(CancelEventArgs e)
        {
            bool isClear = false;

            //try
            //{
            //    //コードの入力が無い場合は抜ける
            //    if (0 == this.numClmClassCd.Value)
            //    {
            //        isClear = true;
            //        return;
            //    }

            //    // 請求部門情報を取得
            //    ToraDONClmClassSearchParameter para = new ToraDONClmClassSearchParameter();
            //    para.ClmClassCode = Convert.ToInt32(this.numClmClassCd.Value);
            //    ToraDONClmClassInfo info = _DalUtil.ToraDONClmClass.GetList(para).FirstOrDefault();

            //    if (null == info)
            //    {
            //        MessageBox.Show(
            //            FrameUtilites.GetDefineMessage("MW2201003"),
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        isClear = true;
            //        //イベントをキャンセル
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        ////入力中の得意先
            //        //TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            //        //tokui_para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
            //        //TokuisakiInfo tokuisakiInfo = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //        ////請求部門の得意先と入力されている得意先が不一致か？
            //        //if (tokuisakiInfo != null && (info.TokuisakiId != 0 && info.TokuisakiId != tokuisakiInfo.ToraDONTokuisakiId))
            //        //{
            //        //    //編集をキャンセル
            //        //    isClear = true;
            //        //    //イベントをキャンセル
            //        //    e.Cancel = true;

            //        //    MessageBox.Show(
            //        //    FrameUtilites.GetDefineMessage("MW2203076"),
            //        //    "警告",
            //        //    MessageBoxButtons.OK,
            //        //    MessageBoxIcon.Warning);
            //        //}

            //        //this.numClmClassCd.Tag = info.ClmClassId;
            //        //this.numClmClassCd.Value = info.ClmClassCode;
            //        //this.edtClmClassNm.Text = info.ClmClassName;

            //        ////関連をクリア
            //        //this.numContractCd.Tag = null;
            //        //this.numContractCd.Value = null;
            //        //this.edtContractNm.Text = string.Empty;
            //    }
            //}
            //finally
            //{
            //    if (isClear)
            //    {
            //        this.numClmClassCd.Tag = null;
            //        this.numClmClassCd.Value = null;
            //        this.edtClmClassNm.Text = string.Empty;
            //        this.numContractCd.Tag = null;
            //        this.numContractCd.Value = null;
            //        this.edtContractNm.Text = string.Empty;
            //    }
            //}
        
        }

        /// <summary>
        /// 請負コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateContractCd(CancelEventArgs e)
        {
            bool isClear = false;

            //try
            //{
            //    //コードの入力が無い場合は抜ける
            //    if (0 == this.numContractCd.Value)
            //    {
            //        isClear = true;
            //        return;
            //    }

            //    // 請負情報を取得
            //    ToraDONContractSearchParameter para = new ToraDONContractSearchParameter();
            //    para.ContractCode = Convert.ToInt32(this.numContractCd.Value);
            //    ToraDONContractInfo info = _DalUtil.ToraDONContract.GetList(para).FirstOrDefault();

            //    if (null == info)
            //    {
            //        MessageBox.Show(
            //            FrameUtilites.GetDefineMessage("MW2201003"),
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        isClear = true;
            //        //イベントをキャンセル
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        ////入力中の得意先
            //        //TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            //        //tokui_para.ToraDONTokuisakiCode = Convert.ToInt32(this.numTokuisakiCd.Value);
            //        //TokuisakiInfo tokuisakiInfo = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //        //// 請求部門情報を取得
            //        //ToraDONClmClassSearchParameter clm_para = new ToraDONClmClassSearchParameter();
            //        //clm_para.ClmClassCode = Convert.ToInt32(this.numClmClassCd.Value);
            //        //ToraDONClmClassInfo clmclassInfo = _DalUtil.ToraDONClmClass.GetList(clm_para).FirstOrDefault();

            //        ////請求部門の得意先と入力されている得意先が不一致か？
            //        //if (tokuisakiInfo != null
            //        //    && (info.TokuisakiId != 0 && info.TokuisakiId != tokuisakiInfo.ToraDONTokuisakiId)
            //        //    && (info.ClmClassId != 0 && info.ClmClassId != clmclassInfo.ClmClassId))
            //        //{
            //        //    //編集をキャンセル
            //        //    isClear = true;
            //        //    //イベントをキャンセル
            //        //    e.Cancel = true;

            //        //    MessageBox.Show(
            //        //    FrameUtilites.GetDefineMessage("MW2203076"),
            //        //    "警告",
            //        //    MessageBoxButtons.OK,
            //        //    MessageBoxIcon.Warning);
            //        //}

            //        //this.numContractCd.Tag = info.ContractId;
            //        //this.numContractCd.Value = info.ContractCode;
            //        //this.edtContractNm.Text = info.ContractName;
            //    }
            //}
            //finally
            //{
            //    if (isClear)
            //    {
            //        this.numContractCd.Tag = null;
            //        this.numContractCd.Value = null;
            //        this.edtContractNm.Text = string.Empty;
            //    }
            //}
        }

        /// <summary>
        /// 販路コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHanroCd(CancelEventArgs e)
        {
            bool isClear = false;

            //try
            //{
            //    //コードの入力が無い場合は抜ける
            //    if (0 == this.numHanroCd.Value)
            //    {
            //        isClear = true;
            //        return;
            //    }

            //    // 販路情報を取得
            //    HanroSearchParameter para = new HanroSearchParameter();
            //    para.HanroCode = Convert.ToInt32(this.numHanroCd.Value);
            //    HanroInfo info = _DalUtil.Hanro.GetList(para).FirstOrDefault();

            //    if (null == info)
            //    {
            //        MessageBox.Show(
            //            FrameUtilites.GetDefineMessage("MW2201003"),
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        isClear = true;
            //        //イベントをキャンセル
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //        this.numHanroCd.Tag = info.HanroId;
            //        this.numHanroCd.Value = info.HanroCode;
            //        this.edtHanroNm.Text = info.HanroName;
            //    }
            //}
            //finally
            //{
            //    if (isClear)
            //    {
            //        this.numHanroCd.Tag = null;
            //        this.numHanroCd.Value = null;
            //        this.edtHanroNm.Text = string.Empty;
            //    }
            //}
        }

        /// <summary>
        /// 受注担当コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateJuchuTantoCd(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //    //コードの入力が無い場合は抜ける
                //    if (0 == this.numJuchuTantoCd.Value)
                //    {
                //        isClear = true;
                //        return;
                //    }


                //    // 受注担当情報を取得
                //    StaffSearchParameter para = new StaffSearchParameter();
                //    para.ToraDONStaffCode = Convert.ToInt32(this.numJuchuTantoCd.Value);
                //    StaffInfo info = _DalUtil.Staff.GetList(para, null).FirstOrDefault();

                //    if (null == info)
                //    {
                //        MessageBox.Show(
                //            FrameUtilites.GetDefineMessage("MW2201003"),
                //            this.Text,
                //            MessageBoxButtons.OK,
                //            MessageBoxIcon.Warning);

                //        isClear = true;
                //        //イベントをキャンセル
                //        e.Cancel = true;
                //    }
                //    else
                //    {
                //        this.numJuchuTantoCd.Tag = info.ToraDONStaffId;
                //        this.numJuchuTantoCd.Value = info.ToraDONStaffCode;
                //        this.edtJuchuTantoNm.Text = info.ToraDONStaffName;
                //    }
            }
            finally
            {
                if (isClear)
                {
                    this.numJuchuTantoCd.Tag = null;
                    this.numJuchuTantoCd.Value = null;
                    this.edtJuchuTantoNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 営業所コンボボックスの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCmbBranchOffice(CancelEventArgs e)
        {
            ////営業所情報を取得
            //ToraDONBranchOfficeSearchParameter para = new ToraDONBranchOfficeSearchParameter();
            //para.BranchOfficeCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);
            //ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetList(para).FirstOrDefault();

            //if (info == null)
            //{
            //    this.cmbBranchOffice.Tag = decimal.Zero;

            //    //イベントをキャンセル
            //    e.Cancel = true;
            //    return;
            //}

            //this.cmbBranchOffice.Tag = info.BranchOfficeId;
        }

        private void numCarCd_Validating(object sender, CancelEventArgs e)
        {
            //車両コード
            this.ValidateCarCd(e);
        }

        private void numDriverCd_Validating(object sender, CancelEventArgs e)
        {
            //乗務員コード
            this.ValidateDriverCd(e);
        }

        private void numCarOfChartererCd_Validating(object sender, CancelEventArgs e)
        {
            //傭車先コード
            this.ValidateCarOfChartererCd(e);
        }

        private void numTokuisakiCd_Validating(object sender, CancelEventArgs e)
        {
            //得意先コード
            this.ValidateTokuisakiCd(e);
        }

        private void numClmClassCd_Validating(object sender, CancelEventArgs e)
        {
            //請求部門コード
            this.ValidateClmClassCd(e);
        }

        private void numContractCd_Validating(object sender, CancelEventArgs e)
        {
            //請負コード
            this.ValidateContractCd(e);
        }

        private void numHanroCd_Validating(object sender, CancelEventArgs e)
        {
            //販路コード
            this.ValidateHanroCd(e);
        }

        private void numJuchuTantoCd_Validating(object sender, CancelEventArgs e)
        {
            //受注担当コード
            this.ValidateJuchuTantoCd(e);
        }

        private void cmbBranchOffice_Validating(object sender, CancelEventArgs e)
        {
            //営業所コンボボックス
            this.ValidateCmbBranchOffice(e);
        }

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

        void GcTextBoxEditingControl_KeyDown(object sender, KeyEventArgs e)
        {
            // ※ GcTextBoxCell で上・下キーが押されたら強制的にセル移動させる
            if (e.KeyCode == Keys.Up)
            {
                SelectionActions.MoveToPreviousCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
            }
            else if (e.KeyCode == Keys.Down)
            {
                SelectionActions.MoveToNextCellThenControl.Execute(((IEditingControl)sender).GcMultiRow);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            this.DoGetData();
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

        private void mrsJuchuNyuryokuMeisaiList_CellEnter(object sender, CellEventArgs e)
        {
            GcMultiRow gcMultiRow = sender as GcMultiRow;
            gcMultiRow.BeginEdit(true);
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }

        private void mrsJuchuList_CellMouseDoubleClick(object sender, CellMouseEventArgs e)
        {
            if (e.Scope == CellScope.Row)
            {
                this.DoUpdateEntry();
            }
        }
    }
}
