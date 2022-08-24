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
    /// 作業中分類テーブルのデータアクセスレイヤです。
    /// </summary>
    public class SagyoChuBunrui
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
        /// 作業中分類クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoChuBunrui()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、作業中分類テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SagyoChuBunrui(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoChuBunruiInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoChuBunruiSearchParameter()
            {
                SagyoChuBunruiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で作業中分類情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoChuBunruiInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoChuBunruiSearchParameter()
            {
                SagyoChuBunruiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、作業中分類情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>作業中分類情報のリスト</returns>
        public IList<SagyoChuBunruiInfo> GetList(SagyoChuBunruiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。(一括取得)
        /// </summary>
        /// <param name="keiyakuCodeList">検索条件情報(IDリスト)</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="exclusiveflg">排他フラグ</param>
        /// <returns>情報リスト</returns>
        public List<SagyoChuBunruiInfo> BulkGetListInternal(List<string> list,
            SqlTransaction transaction = null, bool exclusiveflg = false)
        {
            String mySql = string.Empty;

            List<SagyoChuBunruiInfo> resultList = new List<SagyoChuBunruiInfo>();

            // 該当データがない場合は終了
            if (list.Count() == 0) return new List<SagyoChuBunruiInfo>();

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
                sb.AppendLine(" SagyoChuBunrui ");

                if (exclusiveflg)
                {
                    sb.AppendLine(" SagyoChuBunrui WITH (UPDLOCK,ROWLOCK) ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND ShelterId = " + SQLHelper.GetSanitaizingSqlString(id.Trim()) + " ");
                }
                else
                {
                    sb.AppendLine(" SagyoChuBunrui ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND SagyoChuBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(id.Trim()) + "' ");
                }

                mySql = mySql + sb.ToString();
                i++;

                if (i == BLUK_COUNT)
                {
                    resultList.AddRange(SQLHelper.SimpleRead(mySql, rdr =>
                    {
                        //返却用の値
                        SagyoChuBunruiInfo rt_info = new SagyoChuBunruiInfo
                        {
                            SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                            SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                            SagyoChuBunruiCode = SQLServerUtil.dbInt(rdr["SagyoChuBunruiCode"]),
                            SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                            ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                            DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                            DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                        };

                        if (0 < rt_info.SagyoChuBunruiId)
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
                    SagyoChuBunruiInfo rt_info = new SagyoChuBunruiInfo
                    {
                        SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                        SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                        SagyoChuBunruiCode = SQLServerUtil.dbInt(rdr["SagyoChuBunruiCode"]),
                        SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                        ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                        DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                        DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                    };

                    if (0 < rt_info.SagyoChuBunruiId)
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
        /// SqlTransaction情報、作業分類情報を指定して、
        /// 作業分類情報を一括の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業分類情報</param>
        public void BulkSave(SqlTransaction transaction, List<SagyoChuBunruiInfo> list)
        {

            List<SqlCommand> commands = new List<SqlCommand>();
            List<string> codeList = new List<string>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.SagyoChuBunrui, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

            //一括削除
            //SqlCommand command = this.CreateDelCommands(); 
            //SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);

            commands.Add(this.CreateDelCommands());

            foreach (var info in list)
            {
                //データベースに保存されているかどうか
                if (info.IsPersisted)
                {
                    //UPDATE
                    commands.Add(this.CreateUpCommands(info, false));
                }
                else
                {
                    //--InsertのSQLを作る前に、次IDを取得しておく
                    meisaiId_iterator.MoveNext();
                    info.SagyoChuBunruiId = meisaiId_iterator.Current;

                    //INSERT
                    commands.Add(this.CreateInCommands(info));
                }
                codeList.Add(info.SagyoChuBunruiCode.ToString());

                if (commands.Count() == BLUK_COUNT)
                {
                    //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                    string query = SQLHelper.SQLQueryJoin(commands.Select(x => x.CommandText));

                    if (query != string.Empty)
                    {
                        SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
                    }

                    //コードの存在チェック
                    if (SQLHelper.RecordExists(CreateCodeCheckSQL(string.Empty, codeList), transaction))
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

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<SagyoChuBunruiInfo> GetListInternal(SqlTransaction transaction, SagyoChuBunruiSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	SagyoChuBunrui.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" SagyoChuBunrui ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.SagyoChuBunruiId.HasValue && para.SagyoChuBunruiId > 0)
                {
                    sb.AppendLine("AND SagyoChuBunrui.SagyoChuBunruiId = " + para.SagyoChuBunruiId.ToString() + " ");
                }
                if (para.SagyoDaiBunruiId.HasValue && para.SagyoDaiBunruiId > 0)
                {
                    sb.AppendLine("AND SagyoChuBunrui.SagyoDaiBunruiId = " + para.SagyoDaiBunruiId.ToString() + " ");
                }
                if (para.SagyoChuBunruiCode.HasValue && para.SagyoChuBunruiCode > 0)
                {
                    sb.AppendLine("AND SagyoChuBunrui.SagyoChuBunruiCode = " + para.SagyoChuBunruiCode.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND SagyoChuBunrui.DisableFlag = " + NSKUtil.BoolToInt(para.DisableFlag.Value).ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" SagyoChuBunrui.SagyoChuBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoChuBunruiInfo rt_info = new SagyoChuBunruiInfo
                {
                    SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                    SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                    SagyoChuBunruiCode = SQLServerUtil.dbInt(rdr["SagyoChuBunruiCode"]),
                    SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                    ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.SagyoChuBunruiId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 一括削除するSqlCommandを作成する
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private SqlCommand CreateDelCommands()
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" SagyoChuBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine(" DelFlag = " + NSKUtil.BoolToInt(true).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(false).ToString() + "");

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            return command;
        }

        /// <summary>
        /// 更新するSqlCommandを作成する
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private SqlCommand CreateUpCommands(SagyoChuBunruiInfo info, bool exclusiveflg = true)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" SagyoChuBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine(" SagyoChuBunruiId =  " + info.SagyoChuBunruiId.ToString() + " ");
            sb.AppendLine(" ,SagyoDaiBunruiId =  " + info.SagyoDaiBunruiId.ToString() + " ");
            sb.AppendLine(" ,SagyoChuBunruiCode =  " + info.SagyoChuBunruiCode + " ");
            sb.AppendLine(" ,SagyoChuBunruiName = N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoChuBunruiName.Trim()) + "' ");
            sb.AppendLine(" ,ShelterId = " + info.ShelterId.ToString() + "");
            sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("SagyoChuBunruiId = " + info.SagyoChuBunruiId.ToString() + " ");
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
        /// 追加するSqlCommandを作成する
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private SqlCommand CreateInCommands(SagyoChuBunruiInfo info)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" SagyoChuBunrui ");
            sb.AppendLine(" ( ");
            sb.AppendLine(" SagyoChuBunruiId ");
            sb.AppendLine(" ,SagyoDaiBunruiId ");
            sb.AppendLine(" ,SagyoChuBunruiCode  ");
            sb.AppendLine(" ,SagyoChuBunruiName  ");
            sb.AppendLine(" ,ShelterId  ");
            sb.AppendLine(" ,DisableFlag  ");
            sb.AppendLine(" ,DelFlag  ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine("" + info.SagyoChuBunruiId.ToString() + "");
            sb.AppendLine(" , " + info.SagyoDaiBunruiId.ToString() + "");
            sb.AppendLine(" , " + info.SagyoChuBunruiCode.ToString() + " ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.SagyoChuBunruiName.Trim()) + "' ");
            sb.AppendLine(" , " + info.ShelterId.ToString() + "");
            sb.AppendLine(" ," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(" ," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
            sb.AppendLine(") ");

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            return command;
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
                sb.AppendLine(" SagyoChuBunrui ");
                sb.AppendLine(" WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                sb.AppendLine(" AND SagyoChuBunruiCode = N'" + code + "' ");
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
                    sbs.AppendLine(" SagyoChuBunrui ");
                    sbs.AppendLine("WHERE ");
                    sbs.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sbs.AppendLine("AND SagyoChuBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(cd.Trim()) + "' ");

                    mySql = mySql + sbs.ToString();
                    i++;
                }

                sb.AppendLine(" (" + mySql + ") S ");
                sb.AppendLine(" Group By S.SagyoChuBunruiCode ");
            }

            sb.AppendLine(" HAVING Count(*) > 1 ");
            return sb.ToString();
        }

        #endregion
    }
}
