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
using Jpsys.HaishaManageV10.ComLib;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 権限名称テーブルのデータアクセスレイヤです。
    /// </summary>
    public class KengenName
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
        private string _tableName = "権限名称";

        /// <summary>
        /// 権限名称クラスのデフォルトコンストラクタです。
        /// </summary>
        public KengenName()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、権限名称テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public KengenName(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド
        /// <summary>
        /// SqlTransaction情報、権限名称情報を指定して、
        /// 権限名称情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="list">権限名称リスト</param>
        public void SaveList(SqlTransaction transaction, IList<KengenNameInfo> list)
        {
            try
            {
                List<string> mySqlSetList = new List<string>();

                //権限名称
                mySqlSetList.AddRange(
                    this.CreateSqlCommand_KengenName(transaction, list));

                //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                string query = SQLHelper.SQLQueryJoin(mySqlSetList);

                System.Diagnostics.Debug.WriteLine(
                    string.Format("SQL:{0}", query));

                SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
            }
            catch (SqlException err)
            {
                if (err.Number == Constants.SQLErrors.UniqueConstraintViolation)
                {
                    //リトライ可能な例外
                    throw new
                        Model.DALExceptions.CanRetryException(
                        string.Format("{0}情報の登録時にキー違反が発生したため、保存できませんでした。"
                            , this._tableName),
                        MessageBoxIcon.Warning);
                }
                else
                {
                    //ハンドルしない場合は再度スローする
                    throw;
                }
            }            
        }
        
        /// <summary>
        /// 検索条件情報を指定して、権限名称情報のリストを取得します。
        /// </summary>
        /// <returns>権限名称情報のリスト</returns>
        public IList<KengenNameInfo> GetList()
        {
            return GetListInternal(null);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、権限名称情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>権限名称情報リスト</returns>
        public IList<KengenNameInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<KengenNameInfo> rt_list = new List<KengenNameInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	KengenName.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	KengenName ");

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" KengenName.Kengen ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                KengenNameInfo rt_info = new KengenNameInfo
                {
                    Kengen = SQLServerUtil.dbDecimal(rdr["Kengen"]),
                    KengenName = rdr["KengenName"].ToString(),
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、権限名称情報を指定して、
        /// 権限名称情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="list">権限名称リスト</param>
        private IList<string> CreateSqlCommand_KengenName(SqlTransaction transaction, IList<KengenNameInfo> list)
        {
            //戻り値用
            List<string> rt_list = new List<string>();


            //DELETE文を作成
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE  ");
            sbDel.AppendLine(" KengenName ");

            //戻り値のリストに追加
            rt_list.Add(sbDel.ToString());



            //存在チェック用のコネクション
            SqlConnection conn = SQLHelper.GetSqlConnection();

            try
            {
                conn.Open();
                foreach (KengenNameInfo item in list)
                {
                    //常設列の取得オプションを作る
                    //--新規登録
                    SQLHelper.PopulateColumnOptions popOption =
                        SQLHelper.PopulateColumnOptions.EntryColumns |
                            SQLHelper.PopulateColumnOptions.AdditionColumns;

                    //Insert文を作成
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("INSERT INTO ");
                    sb.AppendLine(" KengenName ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine("	Kengen ");
                    sb.AppendLine("	,KengenName ");

                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                    sb.AppendLine(" ) ");
                    sb.AppendLine("VALUES ");
                    sb.AppendLine("( ");
                    sb.AppendLine("" + item.Kengen.ToString() + " ");
                    sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(item.KengenName.Trim()) + "' ");
                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
                    sb.AppendLine(") ");

                    //戻り値のリストに追加
                    rt_list.Add(sb.ToString());
                }



            }
            catch (Exception err)
            {

                throw err;
            }
            finally
            {
                conn.Close();
            }
            return rt_list;
        }

        #endregion
    }
}
