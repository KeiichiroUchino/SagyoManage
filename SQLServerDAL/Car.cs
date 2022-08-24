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
    /// 車両テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Car
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
        /// 車両クラスのデフォルトコンストラクタです。
        /// </summary>
        public Car()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、車両テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Car(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で車両情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns></returns>
        public CarInfo GetInfo(int code, List<Int32?> list = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarSearchParameter()
            {
                CarCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で車両情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns></returns>
        public CarInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarSearchParameter()
            {
                CarId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、車両情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>車両情報のリスト</returns>
        public IList<CarInfo> GetList(CarSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、車両情報を指定して、
        /// 車両情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車両情報</param>
        public void Save(SqlTransaction transaction, CarInfo info)
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
                sb.AppendLine(" Car ");
                sb.AppendLine("SET ");
                sb.AppendLine(" CarId  = " + info.CarId.ToString() + " ");
                sb.AppendLine(",CarCode = " + info.CarCode.ToString() + " ");
                sb.AppendLine(" ,LicPlateCarNo = N'" + SQLHelper.GetSanitaizingSqlString(info.LicPlateCarNo.Trim()) + "' ");
                sb.AppendLine(" ,CarName = N'" + SQLHelper.GetSanitaizingSqlString(info.CarName.Trim()) + "' ");
                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("CarId = " + info.CarId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Car);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Car ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 CarId ");
                sb.AppendLine("	,CarCode ");
                sb.AppendLine("	,LicPlateCarNo ");
                sb.AppendLine("	,CarName ");
                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.CarCode.ToString() + " ");
                sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.LicPlateCarNo.Trim()) + "' ");
                sb.AppendLine(" , N'" + SQLHelper.GetSanitaizingSqlString(info.CarName.Trim()) + "' ");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));
            }

            //コードの存在チェック
            if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.CarCode),
                transaction))
            {
                throw new Model.DALExceptions.UniqueConstraintException(Model.DALExceptions.UniqueConstraintException.message);
            }
        }

        /// <summary>
        ///  SqlTransaction情報、車両情報を指定して、
        ///  車両情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車両情報</param>
        public void Delete(SqlTransaction transaction, CarInfo info)
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
            sb.AppendLine(" Car ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" CarId = " + info.CarId.ToString() + " ");
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
            IList<string> list = GetReferenceTables(transaction, info.CarId);

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

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<CarInfo> GetListInternal(SqlTransaction transaction, CarSearchParameter para)
        {
            //返却用のリスト
            List<CarInfo> rt_list = new List<CarInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Car ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.CarId.HasValue)
                {
                    sb.AppendLine("AND CarId = " + para.CarId.ToString() + " ");
                }
                if (para.CarCode.HasValue)
                {
                    sb.AppendLine("AND CarCode = " + para.CarCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Car.CarCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                CarInfo rt_info = new CarInfo
                {
                    CarId = SQLServerUtil.dbDecimal(rdr["CarId"]),
                    CarCode = SQLServerUtil.dbInt(rdr["CarCode"]),
                    LicPlateCarNo = rdr["LicPlateCarNo"].ToString(),
                    CarName = rdr["CarName"].ToString(),
                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                };

                if (0 < rt_info.CarId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
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

            //車両割り当て 	車両ID
            if (SQLHelper.RecordExists("SELECT * FROM CarWariate WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND CarId = " + id.ToString() + "", transaction))
            {
                list.Add("車両割り当て");
            }

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(int code)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT 1 ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Car ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND CarCode = " + code.ToString() + " ");
            sb.AppendLine(" HAVING Count(*) > 1 ");

            return sb.ToString();
        }

        #endregion
    }
}
