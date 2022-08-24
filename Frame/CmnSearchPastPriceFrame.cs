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
using System.Runtime.InteropServices;

namespace Jpsys.HaishaManageV10.Frame
{
    public partial class CmnSearchPastPriceFrame : Form, IFrameBase
    {
        #region ユーザ定義

        /// <summary>
        /// 画面タイトルの静的な部分
        /// </summary>
        private const string WINDOW_TITLE = "過去受注履歴検索";

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
        private PastPrice searchTblClass;

        /// <summary>
        /// 過去受注履歴情報のリストを保持する領域
        /// </summary>
        private IList<PastPriceInfo> PastPriceList = null;

        /// <summary>
        /// 起動パラメータを保持
        /// </summary>
        private PastPriceDefaultParameters _defpara = null;

        /// <summary>
        /// 削除データを除く全データを表示するかどうかの値を保持
        /// </summary>
        private bool _showAllFlag = false;

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
        /// 販路コード列番号
        /// </summary>
        private const int COL_HANRO_CODE = 0;
        /// <summary>
        /// 販路名列番号
        /// </summary>
        private const int COL_HANRO_NAME = 1;
        /// 得意先コード列番号
        /// </summary>
        private const int COL_TOKUISAKI_CODE = 2;
        /// <summary>
        /// 得意先名称列番号
        /// </summary>
        private const int COL_TOKUISAKI_NAME = 3;
        /// <summary>
        /// 積地コード列番号
        /// </summary>
        private const int COL_HATCHI_CODE = 4;
        /// <summary>
        /// 積地名称列番号
        /// </summary>
        private const int COL_HATCHI_NAME = 5;
        /// <summary>
        /// 着地コード列番号
        /// </summary>
        private const int COL_CHAKUCHI_CODE = 6;
        /// <summary>
        /// 着地名称列番号
        /// </summary>
        private const int COL_CHAKUCHI_NAME = 7;
        /// <summary>
        /// 品目コード列番号
        /// </summary>
        private const int COL_ITEM_CODE = 8;
        /// <summary>
        /// 品目名称列番号
        /// </summary>
        private const int COL_ITEM_NAME = 9;
        /// <summary>
        /// 積日名称列番号
        /// </summary>
        private const int COL_START_YMD = 10;
        /// <summary>
        /// 単位コード列番号
        /// </summary>
        private const int COL_FIG_CODE = 11;
        /// <summary>
        /// 単位名称列番号
        /// </summary>
        private const int COL_FIG_NAME = 12;
        /// <summary>
        /// 車種コード列番号
        /// </summary>
        private const int COL_CARKIND_CODE = 13;
        /// <summary>
        /// 車種名称列番号
        /// </summary>
        private const int COL_CARKIND_NAME = 14;
        /// <summary>
        /// 傭車先コード列番号
        /// </summary>
        private const int COL_YOSHA_CODE = 15;
        /// <summary>
        /// 傭車先名称列番号
        /// </summary>
        private const int COL_YOSHA_NAME = 16;
        /// <summary>
        /// 単価列番号
        /// </summary>
        private const int COL_TANKA = 17;
        /// <summary>
        /// 数量列番号
        /// </summary>
        private const int COL_SURYO = 18;
        /// <summary>
        /// 金額列番号
        /// </summary>
        private const int COL_KINGAKU = 19;
        /// <summary>
        /// 通行料列番号
        /// </summary>
        private const int COL_TSUKORYO = 20;
        /// <summary>
        /// 付帯業務料列番号
        /// </summary>
        private const int COL_FUTAIGYOMURYO = 21;
        /// <summary>
        /// 傭車金額列番号
        /// </summary>
        private const int COL_YOSHA_KINGAKU = 22;
        /// <summary>
        /// 傭車通行料列番号
        /// </summary>
        private const int COL_YOSHA_TSUKORYO = 23;
        /// <summary>
        /// 検索一覧最大列数
        /// </summary>
        private const int COL_MAXCOLNUM_LIST = 24;

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
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public CmnSearchPastPriceFrame()
        {
            //ラジオボタンをEnterキーでタブ遷移した場合に点線が表示されない症状を回避
            SystemParametersInfo((uint)SystemParametersInfoActionFlag.SPI_SETKEYBOARDCUES, 0,
                true, (uint)(SystemParametersInfoFlag.SPIF_UPDATEINIFILE | SystemParametersInfoFlag.SPIF_SENDWININICHANGE));

            InitializeComponent();
        }

        #endregion

        #region 初期化処理

        private void InitCmnSearchPastPriceFrame()
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

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numSearchHanroCode, this.ShowCmnSearchHanro},
                {this.numSearchTokuisakiCode, this.ShowCmnSearchTokuisaki},
                {this.numSearchCarKindCode, this.ShowCmnSearchCarKind},
                {this.numSearchStartPointCode, this.ShowCmnSearchPoint},
                {this.numSearchEndPointCode, this.ShowCmnSearchPoint},
                {this.numSearchFigCode, this.ShowCmnSearchFig},
                {this.numSearchItemCode, this.ShowCmnSearchItem},
                {this.numSearchCarOfChartererCode, this.ShowCmnSearchChartererCar},
            };

            // 各種画面で頻繁に利用されるデータアクセスオブジェクトを
            // 利用するためのファサードクラスのインスタンス作成
            this.dalUtil = new CommonDALUtil(this.appAuth);

            //検索テーブルのクラスインスタンス作成
            this.searchTblClass = new PastPrice();

            //Spread関連の初期化
            this.InitSheet();

            //入力項目をクリア
            this.ClearInputs();

            //登録済み一覧を取得
            this.DoGetData(true);

            //リストのセット
            this.SetListSheet();

            if (this.fpListGrid.Sheets[0].RowCount == 0)
            {
                this.ActiveControl = this.numSearchHanroCode;
            }
            else
            {
                this.ActiveControl = this.fpListGrid;
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
                this.numSearchTokuisakiCode
                );

            this.searchStateBinder.AddStateObject(this.toolStripSearch);
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            // コード検索
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchHanroCode, ctl => ctl.Text, this.numSearchHanroCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchTokuisakiCode, ctl => ctl.Text, this.numSearchTokuisakiCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchCarKindCode, ctl => ctl.Text, this.numSearchCarKindCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchStartPointCode, ctl => ctl.Text, this.numSearchStartPointCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchEndPointCode, ctl => ctl.Text, this.numSearchEndPointCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchFigCode, ctl => ctl.Text, this.numSearchFigCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchItemCode, ctl => ctl.Text, this.numSearchItemCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numSearchCarOfChartererCode, ctl => ctl.Text, this.numSearchCarOfChartererCode_Validating));
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            //過去受注履歴検索画面初期パラメータ検索情報
            PastPriceDefaultSearchParametersInfo searchParamInfo = new PastPrice().GetSearchInfo(this.PastPriceDefaultParameters);

            //得意先
            this.numSearchTokuisakiCode.Tag = searchParamInfo.TokuisakiId;
            this.numSearchTokuisakiCode.Value = searchParamInfo.TokuisakiCode;
            this.edtSearchTokuisakiName.Text = searchParamInfo.TokuisakiName;
            //販路
            this.numSearchHanroCode.Tag = searchParamInfo.HanroId;
            this.numSearchHanroCode.Value = searchParamInfo.HanroCode;
            this.edtSearchHanroName.Text = searchParamInfo.HanroName;
            //積地
            this.numSearchStartPointCode.Tag = searchParamInfo.StartPointId;
            this.numSearchStartPointCode.Value = searchParamInfo.StartPointCode;
            this.edtSearchStartPointName.Text = searchParamInfo.StartPointName;
            //着地
            this.numSearchEndPointCode.Tag = searchParamInfo.EndPointId;
            this.numSearchEndPointCode.Value = searchParamInfo.EndPointCode;
            this.edtSearchEndPointName.Text = searchParamInfo.EndPointName;
            //品目
            this.numSearchItemCode.Tag = searchParamInfo.ItemId;
            this.numSearchItemCode.Value = searchParamInfo.ItemCode;
            this.edtSearchItemName.Text = searchParamInfo.ItemName;
            //単位
            this.numSearchFigCode.Tag = searchParamInfo.FigId;
            this.numSearchFigCode.Value = searchParamInfo.FigCode;
            this.edtSearchFigName.Text = searchParamInfo.FigName;
            //車種
            this.numSearchCarKindCode.Tag = searchParamInfo.CarKindId;
            this.numSearchCarKindCode.Value = searchParamInfo.CarKindCode;
            this.edtSearchCarKindName.Text = searchParamInfo.CarKindName;
            //傭車先
            this.numSearchCarOfChartererCode.Tag = searchParamInfo.TorihikiId;
            this.numSearchCarOfChartererCode.Value = searchParamInfo.TorihikiCode;
            this.edtSearchCarOfChartererName.Text = searchParamInfo.TorihikiName;
            //着日
            this.dteTaishoYMDFrom.Value = DateTime.Today.AddYears(-3); //初期値は過去3年
            this.dteTaishoYMDTo.Value = null;
            //管理情報取得
            KanriInfo kanriInfo = new Kanri().GetInfo();
            //検索先
            this.radHaishaAce.Checked = !kanriInfo.PastPriceSearchTargetFlag;
            this.radToraDON.Checked = kanriInfo.PastPriceSearchTargetFlag;
            //使用停止表示
            this.chkAllFlag.Checked = _showAllFlag;
            //表示件数
            this.edtHyojiKensu.Text = "0件";

            this.btnOk.Enabled = false;
            this.commandSet.Select.Enabled = false;

            // 検証を行うコントロールの前回値を更新
            this.validatingEventRaiserCollection.UpdateOldValue();
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// 検索ボタン押下処理。
        /// </summary>
        private void btnView_Click(object sender, EventArgs e)
        {
            //登録済み一覧を取得
            this.DoGetData(false);

            // 検索処理
            this.SetListSheet();
        }

        /// <summary>
        /// 発着地検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchPoint()
        {
            using (CmnSearchPointFrame f = new CmnSearchPointFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONPointCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 車種検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCarKind()
        {
            using (CmnSearchCarKindFrame f = new CmnSearchCarKindFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarKindCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 傭車検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchChartererCar()
        {
            using (CmnSearchTorihikisakiFrame f = new CmnSearchTorihikisakiFrame())
            {
                // パラメータをセット
                f.ShowAllFlag = false;

                f.InitFrame();
                f.ShowDialog(this);

                if (f.DialogResult == DialogResult.OK)
                {
                    // 画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt64(f.SelectedInfo.ToraDONTorihikiCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 単位検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchFig()
        {
            using (CmnSearchFigFrame f = new CmnSearchFigFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.FigCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 品目検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchItem()
        {
            using (CmnSearchItemFrame f = new CmnSearchItemFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONItemCode);

                    this.OnCmnSearchComplete();
                }
            }
        }

        /// <summary>
        /// 販路検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHanro()
        {
            if (this.radHaishaAce.Checked)
            {
                using (CmnSearchHanroFrame f = new CmnSearchHanroFrame())
                {
                    f.InitFrame();
                    f.ShowDialog(this);
                    if (f.DialogResult == DialogResult.OK)
                    {
                        //画面から値を取得
                        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                            Convert.ToInt32(f.SelectedInfo.HanroCode);

                        this.OnCmnSearchComplete();
                    }
                }
            }
        }

        /// <summary>
        /// 過去受注履歴一覧を取得します。
        /// </summary>
        private void DoGetData(bool initFlg)
        {
            //マウスカーソルを待機中(砂時計)に変更
            Cursor.Current = Cursors.WaitCursor;

            try
            {
                if (CheckInputs(initFlg)) 
                {
                    //過去受注履歴一覧を取得

                    if (this.radHaishaAce.Checked)
                    {
                        this.PastPriceList =
                                this.searchTblClass.GetList(this.GetScreen());
                    }
                    else
                    {
                        this.PastPriceList =
                                this.searchTblClass.GetListToraDon(this.GetScreen());
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
        /// 過去受注履歴画面の検索条件を取得します。
        /// </summary>
        private PastPriceSearchParametersInfo GetScreen()
        {

            // 検索条件設定
            var para = new PastPriceSearchParametersInfo();

            // 受注情報検索条件
            para.HanroId = Convert.ToDecimal(this.numSearchHanroCode.Tag);
            para.TokuisakiId = Convert.ToDecimal(this.numSearchTokuisakiCode.Tag);
            para.CarKindId = Convert.ToDecimal(this.numSearchCarKindCode.Tag);
            para.StartPointId = Convert.ToDecimal(this.numSearchStartPointCode.Tag);
            para.EndPointId = Convert.ToDecimal(this.numSearchEndPointCode.Tag);
            para.ItemId = Convert.ToDecimal(this.numSearchItemCode.Tag);
            para.FigId = Convert.ToDecimal(this.numSearchFigCode.Tag);
            para.TorihikiId = Convert.ToDecimal(this.numSearchCarOfChartererCode.Tag);
            para.TaishoYMDFrom = Convert.ToDateTime(this.dteTaishoYMDFrom.Value);
            para.TaishoYMDTo = Convert.ToDateTime(this.dteTaishoYMDTo.Value);
            para.HaishaAceChecked = this.radHaishaAce.Checked;
            para.TraDonVersionKbn = UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn;

            return para;
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

                if (this.PastPriceList == null)
                {
                    return;
                }

                //使用停止表示フラグによってデータ再抽出
                IList<PastPriceInfo> wk_list =
                    this.chkAllFlag.Checked ? this.PastPriceList : this.PastPriceList.Where(x => !x.DisableFlag).ToList();

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

                //単価フォーマット
                string atpriceFormat = ProjectUtilites.GetDigitsFormat(
                    UserProperty.GetInstance().JuchuAtPriceIntDigits,
                    UserProperty.GetInstance().JuchuAtPriceDecimalDigits,
                    true);

                //数量フォーマット
                string numberFormat = ProjectUtilites.GetDigitsFormat(
                    UserProperty.GetInstance().JuchuNumberIntDigits,
                    UserProperty.GetInstance().JuchuNumberDecimalDigits,
                    true);

                //ループしてモデルにセット
                for (int j = 0; j < wk_list.Count; j++)
                {
                    PastPriceInfo wk_info = wk_list[j];

                    //スタイルモデルのセット
                    for (int k = 0; k < COL_MAXCOLNUM_LIST; k++)
                    {
                        StyleInfo sInfo = new StyleInfo(stylemodel.GetDirectInfo(j, -1, null));

                        //無効データは背景色を変更
                        if (wk_info.DisableFlag)
                        {
                            sInfo.BackColor = Color.Silver;
                        }

                        stylemodel.SetDirectInfo(j, k, sInfo);
                    }

                    //データモデルのセット
                    datamodel.SetValue(j, COL_HANRO_CODE, wk_info.HanroCode.ToString("########"));
                    datamodel.SetValue(j, COL_HANRO_NAME, wk_info.HanroName);
                    datamodel.SetValue(j, COL_TOKUISAKI_CODE, wk_info.TokuisakiCode.ToString("########"));
                    datamodel.SetValue(j, COL_TOKUISAKI_NAME, wk_info.TokuisakiShortName);
                    datamodel.SetValue(j, COL_HATCHI_CODE, wk_info.StartPointCode.ToString("#####"));
                    datamodel.SetValue(j, COL_HATCHI_NAME, wk_info.StartPointName);
                    datamodel.SetValue(j, COL_CHAKUCHI_CODE, wk_info.EndPointCode.ToString("#####"));
                    datamodel.SetValue(j, COL_CHAKUCHI_NAME, wk_info.EndPointName);
                    datamodel.SetValue(j, COL_ITEM_CODE, wk_info.ItemCode.ToString("######"));
                    datamodel.SetValue(j, COL_ITEM_NAME, wk_info.ItemName);
                    datamodel.SetValue(j, COL_START_YMD, wk_info.TaskStartDateTime);
                    datamodel.SetValue(j, COL_FIG_CODE, wk_info.FigCode.ToString("##"));
                    datamodel.SetValue(j, COL_FIG_NAME, wk_info.FigName);
                    datamodel.SetValue(j, COL_CARKIND_CODE, wk_info.CarKindCode.ToString("###"));
                    datamodel.SetValue(j, COL_CARKIND_NAME, wk_info.CarKindShortName);
                    datamodel.SetValue(j, COL_YOSHA_CODE, wk_info.TorihikiCode.ToString("#####"));
                    datamodel.SetValue(j, COL_YOSHA_NAME, wk_info.TorihikiShortName);
                    datamodel.SetValue(j, COL_TANKA, wk_info.AtPrice.ToString(atpriceFormat));
                    datamodel.SetValue(j, COL_SURYO, wk_info.Number.ToString(numberFormat));
                    datamodel.SetValue(j, COL_KINGAKU, wk_info.PriceInPrice.ToString("###,###,###"));
                    datamodel.SetValue(j, COL_TSUKORYO, wk_info.TollFeeInPrice.ToString("###,###"));
                    datamodel.SetValue(j, COL_FUTAIGYOMURYO, wk_info.FutaigyomuryoInPrice.ToString("###,###,###,###,###,###"));
                    datamodel.SetValue(j, COL_YOSHA_KINGAKU, wk_info.PriceInCharterPrice.ToString("###,###,###"));
                    datamodel.SetValue(j, COL_YOSHA_TSUKORYO, wk_info.TollFeeInCharterPrice.ToString("###,###"));

                    // Tagへ1件分のデータを退避（後で使用）
                    datamodel.SetTag(j, COL_HANRO_CODE, wk_info);
                }

                // トラDON_V40の場合
                if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                    == (int)DefaultProperty.TraDonVersionKbn.V40)
                {
                    sheet.SetColumnVisible(COL_FUTAIGYOMURYO, false);
                } 
                else 
                {
                    sheet.SetColumnVisible(COL_FUTAIGYOMURYO, true);
                }

                //検索結果の1行目を選択状態にする
                sheet.SetActiveCell(0, 0, true);
                sheet.AddSelection(0, 0, 1, 1);

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
        /// 選択している過去受注履歴情報をメンバにセットします。
        /// </summary>
        private void SetSelectedInfo()
        {
            List<PastPriceInfo> wk_val = this.GetInfoByListOnSelection();

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
                if (this.CheckInputs(wk_val)) 
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
        }

        /// <summary>
        /// 一覧にて選択中の過去受注履歴情報を取得します。
        /// </summary>
        /// <returns>過去受注履歴情報</returns>
        private List<PastPriceInfo> GetInfoByListOnSelection()
        {
            List<PastPriceInfo> rt_list = new List<PastPriceInfo>();

            SheetView sheet0 = fpListGrid.Sheets[0];

            if (sheet0.SelectionCount > 0)
            {
                //選択行をループで回してコードを取得
                foreach (CellRange item in sheet0.GetSelections())
                {
                    //ブロックの中のセルを取り出してコードをリストに追加
                    for (int i = 0; i < item.RowCount; i++)
                    {
                        PastPriceInfo info =
                            ((PastPriceInfo)sheet0.Cells[item.Row + i, COL_HANRO_CODE].Tag);

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
                    if (this.fpListGrid.Sheets[0].RowCount == 0)
                    {
                        SendKeys.Send("{TAB}");
                    }
                    else
                    {
                        this.SetSelectedInfo();
                    }
                    break;
                case Keys.F12:
                    SendKeys.SendWait("+{TAB}");
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

        #endregion

        #region 検証（Validate）処理

        /// <summary>
        /// 検索用販路コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchHanroCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchHanroCode == 0)
                {
                    is_clear = true;
                    return;
                }

                HanroInfo info =
                    this.dalUtil.Hanro.GetInfo(this.SearchHanroCode);

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
                        this.numSearchHanroCode.Tag = info.HanroId;
                        this.numSearchHanroCode.Value = info.HanroCode;
                        this.edtSearchHanroName.Text = info.HanroName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchHanroCode.Tag = null;
                    this.numSearchHanroCode.Value = null;
                    this.edtSearchHanroName.Text = string.Empty;
                }

            }
        }

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

            }
        }

        /// <summary>
        /// 検索用車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchCarKindCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchCarKindCode == 0)
                {
                    is_clear = true;
                    return;
                }

                CarKindInfo info =
                    this.dalUtil.CarKind.GetInfo(this.SearchCarKindCode);

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
                        this.numSearchCarKindCode.Tag = info.ToraDONCarKindId;
                        this.numSearchCarKindCode.Value = info.ToraDONCarKindCode;
                        this.edtSearchCarKindName.Text = info.ToraDONCarKindName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchCarKindCode.Tag = null;
                    this.numSearchCarKindCode.Value = null;
                    this.edtSearchCarKindName.Text = string.Empty;
                }

            }
        }

        /// <summary>
        /// 発着地コード検索処理
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pointFlg">積地フラグ（true:積、false:着）</param>
        private void SetPointName(CancelEventArgs e, bool startPointFlg)
        {
            bool isClear = false;

            int pointCode = 0;

            if (startPointFlg)
            {
                // 積地
                if (0 < this.SearchStartPointCode)
                {
                    pointCode = this.SearchStartPointCode;
                }
            }
            else
            {
                // 着地
                if (0 < this.SearchEndPointCode)
                {
                    pointCode = this.SearchEndPointCode;
                }
            }

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == pointCode)
                {
                    isClear = true;
                    return;
                }

                // 発着地情報を取得
                PointSearchParameter para = new PointSearchParameter()
                {
                    ToraDONPointCode = pointCode
                };

                PointInfo info = this.dalUtil.Point.GetList(para).FirstOrDefault();

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
                    if (startPointFlg)
                    {
                        // 積地
                        this.numSearchStartPointCode.Tag = info.ToraDONPointId;
                        this.numSearchStartPointCode.Value = info.ToraDONPointCode;
                        this.edtSearchStartPointName.Text = info.ToraDONPointName;
                    }
                    else
                    {
                        // 着地
                        this.numSearchEndPointCode.Tag = info.ToraDONPointId;
                        this.numSearchEndPointCode.Value = info.ToraDONPointCode;
                        this.edtSearchEndPointName.Text = info.ToraDONPointName;
                    }
                }
            }
            finally
            {
                if (isClear)
                {
                    if (startPointFlg)
                    {
                        // 積地
                        this.numSearchStartPointCode.Tag = null;
                        this.numSearchStartPointCode.Value = null;
                        this.edtSearchStartPointName.Text = string.Empty;
                    }
                    else
                    {
                        // 着地
                        this.numSearchEndPointCode.Tag = null;
                        this.numSearchEndPointCode.Value = null;
                        this.edtSearchEndPointName.Text = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 検索用単位コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchFigCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchFigCode == 0)
                {
                    is_clear = true;
                    return;
                }

                // 単位情報を取得
                FigSearchParameter para = new FigSearchParameter();
                para.FigCd = this.SearchFigCode;
                ToraDONFigInfo info = this.dalUtil.ToraDONFig.GetList(para).FirstOrDefault();

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
                        this.numSearchFigCode.Tag = info.FigId;
                        this.numSearchFigCode.Value = info.FigCode;
                        this.edtSearchFigName.Text = info.FigName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchFigCode.Tag = null;
                    this.numSearchFigCode.Value = null;
                    this.edtSearchFigName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 検索用品目コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchItemCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchItemCode == 0)
                {
                    is_clear = true;
                    return;
                }

                ItemInfo info =
                    this.dalUtil.Item.GetInfo(this.SearchItemCode);

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
                        this.numSearchItemCode.Tag = info.ToraDONItemId;
                        this.numSearchItemCode.Value = info.ToraDONItemCode;
                        this.edtSearchItemName.Text = info.ToraDONItemName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchItemCode.Tag = null;
                    this.numSearchItemCode.Value = null;
                    this.edtSearchItemName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 検索用傭車先コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (this.SearchCarOfChartererCode == 0)
                {
                    is_clear = true;
                    return;
                }

                TorihikisakiInfo info =
                    this.dalUtil.Torihikisaki.GetInfo(this.SearchCarOfChartererCode);

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
                        this.numSearchCarOfChartererCode.Tag = info.ToraDONTorihikiId;
                        this.numSearchCarOfChartererCode.Value = info.ToraDONTorihikiCode;
                        this.edtSearchCarOfChartererName.Text = info.ToraDONTorihikiName;
                    }
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numSearchCarOfChartererCode.Tag = null;
                    this.numSearchCarOfChartererCode.Value = null;
                    this.edtSearchCarOfChartererName.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 対象期間（範囲開始）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTaishoYMDFrom(CancelEventArgs e)
        {
            //対象期間（範囲終了）が設定されている場合
            if (this.dteTaishoYMDTo.Value != null)
            {
                //日付の範囲チェック
                if (Convert.ToDateTime(this.dteTaishoYMDTo.Value) < Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "日付" }),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );

                    this.dteTaishoYMDFrom.Focus();
                }
            }
        }

        /// <summary>
        /// 対象期間（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateTaishoYMDTo(CancelEventArgs e)
        {
            //対象期間（範囲終了）が空の場合
            if (this.dteTaishoYMDTo.Value != null)
            {
                //日付の範囲チェック
                if (Convert.ToDateTime(this.dteTaishoYMDTo.Value) < Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "日付" }),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );

                    this.dteTaishoYMDTo.Focus();
                }
            }
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 検索用販路コードの値を取得します。
        /// </summary>
        private int SearchHanroCode
        {
            get { return Convert.ToInt32(this.numSearchHanroCode.Value); }
        }

        /// <summary>
        /// 検索用得意先コードの値を取得します。
        /// </summary>
        private int SearchTokuisakiCode
        {
            get { return Convert.ToInt32(this.numSearchTokuisakiCode.Value); }
        }

        /// <summary>
        /// 検索用車種コードの値を取得します。
        /// </summary>
        private int SearchCarKindCode
        {
            get { return Convert.ToInt32(this.numSearchCarKindCode.Value); }
        }

        /// <summary>
        /// 検索用積地コードの値を取得します。
        /// </summary>
        private int SearchStartPointCode
        {
            get { return Convert.ToInt32(this.numSearchStartPointCode.Value); }
        }

        /// <summary>
        /// 検索用発地コードの値を取得します。
        /// </summary>
        private int SearchEndPointCode
        {
            get { return Convert.ToInt32(this.numSearchEndPointCode.Value); }
        }

        /// <summary>
        /// 検索用単位コードの値を取得します。
        /// </summary>
        private int SearchFigCode
        {
            get { return Convert.ToInt32(this.numSearchFigCode.Value); }
        }

        /// <summary>
        /// 検索用品目コードの値を取得します。
        /// </summary>
        private int SearchItemCode
        {
            get { return Convert.ToInt32(this.numSearchItemCode.Value); }
        }

        /// <summary>
        /// 検索用傭車コードの値を取得します。
        /// </summary>
        private int SearchCarOfChartererCode
        {
            get { return Convert.ToInt32(this.numSearchCarOfChartererCode.Value); }
        }

        /// <summary>
        /// 選択された検索テーブル情報を取得します。
        /// </summary>
        public PastPriceInfo SelectedInfo { get; private set; }

        /// <summary>
        /// 選択された過去受注履歴情報を一覧で取得します。
        /// </summary>
        public List<PastPriceInfo> SelectedInfoList { get; private set; }

        /// <summary>
        /// 起動パラメータを取得・設定します
        /// </summary>
        public PastPriceDefaultParameters PastPriceDefaultParameters
        {
            get { return this._defpara; }
            set { this._defpara = value; }
        }

        /// <summary>
        /// 削除データを除く全データを表示するかどうかの値を取得・設定します（True:表示する、False:表示しない）
        /// </summary>
        public bool ShowAllFlag
        {
            get { return this._showAllFlag; }
            set { this._showAllFlag = value; }
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
            this.InitCmnSearchPastPriceFrame();
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

        private void numSearchHanroCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用販路コード
            this.ValidateSearchHanroCode(e);
        }

        private void numSearchTokuisakiCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidateSearchTokuisakiCode(e);
        }

        private void numSearchCarKindCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用車種コード
            this.ValidateSearchCarKindCode(e);
        }

        private void numSearchStartPointCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidatePointCd(e, true);
        }

        private void numSearchEndPointCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidatePointCd(e, false);
        }

        /// <summary>
        /// 発着地コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pointFlg">積地フラグ（true:積、false:着）</param>
        private void ValidatePointCd(CancelEventArgs e, bool startPointFlg)
        {
            // コード検索処理
            this.SetPointName(e, startPointFlg);
        }

        private void numSearchFigCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用単位コード
            this.ValidateSearchFigCode(e);
        }

        private void numSearchItemCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用品目コード
            this.ValidateSearchItemCode(e);
        }

        private void numSearchCarOfChartererCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用得意先コード
            this.ValidateCarOfChartererCode(e);
        }

        private void dteTaishoYMDFrom_Validating(object sender, CancelEventArgs e)
        {
            //検索用対象日付（範囲開始）
            this.ValidateTaishoYMDFrom(e);
        }

        private void dteTaishoYMDTo_Validating(object sender, CancelEventArgs e)
        {
            //検索用対象日付（範囲終了）
            this.ValidateTaishoYMDTo(e);
        }

        private void CmnSearchPastPriceFrame_KeyDown(object sender, KeyEventArgs e)
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

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs(List<PastPriceInfo> wk_val)
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;


            // 呼び出し元の車両区分が傭車以外、かつ、選択明細の傭車金額が0以上の場合、エラー
            if (rt_val && this.PastPriceDefaultParameters.CarKbn != (int)DefaultProperty.CarKbn.Yosha
                && this.PastPriceDefaultParameters.CarKbn != 0
                && wk_val[0].PriceInCharterPrice.CompareTo(decimal.Zero) > 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2202016", new string[] { "傭車以外の受注情報の場合","傭車金額を設定することは" });
                ctl = this.fpListGrid;
            }


            if (!rt_val)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();

                //返却
                return rt_val;
            }

            //返却
            return true;
        }

        private void Ctl_KeyDown(object sender, KeyEventArgs e)
        {
            FrameUtilites.SelectNextControlByKeyDown(sender, e, this);
        }

        private void Rad_CheckedChanged(object sender, EventArgs e)
        {
            this.numSearchHanroCode.Enabled = this.radHaishaAce.Checked;
            this.sbtnSearchHanroCode.Enabled = this.radHaishaAce.Checked;
            this.edtSearchHanroName.Enabled = this.radHaishaAce.Checked;
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        private bool CheckInputs(bool initFlg)
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            if (this.SearchHanroCode == 0 &&
                this.SearchTokuisakiCode == 0 &&
                this.SearchCarKindCode == 0 &&
                this.SearchStartPointCode == 0 &&
                this.SearchEndPointCode == 0 &&
                this.SearchFigCode == 0 &&
                this.SearchItemCode == 0 &&
                this.SearchCarOfChartererCode == 0
                ) {
                rt_val = false;
                msg = "検索条件のコード値はいずれか1つは設定してください。";

                if (this.numSearchHanroCode.Enabled) 
                {
                    ctl = this.numSearchHanroCode;
                }
                else
                {
                    ctl = this.numSearchStartPointCode;
                }
            }

            //日付の範囲チェック
            if (rt_val
                && DateTime.MinValue < Convert.ToDateTime(this.dteTaishoYMDFrom.Value)
                && DateTime.MinValue < Convert.ToDateTime(this.dteTaishoYMDTo.Value)
                && Convert.ToDateTime(this.dteTaishoYMDTo.Value) < Convert.ToDateTime(this.dteTaishoYMDFrom.Value))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2202018", new string[] { "日付" });
                ctl = this.dteTaishoYMDTo;
            }

            // 初期表示以外のみ
            if (!rt_val && !initFlg)
            {
                MessageBox.Show(msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ctl.Focus();
            }

            //返却
            return rt_val;
        }

        private void dteTaishoYMD_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
