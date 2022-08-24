using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    public class OperateHistory
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo _authInfo =
            new AppAuthInfo { OperatorId = 0, TerminalId = "", UserProcessId = "" };

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public OperateHistory()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、ディーラーマスタの
        /// ビジネスロジックレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">認証済み利用情報</param>
        public OperateHistory(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region IOperateHistoryDAL メンバ

        /// <summary>
        /// ログ出力元のアプリケーション認証情報と、操作区分名、摘要を指定して、
        /// ログデータを登録します。ログ出力元のアプリケーション認証情報は、
        /// 本データアクセスレイヤを使用するためのアプリケーション認証情報
        /// ではなく、ログ出力に使用するアプリケーション認証情報です。
        /// </summary>
        /// <param name="logSrcAuthInfo">ログ出力元のアプリケーション認証情報</param>
        /// <param name="operateKbnString">操作区分の名称</param>
        /// <param name="remarkText">摘要</param>
        public void SetLoggingData(AppAuthInfo logSrcAuthInfo, string operateKbnString, string remarkText)
        {
            //無条件に挿入するのでシリアル値を取得
            //InsertのSQLを作る前に、次IDを取得しておく
            decimal nextid =
                SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.OpereteHistoryId);

            //修正 T.Kuroki 20111109 
            //  OperatorIDからオペレータ名を取得しないように変更する
            //　ためコメントアウト。
            //OperetorIDからオペレータの名前を取得しておく
            //string wk_opnm = GetOperatorName(logSrcAuthInfo.OperatorId);

            //修正 T.kuroki 20111109
            // 管理者ID利用時に顧客のオペレータIDに偽装するのをやめるため、
            // オペレータIDがDBに登録されていないことを想定し、名前を直接
            // 持ちまわるようにする。セキュリティ上問題はあるが仕方がない。。
            string wk_opnm = logSrcAuthInfo.OperetorName.Trim();

            //無条件に挿入するのでSQL文を作る
            StringBuilder sbIns = new StringBuilder();
            sbIns.Append("INSERT INTO ");
            sbIns.Append("OperateHistory ");
            sbIns.Append("( ");
            sbIns.Append("OperateHistoryId, TransactTime, OperatorName, ");
            sbIns.Append("TerminalId, ProcessId, OperateKbn, Remark ");
            sbIns.Append(") ");
            sbIns.Append("VALUES ");
            sbIns.Append("( ");
            sbIns.Append(
                nextid.ToString() + ",");
            sbIns.Append(
                SQLHelper.DateTimeToDbDateTime(DateTime.Now) + ",");
            sbIns.Append("N'" +
                wk_opnm.Trim() + "',");
            sbIns.Append("N'" +
                logSrcAuthInfo.TerminalId.Trim() + "',");
            sbIns.Append("N'" +
                logSrcAuthInfo.UserProcessName.Trim() + "',");
            sbIns.Append("N'" +
                operateKbnString.Trim() + "',");
            sbIns.Append("N'" +
                remarkText.Trim() + "'");
            sbIns.Append(") ");

            string mySqlUpd = sbIns.ToString();

            using (SqlConnection connSet = SQLHelper.GetSqlConnection())
            {
                connSet.Open();

                try
                {
                    var cmd = new SqlCommand(mySqlUpd, connSet);
                    //SQL実行
                    cmd.ExecuteNonQuery();
                }
                finally
                {
                    connSet.Close();
                }
            }
        }

        /// <summary>
        /// 開始処理日、終了処理日、検索文字列1から5を指定して、
        /// 操作履歴情報一覧を取得します。
        /// </summary>
        /// <param name="fromDate">開始処理日</param>
        /// <param name="toDate">終了処理日</param>
        /// <param name="searchString1">検索文字列1</param>
        /// <param name="searchString2">検索文字列2</param>
        /// <param name="searchString3">検索文字列3</param>
        /// <param name="searchString4">検索文字列4</param>
        /// <param name="searchString5">検索文字列5</param>
        /// <returns>操作履歴情報一覧</returns>
        public IList<OperateHistoryInfo> GetOperateHistoryInfoList(DateTime fromDate, DateTime toDate, string searchString1, string searchString2, string searchString3, string searchString4, string searchString5)
        {
            //返却用のリスト
            List<OperateHistoryInfo> rt_list =
                new List<OperateHistoryInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.Append("SELECT ");
            sb.Append("	* ");
            sb.Append("FROM ");
            sb.Append("	OperateHistory AS A ");
            sb.Append("WHERE ");
            //開始日付～処理日付内
            sb.Append(" A.TransactTime >= " +
                SQLHelper.DateTimeToDbDateTime(fromDate) + " ");
            sb.Append(" AND	A.TransactTime < " +
                SQLHelper.DateTimeToDbDateTime(toDate.AddDays(1)) + " ");
            //検索文字列1～5を全て含む
            sb.Append(" AND	(A.ProcessID + A.OperatorName + A.OperateKbn + A.Remark) LIKE '%" +
                SQLHelper.GetSanitaizingSqlString(searchString1.Trim()) + "%' ");
            sb.Append(" AND	(A.ProcessID + A.OperatorName + A.OperateKbn + A.Remark) LIKE '%" +
                SQLHelper.GetSanitaizingSqlString(searchString2.Trim()) + "%' ");
            sb.Append(" AND	(A.ProcessID + A.OperatorName + A.OperateKbn + A.Remark) LIKE '%" +
                SQLHelper.GetSanitaizingSqlString(searchString3.Trim()) + "%' ");
            sb.Append(" AND	(A.ProcessID + A.OperatorName + A.OperateKbn + A.Remark) LIKE '%" +
                SQLHelper.GetSanitaizingSqlString(searchString4.Trim()) + "%' ");
            sb.Append(" AND	(A.ProcessID + A.OperatorName + A.OperateKbn + A.Remark) LIKE '%" +
                SQLHelper.GetSanitaizingSqlString(searchString5.Trim()) + "%' ");
            sb.Append("ORDER BY ");
            //処理日の降順でソート
            sb.Append("	A.TransactTime desc");

            String mySql = sb.ToString();

            Debug.Print("\nmySql：" + mySql + "\n");


            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                OperateHistoryInfo wk_info = new OperateHistoryInfo()
                {
                    TransactTime = SQLHelper.dbDate(rdr["TransactTime"]),
                    OperatorName = rdr["OperatorName"].ToString(),
                    TerminalId = rdr["TerminalId"].ToString(),
                    ProcessId = rdr["ProcessId"].ToString(),
                    OperateKbn = rdr["OperateKbn"].ToString(),
                    Remark = rdr["Remark"].ToString()
                };

                return wk_info;
            });
        }

        #endregion

        /// <summary>
        /// オペレータIDを指定して、オペレータ名を取得します。
        /// </summary>
        /// <param name="operatorId">オペレータID</param>
        /// <returns>オペレータ名</returns>
        private string GetOperatorName(decimal operatorId)
        {
            string rt_val = string.Empty;

            //SQL文を作る
            StringBuilder sb = new StringBuilder();
            sb.Append("SELECT ");
            sb.Append("A.OperatorName ");
            sb.Append("FROM ");
            sb.Append("Operator AS A ");
            sb.Append("WHERE ");
            sb.Append("A.OperatorId =" + operatorId.ToString() + " ");
            string mySql = sb.ToString();

            return SQLHelper.SimpleReadSingle(mySql, rdr => rdr["OperatorName"].ToString()) ?? string.Empty;
        }
    }
}
