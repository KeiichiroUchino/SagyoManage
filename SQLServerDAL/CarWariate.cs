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
    /// 車両割当テーブルのデータアクセスレイヤです。
    /// </summary>
    public class CarWariate
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
        /// 車両割当クラスのデフォルトコンストラクタです。
        /// </summary>
        public CarWariate()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、車両割当テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public CarWariate(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で車両割当情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarWariateInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarWariateSearchParameter()
            {
                SearchKbn = (int)SearchKbnEnum.Basic,
                CarWariateId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、車両割当情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>車両割当情報のリスト</returns>
        public IList<CarWariateInfo> GetList(CarWariateSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、車両割当情報を指定して、
        /// 車両割当情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="sagyoAnkenId">作業案件ID</param>
        /// <param name="info">車両割当情報</param>
        public void Save(SqlTransaction transaction, decimal sagyoAnkenId, List<CarWariateInfo> list)
        {
            List<SqlCommand> commands = new List<SqlCommand>();

            //新規文のIDを一括取得
            IEnumerator<decimal> meisaiId_iterator =
                SQLHelper.GetSequenceIds(
                    SQLHelper.SequenceIdKind.CarWariate, list.Count(x => !x.DelFlag && x.VersionColumn == null)).GetEnumerator();

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
                    sb.AppendLine(" CarWariate ");
                    sb.AppendLine("SET ");
                    sb.AppendLine(" CarWariateId =  " + info.CarWariateId.ToString() + " ");
                    sb.AppendLine(" ,SagyoAnkenId =  " + info.SagyoAnkenId.ToString() + " ");
                    sb.AppendLine(" ,CarId =  " + info.CarId.ToString() + " ");
                    sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                    sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                    sb.AppendLine("WHERE ");
                    sb.AppendLine("CarWariateId = " + info.CarWariateId.ToString() + " ");

                    string sql = sb.ToString();
                    commands.Add(new SqlCommand(sql));

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
                    sb.AppendLine(" CarWariate ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine(" CarWariateId ");
                    sb.AppendLine(" ,SagyoAnkenId ");
                    sb.AppendLine(" ,CarId ");
                    sb.AppendLine("	,DelFlag ");
                    sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
                    sb.AppendLine(" ) ");
                    sb.AppendLine("VALUES ");
                    sb.AppendLine("( ");
                    sb.AppendLine("" + newId.ToString() + "");
                    sb.AppendLine(" ," + sagyoAnkenId.ToString() + " ");
                    sb.AppendLine(" ," + info.CarId.ToString() + " ");
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
        ///  SqlTransaction情報、車両割当情報を指定して、
        ///  車両割当情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車両割当情報</param>
        public void Delete(SqlTransaction transaction, CarWariateInfo info , decimal id = 0)
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
            sb.AppendLine(" CarWariate ");
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
                sb.AppendLine(" CarWariateId = " + info.CarWariateId.ToString() + " ");
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
        public IList<CarWariateInfo> GetListInternal(SqlTransaction transaction, CarWariateSearchParameter para)
        {
            //返却用のリスト
            List<CarWariateInfo> rt_list = new List<CarWariateInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	CarWariate.* ");
            sb.AppendLine("	,Car.CarCode ");
            sb.AppendLine("	,Car.CarName ");
            sb.AppendLine("	,Car.LicPlateCarNo ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" CarWariate ");
            sb.AppendLine(" LEFT JOIN Car ");
            sb.AppendLine(" ON CarWariate.CarId = Car.CarId ");
            sb.AppendLine("     AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoAnken ");
            sb.AppendLine(" ON CarWariate.SagyoAnkenId = SagyoAnken.SagyoAnkenId ");
            sb.AppendLine("     AND CarWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	CarWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                //通常検索
                if (para.SearchKbn == (int)SearchKbnEnum.Basic)
                {
                    if (para.CarWariateId.HasValue)
                    {
                        sb.AppendLine("AND CarWariate.CarWariateId = " + para.CarWariateId.ToString() + " ");
                    }
                    if (para.SagyoAnkenId.HasValue)
                    {
                        sb.AppendLine("AND CarWariate.SagyoAnkenId = " + para.SagyoAnkenId.ToString() + " ");
                    }
                }
                //重複チェック
                else if (para.SearchKbn == (int)SearchKbnEnum.Check)
                {
                    sb.AppendLine("   AND CarWariate.SagyoAnkenId <> " + para.SagyoAnkenId.ToString() + " ");
                    sb.AppendLine("   AND CarWariate.CarId IN( " +
                         SQLHelper.GetSQLQueryWhereInByStructList(para.CarIdList) + ") ");
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
            sb.AppendLine(" CarWariate.CarWariateId ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                CarWariateInfo rt_info = new CarWariateInfo
                {
                    CarWariateId = SQLServerUtil.dbDecimal(rdr["CarWariateId"]),
                    SagyoAnkenId = SQLServerUtil.dbDecimal(rdr["SagyoAnkenId"]),
                    CarId = SQLServerUtil.dbDecimal(rdr["CarId"]),
                    CarCode = SQLServerUtil.dbInt(rdr["CarCode"]),
                    LicPlateCarNo = rdr["LicPlateCarNo"].ToString(),
                    CarName = rdr["CarName"].ToString(),
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
