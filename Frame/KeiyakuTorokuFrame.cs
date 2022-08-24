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
using GrapeCity.Win.Editors;
using System.ComponentModel;
using jp.co.jpsys.util;
using System.Drawing;
using InputManCell = GrapeCity.Win.MultiRow.InputMan;
using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.Frame
{
    public partial class KeiyakuTorokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public KeiyakuTorokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 受注№を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="JuchuSlipNo"></param>
        /// <param name="JuchuId"></param>
        public KeiyakuTorokuFrame(decimal id)
        {
            InitializeComponent();

            //メンバにセット
            this.SelectDtlId = id;
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検索可能状態

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        private SearchStateBinder _searchStateBinder;

        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "契約登録";

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
        /// 契約クラス
        /// </summary>
        private Keiyaku _Keiyaku;

        /// <summary>
        /// 契約パラメータ
        /// </summary>
        private KeiyakuInfo _KeiyakuInfo;

        /// <summary>
        /// 画面表示可能かどうかの値を保持する領域（true：表示可）
        /// </summary>
        private bool canShowFrame = true;

        /// <summary>
        /// 画面を表示する時のエラーメッセージを保持する領域
        /// </summary>
        private string showFrameMsg = string.Empty;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        /// <summary>
        /// 再表示を行うかどうかの値を保持する領域
        /// (true:再表示する)
        /// (false:再表示しない)
        /// </summary>
        private bool isReDraw = false;

        /// <summary>
        /// 一覧のインスタンス
        /// </summary>
        public KeiyakuViewFrame KeiyakuIchiran { get; set; }

        /// <summary>
        /// 一覧にて選択されたID
        /// </summary>
        public decimal SelectDtlId { get; set; } = decimal.Zero;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitJuchuNyuryokuFrame()
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
                {this.edtSagyoBashoCd, this.ShowCmnSearchSagyobasho},
            };

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //契約クラスインスタンス作成
            this._Keiyaku = new Keiyaku(this.appAuth);

            //コンボボックスの初期化
            this.InitCombo();

            //入力項目のクリア
            this.ClearInputs();

            // モード設定（画面間パラメータの設定有無で判定）
            if (decimal.Zero < this.SelectDtlId)
            {
                //データ表示
                this.DoGetData();
                if (this._KeiyakuInfo.ShelterId == decimal.Zero)
                {
                    //現在の画面モードを修正状態に変更
                    this.ChangeMode(FrameEditMode.Editable);
                }
                else
                {
                    //現在の画面モードを参照のみ（作業日情報は入力可）に変更
                    this.ChangeMode(FrameEditMode.ViewOnly);
                }
            }
            else
            {
                //現在の画面モードを初期状態に変更
                this.ChangeMode(FrameEditMode.New);
                this._KeiyakuInfo = new KeiyakuInfo();
            }
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SettingSearchStateBinder()
        {
            this._searchStateBinder = new SearchStateBinder(this);
            this._searchStateBinder.AddSearchableControls(
                this.edtSagyoBashoCd);
            this._searchStateBinder.AddStateObject(this.toolStripSearch);
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

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.cmbKeiyakuJyokyo.SelectedIndex = 0;

            this.edtKeiyakuCode.Text = string.Empty;
            this.edtKeiyakuName.Text = string.Empty;
            this.edtSagyoBashoCd.Text = string.Empty;
            this.edtSagyoBashoCd.Tag = null;
            this.edtSagyoBashoNm.Text = string.Empty;

            this.edtTokuisakiCode.Text = string.Empty;
            this.edtTokuisakiName.Text = string.Empty;

            this.dteKeiyakuYMDFrom.Value = null;
            this.dteKeiyakuYMDTo.Value = null;

            this.cmbSagyoDaiBunrui.SelectedIndex = 0;
            this.cmbSagyoChuBunrui.Items.Clear();
            this.cmbSagyoShoBunrui.Items.Clear();

            this.cmbSagyobiKbn.SelectedIndex = 0;

            this.numSagyoNinzu.Value = null;

            this.chkDisableFlag.Checked = false;

            this.chkMonday.Checked = false;
            this.chkTuesday.Checked = false;
            this.chkWednesday.Checked = false;
            this.chkThursday.Checked = false;
            this.chkFriday.Checked = false;
            this.chkSaturday.Checked = false;
            this.chkSunday.Checked = false;

            this.cmbSagyoDay1.SelectedIndex = 0;
            this.cmbSagyoDay2.SelectedIndex = 0;
            this.cmbSagyoDay3.SelectedIndex = 0;
            this.cmbSagyoDay4.SelectedIndex = 0;
            this.cmbSagyoDay5.SelectedIndex = 0;

            this.dteSagyoDate1.Value = null;
            this.dteSagyoDate2.Value = null;
            this.dteSagyoDate3.Value = null;
            this.dteSagyoDate4.Value = null;
            this.dteSagyoDate5.Value = null;
            this.dteSagyoDate6.Value = null;
            this.dteSagyoDate7.Value = null;
            this.dteSagyoDate8.Value = null;
            this.dteSagyoDate9.Value = null;
            this.dteSagyoDate10.Value = null;

            //メンバをクリア
            this.isConfirmClose = true;

            //作業日情報の切替
            this.ChangeSagyoMode();
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

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
            this.SagyoDaiBunruiCombo();
            this.InitKeiyakuJyokyoKbnCombo();
            this.InitSagyobiKbnCombo();
            this.InitSagyoDayCombo();
        }

        /// <summary>
        ///契約状況コンボボックスを初期化します。
        /// </summary>
        private void InitKeiyakuJyokyoKbnCombo()
        {
            // 契約状況コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<SystemNameInfo> list = this._DalUtil.SystemGlobalName.GetList((Int32)DefaultProperty.SystemNameKbn.KeiyakuJyokyoKbns);

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

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbKeiyakuJyokyo, datasource, false, null, false);
            this.cmbKeiyakuJyokyo.SelectedIndex = 0;
        }

        /// <summary>
        ///作業区分コンボボックスを初期化します。
        /// </summary>
        private void InitSagyobiKbnCombo()
        {
            // 作業区分コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            IList<SystemNameInfo> list = this._DalUtil.SystemGlobalName.GetList((Int32)DefaultProperty.SystemNameKbn.SagyobiKbn);

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

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyobiKbn, datasource, true, null, false);
            this.cmbSagyobiKbn.SelectedIndex = 0;
        }

        /// <summary>
        ///月指定日コンボボックスを初期化します。
        /// </summary>
        private void InitSagyoDayCombo()
        {
            // 月指定日コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            String key = "";
            String value = "";

            for(int i = 1; i <= 31; i++)
            {
                key = i.ToString();
                value = key + "日";

                datasource.Add(key, value);
            }

            //月末日
            datasource.Add(DefaultProperty.SAGYODAY_MONTH_END.ToString(), "月末");

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDay1, datasource, true, null, false);
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDay2, datasource, true, null, false);
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDay3, datasource, true, null, false);
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDay4, datasource, true, null, false);
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDay5, datasource, true, null, false);
            this.cmbSagyoDay1.SelectedIndex = 0;
            this.cmbSagyoDay2.SelectedIndex = 0;
            this.cmbSagyoDay3.SelectedIndex = 0;
            this.cmbSagyoDay4.SelectedIndex = 0;
            this.cmbSagyoDay5.SelectedIndex = 0;
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

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDaiBunrui, datasource, true, null, false);
            this.cmbSagyoDaiBunrui.SelectedIndex = 0;
        }

        /// <summary>
        /// 作業中分類コンボボックスを初期化します。
        /// </summary>
        private void SagyoChuBunruiCombo(decimal id)
        {
            if (id == 0)
            {
                this.cmbSagyoChuBunrui.Items.Clear();
                return;
            }

            // 作業中分類コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            SagyoChuBunruiSearchParameter para = new SagyoChuBunruiSearchParameter();
            para.SagyoDaiBunruiId = id;

            IList<SagyoChuBunruiInfo> list = this._DalUtil.SagyoChuBunrui.GetList(para);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.SagyoChuBunruiCode)
                    .ToList();
            }
            else
            {
                list = new List<SagyoChuBunruiInfo>();
            }

            String key = "";
            String value = "";

            foreach (SagyoChuBunruiInfo item in list)
            {
                key = item.SagyoChuBunruiId.ToString();
                value = item.SagyoChuBunruiCode.ToString() + "." + item.SagyoChuBunruiName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoChuBunrui, datasource, true, null, false);
            this.cmbSagyoChuBunrui.SelectedIndex = 0;
        }

        /// <summary>
        /// 作業小分類コンボボックスを初期化します。
        /// </summary>
        private void SagyoShoBunruiCombo(decimal daiBunruiId, decimal chuBunruiId)
        {
            if (daiBunruiId == 0)
            {
                this.cmbSagyoShoBunrui.Items.Clear();
                return;
            }

            // 作業小分類コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            SagyoShoBunruiSearchParameter para = new SagyoShoBunruiSearchParameter();
            para.SagyoDaiBunruiId = daiBunruiId;
            para.SagyoChuBunruiId = chuBunruiId;

            IList<SagyoShoBunruiInfo> list = this._DalUtil.SagyoShoBunrui.GetList(para);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.SagyoShoBunruiCode)
                    .ToList();
            }
            else
            {
                list = new List<SagyoShoBunruiInfo>();
            }

            String key = "";
            String value = "";

            foreach (SagyoShoBunruiInfo item in list)
            {
                key = item.SagyoShoBunruiId.ToString();
                value = item.SagyoShoBunruiCode.ToString() + "." + item.SagyoShoBunruiName.ToString();

                datasource.Add(key, value);
            }

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoShoBunrui, datasource, true, null, false);
            this.cmbSagyoShoBunrui.SelectedIndex = 0;
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
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
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

        void DeleteAllDtl_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearch();
        }

        private void JuchuNyuryokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void JuchuNyuryokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);

            //画面表示時のメッセージ表示
            if (this.showFrameMsg.Trim().Length != 0)
            {
                MessageBox.Show(
                    this.showFrameMsg,
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);

                if (!this.canShowFrame)
                {
                    //画面を閉じる（終了確認はしない）
                    this.isConfirmClose = false;
                    this.DoClose();
                }
            }
        }

        private void JuchuNyuryokuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
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

        private void ym_Down(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, -1);
        }

        private void ym_Up(object sender, EventArgs e)
        {
            this.AddDayForNskDateTime(this.ActiveControl, 1);
        }

        private void AddMonthForNskDateTime(Control c, int month)
        {
            if (c.GetType().Equals(typeof(NskDateTime)))
            {
                try
                {
                    ((NskDateTime)c).Value = Convert.ToDateTime(((NskDateTime)c).Value).AddMonths(month);
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
            this.InitJuchuNyuryokuFrame();
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

            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {

                if (this.ValidateChildren(ValidationConstraints.None) && this.CheckInputs())
                {
                    //画面から値を取得
                    this.GetScreen();

                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            // 登録・更新
                            this._Keiyaku.Save(tx, this._KeiyakuInfo);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "契約登録", this.edtKeiyakuCode.Text });

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

                        ////初期状態へ移行
                        if (FrameEditMode.New == this.currentMode)
                        {
                            //データ表示
                            this.DoClear(false);
                        }
                        //else if (FrameEditMode.Editable == this.currentMode)
                        else
                        {
                            if (this.KeiyakuIchiran != null)
                            {
                                //呼び出し元のPGの再表示を指示
                                this.KeiyakuIchiran.ReSetScreen(this.SelectDtlId);
                            }

                            // 画面を閉じる
                            this.isConfirmClose = false;
                            this.DoClose();
                        }

                    }
                    catch (CanRetryException ex)
                    {
                        //データがない場合の例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
                        if (FrameEditMode.New == this.currentMode)
                        {
                            //フォーカスを移動
                            this.edtKeiyakuCode.Focus();
                        }
                        else
                        {
                            //フォーカスを移動
                            this.edtKeiyakuName.Focus();
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
                        this.edtKeiyakuCode.Focus();
                    }
                }
            }
            finally
            {
                //マウスカーソルを元に戻す
                Cursor.Current = Cursors.Default;
            }
        }

        /// <summary>
        /// 画面に読み込んだデータを削除します。
        /// </summary>
        private void DoDelData()
        {
            //実行確認ダイアログ
            DialogResult d_result =
                MessageBox.Show(
                FrameUtilites.GetDefineMessage("MQ2102003"),
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            if (this.ValidateChildren(ValidationConstraints.None))
            {
                //画面から値を取得
                this.GetScreen();

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._Keiyaku.Delete(tx, this._KeiyakuInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken = FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "契約削除", this.edtKeiyakuCode.Text });

                    //保存完了メッセージ格納用
                    string msg = string.Empty;

                    //更新完了のメッセージ
                    msg =
                        FrameUtilites.GetDefineMessage("MI2001004");

                    //登録完了メッセージ
                    MessageBox.Show(
                        msg,
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Information);

                    //一覧画面に戻った際に再検索を行う。
                    this.isReDraw = true;

                    if (this.KeiyakuIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.KeiyakuIchiran.ReSetScreen(this.SelectDtlId);
                    }

                    // 画面を閉じる
                    this.isConfirmClose = false;
                    this.DoClose();

                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);

                    if (FrameEditMode.New == this.currentMode)
                    {
                        //フォーカスを移動
                        this.edtKeiyakuCode.Focus();
                    }
                    else
                    {
                        //フォーカスを移動
                        this.edtKeiyakuName.Focus();
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
                    this.edtKeiyakuCode.Focus();
                }
            }
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            if (this.isReDraw)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Cancel;
            }

            this.Close();
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            this._KeiyakuInfo.KeiyakuCode = this.edtKeiyakuCode.Text;

            this._KeiyakuInfo.KeiyakuJyokyo = Convert.ToInt32(this.cmbKeiyakuJyokyo.SelectedValue);

            // 契約日付（From）
            if (this.dteKeiyakuYMDFrom.Value != null)
            {
                this._KeiyakuInfo.KeiyakuStartDate = new DateTime(
                    this.dteKeiyakuYMDFrom.Value.Value.Year
                    , this.dteKeiyakuYMDFrom.Value.Value.Month
                    , this.dteKeiyakuYMDFrom.Value.Value.Day
                    , 0
                    , 0
                    , 0);
            }
            else
            {
                this._KeiyakuInfo.KeiyakuStartDate = DateTime.MinValue;
            }

            // 契約日付（To）
            if (this.dteKeiyakuYMDTo.Value != null)
            {
                this._KeiyakuInfo.KeiyakuEndDate = new DateTime(
                    this.dteKeiyakuYMDTo.Value.Value.Year
                    , this.dteKeiyakuYMDTo.Value.Value.Month
                    , this.dteKeiyakuYMDTo.Value.Value.Day
                    , 23
                    , 59
                    , 59);
            }
            else
            {
                this._KeiyakuInfo.KeiyakuEndDate = DateTime.MinValue;
            }

            this._KeiyakuInfo.KeiyakuName = this.edtKeiyakuName.Text;
            this._KeiyakuInfo.SagyoBashoId = Convert.ToDecimal(this.edtSagyoBashoCd.Tag);
            this._KeiyakuInfo.SagyoDaiBunruiId = Convert.ToInt32(this.cmbSagyoDaiBunrui.SelectedValue);
            this._KeiyakuInfo.SagyoChuBunruiId = Convert.ToInt32(this.cmbSagyoChuBunrui.SelectedValue);
            this._KeiyakuInfo.SagyoShoBunruiId = Convert.ToInt32(this.cmbSagyoShoBunrui.SelectedValue);

            if (FrameEditMode.New == this.currentMode)
            {
                this._KeiyakuInfo.ShelterId = decimal.Zero;
            }

            this._KeiyakuInfo.DisableFlag = this.chkDisableFlag.Checked;

            this._KeiyakuInfo.SagyobiKbn = Convert.ToInt32(this.cmbSagyobiKbn.SelectedValue);
            this._KeiyakuInfo.SagyoNinzu = Convert.ToInt32(this.numSagyoNinzu.Value);

            //作業日情報の初期化

            this._KeiyakuInfo.Monday = false;
            this._KeiyakuInfo.Tuesday = false;
            this._KeiyakuInfo.Wednesday = false;
            this._KeiyakuInfo.Thursday = false;
            this._KeiyakuInfo.Friday = false;
            this._KeiyakuInfo.Saturday = false;
            this._KeiyakuInfo.Sunday = false;

            this._KeiyakuInfo.SagyoDay1 = decimal.Zero;
            this._KeiyakuInfo.SagyoDay2 = decimal.Zero;
            this._KeiyakuInfo.SagyoDay3 = decimal.Zero;
            this._KeiyakuInfo.SagyoDay4 = decimal.Zero;
            this._KeiyakuInfo.SagyoDay5 = decimal.Zero;

            this._KeiyakuInfo.SagyoDate1 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate2 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate3 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate4 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate5 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate6 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate7 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate8 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate9 = DateTime.MinValue;
            this._KeiyakuInfo.SagyoDate10 = DateTime.MinValue;

            switch (this._KeiyakuInfo.SagyobiKbn)
            {
                case (Int32)DefaultProperty.SagyobiKbns.None:
                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Weekly:

                    this._KeiyakuInfo.Monday = this.chkMonday.Checked;
                    this._KeiyakuInfo.Tuesday = this.chkTuesday.Checked;
                    this._KeiyakuInfo.Wednesday = this.chkWednesday.Checked;
                    this._KeiyakuInfo.Thursday = this.chkThursday.Checked;
                    this._KeiyakuInfo.Friday = this.chkFriday.Checked;
                    this._KeiyakuInfo.Saturday = this.chkSaturday.Checked;
                    this._KeiyakuInfo.Sunday = this.chkSunday.Checked;

                    break;

                case (Int32)DefaultProperty.SagyobiKbns.Monthly:

                    this._KeiyakuInfo.SagyoDay1 = Convert.ToDecimal(this.cmbSagyoDay1.SelectedValue);
                    this._KeiyakuInfo.SagyoDay2 = Convert.ToDecimal(this.cmbSagyoDay2.SelectedValue);
                    this._KeiyakuInfo.SagyoDay3 = Convert.ToDecimal(this.cmbSagyoDay3.SelectedValue);
                    this._KeiyakuInfo.SagyoDay4 = Convert.ToDecimal(this.cmbSagyoDay4.SelectedValue);
                    this._KeiyakuInfo.SagyoDay5 = Convert.ToDecimal(this.cmbSagyoDay5.SelectedValue);

                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Day:

                    this._KeiyakuInfo.SagyoDate1 = this.dteSagyoDate1.Value == null ? DateTime.MinValue : this.dteSagyoDate1.Value.Value;
                    this._KeiyakuInfo.SagyoDate2 = this.dteSagyoDate2.Value == null ? DateTime.MinValue : this.dteSagyoDate2.Value.Value;
                    this._KeiyakuInfo.SagyoDate3 = this.dteSagyoDate3.Value == null ? DateTime.MinValue : this.dteSagyoDate3.Value.Value;
                    this._KeiyakuInfo.SagyoDate4 = this.dteSagyoDate4.Value == null ? DateTime.MinValue : this.dteSagyoDate4.Value.Value;
                    this._KeiyakuInfo.SagyoDate5 = this.dteSagyoDate5.Value == null ? DateTime.MinValue : this.dteSagyoDate5.Value.Value;
                    this._KeiyakuInfo.SagyoDate6 = this.dteSagyoDate6.Value == null ? DateTime.MinValue : this.dteSagyoDate6.Value.Value;
                    this._KeiyakuInfo.SagyoDate7 = this.dteSagyoDate7.Value == null ? DateTime.MinValue : this.dteSagyoDate7.Value.Value;
                    this._KeiyakuInfo.SagyoDate8 = this.dteSagyoDate8.Value == null ? DateTime.MinValue : this.dteSagyoDate8.Value.Value;
                    this._KeiyakuInfo.SagyoDate9 = this.dteSagyoDate9.Value == null ? DateTime.MinValue : this.dteSagyoDate9.Value.Value;
                    this._KeiyakuInfo.SagyoDate10 = this.dteSagyoDate10.Value == null ? DateTime.MinValue : this.dteSagyoDate10.Value.Value;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckInputs()
        {

            //戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;
            Control ctl = null;

            //コードの必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtKeiyakuCode.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "コード" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtKeiyakuCode;
            }

            //契約名称の必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtKeiyakuName.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "契約名称" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtKeiyakuName;
            }

            //作業場所の必須チェック
            if (rt_val && string.IsNullOrWhiteSpace(this.edtSagyoBashoCd.Text))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "作業場所" });
                icon = MessageBoxIcon.Warning;
                ctl = this.edtSagyoBashoCd;
            }

            //契約開始日の必須チェック
            if (rt_val && this.dteKeiyakuYMDFrom.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "契約開始日" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteKeiyakuYMDFrom;
            }

            //契約終了日の必須チェック
            if (rt_val && this.dteKeiyakuYMDTo.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "契約終了日" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteKeiyakuYMDTo;
            }

            //作業日時の反転チェック
            if (rt_val && this.dteKeiyakuYMDTo.Value < this.dteKeiyakuYMDFrom.Value)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "契約日" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteKeiyakuYMDTo;
            }

            //作業日付情報必須チェック
            switch (Convert.ToInt32(this.cmbSagyobiKbn.SelectedValue))
            {
                case (Int32)DefaultProperty.SagyobiKbns.None:
                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Weekly:

                    //指定曜日の必須チェック
                    if (rt_val 
                        && !this.chkMonday.Checked
                        && !this.chkTuesday.Checked
                        && !this.chkWednesday.Checked
                        && !this.chkThursday.Checked
                        && !this.chkFriday.Checked
                        && !this.chkSaturday.Checked
                        && !this.chkSunday.Checked
                        )
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                                "MW2203001", new string[] { "作業日区分が毎週の場合、指定曜日" });
                        icon = MessageBoxIcon.Warning;
                        ctl = this.chkMonday;
                    }

                    break;

                case (Int32)DefaultProperty.SagyobiKbns.Monthly:

                    //月指定日の必須チェック
                    if (rt_val
                        && Convert.ToInt32(this.cmbSagyoDay1.SelectedValue) == 0
                        && Convert.ToInt32(this.cmbSagyoDay2.SelectedValue) == 0
                        && Convert.ToInt32(this.cmbSagyoDay3.SelectedValue) == 0
                        && Convert.ToInt32(this.cmbSagyoDay4.SelectedValue) == 0
                        && Convert.ToInt32(this.cmbSagyoDay5.SelectedValue) == 0
                        )
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                                "MW2203001", new string[] { "作業日区分が毎月の場合、月指定日" });
                        icon = MessageBoxIcon.Warning;
                        ctl = this.cmbSagyoDay1;
                    }

                    List<string> list = new List<string>();
                    list.Add(Convert.ToString(this.cmbSagyoDay1.SelectedValue));
                    list.Add(Convert.ToString(this.cmbSagyoDay2.SelectedValue));
                    list.Add(Convert.ToString(this.cmbSagyoDay3.SelectedValue));
                    list.Add(Convert.ToString(this.cmbSagyoDay4.SelectedValue));
                    list.Add(Convert.ToString(this.cmbSagyoDay5.SelectedValue));


                    List<Control> listCon = new List<Control>();
                    listCon.Add(this.cmbSagyoDay1);
                    listCon.Add(this.cmbSagyoDay2);
                    listCon.Add(this.cmbSagyoDay3);
                    listCon.Add(this.cmbSagyoDay4);
                    listCon.Add(this.cmbSagyoDay5);

                    foreach (Control item in listCon)
                    {
                        //月指定日の重複チェック
                        if (rt_val
                        && list.Where(x => x == Convert.ToString(((GcComboBox)item).SelectedValue)).Count() > 1
                        && Convert.ToInt32(((GcComboBox)item).SelectedValue) != 0)
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage(
                                    "MW2203010", new string[] { "月指定日" });
                            icon = MessageBoxIcon.Warning;
                            ctl =item;
                        }
                    }

                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Day:

                    //指定日の必須チェック
                    if (rt_val
                        && this.dteSagyoDate1.Value == null
                        && this.dteSagyoDate2.Value == null
                        && this.dteSagyoDate3.Value == null
                        && this.dteSagyoDate4.Value == null
                        && this.dteSagyoDate5.Value == null
                        && this.dteSagyoDate6.Value == null
                        && this.dteSagyoDate7.Value == null
                        && this.dteSagyoDate8.Value == null
                        && this.dteSagyoDate9.Value == null
                        && this.dteSagyoDate10.Value == null
                        )
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                                "MW2203001", new string[] { "作業日区分が指定日の場合、指定日" });
                        icon = MessageBoxIcon.Warning;
                        ctl = this.dteSagyoDate1;
                    }

                    List<DateTime?> listDay = new List<DateTime?>();
                    listDay.Add(this.dteSagyoDate1.Value);
                    listDay.Add(this.dteSagyoDate2.Value);
                    listDay.Add(this.dteSagyoDate3.Value);
                    listDay.Add(this.dteSagyoDate4.Value);
                    listDay.Add(this.dteSagyoDate5.Value);
                    listDay.Add(this.dteSagyoDate6.Value);
                    listDay.Add(this.dteSagyoDate7.Value);
                    listDay.Add(this.dteSagyoDate8.Value);
                    listDay.Add(this.dteSagyoDate9.Value);
                    listDay.Add(this.dteSagyoDate10.Value);

                    List<Control> listConDay = new List<Control>();
                    listConDay.Add(this.dteSagyoDate1);
                    listConDay.Add(this.dteSagyoDate2);
                    listConDay.Add(this.dteSagyoDate3);
                    listConDay.Add(this.dteSagyoDate4);
                    listConDay.Add(this.dteSagyoDate5);
                    listConDay.Add(this.dteSagyoDate6);
                    listConDay.Add(this.dteSagyoDate7);
                    listConDay.Add(this.dteSagyoDate8);
                    listConDay.Add(this.dteSagyoDate9);
                    listConDay.Add(this.dteSagyoDate10);

                    foreach (Control item in listConDay)
                    {
                        //指定日の契約日範囲チェック
                        if (rt_val
                            &&( ((NskDateTime)item).Value < this.dteKeiyakuYMDFrom.Value
                             || ((NskDateTime)item).Value > this.dteKeiyakuYMDTo.Value)
                            )
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage(
                                    "MW2203011", new string[] { "指定日", "契約日範囲内の日付" });
                            icon = MessageBoxIcon.Warning;
                            ctl =  item;
                            break;
                        }

                        //指定日の重複チェック
                        if (rt_val
                            && listDay.Where(x => x == ((NskDateTime)item).Value).Count() > 1
                            && ((NskDateTime)item).Value != null)
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage(
                                    "MW2203010", new string[] { "指定日" });
                            icon = MessageBoxIcon.Warning;
                            ctl = item;
                            break;
                        }
                    }

                    break;
                default:
                    break;
            }

            if (!rt_val)
            {
                //アイコンの種類によってMassageBoxTitleを変更
                string msg_title = string.Empty;

                switch (icon)
                {
                    case MessageBoxIcon.Error:
                        msg_title = "エラー";
                        break;
                    case MessageBoxIcon.Warning:
                        msg_title = "警告";
                        break;
                    default:
                        break;
                }

                MessageBox.Show(
                    msg, msg_title, MessageBoxButtons.OK, icon);

                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示

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
                    if (this.KeiyakuIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.KeiyakuIchiran.ReSetScreen(this.SelectDtlId);
                    }
                }
                else if (result == DialogResult.No)
                {
                    //Noの場合は終了をキャンセル
                    e.Cancel = true;
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

            if (FrameEditMode.New == this.currentMode)
            {
                this.ChangeMode(FrameEditMode.New);
            }
            else if (FrameEditMode.Editable == this.currentMode || FrameEditMode.ViewOnly == this.currentMode)
            {
                //受注情報を再表示
                this.DoGetData();
                this.ChangeMode(FrameEditMode.Editable);
            }
        }

        /// <summary>
        /// データを取得し、画面上にセットします。
        /// </summary>
        private void DoGetData()
        {
            // 画面コントローラに取得を指示
            // 契約情報取得
            this._KeiyakuInfo = this._Keiyaku.GetInfoById(this.SelectDtlId);

            // 0件なら処理しない
            if (null == this._KeiyakuInfo)
            {
                if (FrameEditMode.Editable == this.currentMode)
                {
                    this.showFrameMsg = FrameUtilites.GetDefineMessage("MW2201015");
                    //画面表示不可
                    this.canShowFrame = false;
                }
                else
                {
                    return;
                }
            }

            this.SetScreen();

        }

        /// <summary>
        /// 画面モードを表す列挙体を指定して、現在の画面の
        /// 表示モードを切り替えます。
        /// </summary>
        /// <param name="mode">変更したい画面モード</param>
        private void ChangeMode(FrameEditMode mode)
        {
            switch (mode)
            {
                case FrameEditMode.New:
                    //初期状態
                    // コントロールの使用可否
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtKeiyakuCode.ReadOnly = false;

                    // ファンクションの使用可否

                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = false;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = false;

                    break;
                case FrameEditMode.Editable:

                    //編集モード
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    this.edtKeiyakuCode.ReadOnly = true;
                    this.edtKeiyakuCode.ActiveBackColor = SystemColors.Control;

                    // ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = true;

                    break;

                case FrameEditMode.ViewOnly:

                    //参照モード※作業日情報は入力可
                    this.pnl.Enabled = true;
                    this.pnlBottom.Enabled = true;

                    this.edtKeiyakuCode.ReadOnly = true;
                    this.edtKeiyakuName.ReadOnly = true;
                    this.edtSagyoBashoCd.ReadOnly = true;
                    this.dteKeiyakuYMDFrom.ReadOnly = true;
                    this.dteKeiyakuYMDTo.ReadOnly = true;
                    this.numSagyoNinzu.ReadOnly = true;
                    this.sideButton1.Enabled = false;

                    this.cmbSagyoDaiBunrui.Enabled = false;
                    this.cmbSagyoChuBunrui.Enabled = false;
                    this.cmbSagyoShoBunrui.Enabled = false;
                    this.dropDownButton5.Enabled = false;
                    this.dropDownButton7.Enabled = false;
                    this.dropDownButton8.Enabled = false;

                    this.spinButton1.Enabled = false;
                    this.spinButton2.Enabled = false;

                    this.dropDownButton6.Enabled = false;
                    this.dropDownButton4.Enabled = false;

                    this.edtKeiyakuCode.ActiveBackColor = SystemColors.Control;
                    this.edtKeiyakuName.ActiveBackColor = SystemColors.Control;
                    this.edtSagyoBashoCd.ActiveBackColor = SystemColors.Control;
                    this.dteKeiyakuYMDFrom.ActiveBackColor = SystemColors.Control;
                    this.dteKeiyakuYMDTo.ActiveBackColor = SystemColors.Control;
                    this.cmbSagyoDaiBunrui.ActiveBackColor = SystemColors.Control;
                    this.cmbSagyoChuBunrui.ActiveBackColor = SystemColors.Control;
                    this.cmbSagyoShoBunrui.ActiveBackColor = SystemColors.Control;
                    this.numSagyoNinzu.ActiveBackColor = SystemColors.Control;

                    this.chkDisableFlag.Enabled = true;

                    // ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Delete.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = true;

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            this.currentMode = mode;
        }

        /// <summary>
        /// 作業区分により、作業
        /// 表示モードを切り替えます。
        /// </summary>
        /// <param name="mode">変更したい画面モード</param>
        private void ChangeSagyoMode()
        {

            //初期状態
            // コントロールの使用可否
            this.chkMonday.Enabled = false;
            this.chkTuesday.Enabled = false;
            this.chkWednesday.Enabled = false;
            this.chkThursday.Enabled = false;
            this.chkFriday.Enabled = false;
            this.chkSaturday.Enabled = false;
            this.chkSunday.Enabled = false;

            this.cmbSagyoDay1.Enabled = false;
            this.cmbSagyoDay2.Enabled = false;
            this.cmbSagyoDay3.Enabled = false;
            this.cmbSagyoDay4.Enabled = false;
            this.cmbSagyoDay5.Enabled = false;

            this.dteSagyoDate1.Enabled = false;
            this.dteSagyoDate2.Enabled = false;
            this.dteSagyoDate3.Enabled = false;
            this.dteSagyoDate4.Enabled = false;
            this.dteSagyoDate5.Enabled = false;
            this.dteSagyoDate6.Enabled = false;
            this.dteSagyoDate7.Enabled = false;
            this.dteSagyoDate8.Enabled = false;
            this.dteSagyoDate9.Enabled = false;
            this.dteSagyoDate10.Enabled = false;

            switch (Convert.ToInt32(this.cmbSagyobiKbn.SelectedValue))
            {
                case (Int32)DefaultProperty.SagyobiKbns.None:
                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Weekly:

                    this.chkMonday.Enabled = true;
                    this.chkTuesday.Enabled = true;
                    this.chkWednesday.Enabled = true;
                    this.chkThursday.Enabled = true;
                    this.chkFriday.Enabled = true;
                    this.chkSaturday.Enabled = true;
                    this.chkSunday.Enabled = true;

                    break;

                case (Int32)DefaultProperty.SagyobiKbns.Monthly:

                    this.cmbSagyoDay1.Enabled = true;
                    this.cmbSagyoDay2.Enabled = true;
                    this.cmbSagyoDay3.Enabled = true;
                    this.cmbSagyoDay4.Enabled = true;
                    this.cmbSagyoDay5.Enabled = true;

                    break;
                case (Int32)DefaultProperty.SagyobiKbns.Day:

                    this.dteSagyoDate1.Enabled = true;
                    this.dteSagyoDate2.Enabled = true;
                    this.dteSagyoDate3.Enabled = true;
                    this.dteSagyoDate4.Enabled = true;
                    this.dteSagyoDate5.Enabled = true;
                    this.dteSagyoDate6.Enabled = true;
                    this.dteSagyoDate7.Enabled = true;
                    this.dteSagyoDate8.Enabled = true;
                    this.dteSagyoDate9.Enabled = true;
                    this.dteSagyoDate10.Enabled = true;

                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            this.edtKeiyakuCode.Text = this._KeiyakuInfo.KeiyakuCode; ;
            this.cmbKeiyakuJyokyo.SelectedValue = this._KeiyakuInfo.KeiyakuJyokyo.ToString();

            this.edtKeiyakuName.Text = this._KeiyakuInfo.KeiyakuName;
            this.edtSagyoBashoCd.Text  = this._KeiyakuInfo.SagyoBashoCode;
            this.edtSagyoBashoCd.Tag = this._KeiyakuInfo.SagyoBashoId;
            this.edtSagyoBashoNm.Text = this._KeiyakuInfo.SagyoBashoName;

            this.edtTokuisakiCode.Text = this._KeiyakuInfo.TokuisakiCode;
            this.edtTokuisakiName.Text = this._KeiyakuInfo.TokuisakiName;

            if (this._KeiyakuInfo.KeiyakuStartDate == DateTime.MinValue)
            {
                this.dteKeiyakuYMDFrom.Value = null;
            }
            else
            {
                this.dteKeiyakuYMDFrom.Value = this._KeiyakuInfo.KeiyakuStartDate;
            }

            if (this._KeiyakuInfo.KeiyakuEndDate == DateTime.MinValue)
            {
                this.dteKeiyakuYMDTo.Value = null;
            }
            else
            {
                this.dteKeiyakuYMDTo.Value = this._KeiyakuInfo.KeiyakuEndDate;
            }

            this.cmbSagyoDaiBunrui.SelectedValue = this._KeiyakuInfo.SagyoDaiBunruiId.ToString();

            //中分類コンボ検索
            this.SagyoChuBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue));
            this.cmbSagyoChuBunrui.SelectedValue = this._KeiyakuInfo.SagyoChuBunruiId.ToString();

            //小分類コンボ検索
            this.SagyoShoBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue),
                Convert.ToDecimal(this.cmbSagyoChuBunrui.SelectedValue));
            this.cmbSagyoShoBunrui.SelectedValue = this._KeiyakuInfo.SagyoShoBunruiId.ToString();

            this.chkDisableFlag.Checked = this._KeiyakuInfo.DisableFlag;

            this.cmbSagyobiKbn.SelectedValue = this._KeiyakuInfo.SagyobiKbn.ToString();

            this.numSagyoNinzu.Value = this._KeiyakuInfo.SagyoNinzu;

            this.chkMonday.Checked = this._KeiyakuInfo.Monday;
            this.chkTuesday.Checked = this._KeiyakuInfo.Tuesday;
            this.chkWednesday.Checked = this._KeiyakuInfo.Wednesday;
            this.chkThursday.Checked = this._KeiyakuInfo.Thursday;
            this.chkFriday.Checked = this._KeiyakuInfo.Friday;
            this.chkSaturday.Checked = this._KeiyakuInfo.Saturday;
            this.chkSunday.Checked = this._KeiyakuInfo.Sunday;

            this.cmbSagyoDay1.SelectedValue = this._KeiyakuInfo.SagyoDay1.ToString();
            this.cmbSagyoDay2.SelectedValue = this._KeiyakuInfo.SagyoDay2.ToString();
            this.cmbSagyoDay3.SelectedValue = this._KeiyakuInfo.SagyoDay3.ToString();
            this.cmbSagyoDay4.SelectedValue = this._KeiyakuInfo.SagyoDay4.ToString();
            this.cmbSagyoDay5.SelectedValue = this._KeiyakuInfo.SagyoDay5.ToString();

            if (this._KeiyakuInfo.SagyoDate1 == DateTime.MinValue)
            {
                this.dteSagyoDate1.Value = null;
            }
            else
            {
                this.dteSagyoDate1.Value = this._KeiyakuInfo.SagyoDate1;
            }

            if (this._KeiyakuInfo.SagyoDate2 == DateTime.MinValue)
            {
                this.dteSagyoDate2.Value = null;
            }
            else
            {
                this.dteSagyoDate2.Value = this._KeiyakuInfo.SagyoDate2;
            }

            if (this._KeiyakuInfo.SagyoDate3 == DateTime.MinValue)
            {
                this.dteSagyoDate3.Value = null;
            }
            else
            {
                this.dteSagyoDate3.Value = this._KeiyakuInfo.SagyoDate3;
            }

            if (this._KeiyakuInfo.SagyoDate4 == DateTime.MinValue)
            {
                this.dteSagyoDate4.Value = null;
            }
            else
            {
                this.dteSagyoDate4.Value = this._KeiyakuInfo.SagyoDate4;
            }

            if (this._KeiyakuInfo.SagyoDate5 == DateTime.MinValue)
            {
                this.dteSagyoDate5.Value = null;
            }
            else
            {
                this.dteSagyoDate5.Value = this._KeiyakuInfo.SagyoDate5;
            }

            if (this._KeiyakuInfo.SagyoDate6 == DateTime.MinValue)
            {
                this.dteSagyoDate6.Value = null;
            }
            else
            {
                this.dteSagyoDate6.Value = this._KeiyakuInfo.SagyoDate6;
            }

            if (this._KeiyakuInfo.SagyoDate7 == DateTime.MinValue)
            {
                this.dteSagyoDate7.Value = null;
            }
            else
            {
                this.dteSagyoDate7.Value = this._KeiyakuInfo.SagyoDate7;
            }

            if (this._KeiyakuInfo.SagyoDate8 == DateTime.MinValue)
            {
                this.dteSagyoDate8.Value = null;
            }
            else
            {
                this.dteSagyoDate8.Value = this._KeiyakuInfo.SagyoDate8;
            }

            if (this._KeiyakuInfo.SagyoDate9 == DateTime.MinValue)
            {
                this.dteSagyoDate9.Value = null;
            }
            else
            {
                this.dteSagyoDate9.Value = this._KeiyakuInfo.SagyoDate9;
            }

            if (this._KeiyakuInfo.SagyoDate10 == DateTime.MinValue)
            {
                this.dteSagyoDate10.Value = null;
            }
            else
            {
                this.dteSagyoDate10.Value = this._KeiyakuInfo.SagyoDate10;
            }

            //作業日情報の切替
            this.ChangeSagyoMode();
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

                    if (!this.edtSagyoBashoCd.ReadOnly)
                    {
                        //キーボードイベントの抑止
                        e.Handled = true;

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

        #endregion

        #region 検証（Validate）処理

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
                    if (info.DisableFlag)
                    {
                        MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            this.Text,
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        isClear = true;
                    }
                    else
                    {
                        this.edtSagyoBashoCd.Tag = info.SagyoBashoId;
                        this.edtSagyoBashoNm.Text = info.SagyoBashoName;

                        this.edtTokuisakiCode.Text = info.TokuisakiCode;
                        this.edtTokuisakiName.Text = info.TokuisakiName;
                    }
                }
            }
            finally
            {
                if (isClear)
                {
                    this.edtSagyoBashoCd.Tag = null;
                    this.edtSagyoBashoCd.Text = string.Empty;
                    this.edtSagyoBashoNm.Text = string.Empty;

                    this.edtTokuisakiCode.Text = string.Empty;
                    this.edtTokuisakiName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 作業大分類のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbSagyoDaiBunrui_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSagyoChuBunrui.Items.Clear();
            this.cmbSagyoShoBunrui.Items.Clear();
            this.SagyoChuBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue));
            this.SagyoShoBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue),
                Convert.ToDecimal(this.cmbSagyoChuBunrui.SelectedValue));
        }
        /// <summary>
        /// 作業中分類のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbSagyoChuBunrui_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.cmbSagyoShoBunrui.Items.Clear();
            this.SagyoShoBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue),
                Convert.ToDecimal(this.cmbSagyoChuBunrui.SelectedValue));
        }

        /// <summary>
        /// 作業日区分のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbSagyobiKbn_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ChangeSagyoMode();
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

    }
}
