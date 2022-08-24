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
    /// 得意先テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Tokuisaki
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
        /// 得意先クラスのデフォルトコンストラクタです。
        /// </summary>
        public Tokuisaki()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、得意先テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Tokuisaki(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、得意先情報を指定して、
        /// 得意先情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">得意先情報</param>
        public void Save(SqlTransaction transaction, TokuisakiInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateUpTokuisakiCommands(info));
            }
            else
            {
                info.TokuisakiId = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Tokuisaki);

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateInTokuisakiCommands(info));
            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.TokuisakiCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }
        }

        /// <summary>
        /// SqlTransaction情報、得意先情報を指定して、
        /// 得意先情報を一括の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">得意先情報</param>
        public void BulkSave(SqlTransaction transaction, List<TokuisakiInfo> list)
        {

            List<SqlCommand> commands = new List<SqlCommand>();
            List<string> codeList = new List<string>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.Tokuisaki, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

            foreach (var info in list)
            {
                //データベースに保存されているかどうか
                if (info.IsPersisted)
                {
                    //UPDATE
                    commands.Add(this.CreateUpTokuisakiCommands(info, false));
                }
                else
                {
                    //--InsertのSQLを作る前に、次IDを取得しておく
                    meisaiId_iterator.MoveNext();
                    info.TokuisakiId = meisaiId_iterator.Current;

                    //INSERT
                    commands.Add(this.CreateInTokuisakiCommands(info));
                }
                codeList.Add(info.TokuisakiCode);

                if (commands.Count() == BLUK_COUNT)
                {
                    //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                    string query = SQLHelper.SQLQueryJoin(commands.Select(x => x.CommandText));

                    if (query != string.Empty)
                    {
                        SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
                    }

                    //コードの存在チェック
                    if (SQLHelper.RecordExists(CreateCodeCheckSQL(string.Empty, codeList),transaction))
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
        ///  SqlTransaction情報、得意先情報を指定して、
        ///  得意先情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">社員情報</param>
        public void Delete(SqlTransaction transaction, TokuisakiInfo info)
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
            sb.AppendLine(" Tokuisaki ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" TokuisakiId = " + info.TokuisakiId.ToString() + " ");
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
            IList<string> list = GetReferenceTables(transaction, info.TokuisakiId);

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
        public TokuisakiInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new TokuisakiSearchParameter()
            {
                TokuisakiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で得意先情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TokuisakiInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new TokuisakiSearchParameter()
            {
                TokuisakiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、得意先情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>得意先情報のリスト</returns>
        public IList<TokuisakiInfo> GetList(TokuisakiSearchParameter para = null)
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
        public IList<TokuisakiInfo> GetListInternal(SqlTransaction transaction, TokuisakiSearchParameter para)
        {
            String mySql = string.Empty;

            // クエリ
            mySql = this.GetQuerySelect(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                TokuisakiInfo rt_info = new TokuisakiInfo
                {
                    TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),
                    TokuisakiAddress1 = rdr["TokuisakiAddress1"].ToString(),
                    TokuisakiAddress2 = rdr["TokuisakiAddress2"].ToString(),
                    TokuisakiTel = rdr["TokuisakiTel"].ToString(),
                    TokuisakiFax = rdr["TokuisakiFax"].ToString(),
                    ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.TokuisakiId)
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
        /// <param name="keiyakuCodeList">検索条件情報(IDリスト)</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="exclusiveflg">排他フラグ</param>
        /// <returns>情報リスト</returns>
        public List<TokuisakiInfo> BulkGetListInternal(List<string> list,
            SqlTransaction transaction = null, bool exclusiveflg = false)
        {
            String mySql = string.Empty;

            List<TokuisakiInfo> resultList = new List<TokuisakiInfo>();

            // 該当データがない場合は終了
            if (list.Count() == 0) return new List<TokuisakiInfo>();

            // キー検索の場合
            int i = 0;
            foreach (string id in list)
            {
                // クエリ
                // 2回目以降はUNIONで連結
                if (i > 0) mySql = mySql + Environment.NewLine + " UNION " + Environment.NewLine;

                StringBuilder sb = new StringBuilder();
                sb.AppendLine("SELECT ");
                sb.AppendLine("	* ");
                sb.AppendLine("FROM ");
                sb.AppendLine(" Tokuisaki ");

                if (exclusiveflg)
                {
                    sb.AppendLine(" Tokuisaki WITH (UPDLOCK,ROWLOCK) ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND ShelterId = " + SQLHelper.GetSanitaizingSqlString(id.Trim()) + " ");
                }
                else
                {
                    sb.AppendLine(" Tokuisaki ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND TokuisakiCode = N'" + SQLHelper.GetSanitaizingSqlString(id.Trim()) + "' ");
                }

                mySql = mySql + sb.ToString();
                i++;

                if (i == BLUK_COUNT)
                {
                    resultList.AddRange(SQLHelper.SimpleRead(mySql, rdr =>
                    {
                        //返却用の値
                        TokuisakiInfo rt_info = new TokuisakiInfo
                        {
                            TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                            TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                            TokuisakiName = rdr["TokuisakiName"].ToString(),
                            TokuisakiAddress1 = rdr["TokuisakiAddress1"].ToString(),
                            TokuisakiAddress2 = rdr["TokuisakiAddress2"].ToString(),
                            TokuisakiTel = rdr["TokuisakiTel"].ToString(),
                            TokuisakiFax = rdr["TokuisakiFax"].ToString(),
                            ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                            DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                            DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                        };

                        if (0 < rt_info.TokuisakiId)
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
                    TokuisakiInfo rt_info = new TokuisakiInfo
                    {
                        TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]),
                        TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                        TokuisakiName = rdr["TokuisakiName"].ToString(),
                        TokuisakiAddress1 = rdr["TokuisakiAddress1"].ToString(),
                        TokuisakiAddress2 = rdr["TokuisakiAddress2"].ToString(),
                        TokuisakiTel = rdr["TokuisakiTel"].ToString(),
                        TokuisakiFax = rdr["TokuisakiFax"].ToString(),
                        ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                        DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                        DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                    };

                    if (0 < rt_info.TokuisakiId)
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

            //作業場所	得意先ID
            if (SQLHelper.RecordExists("SELECT * FROM SagyoBasho WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND TokuisakiId = " + id.ToString() + "", transaction))
            {
                list.Add("作業場所");
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
            sb.AppendLine(" SELECT 1 ");
            sb.AppendLine(" FROM ");

            if (list == null)
            {
                sb.AppendLine(" Tokuisaki ");
                sb.AppendLine(" WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                sb.AppendLine(" AND TokuisakiCode = N'" + code + "' ");
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
                    sbs.AppendLine(" Tokuisaki ");
                    sbs.AppendLine("WHERE ");
                    sbs.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sbs.AppendLine("AND TokuisakiCode = N'" + SQLHelper.GetSanitaizingSqlString(cd.Trim()) + "' ");

                    mySql = mySql + sbs.ToString();
                    i++;
                }

                sb.AppendLine(" (" + mySql + ") T ");
                sb.AppendLine(" Group By T.TokuisakiCode ");
            }

            sb.AppendLine(" HAVING Count(*) > 1 ");
            return sb.ToString();
        }

        /// <summary>
        /// 得意先を更新するSqlCommandを作成する
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private SqlCommand CreateUpTokuisakiCommands(TokuisakiInfo info, bool exclusiveflg = true)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" Tokuisaki ");
            sb.AppendLine("SET ");
            sb.AppendLine(" TokuisakiId =  " + info.TokuisakiId.ToString() + " ");
            sb.AppendLine(" ,TokuisakiCode =  N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiCode.Trim()) + "' ");
            sb.AppendLine(" ,TokuisakiName = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiName.Trim()) + "' ");
            sb.AppendLine(" ,TokuisakiAddress1 = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiAddress1.Trim()) + "' ");
            sb.AppendLine(" ,TokuisakiAddress2 = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiAddress2.Trim()) + "' ");
            sb.AppendLine(" ,TokuisakiTel = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiTel.Trim()) + "' ");
            sb.AppendLine(" ,TokuisakiFax = N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiFax.Trim()) + "' ");
            sb.AppendLine(" ,ShelterId = " + info.ShelterId.ToString() + "");
            sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("TokuisakiId = " + info.TokuisakiId.ToString() + " ");
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
        /// 得意先を追加するSqlCommandを作成する
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private SqlCommand CreateInTokuisakiCommands(TokuisakiInfo info)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" Tokuisaki ");
            sb.AppendLine(" ( ");
            sb.AppendLine(" TokuisakiId ");
            sb.AppendLine(" ,TokuisakiCode  ");
            sb.AppendLine(" ,TokuisakiName  ");
            sb.AppendLine(" ,TokuisakiAddress1  ");
            sb.AppendLine(" ,TokuisakiAddress2  ");
            sb.AppendLine(" ,TokuisakiTel  ");
            sb.AppendLine(" ,TokuisakiFax  ");
            sb.AppendLine(" ,ShelterId  ");
            sb.AppendLine(" ,DisableFlag  ");
            sb.AppendLine(" ,DelFlag  ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine("" + info.TokuisakiId.ToString() + "");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiCode.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiName.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiAddress1.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiAddress2.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiTel.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.TokuisakiFax.Trim()) + "' ");
            sb.AppendLine(" , " + info.ShelterId.ToString() + "");
            sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
            sb.AppendLine(") ");

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            return command;
        }

        /// <summary>
        /// 得意先を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private string GetQuerySelect(TokuisakiSearchParameter para, bool orderByFlg = true)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Tokuisaki ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.TokuisakiId.HasValue && para.TokuisakiId > 0)
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiCode))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiCode.Trim()) + "' ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiCodeAmbiguous))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiCode  LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiCodeAmbiguous.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiName))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiName LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiName.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiAddress1))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiAddress1 LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiAddress1.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiAddress2))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiAddress2 LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiAddress2.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiTel))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiTel LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiTel.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.TokuisakiFax))
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiFax LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.TokuisakiFax.Trim()) + "%') ");
                }
                if (para.ShelterId.HasValue && para.ShelterId > 0 && !para.DuplicateFlg)
                {
                    sb.AppendLine("AND Tokuisaki.ShelterId = " + para.ShelterId.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND Tokuisaki.DisableFlag = " + NSKUtil.BoolToInt(para.DisableFlag.Value).ToString() + " ");
                }
                if (para.DuplicateFlg)
                {
                    sb.AppendLine("AND Tokuisaki.ShelterId <> " + para.ShelterId.ToString() + " ");
                }
            }

            if (orderByFlg)
            {
                sb.AppendLine("ORDER BY ");
                sb.AppendLine(" Tokuisaki.TokuisakiCode ");
            }

            return sb.ToString();
        }

        #endregion
    }
}
