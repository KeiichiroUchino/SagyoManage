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
    public class Keiyaku
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
        public Keiyaku()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、作業場所テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Keiyaku(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、作業場所情報を指定して、
        /// 作業場所情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">社員情報</param>
        public void Save(SqlTransaction transaction, KeiyakuInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateUpKeiyakuCommands(info));
            }
            else
            {
                info.KeiyakuId = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Keiyaku);

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateInKeiyakuCommands(info));

            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.KeiyakuCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }
        }

        /// <summary>
        /// SqlTransaction情報、作業場所情報を指定して、
        /// 契約情報を一括の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">契約情報</param>
        public void BulkSave(SqlTransaction transaction, List<KeiyakuInfo> list)
        {

            List<SqlCommand> commands = new List<SqlCommand>();
            List<string> codeList = new List<string>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.Keiyaku, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

            foreach (var info in list)
            {
                //データベースに保存されているかどうか、または先にINSERTしたレコードと同じコードのデータが存在している
                if (info.IsPersisted)
                {
                    //UPDATE
                    commands.Add(this.CreateUpKeiyakuCommands(info, false));
                }
                else
                {
                    //--InsertのSQLを作る前に、次IDを取得しておく
                    meisaiId_iterator.MoveNext();
                    info.KeiyakuId = meisaiId_iterator.Current;

                    //INSERT
                    commands.Add(this.CreateInKeiyakuCommands(info));
                }
                codeList.Add(info.KeiyakuCode);

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
        /// <param name="info">社員情報</param>
        public void Delete(SqlTransaction transaction, KeiyakuInfo info)
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
            sb.AppendLine(" Keiyaku ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" KeiyakuId = " + info.KeiyakuId.ToString() + " ");
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
            IList<string> list = GetReferenceTables(transaction, info.KeiyakuId);

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
        public KeiyakuInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new KeiyakuSearchParameter()
            {
                KeiyakuCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で作業場所情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public KeiyakuInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new KeiyakuSearchParameter()
            {
                KeiyakuId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、作業場所情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>作業場所情報のリスト</returns>
        public IList<KeiyakuInfo> GetList(KeiyakuSearchParameter para = null)
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
        public IList<KeiyakuInfo> GetListInternal(SqlTransaction transaction, KeiyakuSearchParameter para)
        {
            // クエリ
            String mySql = this.GetQuerySelect(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                KeiyakuInfo rt_info = new KeiyakuInfo
                {
                    KeiyakuId = SQLServerUtil.dbDecimal(rdr["KeiyakuId"]),
                    SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                    KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                    KeiyakuName = rdr["KeiyakuName"].ToString(),
                    SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                    SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                    SagyoShoBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoShoBunruiId"]),
                    KeiyakuStartDate = SQLHelper.dbDate(rdr["KeiyakuStartDate"]),
                    KeiyakuEndDate = SQLHelper.dbDate(rdr["KeiyakuEndDate"]),

                    KeiyakuJyokyo = SQLServerUtil.dbInt(rdr["KeiyakuJyokyo"]),
                    SagyoNinzu = SQLServerUtil.dbInt(rdr["SagyoNinzu"]),
                    SagyobiKbn = SQLServerUtil.dbInt(rdr["SagyobiKbn"]),
                    Monday = SQLHelper.dbBit(rdr["Monday"]),
                    Tuesday = SQLHelper.dbBit(rdr["Tuesday"]),
                    Wednesday = SQLHelper.dbBit(rdr["Wednesday"]),
                    Thursday = SQLHelper.dbBit(rdr["Thursday"]),
                    Friday = SQLHelper.dbBit(rdr["Friday"]),
                    Saturday = SQLHelper.dbBit(rdr["Saturday"]),
                    Sunday = SQLHelper.dbBit(rdr["Sunday"]),
                    SagyoDay1 = SQLServerUtil.dbDecimal(rdr["SagyoDay1"]),
                    SagyoDay2 = SQLServerUtil.dbDecimal(rdr["SagyoDay2"]),
                    SagyoDay3 = SQLServerUtil.dbDecimal(rdr["SagyoDay3"]),
                    SagyoDay4 = SQLServerUtil.dbDecimal(rdr["SagyoDay4"]),
                    SagyoDay5 = SQLServerUtil.dbDecimal(rdr["SagyoDay5"]),
                    SagyoDate1 = SQLHelper.dbDate(rdr["SagyoDate1"]),
                    SagyoDate2 = SQLHelper.dbDate(rdr["SagyoDate2"]),
                    SagyoDate3 = SQLHelper.dbDate(rdr["SagyoDate3"]),
                    SagyoDate4 = SQLHelper.dbDate(rdr["SagyoDate4"]),
                    SagyoDate5 = SQLHelper.dbDate(rdr["SagyoDate5"]),
                    SagyoDate6 = SQLHelper.dbDate(rdr["SagyoDate6"]),
                    SagyoDate7 = SQLHelper.dbDate(rdr["SagyoDate7"]),
                    SagyoDate8 = SQLHelper.dbDate(rdr["SagyoDate8"]),
                    SagyoDate9 = SQLHelper.dbDate(rdr["SagyoDate9"]),
                    SagyoDate10 = SQLHelper.dbDate(rdr["SagyoDate10"]),

                    ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),
                    SagyoDaiBunruiName = rdr["SagyoDaiBunruiName"].ToString(),
                    SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                    SagyoShoBunruiName = rdr["SagyoShoBunruiName"].ToString(),
                };

                if (0 < rt_info.KeiyakuId)
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
        /// <param name="keiyakuCodeList">検索条件情報(Idリスト)</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="exclusiveflg">排他フラグ</param>
        /// <returns>情報リスト</returns>
        public List<KeiyakuInfo> BulkGetListInternal(List<string> list, SqlTransaction transaction = null, bool exclusiveflg = false)
        {
            String mySql = string.Empty;

            List<KeiyakuInfo> resultList = new List<KeiyakuInfo>();

            // 該当データがない場合は終了
            if (list.Count() == 0) return new List<KeiyakuInfo>();

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
                sb.AppendLine(" Keiyaku ");

                if (exclusiveflg)
                {
                    sb.AppendLine(" Keiyaku WITH (UPDLOCK,ROWLOCK) ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND ShelterId = " + SQLHelper.GetSanitaizingSqlString(id.Trim()) + " ");
                }
                else
                {
                    sb.AppendLine(" Keiyaku ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sb.AppendLine("AND KeiyakuCode = N'" + SQLHelper.GetSanitaizingSqlString(id.Trim()) + "' ");
                }

                mySql = mySql + sb.ToString();

                i++;

                if (i == BLUK_COUNT)
                {
                    resultList.AddRange(SQLHelper.SimpleRead(mySql, rdr =>
                        {
                            //返却用の値
                            KeiyakuInfo rt_info = new KeiyakuInfo
                            {
                                KeiyakuId = SQLServerUtil.dbDecimal(rdr["KeiyakuId"]),
                                SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                                KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                                KeiyakuName = rdr["KeiyakuName"].ToString(),
                                SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                                SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                                SagyoShoBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoShoBunruiId"]),
                                KeiyakuStartDate = SQLHelper.dbDate(rdr["KeiyakuStartDate"]),
                                KeiyakuEndDate = SQLHelper.dbDate(rdr["KeiyakuEndDate"]),

                                KeiyakuJyokyo = SQLServerUtil.dbInt(rdr["KeiyakuJyokyo"]),
                                SagyoNinzu = SQLServerUtil.dbInt(rdr["SagyoNinzu"]),
                                SagyobiKbn = SQLServerUtil.dbInt(rdr["SagyobiKbn"]),
                                Monday = SQLHelper.dbBit(rdr["Monday"]),
                                Tuesday = SQLHelper.dbBit(rdr["Tuesday"]),
                                Wednesday = SQLHelper.dbBit(rdr["Wednesday"]),
                                Thursday = SQLHelper.dbBit(rdr["Thursday"]),
                                Friday = SQLHelper.dbBit(rdr["Friday"]),
                                Saturday = SQLHelper.dbBit(rdr["Saturday"]),
                                Sunday = SQLHelper.dbBit(rdr["Sunday"]),
                                SagyoDay1 = SQLServerUtil.dbDecimal(rdr["SagyoDay1"]),
                                SagyoDay2 = SQLServerUtil.dbDecimal(rdr["SagyoDay2"]),
                                SagyoDay3 = SQLServerUtil.dbDecimal(rdr["SagyoDay3"]),
                                SagyoDay4 = SQLServerUtil.dbDecimal(rdr["SagyoDay4"]),
                                SagyoDay5 = SQLServerUtil.dbDecimal(rdr["SagyoDay5"]),
                                SagyoDate1 = SQLHelper.dbDate(rdr["SagyoDate1"]),
                                SagyoDate2 = SQLHelper.dbDate(rdr["SagyoDate2"]),
                                SagyoDate3 = SQLHelper.dbDate(rdr["SagyoDate3"]),
                                SagyoDate4 = SQLHelper.dbDate(rdr["SagyoDate4"]),
                                SagyoDate5 = SQLHelper.dbDate(rdr["SagyoDate5"]),
                                SagyoDate6 = SQLHelper.dbDate(rdr["SagyoDate6"]),
                                SagyoDate7 = SQLHelper.dbDate(rdr["SagyoDate7"]),
                                SagyoDate8 = SQLHelper.dbDate(rdr["SagyoDate8"]),
                                SagyoDate9 = SQLHelper.dbDate(rdr["SagyoDate9"]),
                                SagyoDate10 = SQLHelper.dbDate(rdr["SagyoDate10"]),

                                ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                                DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                                DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                            };

                            if (0 < rt_info.KeiyakuId)
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
                    KeiyakuInfo rt_info = new KeiyakuInfo
                    {
                        KeiyakuId = SQLServerUtil.dbDecimal(rdr["KeiyakuId"]),
                        SagyoBashoId = SQLServerUtil.dbDecimal(rdr["SagyoBashoId"]),
                        KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                        KeiyakuName = rdr["KeiyakuName"].ToString(),
                        SagyoDaiBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoDaiBunruiId"]),
                        SagyoChuBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoChuBunruiId"]),
                        SagyoShoBunruiId = SQLServerUtil.dbDecimal(rdr["SagyoShoBunruiId"]),
                        KeiyakuStartDate = SQLHelper.dbDate(rdr["KeiyakuStartDate"]),
                        KeiyakuEndDate = SQLHelper.dbDate(rdr["KeiyakuEndDate"]),

                        KeiyakuJyokyo = SQLServerUtil.dbInt(rdr["KeiyakuJyokyo"]),
                        SagyoNinzu = SQLServerUtil.dbInt(rdr["SagyoNinzu"]),
                        SagyobiKbn = SQLServerUtil.dbInt(rdr["SagyobiKbn"]),
                        Monday = SQLHelper.dbBit(rdr["Monday"]),
                        Tuesday = SQLHelper.dbBit(rdr["Tuesday"]),
                        Wednesday = SQLHelper.dbBit(rdr["Wednesday"]),
                        Thursday = SQLHelper.dbBit(rdr["Thursday"]),
                        Friday = SQLHelper.dbBit(rdr["Friday"]),
                        Saturday = SQLHelper.dbBit(rdr["Saturday"]),
                        Sunday = SQLHelper.dbBit(rdr["Sunday"]),
                        SagyoDay1 = SQLServerUtil.dbDecimal(rdr["SagyoDay1"]),
                        SagyoDay2 = SQLServerUtil.dbDecimal(rdr["SagyoDay2"]),
                        SagyoDay3 = SQLServerUtil.dbDecimal(rdr["SagyoDay3"]),
                        SagyoDay4 = SQLServerUtil.dbDecimal(rdr["SagyoDay4"]),
                        SagyoDay5 = SQLServerUtil.dbDecimal(rdr["SagyoDay5"]),
                        SagyoDate1 = SQLHelper.dbDate(rdr["SagyoDate1"]),
                        SagyoDate2 = SQLHelper.dbDate(rdr["SagyoDate2"]),
                        SagyoDate3 = SQLHelper.dbDate(rdr["SagyoDate3"]),
                        SagyoDate4 = SQLHelper.dbDate(rdr["SagyoDate4"]),
                        SagyoDate5 = SQLHelper.dbDate(rdr["SagyoDate5"]),
                        SagyoDate6 = SQLHelper.dbDate(rdr["SagyoDate6"]),
                        SagyoDate7 = SQLHelper.dbDate(rdr["SagyoDate7"]),
                        SagyoDate8 = SQLHelper.dbDate(rdr["SagyoDate8"]),
                        SagyoDate9 = SQLHelper.dbDate(rdr["SagyoDate9"]),
                        SagyoDate10 = SQLHelper.dbDate(rdr["SagyoDate10"]),

                        ShelterId = SQLServerUtil.dbDecimal(rdr["ShelterId"]),
                        DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                        DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                    };

                    if (0 < rt_info.KeiyakuId)
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
            if (SQLHelper.RecordExists("SELECT * FROM SagyoAnken WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND KeiyakuId = " + id.ToString() + "", transaction))
            {
                list.Add("作業案件");
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
                sb.AppendLine(" Keiyaku ");
                sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                sb.AppendLine("AND KeiyakuCode = N'" + code + "' ");
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
                    sbs.AppendLine(" Keiyaku ");
                    sbs.AppendLine("WHERE ");
                    sbs.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                    sbs.AppendLine("AND KeiyakuCode = N'" + SQLHelper.GetSanitaizingSqlString(cd.Trim()) + "' ");

                    mySql = mySql + sbs.ToString();
                    i++;
                }

                sb.AppendLine(" (" + mySql + ") K ");
                sb.AppendLine(" Group By K.KeiyakuCode ");
            }

            sb.AppendLine(" HAVING Count(*) > 1 ");
            return sb.ToString();
        }

        /// <summary>
        /// 契約を更新するSqlCommandを作成する
        /// </summary>
        /// <param name="info">契約</param>
        /// <returns></returns>
        private SqlCommand CreateUpKeiyakuCommands(KeiyakuInfo info, bool exclusiveflg = true)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" Keiyaku ");
            sb.AppendLine("SET ");
            sb.AppendLine(" KeiyakuId =  " + info.KeiyakuId.ToString() + " ");
            sb.AppendLine(" ,SagyoBashoId =  " + info.SagyoBashoId.ToString() + " "); 
            sb.AppendLine(" ,KeiyakuCode =  N'" + SQLHelper.GetSanitaizingSqlString(info.KeiyakuCode.Trim()) + "' ");
            sb.AppendLine(" ,KeiyakuName =  N'" + SQLHelper.GetSanitaizingSqlString(info.KeiyakuName.Trim()) + "' ");
            sb.AppendLine(" ,SagyoDaiBunruiId =  " + info.SagyoDaiBunruiId.ToString() + " ");
            sb.AppendLine(" ,SagyoChuBunruiId =  " + info.SagyoChuBunruiId.ToString() + " ");
            sb.AppendLine(" ,SagyoShoBunruiId =  " + info.SagyoShoBunruiId.ToString() + " ");
            sb.AppendLine(" ,KeiyakuStartDate =  " + SQLHelper.DateTimeToDbDateTime(info.KeiyakuStartDate) + " ");
            sb.AppendLine(" ,KeiyakuEndDate =  " + SQLHelper.DateTimeToDbDateTime(info.KeiyakuEndDate) + " ");
            sb.AppendLine(" ,KeiyakuJyokyo =  " + info.KeiyakuJyokyo.ToString() + " ");
            sb.AppendLine(" ,SagyoNinzu =  " + info.SagyoNinzu.ToString() + " ");
            sb.AppendLine(" ,SagyobiKbn =  " + info.SagyobiKbn.ToString() + " ");
            sb.AppendLine(" ,Monday = " + NSKUtil.BoolToInt(info.Monday).ToString() + "");
            sb.AppendLine(" ,Tuesday = " + NSKUtil.BoolToInt(info.Tuesday).ToString() + "");
            sb.AppendLine(" ,Wednesday = " + NSKUtil.BoolToInt(info.Wednesday).ToString() + "");
            sb.AppendLine(" ,Thursday = " + NSKUtil.BoolToInt(info.Thursday).ToString() + "");
            sb.AppendLine(" ,Friday = " + NSKUtil.BoolToInt(info.Friday).ToString() + "");
            sb.AppendLine(" ,Saturday = " + NSKUtil.BoolToInt(info.Saturday).ToString() + "");
            sb.AppendLine(" ,Sunday = " + NSKUtil.BoolToInt(info.Sunday).ToString() + "");
            sb.AppendLine(" ,SagyoDay1 =  " + info.SagyoDay1.ToString() + " ");
            sb.AppendLine(" ,SagyoDay2 =  " + info.SagyoDay2.ToString() + " ");
            sb.AppendLine(" ,SagyoDay3 =  " + info.SagyoDay3.ToString() + " ");
            sb.AppendLine(" ,SagyoDay4 =  " + info.SagyoDay4.ToString() + " ");
            sb.AppendLine(" ,SagyoDay5 =  " + info.SagyoDay5.ToString() + " ");
            sb.AppendLine(" ,SagyoDate1 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate1) + " ");
            sb.AppendLine(" ,SagyoDate2 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate2) + " ");
            sb.AppendLine(" ,SagyoDate3 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate3) + " ");
            sb.AppendLine(" ,SagyoDate4 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate4) + " ");
            sb.AppendLine(" ,SagyoDate5 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate5) + " ");
            sb.AppendLine(" ,SagyoDate6 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate6) + " ");
            sb.AppendLine(" ,SagyoDate7 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate7) + " ");
            sb.AppendLine(" ,SagyoDate8 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate8) + " ");
            sb.AppendLine(" ,SagyoDate9 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate9) + " ");
            sb.AppendLine(" ,SagyoDate10 =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate10) + " ");
            sb.AppendLine(" ,ShelterId = " + info.ShelterId.ToString() + "");
            sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
            sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("KeiyakuId = " + info.KeiyakuId.ToString() + " ");

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
        /// 契約を追加するSqlCommandを作成する
        /// </summary>
        /// <param name="info">契約</param>
        /// <returns></returns>
        private SqlCommand CreateInKeiyakuCommands(KeiyakuInfo info)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" Keiyaku ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  KeiyakuId ");
            sb.AppendLine(" ,KeiyakuCode  ");
            sb.AppendLine(" ,KeiyakuName ");
            sb.AppendLine(" ,SagyoBashoId  ");
            sb.AppendLine(" ,SagyoDaiBunruiId  ");
            sb.AppendLine(" ,SagyoChuBunruiId  ");
            sb.AppendLine(" ,SagyoShoBunruiId  ");
            sb.AppendLine(" ,KeiyakuStartDate ");
            sb.AppendLine(" ,KeiyakuEndDate ");
            sb.AppendLine(" ,KeiyakuJyokyo ");
            sb.AppendLine(" ,SagyoNinzu ");
            sb.AppendLine(" ,SagyobiKbn ");
            sb.AppendLine(" ,Monday ");
            sb.AppendLine(" ,Tuesday ");
            sb.AppendLine(" ,Wednesday ");
            sb.AppendLine(" ,Thursday ");
            sb.AppendLine(" ,Friday ");
            sb.AppendLine(" ,Saturday ");
            sb.AppendLine(" ,Sunday ");
            sb.AppendLine(" ,SagyoDay1 ");
            sb.AppendLine(" ,SagyoDay2 ");
            sb.AppendLine(" ,SagyoDay3 ");
            sb.AppendLine(" ,SagyoDay4 ");
            sb.AppendLine(" ,SagyoDay5 ");
            sb.AppendLine(" ,SagyoDate1 ");
            sb.AppendLine(" ,SagyoDate2 ");
            sb.AppendLine(" ,SagyoDate3 ");
            sb.AppendLine(" ,SagyoDate4 ");
            sb.AppendLine(" ,SagyoDate5 ");
            sb.AppendLine(" ,SagyoDate6 ");
            sb.AppendLine(" ,SagyoDate7 ");
            sb.AppendLine(" ,SagyoDate8 ");
            sb.AppendLine(" ,SagyoDate9 ");
            sb.AppendLine(" ,SagyoDate10 ");
            sb.AppendLine(" ,ShelterId ");
            sb.AppendLine(" ,DisableFlag  ");
            sb.AppendLine(" ,DelFlag  ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine("" + info.KeiyakuId.ToString() + "");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.KeiyakuCode.Trim()) + "' ");
            sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.KeiyakuName.Trim()) + "' ");
            sb.AppendLine(" , " + info.SagyoBashoId.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoDaiBunruiId.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoChuBunruiId.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoShoBunruiId.ToString() + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.KeiyakuStartDate) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.KeiyakuEndDate) + " ");
            sb.AppendLine(" , " + info.KeiyakuJyokyo.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoNinzu.ToString() + " ");
            sb.AppendLine(" , " + info.SagyobiKbn.ToString() + " ");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Monday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Tuesday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Wednesday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Thursday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Friday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Saturday).ToString() + "");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.Sunday).ToString() + "");
            sb.AppendLine(" , " + info.SagyoDay1.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoDay2.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoDay3.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoDay4.ToString() + " ");
            sb.AppendLine(" , " + info.SagyoDay5.ToString() + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate1) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate2) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate3) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate4) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate5) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate6) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate7) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate8) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate9) + " ");
            sb.AppendLine(" , " + SQLHelper.DateTimeToDbDateTime(info.SagyoDate10) + " ");
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
        /// 契約を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">得意先</param>
        /// <returns></returns>
        private string GetQuerySelect(KeiyakuSearchParameter para, bool orderByFlg = true)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Keiyaku.* ");
            sb.AppendLine("	 ,SagyoBasho.SagyoBashoCode ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiCode ");
            sb.AppendLine("	 ,SagyoBasho.SagyoBashoName ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiName ");
            sb.AppendLine("	 ,SagyoDaiBunrui.SagyoDaiBunruiName ");
            sb.AppendLine("	 ,SagyoChuBunrui.SagyoChuBunruiName ");
            sb.AppendLine("	 ,SagyoShoBunrui.SagyoShoBunruiName ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" Keiyaku ");
            sb.AppendLine(" LEFT JOIN SagyoBasho ");
            sb.AppendLine(" ON Keiyaku.SagyoBashoId = SagyoBasho.SagyoBashoId ");
            sb.AppendLine("     AND SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Tokuisaki ");
            sb.AppendLine(" ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" LEFT JOIN SagyoDaiBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoDaiBunruiId = SagyoDaiBunrui.SagyoDaiBunruiId ");
            sb.AppendLine("     AND SagyoDaiBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoChuBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoChuBunruiId = SagyoChuBunrui.SagyoChuBunruiId ");
            sb.AppendLine("     AND SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoShoBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoShoBunruiId = SagyoShoBunrui.SagyoShoBunruiId ");
            sb.AppendLine("     AND SagyoShoBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Keiyaku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.KeiyakuId.HasValue && para.KeiyakuId > 0)
                {
                    sb.AppendLine("AND Keiyaku.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                }
                if (para.SagyoBashoId.HasValue && para.SagyoBashoId > 0)
                {
                    sb.AppendLine("AND Keiyaku.SagyoBashoId = " + para.SagyoBashoId.ToString() + " ");
                }
                if (para.TokuisakiId.HasValue && para.TokuisakiId > 0)
                {
                    sb.AppendLine("AND Tokuisaki.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (!string.IsNullOrWhiteSpace(para.KeiyakuCode))
                {
                    sb.AppendLine("AND Keiyaku.KeiyakuCode = N'" + SQLHelper.GetSanitaizingSqlString(para.KeiyakuCode.Trim()) + "' ");
                }
                if (!string.IsNullOrWhiteSpace(para.KeiyakuCodeAmbiguous))
                {
                    sb.AppendLine("AND Keiyaku.KeiyakuCode LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.KeiyakuCodeAmbiguous.Trim()) + "%') ");
                }
                if (!string.IsNullOrWhiteSpace(para.KeiyakuName))
                {
                    sb.AppendLine("AND Keiyaku.KeiyakuName LIKE( N'%" + SQLHelper.GetSanitaizingSqlString(para.KeiyakuName.Trim()) + "%') ");
                }
                if (para.SagyoDaiBunruiId.HasValue && para.SagyoDaiBunruiId > 0)
                {
                    sb.AppendLine("AND Keiyaku.SagyoDaiBunruiId = " + para.SagyoDaiBunruiId.ToString() + " ");
                }
                if (para.SagyoChuBunruiId.HasValue && para.SagyoChuBunruiId > 0)
                {
                    sb.AppendLine("AND Keiyaku.SagyoChuBunruiId = " + para.SagyoChuBunruiId.ToString() + " ");
                }
                if (para.SagyoShoBunruiId.HasValue && para.SagyoShoBunruiId > 0)
                {
                    sb.AppendLine("AND Keiyaku.SagyoShoBunruiId = " + para.SagyoShoBunruiId.ToString() + " ");
                }
                if (para.ShelterId.HasValue && para.ShelterId > 0 && !para.DuplicateFlg)
                {
                    sb.AppendLine("AND Keiyaku.ShelterId = " + para.ShelterId.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND Keiyaku.DisableFlag = " + NSKUtil.BoolToInt(para.DisableFlag.Value).ToString() + " ");
                }
                // 日付指定区分によって対象日付の比較項目を判断
                if ((int)BizProperty.DefaultProperty.FilterDateKbns.StartDate == para.FilterDateKbns)
                {
                    // 作業開始日付
                    sb.AppendLine(" AND ( Keiyaku.KeiyakuStartDate BETWEEN " + "'" + para.StartYMD.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'"
                        + " AND " + SQLHelper.DateTimeToDbDateTime(para.EndYMD) + " ");
                    sb.AppendLine(" OR Keiyaku.KeiyakuStartDate IS NULL )");
                }
                else if ((int)BizProperty.DefaultProperty.FilterDateKbns.EndDate == para.FilterDateKbns)
                {
                    // 作業終了日付
                    sb.AppendLine(" AND Keiyaku.KeiyakuEndDate BETWEEN " + "'" + para.StartYMD.ToString("yyyy/MM/dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture) + "'"
                        + " AND " + SQLHelper.DateTimeToDbDateTime(para.EndYMD) + " ");
                }
                if (para.DuplicateFlg)
                {
                    sb.AppendLine("AND Keiyaku.ShelterId <> " + para.ShelterId.ToString() + " ");
                }
            }

            if (orderByFlg)
            {
                sb.AppendLine("ORDER BY ");
                sb.AppendLine(" Keiyaku.KeiyakuCode ");
            }

            return sb.ToString();
        }

        #endregion
    }
}
