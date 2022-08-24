using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using jp.co.jpsys.util;
using jp.co.jpsys.util.print;

namespace Jpsys.SagyoManage.ReportAppendix
{
    //*** s.arimura 2013/05/23 NSKFrameworkからコピー。PageSetupDialogでPaperSizeを取得できない現象の対策用
    /// <summary>
    /// OKボタンとキャンセルボタンのラベルに任意の文字を
    /// 表示できるプリンタ選択ダイアログを表示するクラスです。
    /// CrystalReport for Visual Studio 2008で印刷する際に使用します。
    /// </summary>
    /// <remarks>
    /// プリンタ及びそのプリンタで選択できる給紙方法及び、用紙設定を
    /// 選択するためのダイアログを表示するクラスです。
    /// OKボタンとキャンセルボタンに表示される文字列を任意に変更する
    /// 事が可能です。
    /// コンストラクタによって、ダイアログ表示前に選択項目を事前に
    /// 設定することが可能です。
    /// 本クラスは、CrystalReport for Visual Studio 2008で印刷を
    /// 行う際に使用することを想定しています。想定外の環境で使用した場合の
    /// 動作については保証しません。
    /// </remarks>
    public partial class PrinterChoiceDialog : Form
    {
        /// <summary>
        /// プリンタ設定のヘルパークラスを保持
        /// </summary>
        private NSKPrinterSettingHelper printerSettingHelper;

        //*** s.arimura 追加
        /// <summary>
        /// ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか
        /// </summary>
        private bool nullToPaperSizeOnPageSetupDialog;

        /// <summary>
        /// PrinterChoiceDialogクラスのインスタンスを初期化します。
        /// </summary>
        /// <remarks>
        /// 本コンストラクタにて初期化されたインスタンスに保持される、
        /// PageSettingsは、現在のコンピュータに設定されているデフォルト
        /// プリンタ（通常使うプリンタ）の設定が設定されたものになります。
        /// </remarks>
        public PrinterChoiceDialog()
        {
            InitializeComponent();

            //ヘルパークラスのインスタンス化
            this.printerSettingHelper = new NSKPrinterSettingHelper();

            this.InitPrinterChoiceDialog();
        }
        /// <summary>
        /// PageSettingsクラスのインスタンスを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="page">PageSettingsクラスのインスタンス</param>
        /// <param name="nullToPaperSizeOnPageSetupDialog">ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか</param>
        /// <remarks>
        /// PageSettingsクラスのインスタンスを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。デシリアライズされたPageSettingsオブジェクト
        /// からPrinterChoiceDialogのインスタンスを初期化する場合などにしようします。
        /// </remarks>
        public PrinterChoiceDialog(PageSettings page, bool nullToPaperSizeOnPageSetupDialog)
        {
            InitializeComponent();

            //ヘルパークラスのインスタンス化
            this.printerSettingHelper =
                new NSKPrinterSettingHelper(page);
            this.nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;

            this.InitPrinterChoiceDialog();

            this.PageCopies = page.PrinterSettings.Copies;

            this.nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;
        }

        /// <summary>
        /// プリンタ名と給紙方法名と用紙サイズを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="printerName">プリンタ名</param>
        /// <param name="sourceName">給紙方法名</param>
        /// <param name="sizeName">用紙サイズ名</param>
        /// <remarks>
        /// プリンタ名と給紙方法名・用紙サイズを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。指定した各項目の値に設定された
        /// PageSettingsを保持します。ただし、インストールされていないプリンタや
        /// プリンタが保持しない給紙方法および用紙サイズを指定した場合は、例外が
        /// 発生します。
        /// </remarks>
        public PrinterChoiceDialog(string printerName, string sourceName,
                string sizeName)
        {
            InitializeComponent();

            //ヘルパークラスのインスタンス化
            this.printerSettingHelper =
                new NSKPrinterSettingHelper(printerName, sourceName, sizeName);

            this.InitPrinterChoiceDialog();

        }

        /// <summary>
        /// NSKPrinterSettingHelperクラスのインスタンスを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="nskPrinterHelper">NSKPrinterSettingHelperクラスのインスタンス</param>
        /// <remarks>
        /// NSKPrinterSettingHelperクラスのインスタンスを指定して、PrinterChoiceDialog
        /// クラスのインスタンスを初期化します。事前にプリンタの設定などを変更した
        /// NSKPrinterSettingHelperオブジェクトからPageSettingsを取り出して保持します。
        /// </remarks>
        public PrinterChoiceDialog(NSKPrinterSettingHelper nskPrinterHelper)
        {
            InitializeComponent();

            //ヘルパークラスのインスタンスを設定
            this.printerSettingHelper = nskPrinterHelper;

            this.InitPrinterChoiceDialog();

        }

        /// <summary>
        /// 本画面の初期化を行います。
        /// </summary>
        private void InitPrinterChoiceDialog()
        {
            //親フォームがあるときは、そのフォームを中心に表示するように変更
            if (this.ParentForm == null)
            {
                this.StartPosition = FormStartPosition.CenterScreen;
            }
            else
            {
                this.StartPosition = FormStartPosition.CenterParent;
            }

            this.CreatePrinterCombo();
            this.CreatePagerSourceCombo();
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            this.EnableDuplex();
            this.EnableMono();
            this.EnableCopies();

            this.AllowMargins = true;//初期値として設定する(余白設定可能)※必要に応じて外部からプロパティ経由で変更可能
            this.AllowOrientation = true;//初期値として設定する(用紙方向設定可能)※必要に応じて外部からプロパティ経由で変更可能

        }

        #region プライベートメソッド

        /// <summary>
        /// 両面印刷の有効無効を制御
        /// </summary>
        private void EnableDuplex()
        {
            //一端両面印刷関係を無効に
            this.chkUseDuplex.Checked = false;
            this.chkUseDuplex.Enabled = false;
            this.grbDuplexAllign.Enabled = false;


            //プリンタ設定から両面の可否をとる
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            PageSettings wk_ps =
                this.printerSettingHelper.StoredPageSettings;
            if (wk_pr.CanDuplex)
            {
                //両面に設定されていたらチェックON
                if (wk_pr.Duplex != Duplex.Simplex)
                {
                    this.chkUseDuplex.Checked = true;

                    //水平方向か垂直方向かを取得する
                    switch (wk_pr.Duplex)
                    {
                        case Duplex.Default:
                            //Defaultの時は垂直方向
                            this.rbtDuplexVertical.Checked = true;
                            break;
                        case Duplex.Horizontal:
                            //水平
                            //--横置きかどうか判断して横置きの場合は、方向を逆転
                            if (!wk_ps.Landscape)
                            {
                                //縦置き
                                this.rbtDuplexVertical.Checked = false;
                                this.rbtDuplexHorizontal.Checked = true;
                            }
                            else
                            {
                                //横置き
                                this.rbtDuplexVertical.Checked = true;
                                this.rbtDuplexHorizontal.Checked = false;
                            }
                            break;
                        case Duplex.Simplex:
                            //ありえない。。
                            break;
                        case Duplex.Vertical:
                            //垂直
                            //--横置きかどうか判断して横置きの場合は、方向を逆転
                            if (!wk_ps.Landscape)
                            {
                                //縦置き
                                this.rbtDuplexVertical.Checked = true;
                                this.rbtDuplexHorizontal.Checked = false;
                            }
                            else
                            {
                                //横置き
                                this.rbtDuplexVertical.Checked = false;
                                this.rbtDuplexHorizontal.Checked = true;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }
            //両面仕様可否を使用可能の状態をプリンタ設定の両面印刷可否から取得
            this.chkUseDuplex.Enabled = wk_pr.CanDuplex;
            this.grbDuplexAllign.Enabled = this.chkUseDuplex.Checked;

        }

        /// <summary>
        /// 白黒印刷の有効無効を制御
        /// </summary>
        private void EnableMono()
        {
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            //白黒印刷使用可否をカラー印刷のサポート可否から取得
            this.chkUseMono.Enabled = wk_pr.SupportsColor;
            //PageSettingsからカラー印刷の使用有無を取得して、フラグを逆転
            this.chkUseMono.Checked = !this.printerSettingHelper.StoredPageSettings.Color;

        }

        /// <summary>
        /// 部数の設定の有効無効を制御
        /// </summary>
        private void EnableCopies()
        {
            //画面上で指定した部数を表示し、グループボックス「部数指定」のEnabledをFalseにする
            this.grbPageCopies.Enabled = false;
            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            this.nudCopies.Maximum = wk_pr.MaximumCopies;

            //PrinterSettings wk_pr =
            //    this.printerSettingHelper.StoredPageSettings.PrinterSettings;

            //if (wk_pr.MaximumCopies == 1)
            //{
            //    this.nudCopies.Value = 1;
            //    this.grbPageCopies.Enabled = false;
            //}
            //else
            //{
            //    this.grbPageCopies.Enabled = true;
            //    this.nudCopies.Maximum = wk_pr.MaximumCopies;
            //}
        }


        /// <summary>
        /// プリンタのコンボボックスを作成
        /// </summary>
        private void CreatePrinterCombo()
        {
            IList<PrinterNameIndexItem>
                printer_list = this.printerSettingHelper.GetPrinterNameIndexList();

            if (printer_list.Count != 0)
            {
                this.comboInstalledPrinters.DataSource = printer_list;
                this.comboInstalledPrinters.DisplayMember =
                    PrinterNameIndexItem.DisplayMemberString;
                this.comboInstalledPrinters.ValueMember =
                    PrinterNameIndexItem.ValueMemberString;

                //現在選択されているプリンタを取得する
                string printer_name =
                    this.printerSettingHelper.StoredPageSettings.PrinterSettings.PrinterName;
                for (int i = 0; i < printer_list.Count; i++)
                {
                    if (printer_list[i].PrinterNameString == printer_name)
                    {
                        //該当しているプリンタが存在していたら、comboの選択を
                        //変更。
                        this.comboInstalledPrinters.SelectedIndex = i;
                        break;
                    }
                }
            }
        }

        /// <summary>
        /// 給紙方法のコンボボックスを作成
        /// </summary>
        private void CreatePagerSourceCombo()
        {
            IList<PaperSourceNameIndexItem>
                source_list =
                this.printerSettingHelper.GetPaperSourceNameIndexList();

            if (source_list.Count != 0)
            {
                this.comboPrinterPaperSources.DataSource = source_list;
                this.comboPrinterPaperSources.DisplayMember =
                    PaperSourceNameIndexItem.DisplayMemberString;
                this.comboPrinterPaperSources.ValueMember =
                    PaperSourceNameIndexItem.ValueMemberString;

                //現在選択されている給紙方法を取得する
                string source_name =
                    this.printerSettingHelper.StoredPageSettings.PaperSource.SourceName;
                for (int i = 0; i < source_list.Count; i++)
                {
                    if (source_list[i].PaperSourceNameString == source_name)
                    {
                        //該当している給紙方法が存在していたら、comboの選択を
                        //変更
                        this.comboPrinterPaperSources.SelectedIndex = i;
                        break;
                    }
                }
            }

            //給紙方法を変更した場合用紙サイズが変更される可能性があるため再設定を行う
            SetPaperSourceInnerSetting(
                this.comboPrinterPaperSources.Text.Trim());

        }

        /// <summary>
        /// プリンタ名を指定して給紙方法のコンボボックスのリストに
        /// 指定したプリンタの給紙方法のリストをセットします。
        /// </summary>
        private void ChangePaperSourceComboItemList(string printerName)
        {
            this.printerSettingHelper.SetCurrentPrinter(printerName);
            this.CreatePagerSourceCombo();
        }

        /// <summary>
        /// 給紙方法名を指定して、プリンタ情報を保持する内部変数を
        /// セットします。
        /// </summary>
        /// <param name="paperSourceName"></param>
        private void SetPaperSourceInnerSetting(string paperSourceName)
        {
            this.printerSettingHelper.SetCurrentPaperSource(paperSourceName);
        }


        #endregion

        #region パブリックメソッド

        /// <summary>
        /// OKボタンに表示する文字列を設定します。
        /// </summary>
        /// <param name="buttonText">OKボタンに表示する文字列</param>
        /// <remarks>
        /// ダイアログ画面のOKボタンに表示されている文字列を変更します。
        /// OKボタンを押下するとDialogResult値にDialogResult.OKが設定
        /// されてダイアログが閉じられます。
        /// </remarks>
        public void SetOkButtonDisplayText(string buttonText)
        {
            this.btnOk.Text = buttonText;
        }

        /// <summary>
        /// キャンセルボタンに表示する文字列を設定します。
        /// </summary>
        /// <param name="buttonText">キャンセルボタンに表示する文字列</param>
        /// <remarks>
        /// ダイアログ画面のキャンセルボタンに表示されている文字列を変更します。
        /// キャンセルボタンを押下するとDialogResult値にDialogResult.Cancelが
        /// 設定されてダイアログが閉じられます。
        /// </remarks>
        public void SetCancelButtonDisplayText(string buttonText)
        {
            this.btnCancel.Text = buttonText;
        }

        #endregion

        #region プロパティ

        /// <summary>
        /// 本クラスで設定されたPageSettingsクラスのインスタンスを取得します。
        /// （読み取り専用）
        /// </summary>
        public PageSettings PopulatePageSetting
        {
            get
            {
                return this.printerSettingHelper.StoredPageSettings;
            }
        }

        /// <summary>
        /// 本クラスで設定されたNSKPrinterSettingHelperクラスのインスタンスを
        /// 取得します。（読み取り専用）
        /// </summary>
        public NSKPrinterSettingHelper PopulateNSKPrinterSettingHelper
        {
            get
            {
                return this.printerSettingHelper;
            }
        }

        /// <summary>
        /// 印刷範囲と印刷部数の設定を許可するかどうかを指定します。
        /// 規定値はfalse(許可しない）に設定されています。
        /// </summary>
        /// <remarks>
        /// このプロパティがtrueの時は、印刷範囲と印刷部数を設定する
        /// 事ができるようになります。規定値はfalse（許可しない）に
        /// 設定されています。
        /// </remarks>
        public bool AllowPrintRangeCopiesOption
        {
            get
            {
                return this.plnRangeCopies.Visible;
            }
            set
            {
                this.plnRangeCopies.Visible = value;
            }

        }

        /// <summary>
        /// 印刷部数を取得・設定します。
        /// </summary>
        public decimal PageCopies
        {
            get
            {
                return this.nudCopies.Value;
            }
            set
            {
                this.nudCopies.Value = value;
            }
        }

        /// <summary>
        /// 複数部印刷する際に部単位で印刷するかどうかを取得・設定します。
        /// 規定値はtrue（部単位で印刷する）に設定されています。
        /// </summary>
        public bool PrintCollated
        {
            get
            {
                return this.chkCollated.Checked;
            }
            set
            {
                this.chkCollated.Checked = value;
            }

        }

        /// <summary>
        /// 印刷範囲で「すべて」が選択されているかどうかを取得・設定します。
        /// </summary>
        public bool PrintRangeAll
        {
            get { return this.rbtAllRange.Checked; }
            set { this.rbtAllRange.Checked = value; }
        }

        /// <summary>
        /// 印刷範囲で「現在のぺージ」が選択されているかどうかを取得・設定します。
        /// </summary>
        public bool PrintRangeCurrentPage
        {
            get { return this.rbtRangeCurrent.Checked; }
            set { this.rbtRangeCurrent.Checked = value; }
        }

        /// <summary>
        /// 印刷範囲で「ページ指定」が選択されているかどうかを取得・設定します。
        /// </summary>
        public bool PrintRangeSelect
        {
            get { return this.rbtRangeSelect.Checked; }
            set { this.rbtRangeSelect.Checked = value; }
        }

        /// <summary>
        /// 印刷範囲のページ指定の開始ページを取得・設定します。
        /// </summary>
        public decimal PrintRangeFrom
        {
            get { return this.nudRangeFrom.Value; }
            set { this.nudRangeFrom.Value = value; }

        }

        /// <summary>
        /// 印刷範囲のページ指定の終了ページを取得・設定します。
        /// </summary>
        public decimal PrintRangeTo
        {
            get { return this.nudRangeTo.Value; }
            set { this.nudRangeTo.Value = value; }


        }

        /// <summary>
        /// 両面印刷を使用するかどうかを取得します。（読み取り専用）
        /// </summary>
        public bool PrintUseDuplex
        {
            get { return this.chkUseDuplex.Checked; }
        }

        /// <summary>
        /// 白黒印刷を使用するかどうかを取得します。（読み取り専用）
        /// </summary>
        public bool PrintUseMono
        {
            get { return this.chkUseMono.Checked; }
        }

        /// <summary>
        /// ページ設定ダイアログ ボックスの余白セクションが有効かどうかを表す値を取得または設定します。
        /// </summary>
        public bool AllowMargins { get; set; }

        /// <summary>
        /// ページ設定ダイアログ ボックスの用紙方向セクション (横向きまたは縦向き) が有効かどうかを表す値を取得または設定します。
        /// </summary>
        public bool AllowOrientation { get; set; }

        #endregion


        private void comboInstalledPrinters_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //プリンターのコンボがユーザで選択されて変更されたときのイベント
            Cursor.Current = Cursors.WaitCursor;
            this.ChangePaperSourceComboItemList(
                this.comboInstalledPrinters.Text.Trim());
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            //プリンターが変更されたら、両面設定と白黒設定をリフレッシュ
            this.EnableDuplex();
            this.EnableMono();
            this.EnableCopies();//印刷部数の設定も切り替える

        }


        private void comboPrinterPaperSources_SelectionChangeCommitted(object sender, EventArgs e)
        {
            //給紙方法をユーザで選択されて変更されたときのイベント
            Cursor.Current = Cursors.WaitCursor;
            SetPaperSourceInnerSetting(
                this.comboPrinterPaperSources.Text.Trim());
            this.textPaperName.Text =
                this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;

        }

        private void btnShowPaperSetting_Click(object sender, EventArgs e)
        {
            //用紙設定のダイアログを表示
            using (PageSetupDialog psd = new PageSetupDialog())
            {

                psd.AllowMargins = this.AllowMargins;
                psd.AllowOrientation = this.AllowOrientation;

                psd.EnableMetric = true;
                psd.PrinterSettings =
                    (System.Drawing.Printing.PrinterSettings)this.PopulatePageSetting.PrinterSettings.Clone();
                psd.PageSettings = 
                    (System.Drawing.Printing.PageSettings)this.printerSettingHelper.StoredPageSettings.Clone();

                if (this.nullToPaperSizeOnPageSetupDialog)
                {
                    psd.PageSettings.PaperSize = null;
                }
                else
                {
                    //初期化しない場合でも対象のプリンタに対象の用紙サイズがが存在しない場合は
                    //初期化する

                    bool _nullToPaperSize = true;

                    //PrinterSettings.PaperSizesをループして、レポートの設定と一致するPaperSizeをセットする
                    foreach (PaperSize paperSize in psd.PrinterSettings.PaperSizes)
                    {
                        //ユーザー定義サイズかどうかの判断
                        if (paperSize.Kind == PaperKind.Custom)
                        {
                            //カスタムの場合は高さと幅が同じかどうかで判断する
                            if (paperSize.Height == psd.PageSettings.PaperSize.Height &&
                                paperSize.Width == psd.PageSettings.PaperSize.Width)
                            {
                                //用紙があった場合は初期化しない
                                _nullToPaperSize = false;
                                break;
                            }
                        }
                        else
                        {
                            if (paperSize.Kind == psd.PageSettings.PaperSize.Kind)
                            {
                                //用紙があった場合は初期化しない
                                _nullToPaperSize = false;
                                break;
                            }
                        }
                    }

                    if (_nullToPaperSize)
                    {
                        //用紙サイズが存在しない場合には初期化する
                        psd.PageSettings.PaperSize = null;
                    }

                }

                if (psd.ShowDialog(this) == DialogResult.OK)
                {
                    this.printerSettingHelper = new NSKPrinterSettingHelper(psd.PageSettings);
                }

                this.CreatePagerSourceCombo();

                this.textPaperName.Text =
                    this.printerSettingHelper.StoredPageSettings.PaperSize.PaperName;
            }
        }

        private void PrinterChoiceDialog_Load(object sender, EventArgs e)
        {
            //印刷範囲・部数指定のパネルが表示の時は、その分画面を小さくする
            if (!this.plnRangeCopies.Visible)
            {
                this.Height = this.Height - this.plnRangeCopies.Height;
            }

            //フォーカスをOKボタンにする
            this.btnOk.Focus();
        }

        private void nudRangeFrom_Enter(object sender, EventArgs e)
        {
            //ページ範囲を指定する入力枠にフォーカスが入ったら
            //印刷ページを指定するラジオボタンのチェックを移動する
            this.rbtRangeSelect.Checked = true;
        }

        private void nudRangeTo_Enter(object sender, EventArgs e)
        {
            //ページ範囲を指定する入力枠にフォーカスが入ったら
            //印刷ページを指定するラジオボタンのチェックを移動する
            this.rbtRangeSelect.Checked = true;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            //基本の設定部分だけはOKの時にコミットしておく

            PrinterSettings wk_pr =
                this.printerSettingHelper.StoredPageSettings.PrinterSettings;
            PageSettings wk_ps =
                this.printerSettingHelper.StoredPageSettings;

            //--両面の設定
            if (this.chkUseDuplex.Checked)
            {
                //垂直？
                if (this.rbtDuplexVertical.Checked)
                {
                    //横置きかどうか
                    if (!wk_ps.Landscape)
                    {
                        wk_pr.Duplex = Duplex.Vertical;
                    }
                    else
                    {
                        wk_pr.Duplex = Duplex.Horizontal;
                    }
                }
                //水平？
                else if (this.rbtDuplexHorizontal.Checked)
                {
                    //横置きかどうか？
                    if (!wk_ps.Landscape)
                    {
                        wk_pr.Duplex = Duplex.Horizontal;
                    }
                    else
                    {
                        wk_pr.Duplex = Duplex.Vertical;
                    }
                }
            }
            else
            {
                //両面のチェックが無ければ
                wk_pr.Duplex = Duplex.Simplex;
            }

            //白黒
            wk_ps.Color = !this.chkUseMono.Checked;

            //一度もプロパティが変更されていない場合は、内部変数が設定されていない
            //場合がある問題への対応策でプロパティーの幾つかは設定する。
            wk_pr.PrinterName = wk_pr.PrinterName;
            wk_ps.PaperSource = wk_ps.PaperSource;
            wk_ps.PaperSize = wk_ps.PaperSize;
            wk_ps.PrinterResolution = wk_ps.PrinterResolution;
            wk_pr.Copies = (short)Convert.ToInt32(this.nudCopies.Value);
        }

        private void chkUseDuplex_CheckedChanged(object sender, EventArgs e)
        {
            this.grbDuplexAllign.Enabled = this.chkUseDuplex.Checked;
        }

    }
}