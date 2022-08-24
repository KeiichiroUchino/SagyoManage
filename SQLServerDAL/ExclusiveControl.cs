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
    /// 排他制御テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ExclusiveControl
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
        /// 排他制御クラスのデフォルトコンストラクタです。
        /// </summary>
        public ExclusiveControl()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、排他制御テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ExclusiveControl(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 区分指定で排他制御情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExclusiveControlInfo GetInfo(int kbn, SqlTransaction transaction = null)
        {
            ExclusiveControlInfo rt_info = new ExclusiveControlInfo();

            IList<ExclusiveControlInfo> list = this.GetListInternal(transaction, new ExclusiveControlSearchParameter()
            {
                ExclusiveControlKbn = kbn,
            });

            if (list != null && 0 < list.Count)
            {
                rt_info = list.FirstOrDefault();
            }

            return rt_info;
        }

        /// <summary>
        /// 条件指定で排他制御情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ExclusiveControlInfo GetInfoBySearchParameter(ExclusiveControlSearchParameter para, SqlTransaction transaction = null)
        {
            ExclusiveControlInfo rt_info = new ExclusiveControlInfo();

            IList<ExclusiveControlInfo> list = this.GetListInternal(transaction, para);

            if (list != null && 0 < list.Count)
            {
                rt_info = list.FirstOrDefault();
            }

            return rt_info;
        }

        /// <summary>
        /// 検索条件情報を指定して、排他制御情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>排他制御情報のリスト</returns>
        public IList<ExclusiveControlInfo> GetList(ExclusiveControlSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、排他制御情報を指定して、
        /// 排他制御情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">排他制御情報</param>
        public void Save(SqlTransaction transaction, ExclusiveControlInfo info)
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
                sb.AppendLine(" ExclusiveControl ");
                sb.AppendLine("SET ");
                sb.AppendLine("	 " + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                sb.AppendLine(" ,KinoKbn = " + info.KinoKbn.ToString() + " ");
                sb.AppendLine(" ,ShoriValue = " + info.ShoriValue.ToString() + " ");
                sb.AppendLine(" ,StatusKbn = " + info.StatusKbn.ToString() + " ");
                sb.AppendLine(" ,OperatorId = " + info.OperatorId.ToString() + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine(" ExclusiveControlKbn = " + info.ExclusiveControlKbn.ToString() + " ");
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

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine("  ExclusiveControl ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ExclusiveControlKbn ");
                sb.AppendLine(" ,KinoKbn ");
                sb.AppendLine(" ,ShoriValue ");
                sb.AppendLine(" ,StatusKbn ");
                sb.AppendLine(" ,OperatorId ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("  " + info.ExclusiveControlKbn.ToString() + "");
                sb.AppendLine(", " + info.KinoKbn.ToString() + " ");
                sb.AppendLine(", " + info.ShoriValue.ToString() + " ");
                sb.AppendLine(", " + info.StatusKbn.ToString() + " ");
                sb.AppendLine(", " + info.OperatorId.ToString() + " ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.ExclusiveControlKbn),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException("他のユーザが実行中のため処理を開始できません。");
                }
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<ExclusiveControlInfo> GetListInternal(SqlTransaction transaction, ExclusiveControlSearchParameter para)
        {
            //返却用のリスト
            List<ExclusiveControlInfo> rt_list = new List<ExclusiveControlInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ExclusiveControl.* ");
            sb.AppendLine("	,Operator.OperatorCode ");
            sb.AppendLine("	,Operator.OperatorName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" ExclusiveControl ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" Operator ");
            sb.AppendLine(" ON  Operator.OperatorId = ExclusiveControl.OperatorId ");
            sb.AppendLine(" AND Operator.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" ExclusiveControlKbn = " + para.ExclusiveControlKbn.ToString() + " ");

            if (para != null)
            {
                if (para.ExclusiveControlKbn.HasValue)
                {
                    sb.AppendLine("AND ExclusiveControl.ExclusiveControlKbn = " + para.ExclusiveControlKbn.ToString() + " ");
                }
                if (para.KinoKbn.HasValue)
                {
                    sb.AppendLine("AND ExclusiveControl.KinoKbn = " + para.KinoKbn.ToString() + " ");
                }
                if (para.ShoriValue.HasValue)
                {
                    sb.AppendLine("AND ExclusiveControl.ShoriValue = " + para.ShoriValue.ToString() + " ");
                }
                if (para.StatusKbn.HasValue)
                {
                    sb.AppendLine("AND ExclusiveControl.StatusKbn = " + para.StatusKbn.ToString() + " ");
                }
                if (para.ExclusionOperatorId.HasValue)
                {
                    sb.AppendLine("AND ExclusiveControl.OperatorId <> " + para.ExclusionOperatorId.ToString() + " ");
                }
            }

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ExclusiveControlInfo rt_info = new ExclusiveControlInfo
                {
                    ExclusiveControlKbn = SQLServerUtil.dbInt(rdr["ExclusiveControlKbn"]),
                    KinoKbn = SQLServerUtil.dbInt(rdr["KinoKbn"]),
                    ShoriValue = SQLServerUtil.dbDecimal(rdr["ShoriValue"]),
                    StatusKbn = SQLServerUtil.dbInt(rdr["StatusKbn"]),
                    OperatorId = SQLServerUtil.dbDecimal(rdr["OperatorId"]),

                    OperatorCode = rdr["OperatorCode"].ToString(),
                    OperatorName = rdr["OperatorName"].ToString()
                };

                if (0 < rt_info.ExclusiveControlKbn)
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
        /// <param name="kbn"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(int kbn)
        {
            return "SELECT 1 FROM ExclusiveControl WHERE ExclusiveControlKbn = " + kbn.ToString() + " HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
