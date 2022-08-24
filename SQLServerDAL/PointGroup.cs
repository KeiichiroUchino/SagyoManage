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
    /// 発着地グループテーブルのデータアクセスレイヤです。
    /// </summary>
    public class PointGroup
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
        /// テーブル名
        /// </summary>
        private string _tableName = "発着地グループ";

        /// <summary>
        /// PointGroupクラスのデフォルトコンストラクタです。
        /// </summary>
        public PointGroup()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地グループテーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public PointGroup(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で発着地グループ情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public PointGroupInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointGroupSearchParameter()
            {
                PointGroupCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で発着地グループ情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PointGroupInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointGroupSearchParameter()
            {
                PointGroupId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、発着地グループ情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地グループ情報のリスト</returns>
        public IList<PointGroupInfo> GetList(PointGroupSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、発着地グループ情報を指定して、
        /// 発着地グループ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地グループ情報</param>
        public void Save(SqlTransaction transaction, PointGroupInfo info)
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
                sb.AppendLine(" PointGroup ");
                sb.AppendLine("SET ");
                sb.AppendLine(" PointGroupName = N'" + SQLHelper.GetSanitaizingSqlString(info.PointGroupName.Trim()) + "'");
                sb.AppendLine(",PointGroupNameKana = N'" + SQLHelper.GetSanitaizingSqlString(info.PointGroupNameKana.Trim()) + "'");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("PointGroupId = " + info.PointGroupId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.PointGroupMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" PointGroup ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 PointGroupId ");
                sb.AppendLine("	,PointGroupCode ");
                sb.AppendLine("	,PointGroupName ");
                sb.AppendLine("	,PointGroupNameKana ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.PointGroupCode.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointGroupName.Trim()) + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointGroupNameKana.Trim()) + "'");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.PointGroupCode.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、発着地グループ情報を指定して、
        ///  発着地グループ情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地グループ情報</param>
        public void Delete(SqlTransaction transaction, PointGroupInfo info)
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
            sb.AppendLine(" PointGroup ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointGroupId = " + info.PointGroupId.ToString() + " ");
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

            ////発着地（トラDON補）テーブルの件数取得
            //if (new Point().GetKenSuu(transaction, new PointSearchParameter() { PointGroupId = info.PointGroupId }) != 0)
            //{
            //    //リトライ可能な例外
            //    throw new
            //        Model.DALExceptions.CanRetryException(
            //        "発着地（トラDON補）に該当データが使用されている為、削除できません。",
            //        MessageBoxIcon.Warning);
            //}

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、発着地グループテーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地グループテーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, PointGroupSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// SqlTransaction情報、発着地グループ情報、検索条件情報を指定して、
        /// 発着地グループ情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地グループ情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, PointGroupInfo info, int newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new PointGroup().GetKenSuu(transaction, new PointGroupSearchParameter() { PointGroupCode = newCode }) != 0)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName),
                    MessageBoxIcon.Warning);
            }

            #region コードの変更

            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Delete文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" PointGroup ");
            sb.AppendLine("SET ");
            sb.AppendLine("	PointGroupCode = " + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointGroupId = " + info.PointGroupId.ToString() + " ");
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
        public IList<PointGroupInfo> GetListInternal(SqlTransaction transaction, PointGroupSearchParameter para)
        {
            //返却用のリスト
            List<PointGroupInfo> rt_list = new List<PointGroupInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	PointGroup.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	PointGroup ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.PointGroupId.HasValue)
                {
                    sb.AppendLine("AND PointGroup.PointGroupId = " + para.PointGroupId.ToString() + " ");
                }
                if (para.PointGroupCode.HasValue)
                {
                    sb.AppendLine("AND PointGroup.PointGroupCode = " + para.PointGroupCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PointGroup.PointGroupCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointGroupInfo rt_info = new PointGroupInfo
                {
                    PointGroupId = SQLServerUtil.dbDecimal(rdr["PointGroupId"]),
                    PointGroupCode = SQLServerUtil.dbInt(rdr["PointGroupCode"]),
                    PointGroupName = rdr["PointGroupName"].ToString(),
                    PointGroupNameKana = rdr["PointGroupNameKana"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.PointGroupCode)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(string code)
        {
            return "SELECT 1 FROM PointGroup WHERE DelFlag = 0 " + "AND PointGroupCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
