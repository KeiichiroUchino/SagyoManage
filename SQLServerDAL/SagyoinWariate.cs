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
    /// 作業員割当テーブルのデータアクセスレイヤです。
    /// </summary>
    public class SagyoinWariate
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
        /// 検索区分を表します。
        /// </summary>
        public enum SearchKbnEnum
        {
            Basic,
            Check,
            LastTime,
        }

        /// <summary>
        /// 作業員割当クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoinWariate()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、作業員割当テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SagyoinWariate(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で作業員割当情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoinWariateInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoinWariateSearchParameter()
            {
                SearchKbn = (int)SearchKbnEnum.Basic,
                SagyoinWariateId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、作業員割当情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>作業員割当情報のリスト</returns>
        public IList<SagyoinWariateInfo> GetList(SagyoinWariateSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、作業員割当情報を指定して、
        /// 作業員割当情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="sagyoAnkenId">作業案件ID</param>
        /// <param name="info">作業員割当情報</param>
        public void Save(SqlTransaction transaction, decimal sagyoAnkenId, List<SagyoinWariateInfo> list)
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.SagyoinWariate, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

            foreach (var info in list)
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
                    sb.AppendLine(" SagyoinWariate ");
                    sb.AppendLine("SET ");
                    sb.AppendLine(" SagyoinWariateId =  " + info.SagyoinWariateId.ToString() + " ");
                    sb.AppendLine(" ,SagyoAnkenId =  " + info.SagyoAnkenId.ToString() + " ");
                    sb.AppendLine(" ,StaffId =  " + info.StaffId.ToString() + " ");
                    sb.AppendLine(" ,ShuJitsuFlg = " + NSKUtil.BoolToInt(info.ShuJitsuFlg).ToString() + "");
                    sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                    sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                    sb.AppendLine("WHERE ");
                    sb.AppendLine("SagyoinWariateId = " + info.SagyoinWariateId.ToString() + " ");
                    //sb.AppendLine("--排他のチェック ");
                    //sb.AppendLine("AND ");
                    //sb.AppendLine("VersionColumn = @versionColumn ");

                    string sql = sb.ToString();
                    commands.Add(new SqlCommand(sql));

                    //SqlCommand command = new SqlCommand(sql);
                    //command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                    ////指定したトランザクション上でExecuteNonqueryを実行し
                    ////影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                    ////影響を受けた行数が1以上場合は、致命的なエラーの例外。
                    //SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
                }
                else
                {

                    //常設列の取得オプションを作る
                    //--新規登録
                    SQLHelper.PopulateColumnOptions popOption =
                        SQLHelper.PopulateColumnOptions.EntryColumns |
                            SQLHelper.PopulateColumnOptions.AdditionColumns;

                    //--InsertのSQLを作る前に、次IDを取得しておく
                    meisaiId_iterator.MoveNext();
                    decimal newId = meisaiId_iterator.Current;

                    //Insert文を作成
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("INSERT INTO ");
                    sb.AppendLine(" SagyoinWariate ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" SagyoinWariateId ");
                    sb.AppendLine(" ,SagyoAnkenId ");
                    sb.AppendLine(" ,StaffId ");
                    sb.AppendLine(" ,ShuJitsuFlg ");
                    sb.AppendLine("	,DelFlag ");
                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                    sb.AppendLine(" ) ");
                    sb.AppendLine("VALUES ");
                    sb.AppendLine("( ");
                    sb.AppendLine("" + newId.ToString() + "");
                    sb.AppendLine(" ," + sagyoAnkenId.ToString() + " ");
                    sb.AppendLine(" ," + info.StaffId.ToString() + " ");
                    sb.AppendLine(" ," + NSKUtil.BoolToInt(info.ShuJitsuFlg).ToString() + "");
                    sb.AppendLine(" ," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                    sb.AppendLine(") ");

                    string sql = sb.ToString();
                    commands.Add(new SqlCommand(sql));

                }
            }

            //パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(commands.Select(x => x.CommandText));

            if (query != string.Empty)
            {
                SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
            }

        }

        /// <summary>
        ///  SqlTransaction情報、作業員割当情報を指定して、
        ///  作業員割当情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業員割当情報</param>
        public void Delete(SqlTransaction transaction, SagyoinWariateInfo info, decimal id = 0)
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
            sb.AppendLine(" SagyoinWariate ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");

            if (id > 0)
            {
                sb.AppendLine(" SagyoAnkenId = " + id.ToString() + " ");
            }
            else
            {
                sb.AppendLine(" SagyoinWariateId = " + info.SagyoinWariateId.ToString() + " ");
            }

            if (id < 1)
            {
                sb.AppendLine(" --排他のチェック ");
                sb.AppendLine(" AND	VersionColumn = @versionColumn ");
            }

            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);

            if (id < 1)
            {
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));
            }

            if (id > 0)
            {
                SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);
            }
            else
            {
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command);
            }

            #endregion

        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<SagyoinWariateInfo> GetListInternal(SqlTransaction transaction, SagyoinWariateSearchParameter para)
        {
            //返却用のリスト
            List<SagyoinWariateInfo> rt_list = new List<SagyoinWariateInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	SagyoinWariate.* ");
            sb.AppendLine("	,Staff.StaffCode ");
            sb.AppendLine("	,Staff.StaffName ");
            sb.AppendLine("	,StaffKbn.MeishoCode AS StaffKbnCode ");
            sb.AppendLine("	,StaffKbn.Meisho AS StaffKbnName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" SagyoinWariate ");
            sb.AppendLine(" LEFT JOIN Staff");
            sb.AppendLine(" ON SagyoinWariate.StaffId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Meisho AS StaffKbn ");
            sb.AppendLine(" ON Staff.StaffKbnId = StaffKbn.MeishoId ");
            sb.AppendLine("     AND StaffKbn.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoAnken ");
            sb.AppendLine(" ON SagyoinWariate.SagyoAnkenId = SagyoAnken.SagyoAnkenId ");
            sb.AppendLine("     AND SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                //通常検索
                if (para.SearchKbn == (int)SearchKbnEnum.Basic)
                {
                    if (para.SagyoinWariateId.HasValue)
                    {
                        sb.AppendLine("AND SagyoinWariate.SagyoinWariateId = " + para.SagyoinWariateId.ToString() + " ");
                    }
                    if (para.SagyoAnkenId.HasValue)
                    {
                        sb.AppendLine("AND SagyoinWariate.SagyoAnkenId = " + para.SagyoAnkenId.ToString() + " ");
                    }
                }
                //重複チェック
                else if (para.SearchKbn == (int)SearchKbnEnum.Check)
                {
                    sb.AppendLine("   AND SagyoinWariate.SagyoAnkenId <> " + para.SagyoAnkenId.ToString() + " ");
                    sb.AppendLine("   AND SagyoinWariate.StaffId IN( " +
                         SQLHelper.GetSQLQueryWhereInByStructList(para.StaffIdList) + ") ");
                    sb.AppendLine("   AND (  ");
                    sb.AppendLine("     (  ");
                    sb.AppendLine("       SagyoAnken.SagyoStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.SagyoStartDateTime));
                    sb.AppendLine("       AND SagyoAnken.SagyoStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.SagyoEndDateTime));
                    sb.AppendLine("     )  ");
                    sb.AppendLine("     OR (  ");
                    sb.AppendLine("       SagyoAnken.SagyoEndDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.SagyoStartDateTime));
                    sb.AppendLine("       AND SagyoAnken.SagyoEndDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.SagyoEndDateTime));
                    sb.AppendLine("     ) ");
                    sb.AppendLine("     OR (  ");
                    sb.AppendLine("       SagyoAnken.SagyoStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.SagyoStartDateTime));
                    sb.AppendLine("       AND SagyoAnken.SagyoEndDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.SagyoEndDateTime));
                    sb.AppendLine("     ) ");
                    sb.AppendLine("   ) ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" SagyoinWariateId ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoinWariateInfo rt_info = new SagyoinWariateInfo
                {
                    SagyoinWariateId = SQLServerUtil.dbDecimal(rdr["SagyoinWariateId"]),
                    SagyoAnkenId = SQLServerUtil.dbDecimal(rdr["SagyoAnkenId"]),
                    StaffId = SQLServerUtil.dbDecimal(rdr["StaffId"]),
                    StaffCode = SQLServerUtil.dbInt(rdr["StaffCode"]),
                    StaffName = rdr["StaffName"].ToString(),
                    StaffKbnCode = SQLServerUtil.dbInt(rdr["StaffKbnCode"]),
                    StaffKbnName = rdr["StaffKbnName"].ToString(),
                    ShuJitsuFlg = SQLHelper.dbBit(rdr["ShuJitsuFlg"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.SagyoAnkenId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
