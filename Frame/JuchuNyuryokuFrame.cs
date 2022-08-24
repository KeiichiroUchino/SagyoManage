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
    public partial class JuchuNyuryokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public JuchuNyuryokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 受注№を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="JuchuSlipNo"></param>
        /// <param name="JuchuId"></param>
        public JuchuNyuryokuFrame(decimal JuchuSlipNo, decimal JuchuId)
        {
            InitializeComponent();

            //メンバにセット
            this._OriginJuchuNo = JuchuSlipNo;
            this._OriginJuchuId = JuchuId;
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

        /// <summary>
        /// 各GcNumberに対応するSearchFunctionを返します。
        /// ex) SearchFunction[numCarCode] => ShowCmnSearchCar
        /// </summary>
        private Dictionary<Control, System.Action> SearchFunctions;

        #endregion

        #region ユーザ定義

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "受注入力";

        /// <summary>
        /// 品目が必須かどうか
        /// </summary>
        private bool _itemIsRequired;

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
        /// 受注入力クラス
        /// </summary>
        private JuchuNyuryoku _JuchuNyuryoku;

        /// <summary>
        /// 受注検索用パラメータ
        /// </summary>
        private JuchuNyuryokuSearchParameter _JuchuNyuryokuSearchParameter;

        //ローカル変数（本画面特有）
        /// <summary>
        /// 外部から指定された受注伝票番号を保持する領域
        /// </summary>
        private decimal _OriginJuchuNo = decimal.MinValue;

        /// <summary>
        /// 外部から指定された受注IDを保持する領域
        /// </summary>
        private decimal _OriginJuchuId = decimal.MinValue;

        /// <summary>
        /// 画面表示可能かどうかの値を保持する領域（true：表示可）
        /// </summary>
        private bool canShowFrame = true;

        /// <summary>
        /// 画面を表示する時のエラーメッセージを保持する領域
        /// </summary>
        private string showFrameMsg = string.Empty;

        /// <summary>
        /// ロックパターン
        /// </summary>
        private LockPattern _lockPattern = LockPattern.None;

        /// <summary>
        /// 明細・ヘッダーをロックする理由を表す列挙体
        /// </summary>
        private enum LockPattern
        {
            /// <summary>
            /// ロック無し
            /// </summary>
            None = 0,
            /// <summary>
            /// 受注明細が請求集計済み
            /// </summary>
            JuchuDtlIsFixedClaim = 1,
            /// <summary>
            /// 受注明細が月次更新済み
            /// </summary>
            JuchuDtlIsFixedMonth = 2,
            /// <summary>
            /// 受注明細が支払集計済み
            /// </summary>
            JuchuDtlIsFixedPay = 4,
        }

        /// <summary>
        /// 独自ロックの色定義
        /// </summary>
        private readonly static Color _originalLockBackColor = SystemColors.Control;

        #region MultiRow関連

        /// <summary>
        /// 明細の列名を表します。
        /// </summary>
        private enum MrowCellKeys
        {
            RowNo,
            HanroCd,
            HanroName,
            OfukuButton,
            HanroAddButton,
            TokuisakiCd,
            TokuisakiNM,
            ClmClassCd,
            ClmClassNM,
            ContractCd,
            ContractNM,
            OwnerCd,
            OwnerNM,
            StartPointCd,
            StartPointNM,
            EndPointCd,
            EndPointNM,
            ItemCd,
            ItemNM,
            Number,
            FigCd,
            FigNM,
            CarCd,
            LicPlateCarNo,
            Chartererkbn,
            CarKbn,
            CarKindCd,
            CarKindName,
            DriverCd,
            DriverNm,
            CarOfChartererCd,
            CarOfChartererName,
            TaskStartYMD,
            TaskStartHM,
            TaskEndYMD,
            TaskEndHM,
            AtPrice,
            Weight,
            Memo,
            JomuinUriageDogakuFlag,
            JomuinUriageKingaku,
            FutaigyomuryoInPrice,
            JuchuTantoCd,
            JuchuTantoNm,
            Price,
            CharterPrice,
            TaxDispKbn,
            CharterTaxDispKbn,
            TollFeeInPrice,
            TollFeeInCharterPrice,
            AddUpYMD,
            CharterAddUpYMD,
            FixFlag,
            CharterFixFlag,
            ReuseYMD,
            ReuseHM,
            MagoYoshasaki,
            ReceivedFlag,
        }

        /// <summary>
        /// 金額の最大値
        /// </summary>
        private const decimal PRICE_MAXVALUE = 999999999;

        /// <summary>
        /// 金額の最小値
        /// </summary>
        private const decimal PRICE_MINVALUE = -999999999;

        /// <summary>
        /// 重量の最大値
        /// </summary>
        private const decimal WEIGHT_MAXVALUE = 99999;

        /// <summary>
        /// 重量の最小値
        /// </summary>
        private const decimal WEIGHT_MINVALUE = -99999;

        /// <summary>
        /// 表示用の受注情報リストを保持する。
        /// </summary>
        private List<JuchuNyuryokuInfo> _JuchuInfoSelList = new List<JuchuNyuryokuInfo>();

        /// <summary>
        /// 登録用の受注情報リストを保持する。
        /// </summary>
        private List<JuchuNyuryokuInfo> _JuchuInfoUpdList = new List<JuchuNyuryokuInfo>();

        /// <summary>
        /// 削除用の受注情報リストを保持する。
        /// </summary>
        private List<JuchuNyuryokuInfo> _JuchuInfoDelList = new List<JuchuNyuryokuInfo>();

        /// <summary>
        /// システム設定情報を保持する。
        /// </summary>
        private SystemSettingsInfo _SystemSettingsInfo;

        /// <summary>
        /// 管理マスタレコードを保持する。
        /// </summary>
        private ToraDonSystemPropertyInfo _ToraDonSystemPropertyInfo;

        /// <summary>
        /// 管理マスタを保持する。
        /// </summary>
        private KanriInfo _KanriInfo;

        /// <summary>
        /// 税区分短縮名リストを保持する。（税区分コンボボックス用）
        /// </summary>
        private IList<SystemNameInfo> _TaxDispKbnShortList;

        #endregion

        /// <summary>
        /// 複写メッセージの初期行
        /// </summary>
        private const string FUKUSHA_DEFAULT_STR = "上";

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
                {this.numFukushaHanroGroupCode, this.ShowCmnSearchHanroGroup},
            };

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            //受注入力クラスインスタンス作成
            this._JuchuNyuryoku = new JuchuNyuryoku(this.appAuth);
            //システム設定情報取得
            this._SystemSettingsInfo = UserProperty.GetInstance().SystemSettingsInfo;
            ////トラDONの管理情報取得
            //this._ToraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();
            //管理情報取得
            this._KanriInfo = this._DalUtil.Kanri.GetInfo();
            //税区分短縮名リスト取得
            this._TaxDispKbnShortList = this._DalUtil.SystemGlobalName.GetList((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort);

            //コンボボックス関連の初期化
            this.InitCombo();

            //入力項目のクリア
            this.ClearInputs();

            // モード設定（画面間パラメータの設定有無で判定）
            if (decimal.MinValue < this._OriginJuchuNo)
            {
                this._JuchuNyuryokuSearchParameter = new JuchuNyuryokuSearchParameter();
                this._JuchuNyuryokuSearchParameter.JuchuSlipNo = this._OriginJuchuNo;
                //現在の画面モードを修正状態に変更
                this.ChangeMode(FrameEditMode.Editable);
                //データ表示
                this.DoGetData();
            }
            else
            {
                //現在の画面モードを初期状態に変更
                this.ChangeMode(FrameEditMode.New);
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
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.EditCancel);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.DeleteAllDtl);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 入力項目をクリアします。
        /// </summary>
        private void ClearInputs()
        {
            this.edtLastEntryJuchuSlipNo.Text = string.Empty;
            this.edtJuchuSlipNo.Text = string.Empty;
            this.cmbBranchOffice.SelectedValue = null;
            this.cmbBranchOffice.Tag = null;
            this.numFukushaMotoNo.Value = null;
            this.chkHizukeFukushaFlag.Checked = this._KanriInfo.JuchuNyuryokuHizukeFukushaKahiDefaultFlag;
            this.edtPriceTotal.Text = "0";
            this.edtTollFeeInPrice.Text = "0";
            this.edtFutaigyomuryoInPrice.Text = "0";
            this.edtCharterPriceTotal.Text = "0";
            this.edtTollFeeInCharterPrice.Text = "0";

            this.numFukushaHanroGroupCode.Tag = null;
            this.numFukushaHanroGroupCode.Value = null;
            this.edtFukushaHanroGroupName.Text = string.Empty;

            //受注明細の初期化
            this.InitMrow();
            //受注明細クリア
            this.ClearJuchuDtlList();

            //メンバをクリア
            this.isConfirmClose = true;
            this._JuchuInfoDelList = new List<JuchuNyuryokuInfo>();
        }

        /// <summary>
        /// Mrow関連を初期化します。
        /// </summary>
        private void InitMrow()
        {
            this.InitJuchuNyuryokuMeisaiListMrow();
        }

        /// <summary>
        /// 受注明細のMrowを初期化します。
        /// </summary>
        private void InitJuchuNyuryokuMeisaiListMrow()
        {
            //描画を停止
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {

                //Mrowのセルのスタイルの設定
                DynamicCellStyle dynamicellstyle = new DynamicCellStyle();

                //テンプレートを取得
                Template tpl = this.mrsJuchuMeisai.Template;

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
                }

                tpl.Row.Cells[MrowCellKeys.RowNo.ToString()].Style = new DynamicCellStyle(context =>
                {
                    var style = new CellStyle();

                    GrapeCity.Win.MultiRow.Border border = new GrapeCity.Win.MultiRow.Border();
                    border.Left = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
                    border.Right = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
                    border.Top = new GrapeCity.Win.MultiRow.Line(GrapeCity.Win.MultiRow.LineStyle.Thin, System.Drawing.Color.DarkGray);
                    style.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(220)))), ((int)(((byte)(200)))));
                    style.Border = border;
                    style.Font = new System.Drawing.Font("メイリオ", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
                    style.ForeColor = System.Drawing.SystemColors.ControlText;
                    style.ImageAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleCenter;
                    style.TextAlign = GrapeCity.Win.MultiRow.MultiRowContentAlignment.MiddleRight;
                    style.TextAngle = 0F;
                    style.TextEffect = GrapeCity.Win.MultiRow.TextEffect.Flat;
                    style.TextImageRelation = GrapeCity.Win.MultiRow.MultiRowTextImageRelation.ImageBeforeText;
                    style.TextIndent = 0;
                    style.WordWrap = GrapeCity.Win.MultiRow.MultiRowTriState.False;

                    if (context.GcMultiRow.NewRowIndex == context.RowIndex)
                    {
                        //新規行の場合は背景色を変える
                        if (context.GcMultiRow.CurrentCellPosition.RowIndex != context.RowIndex || context.GcMultiRow.EditingControl == null)
                        {
                            style.BackColor = SystemColors.Control;
                        }
                    }

                    return style;
                });

                //税区分コンボボックスの作成
                GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell cmbTaxKbn =
                    (GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell)tpl.Row.Cells[MrowCellKeys.TaxDispKbn.ToString()];
                Dictionary<int, string> wk_taxkbns = new Dictionary<int, string>();
                foreach (SystemNameInfo item in this._TaxDispKbnShortList)
                {
                    wk_taxkbns.Add(item.SystemNameCode, item.SystemName);
                }
                FrameUtilites.SetupGcComboBoxCellEx(cmbTaxKbn, wk_taxkbns);

                //傭車税区分コンボボックスの作成
                GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell cmbCharterTaxKbn =
                    (GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell)tpl.Row.Cells[MrowCellKeys.CharterTaxDispKbn.ToString()];
                Dictionary<int, string> wk_chartertaxkbns = new Dictionary<int, string>();
                foreach (SystemNameInfo item in this._TaxDispKbnShortList)
                {
                    wk_chartertaxkbns.Add(item.SystemNameCode, item.SystemName);
                }
                FrameUtilites.SetupGcComboBoxCellEx(cmbCharterTaxKbn, wk_chartertaxkbns);

                //品目必須フラグ
                this._itemIsRequired = _ToraDonSystemPropertyInfo.ItemMustFlag;

                //ヘッダ
                ColumnHeaderSection columnHeader = tpl.ColumnHeaders["columnHeaderSection1"];
                // 品目　必須項目の場合は "*" を付加する
                columnHeader["clmHeaderHinmoku"].Value =
                    "品目" + ((this._itemIsRequired) ? " *" : string.Empty);

                //セルの初期値の設定
                // 販路ID
                tpl.Row.Cells[MrowCellKeys.HanroCd.ToString()].Tag = null;
                // 販路コード
                tpl.Row.Cells[MrowCellKeys.HanroCd.ToString()].Value = 0;
                // 販路名
                tpl.Row.Cells[MrowCellKeys.HanroName.ToString()].Value = "";
                // 往復区分
                tpl.Row.Cells[MrowCellKeys.OfukuButton.ToString()].Tag = null;
                // 往復区分名
                tpl.Row.Cells[MrowCellKeys.OfukuButton.ToString()].Value = "";
                // 得意先ID
                tpl.Row.Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = null;
                // 得意先コード
                tpl.Row.Cells[MrowCellKeys.TokuisakiCd.ToString()].Value = 0;
                // 得意先名
                tpl.Row.Cells[MrowCellKeys.TokuisakiNM.ToString()].Value = "";
                // 請求部門コード
                tpl.Row.Cells[MrowCellKeys.ClmClassCd.ToString()].Value = 0;
                // 請求部門名
                tpl.Row.Cells[MrowCellKeys.ClmClassNM.ToString()].Value = "";
                // 請負コード
                tpl.Row.Cells[MrowCellKeys.ContractCd.ToString()].Value = 0;
                // 請負名
                tpl.Row.Cells[MrowCellKeys.ContractNM.ToString()].Value = "";
                // 積地ID
                tpl.Row.Cells[MrowCellKeys.StartPointCd.ToString()].Tag = null;
                // 積地コード
                tpl.Row.Cells[MrowCellKeys.StartPointCd.ToString()].Value = 0;
                // 積地名
                tpl.Row.Cells[MrowCellKeys.StartPointNM.ToString()].Value = "";
                // 着地ID
                tpl.Row.Cells[MrowCellKeys.EndPointCd.ToString()].Tag = null;
                // 着地コード
                tpl.Row.Cells[MrowCellKeys.EndPointCd.ToString()].Value = 0;
                // 着地名
                tpl.Row.Cells[MrowCellKeys.EndPointNM.ToString()].Value = "";
                // 品目ID
                tpl.Row.Cells[MrowCellKeys.ItemCd.ToString()].Tag = 0;
                // 品目コード
                tpl.Row.Cells[MrowCellKeys.ItemCd.ToString()].Value = 0;
                // 品目名
                tpl.Row.Cells[MrowCellKeys.ItemNM.ToString()].Value = "";
                // 車両ID
                tpl.Row.Cells[MrowCellKeys.CarCd.ToString()].Tag = null;
                // 車両コード
                tpl.Row.Cells[MrowCellKeys.CarCd.ToString()].Value = 0;
                // 車番
                tpl.Row.Cells[MrowCellKeys.LicPlateCarNo.ToString()].Value = string.Empty;
                // 車両区分（隠し）
                tpl.Row.Cells[MrowCellKeys.CarKbn.ToString()].Value = 0;
                // 傭車区分
                tpl.Row.Cells[MrowCellKeys.Chartererkbn.ToString()].Value = string.Empty;
                // 車種ID
                tpl.Row.Cells[MrowCellKeys.CarKindCd.ToString()].Tag = null;
                // 車種コード
                tpl.Row.Cells[MrowCellKeys.CarKindCd.ToString()].Value = 0;
                // 車種名
                tpl.Row.Cells[MrowCellKeys.CarKindName.ToString()].Value = string.Empty;
                // 乗務員ID
                tpl.Row.Cells[MrowCellKeys.DriverCd.ToString()].Tag = null;
                // 乗務員コード
                tpl.Row.Cells[MrowCellKeys.DriverCd.ToString()].Value = 0;
                // 乗務員名
                tpl.Row.Cells[MrowCellKeys.DriverNm.ToString()].Value = string.Empty;
                // 傭車先ID
                tpl.Row.Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = null;
                // 傭車先コード
                tpl.Row.Cells[MrowCellKeys.CarOfChartererCd.ToString()].Value = 0;
                // 傭車先名
                tpl.Row.Cells[MrowCellKeys.CarOfChartererName.ToString()].Value = string.Empty;
                // 積日
                tpl.Row.Cells[MrowCellKeys.TaskStartYMD.ToString()].Value = null;
                // 積時刻
                tpl.Row.Cells[MrowCellKeys.TaskStartHM.ToString()].Value = null;
                // 着日
                tpl.Row.Cells[MrowCellKeys.TaskEndYMD.ToString()].Value = null;
                // 着時刻
                tpl.Row.Cells[MrowCellKeys.TaskEndHM.ToString()].Value = null;
                // 再使用可能日
                tpl.Row.Cells[MrowCellKeys.ReuseYMD.ToString()].Value = null;
                // 再使用可能時刻
                tpl.Row.Cells[MrowCellKeys.ReuseHM.ToString()].Value = null;
                // 数量
                tpl.Row.Cells[MrowCellKeys.Number.ToString()].Value = this._ToraDonSystemPropertyInfo.DefaultDailyDriveReportNumber;
                // 単位ID
                tpl.Row.Cells[MrowCellKeys.FigCd.ToString()].Tag = null;
                // 単位コード
                tpl.Row.Cells[MrowCellKeys.FigCd.ToString()].Value = 0;
                // 単位名
                tpl.Row.Cells[MrowCellKeys.FigNM.ToString()].Value = "";
                // 単価
                tpl.Row.Cells[MrowCellKeys.AtPrice.ToString()].Value = 0;
                // 重量
                tpl.Row.Cells[MrowCellKeys.Weight.ToString()].Value = 0;
                // 受注金額
                tpl.Row.Cells[MrowCellKeys.Price.ToString()].Value = 0;
                // 金額_通行料
                tpl.Row.Cells[MrowCellKeys.TollFeeInPrice.ToString()].Value = 0;
                // 税区分 初期値
                tpl.Row.Cells[MrowCellKeys.TaxDispKbn.ToString()].Value = (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei;
                // 計上日
                tpl.Row.Cells[MrowCellKeys.AddUpYMD.ToString()].Value = null;
                // 確定フラグ
                tpl.Row.Cells[MrowCellKeys.FixFlag.ToString()].Value = false;
                // 傭車金額
                tpl.Row.Cells[MrowCellKeys.CharterPrice.ToString()].Value = 0;
                // 傭車金額_通行料
                tpl.Row.Cells[MrowCellKeys.TollFeeInCharterPrice.ToString()].Value = 0;
                // 傭車税区分 初期値
                tpl.Row.Cells[MrowCellKeys.CharterTaxDispKbn.ToString()].Value = (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei;
                // 傭車計上日
                tpl.Row.Cells[MrowCellKeys.CharterAddUpYMD.ToString()].Value = null;
                // 傭車確定フラグ
                tpl.Row.Cells[MrowCellKeys.CharterFixFlag.ToString()].Value = false;
                // 荷主ID
                tpl.Row.Cells[MrowCellKeys.OwnerCd.ToString()].Tag = null;
                // 荷主コード
                tpl.Row.Cells[MrowCellKeys.OwnerCd.ToString()].Value = 0;
                // 荷主名
                tpl.Row.Cells[MrowCellKeys.OwnerNM.ToString()].Value = "";
                // 備考
                tpl.Row.Cells[MrowCellKeys.Memo.ToString()].Value = "";
                // 受注担当ID
                tpl.Row.Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag = null;
                // 受注担当コード
                tpl.Row.Cells[MrowCellKeys.JuchuTantoCd.ToString()].Value = 0;
                // 受注担当名
                tpl.Row.Cells[MrowCellKeys.JuchuTantoNm.ToString()].Value = string.Empty;
                // 金額_附帯業務料
                tpl.Row.Cells[MrowCellKeys.FutaigyomuryoInPrice.ToString()].Value = 0;
                // 乗務員売上同額フラグ
                tpl.Row.Cells[MrowCellKeys.JomuinUriageDogakuFlag.ToString()].Value = false;
                // 乗務員売上金額
                tpl.Row.Cells[MrowCellKeys.JomuinUriageKingaku.ToString()].Value = 0;
                // 孫傭車先
                tpl.Row.Cells[MrowCellKeys.MagoYoshasaki.ToString()].Value = "";
                // 受領済フラグ
                tpl.Row.Cells[MrowCellKeys.ReceivedFlag.ToString()].Value = false;

                // トラDON_V40の場合
                if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                    == (int)DefaultProperty.TraDonVersionKbn.V40)
                {
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

                tpl.Row.DefaultCellStyle = dynamicellstyle;

                //数量、単価のセルの書式を設定します
                this.SetCellFormatSetting(tpl);

                //テンプレートを再設定
                this.mrsJuchuMeisai.Template = tpl;

                // F9制御を追加(独自にアクションクラスを作成)
                this.mrsJuchuMeisai.ShortcutKeyManager.Register(new MRowTaxKbnToNext(), Keys.F9);

                // Shift+F4制御を追加（独自にアクションクラスを作成）
                this.mrsJuchuMeisai.ShortcutKeyManager.Register(new DelegateAction(this.DoMRowLineRemove), Keys.F4 | Keys.Shift);

                // Shift+F11制御を追加(独自にアクションクラスを作成)
                this.mrsJuchuMeisai.ShortcutKeyManager.Register(new DelegateAction(this.DoMRowAddCopy), Keys.F11 | Keys.Shift);

                //***ショートカットキー
                // 基本設定
                this.mrsJuchuMeisai.InitialShortcutKeySetting();

                // 単一セル選択モード
                this.mrsJuchuMeisai.MultiSelect = false;

                //行をクリア
                // -MultiRow.Clear() メソッドは未対応
                this.mrsJuchuMeisai.Rows.Clear();

                // ユーザでの行追加を一時的に許可
                this.mrsJuchuMeisai.AllowUserToAddRows = true;
            }
            finally
            {
                this.mrsJuchuMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 売上一覧の数量、単価セルの書式を設定します。
        /// </summary>
        /// <param name="tpl"></param>
        private void SetCellFormatSetting(Template tpl)
        {
            //最大値と最小値を設定します
            decimal maxValue_integer = 0;
            decimal maxValue_decimal = 0;
            decimal maxValue = 0;

            InputManCell.NumberDecimalPartDisplayField numberDecimalPartDisplayField = null;

            #region 数量

            //数量の桁数を取得する(整数部と小数部)
            int intDegit_Number = Property.UserProperty.GetInstance().JuchuNumberIntDigits;
            int decimaDegit_Number = Property.UserProperty.GetInstance().JuchuNumberDecimalDigits;

            //数量のセルを変数で取り出す
            var numbercell = (InputManCell.GcNumberCell)tpl.Row.Cells[MrowCellKeys.Number.ToString()];

            //Fieldsプロパティから整数部、小数部を設定します。
            numbercell.Fields.IntegerPart.MaxDigits = intDegit_Number;
            numbercell.Fields.DecimalPart.MaxDigits = decimaDegit_Number;
            numbercell.Fields.DecimalPart.MinDigits = decimaDegit_Number;

            // 表示フィールドのオブジェクトを作成します。
            numberDecimalPartDisplayField =
                //既存にNumberDecimalPartDisplayFieldがあれば取り出す
                numbercell.DisplayFields.Where(element => element is InputManCell.NumberDecimalPartDisplayField).FirstOrDefault() as InputManCell.NumberDecimalPartDisplayField;

            //なければ NumberDecimalPartDisplayFieldをNewして追加する
            if (numberDecimalPartDisplayField == null)
            {
                numberDecimalPartDisplayField = new InputManCell.NumberDecimalPartDisplayField();
                numbercell.DisplayFields.Add(numberDecimalPartDisplayField);
            }

            //小数部の桁数
            numberDecimalPartDisplayField.MaxDigits = decimaDegit_Number;
            numberDecimalPartDisplayField.MinDigits = decimaDegit_Number;

            //最大値と最小値を設定します
            maxValue_integer = 0;
            maxValue_decimal = 0;
            maxValue = 0;

            if (intDegit_Number > 0)
            {
                maxValue_integer = (decimal)Math.Pow(10, intDegit_Number) - 1;
            }

            if (decimaDegit_Number > 0)
            {
                maxValue_decimal = 1 - (decimal)Math.Pow(0.1, decimaDegit_Number);
            }

            maxValue = maxValue_integer + maxValue_decimal;

            numbercell.MaxValue = maxValue;
            numbercell.MinValue = maxValue * -1;

            #endregion

            #region 単価

            //数量の桁数を取得する(整数部と小数部)
            int intDegit_AtPrice = Property.UserProperty.GetInstance().JuchuAtPriceIntDigits;
            int decimaDegit_AtPrice = Property.UserProperty.GetInstance().JuchuAtPriceDecimalDigits;

            //単価のセルを変数で取り出す
            var atpricecell = (InputManCell.GcNumberCell)tpl.Row.Cells[MrowCellKeys.AtPrice.ToString()];

            //Fieldsプロパティから整数部、小数部を設定します。
            atpricecell.Fields.IntegerPart.MaxDigits = intDegit_AtPrice;
            atpricecell.Fields.DecimalPart.MaxDigits = decimaDegit_AtPrice;
            atpricecell.Fields.DecimalPart.MinDigits = decimaDegit_AtPrice;

            // 表示フィールドのオブジェクトを作成します。
            numberDecimalPartDisplayField =
                //既存にNumberDecimalPartDisplayFieldがあれば取り出す
                atpricecell.DisplayFields.Where(element => element is InputManCell.NumberDecimalPartDisplayField).FirstOrDefault() as InputManCell.NumberDecimalPartDisplayField;

            //なければ NumberDecimalPartDisplayFieldをNewして追加する
            if (numberDecimalPartDisplayField == null)
            {
                numberDecimalPartDisplayField = new InputManCell.NumberDecimalPartDisplayField();
                atpricecell.DisplayFields.Add(numberDecimalPartDisplayField);
            }

            //小数部の桁数
            numberDecimalPartDisplayField.MaxDigits = decimaDegit_AtPrice;
            numberDecimalPartDisplayField.MinDigits = decimaDegit_AtPrice;

            //最大値と最小値を設定します
            maxValue_integer = 0;
            maxValue_decimal = 0;
            maxValue = 0;

            if (intDegit_AtPrice > 0)
            {
                maxValue_integer = (decimal)Math.Pow(10, intDegit_AtPrice) - 1;
            }

            if (decimaDegit_AtPrice > 0)
            {
                maxValue_decimal = 1 - (decimal)Math.Pow(0.1, decimaDegit_AtPrice);
            }

            maxValue = maxValue_integer + maxValue_decimal;

            atpricecell.MaxValue = maxValue;
            atpricecell.MinValue = maxValue * -1;

            #endregion
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
            //// 営業所コンボ設定
            //Dictionary<String, String> datasource = new Dictionary<String, String>();

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

            //String key = "";
            //String value = "";

            //foreach (ToraDONBranchOfficeInfo item in list)
            //{
            //    key = item.BranchOfficeCode.ToString();
            //    value = item.BranchOfficeCode.ToString() + " " + item.BranchOfficeShortName.ToString();

            //    datasource.Add(key, value);
            //}

            //FrameUtilites.SetupGcComboBoxForValueText(this.cmbBranchOffice, datasource, true, null, false);
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

            // 販路コードの場合
            if (curCell.Name == MrowCellKeys.HanroCd.ToString())
            {
                int TokuisakiCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.TokuisakiCd.ToString()].Value);

                //using (CmnSearchHanroFrame f = new CmnSearchHanroFrame() { ParamTokuisakiCode = TokuisakiCd })
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.DisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.HanroCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 得意先コードの場合
            else if (curCell.Name == MrowCellKeys.TokuisakiCd.ToString())
            {
                using (CmnSearchTokuisakiFrame f = new CmnSearchTokuisakiFrame())
                {
                    f.InitFrame();
                    f.ShowDialog();

                    if (f.DialogResult == DialogResult.OK)
                    {
                        //isDisable = f.SelectedInfo.ToraDONDisableFlag;
                        //if (!isDisable)
                        //{
                        //    curMRow.BeginEdit(f.SelectedInfo.ToraDONTokuisakiCode);
                        //    this.OnCmnSearchComplete();
                        //}
                    }
                }
            }

            // 請求部門コードの場合
            else if (curCell.Name == MrowCellKeys.ClmClassCd.ToString())
            {
                //using (CmnSearchClmClassFrame f = new CmnSearchClmClassFrame())
                //{
                //    f.ParamTokuisakiCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(curRowIndex, MrowCellKeys.TokuisakiCd.ToString()));
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.DisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ClmClassCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 請負コードの場合
            else if (curCell.Name == MrowCellKeys.ContractCd.ToString())
            {
                //using (CmnSearchContractFrame f = new CmnSearchContractFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        curMRow.BeginEdit(f.SelectedInfo.ContractCode);
                //        this.OnCmnSearchComplete();
                //    }
                //}
            }

            // 荷主コードの場合
            else if (curCell.Name == MrowCellKeys.OwnerCd.ToString())
            {
                //using (CmnSearchOwnerFrame f = new CmnSearchOwnerFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.DisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.OwnerCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 積地コード、着地コードの場合
            else if (curCell.Name == MrowCellKeys.StartPointCd.ToString() || curCell.Name == MrowCellKeys.EndPointCd.ToString())
            {
                //using (CmnSearchPointFrame f = new CmnSearchPointFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONPointCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 車両コードの場合
            else if (curCell.Name == MrowCellKeys.CarCd.ToString())
            {
                using (CmnSearchCarFrame f = new CmnSearchCarFrame())
                {
                    f.InitFrame();
                    f.ShowDialog();

                    //if (f.DialogResult == DialogResult.OK)
                    //{
                    //    isDisable = f.SelectedInfo.ToraDONDisableFlag;
                    //    if (!isDisable)
                    //    {
                    //        curMRow.BeginEdit(f.SelectedInfo.ToraDONCarCode);
                    //        this.OnCmnSearchComplete();
                    //    }
                    //}
                }
            }

            // 車種コードの場合
            else if (curCell.Name == MrowCellKeys.CarKindCd.ToString())
            {
                //using (CmnSearchCarKindFrame f = new CmnSearchCarKindFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONCarKindCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 品目コードの場合
            else if (curCell.Name == MrowCellKeys.ItemCd.ToString())
            {
                //using (CmnSearchItemFrame f = new CmnSearchItemFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONItemCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 乗務員コードの場合
            else if (curCell.Name == MrowCellKeys.DriverCd.ToString())
            {
                //using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONStaffCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 受注担当コードの場合
            else if (curCell.Name == MrowCellKeys.JuchuTantoCd.ToString())
            {
                //using (CmnSearchJuchuTantoshaFrame f = new CmnSearchJuchuTantoshaFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONStaffCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 単位コードの場合
            else if (curCell.Name == MrowCellKeys.FigCd.ToString())
            {
                //using (CmnSearchFigFrame f = new CmnSearchFigFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.DisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.FigCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 傭車先コードの場合
            else if (curCell.Name == MrowCellKeys.CarOfChartererCd.ToString())
            {
                //using (CmnSearchTorihikisakiFrame f = new CmnSearchTorihikisakiFrame())
                //{
                //    //パラメータをセット
                //    f.ShowAllFlag = false;
                //    f.ShowYoshaFlag = true;

                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.ToraDONDisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONTorihikiCode);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 備考の場合
            else if (curCell.Name == MrowCellKeys.Memo.ToString())
            {
                //using (CmnSearchMemoFrame f = new CmnSearchMemoFrame())
                //{
                //    f.InitFrame();
                //    f.ShowDialog();

                //    if (f.DialogResult == DialogResult.OK)
                //    {
                //        isDisable = f.SelectedInfo.DisableFlag;
                //        if (!isDisable)
                //        {
                //            curMRow.BeginEdit(f.SelectedInfo.ToraDONMemoName);
                //            this.OnCmnSearchComplete();
                //        }
                //    }
                //}
            }

            // 単価の場合
            //if (curCell.Name == MrowCellKeys.AtPrice.ToString())
            //{
            //    int HanroCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.HanroCd.ToString()].Value);
            //    int TokuisakiCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.TokuisakiCd.ToString()].Value);
            //    int StartPointCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.StartPointCd.ToString()].Value);
            //    int EndPointCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.EndPointCd.ToString()].Value);
            //    int ItemCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.ItemCd.ToString()].Value);
            //    int FigCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.FigCd.ToString()].Value);
            //    int CarKindCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.CarKindCd.ToString()].Value);
            //    int TorihikiCd = Convert.ToInt32(curRow.Cells[MrowCellKeys.CarOfChartererCd.ToString()].Value);
            //    int carKbn = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(curRowIndex, MrowCellKeys.CarKbn.ToString()));

            //    PastPriceDefaultParameters defpara = new PastPriceDefaultParameters();
            //    if (0 < HanroCd)
            //    {
            //        defpara.HanroCode = HanroCd;
            //    }
            //    if (0 < TokuisakiCd)
            //    {
            //        defpara.TokuisakiCode = TokuisakiCd;
            //    }
            //    if (0 < StartPointCd)
            //    {
            //        defpara.StartPointCode = StartPointCd;
            //    }
            //    if (0 < EndPointCd)
            //    {
            //        defpara.EndPointCode = EndPointCd;
            //    }
            //    if (0 < ItemCd)
            //    {
            //        defpara.ItemCode = ItemCd;
            //    }
            //    if (0 < FigCd)
            //    {
            //        defpara.FigCode = FigCd;
            //    }
            //    if (0 < CarKindCd)
            //    {
            //        defpara.CarKindCode = CarKindCd;
            //    }
            //    if (0 < TorihikiCd)
            //    {
            //        defpara.TorihikiCode = TorihikiCd;
            //    }

            //    defpara.CarKbn = carKbn;

            //    using (CmnSearchPastPriceFrame f = new CmnSearchPastPriceFrame() { PastPriceDefaultParameters = defpara })
            //    {
            //        f.InitFrame();
            //        f.ShowDialog();

            //        if (f.DialogResult == DialogResult.OK)
            //        {
            //            //単価の値を反映させるため、一時的にフォーカスを外す
            //            this.cmbBranchOffice.Focus();

            //            isDisable = f.SelectedInfo.DisableFlag;
            //            if (!isDisable)
            //            {
            //                decimal number = decimal.Zero;
            //                decimal price = decimal.Zero;
            //                decimal tollFee = decimal.Zero;
            //                decimal futaigyomuryo = decimal.Zero;

            //                //単価
            //                this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.AtPrice.ToString(), f.SelectedInfo.AtPrice);

            //                //数量
            //                number = f.SelectedInfo.Number;
            //                this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.Number.ToString(), number);

            //                //単位IDが存在する場合
            //                if (0 < f.SelectedInfo.FigId)
            //                {
            //                    //単位コード
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.FigCd.ToString(), f.SelectedInfo.FigCode);
            //                    //単位ID
            //                    this.mrsJuchuMeisai.Rows[curRowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = f.SelectedInfo.FigId;
            //                    //単位名
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.FigNM.ToString(), f.SelectedInfo.FigName);
            //                }

            //                //金額
            //                price = f.SelectedInfo.PriceInPrice;
            //                this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.Price.ToString(), price);

            //                //通行料
            //                tollFee = f.SelectedInfo.TollFeeInPrice;
            //                this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.TollFeeInPrice.ToString(), tollFee);

            //                //トラDON_V40以外の場合
            //                if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
            //                    != (int)DefaultProperty.TraDonVersionKbn.V40)
            //                {
            //                    //付帯業務料
            //                    futaigyomuryo = f.SelectedInfo.FutaigyomuryoInPrice;
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString(), futaigyomuryo);
            //                }

            //                //傭車の場合
            //                if (carKbn == (int)DefaultProperty.CarKbn.Yosha)
            //                {
            //                    //傭車金額
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.CharterPrice.ToString(), f.SelectedInfo.PriceInCharterPrice);

            //                    //傭車通行料
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString(), f.SelectedInfo.TollFeeInCharterPrice);
            //                }

            //                //重量計算
            //                //商品
            //                ItemSearchParameter item_para = new ItemSearchParameter();
            //                item_para.ToraDONItemCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(curRowIndex, MrowCellKeys.ItemCd.ToString()));
            //                ItemInfo itemInfo = this._DalUtil.Item.GetList(item_para).FirstOrDefault();

            //                if (itemInfo != null)
            //                {
            //                    //重量計算
            //                    decimal wk_weight = 0;

            //                    if (this.CalcWeight(number, itemInfo.ToraDONWeight, out wk_weight))
            //                    {
            //                        if (wk_weight != 0)
            //                        {
            //                            //重量セット
            //                            this.mrsJuchuMeisai.SetValue(
            //                                curRowIndex, MrowCellKeys.Weight.ToString(), wk_weight);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        //クリア
            //                        this.mrsJuchuMeisai.SetValue(
            //                            curRowIndex, MrowCellKeys.Weight.ToString(), decimal.Zero);
            //                    }
            //                }

            //                //乗務員売上同額フラグがチェックされた場合
            //                if ((bool)this.mrsJuchuMeisai.GetValue(curRowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString()))
            //                {
            //                    //金額を乗務員売上金額に設定
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(),
            //                        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(curRowIndex, MrowCellKeys.Price.ToString())));
            //                }

            //                //金額によって確定フラグ変更
            //                bool fixflag = false;
            //                if (price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                {
            //                    fixflag = true;
            //                }
            //                this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                //車両区分が傭車の場合
            //                if (carKbn == (int)BizProperty.DefaultProperty.CarKbn.Yosha)
            //                {
            //                    //傭車確定フラグ
            //                    bool wk_charterfixflag = false;
            //                    if (f.SelectedInfo.PriceInCharterPrice != 0 || f.SelectedInfo.TollFeeInCharterPrice != 0)
            //                    {
            //                        wk_charterfixflag = true;
            //                    }
            //                    this.mrsJuchuMeisai.SetValue(curRowIndex, MrowCellKeys.CharterFixFlag.ToString(), wk_charterfixflag);
            //                }

            //                //合計金額を再計算
            //                this.SetPriceTotal();
            //            }

            //            //フォーカスを戻す
            //            this.mrsJuchuMeisai.Focus();

            //            this.OnCmnSearchComplete();

            //            // 最終行の場合は行追加
            //            if (curRowIndex == this.mrsJuchuMeisai.RowCount - 1)
            //            {
            //                //最終行へ行追加
            //                this.mrsJuchuMeisai.NotifyCurrentCellDirty(true);
            //            }
            //        }
            //    }
            //}

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
        /// DateTimeCellコントロールのスピンアップ処理。
        /// </summary>
        /// <param name="curMRow"></param>
        private void SpinUpJuchuNyuryokuList(GcMultiRow curMRow)
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

            try
            {
                DateTime EditingDateTime = DateTime.MinValue;
                DateTime NewDateTime = DateTime.MinValue;

                // 積日の場合
                if (curCell.Name == MrowCellKeys.TaskStartYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 積時間の場合
                else if (curCell.Name == MrowCellKeys.TaskStartHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 着日の場合
                else if (curCell.Name == MrowCellKeys.TaskEndYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 着時間の場合
                else if (curCell.Name == MrowCellKeys.TaskEndHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 再使用可能日の場合
                else if (curCell.Name == MrowCellKeys.ReuseYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 再使用可能時間の場合
                else if (curCell.Name == MrowCellKeys.ReuseHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 計上日の場合
                else if (curCell.Name == MrowCellKeys.AddUpYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 傭車計上日の場合
                else if (curCell.Name == MrowCellKeys.CharterAddUpYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(1);

                    curMRow.BeginEdit(NewDateTime);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// DateTimeCellコントロールのスピンアップ処理。
        /// </summary>
        /// <param name="curMRow"></param>
        private void SpinDownJuchuNyuryokuList(GcMultiRow curMRow)
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

            try
            {
                DateTime EditingDateTime = DateTime.MinValue;
                DateTime NewDateTime = DateTime.MinValue;

                // 積日の場合
                if (curCell.Name == MrowCellKeys.TaskStartYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 積時間の場合
                else if (curCell.Name == MrowCellKeys.TaskStartHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 着日の場合
                else if (curCell.Name == MrowCellKeys.TaskEndYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 着時間の場合
                else if (curCell.Name == MrowCellKeys.TaskEndHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 再使用可能日の場合
                else if (curCell.Name == MrowCellKeys.ReuseYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 再使用可能時間の場合
                else if (curCell.Name == MrowCellKeys.ReuseHM.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddHours(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 計上日の場合
                else if (curCell.Name == MrowCellKeys.AddUpYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(-1);

                    curMRow.BeginEdit(NewDateTime);
                }

                // 傭車計上日の場合
                else if (curCell.Name == MrowCellKeys.CharterAddUpYMD.ToString())
                {
                    EditingDateTime = Convert.ToDateTime(GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                    NewDateTime = EditingDateTime.AddDays(-1);

                    curMRow.BeginEdit(NewDateTime);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            // チェンジイベント
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.cmbBranchOffice, ctl => ctl.Text, this.cmbBranchOffice_Validating));

            // コード検索
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numFukushaHanroGroupCode, ctl => ctl.Text, this.numSearchHanroGroupCode_Validating));

            this.mrsJuchuMeisai.RowsAdded += mrsJuchuMeisai_RowsAdded;
            this.mrsJuchuMeisai.CellEnter += mrsJuchuMeisai_CellEnter;
            this.mrsJuchuMeisai.CellLeave += mrsJuchuMeisai_CellLeave;
            this.mrsJuchuMeisai.EditingControlShowing += mrsRyokenUriageMeisaiList_EditingControlShowing;
            this.mrsJuchuMeisai.CellValidating += mrsJuchuMeisai_CellValidating;
        }

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {
            this.searchStateBinder = new SearchStateBinder(this);

            string[] SearchCells = new string[] { MrowCellKeys.HanroCd.ToString(),
                                                  MrowCellKeys.TokuisakiCd.ToString(),
                                                  MrowCellKeys.ClmClassCd.ToString(),
                                                  MrowCellKeys.ContractCd.ToString(),
                                                  MrowCellKeys.OwnerCd.ToString(),
                                                  MrowCellKeys.StartPointCd.ToString(),
                                                  MrowCellKeys.EndPointCd.ToString(),
                                                  MrowCellKeys.ItemCd.ToString(),
                                                  MrowCellKeys.FigCd.ToString(),
                                                  MrowCellKeys.CarCd.ToString(),
                                                  MrowCellKeys.CarKindCd.ToString(),
                                                  MrowCellKeys.DriverCd.ToString(),
                                                  MrowCellKeys.CarOfChartererCd.ToString(),
                                                  MrowCellKeys.JuchuTantoCd.ToString()};

            this.searchStateBinder.AddSearchableControls(this.mrsJuchuMeisai, SearchCells);
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

            //***編集取消
            _commandSet.EditCancel.Execute += EditCancel_Execute;
            _commandSet.Bind(
                _commandSet.EditCancel, this.btnCancel, actionMenuItems.GetMenuItemBy(ActionMenuItems.EditCancel), this.toolStripCancel);

            //***保存
            _commandSet.Save.Execute += Save_Execute;
            _commandSet.Bind(
                _commandSet.Save, this.btnSave, actionMenuItems.GetMenuItemBy(ActionMenuItems.Update), this.toolStripSave);

            //***全明細削除
            _commandSet.DeleteAllDtl.Execute += DeleteAllDtl_Execute;
            _commandSet.Bind(
                _commandSet.DeleteAllDtl, this.btnDeleteAllDtl, actionMenuItems.GetMenuItemBy(ActionMenuItems.DeleteAllDtl), this.toolStripDeleteAllDtl);

            //***終了
            _commandSet.Close.Execute += Close_Execute;
            _commandSet.Bind(
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripClose);
        }

        void EditCancel_Execute(object sender, EventArgs e)
        {
            this.DoClear(true);
        }

        void Save_Execute(object sender, EventArgs e)
        {
            this.DoUpdate();
        }

        void Close_Execute(object sender, EventArgs e)
        {
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

        void DeleteAllDtl_Execute(object sender, EventArgs e)
        {
            this.DoDelData();
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

        private void mrsJuchuMeisai_CellValidating(object sender, CellValidatingEventArgs e)
        {
            this.ProcessJuchuNyuryokuMeisaiMrowCellValidating(e);
        }

        private void mrsRyokenUriageMeisaiList_EditingControlShowing(object sender, EditingControlShowingEventArgs e)
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
                sideButton.Click -= new EventHandler(mrsJuchuNyuryokuListSideButton_Click);
                sideButton.Click += new EventHandler(mrsJuchuNyuryokuListSideButton_Click);
                e.Control.DoubleClick -= new EventHandler(mrsJuchuNyuryokuListSideButton_Click);
                e.Control.DoubleClick += new EventHandler(mrsJuchuNyuryokuListSideButton_Click);
            }

            //スピンボタンが存在する場合スピンボタンイベントを設定。
            GrapeCity.Win.Editors.SpinButton spinButton = null;
            if (e.Control is GcDateTimeEditingControl && (e.Control as GcDateTimeEditingControl).SideButtons.Count != 0)
            {
                spinButton = (e.Control as GcDateTimeEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SpinButton;
            }
            if (null != spinButton)
            {
                spinButton.SpinUp -= new EventHandler(mrsJuchuNyuryokuListSpinButton_SpinUp);
                spinButton.SpinUp += new EventHandler(mrsJuchuNyuryokuListSpinButton_SpinUp);
                spinButton.SpinDown -= new EventHandler(mrsJuchuNyuryokuListSpinButton_SpinDown);
                spinButton.SpinDown += new EventHandler(mrsJuchuNyuryokuListSpinButton_SpinDown);
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

        private void mrsJuchuNyuryokuListSideButton_Click(object sender, EventArgs e)
        {
            this.ShowCmnSearchJuchuNyuryokuList(this.mrsJuchuMeisai);
        }

        private void mrsJuchuNyuryokuListSpinButton_SpinUp(object sender, EventArgs e)
        {
            this.SpinUpJuchuNyuryokuList(this.mrsJuchuMeisai);
        }
        private void mrsJuchuNyuryokuListSpinButton_SpinDown(object sender, EventArgs e)
        {
            this.SpinDownJuchuNyuryokuList(this.mrsJuchuMeisai);
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

        /// <summary>
        /// 受注明細のセルが新しいセルに移動した後のイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mrsJuchuMeisai_CellEnter(object sender, CellEventArgs e)
        {
            this.ProcessJuchuDtlCellEnter(e);
        }

        /// <summary>
        /// 伝票明細のCellEnterイベントを処理します。
        /// </summary>
        private void ProcessJuchuDtlCellEnter(CellEventArgs e)
        {
            if (e.Scope != CellScope.Row)
            {
                return;
            }

            //現在の行
            Row currentRow = this.mrsJuchuMeisai.CurrentRow;
            //現在のセル
            Cell currentCell = this.mrsJuchuMeisai.CurrentCell;
            //現在のセルポジション
            CellPosition currentCellPosition = this.mrsJuchuMeisai.CurrentCellPosition;
            //現在のセルキー
            string wk_curcellkey = currentCell.Name;

            //セルがロックされているか？
            if (currentCell.ReadOnly)
            {
                //次に移動するセル
                var cellPosition = CellPosition.Empty;

                if (
                    CellMovingOrderIsGreater(
                    this.mrsJuchuMeisai, currentCellPosition, this._juchuDtlListCellPositionAtCellLeave))
                {
                    cellPosition =
                        FindNextMovingOrderCellAsCellPosition(
                        this.mrsJuchuMeisai, currentCellPosition, cell => !cell.ReadOnly);
                }
                else
                {
                    cellPosition =
                        FindPreviousMovingOrderCellAsCellPosition(
                        this.mrsJuchuMeisai, currentCellPosition, cell => !cell.ReadOnly);
                }

                if (!cellPosition.IsEmpty)
                {
                    this.mrsJuchuMeisai.CurrentCellPosition = cellPosition;
                }
                else
                {
                    this.mrsJuchuMeisai.CurrentCellPosition = this._juchuDtlListCellPositionAtCellLeave;
                }
            }
            else
            {
                //金額 or 傭車金額　税区分ショートカットガイドを活性化（新規行かどうかに関係しないのでここに記述する）
                if (wk_curcellkey == MrowCellKeys.Price.ToString() ||
                    wk_curcellkey == MrowCellKeys.CharterPrice.ToString())
                {
                    this.toolStripTaxKbn.Enabled = true;
                }

                //新規行の場合は処理しない
                if (currentRow.IsNewRow)
                {
                    return;
                }

                //日付フラグ
                bool dateFlag = false;

                //積日
                if (wk_curcellkey == MrowCellKeys.TaskStartYMD.ToString())
                {
                    this.SetInitValueInTaskStartDateCell(e.RowIndex);
                    dateFlag = true;
                }

                //着日
                if (wk_curcellkey == MrowCellKeys.TaskEndYMD.ToString())
                {
                    this.SetInitValueInTaskEndDateCell(e.RowIndex);
                    dateFlag = true;
                }

                //再使用可能日
                if (wk_curcellkey == MrowCellKeys.ReuseYMD.ToString())
                {
                    this.SetInitValueInReuseDateCell(e.RowIndex);
                    dateFlag = true;
                }

                //計上日
                if (wk_curcellkey == MrowCellKeys.AddUpYMD.ToString())
                {
                    this.SetInitValueInAddUpDateCell(e.RowIndex);
                    dateFlag = true;
                }

                //傭車計上日
                if (wk_curcellkey == MrowCellKeys.CharterAddUpYMD.ToString())
                {
                    this.SetInitValueInCharterAddUpDateCell(e.RowIndex);
                    dateFlag = true;
                }

                // 日付の場合
                if (dateFlag)
                {
                    // 全選択しない
                    this.mrsJuchuMeisai.BeginEdit(false);
                }
                else
                {
                    // 全選択する
                    this.mrsJuchuMeisai.BeginEdit(true);
                }
            }
        }

        /// <summary>
        /// 受注明細のCellLeaveイベントハンドラです。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mrsJuchuMeisai_CellLeave(object sender, CellEventArgs e)
        {
            this._juchuDtlListCellPositionAtCellLeave = this.mrsJuchuMeisai.CurrentCellPosition;

            if (e.Scope != CellScope.Row)
            {
                return;
            }

            //現在の行
            Row currentRow = this.mrsJuchuMeisai.CurrentRow;
            //現在のセル
            Cell currentCell = this.mrsJuchuMeisai.CurrentCell;
            //現在のセルポジション
            CellPosition currentCellPosition = this.mrsJuchuMeisai.CurrentCellPosition;
            //現在のセルキー
            string wk_curcellkey = currentCell.Name;

            //金額 or 傭車金額から抜けたらショートカットキーガイドを非活性にする
            if (e.CellName == MrowCellKeys.Price.ToString() ||
                    e.CellName == MrowCellKeys.CharterPrice.ToString())
            {
                this.toolStripTaxKbn.Enabled = false;
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
                //MRSの編集中のセルを確定させる
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

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
                            this._JuchuNyuryoku.Save(tx, this._JuchuInfoUpdList);
                            // 削除
                            this._JuchuNyuryoku.Delete(tx, this._JuchuInfoDelList);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "受注明細登録", _OriginJuchuNo.ToString() });

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
                            this.edtLastEntryJuchuSlipNo.Text = this._OriginJuchuNo.ToString();
                        }
                        else if (FrameEditMode.Editable == this.currentMode)
                        {
                            //一覧画面に戻った際に再検索を行う。
                            this.isReDraw = true;

                            // 画面を閉じる
                            this.isConfirmClose = false;
                            this.DoClose();
                        }

                    }
                    catch (CanRetryException ex)
                    {
                        //データがない場合の例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
                        //フォーカスを移動
                        this.mrsJuchuMeisai.Focus();
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
                        this.mrsJuchuMeisai.Focus();
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

            //MRSの編集中のセルを確定させる
            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            if (this.ValidateChildren(ValidationConstraints.None))
            {
                //画面から値を取得
                this.GetScreen();

                if (this.CheckDeleteInputs(this._JuchuInfoUpdList))
                {
                    try
                    {
                        //保存
                        SQLHelper.ActionWithTransaction(tx =>
                        {
                            this._JuchuNyuryoku.Delete(tx, this._JuchuInfoUpdList);
                        });

                        //操作ログ(保存)の条件取得
                        string log_jyoken = FrameUtilites.GetDefineLogMessage(
                            "C10002", new string[] { "受注明細削除", _OriginJuchuNo.ToString() });

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

                        // 画面を閉じる
                        this.isConfirmClose = false;
                        this.DoClose();

                    }
                    catch (CanRetryException ex)
                    {
                        //データがない場合の例外ハンドラ
                        FrameUtilites.ShowExceptionMessage(ex, this);
                        //フォーカスを移動
                        this.mrsJuchuMeisai.Focus();
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
                        this.mrsJuchuMeisai.Focus();
                    }
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
        /// 選択中の明細を１行削除します。
        /// </summary>
        private void DoMRowLineRemove(GcMultiRow curMRow)
        {
            //現在の行の位置を取得しておく
            GrapeCity.Win.MultiRow.Row curRow = curMRow.CurrentRow;

            //現在行が最後の行の時は削除は許可しない(自動行挿入可の場合)
            //自動行挿入不可の場合はNoチェック
            if (curRow != null && this.mrsJuchuMeisai.RowCount - 1 != curRow.Index)
            {
                //処理前に確認メッセージ
                DialogResult wk_result =
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MQ2102005"),
                        "確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button2);

                if (wk_result == DialogResult.Yes)
                {
                    //削除可否
                    bool delFlag = false;

                    //削除する受注情報が登録済なら受注情報を取得
                    if (null != curRow.Tag)
                    {
                        JuchuNyuryokuInfo info = (JuchuNyuryokuInfo)curRow.Tag;
                        List<JuchuNyuryokuInfo> list = new List<JuchuNyuryokuInfo>();
                        list.Add(info);

                        //削除予定受注に紐付く配車データの存在確認
                        if (this._JuchuNyuryoku.ExistsHaishaInfo(list))
                        {
                            //再確認メッセージ
                            wk_result =
                                MessageBox.Show(
                                    "既に配車データが作成されています。\r\n削除してもよろしいですか？",
                                    "確認",
                                    MessageBoxButtons.YesNo,
                                    MessageBoxIcon.Warning,
                                    MessageBoxDefaultButton.Button2);
                        };

                        //TODO 削除可能かの排他管理チェックをここに実装する！

                        if (wk_result == DialogResult.Yes)
                        {
                            this._JuchuInfoDelList.Add(info);
                            delFlag = true;
                        }
                    }
                    else
                    {
                        delFlag = true;
                    }

                    if (delFlag)
                    {
                        //行を削除
                        this.mrsJuchuMeisai.Rows.RemoveAt(curRow.Index);
                        //合計金額を再計算
                        this.SetPriceTotal();
                    }
                }
            }
        }

        /// <summary>
        /// 選択中の明細をコピーします。
        /// </summary>
        /// <param name="curMrow"></param>
        private void DoMRowAddCopy(GcMultiRow curMrow)
        {
            GrapeCity.Win.MultiRow.Row curRow = curMrow.CurrentRow;

            if (curRow.IsNewRow && curRow.Index > 0)
            {
                string gyoStr = FUKUSHA_DEFAULT_STR;
                int gyoInt = curRow.Index;

                //複写行が有効行の場合
                if (0 < Convert.ToInt32(this.numFukushaMotoNo.Value)
                    && Convert.ToInt32(this.numFukushaMotoNo.Value)
                    <= Convert.ToInt32(curMrow.Rows[curRow.Index - 1].Cells[MrowCellKeys.RowNo.ToString()].FormattedValue))
                {
                    gyoStr = "No." + Convert.ToInt32(this.numFukushaMotoNo.Value).ToString();
                    gyoInt = Convert.ToInt32(this.numFukushaMotoNo.Value);
                }

                //コピー元行が非活性の場合
                if (!curMrow.Rows[gyoInt - 1].Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                {
                    //何もしない
                    return;
                }

                DialogResult c_result =
                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                        "MQ2102026", new string[] { gyoStr }),
                    "確認",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);

                if (c_result == DialogResult.Yes)
                {
                    int indexDestination = curRow.Index;

                    curMrow.Rows.InsertCopy(gyoInt - 1, indexDestination);
                    curMrow.Rows[indexDestination].Tag = null;

                    //日付を複写しない場合
                    if (!this.chkHizukeFukushaFlag.Checked)
                    {
                        //日付削除
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.TaskStartYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.TaskStartHM.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.TaskEndYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.TaskEndHM.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.ReuseYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.ReuseHM.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.AddUpYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(indexDestination, MrowCellKeys.CharterAddUpYMD.ToString(), null);
                    }

                    if (curRow.Index != curMrow.Rows.Count - 1)
                    {
                        curMrow.Rows.RemoveAt(curRow.Index);
                    }

                    this.SetPriceTotal();
                }
            }
        }

        /// <summary>
        /// 画面の値をコントローラーにセットします。
        /// </summary>
        private void GetScreen()
        {
            //受注明細
            this.GetJuchuDtlList();
        }

        /// <summary>
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckInputs()
        {
            //戻り値用
            bool rt_val = true;

            if (rt_val)
            {
                //集計済みの受注明細が存在する場合は、処理中断
                if (this.ContainsTotaledJuchuDtl())
                {
                    rt_val = false;
                }
            }

            if (rt_val)
            {
                //ヘッダ部入力項目のチェック
                rt_val = this.CheckHeaderInputs();
            }

            if (rt_val)
            {
                //受注明細のチェック
                rt_val = this.CheckJuchuDtl();
            }

            return rt_val;
        }

        /// <summary>
        /// 削除チェックします。
        /// </summary>
        /// <param name="juchuInfoList">削除明細</param>
        /// <returns>チェック結果</returns>
        private bool CheckDeleteInputs(List<JuchuNyuryokuInfo> juchuInfoList)
        {
            //戻り値用
            bool rt_val = true;

            //削除明細が存在する場合
            if (juchuInfoList != null && 0 < juchuInfoList.Count)
            {
                //削除予定受注に紐付く配車データの存在確認
                if (this._JuchuNyuryoku.ExistsHaishaInfo(juchuInfoList))
                {
                    //確認メッセージ
                    DialogResult wk_result =
                        MessageBox.Show(
                            "配車データが既に作成されている明細が存在します。\r\n削除してもよろしいですか？",
                            "確認",
                            MessageBoxButtons.YesNo,
                            MessageBoxIcon.Warning,
                            MessageBoxDefaultButton.Button2);

                    if (wk_result == DialogResult.No)
                    {
                        rt_val = false;
                    }
                    else
                    {
                        //TODO 削除可能かの排他管理チェックをここに実装する！
                    };
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 集計済みの受注明細が存在するかチェックします。
        /// 存在する場合は true を返します。
        /// </summary>
        /// <returns></returns>
        private bool ContainsTotaledJuchuDtl()
        {
            //戻り値用
            bool rt_val = false;

            string msg = string.Empty;
            int mrowidx = -1;

            //最終月次集計月を取得しておく
            DateTime last_sum_of_monthday = this._ToraDonSystemPropertyInfo.LastSummaryOfMonthDate;

            //受注明細件数を取得しておく
            int rowcount = this.mrsJuchuMeisai.RowCount;

            //①読込み時のデータは伝票単位で行う
            if (this._OriginJuchuNo != 0)
            {
                // 読込み時に既に集計済みである明細の受注ID一覧を取得
                List<decimal> excluded_list = this.GetJuchuIdListOfUnchangeJuchuDtlRow();

                //TODO 性能問題のためコメント化中 START
                //// 請求済みチェック
                //if (!rt_val && this._JuchuNyuryoku.ContainsClmTotaledJuchu(this._OriginJuchuNo, excluded_list))
                //{
                //    rt_val = true;
                //    msg = FrameUtilites.GetDefineMessage("ME2303011");
                //}

                //// 支払集計済みチェック TODO TORADON_Saleからの実装漏れ！（性能問題でコメント化中だが。。。）
                //if (!rt_val && this._JuchuNyuryoku.ContainsPayTotaledJuchu(this._OriginJuchuNo, excluded_list))
                //{
                //    rt_val = true;
                //    msg = FrameUtilites.GetDefineMessage("ME2303012");
                //}
                //TODO 性能問題のためコメント化中 END

                // 月次締処理済みチェック
                if (!rt_val && this._JuchuNyuryoku.ContainsMonthlyTotaledJuchu(this._OriginJuchuNo, excluded_list, last_sum_of_monthday))
                {
                    rt_val = true;
                    msg = FrameUtilites.GetDefineMessage("ME2303013",
                        new string[] { last_sum_of_monthday.ToString("yyyy/MM/dd") });
                }
            }

            //②明細上に入力されたデータでチェック
            if (!rt_val)
            {
                //リストを取る
                for (int i = 0; i < rowcount; i++)
                {
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[i];
                    //明細行が活性の場合
                    if (this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                    {
                        //得意先の最終請求日を取得しておく
                        DateTime last_clmday = DateTime.MinValue;
                        this._JuchuNyuryoku.GetLastClmFixDayOfClmByTokuisaki(
                            Convert.ToInt32(
                                this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiCd.ToString())),
                            out last_clmday);

                        int carOfChartererCode =
                            Convert.ToInt32(
                                this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CarOfChartererCd.ToString()));

                        //傭車先の最終締切日を取得しておく
                        DateTime last_payFixDay = DateTime.MinValue;
                        //傭車先が設定されている場合のみ取得するのでその判定
                        if (carOfChartererCode > 0)
                        {
                            this._JuchuNyuryoku.GetLastPaymentFixDayByTorihikisaki(
                               carOfChartererCode,
                               out last_payFixDay);
                        }

                        //計上日を取得
                        DateTime addupdate =
                            Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.AddUpYMD.ToString()));

                        //確定フラグを取得
                        bool fixflag =
                            Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.FixFlag.ToString()));

                        //傭車計上日を取得
                        DateTime charter_addupdate =
                            Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterAddUpYMD.ToString()));

                        //傭車確定フラグを取得
                        bool charter_fixflag =
                            Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterFixFlag.ToString()));

                        //TODO 性能問題のためコメント化中 START
                        ////請求済みチェック
                        //if (wk_mrow.Tag != null &&
                        //    ((JuchuNyuryokuInfo)wk_mrow.Tag).MinClmFixDate != DateTime.MinValue)//読込の時点で請求zみのの場合は日付を変更できないので入力チェックはスキップ
                        //{
                        //    //なにもしない
                        //}
                        //else if (addupdate != DateTime.MinValue && addupdate <= last_clmday && fixflag)
                        //{
                        //    rt_val = true;
                        //    msg = FrameUtilites.GetDefineMessage("ME2303014",
                        //        new string[] { last_clmday.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                        //    mrowidx = i;
                        //    break;
                        //}

                        ////支払済みチェック
                        //if (wk_mrow.Tag != null &&
                        //    ((JuchuNyuryokuInfo)wk_mrow.Tag).MinCharterPayFixDate != DateTime.MinValue)//読込の時点で支払済みの場合は日付を変更できないので入力チェックはスキップ
                        //{
                        //    //なにもしない
                        //}
                        //else if (charter_addupdate != DateTime.MinValue && charter_addupdate <= last_payFixDay && charter_fixflag)
                        //{
                        //    rt_val = true;
                        //    msg = FrameUtilites.GetDefineMessage("ME2303015",
                        //        new string[] { last_payFixDay.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                        //    mrowidx = i;
                        //    break;
                        //}
                        //請求済みチェック
                        if (addupdate != DateTime.MinValue && addupdate <= last_clmday && fixflag)
                        {
                            rt_val = true;
                            msg = FrameUtilites.GetDefineMessage("ME2303014",
                                new string[] { last_clmday.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                            mrowidx = i;
                            break;
                        }

                        //支払済みチェック
                        if (charter_addupdate != DateTime.MinValue && charter_addupdate <= last_payFixDay && charter_fixflag)
                        {
                            rt_val = true;
                            msg = FrameUtilites.GetDefineMessage("ME2303015",
                                new string[] { last_payFixDay.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                            mrowidx = i;
                            break;
                        }
                        //TODO 性能問題のためコメント化中 END

                        //月次締処理済みチェック
                        if (addupdate != DateTime.MinValue &&
                            addupdate <= last_sum_of_monthday && fixflag)
                        {
                            rt_val = true;
                            msg = FrameUtilites.GetDefineMessage("ME2303016",
                                new string[] { last_sum_of_monthday.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                            mrowidx = i;
                            break;
                        }

                        if (charter_addupdate != DateTime.MinValue &&
                            charter_addupdate <= last_sum_of_monthday && charter_fixflag)
                        {
                            rt_val = true;
                            msg = FrameUtilites.GetDefineMessage("ME2303016",
                                new string[] { last_sum_of_monthday.ToString("yyyy/MM/dd"), (i + 1).ToString() });
                            mrowidx = i;
                            break;
                        }
                    }
                }
            }

            //エラー処理
            if (rt_val)
            {
                MessageBox.Show(msg, "警告",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                if (mrowidx >= 0)
                {
                    //カーソル設定行の指定がある場合のみ、カーソル位置指定
                    if (this.mrsJuchuMeisai[mrowidx, MrowCellKeys.HanroCd.ToString()].Selectable)
                    {
                        this.mrsJuchuMeisai.CurrentCellPosition =
                            new CellPosition(mrowidx, MrowCellKeys.HanroCd.ToString());
                    }
                }

                this.mrsJuchuMeisai.Focus();
            }

            return rt_val;
        }

        /// <summary>
        /// 画面から編集不可能な受注明細の受注Idを取得します。
        /// </summary>
        /// <returns></returns>
        private List<decimal> GetJuchuIdListOfUnchangeJuchuDtlRow()
        {
            return
                this.GetJuchuListOfUnchangeJuchuDtlRow().Select(inf => inf.JuchuId).ToList();
        }

        /// <summary>
        /// 画面から編集不可能な受注明細の受注情報を取得します。
        /// </summary>
        /// <returns></returns>
        private List<JuchuNyuryokuInfo> GetJuchuListOfUnchangeJuchuDtlRow()
        {
            return
                this.mrsJuchuMeisai.Rows
                .Where(row => !row.Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                .Select(row => row.Tag as JuchuNyuryokuInfo)
                .ToList();
        }

        /// <summary>
        /// ヘッダ部の入力項目をチェックします。
        /// 問題がない場合は true を返します。
        /// </summary>
        /// <returns>入力チェック結果</returns>
        private bool CheckHeaderInputs()
        {
            //戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;
            Control ctl = null;

            //営業所の必須チェック
            if (rt_val && Convert.ToInt32(this.cmbBranchOffice.SelectedValue) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "営業所" });
                icon = MessageBoxIcon.Warning;
                ctl = this.cmbBranchOffice;
            }

            if (rt_val)
            {
                //期首年月の取得
                DateTime account_startdate =
                    this._ToraDonSystemPropertyInfo.AccountStartDate;
                //先行年月の取得
                DateTime input_maxdate =
                    NSKUtil.MonthLastDay(this._ToraDonSystemPropertyInfo.InputMaxDate);
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
        /// 受注明細の登録対象項目をチェックします。
        /// 問題がない場合は true を返します。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckJuchuDtl()
        {
            //戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            int mrowidx = 0;

            MrowCellKeys colidx = MrowCellKeys.TokuisakiCd;

            //請求部門未入力の警告メッセージを表示した得意先コードのリスト
            List<int> ccQuestionMsgShowedTokuisakiCodeList = new List<int>();

            //チェックする明細数を取得（自動行挿入に設定している場合は新規行分を除く）
            int rowcount = this.mrsJuchuMeisai.RowCount;

            if (this.mrsJuchuMeisai.AllowUserToAddRows)
            {
                rowcount -= 1;
            }

            if (rowcount == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2203002");
            }

            //for (int i = 0; i < rowcount; i++)
            //{
            //    //行追加可能、かつ、明細行が活性の場合
            //    if (this.mrsJuchuMeisai.AllowUserToAddRows && this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
            //    {
            //        //得意先コードの必須チェック
            //        if (Convert.ToInt32(
            //            this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiCd.ToString())) == 0)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "得意先コード" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TokuisakiCd;
            //            break;
            //        }

            //        //得意先名の必須チェック
            //        if (Convert.ToString(
            //            this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiNM.ToString())).Trim().Length == 0)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "得意先名" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TokuisakiNM;
            //            break;
            //        }

            //        // 部門管理する得意先の場合に、請求部門が未入力だったら処理続行の確認メッセージ
            //        // 得意先情報取得
            //        int tokuisakicd =
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiCd.ToString()));

            //        // 得意先情報を取得
            //        TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisakicd };
            //        TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //        // 部門管理区分取得
            //        bool clmclass_usekbn = false;
            //        if (tokui_info != null)
            //        {
            //            clmclass_usekbn = NSKUtil.IntToBool(tokui_info.ToraDONClmClassUseKbn);
            //        }

            //        // 請求部門コード取得
            //        int clmclasscd =
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ClmClassCd.ToString()));

            //        //未チェックの得意先かつ、請求部門が未入力かどうか
            //        if (!ccQuestionMsgShowedTokuisakiCodeList.Any(cd => cd == tokuisakicd) &&
            //            clmclass_usekbn && clmclasscd == 0)
            //        {
            //            string name = string.Empty;
            //            if (tokui_info != null)
            //            {
            //                name = tokui_info.ToraDONTokuisakiShortName;
            //            }

            //            DialogResult wk_result =
            //                MessageBox.Show(
            //                FrameUtilites.GetDefineMessage("MQ2102028", new string[] { name }),
            //                    "確認",
            //                    MessageBoxButtons.YesNo,
            //                    MessageBoxIcon.Question,
            //                    MessageBoxDefaultButton.Button2);

            //            if (wk_result == DialogResult.No)
            //            {
            //                //Noの場合は処理中断
            //                rt_val = false;
            //                mrowidx = i;
            //                colidx = MrowCellKeys.ClmClassCd;
            //                break;
            //            }

            //            ccQuestionMsgShowedTokuisakiCodeList.Add(tokuisakicd);
            //        }

            //        //品目コードの必須チェック
            //        if (this._itemIsRequired && 
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ItemCd.ToString())) == 0)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "品目コード" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.ItemCd;
            //            break;
            //        }

            //        int carKbn =
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CarKbn.ToString()));

            //        //車両情報を取得
            //        CarSearchParameter car_para = new CarSearchParameter();
            //        car_para.ToraDONCarCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CarCd.ToString()));
            //        CarInfo car_info = this._DalUtil.Car.GetList(car_para).FirstOrDefault();

            //        //車両が廃車かどうか
            //        if (rt_val && this.CarIsScrapedOnJuchuList(i, car_info))
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203061", new string[] { 
            //                        car_info.LicPlateCarNo, 
            //                        car_info.ScrapDate.ToString("yyyy/MM/dd") });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.CarCd;
            //            break;
            //        }

            //        //乗務員情報を取得
            //        StaffSearchParameter driver_para = new StaffSearchParameter();
            //        driver_para.ToraDONStaffCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.DriverCd.ToString()));
            //        StaffInfo driver_info = this._DalUtil.Staff.GetList(driver_para).FirstOrDefault();

            //        //乗務員が廃車かどうか
            //        if (rt_val && this.DriverIsRetiredOnJuchuList(i, driver_info))
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203062", new string[] { 
            //                        driver_info.ToraDONStaffName, 
            //                        driver_info.ToraDONRetireDate.ToString("yyyy/MM/dd") });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.DriverCd;
            //            break;
            //        }

            //        //期首年月保持用
            //        DateTime account_startdate = this._ToraDonSystemPropertyInfo.AccountStartDate;
            //        //先行年月保持用
            //        DateTime input_maxdate =
            //            NSKUtil.MonthLastDay(this._ToraDonSystemPropertyInfo.InputMaxDate);

            //        //積日の必須チェック
            //        DateTime start_date =
            //            Convert.ToDateTime(
            //                this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskStartYMD.ToString())).Date;
            //        if (start_date == DateTime.MinValue)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "積日" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TaskStartYMD;
            //            break;
            //        }

            //        //積日の有効範囲チェック
            //        if (start_date != DateTime.MinValue)
            //        {

            //            if (start_date < account_startdate || start_date > input_maxdate)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2202019", new string[] { "積日", 
            //                    account_startdate.ToString("yyyy/MM/dd"),
            //                    input_maxdate.ToString("yyyy/MM/dd") });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.TaskStartYMD;
            //                break;
            //            }
            //        }

            //        //着日の必須チェック
            //        DateTime end_date =
            //            Convert.ToDateTime(
            //                this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskEndYMD.ToString())).Date;
            //        if (end_date == DateTime.MinValue)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "着日" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TaskEndYMD;
            //            break;
            //        }

            //        //着日の有効範囲チェック
            //        if (end_date < account_startdate || end_date > input_maxdate)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2202019", new string[] { "着日", 
            //                    account_startdate.ToString("yyyy/MM/dd"),
            //                    input_maxdate.ToString("yyyy/MM/dd") });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TaskEndYMD;
            //            break;
            //        }

            //        // 積日
            //        DateTime DateTime_TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskStartYMD.ToString()));
            //        // 積時刻
            //        DateTime DateTime_TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskStartHM.ToString()));
            //        start_date = new DateTime(
            //            DateTime_TaskStartYMD.Year,
            //            DateTime_TaskStartYMD.Month,
            //            DateTime_TaskStartYMD.Day,
            //            DateTime_TaskStartHM.Hour,
            //            DateTime_TaskStartHM.Minute,
            //            0);

            //        // 着日
            //        DateTime DateTime_TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskEndYMD.ToString()));
            //        // 着時刻
            //        DateTime DateTime_TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskEndHM.ToString()));
            //        end_date = new DateTime(
            //            DateTime_TaskEndYMD.Year,
            //            DateTime_TaskEndYMD.Month,
            //            DateTime_TaskEndYMD.Day,
            //            DateTime_TaskEndHM.Hour,
            //            DateTime_TaskEndHM.Minute,
            //            0);

            //        //積日、着日の大小チェック
            //        if (start_date > end_date)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2202021", new string[] { "着日", "積日" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.TaskEndYMD;
            //            break;
            //        }

            //        //再使用可能日が入力されている場合
            //        if (DateTime.MinValue < Convert.ToDateTime(
            //                this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseYMD.ToString())))
            //        {
            //            DateTime reuse_date =
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseYMD.ToString())).Date;

            //            //再使用可能日の有効範囲チェック
            //            if (reuse_date < account_startdate || reuse_date > input_maxdate)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2202019", new string[] { "再使用可能日",
            //                    account_startdate.ToString("yyyy/MM/dd"),
            //                    input_maxdate.ToString("yyyy/MM/dd") });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.ReuseYMD;
            //                break;
            //            }

            //            // 再使用可能日
            //            DateTime DateTime_ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseYMD.ToString()));
            //            // 再使用可能時刻
            //            DateTime DateTime_ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseHM.ToString()));
            //            reuse_date = new DateTime(
            //                DateTime_ReuseYMD.Year,
            //                DateTime_ReuseYMD.Month,
            //                DateTime_ReuseYMD.Day,
            //                DateTime_ReuseHM.Hour,
            //                DateTime_ReuseHM.Minute,
            //                0);

            //            //着日、再使用可能日の大小チェック
            //            if (end_date > reuse_date)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2202021", new string[] { "再使用可能日", "着日" });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.ReuseYMD;
            //                break;
            //            }
            //        }

            //        //計上日の必須チェック
            //        DateTime addupdate =
            //            Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.AddUpYMD.ToString()));

            //        if (addupdate == DateTime.MinValue)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2203004", new string[] { "計上日" });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.AddUpYMD;
            //            break;
            //        }

            //        //金額
            //        decimal price = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Price.ToString()));
            //        //通行料
            //        decimal tollFeeInPrice = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInPrice.ToString()));
            //        //附帯業務料
            //        decimal futaigyomuryoInPrice = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.FutaigyomuryoInPrice.ToString()));
            //        //金額 + 通行料 + 附帯業務料
            //        decimal price_tollFeeInPrice = price + tollFeeInPrice + futaigyomuryoInPrice;
            //        //金額 + 通行料の範囲チェック
            //        if (price_tollFeeInPrice < PRICE_MINVALUE || price_tollFeeInPrice > PRICE_MAXVALUE)
            //        {
            //            rt_val = false;
            //            msg = FrameUtilites.GetDefineMessage(
            //                "MW2202019", new string[] { "受注金額と通行料、附帯業務料の合計", 
            //                    PRICE_MINVALUE.ToString("#,##0"),
            //                    PRICE_MAXVALUE.ToString("#,##0") });
            //            mrowidx = i;
            //            colidx = MrowCellKeys.Price;
            //            break;
            //        }

            //        //傭車の場合のみチェック
            //        if (carKbn ==
            //            (int)BizProperty.DefaultProperty.CarKbn.Yosha)
            //        {
            //            //傭車計上日の必須チェック
            //            DateTime charter_addupdate =
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterAddUpYMD.ToString()));

            //            if (charter_addupdate == DateTime.MinValue)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2203004", new string[] { "傭車計上日" });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.CharterAddUpYMD;
            //                break;
            //            }

            //            //傭車計上日の有効範囲チェック
            //            if (charter_addupdate < account_startdate || charter_addupdate > input_maxdate)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2202019", new string[] { "傭車計上日", 
            //                    account_startdate.ToString("yyyy/MM/dd"),
            //                    input_maxdate.ToString("yyyy/MM/dd") });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.CharterAddUpYMD;
            //                break;
            //            }

            //            //傭車金額
            //            decimal charterPrice = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterPrice.ToString()));
            //            //通行料
            //            decimal tollFeeInCharterPrice = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInCharterPrice.ToString()));
            //            //金額 + 通行料
            //            decimal charterPrice_tollFee = charterPrice + tollFeeInCharterPrice;
            //            //金額 + 通行料の範囲チェック
            //            if (charterPrice_tollFee < PRICE_MINVALUE || charterPrice_tollFee > PRICE_MAXVALUE)
            //            {
            //                rt_val = false;
            //                msg = FrameUtilites.GetDefineMessage(
            //                    "MW2202019", new string[] { "傭車金額と通行料の合計", 
            //                    PRICE_MINVALUE.ToString("#,##0"),
            //                    PRICE_MAXVALUE.ToString("#,##0") });
            //                mrowidx = i;
            //                colidx = MrowCellKeys.CharterPrice;
            //                break;
            //            }
            //        }
            //    }
            //}

            //if (!rt_val)
            //{
            //    //エラーメッセージ表示
            //    if (msg.Trim().Length > 0)
            //    {
            //        MessageBox.Show(
            //            msg, "警告",
            //            MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //    }

            //    //選択可能の場合のみフォーカスを遷移する
            //    if (this.mrsJuchuMeisai[mrowidx, colidx.ToString()].Selectable)
            //    {
            //        this.mrsJuchuMeisai.CurrentCellPosition =
            //            new CellPosition(mrowidx, colidx.ToString());
            //    }

            //    this.mrsJuchuMeisai.Focus();
            //}

            return rt_val;
        }

        ///// <summary>
        ///// 日報の車両を廃車扱いとするかどうかを取得します。
        ///// </summary>
        ///// <param name="carInfo">車両情報</param>
        ///// <returns></returns>
        //private bool CarIsScrapedOnJuchuList(int rowIndex, CarInfo carInfo)
        //{
        //    if (carInfo == null)
        //    {
        //        return false;
        //    }

        //    //比較対象の基になる画面値日付のリスト
        //    List<DateTime> vDateList = new List<DateTime>();

        //    Row row = this.mrsJuchuMeisai.Rows[rowIndex];
        //    vDateList.Add(Convert.ToDateTime(row.GetValue(MrowCellKeys.TaskStartYMD.ToString())));
        //    vDateList.Add(Convert.ToDateTime(row.GetValue(MrowCellKeys.TaskEndYMD.ToString())));

        //    //比較対象の日付
        //    List<DateTime> dateList =
        //         vDateList.Where(dt => dt > DateTime.MinValue).ToList();

        //    if (dateList.Count == 0)
        //    {
        //        return false;
        //    }

        //    //日付の最小値
        //    DateTime min = dateList.Min();

        //    //if (carInfo.ScrapDate > DateTime.MinValue &&
        //    //    carInfo.ScrapDate <= NSKUtil.MonthLastDay(min.AddMonths(-1)))
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

        ///// <summary>
        ///// 日報の乗務員を退職扱いとするかどうかを取得します。
        ///// </summary>
        ///// <param name="staffInfo">社員情報</param>
        ///// <returns></returns>
        //private bool DriverIsRetiredOnJuchuList(int rowIndex, StaffInfo staffInfo)
        //{
        //    if (staffInfo == null)
        //    {
        //        return false;
        //    }

        //    //比較対象の基になる画面値日付のリスト
        //    List<DateTime> vDateList = new List<DateTime>();

        //    Row row = this.mrsJuchuMeisai.Rows[rowIndex];
        //    vDateList.Add(Convert.ToDateTime(row.GetValue(MrowCellKeys.TaskStartYMD.ToString())));
        //    vDateList.Add(Convert.ToDateTime(row.GetValue(MrowCellKeys.TaskEndYMD.ToString())));

        //    //比較対象の日付
        //    List<DateTime> dateList =
        //         vDateList.Where(dt => dt > DateTime.MinValue).ToList();

        //    if (dateList.Count == 0)
        //    {
        //        return false;
        //    }
        //    //日付の最小値
        //    DateTime min = dateList.Min();

        //    //if (staffInfo.ToraDONRetireDate > DateTime.MinValue &&
        //    //    staffInfo.ToraDONRetireDate <= NSKUtil.MonthLastDay(min.AddMonths(-1)))
        //    //{
        //    //    return true;
        //    //}
        //    //else
        //    //{
        //    //    return false;
        //    //}
        //}

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
            // 受注情報取得
            this._JuchuInfoSelList = this._JuchuNyuryoku.GetJuchuInfoList(this._JuchuNyuryokuSearchParameter);

            // 0件なら処理しない
            if ((null == this._JuchuInfoSelList) || 0 == this._JuchuInfoSelList.Count)
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

            //ロックパターンを取得
            LockPattern lockpattern =
                this.GetMRowLockPatternJuchuDtlList();

            this._lockPattern = lockpattern;

            //ヘッダ部項目の編集制御
            this.ControlEditHeaderInputs(lockpattern);
        }

        /// <summary>
        /// ヘッダ部の入力項目の入力の可否の変更を一括して行います。
        /// </summary>
        /// <param name="lockpattern"></param>
        private void ControlEditHeaderInputs(LockPattern lockpattern)
        {
            System.Diagnostics.Debug.WriteLine(
                string.Format("ControlEditHeaderInputs!　LockPattern：{0}", lockpattern.ToString()));

            //いったん入力可能な状態にしておく
            //ReadOnly
            this.cmbBranchOffice.ReadOnly = false;
            //TabStop
            this.cmbBranchOffice.TabStop = true;
            //背景色
            this.SetNoLockControlBackColor(this.cmbBranchOffice);

            //明細が請求済み or 月次締め済みか？
            if (((lockpattern & LockPattern.JuchuDtlIsFixedClaim)
                    == LockPattern.JuchuDtlIsFixedClaim) ||
                ((lockpattern & LockPattern.JuchuDtlIsFixedMonth)
                    == LockPattern.JuchuDtlIsFixedMonth))
            {
            }
        }

        /// <summary>
        /// 受注明細をクリアします。
        /// </summary>
        private void ClearJuchuDtlList()
        {
            //描画を停止
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {
                this.mrsJuchuMeisai.Rows.Clear();

                if (this.mrsJuchuMeisai.RowCount > 0)
                {
                    //1行目はRowsAddedイベントが走らないのでここで準備処理をする
                    this.PrepareJuchuDtlRow(0);
                    //初期値の設定
                    if (this.mrsJuchuMeisai.NewRowIndex < 0)
                    {
                        this.SetDefaultValueToRow(this.mrsJuchuMeisai.NewRowIndex); return;
                    }
                    else
                    {
                        this.mrsJuchuMeisai.SetValue(0, MrowCellKeys.JomuinUriageDogakuFlag.ToString(), true);
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 受注明細行の準備処理を行います。
        /// セルのスタイルや入力可否などを設定します。
        /// </summary>
        /// <param name="rowIndex"></param>
        private void PrepareJuchuDtlRow(int rowIndex)
        {
            GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIndex];

            ////編集不可
            // 得意先名
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TokuisakiNM.ToString()], true);
            // 請求部門コード
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.ClmClassCd.ToString()], true);
            // 乗務員売上
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], true);

            //車両区分に関連するセル
            this.RefreshStyleForCarKbn(rowIndex);
        }

        /// <summary>
        /// 受注明細の行が追加されたときのイベントハンドラ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void mrsJuchuMeisai_RowsAdded(object sender, RowsAddedEventArgs e)
        {
            this.ProcessJuchuDtlRowsAddedRow(e);
        }

        /// <summary>
        /// 受注明細のMRowInsertedイベントを処理します。
        /// </summary>
        /// <param name="e"></param>
        private void ProcessJuchuDtlRowsAddedRow(RowsAddedEventArgs e)
        {
            int addedRowIndex = this.mrsJuchuMeisai.RowCount - 1;

            this.PrepareJuchuDtlRow(addedRowIndex);

            this.SetDefaultValueToRow(addedRowIndex);
        }

        /// <summary>
        /// 指定した行に初期値を設定します。
        /// </summary>
        /// <param name="p"></param>
        private void SetDefaultValueToRow(int rowIndex)
        {

            if (rowIndex < 0)
            {
                return;
            }

            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroName.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Tag = null;
                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.OfukuButton.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.OwnerCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.OwnerCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.OwnerNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.StartPointCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.EndPointCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.EndPointCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.EndPointNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ItemCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ItemCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ItemNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Number.ToString(), this._ToraDonSystemPropertyInfo.DefaultDailyDriveReportNumber);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FigCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FigNM.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKbn.ToString(), 0);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindName.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.DriverCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), string.Empty);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskStartHM.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskEndHM.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseHM.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Weight.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Memo.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString(), true);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), 0);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag = null;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoNm.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaxDispKbn.ToString(), (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterTaxDispKbn.ToString(), (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterAddUpYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), false);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterFixFlag.ToString(), false);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.MagoYoshasaki.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReceivedFlag.ToString(), false);

            this.SetPriceTotal();
        }

        /// <summary>
        /// 金額項目の合計を計算し、画面上にセットします。
        /// </summary>
        private void SetPriceTotal()
        {
            decimal wk_price_total = 0;
            decimal wk_tollfeeinprice_total = 0;
            decimal wk_charter_total = 0;
            decimal wk_tollfeeincharterprice_total = 0;
            decimal wk_futaigyomuryoinprice_total = 0;

            Cell wk_curmcell = this.mrsJuchuMeisai.CurrentCell;

            //受注明細の現在のセルキー
            string wk_curcellkey = string.Empty;
            if (this.mrsJuchuMeisai.CurrentCell != null)
            {
                wk_curcellkey = this.mrsJuchuMeisai.CurrentCell.Name;
            }

            //受注明細から受注金額、傭車金額の合計を計算
            int rowcount = this.mrsJuchuMeisai.RowCount;

            for (int i = 0; i < rowcount; i++)
            {
                wk_price_total +=
                    Convert.ToDecimal(
                        this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Price.ToString()));
                wk_tollfeeinprice_total +=
                    Convert.ToDecimal(
                        this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInPrice.ToString()));
                wk_charter_total +=
                    Convert.ToDecimal(
                        this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterPrice.ToString()));
                wk_tollfeeincharterprice_total +=
                    Convert.ToDecimal(
                        this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInCharterPrice.ToString()));
                wk_futaigyomuryoinprice_total +=
                    Convert.ToDecimal(
                        this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.FutaigyomuryoInPrice.ToString()));
            }

            //集計した合計を画面上にセット
            this.edtPriceTotal.Text = wk_price_total.ToString("#,##0");
            this.edtTollFeeInPrice.Text = wk_tollfeeinprice_total.ToString("#,##0");
            this.edtCharterPriceTotal.Text = wk_charter_total.ToString("#,##0");
            this.edtTollFeeInCharterPrice.Text = wk_tollfeeincharterprice_total.ToString("#,##0");
            this.edtFutaigyomuryoInPrice.Text = wk_futaigyomuryoinprice_total.ToString("#,##0");
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

        /// <summary>
        /// 受注明細から登録に必要な項目を画面コントローラにセットします。
        /// </summary>
        private void GetJuchuDtlList()
        {
            this._JuchuInfoUpdList = new List<JuchuNyuryokuInfo>();

            //描画を停止
            this.mrsJuchuMeisai.SuspendLayout();
            //自動行追加機能をオフ
            this.mrsJuchuMeisai.AllowUserToAddRows = false;

            //// 新規モードの場合は受注番号を採番
            //if (FrameEditMode.New == this.currentMode) this._OriginJuchuNo = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.JuchuSlipNo);

            try
            {
                //明細を回しながら受注情報作成
                int rowcount = this.mrsJuchuMeisai.Rows.Count;
                for (int i = 0; i < rowcount; i++)
                {
                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[i];

                    //1件分の受注情報を作成
                    JuchuNyuryokuInfo info = new JuchuNyuryokuInfo();

                    // 修正データは行コレクションのTagから復元する
                    if (this.mrsJuchuMeisai.Rows[i].Tag != null)
                    {
                        info = (JuchuNyuryokuInfo)wk_mrow.Tag;
                    }

                    ////明細行が活性の場合
                    //if (wk_mrow.Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                    //{
                    //    // 得意先情報を取得
                    //    decimal tokuisaki_cd = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiCd.ToString()));

                    //    // 受注伝票番号
                    //    info.JuchuSlipNo = this._OriginJuchuNo;
                    //    // 営業所ID
                    //    info.BranchOfficeId = Convert.ToDecimal(this.cmbBranchOffice.Tag);

                    //    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
                    //    tokui_para.ToraDONTokuisakiCode = tokuisaki_cd;
                    //    TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

                    //    // 得意先ID・消費税丸め区分・会社IDを取得
                    //    decimal tokuisaki_id = 0;
                    //    int taxcutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

                    //    if (tokui_info != null)
                    //    {
                    //        tokuisaki_id = tokui_info.ToraDONTokuisakiId;
                    //        taxcutkbn = tokui_info.ToraDONTaxCutKbn;
                    //    }

                    //    // 傭車先情報を取得
                    //    TorihikisakiSearchParameter carofcharterer_param = new TorihikisakiSearchParameter();
                    //    carofcharterer_param.ToraDONTorihikiCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CarOfChartererCd.ToString()));
                    //    TorihikisakiInfo carofcharterer_info = this._DalUtil.Torihikisaki.GetListInternal(null, carofcharterer_param).FirstOrDefault();

                    //    // 得意先ID・消費税丸め区分・会社IDを取得
                    //    decimal carofcharterer_id = 0;
                    //    int carofcharterercharter_taxcutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

                    //    if (carofcharterer_info != null)
                    //    {
                    //        carofcharterer_id = carofcharterer_info.ToraDONTorihikiId;
                    //        carofcharterercharter_taxcutkbn = carofcharterer_info.TaxCutKbn;
                    //    }

                    //    //ID(画面値にて更新しない)
                    //    //受注伝票番号(画面値にて更新しない)

                    //    info.TokuisakiId = tokuisaki_id;//得意先ID
                    //    info.TokuisakiNM =
                    //        Convert.ToString(
                    //            this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TokuisakiNM.ToString()));//得意先名

                    //    //請求部門
                    //    int clmclass_cd =
                    //        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ClmClassCd.ToString()));

                    //    decimal clmClassId = 0;

                    //    //請求部門情報を取得
                    //    ToraDONClmClassSearchParameter ClmClass_para = new ToraDONClmClassSearchParameter() { ClmClassCode = clmclass_cd };
                    //    ClmClass_para.TokuisakiId = tokuisaki_id;
                    //    ClmClass_para.ClmClassCode = clmclass_cd;
                    //    ToraDONClmClassInfo clmClassInfo = this._DalUtil.ToraDONClmClass.GetList(ClmClass_para).FirstOrDefault();

                    //    if (clmClassInfo != null)
                    //    {
                    //        clmClassId = clmClassInfo.ClmClassId;
                    //    }

                    //    //請求部門ID
                    //    info.ClmClassId = clmClassId;

                    //    //請負
                    //    int contract_cd =
                    //        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ContractCd.ToString()));

                    //    decimal contractId = 0;

                    //    ToraDONContractSearchParameter cntract_para = new ToraDONContractSearchParameter();
                    //    cntract_para.TokuisakiId = tokuisaki_id;
                    //    cntract_para.ClmClassId = clmClassId;
                    //    cntract_para.ContractCode = contract_cd;
                    //    ToraDONContractInfo contract_info = this._DalUtil.ToraDONContract.GetList(cntract_para).FirstOrDefault();

                    //    if (contract_info != null)
                    //    {
                    //        contractId = contract_info.ContractId;
                    //    }

                    //    contractId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.ContractCd.ToString()].Tag);

                    //    // 請負ID
                    //    info.ContractId = contractId;

                    //    // 積地ID
                    //    info.StartPointId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.StartPointCd.ToString()].Tag);
                    //    // 積地名
                    //    info.StartPointNM = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.StartPointNM.ToString()));
                    //    // 着地ID
                    //    info.EndPointId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.EndPointCd.ToString()].Tag);
                    //    // 着地名
                    //    info.EndPointNM = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.EndPointNM.ToString()));
                    //    // 品目ID
                    //    info.ItemId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.ItemCd.ToString()].Tag);
                    //    // 品目名
                    //    info.ItemNM = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ItemNM.ToString()));
                    //    // 荷主ID
                    //    info.OwnerId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.OwnerCd.ToString()].Tag);
                    //    // 荷主名
                    //    info.OwnerNM = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.OwnerNM.ToString()));

                    //    // 作業開始日付
                    //    DateTime DateTime_TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskStartYMD.ToString()));
                    //    // 作業開始時刻
                    //    DateTime DateTime_TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskStartHM.ToString()));
                    //    info.TaskStartDateTime = new DateTime(
                    //        DateTime_TaskStartYMD.Year,
                    //        DateTime_TaskStartYMD.Month,
                    //        DateTime_TaskStartYMD.Day,
                    //        DateTime_TaskStartHM.Hour,
                    //        DateTime_TaskStartHM.Minute,
                    //        0);

                    //    // 作業終了日付
                    //    DateTime DateTime_TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskEndYMD.ToString()));
                    //    // 作業終了時刻
                    //    DateTime DateTime_TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaskEndHM.ToString()));
                    //    info.TaskEndDateTime = new DateTime(
                    //        DateTime_TaskEndYMD.Year,
                    //        DateTime_TaskEndYMD.Month,
                    //        DateTime_TaskEndYMD.Day,
                    //        DateTime_TaskEndHM.Hour,
                    //        DateTime_TaskEndHM.Minute,
                    //        0);

                    //    //再使用可能日が入力されている場合
                    //    if (DateTime.MinValue < Convert.ToDateTime(
                    //            this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseYMD.ToString())))
                    //    {
                    //        // 再使用可能日付
                    //        DateTime DateTime_ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseYMD.ToString()));
                    //        // 再使用可能時刻
                    //        DateTime DateTime_ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReuseHM.ToString()));
                    //        info.ReuseYMD = new DateTime(
                    //            DateTime_ReuseYMD.Year,
                    //            DateTime_ReuseYMD.Month,
                    //            DateTime_ReuseYMD.Day,
                    //            DateTime_ReuseHM.Hour,
                    //            DateTime_ReuseHM.Minute,
                    //            0);
                    //    }
                    //    else
                    //    {
                    //        info.ReuseYMD = DateTime.MinValue;
                    //    }

                    //    // 数量
                    //    info.Number = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Number.ToString()));
                    //    // 単位ID
                    //    info.FigId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.FigCd.ToString()].Tag);
                    //    // 車両ID
                    //    info.CarId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.CarCd.ToString()].Tag);
                    //    // 車番
                    //    info.LicPlateCarNo = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.LicPlateCarNo.ToString()));
                    //    // 車種ID
                    //    info.CarKindId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.CarKindCd.ToString()].Tag);
                    //    // 乗務員ID
                    //    info.DriverId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.DriverCd.ToString()].Tag);
                    //    // 単価
                    //    info.AtPrice = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.AtPrice.ToString()));
                    //    // 備考
                    //    info.Memo = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Memo.ToString()));
                    //    // 重量
                    //    info.Weight = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Weight.ToString()));
                    //    // 販路ID
                    //    info.HanroId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.HanroCd.ToString()].Tag);
                    //    // 往復区分
                    //    info.OfukuKbn = Convert.ToInt32(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.OfukuButton.ToString()].Tag);
                    //    // 受注担当ID
                    //    info.JuchuTantoId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag);
                    //    // 乗務員売上を取得
                    //    info.JomuinUriageKingaku = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.JomuinUriageKingaku.ToString()));
                    //    // 乗務員売上同額フラグを取得
                    //    info.JomuinUriageDogakuFlag = (bool)this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.JomuinUriageDogakuFlag.ToString());
                    //    //孫傭車先
                    //    info.MagoYoshasaki = StringHelper.ConvertToString(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.MagoYoshasaki.ToString()));
                    //    //受領済フラグを取得
                    //    info.ReceivedFlag = (bool)this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.ReceivedFlag.ToString());

                    //    //受注金額関連

                    //    // 金額を取得
                    //    decimal price = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.Price.ToString()));
                    //    // 受注金額通行料を取得
                    //    decimal tollFeeInPrice = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInPrice.ToString()));
                    //    // 受注金額附帯業務料を取得
                    //    decimal futaigyomuryoInPrice = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

                    //    // 税区分
                    //    int taxdispkbn = 0;
                    //    // 計上日
                    //    DateTime addupdate = DateTime.MinValue;
                    //    // 確定フラグ
                    //    bool fixflag = false;

                    //    //請負ではない通常の受注か？
                    //    if (contract_info == null)
                    //    {
                    //        //画面値をセットする
                    //        taxdispkbn = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TaxDispKbn.ToString()));

                    //        // 計上日を取得
                    //        addupdate = (Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.AddUpYMD.ToString()))).Date;
                    //        // 確定フラグを取得
                    //        fixflag = (bool)this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.FixFlag.ToString());
                    //    }
                    //    else
                    //    {
                    //        //請負データの場合は、請負の税区分・計上日をセット
                    //        taxdispkbn = contract_info.TaxDispKbn;
                    //        addupdate = contract_info.AddUpDate;

                    //        //請負データの場合は、確定フラグに固定で"1"をセット
                    //        fixflag = true;
                    //    }

                    //    // 税率取得
                    //    decimal juchu_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(addupdate);

                    //    // 税区分（列挙値）取得
                    //    BizProperty.DefaultProperty.ZeiKbn wk_juchu_taxkbn =
                    //        (BizProperty.DefaultProperty.ZeiKbn)taxdispkbn;

                    //    // 端数区分（列挙値）取得
                    //    BizProperty.DefaultProperty.HasuMArumeKbn wk_juchu_cutkbn =
                    //        (BizProperty.DefaultProperty.HasuMArumeKbn)taxcutkbn;

                    //    //税計算
                    //    CalcTaxResultStructInfo juchu_taxstruct_info = this.ComputeTaxStruct(price + futaigyomuryoInPrice, juchu_taxrate, wk_juchu_taxkbn, wk_juchu_cutkbn);

                    //    decimal outtaxcalc = 0;
                    //    decimal outtax = 0;
                    //    decimal intaxcalc = 0;
                    //    decimal intax = 0;
                    //    decimal notaxcalc = 0;

                    //    switch (wk_juchu_taxkbn)
                    //    {
                    //        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                    //            outtaxcalc = juchu_taxstruct_info.BaseAmount;
                    //            outtax = juchu_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                    //            intaxcalc = juchu_taxstruct_info.WithoutTaxAmount;
                    //            intax = juchu_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                    //            notaxcalc = juchu_taxstruct_info.BaseAmount;
                    //            break;
                    //        default:
                    //            break;
                    //    }

                    //    info.Price = price + tollFeeInPrice + futaigyomuryoInPrice;//金額
                    //    info.PriceInPrice = price; //金額_金額
                    //    info.TollFeeInPrice = tollFeeInPrice; //金額_通行料
                    //    info.FutaigyomuryoInPrice = futaigyomuryoInPrice; //金額_附帯業務料
                    //    info.PriceOutTaxCalc = outtaxcalc;//外税対象額
                    //    info.PriceOutTax = outtax;//外税額
                    //    info.PriceInTaxCalc = intaxcalc;//内税対象額
                    //    info.PriceInTax = intax;//内税額
                    //    info.PriceNoTaxCalc = notaxcalc + tollFeeInPrice;//非課税対象額
                    //    info.TaxDispKbn = taxdispkbn;//税区分
                    //    info.AddUpYMD = addupdate;//計上日付
                    //    info.FixFlag = fixflag;//確定フラグ

                    //    //請求締切日	ClmFixDate(画面値により更新しない)

                    //    //傭車金額関連

                    //    // 傭車金額を取得
                    //    decimal charter_price = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterPrice.ToString()));

                    //    // 傭車金額通行料
                    //    decimal tollFeeInCharterPrice = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.TollFeeInCharterPrice.ToString()));

                    //    // 傭車税区分を取得
                    //    int charter_taxdispkbn = taxdispkbn = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterTaxDispKbn.ToString()));

                    //    // 傭車計上日を取得
                    //    DateTime charter_addupdate = (Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterAddUpYMD.ToString()))).Date;

                    //    // 傭車確定フラグを取得
                    //    bool charter_fixflag = (bool)this.mrsJuchuMeisai.GetValue(i, MrowCellKeys.CharterFixFlag.ToString());

                    //    // 税率取得
                    //    decimal charter_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(charter_addupdate);

                    //    // 税区分（列挙値）取得
                    //    BizProperty.DefaultProperty.ZeiKbn wk_charter_taxkbn =
                    //        (BizProperty.DefaultProperty.ZeiKbn)charter_taxdispkbn;

                    //    // 端数区分（列挙値）取得
                    //    BizProperty.DefaultProperty.HasuMArumeKbn wk_charter_cutkbn =
                    //        (BizProperty.DefaultProperty.HasuMArumeKbn)carofcharterercharter_taxcutkbn;

                    //    //税計算
                    //    CalcTaxResultStructInfo charter_taxstruct_info = this.ComputeTaxStruct(charter_price, charter_taxrate, wk_charter_taxkbn, wk_charter_cutkbn);

                    //    decimal charter_outtaxcalc = 0;
                    //    decimal charter_outtax = 0;
                    //    decimal charter_intaxcalc = 0;
                    //    decimal charter_intax = 0;
                    //    decimal charter_notaxcalc = 0;

                    //    switch (wk_charter_taxkbn)
                    //    {
                    //        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                    //            charter_outtaxcalc = charter_taxstruct_info.BaseAmount;
                    //            charter_outtax = charter_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                    //            charter_intaxcalc = charter_taxstruct_info.WithoutTaxAmount;
                    //            charter_intax = charter_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                    //            charter_notaxcalc = charter_taxstruct_info.BaseAmount;
                    //            break;
                    //        default:
                    //            break;
                    //    }

                    //    info.CarOfChartererId = carofcharterer_id;//傭車先ID
                    //    info.CharterPrice = charter_price + tollFeeInCharterPrice;//傭車金額
                    //    info.PriceInCharterPrice = charter_price; //傭車金額_金額
                    //    info.TollFeeInCharterPrice = tollFeeInCharterPrice; //傭車金額_通行料
                    //    info.CharterPriceOutTaxCalc = charter_outtaxcalc;//傭車外税対象額
                    //    info.CharterPriceOutTax = charter_outtax;//傭車外税額
                    //    info.CharterPriceInTaxCalc = charter_intaxcalc;//傭車内税対象額
                    //    info.CharterPriceInTax = charter_intax;//傭車内税額
                    //    info.CharterPriceNoTaxCalc = charter_notaxcalc + tollFeeInCharterPrice;//傭車非課税対象額
                    //    info.CharterTaxDispKbn = charter_taxdispkbn;//傭車税区分
                    //    info.CharterAddUpYMD = charter_addupdate;//傭車計上日付
                    //    info.CharterFixFlag = charter_fixflag;//傭車確定フラグ

                    //    //運賃_金額
                    //    decimal priceInFee = price;
                    //    //運賃_通行料 
                    //    decimal tollFeeInFee = tollFeeInPrice;
                    //    //運賃_附帯業務料 
                    //    decimal futaigyomuryoInFee = futaigyomuryoInPrice;
                    //    // 税率取得
                    //    decimal fee_taxrate = 0;
                    //    // 税区分（列挙値）取得
                    //    BizProperty.DefaultProperty.ZeiKbn wk_fee_taxkbn;
                    //    // 端数区分（列挙値）取得
                    //    BizProperty.DefaultProperty.HasuMArumeKbn wk_fee_cutkbn;

                    //    fee_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(addupdate);
                    //    wk_fee_taxkbn = (BizProperty.DefaultProperty.ZeiKbn)taxdispkbn;
                    //    wk_fee_cutkbn = (BizProperty.DefaultProperty.HasuMArumeKbn)taxcutkbn;

                    //    //税計算
                    //    CalcTaxResultStructInfo fee_taxstruct_info = this.ComputeTaxStruct(priceInFee + futaigyomuryoInFee, fee_taxrate, wk_fee_taxkbn, wk_fee_cutkbn);

                    //    decimal fee_outtaxcalc = 0;
                    //    decimal fee_outtax = 0;
                    //    decimal fee_intaxcalc = 0;
                    //    decimal fee_intax = 0;
                    //    decimal fee_notaxcalc = 0;

                    //    switch (wk_fee_taxkbn)
                    //    {
                    //        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                    //            fee_outtaxcalc = fee_taxstruct_info.BaseAmount;
                    //            fee_outtax = fee_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                    //            fee_intaxcalc = fee_taxstruct_info.WithoutTaxAmount;
                    //            fee_intax = fee_taxstruct_info.TaxAmount;
                    //            break;
                    //        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                    //            fee_notaxcalc = fee_taxstruct_info.BaseAmount;
                    //            break;
                    //        default:
                    //            break;
                    //    }

                    //    info.Fee = priceInFee + tollFeeInFee + futaigyomuryoInFee;
                    //    info.PriceInFee = priceInFee;
                    //    info.TollFeeInFee = tollFeeInFee;
                    //    info.FutaigyomuryoInFee = futaigyomuryoInFee;
                    //    info.FeeOutTaxCalc = fee_outtaxcalc;//運賃外税対象額
                    //    info.FeeOutTax = fee_outtax;//運賃外税額
                    //    info.FeeInTaxCalc = fee_intaxcalc;//運賃内税対象額
                    //    info.FeeInTax = fee_intax;//運賃内税額
                    //    info.FeeNoTaxCalc = fee_notaxcalc + tollFeeInFee;//運賃非課税対象額

                    //    //更新を行う
                    //    info.UnChange = false;
                    //}
                    //else
                    //{
                    //    if (info != null)
                    //    {
                    //        //不変更にする
                    //        info.UnChange = true;
                    //    }
                    //}

                    this._JuchuInfoUpdList.Add(info);
                }

            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //自動行追加機能をオン
                this.mrsJuchuMeisai.AllowUserToAddRows = true;
                //描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }
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
                    this.pnlTop.Enabled = true;
                    this.pnlMid.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    // ファンクションの使用可否

                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = false;

                    this.cmbBranchOffice.Focus();

                    break;
                case FrameEditMode.Editable:

                    this.edtJuchuSlipNo.Text = this._OriginJuchuNo.ToString();

                    //編集モード
                    this.pnlTop.Enabled = true;
                    this.pnlMid.Enabled = true;
                    this.pnlBottom.Enabled = true;
                    // ファンクションの使用可否
                    _commandSet.EditCancel.Enabled = true;

                    _commandSet.Save.Enabled = true;

                    _commandSet.Close.Enabled = true;

                    _commandSet.DeleteAllDtl.Enabled = true;
                    // フォーカス設定
                    this.mrsJuchuMeisai.Focus();

                    break;
                default:
                    break;
            }

            //現在のモードを切り替える
            currentMode = mode;
        }

        /// <summary>
        /// コントローラーの値を画面にセットします。
        /// </summary>
        private void SetScreen()
        {
            //MultiRowの描画を停止
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {
                decimal wk_price_total = 0;
                decimal wk_chaterprice_total = 0;

                //件数取得
                int rowcount = this._JuchuInfoSelList.Count;
                if (rowcount == 0)
                {
                    //ないときはもどる
                    return;
                }

                //最終月次集計月を取得しておく
                DateTime wk_date = this._ToraDonSystemPropertyInfo.LastSummaryOfMonthDate;
                DateTime last_sum_of_monthday;
                if (wk_date > DateTime.MinValue)
                {
                    last_sum_of_monthday = NSKUtil.MonthLastDay(wk_date);
                }
                else
                {
                    last_sum_of_monthday = wk_date;
                }

                //行数を設定
                this.mrsJuchuMeisai.RowCount = rowcount + 1;

                //外部選択明細行
                int selectRowIndex = 0;

                for (int i = 0; i < rowcount; i++)
                {
                    //1件分の受注情報を取得
                    JuchuNyuryokuInfo info = this._JuchuInfoSelList[i];

                    if (0 < this._OriginJuchuId && info.JuchuId == this._OriginJuchuId)
                    {
                        selectRowIndex = i;
                    }

                    //Mrowにセット
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[i];
                    // 営業先コード
                    this.cmbBranchOffice.SelectedValue = info.BranchOfficeCd.ToString();

                    // 営業先ID
                    this.cmbBranchOffice.Tag = info.BranchOfficeId;
                    // 得意先コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.TokuisakiCd.ToString(), info.TokuisakiCd);
                    // 得意先ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = info.TokuisakiId;
                    // 得意先名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.TokuisakiNM.ToString(), info.TokuisakiNM);
                    // 販路コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.HanroCd.ToString(), info.HanroCd);
                    // 販路ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.HanroCd.ToString()].Tag = info.HanroId;
                    // 販路名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.HanroName.ToString(), info.HanroNm);
                    // 往復区分
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.OfukuButton.ToString()].Tag = info.OfukuKbn;
                    // 往復区分名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.OfukuButton.ToString(), info.OfukuKbnNm);
                    // 請求部門コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ClmClassCd.ToString(), info.ClmClassCd);
                    // 請求部門ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = info.ClmClassId;
                    // 請求部門名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ClmClassNM.ToString(), info.ClmClassSNM);
                    // 請負コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ContractCd.ToString(), info.ContractCd);
                    // 請負ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.ContractCd.ToString()].Tag = info.ContractId;
                    // 請負名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ContractNM.ToString(), info.ContractNm);
                    // 品目コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ItemCd.ToString(), info.ItemCd);
                    // 品目ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.ItemCd.ToString()].Tag = info.ItemId;
                    // 品目名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ItemNM.ToString(), info.ItemNM);
                    // 積地コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.StartPointCd.ToString(), info.StartPointCd);
                    // 積地ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.StartPointCd.ToString()].Tag = info.StartPointId;
                    // 積地名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.StartPointNM.ToString(), info.StartPointNM);
                    // 着地コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.EndPointCd.ToString(), info.EndPointCd);
                    // 着地ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.EndPointCd.ToString()].Tag = info.EndPointId;
                    // 着地名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.EndPointNM.ToString(), info.EndPointNM);
                    // 積日時
                    if (info.TaskStartDateTime != DateTime.MinValue)
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskStartYMD.ToString(), info.TaskStartDateTime);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskStartHM.ToString(), info.TaskStartDateTime);
                    }
                    else
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskStartYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskStartHM.ToString(), null);
                    }
                    // 着日時
                    if (info.TaskEndDateTime != DateTime.MinValue)
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskEndYMD.ToString(), info.TaskEndDateTime);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskEndHM.ToString(), info.TaskEndDateTime);
                    }
                    else
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskEndYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.TaskEndHM.ToString(), null);
                    }
                    // 再使用可能日時
                    if (info.ReuseYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.ReuseYMD.ToString(), info.ReuseYMD);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.ReuseHM.ToString(), info.ReuseYMD);
                    }
                    else
                    {
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.ReuseYMD.ToString(), null);
                        this.mrsJuchuMeisai.SetValue(i, MrowCellKeys.ReuseHM.ToString(), null);
                    }

                    // 数量
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.Number.ToString(), info.Number);
                    // 単位コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.FigCd.ToString(), info.FigCd);
                    // 単位ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.FigCd.ToString()].Tag = info.FigId;
                    // 単位名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.FigNM.ToString(), info.FigNm);
                    // 車両コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarCd.ToString(), info.CarCd);
                    // 車両ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.CarCd.ToString()].Tag = info.CarId;
                    // 車番
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.LicPlateCarNo.ToString(), info.LicPlateCarNo);
                    // 傭車区分
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.Chartererkbn.ToString(),
                        info.CarKbn == (int)DefaultProperty.CarKbn.Yosha
                            ? FrameUtilites.GetSystemName(DefaultProperty.SystemNameKbn.CarKbn, (int)DefaultProperty.CarKbn.Yosha)
                            : info.CarBranchOfficeSNM);
                    // 車両区分（隠し）
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarKbn.ToString(), info.CarKbn);
                    // 車種コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarKindCd.ToString(), info.CarKindCd);
                    // 車種ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = info.CarKindId;
                    // 車種名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarKindName.ToString(), info.CarKindSNM);
                    // 乗務員コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.DriverCd.ToString(), info.DriverCd);
                    // 乗務員ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.DriverCd.ToString()].Tag = info.DriverId;
                    // 乗務員名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.DriverNm.ToString(), info.DriverNm);
                    // 傭車先コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarOfChartererCd.ToString(), info.CarOfChartererCd);
                    // 傭車先ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = info.CarOfChartererId;
                    // 傭車先名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CarOfChartererName.ToString(), info.CarOfChartererShortNm);
                    // 重量
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.Weight.ToString(), info.Weight);
                    // 単価
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.AtPrice.ToString(), info.AtPrice);
                    // 受注金額
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.Price.ToString(), info.PriceInPrice);
                    // 金額_通行料
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.TollFeeInPrice.ToString(), info.TollFeeInPrice);
                    // 税区分
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.TaxDispKbn.ToString(), info.TaxDispKbn);
                    // 計上日
                    if (info.AddUpYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuMeisai.SetValue(
                            i, MrowCellKeys.AddUpYMD.ToString(),
                                NSKUtil.DateTimeMinValueToNull(info.AddUpYMD));
                    }
                    // 確定フラグ
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.FixFlag.ToString(), info.FixFlag);
                    // 傭車金額
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CharterPrice.ToString(), info.PriceInCharterPrice);
                    // 傭車金額_通行料
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.TollFeeInCharterPrice.ToString(), info.TollFeeInCharterPrice);
                    // 傭車税区分
                    if (info.CharterTaxDispKbn != 0)
                    {
                        //傭車税区分
                        this.mrsJuchuMeisai.SetValue(
                              i, MrowCellKeys.CharterTaxDispKbn.ToString(), info.CharterTaxDispKbn);
                    }
                    else
                    {
                        this.mrsJuchuMeisai.SetValue(
                            i, MrowCellKeys.CharterTaxDispKbn.ToString(), null);
                    }
                    // 傭車計上日
                    if (info.CharterAddUpYMD != DateTime.MinValue)
                    {
                        this.mrsJuchuMeisai.SetValue(
                            i, MrowCellKeys.CharterAddUpYMD.ToString(),
                                NSKUtil.DateTimeMinValueToNull(info.CharterAddUpYMD));
                    }
                    // 傭車確定フラグ
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.CharterFixFlag.ToString(), info.CharterFixFlag);
                    // 荷主コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.OwnerCd.ToString(), info.OwnerCd);
                    // 荷主ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.OwnerCd.ToString()].Tag = info.OwnerId;
                    // 荷主名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.OwnerNM.ToString(), info.OwnerNM);
                    // 備考
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.Memo.ToString(), info.Memo);
                    // 受注担当コード
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.JuchuTantoCd.ToString(), info.JuchuTantoCd);
                    // 受注担当ID
                    this.mrsJuchuMeisai.Rows[i].Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag = info.JuchuTantoId;
                    // 受注担当名
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.JuchuTantoNm.ToString(), info.JuchuTantoNm);
                    // 金額_附帯業務料
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.FutaigyomuryoInPrice.ToString(), info.FutaigyomuryoInPrice);
                    // 乗務員売上同額フラグ
                    this.mrsJuchuMeisai.SetValue(
                          i, MrowCellKeys.JomuinUriageDogakuFlag.ToString(), info.JomuinUriageDogakuFlag);
                    // 乗務員売上金額
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.JomuinUriageKingaku.ToString(), info.JomuinUriageKingaku);
                    // 乗務員売上金額_編集可否
                    this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], info.JomuinUriageDogakuFlag);
                    // 孫傭車先
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.MagoYoshasaki.ToString(), info.MagoYoshasaki);
                    // 受領済フラグ
                    this.mrsJuchuMeisai.SetValue(
                        i, MrowCellKeys.ReceivedFlag.ToString(), info.ReceivedFlag);

                    //１件分の受注情報をMRowのTagにセットしておく
                    wk_mrow.Tag = info;
                    //金額の合計を計算
                    wk_price_total += info.Price;
                    //傭車金額の合計を計算
                    wk_chaterprice_total += info.CharterPrice;

                    //明細のロックパターン保持用
                    LockPattern lockpt =
                        LockPattern.None;

                    //日報の指定があった時のみ(修正のみ)行のロックを行う
                    if (this._OriginJuchuNo != 0)
                    {
                        //TODO 性能問題のためコメント化中 START
                        ////請求済みの場合は行レベルでロック
                        //if (info.MinClmFixDate != DateTime.MinValue)
                        //{
                        //    lockpt = LockPattern.JuchuDtlIsFixedClaim;
                        //}

                        ////支払済みの場合は行レベルでロック
                        //if (info.MinCharterPayFixDate != DateTime.MinValue)
                        //{
                        //    lockpt = LockPattern.JuchuDtlIsFixedPay;
                        //}
                        //TODO 性能問題のためコメント化中 END

                        //月次締処理済みの場合は行レベルでロック(新規明細(IDが0の時にはロックしない))
                        //（月次締処理済みに関しては配車Ace側の計上日、確定フラグに加え、
                        //  請求データ連携後のトラDONの売上テーブルの計上日、確定フラグもチェック対象とする）
                        if (info.JuchuId != 0 &&
                            (this.IsMonthlyTotaled(info.AddUpYMD, info.FixFlag, last_sum_of_monthday) ||
                            //TODO 性能問題のためコメント化中 START
                            //this.IsMonthlyTotaled(info.MinAddUpDate, info.MaxFixFlag, last_sum_of_monthday) ||
                            //TODO 性能問題のためコメント化中 END
                            this.IsMonthlyTotaled(info.CharterAddUpYMD, info.CharterFixFlag, last_sum_of_monthday)
                            //TODO 性能問題のためコメント化中 START
                            //||
                            //this.IsMonthlyTotaled(info.MinCharterAddUpDate, info.MaxCharterFixFlag, last_sum_of_monthday)
                            //TODO 性能問題のためコメント化中 END
                            ))
                        {
                            lockpt = LockPattern.JuchuDtlIsFixedMonth;
                        }
                    }

                    if (lockpt == LockPattern.JuchuDtlIsFixedMonth)
                    {
                        // 行レベルのロック
                        OriginalRowLocked(wk_mrow, true);
                    }
                    else if (lockpt == LockPattern.JuchuDtlIsFixedPay)
                    {
                        // 行レベルのロック
                        OriginalRowLocked(wk_mrow, true);
                    }
                    else if (lockpt == LockPattern.JuchuDtlIsFixedClaim)
                    {
                        // 行レベルのロック
                        OriginalRowLocked(wk_mrow, true);
                    }
                    else
                    {
                        // 得意先[諸口区分]="1"の場合は、得意先名編集可
                        if ((int)BizProperty.DefaultProperty.MemoAccount.Shusei == info.MemoAccount)
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TokuisakiNM.ToString()], false);
                        }
                        else
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TokuisakiNM.ToString()], true);
                        }

                        // 得意先[部門管理区分]="1"の場合は、請求部門編集可
                        if ((int)BizProperty.DefaultProperty.ClmClassUseKbn.Kanri == info.ClmClassUseKbn)
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.ClmClassCd.ToString()], false);
                        }
                        else
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.ClmClassCd.ToString()], true);
                        }

                        this.RefreshStyleForCarKbn(i, info.CarKbn);

                        // 請負コード=0以外の場合は編集可
                        if (info.ContractCd != 0)
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TaxDispKbn.ToString()], true);
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.AddUpYMD.ToString()], true);
                        }
                        else
                        {
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TaxDispKbn.ToString()], false);
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.AddUpYMD.ToString()], false);
                        }
                    }
                }

                //販路コードのセルポジション（初期値）
                CellPosition cposition =
                    new CellPosition(selectRowIndex, MrowCellKeys.HanroCd.ToString());

                //明細行が非活性の場合
                if (!this.mrsJuchuMeisai.Rows[selectRowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                {
                    //セルポジションを販路マスタ追加ボタンに変更
                    cposition =
                        new CellPosition(selectRowIndex, MrowCellKeys.HanroAddButton.ToString());

                    //販路マスタ追加ボタンを選択化に変更
                    this.mrsJuchuMeisai.Rows[selectRowIndex].Cells[MrowCellKeys.HanroAddButton.ToString()].Selectable = true;
                }

                //表示行設定
                this.mrsJuchuMeisai.CurrentCellPosition = cposition;
                this.mrsJuchuMeisai.FirstDisplayedCellPosition = cposition;

                this.SetPriceTotal();
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }
        }

        /// <summary>
        /// 販路情報及び積日から着日を設定します。
        /// </summary>
        private void SetTaskEndYMDHM(int rowIndex, HanroInfo hanro_info, DateTime TaskStartYMD, DateTime TaskStartHM, DateTime TaskEndYMD, DateTime TaskEndHM)
        {
            //販路情報が存在しない場合にはスキップ
            if (null != hanro_info)
            {
                // 着日及び着時間が入力済の場合にはスキップ
                if (DateTime.MinValue == TaskEndYMD || DateTime.MinValue == TaskEndHM)
                {
                    // 行程日数取得
                    int KoteiNissu = hanro_info.KoteiNissu;
                    // 行程時間取得
                    int KoteiJikan = hanro_info.KoteiJikan;

                    int KoteiJikanH = 0;
                    int KoteiJikanM = 0;
                    int KoteiJikanS = 0;

                    // 行程時間を時、分、秒に変換する。
                    // 時
                    KoteiJikanH = KoteiJikan / 10000;
                    // 分
                    KoteiJikanM = ((KoteiJikan - (KoteiJikanH * 10000)) / 100);
                    // 秒
                    KoteiJikanS = KoteiJikan - (KoteiJikanH * 10000) - (KoteiJikanM * 100);

                    if (DateTime.MinValue != TaskStartYMD && DateTime.MinValue != TaskStartHM) //積日、積時間が入力されている場合
                    {
                        //積日時を取得
                        DateTime TaskStartYMDHM = new DateTime(TaskStartYMD.Year, TaskStartYMD.Month, TaskStartYMD.Day, TaskStartHM.Hour, TaskStartHM.Minute, 0);

                        //着日時を取得
                        DateTime TaskEndYMDHM = TaskStartYMDHM.AddDays(KoteiNissu).AddHours(KoteiJikanH).AddMinutes(KoteiJikanM).AddSeconds(KoteiJikanS);

                        //着日を設定
                        DateTime NewTaskEndYMD = TaskEndYMDHM.Date;
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString(), NewTaskEndYMD);

                        //着時間を設定
                        DateTime NewTaskEndHM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            TaskEndYMDHM.Hour, TaskEndYMDHM.Minute, 0);
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskEndHM.ToString(), NewTaskEndHM);
                    }
                    else if (DateTime.MinValue != TaskStartYMD)//積日が入力されている場合
                    {
                        // 着日を設定（積日から行程日数を除算する。）
                        DateTime NewTaskEndYMD = TaskStartYMD.AddDays(KoteiNissu);
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString(), NewTaskEndYMD);
                    }
                }
            }
        }

        /// <summary>
        /// 販路情報及び着日から積日を設定します。
        /// </summary>
        private void SetTaskStartYMDHM(int rowIndex, HanroInfo hanro_info, DateTime TaskStartYMD, DateTime TaskStartHM, DateTime TaskEndYMD, DateTime TaskEndHM)
        {
            //販路情報が存在しない場合にはスキップ
            if (null != hanro_info)
            {
                // 積日及び積時間が入力済の場合にはスキップ
                if (DateTime.MinValue == TaskStartYMD || DateTime.MinValue == TaskStartHM)
                {
                    // 行程日数取得
                    int KoteiNissu = hanro_info.KoteiNissu;
                    // 行程時間取得
                    int KoteiJikan = hanro_info.KoteiJikan;

                    int KoteiJikanH = 0;
                    int KoteiJikanM = 0;
                    int KoteiJikanS = 0;

                    // 行程時間を時、分、秒に変換する。
                    // 時
                    KoteiJikanH = KoteiJikan / 10000;
                    // 分
                    KoteiJikanM = ((KoteiJikan - (KoteiJikanH * 10000)) / 100);
                    // 秒
                    KoteiJikanS = KoteiJikan - (KoteiJikanH * 10000) - (KoteiJikanM * 100);

                    if (DateTime.MinValue != TaskEndYMD && DateTime.MinValue != TaskEndHM) //着日、着時間が入力されている場合
                    {
                        //着日時を取得
                        DateTime TaskEndYMDHM = new DateTime(TaskEndYMD.Year, TaskEndYMD.Month, TaskEndYMD.Day, TaskEndHM.Hour, TaskEndHM.Minute, 0);

                        //積日時を取得
                        DateTime TaskStartYMDHM = TaskEndYMDHM.AddDays(-1 * KoteiNissu).AddHours(-1 * KoteiJikanH).AddMinutes(-1 * KoteiJikanM).AddSeconds(-1 * KoteiJikanS);

                        //積日を設定
                        DateTime NewTaskStartYMD = TaskStartYMDHM.Date;
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString(), NewTaskStartYMD);

                        //積時間を設定
                        DateTime NewTaskStartHM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                            TaskStartYMDHM.Hour, TaskStartYMDHM.Minute, 0);
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskStartHM.ToString(), NewTaskStartHM);
                    }
                    else if (DateTime.MinValue != TaskEndYMD)//着日が入力されている場合
                    {
                        // 積日を設定（着日から行程日数を除算する。）
                        DateTime NewTaskStartYMD = TaskEndYMD.AddDays(-1 * KoteiNissu);
                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString(), NewTaskStartYMD);
                    }
                }
            }
        }

        /// <summary>
        /// 販路情報及び着日から再使用可能日を設定します。
        /// </summary>
        private void SetReuseYMDHM(int rowIndex, HanroInfo hanro_info, DateTime TaskEndYMD, DateTime TaskEndHM, DateTime ReuseYMD, DateTime ReuseHM)
        {
            //販路情報が存在しない場合にはスキップ
            if (null != hanro_info)
            {
                //再使用可能日数、再使用可能時間が未設定の場合
                if (Convert.ToInt32(hanro_info.ReuseNissu) == 0
                    && Convert.ToInt32(hanro_info.ReuseJikan) == 0)
                {
                    //再使用可能日時を初期化
                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString(), null);
                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseHM.ToString(), null);
                }
                else
                {
                    // 再使用可能日及び再使用可能時間が入力済の場合にはスキップ
                    if (DateTime.MinValue == ReuseYMD || DateTime.MinValue == ReuseHM)
                    {
                        // 再使用可能日数取得
                        int ReuseNissu = hanro_info.ReuseNissu;
                        // 再使用可能時間取得
                        int ReuseJikan = hanro_info.ReuseJikan;

                        int ReuseJikanH = 0;
                        int ReuseJikanM = 0;
                        int ReuseJikanS = 0;

                        // 再使用可能時間を時、分、秒に変換する。
                        // 時
                        ReuseJikanH = ReuseJikan / 10000;
                        // 分
                        ReuseJikanM = ((ReuseJikan - (ReuseJikanH * 10000)) / 100);
                        // 秒
                        ReuseJikanS = ReuseJikan - (ReuseJikanH * 10000) - (ReuseJikanM * 100);

                        if (DateTime.MinValue != TaskEndYMD && DateTime.MinValue != TaskEndHM) //着日、着時間が入力されている場合
                        {
                            //着日時を取得
                            DateTime TaskEndYMDHM = new DateTime(TaskEndYMD.Year, TaskEndYMD.Month, TaskEndYMD.Day, TaskEndHM.Hour, TaskEndHM.Minute, 0);

                            //再使用可能日時を取得
                            DateTime ReuseYMDHM = TaskEndYMDHM.AddDays(ReuseNissu).AddHours(ReuseJikanH).AddMinutes(ReuseJikanM).AddSeconds(ReuseJikanS);

                            //再使用可能日を設定
                            DateTime NewReuseYMD = ReuseYMDHM.Date;
                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString(), NewReuseYMD);

                            //再使用可能時間を設定
                            DateTime NewReuseHM = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day,
                                ReuseYMDHM.Hour, ReuseYMDHM.Minute, 0);
                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseHM.ToString(), NewReuseHM);
                        }
                        else if (DateTime.MinValue != TaskEndYMD)//着日が入力されている場合
                        {
                            // 再使用可能日を設定（着日から再使用可能日数を除算する。）
                            DateTime NewReuseYMD = TaskEndYMD.AddDays(ReuseNissu);
                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString(), NewReuseYMD);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 受注明細のCellLeaveイベント時のセルポジションを保持
        /// </summary>
        private CellPosition _juchuDtlListCellPositionAtCellLeave;

        #region セルのフォーカス移動支援

        /// <summary>
        /// セルの移動順番がもう一つのセルの移動順番よりも大きいかどうかを取得します。
        /// ※移動順番はTab移動の順番こと。 ≠ TabIndex（行の上下位置も考慮する）
        /// </summary>
        /// <param name="multiRow">比較対象のセルのGCMultiRow</param>
        /// <param name="positionA">一つ目のセルのCellPostion</param>
        /// <param name="positionB">二つ目のセルのCellPostion</param>
        /// <returns></returns>
        private static bool CellMovingOrderIsGreater(GcMultiRow multiRow, CellPosition positionA, CellPosition positionB)
        {
            bool forward = false;

            var cellA = multiRow[positionA.RowIndex, positionA.CellIndex];
            var cellB = multiRow[positionB.RowIndex, positionB.CellIndex];

            if (cellA.RowIndex > cellB.RowIndex)
            {
                forward = true;
            }
            else
            {
                if (cellA.RowIndex == cellB.RowIndex &&
                    cellA.TabIndex > cellB.TabIndex)
                {
                    forward = true;
                }
            }

            return forward;
        }

        /// <summary>
        /// 移動順番が次のセルを探索
        /// </summary>
        /// <param name="multiRow">探索対象のGCMultiRow</param>
        /// <param name="startPosition">探索を開始するCellPosition</param>
        /// <param name="predicate">探索対象に含めるかの述語</param>
        /// <returns></returns>
        private static CellPosition FindNextMovingOrderCellAsCellPosition(
            GcMultiRow multiRow, CellPosition startPosition, Predicate<Cell> predicate)
        {
            CellPosition position = CellPosition.Empty;

            Cell startCell = multiRow[startPosition.RowIndex, startPosition.CellIndex];

            for (int i = startPosition.RowIndex; i < multiRow.RowCount; i++)
            {
                Row row = multiRow.Rows[i];

                foreach (Cell cell in row.Cells.OrderBy(cell => cell.TabIndex))
                {
                    if (cell.RowIndex == startPosition.RowIndex && cell.TabIndex <= startCell.TabIndex)
                    {
                        continue;
                    }

                    if (cell.Selectable && predicate(cell))
                    {
                        return new CellPosition(cell.RowIndex, cell.CellIndex);
                    }
                }
            }

            return position;
        }

        /// <summary>
        /// 移動順番が前のセルを探索
        /// </summary>
        /// <param name="multiRow">探索対象のGCMultiRow</param>
        /// <param name="startPosition">探索を開始するCellPosition</param>
        /// <param name="predicate">探索対象に含めるかの述語</param>
        /// <returns></returns>
        private static CellPosition FindPreviousMovingOrderCellAsCellPosition(
            GcMultiRow multiRow, CellPosition startPosition, Predicate<Cell> predicate)
        {
            CellPosition position = CellPosition.Empty;

            Cell startCell = multiRow[startPosition.RowIndex, startPosition.CellIndex];

            for (int i = startPosition.RowIndex; i > -1; i--)
            {
                Row row = multiRow.Rows[i];

                foreach (Cell cell in row.Cells.OrderByDescending(cell => cell.TabIndex))
                {

                    if (cell.RowIndex == startPosition.RowIndex && cell.TabIndex >= startCell.TabIndex)
                    {
                        continue;
                    }

                    if (cell.Selectable && predicate(cell))
                    {
                        return new CellPosition(cell.RowIndex, cell.CellIndex);
                    }
                }
            }

            return position;
        }

        #endregion

        /// <summary>
        /// MultiRowの税区分を次にすすめる処理を行うユーザー定義アクションクラスです。
        /// </summary>
        private class MRowTaxKbnToNext : IAction
        {
            #region IAction メンバ

            public bool CanExecute(GcMultiRow target)
            {
                return true;
            }

            public string DisplayName
            {
                get { return this.ToString(); }
            }

            public void Execute(GcMultiRow target)
            {
                TaxKbnToNext(target);
            }

            #endregion

            /// <summary>
            /// 税区分を次の項目に進めます。
            /// </summary>
            static public void TaxKbnToNext(GcMultiRow multiRow)
            {
                if (multiRow.CurrentCell != null)
                {
                    //現在のセル位置を取得
                    GrapeCity.Win.MultiRow.Cell curCell = multiRow.CurrentCell;
                    Row curRow = multiRow.CurrentRow;

                    GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell taxKbnCell;

                    if (curCell.Name == MrowCellKeys.Price.ToString())
                    {
                        if (Convert.ToInt32(multiRow.GetValue(curRow.Index, MrowCellKeys.ContractCd.ToString())) > 0)
                        {
                            return;
                        }

                        taxKbnCell =
                            multiRow[curRow.Index, MrowCellKeys.TaxDispKbn.ToString()] as GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell;
                    }
                    else if (curCell.Name == MrowCellKeys.CharterPrice.ToString())
                    {
                        taxKbnCell =
                            multiRow[curRow.Index, MrowCellKeys.CharterTaxDispKbn.ToString()] as GrapeCity.Win.MultiRow.InputMan.GcComboBoxCell;
                    }
                    else
                    {
                        return;
                    }

                    // this.NextItemOrFirst(taxKbnCell);

                    object value = taxKbnCell.Value;

                    int currentindex = taxKbnCell.FindObject(value, -1, taxKbnCell.ValueSubItemIndex);

                    //次のインデックス
                    int nextIndex;

                    //インデックスが最後の項目か？
                    if (currentindex == -1 || taxKbnCell.Items.Count - 1 <= currentindex)
                    {
                        nextIndex = 0;
                    }
                    else
                    {
                        nextIndex = currentindex + 1;
                    }

                    taxKbnCell.Value = taxKbnCell.Items[nextIndex].SubItems[taxKbnCell.ValueSubItemIndex].Value;
                }
            }
        }

        #region 受注明細 EnterdCellイベント処理

        /// <summary>
        /// 受注明細の積日セルに初期値をセットします。
        /// </summary>
        private void SetInitValueInTaskStartDateCell(int rowIndex)
        {
            //未入力だったら、積日をセット
            if (this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString()) == null)
            {
                //システム日付の月初日をセット
                this.mrsJuchuMeisai.SetValue(rowIndex,
                    MrowCellKeys.TaskStartYMD.ToString(),
                        DateTimeExtensions.FirstDayOfMonth(DateTime.Today));

                //編集モードにする
                EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
            }
        }

        /// <summary>
        /// 受注明細の着日セルに初期値をセットします。
        /// </summary>
        private void SetInitValueInTaskEndDateCell(int rowIndex)
        {
            //未入力だったら、積日をセット
            if (this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString()) == null)
            {
                //参照先がnullでないときのみ設定する
                if (this.mrsJuchuMeisai.GetValue(rowIndex,
                    MrowCellKeys.TaskStartYMD.ToString()) != null)
                {
                    this.mrsJuchuMeisai.SetValue(rowIndex,
                        MrowCellKeys.TaskEndYMD.ToString(),
                            Convert.ToDateTime(
                                this.mrsJuchuMeisai.GetValue(rowIndex,
                                    MrowCellKeys.TaskStartYMD.ToString())));

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
                }
            }
            else
            {
                if (this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString()) != null
                    && Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString()))
                        < Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString())))
                {
                    this.mrsJuchuMeisai.SetValue(rowIndex,
                        MrowCellKeys.ReuseYMD.ToString(),
                            Convert.ToDateTime(
                                this.mrsJuchuMeisai.GetValue(rowIndex,
                                    MrowCellKeys.TaskEndYMD.ToString())));
                }
            }
        }

        /// <summary>
        /// 受注明細の再使用可能日セルに初期値をセットします。
        /// </summary>
        private void SetInitValueInReuseDateCell(int rowIndex)
        {
            //未入力だったら、再使用可能日をセット
            if (this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString()) == null)
            {
                //参照先がnullでないときのみ設定する
                if (this.mrsJuchuMeisai.GetValue(rowIndex,
                    MrowCellKeys.TaskEndYMD.ToString()) != null)
                {
                    this.mrsJuchuMeisai.SetValue(rowIndex,
                        MrowCellKeys.ReuseYMD.ToString(),
                            Convert.ToDateTime(
                                this.mrsJuchuMeisai.GetValue(rowIndex,
                                    MrowCellKeys.TaskEndYMD.ToString())));

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
                }
            }
            else
            {
                if (this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString()) != null
                    && Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString()))
                        < Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ReuseYMD.ToString())))
                {
                    this.mrsJuchuMeisai.SetValue(rowIndex,
                        MrowCellKeys.ReuseYMD.ToString(),
                            Convert.ToDateTime(
                                this.mrsJuchuMeisai.GetValue(rowIndex,
                                    MrowCellKeys.ReuseYMD.ToString())));
                }
            }
        }

        /// <summary>
        /// 受注明細の計上日セルに初期値をセットします。
        /// </summary>
        private void SetInitValueInAddUpDateCell(int rowIndex)
        {
            ////計上日付が未入力の時のみ処理を行う
            //if (this.mrsJuchuMeisai.GetValue(rowIndex,
            //    MrowCellKeys.AddUpYMD.ToString()) == null)
            //{
            //    //未入力だったら、得意先の計上日区分に応じて積日または、着日をセット
            //    //※得意先情報がない場合は積日をセット
            //    int clmdaykbn = (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate;

            //    int tokuisaki_cd =
            //        Convert.ToInt32(
            //            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));

            //    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //    TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetListInternal(null, tokui_para).FirstOrDefault();

            //    if (tokui_info != null)
            //    {
            //        clmdaykbn = tokui_info.ToraDONSaleSlipToClmDayKbn;
            //    }

            //    if (clmdaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate)
            //    {
            //        //積日が未入力でない場合は設定する
            //        if (this.mrsJuchuMeisai.GetValue(
            //            rowIndex, MrowCellKeys.TaskStartYMD.ToString()) != null)
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(),
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString())));

            //            //編集モードにする
            //            EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
            //        }
            //    }
            //    else if (clmdaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate)
            //    {
            //        //着日が未入力でない場合セットする
            //        if (this.mrsJuchuMeisai.GetValue(
            //            rowIndex, MrowCellKeys.TaskEndYMD.ToString()) != null)
            //        {
            //            //着日をセット
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(),
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString())));

            //            //編集モードにする
            //            EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
            //        }
            //    }
            //}

        }

        /// <summary>
        /// 受注明細の傭車計上日セルに初期値をセットします。
        /// </summary>
        private void SetInitValueInCharterAddUpDateCell(int rowIndex)
        {
            ////傭車計上日付が未入力の時のみ処理を行う
            //if (this.mrsJuchuMeisai.GetValue(rowIndex,
            //    MrowCellKeys.CharterAddUpYMD.ToString()) == null)
            //{
            //    //未入力だったら、取引先の傭車計上日区分に応じて計上日、積日、着日のいずれかをセット

            //    int paydaykbn = 0;

            //    int torihiki_cd =
            //        Convert.ToInt32(
            //            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString()));

            //    TorihikisakiSearchParameter torihiki_para = new TorihikisakiSearchParameter() { ToraDONTorihikiCode = torihiki_cd };
            //    TorihikisakiInfo torihiki_info = this._DalUtil.Torihikisaki.GetListInternal(null, torihiki_para).FirstOrDefault();

            //    if (torihiki_info != null)
            //    {
            //        paydaykbn = torihiki_info.SaleSlipToPayDayKbn;
            //    }

            //    if (paydaykbn == 0)
            //    {
            //        //受注計上日が未入力でない場合は設定する
            //        if (this.mrsJuchuMeisai.GetValue(
            //            rowIndex, MrowCellKeys.AddUpYMD.ToString()) != null)
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterAddUpYMD.ToString(),
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString())));

            //            //編集モードにする
            //            EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
            //        }
            //    }

            //    else if (paydaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate)
            //    {
            //        //積日が未入力でない場合は設定する
            //        if (this.mrsJuchuMeisai.GetValue(
            //            rowIndex, MrowCellKeys.TaskStartYMD.ToString()) != null)
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterAddUpYMD.ToString(),
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskStartYMD.ToString())));

            //            //編集モードにする
            //            EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);

            //        }
            //    }

            //    else if (paydaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate)
            //    {
            //        //着日が未入力でない場合セットする
            //        if (this.mrsJuchuMeisai.GetValue(
            //            rowIndex, MrowCellKeys.TaskEndYMD.ToString()) != null)
            //        {

            //            //着日をセット
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterAddUpYMD.ToString(),
            //                Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TaskEndYMD.ToString().ToString())));

            //            //編集モードにする
            //            EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);

            //        }
            //    }
            //}
        
        }

        /// <summary>
        /// 受注明細の傭車計上日セルに初期値をセットします。
        /// </summary>
        private int[] GetSpanDateTime(DateTime startYMD, DateTime endYMD)
        {
            int[] rt_val = new int[2] { 0, 0 };

            if (endYMD < startYMD)
            {
                return rt_val;
            }

            rt_val[0] = Convert.ToInt32((endYMD - startYMD).TotalDays);

            int hh = Convert.ToInt32((endYMD - startYMD).TotalHours % 24);
            int mm = Convert.ToInt32((endYMD - startYMD).TotalMinutes % 60);
            rt_val[1] = hh * 10000 + mm * 100;

            return rt_val;
        }

        #endregion

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
                    if (mrsJuchuMeisai.ContainsFocus)
                    {
                        //キーボードイベントの抑止
                        e.Handled = true;
                        this.ShowCmnSearchJuchuNyuryokuList(this.mrsJuchuMeisai);
                    }
                    break;
                default:
                    break;
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
            //if (null == this.cmbBranchOffice.SelectedValue)
            //{
            //    //イベントをキャンセル
            //    e.Cancel = true;
            //    return;
            //}

            ////営業所情報を取得
            //ToraDONBranchOfficeSearchParameter para = new ToraDONBranchOfficeSearchParameter();
            //para.BranchOfficeCode = Convert.ToInt32(this.cmbBranchOffice.SelectedValue);
            //ToraDONBranchOfficeInfo info = this._DalUtil.ToraDONBranchOffice.GetList(para).FirstOrDefault();

            //if (info == null)
            //{
            //    //イベントをキャンセル
            //    e.Cancel = true;
            //    return;
            //}

            //this.cmbBranchOffice.Tag = info.BranchOfficeId;
        }

        #endregion

        #region CellValidatingの処理

        /// <summary>
        /// 明細一覧のMrowのCellValidatingイベントを処理します。
        /// </summary>
        /// <param name="e">CellValidatingイベントのイベントデータ</param>
        private void ProcessJuchuNyuryokuMeisaiMrowCellValidating(CellValidatingEventArgs e)
        {
            //編集された場合のみ処理する
            if (!this.mrsJuchuMeisai.IsCurrentCellInEditMode)
            {
                return;
            }

            //描画を止める
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {
                //現在アクティブなセルのキーを取得
                string wk_curcellkey = this.mrsJuchuMeisai.CurrentCell.Name;

                // 販路コード
                if (wk_curcellkey == MrowCellKeys.HanroCd.ToString())
                {
                    e.Cancel = this.HanroCdCellValidating(e.RowIndex);
                }

                // 得意先コード
                else if (wk_curcellkey == MrowCellKeys.TokuisakiCd.ToString())
                {
                    e.Cancel = this.TokuisakiCdCellValidating(e.RowIndex);
                }

                // 請求部門
                else if (wk_curcellkey == MrowCellKeys.ClmClassCd.ToString())
                {
                    e.Cancel = this.ClmClassCdCellValidating(e.RowIndex);
                }

                // 請負コード
                else if (wk_curcellkey == MrowCellKeys.ContractCd.ToString())
                {
                    e.Cancel = this.ContractCdCellValidating(e.RowIndex);
                }

                // 積地コード
                else if (wk_curcellkey == MrowCellKeys.StartPointCd.ToString())
                {
                    e.Cancel = this.StartPointCdCellValidating(e.RowIndex);
                }

                // 着地コード
                else if (wk_curcellkey == MrowCellKeys.EndPointCd.ToString())
                {
                    e.Cancel = this.EndPointCdCellValidating(e.RowIndex);
                }

                // 品目コード
                else if (wk_curcellkey == MrowCellKeys.ItemCd.ToString())
                {
                    e.Cancel = this.ItemCdCellValidating(e.RowIndex);
                }

                // 数量
                else if (wk_curcellkey == MrowCellKeys.Number.ToString())
                {
                    e.Cancel = this.NumberCellValidating(e.RowIndex);
                }

                // 単位コード
                else if (wk_curcellkey == MrowCellKeys.FigCd.ToString())
                {
                    e.Cancel = this.FigCdCellValidating(e.RowIndex);
                }

                // 車両コード
                else if (wk_curcellkey == MrowCellKeys.CarCd.ToString())
                {
                    e.Cancel = this.CarCdCellValidating(e.RowIndex);
                }

                // 車種コード
                else if (wk_curcellkey == MrowCellKeys.CarKindCd.ToString())
                {
                    e.Cancel = this.CarKindCdCellValidating(e.RowIndex);
                }

                // 乗務員コード
                else if (wk_curcellkey == MrowCellKeys.DriverCd.ToString())
                {
                    e.Cancel = this.DriverCdCellValidating(e.RowIndex);
                }
                // 傭車先コード
                else if (wk_curcellkey == MrowCellKeys.CarOfChartererCd.ToString())
                {
                    e.Cancel = this.CarOfChartererCdCellValidating(e.RowIndex);
                }

                // 単価
                else if (wk_curcellkey == MrowCellKeys.AtPrice.ToString())
                {
                    e.Cancel = this.AtPriceCellValidating(e.RowIndex);
                }

                // 荷主
                else if (wk_curcellkey == MrowCellKeys.OwnerCd.ToString())
                {
                    e.Cancel = this.OwnerCdCellValidating(e.RowIndex);
                }

                // 金額
                else if (wk_curcellkey == MrowCellKeys.Price.ToString())
                {
                    e.Cancel = this.PriceCellValidating(e.RowIndex);
                }

                // 金額_通行料
                else if (wk_curcellkey == MrowCellKeys.TollFeeInPrice.ToString())
                {
                    e.Cancel = this.TollFeeInPriceCellValidating(e.RowIndex);
                }

                // 傭車金額
                else if (wk_curcellkey == MrowCellKeys.CharterPrice.ToString())
                {
                    e.Cancel = this.CharterPriceCellValidating(e.RowIndex);
                }

                // 傭車金額_通行料
                else if (wk_curcellkey == MrowCellKeys.TollFeeInCharterPrice.ToString())
                {
                    e.Cancel = this.TollFeeInCharterPriceCellValidating(e.RowIndex);
                }

                // 開始日付
                else if (wk_curcellkey == MrowCellKeys.TaskStartYMD.ToString())
                {
                    e.Cancel = this.TaskStartYMDCellValidating(e.RowIndex);
                }

                // 開始時刻
                else if (wk_curcellkey == MrowCellKeys.TaskStartHM.ToString())
                {
                    e.Cancel = this.TaskStartHMCellValidating(e.RowIndex);
                }

                // 終了日付
                else if (wk_curcellkey == MrowCellKeys.TaskEndYMD.ToString())
                {
                    e.Cancel = this.TaskEndYMDCellValidating(e.RowIndex);
                }

                // 終了時刻
                else if (wk_curcellkey == MrowCellKeys.TaskEndHM.ToString())
                {
                    e.Cancel = this.TaskEndHMCellValidating(e.RowIndex);
                }

                // 受注担当コード
                else if (wk_curcellkey == MrowCellKeys.JuchuTantoCd.ToString())
                {
                    e.Cancel = this.JuchuTantoCdCellValidating(e.RowIndex);
                }

                // 金額_附帯業務料
                else if (wk_curcellkey == MrowCellKeys.FutaigyomuryoInPrice.ToString())
                {
                    e.Cancel = this.FutaigyomuryoInPriceCellValidating(e.RowIndex);
                }

                if (e.Cancel)
                {
                    //クリア
                    EditingActions.ClearEdit.Execute(this.mrsJuchuMeisai);

                    //前の値に戻す前にいったん確定する
                    EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

                    //編集モードにする
                    EditingActions.BeginEdit.Execute(this.mrsJuchuMeisai);
                }
            }
            finally
            {
                //描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }
        }

        #region CellEditedFormattedValueChangedの処理

        /// <summary>
        /// 明細一覧のMrowのCellEditedFormattedValueChangedイベントを処理します。
        /// </summary>
        /// <param name="e">CellEditedFormattedValueChangedイベントのイベントデータ</param>
        private void ProcessJuchuNyuryokuMeisaiMrowCellEditedFormattedValueChanged(CellEditedFormattedValueChangedEventArgs e)
        {
            //描画を止める
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {
                Cell currentCell = this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[e.CellIndex];

                if (e.Scope == CellScope.Row)
                {
                    //乗務員売上同額フラグの場合
                    if (currentCell is CheckBoxCell
                        && currentCell.Name.Equals(MrowCellKeys.JomuinUriageDogakuFlag.ToString()))
                    {
                        //セル値
                        bool cell_value =
                            Convert.ToBoolean(currentCell.EditedFormattedValue);

                        GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[e.RowIndex];

                        //チェックされた場合
                        if (cell_value)
                        {
                            //金額を乗務員売上金額に設定
                            this.mrsJuchuMeisai.SetValue(e.RowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(),
                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.Price.ToString())));

                            //編集不可
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], true);
                        }
                        else
                        {
                            //編集可
                            this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], false);
                        }
                    }
                }
            }
            finally
            {
                //描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }
        }

        #endregion

        /// <summary>
        /// 受注明細の販路コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool HanroCdCellValidating(int rowIndex, bool ikkatsuFlg = false)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    bool valueChanged;

            //    if (ikkatsuFlg)
            //    {
            //        //一括設定時
            //        valueChanged = true;
            //    }
            //    else
            //    {
            //        //値が変更されたか？
            //        valueChanged = cell_code != editing_code;
            //    }


            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //販路情報を取得
            //            HanroSearchParameter para = new HanroSearchParameter() { HanroCode = editing_code };
            //            HanroInfo info = this._DalUtil.Hanro.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //編集エラー出なければ販路情報セット
            //                if (!rt_val)
            //                {
            //                    //未使用データの場合は、エラー
            //                    if (info.DisableFlag)
            //                    {
            //                        //使用可否エラー
            //                        DialogResult mq_result = MessageBox.Show(
            //                            FrameUtilites.GetDefineMessage("MW2201016"),
            //                            "警告",
            //                            MessageBoxButtons.OK,
            //                            MessageBoxIcon.Warning);
            //                        //情報クリア
            //                        rt_val = true;
            //                        is_clear = true;
            //                    }
            //                    else
            //                    {
            //                        //IDセット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Tag = info.HanroId;
            //                        //名称セット
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroName.ToString(), info.HanroName);
            //                        //往復区分セット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Tag = info.OfukuKbn;
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.OfukuButton.ToString(), info.OfukuKbnName);
            //                        //単価セット
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), info.SeikyuTanka);
            //                        //業務料セット
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString(), info.Futaigyomuryo);

            //                        //得意先情報を取得
            //                        if (0 < info.JoinToraDONTokuisakiId)
            //                        {
            //                            if (!info.ToraDONTokuisakiDisableFlag)
            //                            {
            //                                //得意先コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString(), info.ToraDONTokuisakiCode);
            //                                //得意先ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = info.JoinToraDONTokuisakiId;
            //                                //得意先名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiNM.ToString(), info.ToraDONTokuisakiShortName);

            //                                if (NSKUtil.IntToBool(info.ToraDONTokuisakiMemoAccount))
            //                                {
            //                                    //諸口の場合は、得意先名を編集可に設定
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TokuisakiNM.ToString()], false);
            //                                }
            //                                else
            //                                {
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TokuisakiNM.ToString()], true);
            //                                }

            //                                if (NSKUtil.IntToBool(info.ToraDONTokuisakiClmClassUseKbn))
            //                                {
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.ClmClassCd.ToString()], false);
            //                                }
            //                                else
            //                                {
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.ClmClassCd.ToString()], true);
            //                                }

            //                                //関連をクリア
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassCd.ToString(), 0);
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = null;
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), string.Empty);

            //                                int code = Convert.ToInt32(mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ContractCd.ToString()));

            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractCd.ToString(), 0);
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = null;
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractNM.ToString(), string.Empty);

            //                                //請負コードが0でない時のみクリア、編集可にする
            //                                if (code != 0)
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaxDispKbn.ToString(), 1);
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(), null);
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TaxDispKbn.ToString()], false);
            //                                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.AddUpYMD.ToString()], false);
            //                                }
            //                            }
            //                        }

            //                        //発着地情報を取得
            //                        if (0 < info.JoinToraDONHatchiId)
            //                        {
            //                            if (!info.ToraDONHatchiDisableFlag)
            //                            {
            //                                //積地コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointCd.ToString(), info.ToraDONHatchiCode);
            //                                //積地ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.StartPointCd.ToString()].Tag = info.JoinToraDONHatchiId;
            //                                //積地名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointNM.ToString(), info.ToraDONHatchiName);
            //                            }
            //                        }

            //                        //発着地情報を取得
            //                        if (0 < info.JoinToraDONChakuchiId)
            //                        {
            //                            if (!info.ToraDONChakuchiDisableFlag)
            //                            {
            //                                //着地コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.EndPointCd.ToString(), info.ToraDONChakuchiCode);
            //                                //着地ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.EndPointCd.ToString()].Tag = info.JoinToraDONChakuchiId;
            //                                //着地名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.EndPointNM.ToString(), info.ToraDONChakuchiName);
            //                            }
            //                        }

            //                        //品目情報を取得
            //                        if (0 < info.JoinToraDONItemId)
            //                        {
            //                            if (!info.ToraDONItemDisableFlag)
            //                            {
            //                                //品目コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ItemCd.ToString(), info.ToraDONItemCode);
            //                                //品目ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ItemCd.ToString()].Tag = info.JoinToraDONItemId;
            //                                //品目名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ItemNM.ToString(), info.ToraDONItemName);

            //                                //単位情報を取得
            //                                if (0 < info.JoinToraDONFigId)
            //                                {
            //                                    //IDをセット
            //                                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = info.JoinToraDONFigId;
            //                                    this.mrsJuchuMeisai.SetValue(
            //                                        rowIndex, MrowCellKeys.FigCd.ToString(), info.ToraDONFigCode);
            //                                    this.mrsJuchuMeisai.SetValue(
            //                                        rowIndex, MrowCellKeys.FigNM.ToString(), info.ToraDONFigName);

            //                                }

            //                                this.mrsJuchuMeisai.SetValue(
            //                                    rowIndex, MrowCellKeys.TaxDispKbn.ToString(), info.ToraDONItemItemTaxKbn);

            //                                this.mrsJuchuMeisai.SetValue(
            //                                    rowIndex, MrowCellKeys.CharterTaxDispKbn.ToString(), info.ToraDONItemItemTaxKbn);
            //                            }
            //                        }

            //                        //車両情報を取得
            //                        CarSearchParameter car_para = new CarSearchParameter() { ToraDONCarId = info.ToraDONCarId };
            //                        CarInfo car_info = this._DalUtil.Car.GetList(car_para).FirstOrDefault();
            //                        bool yoshakingakusetflag = false;
            //                        if (null != car_info)
            //                        {
            //                            if (!car_info.ToraDONDisableFlag)
            //                            {
            //                                //車両コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarCd.ToString(), car_info.ToraDONCarCode);
            //                                //車両ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarCd.ToString()].Tag = car_info.ToraDONCarId;
            //                                if (car_info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), string.Empty);
            //                                    //傭車区分
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(),
            //                                        FrameUtilites.GetSystemName(DefaultProperty.SystemNameKbn.CarKbn, (int)DefaultProperty.CarKbn.Yosha));
            //                                    //傭車金額
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterPrice.ToString(), info.YoshaKingaku);
            //                                    yoshakingakusetflag = true;

            //                                    //通行料
            //                                    decimal tollFeeInCharterPrice =
            //                                        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString()));

            //                                    //傭車確定フラグ
            //                                    bool wk_charterfixflag = false;
            //                                    if (info.YoshaKingaku != 0 || tollFeeInCharterPrice != 0)
            //                                    {
            //                                        wk_charterfixflag = true;
            //                                    }
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterFixFlag.ToString(), wk_charterfixflag);
            //                                }
            //                                else
            //                                {
            //                                    //車番
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), car_info.LicPlateCarNo);
            //                                    //傭車区分
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(), car_info.BranchOfficeShortName);
            //                                    //傭車金額
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterPrice.ToString(), 0);

            //                                    //通行料
            //                                    decimal tollFeeInCharterPrice =
            //                                        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString()));

            //                                    //傭車確定フラグ
            //                                    bool wk_charterfixflag = false;
            //                                    if (tollFeeInCharterPrice != 0)
            //                                    {
            //                                        wk_charterfixflag = true;
            //                                    }
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterFixFlag.ToString(), wk_charterfixflag);
            //                                }

            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(), this.GetCharterKbnValue(car_info));
            //                                //車両区分
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKbn.ToString(), car_info.CarKbn);

            //                                //乗務員情報を取得
            //                                StaffSearchParameter staff_para = new StaffSearchParameter() { ToraDONStaffId = car_info.DriverId };
            //                                StaffInfo staff_info = this._DalUtil.Staff.GetList(staff_para).FirstOrDefault();
            //                                if (null != staff_info && staff_info.ToraDONDisableFlag)
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), staff_info.ToraDONStaffCode);
            //                                    //IDをセット
            //                                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.DriverCd.ToString()].Tag = staff_info.ToraDONStaffId;
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), staff_info.ToraDONStaffName);
            //                                }

            //                                //傭車かそうでないかで有効・無効を切り替えるが無効になった場合は値をクリアするのでその判断
            //                                if (this.CarKbnIsCharter(car_info.CarKbn))
            //                                {
            //                                    this.ClearDriverValuesForCarKbn(rowIndex);
            //                                }
            //                                else
            //                                {
            //                                    this.ClearCharterValuesForCarKbn(rowIndex);
            //                                }

            //                                this.RefreshStyleForCarKbn(rowIndex, car_info.CarKbn);
            //                            }
            //                        }

            //                        //車種情報を取得
            //                        if (0 < info.JoinToraDONCarKindId)
            //                        {
            //                            if (!info.ToraDONCarKindDisableFlag)
            //                            {
            //                                //車種コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindCd.ToString(), info.ToraDONCarKindCode);
            //                                //車種ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = info.JoinToraDONCarKindId;
            //                                //車種名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindName.ToString(), info.ToraDONCarKindName);
            //                            }
            //                        }

            //                        //傭車先情報を取得
            //                        if (0 < info.JoinToraDONTorihikiId)
            //                        {
            //                            if (!info.ToraDONTorihikiDisableFlag)
            //                            {
            //                                //傭車先コード
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), info.ToraDONTorihikiCode);
            //                                //傭車先ID
            //                                this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = info.JoinToraDONTorihikiId;
            //                                //傭車先名
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), info.ToraDONTorihikiShortName);
            //                            }
            //                        }

            //                        //単価マスタから単価を取得
            //                        int tokuisaki_cd = Convert.ToInt32(
            //                            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                        int item_cd =
            //                            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                        int startpoint_cd = Convert.ToInt32(
            //                            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                        int endpoint_cd = Convert.ToInt32(
            //                            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                        int fig_cd = Convert.ToInt32(
            //                            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));
            //                        int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

            //                        decimal atprice = 0;
            //                        if (0 < info.SeikyuTanka)
            //                        {
            //                            atprice = info.SeikyuTanka;
            //                        }
            //                        else
            //                        {
            //                            this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, startpoint_cd, endpoint_cd, fig_cd, carkind_cd);
            //                        }

            //                        //取得できたら金額を計算
            //                        if (atprice != 0)
            //                        {
            //                            //得意先の金額丸め区分を取得
            //                            int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                            //得意先情報を取得
            //                            if (0 < info.JoinToraDONTokuisakiId)
            //                            {
            //                                cutkbn = info.ToraDONTokuisakiGakCutKbn;
            //                            }

            //                            //数量
            //                            decimal number =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));

            //                            //金額計算
            //                            decimal wk_price = 0;
            //                            if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                            {
            //                                //単価・金額セット
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //                                if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //                                }
                                            
            //                                //通行料
            //                                decimal tollFee =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                                //附帯業務料
            //                                decimal futaigyomuryo =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                                //金額によって確定フラグ変更
            //                                bool fixflag = false;
            //                                if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                                {
            //                                    fixflag = true;
            //                                }
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                                if (!yoshakingakusetflag)
            //                                {
            //                                    //入力値を確定
            //                                    EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
            //                                    //傭車金額の計算
            //                                    this.CalcCharterFeeJuchuDtl(rowIndex);
            //                                }

            //                                //合計金額を再計算
            //                                this.SetPriceTotal();
            //                            }
            //                            else
            //                            {
            //                                //金額計算に失敗したら、クリアして編集をキャンセル
            //                                rt_val = true;
            //                                is_clear = true;
            //                            }
            //                        }

            //                        //積日を取得
            //                        DateTime TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskStartYMD.ToString()));

            //                        //積時間を取得
            //                        DateTime TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskStartHM.ToString()));

            //                        //着日を取得
            //                        DateTime TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                        //着時間を取得
            //                        DateTime TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                        //販路情報を取得
            //                        HanroSearchParameter hanro_para = new HanroSearchParameter() { HanroCode = editing_code };
            //                        HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

            //                        if (hanro_info != null && 0 < hanro_info.HanroId)
            //                        {
            //                            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //                                == (int)DefaultProperty.JuchuYMDCalculationKbn.TsumiToChaku)
            //                            {
            //                                //販路情報及び積日から着日を設定
            //                                this.SetTaskEndYMDHM(rowIndex, hanro_info, TaskStartYMD, TaskStartHM, TaskEndYMD, TaskEndHM);
            //                            }
            //                            else if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //                                == (int)DefaultProperty.JuchuYMDCalculationKbn.ChakuToTsumi)
            //                            {
            //                                //販路情報及び着日から積日を設定
            //                                this.SetTaskStartYMDHM(rowIndex, hanro_info, TaskStartYMD, TaskStartHM, TaskEndYMD, TaskEndHM);
            //                            }

            //                            //着日を再取得
            //                            TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                    rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                            //着時間を再取得
            //                            TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                    rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                            //再使用可能日を取得
            //                            DateTime ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                    rowIndex, MrowCellKeys.ReuseYMD.ToString()));

            //                            //再使用可能時間を取得
            //                            DateTime ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                    rowIndex, MrowCellKeys.ReuseHM.ToString()));

            //                            //販路情報及び着日から再使用可能日を設定
            //                            this.SetReuseYMDHM(rowIndex, hanro_info, TaskEndYMD, TaskEndHM, ReuseYMD, ReuseHM);
            //                        }

            //                        //設定後の積日を取得
            //                        DateTime TaskStartYMDAfter = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskStartYMD.ToString()));

            //                        //積日が変更された場合、計上日の処理を行う
            //                        if (TaskStartYMD != TaskStartYMDAfter)
            //                        {
            //                            //計上日が未入力の場合
            //                            if (DateTime.MinValue == Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString())))
            //                            {
            //                                //※得意先情報がない場合、もしくは得意先の計上日区分が"積日"の場合は積日をセット
            //                                int clmdaykbn = (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate;

            //                                if (0 < info.JoinToraDONTokuisakiId)
            //                                {
            //                                    clmdaykbn = info.ToraDONTokuisakiSaleSlipToClmDayKbn;
            //                                }

            //                                if (clmdaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate)
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(), TaskStartYMDAfter);
            //                                }
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroName.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の得意先コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TokuisakiCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            try
            {
                //セル値
                int cell_code =
                    Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
                //編集値
                int editing_code =
                    Convert.ToInt32(
                        GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

                //値が変更されたか？
                bool valueChanged = cell_code != editing_code;

                //if (valueChanged)
                //{
                //    if (editing_code == 0)
                //    {
                //        //未入力時はクリアのみ
                //        is_clear = true;
                //    }
                //    else
                //    {
                //        //得意先情報を取得
                //        TokuisakiSearchParameter para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = editing_code };
                //        TokuisakiInfo info = this._DalUtil.Tokuisaki.GetList(para).FirstOrDefault();

                //        if (info == null)
                //        {
                //            //編集をキャンセル
                //            rt_val = true;

                //            MessageBox.Show(
                //               FrameUtilites.GetDefineMessage("MW2201003"),
                //               "警告",
                //               MessageBoxButtons.OK,
                //               MessageBoxIcon.Warning);

                //            //得意先情報クリア
                //            is_clear = true;
                //        }
                //        else
                //        {
                //            ////未使用データの場合は、エラー
                //            //if (info.ToraDONDisableFlag)
                //            //{
                //            //    //使用可否エラー
                //            //    DialogResult mq_result = MessageBox.Show(
                //            //        FrameUtilites.GetDefineMessage("MW2201016"),
                //            //        "警告",
                //            //        MessageBoxButtons.OK,
                //            //        MessageBoxIcon.Warning);
                //            //    //情報クリア
                //            //    rt_val = true;
                //            //    is_clear = true;
                //            //}

                //            ////単価マスタから単価を取得
                //            //if (!rt_val)
                //            //{
                //            //    //単価マスタから単価を取得
                //            //    int item_cd =
                //            //        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
                //            //    int startpoint_cd =
                //            //        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
                //            //    int endpoint_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
                //            //    int fig_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));
                //            //    int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

                //            //    decimal atprice =
                //            //        this.GetAtPriceByJuchuDtlKey(rowIndex, editing_code, item_cd, startpoint_cd, endpoint_cd, fig_cd, carkind_cd);

                //            //    //数量を取得
                //            //    decimal number =
                //            //        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));
                //            //    //金額計算
                //            //    decimal wk_price = 0;
                //            //    //※金額丸め区分が関係する為、金額計算はしておく
                //            //    if (this.CalcPrice(number, atprice, info.ToraDONGakCutKbn, out wk_price))
                //            //    {
                //            //        //単価・金額セット
                //            //        if (atprice != 0)
                //            //        {
                //            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
                //            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
                //            //            if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
                //            //            {
                //            //                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
                //            //            }

                //            //            //通行料
                //            //            decimal tollFee =
                //            //                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

                //            //            //附帯業務料
                //            //            decimal futaigyomuryo =
                //            //                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

                //            //            //金額によって確定フラグ変更
                //            //            bool fixflag = false;
                //            //            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
                //            //            {
                //            //                fixflag = true;
                //            //            }
                //            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

                //            //            //入力値を確定
                //            //            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
                //            //            //傭車金額の計算
                //            //            this.CalcCharterFeeJuchuDtl(rowIndex);

                //            //            //合計金額を再計算
                //            //            this.SetPriceTotal();
                //            //        }
                //            //    }
                //            //    else
                //            //    {
                //            //        //金額計算に失敗したら、クリアして編集をキャンセル
                //            //        rt_val = true;
                //            //        is_clear = true;
                //            //    }
                //            //}

                //            ////編集エラー出なければ得意先情報セット
                //            //if (!rt_val)
                //            //{
                //            //    //得意先名セット
                //            //    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiNM.ToString(), info.ToraDONTokuisakiShortName);
                //            //    //得意先名ID
                //            //    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = info.ToraDONTokuisakiId;

                //            //    //販路情報を取得
                //            //    HanroSearchParameter hanro_para = new HanroSearchParameter()
                //            //    {
                //            //        HanroCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.HanroCd.ToString()))
                //            //    };
                //            //    HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

                //            //    //販路情報が存在し、得意先IDが一致しない場合
                //            //    if (null != hanro_info && hanro_info.ToraDONTokuisakiId != info.ToraDONTokuisakiId)
                //            //    {
                //            //        //情報クリア
                //            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroCd.ToString(), 0);
                //            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Tag = null;
                //            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.HanroName.ToString(), string.Empty);
                //            //    }

                //            //    //編集エラー出なけれ入力編集情報セット
                //            //    if (!rt_val)
                //            //    {
                //            //        if (NSKUtil.IntToBool(info.ToraDONMemoAccount))
                //            //        {
                //            //            //諸口の場合は、得意先名を編集可に設定
                //            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TokuisakiNM.ToString()], false);
                //            //        }
                //            //        else
                //            //        {
                //            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TokuisakiNM.ToString()], true);
                //            //        }

                //            //        if (NSKUtil.IntToBool(info.ToraDONClmClassUseKbn))
                //            //        {
                //            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.ClmClassCd.ToString()], false);
                //            //        }
                //            //        else
                //            //        {
                //            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.ClmClassCd.ToString()], true);
                //            //        }
                //            //    }
                //            //}
                //        }
                //    }

                //    //関連をクリア
                //    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassCd.ToString(), 0);
                //    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = null;
                //    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), string.Empty);

                //    int code = Convert.ToInt32(mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ContractCd.ToString()));

                //    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractCd.ToString(), 0);
                //    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = null;
                //    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractNM.ToString(), string.Empty);

                //    //請負コードが0でない時のみクリア、編集可にする
                //    if (code != 0)
                //    {
                //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaxDispKbn.ToString(), 1);
                //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(), null);
                //        this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TaxDispKbn.ToString()], false);
                //        this.OriginalCellLocked(wk_curmrow[MrowCellKeys.AddUpYMD.ToString()], false);
                //    }
                //}
           
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                if (is_clear)
                {
                    //得意先情報クリア
                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TokuisakiNM.ToString(), string.Empty);
                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag = null;
                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TokuisakiNM.ToString()], true);
                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.ClmClassCd.ToString()], true);
                }
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の請求部門コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool ClmClassCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //請求部門情報を取得
            //            ToraDONClmClassSearchParameter para = new ToraDONClmClassSearchParameter();
            //            para.ClmClassCode = editing_code;
            //            ToraDONClmClassInfo info = this._DalUtil.ToraDONClmClass.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //入力中の得意先
            //                TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            //                tokui_para.ToraDONTokuisakiCode = Convert.ToDecimal( this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                TokuisakiInfo tokuisakiInfo = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //                //請求部門の得意先と入力されている得意先が不一致か？
            //                if (tokuisakiInfo == null || (info.TokuisakiId != 0 && info.TokuisakiId != tokuisakiInfo.ToraDONTokuisakiId))
            //                {
            //                    //編集をキャンセル
            //                    rt_val = true;
            //                    is_clear = true;

            //                    MessageBox.Show(
            //                    FrameUtilites.GetDefineMessage("MW2203076"),
            //                    "警告",
            //                    MessageBoxButtons.OK,
            //                    MessageBoxIcon.Warning);
            //                }

            //                //編集エラー出なければ請求部門情報セット
            //                if (!rt_val)
            //                {
            //                    //未使用データの場合は、エラー
            //                    if (info.DisableFlag)
            //                    {
            //                        //確認ダイアログ
            //                        DialogResult d_result =
            //                            MessageBox.Show(
            //                            FrameUtilites.GetDefineMessage("MW2203075"),
            //                            "確認",
            //                            MessageBoxButtons.YesNo,
            //                            MessageBoxIcon.Question,
            //                            MessageBoxDefaultButton.Button1);

            //                        //Noだったら抜ける
            //                        if (d_result == DialogResult.No)
            //                        {
            //                            //情報クリア
            //                            rt_val = true;
            //                            is_clear = true;
            //                        }
                                    
            //                        //IDセット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = info.ClmClassId;
            //                        //名称セット
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), info.ClmClassName);
            //                    }
            //                    else
            //                    {
            //                        //IDセット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = info.ClmClassId;
            //                        //名称セット
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), info.ClmClassName);
            //                    }

            //                }
            //            }
            //        }

            //        //関連情報をクリア
            //        int code = Convert.ToInt32(mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ContractCd.ToString()));

            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractNM.ToString(), string.Empty);

            //        //請負コードが0でない時のみクリア、編集可にする
            //        if (code != 0)
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TaxDispKbn.ToString(), 1);
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AddUpYMD.ToString(), null);
            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TaxDispKbn.ToString()], false);
            //            this.OriginalCellLocked(wk_curmrow[MrowCellKeys.AddUpYMD.ToString()], false);
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //情報クリア
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ClmClassNM.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の請負コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool ContractCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //得意先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //請負情報を取得
            //            ToraDONContractSearchParameter para = new ToraDONContractSearchParameter();
            //            para.TokuisakiId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.TokuisakiCd.ToString()].Tag);
            //            para.ClmClassId = Convert.ToDecimal(this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ClmClassCd.ToString()].Tag);
            //            para.ContractCode = editing_code;
            //            ToraDONContractInfo info = this._DalUtil.ToraDONContract.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //編集エラー出なければ請負情報セット
            //                if (!rt_val)
            //                {
            //                    //名称セット
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.ContractNM.ToString(), info.ContractName);

            //                    //IDセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = info.ContractId;

            //                    //税区分
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.TaxDispKbn.ToString(), info.TaxDispKbn);

            //                    //計上日
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.AddUpYMD.ToString(), info.AddUpDate);

            //                    //編集不可にする
            //                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TaxDispKbn.ToString()], true);
            //                    this.OriginalCellLocked(wk_curmrow[MrowCellKeys.AddUpYMD.ToString()], true);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.ContractNM.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ContractCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.TaxDispKbn.ToString(), 1);
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.AddUpYMD.ToString(), null);
            //        //編集可にする
            //        this.OriginalCellLocked(wk_curmrow[MrowCellKeys.TaxDispKbn.ToString()], false);
            //        this.OriginalCellLocked(wk_curmrow[MrowCellKeys.AddUpYMD.ToString()], false);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の積地コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool StartPointCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            //MRow wk_curmrow = this.mrsJuchuMeisai.MRows[wk_currowidx];

            //積地情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //積地情報を取得
            //            PointSearchParameter para = new PointSearchParameter() { ToraDONPointCode = editing_code };
            //            PointInfo info = this._DalUtil.Point.GetListInternal(null, para).FirstOrDefault();


            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //エラーがなければ、単価マスタから単価を取得
            //                if (!rt_val)
            //                {
            //                    //単価マスタから単価を取得
            //                    int tokuisaki_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                    int item_cd =
            //                        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                    int endpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                    int fig_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));
            //                    int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

            //                    decimal atprice =
            //                        this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, editing_code, endpoint_cd, fig_cd, carkind_cd);

            //                    //取得できたら金額を計算
            //                    if (atprice != 0)
            //                    {
            //                        //得意先の金額丸め区分を取得
            //                        int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                        //得意先情報を取得
            //                        TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                        TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();
            //                        if (tokui_info != null)
            //                        {
            //                            cutkbn = tokui_info.ToraDONGakCutKbn;
            //                        }

            //                        //数量
            //                        decimal number =
            //                            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));

            //                        //金額計算
            //                        decimal wk_price = 0;
            //                        if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                        {
            //                            //単価・金額セット
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //                            if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //                            {
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //                            }

            //                            //通行料
            //                            decimal tollFee =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                            //附帯業務料
            //                            decimal futaigyomuryo =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                            //金額によって確定フラグ変更
            //                            bool fixflag = false;
            //                            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                            {
            //                                fixflag = true;
            //                            }
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                            //入力値を確定
            //                            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
            //                            //傭車金額の計算
            //                            this.CalcCharterFeeJuchuDtl(rowIndex);

            //                            //合計金額を再計算
            //                            this.SetPriceTotal();
            //                        }
            //                        else
            //                        {
            //                            //金額計算に失敗したら、クリアして編集をキャンセル
            //                            rt_val = true;
            //                            is_clear = true;
            //                        }
            //                    }
            //                }

            //                //編集した内容に問題がなければ積地名をセット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointNM.ToString(), info.ToraDONPointName);
            //                    //積地ID
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.StartPointCd.ToString()].Tag = info.ToraDONPointId;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //積地情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.StartPointNM.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.StartPointCd.ToString()].Tag = null;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の着地コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool EndPointCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //着地情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //着地情報を取得
            //            PointSearchParameter para = new PointSearchParameter() { ToraDONPointCode = editing_code };
            //            PointInfo info = this._DalUtil.Point.GetListInternal(null, para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //着地情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //単価マスタから単価を取得
            //                if (!rt_val)
            //                {
            //                    //単価マスタから単価を取得
            //                    int tokuisaki_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                    int item_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                    int startpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                    int fig_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));
            //                    int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

            //                    decimal atprice =
            //                        this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, startpoint_cd, editing_code, fig_cd, carkind_cd);

            //                    //取得できたら金額を計算
            //                    if (atprice != 0)
            //                    {
            //                        //得意先の金額丸め区分を取得
            //                        int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                        //得意先情報を取得
            //                        TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                        TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();
            //                        if (tokui_info != null)
            //                        {
            //                            cutkbn = tokui_info.ToraDONGakCutKbn;
            //                        }

            //                        //数量
            //                        decimal number =
            //                            Convert.ToDecimal(
            //                                this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));

            //                        //金額計算
            //                        decimal wk_price = 0;

            //                        if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                        {
            //                            //単価・金額セット
            //                            this.mrsJuchuMeisai.SetValue(
            //                                rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                            this.mrsJuchuMeisai.SetValue(
            //                                rowIndex, MrowCellKeys.Price.ToString(), wk_price);

            //                            //通行料
            //                            decimal tollFee =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                            //附帯業務料
            //                            decimal futaigyomuryo =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                            //金額によって確定フラグ変更
            //                            bool fixflag = false;
            //                            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                            {
            //                                fixflag = true;
            //                            }
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                            //入力値を確定
            //                            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
            //                            //傭車金額の計算
            //                            this.CalcCharterFeeJuchuDtl(rowIndex);

            //                            //合計金額を再計算
            //                            this.SetPriceTotal();
            //                        }
            //                        else
            //                        {
            //                            //金額計算に失敗したら、クリアして編集をキャンセル
            //                            rt_val = true;
            //                            is_clear = true;
            //                        }
            //                    }
            //                }

            //                //編集した内容に問題が無ければ着地名をセット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.EndPointNM.ToString(), info.ToraDONPointName);
            //                    //IDセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.EndPointCd.ToString()].Tag = info.ToraDONPointId;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //着地情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.EndPointNM.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.EndPointCd.ToString()].Tag = null;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の品目コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool ItemCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行
            Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //品目情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //品目情報を取得
            //            //得意先情報を取得
            //            ItemSearchParameter para = new ItemSearchParameter() { ToraDONItemCode = editing_code };
            //            ItemInfo info = this._DalUtil.Item.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //品目情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //単価マスタから単価を取得
            //                if (!rt_val)
            //                {
            //                    int tokuisaki_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                    int startpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                    int endpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                    //※単位は品目に設定されている単位コードを使用

            //                    //単位情報を取得
            //                    FigSearchParameter fig_para = new FigSearchParameter() { FigId = info.ToraDONFigId };
            //                    ToraDONFigInfo fig_info = this._DalUtil.ToraDONFig.GetListInternal(null, fig_para).FirstOrDefault();

            //                    if (null != fig_info)
            //                    {

            //                        int fig_cd = fig_info.FigCode;

            //                        int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

            //                        decimal atprice =
            //                            this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, editing_code, startpoint_cd, endpoint_cd, fig_cd, carkind_cd);

            //                        //取得できたら金額を計算
            //                        if (atprice != 0)
            //                        {
            //                            //得意先の金額丸め区分を取得
            //                            int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

            //                            TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                            TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();
            //                            if (tokui_info != null)
            //                            {
            //                                cutkbn = tokui_info.ToraDONGakCutKbn;
            //                            }

            //                            //数量
            //                            decimal number =
            //                                Convert.ToDecimal(
            //                                    this.mrsJuchuMeisai.GetValue(
            //                                    rowIndex, MrowCellKeys.Number.ToString()));

            //                            //金額計算
            //                            decimal wk_price = 0;

            //                            if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                            {
            //                                //単価・金額セット
            //                                this.mrsJuchuMeisai.SetValue(
            //                                    rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                                this.mrsJuchuMeisai.SetValue(
            //                                    rowIndex, MrowCellKeys.Price.ToString(), wk_price);

            //                                //通行料
            //                                decimal tollFee =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                                //附帯業務料
            //                                decimal futaigyomuryo =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                                //金額によって確定フラグ変更
            //                                bool fixflag = false;
            //                                if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                                {
            //                                    fixflag = true;
            //                                }
            //                                this.mrsJuchuMeisai.SetValue(
            //                                    rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                                //入力値を確定
            //                                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
            //                                //傭車金額の計算
            //                                this.CalcCharterFeeJuchuDtl(rowIndex);

            //                                //合計金額を再計算
            //                                this.SetPriceTotal();
            //                            }
            //                            else
            //                            {
            //                                //金額計算に失敗したら、クリアして編集をキャンセル
            //                                rt_val = true;
            //                                is_clear = true;
            //                            }
            //                        }
            //                    }
            //                }

            //                //重量計算
            //                if (!rt_val)
            //                {
            //                    //数量
            //                    decimal number =
            //                        Convert.ToDecimal(
            //                            this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.Number.ToString()));

            //                    //重量計算
            //                    decimal wk_weight = 0;

            //                    if (this.CalcWeight(number, info.ToraDONWeight, out wk_weight))
            //                    {
            //                        if (wk_weight != 0)
            //                        {
            //                            //重量セット
            //                            this.mrsJuchuMeisai.SetValue(
            //                                rowIndex, MrowCellKeys.Weight.ToString(), wk_weight);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        //金額計算に失敗したら、クリアして編集をキャンセル
            //                        rt_val = true;
            //                        is_clear = true;
            //                    }
            //                }

            //                //編集した内容に問題が無ければ品目名をセット
            //                if (!rt_val)
            //                {
            //                    //IDをセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ItemCd.ToString()].Tag = info.ToraDONItemId;
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.ItemNM.ToString(), info.ToraDONItemName);

            //                    //単位情報を取得
            //                    FigSearchParameter fig_para = new FigSearchParameter() { FigId = info.ToraDONFigId };
            //                    ToraDONFigInfo fig_info = this._DalUtil.ToraDONFig.GetListInternal(null, fig_para).FirstOrDefault();

            //                    if (null != fig_info)
            //                    {
            //                        //IDをセット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = fig_info.FigId;
            //                        this.mrsJuchuMeisai.SetValue(
            //                            rowIndex, MrowCellKeys.FigCd.ToString(), fig_info.FigCode);
            //                        this.mrsJuchuMeisai.SetValue(
            //                            rowIndex, MrowCellKeys.FigNM.ToString(), fig_info.FigName);

            //                    }

            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.TaxDispKbn.ToString(), info.ToraDONItemTaxKbn);

            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.CharterTaxDispKbn.ToString(), info.ToraDONItemTaxKbn);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //品目情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.ItemNM.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.ItemCd.ToString()].Tag = null;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の数量を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool NumberCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            //if (valueChanged)
            //{
            //    //単価を取得
            //    decimal atprice =
            //        Convert.ToDecimal(
            //            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.AtPrice.ToString()));

            //    //得意先の金額丸め区分を取得
            //    int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

            //    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            //    tokui_para.ToraDONTokuisakiCode = Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //    TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //    if (tokui_info != null)
            //    {
            //        cutkbn = tokui_info.ToraDONGakCutKbn;
            //    }

            //    //金額計算
            //    decimal wk_price = 0;

            //    if (this.CalcPrice(editing_value, atprice, cutkbn, out wk_price))
            //    {
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //        if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //        }

            //        //通行料
            //        decimal tollFee =
            //            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //        //附帯業務料
            //        decimal futaigyomuryo =
            //            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //        //金額によって確定フラグ変更
            //        bool fixflag = false;
            //        if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //        {
            //            fixflag = true;
            //        }
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //        //合計金額を再計算
            //        this.SetPriceTotal();
            //    }
            //    else
            //    {
            //        //編集をキャンセル
            //        rt_val = true;
            //    }

            //    //ここまでエラーがなければ傭車金額の計算を行う
            //    if (!rt_val)
            //    {
            //        int carCode =
            //            Convert.ToInt32(
            //                this.mrsJuchuMeisai.GetValue(
            //                rowIndex, MrowCellKeys.CarCd.ToString()));
            //        CarInfo car_info =
            //            this._DalUtil.Car.GetInfo(carCode);

            //        if (car_info != null)
            //        {
            //            //入力値を確定
            //            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
            //            //傭車金額の計算
            //            this.CalcCharterFeeJuchuDtl(rowIndex);

            //            //合計金額を再計算
            //            this.SetPriceTotal();
            //        }
            //    }

            //    //重量計算
            //    if (!rt_val)
            //    {
            //        //商品
            //        ItemSearchParameter item_para = new ItemSearchParameter();
            //        item_para.ToraDONItemCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //        ItemInfo itemInfo = this._DalUtil.Item.GetList(item_para).FirstOrDefault();

            //        if (itemInfo != null)
            //        {
            //            //重量計算
            //            decimal wk_weight = 0;

            //            if (this.CalcWeight(editing_value, itemInfo.ToraDONWeight, out wk_weight))
            //            {
            //                if (wk_weight != 0)
            //                {
            //                    //重量セット
            //                    this.mrsJuchuMeisai.SetValue(
            //                        rowIndex, MrowCellKeys.Weight.ToString(), wk_weight);
            //                }
            //            }
            //            else
            //            {
            //                //金額計算に失敗したら、クリアして編集をキャンセル
            //                rt_val = true;
            //            }
            //        }
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の単位コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool FigCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //単位情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //単位情報を取得
            //            FigSearchParameter para = new FigSearchParameter() { FigCd = editing_code };
            //            ToraDONFigInfo info = this._DalUtil.ToraDONFig.GetListInternal(null, para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //単位情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.DisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                if (!rt_val)
            //                {
            //                    //単位コード
            //                    int figcode = editing_code;

            //                    //開始時刻
            //                    TimeSpan starttime =
            //                        FrameUtilites.ConvertDateTimeToTimeSpan(Convert.ToDateTime(
            //                            this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskStartHM.ToString())));
            //                    //終了時刻
            //                    TimeSpan endtime =
            //                        FrameUtilites.ConvertDateTimeToTimeSpan(Convert.ToDateTime(
            //                            this.mrsJuchuMeisai.GetValue(
            //                                rowIndex, MrowCellKeys.TaskEndHM.ToString())));

            //                    //値が編集された場合は時間計算をします。
            //                    this.CalcTime(figcode, starttime, endtime, rowIndex);

            //                    //単価マスタから単価を取得
            //                    int tokuisaki_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                    int item_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                    int startpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                    int endpoint_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                    int carkind_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));

            //                    decimal atprice =
            //                        this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, startpoint_cd, endpoint_cd, editing_code, carkind_cd);

            //                    //取得できたら金額を計算
            //                    if (atprice != 0)
            //                    {
            //                        //得意先の金額丸め区分を取得
            //                        int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                        TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                        TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();
            //                        if (tokui_info != null)
            //                        {
            //                            cutkbn = tokui_info.ToraDONGakCutKbn;
            //                        }

            //                        //数量
            //                        decimal number =
            //                            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));

            //                        //金額計算
            //                        decimal wk_price = 0;

            //                        if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                        {
            //                            //単価・金額セット
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //                            if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //                            {
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //                            }

            //                            //通行料
            //                            decimal tollFee =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                            //附帯業務料
            //                            decimal futaigyomuryo =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                            //金額によって確定フラグ変更
            //                            bool fixflag = false;
            //                            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                            {
            //                                fixflag = true;
            //                            }
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                            //入力値を確定
            //                            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //                            //傭車金額の計算
            //                            this.CalcCharterFeeJuchuDtl(rowIndex);

            //                            //合計金額を再計算
            //                            this.SetPriceTotal();
            //                        }
            //                        else
            //                        {
            //                            //金額計算に失敗したら、クリアして編集をキャンセル
            //                            rt_val = true;
            //                            is_clear = true;
            //                        }
            //                    }
            //                }

            //                //編集した内容に問題が無ければ単位名をセット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FigNM.ToString(), info.FigName);
            //                    //IDをセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = info.FigId;
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //単位名情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FigNM.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.FigCd.ToString()].Tag = null;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の車両コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool CarCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //車両情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //車両情報を取得
            //            CarSearchParameter para = new CarSearchParameter() { ToraDONCarCode = editing_code };
            //            CarInfo info = this._DalUtil.Car.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //車両情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //単価マスタから単価を取得
            //                if (!rt_val)
            //                {
            //                    //セルの車種コード
            //                    int carKindCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CarKindCd.ToString()));
            //                    //車両情報の車種コード
            //                    int carKindCodeOfCarInfo = info.CarKindCode;

            //                    //車種コードが変わった場合のみ単価を取得するのでその判断
            //                    if (carKindCode != carKindCodeOfCarInfo)
            //                    {
            //                        //単価マスタから単価を取得
            //                        int tokuisaki_cd = Convert.ToInt32(
            //                            this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                        int item_cd =
            //                            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                        int startpoint_cd =
            //                            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                        int endpoint_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                        int fig_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));
            //                        //車種は車両の車種を使用する
            //                        int carkind_cd = carKindCodeOfCarInfo;

            //                        decimal atprice =
            //                            this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, startpoint_cd, endpoint_cd, fig_cd, carkind_cd);

            //                        //得意先の金額丸め区分を取得
            //                        int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                        TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                        TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();
            //                        if (tokui_info != null)
            //                        {
            //                            cutkbn = tokui_info.ToraDONGakCutKbn;
            //                        }

            //                        //数量を取得
            //                        decimal number =
            //                            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));
            //                        //金額計算
            //                        decimal wk_price = 0;
            //                        //※金額丸め区分が関係する為、金額計算はしておく
            //                        if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                        {
            //                            //単価・金額セット
            //                            if (atprice != 0)
            //                            {
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //                                if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //                                {
            //                                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //                                }

            //                                //通行料
            //                                decimal tollFee =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                                //附帯業務料
            //                                decimal futaigyomuryo =
            //                                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                                //金額によって確定フラグ変更
            //                                bool fixflag = false;
            //                                if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                                {
            //                                    fixflag = true;
            //                                }
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                                //入力値を確定
            //                                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //                                //傭車金額の計算
            //                                this.CalcCharterFeeJuchuDtl(rowIndex);

            //                                //合計金額を再計算
            //                                this.SetPriceTotal();
            //                            }

            //                        }
            //                        else
            //                        {
            //                            //金額計算に失敗したら、クリアして編集をキャンセル
            //                            rt_val = true;
            //                            is_clear = true;
            //                        }
            //                    }
            //                }

            //                //編集エラー出なければ車両情報セット
            //                if (!rt_val)
            //                {
            //                    //車両情報の値を設定
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarCd.ToString()].Tag = info.ToraDONCarId;
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKbn.ToString(), info.CarKbn);
            //                    if (info.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            //                    {
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), string.Empty);
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(),
            //                            FrameUtilites.GetSystemName(DefaultProperty.SystemNameKbn.CarKbn, (int)DefaultProperty.CarKbn.Yosha));
            //                        //傭車コードが存在する場合
            //                        if (0 < info.YoshasakiCode)
            //                        {
            //                            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = info.YoshasakiId;
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), info.YoshasakiCode);
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), info.YoshasakiShortName);

            //                            //入力値を確定
            //                            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //                            //傭車金額の計算
            //                            this.CalcCharterFeeJuchuDtl(rowIndex);
            //                        }
            //                    }
            //                    else
            //                    {
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), info.LicPlateCarNo);
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(), info.BranchOfficeShortName);
            //                    }
            //                    //車種の値を設定
            //                    if (!info.ToraDONCarKindDisableFlag)
            //                    {
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = info.CarKindId;
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindCd.ToString(), info.CarKindCode);
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindName.ToString(), info.CarKindShortName);
            //                    }
            //                    //乗務員の値を設定
            //                    if (!info.ToraDONStaffDisableFlag)
            //                    {
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.DriverCd.ToString()].Tag = info.DriverId;
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), info.StaffCode);
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), info.StaffName);
            //                    }
            //                }

            //                //傭車かそうでないかで有効・無効を切り替えるが無効になった場合は値をクリアするのでその判断
            //                if (this.CarKbnIsCharter(info.CarKbn))
            //                {
            //                    this.ClearDriverValuesForCarKbn(rowIndex);
            //                }
            //                else
            //                {
            //                    this.ClearCharterValuesForCarKbn(rowIndex);
            //                }

            //                this.SetPriceTotal();

            //                this.RefreshStyleForCarKbn(rowIndex, info.CarKbn);
            //            }
            //        } //-- editing_code != 0の場合のブロック
            //    } //-- ValueChangedのブロック
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //車両情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.LicPlateCarNo.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Chartererkbn.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKbn.ToString(), 0);

            //        this.RefreshStyleForCarKbn(rowIndex);
            //        this.ClearCharterValuesForCarKbn(rowIndex);
            //        this.SetPriceTotal();
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の車種コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool CarKindCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //車種情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //車種情報を取得
            //            CarKindSearchParameter para = new CarKindSearchParameter() { ToraDONCarKindCode = editing_code };
            //            CarKindInfo info = this._DalUtil.CarKind.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //車種情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //単価マスタから単価を取得
            //                if (!rt_val)
            //                {

            //                    //単価マスタから単価を取得
            //                    int tokuisaki_cd = Convert.ToInt32(
            //                        this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //                    int item_cd =
            //                        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.ItemCd.ToString()));
            //                    int startpoint_cd =
            //                        Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.StartPointCd.ToString()));
            //                    int endpoint_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.EndPointCd.ToString()));
            //                    int fig_cd = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FigCd.ToString()));

            //                    decimal atprice =
            //                        this.GetAtPriceByJuchuDtlKey(rowIndex, tokuisaki_cd, item_cd, startpoint_cd, endpoint_cd, fig_cd, editing_code);

            //                    //得意先の金額丸め区分を取得
            //                    int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            //                    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
            //                    TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //                    if (tokui_info != null)
            //                    {
            //                        cutkbn = tokui_info.ToraDONGakCutKbn;
            //                    }

            //                    //数量を取得
            //                    decimal number =
            //                        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));
            //                    //金額計算
            //                    decimal wk_price = 0;
            //                    //※金額丸め区分が関係する為、金額計算はしておく
            //                    if (this.CalcPrice(number, atprice, cutkbn, out wk_price))
            //                    {
            //                        //単価・金額セット
            //                        if (atprice != 0)
            //                        {
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.AtPrice.ToString(), atprice);
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //                            if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //                            {
            //                                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //                            }

            //                            //通行料
            //                            decimal tollFee =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //                            //附帯業務料
            //                            decimal futaigyomuryo =
            //                                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //                            //金額によって確定フラグ変更
            //                            bool fixflag = false;
            //                            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //                            {
            //                                fixflag = true;
            //                            }
            //                            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //                            //入力値を確定
            //                            EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //                            //傭車金額の計算
            //                            this.CalcCharterFeeJuchuDtl(rowIndex);

            //                            //合計金額を再計算
            //                            this.SetPriceTotal();
            //                        }

            //                    }
            //                    else
            //                    {
            //                        //金額計算に失敗したら、クリアして編集をキャンセル
            //                        rt_val = true;
            //                        is_clear = true;
            //                    }
            //                }

            //                //編集エラー出なければ車種情報セット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindCd.ToString(), info.ToraDONCarKindCode);
            //                    //IDをセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = info.ToraDONCarKindId;
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindName.ToString(), info.ToraDONCarKindSNM);
            //                }
            //            }
            //        } //-- editing_code != 0の場合のブロック
            //    } //-- ValueChangedのブロック
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //車種情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindCd.ToString(), 0);
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarKindName.ToString(), string.Empty);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarKindCd.ToString()].Tag = null;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の乗務員コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool DriverCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //乗務員情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //乗務員情報を取得
            //            StaffSearchParameter para = new StaffSearchParameter() { ToraDONStaffCode = editing_code };
            //            StaffInfo info = this._DalUtil.Staff.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //乗務員情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //編集エラー出なければ乗務員情報セット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), info.ToraDONStaffCode);
            //                    //IDをセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.DriverCd.ToString()].Tag = info.ToraDONStaffId;
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), info.ToraDONStaffName);
            //                }
            //            }
            //        } //-- editing_code != 0の場合のブロック
            //    } //-- ValueChangedのブロック
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //乗務員情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.DriverCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の傭車先コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool CarOfChartererCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //傭車先情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //傭車先情報を取得
            //            TorihikisakiSearchParameter param = new TorihikisakiSearchParameter() { ToraDONTorihikiCode = editing_code };
            //            TorihikisakiInfo info = this._DalUtil.Torihikisaki.GetListInternal(null, param).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //傭車先情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //傭車先の場合だけ処理をすすめるのでその判断
            //                if (info.CharterKbn == (int)BizProperty.DefaultProperty.YoshaKbn.Yosha)
            //                {
            //                    //未使用データの場合は、エラー
            //                    if (info.ToraDONDisableFlag)
            //                    {
            //                        //使用可否エラー
            //                        DialogResult mq_result = MessageBox.Show(
            //                            FrameUtilites.GetDefineMessage("MW2201016"),
            //                            "警告",
            //                            MessageBoxButtons.OK,
            //                            MessageBoxIcon.Warning);
            //                        //情報クリア
            //                        rt_val = true;
            //                        is_clear = true;
            //                    }

            //                    //編集エラーが出なければ傭車金額を計算
            //                    if (!rt_val)
            //                    {
            //                        //入力値を確定
            //                        EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //                        //傭車金額の計算
            //                        this.CalcCharterFeeJuchuDtl(rowIndex);

            //                        //合計を再計算
            //                        this.SetPriceTotal();

            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), info.ToraDONTorihikiCode);
            //                        //IDをセット
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = info.ToraDONTorihikiId;
            //                        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), info.ToraDONTorihikiShortName);
            //                    }
            //                }
            //                else
            //                {
            //                    //編集をキャンセル
            //                    rt_val = true;
            //                    is_clear = true;

            //                    MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2203079"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                }
            //            }
            //        } //-- editing_code != 0の場合のブロック
            //    } //-- ValueChangedのブロック
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //傭車先情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CarOfChartererCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の単価を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool AtPriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            //if (valueChanged)
            //{
            //    //数量を取得
            //    decimal number =
            //        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));

            //    //得意先の金額丸め区分を取得
            //    int cutkbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

            //    TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            //    tokui_para.ToraDONTokuisakiCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TokuisakiCd.ToString()));
            //    TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //    if (tokui_info != null)
            //    {
            //        cutkbn = tokui_info.ToraDONGakCutKbn;
            //    }

            //    //金額計算
            //    decimal wk_price = 0;

            //    if (this.CalcPrice(number, editing_value, cutkbn, out wk_price))
            //    {
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.Price.ToString(), wk_price);
            //        if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
            //        {
            //            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), wk_price);
            //        }

            //        //通行料
            //        decimal tollFee =
            //            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

            //        //附帯業務料
            //        decimal futaigyomuryo =
            //            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

            //        //金額によって確定フラグ変更
            //        bool fixflag = false;
            //        if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            //        {
            //            fixflag = true;
            //        }
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

            //        //入力値を確定
            //        EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

            //        //傭車金額の計算
            //        this.CalcCharterFeeJuchuDtl(rowIndex);

            //        //合計金額を再計算
            //        this.SetPriceTotal();
            //    }
            //    else
            //    {
            //        //編集をキャンセル
            //        rt_val = true;
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の荷主コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool OwnerCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //荷主情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //荷主情報を取得
            //            ToraDONOwnerSearchParameter para = new ToraDONOwnerSearchParameter() { OwnerCode = editing_code };
            //            ToraDONOwnerInfo info = this._DalUtil.ToraDONOwner.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //荷主情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //編集した内容に問題が無ければ荷主情報をセット
            //                if (!rt_val)
            //                {
            //                    //未使用データの場合は、エラー
            //                    if (info.DisableFlag)
            //                    {
            //                        //使用可否エラー
            //                        DialogResult mq_result = MessageBox.Show(
            //                            FrameUtilites.GetDefineMessage("MW2201016"),
            //                            "警告",
            //                            MessageBoxButtons.OK,
            //                            MessageBoxIcon.Warning);
            //                        //情報クリア
            //                        rt_val = true;
            //                        is_clear = true;
            //                    }
            //                    else
            //                    {
            //                        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.OwnerCd.ToString()].Tag = info.OwnerId;
            //                        this.mrsJuchuMeisai.SetValue(
            //                            rowIndex, MrowCellKeys.OwnerNM.ToString(), info.OwnerName);
            //                    }
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //荷主情報クリア
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.OwnerCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.OwnerNM.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の金額を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool PriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                //金額によって確定フラグ変更
                bool fixflag = false;

                //通行料
                decimal tollFee =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

                //附帯業務料
                decimal futaigyomuryo =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

                if (editing_value != 0 || tollFee != 0 || futaigyomuryo != 0)
                {
                    fixflag = true;
                }

                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

                //乗務員売上金額
                if (Convert.ToBoolean(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.JomuinUriageDogakuFlag.ToString())))
                {
                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(), editing_value);
                }

                //入力値を確定
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);

                //ここまでエラーがなければ傭車金額の計算を行う
                if (!rt_val)
                {
                    //傭車金額の計算
                    this.CalcCharterFeeJuchuDtl(rowIndex);
                }

                //合計金額を再計算
                this.SetPriceTotal();
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の傭車金額を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool CharterPriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                //傭車金額によって傭車確定フラグ変更
                bool charter_fixflag = false;

                //通行料
                decimal tollFeeInCharterPrice =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString()));

                if (editing_value != 0 || tollFeeInCharterPrice != 0)
                {
                    charter_fixflag = true;
                }

                this.mrsJuchuMeisai.SetValue(
                    rowIndex, MrowCellKeys.CharterFixFlag.ToString(), charter_fixflag);

                //入力値を確定
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
                //傭車金額合計を再計算
                this.SetPriceTotal();
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の金額通行料を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <param name="p"></param>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TollFeeInPriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                //金額によって確定フラグ変更
                bool fixflag = false;

                //金額
                decimal price =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Price.ToString()));

                //金額_附帯業務料
                decimal futaigyomuryoinprice =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString()));

                if (price != 0 || futaigyomuryoinprice != 0 || editing_value != 0)
                {
                    fixflag = true;
                }

                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

                //入力値を確定
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
                //合計を再計算
                this.SetPriceTotal();
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の傭車金額通行料を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <param name="p"></param>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TollFeeInCharterPriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                //傭車金額によって傭車確定フラグ変更
                bool charter_fixflag = false;

                //傭車金額
                decimal charterPrice =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.CharterPrice.ToString()));

                if (charterPrice != 0 || editing_value != 0)
                {
                    charter_fixflag = true;
                }

                this.mrsJuchuMeisai.SetValue(
                    rowIndex, MrowCellKeys.CharterFixFlag.ToString(), charter_fixflag);

                //入力値を確定
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
                //合計を再計算
                this.SetPriceTotal();
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の開始日付を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TaskStartYMDCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //受注積着地計算区分が「着日→積日」の場合
            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
                            == (int)DefaultProperty.JuchuYMDCalculationKbn.ChakuToTsumi)
            {
                //何もしない
                return rt_val;
            }

            //終了日付をクリアするかどうか（true：クリア）
            bool is_clear = false;
            //try
            //{
            //    //セル値
            //    DateTime cell_time =
            //             Convert.ToDateTime(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    DateTime editing_time =
            //            Convert.ToDateTime(GetEditingControlValue(
            //                this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_time != editing_time;

            //    if (valueChanged)
            //    {
            //        if (editing_time == DateTime.MinValue)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //                == (int)DefaultProperty.JuchuYMDCalculationKbn.TsumiToChaku)
            //            {
            //                //販路コードを取得
            //                int HanroCD = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.HanroCd.ToString()));

            //                //販路情報を取得
            //                HanroSearchParameter hanro_para = new HanroSearchParameter() { HanroCode = HanroCD };
            //                HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

            //                if (hanro_info != null && 0 < hanro_info.HanroId)
            //                {
            //                    //積時間を取得
            //                    DateTime TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskStartHM.ToString()));

            //                    //着日を取得
            //                    DateTime TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                    //着時間を取得
            //                    DateTime TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                    //販路情報及び積日から着日を設定
            //                    this.SetTaskEndYMDHM(rowIndex, hanro_info, editing_time, TaskStartHM, TaskEndYMD, TaskEndHM);

            //                    //着日を再取得
            //                    TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                    //着時間を再取得
            //                    TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                    //再使用可能日を取得
            //                    DateTime ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.ReuseYMD.ToString()));

            //                    //再使用可能時間を取得
            //                    DateTime ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.ReuseHM.ToString()));

            //                    //販路情報及び着日から再使用可能日を設定
            //                    this.SetReuseYMDHM(rowIndex, hanro_info, TaskEndYMD, TaskEndHM, ReuseYMD, ReuseHM);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //終了日付情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.TaskEndYMD.ToString(), null);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の開始時刻を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TaskStartHMCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //開始時刻をクリアするかどうか（true：クリア）
            bool is_clear = false;
            //try
            //{
            //    //セル値
            //    TimeSpan cell_time =
            //         FrameUtilites.ConvertDateTimeToTimeSpan(
            //             Convert.ToDateTime(this.mrsJuchuMeisai.CurrentCell.Value));
            //    //編集値
            //    TimeSpan editing_time =
            //         FrameUtilites.ConvertDateTimeToTimeSpan(
            //            Convert.ToDateTime(GetEditingControlValue(
            //                this.mrsJuchuMeisai.EditingControl)));

            //    //値が変更されたか？
            //    bool valueChanged = cell_time != editing_time;

            //    if (valueChanged)
            //    {
            //        if (editing_time == TimeSpan.MinValue)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //単位コード
            //            int figcode =
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(
            //                rowIndex, MrowCellKeys.FigCd.ToString()));
            //            //開始時刻
            //            TimeSpan starttime = editing_time;
            //            //終了時刻
            //            TimeSpan endtime =
            //                FrameUtilites.ConvertDateTimeToTimeSpan(Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskEndHM.ToString())));

            //            //値が編集された場合は時間計算をします。
            //            this.CalcTime(figcode, starttime, endtime, rowIndex);
            //        }

            //        if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //            == (int)DefaultProperty.JuchuYMDCalculationKbn.TsumiToChaku)
            //        {
            //            //販路コードを取得
            //            int HanroCD = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.HanroCd.ToString()));

            //            //販路情報を取得
            //            HanroSearchParameter hanro_para = new HanroSearchParameter() { HanroCode = HanroCD };
            //            HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

            //            if (hanro_info != null && 0 < hanro_info.HanroId)
            //            {
            //                //積日を取得
            //                DateTime TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                    rowIndex, MrowCellKeys.TaskStartYMD.ToString()));

            //                //積時間を取得
            //                DateTime TaskStartHM = Convert.ToDateTime(GetEditingControlValue(
            //                        this.mrsJuchuMeisai.EditingControl));

            //                //着日を取得
            //                DateTime TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                //着時間を取得
            //                DateTime TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                //販路情報及び着日から積日を設定
            //                this.SetTaskEndYMDHM(rowIndex, hanro_info, TaskStartYMD, TaskStartHM, TaskEndYMD, TaskEndHM);

            //                //着日を再取得
            //                TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                //着時間を再取得
            //                TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                //再使用可能日を取得
            //                DateTime ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.ReuseYMD.ToString()));

            //                //再使用可能時間を取得
            //                DateTime ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.ReuseHM.ToString()));

            //                //販路情報及び着日から再使用可能日を設定
            //                this.SetReuseYMDHM(rowIndex, hanro_info, TaskEndYMD, TaskEndHM, ReuseYMD, ReuseHM);
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //開始時刻情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.TaskStartHM.ToString(), null);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の終了日付を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TaskEndYMDCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //受注積着地計算区分が「積日→着日」の場合
            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
                            == (int)DefaultProperty.JuchuYMDCalculationKbn.TsumiToChaku)
            {
                //何もしない
                return rt_val;
            }

            //終了日付をクリアするかどうか（true：クリア）
            bool is_clear = false;
            //try
            //{
            //    //セル値
            //    DateTime cell_time =
            //             Convert.ToDateTime(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    DateTime editing_time =
            //            Convert.ToDateTime(GetEditingControlValue(
            //                this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_time != editing_time;

            //    if (valueChanged)
            //    {
            //        if (editing_time == DateTime.MinValue)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //                == (int)DefaultProperty.JuchuYMDCalculationKbn.ChakuToTsumi)
            //            {
            //                //販路コードを取得
            //                int HanroCD = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.HanroCd.ToString()));

            //                //販路情報を取得
            //                HanroSearchParameter hanro_para = new HanroSearchParameter() { HanroCode = HanroCD };
            //                HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

            //                if (hanro_info != null && 0 < hanro_info.HanroId)
            //                {
            //                    //積日を取得
            //                    DateTime TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskStartYMD.ToString()));

            //                    //積時間を取得
            //                    DateTime TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskStartHM.ToString()));

            //                    //着時間を取得
            //                    DateTime TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                    //販路情報及び着日から積日を設定
            //                    this.SetTaskStartYMDHM(rowIndex, hanro_info, TaskStartYMD, TaskStartHM, editing_time, TaskEndHM);

            //                    //着日を再取得
            //                    DateTime TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //                    //着時間を再取得
            //                    TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.TaskEndHM.ToString()));

            //                    //再使用可能日を取得
            //                    DateTime ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.ReuseYMD.ToString()));

            //                    //再使用可能時間を取得
            //                    DateTime ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                            rowIndex, MrowCellKeys.ReuseHM.ToString()));

            //                    //販路情報及び着日から再使用可能日を設定
            //                    this.SetReuseYMDHM(rowIndex, hanro_info, TaskEndYMD, TaskEndHM, ReuseYMD, ReuseHM);
            //                }
            //            }
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //終了日付情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.TaskEndYMD.ToString(), null);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の終了時刻を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool TaskEndHMCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //終了時刻をクリアするかどうか（true：クリア）
            bool is_clear = false;
            //try
            //{
            //    //セル値
            //    TimeSpan cell_time =
            //         FrameUtilites.ConvertDateTimeToTimeSpan(
            //             Convert.ToDateTime(this.mrsJuchuMeisai.CurrentCell.Value));
            //    //編集値
            //    TimeSpan editing_time =
            //         FrameUtilites.ConvertDateTimeToTimeSpan(
            //            Convert.ToDateTime(GetEditingControlValue(
            //                this.mrsJuchuMeisai.EditingControl)));

            //    //値が変更されたか？
            //    bool valueChanged = cell_time != editing_time;

            //    if (valueChanged)
            //    {
            //        if (editing_time == TimeSpan.MinValue)
            //        {
            //            //未入力時はクリア
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //単位コード
            //            int figcode =
            //            Convert.ToInt32(this.mrsJuchuMeisai.GetValue(
            //                rowIndex, MrowCellKeys.FigCd.ToString()));
            //            //開始時刻
            //            TimeSpan starttime =
            //                FrameUtilites.ConvertDateTimeToTimeSpan(Convert.ToDateTime(
            //                    this.mrsJuchuMeisai.GetValue(
            //                        rowIndex, MrowCellKeys.TaskStartHM.ToString())));
            //            //終了時刻
            //            TimeSpan endtime = editing_time;

            //            //値が編集された場合は時間計算をします。
            //            this.CalcTime(figcode, starttime, endtime, rowIndex);
            //        }

            //        //販路コードを取得
            //        int HanroCD = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.HanroCd.ToString()));

            //        //販路情報を取得
            //        HanroSearchParameter hanro_para = new HanroSearchParameter() { HanroCode = HanroCD };
            //        HanroInfo hanro_info = this._DalUtil.Hanro.GetList(hanro_para).FirstOrDefault();

            //        if (hanro_info != null && 0 < hanro_info.HanroId)
            //        {
            //            //積日を取得
            //            DateTime TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                rowIndex, MrowCellKeys.TaskStartYMD.ToString()));

            //            //積時間を取得
            //            DateTime TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                    rowIndex, MrowCellKeys.TaskStartHM.ToString()));

            //            //着日を取得
            //            DateTime TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                    rowIndex, MrowCellKeys.TaskEndYMD.ToString()));

            //            //着時間を取得
            //            DateTime TaskEndHM = Convert.ToDateTime(GetEditingControlValue(
            //                    this.mrsJuchuMeisai.EditingControl));

            //            if (this._SystemSettingsInfo.JuchuYMDCalculationKbn
            //                == (int)DefaultProperty.JuchuYMDCalculationKbn.TsumiToChaku)
            //            {
            //                //販路情報及び着日から積日を設定
            //                this.SetTaskStartYMDHM(rowIndex, hanro_info, TaskStartYMD, TaskStartHM, TaskEndYMD, TaskEndHM);
            //            }

            //            //再使用可能日を取得
            //            DateTime ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                    rowIndex, MrowCellKeys.ReuseYMD.ToString()));

            //            //再使用可能時間を取得
            //            DateTime ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
            //                    rowIndex, MrowCellKeys.ReuseHM.ToString()));

            //            //販路情報及び着日から再使用可能日を設定
            //            this.SetReuseYMDHM(rowIndex, hanro_info, TaskEndYMD, TaskEndHM, ReuseYMD, ReuseHM);
            //        }
            //    }
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //終了時刻情報クリア
            //        this.mrsJuchuMeisai.SetValue(
            //            rowIndex, MrowCellKeys.TaskEndHM.ToString(), null);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の受注担当コードを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool JuchuTantoCdCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            ////現在の行コレクション
            GrapeCity.Win.MultiRow.Row wk_curmrow = this.mrsJuchuMeisai.Rows[rowIndex];

            //受注担当情報をクリアするかどうか（true：クリア）
            bool is_clear = false;

            //try
            //{
            //    //セル値
            //    int cell_code =
            //        Convert.ToInt32(this.mrsJuchuMeisai.CurrentCell.Value);
            //    //編集値
            //    int editing_code =
            //        Convert.ToInt32(
            //            GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //    //値が変更されたか？
            //    bool valueChanged = cell_code != editing_code;

            //    if (valueChanged)
            //    {
            //        if (editing_code == 0)
            //        {
            //            //未入力時はクリアのみ
            //            is_clear = true;
            //        }
            //        else
            //        {
            //            //受注担当情報を取得
            //            StaffSearchParameter para = new StaffSearchParameter() { ToraDONStaffCode = editing_code };
            //            StaffInfo info = this._DalUtil.Staff.GetList(para).FirstOrDefault();

            //            if (info == null)
            //            {
            //                //編集をキャンセル
            //                rt_val = true;

            //                MessageBox.Show(
            //                   FrameUtilites.GetDefineMessage("MW2201003"),
            //                   "警告",
            //                   MessageBoxButtons.OK,
            //                   MessageBoxIcon.Warning);

            //                //乗務員情報クリア
            //                is_clear = true;
            //            }
            //            else
            //            {
            //                //未使用データの場合は、エラー
            //                if (info.ToraDONDisableFlag)
            //                {
            //                    //使用可否エラー
            //                    DialogResult mq_result = MessageBox.Show(
            //                        FrameUtilites.GetDefineMessage("MW2201016"),
            //                        "警告",
            //                        MessageBoxButtons.OK,
            //                        MessageBoxIcon.Warning);
            //                    //情報クリア
            //                    rt_val = true;
            //                    is_clear = true;
            //                }

            //                //編集エラー出なければ受注担当情報セット
            //                if (!rt_val)
            //                {
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoCd.ToString(), info.ToraDONStaffCode);
            //                    //IDをセット
            //                    this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag = info.ToraDONStaffId;
            //                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoNm.ToString(), info.ToraDONStaffName);
            //                }
            //            }
            //        } //-- editing_code != 0の場合のブロック
            //    } //-- ValueChangedのブロック
            //}
            //catch (Exception err)
            //{
            //    throw err;
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        //受注担当情報クリア
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoCd.ToString(), 0);
            //        this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.JuchuTantoCd.ToString()].Tag = null;
            //        this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JuchuTantoNm.ToString(), string.Empty);
            //    }
            //}

            return rt_val;
        }

        /// <summary>
        /// 受注明細の金額附帯業務料を検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <param name="p"></param>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool FutaigyomuryoInPriceCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            decimal cell_value =
                Convert.ToDecimal(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            decimal editing_value =
                Convert.ToDecimal(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                //金額によって確定フラグ変更
                bool fixflag = false;

                //金額
                decimal price =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Price.ToString()));
                //金額_通行料
                decimal tollfeeinprice =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.TollFeeInPrice.ToString()));

                if (price != 0 || tollfeeinprice != 0 || editing_value != 0)
                {
                    fixflag = true;
                }

                this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.FixFlag.ToString(), fixflag);

                //入力値を確定
                EditingActions.EndEdit.Execute(this.mrsJuchuMeisai);
                //合計を再計算
                this.SetPriceTotal();
            }

            return rt_val;
        }

        /// <summary>
        /// 受注明細の乗務員売上同額フラグを検証します。
        /// 編集をキャンセルする必要がある場合は true を返します。
        /// </summary>
        /// <param name="p"></param>
        /// <returns>編集をキャンセルするかどうか（true：キャンセルする）</returns>
        private bool JomuinUriageDogakuFlagCellValidating(int rowIndex)
        {
            //戻り値
            bool rt_val = false;

            //セル値
            bool cell_value =
                Convert.ToBoolean(this.mrsJuchuMeisai.CurrentCell.Value);
            //編集値
            bool editing_value =
                Convert.ToBoolean(
                    GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));

            //値が変更されたか？
            bool valueChanged = cell_value != editing_value;

            if (valueChanged)
            {
                GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIndex];

                //乗務員売上同額フラグがチェックされた場合
                if (editing_value)
                {
                    //金額を乗務員売上金額に設定
                    this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.JomuinUriageKingaku.ToString(),
                        Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Price.ToString())));

                    //編集不可
                    this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], true);
                }
                else
                {
                    //編集可
                    this.OriginalCellLocked(wk_mrow[MrowCellKeys.JomuinUriageKingaku.ToString()], false);
                }
            }

            return rt_val;
        }

        #endregion

        #region 計算ロジック

        /// <summary>
        /// 金額を計算し、取得します。
        /// 計算結果に問題が無ければ true を返します。
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="atPrice">単価</param>
        /// <param name="cutKbn">端数区分</param>
        /// <param name="price">金額</param>
        /// <returns>計算結果（true：問題なし）</returns>
        private bool CalcPrice(decimal number, decimal atPrice, int cutKbn, out decimal price)
        {
            bool rt_val = true;

            //金額を計算
            decimal wk_price = number * atPrice;

            //端数を丸める
            decimal wk_result = this.GetRoundVal(wk_price, cutKbn);

            //桁あふれのチェック
            if (wk_result > PRICE_MAXVALUE || wk_result < PRICE_MINVALUE)
            {
                rt_val = false;

                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                    "MW2203019", new string[] { "金額" }),
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            //Outキーワードの金額に計算結果をセット
            price = wk_result;

            return rt_val;
        }

        /// <summary>
        /// 重量を計算し、取得します。
        /// 計算結果に問題が無ければ true を返します。
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="atWeight">単重量</param>
        /// <returns>計算結果（true：問題なし）</returns>
        private bool CalcWeight(decimal number, decimal atWeight, out decimal weight)
        {
            bool rt_val = true;

            //計算
            decimal wk_weight = number * atWeight;

            //端数を丸める
            decimal wk_result = this.GetRoundVal(wk_weight, (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff);

            //桁あふれのチェック
            if (wk_result > WEIGHT_MAXVALUE || wk_result < WEIGHT_MINVALUE)
            {
                rt_val = false;

                MessageBox.Show(
                    FrameUtilites.GetDefineMessage(
                    "MW2203019", new string[] { "重量" }),
                    "警告",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }

            //Outキーワードの重量に計算結果をセット
            weight = wk_result;

            return rt_val;
        }

        /// <summary>
        /// 時間計算をして数量を取得します。
        /// </summary>
        /// <param name="number">数量</param>
        /// <param name="starttime">開始時間</param>
        /// <param name="endtime">終了時間</param>
        /// <param name="figcode">単位コード</param>
        /// <param name="startDate">開始日付</param>
        /// <param name="endDate">終了日付</param>
        /// <returns></returns>
        private bool CalcTime(int figcode, TimeSpan starttime, TimeSpan endtime, int rowIndex)
        {
            bool rt_val = true;

            //数量
            decimal number =
                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(
                    rowIndex, MrowCellKeys.Number.ToString()));
            //開始日付
            DateTime startdate =
                Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
                    rowIndex, MrowCellKeys.TaskStartYMD.ToString())).Date;
            //終了日付
            DateTime enddate =
                Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(
                    rowIndex, MrowCellKeys.TaskEndYMD.ToString())).Date;
            if (starttime != null && endtime != null)
            {
                if (startdate != DateTime.MinValue && enddate != DateTime.MinValue)
                {
                    //if (starttime != TimeSpan.MinValue
                    // && endtime != TimeSpan.MinValue)
                    //{
                    //    //単位情報を取得
                    //    FigSearchParameter para = new FigSearchParameter() { FigCd = figcode };
                    //    ToraDONFigInfo info = this._DalUtil.ToraDONFig.GetListInternal(null, para).FirstOrDefault();

                    //    if (info != null)
                    //    {
                    //        //時間区分が1、開始と終了日付が同一日の場合のみ計算する
                    //        if (info.TimeKbn == 1 && startdate == enddate)
                    //        {
                    //            //分に直して時間で割った値を保持する変数
                    //            double time = (endtime - starttime).TotalMinutes / 60;
                    //            //整数化した値を保持する変数
                    //            double roundtime = (double)NSKUtil.Round((decimal)time, NSKRoundType.ROUND_TYPE_DOWN);
                    //            //小数点以下の値を保持する変数
                    //            double fractiontime = time - roundtime;

                    //            // 分は15分単位で計算する
                    //            // 時間がマイナスの場合は値をセットしない
                    //            if (time < 0)
                    //            {
                    //                this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), 0);
                    //            }
                    //            else
                    //            {
                    //                if (fractiontime == 0)
                    //                {
                    //                    this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), roundtime);
                    //                }
                    //                else if (fractiontime <= 0.25)
                    //                {
                    //                    this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), roundtime + 0.25);

                    //                }
                    //                else if (fractiontime <= 0.5)
                    //                {
                    //                    this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), roundtime + 0.5);
                    //                }
                    //                else if (fractiontime <= 0.75)
                    //                {
                    //                    this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), roundtime + 0.75);
                    //                }
                    //                else if (fractiontime <= 1)
                    //                {
                    //                    this.mrsJuchuMeisai.SetValue(
                    //                        rowIndex, MrowCellKeys.Number.ToString(), roundtime + 1);
                    //                }
                    //            }

                    //            //数量
                    //            decimal value =
                    //                Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.Number.ToString()));
                    //        }
                    //    }
                    //}

                }
            }
            return rt_val;
        }

        /// <summary>
        /// 金額と丸め区分を指定して、小数点以下を指定した丸めで計算した金額を返却します。
        /// </summary>
        /// <param name="price">金額</param>
        /// <param name="cutKbn">丸め区分</param>
        /// <returns>丸められた金額</returns>
        private decimal CalcPriceByCutKbn(decimal price, DefaultProperty.HasuMArumeKbn cutKbn)
        {
            decimal rt_val = 0;

            switch (cutKbn)
            {
                case DefaultProperty.HasuMArumeKbn.RoundOff:
                    rt_val = NSKUtil.RoundOff(price, 0);
                    break;
                case DefaultProperty.HasuMArumeKbn.RoundDown:
                    rt_val = NSKUtil.RoundDown(price, 0);
                    break;
                case DefaultProperty.HasuMArumeKbn.RoundUp:
                    rt_val = NSKUtil.RoundUp(price, 0);
                    break;
                default:
                    break;
            }

            return rt_val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baseNum"></param>
        /// <param name="taxRate"></param>
        /// <param name="taxDispKbn"></param>
        /// <param name="cutKbn"></param>
        /// <returns></returns>
        public CalcTaxResultStructInfo ComputeTaxStruct(decimal baseNum, decimal taxRate, DefaultProperty.ZeiKbn taxDispKbn, DefaultProperty.HasuMArumeKbn cutKbn)
        {
            CalcTaxResultStructInfo rt_info = null;

            //税抜額, 税額取得用, 税込額の変数
            decimal wk_withoutTaxAmount = 0;
            decimal wk_taxAmount = 0;
            decimal wk_inTaxAmount = 0;

            switch (taxDispKbn)
            {
                case DefaultProperty.ZeiKbn.Sotozei:
                    //外税の場合
                    wk_taxAmount = this.CalcPriceByCutKbn(baseNum * taxRate, cutKbn);
                    wk_inTaxAmount = baseNum + wk_taxAmount;
                    wk_withoutTaxAmount = baseNum;
                    break;
                case DefaultProperty.ZeiKbn.Uchizei:
                    //内税の場合
                    // (金額 / (1 + 消費意率)) * 消費税率 = 税額
                    wk_taxAmount = this.CalcPriceByCutKbn((baseNum / (1 + taxRate)) * taxRate, cutKbn);
                    wk_inTaxAmount = baseNum;
                    wk_withoutTaxAmount = baseNum - wk_taxAmount;
                    break;
                case DefaultProperty.ZeiKbn.Hikazei:
                    //非課税の場合
                    wk_taxAmount = 0;
                    wk_inTaxAmount = baseNum;
                    wk_withoutTaxAmount = baseNum;
                    break;
                default:
                    break;
            }

            //インスタンスを作成
            rt_info = new CalcTaxResultStructInfo()
            {
                BaseAmount = baseNum,
                TaxRate = taxRate,
                WithoutTaxAmount = wk_withoutTaxAmount,
                TaxAmount = wk_taxAmount,
                InTaxAmount = wk_inTaxAmount,
                CurrentTaxDispKbn = taxDispKbn,
                CurrentCutKbn = cutKbn,
            };

            return rt_info;
        }

        /// <summary>
        /// 行番号を指定して傭車金額を計算して設定します。
        /// 正しく計算された場合にはtrueを返します。
        /// </summary>
        /// <param name="rowIdx">計算する行番号</param>
        /// <returns></returns>
        private void CalcCharterFeeJuchuDtl(int rowIdx)
        {
            GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIdx];
            CellPosition curCellPosition = this.mrsJuchuMeisai.CurrentCellPosition;

            //傭車金額変更に関わる項目を各セルのTagに退避しておく
            wk_mrow[MrowCellKeys.CharterPrice.ToString()].Style.Tag =
                this.mrsJuchuMeisai.GetValue(rowIdx, MrowCellKeys.CharterPrice.ToString());
            wk_mrow[MrowCellKeys.CharterFixFlag.ToString()].Style.Tag =
                this.mrsJuchuMeisai.GetValue(rowIdx, MrowCellKeys.CharterFixFlag.ToString());

            //変更の金額
            decimal wk_price = 0;

            //現在セルが金額で編集中か？
            if (curCellPosition.RowIndex == rowIdx && MrowCellKeys.Price.ToString().Equals(this.mrsJuchuMeisai.CurrentCell.Name) //curCellPosition.CellIndex == MrowCellKeys.Price
                && this.mrsJuchuMeisai.EditingControl != null)
            {
                //編集コントロールから
                wk_price =
                    Convert.ToDecimal(
                        GetEditingControlValue(this.mrsJuchuMeisai.EditingControl));
            }
            else
            {
                //セルから
                wk_price =
                    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(
                        rowIdx, MrowCellKeys.Price.ToString()));
            }

            int carKbn = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIdx, MrowCellKeys.CarKbn.ToString()));

            ////車両区分が傭車の場合
            //if (carKbn == (int)BizProperty.DefaultProperty.CarKbn.Yosha)
            //{
            //    TorihikisakiSearchParameter torihiki_para = new TorihikisakiSearchParameter();
            //    torihiki_para.ToraDONTorihikiCode = Convert.ToInt32(this.mrsJuchuMeisai.GetValue(rowIdx, MrowCellKeys.CarOfChartererCd.ToString()));
            //    TorihikisakiInfo tri_info = this._DalUtil.Torihikisaki.GetList(torihiki_para).FirstOrDefault();

            //    if (tri_info != null)
            //    {
            //        decimal rate = tri_info.CharterRate;
            //        int cutkbn = tri_info.GakCutKbn;

            //        //金額を計算
            //        decimal wk_charterprice = 0;
            //        wk_charterprice =
            //            this.GetRoundVal(rate * wk_price / 100, cutkbn);

            //        //金額
            //        this.mrsJuchuMeisai.SetValue(rowIdx, MrowCellKeys.CharterPrice.ToString(), wk_charterprice);

            //        //通行料
            //        decimal tollFeeInCharterPrice =
            //            Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIdx, MrowCellKeys.TollFeeInCharterPrice.ToString()));

            //        //傭車確定フラグ
            //        bool wk_charterfixflag = false;
            //        if (wk_charterprice != 0 || tollFeeInCharterPrice != 0)
            //        {
            //            wk_charterfixflag = true;
            //        }
            //        this.mrsJuchuMeisai.SetValue(rowIdx, MrowCellKeys.CharterFixFlag.ToString(), wk_charterfixflag);
            //    }
            //}
        
        }

        /// <summary>
        /// 対象値、丸め(端数)区分を指定して、端数を丸めた値を返します。
        /// </summary>
        /// <param name="val">対象値</param>
        /// <param name="cutKbn">丸め(端数)区分</param>
        /// <returns>端数を丸めた値</returns>
        private decimal GetRoundVal(decimal val, int cutKbn)
        {
            decimal rt_val = 0;

            //端数を丸める
            switch (cutKbn)
            {
                case (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff:
                    //四捨五入
                    rt_val = NSKUtil.Round(val, NSKRoundType.ROUND_TYPE_OFF);

                    break;

                case (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundDown:
                    //切捨て
                    rt_val = NSKUtil.Round(val, NSKRoundType.ROUND_TYPE_DOWN);

                    break;

                case (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundUp:
                    //切上げ
                    rt_val = NSKUtil.Round(val, NSKRoundType.ROUND_TYPE_UP);

                    break;

                default:
                    break;
            }

            return rt_val;
        }

        #region DBアクセス

        /// <summary>
        /// 受注明細上の単価取得の条件となる値を指定して、受注単価を取得します。
        /// </summary>
        /// <param name="TokuisakiCd">得意先コード</param>
        /// <param name="itemCode">品目コード</param>
        /// <param name="startPointCode">積地コード</param>
        /// <param name="endPointCode">着地コード</param>
        /// <param name="figCode">単位コード</param>
        /// <returns>受注単価</returns>
        private decimal GetAtPriceByJuchuDtlKey(int rowIndex, int TokuisakiCd, int itemCode, int startPointCode,
    int endPointCode, int figCode, int carKindCode)
        {
            decimal rt_val = 0;

            ////現在行の単価を取得
            //decimal wk_atprice =
            //    Convert.ToDecimal(this.mrsJuchuMeisai.GetValue(rowIndex, MrowCellKeys.AtPrice.ToString()));

            ////得意先情報を取得  
            //TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = TokuisakiCd };
            //TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            //////取得条件を指定して単価を取得
            //AtPriceInfo wk_info =
            //    this._JuchuNyuryoku.GetAtPrice(TokuisakiCd, itemCode, startPointCode,
            //        endPointCode, figCode, carKindCode, tokui_info);

            //if (wk_info != null)
            //{
            //    //存在したら取得した単価をセット
            //    if (wk_atprice == 0)
            //    {
            //        rt_val = wk_info.AtPrice;
            //    }
            //    else
            //    {
            //        if (wk_atprice != wk_info.AtPrice)
            //        {
            //            //現在の単価と取得単価が異なる場合は、単価変更の確認メッセージ
            //            DialogResult wk_result =
            //                MessageBox.Show(
            //                    FrameUtilites.GetDefineMessage(
            //                        "MQ2102009", new string[] { "単価" }),
            //                    "確認",
            //                    MessageBoxButtons.YesNo,
            //                    MessageBoxIcon.Question);

            //            if (wk_result == DialogResult.Yes)
            //            {
            //                rt_val = wk_info.AtPrice;
            //            }
            //            else
            //            {

            //            }
            //        }
            //    }
            //}

            return rt_val;
        }

        #endregion

        #endregion

        #region 車両区分関連

        /// <summary>
        /// 指定した車両区分が傭車かどうかを返します。
        /// 車両区分の値が0の場合はfalseを返します。
        /// </summary>
        /// <param name="carKbn"></param>
        /// <returns></returns>
        private bool CarKbnIsCharter(int carKbn)
        {
            return (carKbn == (int)BizProperty.DefaultProperty.CarKbn.Yosha);
        }

        ///// <summary>
        ///// 車両情報から画面の傭車区分に設定する値を取得します。
        ///// </summary>
        ///// <param name="info"></param>
        ///// <returns></returns>
        //private string GetCharterKbnValue(CarInfo info)
        //{
        //    ////営業所情報を取得
        //    //ToraDONBranchOfficeSearchParameter BranchOffice_para = new ToraDONBranchOfficeSearchParameter();
        //    //BranchOffice_para.BranchOfficeId = info.BranchOfficeId;
        //    //ToraDONBranchOfficeInfo BranchOffice_info = this._DalUtil.ToraDONBranchOffice.GetList(BranchOffice_para).FirstOrDefault();

        //    //if (BranchOffice_info == null || BranchOffice_info.BranchOfficeId == 0)
        //    //{
        //    //    return string.Empty;
        //    //}

        //    return this.GetCharterKbnValue(info.CarKbn, BranchOffice_info.BranchOfficeShortName);
        //}

        /// <summary>
        /// 車両区分と車両の営業所情報を指定して、画面の傭車区分に設定する情報を取得します。
        /// </summary>
        /// <param name="carKbn"></param>
        /// <param name="carBranchOfficeShortName"></param>
        /// <returns></returns>
        private string GetCharterKbnValue(int carKbn, string carBranchOfficeShortName)
        {
            if (carKbn == (int)BizProperty.DefaultProperty.CarKbn.Yosha)
            {
                SystemNameInfo carKbnInfo = this._DalUtil.SystemGlobalName.GetInfo((int)BizProperty.DefaultProperty.SystemNameKbn.CarKbn, carKbn);

                return (carKbnInfo != null) ? carKbnInfo.SystemName : string.Empty;
            }
            else
            {
                return carBranchOfficeShortName;
            }
        }

        /// <summary>
        /// 指定した受注明細行の車両区分に連動する傭車関連の値をクリアします。
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ClearCharterValuesForCarKbn(int rowIndex)
        {
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CarOfChartererName.ToString(), string.Empty);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterPrice.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.TollFeeInCharterPrice.ToString(), 0);
            this.mrsJuchuMeisai.Rows[rowIndex].Cells[MrowCellKeys.CharterTaxDispKbn.ToString()].Tag = (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei;
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterTaxDispKbn.ToString(), (int)BizProperty.DefaultProperty.ZeiKbn.Sotozei);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterAddUpYMD.ToString(), null);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.CharterFixFlag.ToString(), false);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.MagoYoshasaki.ToString(), string.Empty);
        }

        /// <summary>
        /// 指定した受注明細行の車両区分に連動する乗務員関連の値をクリアします。
        /// </summary>
        /// <param name="rowIndex"></param>
        private void ClearDriverValuesForCarKbn(int rowIndex)
        {
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverCd.ToString(), 0);
            this.mrsJuchuMeisai.SetValue(rowIndex, MrowCellKeys.DriverNm.ToString(), string.Empty);
        }

        #endregion

        #region スタイル（入力の可否, 背景色）等の設定

        private bool IsMonthlyTotaled(DateTime addUpDate, bool fixFlag, DateTime lastSumOfMonthDay)
        {
            //戻り値用
            bool rt_val = false;

            if (lastSumOfMonthDay != DateTime.MinValue && addUpDate != DateTime.MinValue)
            {
                //確定済みで、計上日が最終月次集計月(末日)以前の場合は集計済みとする
                if (addUpDate <= lastSumOfMonthDay && fixFlag)
                {
                    rt_val = true;
                }
            }

            return rt_val;
        }

        /// <summary>
        /// セルを指定して、テンプレートの背景色を取得します。
        /// </summary>
        /// <param name="cell">テンプレートの背景色</param>
        private Color GetTempleteCellBackColor(Cell cell)
        {
            //テンプレートから取得
            return cell.GcMultiRow.Template.Row[cell.CellIndex].Style.BackColor;
        }

        /// <summary>
        /// 受注明細からロックされている状態の理由を取得します。
        /// </summary>
        /// <returns>ロック行数</returns>
        private LockPattern GetMRowLockPatternJuchuDtlList()
        {
            //戻り値用
            LockPattern rt_val =
                LockPattern.None;

            //MultiRowの描画を停止
            this.mrsJuchuMeisai.SuspendLayout();

            try
            {
                //最終月次集計月を取得しておく
                DateTime wk_date = this._ToraDonSystemPropertyInfo.LastSummaryOfMonthDate;
                DateTime last_sum_of_monthday;
                if (wk_date > DateTime.MinValue)
                {
                    last_sum_of_monthday = NSKUtil.MonthLastDay(wk_date);
                }
                else
                {
                    last_sum_of_monthday = wk_date;
                }

                //明細を回しながら受注情報作成
                int rowcount = this._JuchuInfoSelList.Count;

                for (int i = 0; i < rowcount; i++)
                {
                    //1行分を取得
                    GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[i];

                    //1件分の受注情報を作成
                    JuchuNyuryokuInfo info = new JuchuNyuryokuInfo();

                    //修正データは行コレクションのTagから復元する
                    if (this.mrsJuchuMeisai.Rows[i].Tag != null)
                    {
                        info = (JuchuNyuryokuInfo)wk_mrow.Tag;
                    }
                    else
                    {
                        //Tagに受注情報が存在しない場合はスキップ
                        continue;
                    }

                    //TODO 性能問題のためコメント化中 START
                    ////請求済みの場合
                    //if (info.MinClmFixDate != DateTime.MinValue)
                    //{
                    //    //請求済みを追加
                    //    rt_val = rt_val | LockPattern.JuchuDtlIsFixedClaim;
                    //}

                    ////支払済みの場合
                    //if (info.MinCharterPayFixDate != DateTime.MinValue)
                    //{
                    //    //支払済みを追加
                    //    rt_val = rt_val | LockPattern.JuchuDtlIsFixedPay;
                    //}
                    //TODO 性能問題のためコメント化中 END

                    //月次締処理済みの場合
                    //（月次締処理済みに関しては配車Ace側の計上日、確定フラグに加え、
                    //  請求データ連携後のトラDONの売上テーブルの計上日、確定フラグもチェック対象とする）
                    if (info.JuchuId != 0 &&
                        (this.IsMonthlyTotaled(info.AddUpYMD, info.FixFlag, last_sum_of_monthday) ||
                        //TODO 性能問題のためコメント化中 START
                        //this.IsMonthlyTotaled(info.MinAddUpDate, info.MaxFixFlag, last_sum_of_monthday) ||
                        //TODO 性能問題のためコメント化中 END
                        this.IsMonthlyTotaled(info.CharterAddUpYMD, info.CharterFixFlag, last_sum_of_monthday)
                        //TODO 性能問題のためコメント化中 START
                        //||
                        //this.IsMonthlyTotaled(info.MinCharterAddUpDate, info.MaxCharterFixFlag, last_sum_of_monthday)
                        //TODO 性能問題のためコメント化中 END
                        ))
                    {
                        //月締済みを追加
                        rt_val = rt_val | LockPattern.JuchuDtlIsFixedMonth;
                    }
                }
            }
            catch (Exception err)
            {
                throw err;
            }
            finally
            {
                //MultiRowの描画を再開
                this.mrsJuchuMeisai.ResumeLayout();
            }

            return rt_val;
        }

        /// <summary>
        /// コントロールに非ロック時の背景色を設定します。
        /// </summary>
        /// <param name="ctl">コントロール</param>
        /// <returns></returns>
        private void SetNoLockControlBackColor(Control ctl)
        {
            ctl.BackColor = this.GetNoLockControlBackColor(ctl);
        }

        /// <summary>
        /// 指定したコントロールの非ロック時の背景色を取得します。
        /// </summary>
        /// <param name="ctl">コントロール</param>
        /// <returns>アンロック</returns>
        private Color GetNoLockControlBackColor(Control ctl)
        {
            //数値セルでサイドボタンが存在する
            if (ctl is GcNumberEditingControl &&
                (ctl as GcNumberEditingControl).SideButtons.Count != 0)
            {
                if (ctl is GcNumberEditingControl)
                {
                    GrapeCity.Win.Editors.SideButton sideButton =
                        (ctl as GcNumberEditingControl).SideButtons[0] as GrapeCity.Win.Editors.SideButton;

                    return Color.LightYellow;
                }
            }

            return SystemColors.Window;
        }

        /// <summary>
        /// フォーカス移動の制御をするために、項目のロックをReadOnlyにて実装するセルのリスト
        /// </summary>
        private List<MrowCellKeys> _JuchuDtlReadOnlyLockCellList;

        /// <summary>
        /// セルの編集可／不可を切り替えないセルのリスト
        /// 税区分（常にSelectable：False）などの常に編集の可／不可が不変の項目が対象になる
        /// </summary>
        private List<MrowCellKeys> _JuchuDtlUnchangeEditableCellList;

        /// <summary>
        /// 諸口区分のスタイル
        /// </summary>
        /// <param name="rowIndex">受注明細行インデックス</param>
        /// <param name="memoAccount">諸口区分</param>
        private void RefreshStyleForMemoAccount(int rowIndex, int memoAccount)
        {
            GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIndex];

            bool isMemoaccount = (memoAccount == 1);

            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TokuisakiNM.ToString()], !isMemoaccount);
        }

        /// <summary>
        /// 諸口区分のスタイル
        /// </summary>
        /// <param name="rowIndex">受注明細行インデックス</param>
        /// <param name="clmClassUseKbn">部門管理区分</param>
        private void RefreshStyleForClmClassUseKbn(int rowIndex, int clmClassUseKbn)
        {
            GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIndex];

            bool useClmClass = (clmClassUseKbn == 1);

            this.OriginalCellLocked(wk_mrow[MrowCellKeys.ClmClassCd.ToString()], !useClmClass);
        }

        /// <summary>
        /// 指定した受注明細行の車両区分に関連するセルのスタイルを更新します。
        /// </summary>
        /// <param name="rowIndex">受注明細行インデックス</param>
        /// <param name="carKbn">車両区分</param>
        private void RefreshStyleForCarKbn(int rowIndex, int carKbn = 0)
        {
            GrapeCity.Win.MultiRow.Row wk_mrow = this.mrsJuchuMeisai.Rows[rowIndex];

            bool carkbnIsYosha = this.CarKbnIsCharter(carKbn);

            //車番の編集可否の設定
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.LicPlateCarNo.ToString()], !carkbnIsYosha);
            //乗務員の編集可否の設定
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.DriverCd.ToString()], carkbnIsYosha);
            //傭車項目の編集可否の設定
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.CarOfChartererCd.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.CharterPrice.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.TollFeeInCharterPrice.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.CharterTaxDispKbn.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.CharterAddUpYMD.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.CharterFixFlag.ToString()], !carkbnIsYosha);
            this.OriginalCellLocked(wk_mrow[MrowCellKeys.MagoYoshasaki.ToString()], !carkbnIsYosha);
        }

        /// <summary>
        /// MultiRowセルに対して、独自のロック制御をします。
        /// </summary>
        /// <param name="cell">セル</param>
        /// <param name="locked">ロックするかどうか</param>
        private void OriginalCellLocked(Cell cell, bool locked)
        {
            //無ければ作成するのでその判断
            if (this._JuchuDtlUnchangeEditableCellList == null)
            {
                this._JuchuDtlUnchangeEditableCellList = new List<MrowCellKeys>()
                {
                    MrowCellKeys.TaxDispKbn,
                    MrowCellKeys.CharterTaxDispKbn,
                };
            }

            //編集可／不可を切り替えないセルかどうか
            bool cellIsUnchangeEditable = this._JuchuDtlUnchangeEditableCellList.Any(element => element.ToString() == cell.Name);

            if (locked)
            {
                if (this._JuchuDtlReadOnlyLockCellList == null)
                {
                    this._JuchuDtlReadOnlyLockCellList = new List<MrowCellKeys>()
                    {
                        MrowCellKeys.TokuisakiNM,
                        MrowCellKeys.ClmClassCd,
                        MrowCellKeys.LicPlateCarNo,
                    };
                }

                //Selectable = Falseのロックだとフォーカス移動に不都合があるかどうか？
                if (cell.GcMultiRow == this.mrsJuchuMeisai &&
                    this._JuchuDtlReadOnlyLockCellList.Any(c => c.ToString() == cell.Name))
                {
                    if (!cellIsUnchangeEditable)
                    {
                        cell.ReadOnly = true;
                    }


                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = _originalLockBackColor;
                    }

                }
                else
                {
                    if (!cellIsUnchangeEditable)
                    {
                        cell.Selectable = false;
                    }

                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = _originalLockBackColor;
                    }
                }
            }
            else
            {
                Color backColor = this.GetTempleteCellBackColor(cell);

                if (cell.GcMultiRow == this.mrsJuchuMeisai &&
                    this._JuchuDtlReadOnlyLockCellList.Any(c => c.ToString() == cell.Name))
                {
                    if (!cellIsUnchangeEditable)
                    {
                        cell.ReadOnly = false;
                    }

                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = backColor;
                    }
                }
                else
                {
                    if (!cellIsUnchangeEditable)
                    {
                        cell.Selectable = true;
                    }

                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = backColor;
                    }
                }
            }
        }

        /// <summary>
        /// MultiRow行に対して、独自のロック制御をします。
        /// </summary>
        /// <param name="row">行</param>
        /// <param name="locked">ロックするかどうか</param>
        private void OriginalRowLocked(Row row, bool locked)
        {
            if (locked)
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = _originalLockBackColor;
                        //販路マスタ追加ボタン以外の場合
                        if (!cell.Name.Equals(MrowCellKeys.HanroAddButton.ToString()))
                        {
                            //非活性
                            cell.Selectable = false;
                        }
                        else
                        {
                            cell.Selectable = true;
                        }
                    }
                }
            }
            else
            {
                foreach (Cell cell in row.Cells)
                {
                    if (cell.Style.GetType() != typeof(DynamicCellStyle))
                    {
                        cell.Style.BackColor = _originalLockBackColor;
                        cell.Selectable = true;
                    }
                }
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

        private void mrsJuchuMeisai_CellContentClick(object sender, CellEventArgs e)
        {
            if (e.Scope == CellScope.Row)
            {
                if (e.CellName.Equals(MrowCellKeys.OfukuButton.ToString()))
                {
                    //明細行が非活性の場合
                    if (!this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[MrowCellKeys.HanroCd.ToString()].Selectable)
                    {
                        return;
                    }

                    int ofukuKbn = Convert.ToInt32(this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Tag);

                    int newOfukuKbn = 0;
                    string newOfukuKbnName = string.Empty;

                    IList<SystemNameInfo> list = UserProperty.GetInstance().SystemNameList
                        .Where(x => x.SystemNameKbn == (int)DefaultProperty.SystemNameKbn.OfukuKbn
                            && x.SystemNameCode == ofukuKbn + 1)
                        .ToList();

                    if (0 < list.Count)
                    {
                        newOfukuKbn = list.FirstOrDefault().SystemNameCode;
                        newOfukuKbnName = list.FirstOrDefault().SystemName;
                    }

                    this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Tag = newOfukuKbn;
                    this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Value = newOfukuKbnName;

                    // 最終行の場合は行追加
                    if (e.RowIndex == this.mrsJuchuMeisai.RowCount - 1)
                    {
                        //最終行へ行追加
                        this.mrsJuchuMeisai.NotifyCurrentCellDirty(true);
                    }
                }
                else if (e.CellName.Equals(MrowCellKeys.HanroAddButton.ToString()))
                {
                    //実行確認ダイアログ
                    DialogResult d_result =
                        MessageBox.Show(FrameUtilites.GetDefineMessage(
                            "MQ2102019", new string[] { "販路マスタ" }),
                        "確認",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question,
                        MessageBoxDefaultButton.Button1);

                    //Noだったら抜ける
                    if (d_result == DialogResult.No)
                    {
                        return;
                    }

                    // 作業開始日付
                    DateTime TaskStartDateTime = DateTime.MinValue;
                    DateTime DateTime_TaskStartYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.TaskStartYMD.ToString()));
                    // 作業開始時刻
                    DateTime DateTime_TaskStartHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.TaskStartHM.ToString()));
                    if (DateTime_TaskStartYMD != DateTime.MinValue)
                    {
                        if (DateTime_TaskStartHM != DateTime.MinValue)
                        {
                            TaskStartDateTime = new DateTime(
                                DateTime_TaskStartYMD.Year,
                                DateTime_TaskStartYMD.Month,
                                DateTime_TaskStartYMD.Day,
                                DateTime_TaskStartHM.Hour,
                                DateTime_TaskStartHM.Minute,
                                0);
                        }
                        else
                        {
                            TaskStartDateTime = DateTime_TaskStartYMD.Date;
                        }
                    }

                    // 作業終了日付
                    DateTime TaskEndDateTime = DateTime.MinValue;
                    DateTime DateTime_TaskEndYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.TaskEndYMD.ToString()));
                    // 作業終了時刻
                    DateTime DateTime_TaskEndHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.TaskEndHM.ToString()));
                    if (DateTime_TaskEndYMD != DateTime.MinValue)
                    {
                        if (DateTime_TaskEndHM != DateTime.MinValue)
                        {
                            TaskEndDateTime = new DateTime(
                                DateTime_TaskEndYMD.Year,
                                DateTime_TaskEndYMD.Month,
                                DateTime_TaskEndYMD.Day,
                                DateTime_TaskEndHM.Hour,
                                DateTime_TaskEndHM.Minute,
                                0);
                        }
                        else
                        {
                            TaskEndDateTime = DateTime_TaskEndYMD.Date;
                        }
                    }

                    // 再使用可能日付
                    DateTime ReuseYMD = DateTime.MinValue;
                    DateTime DateTime_ReuseYMD = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.ReuseYMD.ToString()));
                    // 再使用可能時刻
                    DateTime DateTime_ReuseHM = Convert.ToDateTime(this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.ReuseHM.ToString()));
                    if (DateTime_ReuseYMD != DateTime.MinValue)
                    {
                        if (DateTime_ReuseHM != DateTime.MinValue)
                        {
                            ReuseYMD = new DateTime(
                                DateTime_ReuseYMD.Year,
                                DateTime_ReuseYMD.Month,
                                DateTime_ReuseYMD.Day,
                                DateTime_ReuseHM.Hour,
                                DateTime_ReuseHM.Minute,
                                0);
                        }
                        else
                        {
                            ReuseYMD = DateTime_ReuseYMD.Date;
                        }
                    }

                    // 行程日時
                    int[] koteiList = this.GetSpanDateTime(TaskStartDateTime, TaskEndDateTime);

                    // 再使用可能日時
                    int[] reuseList = this.GetSpanDateTime(TaskEndDateTime, ReuseYMD);

                    HanroInfo param = new HanroInfo()
                    {
                        HanroCode = 0,
                        HanroName = string.Empty,
                        HanroNameKana = string.Empty,
                        HanroSName = string.Empty,
                        HanroSSName = string.Empty,
                        ToraDONTokuisakiCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.TokuisakiCd.ToString())),
                        ToraDONHatchiCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.StartPointCd.ToString())),
                        ToraDONChakuchiCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.EndPointCd.ToString())),
                        ToraDONItemCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.ItemCd.ToString())),
                        ToraDONCarKindCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.CarKindCd.ToString())),
                        ToraDONCarCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.CarCd.ToString())),
                        ToraDONTorihikiCode = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.CarOfChartererCd.ToString())),
                        ToraDONCarKbn = Convert.ToInt32(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.CarKbn.ToString())),
                        OfukuKbn = Convert.ToInt32(
                            this.mrsJuchuMeisai.Rows[e.RowIndex].Cells[MrowCellKeys.OfukuButton.ToString()].Tag),
                        SeikyuTanka = Convert.ToDecimal(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.AtPrice.ToString())),
                        YoshaKingaku = Convert.ToDecimal(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.CharterPrice.ToString())),
                        Futaigyomuryo = Convert.ToDecimal(
                            this.mrsJuchuMeisai.GetValue(e.RowIndex, MrowCellKeys.FutaigyomuryoInPrice.ToString())),
                        KoteiNissu = koteiList[0],
                        KoteiJikan = koteiList[1],
                        ReuseNissu = reuseList[0],
                        ReuseJikan = reuseList[1],
                    };

                    //using (HanroFrame f = new HanroFrame() { ParamHanroInfo = param })
                    //{
                    //    f.InitFrame();
                    //    f.ShowDialog();
                    //}
                }
            }
        }

        private void mrsJuchuMeisai_CellEditedFormattedValueChanged(object sender, CellEditedFormattedValueChangedEventArgs e)
        {
            this.ProcessJuchuNyuryokuMeisaiMrowCellEditedFormattedValueChanged(e);
        }

        /// <summary>
        /// 販路グループ検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchHanroGroup()
        {
            //using (CmnSearchHanroGroupFrame f = new CmnSearchHanroGroupFrame())
            //{
            //    f.InitFrame();
            //    f.ShowDialog(this);
            //    if (f.DialogResult == DialogResult.OK)
            //    {
            //        //画面から値を取得
            //        ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
            //            Convert.ToInt32(f.SelectedInfo.HanroGroupCode);

            //        this.OnCmnSearchComplete();
            //    }
            //}
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            //共通検索画面
            this.ShowCmnSearch();
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
        /// 検索用販路コードの値を取得します。
        /// </summary>
        private int SearchHanroGroupCode
        {
            get { return Convert.ToInt32(this.numFukushaHanroGroupCode.Value); }
        }

        private void numSearchHanroGroupCode_Validating(object sender, CancelEventArgs e)
        {
            //検索用販路グループコード
            this.ValidateSearchHanroGroupCode(e);
        }

        /// <summary>
        /// 検索用販路グループコードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateSearchHanroGroupCode(CancelEventArgs e)
        {
            bool is_clear = false;

            //try
            //{
            //    //コードの入力が無い場合は抜ける
            //    if (this.SearchHanroGroupCode == 0)
            //    {
            //        is_clear = true;
            //        return;
            //    }

            //    HanroGroupInfo info =
            //        this._DalUtil.HanroGroup.GetInfo(this.SearchHanroGroupCode);

            //    if (info == null)
            //    {
            //        //データなし
            //        MessageBox.Show(
            //            FrameUtilites.GetDefineMessage("MW2201003"),
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Warning);

            //        is_clear = true;
            //        e.Cancel = true;
            //    }
            //    else
            //    {
            //if (info.DisableFlag)
            //{
            //    MessageBox.Show(
            //        FrameUtilites.GetDefineMessage("MW2201016"),
            //        this.Text,
            //        MessageBoxButtons.OK,
            //        MessageBoxIcon.Warning);

            //    is_clear = true;
            //    e.Cancel = true;
            //}
            //else
            //{
            //    this.numFukushaHanroGroupCode.Tag = info.HanroGroupId;
            //    this.numFukushaHanroGroupCode.Value = info.HanroGroupCode;
            //    this.edtFukushaHanroGroupName.Text = info.HanroGroupName;
            //}
            //    }
            //}
            //finally
            //{
            //    if (is_clear)
            //    {
            //        this.numFukushaHanroGroupCode.Tag = null;
            //        this.numFukushaHanroGroupCode.Value = null;
            //        this.edtFukushaHanroGroupName.Text = string.Empty;
            //    }
            //}

        }

        /// <summary>
        /// 販路一括設定を行います。
        /// </summary>
        private void btnHanroIkkatsu_Click(object sender, EventArgs e)
        {

            //try
            //{

            //    if (this.CheckHanroIkkatsu())
            //    {
            //        DialogResult wk_result = MessageBox.Show(
            //            "販路を一括設定します。よろしいですか？",
            //            "確認",
            //            MessageBoxButtons.YesNo,
            //            MessageBoxIcon.Question,
            //            MessageBoxDefaultButton.Button2);

            //        if (wk_result == DialogResult.No)
            //        {
            //            //Noの場合は処理中断
            //            return;
            //        }

            //        var hanroGroupDal = new HanroGroupMeisai();

            //        HanroGroupMeisaiSearchParameter para = new HanroGroupMeisaiSearchParameter();

            //        //検索条件
            //        para.HanroGroupId = Convert.ToDecimal(this.numFukushaHanroGroupCode.Tag);   //販路グループコード
            //        para.DisableFlag = false;  //非表示フラグ

            //        //マウスカーソルを待機中(砂時計)に変更
            //        Cursor.Current = Cursors.WaitCursor;

            //        //描画を停止
            //        this.SuspendLayout();
            //        this.mrsJuchuMeisai.SuspendLayout();

            //        //販路グループの検索
            //        var list = hanroGroupDal.GetList(para);

            //        if (list.Count() == 0) 
            //        {

            //            MessageBox.Show(
            //                FrameUtilites.GetDefineMessage("MW2201015"),
            //                "警告",
            //                MessageBoxButtons.OK,
            //                MessageBoxIcon.Warning);

            //            this.numFukushaHanroGroupCode.Focus();
            //            return;

            //        }

            //        //明細行数
            //        int rowCount = this.mrsJuchuMeisai.RowCount;

            //        //行数を設定
            //        this.mrsJuchuMeisai.RowCount = rowCount + list.Count;

            //        for (int i = 0; i < list.Count; i++)
            //        {
            //            //1件分の受注情報を取得
            //            HanroGroupMeisaiInfo info = list[i];
            //            int mrowidx = i + rowCount - 1;

            //            //販路コード
            //            this.mrsJuchuMeisai.SetValue(
            //            mrowidx, MrowCellKeys.HanroCd.ToString(), info.HanroCode);

            //            //選択可能の場合のみフォーカスを遷移する
            //            if (this.mrsJuchuMeisai[mrowidx, MrowCellKeys.HanroCd.ToString()].Selectable)
            //            {
            //                this.mrsJuchuMeisai.CurrentCellPosition =
            //                    new CellPosition(mrowidx, MrowCellKeys.HanroCd.ToString());
            //            }

            //            this.HanroCdCellValidating(mrowidx, true);
            //        }

            //        //登録完了メッセージ
            //        MessageBox.Show(
            //            "販路の一括設定が完了しました。",
            //            this.Text,
            //            MessageBoxButtons.OK,
            //            MessageBoxIcon.Information);

            //    }
            //}
            //catch (CanRetryException ex)
            //{
            //    //データがない場合の例外ハンドラ
            //    FrameUtilites.ShowExceptionMessage(ex, this);
            //}
            //catch (Exception)
            //{
            //    throw;
            //}
            //finally
            //{
            //    //マウスカーソルを元に戻す
            //    Cursor.Current = Cursors.Default;

            //    //描画を再開
            //    this.ResumeLayout();
            //    this.mrsJuchuMeisai.ResumeLayout();
            //}
        
        }

        /// <summary>
        /// 販路一括設定時の入力項目をチェックします。
        /// </summary>
        /// <returns>チェック結果</returns>
        private bool CheckHanroIkkatsu()
        {
            ////戻り値用
            bool rt_val = true;

            string msg = string.Empty;
            MessageBoxIcon icon = MessageBoxIcon.None;
            Control ctl = null;

            //営業所の必須チェック
            if (rt_val && Convert.ToInt32(this.numFukushaHanroGroupCode.Value) == 0)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                        "MW2203004", new string[] { "販路グループ" });
                icon = MessageBoxIcon.Warning;
                ctl = this.numFukushaHanroGroupCode;
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
    }
}
