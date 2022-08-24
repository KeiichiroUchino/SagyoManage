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
    /// 郵便番号ワークテーブルのデータアクセスレイヤです。
    /// </summary>
    public class WK_PostalZipData
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
        /// 郵便番号ワーククラスのデフォルトコンストラクタです。
        /// </summary>
        public WK_PostalZipData()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、郵便番号ワークテーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public WK_PostalZipData(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、郵便番号ワーク情報を指定して、
        /// 郵便番号ワーク情報の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">郵便番号ワーク情報</param>
        /// <param name="kenAllCsvList">KEN_ALL_CSV情報リスト</param>
        /// <param name="araigaeFlag">洗い替えフラグ（true：洗い替え、false：追記）</param>
        public void Save(SqlTransaction transaction, ExclusiveControlInfo info, IList<KenAllCsvInfo> kenAllCsvList,
            bool araigaeFlag)
        {
            //排他制御
            new ExclusiveControl(this._authInfo).Save(transaction, info);

            //SQLリスト
            IList<string> mySqlList = new List<string>();

            //常設列の取得オプションを作る
            //--新規登録
            SQLHelper.PopulateColumnOptions popInsOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //郵便番号、郵便番号ワーク初期化
            this.InitTables(transaction, araigaeFlag);

            //KEN_ALL_CSVリストが存在する場合
            if (kenAllCsvList != null && 0 < kenAllCsvList.Count)
            {
                decimal rowId = 1;

                //追記の場合
                if (!araigaeFlag)
                {
                    //現在の郵便番号ID + 1を取得
                    rowId = this.GetMaxId(transaction);
                }

                //郵便番号データ登録SQL取得
                foreach (KenAllCsvInfo meisai in kenAllCsvList)
                {
                    try
                    {
                        //Insert文を作成
                        StringBuilder sb = new StringBuilder();
                        sb.AppendLine("INSERT INTO ");
                        sb.AppendLine(" WK_PostalZipData ");
                        sb.AppendLine(" ( ");
                        sb.AppendLine("  OperatorId ");
                        sb.AppendLine(" ,ZipId ");
                        sb.AppendLine(" ,ZipCode ");
                        sb.AppendLine(" ,PrefName ");
                        sb.AppendLine(" ,CityName ");
                        sb.AppendLine(" ,TownName ");
                        sb.AppendLine(" ,OtherName ");

                        sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popInsOption));

                        sb.AppendLine(" ) ");
                        sb.AppendLine("VALUES ");
                        sb.AppendLine("( ");
                        sb.AppendLine("  " + this._authInfo.OperatorId.ToString() + " ");
                        sb.AppendLine(" ," + rowId++.ToString() + " ");
                        sb.AppendLine(" ," + meisai.PostCode.ToString() + " ");
                        sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(meisai.Prefecture).Trim() + "' ");
                        sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(meisai.City).Trim() + "' ");
                        sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(meisai.TownArea).Trim() + "' ");
                        sb.AppendLine(" ,''");

                        sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popInsOption));

                        sb.AppendLine(") ");

                        //郵便番号ワーク登録SQL追加
                        mySqlList.Add(sb.ToString());
                    }
                    catch
                    {
                        throw;
                    }
                }

                //郵便番号登録SQL取得
                StringBuilder sbIns = new StringBuilder();
                sbIns.AppendLine("INSERT INTO PostalZipData( ");
                sbIns.AppendLine("  ZipId ");
                sbIns.AppendLine(" ,ZipTextCode ");
                sbIns.AppendLine(" ,ZipCode ");
                sbIns.AppendLine(" ,PrefName ");
                sbIns.AppendLine(" ,CityName ");
                sbIns.AppendLine(" ,TownName ");
                sbIns.AppendLine(" ,OtherName ");
                sbIns.AppendLine(") ");
                sbIns.AppendLine("SELECT ");
                sbIns.AppendLine("  ZipId ");
                sbIns.AppendLine(" ,FORMAT(ZipCode, 'D7') ");
                sbIns.AppendLine(" ,ZipCode ");
                sbIns.AppendLine(" ,PrefName ");
                sbIns.AppendLine(" ,CityName ");
                sbIns.AppendLine(" ,TownName ");
                sbIns.AppendLine(" ,OtherName ");
                sbIns.AppendLine("FROM WK_PostalZipData ");
                sbIns.AppendLine("WHERE OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

                //郵便番号登録SQL追加
                mySqlList.Add(sbIns.ToString());
            }

            //1000件ごとのSQLリストに分割
            var sqlLists = mySqlList.Select((v, i) => new { v, i })
                .GroupBy(x => x.i / 1000)
                .Select(g => g.Select(x => x.v));

            //1000件ごとに実行
            foreach (var sqlList in sqlLists)
            {
                // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                string query = SQLHelper.SQLQueryJoin(sqlList);

                // 指定したトランザクション上でExecuteNonqueryを実行
                SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報を指定して、テーブルの初期化を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="araigaeFlag">洗い替えフラグ（true：洗い替え、false：追記）</param>
        private void InitTables(SqlTransaction transaction, bool araigaeFlag)
        {
            //SQLリスト
            IList<string> mySqlList = new List<string>();

            //郵便番号ワーク削除SQL取得
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE FROM ");
            sbDel.AppendLine(" WK_PostalZipData ");
            sbDel.AppendLine("WHERE ");
            sbDel.AppendLine(" OperatorId = " + this._authInfo.OperatorId.ToString() + " ");

            //郵便番号ワーク削除SQL追加
            mySqlList.Add(sbDel.ToString());

            if (araigaeFlag)
            {
                //郵便番号削除SQL取得
                sbDel = new StringBuilder();
                sbDel.AppendLine("DELETE FROM ");
                sbDel.AppendLine(" PostalZipData ");

                //郵便番号削除SQL追加
                mySqlList.Add(sbDel.ToString());
            }

            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(mySqlList);

            // 指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
        }

        /// <summary>
        /// 郵便番号辞書の最大ID + 1を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        private decimal GetMaxId(SqlTransaction transaction)
        {
            //SQL文の作成
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append(" Max(ZipId) AS ZipId ");
            sb.Append("FROM ");
            sb.Append(" PostalZipData ");

            string mySql = sb.ToString();

            decimal menuId = SQLHelper.SimpleReadSingle(mySql, rdr => SQLServerUtil.dbDecimal(rdr["ZipId"]), transaction);

            return menuId + 1;
        }

        #endregion
    }
}
