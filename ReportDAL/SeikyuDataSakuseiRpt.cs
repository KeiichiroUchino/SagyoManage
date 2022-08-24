using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model.DALExceptions;
using Jpsys.HaishaManageV10.SQLServerDAL;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.Property;

namespace Jpsys.HaishaManageV10.ReportDAL
{
    /// <summary>
    /// 請求データ作成のデータアクセスレイヤです。
    /// </summary>
    public class SeikyuDataSakuseiRpt
    {
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

        /// <summary>
        /// 請求データクラス
        /// </summary>
        private SeikyuData _SeikyuData;

        /// <summary>
        /// 日報ワーククラス
        /// </summary>
        private WK_DailyReport _WK_DailyReport;

        /// <summary>
        /// 売上ワーククラス
        /// </summary>
        private WK_Sale _WK_Sale;

        /// <summary>
        /// 管理情報の保持用
        /// </summary>
        private KanriInfo _KanriInfo = new KanriInfo();

        /// <summary>
        /// 一括INSERT件数
        /// </summary>
        private const int _InsertNumber = 20;

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public SeikyuDataSakuseiRpt()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SeikyuDataSakuseiRpt(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;

            //管理情報を取得
            this._KanriInfo = new Kanri(this.authInfo).GetInfo();

            //請求データクラス生成
            this._SeikyuData = new SeikyuData(this.authInfo);

            //日報ワーククラス生成
            this._WK_DailyReport = new WK_DailyReport(this.authInfo);

            //売上ワーククラス生成
            this._WK_Sale = new WK_Sale(this.authInfo);
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、締切済の得意先情報リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>締切済の得意先情報リスト</returns>
        public IList<String> GetClmFixDateList(SeikyuDataSakuseiConditionInfo para = null)
        {
            return GetClmFixDateListInternal(null, para);
        }

        /// <summary>
        /// 検索条件情報を指定して、支払済の取引先情報リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>支払済の取引先情報リスト</returns>
        public IList<String> GetCharterPayFixDateList(SeikyuDataSakuseiConditionInfo para = null)
        {
            return GetCharterPayFixDateListInternal(null, para);
        }

        /// <summary>
        /// 抽出条件情報を指定して、請求確認リストに関する配列を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">抽出条件情報</param>
        /// <returns>配列</returns>
        public IList<SeikyuKakuninListRptInfo> GetRptInfoList(SqlTransaction transaction,
            SeikyuDataSakuseiConditionInfo para)
        {
            //返却用リスト
            IList<SeikyuKakuninListRptInfo> rtlist = new List<SeikyuKakuninListRptInfo>();

            List<string> mySqlList = new List<string>();

            //日報ワーク初期化
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DELETE FROM ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" OperatorId = " + this.authInfo.OperatorId.ToString() + " ");

            mySqlList.Add(sb.ToString());

            //売上ワーク初期化
            sb = new StringBuilder();
            sb.AppendLine("DELETE FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" OperatorId = " + this.authInfo.OperatorId.ToString() + " ");

            mySqlList.Add(sb.ToString());

            //売上ワーク取得（配車Aceの対象配車データ）
            mySqlList.Add(this.GetInsertSqlIntoWK_Sale(para));

            //日報ワーク取得（トラDONに未登録分の日報データ）
            mySqlList.Add(this.GetInsertSqlIntoWK_DailyReport(para));

            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(mySqlList);

            // 指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));

            //請求データ取得
            rtlist = this.GetListInternal(transaction);

            //返却用リストを返却します
            return rtlist;
        }

        /// <summary>
        /// 作成条件情報を指定して、請求データを作成します。
        /// （配車Aceの配車「営業所ID、発日、車両ID、車番、車種ID、乗務員ID、傭車先ID」ごとにトラDONの日報と売上を1：Nで作成します）
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">作成条件情報</param>
        /// <param name="haitaInfo">請求データ情報</param>
        public void CreateSeikyuData(SqlTransaction transaction,
        SeikyuDataSakuseiConditionInfo para, SeikyuDataInfo haitaInfo)
        {
            //日報ワーク件数取得
            int dailyReportCnt = this._WK_DailyReport.GetCount(transaction);

            //日報ワーク件数が存在する場合
            if (0 < dailyReportCnt)
            {
                //日報IDリストを採番（トラDONのIDManageから取得）
                List<decimal> dailyReportIds = SQLHelper.GetTraDonSequenceIds(SQLHelper.SequenceIdKind.DailyReportTrn, dailyReportCnt);

                //日報ID更新（日報ワーク）
                this._WK_DailyReport.UpdateDailyReportIds(transaction, dailyReportIds[0]);

                //日報ID更新（売上ワーク）
                this._WK_Sale.UpdateDailyReportIds(transaction);
            }

            //売上件数取得
            int saleCnt = this._WK_Sale.GetCount(transaction);

            //売上IDリストを採番（トラDONのIDManageから取得）
            List<decimal> saleIds = SQLHelper.GetTraDonSequenceIds(SQLHelper.SequenceIdKind.SaleTrn, saleCnt);

            //トラDONの日報、売上を更新（トラDONの日報と売上に登録）
            this.InsertDailyReportAndSale(transaction, saleCnt, saleIds[0]);

            //配車請求連携IDリスト取得
            List<decimal> haishaSeikyuRenkeiIds = SQLHelper.GetSequenceIds(SQLHelper.SequenceIdKind.HaishaSeikyuRenkeiManageTrn, saleCnt);

            //配車請求連携管理の登録
            this.InsertHaishaSeikyuRenkeiManage(transaction, haishaSeikyuRenkeiIds[0], saleIds[0]);

            try
            {
                //排他制御
                //（請求データの登録／更新）
                this._SeikyuData.Save(transaction, haitaInfo);
            }
            catch (Exception e)
            {
                if (e is CanRetryException || e is UniqueConstraintException)
                {
                    throw new Model.DALExceptions.CanRetryException(
                        "他のユーザーが実行中のため処理を中断しました。\r\n現在の状況を確認後、処理をやり直してください。");
                }
                else
                {
                    throw e;
                }
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報を指定して、
        /// 売上ワーク情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>配車情報リスト</returns>
        private List<SeikyuKakuninListRptInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<SeikyuKakuninListRptInfo> rt_list = new List<SeikyuKakuninListRptInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            #region SQl文

            //SQL With
            sb.AppendLine("SELECT ");
            sb.AppendLine("  WK_Sale.* ");
            sb.AppendLine(" ,TORADON_BranchOffice.BranchOfficeCd ");
            sb.AppendLine(" ,TORADON_BranchOffice.BranchOfficeSNM ");
            sb.AppendLine(" ,TORADON_Car.CarCd ");
            sb.AppendLine(" ,TORADON_Car.CarKbn ");
            sb.AppendLine(" ,TORADON_CarKind.CarKindCd ");
            sb.AppendLine(" ,TORADON_CarKind.CarKindSNM ");
            sb.AppendLine(" ,TORADON_Staff.StaffCd ");
            sb.AppendLine(" ,TORADON_Staff.StaffNM ");
            sb.AppendLine(" ,TORADON_Tokuisaki.TokuisakiCd ");
            sb.AppendLine(" ,TORADON_Item.ItemCd ");
            sb.AppendLine(" ,TORADON_Fig.FigNM ");
            sb.AppendLine(" ,TORADON_Torihikisaki.TorihikiNM ");
            sb.AppendLine(" ,TORADON_Torihikisaki.TorihikiSNM ");
            sb.AppendLine(" ,TORADON_Point_Hatchi.PointCd StartPointCd ");
            sb.AppendLine(" ,TORADON_Point_Chakuchi.PointCd EndPointCd ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_BranchOffice ");
            sb.AppendLine(" ON TORADON_BranchOffice.BranchOfficeId = WK_Sale.BranchOfficeId ");
            sb.AppendLine(" AND TORADON_BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Car ");
            sb.AppendLine(" ON TORADON_Car.CarId = WK_Sale.CarId ");
            sb.AppendLine(" AND TORADON_Car.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_CarKind ");
            sb.AppendLine(" ON TORADON_CarKind.CarKindId = WK_Sale.CarKindId ");
            sb.AppendLine(" AND TORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Staff ");
            sb.AppendLine(" ON TORADON_Staff.StaffId = WK_Sale.DriverId ");
            sb.AppendLine(" AND TORADON_Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki ");
            sb.AppendLine(" ON TORADON_Tokuisaki.TokuisakiId = WK_Sale.TokuisakiId ");
            sb.AppendLine(" AND TORADON_Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Item ");
            sb.AppendLine(" ON TORADON_Item.ItemId = WK_Sale.ItemId ");
            sb.AppendLine(" AND TORADON_Item.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Fig ");
            sb.AppendLine(" ON TORADON_Fig.FigId = WK_Sale.FigId ");
            sb.AppendLine(" AND TORADON_Fig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki ");
            sb.AppendLine(" ON TORADON_Torihikisaki.TorihikiId = WK_Sale.CarOfChartererId ");
            sb.AppendLine(" AND TORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Point TORADON_Point_Hatchi ");
            sb.AppendLine(" ON TORADON_Point_Hatchi.PointId = WK_Sale.StartPointId ");
            sb.AppendLine(" AND TORADON_Point_Hatchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Point TORADON_Point_Chakuchi ");
            sb.AppendLine(" ON TORADON_Point_Chakuchi.PointId = WK_Sale.EndPointId ");
            sb.AppendLine(" AND TORADON_Point_Chakuchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine(" ON WK_DailyReport.DailyReportId = WK_Sale.DailyReportId ");
            sb.AppendLine(" AND WK_DailyReport.OperatorId = " + this.authInfo.OperatorId.ToString() + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_Sale.OperatorId = " + this.authInfo.OperatorId.ToString() + " ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SeikyuKakuninListRptInfo rt_info = new SeikyuKakuninListRptInfo
                {
                    OperatorId = SQLServerUtil.dbDecimal(rdr["OperatorId"]),
                    SaleId = SQLServerUtil.dbDecimal(rdr["SaleId"]),
                    DailyReportId = SQLServerUtil.dbDecimal(rdr["DailyReportId"]),
                    SaleSlipNo = SQLServerUtil.dbDecimal(rdr["SaleSlipNo"]),
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                    CarId = SQLServerUtil.dbDecimal(rdr["CarId"]),
                    CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]),
                    LicPlateCarNo = rdr["LicPlateCarNo"].ToString(),
                    DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]),
                    TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                    TokuisakiName = rdr["TokuisakiNM"].ToString(),
                    ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]),
                    ContractId = SQLServerUtil.dbDecimal(rdr["ContractId"]),
                    StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]),
                    StartPointName = rdr["StartPointNM"].ToString(),
                    EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]),
                    EndPointName = rdr["EndPointNM"].ToString(),
                    ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]),
                    ItemName = rdr["ItemNM"].ToString(),
                    OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]),
                    OwnerName = rdr["OwnerNM"].ToString(),
                    TaskStartDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["TaskStartDate"])),
                    TaskStartTime = SQLServerUtil.dbDecimal(rdr["TaskStartTime"]),
                    TaskEndDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["TaskEndDate"])),
                    TaskEndTime = SQLServerUtil.dbDecimal(rdr["TaskEndTime"]),
                    Number = SQLServerUtil.dbDecimal(rdr["Number"]),
                    FigId = SQLServerUtil.dbDecimal(rdr["FigId"]),
                    AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]),
                    Price = SQLServerUtil.dbDecimal(rdr["Price"]),
                    PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]),
                    TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]),
                    PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]),
                    PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]),
                    PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]),
                    PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]),
                    PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]),
                    TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]),
                    AddUpDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["AddUpDate"])),
                    FixFlag =
                        NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["FixFlag"])),
                    Memo = rdr["Memo"].ToString(),
                    ClmFixDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["ClmFixDate"])),
                    CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]),
                    CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]),
                    PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]),
                    TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]),
                    CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]),
                    CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]),
                    CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]),
                    CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]),
                    CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]),
                    CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]),
                    CharterAddUpDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["CharterAddUpDate"])),
                    CharterFixFlag =
                        NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["CharterFixFlag"])),
                    CharterPayFixDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["CharterPayFixDate"])),
                    Fee = SQLServerUtil.dbDecimal(rdr["Fee"]),
                    PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]),
                    TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]),
                    FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]),
                    FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]),
                    FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]),
                    FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]),
                    FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]),
                    Weight = SQLServerUtil.dbDecimal(rdr["Weight"]),

                    BranchOfficeCode = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]),
                    BranchOfficeShortName = rdr["BranchOfficeSNM"].ToString(),
                    CarCode = SQLServerUtil.dbInt(rdr["CarCd"]),
                    CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]),
                    CarKindShortName = rdr["CarKindSNM"].ToString(),
                    DriverCode = SQLServerUtil.dbInt(rdr["StaffCd"]),
                    DriverName = rdr["StaffNM"].ToString(),
                    TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]),
                    ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]),
                    FigName = rdr["FigNM"].ToString(),
                    CarOfChartererName = rdr["TorihikiNM"].ToString(),
                    CarOfChartererShortName = rdr["TorihikiSNM"].ToString(),
                    StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]),
                    EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]),
                    CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]),
                };

                //返却用の値を返します
                return rt_info;

            }, transaction);
        }

        /// <summary>
        /// 日報ワーク情報を作成するSQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>日報ワーク情報を作成するSQL</returns>
        private string GetInsertSqlIntoWK_DailyReport(SeikyuDataSakuseiConditionInfo para)
        {
            //常設列の取得オプションを作る
            //--新規登録
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  OperatorId ");
            sb.AppendLine(" ,DailyReportId ");
            sb.AppendLine(" ,CarDispatchId ");
            sb.AppendLine(" ,DailyReportDate ");
            sb.AppendLine(" ,BranchOfficeId ");
            sb.AppendLine(" ,WeatherId ");
            sb.AppendLine(" ,CarId ");
            sb.AppendLine(" ,LicPlateCarNo ");
            sb.AppendLine(" ,CarKindId ");
            sb.AppendLine(" ,CarOfChartererId ");
            sb.AppendLine(" ,DriverId ");
            sb.AppendLine(" ,StartDate ");
            sb.AppendLine(" ,StartTime ");
            sb.AppendLine(" ,StartMeter ");
            sb.AppendLine(" ,EndDate ");
            sb.AppendLine(" ,EndTime ");
            sb.AppendLine(" ,EndMeter ");
            sb.AppendLine(" ,WorkDays ");
            sb.AppendLine(" ,TransportTimes ");
            sb.AppendLine(" ,TransportWeight ");
            sb.AppendLine(" ,MileageKm ");
            sb.AppendLine(" ,ActualKm ");
            sb.AppendLine(" ,Memo ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" ) ");
            sb.AppendLine("SELECT ");
            sb.AppendLine("  " + this.authInfo.OperatorId.ToString() + " OperatorId, ");
            sb.AppendLine(" CONVERT(decimal, row_number() OVER( ");
            sb.AppendLine("  ORDER BY MAX(CONVERT(decimal, CONVERT(nvarchar,Haisha.StartYMD,112) + '000000')), MAX(Haisha.HaishaId))) - 1 DailyReportId, ");
            sb.AppendLine(" 0 CarDispatchId, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,MAX(Haisha.StartYMD),112) + '000000') DailyReportDate, ");
            sb.AppendLine(" WK_Sale.BranchOfficeId, ");
            sb.AppendLine(" 0 WeatherId, ");
            sb.AppendLine(" WK_Sale.CarId, ");
            sb.AppendLine(" WK_Sale.LicPlateCarNo, ");
            sb.AppendLine(" WK_Sale.CarKindId CarKindId, ");
            sb.AppendLine(" WK_Sale.CarOfChartererId, ");
            sb.AppendLine(" WK_Sale.DriverId, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,MIN(Haisha.StartYMD),112) + '000000') StartDate, ");
            sb.AppendLine(" CASE WHEN ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(MIN(Haisha.StartYMD), 'HH') * 60 + FORMAT(MAX(Haisha.StartYMD), 'mm')) = 0 ");
            sb.AppendLine(" THEN NULL ELSE ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(MIN(Haisha.StartYMD), 'HH') * 60 + FORMAT(MAX(Haisha.StartYMD), 'mm')) END StartTime, ");
            sb.AppendLine(" 0 StartMeter, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,MAX(Haisha.ReuseYMD),112) + '000000') EndDate, ");
            sb.AppendLine(" CASE WHEN ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(MAX(Haisha.ReuseYMD), 'HH') * 60 + FORMAT(MAX(Haisha.ReuseYMD), 'mm')) = 0 ");
            sb.AppendLine(" THEN NULL ELSE ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(MAX(Haisha.ReuseYMD), 'HH') * 60 + FORMAT(MAX(Haisha.ReuseYMD), 'mm')) END EndTime, ");
            sb.AppendLine(" 0 EndMeter, ");
            sb.AppendLine(" 0 WorkDays, ");
            sb.AppendLine(" 0 TransportTimes, ");
            sb.AppendLine(" 0 TransportWeight, ");
            sb.AppendLine(" 0 MileageKm, ");
            sb.AppendLine(" 0 ActualKm, ");
            sb.AppendLine(" '' Memo, ");
            sb.AppendLine(" " + SQLHelper.GetPopulateColumnInsertString(this.authInfo, popOption));
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" Haisha ");
            sb.AppendLine(" ON  Haisha.HaishaId = WK_Sale.SaleSlipNo "); //仮登録していた配車IDで繋ぐ
            sb.AppendLine(" AND Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("     WK_Sale.OperatorId = " + this.authInfo.OperatorId.ToString() + " ");
            sb.AppendLine(" AND WK_Sale.DailyReportId = 0 ");

            sb.AppendLine("GROUP BY ");
            sb.AppendLine("WK_Sale.BranchOfficeId, CONVERT(decimal, CONVERT(nvarchar,Haisha.StartYMD,112) + '000000'), ");
            sb.AppendLine("WK_Sale.CarId, WK_Sale.CarKindId, WK_Sale.CarOfChartererId, WK_Sale.DriverId, WK_Sale.LicPlateCarNo ");

            return sb.ToString();
        }

        /// <summary>
        /// SqlTransaction情報、配車請求連携管理IDを指定して、
        /// 配車請求連携管理情報の登録を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="id">配車請求連携管理ID</param>
        /// <param name="saleid">売上ID</param>
        private int InsertHaishaSeikyuRenkeiManage(SqlTransaction transaction, decimal id, decimal saleid)
        {
            //常設列の取得オプションを取得
            //--新規登録
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("HaishaSeikyuRenkeiManage ");
            sb.AppendLine("(");
            sb.AppendLine("HaishaSeikyuRenkeiManageId, ");
            sb.AppendLine("HaishaId, ");
            sb.AppendLine("ToraDONSaleId, ");
            sb.AppendLine(SQLHelper.GetPopulateColumnSelectionString(popOption) + " ");

            sb.AppendLine(") ");
            sb.AppendLine("SELECT ");
            sb.AppendLine("SaleId + " + id.ToString() + " HaishaSeikyuRenkeiManageId, ");
            sb.AppendLine("SaleSlipNo, ");
            sb.AppendLine("SaleId + " + saleid.ToString() + " ToraDONSaleId, ");
            sb.AppendLine(SQLHelper.GetPopulateColumnInsertString(this.authInfo, popOption) + " ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_Sale.OperatorId = " + this.authInfo.OperatorId.ToString() + " ");

            string sql = sb.ToString();

            //指定したトランザクション上でExecuteNonqueryを実行
            return SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(sql));
        }

        /// <summary>
        /// 売上ワーク情報を作成するSQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>売上ワーク情報を作成するSQL</returns>
        private string GetInsertSqlIntoWK_Sale(SeikyuDataSakuseiConditionInfo para)
        {
            //常設列の取得オプションを作る
            //--新規登録
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //売上ワーク取得
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  OperatorId ");
            sb.AppendLine(" ,SaleId ");
            sb.AppendLine(" ,DailyReportId ");
            sb.AppendLine(" ,SaleSlipNo ");
            sb.AppendLine(" ,BranchOfficeId ");
            sb.AppendLine(" ,CarId ");
            sb.AppendLine(" ,LicPlateCarNo ");
            sb.AppendLine(" ,CarKindId ");
            sb.AppendLine(" ,DriverId ");
            sb.AppendLine(" ,TokuisakiId ");
            sb.AppendLine(" ,TokuisakiNM ");
            sb.AppendLine(" ,ClmClassId ");
            sb.AppendLine(" ,ContractId ");
            sb.AppendLine(" ,StartPointId ");
            sb.AppendLine(" ,StartPointNM ");
            sb.AppendLine(" ,EndPointId ");
            sb.AppendLine(" ,EndPointNM ");
            sb.AppendLine(" ,ItemId ");
            sb.AppendLine(" ,ItemNM ");
            sb.AppendLine(" ,OwnerId ");
            sb.AppendLine(" ,OwnerNM ");
            sb.AppendLine(" ,TaskStartDate ");
            sb.AppendLine(" ,TaskStartTime ");
            sb.AppendLine(" ,TaskEndDate ");
            sb.AppendLine(" ,TaskEndTime ");
            sb.AppendLine(" ,Number ");
            sb.AppendLine(" ,FigId ");
            sb.AppendLine(" ,AtPrice ");
            sb.AppendLine(" ,Price ");
            sb.AppendLine(" ,PriceInPrice ");
            sb.AppendLine(" ,TollFeeInPrice ");
            sb.AppendLine(" ,PriceOutTaxCalc ");
            sb.AppendLine(" ,PriceOutTax ");
            sb.AppendLine(" ,PriceInTaxCalc ");
            sb.AppendLine(" ,PriceInTax ");
            sb.AppendLine(" ,PriceNoTaxCalc ");
            sb.AppendLine(" ,TaxDispKbn ");
            sb.AppendLine(" ,AddUpDate ");
            sb.AppendLine(" ,FixFlag ");
            sb.AppendLine(" ,Memo ");
            sb.AppendLine(" ,ClmFixDate ");
            sb.AppendLine(" ,CarOfChartererId ");
            sb.AppendLine(" ,CharterPrice ");
            sb.AppendLine(" ,PriceInCharterPrice ");
            sb.AppendLine(" ,TollFeeInCharterPrice ");
            sb.AppendLine(" ,CharterPriceOutTaxCalc ");
            sb.AppendLine(" ,CharterPriceOutTax ");
            sb.AppendLine(" ,CharterPriceInTaxCalc ");
            sb.AppendLine(" ,CharterPriceInTax ");
            sb.AppendLine(" ,CharterPriceNoTaxCalc ");
            sb.AppendLine(" ,CharterTaxDispKbn ");
            sb.AppendLine(" ,CharterAddUpDate ");
            sb.AppendLine(" ,CharterFixFlag ");
            sb.AppendLine(" ,CharterPayFixDate ");
            sb.AppendLine(" ,Fee ");
            sb.AppendLine(" ,PriceInFee ");
            sb.AppendLine(" ,TollFeeInFee ");
            sb.AppendLine(" ,FeeOutTaxCalc ");
            sb.AppendLine(" ,FeeOutTax ");
            sb.AppendLine(" ,FeeInTaxCalc ");
            sb.AppendLine(" ,FeeInTax ");
            sb.AppendLine(" ,FeeNoTaxCalc ");
            sb.AppendLine(" ,Weight ");
            sb.AppendLine(" ,DelFlag ");
            sb.AppendLine(" ,TaikijikanNumber ");
            sb.AppendLine(" ,TaikijikanFigId ");
            sb.AppendLine(" ,TaikijikanAtPrice ");
            sb.AppendLine(" ,TaikijikanryoInPrice ");
            sb.AppendLine(" ,NizumiryoInPrice ");
            sb.AppendLine(" ,NioroshiryoInPrice ");
            sb.AppendLine(" ,FutaigyomuryoInPrice ");
            sb.AppendLine(" ,TaikijikanryoInFee ");
            sb.AppendLine(" ,NizumiryoInFee ");
            sb.AppendLine(" ,NioroshiryoInFee ");
            sb.AppendLine(" ,FutaigyomuryoInFee ");
            sb.AppendLine(" ,NizumijikanNumber ");
            sb.AppendLine(" ,NizumijikanFigId ");
            sb.AppendLine(" ,NizumijikanAtPrice ");
            sb.AppendLine(" ,NioroshijikanNumber ");
            sb.AppendLine(" ,NioroshijikanFigId ");
            sb.AppendLine(" ,NioroshijikanAtPrice ");
            sb.AppendLine(" ,FutaigyomujikanNumber ");
            sb.AppendLine(" ,FutaigyomujikanFigId ");
            sb.AppendLine(" ,FutaigyomujikanAtPrice ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" ) ");
            sb.AppendLine("SELECT ");
            sb.AppendLine("  " + this.authInfo.OperatorId.ToString() + " OperatorId, ");
            sb.AppendLine(" CONVERT(decimal, row_number() OVER( ");
            sb.AppendLine("  ORDER BY Haisha.TaskStartDateTime, Haisha.TaskEndDateTime, Haisha.HaishaId)) - 1 SaleId, ");
            sb.AppendLine(" ISNULL(TORADON_DailyReport.DailyReportId, 0) DailyReportId, ");
            sb.AppendLine(" Haisha.HaishaId, "); //配車IDを仮登録（配車請求連携管理の作成に利用）
            sb.AppendLine(" Juchu.BranchOfficeId, ");
            sb.AppendLine(" Haisha.CarId, ");
            sb.AppendLine(" CASE WHEN ISNULL(TORADON_Car.CarKbn, 0) = "
                + ((int)DefaultProperty.CarKbn.Yosha).ToString()
                + " THEN Haisha.LicPlateCarNo ELSE ISNULL(TORADON_Car.LicPlateCarNo, '') END LicPlateCarNo, ");
            sb.AppendLine(" CASE WHEN ISNULL(RenkeiCarKind.CarKindId,0) = 0 THEN Haisha.CarKindId ELSE ISNULL(RenkeiCarKind.CarKindId,0) END CarKindId, ");
            sb.AppendLine(" Haisha.DriverId, ");
            sb.AppendLine(" Juchu.TokuisakiId, ");
            sb.AppendLine(" Juchu.TokuisakiNM, ");
            sb.AppendLine(" Juchu.ClmClassId, ");
            sb.AppendLine(" Juchu.ContractId, ");
            sb.AppendLine(" Haisha.StartPointId, ");
            sb.AppendLine(" Haisha.StartPointNM, ");
            sb.AppendLine(" Haisha.EndPointId, ");
            sb.AppendLine(" Haisha.EndPointNM, ");
            sb.AppendLine(" Haisha.ItemId, ");
            sb.AppendLine(" Haisha.ItemNM, ");
            sb.AppendLine(" Haisha.OwnerId, ");
            sb.AppendLine(" Haisha.OwnerNM, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,Haisha.StartYMD,112) + '000000') TaskStartDate, ");
            sb.AppendLine(" CASE WHEN ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(Haisha.StartYMD, 'HH') * 60 + FORMAT(Haisha.StartYMD, 'mm')) = 0 ");
            sb.AppendLine(" THEN NULL ELSE ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(Haisha.StartYMD, 'HH') * 60 + FORMAT(Haisha.StartYMD, 'mm')) END TaskStartTime, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,Haisha.TaskEndDateTime,112) + '000000') TaskEndDate, ");
            sb.AppendLine(" CASE WHEN ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(Haisha.TaskEndDateTime, 'HH') * 60 + FORMAT(Haisha.TaskEndDateTime, 'mm')) = 0 ");
            sb.AppendLine(" THEN NULL ELSE ");
            sb.AppendLine("  CONVERT(decimal, FORMAT(Haisha.TaskEndDateTime, 'HH') * 60 + FORMAT(Haisha.TaskEndDateTime, 'mm')) END TaskEndTime, ");
            sb.AppendLine(" Haisha.Number, ");
            sb.AppendLine(" Haisha.FigId, ");
            sb.AppendLine(" Haisha.AtPrice, ");
            sb.AppendLine(" Haisha.Price, ");
            sb.AppendLine(" Haisha.PriceInPrice, ");
            sb.AppendLine(" Haisha.TollFeeInPrice, ");
            sb.AppendLine(" Haisha.PriceOutTaxCalc, ");
            sb.AppendLine(" Haisha.PriceOutTax, ");
            sb.AppendLine(" Haisha.PriceInTaxCalc, ");
            sb.AppendLine(" Haisha.PriceInTax, ");
            sb.AppendLine(" Haisha.PriceNoTaxCalc, ");
            sb.AppendLine(" Haisha.TaxDispKbn, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,Haisha.AddUpYMD,112) + '000000') ClmFixDate, ");
            sb.AppendLine(" Juchu.FixFlag, ");
            sb.AppendLine(" Juchu.Memo, ");
            sb.AppendLine(" 0 ClmFixDate, ");
            sb.AppendLine(" Haisha.CarOfChartererId, ");
            sb.AppendLine(" Haisha.CharterPrice, ");
            sb.AppendLine(" Haisha.PriceInCharterPrice, ");
            sb.AppendLine(" Haisha.TollFeeInCharterPrice, ");
            sb.AppendLine(" Haisha.CharterPriceOutTaxCalc, ");
            sb.AppendLine(" Haisha.CharterPriceOutTax, ");
            sb.AppendLine(" Haisha.CharterPriceInTaxCalc, ");
            sb.AppendLine(" Haisha.CharterPriceInTax, ");
            sb.AppendLine(" Haisha.CharterPriceNoTaxCalc, ");
            sb.AppendLine(" Haisha.CharterTaxDispKbn, ");
            sb.AppendLine(" CONVERT(decimal, CONVERT(nvarchar,Haisha.CharterAddUpYMD,112) + '000000') CharterPayFixDate, ");
            sb.AppendLine(" Juchu.CharterFixFlag, ");
            sb.AppendLine(" 0 CharterPayFixDate, ");
            sb.AppendLine(" Haisha.Fee, ");
            sb.AppendLine(" Haisha.PriceInFee, ");
            sb.AppendLine(" Haisha.TollFeeInFee, ");
            sb.AppendLine(" Haisha.FeeOutTaxCalc, ");
            sb.AppendLine(" Haisha.FeeOutTax, ");
            sb.AppendLine(" Haisha.FeeInTaxCalc, ");
            sb.AppendLine(" Haisha.FeeInTax, ");
            sb.AppendLine(" Haisha.FeeNoTaxCalc, ");
            sb.AppendLine(" Haisha.Weight, ");
            sb.AppendLine(" 0 DelFlag, ");
            sb.AppendLine(" 0 TaikijikanNumber, ");
            sb.AppendLine(" 0 TaikijikanFigId, ");
            sb.AppendLine(" 0 TaikijikanAtPrice, ");
            sb.AppendLine(" 0 TaikijikanryoInPrice, ");
            sb.AppendLine(" 0 NizumiryoInPrice, ");
            sb.AppendLine(" 0 NioroshiryoInPrice, ");
            sb.AppendLine(" Haisha.FutaigyomuryoInPrice, ");
            sb.AppendLine(" 0 TaikijikanryoInFee, ");
            sb.AppendLine(" 0 NizumiryoInFee, ");
            sb.AppendLine(" 0 NioroshiryoInFee, ");
            sb.AppendLine(" Haisha.FutaigyomuryoInFee, ");
            sb.AppendLine(" 0 NizumijikanNumber, ");
            sb.AppendLine(" 0 NizumijikanFigId, ");
            sb.AppendLine(" 0 NizumijikanAtPrice, ");
            sb.AppendLine(" 0 NioroshijikanNumber, ");
            sb.AppendLine(" 0 NioroshijikanFigId, ");
            sb.AppendLine(" 0 NioroshijikanAtPrice, ");
            sb.AppendLine(" 0 FutaigyomujikanNumber, ");
            sb.AppendLine(" 0 FutaigyomujikanFigId, ");
            sb.AppendLine(" 0 FutaigyomujikanAtPrice, ");
            sb.AppendLine(" " + SQLHelper.GetPopulateColumnInsertString(this.authInfo, popOption));
            sb.AppendLine("FROM ");
            sb.AppendLine(" Juchu ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" Haisha ");
            sb.AppendLine(" ON  Haisha.JuchuId = Juchu.JuchuId ");
            sb.AppendLine(" AND Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" TORADON_Car ");
            sb.AppendLine(" ON TORADON_Car.CarId = Haisha.CarId ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" TORADON_CarKind ");
            sb.AppendLine(" ON TORADON_CarKind.CarKindId = Haisha.CarKindId ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" CarKind ");
            sb.AppendLine(" ON CarKind.ToraDONCarKindId = TORADON_CarKind.CarKindId ");
            sb.AppendLine(" AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_CarKind RenkeiCarKind ");
            sb.AppendLine(" ON RenkeiCarKind.CarKindId = CarKind.ToraDONRenkeiCarKindId ");
            sb.AppendLine(" AND RenkeiCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  SELECT ");
            sb.AppendLine("    MIN(TORADON_DailyReport.DailyReportId) DailyReportId ");
            sb.AppendLine("   ,TORADON_DailyReport.BranchOfficeId ");
            sb.AppendLine("   ,TORADON_DailyReport.DailyReportDate ");
            sb.AppendLine("   ,TORADON_DailyReport.CarId ");
            sb.AppendLine("   ,TORADON_DailyReport.CarKindId ");
            sb.AppendLine("   ,TORADON_DailyReport.CarOfChartererId ");
            sb.AppendLine("   ,TORADON_DailyReport.DriverId ");
            sb.AppendLine("   ,TORADON_DailyReport.LicPlateCarNo ");
            sb.AppendLine("  FROM ");
            sb.AppendLine("   TORADON_DailyReport ");
            sb.AppendLine("  WHERE ");
            sb.AppendLine("      TORADON_DailyReport.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("  AND TORADON_DailyReport.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");
            sb.AppendLine("  AND TORADON_DailyReport.DailyReportDate >= "
                + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom.AddMonths(-1)).ToString() + " "); //性能対策のため1ヶ月前までを日報検索対象とする
            sb.AppendLine("  GROUP BY ");
            sb.AppendLine("    TORADON_DailyReport.BranchOfficeId ");
            sb.AppendLine("   ,TORADON_DailyReport.DailyReportDate ");
            sb.AppendLine("   ,TORADON_DailyReport.CarId ");
            sb.AppendLine("   ,TORADON_DailyReport.CarKindId ");
            sb.AppendLine("   ,TORADON_DailyReport.CarOfChartererId ");
            sb.AppendLine("   ,TORADON_DailyReport.DriverId ");
            sb.AppendLine("   ,TORADON_DailyReport.LicPlateCarNo ");
            sb.AppendLine(" ) TORADON_DailyReport ");
            sb.AppendLine(" ON  TORADON_DailyReport.BranchOfficeId = Juchu.BranchOfficeId ");
            sb.AppendLine(" AND TORADON_DailyReport.DailyReportDate = CONVERT(decimal, CONVERT(nvarchar,Haisha.StartYMD,112) + '000000') ");
            sb.AppendLine(" AND TORADON_DailyReport.CarId = Haisha.CarId ");
            sb.AppendLine(" AND TORADON_DailyReport.CarKindId = CASE WHEN ISNULL(RenkeiCarKind.CarKindId,0) = 0 THEN Haisha.CarKindId ELSE ISNULL(RenkeiCarKind.CarKindId,0) END ");
            sb.AppendLine(" AND TORADON_DailyReport.CarOfChartererId = Haisha.CarOfChartererId ");
            sb.AppendLine(" AND TORADON_DailyReport.DriverId = Haisha.DriverId ");
            sb.AppendLine(" AND TORADON_DailyReport.LicPlateCarNo = ");
            sb.AppendLine(" CASE WHEN ISNULL(TORADON_Car.CarKbn, 0) = "
                + ((int)DefaultProperty.CarKbn.Yosha).ToString()
                + " THEN Haisha.LicPlateCarNo ELSE ISNULL(TORADON_Car.LicPlateCarNo, '') END ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" AND NOT EXISTS ( ");
            sb.AppendLine("  SELECT 1 FROM ");
            sb.AppendLine("   HaishaSeikyuRenkeiManage ");
            sb.AppendLine("  WHERE ");
            sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId ");
            sb.AppendLine(" ) ");
            sb.AppendLine(" AND NOT ( ");
            sb.AppendLine("      ISNULL(Haisha.PriceInPrice, 0) = 0 ");
            sb.AppendLine("  AND ISNULL(Haisha.TollFeeInPrice, 0) = 0 ");
            sb.AppendLine("  AND ISNULL(Haisha.PriceInCharterPrice, 0) = 0 ");
            sb.AppendLine("  AND ISNULL(Haisha.TollFeeInCharterPrice, 0) = 0 ");
            sb.AppendLine("  AND ISNULL(Haisha.FutaigyomuryoInFee, 0) = 0 ");
            sb.AppendLine(" ) ");

            sb.AppendLine("	--引数「営業所」 ");
            sb.AppendLine(" AND Juchu.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");

            switch (para.SeikyuRenkeiHizukeKbn)
            {
                case DefaultProperty.SeikyuRenkeiHizukeKbn.AddUpYMD:
                    sb.AppendLine("	--引数「計上日（範囲開始）」 ");
                    sb.AppendLine(" AND Haisha.AddUpYMD >= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDFrom).ToString() + " ");
                    sb.AppendLine("	--引数「計上日（範囲終了）」 ");
                    sb.AppendLine(" AND Haisha.AddUpYMD <= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDTo).ToString() + " ");
                    break;
                case DefaultProperty.SeikyuRenkeiHizukeKbn.StartYMD:
                    sb.AppendLine("	--引数「発日（範囲開始）」 ");
                    sb.AppendLine(" AND Haisha.StartYMD >= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDFrom).ToString() + " ");
                    sb.AppendLine("	--引数「発日（範囲終了）」 ");
                    sb.AppendLine(" AND Haisha.StartYMD < " + SQLHelper.DateTimeToDbDate(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                    break;
                case DefaultProperty.SeikyuRenkeiHizukeKbn.ChakuYMD:
                    sb.AppendLine("	--引数「着日（範囲開始）」 ");
                    sb.AppendLine(" AND Haisha.TaskEndDateTime >= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDFrom).ToString() + " ");
                    sb.AppendLine("	--引数「着日（範囲終了）」 ");
                    sb.AppendLine(" AND Haisha.TaskEndDateTime < " + SQLHelper.DateTimeToDbDate(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                    break;
                default:
                    break;
            }

            return sb.ToString();
        }

        /// <summary>
        /// SqlTransaction情報を指定して、
        /// 日報情報の登録を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="saleCnt">売上件数</param>
        /// <param name="saleId">売上ID</param>
        private void InsertDailyReportAndSale(SqlTransaction transaction,
            int saleCnt, decimal saleId)
        {
            //登録SQLリスト
            List<string> mySqlAllList = new List<string>();

            //日報ワークリスト取得
            IList<WK_DailyReportInfo> wkDailyReportList = this._WK_DailyReport.GetList(transaction);

            foreach (WK_DailyReportInfo info in wkDailyReportList)
            {
                //日報登録SQL取得
                mySqlAllList.Add(this.GetInsertDailyReportSql(info));
            }

            //売上ワークリスト取得
            IList<WK_SaleInfo> wkSaleList = this._WK_Sale.GetList(transaction);

            foreach (WK_SaleInfo info in wkSaleList)
            {
                //売上登録SQL取得
                mySqlAllList.Add(this.GetInsertSaleSql(info, saleId));
            }

            //トラDONのコネクションを取得
            string connString = SQLHelper.GetConnectionStringTraDon();

            // 接続する
            using (SqlConnection conn = new SqlConnection(connString))
            {
                conn.Open();

                // トランザクションを開く
                using (SqlTransaction tx = conn.BeginTransaction())
                {
                    try
                    {
                        #region 指定件数毎トラDON登録処理

                        int i = 0;
                        List<string> mySqlList = new List<string>();

                        foreach (string sql in mySqlAllList)
                        {
                            mySqlList.Add(sql);
                            i++;

                            if (i % _InsertNumber == 0 || mySqlAllList.Count == i)
                            {
                                // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                                string query = SQLHelper.SQLQueryJoin(mySqlList);
                                SQLHelper.ExecuteNonQueryOnTransaction(tx, new SqlCommand(query));
                                mySqlList.Clear();
                            }
                        }

                        #endregion

                        // コミット
                        tx.Commit();
                    }
                    catch (Exception ex)
                    {
                        tx.Rollback();
                        throw ex;
                    }
                }
            }
        }

        /// <summary>
        /// 日報ワーク情報を指定して、
        /// 日報情報を作成するSQLを取得します。
        /// </summary>
        /// <param name="info">日報ワーク情報</param>
        /// <returns>日報情報を作成するSQL</returns>
        private string GetInsertDailyReportSql(WK_DailyReportInfo info)
        {
            //常設列の取得オプションを取得
            //--新規登録
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //戻り値
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("DailyReport ");
            sb.AppendLine("( ");
            sb.AppendLine("DailyReportId, CarDispatchId, DailyReportDate, BranchOfficeId, WeatherId, ");
            sb.AppendLine("CarId, LicPlateCarNo, CarKindId, CarOfChartererId, DriverId, ");
            sb.AppendLine("StartDate, StartTime, StartMeter, EndDate, EndTime, EndMeter, ");
            sb.AppendLine("WorkDays, TransportTimes, TransportWeight, MileageKm, ActualKm, ");
            sb.AppendLine("Memo,  ");
            sb.AppendLine("DelFlag, ");
            sb.AppendLine(SQLHelper.GetPopulateColumnSelectionString(popOption) + " ");
            sb.AppendLine(") ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine(info.DailyReportId.ToString() + ", ");
            sb.AppendLine(info.CarDispatchId.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.DailyReportDate).ToString() + ", ");
            sb.AppendLine(info.BranchOfficeId.ToString() + ", ");
            sb.AppendLine(info.WeatherId.ToString() + ", ");
            sb.AppendLine(info.CarId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.LicPlateCarNo.Trim()) + "', ");
            sb.AppendLine(info.CarKindId.ToString() + ", ");
            sb.AppendLine(info.CarOfChartererId.ToString() + ", ");
            sb.AppendLine(info.DriverId.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.StartDate).ToString() + ", ");
            sb.AppendLine(
                SQLHelper.TimeSpanTodbCustomDecimalString(
                    info.StartTime) + ", ");
            sb.AppendLine(info.StartMeter.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.EndDate).ToString() + ", ");
            sb.AppendLine(
                SQLHelper.TimeSpanTodbCustomDecimalString(
                    info.EndTime) + ", ");
            sb.AppendLine(info.EndMeter.ToString() + ", ");
            sb.AppendLine(info.WorkDays.ToString() + ", ");
            sb.AppendLine(info.TransportTimes.ToString() + ", ");
            sb.AppendLine(info.TransportWeight.ToString() + ", ");
            sb.AppendLine(info.MileageKm.ToString() + ", ");
            sb.AppendLine(info.ActualKm.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.Memo.Trim()) + "', ");
            sb.AppendLine(
                NSKUtil.BoolToInt(false).ToString() + ", ");
            sb.AppendLine(
                SQLHelper.GetPopulateColumnInsertStringForTraDon(this.authInfo, popOption));
            sb.AppendLine(") ");

            return sb.ToString();
        }

        /// <summary>
        /// 売上ワーク情報を指定して、
        /// 売上情報を作成するSQLを取得します。
        /// </summary>
        /// <param name="info">売上ワーク情報</param>
        /// <param name="saleId">売上ID</param>
        /// <returns>売上情報を作成するSQL</returns>
        private string GetInsertSaleSql(WK_SaleInfo info, decimal saleId)
        {
            //常設列の取得オプションを取得
            //--新規登録
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //戻り値
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine("Sale ");
            sb.AppendLine("(");
            sb.AppendLine("SaleId, DailyReportId, SaleSlipNo, ");
            sb.AppendLine("BranchOfficeId, CarId, LicPlateCarNo, CarKindId, DriverId, ");
            sb.AppendLine("TokuisakiId, TokuisakiNM, ClmClassId, ContractId,  ");
            sb.AppendLine("StartPointId, StartPointNM, EndPointId, EndPointNM, ");
            sb.AppendLine("ItemId, ItemNM, OwnerId, OwnerNM, TaskStartDate, TaskStartTime, TaskEndDate, ");
            sb.AppendLine("TaskEndTime, Number, FigId, AtPrice, Price, PriceInPrice, TollFeeInPrice, PriceOutTaxCalc, ");
            sb.AppendLine("PriceOutTax, PriceInTaxCalc, PriceInTax, PriceNoTaxCalc, TaxDispKbn, ");
            sb.AppendLine("AddUpDate, FixFlag, Memo, ClmFixDate, CarOfChartererId, CharterPrice, PriceInCharterPrice, TollFeeInCharterPrice, ");
            sb.AppendLine("CharterPriceOutTaxCalc, CharterPriceOutTax, CharterPriceInTaxCalc, ");
            sb.AppendLine("CharterPriceInTax, CharterPriceNoTaxCalc, CharterTaxDispKbn, ");
            sb.AppendLine("CharterAddUpDate, CharterFixFlag, CharterPayFixDate, ");
            sb.AppendLine("Fee, PriceInFee, TollFeeInFee, FeeOutTaxCalc, FeeOutTax, FeeInTaxCalc, FeeInTax, FeeNoTaxCalc, ");
            sb.AppendLine("Weight, DelFlag, ");
            // トラDON_V50の場合
            if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                == (int)DefaultProperty.TraDonVersionKbn.V50)
            {
                sb.AppendLine("TaikijikanNumber, ");
                sb.AppendLine("TaikijikanFigId, ");
                sb.AppendLine("TaikijikanAtPrice, ");
                sb.AppendLine("TaikijikanryoInPrice, ");
                sb.AppendLine("NizumiryoInPrice, ");
                sb.AppendLine("NioroshiryoInPrice, ");
                sb.AppendLine("FutaigyomuryoInPrice, ");
                sb.AppendLine("TaikijikanryoInFee, ");
                sb.AppendLine("NizumiryoInFee, ");
                sb.AppendLine("NioroshiryoInFee, ");
                sb.AppendLine("FutaigyomuryoInFee, ");
                sb.AppendLine("NizumijikanNumber, ");
                sb.AppendLine("NizumijikanFigId, ");
                sb.AppendLine("NizumijikanAtPrice, ");
                sb.AppendLine("NioroshijikanNumber, ");
                sb.AppendLine("NioroshijikanFigId, ");
                sb.AppendLine("NioroshijikanAtPrice, ");
                sb.AppendLine("FutaigyomujikanNumber, ");
                sb.AppendLine("FutaigyomujikanFigId, ");
                sb.AppendLine("FutaigyomujikanAtPrice, ");
            }
            sb.AppendLine(SQLHelper.GetPopulateColumnSelectionString(popOption) + " ");

            sb.AppendLine(") ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");

            sb.AppendLine(info.SaleId.ToString() + " + " + saleId.ToString() + ", ");
            sb.AppendLine(info.DailyReportId.ToString() + ", ");
            sb.AppendLine(info.SaleSlipNo.ToString() + ", ");
            sb.AppendLine(info.BranchOfficeId.ToString() + ", ");
            sb.AppendLine(info.CarId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.LicPlateCarNo.Trim()) + "', ");
            sb.AppendLine(info.CarKindId.ToString() + ", ");
            sb.AppendLine(info.DriverId.ToString() + ", ");
            sb.AppendLine(info.TokuisakiId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.TokuisakiName.Trim()) + "', ");
            sb.AppendLine(info.ClmClassId.ToString() + ", ");
            sb.AppendLine(info.ContractId.ToString() + ", ");
            sb.AppendLine(info.StartPointId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.StartPointName.Trim()) + "', ");
            sb.AppendLine(info.EndPointId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.EndPointName.Trim()) + "', ");
            sb.AppendLine(info.ItemId.ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.ItemName.Trim()) + "', ");
            sb.AppendLine(info.OwnerId.ToString() + ", ");
            sb.AppendLine("N'" +
               SQLHelper.GetSanitaizingSqlString(
                   info.OwnerName.Trim()) + "', ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.TaskStartDate).ToString() + ", ");
            sb.AppendLine(
                SQLHelper.TimeSpanTodbCustomDecimalString(
                    info.TaskStartTime) + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.TaskEndDate).ToString() + ", ");
            sb.AppendLine(
                SQLHelper.TimeSpanTodbCustomDecimalString(
                    info.TaskEndTime) + ", ");
            sb.AppendLine(info.Number.ToString() + ", ");
            sb.AppendLine(info.FigId.ToString() + ", ");
            sb.AppendLine(info.AtPrice.ToString() + ", ");
            sb.AppendLine(info.Price.ToString() + ", ");
            sb.AppendLine(info.PriceInPrice.ToString() + ", ");
            sb.AppendLine(info.TollFeeInPrice.ToString() + ", ");
            sb.AppendLine(info.PriceOutTaxCalc.ToString() + ", ");
            sb.AppendLine(info.PriceOutTax.ToString() + ", ");
            sb.AppendLine(info.PriceInTaxCalc.ToString() + ", ");
            sb.AppendLine(info.PriceInTax.ToString() + ", ");
            sb.AppendLine(info.PriceNoTaxCalc.ToString() + ", ");
            sb.AppendLine(info.TaxDispKbn.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.AddUpDate).ToString() + ", ");
            sb.AppendLine(
                NSKUtil.BoolToInt(info.FixFlag).ToString() + ", ");
            sb.AppendLine("N'" +
                SQLHelper.GetSanitaizingSqlString(
                    info.Memo.Trim()) + "', ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.ClmFixDate).ToString() + ", ");
            sb.AppendLine(info.CarOfChartererId.ToString() + ", ");
            sb.AppendLine(info.CharterPrice.ToString() + ", ");
            sb.AppendLine(info.PriceInCharterPrice.ToString() + ", ");
            sb.AppendLine(info.TollFeeInCharterPrice.ToString() + ", ");
            sb.AppendLine(info.CharterPriceOutTaxCalc.ToString() + ", ");
            sb.AppendLine(info.CharterPriceOutTax.ToString() + ", ");
            sb.AppendLine(info.CharterPriceInTaxCalc.ToString() + ", ");
            sb.AppendLine(info.CharterPriceInTax.ToString() + ", ");
            sb.AppendLine(info.CharterPriceNoTaxCalc.ToString() + ", ");
            sb.AppendLine(info.CharterTaxDispKbn.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.CharterAddUpDate).ToString() + ", ");
            sb.AppendLine(
                NSKUtil.BoolToInt(info.CharterFixFlag).ToString() + ", ");
            sb.AppendLine(
                NSKUtil.DateTimeToDecimalWithTime(
                    info.CharterPayFixDate).ToString() + ", ");
            sb.AppendLine(info.Fee.ToString() + ", ");
            sb.AppendLine(info.PriceInFee.ToString() + ", ");
            sb.AppendLine(info.TollFeeInFee.ToString() + ", ");
            sb.AppendLine(
                info.FeeOutTaxCalc.ToString() + ", ");
            sb.AppendLine(
               info.FeeOutTax.ToString() + ", ");
            sb.AppendLine(
               info.FeeInTaxCalc.ToString() + ", ");
            sb.AppendLine(
               info.FeeInTax.ToString() + ", ");
            sb.AppendLine(
               info.FeeNoTaxCalc.ToString() + ", ");
            sb.AppendLine(
                info.Weight.ToString() + ", ");
            sb.AppendLine(
                NSKUtil.BoolToInt(info.DelFlag).ToString() + ", ");
            // トラDON_V50の場合
            if (UserProperty.GetInstance().SystemSettingsInfo.TraDonVersionKbn
                == (int)DefaultProperty.TraDonVersionKbn.V50)
            {
                sb.AppendLine(info.TaikijikanNumber.ToString() + ", ");
                sb.AppendLine(info.TaikijikanFigId.ToString() + ", ");
                sb.AppendLine(info.TaikijikanAtPrice.ToString() + ", ");
                sb.AppendLine(info.TaikijikanryoInPrice.ToString() + ", ");
                sb.AppendLine(info.NizumiryoInPrice.ToString() + ", ");
                sb.AppendLine(info.NioroshiryoInPrice.ToString() + ", ");
                sb.AppendLine(info.FutaigyomuryoInPrice.ToString() + ", ");
                sb.AppendLine(info.TaikijikanryoInFee.ToString() + ", ");
                sb.AppendLine(info.NizumiryoInFee.ToString() + ", ");
                sb.AppendLine(info.NioroshiryoInFee.ToString() + ", ");
                sb.AppendLine(info.FutaigyomuryoInFee.ToString() + ", ");
                sb.AppendLine(info.NizumijikanNumber.ToString() + ", ");
                sb.AppendLine(info.NizumijikanFigId.ToString() + ", ");
                sb.AppendLine(info.NizumijikanAtPrice.ToString() + ", ");
                sb.AppendLine(info.NioroshijikanNumber.ToString() + ", ");
                sb.AppendLine(info.NioroshijikanFigId.ToString() + ", ");
                sb.AppendLine(info.NioroshijikanAtPrice.ToString() + ", ");
                sb.AppendLine(info.FutaigyomujikanNumber.ToString() + ", ");
                sb.AppendLine(info.FutaigyomujikanFigId.ToString() + ", ");
                sb.AppendLine(info.FutaigyomujikanAtPrice.ToString() + ", ");
            }
            sb.AppendLine(
                SQLHelper.GetPopulateColumnInsertStringForTraDon(this.authInfo, popOption));

            sb.AppendLine(") ");

            return sb.ToString();
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、締切済の得意先情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>締切済の得意先情報リスト</returns>
        public IList<String> GetClmFixDateListInternal(SqlTransaction transaction, SeikyuDataSakuseiConditionInfo para)
        {
            //返却用のリスト
            List<SaleInfo> rt_list = new List<SaleInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 CONCAT(TORADON_Tokuisaki.TokuisakiCd,':' + MAX(ToraDONSale.TokuisakiNM)) TokuisakiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Sale ToraDONSale ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki ");
            sb.AppendLine(" ON TORADON_Tokuisaki.TokuisakiId = ToraDONSale.TokuisakiId ");
            sb.AppendLine(" AND TORADON_Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONSale.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" AND 0 < ToraDONSale.ClmFixDate ");
            sb.AppendLine(" AND ToraDONSale.AddProcessId = '" + DefaultProperty.SEIKYU_DATA_SAKUSEI_FRAMENAME + "' ");

            if (para != null)
            {
                sb.AppendLine("	--引数「営業所」 ");
                sb.AppendLine(" AND ToraDONSale.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");

                switch (para.SeikyuRenkeiHizukeKbn)
                {
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.AddUpYMD:
                        sb.AppendLine("	--引数「計上日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.AddUpDate >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「計上日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.AddUpDate <= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo).ToString() + " ");
                        break;
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.StartYMD:
                        sb.AppendLine("	--引数「発日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskStartDate >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「発日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskStartDate < " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                        break;
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.ChakuYMD:
                        sb.AppendLine("	--引数「着日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskEndDateTime >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「着日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskEndDateTime < " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                        break;
                    default:
                        break;
                }
            }
            sb.AppendLine("GROUP BY TORADON_Tokuisaki.TokuisakiCd ");
            sb.AppendLine("ORDER BY TORADON_Tokuisaki.TokuisakiCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値を返します
                return rdr["TokuisakiName"].ToString();
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、支払済の取引先情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>支払済の取引先情報リスト</returns>
        public IList<String> GetCharterPayFixDateListInternal(SqlTransaction transaction, SeikyuDataSakuseiConditionInfo para)
        {
            //返却用のリスト
            List<SaleInfo> rt_list = new List<SaleInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 CONCAT(TORADON_Torihikisaki.TorihikiCd,':' + MAX(TORADON_Torihikisaki.TorihikiSNM)) TokuisakiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Sale ToraDONSale ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki ");
            sb.AppendLine(" ON TORADON_Torihikisaki.TorihikiId = ToraDONSale.CarOfChartererId ");
            sb.AppendLine(" AND TORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONSale.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" AND 0 < ToraDONSale.CharterPayFixDate ");
            sb.AppendLine(" AND ToraDONSale.AddProcessId = '" + DefaultProperty.SEIKYU_DATA_SAKUSEI_FRAMENAME + "' ");

            if (para != null)
            {
                sb.AppendLine("	--引数「営業所」 ");
                sb.AppendLine(" AND ToraDONSale.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");

                switch (para.SeikyuRenkeiHizukeKbn)
                {
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.AddUpYMD:
                        sb.AppendLine("	--引数「計上日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.AddUpDate >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「計上日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.AddUpDate <= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo).ToString() + " ");
                        break;
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.StartYMD:
                        sb.AppendLine("	--引数「発日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskStartDate >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「発日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskStartDate < " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                        break;
                    case DefaultProperty.SeikyuRenkeiHizukeKbn.ChakuYMD:
                        sb.AppendLine("	--引数「着日（範囲開始）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskEndDateTime >= " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDFrom).ToString() + " ");
                        sb.AppendLine("	--引数「着日（範囲終了）」 ");
                        sb.AppendLine(" AND ToraDONSale.TaskEndDateTime < " + NSKUtil.DateTimeToDecimalWithTime(para.HizukeYMDTo.AddDays(1)).ToString() + " ");
                        break;
                    default:
                        break;
                }
            }
            sb.AppendLine("GROUP BY TORADON_Torihikisaki.TorihikiCd ");
            sb.AppendLine("ORDER BY TORADON_Torihikisaki.TorihikiCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値を返します
                return rdr["TokuisakiName"].ToString();
            }, transaction);
        }

        #endregion
    }
}