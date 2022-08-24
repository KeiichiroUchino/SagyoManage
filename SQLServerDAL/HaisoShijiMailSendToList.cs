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
    /// 配送指示メール送信先リストテーブルのデータアクセスレイヤです。
    /// </summary>
    public class HaisoShijiMailSendToList
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
        /// 配送指示メール送信先リストクラスのデフォルトコンストラクタです。
        /// </summary>
        public HaisoShijiMailSendToList()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、配送指示メール送信先リストテーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaisoShijiMailSendToList(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で配送指示メール送信先リスト情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HaisoShijiMailSendToListInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HaisoShijiMailSendToListSearchParameter()
            {
                ToraDONStaffId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、配送指示メール送信先リスト情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>配送指示メール送信先リスト情報のリスト</returns>
        public IList<HaisoShijiMailSendToListInfo> GetList(HaisoShijiMailSendToListSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、配送指示メール送信先リスト情報を指定して、
        /// 配送指示メール送信先リスト情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配送指示メール送信先リスト情報</param>
        public void Save(SqlTransaction transaction, HaisoShijiMailSendToListInfo info)
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
                sb.AppendLine(" HaisoShijiMailSendToList ");
                sb.AppendLine("SET ");
                sb.AppendLine(" Email = N'" + SQLHelper.GetSanitaizingSqlString(info.Email.Trim()) + "'");
                sb.AppendLine(",SendFlag = " + NSKUtil.BoolToInt(info.SendFlag).ToString() + "");

                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HaisoShijiMailSendToListId = " + info.HaisoShijiMailSendToListId.ToString() + " ");
                sb.AppendLine("AND EXISTS ( ");
                sb.AppendLine("SELECT 1 FROM ");
                sb.AppendLine(" TORADON_Staff ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("StaffId = " + info.ToraDONStaffId.ToString() + " ");
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
                //トラDONの社員存在チェック
                if (!SQLHelper.RecordExists(CreateCheckSQL(info.ToraDONStaffId),
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HaisoShijiMailSendToMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" HaisoShijiMailSendToList ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 HaisoShijiMailSendToListId ");
                sb.AppendLine("	,ToraDONStaffId ");
                sb.AppendLine("	,Email ");
                sb.AppendLine("	,SendFlag ");

                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.ToraDONStaffId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.Email.Trim()) + "'");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.SendFlag).ToString() + " ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));
            }
        }

        /// <summary>
        ///  SqlTransaction情報、配送指示メール送信先リスト情報を指定して、
        ///  配送指示メール送信先リスト情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配送指示メール送信先リスト情報</param>
        public void Delete(SqlTransaction transaction, HaisoShijiMailSendToListInfo info)
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
            sb.AppendLine(" HaisoShijiMailSendToList ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HaisoShijiMailSendToListId = " + info.HaisoShijiMailSendToListId.ToString() + " ");
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

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<HaisoShijiMailSendToListInfo> GetListInternal(SqlTransaction transaction, HaisoShijiMailSendToListSearchParameter para)
        {
            //返却用のリスト
            List<HaisoShijiMailSendToListInfo> rt_list = new List<HaisoShijiMailSendToListInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 HaisoShijiMailSendToList.* ");
            sb.AppendLine("	,ToraDONStaff.StaffId ToraDONStaffId_Main ");
            sb.AppendLine("	,ToraDONStaff.StaffCd ToraDONStaffCd ");
            sb.AppendLine("	,ToraDONStaff.StaffNM ToraDONStaffNM ");
            sb.AppendLine("	,ToraDONStaff.StaffNMK ToraDONStaffNMK ");
            sb.AppendLine("	,ToraDONStaff.DisableFlag ToraDONDisableFlag ");
            sb.AppendLine("	,ToraDONStaff.RetireDate ToraDONRetireDate ");
            sb.AppendLine("	,ToraDONBranchOffice.BranchOfficeId ToraDONBranchOfficeId ");
            sb.AppendLine("	,ToraDONBranchOffice.BranchOfficeCd ToraDONBranchOfficeCd ");
            sb.AppendLine("	,ToraDONBranchOffice.BranchOfficeNM ToraDONBranchOfficeNM ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Staff AS ToraDONStaff ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	HaisoShijiMailSendToList ");
            sb.AppendLine("ON HaisoShijiMailSendToList.ToraDONStaffId = ToraDONStaff.StaffId ");
            sb.AppendLine("AND HaisoShijiMailSendToList.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_BranchOffice AS ToraDONBranchOffice ");
            sb.AppendLine("ON ToraDONBranchOffice.BranchOfficeId = ToraDONStaff.BranchOfficeId ");
            sb.AppendLine("AND ToraDONBranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Staff ");
            sb.AppendLine("ON Staff.ToraDONStaffId = ToraDONStaff.StaffId ");
            sb.AppendLine("AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONStaff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ToraDONStaffId.HasValue)
                {
                    sb.AppendLine("AND ToraDONStaff.StaffId = " + para.ToraDONStaffId.ToString() + " ");
                }
                if (para.DriverFlag.HasValue)
                {
                    sb.AppendLine("AND Staff.DriverFlag = " + NSKUtil.BoolToInt(para.DriverFlag.Value) + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ToraDONStaffCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HaisoShijiMailSendToListInfo rt_info = new HaisoShijiMailSendToListInfo
                {
                    HaisoShijiMailSendToListId = SQLServerUtil.dbDecimal(rdr["HaisoShijiMailSendToListId"]),
                    Email = rdr["Email"].ToString(),
                    SendFlag = SQLHelper.dbBit(rdr["SendFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    ToraDONStaffId = SQLServerUtil.dbDecimal(rdr["ToraDONStaffId_Main"]),
                    ToraDONStaffCode = SQLServerUtil.dbInt(rdr["ToraDONStaffCd"]),
                    ToraDONStaffName = rdr["ToraDONStaffNM"].ToString(),
                    ToraDONStaffNameKana = rdr["ToraDONStaffNMK"].ToString(),
                    ToraDONRetireDate = NSKUtil.DecimalWithTimeToDateTime(SQLServerUtil.dbDecimal(rdr["ToraDONRetireDate"])),
                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),

                    ToraDONBranchOfficeId = SQLServerUtil.dbDecimal(rdr["ToraDONBranchOfficeId"]),
                    ToraDONBranchOfficeCode = SQLServerUtil.dbInt(rdr["ToraDONBranchOfficeCd"]),
                    ToraDONBranchOfficeName = rdr["ToraDONBranchOfficeNM"].ToString(),
                };

                if (0 < rt_info.HaisoShijiMailSendToListId)
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
            sb.AppendLine(" TORADON_Staff ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND StaffId = " + id.ToString() + " ");
            return sb.ToString();
        }

        #endregion
    }
}
