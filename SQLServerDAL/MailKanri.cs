using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;
using System.Configuration;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// メール管理テーブルのデータアクセスレイヤです。
    /// </summary>
    public class MailKanri
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
        private string _tableName = "メール管理";

        /// <summary>
        /// メール管理クラスのデフォルトコンストラクタです。
        /// </summary>
        public MailKanri()
        {

        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public MailKanri(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// SqlTransaction情報、メール管理情報を指定して、
        /// メール管理情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">メール管理情報</param>
        public void Save(SqlTransaction transaction, MailKanriInfo info)
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
                sb.AppendLine(" MailKanri ");
                sb.AppendLine("SET ");

                sb.AppendLine("  MailKanriId = " + info.MailKanriId.ToString() + " ");
                sb.AppendLine(" ,SMTPServer = N'" + SQLHelper.GetSanitaizingSqlString(info.SMTPServer).Trim() + "' ");
                sb.AppendLine(" ,SoshinPortBango = " + info.SoshinPortBango.ToString() + " ");
                sb.AppendLine(" ,MailAccount = N'" + SQLHelper.GetSanitaizingSqlString(info.MailAccount).Trim() + "' ");
                sb.AppendLine(" ,MailPassword = N'" + SQLHelper.GetSanitaizingSqlString(info.MailPassword).Trim() + "' ");
                sb.AppendLine(" ,MailTitle = N'" + SQLHelper.GetSanitaizingSqlString(info.MailTitle).Trim() + "' ");

                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                sb.AppendLine("WHERE ");
                sb.AppendLine(" MailKanriId = 1 ");
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

                //メール管理IDは1を登録する
                info.MailKanriId = 1;

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" MailKanri ");
                sb.AppendLine(" ( ");

                sb.AppendLine("  MailKanriId ");
                sb.AppendLine(" ,SMTPServer ");
                sb.AppendLine(" ,SoshinPortBango ");
                sb.AppendLine(" ,MailAccount ");
                sb.AppendLine(" ,MailPassword ");
                sb.AppendLine(" ,MailTitle ");

                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");

                sb.AppendLine("  " + info.MailKanriId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.SMTPServer).Trim() + "' ");
                sb.AppendLine(", " + info.SoshinPortBango.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MailAccount).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MailPassword).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MailTitle).Trim() + "' ");

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
        /// コードを指定して、メール管理情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>メール管理情報</returns>
        public MailKanriInfo GetInfo(SqlTransaction transaction = null)
        {
            MailKanriInfo info = this.GetListInternal(transaction)
                .FirstOrDefault();

            return info;
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// メール管理情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>メール管理情報一覧</returns>
        private IList<MailKanriInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用の一覧
            List<MailKanriInfo> rt_list = new List<MailKanriInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 MailKanri.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	MailKanri --メール管理 ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	MailKanriId = 1 ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                MailKanriInfo rt_info = new MailKanriInfo
                {
                    MailKanriId = SQLServerUtil.dbDecimal(rdr["MailKanriId"]),
                    SMTPServer = rdr["SMTPServer"].ToString(),
                    SoshinPortBango = SQLServerUtil.dbInt(rdr["SoshinPortBango"]),
                    MailAccount = rdr["MailAccount"].ToString(),
                    MailPassword = rdr["MailPassword"].ToString(),
                    MailTitle = rdr["MailTitle"].ToString(),
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
