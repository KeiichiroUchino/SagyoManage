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
    /// 受注配車一覧表のExcelファイルを作成します。
    /// </summary>
    public class JuchuHaishaIchiranHyo
    {
        #region ユーザー定義

        #region 受注配車一覧表関係

        /// <summary>
        /// シート名_受注配車一覧表
        /// </summary>
        private string SHEET_NAME_JuchuHaishaIchiranHyo = "受注配車一覧表";

        /// <summary>
        /// 詳細部の開始行（1～）
        /// </summary>
        private const int SHOSAI_START_ROW = 8;

        /// <summary>
        /// 行の高さ
        /// </summary>
        private const double ROW_HEIGHT = (double)(16.50);

        /// <summary>
        /// 受注配車一覧表の1ページのMAX行数(48行)
        /// </summary>
        private const int MAX_ROWS = 48;

        /// <summary>
        /// 1レコードの行数（間隔）
        /// </summary>
        private const int NUM_OF_ROWS_PER_RECORD = 3;

        /// <summary>
        /// テンプレートファイル
        /// </summary>
        private static readonly byte[] TEMPLATE_FILE
               = global::Jpsys.HaishaManageV10.VBReport.Properties.Resources.JuchuHaishaIchiranHyo;

        /// <summary>
        /// 使用するページ
        /// </summary>
        private const string PAGE_INFO = "1-99999";

        /// <summary>
        /// 処理日時
        /// </summary>
        private String _processDateTime = string.Empty;

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
        public JuchuHaishaIchiranHyo()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、インスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public JuchuHaishaIchiranHyo(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #endregion

        #region パブリックメソッド

        /// <summary>
        /// 受注配車一覧表のExcelファイルを作成します。
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="info">受注配車一覧表情報</param>
        /// <param name="searchparameter">受注配車一覧表（印刷条件）</param>
        /// <returns>受注配車一覧表のExcelファイル</returns>
        public CellReport JuchuHaishaIchiranHyoData(CellReport vbrList, IList<JuchuHaishaIchiranHyoRptInfo> reportData, JuchuHaishaIchiranHyoSearchParameter param)
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
        /// <param name="list">受注配車一覧表情報</param>
        /// <returns>レポートファイル</returns>
        private CellReport CreateReportList(CellReport vbrList, IList<JuchuHaishaIchiranHyoRptInfo> reportData, JuchuHaishaIchiranHyoSearchParameter param)
        {
            ////【1】===========================================================//
            //// 帳票作成に必要な開始処理
            ////================================================================//
            vbrList.Report.Embed(TEMPLATE_FILE);

            Boolean firstflg = true;

            //【2】===========================================================//
            // Page.Startでデザインファイルのテンプレート(シート)を指定し、値の
            // 設定を行います。値の設定後、Page.Endでページ処理を終了します。
            // 　[2-VB-Reportの機能]-[2-帳票ページ処理]-[Page.Start]
            //================================================================//

            // ページ処理の開始
            vbrList.Page.Start(SHEET_NAME_JuchuHaishaIchiranHyo, PAGE_INFO);

            // 処理日時取得
            this._processDateTime = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

            //営業所毎グループ取得
            var branchOfficeList = reportData
                    .OrderBy(x => x.BranchOfficeCd)
                    .GroupBy(x => new { BranchOfficeId = x.BranchOfficeId });

            // 1件目の情報
            JuchuHaishaIchiranHyoRptInfo wkInfo = new JuchuHaishaIchiranHyoRptInfo();

            // ページ数初期化
            int pageCount = 0;

            foreach (var group in branchOfficeList)
            {
                if (firstflg)
                {
                    //最初は1シートの終了処理はしません
                    firstflg = false;
                }
                else
                {
                    #region 終了処理

                    //シート名を設定する
                    vbrList.Page.Name = string.Format("{0} {1}"
                        , wkInfo.BranchOfficeCd.ToString()
                        , wkInfo.BranchOfficeSNM);

                    // ページ処理の終了
                    vbrList.Page.End();

                    // ページ中央
                    vbrList.Page.Attr.Center = PageCenter.Horz;

                    #endregion
                }

                #region 開始処理

                //【2】===========================================================//
                // Page.Startでデザインファイルのテンプレート(シート)を指定し、値の
                // 設定を行います。値の設定後、Page.Endでページ処理を終了します。
                // 　[2-VB-Reportの機能]-[2-帳票ページ処理]-[Page.Start]
                //================================================================//
                // ページ処理の開始
                vbrList.Page.Start(SHEET_NAME_JuchuHaishaIchiranHyo, PAGE_INFO);
                vbrList.Page.Repeat(1);

                // 1件目の情報取得
                wkInfo = group.First();

                // 総ページ数
                pageCount++;

                // 総明細行
                int maxMeisaiCount = (group.GroupBy(x => x.JuchuId).Count() * 2) + group.Where(x => 0 < x.HaishaId).GroupBy(x => x.HaishaId).Count();

                // グループ内総ページ数
                int groupTotalPageCount = (int)Math.Ceiling(((double)(maxMeisaiCount * 3) / MAX_ROWS));

                // グループ内ページ数
                int groupPageCount = 1;

                // ヘッダ作成
                this.CreateRepotHeader(vbrList, wkInfo, param, pageCount, groupPageCount, groupTotalPageCount);

                #endregion

                // 詳細情報 -----------------------------------

                // 行数初期化
                int row = 0;

                // 明細カウント
                int detailsCount = 0;

                // 受注単位でグループ化
                var juchuList = group
                    .OrderBy(x => x.TaskStartDateTime)
                    .ThenBy(x => x.JuchuId)
                    .GroupBy(x => new { JuchuId = x.JuchuId });

                foreach (var juchuItem in juchuList)
                {
                    // 合計情報
                    JuchuHaishaIchiranHyoSumInfo sumInfo = new JuchuHaishaIchiranHyoSumInfo();

                    // 受注情報設定フラグ
                    bool juchuSetFlag = true;

                    foreach (JuchuHaishaIchiranHyoRptInfo item in juchuItem
                        .OrderBy(x => x.Haisha_TaskStartDateTime)
                        .ThenBy(x => x.HaishaId))
                    {
                        // 受注ごと初回のみ
                        if (juchuSetFlag)
                        {
                            // 受注情報処理
                            this.CreateJuchuInfo(vbrList, item, row);

                            // 表示行カウント
                            row += NUM_OF_ROWS_PER_RECORD;

                            // 明細カウント
                            detailsCount++;

                            // 改ページ
                            if (MAX_ROWS <= row)
                            {
                                // 総ページ数
                                pageCount++;

                                // グループ内ページ数
                                groupPageCount++;

                                // 改ページ
                                this.CreateNextPage(vbrList, wkInfo, param, row, pageCount, groupPageCount, groupTotalPageCount);

                                row = 0;
                            }

                            juchuSetFlag = false;
                        }

                        // 配車情報が存在する場合
                        if (0 < item.HaishaId)
                        {
                            // 配車情報処理
                            this.CreateHaishaInfo(vbrList, item, sumInfo, row);

                            row += NUM_OF_ROWS_PER_RECORD;

                            // 明細カウント
                            detailsCount++;

                            // 改ページ
                            if (MAX_ROWS <= row)
                            {
                                // 総ページ数
                                pageCount++;

                                // グループ内ページ数
                                groupPageCount++;

                                // 改ページ
                                this.CreateNextPage(vbrList, wkInfo, param, row, pageCount, groupPageCount, groupTotalPageCount);

                                row = 0;
                            }
                        }
                    }

                    // 合計処理
                    this.CreateSumInfo(vbrList, sumInfo, row);

                    row += NUM_OF_ROWS_PER_RECORD;

                    // 明細カウント
                    detailsCount++;

                    // 改ページ
                    if (MAX_ROWS <= row
                        && detailsCount > maxMeisaiCount)
                    {
                        // 総ページ数
                        pageCount++;

                        // グループ内ページ数
                        groupPageCount++;

                        // 改ページ
                        this.CreateNextPage(vbrList, wkInfo, param, row, pageCount, groupPageCount, groupTotalPageCount);

                        row = 0;
                    }

                    // 最終行の場合
                    if (detailsCount == maxMeisaiCount)
                    {
                        // 罫線の設定
                        vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row))
                            .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
                    }
                }
            }

            //シート名を設定する
            vbrList.Page.Name = string.Format("{0} {1}"
                , wkInfo.BranchOfficeCd.ToString()
                , wkInfo.BranchOfficeSNM);

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
        /// <param name="wkInfo">受注配車一覧表情報</param>
        /// <param name="param">受注配車一覧表の印刷条件</param>
        /// <param name="pageCount">総ページ数</param>
        /// <param name="groupPageCount">グループ内ページ数</param>
        /// <param name="groupTotalPageCount">グループ内総ページ数</param>
        private void CreateRepotHeader(CellReport vbrList, JuchuHaishaIchiranHyoRptInfo wkInfo, JuchuHaishaIchiranHyoSearchParameter param,
            int pageCount, int groupPageCount, int groupTotalPageCount)
        {
            // 出力日
            vbrList.Cell("BL2").Value = this._processDateTime;

            // ページ数／総ページ数
            vbrList.Cell("BS2").Value = groupPageCount.ToString() + " / " + groupTotalPageCount.ToString();

            // 条件
            vbrList.Cell("C2").Value = param.PrintJokenString;

            // 営業所名称
            vbrList.Cell("D3").Value = wkInfo.BranchOfficeSNM;

            // ページ数
            vbrList.Cell("C57").Value = pageCount.ToString();
        }

        /// <summary>
        /// レポートファイルのヘッダを作成します
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="wkInfo">受注配車一覧表情報</param>
        /// <param name="param">受注配車一覧表の印刷条件</param>
        private void CreateNextPage(CellReport vbrList, JuchuHaishaIchiranHyoRptInfo wkInfo, JuchuHaishaIchiranHyoSearchParameter param,
            int row, int pageCount, int groupPageCount, int groupTotalPageCount)
        {
            // 罫線の設定
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row))
                .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

            //改ページ
            vbrList.Page.Next(true);
            vbrList.Page.Repeat(1);

            // ヘッダ作成
            this.CreateRepotHeader(vbrList, wkInfo, param, pageCount, groupPageCount, groupTotalPageCount);
        }

        /// <summary>
        /// 受注情報を作成します
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="wkInfo">受注配車一覧表情報</param>
        /// <param name="param">受注配車一覧表の印刷条件</param>
            private void CreateJuchuInfo(CellReport vbrList, JuchuHaishaIchiranHyoRptInfo meisai, int row)
        {
            // セルコピー
            vbrList.Cell("A8:BV10").Copy("A" + (SHOSAI_START_ROW + row).ToString());
            // 行高を設定
            vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 1).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 2).ToString()).RowHeight = ROW_HEIGHT;

            // 罫線の設定
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row))
                .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":A" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineLeft(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("BV" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineRight(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

            vbrList.Cell("**TokuisakiCode", 0, row).Value = meisai.TokuisakiCd.ToString();
            vbrList.Cell("**TokuisakiName", 0, row).Value = meisai.TokuisakiNM;
            vbrList.Cell("**HanroCode", 0, row).Value = meisai.HanroCd == 0 ? string.Empty : meisai.HanroCd.ToString();
            vbrList.Cell("**HanroName", 0, row).Value = meisai.HanroNm;
            vbrList.Cell("**OfukuKbnName", 0, row).Value = meisai.OfukuKbnNm;
            vbrList.Cell("**StartPointCode", 0, row).Value = meisai.StartPointCd == 0 ? string.Empty : meisai.StartPointCd.ToString();
            vbrList.Cell("**StartPointName", 0, row).Value = meisai.StartPointNM;
            vbrList.Cell("**TaskStartDate", 0, row).Value = meisai.TaskStartDateTime.ToString("yyyy/MM/dd");
            vbrList.Cell("**TaskStartTime", 0, row).Value = meisai.TaskStartDateTime.ToString("HH:mm");
            vbrList.Cell("**StartYMD", 0, row).Value = meisai.StartYMD == DateTime.MinValue ? string.Empty : meisai.StartYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**StartHH", 0, row).Value = meisai.StartYMD == DateTime.MinValue ? string.Empty : meisai.StartYMD.ToString("HH:mm");
            vbrList.Cell("**EndPointCode", 0, row).Value = meisai.EndPointCd == 0 ? string.Empty : meisai.EndPointCd.ToString();
            vbrList.Cell("**EndPointName", 0, row).Value = meisai.EndPointNM;
            vbrList.Cell("**TaskEndDate", 0, row).Value = meisai.TaskEndDateTime.ToString("yyyy/MM/dd");
            vbrList.Cell("**TaskEndTime", 0, row).Value = meisai.TaskEndDateTime.ToString("HH:mm");
            vbrList.Cell("**ReuseYMD", 0, row).Value = meisai.ReuseYMD == DateTime.MinValue ? string.Empty : meisai.ReuseYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**ReuseHH", 0, row).Value = meisai.ReuseYMD == DateTime.MinValue ? string.Empty : meisai.ReuseYMD.ToString("HH:mm");
            vbrList.Cell("**ItemCode", 0, row).Value = meisai.ItemCd == 0 ? string.Empty : meisai.ItemCd.ToString();
            vbrList.Cell("**ItemName", 0, row).Value = meisai.ItemNM;
            vbrList.Cell("**AtPrice", 0, row).Value = meisai.AtPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**Weight", 0, row).Value = meisai.Weight.ToString("###,###,###,###,###,##0") + "Kg";
            vbrList.Cell("**YoshasakiCode", 0, row).Value = meisai.TorihikiCd == 0 ? string.Empty : meisai.TorihikiCd.ToString();
            vbrList.Cell("**YoshasakiName", 0, row).Value = meisai.TorihikiSNm;
            vbrList.Cell("**Number", 0, row).Value = meisai.Number.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**UriageKingaku", 0, row).Value = meisai.PriceInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaKingaku", 0, row).Value = meisai.PriceInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarCode", 0, row).Value = meisai.CarCd.ToString();
            vbrList.Cell("**LicPlateCarNo", 0, row).Value = meisai.LicPlateCarNo;
            vbrList.Cell("**Tsukoryo", 0, row).Value = meisai.TollFeeInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaTsukoryo", 0, row).Value = meisai.TollFeeInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarKindCode", 0, row).Value = meisai.CarKindCd == 0 ? string.Empty : meisai.CarKindCd.ToString();
            vbrList.Cell("**CarKindName", 0, row).Value = meisai.CarKindSNM;
            vbrList.Cell("**ZeiKbn", 0, row).Value = meisai.TaxDispKbnShortNmString;
            vbrList.Cell("**JomuinUriage", 0, row).Value = meisai.JomuinUriageKingaku.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaZeiKbn", 0, row).Value = meisai.CharterTaxDispKbnShortNmString;
            vbrList.Cell("**FutaiGyomuryo", 0, row).Value = meisai.FutaigyomuryoInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**StaffCode", 0, row).Value = meisai.DriverCd == 0 ? string.Empty : meisai.DriverCd.ToString();
            vbrList.Cell("**StaffName", 0, row).Value = meisai.DriverNm;
            vbrList.Cell("**AddUpYMD", 0, row).Value = meisai.AddUpYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**CharterAddUpYMD", 0, row).Value = meisai.CharterAddUpYMD == DateTime.MinValue ? string.Empty : meisai.CharterAddUpYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**Biko", 0, row).Value = meisai.Memo;
            vbrList.Cell("**Kakutei", 0, row).Value = meisai.FixFlagString;
            vbrList.Cell("**OwnerCode", 0, row).Value = meisai.OwnerCd == 0 ? string.Empty : meisai.OwnerCd.ToString();
            vbrList.Cell("**OwnerName", 0, row).Value = meisai.OwnerNM;
            vbrList.Cell("**YoshaKakutei", 0, row).Value = meisai.CharterFixFlagString;
            vbrList.Cell("**Magoyoshasaki", 0, row).Value = meisai.MagoYoshasaki;
            vbrList.Cell("**Juryozumi", 0, row).Value = meisai.ReceivedFlagString;
            vbrList.Cell("**JuchuTantoshaName", 0, row).Value = meisai.JuchuTantoNm;
            vbrList.Cell("**JuchuSlipNo", 0, row).Value = meisai.JuchuSlipNo.ToString();
        }

        /// <summary>
        /// 配車情報を作成します
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="meisai">受注配車一覧表情報</param>
        /// <param name="sumInfo">合計情報</param>
        /// <param name="row"></param>
        private void CreateHaishaInfo(CellReport vbrList, JuchuHaishaIchiranHyoRptInfo meisai,
            JuchuHaishaIchiranHyoSumInfo sumInfo, int row)
        {
            // セルコピー
            vbrList.Cell("A8:BV10").Copy("A" + (SHOSAI_START_ROW + row).ToString());
            // 行高を設定
            vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 1).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 2).ToString()).RowHeight = ROW_HEIGHT;

            // 罫線の設定
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":J" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":A" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineLeft(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("K" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("BV" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineRight(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);

            vbrList.Cell("**TokuisakiCode", 0, row).Value = string.Empty;
            vbrList.Cell("**TokuisakiName", 0, row).Value = string.Empty;
            vbrList.Cell("**HanroCode", 0, row).Value = string.Empty;
            vbrList.Cell("**HanroName", 0, row).Value = string.Empty;
            vbrList.Cell("**OfukuKbnName", 0, row).Value = string.Empty;
            vbrList.Cell("**StartPointCode", 0, row).Value = meisai.Haisha_StartPointCd == 0 ? string.Empty : meisai.Haisha_StartPointCd.ToString();
            vbrList.Cell("**StartPointName", 0, row).Value = meisai.Haisha_StartPointNM;
            vbrList.Cell("**TaskStartDate", 0, row).Value = meisai.Haisha_TaskStartDateTime.ToString("yyyy/MM/dd");
            vbrList.Cell("**TaskStartTime", 0, row).Value = meisai.Haisha_TaskStartDateTime.ToString("HH:mm");
            vbrList.Cell("**StartYMD", 0, row).Value = meisai.Haisha_StartYMD == DateTime.MinValue ? string.Empty : meisai.Haisha_StartYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**StartHH", 0, row).Value = meisai.Haisha_StartYMD == DateTime.MinValue ? string.Empty : meisai.Haisha_StartYMD.ToString("HH:mm");
            vbrList.Cell("**EndPointCode", 0, row).Value = meisai.Haisha_EndPointCd == 0 ? string.Empty : meisai.Haisha_EndPointCd.ToString();
            vbrList.Cell("**EndPointName", 0, row).Value = meisai.Haisha_EndPointNM;
            vbrList.Cell("**TaskEndDate", 0, row).Value = meisai.Haisha_TaskEndDateTime.ToString("yyyy/MM/dd");
            vbrList.Cell("**TaskEndTime", 0, row).Value = meisai.Haisha_TaskEndDateTime.ToString("HH:mm");
            vbrList.Cell("**ReuseYMD", 0, row).Value = meisai.Haisha_ReuseYMD == DateTime.MinValue ? string.Empty : meisai.Haisha_ReuseYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**ReuseHH", 0, row).Value = meisai.Haisha_ReuseYMD == DateTime.MinValue ? string.Empty : meisai.Haisha_ReuseYMD.ToString("HH:mm");
            vbrList.Cell("**ItemCode", 0, row).Value = meisai.Haisha_ItemCd == 0 ? string.Empty : meisai.Haisha_ItemCd.ToString();
            vbrList.Cell("**ItemName", 0, row).Value = meisai.Haisha_ItemNM;
            vbrList.Cell("**AtPrice", 0, row).Value = meisai.Haisha_AtPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**Weight", 0, row).Value = meisai.Haisha_Weight.ToString("###,###,###,###,###,##0") + "Kg";
            vbrList.Cell("**YoshasakiCode", 0, row).Value = meisai.Haisha_TorihikiCd == 0 ? string.Empty : meisai.Haisha_TorihikiCd.ToString();
            vbrList.Cell("**YoshasakiName", 0, row).Value = meisai.Haisha_TorihikiSNm;
            vbrList.Cell("**Number", 0, row).Value = meisai.Haisha_Number.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**UriageKingaku", 0, row).Value = meisai.Haisha_PriceInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaKingaku", 0, row).Value = meisai.Haisha_PriceInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarCode", 0, row).Value = meisai.Haisha_CarCd.ToString();
            vbrList.Cell("**LicPlateCarNo", 0, row).Value = meisai.Haisha_LicPlateCarNo;
            vbrList.Cell("**Tsukoryo", 0, row).Value = meisai.Haisha_TollFeeInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaTsukoryo", 0, row).Value = meisai.Haisha_TollFeeInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarKindCode", 0, row).Value = meisai.Haisha_CarKindCd == 0 ? string.Empty : meisai.Haisha_CarKindCd.ToString();
            vbrList.Cell("**CarKindName", 0, row).Value = meisai.Haisha_CarKindSNM;
            vbrList.Cell("**ZeiKbn", 0, row).Value = meisai.Haisha_TaxDispKbnShortNmString;
            vbrList.Cell("**JomuinUriage", 0, row).Value = meisai.Haisha_JomuinUriageKingaku.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaZeiKbn", 0, row).Value = meisai.Haisha_CharterTaxDispKbnShortNmString;
            vbrList.Cell("**FutaiGyomuryo", 0, row).Value = meisai.Haisha_FutaigyomuryoInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**StaffCode", 0, row).Value = meisai.Haisha_DriverCd == 0 ? string.Empty : meisai.Haisha_DriverCd.ToString();
            vbrList.Cell("**StaffName", 0, row).Value = meisai.Haisha_DriverNm;
            vbrList.Cell("**AddUpYMD", 0, row).Value = meisai.Haisha_AddUpYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**CharterAddUpYMD", 0, row).Value = meisai.Haisha_CharterAddUpYMD == DateTime.MinValue ? string.Empty : meisai.Haisha_CharterAddUpYMD.ToString("yyyy/MM/dd");
            vbrList.Cell("**Biko", 0, row).Value = meisai.Haisha_Biko;
            vbrList.Cell("**Kakutei", 0, row).Value = meisai.Haisha_FixFlagString;
            vbrList.Cell("**OwnerCode", 0, row).Value = meisai.Haisha_OwnerCd == 0 ? string.Empty : meisai.Haisha_OwnerCd.ToString();
            vbrList.Cell("**OwnerName", 0, row).Value = meisai.Haisha_OwnerNM;
            vbrList.Cell("**YoshaKakutei", 0, row).Value = meisai.Haisha_CharterFixFlagString;
            vbrList.Cell("**Magoyoshasaki", 0, row).Value = meisai.Haisha_MagoYoshasaki;
            vbrList.Cell("**Juryozumi", 0, row).Value = string.Empty;
            vbrList.Cell("**JuchuTantoshaName", 0, row).Value = string.Empty;
            vbrList.Cell("**JuchuSlipNo", 0, row).Value = string.Empty;

            sumInfo.Number = sumInfo.Number + meisai.Haisha_Number;
            sumInfo.PriceInPrice = sumInfo.PriceInPrice + meisai.Haisha_PriceInPrice;
            sumInfo.PriceInCharterPrice = sumInfo.PriceInCharterPrice + meisai.Haisha_PriceInCharterPrice;
            sumInfo.TollFeeInPrice = sumInfo.TollFeeInPrice = meisai.Haisha_TollFeeInPrice;
            sumInfo.TollFeeInCharterPrice = sumInfo.TollFeeInCharterPrice + meisai.Haisha_TollFeeInCharterPrice;
            sumInfo.JomuinUriageKingaku = sumInfo.JomuinUriageKingaku + meisai.Haisha_JomuinUriageKingaku;
            sumInfo.FutaigyomuryoInPrice = sumInfo.FutaigyomuryoInPrice + meisai.Haisha_FutaigyomuryoInPrice;
        }

        /// <summary>
        /// 配車情報を作成します
        /// </summary>
        /// <param name="vbrList">レポートファイル</param>
        /// <param name="sumInfo">合計情報</param>
        /// <param name="row"></param>
        private void CreateSumInfo(CellReport vbrList, JuchuHaishaIchiranHyoSumInfo sumInfo, int row)
        {
            // セルコピー
            vbrList.Cell("A8:BV10").Copy("A" + (SHOSAI_START_ROW + row).ToString());
            // 行高を設定
            vbrList.Cell((SHOSAI_START_ROW + row).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 1).ToString()).RowHeight = ROW_HEIGHT;
            vbrList.Cell((SHOSAI_START_ROW + row + 2).ToString()).RowHeight = ROW_HEIGHT;

            // 罫線の設定
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":AJ" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("A" + (SHOSAI_START_ROW + row) + ":A" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineLeft(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("AY" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("AY" + (SHOSAI_START_ROW + row) + ":AY" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineLeft(AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("BV" + (SHOSAI_START_ROW + row) + ":BV" + (SHOSAI_START_ROW + row + 2))
                .Attr.LineRight(AdvanceSoftware.VBReport8.BorderStyle.Thin, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("AK" + (SHOSAI_START_ROW + row) + ":AX" + (SHOSAI_START_ROW + row))
                .Attr.LineTop(AdvanceSoftware.VBReport8.BorderStyle.Hair, AdvanceSoftware.VBReport8.xlColor.Black);

            vbrList.Cell("AO" + (SHOSAI_START_ROW + row) + ":AX" + (SHOSAI_START_ROW + row))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);
            vbrList.Cell("AS" + (SHOSAI_START_ROW + row) + ":AT" + (SHOSAI_START_ROW + row + 2))
                .Attr.Box(AdvanceSoftware.VBReport8.BoxType.Ltc, AdvanceSoftware.VBReport8.BorderStyle.None, AdvanceSoftware.VBReport8.xlColor.Black);

            vbrList.Cell("**TokuisakiCode", 0, row).Value = string.Empty;
            vbrList.Cell("**TokuisakiName", 0, row).Value = string.Empty;
            vbrList.Cell("**HanroCode", 0, row).Value = string.Empty;
            vbrList.Cell("**HanroName", 0, row).Value = string.Empty;
            vbrList.Cell("**OfukuKbnName", 0, row).Value = string.Empty;
            vbrList.Cell("**StartPointCode", 0, row).Value = string.Empty;
            vbrList.Cell("**StartPointName", 0, row).Value = string.Empty;
            vbrList.Cell("**TaskStartDate", 0, row).Value = string.Empty;
            vbrList.Cell("**TaskStartTime", 0, row).Value = string.Empty;
            vbrList.Cell("**StartYMD", 0, row).Value = string.Empty;
            vbrList.Cell("**StartHH", 0, row).Value = string.Empty;
            vbrList.Cell("**EndPointCode", 0, row).Value = string.Empty;
            vbrList.Cell("**EndPointName", 0, row).Value = string.Empty;
            vbrList.Cell("**TaskEndDate", 0, row).Value = string.Empty;
            vbrList.Cell("**TaskEndTime", 0, row).Value = string.Empty;
            vbrList.Cell("**ReuseYMD", 0, row).Value = string.Empty;
            vbrList.Cell("**ReuseHH", 0, row).Value = string.Empty;
            vbrList.Cell("**ItemCode", 0, row).Value = string.Empty;
            vbrList.Cell("**ItemName", 0, row).Value = string.Empty;
            vbrList.Cell("**AtPrice", 0, row).Value = "【配車合計】";
            vbrList.Cell("**Weight", 0, row).Value = string.Empty;
            vbrList.Cell("**YoshasakiCode", 0, row).Value = string.Empty;
            vbrList.Cell("**YoshasakiName", 0, row).Value = string.Empty;
            vbrList.Cell("**Number", 0, row).Value = sumInfo.Number.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**UriageKingaku", 0, row).Value = sumInfo.PriceInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaKingaku", 0, row).Value = sumInfo.PriceInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarCode", 0, row).Value = string.Empty;
            vbrList.Cell("**LicPlateCarNo", 0, row).Value = string.Empty;
            vbrList.Cell("**Tsukoryo", 0, row).Value = sumInfo.TollFeeInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaTsukoryo", 0, row).Value = sumInfo.TollFeeInCharterPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**CarKindCode", 0, row).Value = string.Empty;
            vbrList.Cell("**CarKindName", 0, row).Value = string.Empty;
            vbrList.Cell("**ZeiKbn", 0, row).Value = string.Empty;
            vbrList.Cell("**JomuinUriage", 0, row).Value = sumInfo.JomuinUriageKingaku.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**YoshaZeiKbn", 0, row).Value = string.Empty;
            vbrList.Cell("**FutaiGyomuryo", 0, row).Value = sumInfo.FutaigyomuryoInPrice.ToString("###,###,###,###,###,##0");
            vbrList.Cell("**StaffCode", 0, row).Value = string.Empty;
            vbrList.Cell("**StaffName", 0, row).Value = string.Empty;
            vbrList.Cell("**AddUpYMD", 0, row).Value = string.Empty;
            vbrList.Cell("**CharterAddUpYMD", 0, row).Value = string.Empty;
            vbrList.Cell("**Biko", 0, row).Value = string.Empty;
            vbrList.Cell("**Kakutei", 0, row).Value = string.Empty;
            vbrList.Cell("**OwnerCode", 0, row).Value = string.Empty;
            vbrList.Cell("**OwnerName", 0, row).Value = string.Empty;
            vbrList.Cell("**YoshaKakutei", 0, row).Value = string.Empty;
            vbrList.Cell("**Magoyoshasaki", 0, row).Value = string.Empty;
            vbrList.Cell("**Juryozumi", 0, row).Value = string.Empty;
            vbrList.Cell("**JuchuTantoshaName", 0, row).Value = string.Empty;
            vbrList.Cell("**JuchuSlipNo", 0, row).Value = string.Empty;
        }

        #endregion
    }
}
