using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.ComLib;
using Jpsys.SagyoManage.BizProperty;


namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 利用者クラスのビジネスロジックです。
    /// </summary>
    public class Operator
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
        /// オペレータクラスのデフォルトコンストラクタです。
        /// </summary>
        public Operator()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Operator(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// ログインIdを指定して、情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="code">ログインId</param>
        /// <returns>利用者情報</returns>
        public OperatorInfo GetInfo(string code)
        {
            if (code == null || code.Equals(string.Empty))
                return null;

            OperatorInfo info =
                this.GetListInternal(null,
                new OperatorSearchParameter
                {
                    OperatorCode = code,
                }).FirstOrDefault();
            return info;
        }

        /// <summary>
        /// Id指定で利用者情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public OperatorInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new OperatorSearchParameter()
            {
                OperatoId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、オペレータ情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>オペレータ情報のリスト</returns>
        public IList<OperatorInfo> GetList(OperatorSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、オペレータ情報を指定して、
        /// オペレータ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">オペレータ情報</param>
        public void Save(SqlTransaction transaction, OperatorInfo info)
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
                sb.AppendLine("Operator ");
                sb.AppendLine("SET ");
                sb.AppendLine("OperatorCode = N'" + SQLHelper.GetSanitaizingSqlString(info.OperatorCode.Trim()) + "', ");
                sb.AppendLine("OperatorName = N'" + SQLHelper.GetSanitaizingSqlString(info.OperatorName.Trim()) + "', ");
                sb.AppendLine("Password = N'" + SQLHelper.GetSanitaizingSqlString(info.Password.Trim()) + "', ");
                sb.AppendLine("ToraDONStaffId = " + info.ToraDONStaffId.ToString() + ", ");
                sb.AppendLine("AuthorityId = " + info.AuthorityId.ToString() + ", ");
                sb.AppendLine("AdminKbn = " + info.AdminKbn.ToString() + ", ");
                sb.AppendLine("LoginYMD = " + SQLHelper.DateTimeToDbDateTime(info.LoginYMD).ToString() + ", ");

                sb.AppendLine("DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + ", ");
                sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine(", " + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("OperatorId = " + info.OperatorId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.OperatorMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine("Operator ");
                sb.AppendLine("( ");
                sb.AppendLine(" OperatorId ");
                sb.AppendLine(" ,OperatorCode ");
                sb.AppendLine(" ,OperatorName ");
                sb.AppendLine(" ,PassWord ");
                sb.AppendLine(" ,ToraDONStaffId ");
                sb.AppendLine(" ,AuthorityId ");
                sb.AppendLine(" ,AdminKbn ");
                sb.AppendLine(" ,LoginYMD ");

                sb.AppendLine(" ,DisableFlag ");
                sb.AppendLine(" ,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(") ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine(" " + newId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.OperatorCode.Trim()) + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.OperatorName.Trim()) + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.Password.Trim()) + "'");
                sb.AppendLine("," + info.ToraDONStaffId.ToString() + " ");
                sb.AppendLine("," + info.AuthorityId.ToString() + " ");
                sb.AppendLine("," + info.AdminKbn.ToString() + " ");
                sb.AppendLine(", " + SQLHelper.DateTimeToDbDateTime(info.LoginYMD).ToString() + " ");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.OperatorCode),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException("同じログインIDが既に登録されています。");
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、オペレータ情報を指定して、
        ///  オペレータ情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">利用者情報</param>
        public void Delete(SqlTransaction transaction, OperatorInfo info)
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
            sb.AppendLine(" Operator ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("Operator.OperatorCode = N'"
                + SQLHelper.GetSanitaizingSqlString(info.OperatorCode.Trim()) + "' ");
            sb.AppendLine("AND  Operator.DelFlag = "
                + NSKUtil.BoolToInt(false).ToString() + "");
            sb.AppendLine("--排他のチェック ");
            sb.AppendLine("AND	VersionColumn = @versionColumn ");
            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。
            SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command);

            //他のテーブルに使用されていないか
            IList<string> list = GetReferenceTables(transaction, info);

            if (list.Count > 0)
            {
                StringBuilder sb_table = new StringBuilder();

                foreach (string table in list)
                {
                    sb_table.AppendLine(table);
                }

                //リトライ可能な例外
                SQLHelper.ThrowCanNotDeleteException(sb_table.ToString(), info.OperatorCode.ToString());
            }

            #endregion
        }
        
        /// <summary>
        /// オペレータコードとパスワードを指定してログイン処理を実行します。
        /// </summary>
        /// <param name="operatorCode">オペレータコード</param>
        /// <param name="password">パスワード</param>
        /// <returns>オペレーター情報</returns>
        public OperatorInfo Login(string operatorCode, string password)
        {
            //返却値用
            OperatorInfo rt_info = null;

            //共通_利用者情報を取得
            OperatorInfo info =
                GetListInternal(null
                , new OperatorSearchParameter() { OperatorCode = operatorCode })
                .FirstOrDefault();

            if (info != null)
            {
                //非表示ではない
                if (!info.DisableFlag)
                {
                    //パスワードが同じか？
                    if (info.Password == password)
                    {
                        rt_info = info;
                    }
                    else
                    {
                        //リトライ可能な例外
                        throw new
                            Model.DALExceptions.CanRetryException(
                            "ログインID、またはパスワードが正しくありません。"
                            , MessageBoxIcon.Warning);
                    }
                }
            }
            //返却
            return rt_info;
        }

        /// <summary>
        /// SqlTransaction情報、オペレータ情報を指定して、
        /// オペレータ情報のログイン日時を更新します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">オペレータ情報</param>
        public void SaveLoginYMD(SqlTransaction transaction, OperatorInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //Update文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE ");
                sb.AppendLine("Operator ");
                sb.AppendLine("SET ");
                sb.AppendLine("LoginYMD = " + SQLHelper.DateTimeToDbDateTime(info.LoginYMD).ToString() + " ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("OperatorId = " + info.OperatorId.ToString() + " ");
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
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<OperatorInfo> GetListInternal(SqlTransaction transaction, OperatorSearchParameter para)
        {
            //返却用のリスト
            List<OperatorInfo> rt_list = new List<OperatorInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Operator.* ");
            //sb.AppendLine(" ,KengenName.KengenName ");
            //sb.AppendLine(" ,ToraDONStaff.StaffCd ToraDONStaffCode ");
            //sb.AppendLine(" ,ToraDONStaff.StaffNM ToraDONStaffName ");
            //sb.AppendLine(" ,ToraDONBranchOffice.BranchOfficeId ToraDONBranchOfficeId ");
            //sb.AppendLine(" ,ToraDONBranchOffice.BranchOfficeCd ToraDONBranchOfficeCode ");
            //sb.AppendLine(" ,ToraDONBranchOffice.BranchOfficeNM ToraDONBranchOfficeName ");
            //sb.AppendLine(" ,HaishaExclusiveManage.LastHaishaEntryDateTime ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	Operator ");
            //sb.AppendLine(" LEFT OUTER JOIN ");
            //sb.AppendLine(" KengenName ON ");
            //sb.AppendLine(" Operator.AuthorityId = KengenName.Kengen ");
            //sb.AppendLine(" LEFT OUTER JOIN ");
            //sb.AppendLine(" TORADON_Staff ToraDONStaff ");
            //sb.AppendLine(" ON  ToraDONStaff.StaffId = Operator.ToraDONStaffId ");
            //sb.AppendLine(" AND ToraDONStaff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            //sb.AppendLine(" LEFT OUTER JOIN ");
            //sb.AppendLine(" TORADON_BranchOffice ToraDONBranchOffice ");
            //sb.AppendLine(" ON  ToraDONBranchOffice.BranchOfficeId = ToraDONStaff.BranchOfficeId ");
            //sb.AppendLine(" AND ToraDONBranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            //sb.AppendLine(" LEFT OUTER JOIN ");
            //sb.AppendLine(" (SELECT OperatorId, MAX(EntryDateTime) LastHaishaEntryDateTime FROM HaishaExclusiveManage GROUP BY OperatorId) HaishaExclusiveManage ");
            //sb.AppendLine(" ON  HaishaExclusiveManage.OperatorId = Operator.OperatorId ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	Operator.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.OperatoId.HasValue)
                {
                    sb.AppendLine("AND Operator.OperatorId = "
                        + para.OperatoId.Value.ToString() + " ");
                }

                if (para.OperatorCode != string.Empty && para.OperatorCode != null)
                {
                    sb.AppendLine("AND Operator.OperatorCode = N'" + SQLHelper.GetSanitaizingSqlString(para.OperatorCode.ToString().Trim()) + "' ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Operator.OperatorCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                OperatorInfo rt_info = new OperatorInfo
                {
                    OperatorId = SQLServerUtil.dbDecimal(rdr["OperatorId"]),
                    OperatorCode = rdr["OperatorCode"].ToString(),
                    OperatorName = rdr["OperatorName"].ToString(),
                    Password = rdr["Password"].ToString(),
                    AuthorityId = SQLServerUtil.dbDecimal(rdr["AuthorityId"]),
                    //KengenName = rdr["KengenName"].ToString(),
                    LoginYMD = SQLHelper.dbDate(rdr["LoginYMD"]),
                    AdminKbn = SQLServerUtil.dbInt(rdr["AdminKbn"]),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    //ToraDONStaffCode = SQLServerUtil.dbInt(rdr["ToraDONStaffCode"]),
                    //ToraDONStaffName = rdr["ToraDONStaffName"].ToString(),
                    //ToraDONBranchOfficeId = SQLServerUtil.dbDecimal(rdr["ToraDONBranchOfficeId"]),
                    //ToraDONBranchOfficeCode = SQLServerUtil.dbInt(rdr["ToraDONBranchOfficeCode"]),
                    //ToraDONBranchOfficeName = rdr["ToraDONBranchOfficeName"].ToString(),
                    //LastHaishaEntryDateTime = SQLHelper.dbDate(rdr["LastHaishaEntryDateTime"])
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, OperatorInfo info)
        {
            List<string> list = new List<string>();

            ////コンテナ予約（受付担当者）
            //if (SQLHelper.RecordExists("SELECT 1 FROM ContainerYoyaku WHERE UketsukeTantoId = " + info.OperatorId.ToString() + "", transaction))
            //{
            //    list.Add("コンテナ予約（受付担当者）");
            //}

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(string code)
        {
            return "SELECT 1 FROM Operator WHERE DelFlag = 0 " + "AND OperatorCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
