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
using Jpsys.HaishaManageV10.BizProperty;


namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 請求データクラスのビジネスロジックです。
    /// </summary>
    public class SeikyuData
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
        /// 請求データクラスのデフォルトコンストラクタです。
        /// </summary>
        public SeikyuData()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SeikyuData(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 請求データIdを指定して、情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="id">営業所Id</param>
        /// <returns>請求データ情報</returns>
        public SeikyuDataInfo GetInfo(decimal id)
        {
            SeikyuDataInfo info =
                this.GetListInternal(null,
                new SeikyuDataSearchParameter
                {
                    BranchOfficeId = id,
                }).FirstOrDefault();
            return info;
        }

        #region Save

        /// <summary>
        /// SqlTransaction情報、請求データ情報を指定して、
        /// 請求データ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">請求データ情報</param>
        public void Save(SqlTransaction transaction, SeikyuDataInfo info)
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
                sb.AppendLine("UPDATE ");
                sb.AppendLine("SeikyuData ");
                sb.AppendLine("SET ");

                sb.AppendLine("  SeikyuDataId = " + info.SeikyuDataId.ToString() + " ");
                sb.AppendLine(" ,BranchOfficeId = " + info.BranchOfficeId.ToString() + " ");

                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption));
                sb.AppendLine("WHERE ");
                sb.AppendLine("SeikyuDataId = " + info.SeikyuDataId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.SeikyuDataTrn);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine("SeikyuData ");
                sb.AppendLine("( ");

                sb.AppendLine("  SeikyuDataId ");
                sb.AppendLine(" ,BranchOfficeId ");

                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(") ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");

                sb.AppendLine("  " + newId.ToString() + "");
                sb.AppendLine(", " + info.BranchOfficeId.ToString() + " ");

                sb.AppendLine("," + SQLHelper.GetPopulateColumnInsertString(_authInfo, popOption));
                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        "他のユーザーによって請求データ作成が行われました。\r\n現在の状況を確認後、処理をやり直してください。");
                }
            }
        }

        #endregion

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<SeikyuDataInfo> GetListInternal(SqlTransaction transaction, SeikyuDataSearchParameter para)
        {
            //返却用のリスト
            List<SeikyuDataInfo> rt_list = new List<SeikyuDataInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	 SeikyuData ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" 1 = 1 ");

            if (para != null)
            {
                if (para.BranchOfficeId.HasValue)
                {
                    sb.AppendLine("AND SeikyuData.BranchOfficeId = "
                        + para.BranchOfficeId.Value.ToString() + " ");
                }
            }

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SeikyuDataInfo rt_info = new SeikyuDataInfo
                {
                    SeikyuDataId = SQLServerUtil.dbDecimal(rdr["SeikyuDataId"]),
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(SeikyuDataInfo info)
        {
            return "SELECT 1 FROM SeikyuData WHERE "
                + " BranchOfficeId = " + info.BranchOfficeId.ToString()
                + " HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
