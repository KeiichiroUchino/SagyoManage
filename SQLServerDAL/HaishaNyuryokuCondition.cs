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

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 配車入力のデータアクセスレイヤです。
    /// </summary>
    public class HaishaNyuryokuCondition
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
        /// 一括登録件数
        /// </summary>
        private const int EXECUTE_COUNT = 1000;

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaNyuryokuCondition()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaishaNyuryokuCondition(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        ///発着地のID一覧を取得します。
        /// </summary>
        /// <param name="checkIdList">検索IDリスト</param>
        /// <returns>発着地一覧データ</returns>
        public List<decimal> GetPointSelect(string checkIdList)
        {
            // クエリ
            string query = this.GeQuerytPointSelect(checkIdList);
            List<decimal> list = SQLHelper.SimpleRead(query, rdr => SQLServerUtil.dbDecimal(rdr["ToraDONPointId"]));
            return list;
        }

        /// <summary>
        ///方面が未設定の発着地のID一覧を取得します。
        /// </summary>
        /// <returns>方面が未設定の発着地一覧データ</returns>
        public List<decimal> GetPointSelect0()
        {
            // クエリ
            string query = this.GeQuerytPointSelect0();
            List<decimal> list = SQLHelper.SimpleRead(query, rdr => SQLServerUtil.dbDecimal(rdr["ToraDONPointId"]));
            return list;
        }

        /// <summary>
        ///登録済み排他情報を取得します。
        /// </summary>
        /// <param name="branchOfficeId">営業所ID</param>
        /// <param name="checkIdList">検索IDリスト</param>
        /// <param name="haishaNyuryokuJokenKbn">配車入力条件区分</param>
        /// <returns>登録済み排他情報データ</returns>
        public List<HaitaResultInfo> GetHaishaExclusiveManageSelect(decimal? branchOfficeId, string checkIdList, int haishaNyuryokuJokenKbn)
        {
            // クエリ
            decimal id = branchOfficeId ?? 0;
            string query = this.GeQuerytHaishaExclusiveManageSelect(id, checkIdList, haishaNyuryokuJokenKbn);
            return SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaitaResultInfo ret = new HaitaResultInfo();
                ret.kbn = haishaNyuryokuJokenKbn;
                ret.Code = SQLServerUtil.dbInt(rdr["Code"]);
                ret.BranchOfficeShortName = rdr["BranchOfficeSNM"].ToString();
                ret.OperatorCode = rdr["OperatorCode"].ToString();
                ret.OperatorName = rdr["OperatorName"].ToString();

                //返却用の値を返します
                return ret;
            });
        }

        /// <summary>
        /// 配車排他管理テーブルの追加を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">追加配車情報</param>
        public void AddHaishaExclusiveManage(SqlTransaction transaction, HaishaNyuryokuConditionInfo para = null)
        {

            int i = 0;
            List<string> mySqlList = new List<string>();

            //配車入力条件区分よって制御を分岐
            switch (para.HaishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先グループの場合
                    foreach (decimal id in para.TokuisakiIdMeisaiList)
                    {

                        mySqlList.Add(this.GetCommandHaishaExclusiveManageInsert(para.BranchOfficeId, id));
                        i++;

                        if (i % EXECUTE_COUNT == 0 || para.TokuisakiIdMeisaiList.Count() == i)
                        {
                            int createCnt = mySqlList.Count();

                            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                            string query = SQLHelper.SQLQueryJoin(mySqlList);

                            //指定したトランザクション上でExecuteNonqueryを実行し
                            int execCnt = SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));

                            if (createCnt != execCnt)
                            {
                                //リトライ可能な例外
                                throw new
                                    Model.DALExceptions.CanRetryException(
                                    "配車情報作成に失敗しました。\r\n再度処理を実行してください。"
                                    , MessageBoxIcon.Warning);
                            }

                            mySqlList.Clear();
                        }
                    }

                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面グループの場合
                    foreach (decimal id in para.PointIdMeisaiList)
                    {

                        mySqlList.Add(this.GetCommandHaishaExclusiveManageInsert(para.BranchOfficeId, id));
                        i++;

                        if (i % EXECUTE_COUNT == 0 || para.PointIdMeisaiList.Count() == i)
                        {
                            int createCnt = mySqlList.Count();

                            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                            string query = SQLHelper.SQLQueryJoin(mySqlList);

                            //指定したトランザクション上でExecuteNonqueryを実行し
                            int execCnt = SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));

                            if (createCnt != execCnt)
                            {
                                //リトライ可能な例外
                                throw new
                                    Model.DALExceptions.CanRetryException(
                                    "配車情報作成に失敗しました。\r\n再度処理を実行してください。"
                                    , MessageBoxIcon.Warning);
                            }

                            mySqlList.Clear();
                        }
                    }

                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種グループの場合
                    foreach (decimal id in para.CarKindIdMeisaiList)
                    {

                        mySqlList.Add(this.GetCommandHaishaExclusiveManageInsert(para.BranchOfficeId, id));
                        i++;

                        if (i % EXECUTE_COUNT == 0 || para.CarKindIdMeisaiList.Count() == i)
                        {
                            int createCnt = mySqlList.Count();

                            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                            string query = SQLHelper.SQLQueryJoin(mySqlList);

                            //指定したトランザクション上でExecuteNonqueryを実行し
                            int execCnt = SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));

                            if (createCnt != execCnt)
                            {
                                //リトライ可能な例外
                                throw new
                                    Model.DALExceptions.CanRetryException(
                                    "配車情報作成に失敗しました。\r\n再度処理を実行してください。"
                                    , MessageBoxIcon.Warning);
                            }

                            mySqlList.Clear();
                        }
                    }

                    break;
                default:

                    // 営業所のみ
                    SqlCommand command = new SqlCommand(this.GetCommandHaishaExclusiveManageInsert(para.BranchOfficeId, decimal.Zero).ToString());

                    // 登録実行
                    SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);

                    break;
            }
        }

        /// <summary>
        /// 配車排他管理テーブルのレコードをDELETE
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="info">削除配車情報</param>
        public void DeleteHaishaExclusiveManage(SqlTransaction transaction)
        {
            SqlCommand command = this.GetCommandHaishaaExclusiveManageDelete();

            // 削除実行（該当データがある場合は必ず削除し、ない場合はそのまま続行）
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 発着地のIDを取得するSqlを返す。
        /// </summary>
        /// <param name="checkIdList">検索IDリスト</param>
        /// <returns>発着地のIDを取得するSql</returns>
        private string GeQuerytPointSelect(string checkIdList)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ToraDONPointId FROM Point ");
            sb.AppendLine("   WHERE HomenId IN ( " + checkIdList + ") ");
            sb.AppendLine("    AND DelFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        /// <summary>
        /// 方面が未設定の発着地のIDを取得するSqlを返す。
        /// </summary>
        /// <returns>方面が未設定の発着地のIDを取得するSql</returns>
        private string GeQuerytPointSelect0()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ToraDONPoint.PointId ToraDONPointId ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Point AS ToraDONPoint ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Point ");
            sb.AppendLine("ON Point.ToraDONPointId = ToraDONPoint.PointId ");
            sb.AppendLine("AND Point.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Homen ");
            sb.AppendLine("ON Homen.HomenId = Point.HomenId ");
            sb.AppendLine("AND Homen.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	ISNULL(Homen.HomenId, 0) = 0 ");

            return sb.ToString();
        }

        /// <summary>
        /// 登録済みするSqlQueryを返す。
        /// </summary>
        /// <param name="branchOfficeId">営業所ID</param>
        /// <param name="checkIdList">検索IDリスト</param>
        /// <param name="haishaNyuryokuJokenKbn">配車入力条件区分</param>
        /// <returns></returns>
        private string GeQuerytHaishaExclusiveManageSelect(decimal branchOfficeId, string checkIdList, int haishaNyuryokuJokenKbn)
        {
            StringBuilder sb = new StringBuilder();


            // 配車入力条件区分
            switch (haishaNyuryokuJokenKbn)
            {
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                    // 得意先
                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine("  BranchOffice.BranchOfficeSNM ");
                    sb.AppendLine("  ,Tokuisaki.TokuisakiCd AS Code ");
                    sb.AppendLine("  ,Operator.OperatorCode ");
                    sb.AppendLine("  ,Operator.OperatorName ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine("  HaishaExclusiveManage ");
                    sb.AppendLine("  LEFT JOIN TORADON_BranchOffice BranchOffice ");
                    sb.AppendLine("    ON HaishaExclusiveManage.BranchOfficeId = BranchOffice.BranchOfficeId ");
                    sb.AppendLine("    AND BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN TORADON_Tokuisaki Tokuisaki ");
                    sb.AppendLine("    ON HaishaExclusiveManage.WorkId = Tokuisaki.TokuisakiId ");
                    sb.AppendLine("    AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Operator ");
                    sb.AppendLine("    ON HaishaExclusiveManage.OperatorId = Operator.OperatorId ");
                    sb.AppendLine("    AND Operator.DelFlag = " + NSKUtil.BoolToInt(false) + "");

                    sb.AppendLine(" WHERE ");
                    sb.AppendLine("  HaishaExclusiveManage.OperatorId <> " + authInfo.OperatorId);

                    if (branchOfficeId.CompareTo(decimal.Zero) != 0)
                    {
                        // 営業所コード(画面より指定した営業所、または0:全営業所)
                        sb.AppendLine("   AND (HaishaExclusiveManage.BranchOfficeId = " + branchOfficeId);
                        sb.AppendLine("   OR HaishaExclusiveManage.BranchOfficeId = 0)");
                    }

                    if (!string.IsNullOrWhiteSpace(checkIdList))
                    {
                        sb.AppendLine("  AND HaishaExclusiveManage.WorkId IN ( " + checkIdList + ") ");
                    }

                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                    // 方面
                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine("  BranchOffice.BranchOfficeSNM ");
                    sb.AppendLine("  ,Homen.HomenCode AS Code");
                    sb.AppendLine("  ,Operator.OperatorCode ");
                    sb.AppendLine("  ,Operator.OperatorName ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine("  HaishaExclusiveManage ");
                    sb.AppendLine("  LEFT JOIN TORADON_BranchOffice BranchOffice ");
                    sb.AppendLine("    ON HaishaExclusiveManage.BranchOfficeId = BranchOffice.BranchOfficeId ");
                    sb.AppendLine("    AND BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN TORADON_Tokuisaki Tokuisaki ");
                    sb.AppendLine("    ON HaishaExclusiveManage.WorkId = Tokuisaki.TokuisakiId ");
                    sb.AppendLine("    AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Point ");
                    sb.AppendLine("    ON HaishaExclusiveManage.WorkId = Point.ToraDONPointId ");
                    sb.AppendLine("    AND Point.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Homen ");
                    sb.AppendLine("    ON Point.HomenId = Homen.HomenId ");
                    sb.AppendLine("    AND Homen.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Operator ");
                    sb.AppendLine("    ON HaishaExclusiveManage.OperatorId = Operator.OperatorId ");
                    sb.AppendLine("    AND Operator.DelFlag = " + NSKUtil.BoolToInt(false) + "");

                    sb.AppendLine(" WHERE ");
                    sb.AppendLine("  HaishaExclusiveManage.OperatorId <> " + authInfo.OperatorId);

                    if (branchOfficeId.CompareTo(decimal.Zero) != 0)
                    {
                        // 営業所コード(画面より指定した営業所、または0:全営業所)
                        sb.AppendLine("   AND (HaishaExclusiveManage.BranchOfficeId = " + branchOfficeId);
                        sb.AppendLine("   OR HaishaExclusiveManage.BranchOfficeId = 0)");
                    }

                    if (!string.IsNullOrWhiteSpace(checkIdList))
                    {
                        sb.AppendLine("  AND HaishaExclusiveManage.WorkId IN ( " + checkIdList + ") ");
                    }

                    break;
                case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                    // 車種
                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine("  BranchOffice.BranchOfficeSNM ");
                    sb.AppendLine("  ,CarKind.CarKindCd AS Code ");
                    sb.AppendLine("  ,Operator.OperatorCode ");
                    sb.AppendLine("  ,Operator.OperatorName ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine("  HaishaExclusiveManage ");
                    sb.AppendLine("  LEFT JOIN TORADON_BranchOffice BranchOffice ");
                    sb.AppendLine("    ON HaishaExclusiveManage.BranchOfficeId = BranchOffice.BranchOfficeId ");
                    sb.AppendLine("    AND BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN TORADON_CarKind CarKind ");
                    sb.AppendLine("    ON HaishaExclusiveManage.WorkId = CarKind.CarKindId ");
                    sb.AppendLine("    AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Operator ");
                    sb.AppendLine("    ON HaishaExclusiveManage.OperatorId = Operator.OperatorId ");
                    sb.AppendLine("    AND Operator.DelFlag = " + NSKUtil.BoolToInt(false) + "");

                    sb.AppendLine(" WHERE ");
                    sb.AppendLine("  HaishaExclusiveManage.OperatorId <> " + authInfo.OperatorId);

                    if (branchOfficeId.CompareTo(decimal.Zero) != 0)
                    {
                        // 営業所コード(画面より指定した営業所、または0:全営業所)
                        sb.AppendLine("   AND (HaishaExclusiveManage.BranchOfficeId = " + branchOfficeId);
                        sb.AppendLine("   OR HaishaExclusiveManage.BranchOfficeId = 0)");
                    }

                    if (!string.IsNullOrWhiteSpace(checkIdList))
                    {
                        sb.AppendLine("  AND HaishaExclusiveManage.WorkId IN ( " + checkIdList + ") ");
                    }

                    break;
                default:

                    sb.AppendLine(" SELECT DISTINCT ");
                    sb.AppendLine("  BranchOffice.BranchOfficeSNM ");
                    sb.AppendLine("  ,0 AS Code");
                    sb.AppendLine("  ,Operator.OperatorCode ");
                    sb.AppendLine("  ,Operator.OperatorName ");
                    sb.AppendLine(" FROM ");
                    sb.AppendLine("  HaishaExclusiveManage ");
                    sb.AppendLine("  LEFT JOIN TORADON_BranchOffice BranchOffice ");
                    sb.AppendLine("    ON HaishaExclusiveManage.BranchOfficeId = BranchOffice.BranchOfficeId ");
                    sb.AppendLine("    AND BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                    sb.AppendLine("  LEFT JOIN Operator ");
                    sb.AppendLine("    ON HaishaExclusiveManage.OperatorId = Operator.OperatorId ");
                    sb.AppendLine("    AND Operator.DelFlag = " + NSKUtil.BoolToInt(false) + "");

                    sb.AppendLine(" WHERE ");
                    sb.AppendLine("  HaishaExclusiveManage.OperatorId <> " + authInfo.OperatorId);

                    if (branchOfficeId.CompareTo(decimal.Zero) != 0)
                    {
                        // 営業所コード(画面より指定した営業所、または0:全営業所)
                        sb.AppendLine("   AND (HaishaExclusiveManage.BranchOfficeId = " + branchOfficeId);
                        sb.AppendLine("   OR HaishaExclusiveManage.BranchOfficeId = 0)");
                    }

                    break;
            }


            return sb.ToString();
        }

        /// <summary>
        /// 配車排他管理を登録するSqlを返す。
        /// </summary>
        /// <param name="branchOfficeId">営業所ID</param>
        /// <param name="workId">検索ID</param>
        /// <returns>配車排他管理を登録するSql</returns>
        private string GetCommandHaishaExclusiveManageInsert(decimal? branchOfficeId, decimal workId)
        {
            //常設列の取得オプション
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            // nullの場合は0を設定
            branchOfficeId = branchOfficeId ?? 0;

            //// クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" INSERT  ");
            sb.AppendLine(" INTO HaishaExclusiveManage (  ");
            sb.AppendLine("   OperatorId ");
            sb.AppendLine("   , BranchOfficeId ");
            sb.AppendLine("   , WorkId ");
            sb.AppendLine("   , " + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" )  ");
            sb.AppendLine(" VALUES (  ");
            sb.AppendLine("   " + this.dbNullableString(authInfo.OperatorId));
            sb.AppendLine("   , " + this.dbNullableString(branchOfficeId));
            sb.AppendLine("   , " + this.dbNullableString(workId));
            sb.AppendLine("   , " + SQLHelper.GetPopulateColumnInsertString(this.authInfo, popOption));
            sb.AppendLine(" )");

            return sb.ToString();
        }

        /// <summary>
        /// 配車情報を削除するSqlCommandを返す。
        /// </summary>
        /// <returns>配車情報を削除するSqlCommand</returns>
        private SqlCommand GetCommandHaishaaExclusiveManageDelete()
        {
            // クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" DELETE FROM HaishaExclusiveManage WHERE OperatorId = " + authInfo.OperatorId);

            // パラメータ設定
            SqlCommand command = new SqlCommand(sb.ToString());

            return command;
        }

        /// <summary>
        /// DataReaderのNull値の場合、Stringのnull値に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string dbNullableString(object val)
        {
            if (val == null) return "null";

            return SQLHelper.dbNullableString(val);
        }

        #endregion
    }
}
