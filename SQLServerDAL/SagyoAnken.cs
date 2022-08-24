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
    /// 作業案件テーブルのデータアクセスレイヤです。
    /// </summary>
    public class SagyoAnken
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
        /// 検索区分を表します。
        /// </summary>
        public enum SearchKbnEnum
        {
            Basic,
            Check,
            LastTime,
            History,
        }

        /// <summary>
        /// 作業案件クラスのデフォルトコンストラクタです。
        /// </summary>
        public SagyoAnken()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、作業案件テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SagyoAnken(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// SqlTransaction情報、作業案件情報を指定して、
        /// 作業案件情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業案件情報</param>
        public decimal Save(SqlTransaction transaction, SagyoAnkenInfo info)
        {
            //データベースに保存されているかどうか
            if (info.IsPersisted)
            {
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateUpSagyoAnkenCommands(info));
            }
            else
            {

                //InsertのSQLを作る前に、次IDを取得しておく
                info.SagyoAnkenId =
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.SagyoAnken);

                //InsertのSQLを作る前に、次IDを取得しておく
                info.SagyoAnkenCode = Convert.ToInt32(SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.SagyoAnkenCode));

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, this.CreateInSagyoAnkenCommands(info));
            }

            return info.SagyoAnkenId;
        }

        /// <summary>
        ///  SqlTransaction情報、作業場所情報を指定して、
        ///  作業場所情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">作業案件情報</param>
        public void Delete(SqlTransaction transaction, SagyoAnkenInfo info)
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
            sb.AppendLine(" SagyoAnken ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" SagyoAnkenId = " + info.SagyoAnkenId.ToString() + " ");
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
            IList<string> list = GetReferenceTables(transaction, info.SagyoAnkenId);

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
        /// コード指定で作業案件情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoAnkenInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoAnkenSearchParameter()
            {
                SagyoAnkenCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で作業案件情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public SagyoAnkenInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new SagyoAnkenSearchParameter()
            {
                SagyoAnkenId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、作業案件情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>作業案件情報のリスト</returns>
        public IList<SagyoAnkenInfo> GetList(SagyoAnkenSearchParameter para = null)
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
        public IList<SagyoAnkenInfo> GetListInternal(SqlTransaction transaction, SagyoAnkenSearchParameter para)
        {
            // クエリ
            String mySql = this.GetQuerySelect(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoAnkenInfo rt_info = new SagyoAnkenInfo
                {
                    SagyoAnkenId = SQLServerUtil.dbDecimal(rdr["SagyoAnkenId"]),
                    SagyoAnkenCode = SQLServerUtil.dbInt(rdr["SagyoAnkenCode"]),

                    SagyoStartDateTime = SQLHelper.dbDate(rdr["SagyoStartDateTime"]),
                    SagyoEndDateTime = SQLHelper.dbDate(rdr["SagyoEndDateTime"]),

                    SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),

                    Biko = rdr["Biko"].ToString(),
                    TokkiJiko = rdr["TokkiJiko"].ToString(),

                    KeiyakuId = SQLServerUtil.dbDecimal(rdr["K_KeiyakuId"]),
                    KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                    KeiyakuName = rdr["KeiyakuName"].ToString(),

                    SagyoDaiBunruiName = rdr["SagyoDaiBunruiName"].ToString(),
                    SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                    SagyoShoBunruiName = rdr["SagyoShoBunruiName"].ToString(),

                    StaffCode = SQLServerUtil.dbInt(rdr["StaffCode"]),
                    StaffName = rdr["StaffName"].ToString(),

                    StaffSu = SQLServerUtil.dbInt(rdr["StaffSu"]),
                    CarSu = SQLServerUtil.dbInt(rdr["CarSu"]),

                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
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

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。（作業案件一覧画面用）
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<SagyoAnkenInfo> GetListInternalIchiran(SqlTransaction transaction, SagyoAnkenSearchParameter para)
        {
            // クエリ
            String mySql = this.GetQuerySagyoAnkenIchiranSelect(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoAnkenInfo rt_info = new SagyoAnkenInfo
                {
                    SagyoAnkenId = SQLServerUtil.dbDecimal(rdr["SagyoAnkenId"]),
                    SagyoAnkenCode = SQLServerUtil.dbInt(rdr["SagyoAnkenCode"]),

                    SagyoStartDateTime = SQLHelper.dbDate(rdr["SagyoStartDateTime"]),
                    SagyoEndDateTime = SQLHelper.dbDate(rdr["SagyoEndDateTime"]),

                    SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),

                    Biko = rdr["Biko"].ToString(),
                    TokkiJiko = rdr["TokkiJiko"].ToString(),

                    KeiyakuId = SQLServerUtil.dbDecimal(rdr["K_KeiyakuId"]),
                    KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                    KeiyakuName = rdr["KeiyakuName"].ToString(),

                    SagyoDaiBunruiName = rdr["SagyoDaiBunruiName"].ToString(),
                    SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                    SagyoShoBunruiName = rdr["SagyoShoBunruiName"].ToString(),

                    StaffCode = SQLServerUtil.dbInt(rdr["StaffCode"]),
                    StaffName = rdr["StaffName"].ToString(),

                    StaffSu = SQLServerUtil.dbInt(rdr["StaffSu"]),
                    CarSu = SQLServerUtil.dbInt(rdr["CarSu"]),

                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

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

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"])
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。（作業予定画面用）
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<SagyoAnkenInfo> GetListInternalSagyoYotei(SagyoAnkenSearchParameter para)
        {
            // クエリ
            String mySql = this.GetQuerySelectSagyoYotei(para);

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SagyoAnkenInfo rt_info = new SagyoAnkenInfo
                {
                    SagyoAnkenId = SQLServerUtil.dbDecimal(rdr["SagyoAnkenId"]),
                    SagyoAnkenCode = SQLServerUtil.dbInt(rdr["SagyoAnkenCode"]),

                    SagyoStartDateTime = SQLHelper.dbDate(rdr["SagyoStartDateTime"]),
                    SagyoEndDateTime = SQLHelper.dbDate(rdr["SagyoEndDateTime"]),

                    SagyoBashoCode = rdr["SagyoBashoCode"].ToString(),
                    TokuisakiCode = rdr["TokuisakiCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    TokuisakiName = rdr["TokuisakiName"].ToString(),

                    Biko = rdr["Biko"].ToString(),
                    TokkiJiko = rdr["TokkiJiko"].ToString(),

                    KeiyakuId = SQLServerUtil.dbDecimal(rdr["K_KeiyakuId"]),
                    KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                    KeiyakuName = rdr["KeiyakuName"].ToString(),

                    SagyoDaiBunruiName = rdr["SagyoDaiBunruiName"].ToString(),
                    SagyoChuBunruiName = rdr["SagyoChuBunruiName"].ToString(),
                    SagyoShoBunruiName = rdr["SagyoShoBunruiName"].ToString(),

                    StaffCode = SQLServerUtil.dbInt(rdr["StaffCode"]),
                    StaffName = rdr["StaffNameLinkResult"].ToString(),

                    LicPlateCarNo = rdr["CarNameLinkResult"].ToString(),

                    //StaffSu = SQLServerUtil.dbInt(rdr["StaffSu"]),
                    //CarSu = SQLServerUtil.dbInt(rdr["CarSu"]),

                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                };

                //返却用の値を返します
                return rt_info;
            });
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
            if (SQLHelper.RecordExists("SELECT * FROM Keiyaku WHERE DelFlag = " + NSKUtil.BoolToInt(false)
                + " AND KeiyakuId = " + id.ToString() + "", transaction))
            {
                list.Add("契約");
            }

            //重複を除いて返却
            return list.Distinct().ToList();
        }

        /// <summary>
        /// 作業案件を更新するSqlCommandを作成する
        /// </summary>
        /// <param name="info">作業案件</param>
        /// <returns></returns>
        private SqlCommand CreateUpSagyoAnkenCommands(SagyoAnkenInfo info, bool exclusiveflg = true)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            //Update文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" SagyoAnken ");
            sb.AppendLine("SET ");
            sb.AppendLine("  SagyoAnkenId =  " + info.SagyoAnkenId.ToString() + " ");
            sb.AppendLine(" ,SagyoAnkenCode =  " + info.SagyoAnkenCode.ToString() + " ");
            sb.AppendLine(" ,KeiyakuId =  " + info.KeiyakuId.ToString() + " ");
            sb.AppendLine(" ,SagyoStartDateTime =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoStartDateTime) + " ");
            sb.AppendLine(" ,SagyoEndDateTime =  " + SQLHelper.DateTimeToDbDateTime(info.SagyoEndDateTime) + " ");
            sb.AppendLine(" ,SekininshaId =  " + info.SekininshaId.ToString() + " ");
            sb.AppendLine(" ,KanryoFlg =  " + NSKUtil.BoolToInt(info.KanryoFlg).ToString() + " ");
            sb.AppendLine(" ,Biko =  N'" + SQLHelper.GetSanitaizingSqlString(info.Biko.Trim()) + "' ");
            sb.AppendLine(" ,TokkiJiko =  N'" + SQLHelper.GetSanitaizingSqlString(info.TokkiJiko.Trim()) + "' ");
            sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("SagyoAnkenId = " + info.SagyoAnkenId.ToString() + " ");

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
        /// 作業案件を追加するSqlCommandを作成する
        /// </summary>
        /// <param name="info">作業案件</param>
        /// <returns></returns>
        private SqlCommand CreateInSagyoAnkenCommands(SagyoAnkenInfo info)
        {
            //常設列の取得オプションを作る
            //--更新は入力と更新
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            //Insert文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" SagyoAnken ");
            sb.AppendLine(" ( ");
            sb.AppendLine("  SagyoAnkenId ");
            sb.AppendLine(" ,SagyoAnkenCode  ");
            sb.AppendLine(" ,KeiyakuId  ");
            sb.AppendLine(" ,SagyoStartDateTime  ");
            sb.AppendLine(" ,SagyoEndDateTime  ");
            sb.AppendLine(" ,SekininshaId  ");
            sb.AppendLine(" ,KanryoFlg  ");
            sb.AppendLine(" ,Biko  ");
            sb.AppendLine(" ,TokkiJiko  ");
            sb.AppendLine(" ,DelFlag  ");
            sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine("" + info.SagyoAnkenId.ToString() + "");
            sb.AppendLine(" , " + info.SagyoAnkenCode.ToString() + " ");
            sb.AppendLine(" , " + info.KeiyakuId.ToString() + " ");
            sb.AppendLine(" ,  " + SQLHelper.DateTimeToDbDateTime(info.SagyoStartDateTime) + " ");
            sb.AppendLine(" ,  " + SQLHelper.DateTimeToDbDateTime(info.SagyoEndDateTime) + " ");
            sb.AppendLine(" ,  " + info.SekininshaId.ToString() + " ");
            sb.AppendLine(" ,  " + NSKUtil.BoolToInt(info.KanryoFlg).ToString() + " ");
            sb.AppendLine(" ,  N'" + SQLHelper.GetSanitaizingSqlString(info.Biko.Trim()) + "' ");
            sb.AppendLine(" ,  N'" + SQLHelper.GetSanitaizingSqlString(info.TokkiJiko.Trim()) + "' ");
            sb.AppendLine(" , " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
            sb.AppendLine(" , " + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
            sb.AppendLine(") ");

            string sql = sb.ToString();
            SqlCommand command = new SqlCommand(sql);

            return command;
        }

        /// <summary>
        /// 作業案件を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">作業案件</param>
        /// <returns></returns>
        private string GetQuerySelect(SagyoAnkenSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 SagyoAnken.* ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuId AS K_KeiyakuId ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuCode ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuName ");
            sb.AppendLine("	 ,SagyoBasho.SagyoBashoCode ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiCode ");
            sb.AppendLine("	 ,SagyoBasho.SagyoBashoName ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiName ");
            sb.AppendLine("	 ,SagyoDaiBunrui.SagyoDaiBunruiName ");
            sb.AppendLine("	 ,SagyoChuBunrui.SagyoChuBunruiName ");
            sb.AppendLine("	 ,SagyoShoBunrui.SagyoShoBunruiName ");
            sb.AppendLine("	 ,Staff.StaffCode ");
            sb.AppendLine("  ,Staff.StaffName ");
            sb.AppendLine("	 ,SW.StaffSu ");
            sb.AppendLine("  ,CW.CarSu ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" SagyoAnken ");
            sb.AppendLine(" LEFT JOIN Keiyaku ");
            sb.AppendLine(" ON SagyoAnken.KeiyakuId = Keiyaku.KeiyakuId ");
            sb.AppendLine("     AND Keiyaku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoBasho ");
            sb.AppendLine(" ON Keiyaku.SagyoBashoId = SagyoBasho.SagyoBashoId ");
            sb.AppendLine("     AND SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Tokuisaki ");
            sb.AppendLine(" ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Staff");
            sb.AppendLine(" ON SagyoAnken.SekininshaId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" LEFT JOIN SagyoDaiBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoDaiBunruiId = SagyoDaiBunrui.SagyoDaiBunruiId ");
            sb.AppendLine("     AND SagyoDaiBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoChuBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoChuBunruiId = SagyoChuBunrui.SagyoChuBunruiId ");
            sb.AppendLine("     AND SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoShoBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoShoBunruiId = SagyoShoBunrui.SagyoShoBunruiId ");
            sb.AppendLine("     AND SagyoShoBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" LEFT JOIN ( ");
            sb.AppendLine("     SELECT ");
            sb.AppendLine("       SagyoinWariate.SagyoAnkenId ");
            sb.AppendLine("       , Count(SagyoinWariate.SagyoAnkenId) AS StaffSu");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       SagyoinWariate  ");
            sb.AppendLine("     WHERE ");
            sb.AppendLine("       SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       SagyoinWariate.SagyoAnkenId  ");
            sb.AppendLine(" ) SW ");
            sb.AppendLine(" ON SagyoAnken.SagyoAnkenId = SW.SagyoAnkenId ");

            sb.AppendLine(" LEFT JOIN ( ");
            sb.AppendLine("     SELECT ");
            sb.AppendLine("       CarWariate.SagyoAnkenId ");
            sb.AppendLine("       , Count(CarWariate.SagyoAnkenId) AS CarSu ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       CarWariate  ");
            sb.AppendLine("     WHERE ");
            sb.AppendLine("       CarWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       CarWariate.SagyoAnkenId  ");
            sb.AppendLine(" ) CW ");
            sb.AppendLine(" ON SagyoAnken.SagyoAnkenId = CW.SagyoAnkenId ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	SagyoAnken.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {

                //通常検索
                if (para.SearchKbn == (int)SearchKbnEnum.Basic)
                {
                    if (para.SagyoAnkenId.HasValue && para.SagyoAnkenId > 0)
                    {
                        sb.AppendLine(" AND SagyoAnken.SagyoAnkenId = " + para.SagyoAnkenId.ToString() + " ");
                    }
                    if (para.SagyoAnkenCode.HasValue && para.SagyoAnkenCode > 0)
                    {
                        sb.AppendLine(" AND SagyoAnken.SagyoAnkenCode = " + para.SagyoAnkenCode.ToString() + " ");
                    }
                    if (para.KeiyakuId.HasValue && para.KeiyakuId > 0)
                    {
                        sb.AppendLine(" AND SagyoAnken.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                    }
                    if (para.SagyoBashoId.HasValue && para.SagyoBashoId > 0)
                    {
                        sb.AppendLine(" AND Keiyaku.SagyoBashoId = " + para.SagyoBashoId.ToString() + " ");
                    }
                    if (para.TokuisakiId.HasValue && para.TokuisakiId > 0)
                    {
                        sb.AppendLine(" AND SagyoBasho.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                    }
                    if (para.SagyoDaiBunruiId.HasValue && para.SagyoDaiBunruiId > 0)
                    {
                        sb.AppendLine(" AND Keiyaku.SagyoDaiBunruiId = " + para.SagyoDaiBunruiId.ToString() + " ");
                    }
                    if (para.SagyoChuBunruiId.HasValue && para.SagyoChuBunruiId > 0)
                    {
                        sb.AppendLine(" AND Keiyaku.SagyoChuBunruiId = " + para.SagyoChuBunruiId.ToString() + " ");
                    }
                    if (para.SagyoShoBunruiId.HasValue && para.SagyoShoBunruiId > 0)
                    {
                        sb.AppendLine(" AND Keiyaku.SagyoShoBunruiId = " + para.SagyoShoBunruiId.ToString() + " ");
                    }
                }
                //重複チェック
                else if (para.SearchKbn == (int)SearchKbnEnum.Check)
                {
                    sb.AppendLine("   AND SagyoAnken.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                    sb.AppendLine("   AND SagyoAnken.SagyoAnkenId <> " + para.SagyoAnkenId.ToString() + " ");
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
                //前回値取得
                else if (para.SearchKbn == (int)SearchKbnEnum.LastTime)
                {
                    sb.AppendLine("   AND SagyoAnken.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                    sb.AppendLine("   AND SagyoAnken.SagyoAnkenId <> " + para.SagyoAnkenId.ToString() + " ");
                    sb.AppendLine("   AND SagyoAnken.SagyoEndDateTime < " + SQLHelper.DateTimeToDbDateTime(para.SagyoStartDateTime));
                }

                //履歴取得取得
                else if (para.SearchKbn == (int)SearchKbnEnum.History)
                {
                    if (para.KeiyakuId.HasValue && para.KeiyakuId > 0)
                    {
                        sb.AppendLine(" AND SagyoAnken.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                    }

                    if (para.SagyoStartDateTime.HasValue && para.SagyoStartDateTime > DateTime.MinValue)
                    {
                        sb.AppendLine("      AND SagyoAnken.SagyoStartDateTime >= " + "'"
                            + para.SagyoStartDateTime.Value.ToString("yyyy/MM/dd 00:00:00", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                    }

                    if (para.SagyoEndDateTime.HasValue && para.SagyoEndDateTime > DateTime.MinValue)
                    {
                        sb.AppendLine("      AND SagyoAnken.SagyoEndDateTime <= " + "'"
                            + para.SagyoEndDateTime.Value.ToString("yyyy/MM/dd 23:59:59", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                    }
                }
            }

            sb.AppendLine("ORDER BY ");

            if (para != null && para.SearchKbn == (int)SearchKbnEnum.LastTime)
            {
                sb.AppendLine(" SagyoAnken.SagyoStartDateTime DESC ");
            }
            else
            {
                sb.AppendLine(" SagyoAnken.SagyoAnkenCode ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 作業案件を取得するSQL（作業案件一覧画面用）を作成します。
        /// </summary>
        /// <param name="info">作業案件</param>
        /// <returns></returns>
        private string GetQuerySagyoAnkenIchiranSelect(SagyoAnkenSearchParameter para, bool orderByFlg = true)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 SA.* ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuId AS K_KeiyakuId ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuCode ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuName ");
            sb.AppendLine("	 ,Keiyaku.KeiyakuJyokyo ");
            sb.AppendLine("	 ,Keiyaku.SagyoNinzu ");
            sb.AppendLine("	 ,Keiyaku.SagyobiKbn ");
            sb.AppendLine("	 ,Keiyaku.Monday ");
            sb.AppendLine("	 ,Keiyaku.Tuesday ");
            sb.AppendLine("	 ,Keiyaku.Wednesday ");
            sb.AppendLine("	 ,Keiyaku.Thursday ");
            sb.AppendLine("	 ,Keiyaku.Friday ");
            sb.AppendLine("	 ,Keiyaku.Saturday ");
            sb.AppendLine("	 ,Keiyaku.Sunday ");
            sb.AppendLine("	 ,Keiyaku.SagyoDay1 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDay2 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDay3 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDay4 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDay5 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate1 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate2 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate3 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate4 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate5 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate6 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate7 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate8 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate9 ");
            sb.AppendLine("	 ,Keiyaku.SagyoDate10 ");
            sb.AppendLine("	 ,Keiyaku.DisableFlag ");

            sb.AppendLine("	 ,SagyoBasho.SagyoBashoCode ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiCode ");
            sb.AppendLine("	 ,SagyoBasho.SagyoBashoName ");
            sb.AppendLine("	 ,Tokuisaki.TokuisakiName ");
            sb.AppendLine("	 ,SagyoDaiBunrui.SagyoDaiBunruiName ");
            sb.AppendLine("	 ,SagyoChuBunrui.SagyoChuBunruiName ");
            sb.AppendLine("	 ,SagyoShoBunrui.SagyoShoBunruiName ");
            sb.AppendLine("	 ,Staff.StaffCode ");
            sb.AppendLine("  ,Staff.StaffName ");
            sb.AppendLine("	 ,SW.StaffSu ");
            sb.AppendLine("  ,CW.CarSu ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" Keiyaku ");
            sb.AppendLine("   LEFT JOIN (  ");
            sb.AppendLine("     SELECT ");
            sb.AppendLine("       SagyoAnkenId ");
            sb.AppendLine("       , SagyoAnkenCode ");
            sb.AppendLine("       , KeiyakuId ");
            sb.AppendLine("       , SagyoStartDateTime ");
            sb.AppendLine("       , SagyoEndDateTime ");
            sb.AppendLine("       , SekininshaId ");
            sb.AppendLine("       , KanryoFlg ");
            sb.AppendLine("       , Biko ");
            sb.AppendLine("       , TokkiJiko  ");
            sb.AppendLine("       , DelFlag  ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       SagyoAnken  ");
            sb.AppendLine("     WHERE ");
            if (para.SagyoYmd != DateTime.MinValue)
            {
                sb.AppendLine("       SagyoAnken.SagyoStartDateTime <= " + "'"
                    + para.SagyoYmd.ToString("yyyy/MM/dd 23:59:59", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                sb.AppendLine("       AND SagyoAnken.SagyoEndDateTime >= " + "'"
                    + para.SagyoYmd.ToString("yyyy/MM/dd 00:00:00", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
            }
            else
            {
                sb.AppendLine("       SagyoAnken.SagyoStartDateTime >= " + "'"
                    + para.SagyoStartDateTime.Value.ToString("yyyy/MM/dd 00:00:00", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                sb.AppendLine("       AND SagyoAnken.SagyoEndDateTime <= " + "'"
                    + para.SagyoEndDateTime.Value.ToString("yyyy/MM/dd 23:59:59", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
            }
            sb.AppendLine("   ) SA  ");
            sb.AppendLine(" ON Keiyaku.KeiyakuId = SA.KeiyakuId ");
            sb.AppendLine("     AND SA.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoBasho ");
            sb.AppendLine(" ON Keiyaku.SagyoBashoId = SagyoBasho.SagyoBashoId ");
            sb.AppendLine("     AND SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Tokuisaki ");
            sb.AppendLine(" ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN Staff");
            sb.AppendLine(" ON SA.SekininshaId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" LEFT JOIN SagyoDaiBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoDaiBunruiId = SagyoDaiBunrui.SagyoDaiBunruiId ");
            sb.AppendLine("     AND SagyoDaiBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoChuBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoChuBunruiId = SagyoChuBunrui.SagyoChuBunruiId ");
            sb.AppendLine("     AND SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" LEFT JOIN SagyoShoBunrui ");
            sb.AppendLine(" ON Keiyaku.SagyoShoBunruiId = SagyoShoBunrui.SagyoShoBunruiId ");
            sb.AppendLine("     AND SagyoShoBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" LEFT JOIN ( ");
            sb.AppendLine("     SELECT ");
            sb.AppendLine("       SagyoinWariate.SagyoAnkenId ");
            sb.AppendLine("       , Count(SagyoinWariate.SagyoAnkenId) AS StaffSu");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       SagyoinWariate  ");
            sb.AppendLine("     WHERE ");
            sb.AppendLine("       SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       SagyoinWariate.SagyoAnkenId  ");
            sb.AppendLine(" ) SW ");
            sb.AppendLine(" ON SA.SagyoAnkenId = SW.SagyoAnkenId ");

            sb.AppendLine(" LEFT JOIN ( ");
            sb.AppendLine("     SELECT ");
            sb.AppendLine("       CarWariate.SagyoAnkenId ");
            sb.AppendLine("       , Count(CarWariate.SagyoAnkenId) AS CarSu ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       CarWariate  ");
            sb.AppendLine("     WHERE ");
            sb.AppendLine("       CarWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       CarWariate.SagyoAnkenId  ");
            sb.AppendLine(" ) CW ");
            sb.AppendLine(" ON SA.SagyoAnkenId = CW.SagyoAnkenId ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	Keiyaku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.SagyoAnkenId.HasValue && para.SagyoAnkenId > 0)
                {
                    sb.AppendLine(" AND SagyoAnken.SagyoAnkenId = " + para.SagyoAnkenId.ToString() + " ");
                }
                if (para.SagyoAnkenCode.HasValue && para.SagyoAnkenCode > 0)
                {
                    sb.AppendLine(" AND SagyoAnken.SagyoAnkenCode = " + para.SagyoAnkenCode.ToString() + " ");
                }
                if (para.KeiyakuId.HasValue && para.KeiyakuId > 0)
                {
                    sb.AppendLine(" AND Keiyaku.KeiyakuId = " + para.KeiyakuId.ToString() + " ");
                }
                if (para.SagyoBashoId.HasValue && para.SagyoBashoId > 0)
                {
                    sb.AppendLine(" AND Keiyaku.SagyoBashoId = " + para.SagyoBashoId.ToString() + " ");
                }
                if (para.TokuisakiId.HasValue && para.TokuisakiId > 0)
                {
                    sb.AppendLine(" AND SagyoBasho.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (para.SagyoDaiBunruiId.HasValue && para.SagyoDaiBunruiId > 0)
                {
                    sb.AppendLine(" AND Keiyaku.SagyoDaiBunruiId = " + para.SagyoDaiBunruiId.ToString() + " ");
                }
                if (para.SagyoChuBunruiId.HasValue && para.SagyoChuBunruiId > 0)
                {
                    sb.AppendLine(" AND Keiyaku.SagyoChuBunruiId = " + para.SagyoChuBunruiId.ToString() + " ");
                }
                if (para.SagyoShoBunruiId.HasValue && para.SagyoShoBunruiId > 0)
                {
                    sb.AppendLine(" AND Keiyaku.SagyoShoBunruiId = " + para.SagyoShoBunruiId.ToString() + " ");
                }

                if (para.SagyoYmd != DateTime.MinValue)
                {
                    sb.AppendLine(" AND Keiyaku.KeiyakuStartDate <= " + "'"
                        + para.SagyoYmd.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");

                    sb.AppendLine(" AND Keiyaku.KeiyakuEndDate >= " + "'"
                        + para.SagyoYmd.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                }
            }

            if (orderByFlg)
            {
                sb.AppendLine("ORDER BY ");
                sb.AppendLine(" Keiyaku.KeiyakuCode ");
            }

            return sb.ToString();
        }

        /// <summary>
        /// 作業予定情報を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">作業予定情報</param>
        /// <returns></returns>
        private string GetQuerySelectSagyoYotei(SagyoAnkenSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   SagyoAnken.* ");
            sb.AppendLine("   , Keiyaku.KeiyakuId AS K_KeiyakuId ");
            sb.AppendLine("   , Keiyaku.KeiyakuCode ");
            sb.AppendLine("   , Keiyaku.KeiyakuName ");
            sb.AppendLine("   , SagyoBasho.SagyoBashoCode ");
            sb.AppendLine("   , Tokuisaki.TokuisakiCode ");
            sb.AppendLine("   , SagyoBasho.SagyoBashoName ");
            sb.AppendLine("   , Tokuisaki.TokuisakiName ");
            sb.AppendLine("   , SagyoDaiBunrui.SagyoDaiBunruiName ");
            sb.AppendLine("   , SagyoChuBunrui.SagyoChuBunruiName ");
            sb.AppendLine("   , SagyoShoBunrui.SagyoShoBunruiName ");
            sb.AppendLine("   , Staff.StaffCode ");
            sb.AppendLine("   , Staff.StaffName  ");
            sb.AppendLine("   , TRIM(',' FROM SW1.StaffNameLinkResult) AS StaffNameLinkResult ");
            sb.AppendLine("   , TRIM(',' FROM CW1.CarNameLinkResult) AS CarNameLinkResult ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   SagyoAnken  ");
            sb.AppendLine("   LEFT JOIN Keiyaku  ");
            sb.AppendLine("     ON SagyoAnken.KeiyakuId = Keiyaku.KeiyakuId  ");
            sb.AppendLine("     AND Keiyaku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoBasho  ");
            sb.AppendLine("     ON Keiyaku.SagyoBashoId = SagyoBasho.SagyoBashoId  ");
            sb.AppendLine("     AND SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN Tokuisaki  ");
            sb.AppendLine("     ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId  ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN Staff  ");
            sb.AppendLine("     ON SagyoAnken.SekininshaId = Staff.StaffId  ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN (  ");
            sb.AppendLine("     select ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("       , (  ");
            sb.AppendLine("         SELECT ");
            sb.AppendLine("           T2.StaffName + ','  -- 文字列を連結する ");
            sb.AppendLine("         FROM ");
            sb.AppendLine("           (  ");
            sb.AppendLine("             SELECT ");
            sb.AppendLine("               SagyoinWariateId ");
            sb.AppendLine("               , SagyoAnkenId ");
            sb.AppendLine("               , StaffName  ");
            sb.AppendLine("             FROM ");
            sb.AppendLine("               SagyoinWariate  ");
            sb.AppendLine("               LEFT JOIN Staff  ");
            sb.AppendLine("                 ON SagyoinWariate.StaffId = Staff.StaffId  ");
            sb.AppendLine("                 AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" 			WHERE ");
            sb.AppendLine(" 			  SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("           ) T2  ");
            sb.AppendLine("         WHERE ");
            sb.AppendLine("           T2.SagyoAnkenId = T1.SagyoAnkenId  -- 対象のセクションを選択する ");
            sb.AppendLine("         ORDER BY ");
            sb.AppendLine("           T2.SagyoinWariateId FOR XML PATH ('')  -- FOR XML PATHで副問い合わせの結果を1行1列に連結する ");
            sb.AppendLine("       ) AS StaffNameLinkResult  ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       SagyoinWariate AS T1  ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("   ) SW1  ");
            sb.AppendLine("     ON SagyoAnken.SagyoAnkenId = SW1.SagyoAnkenId  ");


            sb.AppendLine("   LEFT JOIN (  ");
            sb.AppendLine("     select ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("       , (  ");
            sb.AppendLine("         SELECT ");
            sb.AppendLine("           T2.LicPlateCarNo + ','  -- 文字列を連結する ");
            sb.AppendLine("         FROM ");
            sb.AppendLine("           (  ");
            sb.AppendLine("             SELECT ");
            sb.AppendLine("               CarWariateId ");
            sb.AppendLine("               , SagyoAnkenId ");
            sb.AppendLine("               , LicPlateCarNo  ");
            sb.AppendLine("             FROM ");
            sb.AppendLine("               CarWariate  ");
            sb.AppendLine("               LEFT JOIN Car  ");
            sb.AppendLine("                 ON CarWariate.CarId = Car.CarId  ");
            sb.AppendLine("                 AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" 			WHERE ");
            sb.AppendLine(" 			  CarWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("           ) T2  ");
            sb.AppendLine("         WHERE ");
            sb.AppendLine("           T2.SagyoAnkenId = T1.SagyoAnkenId  -- 対象のセクションを選択する ");
            sb.AppendLine("         ORDER BY ");
            sb.AppendLine("           T2.CarWariateId FOR XML PATH ('')  -- FOR XML PATHで副問い合わせの結果を1行1列に連結する ");
            sb.AppendLine("       ) AS CarNameLinkResult  ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       CarWariate AS T1  ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("   ) CW1  ");
            sb.AppendLine("     ON SagyoAnken.SagyoAnkenId = CW1.SagyoAnkenId  ");


            sb.AppendLine("   LEFT JOIN SagyoDaiBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoDaiBunruiId = SagyoDaiBunrui.SagyoDaiBunruiId  ");
            sb.AppendLine("     AND SagyoDaiBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoChuBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoChuBunruiId = SagyoChuBunrui.SagyoChuBunruiId  ");
            sb.AppendLine("     AND SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoShoBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoShoBunruiId = SagyoShoBunrui.SagyoShoBunruiId  ");
            sb.AppendLine("     AND SagyoShoBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   SagyoAnken.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                //// 社員
                //if (para.StaffCheckList.Length != 0)
                //{
                //    sb.AppendLine(" AND ( ");
                //    sb.AppendLine("    SagyoAnken.SekininshaId IN ( " + para.StaffCheckList + ") ");
                //    sb.AppendLine("    OR ");
                //    sb.AppendLine("    ( ");
                //    sb.AppendLine("      EXISTS ( ");
                //    sb.AppendLine("        SELECT 1 FROM SagyoinWariate ");
                //    sb.AppendLine("        WHERE SagyoinWariate.SagyoAnkenId = SagyoAnken.SagyoAnkenId ");
                //    sb.AppendLine("        AND SagyoinWariate.StaffId IN ( " + para.StaffCheckList + ") ");
                //    sb.AppendLine("             ) ");
                //    sb.AppendLine("    ) ");
                //    sb.AppendLine(" ) ");
                //}

                sb.AppendLine("       AND SagyoAnken.SagyoStartDateTime >= '" 
                    + para.SagyoYmd.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
                sb.AppendLine("       AND SagyoAnken.SagyoStartDateTime <= '" 
                    + para.SagyoYmd.ToString("yyyy/MM/dd", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
            }

            sb.AppendLine(" ORDER BY ");

            sb.AppendLine(" SagyoAnken.SagyoStartDateTime DESC ");

            return sb.ToString();
        }

        #endregion
    }
}
