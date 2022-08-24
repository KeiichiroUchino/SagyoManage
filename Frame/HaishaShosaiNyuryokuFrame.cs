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
using Jpsys.HaishaManageV10.FrameLib.WinForms;
using Jpsys.HaishaManageV10.FrameLib.ViewSupport;
using Jpsys.HaishaManageV10.SQLServerDAL;
using GrapeCity.Win.Editors;
using jp.co.jpsys.util;
using System.ComponentModel;


namespace Jpsys.HaishaManageV10.Frame
{
    public partial class HaishaShosaiNyuryokuFrame : Form, IFrameBase
    {
        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaShosaiNyuryokuFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 受注№を指定して、本クラスの初期化を行います。
        /// </summary>
        /// <param name="saleSlipNo"></param>
        public HaishaShosaiNyuryokuFrame(HaishaNyuryokuInfo HaishaShosaiInfo, JuchuInfo JuchuShosaiInfo)
        {
            InitializeComponent();

            //メンバにセット
            this.JuchuShosaiInfo = JuchuShosaiInfo;
            this.HaishaInfo = HaishaShosaiInfo;

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

        #endregion

        #region 配車詳細入力

        /// <summary>
        /// 画面のタイトル
        /// </summary>
        private const string WINDOW_TITLE = "配車詳細入力";
        private const string WINDOW_TITLE_INFO = "配車詳細情報";

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
        /// 処理種別の列挙型
        /// </summary>
        public enum HaishaDialogResult
        {
            Cancel = 0,
            Update = 1,
            Copy = 2,
            Delete = 3,
        }

        /// <summary>
        /// 戻り値（処理種別）処理種別を保持する領域
        /// </summary>
        public HaishaDialogResult ResultProcessing;

        /// <summary>
        /// 受注情報を保持する領域
        /// </summary>
        private JuchuInfo JuchuShosaiInfo;

        /// <summary>
        /// 配車情報を保持する領域
        /// </summary>
        private HaishaNyuryokuInfo HaishaInfo;

        /// <summary>
        /// システム名称情報のリストを保持する領域
        /// </summary>
        private IList<SystemNameInfo> systemNameList =
            UserProperty.GetInstance().SystemNameList;

        /// <summary>
        /// 管理マスタレコードを保持する。
        /// </summary>
        private ToraDonSystemPropertyInfo _ToraDonSystemPropertyInfo;

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

        /// <summary>
        /// 得金額丸め区分
        /// </summary>
        private int TokuiGakCutKbn;

        /// <summary>
        /// 消費税丸め区分
        /// </summary>
        private int TokuiTaxCutKbn;

        /// <summary>
        /// 傭車消費税丸め区分
        /// </summary>
        private int CharterTaxCutKbn;

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitHaishaShosaiNyuryokuFrame()
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
            this.InitMenuHaishaShosaiNyuryoku();

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

            //管理情報取得
            this._ToraDonSystemPropertyInfo = this._DalUtil.ToraDonSystemProperty.GetInfo();

            //各種画面で頻繁に利用されるデータアクセスオブジェクトを利用するためのファサードクラスのインスタンス作成
            this._DalUtil = new CommonDALUtil(this.appAuth);

            // 検索画面を起動するコントロールと検索画面起動メソッドの設定
            this.SearchFunctions = new Dictionary<Control, System.Action>
            {
                {this.numHaishaStartPointCd, this.ShowCmnSearchPoint},
                {this.numHaishaEndPointCd, this.ShowCmnSearchPoint},
                {this.numFigCd, this.ShowCmnSearchFig},
                {this.numStaffCd, this.ShowCmnSearchStaff},
                {this.numCarOfChartererCd, this.ShowCmnSearchChartererCar},
                {this.numCarCd, this.ShowCmnSearchCar},
                {this.numCarKindCd, this.ShowCmnSearchCarKind},
                {this.numItemCd, this.ShowCmnSearchItem},
                {this.numOwnerCd, this.ShowCmnSearchOwner},
            };

            //画面レイアウト調整
            this.SetLeyout();

            //数量、単価、小数部制御
            this.SetControlDigits();

            //コンボボックスの初期化
            this.InitCombo();

            //配車情報の設定
            this.InitInputs();

            if (this.HaishaInfo.UppdateFlg)
            {
                //現在の画面モードを修正状態に変更
                this.ChangeMode(FrameEditMode.Editable);
            }
            else
            {
                //現在の画面モードを参照状態に変更
                this.ChangeMode(FrameEditMode.ViewOnly);
            }

            // 得金額丸め区分
            this.GetCutkbn();
            this.GetCharterCutkbn();
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
        private void InitMenuHaishaShosaiNyuryoku()
        {
            // 操作メニュー
            this.InitActionMenuHaishaShosaiNyuryokus();
        }

        /// <summary>
        /// 操作メニューを初期化します。
        /// </summary>
        private void InitActionMenuHaishaShosaiNyuryokus()
        {
            //操作メニューのクラスをインスタンス化
            this.actionMenuItems = new AddActionMenuItem();
            //メニューを作成
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Copy);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Update);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Delete);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Separator);
            this.actionMenuItems.SetCreatingItem(ActionMenuItems.Close);

            //画面内のメニューにファイルメニューを追加
            this.actionMenuItems.AddMenu(this.menuStripTop);
        }

        /// <summary>
        /// 配車情報を設定します。
        /// </summary>
        private void InitInputs()
        {
            /***************
             * 配車情報
             ***************/
            // 積地名
            this.numHaishaStartPointCd.Tag = this.HaishaInfo.StartPointId;
            this.numHaishaStartPointCd.Value = this.HaishaInfo.StartPointCode;
            this.edtHaishaStartPointNM.Text = this.HaishaInfo.StartPointName;
            // 着地名
            this.numHaishaEndPointCd.Tag = this.HaishaInfo.EndPointId;
            this.numHaishaEndPointCd.Value = this.HaishaInfo.EndPointCode;
            this.edtHaishaEndPointNM.Text = this.HaishaInfo.EndPointName;
            // 車両
            this.numCarCd.Tag = new CarInfo() { ToraDONCarId = this.HaishaInfo.CarId, CarKbn = this.HaishaInfo.CarKbn, LicPlateCarNo = this.HaishaInfo.CarLicPlateCarNo };
            this.numCarCd.Value = this.HaishaInfo.CarCode;
            // 車番
            if (this.HaishaInfo.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            {
                this.edtLicPlateCarNo.Text = this.HaishaInfo.LicPlateCarNo;
            }
            else
            {
                this.edtLicPlateCarNo.Text = this.HaishaInfo.CarLicPlateCarNo;
            }
            // 車種
            this.numCarKindCd.Tag = this.HaishaInfo.CarKindId;
            this.numCarKindCd.Value = this.HaishaInfo.CarKindCode;
            this.edtCarKindNm.Text = this.HaishaInfo.CarKindName;
            // 発日時
            this.dteStartYMD.Value = this.HaishaInfo.StartYMD;
            this.dteStartHM.Value = this.HaishaInfo.StartYMD;
            // 積日時
            this.dteTaskStartYMD.Value = this.HaishaInfo.TaskStartDateTime;
            this.dteTaskStartHM.Value = this.HaishaInfo.TaskStartDateTime;
            // 着日時
            this.dteTaskEndYMD.Value = this.HaishaInfo.TaskEndDateTime;
            this.dteTaskEndHM.Value = this.HaishaInfo.TaskEndDateTime;
            // 再使用可能日
            this.dteReuseYMD.Value = this.HaishaInfo.ReuseYMD;
            this.dteReuseHM.Value = this.HaishaInfo.ReuseYMD;
            // コメント
            this.edtBiko.Text = this.HaishaInfo.Biko;
            // 乗務員・傭車先
            if (this.HaishaInfo.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
            {
                // 傭車の場合
                if (this.HaishaInfo.TorihikiCd != 0)
                {
                    this.numCarOfChartererCd.Value = this.HaishaInfo.TorihikiCd;
                }
                else
                {
                    this.numCarOfChartererCd.Value = null;
                }
                this.edtLicPlateCarNo.Text = this.HaishaInfo.LicPlateCarNo;
                this.numCarOfChartererCd.Tag = this.HaishaInfo.CarOfChartererId;
                this.edtCarOfChartererNm.Text = this.HaishaInfo.TorihikiName;
                this.edtMagoYoshasaki.Text = this.HaishaInfo.MagoYoshasaki;

                this.numStaffCd.Value = null;
                this.numStaffCd.Tag = decimal.Zero;
                this.edtStaffNm.Text = string.Empty;
            }
            else
            {
                // 自社車両の場合
                if (this.HaishaInfo.StaffCd != 0)
                {
                    this.numStaffCd.Value = this.HaishaInfo.StaffCd;
                }
                else
                {
                    this.numStaffCd.Value = null;
                }
                this.edtLicPlateCarNo.Text = this.HaishaInfo.CarLicPlateCarNo;
                this.numStaffCd.Tag = this.HaishaInfo.DriverId;
                this.edtStaffNm.Text = this.HaishaInfo.StaffName;

                this.numCarOfChartererCd.Value = null;
                this.numCarOfChartererCd.Tag = decimal.Zero;
                this.edtCarOfChartererNm.Text = string.Empty;
                this.edtMagoYoshasaki.Text = string.Empty;
            }

            /***************
             * 受注情報
             ***************/
            // 得意先
            this.numTokuisakiCd.Value = this.JuchuShosaiInfo.TokuisakiCode;
            this.edtTokuisakiNM.Text = this.JuchuShosaiInfo.TokuisakiName;
            // 販路
            this.numHanroCd.Value = this.JuchuShosaiInfo.HanroCode;
            this.edtHanroName.Text = this.JuchuShosaiInfo.HanroName;
            // 往復区分
            string strOfukuKbn = String.Empty;
            var OfukuKbnList = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.OfukuKbn)
                    && x.SystemNameCode == this.JuchuShosaiInfo.OfukuKbn);
            if (OfukuKbnList.Count() > 0)
            {
                strOfukuKbn = OfukuKbnList.First().SystemName;
            }
            this.edtOfukuKbn.Text = strOfukuKbn;
            // 営業所
            this.numBranchOfficeCd.Value = this.JuchuShosaiInfo.BranchOfficeCode;
            this.edtBranchOfficeNM.Text = this.JuchuShosaiInfo.BranchOfficeShortName;
            // 請求部門
            this.numClassCd.Value = this.JuchuShosaiInfo.ClmClassCode;
            this.edtClassNM.Text = this.JuchuShosaiInfo.ClmClassName;
            // 請負
            this.numContractCd.Value = this.JuchuShosaiInfo.ContractCode;
            this.edtContractNM.Text = this.JuchuShosaiInfo.ContractName;
            // 受注担当
            this.numJuchuTantoCd.Value = this.JuchuShosaiInfo.JuchuTantoCode;
            this.edtJuchuTantoNm.Text = this.JuchuShosaiInfo.JuchuTantoName;

            /***************
             * 請求情報
             ***************/
            // 品目
            this.numItemCd.Tag = this.HaishaInfo.ItemId;
            this.numItemCd.Value = this.HaishaInfo.ItemCode;
            this.edtItemNM.Text = this.HaishaInfo.ItemName;
            // 数量
            this.numNumber.Value = this.HaishaInfo.Number;
            this.lblNumber.Text = "/ " + this.JuchuShosaiInfo.Number.ToString("###,###,##0.#####");
            // 単価
            this.numAtPrice.Value = this.HaishaInfo.AtPrice;
            this.lblAtPrice.Text = "/ " + this.JuchuShosaiInfo.AtPrice.ToString("###,###,##0.#####");
            // 売上金額
            this.numPrice.Value = this.HaishaInfo.PriceInPrice;
            this.lblPrice.Text = "/ " + this.JuchuShosaiInfo.PriceInPrice.ToString("###,###,##0");
            // 税区分
            this.cmbTaxDispKbn.SelectedValue = this.HaishaInfo.TaxDispKbn;
            // 通行料
            this.numTollFeeInPrice.Value = this.HaishaInfo.TollFeeInPrice;
            this.ldlTollFeeInPrice.Text = "/ " + this.JuchuShosaiInfo.TollFeeInPrice.ToString("###,###,##0");
            // 付帯業務料
            this.numFutaigyomuryoInPrice.Value = this.HaishaInfo.FutaigyomuryoInPrice;
            this.lblFutaigyomuryoInPrice.Text = "/ " + this.JuchuShosaiInfo.FutaigyomuryoInPrice.ToString("###,###,##0");
            // 荷主
            this.numOwnerCd.Tag = this.HaishaInfo.OwnerId;
            this.numOwnerCd.Value = this.HaishaInfo.OwnerCode;
            this.edtOwnerNM.Text = this.HaishaInfo.OwnerName;
            // 単位
            this.numFigCd.Tag = this.HaishaInfo.FigId;
            this.numFigCd.Value = this.HaishaInfo.FigCode;
            this.edtFigNm.Text = this.HaishaInfo.FigName;
            // 重量
            this.numWeight.Value = this.HaishaInfo.Weight;
            this.lblWeight.Text = "/ " + this.JuchuShosaiInfo.Weight.ToString("###,###,##0") + "kg";
            // 傭車金額
            this.numCharterPrice.Value = this.HaishaInfo.PriceInCharterPrice;
            this.lblCharterPrice.Text = "/ " + this.JuchuShosaiInfo.PriceInCharterPrice.ToString("###,###,##0");
            // 傭車税区分
            string strCharterTaxDispKbn = String.Empty;
            var CharterTaxDispKbnList = this.systemNameList
                .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.TaxDispKbnShort)
                    && x.SystemNameCode == this.HaishaInfo.CharterTaxDispKbn);
            if (CharterTaxDispKbnList.Count() > 0)
            {
                strCharterTaxDispKbn = CharterTaxDispKbnList.First().SystemName;
            }
            this.cmbCharterTaxDispKbn.SelectedValue = this.HaishaInfo.CharterTaxDispKbn;
            // 傭車通行料
            this.numTollFeeInCharterPrice.Value = this.HaishaInfo.TollFeeInCharterPrice;
            this.lblTollFeeInCharterPrice.Text = "/ " + this.JuchuShosaiInfo.TollFeeInCharterPrice.ToString("###,###,##0");
            // 乗務員売上
            this.chkJomuinUriageDogakuFlag.Checked = this.HaishaInfo.JomuinUriageDogakuFlag;
            this.numJomuinUriageKingaku.Value = this.HaishaInfo.JomuinUriageKingaku;
            this.lblJomuinUriageKingaku.Text = "/ " + this.JuchuShosaiInfo.JomuinUriageKingaku.ToString("###,###,##0");
            // 計上日
            this.dteAddUpYMD.Value = this.HaishaInfo.AddUpYMD;
            this.chkFixFlag.Checked = this.HaishaInfo.FixFlag;
            // 傭車計上日
            this.dteCharterAddUpYMD.Value = this.HaishaInfo.CharterAddUpYMD == DateTime.MinValue ? (DateTime?)null : this.HaishaInfo.CharterAddUpYMD;
            this.chkCharterFixFlag.Checked = this.HaishaInfo.CharterFixFlag;

            // メンバをクリア
            if (this.HaishaInfo.UppdateFlg)
            {
                // 変更モード
                this.isConfirmClose = true;
            }
            else
            {
                // 参照モード
                this.isConfirmClose = false;
            }

            this.ResultProcessing = HaishaDialogResult.Cancel;
        }

        /// <summary>
        /// 画面レイアウトを調整します。
        /// </summary>
        private void SetLeyout()
        {
            // トラDON_V40の場合
            if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                == (int)DefaultProperty.TraDonVersionKbn.V40)
            {
                //附帯業務料非表示
                this.lblFutaigyomuryoInPriceTitle.Visible = false;
                this.lblFutaigyomuryoInPrice.Visible = false;
                this.numFutaigyomuryoInPrice.Visible = false;
                //乗務員売上場所移動
                this.lblJomuinUriageKingakuTitle.Location =
                    new System.Drawing.Point(this.lblFutaigyomuryoInPriceTitle.Location.X, this.lblFutaigyomuryoInPriceTitle.Location.Y);
                this.numJomuinUriageKingaku.Location =
                    new System.Drawing.Point(this.numFutaigyomuryoInPrice.Location.X, this.numFutaigyomuryoInPrice.Location.Y);
                this.chkJomuinUriageDogakuFlag.Location =
                    new System.Drawing.Point(this.numJomuinUriageKingaku.Location.X
                        + this.numJomuinUriageKingaku.Size.Width
                        + 8,
                        this.chkJomuinUriageDogakuFlag.Location.Y);
                this.lblJomuinUriageKingaku.Location =
                    new System.Drawing.Point(this.chkJomuinUriageDogakuFlag.Location.X
                        + this.chkJomuinUriageDogakuFlag.Size.Width
                        + 8,
                        this.lblFutaigyomuryoInPrice.Location.Y);
            }
        }

        /// <summary>
        /// 数量、単価、小数部を制御します。
        /// </summary>
        private void SetControlDigits()
        {
            //最大値と最小値を設定します
            decimal maxValue_integer = 0;
            decimal maxValue_decimal = 0;
            decimal maxValue = 0;

            #region 数量

            //数量の桁数を取得する(整数部と小数部)
            int intDegit_Number = Property.UserProperty.GetInstance().JuchuNumberIntDigits;
            int decimaDegit_Number = Property.UserProperty.GetInstance().JuchuNumberDecimalDigits;

            //数量のセルを変数で取り出す
            var numnumber = this.numNumber;

            //Fieldsプロパティから整数部、小数部を設定します。
            numnumber.Fields.IntegerPart.MaxDigits = intDegit_Number;
            numnumber.Fields.DecimalPart.MaxDigits = decimaDegit_Number;
            numnumber.Fields.DecimalPart.MinDigits = decimaDegit_Number;

            //小数部の桁数
            numnumber.Fields.DecimalPart.MaxDigits = decimaDegit_Number;
            numnumber.Fields.DecimalPart.MinDigits = decimaDegit_Number;

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

            numnumber.MaxValue = maxValue;
            numnumber.MinValue = maxValue * -1;

            #endregion

            #region 単価

            //数量の桁数を取得する(整数部と小数部)
            int intDegit_AtPrice = Property.UserProperty.GetInstance().JuchuAtPriceIntDigits;
            int decimaDegit_AtPrice = Property.UserProperty.GetInstance().JuchuAtPriceDecimalDigits;

            //単価のセルを変数で取り出す
            var numatprice = this.numAtPrice;

            //Fieldsプロパティから整数部、小数部を設定します。
            numatprice.Fields.IntegerPart.MaxDigits = intDegit_AtPrice;
            numatprice.Fields.DecimalPart.MaxDigits = decimaDegit_AtPrice;
            numatprice.Fields.DecimalPart.MinDigits = decimaDegit_AtPrice;

            //小数部の桁数
            numatprice.Fields.DecimalPart.MaxDigits = decimaDegit_AtPrice;
            numatprice.Fields.DecimalPart.MinDigits = decimaDegit_AtPrice;

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

            numatprice.MaxValue = maxValue;
            numatprice.MinValue = maxValue * -1;

            #endregion
        }

        /// <summary>
        /// コンボボックスを初期化します。
        /// </summary>
        private void InitCombo()
        {
            this.InitTimeScheduleTypeCombo();
        }

        /// <summary>
        /// ValidatingEventRaiserの初期化
        /// </summary>
        private void SetupValidatingEventRaiser()
        {
            // 照会モード時は、ValidatingEventRaiser不要
            if (!this.HaishaInfo.UppdateFlg) return;

            validatingEventRaiserCollection = new ControlValidatingEventRaiserCollection();

            // コード検索
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHaishaStartPointCd, ctl => ctl.Text, this.numHaishaStartPointcd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numHaishaEndPointCd, ctl => ctl.Text, this.numHaishaEndPointcd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarCd, ctl => ctl.Text, this.numCarCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarKindCd, ctl => ctl.Text, this.numCarKindCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numStaffCd, ctl => ctl.Text, this.numStaffCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCarOfChartererCd, ctl => ctl.Text, this.numCarOfChartererCode_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numItemCd, ctl => ctl.Text, this.numItemCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numOwnerCd, ctl => ctl.Text, this.numOwnerCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numFigCd, ctl => ctl.Text, this.numFigCd_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numNumber, ctl => ctl.Text, this.numNumber_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numAtPrice, ctl => ctl.Text, this.numAtPrice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numPrice, ctl => ctl.Text, this.numPrice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTollFeeInPrice, ctl => ctl.Text, this.numPrice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numFutaigyomuryoInPrice, ctl => ctl.Text, this.numPrice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numCharterPrice, ctl => ctl.Text, this.numCharterPrice_Validating));
            validatingEventRaiserCollection.Add(
                ControlValidatingEventRaiser.Create(this.numTollFeeInCharterPrice, ctl => ctl.Text, this.numCharterPrice_Validating));
        }

        #endregion

        #region コマンドの設定

        /// <summary>
        /// コマンドの設定をします。
        /// </summary>
        private void SettingCommands()
        {
            _commandSet = new CommandSet();

            //***分割
            _commandSet.Copy.Execute += Copy_Execute;
            _commandSet.Bind(
                _commandSet.Copy, this.btnCopy, actionMenuItems.GetMenuItemBy(ActionMenuItems.Copy), this.toolStripCopy);

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
                _commandSet.Close, this.btnClose, actionMenuItems.GetMenuItemBy(ActionMenuItems.Close), this.toolStripEnd);
        }

        void Copy_Execute(object sender, EventArgs e)
        {
            this.DoCopy(true);
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
            this.DoClose(DialogResult.Cancel);
        }

        #endregion

        /// <summary>
        /// 検索状態バインドの設定をします。
        /// </summary>
        private void SetupSearchStateBinder()
        {

            this.searchStateBinder = new SearchStateBinder(this);

            // 照会モード時は、ValidatingEventRaiser不要
            if (!this.HaishaInfo.UppdateFlg) return;

            this.searchStateBinder.AddSearchableControls(
                this.numHaishaStartPointCd,
                this.numHaishaEndPointCd,
                this.numFigCd
                );

            this.searchStateBinder.AddStateObject(this.toolStripReference);
        }

        #region IFrameBase メンバ

        /// <summary>
        /// 本画面のインスタンスを初期化します。
        /// </summary>
        public void InitFrame()
        {
            this.InitHaishaShosaiNyuryokuFrame();
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
                case FrameEditMode.ViewOnly:
                    //参照モード

                    //画面タイトルの設定
                    this.Text = WINDOW_TITLE_INFO;

                    this.toolStripReference.Enabled = false;

                    //--コントロールの使用可否
                    this.ChangeReadOnly(true);
                    this.ChangeEnabled(false);
                    //--ファンクションの使用可否
                    _commandSet.Copy.Enabled = false;
                    _commandSet.Delete.Enabled = false;
                    _commandSet.Save.Enabled = false;
                    _commandSet.Close.Enabled = true;
                    break;

                case FrameEditMode.Editable:
                    //編集モード

                    this.toolStripReference.Enabled = true;

                    //--コントロールの使用可否
                    this.ChangeReadOnly(false);
                    this.ChangeEnabled(true);
                    this.RefreshStyleForCarKbn(this.HaishaInfo.CarKbn);
                    //乗務員売上同額フラグがチェックされている場合
                    if (this.HaishaInfo.JomuinUriageDogakuFlag)
                    {
                        //編集不可
                        this.numJomuinUriageKingaku.ReadOnly = true;
                    }
                    else
                    {
                        //編集可
                        this.numJomuinUriageKingaku.ReadOnly = false;
                    }
                    //--ファンクションの使用可否
                    _commandSet.Copy.Enabled = true;
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
        /// 各項目のReadOnlyプロパティを切り替えます。
        /// </summary>
        /// <param name="mode">ReadOnlyの値</param>
        private void ChangeReadOnly(bool mode)
        {
            /***************
             * 配車情報
             ***************/
            // 積地名
            this.numHaishaStartPointCd.ReadOnly = mode;
            this.edtHaishaStartPointNM.ReadOnly = mode;
            // 着地名
            this.numHaishaEndPointCd.ReadOnly = mode;
            this.edtHaishaEndPointNM.ReadOnly = mode;
            // 車両
            this.numCarCd.ReadOnly = mode;
            // 車種
            this.numCarKindCd.ReadOnly = mode;
            // 乗務員
            this.numStaffCd.ReadOnly = mode;
            // 傭車先
            this.numCarOfChartererCd.ReadOnly = mode;
            // 孫傭車先
            this.edtMagoYoshasaki.ReadOnly = mode;
            // 車番
            this.edtLicPlateCarNo.ReadOnly = mode;
            // 発日時
            this.dteStartYMD.ReadOnly = mode;
            this.dteStartHM.ReadOnly = mode;
            // 積日時
            this.dteTaskStartYMD.ReadOnly = mode;
            this.dteTaskStartHM.ReadOnly = mode;
            // 着日時
            this.dteTaskEndYMD.ReadOnly = mode;
            this.dteTaskEndHM.ReadOnly = mode;
            // 再使用可能日
            this.dteReuseYMD.ReadOnly = mode;
            this.dteReuseHM.ReadOnly = mode;
            // コメント
            this.edtBiko.ReadOnly = mode;

            /***************
             * 請求情報
             ***************/
            // 品目
            this.numItemCd.ReadOnly = mode;
            this.edtItemNM.ReadOnly = mode;
            // 数量
            this.numNumber.ReadOnly = mode;
            // 単価
            this.numAtPrice.ReadOnly = mode;
            // 売上金額
            this.numPrice.ReadOnly = mode;
            // 通行料
            this.numTollFeeInPrice.ReadOnly = mode;
            // 付帯業務料
            this.numFutaigyomuryoInPrice.ReadOnly = mode;
            // 荷主
            this.numOwnerCd.ReadOnly = mode;
            this.edtOwnerNM.ReadOnly = mode;
            // 単位
            this.numFigCd.ReadOnly = mode;
            // 重量
            this.numWeight.ReadOnly = mode;
            // 傭車金額・傭車通行料
            if (!mode)
            {
                if (this.HaishaInfo.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                {
                    // 傭車の場合
                    this.numCharterPrice.ReadOnly = false;
                    this.numTollFeeInCharterPrice.ReadOnly = false;
                }
                else
                {
                    // 自社車両の場合
                    this.numCharterPrice.ReadOnly = true;
                    this.numTollFeeInCharterPrice.ReadOnly = true;
                }
            }
            else
            {
                this.numCharterPrice.ReadOnly = true;
                this.numTollFeeInCharterPrice.ReadOnly = true;
            }
            
            // 乗務員売上
            this.numJomuinUriageKingaku.ReadOnly = mode;
            // 計上日付
            this.dteAddUpYMD.ReadOnly = mode;
            // 傭車計上日付
            this.dteCharterAddUpYMD.ReadOnly = mode;
        }

        /// <summary>
        /// 各項目のEnabledプロパティを切り替えます。
        /// </summary>
        /// <param name="mode">Enabledの値</param>
        private void ChangeEnabled(bool mode)
        {
            this.sideButton1.Enabled = mode;
            this.sideButton2.Enabled = mode;
            this.sideButton3.Enabled = mode;
            
            this.sideButton5.Enabled = mode;
            
            this.sideButton8.Enabled = mode;
            this.sideButton9.Enabled = mode;

            this.spinButton1.Enabled = mode;
            this.spinButton2.Enabled = mode;
            this.spinButton3.Enabled = mode;
            this.spinButton4.Enabled = mode;
            this.spinButton5.Enabled = mode;
            this.spinButton6.Enabled = mode;
            this.spinButton7.Enabled = mode;
            this.spinButton10.Enabled = mode;

            this.dropDownButton1.Enabled = mode;
            this.dropDownButton2.Enabled = mode;
            this.dropDownButton4.Enabled = mode;

            this.chkJomuinUriageDogakuFlag.Enabled = mode;
            this.chkFixFlag.Enabled = mode;
            this.chkCharterFixFlag.Enabled = mode;

            this.btnCopy.Enabled = mode;
            this.btnDelete.Enabled = mode;
            this.btnSave.Enabled = mode;

            // 乗務員・傭車先
            if (mode)
            {
                if (this.HaishaInfo.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                {
                    // 傭車の場合
                    this.sideButton4.Enabled = false;
                    this.sideButton6.Enabled = true;
                    this.dropDownButton6.Enabled = true;
                    this.cmbCharterTaxDispKbn.Enabled = true;
                    this.spinButton8.Enabled = true;
                }
                else
                {
                    // 自社車両の場合
                    this.sideButton4.Enabled = true;
                    this.sideButton6.Enabled = false;
                    this.dropDownButton6.Enabled = false;
                    this.cmbCharterTaxDispKbn.Enabled = false;
                    this.spinButton8.Enabled = false;
                }

                // 請負コード=0以外の場合は編集可
                if (this.JuchuShosaiInfo.ContractCode != 0)
                {
                    this.dteAddUpYMD.Enabled = false;
                    this.cmbTaxDispKbn.Enabled = false;
                    this.spinButton9.Enabled = false;
                    this.dropDownButton3.Enabled = false;
                }
                else
                {
                    this.dteAddUpYMD.Enabled = true;
                    this.cmbTaxDispKbn.Enabled = true;
                    this.spinButton9.Enabled = true;
                    this.dropDownButton3.Enabled = true;
                }
            }
            else
            {
                this.sideButton4.Enabled = false;
                this.sideButton6.Enabled = false;
                this.dropDownButton6.Enabled = false;
                this.cmbCharterTaxDispKbn.Enabled = false;
                this.spinButton8.Enabled = false;
                this.dteAddUpYMD.Enabled = false;
                this.cmbTaxDispKbn.Enabled = false;
                this.spinButton9.Enabled = false;
                this.dropDownButton3.Enabled = false;
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
                /***************
                 * 配車情報
                 ***************/
                // 積地名
                this.HaishaInfo.StartPointId = (decimal)this.numHaishaStartPointCd.Tag;
                this.HaishaInfo.StartPointCode = (int)this.numHaishaStartPointCd.Value;
                this.HaishaInfo.StartPointName = this.edtHaishaStartPointNM.Text;
                // 着地名
                this.HaishaInfo.EndPointId = (decimal)this.numHaishaEndPointCd.Tag;
                this.HaishaInfo.EndPointCode = (int)this.numHaishaEndPointCd.Value;
                this.HaishaInfo.EndPointName = this.edtHaishaEndPointNM.Text;
                // 車両
                CarInfo carInfo = this.numCarCd.Tag == null ? new CarInfo() : (CarInfo)this.numCarCd.Tag;
                this.HaishaInfo.CarId = carInfo.ToraDONCarId;
                this.HaishaInfo.CarCode = (int)this.numCarCd.Value;

                // 車両区分
                this.HaishaInfo.CarKbn = carInfo.CarKbn;

                // 車種
                this.HaishaInfo.CarKindId = (decimal)this.numCarKindCd.Tag;
                this.HaishaInfo.CarKindCode = (int)this.numCarKindCd.Value;
                this.HaishaInfo.CarKindName = this.edtCarKindNm.Text;
                // 乗務員・傭車先
                if (carInfo.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                {
                    // 傭車の場合
                    this.HaishaInfo.TorihikiCd = (int)this.numCarOfChartererCd.Value;
                    this.HaishaInfo.CarOfChartererId = (decimal)this.numCarOfChartererCd.Tag;
                    this.HaishaInfo.TorihikiName = this.edtCarOfChartererNm.Text;
                    this.HaishaInfo.LicPlateCarNo = this.edtLicPlateCarNo.Text;
                    this.HaishaInfo.CarLicPlateCarNo = string.Empty;
                    this.HaishaInfo.MagoYoshasaki = this.edtMagoYoshasaki.Text;
                }
                else
                {
                    // 自社車両の場合
                    this.HaishaInfo.StaffCd = (int)this.numStaffCd.Value;
                    this.HaishaInfo.DriverId = (decimal)this.numStaffCd.Tag;
                    this.HaishaInfo.StaffName = this.edtStaffNm.Text;
                    this.HaishaInfo.LicPlateCarNo = string.Empty;
                    this.HaishaInfo.CarLicPlateCarNo = carInfo.LicPlateCarNo;
                    this.HaishaInfo.MagoYoshasaki = string.Empty;
                }
                // 発日時
                this.HaishaInfo.StartYMD = new DateTime(
                    this.dteStartYMD.Value.Value.Year,
                    this.dteStartYMD.Value.Value.Month,
                    this.dteStartYMD.Value.Value.Day,
                    this.dteStartHM.Value.Value.Hour,
                    this.dteStartHM.Value.Value.Minute,
                    0);
                // 積日時
                this.HaishaInfo.TaskStartDateTime = new DateTime(
                    this.dteTaskStartYMD.Value.Value.Year,
                    this.dteTaskStartYMD.Value.Value.Month,
                    this.dteTaskStartYMD.Value.Value.Day,
                    this.dteTaskStartHM.Value.Value.Hour,
                    this.dteTaskStartHM.Value.Value.Minute,
                    0);
                // 着日時
                this.HaishaInfo.TaskEndDateTime = new DateTime(
                    this.dteTaskEndYMD.Value.Value.Year,
                    this.dteTaskEndYMD.Value.Value.Month,
                    this.dteTaskEndYMD.Value.Value.Day,
                    this.dteTaskEndHM.Value.Value.Hour,
                    this.dteTaskEndHM.Value.Value.Minute,
                    0);
                // 再使用可能日
                this.HaishaInfo.ReuseYMD = new DateTime(
                    this.dteReuseYMD.Value.Value.Year,
                    this.dteReuseYMD.Value.Value.Month,
                    this.dteReuseYMD.Value.Value.Day,
                    this.dteReuseHM.Value.Value.Hour,
                    this.dteReuseHM.Value.Value.Minute,
                    0);
                // コメント
                this.HaishaInfo.Biko = this.edtBiko.Text;

                /***************
                 * 請求情報
                 ***************/

                // 請求部門
                decimal clmClassId = this.JuchuShosaiInfo.ClmClassId;

                // 請負
                int contract_cd = this.JuchuShosaiInfo.ContractCode;

                ToraDONContractSearchParameter cntract_para = new ToraDONContractSearchParameter();
                cntract_para.TokuisakiId = this.HaishaInfo.TokuisakiId;
                cntract_para.ClmClassId = clmClassId;
                cntract_para.ContractCode = contract_cd;
                ToraDONContractInfo contract_info = this._DalUtil.ToraDONContract.GetList(cntract_para).FirstOrDefault();

                // 請求関連の項目で変更がある場合は再計算
                if (this.ChkBillingInfo(contract_info))
                {

                    // 金額を取得
                    decimal price = Convert.ToDecimal(this.numPrice.Value);
                    // 通行料を取得
                    decimal tollFeeInPrice = Convert.ToDecimal(this.numTollFeeInPrice.Value);
                    // 附帯業務料を取得
                    decimal futaigyomuryoInPrice = Convert.ToDecimal(this.numFutaigyomuryoInPrice.Value);

                    // 税区分
                    int taxdispkbn = 0;
                    // 計上日
                    DateTime addupdate = DateTime.MinValue;
                    // 確定フラグ
                    bool fixflag = false;

                    // 請負ではない通常の受注か？
                    if (contract_info == null)
                    {
                        //画面値をセットする
                        taxdispkbn = Convert.ToInt32(this.cmbTaxDispKbn.SelectedValue);

                        // 計上日を取得
                        addupdate = (Convert.ToDateTime(this.dteAddUpYMD.Value)).Date;
                        // 確定フラグを取得
                        fixflag = this.chkFixFlag.Checked;
                    }
                    else
                    {
                        // 請負データの場合は、請負の税区分・計上日をセット
                        taxdispkbn = contract_info.TaxDispKbn;
                        addupdate = contract_info.AddUpDate;

                        // 請負データの場合は、確定フラグに固定で"1"をセット
                        fixflag = true;
                    }

                    // 税率取得
                    decimal juchu_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(addupdate);

                    // 税区分（列挙値）取得
                    BizProperty.DefaultProperty.ZeiKbn wk_juchu_taxkbn =
                        (BizProperty.DefaultProperty.ZeiKbn)taxdispkbn;

                    // 端数区分（列挙値）取得
                    BizProperty.DefaultProperty.HasuMArumeKbn wk_juchu_cutkbn =
                        (BizProperty.DefaultProperty.HasuMArumeKbn)this.TokuiTaxCutKbn;

                    // 税計算
                    CalcTaxResultStructInfo juchu_taxstruct_info = this.ComputeTaxStruct(price + futaigyomuryoInPrice, juchu_taxrate, wk_juchu_taxkbn, wk_juchu_cutkbn);

                    decimal outtaxcalc = 0;
                    decimal outtax = 0;
                    decimal intaxcalc = 0;
                    decimal intax = 0;
                    decimal notaxcalc = 0;

                    switch (wk_juchu_taxkbn)
                    {
                        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                            outtaxcalc = juchu_taxstruct_info.BaseAmount;
                            outtax = juchu_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                            intaxcalc = juchu_taxstruct_info.WithoutTaxAmount;
                            intax = juchu_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                            notaxcalc = juchu_taxstruct_info.BaseAmount;
                            break;
                        default:
                            break;
                    }

                    // 品目
                    this.HaishaInfo.ItemId = (decimal)this.numItemCd.Tag;
                    this.HaishaInfo.ItemCode = (int)this.numItemCd.Value;
                    this.HaishaInfo.ItemName = this.edtItemNM.Text;
                    // 数量
                    this.HaishaInfo.Number = (decimal)this.numNumber.Value;
                    // 単価
                    this.HaishaInfo.AtPrice = (decimal)this.numAtPrice.Value;
                    // 売上金額
                    this.HaishaInfo.PriceInPrice = price;
                    // 通行料
                    this.HaishaInfo.TollFeeInPrice = tollFeeInPrice;
                    // 外税対象額
                    this.HaishaInfo.PriceOutTaxCalc = outtaxcalc;
                    // 外税額
                    this.HaishaInfo.PriceOutTax = outtax;
                    // 内税対象額
                    this.HaishaInfo.PriceInTaxCalc = intaxcalc;
                    // 内税額
                    this.HaishaInfo.PriceInTax = intax;
                    // 非課税対象額
                    this.HaishaInfo.PriceNoTaxCalc = notaxcalc + tollFeeInPrice;
                    // 税区分
                    this.HaishaInfo.TaxDispKbn = taxdispkbn;
                    // 計上日付
                    this.HaishaInfo.AddUpYMD = addupdate;
                    // 確定フラグ
                    this.HaishaInfo.FixFlag = fixflag;

                    // 付帯業務料
                    this.HaishaInfo.FutaigyomuryoInPrice = futaigyomuryoInPrice;
                    // 荷主
                    this.HaishaInfo.OwnerId = (decimal)this.numOwnerCd.Tag;
                    this.HaishaInfo.OwnerCode = (int)this.numOwnerCd.Value;
                    this.HaishaInfo.OwnerName = this.edtOwnerNM.Text;
                    // 単位
                    this.HaishaInfo.FigId = (decimal)this.numFigCd.Tag;
                    this.HaishaInfo.FigCode = (int)this.numFigCd.Value;
                    this.HaishaInfo.FigName = this.edtFigNm.Text;
                    // 重量
                    this.HaishaInfo.Weight = (decimal)this.numWeight.Value;

                    // 乗務員売上
                    this.HaishaInfo.JomuinUriageDogakuFlag = this.chkJomuinUriageDogakuFlag.Checked;
                    this.HaishaInfo.JomuinUriageKingaku = (decimal)this.numJomuinUriageKingaku.Value;

                    // 金額
                    this.HaishaInfo.Price = this.HaishaInfo.PriceInPrice + this.HaishaInfo.TollFeeInPrice + this.HaishaInfo.FutaigyomuryoInPrice;

                    //傭車金額関連

                    // 傭車金額を取得
                    decimal charter_price = Convert.ToDecimal(this.numCharterPrice.Value);

                    // 傭車金額通行料
                    decimal tollFeeInCharterPrice = Convert.ToDecimal(this.numTollFeeInCharterPrice.Value);

                    // 傭車税区分を取得
                    int charter_taxdispkbn = taxdispkbn = Convert.ToInt32(this.cmbCharterTaxDispKbn.SelectedValue);

                    // 傭車計上日を取得
                    DateTime charter_addupdate = (Convert.ToDateTime(this.dteCharterAddUpYMD.Value)).Date;

                    // 傭車確定フラグを取得
                    bool charter_fixflag = (bool)this.chkCharterFixFlag.Checked;

                    // 税率取得
                    decimal charter_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(charter_addupdate);

                    // 税区分（列挙値）取得
                    BizProperty.DefaultProperty.ZeiKbn wk_charter_taxkbn =
                        (BizProperty.DefaultProperty.ZeiKbn)charter_taxdispkbn;

                    // 端数区分（列挙値）取得
                    BizProperty.DefaultProperty.HasuMArumeKbn wk_charter_cutkbn =
                        (BizProperty.DefaultProperty.HasuMArumeKbn)this.CharterTaxCutKbn;

                    //税計算
                    CalcTaxResultStructInfo charter_taxstruct_info = this.ComputeTaxStruct(charter_price, charter_taxrate, wk_charter_taxkbn, wk_charter_cutkbn);

                    decimal charter_outtaxcalc = 0;
                    decimal charter_outtax = 0;
                    decimal charter_intaxcalc = 0;
                    decimal charter_intax = 0;
                    decimal charter_notaxcalc = 0;

                    switch (wk_charter_taxkbn)
                    {
                        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                            charter_outtaxcalc = charter_taxstruct_info.BaseAmount;
                            charter_outtax = charter_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                            charter_intaxcalc = charter_taxstruct_info.WithoutTaxAmount;
                            charter_intax = charter_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                            charter_notaxcalc = charter_taxstruct_info.BaseAmount;
                            break;
                        default:
                            break;
                    }

                    // 傭車金額_金額
                    this.HaishaInfo.PriceInCharterPrice = (decimal)this.numCharterPrice.Value;
                    // 傭車通行料
                    this.HaishaInfo.TollFeeInCharterPrice = (decimal)this.numTollFeeInCharterPrice.Value;
                    // 傭車外税対象額
                    this.HaishaInfo.CharterPriceOutTaxCalc = charter_outtaxcalc;
                    // 傭車外税額
                    this.HaishaInfo.CharterPriceOutTax = charter_outtax;
                    // 傭車内税対象額
                    this.HaishaInfo.CharterPriceInTaxCalc = charter_intaxcalc;
                    // 傭車内税額
                    this.HaishaInfo.CharterPriceInTax = charter_intax;
                    // 傭車非課税対象額
                    this.HaishaInfo.CharterPriceNoTaxCalc = charter_notaxcalc + tollFeeInCharterPrice;
                    // 傭車税区分
                    this.HaishaInfo.CharterTaxDispKbn = charter_taxdispkbn;
                    // 傭車計上日付
                    this.HaishaInfo.CharterAddUpYMD = charter_addupdate;
                    // 傭車確定フラグ
                    this.HaishaInfo.CharterFixFlag = charter_fixflag;

                    // 傭車金額
                    this.HaishaInfo.CharterPrice = this.HaishaInfo.PriceInCharterPrice + this.HaishaInfo.TollFeeInCharterPrice;

                    //運賃_金額
                    decimal priceInFee = price;
                    //運賃_通行料 
                    decimal tollFeeInFee = tollFeeInPrice;
                    //運賃_附帯業務料 
                    decimal futaigyomuryoInFee = futaigyomuryoInPrice;
                    // 税率取得
                    decimal fee_taxrate = 0;
                    // 税区分（列挙値）取得
                    BizProperty.DefaultProperty.ZeiKbn wk_fee_taxkbn;
                    // 端数区分（列挙値）取得
                    BizProperty.DefaultProperty.HasuMArumeKbn wk_fee_cutkbn;

                    fee_taxrate = this._ToraDonSystemPropertyInfo.GetTaxRateByDay(addupdate);
                    wk_fee_taxkbn = (BizProperty.DefaultProperty.ZeiKbn)taxdispkbn;
                    wk_fee_cutkbn = (BizProperty.DefaultProperty.HasuMArumeKbn)this.CharterTaxCutKbn;

                    //税計算
                    CalcTaxResultStructInfo fee_taxstruct_info = this.ComputeTaxStruct(priceInFee + futaigyomuryoInFee, fee_taxrate, wk_fee_taxkbn, wk_fee_cutkbn);

                    decimal fee_outtaxcalc = 0;
                    decimal fee_outtax = 0;
                    decimal fee_intaxcalc = 0;
                    decimal fee_intax = 0;
                    decimal fee_notaxcalc = 0;

                    switch (wk_fee_taxkbn)
                    {
                        case BizProperty.DefaultProperty.ZeiKbn.Sotozei:
                            fee_outtaxcalc = fee_taxstruct_info.BaseAmount;
                            fee_outtax = fee_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Uchizei:
                            fee_intaxcalc = fee_taxstruct_info.WithoutTaxAmount;
                            fee_intax = fee_taxstruct_info.TaxAmount;
                            break;
                        case BizProperty.DefaultProperty.ZeiKbn.Hikazei:
                            fee_notaxcalc = fee_taxstruct_info.BaseAmount;
                            break;
                        default:
                            break;
                    }

                    this.HaishaInfo.Fee = priceInFee + tollFeeInFee + futaigyomuryoInFee;
                    this.HaishaInfo.PriceInFee = priceInFee;
                    this.HaishaInfo.TollFeeInFee = tollFeeInFee;
                    this.HaishaInfo.FutaigyomuryoInFee = futaigyomuryoInFee;
                    // 運賃外税対象額
                    this.HaishaInfo.FeeOutTaxCalc = fee_outtaxcalc;
                    // 運賃外税額
                    this.HaishaInfo.FeeOutTax = fee_outtax;
                    // 運賃内税対象額
                    this.HaishaInfo.FeeInTaxCalc = fee_intaxcalc;
                    // 運賃内税額
                    this.HaishaInfo.FeeInTax = fee_intax;
                    // 運賃非課税対象額
                    this.HaishaInfo.FeeNoTaxCalc = fee_notaxcalc + tollFeeInFee;
                }
                else 
                {
                    // 確定フラグ
                    bool fixflag = false;

                    // 請負ではない通常の受注か？
                    if (contract_info == null)
                    {
                        // 確定フラグを取得
                        fixflag = this.chkFixFlag.Checked;
                    }
                    else
                    {
                        // 請負データの場合は、確定フラグに固定で"1"をセット
                        fixflag = true;
                    }
                    // 確定フラグ
                    this.HaishaInfo.FixFlag = fixflag;

                    // 数量
                    this.HaishaInfo.Number = (decimal)this.numNumber.Value;
                    // 単価
                    this.HaishaInfo.AtPrice = (decimal)this.numAtPrice.Value;
                    // 荷主
                    this.HaishaInfo.OwnerId = (decimal)this.numOwnerCd.Tag;
                    this.HaishaInfo.OwnerCode = (int)this.numOwnerCd.Value;
                    this.HaishaInfo.OwnerName = this.edtOwnerNM.Text;
                    // 単位
                    this.HaishaInfo.FigId = (decimal)this.numFigCd.Tag;
                    this.HaishaInfo.FigCode = (int)this.numFigCd.Value;
                    this.HaishaInfo.FigName = this.edtFigNm.Text;
                    // 重量
                    this.HaishaInfo.Weight = (decimal)this.numWeight.Value;
                    // 乗務員売上
                    this.HaishaInfo.JomuinUriageDogakuFlag = this.chkJomuinUriageDogakuFlag.Checked;
                    this.HaishaInfo.JomuinUriageKingaku = (decimal)this.numJomuinUriageKingaku.Value;

                    // 傭車確定フラグ
                    this.HaishaInfo.CharterFixFlag = (bool)this.chkCharterFixFlag.Checked;
                }

                // 処理種別を設定
                this.ResultProcessing = HaishaDialogResult.Update;

                //画面を閉じます
                this.DoClose(DialogResult.OK);
            }
        }

        /// <summary>
        /// 入力後の入力チェックをします。
        /// </summary>
        /// <returns></returns>
        private bool ValidatingCheckInputs()
        {
            //返却用
            bool rt_val = true;

            //メッセージ表示用
            string msg = string.Empty;

            //フォーカス移動先
            Control ctl = null;

            // 日付の範囲チェック
            if (rt_val && this.dteTaskStartYMD.Value != null && this.dteStartYMD.Value != null)
            {
                if (this.dteTaskStartYMD.Value > this.dteStartYMD.Value)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202032", new string[] { "積載日付には発日以前の日付" });
                    ctl = this.dteStartYMD;
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
        /// 入力項目をチェックします。
        /// </summary>
        /// <returns></returns>
        private bool CheckInputs()
        {
            // 返却用
            bool rt_val = true;

            // メッセージ表示用
            string msg = string.Empty;

            // フォーカス移動先
            Control ctl = null;

            // 積日の必須入力チェック
            if (rt_val && this.dteTaskStartYMD.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "積日" });
                ctl = this.dteTaskStartYMD;
            }

            // 積時間の必須入力チェック
            if (rt_val && this.dteTaskStartHM.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "積時間" });
                ctl = this.dteTaskStartHM;
            }

            // 着日の必須入力チェック
            if (rt_val && this.dteTaskEndYMD.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "着日" });
                ctl = this.dteTaskEndYMD;
            }

            // 着時間の必須入力チェック
            if (rt_val && this.dteTaskEndHM.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "着時間" });
                ctl = this.dteTaskEndHM;
            }

            // 再使用可能日の必須入力チェック
            if (rt_val && this.dteReuseYMD.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "再使日" });
                ctl = this.dteReuseYMD;
            }

            // 再使用可能時間の必須入力チェック
            if (rt_val && this.dteReuseHM.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "再使時間" });
                ctl = this.dteReuseHM;
            }

            // 車両情報
            CarInfo carInfo = this.numCarCd.Tag == null ? new CarInfo() : (CarInfo)this.numCarCd.Tag;

            // 車両の必須入力チェック
            if (rt_val && this.numCarCd.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "車両" });
                ctl = this.numCarCd;
            }

            // 乗務員の必須入力チェック
            if (rt_val && this.numStaffCd.Value == null
                && !this.CarKbnIsCharter(carInfo.CarKbn))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "乗務員" });
                ctl = this.numStaffCd;
            }

            // 傭車先の必須入力チェック
            if (rt_val && this.numCarOfChartererCd.Value == null
                && this.CarKbnIsCharter(carInfo.CarKbn))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "傭車先" });
                ctl = this.numCarOfChartererCd;
            }

            // 発日付の必須入力チェック
            if (rt_val && this.dteStartYMD.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "発日" });
                ctl = this.dteStartYMD;
            }

            // 発時間の必須入力チェック
            if (rt_val && this.dteStartHM.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "発時間" });
                ctl = this.dteStartHM;
            }

            // 発着日日時範囲チェック
            if (rt_val)
            {
                decimal startYmd = this.CnvDayDecimal(this.dteTaskStartYMD.Value, true).Value;
                decimal startHm = this.CnvHmDecimal(this.dteTaskStartHM.Value, true).Value;
                string ymdhmS = startYmd.ToString("00000000") + startHm.ToString("0000");

                decimal EndYmd = this.CnvDayDecimal(this.dteReuseYMD.Value, true).Value;
                decimal EndHm = this.CnvHmDecimal(this.dteReuseHM.Value, true).Value;
                string ymdhmE = EndYmd.ToString("00000000") + EndHm.ToString("0000");

                if (ymdhmE.CompareTo(ymdhmS) < 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202018", new string[] { "再使日" });
                    ctl = this.dteTaskStartYMD;
                }
            }

            // 積日時の範囲チェック
            if (rt_val && this.dteTaskStartYMD.Value != null && this.dteStartYMD.Value != null)
            {
                decimal startYmd = this.CnvDayDecimal(this.dteStartYMD.Value, true).Value;
                decimal startHm = this.CnvHmDecimal(this.dteStartHM.Value, true).Value;
                string ymdhmS = startYmd.ToString("00000000") + startHm.ToString("0000");

                decimal sekisaiYmd = this.CnvDayDecimal(this.dteTaskStartYMD.Value, true).Value;
                decimal sekisaiHm = this.CnvHmDecimal(this.dteTaskStartHM.Value, true).Value;
                string ymdhmTsumi = sekisaiYmd.ToString("00000000") + sekisaiHm.ToString("0000");

                if (ymdhmTsumi.CompareTo(ymdhmS) > 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202032", new string[] { "積日には発日以前の日付" });
                    ctl = this.dteStartYMD;
                }
            }

            // 着日時の範囲チェック
            if (rt_val && this.dteReuseYMD.Value != null && this.dteTaskEndYMD.Value != null)
            {
                decimal startYmd = this.CnvDayDecimal(this.dteTaskEndYMD.Value, true).Value;
                decimal startHm = this.CnvHmDecimal(this.dteTaskEndHM.Value, true).Value;
                string ymdhmS = startYmd.ToString("00000000") + startHm.ToString("0000");

                decimal reuseYmd = this.CnvDayDecimal(this.dteReuseYMD.Value, true).Value;
                decimal reuseiHm = this.CnvHmDecimal(this.dteReuseHM.Value, true).Value;
                string ymdhmReusei = reuseYmd.ToString("00000000") + reuseiHm.ToString("0000");

                if (ymdhmS.CompareTo(ymdhmReusei) > 0)
                {
                    rt_val = false;
                    msg = FrameUtilites.GetDefineMessage(
                        "MW2202032", new string[] { "着日には再使日以前の日付" });
                    ctl = this.dteTaskEndYMD;
                }
            }

            // 重量の必須入力チェック
            if (rt_val && this.numWeight.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "重量" });
                ctl = this.numWeight;
            }

            // 計上日の必須入力チェック
            if (rt_val && this.dteAddUpYMD.Value == null)
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "計上日" });
                ctl = this.dteTaskStartYMD;
            }

            // 傭車計上日の必須入力チェック
            if (rt_val && this.dteCharterAddUpYMD.Value == null && this.CarKbnIsCharter(carInfo.CarKbn))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage(
                    "MW2203001", new string[] { "傭車計上日" });
                ctl = this.dteCharterAddUpYMD;
            }

            // 車両区分の整合性チェック
            if (rt_val
                && (this.CarKbnIsCharter(this.JuchuShosaiInfo.CarKbn) && !this.CarKbnIsCharter(carInfo.CarKbn)))
            {
                rt_val = false;
                msg = FrameUtilites.GetDefineMessage("MW2202016",
                                new string[] { "この受注情報", "傭車車両の指定があるため、傭車車両以外への配車は" });
                ctl = this.numCarCd;
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

                // 処理種別を設定
                this.ResultProcessing = HaishaDialogResult.Delete;

                //画面を閉じます
                this.DoClose(DialogResult.OK);
            }
        }

        /// <summary>
        /// 画面情報をコピーして画面を閉じます。
        /// </summary>
        /// <param name="showCancelConfirm">確認画面を表示するかどうか(true:表示する)</param>
        private void DoCopy(bool showCancelConfirm)
        {
            if (showCancelConfirm)
            {
                //取消確認の実行確認ダイアログ
                DialogResult d_result =
                    MessageBox.Show(
                    "選択した配車情報を分割します。よろしいですか？" + Environment.NewLine
                    + "(本画面を表示した時点の状態で分割します。)",
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

            // 処理種別を設定
            this.ResultProcessing = HaishaDialogResult.Copy;

            //画面を閉じます
            this.DoClose(DialogResult.OK);
        }

        /// <summary>
        /// 税区分コンボボックスを初期化します。
        /// </summary>
        private void InitTimeScheduleTypeCombo()
        {
            // スケジュール種類コンボ設定
            Dictionary<int, String> datasource = new Dictionary<int, String>();

            // 税区分のコンボ要素を取得
            var list = this.systemNameList
                .Where(x => x.SystemNameKbn == (int)DefaultProperty.SystemNameKbn.TaxDispKbnShort)
                .OrderBy(x => x.SystemNameCode);

            foreach (SystemNameInfo item in list)
            {
                datasource.Add(item.SystemNameCode, item.SystemName);
            }

            // 税区分
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbTaxDispKbn, datasource, false, null, false);
            this.cmbTaxDispKbn.SelectedIndex = 0;

            // 傭車税区分
            FrameUtilites.SetupGcComboBoxForValueText(this.cmbCharterTaxDispKbn, datasource, false, null, false);
            this.cmbCharterTaxDispKbn.SelectedIndex = 0;
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

                    // 照会モード時は、F4キーイベント使用不可
                    if (!this.HaishaInfo.UppdateFlg) return;
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

        #endregion

        /// <summary>
        /// フォーム終了の確認をします。メッセージの選択が「いいえ」の場合は終了をキャンセルします。
        /// </summary>
        /// <param name="e"></param>
        private void ConfirmClose(FormClosingEventArgs e)
        {
            //終了確認メッセージを表示
            if (this.isConfirmClose && this.DialogResult == DialogResult.Cancel)
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
        /// 画面を閉じます。
        /// </summary>
        /// <param name="dialogResult"></param>
        private void DoClose(DialogResult dialogResult)
        {
            this.DialogResult = dialogResult;
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

        private void HaishaShosaiNyuryokuFrame_FormClosing(object sender, FormClosingEventArgs e)
        {
            //フォームクローズ時
            this.ConfirmClose(e);

            //FormClosingイベントキャンセル時にはAutoValidateを有効にする
            if (e.Cancel)
            {
                this.AutoValidate = AutoValidate.EnablePreventFocusChange;
            }
        }

        private void HaishaShosaiNyuryokuFrame_Shown(object sender, EventArgs e)
        {
            //ログ出力
            FrameLogWriter.GetLogger(this.appAuth).LoggingLog(
                FrameLogWriter.LoggingOperateKind.ShownFrame);
        }

        private void HaishaShosaiNyuryokuFrame_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessKeyEvent(e);
        }

        private void sbtn_Click(object sender, EventArgs e)
        {
            //共通検索画面起動
            this.ShowCmnSearch();
        }

        /// <summary>
        /// 日付項目Changeイベント（Down）
        /// </summary>
        private void ymd_Down(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(-1);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 日付項目Changeイベント（Up）
        /// </summary>
        private void ymd_Up(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddDays(1);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 時間項目Changeイベント（Down）
        /// </summary>
        private void hm_Down(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddHours(-1);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// 時間項目Changeイベント（Up）
        /// </summary>
        private void hm_Up(object sender, EventArgs e)
        {
            try
            {
                ((NskDateTime)this.ActiveControl).Value
                    = Convert.ToDateTime(((NskDateTime)this.ActiveControl).Value).AddHours(1);
            }
            catch
            {
                ;
            }
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
        /// 車両検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchCar()
        {
            using (CmnSearchCarFrame f = new CmnSearchCarFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONCarCode);

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
        /// 社員検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchStaff()
        {
            using (CmnSearchDriverFrame f = new CmnSearchDriverFrame())
            {
                f.InitFrame();
                f.ShowDialog();

                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.ToraDONStaffCode);

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
        /// 荷主検索画面を表示して結果を画面に設定します。
        /// </summary>
        private void ShowCmnSearchOwner()
        {
            using (CmnSearchOwnerFrame f = new CmnSearchOwnerFrame())
            {
                f.InitFrame();
                f.ShowDialog(this);
                if (f.DialogResult == DialogResult.OK)
                {
                    //画面から値を取得
                    ((GrapeCity.Win.Editors.GcNumber)this.ActiveControl).Value =
                        Convert.ToInt32(f.SelectedInfo.OwnerCode);

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

        #region Changeイベント

        /// <summary>
        /// 積地コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numHaishaStartPointcd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidatePointCd(e, true);
        }

        /// <summary>
        /// 着地コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numHaishaEndPointcd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidatePointCd(e, false);
        }

        /// <summary>
        /// 単位コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numFigCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateFigCd(e);
        }

        /// <summary>
        /// 積載日コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteSekisaiYMD_Validating(object sender, CancelEventArgs e)
        {
            // 入力値チェック
            this.ValidatingCheckInputs();

        }

        /// <summary>
        /// 社員コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numStaffCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateStaffCd(e);
        }

        /// <summary>
        /// 傭車コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numCarOfChartererCode_Validating(object sender, CancelEventArgs e)
        {
            // 傭車コード
            this.ValidateCarOfChartererCode(e);
        }

        /// <summary>
        /// 車両コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numCarCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCarCd(e);
        }

        /// <summary>
        /// 車種コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numCarKindCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateCarKindCd(e);
        }

        /// <summary>
        /// 品目コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numItemCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateItemCd(e);
        }

        /// <summary>
        /// 荷主コードのChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numOwnerCd_Validating(object sender, CancelEventArgs e)
        {
            this.ValidateOwnerCd(e);
        }

        /// <summary>
        /// 数量のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numNumber_Validating(object sender, CancelEventArgs e)
        {
            // 料金計算
            this.PriceCalculation();
        }

        /// <summary>
        /// 単価のChangeイベント
        /// </summary>
        /// <param name="e"></param>
        private void numAtPrice_Validating(object sender, CancelEventArgs e)
        {
            // 料金計算
            this.PriceCalculation();
        }

        /// <summary>
        /// 売上金額（通行料、付帯業務料）のChangeイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numPrice_Validating(object sender, CancelEventArgs e)
        {
            // 確定フラグ、乗務員売上の設定
            this.SetFixFlagAndJomuinUriage();
        }

        /// <summary>
        /// 傭車金額（傭車通行料）のChangeイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void numCharterPrice_Validating(object sender, CancelEventArgs e)
        {
            // 確定フラグ、乗務員売上の設定
            this.SetCharterFixFlag();
        }

        /// <summary>
        /// 計上日のEnterイベント
        /// </summary>
        /// <param name="e"></param>

        private void dteAddUpYMD_Enter(object sender, EventArgs e)
        {
            // 計上日に初期値を設定
            this.SetInitValueInAddUpDateCell();

            this.dteAddUpYMD.SelectAll();
        }

        /// <summary>
        /// 傭車計上日のEnterイベント
        /// </summary>
        /// <param name="e"></param>
        private void dteCharterAddUpYMD_Enter(object sender, EventArgs e)
        {
            // ReadOnlyの場合は終了
            if (this.dteCharterAddUpYMD.ReadOnly) return;

            // 傭車計上日に初期値を設定
            this.SetInitValueInCharterAddUpDateCell();

            this.dteCharterAddUpYMD.SelectAll();
        }

        #endregion

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
                if (0 < Convert.ToInt32(this.numHaishaStartPointCd.Value))
                {
                    pointCode = Convert.ToInt32(this.numHaishaStartPointCd.Value);
                }
            }
            else
            {
                // 着地
                if (0 < Convert.ToInt32(this.numHaishaEndPointCd.Value))
                {
                    pointCode = Convert.ToInt32(this.numHaishaEndPointCd.Value);
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

                PointInfo info = _DalUtil.Point.GetList(para).FirstOrDefault();

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
                        this.numHaishaStartPointCd.Tag = info.ToraDONPointId;
                        this.edtHaishaStartPointNM.Text = info.ToraDONPointName;
                    }
                    else
                    {
                        // 着地
                        this.numHaishaEndPointCd.Tag = info.ToraDONPointId;
                        this.numHaishaEndPointCd.Value = info.ToraDONPointCode;
                        this.edtHaishaEndPointNM.Text = info.ToraDONPointName;
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
                        this.numHaishaStartPointCd.Tag = null;
                        this.numHaishaStartPointCd.Value = null;
                        this.edtHaishaStartPointNM.Text = string.Empty;
                    }
                    else
                    {
                        // 着地
                        this.numHaishaEndPointCd.Tag = null;
                        this.numHaishaEndPointCd.Value = null;
                        this.edtHaishaEndPointNM.Text = string.Empty;
                    }
                }
            }
        }

        /// <summary>
        /// 単位コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateFigCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetFigName(e);
        }

        /// <summary>
        /// 単位コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetFigName(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numFigCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 単位情報を取得
                FigSearchParameter para = new FigSearchParameter();
                para.FigCd = Convert.ToInt32(this.numFigCd.Value);
                ToraDONFigInfo info = _DalUtil.ToraDONFig.GetList(para).FirstOrDefault();

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
                    this.numFigCd.Tag = info.FigId;
                    this.edtFigNm.Text = info.FigName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numFigCd.Tag = null;
                    this.numFigCd.Value = null;
                    this.edtFigNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateStaffCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetStaffName(e);
        }

        /// <summary>
        /// 社員コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void SetStaffName(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numStaffCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 乗務員情報を取得
                StaffSearchParameter para = new StaffSearchParameter();
                para.ToraDONStaffCode = Convert.ToInt32(this.numStaffCd.Value);
                StaffInfo info = _DalUtil.Staff.GetList(para, null).FirstOrDefault();

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
                    this.numStaffCd.Tag = info.ToraDONStaffId;
                    this.edtStaffNm.Text = info.ToraDONStaffName;
                }
            }
            finally
            {
                if (isClear)
                {
                    this.numStaffCd.Tag = null;
                    this.numStaffCd.Value = null;
                    this.edtStaffNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 車両コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetLicPlateCarNo(e);
        }

        /// <summary>
        /// 車両コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetLicPlateCarNo(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numCarCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 車両情報を取得
                CarSearchParameter para = new CarSearchParameter();
                para.ToraDONCarCode = Convert.ToInt32(this.numCarCd.Value);
                CarInfo info = _DalUtil.Car.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    //車両情報クリア
                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;

                    return;
                }
                else
                {
                    //未使用データの場合は、エラー
                    if (info.ToraDONDisableFlag)
                    {
                        //使用可否エラー
                        DialogResult mq_result = MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        //車両情報クリア
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        return;
                    }

                    //編集エラーが出なければ車両情報セット

                    //車両情報の値を設定
                    this.numCarCd.Tag = info;

                    //受注情報に乗務員IDが存在せず、かつ、車両情報に乗務員が存在する場合
                    if (this.JuchuShosaiInfo.DriverId == 0
                        && 0 < info.StaffCode && !info.ToraDONStaffDisableFlag)
                    {
                        //乗務員の値を設定
                        this.numStaffCd.Tag = info.DriverId;
                        this.numStaffCd.Value = info.StaffCode;
                        this.edtStaffNm.Text = info.StaffName;
                    }

                    //受注情報に車種IDが存在せず、かつ、車両情報に車種が存在する場合
                    if (this.JuchuShosaiInfo.CarKindId == 0
                        && 0 < info.CarKindCode
                        && !info.ToraDONCarKindDisableFlag)
                    {
                        this.numCarKindCd.Tag = info.CarKindId;
                        this.numCarKindCd.Value = info.CarKindCode;
                        this.edtCarKindNm.Text = info.CarKindName;
                    }

                    //傭車の場合
                    if (this.CarKbnIsCharter(info.CarKbn))
                    {
                        this.edtLicPlateCarNo.Text = string.Empty;
                        if (!String.IsNullOrWhiteSpace(info.LicPlateCarNo))
                        {
                            this.edtLicPlateCarNo.Text = info.LicPlateCarNo;
                        }
                        //受注情報に傭車先IDが存在せず、かつ、車両情報に傭車先コードが存在する場合
                        if (this.JuchuShosaiInfo.CarOfChartererId == 0
                            && 0 < info.YoshasakiCode && !info.ToraDONYoshasakiDisableFlag)
                        {
                            this.numCarOfChartererCd.Tag = info.YoshasakiId;
                            this.numCarOfChartererCd.Value = info.YoshasakiCode;
                            this.edtCarOfChartererNm.Text = info.YoshasakiName;
                        }
                        this.ClearDriverValuesForCarKbn();
                    }
                    else
                    {
                        this.edtLicPlateCarNo.Text = info.LicPlateCarNo;
                        this.ClearCharterValuesForCarKbn();
                    }

                    this.RefreshStyleForCarKbn(info.CarKbn);
                }
            }
            finally
            {
                if (isClear)
                {
                    //車両情報クリア
                    this.numCarCd.Tag = null;
                    this.numCarCd.Value = null;
                    this.edtLicPlateCarNo.Text = string.Empty;
                    //受注情報に傭車先IDが存在しない場合
                    if (this.JuchuShosaiInfo.CarOfChartererId == 0)
                    {
                        //傭車先を削除
                        this.numCarOfChartererCd.Tag = null;
                        this.numCarOfChartererCd.Value = null;
                        this.edtCarOfChartererNm.Text = string.Empty;
                    }

                    this.RefreshStyleForCarKbn();
                    this.ClearCharterValuesForCarKbn();
                }
            }
        }

        /// <summary>
        /// 車種コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarKindCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetCarKindNM(e);
        }

        /// <summary>
        /// 車種コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetCarKindNM(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numCarKindCd.Value))
                {
                    isClear = true;
                    return;
                }

                // 車種情報を取得
                CarKindSearchParameter para = new CarKindSearchParameter();
                para.ToraDONCarKindCode = Convert.ToInt32(this.numCarKindCd.Value);
                CarKindInfo info = _DalUtil.CarKind.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    //車種情報クリア
                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;

                    return;
                }
                else
                {
                    //未使用データの場合は、エラー
                    if (info.ToraDONDisableFlag)
                    {
                        //使用可否エラー
                        DialogResult mq_result = MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        //車種情報クリア
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        return;
                    }

                    //編集エラーが出なければ車種情報セット
                    //車種情報の値を設定
                    this.numCarKindCd.Tag = info.ToraDONCarKindId;
                    this.edtCarKindNm.Text = info.ToraDONCarKindName;
                }
            }
            finally
            {
                if (isClear)
                {
                    //車種情報クリア
                    this.numCarKindCd.Tag = null;
                    this.numCarKindCd.Value = null;
                    this.edtCarKindNm.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 品目コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateItemCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetItemNM(e);
        }

        /// <summary>
        /// 品目コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetItemNM(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numItemCd.Value))
                {
                    isClear = true;
                    return;
                }

                //品目情報を取得
                ItemSearchParameter para = new ItemSearchParameter();
                para.ToraDONItemCode = Convert.ToInt32(this.numItemCd.Value);
                ItemInfo info = this._DalUtil.Item.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    //品目情報クリア
                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;

                    return;
                }
                else
                {
                    //未使用データの場合は、エラー
                    if (info.ToraDONDisableFlag)
                    {
                        //使用可否エラー
                        DialogResult mq_result = MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        //品目情報クリア
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        return;
                    }

                    //エラーが出なければ重量計算
                    //数量
                    decimal number = Convert.ToDecimal(this.numNumber.Value);
                    //重量
                    decimal wk_weight = 0;

                    if (this.CalcWeight(number, info.ToraDONWeight, out wk_weight))
                    {
                        if (wk_weight != 0)
                        {
                            //重量セット
                            this.numWeight.Value = wk_weight;
                        }
                    }
                    else
                    {
                        //重量計算に失敗したら、クリアして編集をキャンセル
                        //品目情報クリア
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        return;
                    }

                    //エラーが出なければ品目名をセット
                    //IDをセット
                    this.numItemCd.Tag = info.ToraDONItemId;
                    this.numItemCd.Value = info.ToraDONItemCode;
                    this.edtItemNM.Text = info.ToraDONItemName;

                    //単位情報を取得
                    FigSearchParameter fig_para = new FigSearchParameter() { FigId = info.ToraDONFigId };
                    ToraDONFigInfo fig_info = this._DalUtil.ToraDONFig.GetListInternal(null, fig_para).FirstOrDefault();

                    if (null != fig_info)
                    {
                        //単位をセット
                        this.numFigCd.Tag = fig_info.FigId;
                        this.numFigCd.Value = fig_info.FigCode;
                        this.edtFigNm.Text = fig_info.FigName;
                    }

                    //税区分・傭車税区分をセット
                    this.cmbTaxDispKbn.SelectedValue = info.ToraDONItemTaxKbn;
                    string strTaxDispKbn = String.Empty;
                    var TaxDispKbnList = this.systemNameList
                        .Where(x => x.SystemNameKbn == ((int)DefaultProperty.SystemNameKbn.TaxDispKbnShort)
                            && x.SystemNameCode == info.ToraDONItemTaxKbn);
                    if (TaxDispKbnList.Count() > 0)
                    {
                        strTaxDispKbn = TaxDispKbnList.First().SystemName;
                    }
                    CarInfo carInfo = this.numCarCd.Tag == null ? new CarInfo() : (CarInfo)this.numCarCd.Tag;
                    if (this.CarKbnIsCharter(carInfo.CarKbn)) 
                    {
                        // 傭車の場合
                        this.cmbTaxDispKbn.SelectedValue = info.ToraDONItemTaxKbn;
                        this.cmbCharterTaxDispKbn.SelectedValue = info.ToraDONItemTaxKbn;
                    }
                    else 
                    {
                        // 傭車以外の場合
                        this.cmbTaxDispKbn.SelectedValue = info.ToraDONItemTaxKbn;
                        this.cmbCharterTaxDispKbn.SelectedIndex = 0;
                    }

                }
            }
            finally
            {
                //品目情報クリア
                if (isClear)
                {
                    this.numItemCd.Tag = null;
                    this.numItemCd.Value = null;
                    this.edtItemNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 荷主コードの値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateOwnerCd(CancelEventArgs e)
        {
            // コード検索処理
            this.SetOwnerNM(e);
        }

        /// <summary>
        /// 荷主コード検索処理
        /// </summary>
        /// <param name="e"></param>
        private void SetOwnerNM(CancelEventArgs e)
        {
            bool isClear = false;

            try
            {
                //コードの入力が無い場合は抜ける
                if (0 == Convert.ToInt32(this.numOwnerCd.Value))
                {
                    isClear = true;
                    return;
                }

                //荷主情報を取得
                ToraDONOwnerSearchParameter para = new ToraDONOwnerSearchParameter();
                para.OwnerCode = Convert.ToInt32(this.numOwnerCd.Value);
                ToraDONOwnerInfo info = this._DalUtil.ToraDONOwner.GetList(para).FirstOrDefault();

                if (null == info)
                {
                    MessageBox.Show(
                        FrameUtilites.GetDefineMessage("MW2201003"),
                        this.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);

                    //荷主情報クリア
                    isClear = true;
                    //イベントをキャンセル
                    e.Cancel = true;

                    return;
                }
                else
                {
                    //未使用データの場合は、エラー
                    if (info.DisableFlag)
                    {
                        //使用可否エラー
                        DialogResult mq_result = MessageBox.Show(
                            FrameUtilites.GetDefineMessage("MW2201016"),
                            "警告",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);

                        //荷主情報クリア
                        isClear = true;
                        //イベントをキャンセル
                        e.Cancel = true;

                        return;
                    }
                    else
                    {
                        //荷主情報セット
                        this.numOwnerCd.Tag = info.OwnerId;
                        this.edtOwnerNM.Text = info.OwnerName;
                    }
                }
            }
            finally
            {
                //荷主情報クリア
                if (isClear)
                {
                    this.numOwnerCd.Tag = null;
                    this.numOwnerCd.Value = null;
                    this.edtOwnerNM.Text = string.Empty;
                }
            }
        }

        /// <summary>
        /// 料金計算を行います。
        /// </summary>
        private void PriceCalculation()
        {
            // 単価
            decimal atprice = Convert.ToDecimal(this.numAtPrice.Value);

            // 数量を取得
            decimal number = Convert.ToDecimal(this.numNumber.Value);

            // 金額計算
            decimal wk_price = 0;
            // ※金額丸め区分が関係する為、金額計算はしておく
            if (this.CalcPrice(number, atprice, this.TokuiGakCutKbn, out wk_price))
            {
                // 単価・金額セット
                if (atprice.CompareTo(decimal.Zero) !=0)
                {
                    // 売上金額
                    this.numPrice.Value = wk_price;

                    // 乗務員売上同額Flgにチェックが入っている場合、乗務員売上にも値を設定
                    if (this.chkJomuinUriageDogakuFlag.Checked)
                    {
                        this.numJomuinUriageKingaku.Value = wk_price;
                    }

                    //通行料
                    decimal tollFee = Convert.ToDecimal(this.numTollFeeInPrice.Value);

                    //附帯業務料
                    decimal futaigyomuryo = Convert.ToDecimal(this.numFutaigyomuryoInPrice.Value);

                    //金額によって確定フラグ変更
                    bool fixflag = false;
                    if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
                    {
                        fixflag = true;
                    }
                    this.chkFixFlag.Checked = fixflag;

                    //傭車金額の計算
                    this.CalcCharterFeeJuchuDtl();
                }
            }
        }

        /// <summary>
        /// 得金額丸め区分検索処理
        /// </summary>
        /// <param name="e"></param>
        private void GetCutkbn()
        {
            // 得意先の金額丸め区分を取得
            this.TokuiGakCutKbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;
            this.TokuiTaxCutKbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

            TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter();
            tokui_para.ToraDONTokuisakiCode = numTokuisakiCd.Value;
            TokuisakiInfo info = this._DalUtil.Tokuisaki.GetList(tokui_para).FirstOrDefault();

            if (info != null)
            {
                this.TokuiGakCutKbn = info.ToraDONGakCutKbn;
                this.TokuiTaxCutKbn = info.ToraDONTaxCutKbn;
            }
        }

        /// <summary>
        /// 傭車先金額丸め区分検索処理
        /// </summary>
        /// <param name="e"></param>
        private void GetCharterCutkbn()
        {
            // 傭車先の金額丸め区分を取得
            this.CharterTaxCutKbn = (int)BizProperty.DefaultProperty.HasuMArumeKbn.RoundOff;

            TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
            para.ToraDONTorihikiCode = Convert.ToInt32(this.numCarOfChartererCd.Value);
            TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

            if (info != null)
            {
                this.CharterTaxCutKbn = info.TaxCutKbn;
            }
        }

        /// <summary>
        /// 傭車コード（範囲終了）の値の検証を行います。
        /// </summary>
        /// <param name="e"></param>
        private void ValidateCarOfChartererCode(CancelEventArgs e)
        {
            bool is_clear = false;

            try
            {
                //コードの入力が無い場合
                if (0 == Convert.ToInt32(this.numCarOfChartererCd.Value))
                {
                    is_clear = true;
                    return;
                }

                TorihikisakiSearchParameter para = new TorihikisakiSearchParameter();
                para.ToraDONTorihikiCode = Convert.ToInt32(this.numCarOfChartererCd.Value);
                TorihikisakiInfo info = _DalUtil.Torihikisaki.GetList(para, null).FirstOrDefault();

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
                    this.numCarOfChartererCd.Tag = info.ToraDONTorihikiId;
                    this.edtCarOfChartererNm.Text = info.ToraDONTorihikiName;
                    this.CharterTaxCutKbn = info.TaxCutKbn;
                }
            }
            finally
            {
                if (is_clear)
                {
                    this.numCarOfChartererCd.Tag = null;
                    this.numCarOfChartererCd.Value = null;
                    this.edtCarOfChartererNm.Text = string.Empty;
                    this.CharterTaxCutKbn = 0;
                }
            }
        }

        /// <summary>
        /// 日付と時分の数値をDateTimeに変換します。
        /// </summary>
        /// <param name="numDay">日付</param>
        /// <param name="numHm">時分</param>
        /// <param name="nullFlg">null許容（true:null許容、false:null不可）</param>
        private DateTime? CnvDateTime(decimal? numDay, decimal? numHm, bool nullFlg)
        {
            if (numDay == null || numDay.Value.CompareTo(decimal.Zero) == 0)
            {
                // nullありの場合はnullで終了
                if (nullFlg) return null;
                numDay = 19500101;
            }

            if (numHm == null) numHm = decimal.Zero;

            // 日付
            int yer = Int32.Parse(numDay.Value.ToString("00000000").Substring(0, 4));
            int month = Int32.Parse(numDay.Value.ToString("00000000").Substring(4, 2));
            int day = Int32.Parse(numDay.Value.ToString("00000000").Substring(6, 2));

            // 時分
            int hour = Int32.Parse(numHm.Value.ToString("0000").Substring(0, 2));
            int minute = Int32.Parse(numHm.Value.ToString("0000").Substring(2, 2));

            return new DateTime(yer, month, day, hour, minute, 0);
        }

        /// <summary>
        /// DateTimeから日付の数値に変換します。
        /// </summary>
        /// <param name="numDay">DateTime</param>
        /// <param name="nullFlg">null許容（true:null許容、false:null不可）</param>
        private decimal? CnvDayDecimal(DateTime? dateTime, bool nullFlg)
        {
            if (dateTime == null)
            {
                // nullありの場合はnullで終了
                if (nullFlg) return null;
                dateTime = new DateTime(1950, 1, 1, 0, 0, 0);
            }

            return decimal.Parse(
                string.Concat(
                dateTime.Value.Year,
                dateTime.Value.Month.ToString("00"),
                dateTime.Value.Day.ToString("00")));

        }

        /// <summary>
        /// DateTimeから時分の数値に変換します。
        /// </summary>
        /// <param name="numDay">DateTime</param>
        /// <param name="nullFlg">null許容（true:null許容、false:null不可）</param>
        private decimal? CnvHmDecimal(DateTime? dateTime, bool nullFlg)
        {
            if (dateTime == null)
            {
                // nullありの場合はnullで終了
                if (nullFlg) return null;
                dateTime = new DateTime(1950, 1, 1, 0, 0, 0);
            }

            return decimal.Parse(
                string.Concat(
                dateTime.Value.Hour.ToString("00"),
                dateTime.Value.Minute.ToString("00")));
        }

        /// <summary>
        /// 指定した車両区分に関連する項目のスタイルを更新します。
        /// </summary>
        /// <param name="carKbn">車両区分</param>
        private void RefreshStyleForCarKbn(int carKbn = 0)
        {
            bool carkbnIsYosha = this.CarKbnIsCharter(carKbn);

            //車番の編集可否の設定
            this.edtLicPlateCarNo.ReadOnly = !carkbnIsYosha;
            //乗務員の編集可否の設定
            this.numStaffCd.ReadOnly = carkbnIsYosha;
            //傭車項目の編集可否の設定
            this.numCarOfChartererCd.ReadOnly = !carkbnIsYosha;
            this.numCharterPrice.ReadOnly = !carkbnIsYosha;
            this.numTollFeeInCharterPrice.ReadOnly = !carkbnIsYosha;
            this.dteCharterAddUpYMD.ReadOnly = !carkbnIsYosha;
            this.edtMagoYoshasaki.ReadOnly = !carkbnIsYosha;
            this.ddbCharterAddUpYMD.Enabled = carkbnIsYosha;
            this.chkCharterFixFlag.Enabled = carkbnIsYosha;

            if (this.CarKbnIsCharter(carKbn))
            {
                // 傭車の場合
                this.sideButton4.Enabled = false;
                this.sideButton6.Enabled = true;
                this.dropDownButton6.Enabled = true;
                this.cmbCharterTaxDispKbn.Enabled = true;
                this.spinButton8.Enabled = true;
            }
            else
            {
                // 自社車両の場合
                this.sideButton4.Enabled = true;
                this.sideButton6.Enabled = false;
                this.dropDownButton6.Enabled = false;
                this.cmbCharterTaxDispKbn.Enabled = false;
                this.spinButton8.Enabled = false;
            }
        }

        /// <summary>
        /// 車両区分に連動する傭車関連の値をクリアします。
        /// </summary>
        private void ClearCharterValuesForCarKbn()
        {
            this.numCarOfChartererCd.Value = 0;
            this.edtCarOfChartererNm.Text = string.Empty;
            this.edtMagoYoshasaki.Text = string.Empty;
            this.numCharterPrice.Value = 0;
            this.numTollFeeInCharterPrice.Value = 0;
            this.cmbCharterTaxDispKbn.SelectedIndex = 0;
            this.dteCharterAddUpYMD.Value = null;
            this.chkCharterFixFlag.Checked = false;
        }

        /// <summary>
        /// 指定した受注明細行の車両区分に連動する乗務員関連の値をクリアします。
        /// </summary>
        private void ClearDriverValuesForCarKbn()
        {
            this.numStaffCd.Tag = null;
            this.numStaffCd.Value = null;
            this.edtStaffNm.Text = string.Empty;
        }

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
        /// 傭車金額を計算して設定します。
        /// </summary>
        private void CalcCharterFeeJuchuDtl()
        {
 
            //変更の金額
            decimal wk_price = Convert.ToDecimal(this.numPrice.Value);

            int carKbn = this.HaishaInfo.CarKbn;

            // 車両区分が傭車の場合
            if (carKbn == (int)BizProperty.DefaultProperty.CarKbn.Yosha)
            {
                TorihikisakiSearchParameter torihiki_para = new TorihikisakiSearchParameter();
                torihiki_para.ToraDONTorihikiCode = Convert.ToInt32(this.numCarOfChartererCd.Value);
                TorihikisakiInfo tri_info = this._DalUtil.Torihikisaki.GetList(torihiki_para).FirstOrDefault();

                if (tri_info != null)
                {
                    decimal rate = tri_info.CharterRate;
                    int cutkbn = tri_info.GakCutKbn;

                    // 金額を計算
                    decimal wk_charterprice = 0;
                    wk_charterprice =
                        this.GetRoundVal(rate * wk_price / 100, cutkbn);

                    // 金額
                    this.numCharterPrice.Value = wk_charterprice;

                    // 通行料
                    decimal tollFeeInCharterPrice = Convert.ToDecimal(this.numTollFeeInCharterPrice.Value);

                    // 傭車確定フラグ
                    bool wk_charterfixflag = false;
                    if (wk_charterprice != 0 || tollFeeInCharterPrice != 0)
                    {
                        wk_charterfixflag = true;
                    }
                    this.chkCharterFixFlag.Checked = wk_charterfixflag;

                }
            }
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

        /// <summary>
        /// 確定フラグ、乗務員売上を設定します。
        /// </summary>
        private void SetFixFlagAndJomuinUriage()
        {
            //売上金額
            decimal wk_price = Convert.ToDecimal(this.numPrice.Value);

            //通行料
            decimal tollFee = Convert.ToDecimal(this.numTollFeeInPrice.Value);

            //附帯業務料
            decimal futaigyomuryo = Convert.ToDecimal(this.numFutaigyomuryoInPrice.Value);

            //金額によって確定フラグ変更
            bool fixflag = false;
            if (wk_price != 0 || tollFee != 0 || futaigyomuryo != 0)
            {
                fixflag = true;
            }
            this.chkFixFlag.Checked = fixflag;

            //乗務員売上の設定
            this.setJomuinUriage();
        }

        /// <summary>
        /// 傭車確定フラグを設定します。
        /// </summary>
        private void SetCharterFixFlag()
        {
            //傭車金額
            decimal wk_price = Convert.ToDecimal(this.numCharterPrice.Value);

            //傭車通行料
            decimal tollFee = Convert.ToDecimal(this.numTollFeeInCharterPrice.Value);

            //金額によって確定フラグ変更
            bool fixflag = false;
            if (wk_price != 0 || tollFee != 0)
            {
                fixflag = true;
            }
            this.chkCharterFixFlag.Checked = fixflag;
        }

        /// <summary>
        /// 乗務員売上の転記を行います。
        /// </summary>
        private void setJomuinUriage()
        {
            //乗務員売上同額フラグがチェックされた場合
            if (this.chkJomuinUriageDogakuFlag.Checked)
            {
                //金額を乗務員売上金額に設定
                this.numJomuinUriageKingaku.Value = Convert.ToDecimal(this.numPrice.Value);

                //編集不可
                this.numJomuinUriageKingaku.ReadOnly = true;
            }
            else
            {
                //編集可
                this.numJomuinUriageKingaku.ReadOnly = false;
            }
        }

        /// <summary>
        /// 計上日に初期値をセットします。
        /// </summary>
        private void SetInitValueInAddUpDateCell()
        {
            //計上日付が未入力の時のみ処理を行う
            if (this.dteAddUpYMD.Value == null)
            {

                //未入力だったら、得意先の計上日区分に応じて積日または、着日をセット
                //※得意先情報がない場合は積日をセット
                int clmdaykbn = (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate;

                int tokuisaki_cd = this.JuchuShosaiInfo.TokuisakiCode;

                TokuisakiSearchParameter tokui_para = new TokuisakiSearchParameter() { ToraDONTokuisakiCode = tokuisaki_cd };
                TokuisakiInfo tokui_info = this._DalUtil.Tokuisaki.GetListInternal(null, tokui_para).FirstOrDefault();

                if (tokui_info != null)
                {
                    clmdaykbn = tokui_info.ToraDONSaleSlipToClmDayKbn;
                }

                if (clmdaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate)
                {
                    // 積日が未入力でない場合は設定する
                    if (this.dteTaskStartYMD.Value != null)
                    {
                        // 積日をセット
                        this.dteAddUpYMD.Value = this.dteTaskStartYMD.Value;
                    }

                }
                else if (clmdaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate)
                {
                    // 着日が未入力でない場合セットする
                    if (this.dteTaskEndYMD.Value != null)
                    {
                        // 着日をセット
                        this.dteAddUpYMD.Value = this.dteTaskEndYMD.Value;
                    }
                }
            }
        }

        /// <summary>
        /// 傭車計上日に初期値をセットします。
        /// </summary>
        private void SetInitValueInCharterAddUpDateCell()
        {
            //傭車計上日付が未入力の時のみ処理を行う
            if (this.dteCharterAddUpYMD.Value == null)
            {
                //未入力だったら、取引先の傭車計上日区分に応じて計上日、積日、着日のいずれかをセット

                int paydaykbn = 0;

                int torihiki_cd = Convert.ToInt32(this.numCarOfChartererCd.Value);

                TorihikisakiSearchParameter torihiki_para = new TorihikisakiSearchParameter() { ToraDONTorihikiCode = torihiki_cd };
                TorihikisakiInfo torihiki_info = this._DalUtil.Torihikisaki.GetListInternal(null, torihiki_para).FirstOrDefault();

                if (torihiki_info != null)
                {
                    paydaykbn = torihiki_info.SaleSlipToPayDayKbn;
                }

                if (paydaykbn == 0)
                {
                    //受注計上日が未入力でない場合は設定する
                    if (this.dteAddUpYMD.Value != null)
                    {
                        this.dteCharterAddUpYMD.Value = this.dteAddUpYMD.Value;
                    }
                }

                else if (paydaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate)
                {
                    // 積日が未入力でない場合は設定する
                    if (this.dteTaskStartYMD.Value != null)
                    {
                        // 積日をセット
                        this.dteCharterAddUpYMD.Value = this.dteTaskStartYMD.Value;
                    }
                }

                else if (paydaykbn == (int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate)
                {
                    // 着日が未入力でない場合セットする
                    if (this.dteTaskEndYMD.Value != null)
                    {
                        // 着日をセット
                        this.dteCharterAddUpYMD.Value = this.dteTaskEndYMD.Value;
                    }
                }
            }
        }

        #endregion

        private void chkJomuinUriageDogakuFlag_CheckedChanged(object sender, EventArgs e)
        {
            this.setJomuinUriage();
        }

        /// <summary>
        /// 税計算を行います。
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
        /// 請求情報の算出に関連する項目に変更があるかチェックします。
        /// </summary>
        private bool ChkBillingInfo(ToraDONContractInfo contract_info)
        {
            bool rt_val = false;

            // 税区分
            int taxdispkbn = 0;

            // 請負ではない通常の受注か？
            if (contract_info == null)
            {
                //画面値をセットする
                taxdispkbn = Convert.ToInt32(this.cmbTaxDispKbn.SelectedValue);
            }
            else
            {
                // 請負データの場合は、請負の税区分・計上日をセット
                taxdispkbn = contract_info.TaxDispKbn;
            }

            if (Convert.ToDecimal(this.HaishaInfo.PriceInPrice).CompareTo(Convert.ToDecimal(this.numPrice.Value)) != 0  // 売上金額
                || Convert.ToDecimal(this.HaishaInfo.TollFeeInPrice).CompareTo(Convert.ToDecimal(this.numTollFeeInPrice.Value)) != 0 // 通行料
                || Convert.ToDecimal(this.HaishaInfo.FutaigyomuryoInPrice).CompareTo(Convert.ToDecimal(this.numFutaigyomuryoInPrice.Value)) != 0 // 附帯業務料
                || Convert.ToInt32(this.HaishaInfo.ItemCode) != Convert.ToInt32(this.numItemCd.Value) // 品目
                || this.HaishaInfo.AddUpYMD != this.dteAddUpYMD.Value // 計上日
                || Convert.ToDecimal(this.HaishaInfo.PriceInCharterPrice).CompareTo(Convert.ToDecimal(this.numCharterPrice.Value)) != 0  // 傭車金額
                || Convert.ToDecimal(this.HaishaInfo.TollFeeInCharterPrice).CompareTo(Convert.ToDecimal(this.numTollFeeInCharterPrice.Value)) != 0 // 傭車金額通行料
                || this.HaishaInfo.CharterAddUpYMD != this.dteCharterAddUpYMD.Value // 傭車計上日
                || this.HaishaInfo.TaxDispKbn != taxdispkbn // 税区分
                || this.HaishaInfo.CharterTaxDispKbn != Convert.ToInt32(this.cmbCharterTaxDispKbn.SelectedValue) // 傭車税区分
                ) 
            {
                rt_val = true;
            }

            return rt_val;
        }

        private void dte_Enter(object sender, EventArgs e)
        {
            NskDateTime ctl = sender as NskDateTime;
            ctl.SelectAll();
        }
    }
}
