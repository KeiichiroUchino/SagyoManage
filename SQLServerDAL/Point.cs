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

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 発着地（トラDON補）テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Point
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
        private string _tableName = "発着地（トラDON補）";

         /// <summary>
        /// 発着地クラスのデフォルトコンストラクタです。
        /// </summary>
        public Point()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Point(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で発着地情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PointInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointSearchParameter()
            {
                ToraDONPointCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で品目情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PointInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointSearchParameter()
            {
                ToraDONPointId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、発着地情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地情報のリスト</returns>
        public IList<PointInfo> GetList(PointSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、発着地情報を指定して、
        /// 発着地情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地情報</param>
        public void Save(SqlTransaction transaction, PointInfo info)
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
                sb.AppendLine(" Point ");
                sb.AppendLine("SET ");
                sb.AppendLine(" PointSName = N'" + SQLHelper.GetSanitaizingSqlString(info.PointShortName.Trim()) + "'");
                sb.AppendLine(",PointSSName = N'" + SQLHelper.GetSanitaizingSqlString(info.PointSShortName.Trim()) + "'");
                sb.AppendLine(",PointLBunruiId = " + info.PointLBunruiId.ToString() + " ");
                sb.AppendLine(",PointMBunruiId = " + info.PointMBunruiId.ToString() + " ");
                sb.AppendLine(",HomenId = " + info.HomenId.ToString() + " ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("PointId = " + info.PointId.ToString() + " ");
                sb.AppendLine("AND EXISTS ( ");
                sb.AppendLine("SELECT 1 FROM ");
                sb.AppendLine(" TORADON_Point ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("PointId = " + info.ToraDONPointId.ToString() + " ");
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
            }
            else
            {
                //トラDONの発着地存在チェック
                if (!SQLHelper.RecordExists(CreateCheckSQL(info.ToraDONPointId),
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.PointMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Point ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 PointId ");
                sb.AppendLine("	,ToraDONPointId ");
                sb.AppendLine("	,PointSName ");
                sb.AppendLine("	,PointSSName ");
                sb.AppendLine("	,PointLBunruiId ");
                sb.AppendLine("	,PointMBunruiId ");
                sb.AppendLine("	,HomenId ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.ToraDONPointId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointShortName.Trim()) + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.PointSShortName.Trim()) + "'");
                sb.AppendLine("," + info.PointMBunruiId.ToString() + " ");
                sb.AppendLine("," + info.PointLBunruiId.ToString() + " ");
                sb.AppendLine("," + info.HomenId.ToString() + " ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //Idの存在チェック
                if (SQLHelper.RecordExists(CreateIdCheckSQL(info.ToraDONPointId.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、発着地情報を指定して、
        ///  発着地情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">発着地情報</param>
        public void Delete(SqlTransaction transaction, PointInfo info)
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
            sb.AppendLine(" Point ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" PointId = " + info.PointId.ToString() + " ");
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
        /// SqlTransaction情報、検索条件情報を指定して、発着地（トラDON補）テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地（トラDON補）テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, PointSearchParameter para)
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
        public IList<PointInfo> GetListInternal(SqlTransaction transaction, PointSearchParameter para)
        {
            //返却用のリスト
            List<PointInfo> rt_list = new List<PointInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ToraDONPoint.PointId ToraDONPointId_Main ");
            sb.AppendLine("	,ToraDONPoint.PointId ToraDONPointId ");
            sb.AppendLine("	,ToraDONPoint.PointCd ToraDONPointCd ");
            sb.AppendLine("	,ToraDONPoint.PointNM ToraDONPointNM ");
            sb.AppendLine("	,ToraDONPoint.PointNMK ToraDONPointNMK ");
            sb.AppendLine("	,ToraDONPoint.DisableFlag ToraDONDisableFlag ");
            sb.AppendLine("	,Point.* ");
            sb.AppendLine("	,Homen.HomenCode ");
            sb.AppendLine("	,Homen.HomenName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Point AS ToraDONPoint ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Point ");
            sb.AppendLine("ON Point.ToraDONPointId = ToraDONPoint.PointId ");
            sb.AppendLine("AND Point.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Homen ");
            sb.AppendLine("ON Homen.HomenId = Point.HomenId ");
            sb.AppendLine("AND Homen.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.PointId.HasValue)
                {
                    sb.AppendLine("AND Point.PointId = " + para.PointId.ToString() + " ");
                }
                if (para.ToraDONPointId.HasValue)
                {
                    sb.AppendLine("AND ToraDONPoint.PointId = " + para.ToraDONPointId.ToString() + " ");
                }
                if (para.ToraDONPointCode.HasValue)
                {
                    sb.AppendLine("AND ToraDONPoint.PointCd = " + para.ToraDONPointCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ToraDONPointCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PointInfo rt_info = new PointInfo
                {
                    PointId = SQLServerUtil.dbDecimal(rdr["PointId"]),
                    ToraDONPointId = SQLServerUtil.dbDecimal(rdr["ToraDONPointId_Main"]),
                    ToraDONPointCode = SQLServerUtil.dbInt(rdr["ToraDONPointCd"]),
                    ToraDONPointName = rdr["ToraDONPointNM"].ToString(),
                    ToraDONPointNameKana = rdr["ToraDONPointNMK"].ToString(),
                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),
                    PointShortName = rdr["PointSName"].ToString(),
                    PointSShortName = rdr["PointSSName"].ToString(),
                    PointLBunruiId = SQLServerUtil.dbDecimal(rdr["PointLBunruiId"]),
                    PointMBunruiId = SQLServerUtil.dbDecimal(rdr["PointMBunruiId"]),
                    HomenId = SQLServerUtil.dbDecimal(rdr["HomenId"]),
                    HomenCode = SQLServerUtil.dbInt(rdr["HomenCode"]),
                    HomenName = rdr["HomenName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.PointId)
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
            sb.AppendLine(" TORADON_Point ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND PointId = " + id.ToString() + " ");
            return sb.ToString();
        }

        /// <summary>
        /// 指定したIDチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateIdCheckSQL(string id)
        {
            return "SELECT 1 FROM Point WHERE DelFlag = 0 " + "AND ToraDONPointId = N'" + SQLHelper.GetSanitaizingSqlString(id.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
