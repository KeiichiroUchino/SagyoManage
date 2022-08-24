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
    public partial class SagyoAnkenTorokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoAnkenTorokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 契約を指定して、本クラスの初期化を行います。
        /// </summary>
        public SagyoAnkenTorokuFrame(decimal id)
        {
            InitializeComponent();

            //メンバにセット
            this.SelectKeiyakuId = id;
        }

        /// <summary>
        /// 作業案件を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="JuchuSlipNo"></param>
        /// <param name="JuchuId"></param>
        public SagyoAnkenTorokuFrame(decimal id, decimal keiyakuId)
        {
            InitializeComponent();

            //メンバにセット
            this.SelectDtlId = id;
            this.SelectKeiyakuId = keiyakuId;
        }

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion

        #region 検索可能状態

        private SearchStateBinder searchStateBinder;

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;


        #endregion

        #region 検証イベント用

        private ControlValidatingEventRaiserCollection validatingEventRaiserCollection;
        private ValidationResultCollection validationResults = new ValidationResultCollection();

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "作業案件登録";

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
        /// 契約パラメータ
        /// </summary>
        private KeiyakuInfo _KeiyakuInfo;

        /// <summary>
        /// 作業案件クラス
        /// </summary>
        private SagyoAnken _SagyoAnken;

        /// <summary>
        /// 作業案件パラメータ
        /// </summary>
        private SagyoAnkenInfo _SagyoAnkenInfo;

        /// <summary>
        /// 作業員割当クラス
        /// </summary>
        private SagyoinWariate _SagyoinWariate;

        /// <summary>
        /// 作業員割当パラメータ
        /// </summary>
        private List<SagyoinWariateInfo> _SagyoinWariateInfoList;

        /// <summary>
        /// 車両割当クラス
        /// </summary>
        private CarWariate _CarWariate;

        /// <summary>
        /// 車両割当パラメータ
        /// </summary>
        private List<CarWariateInfo> _CarWariateInfoList;

        /// <summary>
        /// 登録用の作業員割当情報リストを保持する。
        /// </summary>
        private List<SagyoinWariateInfo> _SagyoinWariateInfoUpdList = new List<SagyoinWariateInfo>();

        /// <summary>
        /// 登録用の車両割当情報リストを保持する。
        /// </summary>
        private List<CarWariateInfo> _CarWariateInfoUpdList = new List<CarWariateInfo>();

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
        public SagyoAnkenViewFrame SagyoAnkenIchiran { get; set; }

        /// <summary>
        /// 一覧にて選択されたID(作業案件ID)
        /// </summary>
        public decimal SelectDtlId { get; set; } = decimal.Zero;

        /// <summary>
        /// 一覧にて選択されたID(契約ID)
        /// </summary>
        public decimal SelectKeiyakuId { get; set; } = decimal.Zero;

        /// <summary>
        /// 登録最大件数
        /// </summary>
        private const int REG_MAX = 1000;

        #endregion

        #region MultiRow関連

        /// <summary>
        /// 作業員明細の列名を表します。
        /// </summary>
        private enum MrowCellKeysSagyoin
        {
            RowNo,
            StaffCode,
            StaffName,
            StaffKbnName,
            ShuJitsuFlg,
            IsEdited,
        }

        /// <summary>
        /// 車両明細の列名を表します。
        /// </summary>
        private enum MrowCellKeysCar
        {
            RowNo,
            CarCode,
            LicPlateCarNo,
            CarName,
            IsEdited,
        }

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
            this.SettingCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numStaffCode, this.ShowCmnSearcStaff},
            };

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //作業案件クラスインスタンス作成
            this._SagyoAnken = new SagyoAnken(this.appAuth);

            //作業員割当クラスインスタンス作成
            this._SagyoinWariate = new SagyoinWariate(this.appAuth);

            //車両割当クラスインスタンス作成
            this._CarWariate = new CarWariate(this.appAuth);

            //コンボボックスの初期化
            this.InitCombo();

            //入力項目のクリア
            this.ClearInputs();

            // モード設定（画面間パラメータの設定有無で判定）
            if (decimal.Zero < this.SelectDtlId)
            {
                //現在の画面モードを修正状態に変更
                this.ChangeMode(FrameEditMode.Editable);
            }
            else
            {
                //現在の画面モードを初期状態に変更
                this.ChangeMode(FrameEditMode.New);
                this._SagyoAnkenInfo = new SagyoAnkenInfo();
            }

            //データ表示
            this.DoGetData();
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
            //契約情報
            this.edtKeiyakuCode.Text = string.Empty;
            this.edtKeiyakuName.Text = string.Empty;
            this.edtSagyoBashoCd.Text = string.Empty;
            this.edtSagyoBashoNm.Text = string.Empty;
            this.dteKeiyakuYMDFrom.Value = null;
            this.dteKeiyakuYMDTo.Value = null;
            this.edtSagyoDaiBunruiNm.Text = string.Empty;
            this.edtSagyoChuBunruiNm.Text = string.Empty;
            this.edtSagyoShoBunruiNm.Text = string.Empty;

            //作業案件
            this.numSagyoAnkenCode.Value = null;
            this.dteSagyoYMDFrom.Value = null;
            this.dteSagyoYMDTo.Value = null;
            this.numStaffCode.Value = null;
            this.edtStaffName.Text = string.Empty;
            this.edtBiko.Text = string.Empty;
            this.edtTokkiJiko.Text = string.Empty;

            //メンバをクリア
            this.isConfirmClose = true;

            //各明細のクリア
            this.InitHatchuDtlListMrowSagyoin();
            this.InitHatchuDtlListMrowCar();
        }

        /// <summary>
        /// 作業員割当明細のMultiRowを初期化します。
        /// </summary>
        private void InitHatchuDtlListMrowSagyoin()
        {
            //描画を停止
            this.mrsSagyoinWariateMeisai.SuspendLayout();

            try
            {
                //初期値を設定
                //MultiRowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsSagyoinWariateMeisai.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //***値の初期化
                TemplateInitializer initializer = new TemplateInitializer(this.mrsSagyoinWariateMeisai);
                initializer.Initialize();

                //テンプレートを再設定
                this.mrsSagyoinWariateMeisai.Template = tpl;

                //***ショートカットキー
                //基本設定
                this.mrsSagyoinWariateMeisai.InitialShortcutKeySetting();

                // Shift+F4制御を追加（独自にアクションクラスを作成）
                this.mrsSagyoinWariateMeisai.ShortcutKeyManager.Register(new DelegateAction(this.DoMRowLineRemoveSagyoin), Keys.F4 | Keys.Shift);

                //--DeleteキーのClearアクションを再設定
                this.mrsSagyoinWariateMeisai.ShortcutKeyManager.Unregister(Keys.Delete);

                //--単一セル選択モード
                this.mrsSagyoinWariateMeisai.MultiSelect = false;

                //--ヘッダの列幅変更を禁止する
                //---列ヘッダと行ヘッダ個別で列幅変更の可否は調整できません。 T.Kuroki@NSK
                this.mrsSagyoinWariateMeisai.AllowUserToResize = false;

                //行をクリア
                //---MultiRow.Clear() メソッドは未対応
                this.mrsSagyoinWariateMeisai.Rows.Clear();

                //--ユーザでの行追加は許可
                this.mrsSagyoinWariateMeisai.AllowUserToAddRows = true;
                //--ユーザでの行削除は許可しない
                this.mrsSagyoinWariateMeisai.AllowUserToDeleteRows = false;

                //--フォーカスが無い時はセルのハイライトを表示しない
                this.mrsSagyoinWariateMeisai.HideSelection = true;

                //--分割ボタンを表示しない
                this.mrsSagyoinWariateMeisai.SplitMode = SplitMode.None;
            }
            finally
            {
                this.mrsSagyoinWariateMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 作業員割当明細のMultiRowを初期化します。
        /// </summary>
        private void InitHatchuDtlListMrowCar()
        {
            //描画を停止
            this.mrsCarWariateMeisai.SuspendLayout();

            try
            {
                //初期値を設定
                //MultiRowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsCarWariateMeisai.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //***値の初期化
                TemplateInitializer initializer = new TemplateInitializer(this.mrsCarWariateMeisai);
                initializer.Initialize();

                //テンプレートを再設定
                this.mrsCarWariateMeisai.Template = tpl;

                //***ショートカットキー
                //基本設定
                this.mrsCarWariateMeisai.InitialShortcutKeySetting();

                // Shift+F4制御を追加（独自にアクションクラスを作成）
                this.mrsCarWariateMeisai.ShortcutKeyManager.Register(new DelegateAction(this.DoMRowLineRemoveCar), Keys.F4 | Keys.Shift);

                //--DeleteキーのClearアクションを再設定
                this.mrsCarWariateMeisai.ShortcutKeyManager.Unregister(Keys.Delete);

                //--単一セル選択モード
                this.mrsCarWariateMeisai.MultiSelect = false;

                //--ヘッダの列幅変更を禁止する
                //---列ヘッダと行ヘッダ個別で列幅変更の可否は調整できません。 T.Kuroki@NSK
                this.mrsCarWariateMeisai.AllowUserToResize = false;

                //行をクリア
                //---MultiRow.Clear() メソッドは未対応
                this.mrsCarWariateMeisai.Rows.Clear();

                //--ユーザでの行追加は許可
                this.mrsCarWariateMeisai.AllowUserToAddRows = true;
                //--ユーザでの行削除は許可しない
                this.mrsCarWariateMeisai.AllowUserToDeleteRows = false;

                //--フォーカスが無い時はセルのハイライトを表示しない
                this.mrsCarWariateMeisai.HideSelection = true;

                //--分割ボタンを表示しない
                this.mrsCarWariateMeisai.SplitMode = SplitMode.None;
            }
            finally
            {
                this.mrsCarWariateMeisai.ResumeLayout();
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

            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.dteSagyoYMDFrom, ctl => ctl.Text, this.dteSagyoYMDFrom_Validating));

            //this.mrsSagyoinWariateMeisai.RowsAdded += mrsJuchuMeisai_RowsAdded;
            //this.mrsSagyoinWariateMeisai.CellEnter += mrsJuchuMeisai_CellEnter;
            //this.mrsSagyoinWariateMeisai.CellLeave += mrsJuchuMeisai_CellLeave;
            this.mrsSagyoinWariateMeisai.EditingControlShowing += mrsSagyoinWariateMeisai_EditingControlShowing;
            this.mrsSagyoinWariateMeisai.CellValidating += mrsSagyoinWariateMeisai_CellValidating;

            this.mrsCarWariateMeisai.EditingControlShowing += mrsCarWariateMeisai_EditingControlShowing;
            this.mrsCarWariateMeisai.CellValidating += mrsCarWariateMeisai_CellValidating;
        }

        #endregion

        #region コンボ関係

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.SagyoDaiBunruiCombo();
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

            //FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoDaiBunrui, datasource, true, null, false);
            //this.cmbSagyoDaiBunrui.SelectedIndex = 0;
        }

        /// <summary>
        /// 作業中分類コンボボックスを初期化します。
        /// </summary>
        private void SagyoChuBunruiCombo(decimal id)
        {
            if (id == 0)
            {
                //this.cmbSagyoChuBunrui.Items.Clear();
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

            //FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoChuBunrui, datasource, true, null, false);
            //this.cmbSagyoChuBunrui.SelectedIndex = 0;
        }

        /// <summary>
        /// 作業小分類コンボボックスを初期化します。
        /// </summary>
        private void SagyoShoBunruiCombo(decimal id)
        {
            if (id == 0)
            {
                //this.cmbSagyoShoBunrui.Items.Clear();
                return;
            }

            // 作業小分類コンボ設定
            Dictionary<String, String> datasource = new Dictionary<String, String>();

            SagyoShoBunruiSearchParameter para = new SagyoShoBunruiSearchParameter();
            para.SagyoChuBunruiId = id;

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

            //FrameUtilites.SetupGcComboBoxForValueText(this.cmbSagyoShoBunrui, datasource, true, null, false);
            //this.cmbSagyoShoBunrui.SelectedIndex = 0;
        }

        private void mrsSagyoinWariateSideButton_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchJuchuNyuryokuList(this.mrsSagyoinWariateMeisai);
        }

        private void mrsCarWariateSideButton_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchJuchuNyuryokuList(this.mrsCarWariateMeisai);
        }

        /// <summary>
        /// 明細のコード値検索画面を表示します。
        /// </summary>
        /// <param name="curMRow"></param>
        private void ShowCmnSearchJuchuNyuryokuList(GcMultiRow curMRow)
        {
            GrapeCity.Win.MultiRow.Cell curCell = curMRow.CurrentCell;
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;

            int curRowIndex = curRow.Index;

            if (curRow == null || curCell == null)
            {
                return;
            }

            if (!curCell.Selectable || curCell.ReadOnly)
            {
                return;
            }

            //使用可否フラグ
            bool isDisable = false;

            // 社員コードの場合
            if (curCell.Name == MrowCellKeysSagyoin.StaffCode.ToString())
            {
                using (CmnSearchStaffFrame f = new CmnSearchStaffFrame())
                {
                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        isDisable = f.SelectedInfo.DisableFlag;
                        if (!isDisable)
                        {
                            curMRow.BeginEdit(f.SelectedInfo.StaffCode);
                            this.OnCmnSearchComplete();
                        }
                    }
                }
            }

            // 車両コードの場合
            else if (curCell.Name == MrowCellKeysCar.CarCode.ToString())
            {
                using (CmnSearchCarFrame f = new CmnSearchCarFrame())
                {
                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        isDisable = f.SelectedInfo.DisableFlag;
                        if (!isDisable)
                        {
                            curMRow.BeginEdit(f.SelectedInfo.CarCode);
                            this.OnCmnSearchComplete();
                        }
                    }
                }
            }

            if (isDisable)
            {
                //未使用データの場合は、エラー
                DialogResult mq_result = MessageBox.Show(
                    FrameUtilites.GetDefineMessage("MW2201016"),
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }


        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            string[] SearchCellsSagyoin = new string[] { MrowCellKeysSagyoin.StaffCode.ToString()};
            string[] SearchCellsCar = new string[] {MrowCellKeysCar.CarCode.ToString()};

            this.searchStateBinder.AddSearchableControls(
                this.numStaffCode);

            this.searchStateBinder.AddSearchableControls(this.mrsSagyoinWariateMeisai, SearchCellsSagyoin);
            this.searchStateBinder.AddSearchableControls(this.mrsCarWariateMeisai, SearchCellsCar);
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

        private void SagyoAnkenTorokuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void mrsSagyoinWariateMeisai_CellValidating(object sender, CellValidatingEventArgs e)
        {
            this.ProcessSagyoinWariateMeisaiMrowCellValidating(e);
        }


        private void mrsCarWariateMeisai_CellValidating(object sender, CellValidatingEventArgs e)
        {
            this.ProcessCarWariateMeisaiMrowCellValidating(e);
        }

        private void mrsSagyoinWariateMeisai_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            //サイドボタンが存在する場合サイドボタンイベントを設定。
            GrapeCity.Win.Editors.SideButton sideButton = null;
            if (e.Control is GcNumberEditingControl && (e.Control as GcNumberEditingControl).SideButtons.Count != 0)
            {
                sideButton = (e.Control as GcNumberEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;
            }
            else if (e.Control is GcTextBoxEditingControl && (e.Control as GcTextBoxEditingControl).SideButtons.Count != 0)
            {
                sideButton = (e.Control as GcTextBoxEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;
            }
            if (null != sideButton)
            {
                sideButton.Click -= new EventHandler(mrsSagyoinWariateSideButton_Click);
                sideButton.Click += new EventHandler(mrsSagyoinWariateSideButton_Click);
                e.Control.DoubleClick -= new EventHandler(mrsSagyoinWariateSideButton_Click);
                e.Control.DoubleClick += new EventHandler(mrsSagyoinWariateSideButton_Click);
            }
        }

        private void mrsCarWariateMeisai_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            //サイドボタンが存在する場合サイドボタンイベントを設定。
            GrapeCity.Win.Editors.SideButton sideButton = null;
            if (e.Control is GcNumberEditingControl && (e.Control as GcNumberEditingControl).SideButtons.Count != 0)
            {
                sideButton = (e.Control as GcNumberEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;
            }
            else if (e.Control is GcTextBoxEditingControl && (e.Control as GcTextBoxEditingControl).SideButtons.Count != 0)
            {
                sideButton = (e.Control as GcTextBoxEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;
            }
            if (null != sideButton)
            {
                sideButton.Click -= new EventHandler(mrsCarWariateSideButton_Click);
                sideButton.Click += new EventHandler(mrsCarWariateSideButton_Click);
                e.Control.DoubleClick -= new EventHandler(mrsCarWariateSideButton_Click);
                e.Control.DoubleClick += new EventHandler(mrsCarWariateSideButton_Click);
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
                    ((NskDateTime)c).Value = Convert.ToDateTime(((NskDateTime)c).Value).AddHours(days);
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
                            //作業案件 登録・更新
                            this._SagyoAnken.Save(tx, this._SagyoAnkenInfo);

                            if (FrameEditMode.Editable == this.currentMode)
                            {
                                //作業員割当 削除
                                this._SagyoinWariate.Delete(tx, null, this._SagyoAnkenInfo.SagyoAnkenId);

                                //車両割当 削除
                                this._CarWariate.Delete(tx, null, this._SagyoAnkenInfo.SagyoAnkenId);
                            }

                            //作業員割当 登録・更新
                            this._SagyoinWariate.Save(tx, this._SagyoAnkenInfo.SagyoAnkenId, this._SagyoinWariateInfoUpdList);

                            //車両割当 登録・更新
                            this._CarWariate.Save(tx, this._SagyoAnkenInfo.SagyoAnkenId, this._CarWariateInfoUpdList);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "作業案件登録", this.numSagyoAnkenCode.Text });

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

                        if (this.SagyoAnkenIchiran != null)
                        {
                            //呼び出し元のPGの再表示を指示
                            this.SagyoAnkenIchiran.ReSetScreen(this.SelectKeiyakuId);
                        }

                        // 画面を閉じる
                        this.isConfirmClose = false;
                        this.DoClose();

                    }
                    catch (CanRetryException ex)
                    {
                        //フォーカスを移動
                        this.dteSagyoYMDFrom.Focus();
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
                        this.dteSagyoYMDFrom.Focus();
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
        /// 選択中の作業員割当明細を１行削除します。
        /// </summary>
        private void DoMRowLineRemoveSagyoin(GcMultiRow curMRow)
        {
            //現在の行の位置を取得しておく
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;

            //現在行が最後の行の時は削除は許可しない(自動行挿入可の場合)
            //自動行挿入不可の場合はNoチェック
            if (curRow != null && this.mrsSagyoinWariateMeisai.RowCount - 1 != curRow.Index)
            {
                //処理前に確認メッセージ
                DialogResult wk_result =
                    MessageBox.Show(
                        //FrameUtilites.GetDefineMessage("MQ2102005"),
                        "選択された行を削除してもよろしいですか？",
                        "確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (wk_result == DialogResult.Yes)
                {
                    //行を削除
                    this.mrsSagyoinWariateMeisai.Rows.RemoveAt(curRow.Index);
                }
            }
        }

        /// <summary>
        /// 選択中の車両割当明細を１行削除します。
        /// </summary>
        private void DoMRowLineRemoveCar(GcMultiRow curMRow)
        {
            //現在の行の位置を取得しておく
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;

            //現在行が最後の行の時は削除は許可しない(自動行挿入可の場合)
            //自動行挿入不可の場合はNoチェック
            if (curRow != null && this.mrsCarWariateMeisai.RowCount - 1 != curRow.Index)
            {
                //処理前に確認メッセージ
                DialogResult wk_result =
                    MessageBox.Show(
                        //FrameUtilites.GetDefineMessage("MQ2102005"),
                        "選択された行を削除してもよろしいですか？",
                        "確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (wk_result == DialogResult.Yes)
                {
                    //行を削除
                    this.mrsCarWariateMeisai.Rows.RemoveAt(curRow.Index);
                }
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
                        //作業案件
                        this._SagyoAnken.Delete(tx, this._SagyoAnkenInfo);

                        //作業員割当
                        this._SagyoinWariate.Delete(tx, null, this._SagyoAnkenInfo.SagyoAnkenId);

                        //車両割当
                        this._CarWariate.Delete(tx, null, this._SagyoAnkenInfo.SagyoAnkenId);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken = FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "作業案件登録", this.numSagyoAnkenCode.Text });

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

                    if (this.SagyoAnkenIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.SagyoAnkenIchiran.ReSetScreen(this.SelectKeiyakuId);
                    }

                    // 画面を閉じる
                    this.isConfirmClose = false;
                    this.DoClose();

                }
                catch (CanRetryException ex)
                {
                    //データがない場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);

                    //フォーカスを移動
                    this.dteSagyoYMDFrom.Focus();
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
                    this.dteSagyoYMDFrom.Focus();
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
            this._SagyoAnkenInfo.KeiyakuId = this.SelectKeiyakuId;

            this._SagyoAnkenInfo.SagyoAnkenId = this.SelectDtlId;
            this._SagyoAnkenInfo.SagyoAnkenCode = Convert.ToInt32(this.numSagyoAnkenCode.Value);

            // 作業日時（From）
            if (this.dteSagyoYMDFrom.Value != null)
            {
                this._SagyoAnkenInfo.SagyoStartDateTime = this.dteSagyoYMDFrom.Value.Value;
            }
            else
            {
                this._SagyoAnkenInfo.SagyoStartDateTime = DateTime.MinValue;
            }

            // 作業日時（To）
            if (this.dteSagyoYMDTo.Value != null)
            {
                this._SagyoAnkenInfo.SagyoEndDateTime = this.dteSagyoYMDTo.Value.Value;
            }
            else
            {
                this._SagyoAnkenInfo.SagyoEndDateTime = DateTime.MinValue;
            }

            this._SagyoAnkenInfo.SekininshaId = Convert.ToDecimal(numStaffCode.Tag);
            this._SagyoAnkenInfo.Biko = this.edtBiko.Text;
            this._SagyoAnkenInfo.TokkiJiko = this.edtTokkiJiko.Text;

            this.GetSagyoinWariateDtlList();
            this.GetCarWariateDtlList();
        }

        /// <summary>
        /// 作業員割当明細から登録に必要な項目を画面コントローラにセットします。
        /// </summary>
        private void GetSagyoinWariateDtlList()
        {
            this._SagyoinWariateInfoUpdList = new List<SagyoinWariateInfo>();

            //描画を停止
            this.mrsSagyoinWariateMeisai.SuspendLayout();
            //自動行追加機能をオフ
            this.mrsSagyoinWariateMeisai.AllowUserToAddRows = false;

            try
            {
                //明細を回しながら作業員割当情報作成
                int rowcount = this.mrsSagyoinWariateMeisai.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {

                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsSagyoinWariateMeisai.Rows[i];

                    if (wk_mrow.IsNewRow)
                        continue;

                    //1件分の受注情報を作成
                    SagyoinWariateInfo info = new SagyoinWariateInfo();

                    // 修正データは行コレクションのTagから復元する
                    if (this.mrsSagyoinWariateMeisai.Rows[i].Tag != null)
                    {
                        info = (SagyoinWariateInfo)wk_mrow.Tag;
                    }

                    //社員ID
                    info.StaffId = Convert.ToDecimal(this.mrsSagyoinWariateMeisai.Rows[i].Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag);

                    //終日フラグ
                    info.ShuJitsuFlg = Convert.ToBoolean(this.mrsSagyoinWariateMeisai.GetValue(i, MrowCellKeysSagyoin.ShuJitsuFlg.ToString()));

                    this._SagyoinWariateInfoUpdList.Add(info);
                }

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //自動行追加機能をオン
                this.mrsSagyoinWariateMeisai.AllowUserToAddRows = true;
                //描画を再開
                this.mrsSagyoinWariateMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 車両割当明細から登録に必要な項目を画面コントローラにセットします。
        /// </summary>
        private void GetCarWariateDtlList()
        {
            this._CarWariateInfoUpdList = new List<CarWariateInfo>();

            //描画を停止
            this.mrsCarWariateMeisai.SuspendLayout();
            //自動行追加機能をオフ
            this.mrsCarWariateMeisai.AllowUserToAddRows = false;

            try
            {
                //明細を回しながら車両割当情報作成
                int rowcount = this.mrsCarWariateMeisai.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsCarWariateMeisai.Rows[i];

                    if (wk_mrow.IsNewRow)
                        continue;

                    //1件分の受注情報を作成
                    CarWariateInfo info = new CarWariateInfo();

                    // 修正データは行コレクションのTagから復元する
                    if (this.mrsCarWariateMeisai.Rows[i].Tag != null)
                    {
                        info = (CarWariateInfo)wk_mrow.Tag;
                    }

                    //車両ID
                    info.CarId = Convert.ToDecimal(this.mrsCarWariateMeisai.Rows[i].Cells[MrowCellKeysCar.CarCode.ToString()].Tag);

                    this._CarWariateInfoUpdList.Add(info);
                }

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //自動行追加機能をオン
                this.mrsCarWariateMeisai.AllowUserToAddRows = true;
                //描画を再開
                this.mrsCarWariateMeisai.ResumeLayout();
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

            //作業開始日時の必須チェック
            if (rt_val && this.dteSagyoYMDFrom.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "作業開始日時" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteSagyoYMDFrom;
            }

            //作業終了日時の必須チェック
            if (rt_val && this.dteSagyoYMDTo.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "作業終了日時" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteSagyoYMDTo;
            }

            //作業日時の反転チェック
            if (rt_val && this.dteSagyoYMDTo.Value < this.dteSagyoYMDFrom.Value)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "作業日時" });
                icon = MessageBoxIcon.Warning;
                ctl = this.dteSagyoYMDTo;
            }

            //作業案件情報取得
            SagyoAnkenSearchParameter para = new SagyoAnkenSearchParameter();
            para.SearchKbn = (int)SagyoAnken.SearchKbnEnum.Check;
            para.KeiyakuId = this.SelectKeiyakuId;
            para.SagyoAnkenId = this.SelectDtlId;
            para.SagyoStartDateTime = this.dteSagyoYMDFrom.Value;
            para.SagyoEndDateTime = this.dteSagyoYMDTo.Value;
            var listSa = this._SagyoAnken.GetList(para);

            //作業日時の重複チェック
            if (rt_val && listSa.Count() > 0)
            {
                rt_val = false;
                msg = "登録済みの作業案件と重複する作業日時になっています。";
                icon = MessageBoxIcon.Warning;
                ctl = this.dteSagyoYMDTo;
            }

            //作業員の必須チェック
            if (rt_val && this.mrsSagyoinWariateMeisai.Rows.Where(x => !x.IsNewRow).Count() == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "作業員" });
                icon = MessageBoxIcon.Warning;
                ctl = this.mrsSagyoinWariateMeisai;
            }

            //作業員の上限件数チェック
            if (rt_val && REG_MAX < this.mrsSagyoinWariateMeisai.Rows.Where(x => !x.IsNewRow).Count())
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "ME2303003", new string[] { "作業員" , REG_MAX.ToString() });
                icon = MessageBoxIcon.Warning;
                ctl = this.mrsSagyoinWariateMeisai;
            }

            //車両の上限件数チェック
            if (rt_val && REG_MAX < this.mrsCarWariateMeisai.Rows.Where(x => !x.IsNewRow).Count())
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "ME2303003", new string[] { "車両", REG_MAX.ToString() });
                icon = MessageBoxIcon.Warning;
                ctl = this.mrsCarWariateMeisai;
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

            //作業員明細のチェック
            if (rt_val && this.mrsSagyoinWariateMeisai.RowCount > 0)
            {
                int mrowidx = 0;
                MrowCellKeysSagyoin colidx = MrowCellKeysSagyoin.StaffCode;

                List<decimal> idList = new List<decimal>();
                Dictionary<decimal, int> idDic = new Dictionary<decimal, int>();

                int rowcount = this.mrsSagyoinWariateMeisai.RowCount;
                for (int i = 0; i < rowcount; i++)
                {

                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsSagyoinWariateMeisai.Rows[i];

                    if (wk_mrow.IsNewRow)
                        continue;

                    //社員ID
                    decimal id = Convert.ToDecimal(this.mrsSagyoinWariateMeisai.Rows[i].Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag);
                    idList.Add(id);
                    idDic.Add(id, i);

                    //必須チェック
                    if (id < 1)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                                "MW2203004", new string[] { "作業員" });
                        icon = MessageBoxIcon.Warning;
                        mrowidx = i;
                        colidx = MrowCellKeysSagyoin.StaffCode;
                        break;
                    }
                }

                if (rt_val)
                {
                    //作業員割当情報取得
                    SagyoinWariateSearchParameter sagyoinPara = new SagyoinWariateSearchParameter();
                    sagyoinPara.SearchKbn = (int)SagyoinWariate.SearchKbnEnum.Check;
                    sagyoinPara.SagyoAnkenId = this.SelectDtlId;
                    sagyoinPara.StaffIdList = idList;
                    sagyoinPara.SagyoStartDateTime = this.dteSagyoYMDFrom.Value;
                    sagyoinPara.SagyoEndDateTime = this.dteSagyoYMDTo.Value;

                    var list = this._SagyoinWariate.GetList(sagyoinPara).ToList();

                    //作業員重複(他業務)チェック
                    if (list.Count > 0)
                    {
                        rt_val = false;
                        msg = "他案件と作業時間が重複している作業員がいます。";
                        icon = MessageBoxIcon.Warning;
                        mrowidx = idDic[list.First().StaffId];
                        colidx = MrowCellKeysSagyoin.StaffCode;
                    }
                }

                if (!rt_val)
                {
                    //アイコンの種類によってMassageBoxTitleを変更
                    string msg_title = string.Empty;

                    //エラーメッセージ表示
                    if (msg.Trim().Length > 0)
                    {
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

                        //エラーメッセージ表示
                        MessageBox.Show(
                            msg, msg_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //選択可能の場合のみフォーカスを遷移する
                    if (this.mrsSagyoinWariateMeisai[mrowidx, colidx.ToString()].Selectable)
                    {
                        this.mrsSagyoinWariateMeisai.CurrentCellPosition =
                            new CellPosition(mrowidx, colidx.ToString());
                    }

                    this.mrsSagyoinWariateMeisai.Focus();
                }
            }

            //車両明細のチェック
            if (rt_val && this.mrsCarWariateMeisai.Rows.Where(x => !x.IsNewRow).Count() > 0)
            {
                int mrowidx = 0;
                MrowCellKeysCar colidx = MrowCellKeysCar.CarCode;

                List<decimal> idList = new List<decimal>();
                Dictionary<decimal, int> idDic = new Dictionary<decimal, int>();

                int rowcount = this.mrsCarWariateMeisai.RowCount;
                for (int i = 0; i < rowcount; i++)
                {

                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsCarWariateMeisai.Rows[i];

                    if (wk_mrow.IsNewRow)
                        continue;

                    //車両ID
                    decimal id = Convert.ToDecimal(this.mrsCarWariateMeisai.Rows[i].Cells[MrowCellKeysCar.CarCode.ToString()].Tag);
                    idList.Add(id);
                    idDic.Add(id, i);

                    //必須チェック
                    if (id < 1)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                                "MW2203004", new string[] { "車両" });
                        icon = MessageBoxIcon.Warning;
                        mrowidx = i;
                        colidx = MrowCellKeysCar.CarCode;
                        break;
                    }

                }

                if (rt_val)
                {
                    //車両割当情報取得
                    CarWariateSearchParameter carPara = new CarWariateSearchParameter();
                    carPara.SearchKbn = (int)CarWariate.SearchKbnEnum.Check;
                    carPara.SagyoAnkenId = this.SelectDtlId;
                    carPara.CarIdList = idList;
                    carPara.SagyoStartDateTime = this.dteSagyoYMDFrom.Value;
                    carPara.SagyoEndDateTime = this.dteSagyoYMDTo.Value;

                    var list = this._CarWariate.GetList(carPara).ToList();

                    //車両重複(他業務)チェック
                    if (list.Count > 0)
                    {
                        rt_val = false;
                        msg = "他案件と作業時間が重複している車両があります。";
                        icon = MessageBoxIcon.Warning;
                        mrowidx = idDic[list.First().CarId];
                        colidx = MrowCellKeysCar.CarCode;
                    }
                }

                if (!rt_val)
                {
                    //アイコンの種類によってMassageBoxTitleを変更
                    string msg_title = string.Empty;

                    //エラーメッセージ表示
                    if (msg.Trim().Length > 0)
                    {
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

                        //エラーメッセージ表示
                        MessageBox.Show(
                            msg, msg_title,
                            MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    //選択可能の場合のみフォーカスを遷移する
                    if (this.mrsCarWariateMeisai[mrowidx, colidx.ToString()].Selectable)
                    {
                        this.mrsCarWariateMeisai.CurrentCellPosition =
                            new CellPosition(mrowidx, colidx.ToString());
                    }

                    this.mrsCarWariateMeisai.Focus();
                }
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
                    if (this.SagyoAnkenIchiran != null)
                    {
                        //呼び出し元のPGの再表示を指示
                        this.SagyoAnkenIchiran.ReSetScreen(this.SelectKeiyakuId);
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
            else if (FrameEditMode.Editable == this.currentMode)
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
            // 作業案件情報取得
            this._KeiyakuInfo = this._DalUtil.Keiyaku.GetInfoById(this.SelectKeiyakuId);

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

            if (FrameEditMode.Editable == this.currentMode)
            {
                // 作業案件情報取得
                this._SagyoAnkenInfo = this._SagyoAnken.GetInfoById(this.SelectDtlId);

                // 作業員割当情報取得
                SagyoinWariateSearchParameter sagyoinPara = new SagyoinWariateSearchParameter();
                sagyoinPara.SearchKbn =(int) SagyoinWariate.SearchKbnEnum.Basic;
                sagyoinPara.SagyoAnkenId = this.SelectDtlId;
                this._SagyoinWariateInfoList = this._SagyoinWariate.GetList(sagyoinPara).ToList();

                // 車両割当情報取得
                CarWariateSearchParameter carPara = new CarWariateSearchParameter();
                carPara.SearchKbn = (int)CarWariate.SearchKbnEnum.Basic;
                carPara.SagyoAnkenId = this.SelectDtlId;
                this._CarWariateInfoList = this._CarWariate.GetList(carPara).ToList();
            }

            this.SetScreen();

            //フォーカスを移動
            this.dteSagyoYMDFrom.Focus();
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
                    this.btnDelete.Enabled = false;
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
                    this.btnDelete.Enabled = true;
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
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            //契約情報
            this.edtKeiyakuCode.Text = this._KeiyakuInfo.KeiyakuCode;
            this.edtKeiyakuName.Text = this._KeiyakuInfo.KeiyakuName;
            this.edtSagyoBashoCd.Text = this._KeiyakuInfo.SagyoBashoCode;
            this.edtSagyoBashoNm.Text = this._KeiyakuInfo.SagyoBashoName;
            this.dteKeiyakuYMDFrom.Value = this._KeiyakuInfo.KeiyakuStartDate;
            this.dteKeiyakuYMDTo.Value = this._KeiyakuInfo.KeiyakuEndDate;
            this.edtSagyoDaiBunruiNm.Text = this._KeiyakuInfo.SagyoDaiBunruiName;
            this.edtSagyoChuBunruiNm.Text = this._KeiyakuInfo.SagyoChuBunruiName;
            this.edtSagyoShoBunruiNm.Text = this._KeiyakuInfo.SagyoShoBunruiName;

            if (FrameEditMode.Editable == this.currentMode)
            {
                //作業案件情報
                if (this._SagyoAnkenInfo == null)
                {
                    this.numSagyoAnkenCode.Value = null;
                    this.dteSagyoYMDFrom.Value = null;
                    this.dteSagyoYMDTo.Value = null;
                    this.edtBiko.Text = string.Empty;
                    this.edtTokkiJiko.Text = string.Empty;
                }
                else
                {
                    this.numSagyoAnkenCode.Value = this._SagyoAnkenInfo.SagyoAnkenCode;
                    this.dteSagyoYMDFrom.Value = this._SagyoAnkenInfo.SagyoStartDateTime;
                    this.dteSagyoYMDTo.Value = this._SagyoAnkenInfo.SagyoEndDateTime;
                    this.edtBiko.Text = this._SagyoAnkenInfo.Biko;
                    this.edtTokkiJiko.Text = this._SagyoAnkenInfo.TokkiJiko;

                    this.numStaffCode.Value = this._SagyoAnkenInfo.StaffCode;
                    this.numStaffCode.Tag = this._SagyoAnkenInfo.SekininshaId;
                    this.edtStaffName.Text = this._SagyoAnkenInfo.StaffName;
                }

                //作業員割当明細を設定
                this.SetScreenSagyoinWariateMeisai();

                //車両割当明細を設定
                this.SetScreenCarWariateMeisai();
            }

        }

        /// <summary>
        /// 作業員割当明細をセットします。
        /// </summary>
        private void SetScreenSagyoinWariateMeisai()
        {
            try
            {
                //件数取得
                int rowcount = this._SagyoinWariateInfoList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //行数を設定
                this.mrsSagyoinWariateMeisai.RowCount = rowcount + 1;

                for (int i = 0; i < rowcount; i++)
                {
                    //1件分の受注情報を取得
                    SagyoinWariateInfo info = this._SagyoinWariateInfoList[i];

                    //連番
                    this.mrsSagyoinWariateMeisai.SetValue(
                        i, MrowCellKeysSagyoin.RowNo.ToString(), i + 1);

                    //社員コード
                    this.mrsSagyoinWariateMeisai.SetValue(
                        i, MrowCellKeysSagyoin.StaffCode.ToString(), info.StaffCode);
                    //社員ID
                    this.mrsSagyoinWariateMeisai.Rows[i].Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag = info.StaffId;

                    //社員名
                    this.mrsSagyoinWariateMeisai.SetValue(
                        i, MrowCellKeysSagyoin.StaffName.ToString(), info.StaffName);

                    //社員区分
                    this.mrsSagyoinWariateMeisai.SetValue(
                        i, MrowCellKeysSagyoin.StaffKbnName.ToString(), info.StaffKbnName);

                    //終日フラグ
                    this.mrsSagyoinWariateMeisai.SetValue(
                        i, MrowCellKeysSagyoin.ShuJitsuFlg.ToString(), info.ShuJitsuFlg);

                    this.mrsSagyoinWariateMeisai.Rows[i].Tag = info;

                }

                //セルポジション（初期値）
                CellPosition cposition =
                    new CellPosition(0, MrowCellKeysSagyoin.StaffCode.ToString());

                //表示行設定
                this.mrsSagyoinWariateMeisai.CurrentCellPosition = cposition;
                this.mrsSagyoinWariateMeisai.FirstDisplayedCellPosition = cposition;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsSagyoinWariateMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 車両割当明細をセットします。
        /// </summary>
        private void SetScreenCarWariateMeisai()
        {
            try
            {
                //件数取得
                int rowcount = this._CarWariateInfoList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //行数を設定
                this.mrsCarWariateMeisai.RowCount = rowcount + 1;

                for (int i = 0; i < rowcount; i++)
                {
                    //1件分の受注情報を取得
                    CarWariateInfo info = this._CarWariateInfoList[i];

                    //Mrowにセット
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsCarWariateMeisai.Rows[i];

                    //連番
                    this.mrsCarWariateMeisai.SetValue(
                        i, MrowCellKeysCar.RowNo.ToString(), i + 1);

                    //車両コード
                    this.mrsCarWariateMeisai.SetValue(
                        i, MrowCellKeysCar.CarCode.ToString(), info.CarCode);
                    //車両ID
                    this.mrsCarWariateMeisai.Rows[i].Cells[MrowCellKeysCar.CarCode.ToString()].Tag = info.CarId;

                    //車番
                    this.mrsCarWariateMeisai.SetValue(
                        i, MrowCellKeysCar.LicPlateCarNo.ToString(), info.LicPlateCarNo);

                    //車両名
                    this.mrsCarWariateMeisai.SetValue(
                        i, MrowCellKeysCar.CarName.ToString(), info.CarName);

                    this.mrsCarWariateMeisai.Rows[i].Tag = info;
                }

                //セルポジション（初期値）
                CellPosition cposition =
                    new CellPosition(0, MrowCellKeysCar.CarCode.ToString());

                //表示行設定
                this.mrsCarWariateMeisai.CurrentCellPosition = cposition;
                this.mrsCarWariateMeisai.FirstDisplayedCellPosition = cposition;

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsCarWariateMeisai.ResumeLayout();
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
                    if (mrsSagyoinWariateMeisai.ContainsFocus)
                    {
                        //キーボードイベントの抑止
                        e.Handled = true;
                        this.ShowCmnSearchJuchuNyuryokuList(this.mrsSagyoinWariateMeisai);
                    }
                    else if (mrsCarWariateMeisai.ContainsFocus)
                    {
                        //キーボードイベントの抑止
                        e.Handled = true;
                        this.ShowCmnSearchJuchuNyuryokuList(this.mrsCarWariateMeisai);
                    }
                    else
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

        #region CellValidatingの処理

        /// <summary>
        /// 作業割当明細一覧のMrowのCellValidatingイベントを処理します。
        /// </summary>
        /// <param name="e">CellValidatingイベントのイベントデータ</param>
        private void ProcessSagyoinWariateMeisaiMrowCellValidating(CellValidatingEventArgs e)
        {
            //編集された場合のみ処理する
            if (!this.mrsSagyoinWariateMeisai.IsCurrentCellInEditMode)
            {
                return;
            }

            //描画を止める
            this.mrsSagyoinWariateMeisai.SuspendLayout();

            try
            {
                //現在アクティブなセルのキーを取得
                string wk_curcellkey = this.mrsSagyoinWariateMeisai.CurrentCell.Name;

                // 社員コード
                if (wk_curcellkey == MrowCellKeysSagyoin.StaffCode.ToString())
                {
                    e.Cancel = this.StaffCdCellValidating(e.RowIndex);
                }

                if (e.Cancel)
                {
                    //クリア
                    EditingActions.ClearEdit.Execute(this.mrsSagyoinWariateMeisai);

                    //前の値に戻す前にいったん確定する
                    EditingActions.EndEdit.Execute(this.mrsSagyoinWariateMeisai);

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsSagyoinWariateMeisai);
                }
            }
            finally
            {
                //描画を再開
                this.mrsSagyoinWariateMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 作業員割当明細の社員コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool StaffCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsSagyoinWariateMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            try
            {
                //セル値
                int cell_code =
                    Convert.ToInt32(this.mrsSagyoinWariateMeisai.CurrentCell.Value);
                //編集値
                int editing_code =
                    Convert.ToInt32(
                        GetEditingControlValue(this.mrsSagyoinWariateMeisai.EditingControl));

                //値が変更されたか？
                bool valueChanged = cell_code != editing_code;

                if (valueChanged)
                {
                    if (editing_code == 0)
                    {
                        //未入力時はクリアのみ
                        is_clear = true;
                    }
                    else
                    {
                        //社員情報を取得
                        StaffInfo info = this._DalUtil.Staff.GetInfo(editing_code);

                        if (info == null)
                        {
                            //編集をキャンセル
                            rt_val = true;

                            MessageBox.Show(
                               FrameUtilites.GetDefineMessage("MW2201003"),
                               "警告",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);

                            //情報クリア
                            is_clear = true;
                        }
                        else
                        {
                            if (info.DisableFlag)
                            {
                                //編集をキャンセル
                                rt_val = true;

                                MessageBox.Show(
                                    FrameUtilites.GetDefineMessage("MW2201016"),
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                is_clear = true;
                            }
                            else
                            {
                                //重複チェック
                                int rowcount = this.mrsSagyoinWariateMeisai.Rows.Count;
                                for (int i = 0; i < rowcount; i++)
                                {
                                    //1行分を取得
                                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsSagyoinWariateMeisai.Rows[i];

                                    if (wk_mrow.IsNewRow)
                                        continue;

                                    // 修正データは行コレクションのTagから復元する
                                    if (info.StaffId == Convert.ToDecimal(wk_mrow.Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag))
                                    {
                                        //編集をキャンセル
                                        rt_val = true;

                                        MessageBox.Show(
                                            FrameUtilites.GetDefineMessage("MW2203010", new string[] { "作業員" }),
                                            this.Text,
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        is_clear = true;
                                        break;
                                    }
                                }

                                if (!rt_val)
                                {
                                    //名称セット
                                    this.mrsSagyoinWariateMeisai.SetValue(
                                        rowIndex, MrowCellKeysSagyoin.StaffName.ToString(), info.StaffName);

                                    //社員ID
                                    this.mrsSagyoinWariateMeisai.Rows[rowIndex].Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag = info.StaffId;

                                    //社員区分名称セット
                                    this.mrsSagyoinWariateMeisai.SetValue(
                                        rowIndex, MrowCellKeysSagyoin.StaffKbnName.ToString(), info.StaffKbnName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (is_clear)
                {
                    //情報クリア
                    this.mrsSagyoinWariateMeisai.SetValue(
                        rowIndex, MrowCellKeysSagyoin.StaffName.ToString(), string.Empty);
                    this.mrsSagyoinWariateMeisai.Rows[rowIndex].Cells[MrowCellKeysSagyoin.StaffCode.ToString()].Tag = null;
                    this.mrsSagyoinWariateMeisai.SetValue(
                        rowIndex, MrowCellKeysSagyoin.StaffKbnName.ToString(), string.Empty);
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 車両割当明細一覧のMrowのCellValidatingイベントを処理します。
        /// </summary>
        /// <param name="e">CellValidatingイベントのイベントデータ</param>
        private void ProcessCarWariateMeisaiMrowCellValidating(CellValidatingEventArgs e)
        {
            //編集された場合のみ処理する
            if (!this.mrsCarWariateMeisai.IsCurrentCellInEditMode)
            {
                return;
            }

            //描画を止める
            this.mrsCarWariateMeisai.SuspendLayout();

            try
            {
                //現在アクティブなセルのキーを取得
                string wk_curcellkey = this.mrsCarWariateMeisai.CurrentCell.Name;

                // 車両コード
                if (wk_curcellkey == MrowCellKeysCar.CarCode.ToString())
                {
                    e.Cancel = this.CarCdCellValidating(e.RowIndex);
                }

                if (e.Cancel)
                {
                    //クリア
                    EditingActions.ClearEdit.Execute(this.mrsCarWariateMeisai);

                    //前の値に戻す前にいったん確定する
                    EditingActions.EndEdit.Execute(this.mrsCarWariateMeisai);

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsCarWariateMeisai);
                }
            }
            finally
            {
                //描画を再開
                this.mrsCarWariateMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 車両割当明細の車両コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool CarCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsCarWariateMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            try
            {
                //セル値
                int cell_code =
                    Convert.ToInt32(this.mrsCarWariateMeisai.CurrentCell.Value);
                //編集値
                int editing_code =
                    Convert.ToInt32(
                        GetEditingControlValue(this.mrsCarWariateMeisai.EditingControl));

                //値が変更されたか？
                bool valueChanged = cell_code != editing_code;

                if (valueChanged)
                {
                    if (editing_code == 0)
                    {
                        //未入力時はクリアのみ
                        is_clear = true;
                    }
                    else
                    {
                        //車両情報を取得
                        CarInfo info = this._DalUtil.Car.GetInfo(editing_code);

                        if (info == null)
                        {
                            //編集をキャンセル
                            rt_val = true;

                            MessageBox.Show(
                               FrameUtilites.GetDefineMessage("MW2201003"),
                               "警告",
                               MessageBoxButtons.OK,
                               MessageBoxIcon.Warning);

                            //情報クリア
                            is_clear = true;
                        }
                        else
                        {
                            if (info.DisableFlag)
                            {
                                //編集をキャンセル
                                rt_val = true;

                                MessageBox.Show(
                                    FrameUtilites.GetDefineMessage("MW2201016"),
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);

                                is_clear = true;
                            }
                            else
                            {
                                //重複チェック
                                int rowcount = this.mrsCarWariateMeisai.Rows.Count;
                                for (int i = 0; i < rowcount; i++)
                                {
                                    //1行分を取得
                                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsCarWariateMeisai.Rows[i];

                                    if (wk_mrow.IsNewRow)
                                        continue;

                                    // 修正データは行コレクションのTagから復元する
                                    if (info.CarId == Convert.ToDecimal(wk_mrow.Cells[MrowCellKeysCar.CarCode.ToString()].Tag))
                                    {
                                        //編集をキャンセル
                                        rt_val = true;

                                        MessageBox.Show(
                                            FrameUtilites.GetDefineMessage("MW2203010", new string[] { "車両" }),
                                            this.Text,
                                            MessageBoxButtons.OK,
                                            MessageBoxIcon.Warning);

                                        is_clear = true;
                                        break;
                                    }
                                }

                                if (!rt_val)
                                {
                                    //車両IDセット
                                    this.mrsCarWariateMeisai.Rows[rowIndex].Cells[MrowCellKeysCar.CarCode.ToString()].Tag = info.CarId;

                                    //車番セット
                                    this.mrsCarWariateMeisai.SetValue(
                                        rowIndex, MrowCellKeysCar.LicPlateCarNo.ToString(), info.LicPlateCarNo);

                                    //車両名称セット
                                    this.mrsCarWariateMeisai.SetValue(
                                        rowIndex, MrowCellKeysCar.CarName.ToString(), info.CarName);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (is_clear)
                {
                    //情報クリア
                    this.mrsCarWariateMeisai.Rows[rowIndex].Cells[MrowCellKeysCar.CarCode.ToString()].Tag = null;
                    this.mrsCarWariateMeisai.SetValue(
                        rowIndex, MrowCellKeysCar.LicPlateCarNo.ToString(), string.Empty);
                    this.mrsCarWariateMeisai.SetValue(
                        rowIndex, MrowCellKeysCar.CarName.ToString(), string.Empty);
                }
            }

            return rt_val;
        }

        /// <summary>
        /// MultiRowの編集コントロールからCell.Valueに相当するものを取得します。
        /// </summary>
        /// <param name="editingControl">MultiRowの編集コントロール（IEditingControlを実装するオブジェクト）</param>
        /// <returns>Cell.Valueに相当する値</returns>
        private static object GetEditingControlValue(Control editingControl)
        {
            if (!(editingControl is IEditingControl))
            {
                throw new ArgumentNullException("引数：editingControlがIEditingControlを実装していません。");
            }

            if (editingControl == null)
            {
                throw new ArgumentNullException("引数：editingControlがnullです。");
            }

            //GcNumber
            {
                var ec = editingControl as GcNumber;

                if (ec != null)
                {
                    return ec.Value;
                }
            }

            //GcTextBox
            {
                var ec = editingControl as GcTextBox;

                if (ec != null)
                {
                    return ec.Text;
                }
            }

            //GcDateTime
            {
                var ec = editingControl as GcDateTime;

                if (ec != null)
                {
                    return ec.Value;
                }
            }

            //ComboBox
            {
                var ec = editingControl as ComboBoxEditingControl;

                if (ec != null)
                {
                    return ec.SelectedValue;
                }
            }

            throw new ArgumentException("引数に対応するeditingControlが存在しません。");
        }

        #endregion


        #region 検証（Validate）処理

        /// <summary>
        /// 社員検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearcStaff()
        {
            using (CmnSearchStaffFrame f = new CmnSearchStaffFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.StaffCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 社員コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numStaffCode_Validating(object sender, CancelEventArgs e)
        {
            //社員コード
            this.ValidateStaffCode(e);
        }

        /// <summary>
        /// 作業開始日時のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteSagyoYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            if (this.dteSagyoYMDTo.Value == null)
            {
                this.dteSagyoYMDTo.Value = this.dteSagyoYMDFrom.Value;
            }
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
                }
            }
        }

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.StaffCode == 0)
                {
                    is_clear = true;
                    return;
                }

                StaffInfo info =
                    this._DalUtil.Staff.GetInfo(this.StaffCode);

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
                    }
                    else
                    {
                        this.numStaffCode.Tag = info.StaffId;
                        this.numStaffCode.Value = info.StaffCode;
                        this.edtStaffName.Text = info.StaffName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numStaffCode.Tag = null;
                    this.numStaffCode.Value = null;
                    this.edtStaffName.Text = string.Empty;
                }
            }

        }

        /// <summary>
        /// 社員コードの値を取得します。
        /// </summary>
        private int StaffCode
        {
            get { return Convert.ToInt32(this.numStaffCode.Value); }
        }

        /// <summary>
        /// 作業大分類のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbSagyoDaiBunrui_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.cmbSagyoChuBunrui.Items.Clear();
            //this.cmbSagyoShoBunrui.Items.Clear();
            //this.SagyoChuBunruiCombo(Convert.ToDecimal(this.cmbSagyoDaiBunrui.SelectedValue));
        }
        /// <summary>
        /// 作業中分類のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbSagyoChuBunrui_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.cmbSagyoShoBunrui.Items.Clear();
            //this.SagyoShoBunruiCombo(Convert.ToDecimal(this.cmbSagyoChuBunrui.SelectedValue));
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

        private void btnDeleteAllDtlSagyoin_Click(object sender, EventArgs e)
        {
            DialogResult d_result =
                MessageBox.Show(
                "割り当てた作業員をクリアします。よろしいですか？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            this.InitHatchuDtlListMrowSagyoin();
        }
        private void btnDeleteAllDtlCar_Click(object sender, EventArgs e)
        {
            DialogResult d_result =
                MessageBox.Show(
                "割り当てた車両をクリアします。よろしいですか？",
                "確認",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question,
                MessageBoxDefaultButton.Button1);

            //Noだったら抜ける
            if (d_result == DialogResult.No)
            {
                return;
            }

            this.InitHatchuDtlListMrowCar();
        }

        private void mrsSagyoinWariateMeisai_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.Scope == CellScope.Row && e.CellName != MrowCellKeysSagyoin.IsEdited.ToString())
            {
                this.ToEditedSagyoinMeisaiRow(e.RowIndex, this.mrsSagyoinWariateMeisai);
            }
        }

        /// <summary>
        /// 作業員割当明細上の行を編集状態にします。
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ToEditedSagyoinMeisaiRow(int rowIndex, GcMultiRow multiRow)
        {
            multiRow[rowIndex, MrowCellKeysSagyoin.IsEdited.ToString()].Value = true;
        }

        ///// <summary>
        ///// 作業員割当明細「編集フラグ」=Trueにする
        ///// </summary>
        //private void SetSagyoinMeisaiIsEdited()
        //{
        //    foreach (var row in this.mrsSagyoinWariateMeisai.Rows)
        //    {
        //        if (row.IsNewRow)
        //            continue;
        //        row[MrowCellKeysSagyoin.IsEdited.ToString()].Value = true;
        //    }
        //}

        private void mrsCarWariateMeisai_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            if (e.Scope == CellScope.Row && e.CellName != MrowCellKeysCar.IsEdited.ToString())
            {
                this.ToEditedCarMeisaiRow(e.RowIndex, this.mrsCarWariateMeisai);
            }
        }

        /// <summary>
        /// 車両割当明細上の行を編集状態にします。
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ToEditedCarMeisaiRow(int rowIndex, GcMultiRow multiRow)
        {
            multiRow[rowIndex, MrowCellKeysCar.IsEdited.ToString()].Value = true;
        }

        private void btnZenkaiSagyoinFukusha_Click(object sender, EventArgs e)
        {
            this.LastTimeSagyoinWariateSetScreen();
        }

        /// <summary>
        /// 前回の作業員割当情報を画面にセットします。
        /// </summary>
        private void LastTimeSagyoinWariateSetScreen()
        {
            if (this.mrsSagyoinWariateMeisai.Rows.Where(x => !x.IsNewRow).Count() > 0)
            {
                DialogResult d_result =
                    MessageBox.Show(
                    "割り当てた作業員がクリアされます。よろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }
            }

            //作業開始日時の必須チェック
            if (this.dteSagyoYMDFrom.Value == null)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2203004", new string[] { "作業開始日時" }),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                this.dteSagyoYMDFrom.Focus();
                return;
            }

            //作業案件情報取得
            SagyoAnkenSearchParameter para = new SagyoAnkenSearchParameter();
            para.SearchKbn = (int)SagyoAnken.SearchKbnEnum.LastTime;
            para.KeiyakuId = this.SelectKeiyakuId;
            para.SagyoAnkenId = this.SelectDtlId;
            para.SagyoStartDateTime = this.dteSagyoYMDFrom.Value;
            var list = this._SagyoAnken.GetList(para);

            if (list.Count() == 0)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2201015"),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }

            // 作業員割当情報取得
            SagyoinWariateSearchParameter sagyoinPara = new SagyoinWariateSearchParameter();
            sagyoinPara.SearchKbn = (int)SagyoinWariate.SearchKbnEnum.Basic;
            sagyoinPara.SagyoAnkenId = list.First().SagyoAnkenId;

            var listSagyoin = this._SagyoinWariate.GetList(sagyoinPara).ToList();
            if (listSagyoin.Count() == 0)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2201015"),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }

            this._SagyoinWariateInfoList = listSagyoin;

            //作業員割当明細を設定
            this.SetScreenSagyoinWariateMeisai();

        }

        private void btnZenkaiCarFukusha_Click(object sender, EventArgs e)
        {
            this.LastTimeCarWariateSetScreen();
        }

        /// <summary>
        /// 前回の車両割当情報を画面にセットします。
        /// </summary>
        private void LastTimeCarWariateSetScreen()
        {
            if (this.mrsCarWariateMeisai.Rows.Where(x => !x.IsNewRow).Count() > 0)
            {
                DialogResult d_result =
                    MessageBox.Show(
                    "割り当てた車両がクリアされます。よろしいですか？",
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question,
                    MessageBoxDefaultButton.Button1);

                //Noだったら抜ける
                if (d_result == DialogResult.No)
                {
                    return;
                }
            }

            //作業開始日時の必須チェック
            if (this.dteSagyoYMDFrom.Value == null)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2203004", new string[] { "作業開始日時" }),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                this.dteSagyoYMDFrom.Focus();
                return;
            }

            //作業案件情報取得
            SagyoAnkenSearchParameter para = new SagyoAnkenSearchParameter();
            para.SearchKbn = (int)SagyoAnken.SearchKbnEnum.LastTime;
            para.KeiyakuId = this.SelectKeiyakuId;
            para.SagyoAnkenId = this.SelectDtlId;
            para.SagyoStartDateTime = this.dteSagyoYMDFrom.Value;
            para.SagyoEndDateTime = this.dteSagyoYMDTo.Value;
            var list = this._SagyoAnken.GetList(para);

            if (list.Count() == 0)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2201015"),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }

            //車両割当情報取得
            CarWariateSearchParameter carPara = new CarWariateSearchParameter();
            carPara.SearchKbn = (int)CarWariate.SearchKbnEnum.Basic;
            carPara.SagyoAnkenId = list.First().SagyoAnkenId;

            var listCar = this._CarWariate.GetList(carPara).ToList();
            if (listCar.Count() == 0)
            {
                MessageBox.Show(
                   FrameUtilites.GetDefineMessage("MW2201015"),
                   "警告",
                   MessageBoxButtons.OK,
                   MessageBoxIcon.Warning);
                return;
            }

            this._CarWariateInfoList = listCar;

            //車両割当明細を設定
            this.SetScreenCarWariateMeisai();

        }

    }
}
