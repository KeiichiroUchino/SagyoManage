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
    /// 方面テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Homen
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
        private string _tableName = "方面";

        /// <summary>
        /// Homenクラスのデフォルトコンストラクタです。
        /// </summary>
        public Homen()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、方面テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Homen(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で方面情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HomenInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HomenSearchParameter()
            {
                HomenCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で方面情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HomenInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HomenSearchParameter()
            {
                HomenId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、方面情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>方面情報のリスト</returns>
        public IList<HomenInfo> GetList(HomenSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、方面情報を指定して、
        /// 方面情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">方面情報</param>
        public void Save(SqlTransaction transaction, HomenInfo info)
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
                sb.AppendLine(" Homen ");
                sb.AppendLine("SET ");
                sb.AppendLine(" HomenName = N'" + SQLHelper.GetSanitaizingSqlString(info.HomenName.Trim()) + "'");
                sb.AppendLine(",HomenNameKana = N'" + SQLHelper.GetSanitaizingSqlString(info.HomenNameKana.Trim()) + "'");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HomenId = " + info.HomenId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HomenMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Homen ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 HomenId ");
                sb.AppendLine("	,HomenCode ");
                sb.AppendLine("	,HomenName ");
                sb.AppendLine("	,HomenNameKana ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.HomenCode.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HomenName.Trim()) + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HomenNameKana.Trim()) + "'");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.HomenCode.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、方面情報を指定して、
        ///  方面情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">方面情報</param>
        public void Delete(SqlTransaction transaction, HomenInfo info)
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
            sb.AppendLine(" Homen ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HomenId = " + info.HomenId.ToString() + " ");
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

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、方面テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>方面テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, HomenSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// SqlTransaction情報、方面情報、検索条件情報を指定して、
        /// 方面情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">方面情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, HomenInfo info, int newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new Homen().GetKenSuu(transaction, new HomenSearchParameter() { HomenCode = newCode }) != 0)
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
            sb.AppendLine(" Homen ");
            sb.AppendLine("SET ");
            sb.AppendLine("	HomenCode = " + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HomenId = " + info.HomenId.ToString() + " ");
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
        public IList<HomenInfo> GetListInternal(SqlTransaction transaction, HomenSearchParameter para)
        {
            //返却用のリスト
            List<HomenInfo> rt_list = new List<HomenInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Homen.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	Homen ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" Homen.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HomenId.HasValue)
                {
                    sb.AppendLine("AND Homen.HomenId = " + para.HomenId.ToString() + " ");
                }
                if (para.HomenCode.HasValue)
                {
                    sb.AppendLine("AND Homen.HomenCode = " + para.HomenCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Homen.HomenCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HomenInfo rt_info = new HomenInfo
                {
                    HomenId = SQLServerUtil.dbDecimal(rdr["HomenId"]),
                    HomenCode = SQLServerUtil.dbInt(rdr["HomenCode"]),
                    HomenName = rdr["HomenName"].ToString(),
                    HomenNameKana = rdr["HomenNameKana"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.HomenCode)
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
            return "SELECT 1 FROM Homen WHERE DelFlag = 0 " + "AND HomenCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
