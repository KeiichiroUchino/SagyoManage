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
    /// 請求確認リストのExcelファイルを作成します。
    /// </summary>
    public class SeikyuKakuninList
    {
        #region ユーザー定義

        #region 請求確認リスト関係

        /// <summary>
        /// シート名_請求確認リスト
        /// </summary>
        private string SHEET_NAME_SeikyuKakuninList = "請求確認リスト";

        /// <summary>
        /// 詳細部の開始行（1～）
        /// </summary>
        private const int SHOSAI_START_ROW = 5;

        /// <summary>
        /// 行の高さ
        /// </summary>
        private const double ROW_HEIGHT = (double)(16.50);

        /// <summary>
        /// 請求確認リストの１ページのMAX行数(39行)
        /// </summary>
        private const int MAX_ROWS = 39;

        /// <summary>
        /// 1レコードの行数（間隔）
        /// </summary>
        private const int NUM_OF_ROWS_PER_RECORD = 1;

        /// <summary>
        /// テンプレートファイル
        /// </summary>
        private static readonly byte[] TEMPLATE_FILE
               = global::Jpsys.HaishaManageV10.VBReport.Properties.Resources.SeikyuKakuninList;

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
        public SeikyuKakuninList()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、インスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SeikyuKakuninList(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// 請求確認リストのExcelファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="info">請求確認リスト情報</param>
        /// <param name="searchparameter">請求確認リスト（印刷条件）</param>
        /// <returns>請求確認リストのExcelファイル</returns>
        public CellReport SeikyuKakuninListData(CellReport vbrList, IList<SeikyuKakuninListRptInfo> reportData, SeikyuDataSakuseiConditionInfo param)
        {
            // レポートファイルを作成
            return this.CreateReportList(vbrList, reportData, param);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// レポートファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="list">請求確認リスト情報</param>
        /// <returns>レポートファイル</returns>
        private CellReport CreateReportList(CellReport vbrList, IList<SeikyuKakuninListRptInfo> reportData, SeikyuDataSakuseiConditionInfo param)
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
            vbrList.Page.Start(SHEET_NAME_SeikyuKakuninList, PAGE_INFO);
            vbrList.Page.Repeat(1);

            // 出力日
            String NowStriing = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            // 総ページ数
            int totalPageCount = (int)Math.Ceiling(((double)reportData.Count() / MAX_ROWS));

            // ページカウント
            int pageCount = 1;

            // ヘッダ作成
            this.CreateRepotHeader(vbrList, param.PrintJokenString, NowStriing, pageCount, totalPageCount);

            // 詳細情報 -----------------------------------

            // 行カウント
            int row = 0;
            // 明細カウント
            int detailsCount = 0;
            foreach (SeikyuKakuninListRptInfo item in reportData.OrderBy(x => x.TaskStartDate))
            {
                if (row > 0)
                {
                    // セルコピー
                    vbrList.Cell("A5:BJ5").Copy("A" + (SHOSAI_START_ROW + row).ToString());
                    // 行高を設定
                    vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;
                    // 罫線の設定
                    vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":BJ" + (SHOSAI_START_ROW + row))
                        .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
                }

                vbrList.Cell("**TaskStartDate", 0, row).Value = item.TaskStartDate.ToString("yyyy/MM/dd");
                vbrList.Cell("**TaskStartTime", 0, row).Value = item.TaskStartDate.ToString("HH:mm");
                vbrList.Cell("**BranchOfficeCode", 0, row).Value = item.BranchOfficeShortName;
                vbrList.Cell("**CarCode", 0, row).Value = item.CarCode;
                vbrList.Cell("**LicPlateCarNo", 0, row).Value = item.LicPlateCarNo;
                vbrList.Cell("**CarKind", 0, row).Value = item.CarKindShortName;
                vbrList.Cell("**Driver", 0, row).Value = item.DriverName;
                vbrList.Cell("**Yoshasaki", 0, row).Value = item.CarOfChartererShortName;
                vbrList.Cell("**Tokuisaki", 0, row).Value = string.Format("{0} {1}", item.TokuisakiCode, item.TokuisakiName);
                vbrList.Cell("**StartPoint", 0, row).Value = string.Format("{0} {1}", item.StartPointCode, item.StartPointName);
                vbrList.Cell("**EndPoint", 0, row).Value = string.Format("{0} {1}", item.EndPointCode, item.EndPointName);
                vbrList.Cell("**Item", 0, row).Value = string.Format("{0} {1}", item.ItemCode, item.ItemName);
                vbrList.Cell("**Number", 0, row).Value = item.Number;
                vbrList.Cell("**SeikyuKingaku", 0, row).Value = item.PriceInPrice.ToString("###,###,###,###,###,##0");
                vbrList.Cell("**YoshaKingaku", 0, row).Value = item.CharterPrice.ToString("###,###,###,###,###,##0");

                row += NUM_OF_ROWS_PER_RECORD;

                // 明細カウント
                detailsCount++;

                // 改ページ
                if (MAX_ROWS <= row
                    && reportData.Count() > detailsCount)
                {
                    // ページカウント
                    pageCount++;

                    // 罫線の設定
                    vbrList.Cell("A" + (SHOSAI_START_ROW + row - 1) + ":BJ" + (SHOSAI_START_ROW + row - 1))
                        .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

                    //改ページ
                    vbrList.Page.Next(true);
                    vbrList.Page.Repeat(1);

                    // ヘッダ作成
                    this.CreateRepotHeader(vbrList, param.PrintJokenString, NowStriing, pageCount, totalPageCount);

                    row = 0;
                }
            }

            // 最終行の罫線の設定
            vbrList.Cell("A" + (SHOSAI_START_ROW + row - 1) + ":BJ" + (SHOSAI_START_ROW + row - 1))
                .Attr.LineBottom(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

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
        /// <param name="joken">条件</param>
        /// <param name="nowString">出力日</param>
        /// <param name="pageCount">ページ数</param>
        /// <param name="totalPageCount">総ページ数</param>
        private void CreateRepotHeader(CellReport vbrList, String joken, String nowString, int pageCount, int totalPageCount)
        {
            // 出力日
            vbrList.Cell("AY2").Value = nowString;

            // 条件
            vbrList.Cell("C2").Value = joken;

            // 総ページ数
            vbrList.Cell("BG2").Value = pageCount.ToString() + "／" + totalPageCount.ToString();

            // ページ数
            vbrList.Cell("C45").Value = pageCount.ToString();
        }

        #endregion
    }
}
