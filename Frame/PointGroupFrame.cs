using System;
using System.Collections.Generic;
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
using System.Globalization;
using GrapeCity.Win.Editors;


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class PointGroupFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public PointGroupFrame()
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

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "発着地グループ登録";

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
        /// 発着地グループクラス
        /// </summary>
        private PointGroup _PointGroup;

        /// <summary>
        /// 現在編集中の発着地グループ情報を保持する領域
        /// </summary>
        private PointGroupInfo _PointGroupInfo = null;

        /// <summary>
        /// 発着地グループIDを保持する領域
        /// </summary>
        private decimal _PointGroupId = 0;

        /// <summary>
        /// カナ取得を行うためのコントロールを保持する領域
        /// </summary>
        private GrapeCity.Win.Editors.GcIme ime;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region 発着地グループ一覧

        //--Spread列定義

        /// <summary>
        /// 発着地グループコード列番号
        /// </summary>
        private const int COL_ITEMGROUP_CODE = 0;

        /// <summary>
        /// 発着地グループ名列番号
        /// </summary>
        private const int COL_ITEMGROUP_NAME = 1;

        /// <summary>
        /// 発着地グループ名カナ列番号
        /// </summary>
        private const int COL_ITEMGROUP_KANA = 2;

        /// <summary>
        /// 発着地グループリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_ITEMGROUPLIST = 3;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialPointGroupStyleInfoArr;

        /// <summary>
        /// 発着地グループ情報のリストを保持する領域
        /// </summary>
        private IList<PointGroupInfo> _PointGroupInfoList = null;

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
        private void InitPointGroupFrame()
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

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //発着地グループクラスインスタンス作成
            this._PointGroup = new PointGroup(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //Spread関連のクリア
            this.InitSheet();

            //発着地グループリストのセット
            this.SetPointGroupListSheet();

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

        #endregion

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //発着地グループリストの初期化
            this.InitPointGroupListSheet();
        }

        /// <summary>
        /// 発着地グループリストを初期化します。
        /// </summary>
        private void InitPointGroupListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpPointGroupListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_ITEMGROUPLIST);
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
            this.numPointGroupCode.Value = 0;
            this.edtPointGroupName.Text = string.Empty;
            this.edtPointGroupNameKana.Text = string.Empty;

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
            FrameUtilites.SetImeSentenceModeForInputMan(this.edtPointGroupNameKana);
        }

        /// <summary>
        /// SpreadのStyle情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitSpreadStyleInfo()
        {
            //発着地グループスタイル情報初期化
            this.InitPointGroupStyleInfo();
        }

        /// <summary>
        /// 発着地グループのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitPointGroupStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpPointGroupListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialPointGroupStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_ITEMGROUPLIST];

            for (int i = 0; i < COL_MAXCOLNUM_ITEMGROUPLIST; i++)
            {
                this.initialPointGroupStyleInfoArr[i] =
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
            //validatingEventRaiserCollection.Add(
            //    ControlValidatingEventRaiser.Create(this.numJigyoshoCode, ctl => ctl.Text, this.numJigyoshoCode_Validating));

        }

        /// <summary>
        /// カナ用のかな文字取得に使用するImeコントロールを初期化します。
        /// </summary>
        private void InitKanaGrapeCityIme()
        {
            //インスタンス作成
            this.ime = new GcIme();
            //カナを取得するコントロールとイベント発生を指示
            this.ime.SetCausesImeEvent(this.edtPointGroupName, true);
            this.ime.SetInputScope(this.edtPointGroupName, GrapeCity.Win.Editors.InputScopeNameValue.Default);
            //イベントハンドラを作成
            this.ime.ResultString +=
                new EventHandler<GrapeCity.Win.Editors.ResultStringEventArgs>(ime_ResultString);
        }

        private void ime_ResultString(object sender, GrapeCity.Win.Editors.ResultStringEventArgs e)
        {
            //名を入力したときにカナを自動セットする
            this.edtPointGroupNameKana.Text += e.ReadString;
        }

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

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitPointGroupFrame();
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
                    this.numPointGroupCode.Enabled = false;
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
                    this.numPointGroupCode.Enabled = true;
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
                    this.numPointGroupCode.Enabled = false;
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
        /// 発着地グループリストに値を設定します。
        /// </summary>
        private void SetPointGroupListSheet()
        {
            //待機状態へ
            //Cursor.Current = Cursors.WaitCursor;
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._PointGroupInfoList =
                    this._PointGroup.GetList();

                IList<PointGroupInfo> wk_list = this.GetMatchedList(this._PointGroupInfoList);

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
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_ITEMGROUPLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_ITEMGROUPLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    PointGroupInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_ITEMGROUPLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialPointGroupStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_ITEMGROUP_CODE, wk_info.PointGroupCode);
                    datamodel.SetValue(j, COL_ITEMGROUP_NAME, wk_info.PointGroupName);
                    datamodel.SetValue(j, COL_ITEMGROUP_KANA, wk_info.PointGroupNameKana);

                    datamodel.SetTag(j, COL_ITEMGROUP_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpPointGroupListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpPointGroupListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpPointGroupListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpPointGroupListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpPointGroupListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpPointGroupListGrid.Sheets[0].ColumnCount - 1);

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
        private List<PointGroupInfo> GetMatchedList(IList<PointGroupInfo> list)
        {
            List<PointGroupInfo> rt_list = new List<PointGroupInfo>();

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

            foreach (PointGroupInfo item in list)
            {
                //「コード」+「名称」+「カナ」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.PointGroupCode) + Environment.NewLine
                    + item.PointGroupName.Trim() + Environment.NewLine
                    + item.PointGroupNameKana.Trim() + Environment.NewLine;

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
        /// 発着地グループの新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の発着地グループ情報を保持する領域の初期化
            this._PointGroupInfo = new PointGroupInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.numPointGroupCode.Focus();
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //発着地グループコードをリストから取得して設定
            this._PointGroupId = this.GetPointGroupIdByPointGroupListOnSelection();

            try
            {
                //発着地グループ情報取得
                this._PointGroupInfo =
                    this._PointGroup.GetInfoById(_PointGroupId);

                //画面設定
                this.SetScreen();

                if (changeMode)
                {
                    //モード設定
                    this.ChangeMode(FrameEditMode.Editable);

                    //フォーカス設定
                    this.edtPointGroupNameKana.Focus();
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
        /// 発着地グループリストにて選択中の発着地グループの発着地グループコードを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の発着地グループID</returns>
        private decimal GetPointGroupIdByPointGroupListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpPointGroupListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからPointGroupInfoを取り出して、PointGroupIdを取得
                    rt_val = ((PointGroupInfo)sheet0.Cells[select_row, COL_ITEMGROUP_CODE].Tag).PointGroupId;
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
            if (this._PointGroupInfo ==null)
            {
                this.ClearInputs();
            }
            else
            {
                this.numPointGroupCode.Value = Convert.ToInt32(_PointGroupInfo.PointGroupCode);
                this.edtPointGroupName.Text = Convert.ToString(_PointGroupInfo.PointGroupName);
                this.edtPointGroupNameKana.Text = Convert.ToString(_PointGroupInfo.PointGroupNameKana);

                this.chkDisableFlag.Checked = _PointGroupInfo.DisableFlag;
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
                        this._PointGroup.Save(tx, this._PointGroupInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "発着地グループ", _PointGroupInfo.PointGroupCode.ToString() });

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

                    this.edtPointGroupNameKana.Focus();
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
                    //他から更新されている場合の例外ハンドラ
                    FrameUtilites.ShowExceptionMessage(ex, this);
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _PointGroupInfo.PointGroupCode = Convert.ToInt32(this.numPointGroupCode.Value);
            _PointGroupInfo.PointGroupName = this.edtPointGroupName.Text.Trim();
            _PointGroupInfo.PointGroupNameKana = this.edtPointGroupNameKana.Text.Trim();
            _PointGroupInfo.DisableFlag = this.chkDisableFlag.Checked;
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
            if (rt_val && Convert.ToDecimal(this.numPointGroupCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numPointGroupCode;
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
                                this._PointGroup.Delete(tx, _PointGroupInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "発着地グループ", _PointGroupInfo.PointGroupCode.ToString() });

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

            //発着地グループリストをセット
            this.SetPointGroupListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpPointGroupListGrid.Focus();
        }

        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCode()
        {
            //コードを取得
            int oldcode = Convert.ToInt32(this.numPointGroupCode.Value);

            //コードの桁数を取得
            int codedigitcount = Convert.ToInt32(this.numPointGroupCode.MaxValue).ToString().Length;

            //コード変更画面を表示
            using (ChangeMasterCodeFrame f =
                new ChangeMasterCodeFrame(DefaultProperty.MasterCodeKbn.PointGroup, oldcode, codedigitcount))
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
                default:
                    break;
            }
        }

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
        private void ProcessFpPointGroupListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void PointGroupFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void PointGroupFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpPointGroupListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //発着地グループリスト - CellDoubleClick
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
        private void fpPointGroupListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpPointGroupListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpPointGroupListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpPointGroupListGridPreviewKeyDown(e);
        }

        private void PointGroupFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetPointGroupListSheet();
        }

        private void fpPointGroupListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }
    }
}
