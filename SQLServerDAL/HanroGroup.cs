using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 販路グループテーブルのデータアクセスレイヤです。
    /// </summary>
    public class HanroGroup
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
        private string _tableName = "販路グループ";

        /// <summary>
        /// 販路グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public HanroGroup()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HanroGroup(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// SqlTransaction情報、販路グループ情報を指定して、
        /// 販路グループ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路グループ情報</param>
        public void Save(SqlTransaction transaction, HanroGroupInfo info)
        {
            String id = string.Empty;

            //データベースに保存されているか
            if (info.IsPersisted)
            {
                // 更新
                //常設列の取得オプションを作る
                //--更新は入力と更新
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.UpdateColumns;

                //Update文を作成
                StringBuilder sb = new StringBuilder();

                sb.AppendLine("UPDATE  ");
                sb.AppendLine(" HanroGroup ");
                sb.AppendLine("SET ");
                sb.AppendLine(" HanroGroupName = N'" + SQLHelper.GetSanitaizingSqlString(info.HanroGroupName.Trim()) + "'");

                sb.AppendLine(" ,DisableFlag = '" + info.DisableFlag.ToString() + "' ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HanroGroupId = " + info.HanroGroupId.ToString() + " ");
                sb.AppendLine("--排他のチェック ");
                sb.AppendLine(" AND	VersionColumn = @versionColumn ");

                string sql = sb.ToString();

                SqlCommand command = new SqlCommand(sql);
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command, this._tableName);

                id = info.HanroGroupId.ToString();
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HanroGroupMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" HanroGroup ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  HanroGroupId ");
                sb.AppendLine(" ,HanroGroupCode ");
                sb.AppendLine(" ,HanroGroupName ");

                sb.AppendLine(" ,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.HanroGroupCode.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HanroGroupName.Trim()) + "'");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql), this._tableName);

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.HanroGroupCode.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }

                id = newId.ToString();
            }

            List<string> mySqlList = new List<string>();

            // -- 販路グループ明細削除SQL取得
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE FROM ");
            sbDel.AppendLine(" HanroGroupMeisai ");
            sbDel.AppendLine("WHERE ");
            sbDel.AppendLine(" HanroGroupId = " + id + " ");

            mySqlList.Add(sbDel.ToString());

            //販路グループ明細登録SQL取得
            foreach (HanroGroupMeisaiInfo meisai in info.HanroGroupMeisaiList)
            {
                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //Insert文を作成
                StringBuilder sbIns = new StringBuilder();
                sbIns.AppendLine("INSERT INTO ");
                sbIns.AppendLine(" HanroGroupMeisai ");
                sbIns.AppendLine(" ( ");
                sbIns.AppendLine("  HanroGroupId ");
                sbIns.AppendLine(" ,Gyo ");
                sbIns.AppendLine(" ,HanroId ");

                sbIns.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sbIns.AppendLine(" ) ");
                sbIns.AppendLine("VALUES ");
                sbIns.AppendLine("( ");
                sbIns.AppendLine(" " + id + " ");
                sbIns.AppendLine(" ," + meisai.Gyo.ToString() + " ");
                sbIns.AppendLine(" ," + meisai.HanroId.ToString() + " ");

                sbIns.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sbIns.AppendLine(") ");

                mySqlList.Add(sbIns.ToString());
            }

            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(mySqlList);

            // 指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
        }

        /// <summary>
        ///  SqlTransaction情報、販路グループ情報を指定して、
        ///  販路グループ情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路グループ情報</param>
        public void Delete(SqlTransaction transaction, HanroGroupInfo info)
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
            sb.AppendLine(" HanroGroup ");
            sb.AppendLine("SET ");

            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" HanroGroupId = " + info.HanroGroupId.ToString() + " ");
            sb.AppendLine(" --排他のチェック ");
            sb.AppendLine(" AND	VersionColumn = @versionColumn ");
            string sql = sb.ToString();

            SqlCommand command = new SqlCommand(sql);
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。
            SQLHelper.ExecuteNonQueryForSingleRecordDelete(transaction, command);

            //明細のDelete文を作成
            sb = new StringBuilder();
            sb.AppendLine("DELETE FROM ");
            sb.AppendLine(" HanroGroupMeisai ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HanroGroupId = " + info.HanroGroupId.ToString() + " ");

            sql = sb.ToString();
            command = new SqlCommand(sql);

            //明細削除
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);

            #endregion

            #region 他テーブルの存在チェック

            //なし

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、販路グループテーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路グループテーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, HanroGroupSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// コードを指定して、販路グループ情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>販路グループ情報</returns>
        public HanroGroupInfo GetInfo(int code)
        {
            HanroGroupInfo info = this.GetListInternal(null,
                new HanroGroupSearchParameter
                {
                    HanroGroupCode = code,
                }
                ).FirstOrDefault();

            return info;
        }

        /// <summary>
        /// Idを指定して、販路グループ情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>販路グループ情報</returns>
        public HanroGroupInfo GetInfoById(decimal id)
        {
            HanroGroupInfo info = this.GetListInternal(null,
                new HanroGroupSearchParameter
                {
                    HanroGroupId = id,
                }
                ).FirstOrDefault();

            return info;
        }

        /// <summary>
        /// 検索条件情報を指定して、販路グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路グループ情報一覧</returns>
        public IList<HanroGroupInfo> GetList(HanroGroupSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、販路グループ情報、検索条件情報を指定して、
        /// 販路グループ情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路グループ情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, HanroGroupInfo info, int newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new HanroGroup().GetKenSuu(transaction, new HanroGroupSearchParameter() { HanroGroupCode = newCode }) != 0)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName),
                    MessageBoxIcon.Warning);
            }

            #region コードの変更

            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Delete文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" HanroGroup ");
            sb.AppendLine("SET ");
            sb.AppendLine("	HanroGroupCode = " + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HanroGroupId = " + info.HanroGroupId.ToString() + " ");
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
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 販路グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路グループ情報一覧</returns>
        private IList<HanroGroupInfo> GetListInternal(SqlTransaction transaction, HanroGroupSearchParameter para)
        {
            //返却用の一覧
            List<HanroGroupInfo> rt_list = new List<HanroGroupInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	HanroGroup.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	HanroGroup --販路グループ ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("HanroGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HanroGroupId != null)
                {
                    sb.AppendLine("	--引数「車両グループID」 ");
                    sb.AppendLine(" AND HanroGroup.HanroGroupId = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.HanroGroupId)) + " ");
                }
                if (para.HanroGroupCode != null)
                {
                    sb.AppendLine("	--引数「車両グループコード」 ");
                    sb.AppendLine(" AND HanroGroup.HanroGroupCode = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.HanroGroupCode)) + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	HanroGroup.HanroGroupCode ");

            String mySql = sb.ToString();


            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HanroGroupInfo rt_info = new HanroGroupInfo
                {
                    HanroGroupId = SQLServerUtil.dbDecimal(rdr["HanroGroupId"]),
                    HanroGroupCode = SQLServerUtil.dbInt(rdr["HanroGroupCode"]),
                    HanroGroupName = rdr["HanroGroupName"].ToString(),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
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
        /// <param name="info"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, HanroGroupInfo info)
        {
            List<string> list = new List<string>();

            KanriInfo kanriInfo = new Kanri().GetInfo();

            //if (kanriInfo.LynaKanriInfo != null && kanriInfo.LynaKanriInfo.HanroGroupId == info.HanroGroupId)
            //{
            //    list.Add("基本情報「LYNA2」タブ");
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
            return "SELECT 1 FROM HanroGroup WHERE DelFlag = 0 " + "AND HanroGroupCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
