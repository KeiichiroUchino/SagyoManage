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
    /// 車種グループテーブルのデータアクセスレイヤです。
    /// </summary>
    public class CarKindGroup
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
        private string _tableName = "車種グループ";

        /// <summary>
        /// 車種グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public CarKindGroup()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public CarKindGroup(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// SqlTransaction情報、車種グループ情報を指定して、
        /// 車種グループ情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車種グループ情報</param>
        public void Save(SqlTransaction transaction, CarKindGroupInfo info)
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
                sb.AppendLine(" CarKindGroup ");
                sb.AppendLine("SET ");
                sb.AppendLine(" CarKindGroupName = N'" + SQLHelper.GetSanitaizingSqlString(info.CarKindGroupName.Trim()) + "'");

                sb.AppendLine(" ,DisableFlag = '" + info.DisableFlag.ToString() + "' ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("CarKindGroupId = " + info.CarKindGroupId.ToString() + " ");
                sb.AppendLine("--排他のチェック ");
                sb.AppendLine(" AND	VersionColumn = @versionColumn ");

                string sql = sb.ToString();

                SqlCommand command = new SqlCommand(sql);
                command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command, this._tableName);

                id = info.CarKindGroupId.ToString();
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.CarKindGroupMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" CarKindGroup ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  CarKindGroupId ");
                sb.AppendLine(" ,CarKindGroupCode ");
                sb.AppendLine(" ,CarKindGroupName ");

                sb.AppendLine(" ,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.CarKindGroupCode.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.CarKindGroupName.Trim()) + "'");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.CarKindGroupCode.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }

                id = newId.ToString();
            }

            List<string> mySqlList = new List<string>();

            // -- 車種グループ明細削除SQL取得
            StringBuilder sbDel = new StringBuilder();
            sbDel.AppendLine("DELETE FROM ");
            sbDel.AppendLine(" CarKindGroupMeisai ");
            sbDel.AppendLine("WHERE ");
            sbDel.AppendLine(" CarKindGroupId = " + id + " ");

            mySqlList.Add(sbDel.ToString());

            //車種グループ明細登録SQL取得
            foreach (CarKindGroupMeisaiInfo meisai in info.CarKindGroupMeisaiList)
            {
                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //Insert文を作成
                StringBuilder sbIns = new StringBuilder();
                sbIns.AppendLine("INSERT INTO ");
                sbIns.AppendLine(" CarKindGroupMeisai ");
                sbIns.AppendLine(" ( ");
                sbIns.AppendLine("  CarKindGroupId ");
                sbIns.AppendLine(" ,Gyo ");
                sbIns.AppendLine(" ,ToraDONCarKindId ");

                sbIns.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sbIns.AppendLine(" ) ");
                sbIns.AppendLine("VALUES ");
                sbIns.AppendLine("( ");
                sbIns.AppendLine(" " + id + " ");
                sbIns.AppendLine(" ," + meisai.Gyo.ToString() + " ");
                sbIns.AppendLine(" ," + meisai.ToraDONCarKindId.ToString() + " ");

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
        ///  SqlTransaction情報、車種グループ情報を指定して、
        ///  車種グループ情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車種グループ情報</param>
        public void Delete(SqlTransaction transaction, CarKindGroupInfo info)
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
            sb.AppendLine(" CarKindGroup ");
            sb.AppendLine("SET ");

            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" CarKindGroupId = " + info.CarKindGroupId.ToString() + " ");
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
            sb.AppendLine(" CarKindGroupMeisai ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" CarKindGroupId = " + info.CarKindGroupId.ToString() + " ");

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
        /// SqlTransaction情報、検索条件情報を指定して、車種グループテーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種グループテーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, CarKindGroupSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// コードを指定して、車種グループ情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="code">code</param>
        /// <returns>車種グループ情報</returns>
        public CarKindGroupInfo GetInfo(int code)
        {
            CarKindGroupInfo info = this.GetListInternal(null,
                new CarKindGroupSearchParameter
                {
                    CarKindGroupCode = code,
                }
                ).FirstOrDefault();

            return info;
        }

        /// <summary>
        /// Idを指定して、車種グループ情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns>車種グループ情報</returns>
        public CarKindGroupInfo GetInfoById(decimal id)
        {
            CarKindGroupInfo info = this.GetListInternal(null,
                new CarKindGroupSearchParameter
                {
                    CarKindGroupId = id,
                }
                ).FirstOrDefault();

            return info;
        }

        /// <summary>
        /// 検索条件情報を指定して、車種グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種グループ情報一覧</returns>
        public IList<CarKindGroupInfo> GetList(CarKindGroupSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、車種グループ情報、検索条件情報を指定して、
        /// 車種グループ情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車種グループ情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, CarKindGroupInfo info, int newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new CarKindGroup().GetKenSuu(transaction, new CarKindGroupSearchParameter() { CarKindGroupCode = newCode }) != 0)
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
            sb.AppendLine(" CarKindGroup ");
            sb.AppendLine("SET ");
            sb.AppendLine("	CarKindGroupCode = " + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" CarKindGroupId = " + info.CarKindGroupId.ToString() + " ");
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
        /// 車種グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種グループ情報一覧</returns>
        private IList<CarKindGroupInfo> GetListInternal(SqlTransaction transaction, CarKindGroupSearchParameter para)
        {
            //返却用の一覧
            List<CarKindGroupInfo> rt_list = new List<CarKindGroupInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	CarKindGroup.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	CarKindGroup --車種グループ ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("CarKindGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.CarKindGroupId != null)
                {
                    sb.AppendLine("	--引数「車両グループID」 ");
                    sb.AppendLine(" AND CarKindGroup.CarKindGroupId = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.CarKindGroupId)) + " ");
                }
                if (para.CarKindGroupCode != null)
                {
                    sb.AppendLine("	--引数「車両グループコード」 ");
                    sb.AppendLine(" AND CarKindGroup.CarKindGroupCode = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.CarKindGroupCode)) + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	CarKindGroup.CarKindGroupCode ");

            String mySql = sb.ToString();


            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                CarKindGroupInfo rt_info = new CarKindGroupInfo
                {
                    CarKindGroupId = SQLServerUtil.dbDecimal(rdr["CarKindGroupId"]),
                    CarKindGroupCode = SQLServerUtil.dbInt(rdr["CarKindGroupCode"]),
                    CarKindGroupName = rdr["CarKindGroupName"].ToString(),
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
        private IList<string> GetReferenceTables(SqlTransaction transaction, CarKindGroupInfo info)
        {
            List<string> list = new List<string>();

            KanriInfo kanriInfo = new Kanri().GetInfo();

            //if (kanriInfo.LynaKanriInfo != null && kanriInfo.LynaKanriInfo.CarKindGroupId == info.CarKindGroupId)
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
            return "SELECT 1 FROM CarKindGroup WHERE DelFlag = 0 " + "AND CarKindGroupCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
