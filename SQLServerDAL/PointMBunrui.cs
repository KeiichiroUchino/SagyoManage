using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 発着地中分類テーブルのデータアクセスレイヤです。
    /// </summary>
    public class PointMBunrui
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
        private string _tableName = "発着地中分類";

         /// <summary>
        /// 発着地中分類クラスのデフォルトコンストラクタです。
        /// </summary>
        public PointMBunrui()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地中分類テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public PointMBunrui(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="code"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public PointMBunruiInfo GetInfo(decimal lid, string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointMBunruiSearchParameter()
            {
                PointLBunruiId = lid,
                PointMBunruiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で発着地中分類情報を取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public PointMBunruiInfo GetInfoById(decimal lid, decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointMBunruiSearchParameter()
            {
                PointLBunruiId = lid,
                PointMBunruiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、発着地中分類情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地中分類情報のリスト</returns>
        public IList<PointMBunruiInfo> GetList(PointMBunruiSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// 発着地中分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <returns>発着地中分類情報のコンボボックス用リスト</returns>
        public IList<PointMBunruiInfo> GetComboList(decimal lid)
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、発着地中分類情報を指定して、
        /// 発着地中分類情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地中分類情報</param>
        public void Save(SqlTransaction transaction, PointMBunruiInfo info)
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
                sb.AppendLine(" PointMBunrui ");
                sb.AppendLine("SET ");
                sb.AppendLine(" PointMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(info.PointMBunruiCode).Trim() + "' ");
                sb.AppendLine(",PointMBunruiName = N'" + SQLHelper.GetSanitaizingSqlString(info.PointMBunruiName).Trim() + "' ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + " ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("PointMBunruiId = " + info.PointMBunruiId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.PointMBunruiMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" PointMBunrui ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  PointLBunruiId ");
                sb.AppendLine(" ,PointMBunruiId ");
                sb.AppendLine(" ,PointMBunruiCode ");
                sb.AppendLine(" ,PointMBunruiName ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("  " + info.PointLBunruiId.ToString() + " ");
                sb.AppendLine(", " + newId.ToString() + "");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointMBunruiCode).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointMBunruiName).Trim() + "' ");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.PointLBunruiId, info.PointMBunruiCode),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                    , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、発着地中分類情報を指定して、
        ///  発着地中分類情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地中分類情報</param>
        public void Delete(SqlTransaction transaction, PointMBunruiInfo info)
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
            sb.AppendLine(" PointMBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine(" PointMBunruiId = " + info.PointMBunruiId.ToString() + " ");
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

            // 他のテーブルに使用されていないか？
            IList<string> list = GetReferenceTables(transaction, info);

            if (list.Count > 0)
            {
                StringBuilder sb_table = new StringBuilder();

                foreach (string table in list)
                {
                    sb_table.AppendLine(table);
                }

                // リトライ可能な例外
                SQLHelper.ThrowCanNotDeleteException(sb_table.ToString());
            }

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、発着地中分類情報、検索条件情報を指定して、
        /// 発着地中分類情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地中分類情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, PointMBunruiInfo info, string newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new PointMBunrui().GetKenSuu(transaction, new PointMBunruiSearchParameter() { PointLBunruiId = info.PointLBunruiId, PointMBunruiCode = newCode }) != 0)
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
            sb.AppendLine(" PointMBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	PointMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + "' ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine(" PointMBunruiId = " + info.PointMBunruiId.ToString() + " ");
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
        private IList<PointMBunruiInfo> GetListInternal(SqlTransaction transaction, PointMBunruiSearchParameter para)
        {
            //返却用のリスト
            List<PointMBunruiInfo> rt_list = new List<PointMBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("	,PointLBunrui.PointLBunruiCode PointLBunruiCode ");
            sb.AppendLine("	,PointLBunrui.PointLBunruiName PointLBunruiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" PointMBunrui ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" PointLBunrui ");
            sb.AppendLine(" ON  PointLBunrui.PointLBunruiId = PointMBunrui.PointLBunruiId ");
            sb.AppendLine(" AND PointLBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	PointMBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.PointLBunruiId.HasValue)
                {
                    sb.AppendLine("AND PointMBunrui.PointLBunruiId = " + para.PointLBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.PointLBunruiCode))
                {
                    sb.AppendLine("AND PointLBunrui.PointLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.PointLBunruiCode) + "' ");
                }
                if (para.PointMBunruiId.HasValue)
                {
                    sb.AppendLine("AND PointMBunrui.PointMBunruiId = " + para.PointMBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.PointMBunruiCode))
                {
                    sb.AppendLine("AND PointMBunrui.PointMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.PointMBunruiCode) + "' ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PointMBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointMBunruiInfo rt_info = new PointMBunruiInfo
                {
                    PointLBunruiId = SQLServerUtil.dbDecimal(rdr["PointLBunruiId"]),
                    PointMBunruiId = SQLServerUtil.dbDecimal(rdr["PointMBunruiId"]),
                    PointMBunruiCode = rdr["PointMBunruiCode"].ToString(),
                    PointMBunruiName = rdr["PointMBunruiName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    PointLBunruiCode = rdr["PointLBunruiCode"].ToString(),
                    PointLBunruiName = rdr["PointLBunruiName"].ToString(),
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 発着地中分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地中分類情報のコンボボックス用リスト</returns>
        public IList<PointMBunruiInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<PointMBunruiInfo> rt_list = new List<PointMBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	PointMBunrui.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" PointMBunrui ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	PointMBunrui.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	PointMBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PointMBunrui.PointMBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointMBunruiInfo rt_info = new PointMBunruiInfo
                {
                    PointLBunruiId = SQLServerUtil.dbDecimal(rdr["PointLBunruiId"]),
                    PointMBunruiId = SQLServerUtil.dbDecimal(rdr["PointMBunruiId"]),
                    PointMBunruiCode = rdr["PointMBunruiCode"].ToString(),
                    PointMBunruiName = rdr["PointMBunruiName"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(decimal lid, string code)
        {
            return "SELECT 1 FROM PointMBunrui WHERE DelFlag = 0 " + "AND PointLBunruiId = " + lid.ToString() + " AND PointMBunruiCode = '" + code + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、発着地中分類テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地中分類テーブルの件数</returns>
        private int GetKenSuu(SqlTransaction transaction, PointMBunruiSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, PointMBunruiInfo info)
        {
            List<string> list = new List<string>();

            string PointMBunruiIdToString = info.PointMBunruiId.ToString();

            // 発着地マスタ
            if (SQLHelper.RecordExists("SELECT 1 FROM Point WHERE DelFlag = 0 AND Point.PointMBunruiId = " + PointMBunruiIdToString, transaction))
            {
                list.Add("発着地マスタ");
            }

            // 重複を除いて返却
            return list.Distinct().ToList();
        }

        #endregion
    }
}
