using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.ClientDoc;
using jp.co.jpsys.util.print;
using Jpsys.SagyoManage.FrameLib;

namespace Jpsys.SagyoManage.ReportFrame
{
    /// <summary>
    /// CrystalReportのプレビュー表示を行うクラスです。
    /// </summary>
    public partial class ReportPreviewFrame : Form
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
        private ReportClass reportObject;
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
        private PageSettings _pageSettings;

        /// <summary>
        /// 印刷指示をした回数
        /// </summary>
        private int _printedInstructions = 0;

        private int maxPageCount = 0;

        /// <summary>
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        public ReportPreviewFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// プレビュー画面に表示するボタンの有効状態を格納した
        /// 列挙体の値を指定して本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="enableButtons">ボタンの有効状態を格納した列挙体</param>
        public ReportPreviewFrame(PreviewFramesButton enableButtons)
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
        /// プレビュー画面タイトル、プレビューに使用するレポートオブジェクト、
        /// 印刷時のプリンタ設定およびページ設定を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="title">プレビュー画面のタイトル</param>
        /// <param name="reportObj">表示するレポートオブジェクト</param>
        /// <param name="printer">プリンタ設定</param>
        /// <param name="page">ページ設定</param>
        public void InitReportPreviewFrame(string title, ReportClass reportObj, PrinterSettings printer,
            PageSettings page)
        {
            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", title + ":ReportPreviewFrame初期化開始");

            this.cryViewer.ReportSource = reportObj;
            this.reportObject = reportObj;
            page.PrinterSettings = printer;
            this._pageSettings = page;

            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", "CopyFrom");
            //クリレポから印刷設定をプリンタ設定にコピーしておく
            //この対応を行わないと、ページマージンがプリンタ標準になるため
            //印刷前と印刷後でプレビューの結果が異なってしまうので。
            reportObj.PrintOptions.CopyFrom(printer, page);

            this.Text = title;

            //アプリケーションアイコンの指定
            this.Icon =
                global::Jpsys.SagyoManage.ReportFrame.Properties.Resources.printer;

            //2011/05/26 Y.Shinmura 追加
            //ドリルダウンを禁止する
            this.cryViewer.EnableDrillDown = false;

            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", "CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定");
            //--CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定する。
            this.reportObject.PrintOptions.PrinterName = this._pageSettings.PrinterSettings.PrinterName;

            //Michibata 2014.03.06 add
            //プリンタ名が設定されないため、GetPrintOptions().PrinterNameに再セット
            ISCDReportClientDocument _rcd1;
            _rcd1 = this.reportObject.ReportClientDocument;
            _rcd1.PrintOutputController.GetPrintOptions().PrinterName = _pageSettings.PrinterSettings.PrinterName;

            this.reportObject.PrintOptions.PaperSource =
                (CrystalDecisions.Shared.PaperSource)this._pageSettings.PaperSource.RawKind;
            this.reportObject.PrintOptions.PaperSize =
                (CrystalDecisions.Shared.PaperSize)this._pageSettings.PaperSize.RawKind;
            if (this._pageSettings.Landscape)
            {
                this.reportObject.PrintOptions.PaperOrientation = PaperOrientation.Landscape;
            }
            else
            {
                this.reportObject.PrintOptions.PaperOrientation = PaperOrientation.Portrait;
            }

            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", "マージンを再設定");
            //--マージンを再設定する。
            PageMargins myMargins;
            myMargins = this.reportObject.PrintOptions.PageMargins;
            this.reportObject.PrintOptions.ApplyPageMargins(myMargins);

            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", "this.cryViewer.ReportSource = this.reportObject");
            this.cryViewer.ReportSource = this.reportObject;

        }

        /// <summary>
        /// プレビュー画面のタイトル、プレビューに使用するレポートオブジェクト、
        /// 印刷時のプリンタ設定、印刷部数、部単位の印刷有無、開始ページ、終了ページ
        /// を指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="title">プレビュー画面のタイトル</param>
        /// <param name="reportObj">表示するレポートオブジェクト</param>
        /// <param name="printer">プリンタ設定</param>
        /// <param name="page">ページ設定</param>
        /// <param name="copies">印刷部数</param>
        /// <param name="collated">部単位の印刷有無</param>
        /// <param name="startPageNo">開始ページ</param>
        /// <param name="endPageNo">終了ページ</param>
        public void InitReportPreviewFrame(string title, ReportClass reportObj, PrinterSettings printer,
            PageSettings page, int copies, bool collated, int startPageNo, int endPageNo)
        {
            this._pageCopies = copies;
            this._printCollated = collated;
            this._printStartPageNo = startPageNo;
            this._printEndPageNo = endPageNo;
            this.InitReportPreviewFrame(title, reportObject, printer, page);
        }

        /// <summary>
        /// 直接印刷を開始します。
        /// </summary>
        private void PrintToPrinterDirect()
        {

            //プリンタ設定ダイアログを表示 
            {
                using (PrinterChoiceDialogCR2K8 prdlg = new PrinterChoiceDialogCR2K8(this._pageSettings))
                {

                    //印刷範囲の選択を許可する。 
                    prdlg.AllowPrintRangeCopiesOption = true;

                    //ダイアログ表示＆Result取得 
                    DialogResult d_result = prdlg.ShowDialog(this);

                    if (d_result == DialogResult.OK)
                    {
                        //--プリントダイアログから、印刷設定を取り出す 
                        this._pageSettings = prdlg.PopulatePageSetting;

                        //ダイアログでセットされた印刷の範囲条件などをセット 
                        this._pageCopies = (int)prdlg.PageCopies;
                        this._printCollated = prdlg.PrintCollated;
                        if (prdlg.PrintRangeSelect)
                        {
                            //範囲選択を選択したときのみ 
                            this._printStartPageNo = (int)prdlg.PrintRangeFrom;
                            this._printEndPageNo = (int)prdlg.PrintRangeTo;
                        }

                        //--印刷設定を設定 (1回目のCopyFrom)
                        this.reportObject.PrintOptions.CopyFrom(this._pageSettings.PrinterSettings, this._pageSettings);

                        //--CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定する。
                        this.reportObject.PrintOptions.PrinterName = this._pageSettings.PrinterSettings.PrinterName;

                        //Michibata 2014.03.06 add
                        //プリンタ名が設定されないため、GetPrintOptions().PrinterNameに再セット
                        ISCDReportClientDocument _rcd1;
                        _rcd1 = this.reportObject.ReportClientDocument;
                        _rcd1.PrintOutputController.GetPrintOptions().PrinterName = _pageSettings.PrinterSettings.PrinterName;

                        //--印刷設定を設定  ※2回目のCopyFromになるのでカスタムの用紙が正常に適用される。
                        //                  1回目だとPrinterNameがクリアされてデフォルトプリンタに出力される。
                        //　　　　　　　　　　　当初は InitReportPreviewFrame メソッド から数えて2回目だったらOKとしていたが、
                        //                  社内で検証したいたらそれでもNGのケースがあった。
                        this.reportObject.PrintOptions.CopyFrom(this._pageSettings.PrinterSettings, this._pageSettings);

                        if (this._pageSettings.Landscape)
                        {
                            this.reportObject.PrintOptions.PaperOrientation =
                                PaperOrientation.Landscape;

                            //修正 T.Kuroki 20090525
                            // 用紙方向での両面設定逆転はプリンタダイアログでさせるため
                            // ここではコメントアウト
                            ////ランドスケープのときには、両面設定を逆転させる
                            ////--HorizontalとVerticalのときだけ逆転させる
                            //switch (this.reportObject.PrintOptions.PrinterDuplex)
                            //{
                            //    case PrinterDuplex.Default:
                            //        break;
                            //    case PrinterDuplex.Horizontal:
                            //        this.reportObject.PrintOptions.PrinterDuplex
                            //            = PrinterDuplex.Vertical;
                            //        break;
                            //    case PrinterDuplex.Simplex:
                            //        break;
                            //    case PrinterDuplex.Vertical:
                            //        this.reportObject.PrintOptions.PrinterDuplex
                            //            = PrinterDuplex.Horizontal;
                            //        break;
                            //    default:
                            //        break;
                            //}
                        }
                        else
                        {
                            this.reportObject.PrintOptions.PaperOrientation =
                                PaperOrientation.Portrait;
                        }

                        //--印刷前に現在ページを取得しておく
                        int wk_bf_print_cur_page =
                            this.cryViewer.GetCurrentPageNumber();

                        //--直接印刷 
                        this.reportObject.PrintToPrinter(this._pageCopies, this._printCollated, this._printStartPageNo, this._printEndPageNo);

                        //--レポートオブジェクトをビューアに再設定
                        this.cryViewer.ReportSource = this.reportObject;
                        //--レポートを最新状態に
                        this.cryViewer.RefreshReport();
                        //--表示ページを印字前のページに移動
                        this.cryViewer.ShowNthPage(wk_bf_print_cur_page);
                        //--拡大率を100に再設定（Vistaで動かす場合に、RefreshReport直後になぜかプレビューが左による現象に対する対策）
                        this.cryViewer.Invalidate();
                        this.cryViewer.Zoom(90);
                        this.cryViewer.Zoom(100);
                        this.cryViewer.Update();

                        //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                        //カウンターをカウントアップ
                        this._printedInstructions = this._printedInstructions + 1;

                    }

                }
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
        public PageSettings PopulatePageSettings
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
            this.cryViewer.ShowFirstPage();
        }

        private void toolStripButtonMovePrev_Click(object sender, EventArgs e)
        {
            //前のページ
            this.cryViewer.ShowPreviousPage();
        }

        private void toolStripButtonMoveNext_Click(object sender, EventArgs e)
        {
            //次のページ
            this.cryViewer.ShowNextPage();
        }

        private void toolStripButtonMoveLast_Click(object sender, EventArgs e)
        {
            //最後のページ
            this.cryViewer.ShowLastPage();
        }

        private void toolStripButtonExport_Click(object sender, EventArgs e)
        {
            //外部出力
            this.cryViewer.ExportReport();
        }

        private void menuItemPageWidth_Click(object sender, EventArgs e)
        {
            //ページ幅にズーム
            this.cryViewer.Zoom(1);
        }

        private void menuItemPageFull_Click(object sender, EventArgs e)
        {
            //ページ全体にズーム
            this.cryViewer.Zoom(2);
        }

        private void menuItemPage400_Click(object sender, EventArgs e)
        {
            //400%にズーム
            this.cryViewer.Zoom(400);
        }

        private void menuItemPage300_Click(object sender, EventArgs e)
        {
            //300%にズーム
            this.cryViewer.Zoom(300);
        }

        private void menuItemPage200_Click(object sender, EventArgs e)
        {
            //200%にズーム
            this.cryViewer.Zoom(200);
        }

        private void menuItemPage150_Click(object sender, EventArgs e)
        {
            //150%にズーム
            this.cryViewer.Zoom(150);
        }

        private void menuItemPage100_Click(object sender, EventArgs e)
        {
            //100%にズーム
            this.cryViewer.Zoom(100);
        }

        private void menuItemPage75_Click(object sender, EventArgs e)
        {
            //75%にズーム
            this.cryViewer.Zoom(75);
        }

        private void menuItemPage50_Click(object sender, EventArgs e)
        {
            //50%にズーム
            this.cryViewer.Zoom(50);
        }

        private void menuItemPage25_Click(object sender, EventArgs e)
        {
            //25%にズーム
            this.cryViewer.Zoom(25);
        }

        private void ReportPreviewFrame_Shown(object sender, EventArgs e)
        {
            //Debug T.Kuroki 20090708
            FrameUtilites.DebugLogWriter("ReportPreviewFrame", "ReportPreviewFrame_Shown");

            //最終頁までいったん表示して、最終ページ番号を取得する。
            this.cryViewer.ShowLastPage();
            this.maxPageCount = this.cryViewer.GetCurrentPageNumber();
            this.cryViewer.ShowFirstPage();
            this.WindowState = FormWindowState.Maximized;
            this.cryViewer.Zoom(100);
            this.cryViewer.Focus();
        }

        private void cryViewer_Navigate(object source, CrystalDecisions.Windows.Forms.NavigateEventArgs e)
        {
            this.SuspendLayout();

            try
            {
                //１ページしかないときには、ページ繊維ボタンをすべて無効にする
                if (this.maxPageCount > 1)
                {
                    this.toolStripButtonMoveNext.Enabled = true;
                    this.toolStripButtonMoveLast.Enabled = true;
                    this.toolStripButtonMovePrev.Enabled = true;
                    this.toolStripButtonMoveFirst.Enabled = true;

                    if (e.NewPageNumber == this.maxPageCount)
                    {
                        this.toolStripButtonMoveNext.Enabled = false;
                        this.toolStripButtonMoveLast.Enabled = false;
                    }
                    else if (e.NewPageNumber == 1)
                    {
                        this.toolStripButtonMovePrev.Enabled = false;
                        this.toolStripButtonMoveFirst.Enabled = false;
                    }

                }
                else
                {
                    this.toolStripButtonMoveNext.Enabled = false;
                    this.toolStripButtonMoveLast.Enabled = false;
                    this.toolStripButtonMovePrev.Enabled = false;
                    this.toolStripButtonMoveFirst.Enabled = false;
                }


            }
            finally
            {
                this.ResumeLayout(true);
            }
        }

    }
}
