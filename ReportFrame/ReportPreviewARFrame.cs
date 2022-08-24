using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document;
using jp.co.jpsys.util.print;
using Jpsys.HaishaManageV10.FrameLib;

namespace Jpsys.HaishaManageV10.ReportFrame
{
    /// <summary>
    /// CrystalReportのプレビュー表示を行うクラスです。
    /// </summary>
    public partial class ReportPreviewARFrame : Form
    {
        /// <summary>
        /// プレビュー画面の印刷ボタンを表す列挙体です。
        /// </summary>
        [Flags]
        public enum PreviewFramesButton
        {
            /// <summary>
            /// 印刷ボタンを表します。
            /// </summary>
            Print = 1,
            /// <summary>
            /// 最初のページボタンを表します。
            /// </summary>
            MoveFirst = 2,
            /// <summary>
            /// 前のページボタンを表します。
            /// </summary>
            MovePrev = 4,
            /// <summary>
            /// 次のページボタンを表します。
            /// </summary>
            MoveNext = 8,
            /// <summary>
            /// 最後のページボタンを表します。
            /// </summary>
            MoveLast = 16,
            /// <summary>
            /// 拡大/縮小ボタンを表します。
            /// </summary>
            Zoom = 32,
            /// <summary>
            /// 外部出力ボタンを表します。
            /// </summary>
            Export = 64
        }

        /// <summary>
        /// クラス初期化時に設定されたレポートオブジェクト
        /// </summary>
        //private ReportClass reportObject;

        /// <summary>
        /// クラス初期化時に設定されたActiveReportのレポートオブジェクト
        /// </summary>


        /// <summary>
        /// 印刷部数
        /// </summary>
        private int _pageCopies = 1;
        /// <summary>
        /// 部単位で印字するかどうか
        /// </summary>
        private bool _printCollated = true;
        /// <summary>
        /// 印刷する最初のページ
        /// </summary>
        private int _printStartPageNo = 0;
        /// <summary>
        /// 印刷する最後のページ
        /// </summary>
        private int _printEndPageNo = 0;
        /// <summary>
        /// プレビュー内で使用するページ設定
        /// </summary>
        private System.Drawing.Printing.PageSettings _pageSettings;

        /// <summary>
        /// 印刷指示をした回数
        /// </summary>
        private int _printedInstructions = 0;

        private int maxPageCount = 0;

        /// <summary>
        /// ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか
        /// </summary>
        private bool _nullToPaperSizeOnPageSetupDialog;

        /// <summary>
        /// レポートオブジェクトをRunせずに、
        /// レポートオブジェクトの保持するDocumentオブジェクトを直接使用するかどうか
        /// </summary>
        private bool _useDocumentDirectly = false;

        /// <summary>
        /// レポートオブジェクトをRunした直後のドキュメントの余白を保持する領域
        /// </summary>
        private System.Drawing.Printing.Margins _documentMargins;


        /// <summary>
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        public ReportPreviewARFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// プレビュー画面に表示するボタンの有効状態を格納した
        /// 列挙体の値を指定して本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="enableButtons">ボタンの有効状態を格納した列挙体</param>
        public ReportPreviewARFrame(PreviewFramesButton enableButtons)
        {
            InitializeComponent();

            //ボタンの有効無効を切り替える
            //--いったん全てのボタンを無効にする
            this.toolStripButtonPrint.Enabled = false;
            this.toolStripButtonMoveFirst.Enabled = false;
            this.toolStripButtonMovePrev.Enabled = false;
            this.toolStripButtonMoveNext.Enabled = false;
            this.toolStripButtonMoveLast.Enabled = false;
            this.toolStripSplitButtonZoom.Enabled = false;
            this.toolStripButtonExport.Enabled = false;

            //印刷ボタン
            if ((enableButtons & PreviewFramesButton.Print) ==
                PreviewFramesButton.Print)
            {
                this.toolStripButtonPrint.Enabled = true;
            }

            //最初のページボタン
            if ((enableButtons & PreviewFramesButton.MoveFirst) == PreviewFramesButton.MoveFirst)
            {
                toolStripButtonMoveFirst.Enabled = true;
            }

            //前のページボタン
            if ((enableButtons & PreviewFramesButton.MovePrev) == PreviewFramesButton.MovePrev)
            {
                toolStripButtonMovePrev.Enabled = true;
            }

            //次のページボタン
            if ((enableButtons & PreviewFramesButton.MoveNext) == PreviewFramesButton.MoveNext)
            {
                toolStripButtonMoveNext.Enabled = true;
            }

            //最後のページボタン
            if ((enableButtons & PreviewFramesButton.MoveLast) == PreviewFramesButton.MoveLast)
            {
                toolStripButtonMoveLast.Enabled = true;
            }

            //拡大縮小ボタン
            if ((enableButtons & PreviewFramesButton.Zoom) == PreviewFramesButton.Zoom)
            {
                toolStripSplitButtonZoom.Enabled = true;
            }

            //外部出力ボタン
            if ((enableButtons & PreviewFramesButton.Export) == PreviewFramesButton.Export)
            {
                toolStripButtonExport.Enabled = true;
            }

        }

        /// <summary>
        /// プレビュー画面に表示するボタンの有効状態を格納した
        /// 列挙体の値を指定して本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="enableButtons">ボタンの有効状態を格納した列挙体</param>
        /// <param name="nullToPaperSizeOnPageSetupDialog">ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか</param>
        /// <param name="useDocumentDirectly">レポートオブジェクトをRunせずに、レポートオブジェクトの保持するDocumentオブジェクトを直接使用するかどうか</param>
        public ReportPreviewARFrame(PreviewFramesButton enableButtons, bool nullToPaperSizeOnPageSetupDialog, bool useDocumentDirectly)
        {
            InitializeComponent();

            //ボタンの有効無効を切り替える
            //--いったん全てのボタンを無効にする
            this.toolStripButtonPrint.Enabled = false;
            this.toolStripButtonMoveFirst.Enabled = false;
            this.toolStripButtonMovePrev.Enabled = false;
            this.toolStripButtonMoveNext.Enabled = false;
            this.toolStripButtonMoveLast.Enabled = false;
            this.toolStripSplitButtonZoom.Enabled = false;
            this.toolStripButtonExport.Enabled = false;

            //印刷ボタン
            if ((enableButtons & PreviewFramesButton.Print) ==
                PreviewFramesButton.Print)
            {
                this.toolStripButtonPrint.Enabled = true;
            }

            //最初のページボタン
            if ((enableButtons & PreviewFramesButton.MoveFirst) == PreviewFramesButton.MoveFirst)
            {
                toolStripButtonMoveFirst.Enabled = true;
            }

            //前のページボタン
            if ((enableButtons & PreviewFramesButton.MovePrev) == PreviewFramesButton.MovePrev)
            {
                toolStripButtonMovePrev.Enabled = true;
            }

            //次のページボタン
            if ((enableButtons & PreviewFramesButton.MoveNext) == PreviewFramesButton.MoveNext)
            {
                toolStripButtonMoveNext.Enabled = true;
            }

            //最後のページボタン
            if ((enableButtons & PreviewFramesButton.MoveLast) == PreviewFramesButton.MoveLast)
            {
                toolStripButtonMoveLast.Enabled = true;
            }

            //拡大縮小ボタン
            if ((enableButtons & PreviewFramesButton.Zoom) == PreviewFramesButton.Zoom)
            {
                toolStripSplitButtonZoom.Enabled = true;
            }

            //外部出力ボタン
            if ((enableButtons & PreviewFramesButton.Export) == PreviewFramesButton.Export)
            {
                toolStripButtonExport.Enabled = true;
            }

            this._nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;
            this._useDocumentDirectly = useDocumentDirectly;
        }


        /// <summary>
        /// プレビュー画面タイトル、プレビューに使用するレポートオブジェクト、
        /// 印刷時のプリンタ設定およびページ設定を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="title">プレビュー画面のタイトル</param>
        /// <param name="reportObj">表示するレポートオブジェクト</param>
        /// <param name="printer">プリンタ設定</param>
        /// <param name="page">ページ設定</param>
        public void InitReportPreviewARFrame(string title, GrapeCity.ActiveReports.SectionReport reportAR7Obj, PrinterSettings printer,
            System.Drawing.Printing.PageSettings page)
        {
            //Debug T.Kuroki 20090708
            //FrameUtilites.DebugLogWriter("ReportPreviewARFrame", title + ":ReportPreviewARFrame初期化開始");
            this.reportAR7Object = reportAR7Obj;
            page.PrinterSettings = printer;
            this._pageSettings = page;

            //ARのレポートに設定された用紙がユーザー定義サイズだった場合は、用紙設定を
            //設定しなおしておく。実行ユーザーが一般ユーザーの時にユーザー定義サイズの用紙
            //を作成する権限が無いので、実行環境で一度設定された用紙設定でレポートに設定
            //された用紙設定を上書きする必要があるため
            if (this.reportAR7Object.PageSettings.PaperKind == PaperKind.Custom)
            {
                //カスタムのときは、
                //用紙名を設定
                this.reportAR7Object.PageSettings.PaperName =
                    this._pageSettings.PaperSize.PaperName;
                //用紙サイズを設定
                this.reportAR7Object.PageSettings.PaperWidth =
                    Convert.ToSingle((this._pageSettings.PaperSize.Width / 100));
                this.reportAR7Object.PageSettings.PaperHeight =
                    Convert.ToSingle((this._pageSettings.PaperSize.Height / 100));
            }

            #region 2018/04/10 プレビューで2回目以降の用紙サイズがデフォルトプリンタの用紙サイズになってしまう現象の対応

            // 勝利商會様でプレビュー画面から印刷する場合に印字位置が左上にズレる現象が発生
            // （印刷設定ファイルがなくて、プリンタ設定を保存してからだと現象は出ない）
            ////ARのDocumentのPrinterSettingsにControlerからセットされたPrinterSettingsを設定
            //this.reportAR7Object.Document.Printer.PrinterSettings = this._pageSettings.PrinterSettings;

            #endregion

            this.reportAR7Object.PrinterSettingFromPageSettings(this._pageSettings);

            //タイトル設定
            this.Text = title;

            ////アプリケーションアイコンの指定
            //this.Icon =
            //    global::Akashiya.TotalManagement.ReportFrame.Properties.Resources.printer;

            //レポートを実行する前に現在の余白をメンバ変数に保持する
            this._documentMargins = this._pageSettings.Margins;

            //レポート実行
            //this.reportAR7Object.Document.Printer.PaperKind = PaperKind.A4;
            this.reportAR7Object.Run();
            //this.reportAR7Object.Document.Print(true, false, false);
            this.AR7Viewer.Document = reportAR7Obj.Document;

        }

        /// <summary>
        /// プレビュー画面のタイトル、プレビューに使用するレポートオブジェクト、
        /// 印刷時のプリンタ設定、印刷部数、部単位の印刷有無、開始ページ、終了ページ
        /// を指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="title">プレビュー画面のタイトル</param>
        /// <param name="reportAR7Obj">表示するレポートオブジェクト</param>
        /// <param name="printer">プリンタ設定</param>
        /// <param name="page">ページ設定</param>
        /// <param name="copies">印刷部数</param>
        /// <param name="collated">部単位の印刷有無</param>
        /// <param name="startPageNo">開始ページ</param>
        /// <param name="endPageNo">終了ページ</param>
        public void InitReportPreviewARFrame(string title,
            GrapeCity.ActiveReports.SectionReport reportAR7Obj, PrinterSettings printer,
            System.Drawing.Printing.PageSettings page, int copies, bool collated,
            int startPageNo, int endPageNo)
        {
            this._pageCopies = copies;
            this._printCollated = collated;
            this._printStartPageNo = startPageNo;
            this._printEndPageNo = endPageNo;
            this.InitReportPreviewARFrame(title, reportAR7Obj, printer, page);
        }

        /// <summary>
        /// 直接印刷を開始します。
        /// </summary>
        private void PrintToPrinterDirect()
        {

            //プリンタ設定ダイアログを表示 
            {

                //using (PrinterChoiceDialogCR2K8 prdlg = new PrinterChoiceDialogCR2K8(this._pageSettings))
                using (ReportAppendix.PrinterChoiceDialog prdlg =
                    new ReportAppendix.PrinterChoiceDialog(this._pageSettings, this._nullToPaperSizeOnPageSetupDialog))
                {
                    //印刷範囲の選択を許可する。 
                    prdlg.AllowPrintRangeCopiesOption = true;
                    //余白の設定を禁止する。
                    prdlg.AllowMargins = false;
                    //印刷情報の設定を禁止する。
                    prdlg.AllowOrientation = false;

                    //ダイアログ表示＆Result取得 
                    DialogResult d_result = prdlg.ShowDialog(this);

                    if (d_result == DialogResult.OK)
                    {
                        //if (!prdlg.PopulatePageSetting.Margins.Equals(this._documentMargins))
                        //{
                        //    MessageBox.Show("警告", "プレビュー後は余白の設定はできません。\r\nプレビューを実行しなおすか、条件指定画面で印字設定を行ってください。"
                        //        , MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        //    return;
                        //}

                        //--プリントダイアログから、印刷設定を取り出す 
                        this._pageSettings = prdlg.PopulatePageSetting;

                        this.reportAR7Object.Document.Printer.PrinterName = this._pageSettings.PrinterSettings.PrinterName;


                        //ダイアログでセットされた印刷の範囲条件などをセット 
                        this._pageCopies = (int)prdlg.PageCopies;
                        this._printCollated = prdlg.PrintCollated;
                        if (prdlg.PrintRangeSelect)
                        {
                            //範囲選択を選択したときのみ 
                            this._printStartPageNo = (int)prdlg.PrintRangeFrom;
                            this._printEndPageNo = (int)prdlg.PrintRangeTo;
                        }

                        //--印刷設定を設定 
                        //--なぜか印刷部数と部単位の設定が反映されないので個別で設定
                        this._pageSettings.PrinterSettings.Copies = (short)this._pageCopies;
                        this._pageSettings.PrinterSettings.Collate = this._printCollated;
                        //--プリンタ設定をReportに設定
                        this.reportAR7Object.Document.Printer.PrinterSettings = this._pageSettings.PrinterSettings;
                        //--給紙方法を別で設定しておかないとプリンタ設定をしただけでは反映されない
                        this.reportAR7Object.PageSettings.PaperSource = this._pageSettings.PaperSource.Kind;
                        //--プリンタのデフォルト値も変更しておかなければ反映されず、しかもKindプロパティでは設定
                        //  できないためループで名前が一致しているものを探して設定する
                        for (int i = 0; i < this.reportAR7Object.Document.Printer.PrinterSettings.PaperSources.Count; i++)
                        {
                            //速度対策のため、いったんプロパティ値は取り出しておく
                            string wk_docsname =
                                this.reportAR7Object.Document.Printer.PrinterSettings.PaperSources[i].SourceName.ToString();
                            string wk_ppsname =
                                this._pageSettings.PaperSource.SourceName.ToString();

                            //文字列比較で給紙を設定
                            if (wk_docsname == wk_ppsname)
                            {
                                this.reportAR7Object.Document.Printer.DefaultPageSettings.PaperSource =
                                    this.reportAR7Object.Document.Printer.PrinterSettings.PaperSources[i];
                            }
                        }


                        this.reportAR7Object.PrinterSettingFromPageSettings(this._pageSettings);
                        //直接印刷


                        //this.reportAR7Object.Run(false);
                        this.AR7Viewer.Document = this.reportAR7Object.Document;
                        //this.reportAR7Object.Run(false);

                        //--印刷範囲を指定
                        if (prdlg.PrintRangeSelect)
                        {
                            //範囲選択を選択したときのみ 
                            this._printStartPageNo = (int)prdlg.PrintRangeFrom;
                            this._printEndPageNo = (int)prdlg.PrintRangeTo;
                            this.reportAR7Object.Document.Printer.PrinterSettings.PrintRange = PrintRange.SomePages;
                            this.reportAR7Object.Document.Printer.PrinterSettings.FromPage = this._printStartPageNo;
                            this.reportAR7Object.Document.Printer.PrinterSettings.ToPage = this._printEndPageNo;
                        }
                        else
                        {
                            //印刷範囲を指定しなかった場合は全部出力
                            this.reportAR7Object.Document.Printer.PrinterSettings.PrintRange = PrintRange.AllPages;
                        }

                        this.reportAR7Object.PrinterSettingFromPageSettings(this._pageSettings);

                        //--印刷開始
                        this.reportAR7Object.Document.Print(false, false, false);



                        //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                        //カウンターをカウントアップ
                        this._printedInstructions = this._printedInstructions + 1;

                    }

                }
            }
        }

        /// <summary>
        /// statusStripの「現在のページ番号」を再設定します。
        /// </summary>
        private void UpdateCurrentPage()
        {
            this.tslCurrentPage.Text = string.Format(" 現在のページ番号: {0}", this.AR7Viewer.ReportViewer.CurrentPage);
        }

        /// <summary>
        /// statusStripの「合計ページ番号」を再設定します。
        /// </summary>
        private void UpdateDocumentPagesCount()
        {
            this.tslDocumentPagesCount.Text = string.Format(" 合計ページ番号: {0}", this.AR7Viewer.ReportViewer.Document.Pages.Count);
        }

        /// <summary>
        /// statusStripの「ズーム率」を再設定します。
        /// </summary>
        private void UpdateZoomText()
        {
            string str = " ズーム率: ";

            switch ((int)this.AR7Viewer.ReportViewer.Zoom)
            {
                case -1:
                    this.tslZoom.Text = string.Format("{0}{1}", str,"ズーム幅");
                    break;
                case -2:
                    this.tslZoom.Text = string.Format("{0}{1}", str, "ズーム全体");
                    break;
                default:
                    this.tslZoom.Text = string.Format("{0}{1}%", str, this.AR7Viewer.ReportViewer.Zoom * 100);
                    break;
            }
        }


        #region プロパティ

        /// <summary>
        /// 印刷部数を取得・設定します。
        /// </summary>
        public int PageCopies
        {
            get { return this._pageCopies; }
            set { this._pageCopies = value; }
        }

        /// <summary>
        /// 部単位の印刷を行うかどうか取得・設定します。
        /// </summary>
        public bool PrintCollated
        {
            get { return this._printCollated; }
            set { this._printCollated = value; }
        }

        /// <summary>
        /// 印刷の開始ページを取得・設定します。
        /// </summary>
        public int PrintStartPageNo
        {
            get { return this._printStartPageNo; }
            set { this._printStartPageNo = value; }
        }

        /// <summary>
        /// 印刷の終了ページを取得・設定します。
        /// </summary>
        public int PrintEndPageNo
        {
            get { return this._printEndPageNo; }
            set { this._printEndPageNo = value; }
        }

        /// <summary>
        /// プレビューからの印刷に使用したページ設定を取得します。（読み取り専用）
        /// </summary>
        public System.Drawing.Printing.PageSettings PopulatePageSettings
        {
            get { return this._pageSettings; }
        }

        /// <summary>
        /// プレビュー画面から印刷指示をした回数を取得します。
        /// まったく指示をしていない場合は0となります。（読み取り専用）
        /// </summary>
        public int PrintedInstructions
        {
            get { return this._printedInstructions; }
        }

        #endregion

        private void toolStripButtonPrint_Click(object sender, EventArgs e)
        {
            //印刷ボタン
            this.PrintToPrinterDirect();
        }

        private void toolStripButtonMoveFirst_Click(object sender, EventArgs e)
        {
            //最初のページ

            //現在ページを変更
            this.AR7Viewer.ReportViewer.CurrentPage = 1;

            //ページ遷移ボタンの有効無効切り替え
            //　最初のページ：無効
            //　前のページ：無効
            //　次のページ：有効（総ページが１ページ以外のとき）
            //　最後のページ有効（総ページが１ページ以外のとき）
            this.toolStripButtonMoveFirst.Enabled = false;
            this.toolStripButtonMovePrev.Enabled = false;
            if (this.maxPageCount != 1)
            {
                this.toolStripButtonMoveNext.Enabled = true;
                this.toolStripButtonMoveLast.Enabled = true;
            }
            this.UpdateCurrentPage();
        }

        private void toolStripButtonMovePrev_Click(object sender, EventArgs e)
        {
            //前のページ

            //現在ページを変更
            this.AR7Viewer.ReportViewer.CurrentPage =
                this.AR7Viewer.ReportViewer.CurrentPage - 1;

            //ページ遷移ボタンの有効無効切り替え
            //　最初のページ：現在ページが１ページのときは無効それ以外は有効
            //　前のページ：現在ページが１ページのときは無効それ以外は有効
            //　次のページ：有効（総ページが１ページ以外のとき）
            //　最後のページ：有効（総ページが１ページ以外のとき）
            if (this.AR7Viewer.ReportViewer.CurrentPage == 1)
            {
                this.toolStripButtonMoveFirst.Enabled = false;
                this.toolStripButtonMovePrev.Enabled = false;
            }
            else
            {
                this.toolStripButtonMoveFirst.Enabled = true;
                this.toolStripButtonMovePrev.Enabled = true;
            }
            if (this.maxPageCount != 1)
            {
                this.toolStripButtonMoveNext.Enabled = true;
                this.toolStripButtonMoveLast.Enabled = true;
            }

            this.UpdateCurrentPage();
        }

        private void toolStripButtonMoveNext_Click(object sender, EventArgs e)
        {
            //次のページ

            //現在のページを変更
            this.AR7Viewer.ReportViewer.CurrentPage =
                this.AR7Viewer.ReportViewer.CurrentPage + 1;

            //ページ遷移ボタンの有効無効切り替え
            //　最初のページ：有効
            //　前のページ：有効
            //　次のページ：現在ページが最終ページのときは無効
            //　最後のページ：現在ページが最終ページのときは無効
            this.toolStripButtonMoveFirst.Enabled = true;
            this.toolStripButtonMovePrev.Enabled = true;
            if (this.AR7Viewer.ReportViewer.CurrentPage == this.maxPageCount)
            {
                this.toolStripButtonMoveNext.Enabled = false;
                this.toolStripButtonMoveLast.Enabled = false;
            }
            else
            {
                this.toolStripButtonMoveNext.Enabled = true;
                this.toolStripButtonMoveLast.Enabled = true;
            }
            this.UpdateCurrentPage();
        }

        private void toolStripButtonMoveLast_Click(object sender, EventArgs e)
        {
            //最後のページ

            //現在のページを変更
            this.AR7Viewer.ReportViewer.CurrentPage =
                this.AR7Viewer.Document.Pages.Count;

            //ページ遷移ボタンの有効無効切り替え
            //　最初のページ：有効
            //　前のページ：有効
            //　次のページ：無効
            //　最後のページ：無効
            this.toolStripButtonMoveFirst.Enabled = true;
            this.toolStripButtonMovePrev.Enabled = true;
            this.toolStripButtonMoveNext.Enabled = false;
            this.toolStripButtonMoveLast.Enabled = false;

            //現在のページ番号をセット
            this.UpdateCurrentPage();
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            //外部出力
            DialogResult dr = this.dlgExportReport.ShowDialog(this);
            if (dr == DialogResult.OK)
            {
                //保存の時だけ処理
                //ファイルの拡張子を見て判断
                //AddExtentionがtrueなので、常に拡張子が付くくので、ファイルので
                //拡張子を見てどの種類でエクスポートするか決める
                //--ファイル名を取り出しておく
                string wk_filename = this.dlgExportReport.FileName;
                //--拡張子を取り出す。大文字に変換
                string wk_upper_ext =
                    System.IO.Path.GetExtension(this.dlgExportReport.FileName).ToUpper();

                try
                {
                    switch (wk_upper_ext)
                    {
                        case ".PDF":
                            this.pdfExport.Export(this.AR7Viewer.Document, wk_filename);
                            break;
                        case ".RTF":
                            this.rtfExport.Export(this.AR7Viewer.Document, wk_filename);
                            break;
                        case ".XLS":
                            this.xlsExport.Export(this.AR7Viewer.Document, wk_filename);
                            break;
                        default:
                            break;
                    }

                    //正常に終わったらメッセージを出す
                    MessageBox.Show(this, "エクスポートが完了しました。", "レポートのエクスポート",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                catch (Exception err)
                {
                    throw err;
                }
            }
        }

        private void menuItemPageWidth_Click(object sender, EventArgs e)
        {
            //ページ幅にズーム
            this.AR7Viewer.ReportViewer.Zoom = -1;
        }

        private void menuItemPageFull_Click(object sender, EventArgs e)
        {
            //ページ全体にズーム
            this.AR7Viewer.ReportViewer.Zoom = -2;
        }

        private void menuItemPage400_Click(object sender, EventArgs e)
        {
            //400%にズーム
            this.AR7Viewer.ReportViewer.Zoom = 4;
        }

        private void menuItemPage300_Click(object sender, EventArgs e)
        {
            //300%にズーム
            this.AR7Viewer.ReportViewer.Zoom = 3;
        }

        private void menuItemPage200_Click(object sender, EventArgs e)
        {
            //200%にズーム
            this.AR7Viewer.ReportViewer.Zoom = 2;
        }

        private void menuItemPage150_Click(object sender, EventArgs e)
        {
            //150%にズーム
            this.AR7Viewer.ReportViewer.Zoom = Convert.ToSingle(1.5);
        }

        private void menuItemPage100_Click(object sender, EventArgs e)
        {
            //100%にズーム
            this.AR7Viewer.ReportViewer.Zoom = 1;
        }

        private void menuItemPage75_Click(object sender, EventArgs e)
        {
            //75%にズーム
            this.AR7Viewer.ReportViewer.Zoom = Convert.ToSingle(0.75);
        }

        private void menuItemPage50_Click(object sender, EventArgs e)
        {
            //50%にズーム
            this.AR7Viewer.ReportViewer.Zoom = Convert.ToSingle(0.5);
        }

        private void menuItemPage25_Click(object sender, EventArgs e)
        {
            //25%にズーム
            this.AR7Viewer.ReportViewer.Zoom = Convert.ToSingle(0.25);
        }

        private void ReportPreviewARFrame_Shown(object sender, EventArgs e)
        {
            ////フォーム表示時でないと設定できない初期値等を設定しておく
            //FrameUtilites.DebugLogWriter("ReportPreviewARFrame", "ReportPreviewARFrame_Shown");

            //総ページ数を取得
            this.maxPageCount = this.reportAR7Object.Document.Pages.Count;
            //最大表示
            this.WindowState = FormWindowState.Maximized;
            //フォーカス移動
            this.AR7Viewer.Focus();
            //表示拡大率　100%に
            this.AR7Viewer.ReportViewer.Zoom = 1;

            //総ページ数が１ページしかないときは、ページ遷移のボタンを無効にする
            if (this.maxPageCount == 1)
            {
                this.toolStripButtonMoveNext.Enabled = false;
                this.toolStripButtonMoveLast.Enabled = false;
                this.toolStripButtonMovePrev.Enabled = false;
                this.toolStripButtonMoveFirst.Enabled = false;
            }
            else
            {
                //それ以外の場合は、前のページに戻る系は使えないようにする
                this.toolStripButtonMovePrev.Enabled = false;
                this.toolStripButtonMoveFirst.Enabled = false;
            }
        }
        
        private void AR7Viewer_Scroll(object sender, ScrollEventArgs e)
        {
            //スクロールを移動させた場合ページ遷移が起こるので何かやっとく
            Console.WriteLine(this.AR7Viewer.ReportViewer.CurrentPage.ToString());
        }

        private GrapeCity.ActiveReports.SectionReport reportAR7Object;

        private void AR7Viewer_ZoomChanged(object sender, GrapeCity.ActiveReports.Viewer.Win.ZoomChangedEventArgs e)
        {
            this.UpdateZoomText();
        }

        private void AR7Viewer_LoadCompleted(object sender, EventArgs e)
        {
            // ツールバーを非表示します。
            this.AR7Viewer.Toolbar.ToolStrip.Visible = false;

            //各statusStrip情報の初期設定
            this.UpdateZoomText();
            this.UpdateCurrentPage();
            this.UpdateDocumentPagesCount();

            // 特にズームする必要ないのでデフォルトのままにする
            ////ページ全体にズーム
            //this.AR7Viewer.ReportViewer.Zoom = -2;
        }
   }
}
