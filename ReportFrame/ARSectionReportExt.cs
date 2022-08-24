using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Printing;
using GrapeCity.ActiveReports;
using GrapeCity.ActiveReports.Document.Section;

namespace Jpsys.HaishaManageV10.ReportFrame
{
    public static class ARSectionReportExt
    {
        /// <summary>
        /// System.Drawing.Printing.PageSettingsを指定して印刷設定を行う
        /// </summary>
        public static void PrinterSettingFromPageSettings(this SectionReport sr, System.Drawing.Printing.PageSettings ps)
        {
            sr.Document.Printer.PrinterName = ps.PrinterSettings.PrinterName;
            sr.PageSettings.DefaultPaperSource = false;
            sr.PageSettings.PaperSource = ps.PaperSource.Kind;
            sr.Document.Printer.DefaultPageSettings.PaperSource = ps.PaperSource;

            sr.Document.Printer.PaperSize = ps.PaperSize;

            sr.PageSettings.DefaultPaperSize = false;
            if (ps.PaperSize.Kind == PaperKind.Custom)
            {
                sr.PageSettings.PaperKind = PaperKind.Custom;
                // インチで指定
                //sr.PageSettings.PaperWidth = ps.PaperSize.Width;
                //sr.PageSettings.PaperWidth = ps.PaperSize.Height;
                sr.PageSettings.PaperWidth = Convert.ToSingle(ps.PaperSize.Width / 100);
                sr.PageSettings.PaperHeight = Convert.ToSingle(ps.PaperSize.Height / 100);

                if (sr.Document.Printer.PaperKind != PaperKind.Custom)
                {
                    sr.Document.Printer.PaperKind = PaperKind.Custom; //遅い!
                }

                System.Drawing.Printing.PaperSize size =
                    new PaperSize(ps.PaperSize.PaperName, ps.PaperSize.Width, ps.PaperSize.Height);

                sr.Document.Printer.PaperSize = size;

                //カスタムのときは、用紙名を設定
                sr.PageSettings.PaperName = ps.PaperSize.PaperName;


            }
            else
            {
                sr.PageSettings.PaperKind = ps.PaperSize.Kind;
                sr.Document.Printer.PaperKind = ps.PaperSize.Kind;
            }

            sr.Document.Printer.PrinterSettings.Copies = ps.PrinterSettings.Copies;
            sr.PageSettings.Duplex = ps.PrinterSettings.Duplex;
            sr.Document.Printer.PrinterSettings.Duplex = ps.PrinterSettings.Duplex;

            sr.PageSettings.Margins.Top = Convert.ToSingle(ps.Margins.Top) / 100;
            sr.PageSettings.Margins.Bottom = Convert.ToSingle(ps.Margins.Bottom) / 100;
            sr.PageSettings.Margins.Left = Convert.ToSingle(ps.Margins.Left) / 100;
            sr.PageSettings.Margins.Right = Convert.ToSingle(ps.Margins.Right) / 100;

        }

    }
}
