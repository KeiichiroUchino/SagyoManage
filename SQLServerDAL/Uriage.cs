using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.SQLServerDAL;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 売上テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Uriage
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

        ///// <summary>
        ///// テーブル名
        ///// </summary>
        //private string _tableName = "売上";

         /// <summary>
        /// 売上クラスのデフォルトコンストラクタです。
        /// </summary>
        public Uriage()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、売上テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Uriage(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、情報を取得します。
        /// </summary>
        /// <param name="code">売上コード</param>
        /// <returns>売上情報</returns>
        public UriageInfo GetInfo(int? code = null)
        {
            if (!code.HasValue)
                return null;

            UriageInfo info =
                this.GetListInternal(null,
                new UriageSearchParameter
                {
                    UriageCode = code,
                }).FirstOrDefault();
            return info;
        }

        /// <summary>
        /// Id指定で売上情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public UriageInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new UriageSearchParameter()
            {
                UriageId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、売上情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>売上情報のリスト</returns>
        public IList<UriageInfo> GetList(UriageSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、売上情報を指定して、
        /// 売上情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="list">売上情報リスト</param>
        public void Save(SqlTransaction transaction, IList<UriageInfo> list)
        {
        }

        /// <summary>
        /// SqlTransaction情報、売上情報を指定して、
        /// 売上情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">売上情報</param>
        public void Save(SqlTransaction transaction, UriageInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //常設列の取得オプションを作る
                //--更新は入力と更新
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.UpdateColumns;

                //Update文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE  ");
                sb.AppendLine(" Uriage ");
                sb.AppendLine("SET ");
                sb.AppendLine("  DailyReportId = " + info.DailyReportId.ToString() + " ");
                sb.AppendLine(" ,SaleSlipNo = " + info.SaleSlipNo.ToString() + " ");
                sb.AppendLine(" ,BranchOfficeId = " + info.BranchOfficeId.ToString() + " ");
                sb.AppendLine(" ,CarId = " + info.CarId.ToString() + " ");
                sb.AppendLine(" ,LicPlateCarNo = N'" + SQLHelper.GetSanitaizingSqlString(info.LicPlateCarNo).Trim() + "'");
                sb.AppendLine(" ,CarKindId = " + info.CarKindId.ToString() + " ");
                sb.AppendLine(" ,DriverId = " + info.DriverId.ToString() + " ");
                sb.AppendLine(" ,TokuisakiId = " + info.TokuisakiId.ToString() + " ");
                sb.AppendLine(" ,TokuisakiNM = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiName).Trim() + "'");
                sb.AppendLine(" ,ClmClassId = " + info.ClmClassId.ToString() + " ");
                sb.AppendLine(" ,ContractId = " + info.ContractId.ToString() + " ");
                sb.AppendLine(" ,StartPointId = " + info.StartPointId.ToString() + " ");
                sb.AppendLine(" ,StartPointNM = N'" + SQLHelper.GetSanitaizingSqlString(info.StartPointName).Trim() + "'");
                sb.AppendLine(" ,EndPointId = " + info.EndPointId.ToString() + " ");
                sb.AppendLine(" ,EndPointNM = N'" + SQLHelper.GetSanitaizingSqlString(info.EndPointName).Trim() + "'");
                sb.AppendLine(" ,ItemId = " + info.ItemId.ToString() + " ");
                sb.AppendLine(" ,ItemNM = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemName).Trim() + "'");
                sb.AppendLine(" ,OwnerId = " + info.OwnerId.ToString() + " ");
                sb.AppendLine(" ,OwnerNM = N'" + SQLHelper.GetSanitaizingSqlString(info.OwnerName).Trim() + "'");
                sb.AppendLine(" ,TaskStartYMD = " + SQLHelper.DateTimeToDbDate(info.TaskStartYMD).ToString() + " ");
                sb.AppendLine(" ,TaskEndYMD = " + SQLHelper.DateTimeToDbDate(info.TaskEndYMD).ToString() + " ");
                sb.AppendLine(" ,Number = " + info.Number.ToString() + " ");
                sb.AppendLine(" ,FigId = " + info.FigId.ToString() + " ");
                sb.AppendLine(" ,AtPrice = " + info.AtPrice.ToString() + " ");
                sb.AppendLine(" ,Price = " + info.Price.ToString() + " ");
                sb.AppendLine(" ,PriceInPrice = " + info.PriceInPrice.ToString() + " ");
                sb.AppendLine(" ,TollFeeInPrice = " + info.TollFeeInPrice.ToString() + " ");
                sb.AppendLine(" ,PriceOutTaxCalc = " + info.PriceOutTaxCalc.ToString() + " ");
                sb.AppendLine(" ,PriceOutTax = " + info.PriceOutTax.ToString() + " ");
                sb.AppendLine(" ,PriceInTaxCalc = " + info.PriceInTaxCalc.ToString() + " ");
                sb.AppendLine(" ,PriceInTax = " + info.PriceInTax.ToString() + " ");
                sb.AppendLine(" ,PriceNoTaxCalc = " + info.PriceNoTaxCalc.ToString() + " ");
                sb.AppendLine(" ,TaxDispKbn = " + info.TaxDispKbn.ToString() + " ");
                sb.AppendLine(" ,AddUpYMD = " + SQLHelper.DateTimeToDbDate(info.AddUpYMD).ToString() + " ");
                sb.AppendLine(" ,FixFlag = " + NSKUtil.BoolToInt(info.FixFlag).ToString() + " ");
                sb.AppendLine(" ,Memo = N'" + SQLHelper.GetSanitaizingSqlString(info.Memo).Trim() + "'");
                sb.AppendLine(" ,ClmFixYMD = " + SQLHelper.DateTimeToDbDate(info.ClmFixYMD).ToString() + " ");
                sb.AppendLine(" ,CarOfChartererId = " + info.CarOfChartererId.ToString() + " ");
                sb.AppendLine(" ,CharterPrice = " + info.CharterPrice.ToString() + " ");
                sb.AppendLine(" ,PriceInCharterPrice = " + info.PriceInCharterPrice.ToString() + " ");
                sb.AppendLine(" ,TollFeeInCharterPrice = " + info.TollFeeInCharterPrice.ToString() + " ");
                sb.AppendLine(" ,CharterPriceOutTaxCalc = " + info.CharterPriceOutTaxCalc.ToString() + " ");
                sb.AppendLine(" ,CharterPriceOutTax = " + info.CharterPriceOutTax.ToString() + " ");
                sb.AppendLine(" ,CharterPriceInTaxCalc = " + info.CharterPriceInTaxCalc.ToString() + " ");
                sb.AppendLine(" ,CharterPriceInTax = " + info.CharterPriceInTax.ToString() + " ");
                sb.AppendLine(" ,CharterPriceNoTaxCalc = " + info.CharterPriceNoTaxCalc.ToString() + " ");
                sb.AppendLine(" ,CharterTaxDispKbn = " + info.CharterTaxDispKbn.ToString() + " ");
                sb.AppendLine(" ,CharterAddUpYMD = " + SQLHelper.DateTimeToDbDate(info.CharterAddUpYMD).ToString() + " ");
                sb.AppendLine(" ,CharterFixFlag = " + NSKUtil.BoolToInt(info.CharterFixFlag).ToString() + " ");
                sb.AppendLine(" ,CharterPayFixYMD = " + SQLHelper.DateTimeToDbDate(info.CharterPayFixYMD).ToString() + " ");
                sb.AppendLine(" ,Fee = " + info.Fee.ToString() + " ");
                sb.AppendLine(" ,PriceInFee = " + info.PriceInFee.ToString() + " ");
                sb.AppendLine(" ,TollFeeInFee = " + info.TollFeeInFee.ToString() + " ");
                sb.AppendLine(" ,FeeOutTaxCalc = " + info.FeeOutTaxCalc.ToString() + " ");
                sb.AppendLine(" ,FeeOutTax = " + info.FeeOutTax.ToString() + " ");
                sb.AppendLine(" ,FeeInTaxCalc = " + info.FeeInTaxCalc.ToString() + " ");
                sb.AppendLine(" ,FeeInTax = " + info.FeeInTax.ToString() + " ");
                sb.AppendLine(" ,FeeNoTaxCalc = " + info.FeeNoTaxCalc.ToString() + " ");
                sb.AppendLine(" ,Weight = " + info.Weight.ToString() + " ");

                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("UriageId = " + info.UriageId.ToString() + " ");
                sb.AppendLine("--排他のチェック ");
                sb.AppendLine("AND ");
                sb.AppendLine("VersionColumn = @versionColumn ");

                string sql = sb.ToString();

                SqlCommand command = new SqlCommand(sql);
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
            }
            else
            {
                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //InsertのSQLを作る前に、次IDを取得しておく
                decimal newId =
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.JuchuIdx);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Uriage ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 UriageId ");
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
                sb.AppendLine(" ,TaskStartYMD ");
                sb.AppendLine(" ,TaskEndYMD ");
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
                sb.AppendLine(" ,AddUpYMD ");
                sb.AppendLine(" ,FixFlag ");
                sb.AppendLine(" ,Memo ");
                sb.AppendLine(" ,ClmFixYMD ");
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
                sb.AppendLine(" ,CharterAddUpYMD ");
                sb.AppendLine(" ,CharterFixFlag ");
                sb.AppendLine(" ,CharterPayFixYMD ");
                sb.AppendLine(" ,Fee ");
                sb.AppendLine(" ,PriceInFee ");
                sb.AppendLine(" ,TollFeeInFee ");
                sb.AppendLine(" ,FeeOutTaxCalc ");
                sb.AppendLine(" ,FeeOutTax ");
                sb.AppendLine(" ,FeeInTaxCalc ");
                sb.AppendLine(" ,FeeInTax ");
                sb.AppendLine(" ,FeeNoTaxCalc ");
                sb.AppendLine(" ,Weight ");

                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(", " + info.DailyReportId.ToString() + " ");
                sb.AppendLine(", " + info.SaleSlipNo.ToString() + " ");
                sb.AppendLine(", " + info.BranchOfficeId.ToString() + " ");
                sb.AppendLine(", " + info.CarId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.LicPlateCarNo).Trim() + "'");
                sb.AppendLine(", " + info.CarKindId.ToString() + " ");
                sb.AppendLine(", " + info.DriverId.ToString() + " ");
                sb.AppendLine(", " + info.TokuisakiId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiName).Trim() + "'");
                sb.AppendLine(", " + info.ClmClassId.ToString() + " ");
                sb.AppendLine(", " + info.ContractId.ToString() + " ");
                sb.AppendLine(", " + info.StartPointId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.StartPointName).Trim() + "'");
                sb.AppendLine(", " + info.EndPointId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.EndPointName).Trim() + "'");
                sb.AppendLine(", " + info.ItemId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemName).Trim() + "'");
                sb.AppendLine(", " + info.OwnerId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.OwnerName).Trim() + "'");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.TaskStartYMD).ToString() + " ");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.TaskEndYMD).ToString() + " ");
                sb.AppendLine(", " + info.Number.ToString() + " ");
                sb.AppendLine(", " + info.FigId.ToString() + " ");
                sb.AppendLine(", " + info.AtPrice.ToString() + " ");
                sb.AppendLine(", " + info.Price.ToString() + " ");
                sb.AppendLine(", " + info.PriceInPrice.ToString() + " ");
                sb.AppendLine(", " + info.TollFeeInPrice.ToString() + " ");
                sb.AppendLine(", " + info.PriceOutTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.PriceOutTax.ToString() + " ");
                sb.AppendLine(", " + info.PriceInTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.PriceInTax.ToString() + " ");
                sb.AppendLine(", " + info.PriceNoTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.TaxDispKbn.ToString() + " ");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.AddUpYMD).ToString() + " ");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.FixFlag).ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.Memo).Trim() + "'");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.ClmFixYMD).ToString() + " ");
                sb.AppendLine(", " + info.CarOfChartererId.ToString() + " ");
                sb.AppendLine(", " + info.CharterPrice.ToString() + " ");
                sb.AppendLine(", " + info.PriceInCharterPrice.ToString() + " ");
                sb.AppendLine(", " + info.TollFeeInCharterPrice.ToString() + " ");
                sb.AppendLine(", " + info.CharterPriceOutTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.CharterPriceOutTax.ToString() + " ");
                sb.AppendLine(", " + info.CharterPriceInTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.CharterPriceInTax.ToString() + " ");
                sb.AppendLine(", " + info.CharterPriceNoTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.CharterTaxDispKbn.ToString() + " ");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.CharterAddUpYMD).ToString() + " ");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.CharterFixFlag).ToString() + " ");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(info.CharterPayFixYMD).ToString() + " ");
                sb.AppendLine(", " + info.Fee.ToString() + " ");
                sb.AppendLine(", " + info.PriceInFee.ToString() + " ");
                sb.AppendLine(", " + info.TollFeeInFee.ToString() + " ");
                sb.AppendLine(", " + info.FeeOutTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.FeeOutTax.ToString() + " ");
                sb.AppendLine(", " + info.FeeInTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.FeeInTax.ToString() + " ");
                sb.AppendLine(", " + info.FeeNoTaxCalc.ToString() + " ");
                sb.AppendLine(", " + info.Weight.ToString() + " ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));
            }
        }

        /// <summary>
        ///  SqlTransaction情報、売上情報を指定して、
        ///  売上情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">売上情報</param>
        public void Delete(SqlTransaction transaction, UriageInfo info)
        {
            #region レコードの削除

            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Delete文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" Uriage ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" UriageId = " + info.UriageId.ToString() + " ");
            sb.AppendLine(" --排他のチェック ");
            sb.AppendLine(" AND	VersionColumn = @versionColumn ");
            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。
            SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command);

            #endregion
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<UriageInfo> GetListInternal(SqlTransaction transaction, UriageSearchParameter para)
        {
            //返却用のリスト
            List<UriageInfo> rt_list = new List<UriageInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Uriage.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	Uriage ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	Uriage.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.UriageId.HasValue)
                {
                    sb.AppendLine("AND Uriage.UriageId = " + para.UriageId.ToString() + " ");
                }
                if (para.UriageCode.HasValue)
                {
                    sb.AppendLine("AND Uriage.UriageCd = " + para.UriageCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Uriage.UriageCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                UriageInfo rt_info = new UriageInfo
                {
                    UriageId = SQLServerUtil.dbDecimal(rdr["UriageId"]),
                    DailyReportId = SQLServerUtil.dbDecimal(rdr["DailyReportId"]),
                    SaleSlipNo = SQLServerUtil.dbDecimal(rdr["SaleSlipNo"]),
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
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
                    TaskStartYMD = SQLHelper.dbDate(rdr["TaskStartYMD"]),
                    TaskEndYMD = SQLHelper.dbDate(rdr["TaskEndYMD"]),
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
                    AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]),
                    FixFlag = SQLHelper.dbBit(rdr["FixFlag"]),
                    Memo = rdr["Memo"].ToString(),
                    ClmFixYMD = SQLHelper.dbDate(rdr["ClmFixYMD"]),
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
                    CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]),
                    CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]),
                    CharterPayFixYMD = SQLHelper.dbDate(rdr["CharterPayFixYMD"]),
                    Fee = SQLServerUtil.dbDecimal(rdr["Fee"]),
                    PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]),
                    TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]),
                    FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]),
                    FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]),
                    FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]),
                    FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]),
                    FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]),
                    Weight = SQLServerUtil.dbDecimal(rdr["Weight"]),

                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
