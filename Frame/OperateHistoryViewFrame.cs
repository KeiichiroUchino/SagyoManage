using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using jp.co.jpsys.util;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.Model;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.Model.DALExceptions;
using Jpsys.SagyoManage.Frame;
using Jpsys.SagyoManage.Frame.Command;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;
using Jpsys.SagyoManage.SQLServerDAL;

namespace Jpsys.SagyoManage.Frame
{
    public partial class OperateHistoryViewFrame : Form, IFrameBase
    {
        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "操作履歴照会";

        #region 操作履歴一覧(Spread)
        
        /// <summary>
        /// 操作履歴一覧 - 処理日時
        /// </summary>
        private const int COL_TRANSACTTIME_OPERATEHISTORY_LIST = 0;

        /// <summary>
        /// 操作履歴一覧 - 処理ID
        /// </summary>
        private const int COL_PROCESSID_OPERATEHISTORY_LIST = 1;

        /// <summary>
        /// 操作履歴一覧 - オペレーター名
        /// </summary>
        private const int COL_OPERATORNAME_OPERATEHISTORY_LIST = 2;

        /// <summary>
        /// 操作履歴一覧 - 区分
        /// </summary>
        private const int COL_OPERATEKBN_OPERATEHISTORY_LIST = 3;

        /// <summary>
        /// 操作履歴一覧 - 最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_OPERATEHISTORY_LIST = 4;

        #endregion

        /// <summary>
        /// 認証済みアプリケーション情報の保持領域
        /// </summary>
        private AppAuthInfo appAuth;

        /// <summary>
        /// メニューに表示している操作メニューを保持する領域
        /// </summary>
        private AddActionMenuItem actionMenuItems;

        /// <summary>
        /// 現在の操作履歴照会一覧の行のインデックスを保持します。
        /// (明細の行移動を拾う為)
        /// </summary>
        private int currentRowIndexOnThroughChargeListSheet;

        /// <summary>
        /// 各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラス領域
        /// </summary>
        private CommonDALUtil _DalUtil;

        /// <summary>
        /// 操作履歴情報の一覧
        /// </summary>
        private IList<OperateHistoryInfo> _OperateHistoryInfoList
            = new List<OperateHistoryInfo>();

        #endregion

        #region コマンド

        private CommandSet _commandSet;

        #endregion
        
        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitOperateHistoryViewFrame()
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

            //認証アプリケーション情報を設定
            this.appAuth = new AppAuthInfo();
            this.appAuth.OperatorId = UserProperty.GetInstance().LoginOperetorId;
            this.appAuth.TerminalId = UserProperty.GetInstance().LoginTerminalId;
            this.appAuth.UserProcessId = this.Name;
            this.appAuth.UserProcessName = this.Text;

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //入力項目の初期化
            this.ClearInputs();

            //Spread関連の初期化
            this.InitSheet();
        }

        /// <summary>
        /// 画面配色を初期化します。
        /// </summary>
        private void InitFrameColor()
        {
            //表のヘッダー背景色、表の選択行背景色の設定
            FrameUtilites.SetFrameGridBackColor(this);
        }

        /// <summary>
        /// Spread関連の初期化をします。
        /// </summary>
        private void InitSheet()
        {
            //操作履歴リストの初期化
            this.InitOperateHistoryListSheet();
        }

        /// <summary>
        /// 操作履歴リストを初期化します。
        /// </summary>
        private void InitOperateHistoryListSheet()
        {
            //SheetView変数定義
            SheetView sheet0 = this.fpOperateHistoryListGrid.Sheets[0];

            //行数の初期化
            DefaultSheetDataModel dataModel =
                new DefaultSheetDataModel(0, COL_MAXCOLNUM_OPERATEHISTORY_LIST);
            sheet0.Models.Data = dataModel;
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //検索セクションクリア
            this.ClearInputsSearch();
            //結果表示セクションクリア
            this.ClearInputsResult();
        }

        /// <summary>
        /// 結果表示セクションの入力項目をクリアします。
        /// </summary>
        private void ClearInputsResult()
        {
            DateTime wk_now = DateTime.Now;
            //現在時間をセット
            this.dteSearchFromDate.Value = wk_now.Date;
            this.dteSearchToDate.Value = wk_now.Date;

            edtSearchString1.Text = string.Empty;
            edtSearchString2.Text = string.Empty;
            edtSearchString3.Text = string.Empty;
            edtSearchString4.Text = string.Empty;
            edtSearchString5.Text = string.Empty;
        }

        /// <summary>
        /// 検索セクションの入力項目をクリアします。
        /// </summary>
        private void ClearInputsSearch()
        {
            this.edtResultRemark.Text = string.Empty;
        }

        /// <summary>
        /// メニュー関連を初期化します。
        /// </summary>
        private void InitMenuItem()
        {
            // 操作ニューの初期化
            this.InitActionMenuItems();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuItems()
        {
            // 操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();

            // メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.mnuStripTop);
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close));

        }

        void Close_Execute(object sender, EventArgs e)
        {
            this.DoClose();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 画面の情報からデータを取得します。
        /// </summary>
        private void DoGetData()
        {
            if (this.ValidateChildren() && this.CheckInputsSearch())
            {
                //Spreadシートにセット
                this.SetOperateHistoryListSheet();

                //明細が無い場合メッセージを表示して抜ける。
                if (this._OperateHistoryInfoList.Count == 0)
                {
                    MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201015"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                    //フォーカスを検索項目に
                    this.dteSearchFromDate.Focus();

                    return;
                }
            }
        }

        /// <summary>
        /// 操作履歴リストに値を設定します。
        /// </summary>
        private void SetOperateHistoryListSheet()
        {
            //操作履歴一覧の保持をクリア
            this._OperateHistoryInfoList = 
                new List<OperateHistoryInfo>(); 

            //検索条件を画面から取得
            DateTime fromDate = Convert.ToDateTime(this.dteSearchFromDate.Value).Date;
            DateTime toDate = Convert.ToDateTime(this.dteSearchToDate.Value).Date;

            //操作履歴一覧を取得
            this._OperateHistoryInfoList = 
                this._DalUtil.OperateHistory.GetOperateHistoryInfoList(
                fromDate, toDate, this.edtSearchString1.Text, edtSearchString2.Text, edtSearchString3.Text,
                    edtSearchString4.Text, edtSearchString5.Text);

            //件数取得
            int rowCount = this._OperateHistoryInfoList.Count;
            if (rowCount == 0)
            {
                //0件の場合はそのまま抜ける
                return;
            }

            //操作履歴一覧を格納
            IList<OperateHistoryInfo> wk_list = this._OperateHistoryInfoList;

            //Spreadのデータモデルを作る
            DefaultSheetDataModel datamodel =
                new DefaultSheetDataModel(wk_list.Count, COL_MAXCOLNUM_OPERATEHISTORY_LIST);

            //ループしてモデルにセット
            for (int i = 0; i < wk_list.Count; i++)
            {
                OperateHistoryInfo wk_info = wk_list[i];

                datamodel.SetValue(i, COL_TRANSACTTIME_OPERATEHISTORY_LIST, wk_info.TransactTime);
                datamodel.SetValue(i, COL_PROCESSID_OPERATEHISTORY_LIST, wk_info.ProcessId);
                datamodel.SetValue(i, COL_OPERATORNAME_OPERATEHISTORY_LIST, wk_info.OperatorName);
                datamodel.SetValue(i, COL_OPERATEKBN_OPERATEHISTORY_LIST, wk_info.OperateKbn);
                datamodel.SetTag(i, COL_TRANSACTTIME_OPERATEHISTORY_LIST, wk_info);
            }

            //Spreadにセット
            this.fpOperateHistoryListGrid.Sheets[0].Models.Data = datamodel;

            //検索結果の1行目をフォーカスを合わせて選択状態にする
            this.fpOperateHistoryListGrid.Focus();
            this.fpOperateHistoryListGrid.Sheets[0].SetActiveCell(0, 0, true);
            this.fpOperateHistoryListGrid.Sheets[0].AddSelection(
                0, -1, 1, this.fpOperateHistoryListGrid.Sheets[0].ColumnCount - 1);

            //現在行のタグの摘要を画面にセット
            this.SetRemark();
            //操作履歴リストの現在行をリセット
            this.currentRowIndexOnThroughChargeListSheet = 0;
        }

        /// <summary>
        /// 検索の入力項目をチェックします。
        /// (true:正常に入力されている。)
        /// </summary>
        /// <returns>正常に入力されているかどうかの値</returns>
        private bool CheckInputsSearch()
        {
            bool rt_val = true;
            string msg = string.Empty;
            Control ctl = null;

            //開始処理日の必須チェック
            if (rt_val && this.dteSearchFromDate.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "開始処理日" });
                ctl = this.dteSearchFromDate;
            }

            //終了処理日の必須チェック
            if (rt_val && this.dteSearchToDate.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203004", new string[] { "終了処理日" });
                ctl = this.dteSearchToDate;
            }

            //開始 - 終了の範囲チェック
            if (rt_val &&
                    this.dteSearchFromDate.Value >
                        this.dteSearchToDate.Value)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2202018", new string[] { "処理日" });
                ctl = this.dteSearchToDate;
            }

            if (!rt_val)
            {
                MessageBox.Show(msg, "エラー", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 操作履歴一覧のSelectionChangedイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessFpOperateHistoryListGridSelectionChanged(SelectionChangedEventArgs e)
        {
            SheetView sheet0 = this.fpOperateHistoryListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //現在行のIndexを取得しておく
                int wk_rowidx = this.fpOperateHistoryListGrid.Sheets[0].GetSelection(0).Row;

                //メンバの現在値と比較
                if (wk_rowidx != this.currentRowIndexOnThroughChargeListSheet)
                {
                    //Indexが変わったら取込時の情報を画面にセット
                    this.SetRemark();
                }

                //メンバの現在値を更新
                this.currentRowIndexOnThroughChargeListSheet = wk_rowidx;
            }
        }

        /// <summary>
        /// スルー運賃リスト上のアクティブな行の
        /// 取込時情報を画面にセットします。
        /// </summary>
        private void SetRemark()
        {
            //現在行のIndexを取得
            SheetView sheet0 = this.fpOperateHistoryListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                int wk_rowidx = sheet0.GetSelection(0).Row;

                //現在行のタグから操作履歴情報を取得
                OperateHistoryInfo info =
                    (OperateHistoryInfo)sheet0.Cells[wk_rowidx, COL_TRANSACTTIME_OPERATEHISTORY_LIST].Tag;

                //摘要を画面にセット
                this.edtResultRemark.Text = info.Remark;
            }
        }

        /// <summary>
        /// 列番号を指定してその列を基準に行を並び替えます。
        /// </summary>
        /// <param name="col">列番号</param>
        private void SortOperateHistoryListGrid(int col)
        {
            //列番号で自動整列
            fpOperateHistoryListGrid.Sheets[0].AutoSortColumn(col);

            //選択行の摘要を画面にセット
            this.SetRemark();
        }

        /// <summary>
        /// 画面を閉じます。
        /// </summary>
        private void DoClose()
        {
            this.Close();
        }

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public OperateHistoryViewFrame()
        {
            InitializeComponent();
        }

        #endregion

        #region IFrameBase メンバー

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitOperateHistoryViewFrame();
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

        private void OperateHistoryViewFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        /// <summary>
        /// 検索ボタンをクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSearch_Click(object sender, EventArgs e)
        {
            this.DoGetData();
        }

        /// <summary>
        /// 操作履歴一覧上のセルをクリックしたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpOperateHistoryListGrid_CellClick(object sender, CellClickEventArgs e)
        {
            //操作履歴一覧上でセルクリック
            if (e.ColumnHeader && e.Button == MouseButtons.Left)
            {
                //カラムヘッダ上の場合はその列でソート
                this.SortOperateHistoryListGrid(e.Column);
            }
        }

        /// <summary>
        /// 操作履歴情報一覧の選択行が変わったときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void fpOperateHistoryListGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            this.ProcessFpOperateHistoryListGridSelectionChanged(e);
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
