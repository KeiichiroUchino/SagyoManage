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
    /// 日報ワークテーブルのデータアクセスレイヤです。
    /// </summary>
    public class WK_DailyReport
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
        /// 日報ワーククラスのデフォルトコンストラクタです。
        /// </summary>
        public WK_DailyReport()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、日報ワークテーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public WK_DailyReport(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 日報ワーク情報の件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>日報ワーク情報の件数</returns>
        public int GetCount(SqlTransaction transaction)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" COUNT(*) CNT ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_DailyReport.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            return SQLHelper.SimpleReadSingle(sb.ToString(), rdr => SQLServerUtil.dbInt(rdr["CNT"]), transaction);
        }

        /// <summary>
        /// 日報IDを更新します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="id">日報ID</param>
        public void UpdateDailyReportIds(SqlTransaction transaction, decimal id)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine("SET ");
            sb.AppendLine(" DailyReportId = WK_DailyReport.DailyReportId + " + id.ToString() + " ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_DailyReport.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);

            //指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);
        }

        /// <summary>
        /// 日報ワーク情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>日報ワーク情報リスト</returns>
        public IList<WK_DailyReportInfo> GetList(SqlTransaction transaction)
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
        public IList<WK_DailyReportInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<WK_DailyReportInfo> rt_list = new List<WK_DailyReportInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" WK_DailyReport ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" WK_DailyReport.OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                WK_DailyReportInfo rt_info = new WK_DailyReportInfo
                {
                    OperatorId = SQLServerUtil.dbDecimal(rdr["OperatorId"]),
                    DailyReportId = SQLServerUtil.dbDecimal(rdr["DailyReportId"]),
                    CarDispatchId = SQLServerUtil.dbDecimal(rdr["CarDispatchId"]),
                    DailyReportDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["DailyReportDate"])),
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                    WeatherId = SQLServerUtil.dbDecimal(rdr["WeatherId"]),
                    CarId = SQLServerUtil.dbDecimal(rdr["CarId"]),
                    LicPlateCarNo = rdr["LicPlateCarNo"].ToString(),
                    CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]),
                    CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]),
                    DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]),
                    StartDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["StartDate"])),
                    StartTime =
                        SQLHelper.dbCustomDecimalToTimeSpan(rdr["StartTime"]),
                    StartMeter = SQLServerUtil.dbDecimal(rdr["StartMeter"]),
                    EndDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["EndDate"])),
                    EndTime =
                        SQLHelper.dbCustomDecimalToTimeSpan(rdr["EndTime"]),
                    EndMeter = SQLServerUtil.dbDecimal(rdr["EndMeter"]),
                    WorkDays = SQLServerUtil.dbDecimal(rdr["WorkDays"]),
                    TransportTimes = SQLServerUtil.dbDecimal(rdr["TransportTimes"]),
                    TransportWeight = SQLServerUtil.dbDecimal(rdr["TransportWeight"]),
                    MileageKm = SQLServerUtil.dbDecimal(rdr["MileageKm"]),
                    ActualKm = SQLServerUtil.dbDecimal(rdr["ActualKm"]),
                    Memo = rdr["Memo"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
