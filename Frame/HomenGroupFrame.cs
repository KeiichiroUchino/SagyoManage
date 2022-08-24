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
    public partial class HomenGroupFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HomenGroupFrame()
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
            HomenCode,
            HomenName
        }

        /// <summary>
        /// 明細の列番号を表します。
        /// </summary>
        private enum MrowCurumnindices : int
        {
            HomenCode = 0,
            HomenName = 1
        }

        /// <summary>
        /// 明細行に必要で画面には表示しない値です。
        /// </summary>
        private class HomenGroupRowValues
        {
            public decimal HomenId { get; set; }
        }

        /// <summary>
        /// 明細の行から値を格納したオブジェクトを取得します。
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns></returns>
        private HomenGroupRowValues GetHomenGroupListRowValues(int rowIndex)
        {
            return this.mrsHomenGroupList.GetOrNewRowTag<HomenGroupRowValues>(rowIndex);
        }

        /// <summary>
        /// 明細のサイドボタンのNameを格納した配列です。
        /// </summary>
        private readonly string[] ArrayMeisaiSideButtonName = new string[]
        {
            "sbtnHomenCode",
        };

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "方面グループ登録";

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
        /// 方面グループクラス
        /// </summary>
        private HomenGroup _HomenGroup;

        /// <summary>
        /// 方面グループ明細クラス
        /// </summary>
        private HomenGroupMeisai _HomenGroupMeisai;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList = null;

        /// <summary>
        /// 現在編集中の方面グループ情報を保持する領域
        /// </summary>
        private HomenGroupInfo _HomenGroupInfo = null;

        /// <summary>
        /// 方面グループIDを保持する領域
        /// </summary>
        private decimal _HomenGroupId = 0;

        /// <summary>
        /// 終了確認を行うかどうかの値を保持する領域
        /// (true:終了確認する)
        /// </summary>
        private bool isConfirmClose = true;

        #region 方面グループ一覧

        //--Spread列定義

        /// <summary>
        /// 方面グループコード列番号
        /// </summary>
        private const int COL_TOKUISAKIGROUP_CODE = 0;

        /// <summary>
        /// 方面グループ名列番号
        /// </summary>
        private const int COL_TOKUISAKIGROUP_NAME = 1;

        /// <summary>
        /// 方面グループリスト最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_TOKUISAKIGROUPLIST = 2;

        /// <summary>
        /// Spreadのスタイルモデルの各列のStyleInfoの保持
        /// </summary>
        private StyleInfo[] initialHomenGroupStyleInfoArr;

        /// <summary>
        /// 方面グループ情報のリストを保持する領域
        /// </summary>
        private IList<HomenGroupInfo> _HomenGroupInfoList = null;

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
        private void InitHomenGroupFrame()
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

            //方面グループクラスインスタンス作成
            this._HomenGroup = new HomenGroup(this.appAuth);

            //方面グループ明細クラスインスタンス作成
            this._HomenGroupMeisai = new HomenGroupMeisai(this.appAuth);

            //Spreadのスタイルモデルに関する初期化
            this.InitStyleInfo();

            //検索条件のクリア
            this.ClearSearchInputs();

            //入力項目のクリア
            this.ClearInputs();

            //方面グループ明細のクリア
            this.InitMrow();

            //Spread関連のクリア
            this.InitSheet();

            //方面グループリストのセット
            this.SetHomenGroupListSheet();

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
            //方面グループリストの初期化
            this.InitHomenGroupListSheet();
        }

        /// <summary>
        /// 方面グループリストを初期化します。
        /// </summary>
        private void InitHomenGroupListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHomenGroupListGrid.Sheets[0];

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
            this.numHomenGroupCode.Value = 0;
            this.edtHomenGroupName.Text = string.Empty;
            this.numFukushaHomenGroupCode.Value = 0;
            this.edtFukushaHomenGroupName.Text = string.Empty;

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
            this.mrsHomenGroupList.Rows.Clear();

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        /// <summary>
        /// Mrow関連を初期化します。
        /// </summary>
        private void InitMrow()
        {
            this.InitHomenGroupListMrow();
        }

        /// <summary>
        /// 明細のMrowを初期化します。
        /// </summary>
        private void InitHomenGroupListMrow()
        {
            //描画を停止
            this.mrsHomenGroupList.SuspendLayout();

            try
            {
                //初期値を設定
                //Mrowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsHomenGroupList.Template;
                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //***値の初期化
                TemplateInitializer initializer = new TemplateInitializer(mrsHomenGroupList);
                initializer.Initialize();

                //初期値を設定する
                tpl.Row.Cells[MrowCellKeys.HomenCode.ToString()].Value = null;
                tpl.Row.Cells[MrowCellKeys.HomenName.ToString()].Value = string.Empty;

                //テンプレートを再設定
                this.mrsHomenGroupList.Template = tpl;

                //***ショートカットキー
                //基本設定
                this.mrsHomenGroupList.InitialShortcutKeySetting();
                //--F5制御を追加（独自にアクションクラスを作成）
                this.mrsHomenGroupList.ShortcutKeyManager.Register(new DelegateAction(this.ShowCmnSearchHomenGroupList), Keys.F5);

                //--上キー制御を変更（前の行へ移動）
                this.mrsHomenGroupList.ShortcutKeyManager.Unregister(Keys.Up);
                this.mrsHomenGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousRow, Keys.Up);

                //--下キー制御を変更（次の行へ移動）
                this.mrsHomenGroupList.ShortcutKeyManager.Unregister(Keys.Down);
                this.mrsHomenGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToNextRow, Keys.Down);

                //--左キー制御を変更（前のセルへ移動 ※行の折り返しあり）
                this.mrsHomenGroupList.ShortcutKeyManager.Unregister(Keys.Left);
                this.mrsHomenGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToPreviousCell, Keys.Left);

                //--右キー制御を変更（次のセルへ移動 ※行の折り返しあり）
                this.mrsHomenGroupList.ShortcutKeyManager.Unregister(Keys.Right);
                this.mrsHomenGroupList.ShortcutKeyManager.Register(SelectionActions.MoveToNextCell, Keys.Right);

                //--単一セル選択モード
                this.mrsHomenGroupList.MultiSelect = false;

                //--ヘッダの列幅変更を禁止する
                //---列ヘッダと行ヘッダ個別で列幅変更の可否は調整できません。 T.Kuroki@NSK
                this.mrsHomenGroupList.AllowUserToResize = false;

                //行をクリア
                //---MultiRow.Clear() メソッドは未対応
                this.mrsHomenGroupList.Rows.Clear();

                //--ユーザでの行追加を許可する
                this.mrsHomenGroupList.AllowUserToAddRows = this.IsNotMaxRows();
                //--ユーザでの行削除は許可しない
                this.mrsHomenGroupList.AllowUserToDeleteRows = false;

                //--フォーカスが無い時はセルのハイライトを表示しない
                this.mrsHomenGroupList.HideSelection = true;

                //--分割ボタンを表示しない
                this.mrsHomenGroupList.SplitMode = SplitMode.None;
            }
            finally
            {
                this.mrsHomenGroupList.ResumeLayout();
            }
        }

        /// <summary>
        /// 明細のコード値検索画面を表示します。
        /// </summary>
        /// <param name="curMRow"></param>
        private void ShowCmnSearchHomenGroupList(GcMultiRow curMRow)
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

            if (curCell.Name == MrowCellKeys.HomenCode.ToString())
            {
                using (CmnSearchHomenFrame f = new CmnSearchHomenFrame())
                {
                    //複数選択可
                    f.AllowExtendedSelectRow = true;
                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        // 画面から値を取得
                        List<HomenInfo> list = f.SelectedInfoList;

                        //戻り値が存在する場合
                        if (list != null && 0 < list.Count)
                        {
                            //描画を止める
                            curMRow.SuspendLayout();

                            //選択行インデックス取得
                            int gyo = curMRow.CurrentCellPosition.RowIndex;

                            //作業行取得
                            int wk_gyo = gyo;

                            //カレント方面コード
                            string curHomenCode = Convert.ToInt32(curCell.Value).ToString();

                            //初行フラグ
                            bool isFirst = true;

                            //待機状態へ
                            this.Cursor = Cursors.WaitCursor;

                            try
                            {
                                //戻り値の件数分、処理を繰り返す
                                foreach (HomenInfo info in list)
                                {
                                    //存在フラグ
                                    bool exists = false;

                                    for (int i = 0; i < curMRow.RowCount; i++)
                                    {
                                        //カレント方面コード（自分）以外でリストにすでに存在する場合
                                        if (!curHomenCode.Equals(StringHelper.ConvertToString(info.HomenCode))
                                            &&
                                            StringHelper.ConvertToString(curMRow.GetFormattedValue(i, (int)MrowCurumnindices.HomenCode))
                                            .Equals(StringHelper.ConvertToString(info.HomenCode)))
                                        {
                                            //存在フラグを「true：存在する」を設定
                                            exists = true;
                                            break;
                                        }
                                    }

                                    //方面コードが方面グループリストに存在しない場合
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
                                            wk_Row[MrowCellKeys.HomenCode.ToString()].Value = info.HomenCode;
                                            wk_Row[MrowCellKeys.HomenName.ToString()].Value = info.HomenName;
                                            HomenGroupRowValues rowValues = this.GetHomenGroupListRowValues(wk_Row.Index);
                                            rowValues.HomenId = info.HomenId;

                                            //初行保持
                                            firstRow = wk_Row;
                                            firstCode = info.HomenCode;

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
                                                wk_Row[MrowCellKeys.HomenCode.ToString()].Value = info.HomenCode;
                                                wk_Row[MrowCellKeys.HomenName.ToString()].Value = info.HomenName;
                                                HomenGroupRowValues rowValues = this.GetHomenGroupListRowValues(wk_Row.Index);
                                                rowValues.HomenId = info.HomenId;

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
                            curMRow.CurrentCellPosition = new GrapeCity.Win.MultiRow.CellPosition(firstRow.Index, MrowCurumnindices.HomenCode.ToString());
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

            var rowValues = this.GetHomenGroupListRowValues(curRowIndex);

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
            //方面グループスタイル情報初期化
            this.InitHomenGroupStyleInfo();
        }

        /// <summary>
        /// 方面グループのスタイル情報を格納するメンバ変数の初期化をします。
        /// </summary>
        private void InitHomenGroupStyleInfo()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpHomenGroupListGrid.Sheets[0];

            //デザイナから各列のStyleInfoを取得してメンバに保持
            this.initialHomenGroupStyleInfoArr = new StyleInfo[COL_MAXCOLNUM_TOKUISAKIGROUPLIST];

            for (int i = 0; i < COL_MAXCOLNUM_TOKUISAKIGROUPLIST; i++)
            {
                this.initialHomenGroupStyleInfoArr[i] =
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
                ControlValidatingEventRaiser.Create(this.numFukushaHomenGroupCode, ctl => ctl.Text, this.numFukushaHomenGroupCode_Validating));
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            this.searchStateBinder.AddSearchableControls(this.numFukushaHomenGroupCode);
            this.searchStateBinder.AddSearchableControls(
                this.mrsHomenGroupList,
                MrowCellKeys.HomenCode.ToString()
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
        /// 方面グループコードの値を取得します。
        /// </summary>
        private int FukushaHomenGroupCode
        {
            get { return Convert.ToInt32(this.numFukushaHomenGroupCode.Value); }
        }

        #endregion

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHomenGroupFrame();
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
                    this.numHomenGroupCode.Enabled = false;
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
                    this.numHomenGroupCode.Enabled = true;
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
                    this.numHomenGroupCode.Enabled = false;
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
        /// 方面グループリストに値を設定します。
        /// </summary>
        private void SetHomenGroupListSheet()
        {
            //待機状態へ
            this.Cursor = Cursors.WaitCursor;

            try
            {
                //検索結果一覧を初期化
                this.InitSheet();

                //検索結果一覧の取得を指示
                this._HomenGroupInfoList =
                    this._HomenGroup.GetList();

                IList<HomenGroupInfo> wk_list = this.GetMatchedList(this._HomenGroupInfoList);

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
                    .OrderBy(element => element.HomenGroupCode)
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
                    HomenGroupInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_TOKUISAKIGROUPLIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(this.initialHomenGroupStyleInfoArr[k]);

                        //[使用しない]データの場合は、文字色を赤に変更します
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    datamodel.SetValue(j, COL_TOKUISAKIGROUP_CODE, wk_info.HomenGroupCode);
                    datamodel.SetValue(j, COL_TOKUISAKIGROUP_NAME, wk_info.HomenGroupName);

                    datamodel.SetTag(j, COL_TOKUISAKIGROUP_CODE, wk_list[j]);
                }

                //Spreadにデータモデルをセット
                this.fpHomenGroupListGrid.Sheets[0].Models.Data = datamodel;

                //Spreadにスタイルモデルをセット
                this.fpHomenGroupListGrid.Sheets[0].Models.Style = stylemodel;

                //選択行にフォーカスを合わせて選択状態にする
                this.fpHomenGroupListGrid.Sheets[0].SetActiveCell(selectrowidx_Result, 0, true);

                //選択行にスクロールバーを移動させる
                this.fpHomenGroupListGrid.ShowActiveCell(
                    FarPoint.Win.Spread.VerticalPosition.Top,
                    FarPoint.Win.Spread.HorizontalPosition.Left);

                //行がない場合は選択範囲を追加しない
                int dataCount = wk_list.Count;
                if (dataCount > 0)
                {
                    this.fpHomenGroupListGrid.Sheets[0].AddSelection(selectrowidx_Result, -1, 1,
                        this.fpHomenGroupListGrid.Sheets[0].ColumnCount - 1);

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
        private List<HomenGroupInfo> GetMatchedList(IList<HomenGroupInfo> list)
        {
            List<HomenGroupInfo> rt_list = new List<HomenGroupInfo>();

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

            foreach (HomenGroupInfo item in list)
            {
                //「コード」+「名称」であいまい検索
                string item_mei =
                    Convert.ToInt32(item.HomenGroupCode) + Environment.NewLine
                    + item.HomenGroupName.Trim() + Environment.NewLine;

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
        /// 方面グループの新規登録を開始します
        /// </summary>
        private void DoStartNewData()
        {
            //入力項目のクリア
            this.ClearInputs();

            //現在編集中の方面グループ情報を保持する領域の初期化
            this._HomenGroupInfo = new HomenGroupInfo();

            //新規モードへ変更
            this.ChangeMode(FrameEditMode.New);

            //フォーカス移動
            this.numHomenGroupCode.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }

        /// <summary>
        /// 画面からの情報を取得します。
        /// </summary>
        /// <param name="changeMode">モード変更可否（true：する、false：しない）</param>
        private void DoGetData(bool changeMode = true)
        {
            //方面グループコードをリストから取得して設定
            this._HomenGroupId = this.GetHomenGroupIdByHomenGroupListOnSelection();

            if (this._HomenGroupId == 0)
            {
                this._HomenGroupInfo = new HomenGroupInfo();

                this.ChangeMode(FrameEditMode.New);
                this.numHomenGroupCode.Focus();
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
            }
            else
            {
                try
                {
                    //方面グループ取得
                    this._HomenGroupInfo =
                        this._HomenGroup.GetInfoById(this._HomenGroupId);

                    //方面グループ明細取得
                    this._HomenGroupInfo.HomenGroupMeisaiList =
                        this._HomenGroupMeisai.GetList(
                        new HomenGroupMeisaiSearchParameter()
                        {
                            HomenGroupId = this._HomenGroupId
                        });

                    //画面表示
                    this.SetScreen();

                    if (changeMode)
                    {
                        //編集モード
                        this.ChangeMode(FrameEditMode.Editable);

                        //フォーカス設定
                        this.edtHomenGroupName.Focus();
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
        /// 方面グループリストにて選択中の方面グループの方面グループIDを取得します。
        /// 未選択の場合は0を返却します。
        /// </summary>
        /// <returns>選択中の方面グループID</returns>
        private decimal GetHomenGroupIdByHomenGroupListOnSelection()
        {
            //返却用
            decimal rt_val = 0;

            SheetView sheet0 = this.fpHomenGroupListGrid.Sheets[0];

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
                    //最初の列にセットしたTagからHomenGroupInfoを取り出して、HomenGroupIdを取得
                    rt_val = ((HomenGroupInfo)sheet0.Cells[select_row, COL_TOKUISAKIGROUP_CODE].Tag).HomenGroupId;
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
            if (this._HomenGroupInfo == null)
            {
                this.ClearInputs();
            }
            else
            {
                if (initflag)
                {
                    this.numHomenGroupCode.Value = Convert.ToDecimal(_HomenGroupInfo.HomenGroupCode);
                    this.edtHomenGroupName.Text = Convert.ToString(_HomenGroupInfo.HomenGroupName);
                    this.numFukushaHomenGroupCode.Value = 0;
                    this.edtFukushaHomenGroupName.Text = string.Empty;

                    this.chkDisableFlag.Checked = _HomenGroupInfo.DisableFlag;
                }

                this.SetScreenHomenGroupMeisai();
            }
        }

        /// <summary>
        /// 明細を画面に設定します。
        /// </summary>
        private void SetScreenHomenGroupMeisai()
        {
            //一行目にセットフォーカスするため、いったん許可しない
            //--ユーザでの行追加は許可しない
            this.mrsHomenGroupList.AllowUserToAddRows = false;

            //MultiRowの描画を停止
            this.mrsHomenGroupList.SuspendLayout();

            try
            {
                //明細をクリア
                this.mrsHomenGroupList.Rows.Clear();

                var lynaHomenGroupMeisaiList =
                    _HomenGroupInfo.HomenGroupMeisaiList
                    .GroupBy(element =>
                        new
                        {
                            element.Gyo,
                            element.HomenId,
                            element.HomenCode,
                            element.HomenName,
                        })
                    .OrderBy(element => element.Key.Gyo)
                    .ToList();


                //件数取得
                int rowcount = lynaHomenGroupMeisaiList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //明細の行数を設定（新規行なし）
                this.mrsHomenGroupList.RowCount = rowcount;

                for (int i = 0; i < rowcount; i++)
                {
                    var meisaiList = lynaHomenGroupMeisaiList[i];

                    //Mrow取得
                    GrapeCity.Win.MultiRow.Row row = this.mrsHomenGroupList.Rows[i];

                    row.SetValue(MrowCellKeys.HomenCode.ToString(), lynaHomenGroupMeisaiList[i].Key.HomenCode);
                    row.SetValue(MrowCellKeys.HomenName.ToString(), lynaHomenGroupMeisaiList[i].Key.HomenName);

                    var rowValues = this.GetHomenGroupListRowValues(i);
                    rowValues.HomenId = lynaHomenGroupMeisaiList[i].Key.HomenId;
                }

                //表示明細が存在する場合
                if (0 < rowcount)
                {
                    //1行目の1セルを選択
                    this.mrsHomenGroupList.CurrentCellPosition = new CellPosition(0, MrowCurumnindices.HomenCode.ToString());
                }
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsHomenGroupList.ResumeLayout();

                //--ユーザでの行追加を許可
                this.mrsHomenGroupList.AllowUserToAddRows = true;
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
                        this._HomenGroup.Save(tx, this._HomenGroupInfo);
                    });

                    //操作ログ(保存)の条件取得
                    string log_jyoken =
                        FrameUtilites.GetDefineLogMessage(
                        "C10002", new string[] { "方面グループ", _HomenGroupInfo.HomenGroupCode.ToString() });

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
                        this.numHomenGroupCode.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                    else
                    {
                        this.edtHomenGroupName.Focus();
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
                        this.numHomenGroupCode.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                    else
                    {
                        this.edtHomenGroupName.Focus();
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
            _HomenGroupInfo.HomenGroupId = this._HomenGroupId;
            _HomenGroupInfo.HomenGroupCode = Convert.ToInt32(this.numHomenGroupCode.Value);
            _HomenGroupInfo.HomenGroupName = this.edtHomenGroupName.Text.Trim();

            //方面グループ明細取得
            _HomenGroupInfo.HomenGroupMeisaiList = this.GetScreenHomenGroupMeisai();

            _HomenGroupInfo.DisableFlag = this.chkDisableFlag.Checked;
        }

        /// <summary>
        /// 画面の明細値をコントローラーにセットします。
        /// </summary>
        /// <returns>方面グループ明細リスト</returns>
        private IList<HomenGroupMeisaiInfo> GetScreenHomenGroupMeisai()
        {
            IList<HomenGroupMeisaiInfo> rt_list = new List<HomenGroupMeisaiInfo>();

            int gyo = 0;
            foreach (GrapeCity.Win.MultiRow.Row row in this.mrsHomenGroupList.Rows)
            {
                if (!row.IsNewRow)
                {
                    //画面明細取得
                    rt_list.Add(new HomenGroupMeisaiInfo()
                    {
                        HomenGroupId = this._HomenGroupId,
                        Gyo = gyo++,
                        HomenId = this.GetHomenGroupListRowValues(row.Index).HomenId,
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
            if (rt_val && Convert.ToDecimal(this.numHomenGroupCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "コード" });
                ctl = this.numHomenGroupCode;
            }

            //名称の必須入力チェック
            if (rt_val && (this.edtHomenGroupName.Text.Trim().Length == 0))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "名称" });
                ctl = this.edtHomenGroupName;
            }

            //方面の入力チェック
            if (rt_val && (0 < this.mrsHomenGroupList.RowCount))
            {
                List<HomenGroupMeisaiInfo> list = new List<HomenGroupMeisaiInfo>();
                foreach (GrapeCity.Win.MultiRow.Row row in this.mrsHomenGroupList.Rows)
                {
                    if (!row.IsNewRow)
                    {
                        if (row[MrowCellKeys.HomenCode.ToString()].Value != null)
                        {
                            String code = row[MrowCellKeys.HomenCode.ToString()].Value.ToString();
                            list.Add(new HomenGroupMeisaiInfo()
                            {
                                Gyo = row.Index,
                                HomenCode = Convert.ToInt32(code),
                                HomenId = this.GetHomenGroupListRowValues(row.Index).HomenId,
                            });
                        }
                        else
                        {
                            list.Add(new HomenGroupMeisaiInfo()
                            {
                                Gyo = row.Index,
                                HomenCode = 0,
                                HomenId = 0,
                            });
                        }
                    }
                }

                //方面コードの重複チェック
                var check_list =
                  list
                  .GroupBy(x => x.HomenId)
                  .Where(g => g.Count() > 1)
                  .Select(g => g.FirstOrDefault()).ToList();

                if (0 < check_list.Count)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2203010", new string[] { "方面コード" });
                    cpt = new CellPosition(check_list[0].Gyo, MrowCurumnindices.HomenCode.ToString());
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
                    this.mrsHomenGroupList.CurrentCellPosition = cpt;
                    this.mrsHomenGroupList.Focus();
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
                                this._HomenGroup.Delete(tx, _HomenGroupInfo);
                            });

                            //操作ログ(削除)の条件取得
                            string log_jyoken =
                                FrameUtilites.GetDefineLogMessage(
                                "C10002", new string[] { "方面グループ", _HomenGroupInfo.HomenGroupCode.ToString() });

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
        /// 方面グループ明細一覧に行挿入します。
        /// </summary>
        private void AddMeisaiGyo()
        {
            if (this.mrsHomenGroupList.RowCount < COL_MAXROW_TOKUISAKILIST)
            {
                // 明細挿入
                this.AddMeisaiListRow(this.mrsHomenGroupList);
                this.mrsHomenGroupList.Focus();
                this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);

                //--ユーザでの行追加を制御する
                this.mrsHomenGroupList.AllowUserToAddRows = this.IsNotMaxRows();
            }
        }

        /// <summary>
        /// 方面グループ明細一覧から行削除します。
        /// </summary>
        private void RemoveMeisaiGyo()
        {
            // 明細削除
            this.RemoveMeisaiListRow(this.mrsHomenGroupList);
            this.mrsHomenGroupList.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);

            //--ユーザでの行追加を制御する
            this.mrsHomenGroupList.AllowUserToAddRows = this.IsNotMaxRows();
        }

        /// <summary>
        /// 方面グループ明細一覧が最大行数か判定します。
        /// （true：最大行数未満、false：最大行数）
        /// </summary>
        private bool IsNotMaxRows()
        {
            return !(COL_MAXROW_TOKUISAKILIST < this.mrsHomenGroupList.RowCount);
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

            //方面グループリストをセット
            this.SetHomenGroupListSheet();

            //モード変更
            this.ChangeMode(FrameEditMode.Default);

            this.fpHomenGroupListGrid.Focus();
            this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
        }
        
        /// <summary>
        /// 既存コードを指定したコードに変更します。
        /// </summary>
        private void DoChangeCode()
        {
            ////コードを取得
            //int oldcode = Convert.ToInt32(this.numHomenGroupCode.Value);

            ////コードの桁数を取得
            //int codedigitcount = Convert.ToInt32(this.numHomenGroupCode.MaxValue).ToString().Length;

            ////コード変更画面を表示
            //using (ChangeMasterCodeFrame f =
            //    new ChangeMasterCodeFrame(DefaultProperty.MasterCodeKbn.HomenGroup, oldcode, codedigitcount))
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
            if (this.ActiveControl == this.numFukushaHomenGroupCode)
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
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.HomenGroupCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 複写元方面グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateFukushaHomenGroupCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (Convert.ToInt32(this.numFukushaHomenGroupCode.Value) == 0)
                {
                    this.numFukushaHomenGroupCode.Value = null;
                    this.edtFukushaHomenGroupName.Text = string.Empty;
                    is_clear = true;
                    return;
                }

                HomenGroupInfo info =
                    this._DalUtil.HomenGroup.GetInfo(this.FukushaHomenGroupCode);

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
                        //方面グループ明細取得
                        this._HomenGroupInfo.HomenGroupMeisaiList =
                            this._HomenGroupMeisai.GetList(
                            new HomenGroupMeisaiSearchParameter()
                            {
                                HomenGroupId = info.HomenGroupId
                            });

                        //方面グループ明細表示
                        this.SetScreen(false);

                        this.numFukushaHomenGroupCode.Value = info.HomenGroupCode;
                        this.edtFukushaHomenGroupName.Text = info.HomenGroupName;

                        this.mrsHomenGroupList.Focus();
                        this.validatingEventRaiserCollection.PerformWhenEnter(this.ActiveControl);
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numFukushaHomenGroupCode.Value = null;
                    this.edtFukushaHomenGroupName.Text = string.Empty;
                }
            }
        }

        #endregion

        #region CellValidatingの処理

        /// <summary>
        /// 明細一覧のMrowのCellValidatingイベントを処理します。
        /// </summary>
        /// <param name="e">CellValidatingイベントのイベントデータ</param>
        private void ProcessHomenGroupMrowCellValidating(CellValidatingEventArgs e)
        {
            //編集された場合のみ処理する
            if (!this.mrsHomenGroupList.IsCurrentCellInEditMode)
            {
                return;
            }

            //描画を止める
            this.mrsHomenGroupList.SuspendLayout();

            try
            {
                //現在アクティブなセルのキーを取得
                string wk_curcellkey = this.mrsHomenGroupList.CurrentCell.Name;

                if (wk_curcellkey == MrowCellKeys.HomenCode.ToString())
                {
                    e.Cancel = this.HomenCodeCellValidating(e.RowIndex);
                }

                if (e.Cancel)
                {
                    //クリア
                    EditingActions.ClearEdit.Execute(this.mrsHomenGroupList);

                    //前の値に戻す前にいったん確定する
                    EditingActions.EndEdit.Execute(this.mrsHomenGroupList);

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsHomenGroupList);
                }
            }
            finally
            {
                //描画を再開
                this.mrsHomenGroupList.ResumeLayout();
            }
        }

        /// <summary>
        /// 明細の方面コード入力枠のCellValidating処理を行い
        /// イベントのキャンセルの有無を返します。
        /// (true:イベントをキャンセルする)
        /// </summary>
        /// <param name="rowIndex"></param>
        /// <returns>CellValidatingイベントキャンセルの可否(true:キャンセルする)</returns>
        private bool HomenCodeCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            var row = this.mrsHomenGroupList.Rows[rowIndex];

            var rowValues = this.GetHomenGroupListRowValues(rowIndex);

            //クリアするかどうか（true：クリア）
            bool is_clear = false;
            //セル値
            int cell_value =
                Convert.ToInt32(this.mrsHomenGroupList.CurrentCell.Value);
            //編集値
            int editing_value =
                Convert.ToInt32(
                    FrameUtilites.GetEditingControlValue(this.mrsHomenGroupList.EditingControl));

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
                    HomenInfo info =
                        _DalUtil.Homen.GetInfo(editing_value, null);

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
                            if (info.DisableFlag)
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
                        row.SetValue(MrowCellKeys.HomenName.ToString(), info.HomenName);
                        rowValues.HomenId = info.HomenId;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    //関連する情報クリア
                    row.SetValue(MrowCellKeys.HomenCode.ToString(), null);
                    row.SetValue(MrowCellKeys.HomenName.ToString(), string.Empty);
                    rowValues.HomenId = 0;
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
        private void ProcessFpHomenGroupListGridPreviewKeyDown(PreviewKeyDownEventArgs e)
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

        private void HomenGroupFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void HomenGroupFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void fpHomenGroupListGrid_CellDoubleClick(object sender, CellClickEventArgs e)
        {
            //方面グループリスト - CellDoubleClick
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
        private void fpHomenGroupListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            if (e.Button == MouseButtons.Left && e.ColumnHeader)
            {
                this.fpHomenGroupListGrid.Sheets[0].AutoSortColumn(e.Column);
            }
        }

        private void fpHomenGroupListGrid_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            //登録済リスト - PreviewKeyDown
            this.ProcessFpHomenGroupListGridPreviewKeyDown(e);
        }

        private void HomenGroupFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void edtSearch_TextChanged(object sender, EventArgs e)
        {
            //選択中の行番号を初期化
            selectrowidx_Result = 0;

            //リスト再描画
            this.SetHomenGroupListSheet();
        }

        private void numFukushaHomenGroupCode_Validating(object sender, CancelEventArgs e)
        {
            //複写元方面グループコード
            this.ValidateFukushaHomenGroupCode(e);
        }

        private void mrsHomenGroupList_CellValidating(object sender, CellValidatingEventArgs e)
        {
            this.ProcessHomenGroupMrowCellValidating(e);
        }

        private void mrsHomenGroupList_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
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
                        sideButton.Click -= new EventHandler(mrsHomenGroupListSideButton_Click);
                        sideButton.Click += new EventHandler(mrsHomenGroupListSideButton_Click);
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

        private void mrsHomenGroupList_RowsAdded(object sender, RowsAddedEventArgs e)
        {
            if (FrameUtilites.GetMaxRowNumTankList() < this.mrsHomenGroupList.RowCount)
            {
                //--ユーザでの行追加を制御する
                this.mrsHomenGroupList.AllowUserToAddRows = this.IsNotMaxRows();
            }
        }

        private void mrsHomenGroupListSideButton_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchHomenGroupList(this.mrsHomenGroupList);
        }

        private void fpHomenGroupListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.DoGetData(false);
        }
        private void sbtnFukushaHomenGroupCode_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchHomenGroup();
        }

        private void btnAddMeisaiGyo_Click(object sender, EventArgs e)
        {
            this.AddMeisaiGyo();
        }

        private void btnRemoveMeisaiGyo_Click(object sender, EventArgs e)
        {
            this.RemoveMeisaiGyo();
        }
    }
}
