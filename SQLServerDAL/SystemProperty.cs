using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.BizProperty;
using System.Configuration;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 管理テーブルのデータアクセスレイヤです。
    /// </summary>
    public class SystemProperty
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
        private string _tableName = "管理";

        /// <summary>
        /// 管理クラスのデフォルトコンストラクタです。
        /// </summary>
        public SystemProperty()
        {

        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SystemProperty(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// SqlTransaction情報、管理情報を指定して、
        /// 管理情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">管理情報</param>
        public void Save(SqlTransaction transaction, SystemPropertyInfo info)
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
                sb.AppendLine(" SystemProperty ");
                sb.AppendLine("SET ");

                sb.AppendLine("  SystemPropertyId = " + info.SystemPropertyId.ToString() + " ");
                sb.AppendLine(" ,Email = N'" + SQLHelper.GetSanitaizingSqlString(info.Email).Trim() + "'");
                sb.AppendLine(" ,SenderName = N'" + SQLHelper.GetSanitaizingSqlString(info.SenderName).Trim() + "'");
                sb.AppendLine(" ,MailTitle = N'" + SQLHelper.GetSanitaizingSqlString(info.MailTitle).Trim() + "'");
                sb.AppendLine(" ,MailBody = N'" + SQLHelper.GetSanitaizingSqlString(info.MailBody).Trim() + "'");
                sb.AppendLine(" ,PDFTempFolderPath = N'" + SQLHelper.GetSanitaizingSqlString(info.PDFTempFolderPath).Trim() + "'");
                sb.AppendLine(" ,SendGridApiKey = N'" + SQLHelper.GetSanitaizingSqlString(info.SendGridApiKey).Trim() + "'");
                sb.AppendLine(" ,CompanyName = N'" + SQLHelper.GetSanitaizingSqlString(info.CompanyName).Trim() + "'");
                sb.AppendLine(" ,DateSwitchingTime =" + info.DateSwitchingTime.ToString() + " ");

                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                sb.AppendLine("WHERE ");
                sb.AppendLine(" SystemPropertyId = 1 ");
                sb.AppendLine("--排他のチェック ");
                sb.AppendLine(" AND	VersionColumn = @versionColumn ");

                string sql = sb.ToString();

                SqlCommand command = new SqlCommand(sql);
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command, this._tableName);
            }
            else
            {
                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //管理IDは1を登録する
                info.SystemPropertyId = 1;

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" SystemProperty ");
                sb.AppendLine(" ( ");

                sb.AppendLine("  SystemPropertyId ");
                sb.AppendLine(" ,Email "); ;
                sb.AppendLine(" ,SenderName ");
                sb.AppendLine(" ,MailTitle ");
                sb.AppendLine(" ,MailBody ");
                sb.AppendLine(" ,PDFTempFolderPath ");
                sb.AppendLine(" ,SendGridApiKey ");
                sb.AppendLine(" ,CompanyName ");
                sb.AppendLine(" ,DateSwitchingTime ");

                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");

                sb.AppendLine("  " + info.SystemPropertyId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.Email).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.SenderName).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MailTitle).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MailBody).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PDFTempFolderPath).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.SendGridApiKey).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.CompanyName).Trim() + "'");
                sb.AppendLine("," + info.DateSwitchingTime.ToString() + " ");

                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
                sb.AppendLine(") ");

                string sql = sb.ToString();

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql), this._tableName);
            }

            return;
        }

        /// <summary>
        /// コードを指定して、管理情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>管理情報</returns>
        public SystemPropertyInfo GetInfo(SqlTransaction transaction = null)
        {
            SystemPropertyInfo info = this.GetListInternal(transaction)
                .FirstOrDefault();

            return info;
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 管理情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>管理情報一覧</returns>
        private IList<SystemPropertyInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用の一覧
            List<SystemPropertyInfo> rt_list = new List<SystemPropertyInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 SystemProperty.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	SystemProperty --管理 ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	SystemPropertyId = 1 ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SystemPropertyInfo rt_info = new SystemPropertyInfo
                {
                    SystemPropertyId = SQLServerUtil.dbDecimal(rdr["SystemPropertyId"]),
                    Email = rdr["Email"].ToString(),
                    SenderName = rdr["SenderName"].ToString(),
                    MailTitle = rdr["MailTitle"].ToString(),
                    MailBody = rdr["MailBody"].ToString(),
                    PDFTempFolderPath = rdr["PDFTempFolderPath"].ToString(),
                    SendGridApiKey = rdr["SendGridApiKey"].ToString(),
                    CompanyName = rdr["CompanyName"].ToString(),
                    DateSwitchingTime = SQLServerUtil.dbInt(rdr["DateSwitchingTime"]),
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
