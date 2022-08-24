using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.FrameLib;

namespace Jpsys.HaishaManageV10.VBReport
{
    public partial class ReportPreviewVBFrame : Form
    {
        #region ユーザー定義

        private string _title;

        private CellReport _cellReport;

        private ReportPrintDestType _printDestType;

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのインスタンスを初期化します。
        /// </summary>
        private ReportPreviewVBFrame()
        {
            InitializeComponent();
        }

        /// <summary>
        /// レポートオブジェクトを指定して本クラスのインスタンスを初期化します。
        /// </summary>
        /// <param name="title">画面のタイトル</param>
        /// <param name="cellReport">VBReportのレポートオブジェクト</param>
        public ReportPreviewVBFrame(string title, CellReport cellReport,ReportPrintDestType printDestType)
        {
            InitializeComponent();

            _title = title;
            _cellReport = cellReport;
            _printDestType = printDestType;



            this.InitReportPreviewVBFrame();
        }

        #endregion

        #region 初期化処理

        /// <summary>
        /// 本画面を初期化します。
        /// </summary>
        private void InitReportPreviewVBFrame()
        {
            viewerControl1.Clear();
            viewerControl1.Document = _cellReport.Document;

            // 保存時のファイル名を設定します。
            viewerControl1.SaveFileName = 
                string.Format("{0}_{1}",_title , DateTime.Today.ToString("yyyyMMdd"));

            if (Type.GetTypeFromProgID("Excel.Application") == null)
            {
                //環境に Excel アプリケーションがインストールされていない場合は、保存ボタンは、無効にします。
                viewerControl1.EnableSaveButton = false;
            }

            switch (_printDestType)
            {
                case ReportPrintDestType.PrintToPrinter:
                    this.Text = _title + " 印刷中・・・";
                    // PrintOut メソッドで印刷します。
                    viewerControl1.PrintOut();
                    break;
                case ReportPrintDestType.PrintToScreen:
                    this.Text = _title + " プレビュー中・・・";
                    break;
                case ReportPrintDestType.PrintToFax:
                    break;
                case ReportPrintDestType.ExportExcel:
                    break;
                default:
                    break;
            }
        }

        #endregion
    }
}
