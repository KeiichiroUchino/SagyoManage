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
    /// 発着地大分類テーブルのデータアクセスレイヤです。
    /// </summary>
    public class PointLBunrui
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
        private string _tableName = "発着地大分類";

        /// <summary>
        /// 発着地中分類クラス
        /// </summary>
        private PointMBunrui _PointMBunrui = null;

         /// <summary>
        /// 発着地大分類クラスのデフォルトコンストラクタです。
        /// </summary>
        public PointLBunrui()
        {
            this._PointMBunrui = new PointMBunrui();
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地大分類テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public PointLBunrui(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
            this._PointMBunrui = new PointMBunrui(authInfo);
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public PointLBunruiInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointLBunruiSearchParameter()
            {
                PointLBunruiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で発着地大分類情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public PointLBunruiInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointLBunruiSearchParameter()
            {
                PointLBunruiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、発着地大分類情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地大分類情報のリスト</returns>
        public IList<PointLBunruiInfo> GetList(PointLBunruiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 発着地大分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地大分類情報のコンボボックス用リスト</returns>
        public IList<PointLBunruiInfo> GetComboList()
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、発着地大分類情報を指定して、
        /// 発着地大分類情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地大分類情報</param>
        public void Save(SqlTransaction transaction, PointLBunruiInfo info)
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
                sb.AppendLine(" PointLBunrui ");
                sb.AppendLine("SET ");
                sb.AppendLine(" PointLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(info.PointLBunruiCode).Trim() + "' ");
                sb.AppendLine(",PointLBunruiName = N'" + SQLHelper.GetSanitaizingSqlString(info.PointLBunruiName).Trim() + "' ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + " ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.PointLBunruiMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" PointLBunrui ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  PointLBunruiId ");
                sb.AppendLine(" ,PointLBunruiCode ");
                sb.AppendLine(" ,PointLBunruiName ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointLBunruiCode).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointLBunruiName).Trim() + "' ");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.PointLBunruiCode),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                    , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、発着地大分類情報を指定して、
        ///  発着地大分類情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地大分類情報</param>
        public void Delete(SqlTransaction transaction, PointLBunruiInfo info)
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
            sb.AppendLine(" PointLBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
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
        /// SqlTransaction情報、発着地大分類情報、検索条件情報を指定して、
        /// 発着地大分類情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地大分類情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, PointLBunruiInfo info, string newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new PointLBunrui().GetKenSuu(transaction, new PointLBunruiSearchParameter() { PointLBunruiCode = newCode }) != 0)
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
            sb.AppendLine(" PointLBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	PointLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + "' ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
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
        private IList<PointLBunruiInfo> GetListInternal(SqlTransaction transaction, PointLBunruiSearchParameter para, bool getMFlag = true)
        {
            //返却用のリスト
            List<PointLBunruiInfo> rt_list = new List<PointLBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" PointLBunrui ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.PointLBunruiId.HasValue)
                {
                    sb.AppendLine("AND PointLBunruiId = " + para.PointLBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.PointLBunruiCode))
                {
                    sb.AppendLine("AND PointLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.PointLBunruiCode) + "' ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PointLBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointLBunruiInfo rt_info = new PointLBunruiInfo
                {
                    PointLBunruiId = SQLServerUtil.dbDecimal(rdr["PointLBunruiId"]),
                    PointLBunruiCode = rdr["PointLBunruiCode"].ToString(),
                    PointLBunruiName = rdr["PointLBunruiName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (getMFlag)
                {
                    rt_info.PointMBunruiList = this._PointMBunrui.GetList(
                        new PointMBunruiSearchParameter()
                    {
                        PointLBunruiId = rt_info.PointLBunruiId
                    }, transaction);
                }

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 発着地大分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地大分類情報のコンボボックス用リスト</returns>
        public IList<PointLBunruiInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<PointLBunruiInfo> rt_list = new List<PointLBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	PointLBunrui.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" PointLBunrui ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	PointLBunrui.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	PointLBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PointLBunrui.PointLBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointLBunruiInfo rt_info = new PointLBunruiInfo
                {
                    PointLBunruiId = SQLServerUtil.dbDecimal(rdr["PointLBunruiId"]),
                    PointLBunruiCode = rdr["PointLBunruiCode"].ToString(),
                    PointLBunruiName = rdr["PointLBunruiName"].ToString(),
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
        private string CreateCodeCheckSQL(string code)
        {
            return "SELECT 1 FROM PointLBunrui WHERE DelFlag = 0 " + "AND PointLBunruiCode = '" + code + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、発着地大分類テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地大分類テーブルの件数</returns>
        private int GetKenSuu(SqlTransaction transaction, PointLBunruiSearchParameter para)
        {
            return this.GetListInternal(transaction, para, false).Count;
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, PointLBunruiInfo info)
        {
            List<string> list = new List<string>();

            string PointLBunruiIdToString = info.PointLBunruiId.ToString();

            // 発着地中分類
            if (SQLHelper.RecordExists("SELECT 1 FROM PointMBunrui WHERE DelFlag = 0 AND PointMBunrui.PointLBunruiId = " + PointLBunruiIdToString, transaction))
            {
                list.Add("発着地中分類マスタ");
            }

            // 発着地
            if (SQLHelper.RecordExists("SELECT 1 FROM Point WHERE DelFlag = 0 AND Point.PointLBunruiId = " + PointLBunruiIdToString, transaction))
            {
                list.Add("発着地マスタ");
            }

            // 重複を除いて返却
            return list.Distinct().ToList();
        }

        #endregion
    }
}
