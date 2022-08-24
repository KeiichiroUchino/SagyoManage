using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.FrameLib.InputMan;
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.Frame.Command;
using System.Runtime.InteropServices;
using Jpsys.HaishaManageV10.BizProperty;
using System.Configuration;
using System.Text;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaNyuryokuConditionFrame : Form, IFrameBase
    {
        #region ユーザ定義

        //--定数値
        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配車入力（抽出条件）";

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
        private CommonDALUtil _DalUtil;

        /// <summary>ｈ
        /// 方面クラス
        /// </summary>
        private Homen _Homen;

        /// <summary>ｈ
        /// 得意先クラス
        /// </summary>
        private Tokuisaki _Tokuisaki;

        /// <summary>ｈ
        /// 車種クラス
        /// </summary>
        private CarKind _CarKind;

        /// <summary>
        /// 排他用抽出条件を保持する領域
        /// </summary>
        private HaishaNyuryokuConditionInfo _HaishaNyuryokuConditionInfo = null;

        // 営業所コンボ設定
        private Dictionary<int, String> datasource;

        #region 方面リスト

        //--Spread列定義

        /// <summary>
        /// コード列番号
        /// </summary>
        private const int COL_CODE = 0;

        /// <summary>
        /// 名称列番号
        /// </summary>
        private const int COL_NAME = 1;

        /// <summary>
        /// 選択チェックボックス列番号
        /// </summary>
        private const int COL_SELECT_CHECKBOX = 2;

        /// <summary>
        /// Id列番号
        /// </summary>
        private const int COL_ID = 3;

        /// <summary>
        /// リスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LIST = 4;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialStyleInfoArr;

        /// <summary>
        /// 方面情報のリストを保持する領域
        /// </summary>
        private IList<HomenInfo> _HomenInfoList = null;

        /// <summary>
        /// 得意先情報のリストを保持する領域
        /// </summary>
        private IList<TokuisakiInfo> _TokuisakiInfoList = null;

        /// <summary>
        /// 車種情報のリストを保持する領域
        /// </summary>
        private IList<CarKindInfo> _CarKindInfoList = null;

        /// <summary>
        /// 自画面固有データアクセスクラス
        /// </summary>
        private HaishaNyuryokuCondition _HaishaNyuryokuCondition;

        /// <summary>
        /// 配車入力一覧からの遷移フラグ
        /// </summary>
        private bool _showBackFlag = false;

        #endregion

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

        #endregion

        #region コンストラクタ
        
        //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
        [DllImport("user32.dll")]
        public static extern int SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, [MarshalAs(UnmanagedType.Bool)] bool pvParam, UInt32 fWinIni);
        [Flags]
        public enum SystemParametersInfoActionFlag : uint
        {
            SPI_SETKEYBOARDCUES = 0x100B,
        }
        [Flags]
        public enum SystemParametersInfoFlag : uint
        {
            SPIF_UPDATEINIFILE = 0x0001,
            SPIF_SENDWININICHANGE = 0x0002,
        }

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public HaishaNyuryokuConditionFrame()
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0, 
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            InitializeComponent();
        }

        /// <summary>
        /// 本クラスのデフォルトコンストラクタ
        /// </summary>
        public HaishaNyuryokuConditionFrame(HaishaNyuryokuConditionInfo _Info)
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0,
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            // 配車入力に渡している検索条件
            this._HaishaNyuryokuConditionInfo = _Info;

            InitializeComponent();
        }

        #endregion
        
        #region 初期化処理

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitHaishaNyuryokuConditionFrame()
        {
            // 画面タイトルの設定
            this.Text = WINDOW_TITLE;

            //画面配色の初期化
            this.InitFrameColor();

            // 親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            // メニューの初期化
            this.InitMenuItem();

            // バインドの設定
            this.SetupCommands();
            this.SetupValidatingEventRaiser();
            this.SetupSearchStateBinder();

            // アプリケーション認証情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            // 配車入力状態クラスインスタンス作成
            this._HaishaNyuryokuCondition = new HaishaNyuryokuCondition(this.appAuth);

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するための
            // ファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //配車入力条件区分と営業所管理区分によって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.None:
                    switch (UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn)
                    {
                        case (int)DefaultProperty.EigyoshoKanriKbn.None:
                            //配車入力生成
                            HaishaNyuryokuFrame f = new HaishaNyuryokuFrame();
                            ////抽出条件退避 必要に応じて条件設定
                            f.HaishaNyuryokuConditionInfo = this._HaishaNyuryokuConditionInfo;
                            //配車入力初期化
                            f.InitFrame();
                            //配車入力表示
                            f.Show();
                            //フォームを閉じる
                            this.Close();
                            return;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }

            // 画面レイアウト調整（営業所コンボボックスの表示可否）
            this.InitLayoutControl();

            // Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            // Spread 関連の初期化
            this.InitSheet();

            //コンボボックス関連の初期化
            this.InitCombo();

            // 入力項目のクリア
            this.ClearInputs();

            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先クラスインスタンス作成
                    this._Tokuisaki = new Tokuisaki(this.appAuth);

                    // 得意先リストのセット
                    this.SetTokuisakiListSheet();
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面クラスインスタンス作成
                    this._Homen = new Homen(this.appAuth);

                    // 方面リストのセット
                    this.SetHomenListSheet();
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種クラスインスタンス作成
                    this._CarKind = new CarKind(this.appAuth);

                    // 車種リストのセット
                    this.SetCarKindListSheet();
                    break;
                default:
                    break;
            }

            // 抽出条件の設定
            this.SetCondition();

            // コントロール活性化
            this.InitControlsEnabled();

            // フォーカス設定
            this.SetFirstFocus();
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
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            this.actionMenuItems = new AddActionMenuItem();

            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Select);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SetupCommands()
        {
            commandSet = new CommandSet();

            //***確定
            commandSet.Select.Execute += SelectCommand_Execute;
            commandSet.Bind(commandSet.Select,
               this.btnSelect, actionMenuItems.GetMenuItemBy(ActionMenuItems.Select), this.toolStripSelect);

            //***終了
            commandSet.Close.Execute += CloseCommand_Execute;
            commandSet.Bind(commandSet.Close,
               this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        private void SelectCommand_Execute(object sender, EventArgs e)
        {
            // 確定
            this.DoSelect();
        }

        private void CloseCommand_Execute(object sender, EventArgs e)
        {
            // 終了
            this.DoClose();
        }

        /// <summary>
        /// 営業所のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void cmbBranchOffice_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCmbBranchOffice(e);
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            this.searchStateBinder.AddSearchableControls(
                this.numGroupCode
                );

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numGroupCode, ctl => ctl.Text, this.numHomenGroupCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbBranchOffice, ctl => ctl.Text, this.cmbBranchOffice_Validating));
        }

        /// <summary>
        /// レイアウトを制御します。
        /// （営業所コンボボックスの表示可否）
        /// </summary>
        private void InitLayoutControl()
        {
            // 配車入力条件区分＝1：なしの場合、グループ指定、個別指定の項目を非表示
            if (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn
                 == (int)DefaultProperty.HaishaNyuryokuJokenKbn.None)
            {
                this.lblTokuisakiShitei.Visible = false;
                this.grpTokuisakiShitei.Visible = false;
                this.lblTokuisakiGroup.Visible = false;
                this.numGroupCode.Visible = false;
                this.edtGroupName.Visible = false;
                this.fpListGrid.Visible = false;
                this.btnAllRemove.Visible = false;
                this.btnAllSelect.Visible = false;

                // フォームのサイズを変更
                this.Size = new Size(600, 200);
            }
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
            // 方面リストスタイル情報初期化
            this.InitHomenStyleInfo();
        }

        /// <summary>
        /// 方面リストのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitHomenStyleInfo()
        {
            // Id列非表示
            this.fpListGrid.Sheets[0].Columns[COL_ID].Visible = false;

            // SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            // デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_LIST];

            for (int i = 0; i < COL_MAXCOLNUM_LIST; i++)
            {
                this.initialStyleInfoArr[i] =
                    new StyleInfo(sheet0.Models.Style.GetDirectInfo(-1, i, null));
            }
        }

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitBranchOfficeCombo();
        }

        /// <summary>
        /// 営業所コンボボックスを初期化します。
        /// </summary>
        private void InitBranchOfficeCombo()
        {
            // 営業所コンボ設定
            this.datasource = new Dictionary<int, String>();

            IList<ToraDONBranchOfficeInfo> list = this._DalUtil.ToraDONBranchOffice.GetList(null);

            if (list != null && 0 < list.Count())
            {
                list = list
                    .OrderBy(x => x.BranchOfficeCode)
                    .ToList();
            }
            else
            {
                list = new List<ToraDONBranchOfficeInfo>();
            }

            int key = 0;
            String value = "";

            foreach (ToraDONBranchOfficeInfo item in list)
            {
                key = item.BranchOfficeCode;
                value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

                this.datasource.Add(key, value);
            }

            // 空のItemを追加するかどうか
            bool flg = true;

            // 営業所管理区分ありの場合
            if ((int)DefaultProperty.EigyoshoKanriKbn.Use
                == UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn) flg = false;

            FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, this.datasource, flg, DefaultProperty.CMB_BRANCHOFFICE_ALL_NAME, false);
        }

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //リストの初期化
            this.InitHomenListSheet();
        }

        /// <summary>
        /// リストを初期化します。
        /// </summary>
        private void InitHomenListSheet()
        {
            // SheetView変数定義
            SheetView sheet0 = this.fpListGrid.Sheets[0];

            // 行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_LIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 画面入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            switch (UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn)
            {
                case (int)DefaultProperty.EigyoshoKanriKbn.None:
                    this.cmbBranchOffice.SelectedValue = null;
                    this.cmbBranchOffice.Tag = null;
                    break;
                case (int)DefaultProperty.EigyoshoKanriKbn.Use:
                    this.cmbBranchOffice.SelectedIndex = 0;
                    this.ValidateCmbBranchOffice(null);
                    break;
                default:
                    break;
            }

            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:
                    this.lblTokuisakiShitei.Text = "得意先指定 *";
                    this.lblTokuisakiGroup.Text = "得意先グループ";
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:
                    this.lblTokuisakiShitei.Text = "方面指定 *";
                    this.lblTokuisakiGroup.Text = "方面グループ";
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:
                    this.lblTokuisakiShitei.Text = "車種指定 *";
                    this.lblTokuisakiGroup.Text = "車種グループ";
                    break;
                default:
                    break;
            }

            this.radGroup.Checked = true;
            this.numGroupCode.Value = null;
            this.edtGroupName.Text = string.Empty;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #endregion

        #region プライベート メソッド

        /// <summary>コントロールの活性／非活性を初期化します。
        /// </summary>
        private void InitControlsEnabled()
        {
            this.ChangeCondition();
        }
        
        /// <summary>
        /// 最初のコントロールにフォーカスを移動させます。
        /// </summary>
        private void SetFirstFocus()
        {
            //初期表示の場合
            if (!this.ShowBackFlag)
            {
                //日付（範囲開始）にフォーカス設定
                this.cmbBranchOffice.Focus();
            }
            //配車入力画面から遷移の場合
            else
            {
                //方面タングループコードにフォーカス設定
                this.ActiveControl = this.numGroupCode;
            }
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 指定条件の活性／非活性を設定します。
        /// </summary>
        private void ChangeCondition()
        {
            this.numGroupCode.Enabled = this.radGroup.Checked;
            this.fpListGrid.Enabled = this.radKobetsu.Checked;
            this.btnAllRemove.Enabled = this.radKobetsu.Checked;
            this.btnAllSelect.Enabled = this.radKobetsu.Checked;

            // グループコードが非活性の場合
            if (!this.numGroupCode.Enabled)
            {
                // 方面グループ初期化
                this.numGroupCode.Value = null;
                this.edtGroupName.Text = string.Empty;
            }

            // リストが非活性の場合
            if(!this.fpListGrid.Enabled)
            {
                //チェックボックス全削除
                for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
                {
                    this.fpListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = false;
                }

                //選択行解除
                this.fpListGrid.Sheets[0].ClearSelection();
            }
        }

        /// <summary>
        /// 方面リストのチェックON／OFFを設定します。
        /// </summary>
        private void CheckHomenList(int rowIndex)
        {
            this.fpListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value =
                !Convert.ToBoolean((this.fpListGrid.Sheets[0].Cells[rowIndex, COL_SELECT_CHECKBOX].Value));
        }

        /// <summary>
        /// 方面リストに値を設定します。
        /// </summary>
        private void SetHomenListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //件数
                int rowCount = 0;

                IList<HomenInfo> wk_list = null;

                if (this._HaishaNyuryokuConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._HomenInfoList =
                        this._Homen.GetList();

                    //方面リスト取得
                    wk_list = this._HomenInfoList
                        .Where(x => x.DisableFlag == false)
                        .ToList();

                    //方面コード順にソート
                    wk_list = wk_list
                        .OrderBy(x => x.HomenCode)
                        .ToList();

                    //先頭に「方面ID = 0」を挿入
                    wk_list.Insert(0, new HomenInfo()
                    {
                        HomenId = 0,
                        HomenCode = 0,
                        HomenName = "未入力"
                    });

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    //方面リスト件数取得
                    rowCount = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_LIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._HaishaNyuryokuConditionInfo == null)
                    {
                        HomenInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.HomenCode);
                        datamodel.SetValue(i, COL_NAME, wk_info.HomenName);
                        datamodel.SetValue(i, COL_ID, wk_info.HomenId);
                    }
                    else
                    {
                        HaishaNyuryokuConditionListInfo wk_info = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.Code);
                        datamodel.SetValue(i, COL_NAME, wk_info.Name);
                        datamodel.SetValue(i, COL_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_ID, wk_info.Id);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpListGrid.Sheets[0].Models.Style = stylemodel;

                //ヘッダー名を修正
                this.fpListGrid.Sheets[0].ColumnHeader.Cells[0, 1].Text = "方面名";

                //選択行にフォーカスを合わせて選択状態にする
                this.fpListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 得意先リストに値を設定します。
        /// </summary>
        private void SetTokuisakiListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //件数
                int rowCount = 0;

                IList<TokuisakiInfo> wk_list = null;

                if (this._HaishaNyuryokuConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._TokuisakiInfoList =
                        this._Tokuisaki.GetList();

                    //得意先リスト取得
                    wk_list = this._TokuisakiInfoList
                        .Where(x => x.ToraDONDisableFlag == false)
                        .ToList();

                    //得意先コード順にソート
                    wk_list = wk_list
                        .OrderBy(x => x.ToraDONTokuisakiCode)
                        .ToList();

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    //方面リスト件数取得
                    rowCount = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_LIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._HaishaNyuryokuConditionInfo == null)
                    {
                        TokuisakiInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.ToraDONTokuisakiCode);
                        datamodel.SetValue(i, COL_NAME, wk_info.ToraDONTokuisakiName);
                        datamodel.SetValue(i, COL_ID, wk_info.ToraDONTokuisakiId);
                    }
                    else
                    {
                        HaishaNyuryokuConditionListInfo wk_info = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.Code);
                        datamodel.SetValue(i, COL_NAME, wk_info.Name);
                        datamodel.SetValue(i, COL_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_ID, wk_info.Id);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpListGrid.Sheets[0].Models.Style = stylemodel;

                //ヘッダー名を修正
                this.fpListGrid.Sheets[0].ColumnHeader.Cells[0, 1].Text = "得意先名";

                //選択行にフォーカスを合わせて選択状態にする
                this.fpListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 車種リストに値を設定します。
        /// </summary>
        private void SetCarKindListSheet()
        {
            //待機状態へ
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //件数
                int rowCount = 0;

                IList<CarKindInfo> wk_list = null;

                if (this._HaishaNyuryokuConditionInfo == null)
                {
                    //検索結果一覧の取得を指示
                    this._CarKindInfoList =
                        this._CarKind.GetList();

                    //車種リスト取得
                    wk_list = this._CarKindInfoList
                        .Where(x => x.DisableFlag == false)
                        .ToList();

                    //車種コード順にソート
                    wk_list = wk_list
                        .OrderBy(x => x.ToraDONCarKindCode)
                        .ToList();

                    //先頭に「車種ID = 0」を挿入
                    wk_list.Insert(0, new CarKindInfo()
                    {
                        CarKindId = 0,
                        ToraDONCarKindCode = 0,
                        ToraDONCarKindName = "未入力"
                    });

                    //件数取得
                    rowCount = wk_list.Count;
                }
                else
                {
                    //車種リスト件数取得
                    rowCount = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList.Count;
                }

                if (rowCount == 0)
                {
                    //0件の場合は何もしない
                    return;
                }

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(rowCount, COL_MAXCOLNUM_LIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(rowCount, COL_MAXCOLNUM_LIST);

                //ループしてデータモデルにセット
                for (int i = 0; i < rowCount; i++)
                {
                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialStyleInfoArr[k]);

                        stylemodel.SetDirectInfo(i, k, sInfo);
                    }

                    if (this._HaishaNyuryokuConditionInfo == null)
                    {
                        CarKindInfo wk_info = wk_list[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.ToraDONCarKindCode);
                        datamodel.SetValue(i, COL_NAME, wk_info.ToraDONCarKindName);
                        datamodel.SetValue(i, COL_ID, wk_info.ToraDONCarKindId);
                    }
                    else
                    {
                        HaishaNyuryokuConditionListInfo wk_info = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList[i];

                        datamodel.SetValue(i, COL_CODE, wk_info.Code);
                        datamodel.SetValue(i, COL_NAME, wk_info.Name);
                        datamodel.SetValue(i, COL_SELECT_CHECKBOX, wk_info.CheckedFlag);
                        datamodel.SetValue(i, COL_ID, wk_info.Id);
                    }
                }

                //Spreadにデータモデルをセット
                this.fpListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpListGrid.Sheets[0].Models.Style = stylemodel;

                //ヘッダー名を修正
                this.fpListGrid.Sheets[0].ColumnHeader.Cells[0, 1].Text = "車種名";

                //選択行にフォーカスを合わせて選択状態にする
                this.fpListGrid.Sheets[0].SetActiveCell(0, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 得意先グループ検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisaki()
        {
            using (CmnSearchTokuisakiGroupFrame f = new CmnSearchTokuisakiGroupFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.TokuisakiGroupCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 車種グループ検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCarKind()
        {
            using (CmnSearchCarKindGroupFrame f = new CmnSearchCarKindGroupFrame())
            {
                //パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                    Convert.ToInt32(f.SelectedInfo.CarKindGroupCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 抽出条件を設定します。
        /// </summary>
        private void SetCondition()
        {
            if (this._HaishaNyuryokuConditionInfo == null)
            {
                this._HaishaNyuryokuConditionInfo = new HaishaNyuryokuConditionInfo();
            }
            else
            {
                this.SetScreen();
            }
        }

        /// <summary>
        /// 画面に入力した情報で確定します。
        /// </summary>
        /// <returns>処理結果</returns>
        private void DoSelect()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                if (!this.ValidateChildren(ValidationConstraints.None))
                {
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    return;
                }

                //抽出条件取得
                this.GetFromScreen();

                if (this.CheckInputs())
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        // 削除
                        this._HaishaNyuryokuCondition.DeleteHaishaExclusiveManage(tx);

                        // 追加
                        this._HaishaNyuryokuCondition.AddHaishaExclusiveManage(tx, this._HaishaNyuryokuConditionInfo);

                    });

                    //配車入力生成
                    HaishaNyuryokuFrame f = new HaishaNyuryokuFrame();
                    ////抽出条件退避 ここで条件設定
                    f.HaishaNyuryokuConditionInfo = this._HaishaNyuryokuConditionInfo;
                    //配車入力初期化
                    f.InitFrame();
                    //配車入力表示
                    f.Show();
                    //フォームを閉じる
                    this.Close();
                }
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }

        /// <summary>
        /// 抽出条件を画面に設定します。
        /// </summary>
        private void SetScreen()
        {
            this.cmbBranchOffice.SelectedValue =
                this._HaishaNyuryokuConditionInfo.BranchOfficeCode == 0 ? null : this._HaishaNyuryokuConditionInfo.BranchOfficeCode;
            this.cmbBranchOffice.Tag = this._HaishaNyuryokuConditionInfo.BranchOfficeId;
            this.radGroup.Checked = this._HaishaNyuryokuConditionInfo.ShiteiGroupChecked;
            this.radKobetsu.Checked = this._HaishaNyuryokuConditionInfo.ShiteiKobetsuChecked;
            if (0 < this._HaishaNyuryokuConditionInfo.GroupId)
            {
                this.numGroupCode.Tag = (decimal?)this._HaishaNyuryokuConditionInfo.GroupId;
                this.numGroupCode.Value = (decimal?)this._HaishaNyuryokuConditionInfo.GroupCode;
                this.edtGroupName.Text = this._HaishaNyuryokuConditionInfo.GroupName;
            }
            else
            {
                this.numGroupCode.Tag = null;
                this.numGroupCode.Value = null;
                this.edtGroupName.Text = string.Empty;
            }

            //個別リスト
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                ////チェックボックス設定
                HaishaNyuryokuConditionListInfo info = this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList[i];
                if (info.CheckedFlag) this.CheckHomenList(i);
            }
        }

        /// <summary>
        /// 詳細の入力項目をチェックします。
        /// (true:正常に入力されている)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputs()
        {
            bool rt_val = true;

            string msg = string.Empty;
            Control ctl = null;

            string koumoku = string.Empty;

            /***************
             * 必須チェック
             ***************/

            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {

                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:
                    // 得意先の場合
                    koumoku = "得意先";
                    break;

                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:
                    // 方面の場合
                    koumoku = "方面";
                    break;

                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:
                    // 車種の場合
                    koumoku = "車種";
                    break;

                default:
                    break;
            }

            // 配車入力条件区分＝1：なし以外の場合、グループ指定、個別指定の項目を非表示
            if (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn
                 != (int)DefaultProperty.HaishaNyuryokuJokenKbn.None) 
            {
                if (rt_val)
                {
                    // グループ指定の場合
                    if (this.radGroup.Checked)
                    {
                        // グループコード
                        if (rt_val && this.GroupCode == 0)
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage("MW2203001", new string[] { koumoku + "グループ" });
                            ctl = this.numGroupCode;
                        }
                    }
                    //個別指定の場合
                    else
                    {
                        int cnt = 0;
                        //チェック件数取得
                        for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
                        {
                            if ((bool)this.fpListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value)
                                cnt++;
                        }
                        //チェックが0件の場合
                        if (rt_val && cnt == 0)
                        {
                            rt_val = false;
                            msg = FrameUtilites.GetDefineMessage("MW2203012", new string[] { koumoku });
                            ctl = this.fpListGrid;
                        }
                    }
                }
            }

            /***************
             * 排他チェック
             ***************/

            if (rt_val) 
            {
                // 営業所
                decimal branchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag);
                string checkIdList = string.Empty;

                StringBuilder sb = new StringBuilder();
                bool f = false;

                //配車入力条件区分よって制御を分岐
                switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
                {
                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:
                        // 得意先の場合
                        foreach (decimal id in this._HaishaNyuryokuConditionInfo.TokuisakiIdMeisaiList)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(id.ToString());
                            f = true;
                        }
                        checkIdList = sb.ToString();
                        break;

                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:
                        // 方面の場合
                        foreach (decimal id in this._HaishaNyuryokuConditionInfo.PointIdMeisaiList)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(id.ToString());
                            f = true;
                        }
                        checkIdList = sb.ToString();
                        break;

                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:
                        // 車種の場合
                        foreach (decimal id in this._HaishaNyuryokuConditionInfo.CarKindIdMeisaiList)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(id.ToString());
                            f = true;
                        }
                        checkIdList = sb.ToString();
                        break;

                    default:
                        break;
                }

                //登録済み排他情報を検索。
                List<HaitaResultInfo> list = this._HaishaNyuryokuCondition.GetHaishaExclusiveManageSelect(
                    this._HaishaNyuryokuConditionInfo.BranchOfficeId,
                    checkIdList,
                    this._HaishaNyuryokuConditionInfo.HaishaNyuryokuJokenKbn);

                // グループコード
                if (rt_val && list.Count > 0 )
                {
                    rt_val = false;
                    msg = "他ユーザーにて操作中です。利用状況をご確認の上、再度選択してください。";

                    foreach (var info in list)
                    {
                        msg += Environment.NewLine + info.HaitaResultMeisai;
                    }

                    ctl = this.numGroupCode;
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(
                    msg,
                    this.Text,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                    );

                if (ctl != null)
                {
                    ctl.Focus();
                    this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                }
            }
            
            return rt_val;
        }

        /// <summary>
        /// 画面から更新に必要な情報をメンバにセットします。
        /// </summary>
        private void GetFromScreen()
        {
            int bOCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);

            this._HaishaNyuryokuConditionInfo.HaishaNyuryokuJokenKbn = UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn;
            this._HaishaNyuryokuConditionInfo.EigyoshoKanriKbn = UserProperty.GetInstance().SystemSettingsInfo.EigyoshoKanriKbn;

            this._HaishaNyuryokuConditionInfo.BranchOfficeCode = bOCode;
            this._HaishaNyuryokuConditionInfo.BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag);

            // 営業所名ですべて以外を選択している場合
            if (this.datasource.ContainsKey(bOCode))
            {
                this._HaishaNyuryokuConditionInfo.BranchOfficeName =
                    this.datasource[bOCode].Replace(bOCode + " ",string.Empty);
            }
            else 
            {
                this._HaishaNyuryokuConditionInfo.BranchOfficeName = "すべて";
            }

            this._HaishaNyuryokuConditionInfo.ShiteiGroupChecked = this.radGroup.Checked;
            this._HaishaNyuryokuConditionInfo.ShiteiKobetsuChecked = this.radKobetsu.Checked;
            this._HaishaNyuryokuConditionInfo.GroupId = Convert.ToDecimal(this.numGroupCode.Tag);
            this._HaishaNyuryokuConditionInfo.GroupCode = Convert.ToInt32(this.numGroupCode.Value);
            this._HaishaNyuryokuConditionInfo.GroupName = Convert.ToString(this.edtGroupName.Text);

            //リスト取得
            IList<HaishaNyuryokuConditionListInfo> list = new List<HaishaNyuryokuConditionListInfo>();
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                HaishaNyuryokuConditionListInfo info = new HaishaNyuryokuConditionListInfo();
                info.Code = Convert.ToInt32(
                    this.fpListGrid.Sheets[0].Cells[i, COL_CODE].Value);
                info.Name = Convert.ToString(
                    this.fpListGrid.Sheets[0].Cells[i, COL_NAME].Value);
                info.CheckedFlag = Convert.ToBoolean(
                    this.fpListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value);
                info.Id = Convert.ToDecimal(
                    this.fpListGrid.Sheets[0].Cells[i, COL_ID].Value);
                list.Add(info);
            }

            //リスト設定
            this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList = list;


            // 設定リスト
            List<decimal> idList = new List<decimal>();

            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先の場合
                    // グループの場合
                    if (this.radGroup.Checked) 
                    {
                        var tokuisakiList = this._DalUtil.TokuisakiGroupMeisai.GetList(
                            new TokuisakiGroupMeisaiSearchParameter()
                        {
                            TokuisakiGroupId = (decimal?)this.numGroupCode.Tag
                            });
                        foreach (TokuisakiGroupMeisaiInfo item in tokuisakiList)
                        {
                            idList.Add(item.ToraDONTokuisakiId);
                        }
                    }

                    // 個別の場合
                    else if (this.radKobetsu.Checked)
                    {
                        if (!string.IsNullOrWhiteSpace(this._HaishaNyuryokuConditionInfo.CheckIdList)) 
                        {
                            var tokuisakiList = this._HaishaNyuryokuConditionInfo.CheckIdList.Replace(Environment.NewLine, string.Empty).Split(',');

                            // 得意先一覧の場合
                            foreach (string id in tokuisakiList)
                            {
                                idList.Add(Convert.ToDecimal(id));
                            }
                        }
                    }

                    // 得意先Idリスト
                    this._HaishaNyuryokuConditionInfo.TokuisakiIdMeisaiList = idList;

                    break;

                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面の場合
                    // グループの場合
                    if (this.radGroup.Checked)
                    {
                        // 方面グループのID一覧を取得。
                        var homenGroupMeisaiList = this._DalUtil.HomenGroupMeisai.GetList(
                            new HomenGroupMeisaiSearchParameter()
                        {
                            HomenGroupId = (decimal?)this.numGroupCode.Tag
                            });

                        StringBuilder sb = new StringBuilder();
                        bool f = false;
                        foreach (HomenGroupMeisaiInfo info in homenGroupMeisaiList)
                        {
                            if (f) sb.AppendLine(",");
                            sb.AppendLine(info.HomenId.ToString());
                            f = true;
                        }

                        // 方面グループのID一覧に紐づく発着地のIdを取得
                        var pointList = this._HaishaNyuryokuCondition.GetPointSelect(sb.ToString());
                        foreach (decimal id in pointList)
                        {
                            idList.Add(id);
                        }

                        //方面グループに「方面ID = 0」の明細が存在する場合
                        if (0 < homenGroupMeisaiList.Count(x => x.HomenId == 0))
                        {
                            //方面未設定発着地リスト取得
                            var pointListHomen0 = this._HaishaNyuryokuCondition.GetPointSelect0();
                            foreach (decimal id in pointListHomen0)
                            {
                                idList.Add(id);
                            }

                            //発着地IDに0が存在していない場合
                            if (!idList.Contains(0))
                            {
                                //0を追加
                                idList.Add(0);
                            }
                        }
                    }

                    // 個別の場合
                    else if (this.radKobetsu.Checked)
                    {
                        if (!string.IsNullOrWhiteSpace(this._HaishaNyuryokuConditionInfo.CheckIdList)) 
                        {
                            var pointList = this._HaishaNyuryokuCondition.GetPointSelect(this._HaishaNyuryokuConditionInfo.CheckIdList);

                            // 重複チェック用
                            Dictionary<decimal, decimal> chkId = new Dictionary<decimal, decimal>();

                            //「方面ID = 0」が存在する場合
                            if (0 < this._HaishaNyuryokuConditionInfo.HaishaNyuryokuConditionList.Count(x => x.Id == 0 && x.CheckedFlag == true))
                            {
                                //方面未設定発着地リスト取得
                                var pointListHomen0 = this._HaishaNyuryokuCondition.GetPointSelect0();
                                foreach (decimal id in pointListHomen0)
                                {
                                    if (chkIdDuplicate(chkId, id)) idList.Add(id);
                                }

                                //発着地IDに0が存在していない場合
                                if (!idList.Contains(0))
                                {
                                    //0を追加
                                    if (chkIdDuplicate(chkId, 0)) idList.Add(0);
                                }
                            }

                            // 得意先一覧の場合
                            foreach (decimal id in pointList)
                            {
                                if (chkIdDuplicate(chkId, id)) idList.Add(id);
                            }
                        }
                    }

                    // 発着地Idリスト
                    this._HaishaNyuryokuConditionInfo.PointIdMeisaiList = idList;

                    break;

                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種の場合
                    // グループの場合
                    if (this.radGroup.Checked)
                    {
                        var tokuisakiList = this._DalUtil.CarKindGroupMeisai.GetList(
                            new CarKindGroupMeisaiSearchParameter()
                            {
                                CarKindGroupId = (decimal?)this.numGroupCode.Tag
                            });
                        foreach (CarKindGroupMeisaiInfo item in tokuisakiList)
                        {
                            idList.Add(item.ToraDONCarKindId);
                        }
                    }

                    // 個別の場合
                    else if (this.radKobetsu.Checked)
                    {
                        if (!string.IsNullOrWhiteSpace(this._HaishaNyuryokuConditionInfo.CheckIdList))
                        {
                            var tokuisakiList = this._HaishaNyuryokuConditionInfo.CheckIdList.Replace(Environment.NewLine, string.Empty).Split(',');

                            // 車種一覧の場合
                            foreach (string id in tokuisakiList)
                            {
                                idList.Add(Convert.ToDecimal(id));
                            }
                        }
                    }

                    // 車種Idリスト
                    this._HaishaNyuryokuConditionInfo.CarKindIdMeisaiList = idList;

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
            if (this.ActiveControl == this.numGroupCode)
            {
                this.ShowCmnSearchHomenGroup();
            }
        }

        /// <summary>
        /// 方面グループ検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHomenGroup()
        {
            using (CmnSearchHomenGroupFrame f = new CmnSearchHomenGroupFrame())
            {
                // パラメータをセット
                //f.SearchJigyoshoCode = this.JigyoshoCode;
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.HomenGroupCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 営業所入力後の処理を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCmbBranchOffice(CancelEventArgs e)
        {
            //営業所情報を取得
            ToraDONBranchOfficeSearchParameter para = new ToraDONBranchOfficeSearchParameter();
            para.BranchOfficeCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);
            ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetList(para).FirstOrDefault();

            if (info == null)
            {
                this.cmbBranchOffice.Tag = decimal.Zero;

                //イベントをキャンセル
                e.Cancel = true;
                return;
            }

            this.cmbBranchOffice.Tag = info.BranchOfficeId;
        }

        /// <summary>
        /// 得意先グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTokuisakiGroupCodeFrom(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (Convert.ToInt32(this.numGroupCode.Value) == 0 )
                {
                    is_clear = true;
                    return;
                }

                TokuisakiGroupInfo info =
                    this._DalUtil.TokuisakiGroup.GetInfo((int)this.numGroupCode.Value);

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
                        this.numGroupCode.Tag = info.TokuisakiGroupId;
                        this.edtGroupName.Text = info.TokuisakiGroupName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numGroupCode.Tag = null;
                    this.numGroupCode.Value = null;
                    this.edtGroupName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 方面グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateHomenGroupCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (Convert.ToInt32(this.numGroupCode.Value) == 0)
                {
                    is_clear = true;
                    return;
                }

                HomenGroupInfo info =
                    this._DalUtil.HomenGroup.GetInfo(this.GroupCode);

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
                        this.numGroupCode.Tag = info.HomenGroupId;
                        this.edtGroupName.Text = info.HomenGroupName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numGroupCode.Tag = null;
                    this.numGroupCode.Value = null;
                    this.edtGroupName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車種グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindGroupCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (Convert.ToInt32(this.numGroupCode.Value) == 0)
                {
                    is_clear = true;
                    return;
                }

                CarKindGroupInfo info =
                    this._DalUtil.CarKindGroup.GetInfo(this.GroupCode);

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
                        this.numGroupCode.Tag = info.CarKindGroupId;
                        this.edtGroupName.Text = info.CarKindGroupName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numGroupCode.Tag = null;
                    this.numGroupCode.Value = null;
                    this.edtGroupName.Text = string.Empty;
                }
            }
        }

        #endregion

        #region プロパティ

        public bool ShowBackFlag
        {
            set { this._showBackFlag = value; }
            get { return this._showBackFlag; }
        }
        
        /// <summary>
        /// グループコードの値を取得します。
        /// </summary>
        private int GroupCode
        {
            get { return Convert.ToInt32(this.numGroupCode.Value); }
        }

        /// <summary>
        /// 配車入力（抽出条件）を取得・設定します。
        /// </summary>
        public HaishaNyuryokuConditionInfo HaishaNyuryokuConditionInfo
        {
            get
            {
                return this._HaishaNyuryokuConditionInfo;
            }
            set
            {
                this._HaishaNyuryokuConditionInfo = value;
            }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 画面の初期化を行います。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishaNyuryokuConditionFrame();
        }

        /// <summary>
        /// 本クラスの Name プロパティを取得・設定します。
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
        /// 本クラスの Text プロパティを取得・設定します。
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

        private void HaishaNyuryokuConditionFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.AutoValidate = AutoValidate.EnablePreventFocusChange;
        }

        private void HaishaNyuryokuConditionFrame_KeyDown(object sender, KeyEventArgs e)
        {
            // 画面 KeyDown
            this.ProcessKeyEvent(e);
        }

        private void sbtnGroupCode_Click(object sender, EventArgs e)
        {

            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先グループ検索ボタン
                    this.ShowCmnSearchTokuisaki();
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面グループ検索ボタン
                    this.ShowCmnSearchHomenGroup();
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種先グループ検索ボタン
                    this.ShowCmnSearchCarKind();
                    break;
                default:
                    break;
            }
        }

        private void numHomenGroupCode_Validating(object sender, CancelEventArgs e)
        {
            //配車入力条件区分よって制御を分岐
            switch (UserProperty.GetInstance().SystemSettingsInfo.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先グループ検索ボタン
                    this.ValidateTokuisakiGroupCodeFrom(e);
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面グループコード
                    this.ValidateHomenGroupCode(e);
                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種グループコード
                    this.ValidateCarKindGroupCode(e);
                    break;
                default:
                    break;
            }
        }

        private void radHomen_CheckedChanged(object sender, EventArgs e)
        {
            this.ChangeCondition();
        }

        private void fpHomenListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (!e.ColumnHeader)
            {
                this.CheckHomenList(e.Row);
            }

            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void fpHomenListGrid_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Return:
                    SendKeys.Send("{TAB}");
                    break;
                case Keys.Space:
                    if (((FpSpread)sender).Sheets[0].Models.Selection != null)
                    {
                        this.CheckHomenList(((FpSpread)sender).Sheets[0].Models.Selection.AnchorRow);
                    }
                    break;
                default:
                    break;
            }
        }

        private void btnAllSelect_Click(object sender, EventArgs e)
        {
            //チェックボックス全削除
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                this.fpListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = true;
            }
        }

        private void btnAllRemove_Click(object sender, EventArgs e)
        {
            //チェックボックス全削除
            for (int i = 0; i < this.fpListGrid.Sheets[0].RowCount; i++)
            {
                this.fpListGrid.Sheets[0].Cells[i, COL_SELECT_CHECKBOX].Value = false;
            }
        }

        /// <summary>
        /// IDの重複を判定します。
        /// </summary>
        /// <param name="chkId">チェック対象</param>
        private bool chkIdDuplicate(Dictionary<decimal, decimal> chkId, decimal id)
        {
            if (chkId.ContainsKey(id))
            {
                return false;
            }
            else 
            {
                chkId.Add(id, id);
                return true;
            }
        }
    }
}
