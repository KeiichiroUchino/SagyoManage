using Jpsys.HaishaManageV10.Model;
using System.Data.SqlClient;
using System.Text;


namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 配車排他管理クラスのビジネスロジックです。
    /// </summary>
    public class HaishaExclusiveManage
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
        /// 配車排他管理クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaExclusiveManage()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaishaExclusiveManage(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        ///  SqlTransaction情報、配車排他管理情報を指定して、
        ///  配車排他管理情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配車排他管理情報</param>
        public void Delete(SqlTransaction transaction, HaishaExclusiveManageInfo info)
        {
            #region レコードの削除

            //Delete文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("DELETE  ");
            sb.AppendLine(" HaishaExclusiveManage ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" OperatorId = " + info.OperatorId.ToString() + " ");
            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);

            #endregion
        }

        #endregion

        #region プライベートメソッド

        #endregion
    }
}
