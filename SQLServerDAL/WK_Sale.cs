using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using System.Configuration;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 売上ワークテーブルのデータアクセスレイヤです。
    /// </summary>
    public class WK_Sale
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo _authInfo =
            new AppAuthInfo
            {
                OperatorId = 0,
                TerminalId = "",
                UserProcessId = "",
                UserProcessName = ""
            };

         /// <summary>
        /// 売上クラスのデフォルトコンストラクタです。
        /// </summary>
        public WK_Sale()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、売上テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public WK_Sale(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 売上ワーク情報の件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>売上ワーク情報の件数</returns>
        public int GetCount(SqlTransaction transaction)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" COUNT(*) CNT ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_Sale.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            return SQLHelper.SimpleReadSingle(sb.ToString(), rdr => SQLServerUtil.dbInt(rdr["CNT"]), transaction);
        }

        /// <summary>
        /// 日報IDを更新します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        public int UpdateDailyReportIds(SqlTransaction transaction)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("SET ");
            sb.AppendLine(" WK_Sale.DailyReportId = WK_DailyReport.DailyReportId ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine(" ON  WK_DailyReport.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");
            sb.AppendLine(" AND WK_DailyReport.OperatorId = WK_Sale.OperatorId ");
            sb.AppendLine(" AND WK_DailyReport.BranchOfficeId = WK_Sale.BranchOfficeId ");
            sb.AppendLine(" AND WK_DailyReport.DailyReportDate = WK_Sale.TaskStartDate ");
            sb.AppendLine(" AND WK_DailyReport.CarId = WK_Sale.CarId ");
            sb.AppendLine(" AND WK_DailyReport.CarKindId = WK_Sale.CarKindId ");
            sb.AppendLine(" AND WK_DailyReport.CarOfChartererId = WK_Sale.CarOfChartererId ");
            sb.AppendLine(" AND WK_DailyReport.DriverId = WK_Sale.DriverId ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_Sale.DailyReportId = 0 ");

            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);

            //指定したトランザクション上でExecuteNonqueryを実行
            return SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);
        }

        /// <summary>
        /// 売上ワーク情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>売上ワーク情報リスト</returns>
        public IList<WK_SaleInfo> GetList(SqlTransaction transaction)
        {
            return GetListInternal(transaction);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>情報リスト</returns>
        public IList<WK_SaleInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<WK_SaleInfo> rt_list = new List<WK_SaleInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_Sale ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_Sale.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                WK_SaleInfo rt_info = new WK_SaleInfo
                {
                    OperatorId = SQLServerUtil.dbDecimal(rdr["OperatorId"]),
                    SaleId = SQLServerUtil.dbDecimal(rdr["SaleId"]),
                    DailyReportId = SQLServerUtil.dbDecimal(rdr["DailyReportId"]),
                    CarId = SQLServerUtil.dbDecimal(rdr["CarId"]),
                    LicPlateCarNo = rdr["LicPlateCarNo"].ToString(),
                    CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]),
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
                    TaskStartTime =
                        SQLHelper.dbCustomDecimalToTimeSpan(rdr["TaskStartTime"]),
                    TaskEndDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["TaskEndDate"])),
                    TaskEndTime =
                        SQLHelper.dbCustomDecimalToTimeSpan(rdr["TaskEndTime"]),
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
                    Weight =
                        SQLServerUtil.dbDecimal(rdr["Weight"]),
                    DelFlag =
                        NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DelFlag"])),

                    TaikijikanNumber = SQLServerUtil.dbDecimal(rdr["TaikijikanNumber"]),
                    TaikijikanFigId = SQLServerUtil.dbDecimal(rdr["TaikijikanFigId"]),
                    TaikijikanAtPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanAtPrice"]),
                    TaikijikanryoInPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInPrice"]),
                    NizumiryoInPrice = SQLServerUtil.dbDecimal(rdr["NizumiryoInPrice"]),
                    NioroshiryoInPrice = SQLServerUtil.dbDecimal(rdr["NioroshiryoInPrice"]),
                    FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]),
                    TaikijikanryoInFee = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInFee"]),
                    NizumiryoInFee = SQLServerUtil.dbDecimal(rdr["NizumiryoInFee"]),
                    NioroshiryoInFee = SQLServerUtil.dbDecimal(rdr["NioroshiryoInFee"]),
                    FutaigyomuryoInFee = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInFee"]),
                    NizumijikanNumber = SQLServerUtil.dbDecimal(rdr["NizumijikanNumber"]),
                    NizumijikanFigId = SQLServerUtil.dbDecimal(rdr["NizumijikanFigId"]),
                    NizumijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NizumijikanAtPrice"]),
                    NioroshijikanNumber = SQLServerUtil.dbDecimal(rdr["NioroshijikanNumber"]),
                    NioroshijikanFigId = SQLServerUtil.dbDecimal(rdr["NioroshijikanFigId"]),
                    NioroshijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NioroshijikanAtPrice"]),
                    FutaigyomujikanNumber = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanNumber"]),
                    FutaigyomujikanFigId = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanFigId"]),
                    FutaigyomujikanAtPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanAtPrice"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
