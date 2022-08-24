using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.SQLServerDAL;

namespace Jpsys.HaishaManageV10.VBReport
{
    /// <summary>
    /// 傭車依頼書のExcelファイルを作成します。
    /// </summary>
    public class YoshaIraisho
    {
        #region ユーザー定義

        #region 傭車依頼書関係

        /// <summary>
        /// シート名_傭車依頼書
        /// </summary>
        private string SHEET_NAME_YoshaIraisho = "傭車依頼書";

        /// <summary>
        /// 詳細部の開始行（1～）
        /// </summary>
        private const int SHOSAI_START_ROW = 11;

        /// <summary>
        /// 行の高さ
        /// </summary>
        private const double ROW_HEIGHT = (double)(16.50);

        /// <summary>
        /// 傭車依頼書の１ページのMAX行数(18行)
        /// </summary>
        private const int MAX_ROWS = 18;

        /// <summary>
        /// テンプレートファイル
        /// </summary>
        private static readonly byte[] TEMPLATE_FILE
               = global::Jpsys.HaishaManageV10.VBReport.Properties.Resources.YoshaIraisho;

        /// <summary>
        /// 使用するページ
        /// </summary>
        private const string PAGE_INFO = "1-99999";

        #endregion

        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo authInfo =
            new AppAuthInfo
            {
                OperatorId = 0,
                TerminalId = "",
                UserProcessId = "",
                UserProcessName = ""
            };

        #endregion

        #region コンストラクタ

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public YoshaIraisho()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、インスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public YoshaIraisho(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// 傭車依頼書のExcelファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="info">傭車依頼書情報</param>
        /// <param name="searchparameter">傭車依頼書（印刷条件）</param>
        /// <returns>傭車依頼書のExcelファイル</returns>
        public CellReport YoshaIraishoData(CellReport vbrList, List<YoshaIraishoPrtInfo> info, YoshaIraishoPrtSearchParameter searchparameter)
        {
            // レポートファイルを作成
            return this.CreateReportList(vbrList, info, searchparameter);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// レポートファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="list">傭車依頼書情報</param>
        /// <returns>レポートファイル</returns>
        private CellReport CreateReportList(CellReport vbrList, List<YoshaIraishoPrtInfo> info, YoshaIraishoPrtSearchParameter searchparameter)
        {
            //【1】===========================================================//
            // 帳票作成に必要な開始処理
            //================================================================//
            vbrList.Report.Embed(TEMPLATE_FILE);

            //【2】===========================================================//
            // Page.Startでデザインファイルのテンプレート(シート)を指定し、値の
            // 設定を行います。値の設定後、Page.Endでページ処理を終了します。
            // 　[2-VB-Reportの機能]-[2-帳票ページ処理]-[Page.Start]
            //================================================================//

            //管理情報取得
            KanriInfo kanriInfo = new Kanri().GetInfo();

            //出力日
            String outputDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // ページ処理の開始
            vbrList.Page.Start(SHEET_NAME_YoshaIraisho, PAGE_INFO);

            var carList = info
                .OrderBy(x => x.TorihikiCode)
                .ThenBy(x => x.LicPlateCarNo)
                .GroupBy(x => new { CarOfChartererId = x.CarOfChartererId, LicPlateCarNo = x.LicPlateCarNo });

            // 総ページ数
            int totalPageCount = 0;

            foreach (var group in carList)
            {
                // ヘッダ情報 --------------------------------

                YoshaIraishoPrtInfo wkInfo = group.First();

                //改ページ
                vbrList.Page.Next(true);
                vbrList.Page.Repeat(1);

                // 会社情報設定
                this.SetKanriInfo(vbrList, kanriInfo);

                // ヘッダ情報設定
                this.CreateRepotHeader(vbrList, wkInfo, outputDateTime, searchparameter);

                // 詳細情報 -----------------------------------
                List<YoshaIraishoDetailsInfo> YoshaIraishoDetailsList = new List<YoshaIraishoDetailsInfo>();

                // 受注単位でグループ化
                var juchuList = group.GroupBy(x => new { JuchuId = x.JuchuId });

                foreach (var juchuItem in juchuList)
                {
                    // ソート用積載日
                    YoshaIraishoPrtInfo firstItem = juchuItem.First();

                    foreach (YoshaIraishoPrtInfo item in juchuItem)
                    {
                        YoshaIraishoDetailsInfo inStartDate = new YoshaIraishoDetailsInfo();
                        YoshaIraishoDetailsInfo inEndDate = new YoshaIraishoDetailsInfo();

                        // 共通項目
                        CreateCommonItem(item, ref inStartDate, ref inEndDate);

                        // 出発情報
                        inStartDate.Kbn = "積日";
                        inStartDate.TaskYMD = item.TaskStartDateTime.ToString("yyyy/MM/dd");
                        inStartDate.TaskHM = item.TaskStartDateTime.ToString("HH:mm");
                        inStartDate.PointName = item.StartPointName;
                        inStartDate.Address = item.StartPointAddress1 + item.StartPointAddress2;
                        inStartDate.Tel = item.StartPointTel;
                        inStartDate.LicPlateCarNo = item.LicPlateCarNo;
                        inStartDate.Biko = item.Biko;
                        inStartDate.MagoYoshasaki = item.MagoYoshasaki;

                        // ソート情報
                        inStartDate.JuchuId = item.JuchuId;
                        inStartDate.KbnInt = 1;

                        // 積載日時
                        decimal sortSekisaiKey = Convert.ToDecimal(firstItem.TaskStartDateTime.ToString("yyyyMMdd")
                            + firstItem.TaskStartDateTime.ToString("HHmmss"));
                        inStartDate.JuchuMinSekisaiYMDSort = sortSekisaiKey;

                        // 着日
                        decimal sortChakuKey = Convert.ToDecimal(firstItem.TaskEndDateTime.ToString("yyyyMMddHHmm"));
                        inStartDate.JuchuMinChakuYMDSort = sortChakuKey;

                        inStartDate.TaskYMDSort = item.TaskStartDateTime.Date;
                        inStartDate.TaskHMSort = Convert.ToInt32(item.TaskStartDateTime.ToString("HHmmss"));
                        YoshaIraishoDetailsList.Add(inStartDate);

                        // 到着情報
                        inEndDate.Kbn = "卸日";
                        inEndDate.TaskYMD = item.TaskEndDateTime.ToString("yyyy/MM/dd");
                        inEndDate.TaskHM = item.TaskEndDateTime.ToString("HH:mm");
                        inEndDate.PointName = item.EndPointName;
                        inEndDate.Address = item.EndPointAddress1 + item.EndPointAddress2;
                        inEndDate.Tel = item.EndPointTel;

                        // ソート情報
                        inEndDate.JuchuId = item.JuchuId;
                        inEndDate.KbnInt = 2;
                        inEndDate.JuchuMinSekisaiYMDSort = sortSekisaiKey;
                        inEndDate.JuchuMinChakuYMDSort = sortChakuKey;
                        inEndDate.TaskYMDSort = new DateTime(
                            item.TaskEndDateTime.Year
                            , item.TaskEndDateTime.Month
                            , item.TaskEndDateTime.Day
                            , 00
                            , 00
                            , 00);
                        string hmsEnd = string.Concat(item.TaskEndDateTime.Hour.ToString("00"),
                            item.TaskEndDateTime.Minute.ToString("00"),
                            item.TaskEndDateTime.Second.ToString("00"));
                        inEndDate.TaskHMSort = Convert.ToInt32(hmsEnd);
                        inEndDate.LicPlateCarNo = string.Empty;
                        inEndDate.Biko = string.Empty;
                        YoshaIraishoDetailsList.Add(inEndDate);
                    }
                }

                // 受注ごとの最初の積載日＋積載時間区分値、着日、受注ID、発着日、発着時間、積卸区分でソート
                var list = YoshaIraishoDetailsList
                    .OrderBy(x => x.JuchuMinSekisaiYMDSort)
                    .ThenBy(x => x.JuchuMinChakuYMDSort)
                    .ThenBy(x => x.JuchuId)
                    .ThenBy(x => x.TaskYMDSort)
                    .ThenBy(x => x.TaskHMSort)
                    .ThenBy(x => x.KbnInt);

                // ページ数
                totalPageCount++;
                vbrList.Cell("C36").Value = totalPageCount;

                // グループ内総ページ数取得
                int groupTotalPageCount = (int)Math.Ceiling((double)list.Count() / (MAX_ROWS / 2));

                // グループ内総ページ数
                int groupPageCount = 0;

                // 傭車先毎総ページ数
                groupPageCount++;
                vbrList.Cell("AP1").Value = groupPageCount.ToString() + " / " + groupTotalPageCount;

                // 表示行カウント
                int row = 0;
                // 明細カウント
                int detailsCount = 0;
                // ページカウント
                int page = 1;
                // 区分
                int intKbn = 0;
                // 日付
                string strYmd = string.Empty;
                // 時間
                string strHm = string.Empty;
                // 受注ID
                decimal juchuId = decimal.Zero;

                foreach (YoshaIraishoDetailsInfo item in list)
                {
                    if (row > 0)
                    {
                        // セルコピー
                        vbrList.Cell("A" + SHOSAI_START_ROW + ":AT" + (SHOSAI_START_ROW + 1)).Copy("A" + (SHOSAI_START_ROW + row).ToString());
                        // 行高を設定
                        vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;

                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":AT" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
                    }

                    // 区分　罫線
                    if (intKbn == item.KbnInt)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":B" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**Kbn", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**Kbn", 0, row).Value = item.Kbn;
                    }

                    // 日付　罫線
                    if (strYmd.Equals(item.TaskYMD) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("C" + (SHOSAI_START_ROW + row) + ":F" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**Ymd", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**Ymd", 0, row).Value = item.TaskYMD;
                    }

                    // 時間　罫線
                    if (strHm.Equals(item.TaskHM) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("G" + (SHOSAI_START_ROW + row) + ":H" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**Time", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**Time", 0, row).Value = item.TaskHM;
                    }

                    vbrList.Cell("**PointNM", 0, row).Value = item.PointName;
                    vbrList.Cell("**Address", 0, row).Value = item.Address;
                    vbrList.Cell("**ItemNM", 0, row).Value = item.ItemName;
                    vbrList.Cell("**Number", 0, row).Value = item.Number;
                    vbrList.Cell("**FigNM", 0, row).Value = item.FigName;
                    vbrList.Cell("**Weight", 0, row).Value = item.Weight ?? string.Empty;
                    vbrList.Cell("**Tel", 0, row).Value = item.Tel;
                    vbrList.Cell("**TokuisakiNM", 0, row).Value = item.TokuisakiName;
                    vbrList.Cell("**LicPlateCarNo", 0, row).Value = item.LicPlateCarNo;
                    vbrList.Cell("**Biko", 0, row).Value = item.Biko;
                    vbrList.Cell("**MagoYoshasaki", 0, row).Value = item.MagoYoshasaki;

                    // 受注をまたぐ場合　罫線
                    if (juchuId != item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":AT" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 区分
                        intKbn = 0;
                        // 日付
                        strYmd = string.Empty;
                        // 時間
                        strHm = string.Empty;
                        // 受注ID
                        juchuId = decimal.Zero;
                    }

                    // 明細カウント
                    detailsCount++;

                    // 最終行の場合
                    if (list.Count() <= detailsCount)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row + 1) + ":AT" + (SHOSAI_START_ROW + row + 1))
                            .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
                    }

                    // 表示行カウント
                    row = row + 2;

                    // 改ページ
                    if (MAX_ROWS <= row
                        && list.Count() > detailsCount)
                    {
                        // ページカウント
                        page++;

                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row - 1) + ":AT" + (SHOSAI_START_ROW + row - 1))
                            .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 改ページ
                        vbrList.Page.Next(true);
                        vbrList.Page.Repeat(1);

                        // 会社情報設定
                        this.SetKanriInfo(vbrList, kanriInfo);

                        // ヘッダ情報設定
                        this.CreateRepotHeader(vbrList, wkInfo, outputDateTime, searchparameter);

                        // ページ数
                        totalPageCount++;
                        vbrList.Cell("C36").Value = totalPageCount;

                        // 傭車先毎総ページ数
                        groupPageCount++;
                        vbrList.Cell("AP1").Value = groupPageCount.ToString() + " / " + groupTotalPageCount;

                        row = 0;

                        // 区分
                        intKbn = 0;
                        // 日付
                        strYmd = string.Empty;
                        // 時間
                        strHm = string.Empty;
                        // 受注ID
                        juchuId = decimal.Zero;
                    }
                    else
                    {
                        // 区分
                        intKbn = item.KbnInt;
                        // 日付
                        strYmd = item.TaskYMD;
                        // 時間
                        strHm = item.TaskHM;
                        // 受注ID
                        juchuId = item.JuchuId;
                    }
                }
            }

            //シート名を設定する
            //vbrList.Page.Name = "";

            // ページ処理の終了
            vbrList.Page.End();

            // ページ中央
            vbrList.Page.Attr.Center = PageCenter.Horz;

            return vbrList;
        }

        /// <summary>
        /// レポートファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="kanriInfo">管理情報</param>
        /// <returns>レポートファイル</returns>
        private void SetKanriInfo(CellReport vbrList, KanriInfo kanriInfo)
        {
            int startRow = !string.IsNullOrWhiteSpace(kanriInfo.Address2) ? 2 : 3;

            // 会社名
            vbrList.Cell("AL" + (startRow++).ToString()).Value = !string.IsNullOrWhiteSpace(kanriInfo.CompanyShortName) ? kanriInfo.CompanyShortName : kanriInfo.CompanyName;

            // 会社住所1
            vbrList.Cell("AL" + (startRow++).ToString()).Value = kanriInfo.Address1;

            // 会社住所2が存在する場合
            if (!string.IsNullOrWhiteSpace(kanriInfo.Address2))
            {
                vbrList.Cell("AL" + (startRow++).ToString()).Value = kanriInfo.Address2;
            }

            // TEL
            vbrList.Cell("AO" + (startRow++).ToString()).Value = kanriInfo.Tel;

            // FAX
            vbrList.Cell("AO" + (startRow++).ToString()).Value = kanriInfo.Fax;

            // 担当
            vbrList.Cell("AO" + (startRow++).ToString()).Value = this.authInfo.OperetorName;
        }

        /// <summary>
        /// ヘッダ情報を設定します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="wkInfo">傭車依頼書情報</param>
        /// <param name="outputDateTime">出力日</param>
        /// <param name="searchparameter">検索条件</param>
        /// <returns>レポートファイル</returns>
        private void CreateRepotHeader(CellReport vbrList, YoshaIraishoPrtInfo wkInfo,
            String outputDateTime, YoshaIraishoPrtSearchParameter searchparameter)
        {
            // 傭車先名（取引先名称）
            vbrList.Cell("A1").Value = wkInfo.TorihikiName + " " + wkInfo.TorihikiHonorificTitle;

            // 傭車先TEL（取引先TEL）
            vbrList.Cell("C2").Value = wkInfo.TorihikiTel;

            // 傭車先FAX（取引先FAX）
            vbrList.Cell("C3").Value = wkInfo.TorihikiFax;

            // 出力日
            vbrList.Cell("AI1").Value = outputDateTime;

            // 車両
            vbrList.Cell("C6").Value = wkInfo.LicPlateCarNo;

            //備考
            vbrList.Cell("D30").Value = searchparameter.Biko01;
            vbrList.Cell("D31").Value = searchparameter.Biko02;
            vbrList.Cell("D32").Value = searchparameter.Biko03;
            vbrList.Cell("D33").Value = searchparameter.Biko04;
            vbrList.Cell("D34").Value = searchparameter.Biko05;
        }

        /// <summary>
        /// 共通項目を作成します
        /// </summary>
        /// <param name="item">傭車依頼書情報</param>
        /// <param name="inStartDate">傭車依頼書明細情報（発地）</param>
        /// <param name="inEndDate">傭車依頼書明細情報（着地）</param>
        private void CreateCommonItem(YoshaIraishoPrtInfo item, ref YoshaIraishoDetailsInfo inStartDate, ref YoshaIraishoDetailsInfo inEndDate)
        {
            inStartDate.TokuisakiName = inEndDate.TokuisakiName = item.TokuisakiName;
            inStartDate.ItemName = inEndDate.ItemName = item.ItemName;
            inStartDate.Number = inEndDate.Number = item.Number.ToString("F3");
            inStartDate.FigName = inEndDate.FigName = item.FigName;
            if (item.Weight != null && item.Weight.Value != 0) inStartDate.Weight = inEndDate.Weight = item.Weight.Value.ToString();
        }

        #endregion
    }
}
