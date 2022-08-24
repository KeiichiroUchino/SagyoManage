using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using AdvanceSoftware.VBReport8;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.ComLib;

namespace Jpsys.HaishaManageV10.VBReport
{
    /// <summary>
    /// 配送指示書のExcelファイルを作成します。
    /// </summary>
    public class HaisoShijiSho
    {
        #region ユーザー定義

        #region 配送指示書関係

        /// <summary>
        /// シート名_配送指示書
        /// </summary>
        private string SHEET_NAME_HaisoShijiSho = "配送指示書";

        /// <summary>
        /// 詳細部の開始行（1～）
        /// </summary>
        private const int SHOSAI_START_ROW = 10;

        /// <summary>
        /// 行の高さ
        /// </summary>
        private const double ROW_HEIGHT = (double)(16.50);

        /// <summary>
        /// 配送指示書の１ページのMAX行数(24行)
        /// </summary>
        private const int MAX_ROWS = 24;

        /// <summary>
        /// テンプレートファイル
        /// </summary>
        private static readonly byte[] TEMPLATE_FILE
               = global::Jpsys.HaishaManageV10.VBReport.Properties.Resources.HaisoShijiSho;

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
        public HaisoShijiSho()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、インスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaisoShijiSho(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// 配送指示書のExcelファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="info">配送指示書情報</param>
        /// <param name="searchparameter">配送指示書（印刷条件）</param>
        /// <returns>配送指示書のExcelファイル</returns>
        public CellReport HaisoShijiShoData(CellReport vbrList, List<HaisoShijiShoRptInfo> info, HaisoShijiShoRptSearchParameter searchparameter)
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
        /// <param name="list">配送指示書情報</param>
        /// <returns>レポートファイル</returns>
        private CellReport CreateReportList(CellReport vbrList, List<HaisoShijiShoRptInfo> info, HaisoShijiShoRptSearchParameter searchparameter)
        {
            ////【1】===========================================================//
            //// 帳票作成に必要な開始処理
            ////================================================================//
            //CellReport vbrList = new CellReport();
            //// 計算式の再計算を行なう設定
            //// 詳細は[2-VB-Reportの機能]-[3-データ設定/操作]-[5-計算式(自動計算)]
            //vbrList.ApplyFormula = true;
            //vbrList.Report.Start();
            vbrList.Report.Embed(TEMPLATE_FILE);

            //【2】===========================================================//
            // Page.Startでデザインファイルのテンプレート(シート)を指定し、値の
            // 設定を行います。値の設定後、Page.Endでページ処理を終了します。
            // 　[2-VB-Reportの機能]-[2-帳票ページ処理]-[Page.Start]
            //================================================================//

            // ページ処理の開始
            vbrList.Page.Start(SHEET_NAME_HaisoShijiSho, PAGE_INFO);

            //出力日
            String outputDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            var carList = info.GroupBy(x => new { CarId = x.CarId, DriverId = x.DriverId });

            // 総ページ数
            int totalPageCount = 0;

            foreach (var group in carList)
            {
                // ヘッダ情報 --------------------------------

                HaisoShijiShoRptInfo wkInfo = group.First();

                //改ページ
                vbrList.Page.Next(true);
                vbrList.Page.Repeat(1);

                // ヘッダ作成
                this.CreateRepotHeader(vbrList, wkInfo, outputDateTime, searchparameter);

                // 詳細情報 -----------------------------------
                List<HaisoShijishoDetailsInfo> HaisoShijishoDetailsList = new List<HaisoShijishoDetailsInfo>();

                // 受注単位でグループ化
                var juchuList = group.GroupBy(x => new { JuchuId = x.JuchuId });

                foreach (var juchuItem in juchuList)
                {
                    // ソート用積載日
                    HaisoShijiShoRptInfo firstItem = juchuItem.First();

                    foreach (HaisoShijiShoRptInfo item in juchuItem)
                    {
                        HaisoShijishoDetailsInfo inStartDate = new HaisoShijishoDetailsInfo();
                        HaisoShijishoDetailsInfo inEndDate = new HaisoShijishoDetailsInfo();

                        // 共通項目
                        CreateCommonItem(item, ref inStartDate, ref inEndDate);

                        // 出発情報
                        inStartDate.Kbn = "積日";
                        inStartDate.PointName = item.StartPointName;
                        inStartDate.SekisaiYMD = item.TaskStartDateTime.ToString("yyyy/MM/dd");
                        inStartDate.SekisaiHM = item.TaskStartDateTime.ToString("HH:mm");
                        inStartDate.TaskYMD = item.StartYMD.ToString("yyyy/MM/dd");
                        inStartDate.TaskHM = item.StartYMD.ToString("HH:mm");
                        inStartDate.Address = item.StartPointAddress1 + item.StartPointAddress2;
                        inStartDate.Biko = item.Biko ?? string.Empty;

                        // ソート用
                        inStartDate.JuchuId = item.JuchuId;
                        inStartDate.KbnInt = 1;

                        // 積載日時
                        decimal sortSekisaiKey = Convert.ToDecimal(firstItem.TaskStartDateTime.ToString("yyyyMMddHHmm")
                            + firstItem.TaskStartDateTime.ToString("HHmmss"));
                        inStartDate.JuchuMinSekisaiYMDSort = sortSekisaiKey;

                        // 発日＋着日
                        decimal sortHatsuChakuKey = Convert.ToDecimal(firstItem.StartYMD.ToString("yyyyMMddHHmm")
                            + firstItem.TaskEndDateTime.ToString("yyyyMMddHHmm"));
                        inStartDate.JuchuMinHatsuChakuYMDSort = sortHatsuChakuKey;

                        inStartDate.TaskYMDSort = item.TaskStartDateTime.Date;
                        inStartDate.TaskHMSort = Convert.ToInt32(item.TaskStartDateTime.ToString("HHmmss"));
                        HaisoShijishoDetailsList.Add(inStartDate);

                        // 到着情報
                        inEndDate.Kbn = "卸日";
                        inEndDate.PointName = item.EndPointName;
                        inEndDate.SekisaiYMD = item.TaskEndDateTime.ToString("yyyy/MM/dd");
                        inEndDate.SekisaiHM = item.TaskEndDateTime.ToString("HH:mm");
                        inEndDate.TaskYMD = item.ReuseYMD.ToString("yyyy/MM/dd");
                        inEndDate.TaskHM = item.ReuseYMD.ToString("HH:mm");
                        inEndDate.Address = item.EndPointAddress1 + item.EndPointAddress2;

                        // ソート用
                        inEndDate.JuchuId = item.JuchuId;
                        inEndDate.KbnInt = 2;
                        inEndDate.JuchuMinSekisaiYMDSort = sortSekisaiKey;
                        inEndDate.JuchuMinHatsuChakuYMDSort = sortHatsuChakuKey;
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
                        inEndDate.Biko = string.Empty;
                        HaisoShijishoDetailsList.Add(inEndDate);
                    }
                }

                // 積載日＋積載時間区分値、積日＋着日、受注ID、積着日、積時間でソート
                var list = HaisoShijishoDetailsList
                    .OrderBy(x => x.JuchuMinSekisaiYMDSort)
                    .ThenBy(x => x.JuchuMinHatsuChakuYMDSort)
                    .ThenBy(x => x.JuchuId)
                    .ThenBy(x => x.TaskYMDSort)
                    .ThenBy(x => x.TaskHMSort)
                    .ThenBy(x => x.KbnInt);

                // グループ内総ページ数取得
                int groupTotalPageCount = (int)Math.Ceiling((double)list.Count() / (MAX_ROWS / 2));

                // グループ内ページ数
                int groupPageCount = 0;

                // ページ数
                totalPageCount++;
                vbrList.Cell("C36").Value = totalPageCount;

                // 車両・乗務員毎ページ数
                groupPageCount++;
                vbrList.Cell("AS1").Value = groupPageCount + " / " + groupTotalPageCount.ToString();

                // 表示行カウント
                int row = 0;
                // 明細カウント
                int detailsCount = 0;
                // ページカウント
                int page = 1;
                // 区分
                int intKbn = 0;
                // 積載日付
                string strSekisaiYmd = string.Empty;
                // 積載時間
                string strSekisaiHm = string.Empty;
                // 日付
                string strYmd = string.Empty;
                // 時間
                string strHm = string.Empty;
                // 受注ID
                decimal juchuId = decimal.Zero;
                foreach (HaisoShijishoDetailsInfo item in list)
                {
                    if (row > 0)
                    {
                        // セルコピー
                        vbrList.Cell("A" + SHOSAI_START_ROW + ":AV" + (SHOSAI_START_ROW + 1)).Copy("A" + (SHOSAI_START_ROW + row).ToString());
                        // 行高を設定
                        vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;

                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":AV" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
                    }

                    // 区分　罫線
                    if (intKbn == item.KbnInt && juchuId == item.JuchuId)
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

                    // 積載日付　罫線
                    if (strSekisaiYmd.Equals(item.SekisaiYMD) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("C" + (SHOSAI_START_ROW + row) + ":F" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**SekisaiYMD", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**SekisaiYMD", 0, row).Value = item.SekisaiYMD;
                    }

                    // 積載時間　罫線
                    if (strSekisaiHm.Equals(item.SekisaiHM) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("G" + (SHOSAI_START_ROW + row) + ":H" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**SekisaiHM", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**SekisaiHM", 0, row).Value = item.SekisaiHM;
                    }

                    // 日付　罫線
                    if (strYmd.Equals(item.TaskYMD) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("I" + (SHOSAI_START_ROW + row) + ":L" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**TaskYMD", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**TaskYMD", 0, row).Value = item.TaskYMD;
                    }

                    // 時間　罫線
                    if (strHm.Equals(item.TaskHM) && intKbn == item.KbnInt && juchuId == item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("M" + (SHOSAI_START_ROW + row) + ":N" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

                        // 前行と同じ場合は空
                        vbrList.Cell("**TaskHM", 0, row).Value = string.Empty;
                    }
                    else
                    {
                        vbrList.Cell("**TaskHM", 0, row).Value = item.TaskHM;
                    }

                    vbrList.Cell("**PointNM", 0, row).Value = item.PointName;
                    vbrList.Cell("**ItemNM", 0, row).Value = item.ItemName;
                    vbrList.Cell("**Number", 0, row).Value = item.Number;
                    vbrList.Cell("**FigNM", 0, row).Value = item.FigName;
                    vbrList.Cell("**Weight", 0, row).Value = item.Weight ?? string.Empty;
                    vbrList.Cell("**Address", 0, row).Value = item.Address;
                    vbrList.Cell("**TokuisakiNM", 0, row).Value = item.TokuisakiName;
                    vbrList.Cell("**Biko", 0, row).Value = item.Biko;

                    // 受注をまたぐ場合　罫線
                    if (juchuId != item.JuchuId)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":AV" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
                    }

                    // 最終行の場合
                    if (list.Count() - 1 <= detailsCount)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row + 1) + ":AV" + (SHOSAI_START_ROW + row + 1))
                            .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
                    }

                    // 表示行カウント
                    row = row +2;

                    // 明細カウント
                    detailsCount++;

                    // 改ページ
                    if (MAX_ROWS <= row
                        && list.Count() > detailsCount)
                    {
                        // ページカウント
                        page++;

                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row - 1) + ":AV" + (SHOSAI_START_ROW + row - 1))
                            .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

                        //改ページ
                        vbrList.Page.Next(true);
                        vbrList.Page.Repeat(1);

                        // ヘッダ作成
                        this.CreateRepotHeader(vbrList, wkInfo, outputDateTime, searchparameter);

                        // ページ数
                        totalPageCount++;
                        vbrList.Cell("C36").Value = totalPageCount;

                        // 車両・乗務員毎ページ数
                        groupPageCount++;
                        vbrList.Cell("AS1").Value = groupPageCount + " / " + groupTotalPageCount.ToString();

                        row = 0;

                        // 区分
                        intKbn = 0;
                        // 積載日付
                        strSekisaiYmd = string.Empty;
                        // 積載時間
                        strSekisaiHm = string.Empty;
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
                        // 積載日付
                        strSekisaiYmd = item.SekisaiYMD;
                        // 積載時間
                        strSekisaiHm = item.SekisaiHM;
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
        /// レポートファイルのヘッダを作成します
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="wkInfo">配送指示書情報</param>
        /// <param name="searchparameter">配送指示書の印刷条件</param>
        /// <returns>レポートファイルのヘッダ</returns>
        private void CreateRepotHeader(CellReport vbrList, HaisoShijiShoRptInfo wkInfo,
            String outputDateTime, HaisoShijiShoRptSearchParameter searchparameter)
        {
            // 営業所名称
            vbrList.Cell("E1").Value = wkInfo.BranchOfficeName;

            // 出力日
            vbrList.Cell("AP2").Value = outputDateTime;

            // 車両（陸運名 + 分類区分 + 業態区分 + 車番）
            vbrList.Cell("C5").Value = wkInfo.LicPlateDeptName + wkInfo.LicPlateCarKindKbn + wkInfo.LicPlateUsageKbn + wkInfo.LicPlateCarNo;

            // 車種
            vbrList.Cell("M5").Value = wkInfo.CarKindName;

            // 乗務員（社員名）
            vbrList.Cell("C6").Value = wkInfo.StaffCd + " " + wkInfo.StaffName;

            // 伝達事項
            vbrList.Cell("X5").Value = searchparameter.Attentions;

            // 右上車両（陸運名 + 分類区分 + 業態区分 + 車番）
            vbrList.Cell("AM1").Value = wkInfo.LicPlateDeptName + wkInfo.LicPlateCarKindKbn + wkInfo.LicPlateUsageKbn + wkInfo.LicPlateCarNo;
        }

        /// <summary>
        /// 共通項目を作成します
        /// </summary>
        /// <param name="item">配送指示書情報</param>
        /// <param name="inStartDate">配送指示書明細情報（発地）</param>
        /// <param name="inEndDate">配送指示書明細情報（着地）</param>
        private void CreateCommonItem(HaisoShijiShoRptInfo item, ref HaisoShijishoDetailsInfo inStartDate, ref HaisoShijishoDetailsInfo inEndDate)
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
