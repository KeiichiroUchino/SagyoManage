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
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using GrapeCity.Win.MultiRow;
using Jpsys.HaishaManageV10.FrameLib.MultiRow;
using GrapeCity.Win.MultiRow.InputMan;
using System.ComponentModel;
using Jpsys.HaishaManageV10.ComLib;


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class TokuisakiGroupFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public TokuisakiGroupFrame()
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

        #region MultiRow関連

        /// <summary>
        /// 明細の列名を表します。
        /// </summary>
        private enum MrowCellKeys
        {
            TokuisakiCode,
            TokuisakiName
        }

        /// <summary>
        /// 明細の列番号を表します。
        /// </summary>
        private enum MrowCurumnindices : int
        {
            TokuisakiCode = 0,
            TokuisakiName = 1
        }

        /// <summary>
        /// 明細行に必要で画面には表示しない値です。
        /// </summary>
        private class TokuisakiGroupRowValues
        {
            public decimal TokuisakiId { get; set; }
        }

        /// <summary>
        /// 明細の行から値を格納したオブジェクトを取得します。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private TokuisakiGroupRowValues GetTokuisakiGroupListRowValues(int rowIndex)
        {
            return this.mrsTokuisakiGroupList.GetOrNewRowTag<TokuisakiGroupRowValues>(rowIndex);
        }

        /// <summary>
        /// 明細のサイドボタンのNameを格納した配列です。
        /// </summary>
        private readonly string[] ArrayMeisaiSideButtonName = new string[]
        {
            "sbtnTokuisakiCode",
        };

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "得意先グループ登録";

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
        /// 得意先グループクラス
        /// </summary>
        private TokuisakiGroup _TokuisakiGroup;

        /// <summary>
        /// 得意先グループ明細クラス
        /// </summary>
        private TokuisakiGroupMeisai _TokuisakiGroupMeisai;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList = null;

        /// <summary>
        /// 現在編集中の得意先グループ情報を保持する領域
        /// </summary>
        private TokuisakiGroupInfo _TokuisakiGroupInfo = null;

        /// <summary>
        /// 得意先グループIDを保持する領域
        /// </summary>
        private decimal _TokuisakiGroupId = 0;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region 得意先グループ一覧

        //--Spread列定義

        /// <summary>
        /// 得意先グループコード列番号
        /// </summary>
        private const int COL_TOKUISAKIGROUP_CODE = 0;

        /// <summary>
        /// 得意先グループ名列番号
        /// </summary>
        private const int COL_TOKUISAKIGROUP_NAME = 1;

        /// <summary>
        /// 得意先グループリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TOKUISAKIGROUPLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialTokuisakiGroupStyleInfoArr;

        /// <summary>
        /// 得意先グループ情報のリストを保持する領域
        /// </summary>
        private IList<TokuisakiGroupInfo> _TokuisakiGroupInfoList = null;

        /// <summary>
        /// 最大表示行数
        /// </summary>
        public static int COL_MAXROW_TOKUISAKILIST = 1000;

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
        private void InitTokuisakiGroupFrame()
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

            //システム名称リスト取得
            this.systemNameList = this._DalUtil.SystemGlobalName.GetList();

            //得意先グループクラスインスタンス作成
            this._TokuisakiGroup = new TokuisakiGroup(this.appAuth);

            //得意先グループ明細クラスインスタンス作成
            this._TokuisakiGroupMeisai = new TokuisakiGroupMeisai(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //得意先グループ明細のクリア
            this.InitMrow();

            //Spread関連のクリア
            this.InitSheet();

            //得意先グループリストのセット
            this.SetTokuisakiGroupListSheet();

            //現在の画面モードを初期状態に変更
            this.ChangeMode(FrameEditMode.Default);
        }

        /// <summary>
        /// 画面配色の設定をします。
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

        /// <summary>
        /// Spread関連を初期化します。
        /// </summary>
        private void InitSheet()
        {
            //得意先グループリストの初期化
            this.InitTokuisakiGroupListSheet();
        }

        /// <summary>
        /// 得意先グループリストを初期化します。
        /// </summary>
        private void InitTokuisakiGroupListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTokuisakiGroupListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_TOKUISAKIGROUPLIST);
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
            this.numTokuisakiGroupCode.Value = 0;
            this.edtTokuisakiGroupName.Text = string.Empty;
            this.numFukushaTokuisakiGroupCode.Value = 0;
            this.edtFukushaTokuisakiGroupName.Text = string.Empty;

            this.chkDisableFlag.Checked = false;

            this.MeisaiClearInputs();

            this.isConfirmClose = true;
        }

        /// <summary>
        /// 明細をクリアします。
        /// </summary>
        public void MeisaiClearInputs()
        {
            //明細行のクリア
            this.mrsTokuisakiGroupList.Rows.Clear();

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        /// <summary>
        /// Mrow関連を初期化します。
        /// </summary>
        private void InitMrow()
        {
            this.InitTokuisakiGroupListMrow();
        }

        /// <summary>
        /// 明細のMrowを初期化します。
        /// </summary>
        private void InitTokuisakiGroupListMrow()
        {
            //描画を停止
            this.mrsTokuisakiGroupList.SuspendLayout();

            try
            {
                //初期値を設定
                //Mrowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsTokuisakiGroupList.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //***値の初期化
                TemplateInitializer initializer = new TemplateInitializer(mrsTokuisakiGroupList);
                initializer.Initialize();

                //初期値を設定する
                tpl.Row.Cells[MrowCellKeys.TokuisakiCode.ToString()].Value = null;
                tpl.Row.Cells[MrowCellKeys.TokuisakiName.ToString()].Value = string.Empty;

                //テンプレートを再設定
                this.mrsTokuisakiGroupList.Template = tpl;

                //***ショートカットキー
                //基本設定
                this.mrsTokuisakiGroupList.InitialShortcutKeySetting();
                //--F5制御を追加（独自にアクションクラスを作成）
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Register(new DelegateAction(this.ShowCmnSearchTokuisakiGroupList), Keys.F5);

                //--上キー制御を変更（前の行へ移動）
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Unregister(Keys.Up);
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousRow, Keys.Up);

                //--下キー制御を変更（次の行へ移動）
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Unregister(Keys.Down);
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToNextRow, Keys.Down);

                //--左キー制御を変更（前のセルへ移動 ※行の折り返しあり）
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Unregister(Keys.Left);
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCell, Keys.Left);

                //--右キー制御を変更（次のセルへ移動 ※行の折り返しあり）
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Unregister(Keys.Right);
                this.mrsTokuisakiGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Right);

                //--単一セル選択モード
                this.mrsTokuisakiGroupList.MultiSelect = false;

                //--ヘッダの列幅変更を禁止する
                //---列ヘッダと行ヘッダ個別で列幅変更の可否は調整できません。 T.Kuroki@NSK
                this.mrsTokuisakiGroupList.AllowUserToResize = false;

                //行をクリア
                //---MultiRow.Clear() メソッドは未対応
                this.mrsTokuisakiGroupList.Rows.Clear();

                //--ユーザでの行追加を許可する
                this.mrsTokuisakiGroupList.AllowUserToAddRows = this.IsNotMaxRows();
                //--ユーザでの行削除は許可しない
                this.mrsTokuisakiGroupList.AllowUserToDeleteRows = false;

                //--フォーカスが無い時はセルのハイライトを表示しない
                this.mrsTokuisakiGroupList.HideSelection = true;

                //--分割ボタンを表示しない
                this.mrsTokuisakiGroupList.SplitMode = SplitMode.None;
            }
            finally
            {
                this.mrsTokuisakiGroupList.ResumeLayout();
            }
        }

        /// <summary>
        /// 明細のコード値検索画面を表示します。
        /// </summary>
        /// <param name="curMRow"></param>
        private void ShowCmnSearchTokuisakiGroupList(GcMultiRow curMRow)
        {
            GrapeCity.Win.MultiRow.Cell curCell = curMRow.CurrentCell;
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;
            GrapeCity.Win.MultiRow.Row firstRow = null;
            int firstCode = 0;

            if (curRow == null || curCell == null)
            {
                return;
            }

            if (!curCell.Selectable || curCell.ReadOnly)
            {
                return;
            }

            if (curCell.Name == MrowCellKeys.TokuisakiCode.ToString())
            {
                using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
                {
                    //複数選択可
                    f.AllowExtendedSelectRow = true;
                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        // 画面から値を取得
                        List<TokuisakiInfo> list = f.SelectedInfoList;

                        //戻り値が存在する場合
                        if (list != null && 0 < list.Count)
                        {
                            //描画を止める
                            curMRow.SuspendLayout();

                            //選択行インデックス取得
                            int gyo = curMRow.CurrentCellPosition.RowIndex;

                            //作業行取得
                            int wk_gyo = gyo;

                            //カレント得意先コード
                            string curTokuisakiCode = Convert.ToInt32(curCell.Value).ToString();

                            //初行フラグ
                            bool isFirst = true;

                            //待機状態へ
                            this.Cursor = Cursors.WaitCursor;

                            try
                            {
                                //戻り値の件数分、処理を繰り返す
                                foreach (TokuisakiInfo info in list)
                                {
                                    //存在フラグ
                                    bool exists = false;

                                    for (int i = 0; i < curMRow.RowCount; i++)
                                    {
                                        //カレント得意先コード（自分）以外でリストにすでに存在する場合
                                        if (!curTokuisakiCode.Equals(StringHelper.ConvertToString(info.ToraDONTokuisakiCode))
                                            &&
                                            StringHelper.ConvertToString(curMRow.GetFormattedValue(i, (int)MrowCurumnindices.TokuisakiCode))
                                            .Equals(StringHelper.ConvertToString(info.ToraDONTokuisakiCode)))
                                        {
                                            //存在フラグを「true：存在する」を設定
                                            exists = true;
                                            break;
                                        }
                                    }

                                    //得意先コードが得意先グループリストに存在しない場合
                                    if (!exists)
                                    {
                                        //初行の場合
                                        if (isFirst)
                                        {
                                            GrapeCity.Win.MultiRow.Row wk_Row = null;

                                            //初行が新規行の場合
                                            if (curMRow.IsNewRow(gyo))
                                            {
                                                //行を追加
                                                curMRow.Rows.Insert(wk_gyo);

                                                //作業行取得
                                                wk_Row = wk_Row = curMRow.Rows[wk_gyo];
                                            }
                                            //新規行以外の場合
                                            else
                                            {
                                                //作業行取得
                                                wk_Row = curRow;
                                            }

                                            //値設定
                                            wk_Row[MrowCellKeys.TokuisakiCode.ToString()].Value = info.ToraDONTokuisakiCode;
                                            wk_Row[MrowCellKeys.TokuisakiName.ToString()].Value = info.ToraDONTokuisakiName;
                                            TokuisakiGroupRowValues rowValues = this.GetTokuisakiGroupListRowValues(wk_Row.Index);
                                            rowValues.TokuisakiId = info.ToraDONTokuisakiId;

                                            //初行保持
                                            firstRow = wk_Row;
                                            firstCode = info.ToraDONTokuisakiCode;

                                            //初行フラグ解除
                                            isFirst = false;

                                            //カウントアップ
                                            wk_gyo++;
                                        }
                                        else
                                        {
                                            //最大行以下の場合
                                            if (curMRow.RowCount <= COL_MAXROW_TOKUISAKILIST)
                                            {
                                                //最大行に達していない場合
                                                if (curMRow.RowCount <= COL_MAXROW_TOKUISAKILIST)
                                                {
                                                    //行を追加
                                                    curMRow.Rows.Insert(wk_gyo);
                                                }

                                                //作業行取得
                                                GrapeCity.Win.MultiRow.Row wk_Row = wk_Row = curMRow.Rows[wk_gyo];

                                                //追加した行に値を設定
                                                wk_Row[MrowCellKeys.TokuisakiCode.ToString()].Value = info.ToraDONTokuisakiCode;
                                                wk_Row[MrowCellKeys.TokuisakiName.ToString()].Value = info.ToraDONTokuisakiName;
                                                TokuisakiGroupRowValues rowValues = this.GetTokuisakiGroupListRowValues(wk_Row.Index);
                                                rowValues.TokuisakiId = info.ToraDONTokuisakiId;

                                                //カウントアップ
                                                wk_gyo++;
                                            }
                                        }

                                        //作業行が最大行に達した場合
                                        if (COL_MAXROW_TOKUISAKILIST <= wk_gyo)
                                        {
                                            //終了
                                            break;
                                        }
                                    }
                                }
                            }
                            finally
                            {
                                //描画を再開
                                curMRow.ResumeLayout();
                                this.Cursor = Cursors.Default;
                            }
                        }

                        //--ユーザでの行追加を制御する
                        curMRow.AllowUserToAddRows = this.IsNotMaxRows();

                        //初行が存在する場合
                        if (0 < firstCode)
                        {
                            //カーソルを初行に設定
                            curMRow.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(firstRow.Index, MrowCurumnindices.TokuisakiCode.ToString());
                            curMRow.BeginEdit(firstCode);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 明細の挿入処理を行います。
        /// </summary>
        /// <param name="curMRow"></param>
        private void AddMeisaiListRow(GcMultiRow curMRow)
        {
            //現在の行の位置を取得しておく
            GrapeCity.Win.MultiRow.Row currentRow = curMRow.CurrentRow;

            if (currentRow == null)
                return;

            int curRowIndex = currentRow.Index;

            if (curMRow.IsNewRow(curRowIndex))
                return;

            // 確認MSG
            DialogResult res =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                        "MQ2102014", new string[] { (curRowIndex + 1).ToString() }),
                    this.Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question
                    );

            if (res == DialogResult.Yes)
            {
                //行を追加します
                curMRow.Rows.Insert(curRowIndex);

                //選択可能な最初のセルに移動
                curMRow.CellPositionToSelectableFirstOrder(curRowIndex);
            }
        }

        /// <summary>
        /// 明細の削除処理を行います。
        /// </summary>
        /// <param name="curMRow"></param>
        private void RemoveMeisaiListRow(GcMultiRow curMRow)
        {
            //現在の行の位置を取得しておく
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;

            int curRowIndex = curRow.Index;

            if (curMRow.IsNewRow(curRowIndex))
                return;

            var rowValues = this.GetTokuisakiGroupListRowValues(curRowIndex);

            //処理前に確認メッセージ
            DialogResult wk_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                        "MQ2102013", new string[] { (curRowIndex + 1).ToString() }),
                    this.Text,
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning,
                    MessageBoxDefaultButton.Button2
                    );

            if (wk_result == DialogResult.Yes)
            {
                //行を削除
                curMRow.Rows.RemoveAt(curRow.Index);

                //選択可能な最初のセルに移動
                curMRow.CellPositionToSelectableFirstOfCurrentRow();
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
            //得意先グループスタイル情報初期化
            this.InitTokuisakiGroupStyleInfo();
        }

        /// <summary>
        /// 得意先グループのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitTokuisakiGroupStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpTokuisakiGroupListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialTokuisakiGroupStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TOKUISAKIGROUPLIST];

            for (int i = 0; i < COL_MAXCOLNUM_TOKUISAKIGROUPLIST; i++)
            {
                this.initialTokuisakiGroupStyleInfoArr[i] =
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
                ControlValidatingEventRaiser.Create(this.numFukushaTokuisakiGroupCode, ctl => ctl.Text, this.numFukushaTokuisakiGroupCode_Validating));
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            this.searchStateBinder.AddSearchableControls(this.numFukushaTokuisakiGroupCode);
            this.searchStateBinder.AddSearchableControls(
                this.mrsTokuisakiGroupList,
                MrowCellKeys.TokuisakiCode.ToString()
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

        void GyoSakujyo_Execute(object sender, EventArgs e)
        {
            this.RemoveMeisaiGyo();
        }

        void GyoSonyu_Execute(object sender, EventArgs e)
        {
            this.AddMeisaiGyo();
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
        /// 得意先グループコードの値を取得します。
        /// </summary>
        private int FukushaTokuisakiGroupCode
        {
            get { return Convert.ToInt32(this.numFukushaTokuisakiGroupCode.Value); }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitTokuisakiGroupFrame();
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
                    this.numTokuisakiGroupCode.Enabled = false;
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
                    this.numTokuisakiGroupCode.Enabled = true;
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
                    this.numTokuisakiGroupCode.Enabled = false;
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
        /// 得意先グループリストに値を設定します。
        /// </summary>
        private void SetTokuisakiGroupListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._TokuisakiGroupInfoList =
                    this._TokuisakiGroup.GetList();

                IList<TokuisakiGroupInfo> wk_list = this.GetMatchedList(this._TokuisakiGroupInfoList);

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
                    .OrderBy(element => element.TokuisakiGroupCode)
                    .ToList();

                //Spreadのデータモデルを作る
                DefaultSheetDataModel datamodel =
                    new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_TOKUISAKIGROUPLIST);

                //Spreadのスタイルモデルの作成
                DefaultSheetStyleModel stylemodel =
                    new DefaultSheetStyleModel(wk_list.Count, COL_MAXCOLNUM_TOKUISAKIGROUPLIST);

                //ループしてデータモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    //1件分を取得しておく
                    TokuisakiGroupInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TOKUISAKIGROUPLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialTokuisakiGroupStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_TOKUISAKIGROUP_CODE, wk_info.TokuisakiGroupCode);
                    datamodel.SetValue(j, COL_TOKUISAKIGROUP_NAME, wk_info.TokuisakiGroupName);

                    datamodel.SetTag(j, COL_TOKUISAKIGROUP_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpTokuisakiGroupListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpTokuisakiGroupListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpTokuisakiGroupListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpTokuisakiGroupListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpTokuisakiGroupListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpTokuisakiGroupListGrid.Sheets[0].ColumnCount - 1);

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
        private List<TokuisakiGroupInfo> GetMatchedList(IList<TokuisakiGroupInfo> list)
        {
            List<TokuisakiGroupInfo> rt_list = new List<TokuisakiGroupInfo>();

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

            foreach (TokuisakiGroupInfo item in list)
            {
                //「コード」+「名称」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.TokuisakiGroupCode) + Environment.NewLine
                    + item.TokuisakiGroupName.Trim() + Environment.NewLine;

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
        /// 得意先グループの新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の得意先グループ情報を保持する領域の初期化
            this._TokuisakiGroupInfo = new TokuisakiGroupInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.numTokuisakiGroupCode.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //得意先グループコードをリストから取得して設定
            this._TokuisakiGroupId = this.GetTokuisakiGroupIdByTokuisakiGroupListOnSelection();

            if (this._TokuisakiGroupId == 0)
            {
                this._TokuisakiGroupInfo = new TokuisakiGroupInfo();

                this.ChangeMode(FrameEditMode.New);
                this.numTokuisakiGroupCode.Focus();
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
            }
            else
            {
                try
                {
                    //得意先グループ取得
                    this._TokuisakiGroupInfo =
                        this._TokuisakiGroup.GetInfoById(this._TokuisakiGroupId);

                    //得意先グループ明細取得
                    this._TokuisakiGroupInfo.TokuisakiGroupMeisaiList =
                        this._TokuisakiGroupMeisai.GetList(
                        new TokuisakiGroupMeisaiSearchParameter()
                        {
                            TokuisakiGroupId = this._TokuisakiGroupId
                        });

                    //画面表示
                    this.SetScreen();

                    if (changeMode)
                    {
                        //編集モード
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        this.edtTokuisakiGroupName.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
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
        /// 得意先グループリストにて選択中の得意先グループの得意先グループIDを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の得意先グループID</returns>
        private decimal GetTokuisakiGroupIdByTokuisakiGroupListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpTokuisakiGroupListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからTokuisakiGroupInfoを取り出して、TokuisakiGroupIdを取得
                    rt_val = ((TokuisakiGroupInfo)sheet0.Cells[select_row, COL_TOKUISAKIGROUP_CODE].Tag).TokuisakiGroupId;
                }
            }

            //返却
            return rt_val;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        /// <param name="initflag"></param>
        private void SetScreen(bool initflag = true)
        {
            if (this._TokuisakiGroupInfo == null)
            {
                this.ClearInputs();
            }
            else
            {
                if (initflag)
                {
                    this.numTokuisakiGroupCode.Value = Convert.ToDecimal(_TokuisakiGroupInfo.TokuisakiGroupCode);
                    this.edtTokuisakiGroupName.Text = Convert.ToString(_TokuisakiGroupInfo.TokuisakiGroupName);
                    this.numFukushaTokuisakiGroupCode.Value = 0;
                    this.edtFukushaTokuisakiGroupName.Text = string.Empty;

                    this.chkDisableFlag.Checked = _TokuisakiGroupInfo.DisableFlag;
                }

                this.SetScreenTokuisakiGroupMeisai();
            }
        }

        /// <summary>
        /// 明細を画面に設定します。
        /// </summary>
        private void SetScreenTokuisakiGroupMeisai()
        {
            //一行目にセットフォーカスするため、いったん許可しない
            //--ユーザでの行追加は許可しない
            this.mrsTokuisakiGroupList.AllowUserToAddRows = false;

            //MultiRowの描画を停止
            this.mrsTokuisakiGroupList.SuspendLayout();

            try
            {
                //明細をクリア
                this.mrsTokuisakiGroupList.Rows.Clear();

                var lynaTokuisakiGroupMeisaiList =
                    _TokuisakiGroupInfo.TokuisakiGroupMeisaiList
                    .GroupBy(element =>
                        new
                        {
                            element.Gyo,
                            element.ToraDONTokuisakiId,
                            element.ToraDONTokuisakiCode,
                            element.ToraDONTokuisakiName,
                        })
                    .OrderBy(element => element.Key.Gyo)
                    .ToList();


                //件数取得
                int rowcount = lynaTokuisakiGroupMeisaiList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //明細の行数を設定（新規行なし）
                this.mrsTokuisakiGroupList.RowCount = rowcount;

                for (int i = 0; i < rowcount; i++)
                {
                    var meisaiList = lynaTokuisakiGroupMeisaiList[i];

                    //Mrow取得
                    GrapeCity.Win.MultiRow.Row row = this.mrsTokuisakiGroupList.Rows[i];

                    row.SetValue(MrowCellKeys.TokuisakiCode.ToString(), lynaTokuisakiGroupMeisaiList[i].Key.ToraDONTokuisakiCode);
                    row.SetValue(MrowCellKeys.TokuisakiName.ToString(), lynaTokuisakiGroupMeisaiList[i].Key.ToraDONTokuisakiName);
                    //row.SetValue(MrowCellKeys.RenketsuKbn.ToString(),
                    //    DefaultProperty.GetRenketsuKbnMeisho(
                    //        (DefaultProperty.RenketsuKbn)Convert.ToInt32(lynaTokuisakiGroupMeisaiList[i].Key.RenketsuKbn)
                    //        ));

                    var rowValues = this.GetTokuisakiGroupListRowValues(i);
                    rowValues.TokuisakiId = lynaTokuisakiGroupMeisaiList[i].Key.ToraDONTokuisakiId;
                }

                //表示明細が存在する場合
                if (0 < rowcount)
                {
                    //1行目の1セルを選択
                    this.mrsTokuisakiGroupList.CurrentCellPosition = new CellPosition(0, MrowCurumnindices.TokuisakiCode.ToString());
                }
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsTokuisakiGroupList.ResumeLayout();

                //--ユーザでの行追加を許可
                this.mrsTokuisakiGroupList.AllowUserToAddRows = true;
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

                //待機状態へ
                this.Cursor = Cursors.WaitCursor;

                try
                {
                    //保存
                    SQLHelper.ActionWithTransaction(tx =>
                    {
                        this._TokuisakiGroup.Save(tx, this._TokuisakiGroupInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "得意先グループ", _TokuisakiGroupInfo.TokuisakiGroupCode.ToString() });

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
                    //フォーカスを移動
                    if (this.currentMode == FrameEditMode.New)
                    {
                        this.numTokuisakiGroupCode.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                    else
                    {
                        this.edtTokuisakiGroupName.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
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
                        this.numTokuisakiGroupCode.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                    else
                    {
                        this.edtTokuisakiGroupName.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                }
                finally
                {
                    this.Cursor = Cursors.Default;
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            _TokuisakiGroupInfo.TokuisakiGroupId = this._TokuisakiGroupId;
            _TokuisakiGroupInfo.TokuisakiGroupCode = Convert.ToInt32(this.numTokuisakiGroupCode.Value);
            _TokuisakiGroupInfo.TokuisakiGroupName = this.edtTokuisakiGroupName.Text.Trim();

            //得意先グループ明細取得
            _TokuisakiGroupInfo.TokuisakiGroupMeisaiList = this.GetScreenTokuisakiGroupMeisai();

            _TokuisakiGroupInfo.DisableFlag = this.chkDisableFlag.Checked;
        }

        /// <summary>
        /// 画面の明細値をコントローラーにセットします。
        /// </summary>
        /// <returns>得意先グループ明細リスト</returns>
        private IList<TokuisakiGroupMeisaiInfo> GetScreenTokuisakiGroupMeisai()
        {
            IList<TokuisakiGroupMeisaiInfo> rt_list = new List<TokuisakiGroupMeisaiInfo>();

            int gyo = 0;
            foreach (GrapeCity.Win.MultiRow.Row row in this.mrsTokuisakiGroupList.Rows)
            {
                //得意先コードが入力されている場合
                if (!row.IsNewRow && this.GetTokuisakiGroupListRowValues(row.Index).TokuisakiId != 0)
                {
                    //画面明細取得
                    rt_list.Add(new TokuisakiGroupMeisaiInfo()
                    {
                        TokuisakiGroupId = this._TokuisakiGroupId,
                        Gyo = gyo++,
                        ToraDONTokuisakiId = this.GetTokuisakiGroupListRowValues(row.Index).TokuisakiId,
                    });
                }
            }

            return rt_list;
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

            //フォーカス移動先
            CellPosition cpt = new CellPosition();

            //コードの必須入力チェック
            if (rt_val && Convert.ToDecimal(this.numTokuisakiGroupCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numTokuisakiGroupCode;
            }

            //名称の必須入力チェック
            if (rt_val && (this.edtTokuisakiGroupName.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "名称" });
                ctl = this.edtTokuisakiGroupName;
            }

            //得意先の入力チェック
            if (rt_val && (0 < this.mrsTokuisakiGroupList.RowCount))
            {
                List<TokuisakiGroupMeisaiInfo> list = new List<TokuisakiGroupMeisaiInfo>();
                foreach (GrapeCity.Win.MultiRow.Row row in this.mrsTokuisakiGroupList.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row[MrowCellKeys.TokuisakiCode.ToString()].Value != null)
                        {
                            String code = row[MrowCellKeys.TokuisakiCode.ToString()].Value.ToString();
                            list.Add(new TokuisakiGroupMeisaiInfo()
                            {
                                Gyo = row.Index,
                                ToraDONTokuisakiCode = Convert.ToInt32(code),
                                ToraDONTokuisakiId = this.GetTokuisakiGroupListRowValues(row.Index).TokuisakiId,
                            });
                        }
                        else
                        {
                            list.Add(new TokuisakiGroupMeisaiInfo()
                            {
                                Gyo = row.Index,
                                ToraDONTokuisakiCode = 0,
                                ToraDONTokuisakiId = 0,
                            });
                        }
                    }
                }

                var check_list = list.Where(x => x.ToraDONTokuisakiCode == 0).ToList();

                if (0 < check_list.Count)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2203001", new string[] { "得意先コード" });
                    cpt = new CellPosition(check_list[0].Gyo, MrowCurumnindices.TokuisakiCode.ToString());
                }
                else
                {
                    //得意先コードの重複チェック
                    check_list =
                      list.Where(x => 0 < x.ToraDONTokuisakiId)
                      .GroupBy(x => x.ToraDONTokuisakiId)
                      .Where(g => g.Count() > 1)
                      .Select(g => g.FirstOrDefault()).ToList();

                    if (0 < check_list.Count)
                    {
                        rt_val = false;
                        msg = FrameUtilites.GetDefineMessage(
                            "MW2203010", new string[] { "得意先コード" });
                        cpt = new CellPosition(check_list[0].Gyo, MrowCurumnindices.TokuisakiCode.ToString());
                    }
                }
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (ctl != null)
                {
                    ctl.Focus();
                }
                else
                {
                    this.mrsTokuisakiGroupList.CurrentCellPosition = cpt;
                    this.mrsTokuisakiGroupList.Focus();
                }
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
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
                                this._TokuisakiGroup.Delete(tx, _TokuisakiGroupInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "得意先グループ", _TokuisakiGroupInfo.TokuisakiGroupCode.ToString() });

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
        /// 得意先グループ明細一覧に行挿入します。
        /// </summary>
        private void AddMeisaiGyo()
        {
            if (this.mrsTokuisakiGroupList.RowCount < COL_MAXROW_TOKUISAKILIST)
            {
                // 明細挿入
                this.AddMeisaiListRow(this.mrsTokuisakiGroupList);
                this.mrsTokuisakiGroupList.Focus();
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);

                //--ユーザでの行追加を制御する
                this.mrsTokuisakiGroupList.AllowUserToAddRows = this.IsNotMaxRows();
            }
        }

        /// <summary>
        /// 得意先グループ明細一覧から行削除します。
        /// </summary>
        private void RemoveMeisaiGyo()
        {
            // 明細削除
            this.RemoveMeisaiListRow(this.mrsTokuisakiGroupList);
            this.mrsTokuisakiGroupList.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);

            //--ユーザでの行追加を制御する
            this.mrsTokuisakiGroupList.AllowUserToAddRows = this.IsNotMaxRows();
        }

        /// <summary>
        /// 得意先グループ明細一覧が最大行数か判定します。
        /// （true：最大行数未満、false：最大行数）
        /// </summary>
        private bool IsNotMaxRows()
        {
            return !(COL_MAXROW_TOKUISAKILIST < this.mrsTokuisakiGroupList.RowCount);
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

            //得意先グループリストをセット
            this.SetTokuisakiGroupListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpTokuisakiGroupListGrid.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }
        
        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCode()
        {
            ////コードを取得
            //int oldcode = Convert.ToInt32(this.numTokuisakiGroupCode.Value);

            ////コードの桁数を取得
            //int codedigitcount = Convert.ToInt32(this.numTokuisakiGroupCode.MaxValue).ToString().Length;

            ////コード変更画面を表示
            //using (ChangeMasterCodeFrame f =
            //    new ChangeMasterCodeFrame(DefaultProperty.MasterCodeKbn.TokuisakiGroup, oldcode, codedigitcount))
            //{
            //    f.InitFrame();

            //    DialogResult wk_result = f.ShowDialog(this);

            //    //正常に処理されたら初期状態へ変更
            //    if (wk_result == DialogResult.OK)
            //    {
            //        //画面初期化
            //        this.DoClear(false);
            //    }
            //}
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
            if (this.ActiveControl == this.numFukushaTokuisakiGroupCode)
            {
                this.ShowCmnSearchTokuisakiGroup();
            }
        }

        /// <summary>
        /// 得意先グループ検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchTokuisakiGroup()
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
                        Convert.ToInt32(f.SelectedInfo.TokuisakiGroupCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 複写元得意先グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateFukushaTokuisakiGroupCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (Convert.ToInt32(this.numFukushaTokuisakiGroupCode.Value) == 0)
                {
                    this.numFukushaTokuisakiGroupCode.Value = null;
                    this.edtFukushaTokuisakiGroupName.Text = string.Empty;
                    is_clear = true;
                    return;
                }

                TokuisakiGroupInfo info =
                    this._DalUtil.TokuisakiGroup.GetInfo(this.FukushaTokuisakiGroupCode);

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
                        //得意先グループ明細取得
                        this._TokuisakiGroupInfo.TokuisakiGroupMeisaiList =
                            this._TokuisakiGroupMeisai.GetList(
                            new TokuisakiGroupMeisaiSearchParameter()
                            {
                                TokuisakiGroupId = info.TokuisakiGroupId
                            });

                        //得意先グループ明細表示
                        this.SetScreen(false);

                        this.numFukushaTokuisakiGroupCode.Value = info.TokuisakiGroupCode;
                        this.edtFukushaTokuisakiGroupName.Text = info.TokuisakiGroupName;

                        this.mrsTokuisakiGroupList.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numFukushaTokuisakiGroupCode.Value = null;
                    this.edtFukushaTokuisakiGroupName.Text = string.Empty;
                }
            }
        }

        #endregion

        #region CellValidatingの処理

        /// <summary>
        /// 明細一覧のMrowのCellValidatingイベントを処理します。
        /// </summary>
        /// <param name="e">CellValidatingイベントのイベントデータ</param>
        private void ProcessTokuisakiGroupMrowCellValidating(CellValidatingEventArgs e)
        {
            //編集された場合のみ処理する
            if (!this.mrsTokuisakiGroupList.IsCurrentCellInEditMode)
            {
                return;
            }

            //描画を止める
            this.mrsTokuisakiGroupList.SuspendLayout();

            try
            {
                //現在アクティブなセルのキーを取得
                string wk_curcellkey = this.mrsTokuisakiGroupList.CurrentCell.Name;

                if (wk_curcellkey == MrowCellKeys.TokuisakiCode.ToString())
                {
                    e.Cancel = this.TokuisakiCodeCellValidating(e.RowIndex);
                }

                if (e.Cancel)
                {
                    //クリア
                    EditingActions.ClearEdit.Execute(this.mrsTokuisakiGroupList);

                    //前の値に戻す前にいったん確定する
                    EditingActions.EndEdit.Execute(this.mrsTokuisakiGroupList);

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsTokuisakiGroupList);
                }
            }
            finally
            {
                //描画を再開
                this.mrsTokuisakiGroupList.ResumeLayout();
            }
        }

        /// <summary>
        /// 明細の得意先コード入力枠のCellValidating処理を行い
        /// イベントのキャンセルの有無を返します。
        /// (true:イベントをキャンセルする)
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>CellValidatingイベントキャンセルの可否(true:キャンセルする)</returns>
        private bool TokuisakiCodeCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            var row = this.mrsTokuisakiGroupList.Rows[rowIndex];

            var rowValues = this.GetTokuisakiGroupListRowValues(rowIndex);

            //クリアするかどうか（true：クリア）
            bool is_clear = false;
            //セル値
            int cell_value =
                Convert.ToInt32(this.mrsTokuisakiGroupList.CurrentCell.Value);
            //編集値
            int editing_value =
                Convert.ToInt32(
                    FrameUtilites.GetEditingControlValue(this.mrsTokuisakiGroupList.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (!valueChanged)
                return false;

            try
            {
                if (editing_value == 0)
                {
                    //未入力時はクリアのみ
                    is_clear = true;
                }
                else
                {
                    //情報を取得
                    TokuisakiInfo info =
                        _DalUtil.Tokuisaki.GetInfo(editing_value, null);

                    if (!rt_val)
                    {
                        if (info == null)
                        {
                            //編集をキャンセル
                            rt_val = true;
                            is_clear = true;

                            MessageBox.Show(
                                FrameUtilites.GetDefineMessage("MW2201003"),
                                this.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                        }
                        else
                        {
                            if (info.ToraDONDisableFlag)
                            {
                                //編集をキャンセル
                                rt_val = true;
                                is_clear = true;

                                MessageBox.Show(
                                    FrameUtilites.GetDefineMessage("MW2201016"),
                                    this.Text,
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                        }
                    }

                    //編集エラーでなければ値をセット
                    if (!rt_val)
                    {
                        row.SetValue(MrowCellKeys.TokuisakiName.ToString(), info.ToraDONTokuisakiName);
                        rowValues.TokuisakiId = info.ToraDONTokuisakiId;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    //関連する情報クリア
                    row.SetValue(MrowCellKeys.TokuisakiCode.ToString(), null);
                    row.SetValue(MrowCellKeys.TokuisakiName.ToString(), string.Empty);
                    rowValues.TokuisakiId = 0;
                }
            }

            return rt_val;
        }

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
        private void ProcessFpTokuisakiGroupListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void TokuisakiGroupFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void TokuisakiGroupFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpTokuisakiGroupListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //得意先グループリスト - CellDoubleClick
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
        private void fpTokuisakiGroupListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpTokuisakiGroupListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpTokuisakiGroupListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpTokuisakiGroupListGridPreviewKeyDown(e);
        }

        private void TokuisakiGroupFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetTokuisakiGroupListSheet();
        }

        private void numFukushaTokuisakiGroupCode_Validating(object sender, CancelEventArgs e)
        {
            //複写元得意先グループコード
            this.ValidateFukushaTokuisakiGroupCode(e);
        }

        private void mrsTokuisakiGroupList_CellValidating(object sender, CellValidatingEventArgs e)
        {
            this.ProcessTokuisakiGroupMrowCellValidating(e);
        }

        private void mrsTokuisakiGroupList_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
        {
            //数値セルでサイドボタンが存在する
            if (e.Control is GcNumberEditingControl &&
                (e.Control as GcNumberEditingControl).SideButtons.Count != 0)
            {
                if (e.Control is GcNumberEditingControl)
                {
                    GrapeCity.Win.Editors.SideButton sideButton =
                        (e.Control as GcNumberEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;
                    if (this.ArrayMeisaiSideButtonName.Any(x => x == sideButton.Name))
                    {
                        sideButton.Click -= new EventHandler(mrsTokuisakiGroupListSideButton_Click);
                        sideButton.Click += new EventHandler(mrsTokuisakiGroupListSideButton_Click);
                    }
                }
            }

            // 編集用コントロールが GcTextBoxEditingControl の場合、KeyDownイベントを設定します。
            if (e.Control.GetType() == typeof(GcTextBoxEditingControl))
            {
                e.Control.KeyDown -= GcTextBoxEditingControl_KeyDown;
                e.Control.KeyDown += GcTextBoxEditingControl_KeyDown;
            }
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

        private void mrsTokuisakiGroupList_RowsAdded(object sender, RowsAddedEventArgs e)
        {
            if (FrameUtilites.GetMaxRowNumTankList() < this.mrsTokuisakiGroupList.RowCount)
            {
                //--ユーザでの行追加を制御する
                this.mrsTokuisakiGroupList.AllowUserToAddRows = this.IsNotMaxRows();
            }
        }

        private void mrsTokuisakiGroupListSideButton_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchTokuisakiGroupList(this.mrsTokuisakiGroupList);
        }

        private void fpTokuisakiGroupListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }

        private void sbtnFukushaTokuisakiGroupCode_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchTokuisakiGroup();
        }

        private void btnRemoveMeisaiGyo_Click(object sender, EventArgs e)
        {
            this.RemoveMeisaiGyo();
        }

        private void btnAddMeisaiGyo_Click(object sender, EventArgs e)
        {
            this.AddMeisaiGyo();
        }
    }
}
