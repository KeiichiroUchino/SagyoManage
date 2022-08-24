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
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.ReportAppServer.ClientDoc;
using jp.co.jpsys.util.print;
using Jpsys.SagyoManage.Property;
using Jpsys.SagyoManage.FrameLib;

namespace Jpsys.SagyoManage.ReportFrame
{
    public class ReportPrintingControl
    {
        /// <summary>
        /// CrystalReportのReportClassを使用した印刷関連の操作を行う
        /// メソッドおよびプロパティを提供するクラスです。
        /// </summary>
        private Form _printToFrame;
        private ReportClass _rpt;
        //private PageSettings _page;
        private FrameLib.ReportPrintDestType _dest;
        private ReportPreviewFrame.PreviewFramesButton _enablePrevButtons;

        //印刷を指示した回数
        private int _printedInstructions = 0;

        /// <summary>
        /// プリンタ設定のヘルパークラスを保持
        /// </summary>
        private NSKPrinterSettingHelper printerSettingHelper = new NSKPrinterSettingHelper();

        /// <summary>
        /// 本クラスのデフォルトコンストラクタは
        /// プライベート化
        /// </summary>
        private ReportPrintingControl()
        { }

        /// <summary>
        /// 帳票の印刷指示を行ったWindowsForm及びCrystalReportのレポート
        /// オブジェクト、印刷先を表す列挙体を指定して、本クラスのインスタンスを
        /// 初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rpt">CrystalReportのレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        public ReportPrintingControl(Form printToFrame, ReportClass rpt,
            FrameLib.ReportPrintDestType dest)
        {
            this._printToFrame = printToFrame;
            this._rpt = rpt;
            this._dest = dest;

            //追加 T.Kuroki 20090929 プレビュー画面のボタン制御をおこなうため
            //--プレビュー画面で使用するボタンの設定が渡されないときには全てを使用するように
            this._enablePrevButtons =
                ReportPreviewFrame.PreviewFramesButton.Print |
                ReportPreviewFrame.PreviewFramesButton.MoveFirst |
                ReportPreviewFrame.PreviewFramesButton.MovePrev |
                ReportPreviewFrame.PreviewFramesButton.MoveNext |
                ReportPreviewFrame.PreviewFramesButton.MoveLast |
                ReportPreviewFrame.PreviewFramesButton.Zoom |
                ReportPreviewFrame.PreviewFramesButton.Export;

        }

        //追加 T.Kuroki 20090929 プレビュー画面のボタン制御をおこなうため
        /// <summary>
        /// 帳票の印刷指示を行ったWindowsForm及びCrystalReportのレポート
        /// オブジェクト、印刷先を表す列挙体を、プレビュー画面で使用するボタンを
        /// 表す列挙体を指定して、本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="printToFrame">帳票の印刷指示を行ったWindowsForm</param>
        /// <param name="rpt">CrystalReportのレポートオブジェクト</param>
        /// <param name="dest">印刷先を表す列挙体</param>
        /// <param name="enablePrevButtons">プレビュー画面で使用するボタンを表す列挙体</param>
        public ReportPrintingControl(Form printToFrame, ReportClass rpt,
            FrameLib.ReportPrintDestType dest,
            ReportPreviewFrame.PreviewFramesButton enablePrevButtons)
        {
            this._printToFrame = printToFrame;
            this._rpt = rpt;
            this._dest = dest;

            //追加 T.Kuroki 20090929 プレビュー画面のボタン制御をおこなうため
            this._enablePrevButtons = enablePrevButtons;
        }



        /// <summary>
        /// インスタンス作成時に設定された値を元に、印刷処理を実行します。
        /// </summary>
        public void RunPrintProcess()
        {
            //FrameUtilites.DebugLogWriter("RunPrintProcess", this._printToFrame.Name + ":RunPrintProcess開始");

            //FrameUtilites.DebugLogWriter("RunPrintProcess", "デフォルトのページ設定を作成");

            //デフォルトのページ設定を作成
            PrintDocument pd = new PrintDocument();
            PageSettings wk_popupage = pd.DefaultPageSettings;


            //FrameUtilites.DebugLogWriter("RunPrintProcess", wk_popupage.ToString());

            //FrameUtilites.DebugLogWriter("RunPrintProcess", "デフォルトのページ設定からデフォルトプリンタを取得");

            //デフォルトのページからプリンタ設定を取り出す
            PrinterSettings wk_popuprinter = wk_popupage.PrinterSettings;

            //FrameUtilites.DebugLogWriter("RunPrintProcess", wk_popuprinter.ToString());


            //クリレポから印刷設定をコピーする前に、プリンタに設定されている
            //プロパティは取得しておく。
            //--両面印刷設定
            
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

            //FrameUtilites.DebugLogWriter("RunPrintProcess", "レポートのPrintOptionから印刷設定をPrinterSetingsとPageSettingsにコピー");

            //レポートのPrintOptionから印刷設定をPrinterSetingsとPageSettingsにコピーする。
            this._rpt.PrintOptions.CopyTo(wk_popuprinter, wk_popupage);
            //CristalReportsのCopyToメソッドの実行後はプリンタ設定を再設定
            //※再設定しないと、シリアライズに失敗するため
            wk_popuprinter.PrinterName = wk_popuprinter.PrinterName;
            wk_popupage.PaperSource = wk_popupage.PaperSource;
            wk_popupage.PaperSize = wk_popupage.PaperSize;
            wk_popupage.PrinterResolution = wk_popupage.PrinterResolution;



            //一旦取得してきた設定は書き換える。
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


            //--印刷設定ファイルのファイル名を作成する
            string settingfile_nm =
                Path.Combine(
                    SystemProperty.GetInstance().ReportPrinterSettingPath,
                        this._rpt.GetType().Name) + ".xml";

            
            //***プリンタ選択ダイアログを出さない場合は、初回（印刷設定がない）ときだけ、
            //***印刷設定を作るためにプリンタ設定ダイアログを表示する
            if (this.NoPrinterChoiceDialog)
            {
                #region 印刷設定がなければ印刷ダイアログを表示する

                //ファイルが存在しないかチェック
                if (!System.IO.File.Exists(settingfile_nm))
                {
                    //デフォルトのページ設定を作成
                    PrintDocument prindDocument = new PrintDocument();
                    PageSettings pageSettings = prindDocument.DefaultPageSettings;
                    PrinterSettings printerSettings = pageSettings.PrinterSettings;

                    try
                    {
                        //レポートオブジェクトの印刷設定をコピー
                        this._rpt.PrintOptions.CopyTo(printerSettings, pageSettings);
                    }
                    catch (Exception ex)
                    {
                        //エラーメッセージを表示して、処理を続行する
                        MessageBox.Show(this._printToFrame, ex.ToString());
                    }

                    using (ReportAppendix.PrinterChoiceDialog prdlg =
                        new ReportAppendix.PrinterChoiceDialog(pageSettings, false))
                    {
                        //印刷範囲の選択を許可する。 
                        prdlg.AllowPrintRangeCopiesOption = true;
                        //余白の設定を禁止する。
                        prdlg.AllowMargins = false;
                        //印刷情報の設定を禁止する。
                        prdlg.AllowOrientation = false;
                        //ダイアログ表示
                        DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                        //印刷設定を作成しない場合は処理しない
                        if (d_result != DialogResult.OK)
                        {
                            return;
                        }

                        using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                        {
                            SoapFormatter formatter = new SoapFormatter();
                            formatter.Serialize(stream, pageSettings);
                            stream.Close();
                        }
                    }
                }

                #endregion
            }

            //ファイルの存在チェック
            if (File.Exists(settingfile_nm))
            {
                //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定ファイルが存在");

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

                ////Debug T.Kuroki 20110714
                ////FrameUtilites.DebugLogWriter("RunPrintProcess", "リダイレクトプリンタの判断・設定ファイル書き換え処理開始");
                ////シリアライズされたファイルをStreamで開いてプリンタ名だけ取得し、リダイレクトプリンタが
                ////シリアライズされているのかをチェックして・・・
                ////--設定ファイルの再保存のために１行分の文字列を保持する領域
                //List<string> overwrite_list = new List<string>();

                ////ファイルを開く
                //using (System.IO.StreamReader sr = new System.IO.StreamReader(settingfile_nm))
                //{

                //    while (sr.Peek() > -1)
                //    {
                //        //一行読む
                //        string wk_line = sr.ReadLine();

                //        //プリンタ名を保持している行を探す
                //        if (wk_line.IndexOf("<printerName") != -1)
                //        {
                //            //Debug T.Kuroki 20110714
                //            //FrameUtilites.DebugLogWriter("RunPrintProcess", "printerNameを発見");

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
                //                //Debug T.Kuroki 20110714
                //                //FrameUtilites.DebugLogWriter("RunPrintProcess", "リダイレクトされたプリンタが保存されている。:" + save_printer);

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
                //                        //Debug T.Kuroki 20110714
                //                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "保存されているリダイレクトプリンタの置き換え対象発見:" + item.PrinterNameString);

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
                //                //Debug T.Kuroki 20110714
                //                //FrameUtilites.DebugLogWriter("RunPrintProcess", "保存されているのはリダイレクトではない");

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
                            //FrameUtilites.DebugLogWriter("RunPrintProcess", "PageSettingsをデシリアライズ");

                            PageSettings deser_page =
                                (PageSettings)(new SoapFormatter()).Deserialize(stream);
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
                            
                            //FrameUtilites.DebugLogWriter("RunPrintProcess", "デシリアライズ済み：" + wk_popuprinter.ToString());

                        }
                        catch (Exception)
                        {
                            //FrameUtilites.DebugLogWriter("RunPrintProcess", "デシリアライズ時に例外発生:" + err.Message);

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

            //印刷先別で処理分けする
            switch (this._dest)
            {
                case ReportPrintDestType.PrintToPrinter:

                    if (this.NoPrinterChoiceDialog)
                    {
                        #region 直接印刷（ダイアログなし）

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "直接印刷に分岐（ダイアログなし）");

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "直接印刷開始");

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定をReportに設定");
                        
                        //--印刷設定をReportに設定（1回目）
                        this._rpt.PrintOptions.CopyFrom(wk_popuprinter,
                            wk_popupage);

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定");
                        
                        //--CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定する。
                        this._rpt.PrintOptions.PrinterName = wk_popuprinter.PrinterName;

                        //Michibata 2014.03.06 add
                        //プリンタ名が設定されないため、GetPrintOptions().PrinterNameに再セット
                        ISCDReportClientDocument _rcd1;
                        _rcd1 = this._rpt.ReportClientDocument;
                        _rcd1.PrintOutputController.GetPrintOptions().PrinterName = wk_popuprinter.PrinterName;

                        //--再度、印刷設定をReportに設定　※2回目のCopyFromでカスタムの用紙が正常に適用される。
                        //                              1回目だとPrinterNameがクリアされてデフォルトプリンタに出力される。
                        this._rpt.PrintOptions.CopyFrom(wk_popuprinter, wk_popupage);

                        //--直接印刷
                        this._rpt.PrintToPrinter(
                            1,
                            false,
                            0,
                            0);

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定を保存");
                        
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
                        #region 直接印刷（ダイアログあり）

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "直接印刷に分岐");
                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "プリンタ選択ダイアログインスタンス化");
                        
                        //直接印刷
                        //--プリンタ選択ダイアログを表示
                        using (PrinterChoiceDialogCR2K8 prdlg = new PrinterChoiceDialogCR2K8(wk_popupage))
                        {
                            prdlg.AllowPrintRangeCopiesOption = true;

                            //FrameUtilites.DebugLogWriter("RunPrintProcess", "プリンタ選択ダイアロ表示開始");
                            DialogResult d_result = prdlg.ShowDialog(this._printToFrame);

                            if (d_result == DialogResult.OK)
                            {
                                //--プリントダイアログから、印刷設定を取り出す
                                wk_popupage = prdlg.PopulatePageSetting;
                                wk_popuprinter = prdlg.PopulatePageSetting.PrinterSettings;

                                //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定をReportに設定");
                                
                                //--印刷設定をReportに設定（1回目）
                                this._rpt.PrintOptions.CopyFrom(wk_popuprinter,
                                    wk_popupage);

                                //FrameUtilites.DebugLogWriter("RunPrintProcess", "CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定");
                                
                                //--CrystalReportのPrintOptionsの各プロパティに印刷設定を直接設定する。
                                this._rpt.PrintOptions.PrinterName = wk_popuprinter.PrinterName;

                                //Michibata 2014.03.06 add
                                //プリンタ名が設定されないため、GetPrintOptions().PrinterNameに再セット
                                ISCDReportClientDocument _rcd1;
                                _rcd1 = this._rpt.ReportClientDocument;
                                _rcd1.PrintOutputController.GetPrintOptions().PrinterName = wk_popuprinter.PrinterName;

                                //--再度、印刷設定をReportに設定　※2回目のCopyFromでカスタムの用紙が正常に適用される。
                                //                              1回目だとPrinterNameがクリアされてデフォルトプリンタに出力される。
                                this._rpt.PrintOptions.CopyFrom(wk_popuprinter, wk_popupage);

                                if (wk_popupage.Landscape)
                                {
                                    this._rpt.PrintOptions.PaperOrientation =
                                        PaperOrientation.Landscape;
                                }
                                else
                                {
                                    this._rpt.PrintOptions.PaperOrientation =
                                        PaperOrientation.Portrait;
                                }

                                //--プリンタ選択ダイアログから、印刷範囲の指定の有無を確認する
                                int wk_pagefrom = 0;
                                int wk_pageto = 0;
                                if (prdlg.PrintRangeSelect)
                                {
                                    wk_pagefrom = Convert.ToInt32(prdlg.PrintRangeFrom);
                                    wk_pageto = Convert.ToInt32(prdlg.PrintRangeTo);
                                }


                                //FrameUtilites.DebugLogWriter("RunPrintProcess", "直接印刷開始");
                                
                                //--直接印刷
                                this._rpt.PrintToPrinter(
                                    Convert.ToInt32(prdlg.PageCopies),
                                    prdlg.PrintCollated,
                                    wk_pagefrom,
                                    wk_pageto);

                                //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定を保存");
                                
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
                    
                    break;

                case ReportPrintDestType.PrintToScreen:
                    //FrameUtilites.DebugLogWriter("RunPrintProcess", "プレビュー表示に分岐");

                    //プレビューを作成
                    //--修正 T.Kuroki 20090929 プレビュー画面のボタン制御のため修正
                    //using (ReportPreviewFrame f = new ReportPreviewFrame())
                    using (ReportPreviewFrame f = new ReportPreviewFrame(this._enablePrevButtons))
                    {
                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "プレビューダイアログ初期化");
                        
                        f.InitReportPreviewFrame(
                            this._printToFrame.Text,
                            this._rpt,
                            wk_popuprinter,
                            wk_popupage);
                        
                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "プレビューダイアログ表示");
                        
                        f.ShowDialog(this._printToFrame);

                        //FrameUtilites.DebugLogWriter("RunPrintProcess", "印刷設定を保存");
                        
                        //印刷設定を保存する
                        wk_popupage = f.PopulatePageSettings;
                        using (Stream stream = File.Open(settingfile_nm, FileMode.Create))
                        {
                            SoapFormatter formatter = new SoapFormatter();

                            //BinaryFormatter formatter = new BinaryFormatter();

                            formatter.Serialize(stream, wk_popupage);
                            stream.Close();
                        }

                        //印刷を指示した回数を取得する
                        this._printedInstructions = f.PrintedInstructions;

                    }
                    break;
                default:
                    break;
            }

            //レポートオブジェクトをクローズする。
            if (this._rpt != null)
            {
                //FrameUtilites.DebugLogWriter("RunPrintProcess", "レポートオブジェクトをクローズ");
                this._rpt.Close();
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
    }
}
