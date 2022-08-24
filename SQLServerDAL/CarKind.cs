using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using System.Configuration;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 車種（トラDON補）テーブルのデータアクセスレイヤです。
    /// </summary>
    public class CarKind
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
        /// 車種クラスのデフォルトコンストラクタです。
        /// </summary>
        public CarKind()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、車種テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public CarKind(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で車種情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public CarKindInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarKindSearchParameter()
            {
                ToraDONCarKindCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で車種情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarKindInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarKindSearchParameter()
            {
                ToraDONCarKindId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、車種情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種情報のリスト</returns>
        public IList<CarKindInfo> GetList(CarKindSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、車種情報を指定して、
        /// 車種情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車種情報</param>
        public void Save(SqlTransaction transaction, CarKindInfo info)
        {
            String id = string.Empty;

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
                sb.AppendLine(" CarKind ");
                sb.AppendLine("SET ");
                sb.AppendLine(" JomuinKakeritsu = " + info.JomuinKakeritsu.ToString() + " ");
                sb.AppendLine(",ToraDONRenkeiCarKindId = " + info.ToraDONRenkeiCarKindId.ToString() + " ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("CarKindId = " + info.CarKindId.ToString() + " ");
                sb.AppendLine("AND EXISTS ( ");
                sb.AppendLine("SELECT 1 FROM ");
                sb.AppendLine(" TORADON_CarKind ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("CarKindId = " + info.ToraDONCarKindId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(false).ToString() + ") ");
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

                id = info.CarKindId.ToString();
            }
            else
            {
                //トラDONの車種存在チェック
                if (!SQLHelper.RecordExists(CreateCheckSQL(info.ToraDONCarKindId),
                    transaction))
                {
                    //リトライ可能な例外
                    throw new
                        Model.DALExceptions.CanRetryException(
                        string.Format("この{0}データは既に他の利用者に更新されている為、更新できません。" +
                        "\r\n" + "最新情報を確認してください。", string.Empty)
                        , MessageBoxIcon.Warning);
                }

                //常設列の取得オプションを作る
                //--新規登録
                SQLHelper.PopulateColumnOptions popOption =
                    SQLHelper.PopulateColumnOptions.EntryColumns |
                        SQLHelper.PopulateColumnOptions.AdditionColumns;

                //InsertのSQLを作る前に、次IDを取得しておく
                decimal newId =
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.CarKindMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" CarKind ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 CarKindId ");
                sb.AppendLine("	,ToraDONCarKindId ");
                sb.AppendLine("	,JomuinKakeritsu ");
                sb.AppendLine("	,ToraDONRenkeiCarKindId ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.ToraDONCarKindId.ToString() + " ");
                sb.AppendLine("," + info.JomuinKakeritsu.ToString() + " ");
                sb.AppendLine("," + info.ToraDONRenkeiCarKindId.ToString() + " ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                id = newId.ToString();
            }
        }

        /// <summary>
        ///  SqlTransaction情報、車種情報を指定して、
        ///  車種情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">車種情報</param>
        public void Delete(SqlTransaction transaction, CarKindInfo info)
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
            sb.AppendLine(" CarKind ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" CarKindId = " + info.CarKindId.ToString() + " ");
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

            //なし

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、車種（トラDON補）テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>車種（トラDON補）テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, CarKindSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<CarKindInfo> GetListInternal(SqlTransaction transaction, CarKindSearchParameter para)
        {
            //返却用のリスト
            List<CarKindInfo> rt_list = new List<CarKindInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ToraDONCarKind.CarKindId ToraDONCarKindId_Main ");
            sb.AppendLine("	,ToraDONCarKind.CarKindCd ToraDONCarKindCd ");
            sb.AppendLine("	,ToraDONCarKind.CarKindNM ToraDONCarKindNM ");
            sb.AppendLine("	,ToraDONCarKind.CarKindNMK ToraDONCarKindNMK ");
            sb.AppendLine("	,ToraDONCarKind.CarKindSNM ToraDONCarKindSNM ");
            sb.AppendLine("	,ToraDONCarKind.DisableFlag ToraDONDisableFlag ");
            sb.AppendLine("	,CarKind.* ");
            sb.AppendLine("	,ToraDONRenkeiCarKind.CarKindCd ToraDONRenkeiCarKindCd ");
            sb.AppendLine("	,ToraDONRenkeiCarKind.CarKindNM ToraDONRenkeiCarKindNM ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_CarKind AS ToraDONCarKind ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	CarKind ");
            sb.AppendLine("ON CarKind.ToraDONCarKindId = ToraDONCarKind.CarKindId ");
            sb.AppendLine("AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_CarKind ToraDONRenkeiCarKind ");
            sb.AppendLine("ON ToraDONRenkeiCarKind.CarKindId = CarKind.ToraDONRenkeiCarKindId ");
            sb.AppendLine("AND ToraDONRenkeiCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ToraDONCarKindId.HasValue)
                {
                    sb.AppendLine("AND ToraDONCarKind.CarKindId = " + para.ToraDONCarKindId.ToString() + " ");
                }
                if (para.ToraDONCarKindCode.HasValue)
                {
                    sb.AppendLine("AND ToraDONCarKind.CarKindCd = " + para.ToraDONCarKindCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ToraDONCarKindCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                CarKindInfo rt_info = new CarKindInfo
                {
                    CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]),
                    ToraDONCarKindId = SQLServerUtil.dbDecimal(rdr["ToraDONCarKindId_Main"]),
                    ToraDONCarKindCode = SQLServerUtil.dbInt(rdr["ToraDONCarKindCd"]),
                    ToraDONCarKindName = rdr["ToraDONCarKindNM"].ToString(),
                    ToraDONCarKindNameKana = rdr["ToraDONCarKindNMK"].ToString(),
                    ToraDONCarKindSNM = rdr["ToraDONCarKindSNM"].ToString(),
                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),
                    JomuinKakeritsu = SQLServerUtil.dbDecimal(rdr["JomuinKakeritsu"]),
                    ToraDONRenkeiCarKindId = SQLServerUtil.dbDecimal(rdr["ToraDONRenkeiCarKindId"]),
                    ToraDONRenkeiCarKindCode = SQLServerUtil.dbInt(rdr["ToraDONRenkeiCarKindCd"]),
                    ToraDONRenkeiCarKindName = rdr["ToraDONRenkeiCarKindNM"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.CarKindId)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したIdチェック用のSQLを作成します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string CreateCheckSQL(decimal id)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT 1 ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_CarKind ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND CarKindId = " + id.ToString() + " ");
            return sb.ToString();
        }

        #endregion
    }
}
