using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 名称クラスのビジネスロジック
    /// </summary>
    public class Meisho
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
        private string _tableName = "名称";

        /// <summary>
        /// 本クラスのインスタンスを取得します。
        /// </summary>
        public Meisho()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Meisho(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、名称情報を指定して、
        /// 設定した名称情報を新規登録または更新する。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">名称情報</param>
        public decimal Save(SqlTransaction transaction, MeishoInfo info)
        {
            //データベースに保存されているかどうかを「rowversion」でチェックします
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
                sb.AppendLine(" Meisho ");
                sb.AppendLine("SET ");
                sb.AppendLine(" MeishoId = " + info.MeishoId.ToString() + "");
                sb.AppendLine(" ,MeishoKbn = " + info.MeishoKbn.ToString() + "");
                sb.AppendLine(" ,MeishoCode = " + info.MeishoCode.ToString() + "");
                sb.AppendLine(" ,Meisho = N'" + SQLHelper.GetSanitaizingSqlString(info.Meisho.Trim()) + "'");
                sb.AppendLine(" ,MeishoKana = N'" + SQLHelper.GetSanitaizingSqlString(info.MeishoKana.Trim()) + "'");
                sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(" ,DelFlag = '" + info.DelFlag.ToString() + "', ");
                sb.AppendLine("	" + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                sb.AppendLine("WHERE ");
                sb.AppendLine(" MeishoId = " + info.MeishoId.ToString() + "");
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
                if (info.MeishoId == 0)
                {
                    info.MeishoId = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Meisho);
                }

                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //Insert文を作成
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Meisho ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	MeishoId ");
                sb.AppendLine("	,MeishoKbn ");
                sb.AppendLine("	,MeishoCode ");
                sb.AppendLine("	,Meisho ");
                sb.AppendLine("	,MeishoKana ");
                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine(" " + info.MeishoId.ToString() + " ");
                sb.AppendLine("," + info.MeishoKbn.ToString() + " ");
                sb.AppendLine("," + info.MeishoCode.ToString() + "");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.Meisho.Trim()) + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.MeishoKana.Trim()) + "'");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",'" + info.DelFlag.ToString() + "' ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
                sb.AppendLine(") ");

                string sql = sb.ToString();

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql), this._tableName);
   
            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.MeishoKbn, info.MeishoCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }

            return info.MeishoId;
        }

        /// <summary>
        ///  SqlTransaction情報、名称情報を指定して、
        ///  指定した名称情報を1件削除する。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">名称情報</param>
        public void Delete(SqlTransaction transaction, MeishoInfo info)
        {
            #region レコードの削除

            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            #region メインテーブル削除(排他処理あり)

            //Delete文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" Meisho ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" Meisho.MeishoId = " + info.MeishoId.ToString() + " ");
            sb.AppendLine("--排他のチェック ");
            sb.AppendLine(" AND	VersionColumn = @versionColumn ");

            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。
            SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command, this._tableName);

            #endregion

            //他のテーブルに使用されていないか
            IList<string> list = GetReferenceTables(transaction, info.MeishoId);

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
        /// 名称区分、名称コードを指定して、該当する名称情報を1件取得する。
        /// </summary>
        /// <param name="meishoKbn">名称区分</param>
        /// <param name="meishoCode">名称コード</param>
        /// <returns>名称情報</returns>
        public MeishoInfo GetInfo(int meishoKbn, int meishoCode)
        {
            return
                this.GetListInternal(null, new MeishoSearchParameter 
                { 
                    MeishoKbn = meishoKbn, 
                    MeishoCode = meishoCode 
                }).FirstOrDefault();
        }

        /// <summary>
        /// 名称IDを指定して、該当する名称情報を1件取得する。
        /// </summary>
        /// <param name="meishoId">名称ID</param>
        /// <returns>名称情報</returns>
        public MeishoInfo GetInfoById(decimal meishoId)
        {
            return
                this.GetListInternal(null, new MeishoSearchParameter
                {
                    MeishoId = meishoId,
                }).FirstOrDefault();
        }

        /// <summary>
        /// 名称抽出情報を指定して、条件に該当する名称情報を一覧取得する。
        /// </summary>
        /// <param name="para">名称抽出情報</param>
        /// <returns>名称情報一覧</returns>
        public IList<MeishoInfo> GetList(MeishoSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 名称抽出情報を指定して、名称コードの空き番を取得する
        /// </summary>
        /// <returns> 名称コードの空き番</returns>
        public int GetBlankCode(MeishoSearchParameter para = null)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("WITH Nums AS( ");
            //--コードの番号の開始値は1から。それを取得する為の0をNumにセットする
            sb.AppendLine("              SELECT ");
            sb.AppendLine("                 0 AS Num ");
            sb.AppendLine("              UNION ");
            sb.AppendLine("              SELECT ");
            sb.AppendLine("                 Meisho.MeishoCode AS Num ");
            sb.AppendLine("              FROM ");
            sb.AppendLine("                 Meisho ");
            sb.AppendLine("              WHERE ");
            sb.AppendLine("                 Meisho.DelFlag = " +
                                                NSKUtil.BoolToInt(false).ToString() + " ");
            if (para != null)
            {
                if (para.MeishoKbn.HasValue)
                {
                    sb.AppendLine("              AND  Meisho.MeishoKbn = " +
                                                        para.MeishoKbn.ToString() + " ");
                }
            }
            sb.AppendLine("             ) ");
            sb.AppendLine("SELECT ");
            sb.AppendLine(" MIN(N1.Num + 1) AS MeishoCode ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Nums AS N1 ");
            sb.AppendLine("WHERE ");
            //--次番号が欠けている番号を取得する為の条件。
            //--例：番号{3,4,7,8}→番号{4,8}が抽出される。
            //--    この時に番号に0があれば、番号{0,3,4,7,8}→番号{0,4,8}となり、SELECTで1が抽出される。
            sb.AppendLine(" NOT EXISTS ");
            sb.AppendLine(" ( ");
            sb.AppendLine("   SELECT ");
            sb.AppendLine("     *");
            sb.AppendLine("   FROM");
            sb.AppendLine("     Nums AS N2 ");
            sb.AppendLine("   WHERE ");
            //--こっちの条件は結果には影響しない。パフォーマンス向上のため
            sb.AppendLine("     N2.Num > N1.Num ");
            //--次番号有り
            sb.AppendLine("     AND	N2.Num = N1.Num + 1 ");
            sb.AppendLine(" ) ");

            return SQLHelper.SimpleRead(
                sb.ToString(), rdr => SQLServerUtil.dbInt(rdr["MeishoCode"])).FirstOrDefault();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、抽出情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">抽出情報</param>
        /// <returns>情報リスト</returns>
        private IList<MeishoInfo> GetListInternal(SqlTransaction transaction, MeishoSearchParameter para = null)
        {
            //返却用のリスト
            List<MeishoInfo> rt_list = new List<MeishoInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT DISTINCT ");
            sb.AppendLine(" Meisho.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Meisho ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Meisho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.MeishoKbn.HasValue)
                {
                    sb.AppendLine(" AND Meisho.MeishoKbn = " +
                        para.MeishoKbn.ToString() + " ");
                }

                if (para.MeishoCode.HasValue)
                {
                    sb.AppendLine(" AND Meisho.MeishoCode = " +
                        para.MeishoCode.ToString() + " ");
                }

                if (para.MeishoId.HasValue)
                {
                    sb.AppendLine(" AND Meisho.MeishoId = " +
                        para.MeishoId.ToString() + " ");
                }

                if (!String.IsNullOrEmpty(para.Meisho))
                {
                    sb.AppendLine(" AND Meisho.Meisho = " +
                        " N'" + SQLHelper.GetSanitaizingSqlString(para.Meisho.Trim()) + "' ");
                }

                if (para.MeishoIdList != null && para.MeishoIdList.Count > 0)
                {
                    sb.AppendLine(" AND Meisho.MeishoId IN (" +
                        SQLHelper.GetSQLQueryWhereInByStructList(para.MeishoIdList) + ") ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Meisho.MeishoKbn ");
            sb.AppendLine(" ,Meisho.MeishoCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr => this.BuildInfoFromSqlDataReader(rdr), transaction);
        }

        /// <summary>
        /// SqlDataReaderを指定して、
        /// クラスのインスタンスを返却します。
        /// </summary>
        /// <param name="rdr">読み込み済みのSqlDataReader</param>
        /// <returns>クラスのインスタンス</returns>
        private MeishoInfo BuildInfoFromSqlDataReader(SqlDataReader rdr)
        {
            MeishoInfo rt_info = null;

            rt_info = new MeishoInfo
            {
                MeishoId = SQLServerUtil.dbDecimal(rdr["MeishoId"]),
                MeishoKbn = SQLServerUtil.dbInt(rdr["MeishoKbn"]),
                MeishoCode = SQLServerUtil.dbInt(rdr["MeishoCode"]),
                Meisho = rdr["Meisho"].ToString().Trim(),
                MeishoKana = rdr["MeishoKana"].ToString().Trim(),
                DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
            };

            //入力者以下の常設フィールドをセットする
            rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

            return rt_info;
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

            //社員	社員区分
            if (SQLHelper.RecordExists("SELECT * FROM Staff WHERE DelFlag = " + NSKUtil.BoolToInt(false) +
                " AND StaffKbnId = " + id.ToString() + "", transaction))
            {
                list.Add("社員(社員区分)");
            }

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 指定したコードチェック用のSQL文を作成します。
        /// </summary>
        /// <param name="code">コード</param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(int kbn,int code)
        {
            return "SELECT 1 FROM Meisho WHERE DelFlag = 0 "
                + "AND MeishoKbn = " + kbn.ToString()
                + "AND MeishoCode = " + code.ToString()
                + "  HAVING Count(*) > 1 ";

        }

        #endregion
    }
}
