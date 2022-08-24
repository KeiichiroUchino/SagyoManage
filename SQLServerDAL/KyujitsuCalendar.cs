using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 休刊カレンダテーブルのデータアクセスレイヤです。
    /// </summary>
    public class KyujitsuCalendar
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
        private string _tableName = "休刊カレンダ";

        /// <summary>
        /// 休刊カレンダクラスのデフォルトコンストラクタです。
        /// </summary>
        public KyujitsuCalendar()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、休刊カレンダテーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public KyujitsuCalendar(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で休刊カレンダ情報を取得します。
        /// </summary>
        /// <param name="id">休刊カレンダId</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>休刊カレンダ情報</returns>
        public KyujitsuCalendarInfo GetInfo(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new KyujitsuCalendarSearchParameter()
            {
                KyujitsuCalendarId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 年度を指定して、情報を取得します。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <returns>休刊カレンダ情報</returns>
        public KyujitsuCalendarInfo GetInfoByNendoAndStaffId(int nendo, decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new KyujitsuCalendarSearchParameter()
            {
                Nendo = nendo,
                ToraDONStaffId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、休刊カレンダ情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>休刊カレンダ情報のリスト</returns>
        public IList<KyujitsuCalendarInfo> GetList(KyujitsuCalendarSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、休刊カレンダ情報を指定して、
        /// 休刊カレンダ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">休刊カレンダ情報</param>
        public void Save(SqlTransaction transaction, KyujitsuCalendarInfo info)
        {
            decimal KyujitsuCalendarId = 0;

            //常設列の取得オプションを作る
            //--新規登録
            SQLHelper.PopulateColumnOptions popInsOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //常設列の取得オプションを作る
                //--更新は入力と更新
                SQLHelper.PopulateColumnOptions popUpdOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.UpdateColumns;

                //Update文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("UPDATE  ");
                sb.AppendLine(" KyujitsuCalendar ");
                sb.AppendLine("SET ");
                sb.AppendLine(" KyujitsuCalendarId = " + info.KyujitsuCalendarId.ToString() + " ");
                sb.AppendLine(",Nendo = " + info.Nendo.ToString() + " ");
                sb.AppendLine(",ToraDONStaffId = " + info.ToraDONStaffId.ToString() + " ");

                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popUpdOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine(" KyujitsuCalendarId = " + info.KyujitsuCalendarId.ToString() + " ");
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

                KyujitsuCalendarId = info.KyujitsuCalendarId;
            }
            else
            {
                //InsertのSQLを作る前に、次IDを取得しておく
                decimal newId =
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.KyujitsuCalendarMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" KyujitsuCalendar ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  KyujitsuCalendarId ");
                sb.AppendLine(" ,Nendo ");
                sb.AppendLine(" ,ToraDONStaffId ");

                sb.AppendLine(" ,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popInsOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("  " + newId.ToString() + "");
                sb.AppendLine(" ," + info.Nendo.ToString() + " ");
                sb.AppendLine(" ," + info.ToraDONStaffId.ToString() + " ");

                sb.AppendLine(" ," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popInsOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //休刊カレンダの存在チェック
                if (SQLHelper.RecordExists(CreateCheckSQL(info.Nendo.ToString(), info.ToraDONStaffId.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ年度の{0}情報が既に登録されています。"
                                , this._tableName));
                }

                KyujitsuCalendarId = newId;
            }

            List<string> mySqlList = new List<string>();

            //休刊カレンダ明細削除SQL取得
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE FROM ");
            sbDel.AppendLine(" KyujitsuCalendarMeisai ");
            sbDel.AppendLine("WHERE ");
            sbDel.AppendLine(" KyujitsuCalendarId = " + KyujitsuCalendarId.ToString() + " ");

            mySqlList.Add(sbDel.ToString());

            //休刊カレンダ明細が存在する場合
            if (info.KyujitsuCalendarMeisaiList != null && 0 < info.KyujitsuCalendarMeisaiList.Count)
            {
                //休刊カレンダ明細登録SQL取得
                foreach (KyujitsuCalendarMeisaiInfo meisai in info.KyujitsuCalendarMeisaiList)
                {
                    //Insert文を作成
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("INSERT INTO ");
                    sb.AppendLine(" KyujitsuCalendarMeisai ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine("  KyujitsuCalendarId ");
                    sb.AppendLine(" ,HizukeYMD ");
                    sb.AppendLine(" ,KyujitsuKbn ");
                    sb.AppendLine(" ,Memo ");
                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popInsOption));

                    sb.AppendLine(" ) ");
                    sb.AppendLine("VALUES ");
                    sb.AppendLine("( ");
                    sb.AppendLine("  " + KyujitsuCalendarId.ToString() + " ");
                    sb.AppendLine(", " + SQLHelper.DateTimeToDbDate(meisai.HizukeYMD).ToString() + " ");
                    sb.AppendLine(", " + meisai.KyujitsuKbn.ToString() + " ");
                    sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(meisai.Memo.Trim()) + "'");

                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popInsOption));

                    sb.AppendLine(") ");

                    mySqlList.Add(sb.ToString());
                }
            }

            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(mySqlList);

            // 指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
        }

        /// <summary>
        ///  SqlTransaction情報、休刊カレンダ情報を指定して、
        ///  休刊カレンダ情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">休刊カレンダ情報</param>
        public void Delete(SqlTransaction transaction, KyujitsuCalendarInfo info)
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
            sb.AppendLine(" KyujitsuCalendar ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" KyujitsuCalendarId = " + info.KyujitsuCalendarId.ToString() + " ");
            sb.AppendLine(" --排他のチェック ");
            sb.AppendLine(" AND	VersionColumn = @versionColumn ");
            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。
            SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command);

            //明細データは削除しない。
            //（SE対応復旧の可能性を残すため）

            #endregion

            #region 他テーブルの存在チェック

            //なし

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
        public IList<KyujitsuCalendarInfo> GetListInternal(SqlTransaction transaction, KyujitsuCalendarSearchParameter para)
        {
            //返却用のリスト
            List<KyujitsuCalendarInfo> rt_list = new List<KyujitsuCalendarInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 KyujitsuCalendar.* ");
            sb.AppendLine("	,ToraDONStaff.StaffCd ");
            sb.AppendLine("	,ToraDONStaff.StaffNM ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	KyujitsuCalendar ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Staff ToraDONStaff ");
            sb.AppendLine("	ON  ToraDONStaff.StaffId = KyujitsuCalendar.ToraDONStaffId ");
            sb.AppendLine("	AND ToraDONStaff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	KyujitsuCalendar.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.KyujitsuCalendarId.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.KyujitsuCalendarId = " + para.KyujitsuCalendarId.ToString() + " ");
                }
                if (para.Nendo.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.Nendo = " + para.Nendo.ToString() + " ");
                }
                if (para.ToraDONStaffId.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.ToraDONStaffId = " + para.ToraDONStaffId.ToString() + " ");
                }
            }

            String mySql = sb.ToString();

            //休刊カレンダ明細クラス生成
            KyujitsuCalendarMeisai _KyujitsuCalendarMeisai = new KyujitsuCalendarMeisai();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                KyujitsuCalendarInfo rt_info = new KyujitsuCalendarInfo
                {
                    KyujitsuCalendarId = SQLServerUtil.dbDecimal(rdr["KyujitsuCalendarId"]),
                    Nendo = SQLServerUtil.dbInt(rdr["Nendo"]),
                    ToraDONStaffId = SQLServerUtil.dbDecimal(rdr["ToraDONStaffId"]),

                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    ToraDONStaffCode = SQLServerUtil.dbInt(rdr["StaffCd"]),
                    ToraDONStaffName = rdr["StaffNM"].ToString()
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //休刊カレンダ明細情報リストを取得
                rt_info.KyujitsuCalendarMeisaiList = _KyujitsuCalendarMeisai.GetList(
                    new KyujitsuCalendarMeisaiSearchParameter()
                    {
                        KyujitsuCalendarId = rt_info.KyujitsuCalendarId,
                    });

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したデータの存在チェック用のSQLを作成します。
        /// </summary>
        /// <param name="nendo">年度</param>
        /// <param name="id">トラDON社員ID</param>
        /// <returns>チェック結果</returns>
        private string CreateCheckSQL(string nendo, string id)
        {
            return "SELECT 1 FROM KyujitsuCalendar WHERE DelFlag = 0 "
                + " AND Nendo = " + nendo + " AND ToraDONStaffId = " + id + " HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
