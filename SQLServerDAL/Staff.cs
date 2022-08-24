using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using System.Configuration;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 社員テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Staff
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
        /// 社員クラスのデフォルトコンストラクタです。
        /// </summary>
        public Staff()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、社員テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Staff(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で社員情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StaffInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new StaffSearchParameter()
            {
                StaffCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で社員情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public StaffInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new StaffSearchParameter()
            {
                StaffId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、社員情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>社員情報のリスト</returns>
        public IList<StaffInfo> GetList(StaffSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、社員情報を指定して、
        /// 社員情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">社員情報</param>
        public void Save(SqlTransaction transaction, StaffInfo info)
        {
            String id = string.Empty;

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
                sb.AppendLine(" Staff ");
                sb.AppendLine("SET ");
                sb.AppendLine(" StaffId =  " + info.StaffId.ToString() + " ");
                sb.AppendLine(" ,StaffCode =  " + info.StaffCode.ToString() + " ");
                sb.AppendLine(" ,StaffName = N'" + SQLHelper.GetSanitaizingSqlString(info.StaffName.Trim()) + "' ");
                sb.AppendLine(" ,StaffKbnId =  " + info.StaffKbnId.ToString() + " ");
                sb.AppendLine(" ,StaffTel = N'" + SQLHelper.GetSanitaizingSqlString(info.StaffTel.Trim()) + "' ");
                sb.AppendLine(" ,MailAddress =  N'" + SQLHelper.GetSanitaizingSqlString(info.MailAddress.Trim()) + "' ");
                sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("StaffId = " + info.StaffId.ToString() + " ");
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

                id = info.StaffId.ToString();
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Staff);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Staff ");
                sb.AppendLine(" ( ");
                sb.AppendLine(" StaffId ");
                sb.AppendLine(" ,StaffCode ");
                sb.AppendLine(" ,StaffName ");
                sb.AppendLine(" ,StaffKbnId ");
                sb.AppendLine(" ,StaffTel ");
                sb.AppendLine(" ,MailAddress ");
                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.StaffCode.ToString() + " ");
                sb.AppendLine(", N'" + SQLHelper.GetSanitaizingSqlString(info.StaffName.Trim()) + "' ");
                sb.AppendLine("," + info.StaffKbnId.ToString() + " ");
                sb.AppendLine(", N'" + SQLHelper.GetSanitaizingSqlString(info.StaffTel.Trim()) + "' ");
                sb.AppendLine(", N'" + SQLHelper.GetSanitaizingSqlString(info.MailAddress.Trim()) + "' ");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                id = newId.ToString();
            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.StaffCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }
        }

        /// <summary>
        ///  SqlTransaction情報、社員情報を指定して、
        ///  社員情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">社員情報</param>
        public void Delete(SqlTransaction transaction, StaffInfo info)
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
            sb.AppendLine(" Staff ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" StaffId = " + info.StaffId.ToString() + " ");
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

            #region 他テーブルの存在チェック

            //他のテーブルに使用されていないか
            IList<string> list = GetReferenceTables(transaction, info.StaffId);

            if (list.Count > 0)
            {
                StringBuilder sb_table = new StringBuilder();

                foreach (string table in list)
                {
                    sb_table.AppendLine(table);
                }

                //リトライ可能な例外
                SQLHelper.ThrowCanNotDeleteException(sb_table.ToString());
            }

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、社員テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>社員テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, StaffSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<StaffInfo> GetListInternal(SqlTransaction transaction, StaffSearchParameter para)
        {
            //返却用のリスト
            List<StaffInfo> rt_list = new List<StaffInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Staff.* ");
            sb.AppendLine("	,StaffKbn.MeishoCode AS StaffKbnCode ");
            sb.AppendLine("	,StaffKbn.Meisho AS StaffKbnName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Staff ");
            sb.AppendLine(" LEFT JOIN Meisho AS StaffKbn ");
            sb.AppendLine(" ON Staff.StaffKbnId = StaffKbn.MeishoId ");
            sb.AppendLine("     AND StaffKbn.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.StaffId.HasValue)
                {
                    sb.AppendLine("AND StaffId = " + para.StaffId.ToString() + " ");
                }
                if (para.StaffCode.HasValue)
                {
                    sb.AppendLine("AND StaffCode = " + para.StaffCode.ToString() + " ");
                }

                // 一括検索
                if (para.StaffCheckList.Length != 0)
                {
                    sb.AppendLine("AND StaffId IN ( " + para.StaffCheckList + ") ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" StaffCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                StaffInfo rt_info = new StaffInfo
                {
                    StaffId = SQLServerUtil.dbDecimal(rdr["StaffId"]),
                    StaffCode = SQLServerUtil.dbInt(rdr["StaffCode"]),
                    StaffName = rdr["StaffName"].ToString(),
                    StaffKbnId = SQLServerUtil.dbDecimal(rdr["StaffKbnId"]),
                    StaffKbnCode = SQLServerUtil.dbInt(rdr["StaffKbnCode"]),
                    StaffKbnName = rdr["StaffKbnName"].ToString(),
                    StaffTel = rdr["StaffTel"].ToString(),
                    MailAddress = rdr["MailAddress"].ToString(),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.StaffId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 当該IDを指定して、
        /// 各クラスで当該IDが使用されたレコードがないかチェックし存在する場合テーブル名を返します。
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="id">当該ID</param>
        /// <returns>存在するテーブル名</returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, decimal id)
        {
            List<string> list = new List<string>();

            //作業案件	責任者ID（社員ID）
            if (SQLHelper.RecordExists("SELECT * FROM SagyoAnken WHERE DelFlag = " + NSKUtil.BoolToInt(false) 
                + " AND SekininshaId = " + id.ToString() + "", transaction))
            {
                list.Add("作業案件");
            }

            //作業割り当て	社員ID
            if (SQLHelper.RecordExists("SELECT * FROM SagyoinWariate WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND StaffId = " + id.ToString() + "", transaction))
            {
                list.Add("作業割り当て");
            }

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(int code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT 1 ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Staff ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND StaffCode = " + code.ToString() + " ");
            sb.AppendLine(" HAVING Count(*) > 1 ");
            return sb.ToString();
        }

        #endregion
    }
}
