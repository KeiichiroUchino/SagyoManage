using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Printing;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;
using jp.co.jpsys.util.print;
using Jpsys.HaishaManageV10.Property;
using Jpsys.HaishaManageV10.FrameLib;
using Jpsys.HaishaManageV10.ReportAppendix;

namespace Jpsys.HaishaManageV10.ReportFrame
{
    public class ReportPrintingARControl
    {
        /// <summary>
        /// ActiveReportsVer7を使用した印刷関連の操作を行う
        /// メソッドおよびプロパティを提供するクラスです。
        /// </summary>
        private Form _printToFrame;

        private FrameLib.ReportPrintDestType _dest;
        private ReportPreviewARFrame.PreviewFramesButton _enablePrevButtons;
        private short _copies = 1;

        //印刷を指示した回数
        private int _printedInstructions = 0;

        #region 用紙サイズがカスタムになるのを回避する為に追加

        /// <summary>
        /// ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか
        /// </summary>
        private bool _nullToPaperSizeOnPageSetupDialog;

        /// <summary>
        /// レポートオブジェクトをRunせずに、
        /// レポートオブジェクトの保持するDocumentオブジェクトを直接使用するかどうか
        /// </summary>
        private bool _useDocumentDirectly = false;

        #endregion

        /// <summary>
        /// プリンタ設定のヘルパークラスを保持
        /// </summary>
        private NSKPrinterSettingHelper printerSettingHelper = new NSKPrinterSettingHelper();

        /// <summary>
        /// 本クラスのデフォルトコンストラクタは
        /// プライベート化
        /// </summary>
        private ReportPrintingARControl()
        { }

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsFormおよびActiveReportVer7のレポート
        /// オブジェクト、印刷先を表す列挙体を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rptAR7">ActiveReportVer7のレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        public ReportPrintingARControl(Form printToFrame,
            GrapeCity.ActiveReports.SectionReport rptAR7, FrameLib.ReportPrintDestType dest)
        {
            this._printToFrame = printToFrame;
            this._rptAR = rptAR7;
            this._dest = dest;

            //プレビュー画面で使用するボタンの設定が渡されないときには全てを使用するように
            this._enablePrevButtons =
                ReportPreviewARFrame.PreviewFramesButton.Print |
                ReportPreviewARFrame.PreviewFramesButton.MoveFirst |
                ReportPreviewARFrame.PreviewFramesButton.MovePrev |
                ReportPreviewARFrame.PreviewFramesButton.MoveNext |
                ReportPreviewARFrame.PreviewFramesButton.MoveLast |
                ReportPreviewARFrame.PreviewFramesButton.Zoom |
                ReportPreviewARFrame.PreviewFramesButton.Export;
        }

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsFormおよびActiveReportVer7のレポート
        /// オブジェクト、印刷先を表す列挙体、印刷部数を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rptAR7">ActiveReportVer7のレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        /// <param name="copies">コピー枚数</param>
        public ReportPrintingARControl(Form printToFrame,
            GrapeCity.ActiveReports.SectionReport rptAR7, FrameLib.ReportPrintDestType dest, short copies)
        {
            this._printToFrame = printToFrame;
            this._rptAR = rptAR7;
            this._dest = dest;

            //プレビュー画面で使用するボタンの設定が渡されないときには全てを使用するように
            this._enablePrevButtons =
                ReportPreviewARFrame.PreviewFramesButton.Print |
                ReportPreviewARFrame.PreviewFramesButton.MoveFirst |
                ReportPreviewARFrame.PreviewFramesButton.MovePrev |
                ReportPreviewARFrame.PreviewFramesButton.MoveNext |
                ReportPreviewARFrame.PreviewFramesButton.MoveLast |
                ReportPreviewARFrame.PreviewFramesButton.Zoom |
                ReportPreviewARFrame.PreviewFramesButton.Export;

            this._copies = copies;
        }

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsForm及びCrystalReportのレポート
        /// オブジェクト、印刷先を表す列挙体を、プレビュー画面で使用するボタンを
        /// 表す列挙体を指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rpt">CrystalReportのレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        /// <param name="enablePrevButtons">プレビュー画面で使用するボタンを表す列挙体</param>
        public ReportPrintingARControl(Form printToFrame,
            GrapeCity.ActiveReports.SectionReport rptAR7, ReportPrintDestType dest,
            ReportPreviewARFrame.PreviewFramesButton enablePrevButtons)
        {
            this._printToFrame = printToFrame;
            this._rptAR = rptAR7;
            this._dest = dest;

            //プレビューボタンの制御用のフラグ
            this._enablePrevButtons = enablePrevButtons;
        }

        #region 用紙サイズがカスタムになるのを回避する為に追加

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsFormおよびActiveReportVer7のレポート
        /// オブジェクト、印刷先を表す列挙体を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="printToFrame">票の印刷指示を行ったWindowsForm</param>
        /// <param name="rptAR7">ActiveReportVer7のレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        /// <param name="nullToPaperSizeOnPageSetupDialog">ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか。カスタムの用紙サイズが通常時にうまく動作しない場合にONにします。</param>
        /// <param name="useDocumentDirectly">レポートオブジェクトをRunせずに、レポートオブジェクトの保持するDocumentオブジェクトを直接使用するかどうか</param>
        public ReportPrintingARControl(Form printToFrame
            , GrapeCity.ActiveReports.SectionReport rptAR7
            , FrameLib.ReportPrintDestType dest
            , bool nullToPaperSizeOnPageSetupDialog
            , bool useDocumentDirectly = false)
        {
            this._printToFrame = printToFrame;
            this._rptAR = rptAR7;
            this._dest = dest;

            //プレビュー画面で使用するボタンの設定が渡されないときには全てを使用するように
            this._enablePrevButtons =
                ReportPreviewARFrame.PreviewFramesButton.Print |
                ReportPreviewARFrame.PreviewFramesButton.MoveFirst |
                ReportPreviewARFrame.PreviewFramesButton.MovePrev |
                ReportPreviewARFrame.PreviewFramesButton.MoveNext |
                ReportPreviewARFrame.PreviewFramesButton.MoveLast |
                ReportPreviewARFrame.PreviewFramesButton.Zoom |
                ReportPreviewARFrame.PreviewFramesButton.Export;

            this._nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;
            this._useDocumentDirectly = useDocumentDirectly;
        }

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsForm及びCrystalReportのレポート
        /// オブジェクト、印刷先を表す列挙体を、プレビュー画面で使用するボタンを
        /// 表す列挙体を指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rptAR7">CrystalReportのレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        /// <param name="enablePrevButtons">プレビュー画面で使用するボタンを表す列挙体</param>
        /// <param name="nullToPaperSizeOnPageSetupDialog">ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか。カスタムの用紙サイズが通常時にうまく動作しない場合にONにします。</param>
        public ReportPrintingARControl(Form printToFrame
            , GrapeCity.ActiveReports.SectionReport rptAR7
            , FrameLib.ReportPrintDestType dest
            , ReportPreviewARFrame.PreviewFramesButton enablePrevButtons
            , bool nullToPaperSizeOnPageSetupDialog)
        {
            this._printToFrame = printToFrame;
            this._rptAR = rptAR7;
            this._dest = dest;

            //プレビューボタンの制御用のフラグ
            this._enablePrevButtons = enablePrevButtons;

            this._nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;
        }

        #endregion

        /// <summary>
        /// インスタンス作成時に設定された値を元に、印刷処理を実行します。
        /// </summary>
        public void RunPrintProcess()
        {
            //デフォルトのページ設定を作成
            PrintDocument pd = new PrintDocument();
            System.Drawing.Printing.PageSettings wk_popupage = pd.DefaultPageSettings;

            //デフォルトのページからプリンタ設定を取り出す
            PrinterSettings wk_popuprinter = wk_popupage.PrinterSettings;

            //　(FinePrint等で)Duplexに0が入っている時にToStringメソッドを実行する
            //　とArgumentExceptionが発生するため、強制的にSimplex(片面)に書き換える Y.Shinmura 20110422
            //　System.ArgumentException: 値 '0' は enum 'Duplex' の有効な値ではありません。
            if (wk_popuprinter.Duplex == 0)
            {
                wk_popuprinter.Duplex = Duplex.Simplex;
            }

            //プリンタ設定が保持している両面印刷設定値を変更する。
            //Duplex列挙体に値がない場合は、片面印刷とする。
            Duplex wk_popuduplex = Duplex.Default;
            switch (wk_popuprinter.Duplex)
            {
                case Duplex.Default:
                    wk_popuduplex = Duplex.Default;
                    break;
                case Duplex.Horizontal:
                    wk_popuduplex = Duplex.Horizontal;
                    break;
                case Duplex.Simplex:
                    wk_popuduplex = Duplex.Simplex;
                    break;
                case Duplex.Vertical:
                    wk_popuduplex = Duplex.Vertical;
                    break;
                default:
                    wk_popuduplex = Duplex.Simplex;
                    break;
            }

            //ドキュメントオブジェクトからPrinterSettingsを取り出す
            wk_popuprinter = this._rptAR.Document.Printer.PrinterSettings;
            //PageSettingsのPrinterSettingsにドキュメントから取得したPrinterSettingsを設定
            //　これで、給紙位置や用紙サイズが取れれば御の字
            wk_popupage.PrinterSettings = wk_popuprinter;


            //PageSettingsはすべてのプロパティを独自にドキュメントオブジェクトのPageSettingsからSystem..のPageSettingsへ入れる
            //--部単位の設定
            switch (this._rptAR.PageSettings.Collate)
            {
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.Collate:
                    //部単位
                    wk_popupage.PrinterSettings.Collate = true;
                    break;
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.Default:
                    //プリンタ標準なのでそのまま設定
                    wk_popupage.PrinterSettings.Collate = wk_popupage.PrinterSettings.Collate;
                    break;
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.DontCollate:
                    //非部単位
                    wk_popupage.PrinterSettings.Collate = false;
                    break;
                default:
                    break;
            }

            //--レポートをプリンタのデフォルト用紙サイズで印刷するかどうか
            if (!this._rptAR.PageSettings.DefaultPaperSize)
            {
                //--さらにユーザー定義サイズを使うかどうか判断して設定
                if (this._rptAR.PageSettings.PaperKind == PaperKind.Custom)
                {
                    //ユーザー定義サイズを使うよう設定
                    //--PaperSizeのインスタンスを作って設定
                    var wk_custom_ps = new PaperSize(this._rptAR.PageSettings.PaperName,
                        (int)(this._rptAR.PageSettings.PaperWidth * 100),
                        (int)(this._rptAR.PageSettings.PaperHeight * 100));
                    wk_popupage.PaperSize = wk_custom_ps;
                }
                else
                {
                    ////ユーザー定義サイズでなければ普通にセット
                    //wk_popupage.PaperSize.RawKind = (int)this._rptAR7.PageSettings.PaperKind;  


                    //*** s.arimura 2014/08/28 用紙サイズのセット方法を修正
                    //*** 上の方法（修正前）だと用紙サイズが設定されなかった。用紙サイズがA3のレポートが今回初めて出てきたので気づいた。
                    //*** 修正後はプリンターの用紙サイズのコレクションから、PaperKindが合致した用紙サイズを取得してそれを設定するようにした。

                    //プリンターからレポートの用紙サイズに合致するサイズを取り出す。
                    var paperSizeFromPrinter =
                        wk_popuprinter.PaperSizes.Cast<System.Drawing.Printing.PaperSize>()
                        .FirstOrDefault(element => element.Kind == this._rptAR.PageSettings.PaperKind);

                    //合致するプリンターがあれば設定するのでその判断
                    if (paperSizeFromPrinter != null)
                    {
                        wk_popupage.PaperSize = paperSizeFromPrinter;
                    }
                }
            }
            else
            {
                //デフォルトであってもページは設定
                wk_popupage.PaperSize.RawKind = (int)this._rptAR.PageSettings.PaperKind;
            }
            //--レポートをプリンタのデフォルト給紙トレイで印刷するかどうか
            if (!this._rptAR.PageSettings.DefaultPaperSource)
            {
                //デフォルトではないときは、給紙トレイを設定する
                wk_popupage.PaperSource.RawKind = (int)this._rptAR.PageSettings.PaperSource;
            }
            else
            {

                ////デフォルトのときは自分に自分を設定
                //wk_popupage.PaperSource.RawKind = wk_popupage.PaperSource.RawKind;　//遅い

                //もしかしたらプロパティにアクセスするだけでいいかも...？
                var a = wk_popupage.PaperSource.RawKind;
                a.ToString();
            }
            //--両面印刷
            //両面はプリンタ設定から設定している（これより上のセクション）のでここでは設定しない
            //--以下のプロパティはSystem.Drawing.Printing.PageSettings および PrinterSettingsには
            //  設定値が無いので設定しない
            //  Gutter MirrorMargins

            //--ページ余白
            //--ARのMarginsはインチ単位、.NETのMarginsは1/100インチ単位なので、100倍する
            //---いったんレポートのMarginsの要素を取得
            var wk_arpt_mrg_bottom = this._rptAR.PageSettings.Margins.Bottom;
            var wk_arpt_mrg_left = this._rptAR.PageSettings.Margins.Left;
            var wk_arpt_mrg_right = this._rptAR.PageSettings.Margins.Right;
            var wk_arpt_mrg_top = this._rptAR.PageSettings.Margins.Top;
            wk_popupage.Margins.Bottom = (int)(wk_arpt_mrg_bottom * 100);
            wk_popupage.Margins.Left = (int)(wk_arpt_mrg_left * 100);
            wk_popupage.Margins.Right = (int)(wk_arpt_mrg_right * 100);
            wk_popupage.Margins.Top = (int)(wk_arpt_mrg_top * 100);

            //--ページの印刷方向
            switch (this._rptAR.PageSettings.Orientation)
            {
                case PageOrientation.Default:
                    //プリンタ標準なので何もしない
                    break;
                case PageOrientation.Landscape:
                    //横向きで印刷する
                    wk_popupage.Landscape = true;
                    break;
                case PageOrientation.Portrait:
                    //縦方向で印刷
                    wk_popupage.Landscape = false;
                    break;
                default:
                    break;
            }

            //--両面設定
            wk_popuprinter.Duplex = wk_popuduplex;


            //保存されたレポートの印刷設定を読み取って書き換える。
            //ない場合は、そのまま。
            //ディレクトリの存在チェック
            if (!Directory.Exists(SystemProperty.GetInstance().ReportPrinterSettingPath))
            {
                //存在しないときは作る
                Directory.CreateDirectory(SystemProperty.GetInstance().ReportPrinterSettingPath);
            }

            //--印刷設定ファイルのファイル名を作成する。レポート定義クラス名でファイル名を作る。
            string settingfile_nm =
                Path.Combine(
                    SystemProperty.GetInstance().ReportPrinterSettingPath,
                        this._rptAR.GetType().Name) + ".xml";

            //ファイルの存在チェック
            if (File.Exists(settingfile_nm))
            {
                //スコープ内変数定義
                //プリンタ設定保存ファイルのリダイレクトプリンタ対応処理中に、デシリアライズ処理を
                //続行するかどうかが判別可能なので、続行可能かどうかのフラグを用意しておく(true:続行可能）
                //こんなところにフラグを定義したくはないが・・時間切れ

                bool continue_deserialize = true;

                #region 印刷速度を上げるため以下コメントアウト（勝利商會用） K.Yamasako 20170615

                //bool continue_deserialize = false;

                ////存在していたらデシリアライズの前に、リダイレクトされたプリンタの判断して、
                ////リダイレクトの番号が接続のたびに変わる対策をしておく

                ////デシリアライズしたプリンタ設定のプリンタが現在の使用可能な
                ////プリンタの一覧にそんざいしているか確認する
                ////--OSの現在ログインから認識されているプリンタの一覧を取得
                //IList<PrinterNameIndexItem> printer_list =
                //    this.printerSettingHelper.GetPrinterNameIndexList();
                ////--空のプリンタ一覧をリダイレクトプリンタの一覧格納用のリストにセット
                //List<PrinterNameIndexItem> redirect_prt_list =
                //    new List<PrinterNameIndexItem>();
                ////--空のプリンタ一覧を通常プリンタの一覧格納用のリストにセット
                //List<PrinterNameIndexItem> norediret_prt_list =
                //    new List<PrinterNameIndexItem>();
                ////--プリンタの一覧を取得をリダイレクト・通常に振り分ける
                //foreach (PrinterNameIndexItem item in printer_list)
                //{
                //    if (item.PrinterNameString.LastIndexOf("(リダイレクト") == -1)
                //    {
                //        //リダイレクトされていないプリンタ
                //        norediret_prt_list.Add(item);
                //    }
                //    else
                //    {
                //        //リダイレクトされている
                //        redirect_prt_list.Add(item);
                //    }
                //}

                ////シリアライズされたファイルをStreamで開いてプリンタ名だけ取得し、リダイレクトプリンタが
                ////シリアライズされているのかをチェックして・・・
                ////--設定ファイルの再保存のために１行分の文字列を保持する領域
                //List<string> overwrite_list = new List<string>();

                ////ファイルを開く
                //using (System.IO.StreamReader sr = new System.IO.StreamReader(settingfile_nm))
                //{

                //    while (sr.Peek() > -1)
                //    {
                //        #region sr.Peek()のループ　60回

                //        //一行読む
                //        string wk_line = sr.ReadLine();

                //        //プリンタ名を保持している行を探す
                //        if (wk_line.IndexOf("<printerName") != -1)
                //        {
                //            //printerNameの開始タグと終了タグ、プリンタ名を分離して取り出しておく
                //            //--最初に">"が出現したところまでの文字列を開始タグとする
                //            string start_tag = wk_line.Substring(0, wk_line.IndexOf(">") + 1);
                //            //--文字の最後尾からみて、最初に"</"が出現したところより前を取り除いた文字を終了タグとする
                //            string end_tag = wk_line.Remove(0, wk_line.LastIndexOf("</"));
                //            //--最初に">"が出現した位置の次の文字から、一行分の文字数から開始タグと終了タグの文字数を
                //            //  減算した文字数分を取得してプリンタ名とする
                //            string save_printer =
                //                wk_line.Substring(wk_line.IndexOf(">") + 1,
                //                    wk_line.Length - (start_tag.Length + end_tag.Length));
                //            //--書き換えに使うプリンタ名を保持する領域、プリンタを見つけられなかったときのために、
                //            //  読み込んだ時の値を設定しておく。
                //            string replace_printer = save_printer;

                //            //プリンタ名にリダイレクトプリンタと識別できる文字があるか確認する
                //            int redirect_string_index = save_printer.LastIndexOf("(リダイレクト");
                //            if (redirect_string_index != -1)
                //            {
                //                //リダイレクトされているプリンタの場合現在存在する同名のリダイレクト
                //                //プリンタを探す
                //                //--リダイレクトの文字をカットする
                //                string edit_saved_printer =
                //                    save_printer.Substring(0, redirect_string_index);
                //                //--リダイレクトプリンタの一覧から該当するプリンタを探す
                //                foreach (PrinterNameIndexItem item in redirect_prt_list)
                //                {
                //                    //リダイレクトプリンタの名前かリダイレクト判別文字を抜いておく
                //                    string edit_redirect_printer =
                //                        item.PrinterNameString.Substring(
                //                            0, item.PrinterNameString.LastIndexOf("(リダイレクト"));
                //                    //文字列比較
                //                    if (edit_saved_printer.Trim() == edit_redirect_printer.Trim())
                //                    {
                //                        //同一の場合は、一致したリダイレクトプリンタのプリンタ名で書き換えるようにする
                //                        //ので置き換え用の領域にプリンタ名を設定
                //                        replace_printer = item.PrinterNameString;

                //                        //デシリアライズの続行を可能としておく
                //                        continue_deserialize = true;

                //                        //ループを抜ける
                //                        break;
                //                    }
                //                }
                //            }
                //            else
                //            {
                //                //リダイレクトプリンタではない場合でも現在のOSが認識しているプリンタの一覧に
                //                //保存されたプリンタが存在しているか確認をする
                //                foreach (PrinterNameIndexItem item in norediret_prt_list)
                //                {
                //                    if (item.PrinterNameString.Trim() ==
                //                        replace_printer.Trim())
                //                    {
                //                        //プリンタ名が一致しているので、デシリアライズの続行を可能としておく
                //                        continue_deserialize = true;

                //                        //ループを抜ける
                //                        break;
                //                    }
                //                }
                //            }

                //            //書き換えたprinterNameとタグを結合して１行分を作る
                //            StringBuilder sb_line = new StringBuilder();
                //            sb_line.Append(start_tag);
                //            sb_line.Append(replace_printer);
                //            sb_line.Append(end_tag);

                //            //保存用の１行分領域にセット
                //            wk_line = sb_line.ToString();
                //        }

                //        //保存用のStringBuilderに保存しておく
                //        overwrite_list.Add(wk_line);

                //        #endregion

                //    }

                //    sr.Close();
                //}

                ////編集の有無にかかわらず保存する
                //using (System.IO.StreamWriter sw = new System.IO.StreamWriter(settingfile_nm))
                //{
                //    foreach (var item in overwrite_list)
                //    {
                //        sw.WriteLine(item);
                //    }
                //    sw.Close();
                //}

                #endregion

                //デシリアライズ処理を続行するかどうか判別
                if (continue_deserialize)
                {
                    //再保存処理を行った後デシリアライズ処理を走らせる
                    //PageSettingsをデシリアライズ
                    using (Stream stream = File.Open(settingfile_nm, FileMode.Open))
                    {
                        try
                        {
                            System.Drawing.Printing.PageSettings deser_page =
                                (System.Drawing.Printing.PageSettings)(new SoapFormatter()).Deserialize(stream);
                            PrinterSettings deser_printer =
                                deser_page.PrinterSettings;

                            if (!deser_printer.IsValid)
                            {
                                throw new ApplicationException("有効でないプリンタとして、デシリアライズされています。");
                            }

                            //必要に応じて設定を書き換えたデシリアライズ済みの印刷設定値
                            //を内部変数に設定する。
                            wk_popupage = deser_page;
                            wk_popuprinter = deser_printer;
                        }
                        catch (Exception err)
                        {
                            ////メッセージを表示して、標準設定を使用することを通知
                            //MessageBox.Show(this._printToFrame,
                            //    "プリンタ設定の復元に失敗しました、以前使用したプリンタが存在していない可能性があります。" +
                            //    "通常使用するプリンタを使用します。", this._printToFrame.Text, MessageBoxButtons.OK,
                            //    MessageBoxIcon.Information);

                            // K.Yamasako 20170615
                            // プリンタ設定の復元に失敗した場合（前回印刷したプリンタが無いなど）は
                            // 通常使うプリンタではなく、再度選択させる
                            MessageBox.Show(
                                this._printToFrame,
                                "プリンタ設定の復元に失敗しました。\n" +
                                "以前使用したプリンタが存在していない可能性があります。\n" +
                                "再度、プリンタを選択してください。",
                                this._printToFrame.Text,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning
                                );

                            this.NoPrinterChoiceDialog = false;

                            //エラーはスローしないs
                            //throw err;
                        }
                        finally
                        {
                            //後かたずけ
                            stream.Close();
                        }
                    }
                }
                else
                {
                    ////デシリアライズ処理を続行しないときも、標準設定を使用することを通知
                    //MessageBox.Show(this._printToFrame,
                    //    "プリンタ設定の読み込みに失敗しました、以前使用したプリンタが存在していない可能性があります。" +
                    //    "通常使用するプリンタを使用します。", this._printToFrame.Text, MessageBoxButtons.OK,
                    //    MessageBoxIcon.Information);

                    // K.Yamasako 20170615
                    // プリンタ設定の読み込みに失敗した場合は
                    // 通常使うプリンタではなく、再度選択させる
                    MessageBox.Show(
                        this._printToFrame,
                        "プリンタ設定の読み込みに失敗しました。\n" +
                        "以前使用したプリンタが存在していない可能性があります。\n" +
                        "再度、プリンタを選択してください。",
                        this._printToFrame.Text,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning
                        );

                    this.NoPrinterChoiceDialog = false;
                }
            }
            else
            {
                // K.Yamasako 20170616
                // 用紙がカスタムの場合に限らず、プリンタ設定ファイルがない場合は印刷ダイアログを表示


                ////用紙がカスタムだったら。。。
                //if (this._rptAR.PageSettings.PaperKind == PaperKind.Custom)
                //{
                //    //用紙がカスタムの場合、プリンタ設定ファイルが無い場合は保存するために、ダイアログを出す。
                //    MessageBox.Show(this._printToFrame, "本帳票は初回のプリンタ設定が必要です。",
                //        "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);

                using (ReportAppendix.PrinterChoiceDialog prdlg =
                    new ReportAppendix.PrinterChoiceDialog(wk_popupage, this._nullToPaperSizeOnPageSetupDialog))
                {
                    prdlg.AllowPrintRangeCopiesOption = false;
                    prdlg.SetOkButtonDisplayText("保存");

                    //プリンタ設定画面を開く
                    DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                    if (d_result == DialogResult.OK)
                    {
                        //--プリントダイアログから、印刷設定を取り出す
                        wk_popupage = prdlg.PopulatePageSetting;
                        wk_popuprinter = prdlg.PopulatePageSetting.PrinterSettings;

                        //印刷設定を保存する
                        wk_popupage.PrinterSettings = wk_popuprinter;

                        using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                        {
                            SoapFormatter formatter = new SoapFormatter();

                            //BinaryFormatter formatter = new BinaryFormatter();

                            formatter.Serialize(stream, wk_popupage);
                            stream.Close();
                        }

                        //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                        //カウンターをカウントアップ
                        this._printedInstructions =
                            this._printedInstructions + 1;
                    }
                    else
                    {
                        //OK以外のときは印刷させない
                        return;
                    }
                }
                //}
                
            }

            //印刷部数をセット
            wk_popupage.PrinterSettings.Copies = this._copies;

            //印刷先別で処理分けする
            switch (this._dest)
            {
                case ReportPrintDestType.PrintToPrinter:

                    #region 直接印刷

                    if (this.NoPrinterChoiceDialog)
                    {
                        #region プリンタ設定ダイアログを表示しない

                        //直接印刷
                        //2014/05/27 リダイレクトへの印刷で用紙サイズが行かないの対応策。
                        this._rptAR.PrinterSettingFromPageSettings(wk_popupage);

                        //--レポート実行
                        this._rptAR.Run(false);

                        //--印刷範囲はすべて
                        this._rptAR.Document.Printer.PrinterSettings.PrintRange = PrintRange.AllPages;

                        //--印刷開始
                        this._rptAR.Document.Print(false, false, false);

                        //印刷設定を保存する
                        wk_popupage.PrinterSettings = wk_popuprinter;

                        using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                        {
                            SoapFormatter formatter = new SoapFormatter();

                            //BinaryFormatter formatter = new BinaryFormatter();

                            formatter.Serialize(stream, wk_popupage);
                            stream.Close();
                        }

                        //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                        //カウンターをカウントアップ
                        this._printedInstructions =
                            this._printedInstructions + 1;

                        #endregion
                    }
                    else
                    {
                        #region プリンタ設定ダイアログを表示

                        //--プリンタ選択ダイアログを表示
                        using (ReportAppendix.PrinterChoiceDialog prdlg =
                            new ReportAppendix.PrinterChoiceDialog(wk_popupage, this._nullToPaperSizeOnPageSetupDialog))
                        {
                            prdlg.AllowPrintRangeCopiesOption = true;
                            DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                            if (d_result == DialogResult.OK)
                            {
                                //--プリントダイアログから、印刷設定を取り出す
                                wk_popupage = prdlg.PopulatePageSetting;
                                wk_popuprinter = prdlg.PopulatePageSetting.PrinterSettings;
                                
                                //直接印刷
                                //2014/05/27 リダイレクトへの印刷で用紙サイズが行かないの対応策。
                                this._rptAR.PrinterSettingFromPageSettings(wk_popupage);

                                //--レポート実行
                                this._rptAR.Run(false);

                                //--印刷範囲を指定
                                //--プリンタ選択ダイアログから、印刷範囲の指定の有無を確認する
                                int wk_pagefrom = 0;
                                int wk_pageto = 0;
                                if (prdlg.PrintRangeSelect)
                                {
                                    //開始と終了のページ番号を取得
                                    wk_pagefrom = Convert.ToInt32(prdlg.PrintRangeFrom);
                                    wk_pageto = Convert.ToInt32(prdlg.PrintRangeTo);

                                    //指定した複数ページを印刷するようにPrinterSettingsへ指定
                                    this._rptAR.Document.Printer.PrinterSettings.PrintRange = PrintRange.SomePages;

                                    //ページ指定
                                    this._rptAR.Document.Printer.PrinterSettings.FromPage = wk_pagefrom;
                                    this._rptAR.Document.Printer.PrinterSettings.ToPage = wk_pageto;
                                }
                                else
                                {

                                    //印刷範囲を指定しなかった場合は全部出力
                                    this._rptAR.Document.Printer.PrinterSettings.PrintRange = PrintRange.AllPages;
                                }

                                //--印刷開始
                                this._rptAR.Document.Print(false, false, false);

                                //印刷設定を保存する
                                wk_popupage.PrinterSettings = wk_popuprinter;

                                using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                                {
                                    SoapFormatter formatter = new SoapFormatter();

                                    //BinaryFormatter formatter = new BinaryFormatter();

                                    formatter.Serialize(stream, wk_popupage);
                                    stream.Close();
                                }

                                //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                                //カウンターをカウントアップ
                                this._printedInstructions =
                                    this._printedInstructions + 1;
                            }

                        }

                        #endregion
                    }
                    
                    #endregion

                    break;
                case ReportPrintDestType.PrintToScreen:

                    #region プレビューを作成

                    //プレビューを作成
                    //using (ReportPreviewARFrame f = new ReportPreviewARFrame(this._enablePrevButtons))
                    using (ReportPreviewARFrame f =
                        new ReportPreviewARFrame(this._enablePrevButtons
                            , this._nullToPaperSizeOnPageSetupDialog
                            , this._useDocumentDirectly
                            ))
                    {
                        f.InitReportPreviewARFrame(
                            this._printToFrame.Text,
                            this._rptAR,
                            wk_popuprinter,
                            wk_popupage);

                        f.ShowDialog(this._printToFrame);

                        //印刷設定を保存する
                        wk_popupage = f.PopulatePageSettings;
                        using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                        {
                            SoapFormatter formatter = new SoapFormatter();
                            formatter.Serialize(stream, wk_popupage);
                            stream.Close();
                        }

                        //印刷を指示した回数を取得する
                        this._printedInstructions = f.PrintedInstructions;

                    }

                    #endregion

                    break;
                default:
                    break;
            }

            //レポートオブジェクトをクローズする。
            if (this._rptAR != null)
            {
                //AR7は特に、おかたずけは必要なし。
            }
        }

        /// <summary>
        /// 保存されている印刷設定を読み込み、印刷設定ダイアログを表示します。
        /// </summary>
        /// <param name="nullToPaperSizeOnPageSetupDialog">ページ設定ダイアログに渡すPaperSizeの設定を無しとするかどうか。カスタムの用紙サイズが通常時にうまく動作しない場合にONにします。</param>
        public void ShowPrinterSettingDialog(bool nullToPaperSizeOnPageSetupDialog)
        {
            //this._printToFrame = printToFrame;
            //this._rptAR7 = rptAR7;

            this._nullToPaperSizeOnPageSetupDialog = nullToPaperSizeOnPageSetupDialog;

            //デフォルトのページ設定を作成
            PrintDocument pd = new PrintDocument();
            System.Drawing.Printing.PageSettings wk_popupage = pd.DefaultPageSettings;

            //デフォルトのページからプリンタ設定を取り出す
            PrinterSettings wk_popuprinter = wk_popupage.PrinterSettings;

            //　(FinePrint等で)Duplexに0が入っている時にToStringメソッドを実行する
            //　とArgumentExceptionが発生するため、強制的にSimplex(片面)に書き換える Y.Shinmura 20110422
            //　System.ArgumentException: 値 '0' は enum 'Duplex' の有効な値ではありません。
            if (wk_popuprinter.Duplex == 0)
            {
                wk_popuprinter.Duplex = Duplex.Simplex;
            }

            //プリンタ設定が保持している両面印刷設定値を変更する。
            //Duplex列挙体に値がない場合は、片面印刷とする。
            Duplex wk_popuduplex = Duplex.Default;
            switch (wk_popuprinter.Duplex)
            {
                case Duplex.Default:
                    wk_popuduplex = Duplex.Default;
                    break;
                case Duplex.Horizontal:
                    wk_popuduplex = Duplex.Horizontal;
                    break;
                case Duplex.Simplex:
                    wk_popuduplex = Duplex.Simplex;
                    break;
                case Duplex.Vertical:
                    wk_popuduplex = Duplex.Vertical;
                    break;
                default:
                    wk_popuduplex = Duplex.Simplex;
                    break;
            }

            //PageSettingsのPrinterSettingsにドキュメントから取得したPrinterSettingsを設定
            //　これで、給紙位置や用紙サイズが取れれば御の字
            wk_popupage.PrinterSettings = wk_popuprinter;


            //PageSettingsはすべてのプロパティを独自にドキュメントオブジェクトのPageSettingsからSystem..のPageSettingsへ入れる
            //--部単位の設定
            switch (this._rptAR.PageSettings.Collate)
            {
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.Collate:
                    //部単位
                    wk_popupage.PrinterSettings.Collate = true;
                    break;
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.Default:
                    //プリンタ標準なのでそのまま設定
                    wk_popupage.PrinterSettings.Collate = wk_popupage.PrinterSettings.Collate;
                    break;
                case GrapeCity.ActiveReports.PageSettings.PrinterCollate.DontCollate:
                    //非部単位
                    wk_popupage.PrinterSettings.Collate = false;
                    break;
                default:
                    break;
            }

            //--レポートをプリンタのデフォルト用紙サイズで印刷するかどうか
            if (!this._rptAR.PageSettings.DefaultPaperSize)
            {
                //--さらにユーザー定義サイズを使うかどうか判断して設定
                if (this._rptAR.PageSettings.PaperKind == PaperKind.Custom)
                {
                    //ユーザー定義サイズを使うよう設定
                    //--PaperSizeのインスタンスを作って設定
                    var wk_custom_ps = new PaperSize(this._rptAR.PageSettings.PaperName,
                        (int)(this._rptAR.PageSettings.PaperWidth * 100),
                        (int)(this._rptAR.PageSettings.PaperHeight * 100));
                    wk_popupage.PaperSize = wk_custom_ps;
                }
                else
                {
                    //ユーザー定義サイズでなければ普通にセット
                    wk_popupage.PaperSize.RawKind = (int)this._rptAR.PageSettings.PaperKind;
                }
            }
            else
            {
                //デフォルトであってもページは設定
                wk_popupage.PaperSize.RawKind = (int)this._rptAR.PageSettings.PaperKind;
            }
            //--レポートをプリンタのデフォルト給紙トレイで印刷するかどうか
            if (!this._rptAR.PageSettings.DefaultPaperSource)
            {
                //デフォルトではないときは、給紙トレイを設定する
                wk_popupage.PaperSource.RawKind = (int)this._rptAR.PageSettings.PaperSource;
            }
            else
            {
                //デフォルトのときは自分に自分を設定
                wk_popupage.PaperSource.RawKind = wk_popupage.PaperSource.RawKind;
            }
            //--両面印刷
            //両面はプリンタ設定から設定している（これより上のセクション）のでここでは設定しない
            //--以下のプロパティはSystem.Drawing.Printing.PageSettings および PrinterSettingsには
            //  設定値が無いので設定しない
            //  Gutter MirrorMargins

            //--ページ余白
            //--ARのMarginsはインチ単位、.NETのMarginsは1/100インチ単位なので、100倍する
            //---いったんレポートのMarginsの要素を取得
            var wk_arpt_mrg_bottom = this._rptAR.PageSettings.Margins.Bottom;
            var wk_arpt_mrg_left = this._rptAR.PageSettings.Margins.Left;
            var wk_arpt_mrg_right = this._rptAR.PageSettings.Margins.Right;
            var wk_arpt_mrg_top = this._rptAR.PageSettings.Margins.Top;
            wk_popupage.Margins.Bottom = (int)(wk_arpt_mrg_bottom * 100);
            wk_popupage.Margins.Left = (int)(wk_arpt_mrg_left * 100);
            wk_popupage.Margins.Right = (int)(wk_arpt_mrg_right * 100);
            wk_popupage.Margins.Top = (int)(wk_arpt_mrg_top * 100);

            //--ページの印刷方向
            switch (this._rptAR.PageSettings.Orientation)
            {
                case PageOrientation.Default:
                    //プリンタ標準なので何もしない
                    break;
                case PageOrientation.Landscape:
                    //横向きで印刷する
                    wk_popupage.Landscape = true;
                    break;
                case PageOrientation.Portrait:
                    //縦方向で印刷
                    wk_popupage.Landscape = false;
                    break;
                default:
                    break;
            }

            //--両面設定
            wk_popuprinter.Duplex = wk_popuduplex;


            //保存されたレポートの印刷設定を読み取って書き換える。
            //ない場合は、そのまま。
            //ディレクトリの存在チェック
            if (!Directory.Exists(SystemProperty.GetInstance().ReportPrinterSettingPath))
            {
                //存在しないときは作る
                Directory.CreateDirectory(SystemProperty.GetInstance().ReportPrinterSettingPath);
            }

            //--印刷設定ファイルのファイル名を作成する。レポート定義クラス名でファイル名を作る。
            string settingfile_nm =
                Path.Combine(
                    SystemProperty.GetInstance().ReportPrinterSettingPath,
                        this._rptAR.GetType().Name) + ".xml";

            //ファイルの存在チェック
            if (File.Exists(settingfile_nm))
            {
                //スコープ内変数定義
                //プリンタ設定保存ファイルのリダイレクトプリンタ対応処理中に、デシリアライズ処理を
                //続行するかどうかが判別可能なので、続行可能かどうかのフラグを用意しておく(true:続行可能）
                //こんなところにフラグを定義したくはないが・・時間切れ
                bool continue_deserialize = false;

                //存在していたらデシリアライズの前に、リダイレクトされたプリンタの判断して、
                //リダイレクトの番号が接続のたびに変わる対策をしておく

                //デシリアライズしたプリンタ設定のプリンタが現在の使用可能な
                //プリンタの一覧にそんざいしているか確認する
                //--OSの現在ログインから認識されているプリンタの一覧を取得
                IList<PrinterNameIndexItem> printer_list =
                    this.printerSettingHelper.GetPrinterNameIndexList();
                //--空のプリンタ一覧をリダイレクトプリンタの一覧格納用のリストにセット
                List<PrinterNameIndexItem> redirect_prt_list =
                    new List<PrinterNameIndexItem>();
                //--空のプリンタ一覧を通常プリンタの一覧格納用のリストにセット
                List<PrinterNameIndexItem> norediret_prt_list =
                    new List<PrinterNameIndexItem>();
                //--プリンタの一覧を取得をリダイレクト・通常に振り分ける
                foreach (PrinterNameIndexItem item in printer_list)
                {
                    if (item.PrinterNameString.LastIndexOf("(リダイレクト") == -1)
                    {
                        //リダイレクトされていないプリンタ
                        norediret_prt_list.Add(item);
                    }
                    else
                    {
                        //リダイレクトされている
                        redirect_prt_list.Add(item);
                    }
                }

                //シリアライズされたファイルをStreamで開いてプリンタ名だけ取得し、リダイレクトプリンタが
                //シリアライズされているのかをチェックして・・・
                //--設定ファイルの再保存のために１行分の文字列を保持する領域
                List<string> overwrite_list = new List<string>();

                //ファイルを開く
                using (System.IO.StreamReader sr = new System.IO.StreamReader(settingfile_nm))
                {

                    while (sr.Peek() > -1)
                    {
                        //一行読む
                        string wk_line = sr.ReadLine();

                        //プリンタ名を保持している行を探す
                        if (wk_line.IndexOf("<printerName") != -1)
                        {
                            //printerNameの開始タグと終了タグ、プリンタ名を分離して取り出しておく
                            //--最初に">"が出現したところまでの文字列を開始タグとする
                            string start_tag = wk_line.Substring(0, wk_line.IndexOf(">") + 1);
                            //--文字の最後尾からみて、最初に"</"が出現したところより前を取り除いた文字を終了タグとする
                            string end_tag = wk_line.Remove(0, wk_line.LastIndexOf("</"));
                            //--最初に">"が出現した位置の次の文字から、一行分の文字数から開始タグと終了タグの文字数を
                            //  減算した文字数分を取得してプリンタ名とする
                            string save_printer =
                                wk_line.Substring(wk_line.IndexOf(">") + 1,
                                    wk_line.Length - (start_tag.Length + end_tag.Length));
                            //--書き換えに使うプリンタ名を保持する領域、プリンタを見つけられなかったときのために、
                            //  読み込んだ時の値を設定しておく。
                            string replace_printer = save_printer;

                            //プリンタ名にリダイレクトプリンタと識別できる文字があるか確認する
                            int redirect_string_index = save_printer.LastIndexOf("(リダイレクト");
                            if (redirect_string_index != -1)
                            {
                                //リダイレクトされているプリンタの場合現在存在する同名のリダイレクト
                                //プリンタを探す
                                //--リダイレクトの文字をカットする
                                string edit_saved_printer =
                                    save_printer.Substring(0, redirect_string_index);
                                //--リダイレクトプリンタの一覧から該当するプリンタを探す
                                foreach (PrinterNameIndexItem item in redirect_prt_list)
                                {
                                    //リダイレクトプリンタの名前かリダイレクト判別文字を抜いておく
                                    string edit_redirect_printer =
                                        item.PrinterNameString.Substring(
                                            0, item.PrinterNameString.LastIndexOf("(リダイレクト"));
                                    //文字列比較
                                    if (edit_saved_printer.Trim() == edit_redirect_printer.Trim())
                                    {
                                        //同一の場合は、一致したリダイレクトプリンタのプリンタ名で書き換えるようにする
                                        //ので置き換え用の領域にプリンタ名を設定
                                        replace_printer = item.PrinterNameString;

                                        //デシリアライズの続行を可能としておく
                                        continue_deserialize = true;

                                        //ループを抜ける
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                //リダイレクトプリンタではない場合でも現在のOSが認識しているプリンタの一覧に
                                //保存されたプリンタが存在しているか確認をする
                                foreach (PrinterNameIndexItem item in norediret_prt_list)
                                {
                                    if (item.PrinterNameString.Trim() ==
                                        replace_printer.Trim())
                                    {
                                        //プリンタ名が一致しているので、デシリアライズの続行を可能としておく
                                        continue_deserialize = true;

                                        //ループを抜ける
                                        break;
                                    }
                                }
                            }

                            //書き換えたprinterNameとタグを結合して１行分を作る
                            StringBuilder sb_line = new StringBuilder();
                            sb_line.Append(start_tag);
                            sb_line.Append(replace_printer);
                            sb_line.Append(end_tag);

                            //保存用の１行分領域にセット
                            wk_line = sb_line.ToString();
                        }

                        //保存用のStringBuilderに保存しておく
                        overwrite_list.Add(wk_line);

                    }
                    sr.Close();
                }

                //編集の有無にかかわらず保存する
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(settingfile_nm))
                {
                    foreach (var item in overwrite_list)
                    {
                        sw.WriteLine(item);
                    }
                    sw.Close();
                }

                //デシリアライズ処理を続行するかどうか判別
                if (continue_deserialize)
                {
                    //再保存処理を行った後デシリアライズ処理を走らせる
                    //PageSettingsをデシリアライズ
                    using (Stream stream = File.Open(settingfile_nm, FileMode.Open))
                    {
                        try
                        {
                            System.Drawing.Printing.PageSettings deser_page =
                                (System.Drawing.Printing.PageSettings)(new SoapFormatter()).Deserialize(stream);
                            PrinterSettings deser_printer =
                                deser_page.PrinterSettings;

                            if (!deser_printer.IsValid)
                            {
                                throw new ApplicationException("有効でないプリンタとして、デシリアライズされています。");
                            }

                            //必要に応じて設定を書き換えたデシリアライズ済みの印刷設定値
                            //を内部変数に設定する。
                            wk_popupage = deser_page;
                            wk_popuprinter = deser_printer;
                        }
                        catch (Exception err)
                        {
                            //メッセージを表示して、標準設定を使用することを通知
                            MessageBox.Show(this._printToFrame,
                                "プリンタ設定の復元に失敗しました、以前使用したプリンタが存在していない可能性があります。" +
                                "通常使用するプリンタを使用します。", this._printToFrame.Text, MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                            //エラーはスローしないs
                            //throw err;
                        }
                        finally
                        {
                            //後かたずけ
                            stream.Close();
                        }
                    }
                }
                else
                {
                    //デシリアライズ処理を続行しないときも、標準設定を使用することを通知
                    MessageBox.Show(this._printToFrame,
                        "プリンタ設定の読み込みに失敗しました、以前使用したプリンタが存在していない可能性があります。" +
                        "通常使用するプリンタを使用します。", this._printToFrame.Text, MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                }
            }
            else
            {
                //用紙がカスタムだったら。。。
                if (this._rptAR.PageSettings.PaperKind == PaperKind.Custom)
                {
                    //用紙がカスタムの場合、プリンタ設定ファイルが無い場合は保存するために、ダイアログを出す。
                    MessageBox.Show(this._printToFrame, "本帳票は初回のプリンタ設定が必要です。",
                        "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //using (PrinterChoiceDialogCR2K8 prdlg = new PrinterChoiceDialogCR2K8(wk_popupage))
                    using (ReportAppendix.PrinterChoiceDialog prdlg =
                        new ReportAppendix.PrinterChoiceDialog(wk_popupage, this._nullToPaperSizeOnPageSetupDialog))
                    {
                        prdlg.AllowPrintRangeCopiesOption = false;
                        prdlg.SetOkButtonDisplayText("保存");

                        //プリンタ設定画面を開く
                        DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                        if (d_result == DialogResult.OK)
                        {
                            //--プリントダイアログから、印刷設定を取り出す
                            wk_popupage = prdlg.PopulatePageSetting;
                            wk_popuprinter = prdlg.PopulatePageSetting.PrinterSettings;

                            //印刷設定を保存する
                            wk_popupage.PrinterSettings = wk_popuprinter;

                            using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                            {
                                SoapFormatter formatter = new SoapFormatter();

                                //BinaryFormatter formatter = new BinaryFormatter();

                                formatter.Serialize(stream, wk_popupage);
                                stream.Close();
                            }

                            //プリンタ選択ダイアログからの印刷処理が正常に終わった時点で
                            //カウンターをカウントアップ
                            this._printedInstructions =
                                this._printedInstructions + 1;
                        }
                        else
                        {
                            //OK以外のときは印刷させない
                            return;
                        }
                    }
                }
            }

            //--プリンタ選択ダイアログを表示
            //using (PrinterChoiceDialogCR2K8 prdlg = new PrinterChoiceDialogCR2K8(wk_popupage))
            using (ReportAppendix.PrinterChoiceDialog prdlg =
                new ReportAppendix.PrinterChoiceDialog(wk_popupage, this._nullToPaperSizeOnPageSetupDialog))
            {
                prdlg.AllowPrintRangeCopiesOption = true;
                DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                if (d_result == DialogResult.OK)
                {
                    //--プリントダイアログから、印刷設定を取り出す
                    wk_popupage = prdlg.PopulatePageSetting;
                    wk_popuprinter = prdlg.PopulatePageSetting.PrinterSettings;

                    //印刷設定を保存する
                    wk_popupage.PrinterSettings = wk_popuprinter;

                    using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                    {
                        SoapFormatter formatter = new SoapFormatter();

                        //BinaryFormatter formatter = new BinaryFormatter();

                        formatter.Serialize(stream, wk_popupage);
                        stream.Close();
                    }

                }

            }

        }

        #region プロパティ

        /// <summary>
        /// プレビュー画面にて印刷指示をした回数を取得します。
        /// 直接印刷の時および、プレビュー画面にて印刷指示を行わなかった場合には
        /// 0になります（読み取り専用）
        /// </summary>
        public int PrintedInstructions
        {
            get { return this._printedInstructions; }
        }

        /// <summary>
        /// プリンタ選択ダイアログを表示せずに、既存の印刷設定を使用して印刷するか
        /// </summary>
        public bool NoPrinterChoiceDialog { get; set; }

        #endregion

        private GrapeCity.ActiveReports.SectionReport _rptAR;

    }
}
