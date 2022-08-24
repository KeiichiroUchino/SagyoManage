using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using System.Configuration;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 作業場所テーブルのデータアクセスレイヤです。
    /// </summary>
    public class SagyoBasho
    {
        /// <summary>
        /// 一括処理件数
        /// </summary>
        private const int BLUK_COUNT = 100;

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
        /// 作業場所クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoBasho()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、作業場所テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SagyoBasho(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、作業場所情報を指定して、
        /// 作業場所情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業場所情報</param>
        public void Save(SqlTransaction transaction, SagyoBashoInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateUpSagyoBashoCommands(info));
            }
            else
            {
                info.SagyoBashoId = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.SagyoBasho);

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateInSagyoBashoCommands(info));
            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.SagyoBashoCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }
        }

        /// <summary>
        /// SqlTransaction情報、作業場所情報を指定して、
        /// 作業場所情報を一括の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業場所情報</param>
        public void BulkSave(SqlTransaction transaction, List<SagyoBashoInfo> list)
        {

            List<SqlCommand> commands = new List<SqlCommand>();
            List<string> codeList = new List<string>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.SagyoBasho, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

            foreach (var info in list)
            {
                //データベースに保存されているかどうか、または先にINSERTしたレコードと同じコードのデータが存在している
                if (info.IsPersisted)
                {
                    //UPDATE
                    commands.Add(this.CreateUpSagyoBashoCommands(info, false));
                }
                else
                {
                    //--InsertのSQLを作る前に、次IDを取得しておく
                    meisaiId_iterator.MoveNext();
                    info.SagyoBashoId = meisaiId_iterator.Current;

                    //INSERT
                    commands.Add(this.CreateInSagyoBashoCommands(info));
                }
                codeList.Add(info.SagyoBashoCode);

                if (commands.Count() == BLUK_COUNT)
                {
                    //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                    string query = SQLHelper.SQLQueryJoin(commands.Select(x => x.CommandText));

                    if (query != string.Empty)
                    {
                        SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
                    }

                    //コードの存在チェック
                    if (SQLHelper.RecordExists(CreateCodeCheckSQL(string.Empty, codeList),
                        transaction))
                    {
                        throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
                    }

                    commands.Clear();
                    codeList.Clear();
                }
            }

            if (commands.Count() > 0)
            {
                //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                string query = SQLHelper.SQLQueryJoin(commands.Select(x => x.CommandText));

                if (query != string.Empty)
                {
                    SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
                }

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(string.Empty, codeList),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、作業場所情報を指定して、
        ///  作業場所情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業場所情報</param>
        public void Delete(SqlTransaction transaction, SagyoBashoInfo info)
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
            sb.AppendLine(" SagyoBasho ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" SagyoBashoId = " + info.SagyoBashoId.ToString() + " ");
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

            //他のテーブルに使用されていないか
            IList<string> list = GetReferenceTables(transaction, info.SagyoBashoId);

            if (list.Count > 0)
            {
                StringBuilder sb_table = new StringBuilder();

                foreach (string table in list)
                {
                    sb_table.AppendLine(table);
                }

                //リトライ可能な例外
                SQLHelper.ThrowCanNotDeleteException(sb_table.ToString());
            }

            #endregion
        }


        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoBashoInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoBashoSearchParameter()
            {
                SagyoBashoCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で作業場所情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoBashoInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoBashoSearchParameter()
            {
                SagyoBashoId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、作業場所情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>作業場所情報のリスト</returns>
        public IList<SagyoBashoInfo> GetList(SagyoBashoSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<SagyoBashoInfo> GetListInternal(SqlTransaction transaction, SagyoBashoSearchParameter para)
        {

            String mySql = string.Empty;

            // クエリ
            mySql = this.GetQuerySelect(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoBashoInfo rt_info = new SagyoBashoInfo
                {
                    SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                    TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),
                    SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    SagyoBashoAddress1 = rdr["SagyoBashoAddress1"].ToString(),
                    SagyoBashoAddress2 = rdr["SagyoBashoAddress2"].ToString(),
                    ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.SagyoBashoId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。(一括取得)
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public List<SagyoBashoInfo> BulkGetListInternal(List<string> list,
            SqlTransaction transaction = null, bool exclusiveflg = false)
        {
            String mySql = string.Empty;

            List<SagyoBashoInfo> resultList = new List<SagyoBashoInfo>();

            // 該当データがない場合は終了
            if (list.Count() == 0) return new List<SagyoBashoInfo>();

            // キー検索の場合
            int i = 0;
            foreach (string val in list)
            {
                // クエリ
                // 2回目以降はUNIONで連結
                if (i > 0) mySql = mySql + Environment.NewLine + " UNION " + Environment.NewLine;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ");
                sb.AppendLine("	* ");
                sb.AppendLine("FROM ");
                sb.AppendLine(" SagyoBasho ");

                if (exclusiveflg)
                {
                    sb.AppendLine(" SagyoBasho WITH (UPDLOCK,ROWLOCK) ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND ShelterId = " + SQLHelper.GetSanitaizingSqlString(val.Trim()) + " ");
                }
                else
                {
                    sb.AppendLine(" SagyoBasho ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND SagyoBashoCode = N'" + SQLHelper.GetSanitaizingSqlString(val.Trim()) + "' ");
                }


                mySql = mySql + sb.ToString();
                i++;

                if (i == BLUK_COUNT)
                {
                    resultList.AddRange(SQLHelper.SimpleRead(mySql, rdr =>
                    {
                        //返却用の値
                        SagyoBashoInfo rt_info = new SagyoBashoInfo
                        {
                            SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                            TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                            SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                            SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                            SagyoBashoAddress1 = rdr["SagyoBashoAddress1"].ToString(),
                            SagyoBashoAddress2 = rdr["SagyoBashoAddress2"].ToString(),
                            ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                            DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                            DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                        };

                        if (0 < rt_info.SagyoBashoId)
                        {
                            //入力者以下の常設フィールドをセットする
                            rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                        }

                        //返却用の値を返します
                        return rt_info;
                    }, transaction));

                    i = 0;
                    mySql = string.Empty;
                }

            }

            if (i > 0)
            {
                resultList.AddRange(SQLHelper.SimpleRead(mySql, rdr =>
                {
                    //返却用の値
                    SagyoBashoInfo rt_info = new SagyoBashoInfo
                    {
                        SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                        TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                        SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                        SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                        SagyoBashoAddress1 = rdr["SagyoBashoAddress1"].ToString(),
                        SagyoBashoAddress2 = rdr["SagyoBashoAddress2"].ToString(),
                        ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                        DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                        DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                    };

                    if (0 < rt_info.SagyoBashoId)
                    {
                        //入力者以下の常設フィールドをセットする
                        rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                    }

                    //返却用の値を返します
                    return rt_info;
                }, transaction));

            }

            return resultList;
        }


        /// <summary>
        /// 当該IDを指定して、
        /// 各クラスで当該IDが使用されたレコードがないかチェックし存在する場合テーブル名を返します。
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="id">当該ID</param>
        /// <returns>存在するテーブル名</returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, decimal id)
        {
            List<string> list = new List<string>();

            //契約	作業場所ID
            if (SQLHelper.RecordExists("SELECT * FROM Keiyaku WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND SagyoBashoId = " + id.ToString() + "", transaction))
            {
                list.Add("契約");
            }

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(string code, List<string> list = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT 1 ");
            sb.AppendLine("FROM ");

            if (list == null)
            {
                sb.AppendLine(" SagyoBasho ");
                sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                sb.AppendLine("AND SagyoBashoCode = N'" + code + "' ");

            }
            else
            {
                string mySql = string.Empty;

                // キー検索の場合
                int i = 0;
                foreach (string cd in list)
                {
                    // 2回目以降はUNIONで連結
                    if (i > 0) mySql = mySql + Environment.NewLine + " UNION " + Environment.NewLine;

                    //SQL文を作成
                    StringBuilder sbs = new StringBuilder();
                    sbs.AppendLine("SELECT ");
                    sbs.AppendLine("	* ");
                    sbs.AppendLine("FROM ");
                    sbs.AppendLine(" SagyoBasho ");
                    sbs.AppendLine("WHERE ");
                    sbs.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sbs.AppendLine("AND SagyoBashoCode = N'" + SQLHelper.GetSanitaizingSqlString(cd.Trim()) + "' ");

                    mySql = mySql + sbs.ToString();
                    i++;
                }

                sb.AppendLine(" (" + mySql + ") S ");
                sb.AppendLine(" Group By S.SagyoBashoCode ");
            }

            sb.AppendLine(" HAVING Count(*) > 1 ");
            return sb.ToString();
        }

        /// <summary>
        /// 作業場所を更新するSqlCommandを作成する
        /// </summary>
        /// <param name="info">作業場所</param>
        /// <returns></returns>
        private SqlCommand CreateUpSagyoBashoCommands(SagyoBashoInfo info, bool exclusiveflg = true)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" SagyoBasho ");
            sb.AppendLine("SET ");
            sb.AppendLine(" SagyoBashoId =  " + info.SagyoBashoId.ToString() + " ");
            sb.AppendLine(" ,TokuisakiId =  " + info.TokuisakiId.ToString() + " ");
            sb.AppendLine(" ,SagyoBashoCode =  N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoCode.Trim()) + "' ");
            sb.AppendLine(" ,SagyoBashoName =  N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoName.Trim()) + "' ");
            sb.AppendLine(" ,SagyoBashoAddress1 =  N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoAddress1.Trim()) + "' ");
            sb.AppendLine(" ,SagyoBashoAddress2 =  N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoAddress2.Trim()) + "' ");
            sb.AppendLine(" ,ShelterId = " + info.ShelterId.ToString() + "");
            sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("SagyoBashoId = " + info.SagyoBashoId.ToString() + " ");
            if (exclusiveflg)
            {
                sb.AppendLine("--排他のチェック ");
                sb.AppendLine("AND ");
                sb.AppendLine("VersionColumn = @versionColumn ");
            }

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            if (exclusiveflg)
            {
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));
            }
            return command;
        }

        /// <summary>
        /// 作業場所を追加するSqlCommandを作成する
        /// </summary>
        /// <param name="info">作業場所</param>
        /// <returns></returns>
        private SqlCommand CreateInSagyoBashoCommands(SagyoBashoInfo info)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" SagyoBasho ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  SagyoBashoId ");
            sb.AppendLine(" ,TokuisakiId  ");
            sb.AppendLine(" ,SagyoBashoCode  ");
            sb.AppendLine(" ,SagyoBashoName  ");
            sb.AppendLine(" ,SagyoBashoAddress1  ");
            sb.AppendLine(" ,SagyoBashoAddress2  ");
            sb.AppendLine(" ,ShelterId  ");
            sb.AppendLine(" ,DisableFlag  ");
            sb.AppendLine(" ,DelFlag  ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine("" + info.SagyoBashoId.ToString() + "");
            sb.AppendLine(" ," + info.TokuisakiId.ToString() + " ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoCode.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoName.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoAddress1.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoBashoAddress2.Trim()) + "' ");
            sb.AppendLine(" , " + info.ShelterId.ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine(" , " + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
            sb.AppendLine(") ");

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            return command;
        }

        /// <summary>
        /// 作業場所を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private string GetQuerySelect(SagyoBashoSearchParameter para, bool orderByFlg = true)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	SagyoBasho.* ");
            sb.AppendLine("	,Tokuisaki.TokuisakiCode ");
            sb.AppendLine("	,Tokuisaki.TokuisakiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" SagyoBasho ");
            sb.AppendLine(" LEFT JOIN Tokuisaki ");
            sb.AppendLine(" ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.SagyoBashoId.HasValue && para.SagyoBashoId > 0)
                {
                    sb.AppendLine("AND SagyoBasho.SagyoBashoId = " + para.SagyoBashoId.ToString() + " ");
                }
                if (para.TokuisakiId.HasValue && para.TokuisakiId > 0)
                {
                    sb.AppendLine("AND SagyoBasho.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (!string.IsNullOrWhiteSpace(para.SagyoBashoCode))
                {
                    sb.AppendLine("AND SagyoBasho.SagyoBashoCode = N'" + SQLHelper.GetSanitaizingSqlString(para.SagyoBashoCode.Trim()) + "' ");
                }
                if (!string.IsNullOrWhiteSpace(para.SagyoBashoCodeAmbiguous))
                {
                    sb.AppendLine("AND SagyoBasho.SagyoBashoCode LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.SagyoBashoCodeAmbiguous.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.SagyoBashoName))
                {
                    sb.AppendLine("AND SagyoBasho.SagyoBashoName LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.SagyoBashoName.Trim()) + "%') ");
                }
                if (para.ShelterId.HasValue && para.ShelterId > 0 && !para.DuplicateFlg)
                {
                    sb.AppendLine("AND SagyoBasho.ShelterId = " + para.ShelterId.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND SagyoBasho.DisableFlag = " + NSKUtil.BoolToInt(para.DisableFlag.Value).ToString() + " ");
                }
                if (para.DuplicateFlg)
                {
                    sb.AppendLine("AND SagyoBasho.ShelterId <> " + para.ShelterId.ToString() + " ");
                }
            }

            if (orderByFlg)
            {
                sb.AppendLine("ORDER BY ");
                sb.AppendLine(" SagyoBasho.SagyoBashoCode ");
            }

            return sb.ToString();
        }

        #endregion
    }
}
