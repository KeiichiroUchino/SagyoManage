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
    /// 配色テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Haishoku
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
        private string _tableName = "配色";

        /// <summary>
        /// Haishokuクラスのデフォルトコンストラクタです。
        /// </summary>
        public Haishoku()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、配色テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Haishoku(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// テーブルキー、テーブルID指定で配色情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HaishokuExInfo GetInfoEx(HaishokuExSearchParameter para, SqlTransaction transaction = null)
        {
            return GetListExInternal(transaction, para).FirstOrDefault();
        }

        /// <summary>
        /// テーブルキー、テーブルID指定で配色情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HaishokuInfo GetInfo(int key, decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HaishokuSearchParameter()
            {
                TableKey = key,
                TableId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// システム区分、システムId指定で配色情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HaishokuInfo GetInfoBySystemKbn(int kbn, int id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HaishokuSearchParameter()
            {
                TableKey = ((int)DefaultProperty.HaishokuTableKbn.SystemName),
                SystemKbn = kbn,
                SystemId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で配色情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HaishokuInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HaishokuSearchParameter()
            {
                HaishokuId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、配色情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>配色情報のリスト</returns>
        public IList<HaishokuInfo> GetList(HaishokuSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 検索条件情報を指定して、配色情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>配色情報のリスト</returns>
        public IList<HaishokuExInfo> GetListEx(HaishokuExSearchParameter para = null)
        {
            return GetListExInternal(null, para);
        }

        /// <summary>
        /// 検索条件情報を指定して、配色情報のリストを取得します。
        /// </summary>
        /// <param name="tableKeyInfo">テーブルキー検索条件情報</param>
        /// <param name="functionKeyInfo">機能キー検索条件情報</param>
        /// <returns>配色情報のリスト</returns>
        public IList<TargetKeyInfo> GetTargetKeyList(SystemNameInfo tableKeyInfo, SystemNameInfo functionKeyInfo)
        {
            return GetTargetKeyListInternal(null, tableKeyInfo, functionKeyInfo);
        }

        /// <summary>
        /// SqlTransaction情報、配色情報を指定して、
        /// 配色情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配色情報</param>
        public void Save(SqlTransaction transaction, HaishokuInfo info)
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
                sb.AppendLine(" Haishoku ");
                sb.AppendLine("SET ");
                sb.AppendLine("  TableKey = " + info.TableKey.ToString() + " ");
                sb.AppendLine(" ,TableId = " + info.TableId.ToString() + " ");
                sb.AppendLine(" ,SystemKbn = " + info.SystemKbn.ToString() + " ");
                sb.AppendLine(" ,SystemId = " + info.SystemId.ToString() + " ");
                sb.AppendLine(" ,ForeColor = " + info.ForeColor.ToString() + " ");
                sb.AppendLine(" ,BackColor = " + info.BackColor.ToString() + " ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HaishokuId = " + info.HaishokuId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HaishokuMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Haishoku ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  HaishokuId ");
                sb.AppendLine(" ,TableKey ");
                sb.AppendLine(" ,TableId ");
                sb.AppendLine(" ,SystemKbn ");
                sb.AppendLine(" ,SystemId ");
                sb.AppendLine(" ,ForeColor ");
                sb.AppendLine(" ,BackColor ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(", " + info.TableKey.ToString() + " ");
                sb.AppendLine(", " + info.TableId.ToString() + " ");
                sb.AppendLine(", " + info.SystemKbn.ToString() + " ");
                sb.AppendLine(", " + info.SystemId.ToString() + " ");
                sb.AppendLine(", " + info.ForeColor.ToString() + " ");
                sb.AppendLine(", " + info.BackColor.ToString() + " ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                if (info.TableKey == ((int)DefaultProperty.HaishokuTableKbn.SystemName))
                {
                    //データの存在チェック
                    if (SQLHelper.RecordExists(CreateSystemCodeCheckSQL(info.SystemKbn.ToString(), info.SystemId.ToString()),
                        transaction))
                    {
                        throw new Model.DALExceptions.UniqueConstraintException(
                            string.Format("同じシステムコードが既に登録されています。"
                                    , this._tableName));
                    }
                }
                else
                {
                    //データの存在チェック
                    if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.TableKey.ToString(), info.TableId.ToString()),
                        transaction))
                    {
                        throw new Model.DALExceptions.UniqueConstraintException(
                            string.Format("同じテーブルの同じコードが既に登録されています。"
                                    , this._tableName));
                    }
                }
            }
        }

        /// <summary>
        /// SqlTransaction情報、配色情報を指定して、
        /// 配色情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配色情報</param>
        public void SaveEx(SqlTransaction transaction, HaishokuExInfo info)
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
                sb.AppendLine(" HaishokuEx ");
                sb.AppendLine("SET ");
                sb.AppendLine("  TableKey = " + info.TableKey.ToString() + " ");
                sb.AppendLine(" ,FunctionKey = " + info.FunctionKey.ToString() + " ");
                sb.AppendLine(" ,TargetKey = " + info.TargetKey.ToString() + " ");
                if (info.ForeColor == null)
                {
                    sb.AppendLine(" ,ForeColor = NULL ");
                }
                else
                {
                    sb.AppendLine(" ,ForeColor = " + info.ForeColor.ToString() + " ");
                }
                if (info.BackColor == null)
                {
                    sb.AppendLine(" ,BackColor = NULL ");
                }
                else
                {
                    sb.AppendLine(" ,BackColor = " + info.BackColor.ToString() + " ");
                }

                sb.AppendLine(" ,DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(" ,DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HaishokuId = " + info.HaishokuId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HaishokuMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" HaishokuEx ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  HaishokuId ");
                sb.AppendLine(" ,TableKey ");
                sb.AppendLine(" ,FunctionKey ");
                sb.AppendLine(" ,TargetKey ");
                sb.AppendLine(" ,ForeColor ");
                sb.AppendLine(" ,BackColor ");

                sb.AppendLine(" ,DisableFlag ");
                sb.AppendLine(" ,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(", " + info.TableKey.ToString() + " ");
                sb.AppendLine(", " + info.FunctionKey.ToString() + " ");
                sb.AppendLine(", " + info.TargetKey.ToString() + " ");
                if (info.ForeColor == null)
                {
                    sb.AppendLine(" ,NULL ");
                }
                else
                {
                    sb.AppendLine(", " + info.ForeColor.ToString() + " ");
                }
                if (info.BackColor == null)
                {
                    sb.AppendLine(" ,NULL ");
                }
                else
                {
                    sb.AppendLine(", " + info.BackColor.ToString() + " ");
                }

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //データの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQLEx(info.TableKey, info.FunctionKey, info.TargetKey),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ配色情報が既に登録されています。"
                                , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、配色情報を指定して、
        ///  配色情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配色情報</param>
        public void Delete(SqlTransaction transaction, HaishokuInfo info)
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
            sb.AppendLine(" Haishoku ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HaishokuId = " + info.HaishokuId.ToString() + " ");
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

            #endregion
        }

        /// <summary>
        ///  SqlTransaction情報、配色情報を指定して、
        ///  配色情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">配色情報</param>
        public void DeleteEx(SqlTransaction transaction, HaishokuExInfo info)
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
            sb.AppendLine(" HaishokuEx ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HaishokuId = " + info.HaishokuId.ToString() + " ");
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

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<HaishokuInfo> GetListInternal(SqlTransaction transaction, HaishokuSearchParameter para)
        {
            //返却用のリスト
            List<HaishokuInfo> rt_list = new List<HaishokuInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Haishoku.* ");

            foreach (DefaultProperty.HaishokuTableKbn Value in Enum.GetValues(typeof(DefaultProperty.HaishokuTableKbn)))
            {
                string name = Enum.GetName(typeof(DefaultProperty.HaishokuTableKbn), Value);
                string tableName = DefaultProperty.GetHaishokuTableKbnTableMeisho((DefaultProperty.HaishokuTableKbn)Value);

                switch ((DefaultProperty.HaishokuTableKbn)Value)
                {
                    case DefaultProperty.HaishokuTableKbn.SystemName:
                        sb.AppendLine("	,SystemName.SystemNameCode TableCode_" + ((int)Value).ToString() + " ");
                        sb.AppendLine("	,SystemName.SystemNameName TableName_" + ((int)Value).ToString() + " ");
                        break;
                    case DefaultProperty.HaishokuTableKbn.Car:
                        sb.AppendLine("	,ISNULL(" + tableName + "." + name + "Cd, 0) TableCode_" + ((int)Value).ToString() + " ");
                        sb.AppendLine("	,ISNULL(" + tableName + ".LicPlateCarNo, '') TableName_" + ((int)Value).ToString() + " ");
                        break;
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                        sb.AppendLine("	,ISNULL(" + tableName + "." + name + "Cd, 0) TableCode_" + ((int)Value).ToString() + " ");
                        sb.AppendLine("	,ISNULL(" + tableName + "." + name + "NM, '') TableName_" + ((int)Value).ToString() + " ");
                        break;
                    default:
                        break;
                }
            }
            sb.AppendLine("FROM ");
            sb.AppendLine("	Haishoku ");

            foreach (DefaultProperty.HaishokuTableKbn Value in Enum.GetValues(typeof(DefaultProperty.HaishokuTableKbn)))
            {
                string name = Enum.GetName(typeof(DefaultProperty.HaishokuTableKbn), Value);
                string tableName = DefaultProperty.GetHaishokuTableKbnTableMeisho((DefaultProperty.HaishokuTableKbn)Value);

                switch ((DefaultProperty.HaishokuTableKbn)Value)
                {
                    case DefaultProperty.HaishokuTableKbn.SystemName:
                        sb.AppendLine("LEFT OUTER JOIN ");
                        sb.AppendLine("	SystemName ");
                        sb.AppendLine("	ON  Haishoku.TableKey = " + ((int)Value).ToString() + " ");
                        sb.AppendLine("	AND SystemName.SystemNameKbn = Haishoku.SystemKbn ");
                        sb.AppendLine("	AND SystemName.SystemNameCode = Haishoku.SystemId ");
                        break;
                    case DefaultProperty.HaishokuTableKbn.Car:
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                        sb.AppendLine("LEFT OUTER JOIN ");
                        sb.AppendLine("	" + tableName + " ");
                        sb.AppendLine("	ON  Haishoku.TableKey = " + ((int)Value).ToString() + " ");
                        sb.AppendLine("	AND " + tableName + "." + name + "Id = Haishoku.TableId ");
                        sb.AppendLine("	AND " + tableName + ".DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                        break;
                    default:
                        break;
                }
            }

            sb.AppendLine("WHERE ");
            sb.AppendLine(" Haishoku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HaishokuId.HasValue)
                {
                    sb.AppendLine("AND Haishoku.HaishokuId = " + para.HaishokuId.ToString() + " ");
                }
                if (para.TableKey.HasValue)
                {
                    sb.AppendLine("AND Haishoku.TableKey = " + para.TableKey.ToString() + " ");
                }
                if (para.TableId.HasValue)
                {
                    sb.AppendLine("AND Haishoku.TableId = " + para.TableId.ToString() + " ");
                }
                if (para.SystemKbn.HasValue)
                {
                    sb.AppendLine("AND Haishoku.SystemKbn = " + para.SystemKbn.ToString() + " ");
                }
                if (para.SystemId.HasValue)
                {
                    sb.AppendLine("AND Haishoku.SystemId = " + para.SystemId.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Haishoku.TableKey ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HaishokuInfo rt_info = new HaishokuInfo
                {
                    HaishokuId = SQLServerUtil.dbDecimal(rdr["HaishokuId"]),
                    TableKey = SQLServerUtil.dbInt(rdr["TableKey"]),
                    TableId = SQLServerUtil.dbDecimal(rdr["TableId"]),
                    SystemKbn = SQLServerUtil.dbInt(rdr["SystemKbn"]),
                    SystemId = SQLServerUtil.dbInt(rdr["SystemId"]),
                    ForeColor = SQLServerUtil.dbInt(rdr["ForeColor"]),
                    BackColor = SQLServerUtil.dbInt(rdr["BackColor"]),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                };

                switch ((DefaultProperty.HaishokuTableKbn)(rt_info.TableKey))
                {
                    case DefaultProperty.HaishokuTableKbn.SystemName:
                        rt_info.TableCode = 0;
                        rt_info.TableName = string.Empty;
                        rt_info.SystemKbnName = rdr["TableName_" + SQLServerUtil.dbInt(rdr["TableKey"]).ToString()].ToString();
                        break;
                    case DefaultProperty.HaishokuTableKbn.Car:
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                        rt_info.TableCode = SQLServerUtil.dbInt(rdr["TableCode_" + SQLServerUtil.dbInt(rdr["TableKey"]).ToString()]);
                        rt_info.TableName = rdr["TableName_" + SQLServerUtil.dbInt(rdr["TableKey"]).ToString()].ToString();
                        rt_info.SystemKbnName = string.Empty;
                        break;
                    default:
                        break;
                }

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<HaishokuExInfo> GetListExInternal(SqlTransaction transaction, HaishokuExSearchParameter para)
        {
            //返却用のリスト
            List<HaishokuInfo> rt_list = new List<HaishokuInfo>();

            //システム名称取得
            IList<SystemNameInfo> systemNameList = new SystemName().GetList();
            IList<SystemNameInfo> tableKeyList = systemNameList
                .Where(x => x.SystemNameKbn == (int)DefaultProperty.SystemNameKbn.HaishokuTableKbn)
                .OrderBy(x => x.SystemNameCode)
                .ToList();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 HaishokuEx.* ");

            foreach (SystemNameInfo tableKeyInfo in tableKeyList)
            {
                if (tableKeyInfo != null && 0 < tableKeyInfo.IntegerValue01)
                {
                    DefaultProperty.HaishokuTableKbn tableKbn = (DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode;
                    string name = Enum.GetName(typeof(DefaultProperty.HaishokuTableKbn), tableKbn);
                    string tableName = DefaultProperty.GetHaishokuTableKbnTableMeisho(tableKbn);
                    foreach (SystemNameInfo functionKeyInfo in systemNameList
                        .Where(x => x.SystemNameKbn == (int)tableKeyInfo.IntegerValue01)
                        .OrderBy(x => x.SystemNameCode))
                    {
                        string functionKey = ((int)functionKeyInfo.SystemNameCode).ToString();
                        string wk_tableName = string.Empty;

                        switch (tableKbn)
                        {
                            case DefaultProperty.HaishokuTableKbn.SystemName:
                                wk_tableName = "SystemName_" + ((int)functionKeyInfo.SystemNameCode).ToString();
                                sb.AppendLine("	," + wk_tableName + ".SystemNameCode TargetKeyCode_" + functionKey + " ");
                                sb.AppendLine("	," + wk_tableName + ".SystemNameName TargetKeyName_" + functionKey + " ");
                                break;
                            case DefaultProperty.HaishokuTableKbn.Car:
                                wk_tableName = tableName + "_" + ((int)functionKeyInfo.SystemNameCode).ToString();
                                sb.AppendLine("	,ISNULL(" + wk_tableName + "." + name + "Cd, 0) TargetKeyCode_" + functionKey + " ");
                                sb.AppendLine("	,ISNULL(" + wk_tableName + ".LicPlateCarNo, '') TargetKeyName_" + functionKey + " ");
                                break;
                            case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                            case DefaultProperty.HaishokuTableKbn.Staff:
                                wk_tableName = tableName + "_" + ((int)functionKeyInfo.SystemNameCode).ToString();
                                sb.AppendLine("	,ISNULL(" + wk_tableName + "." + name + "Cd, 0) TargetKeyCode_" + functionKey + " ");
                                sb.AppendLine("	,ISNULL(" + wk_tableName + "." + name + "NM, '') TargetKeyName_" + functionKey + " ");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            sb.AppendLine("FROM ");
            sb.AppendLine("	HaishokuEx ");

            foreach (SystemNameInfo tableKeyInfo in tableKeyList)
            {
                if (tableKeyInfo != null && 0 < tableKeyInfo.IntegerValue01)
                {
                    DefaultProperty.HaishokuTableKbn tableKbn = (DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode;
                    string name = Enum.GetName(typeof(DefaultProperty.HaishokuTableKbn), tableKbn);
                    string tableName = DefaultProperty.GetHaishokuTableKbnTableMeisho(tableKbn);
                    foreach (SystemNameInfo functionKeyInfo in systemNameList
                        .Where(x => x.SystemNameKbn == (int)tableKeyInfo.IntegerValue01)
                        .OrderBy(x => x.SystemNameCode))
                    {
                        string functionKey = ((int)functionKeyInfo.SystemNameCode).ToString();
                        string wk_tableName = string.Empty;

                        switch (tableKbn)
                        {
                            case DefaultProperty.HaishokuTableKbn.SystemName:
                                wk_tableName = "SystemName_" + ((int)functionKeyInfo.SystemNameCode).ToString();
                                sb.AppendLine("LEFT OUTER JOIN ");
                                sb.AppendLine("	SystemName " + wk_tableName + " ");
                                sb.AppendLine("	ON  HaishokuEx.TableKey = " + ((int)tableKbn).ToString() + " ");
                                sb.AppendLine("	AND HaishokuEx.FunctionKey = " + ((int)functionKeyInfo.SystemNameCode).ToString() + " ");
                                sb.AppendLine("	AND CONVERT(int, HaishokuEx.TargetKey) = " + wk_tableName + ".SystemNameCode ");
                                sb.AppendLine("	AND " + wk_tableName + ".SystemNameKbn = " + ((int)functionKeyInfo.SystemNameCode).ToString() + " ");
                                break;
                            case DefaultProperty.HaishokuTableKbn.Car:
                            case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                            case DefaultProperty.HaishokuTableKbn.Staff:
                                wk_tableName = tableName + "_" + ((int)functionKeyInfo.SystemNameCode).ToString();
                                sb.AppendLine("LEFT OUTER JOIN ");
                                sb.AppendLine("	" + tableName + " " + wk_tableName + " ");
                                sb.AppendLine("	ON  HaishokuEx.TableKey = " + ((int)tableKbn).ToString() + " ");
                                sb.AppendLine("	AND HaishokuEx.FunctionKey = " + ((int)functionKeyInfo.SystemNameCode).ToString() + " ");
                                sb.AppendLine("	AND " + wk_tableName + "." + name + "Id = HaishokuEx.TargetKey ");
                                sb.AppendLine("	AND " + wk_tableName + ".DelFlag = " + NSKUtil.BoolToInt(false) + " ");
                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            sb.AppendLine("WHERE ");
            sb.AppendLine(" HaishokuEx.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HaishokuId.HasValue)
                {
                    sb.AppendLine("AND HaishokuEx.HaishokuId = " + para.HaishokuId.ToString() + " ");
                }
                if (para.TableKey.HasValue)
                {
                    sb.AppendLine("AND HaishokuEx.TableKey = " + para.TableKey.ToString() + " ");
                }
                if (para.FunctionKey.HasValue)
                {
                    sb.AppendLine("AND HaishokuEx.FunctionKey = " + para.FunctionKey.ToString() + " ");
                }
                if (para.TargetKey.HasValue)
                {
                    sb.AppendLine("AND HaishokuEx.TargetKey = " + para.TargetKey.ToString() + " ");
                }
                if (para.GetDisableDataFlag.HasValue)
                {
                    if (!para.GetDisableDataFlag.Value)
                    {
                        sb.AppendLine("AND HaishokuEx.DisableFlag = 0 ");
                    }
                }
                else
                {
                    sb.AppendLine("AND HaishokuEx.DisableFlag = 0 ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" HaishokuEx.TableKey, HaishokuEx.FunctionKey, HaishokuEx.TargetKey ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HaishokuExInfo rt_info = new HaishokuExInfo
                {
                    HaishokuId = SQLServerUtil.dbDecimal(rdr["HaishokuId"]),
                    TableKey = SQLServerUtil.dbInt(rdr["TableKey"]),
                    FunctionKey = SQLServerUtil.dbInt(rdr["FunctionKey"]),
                    TargetKey = SQLServerUtil.dbDecimal(rdr["TargetKey"]),
                    ForeColor = SQLHelper.dbNullableInt(rdr["ForeColor"]),
                    BackColor = SQLHelper.dbNullableInt(rdr["BackColor"]),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),
                };

                switch ((DefaultProperty.HaishokuTableKbn)(rt_info.TableKey))
                {
                    case DefaultProperty.HaishokuTableKbn.SystemName:
                        rt_info.TargetKeyCode = SQLServerUtil.dbInt(rdr["TargetKeyCode_" + SQLServerUtil.dbInt(rdr["FunctionKey"]).ToString()]);
                        rt_info.TargetKeyName = rdr["TargetKeyName_" + SQLServerUtil.dbInt(rdr["FunctionKey"]).ToString()].ToString();
                        break;
                    case DefaultProperty.HaishokuTableKbn.Car:
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    case DefaultProperty.HaishokuTableKbn.Staff:
                        rt_info.TargetKeyCode = SQLServerUtil.dbInt(rdr["TargetKeyCode_" + rt_info.TableKey.ToString().ToString()]);
                        rt_info.TargetKeyName = rdr["TargetKeyName_" + rt_info.TableKey.ToString()].ToString();
                        break;
                    default:
                        break;
                }

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="tableKeyInfo">テーブルキー検索条件情報</param>
        /// <param name="functionKeyInfo">機能キー検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<TargetKeyInfo> GetTargetKeyListInternal(SqlTransaction transaction, SystemNameInfo tableKeyInfo, SystemNameInfo functionKeyInfo)
        {
            //返却用のリスト
            List<TargetKeyInfo> rt_list = new List<TargetKeyInfo>();

            string name = Enum.GetName(typeof(DefaultProperty.HaishokuTableKbn), (DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode);
            string tableName = DefaultProperty.GetHaishokuTableKbnTableMeisho((DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode);

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 " + tableKeyInfo.SystemNameCode.ToString() + " AS TableKey ");
            sb.AppendLine("	," + functionKeyInfo.SystemNameCode.ToString() + " AS FunctionKey ");
            switch ((DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    sb.AppendLine("	,SystemName.SystemNameCode AS TargetKey ");
                    sb.AppendLine("	,SystemName.SystemNameCode AS TargetKeyCode ");
                    sb.AppendLine("	,SystemName.SystemNameName AS TargetKeyName ");
                    sb.AppendLine("	,0 AS DisableFlag ");
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                    sb.AppendLine("	," + tableName + "." + name + "Id AS TargetKey ");
                    sb.AppendLine("	," + tableName + "." + name + "Cd AS TargetKeyCode ");
                    sb.AppendLine("	," + tableName + ".LicPlateCarNo AS TargetKeyName ");
                    sb.AppendLine("	," + tableName + ".DisableFlag AS DisableFlag ");
                    break;
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                case DefaultProperty.HaishokuTableKbn.Staff:
                    sb.AppendLine("	," + tableName + "." + name + "Id AS TargetKey ");
                    sb.AppendLine("	," + tableName + "." + name + "Cd AS TargetKeyCode ");
                    sb.AppendLine("	," + tableName + "." + name + "NM AS TargetKeyName ");
                    sb.AppendLine("	," + tableName + ".DisableFlag AS DisableFlag ");
                    break;
                default:
                    break;
            }
            sb.AppendLine("FROM ");
            switch ((DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    sb.AppendLine("	SystemName ");
                    break;
                default:
                    sb.AppendLine("	" + tableName + " ");
                    break;
            }

            sb.AppendLine("WHERE ");
            switch ((DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    sb.AppendLine(" SystemName.SystemNameKbn = " + functionKeyInfo.SystemNameCode.ToString() + " ");
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                case DefaultProperty.HaishokuTableKbn.Staff:
                    sb.AppendLine("	" + tableName + ".DelFlag = 0 ");
                    break;
                default:
                    break;
            }

            sb.AppendLine("ORDER BY ");
            switch ((DefaultProperty.HaishokuTableKbn)tableKeyInfo.SystemNameCode)
            {
                case DefaultProperty.HaishokuTableKbn.SystemName:
                    sb.AppendLine("	SystemName.SystemNameCode ");
                    break;
                case DefaultProperty.HaishokuTableKbn.Car:
                case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                case DefaultProperty.HaishokuTableKbn.Staff:
                    sb.AppendLine("	" + tableName + "." + name + "Cd ");
                    break;
                default:
                    break;
            }

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                TargetKeyInfo rt_info = new TargetKeyInfo
                {
                    TableKey = SQLServerUtil.dbInt(rdr["TableKey"]),
                    FunctionKey = SQLServerUtil.dbInt(rdr["FunctionKey"]),
                    TargetKeyCode = SQLServerUtil.dbInt(rdr["TargetKeyCode"]),
                    TargetKeyName = rdr["TargetKeyName"].ToString(),
                };

                //対象キー取得
                decimal targetKey = 0;

                switch ((DefaultProperty.HaishokuTableKbn)(rt_info.TableKey))
                {
                    case DefaultProperty.HaishokuTableKbn.SystemName:
                        targetKey = Convert.ToDecimal(SQLServerUtil.dbInt(rdr["TargetKey"]));
                        break;
                    case DefaultProperty.HaishokuTableKbn.Car:
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    case DefaultProperty.HaishokuTableKbn.Staff:
                        targetKey = SQLServerUtil.dbDecimal(rdr["TargetKey"]);
                        break;
                    default:
                        break;
                }

                rt_info.TargetKey = targetKey;

                //非表示フラグ
                bool disableFlag = false;

                switch ((DefaultProperty.HaishokuTableKbn)(rt_info.TableKey))
                {
                    case DefaultProperty.HaishokuTableKbn.Car:
                    case DefaultProperty.HaishokuTableKbn.Tokuisaki:
                    case DefaultProperty.HaishokuTableKbn.Staff:
                        disableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]));
                        break;
                    default:
                        break;
                }

                rt_info.DisableFlag = disableFlag;

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(string key, string id)
        {
            return "SELECT 1 FROM Haishoku WHERE DelFlag = 0 " + "AND TableKey = N'" + SQLHelper.GetSanitaizingSqlString(key) + "' "
                                                               + "AND TableId = N'" + SQLHelper.GetSanitaizingSqlString(id) + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// 指定したシステムコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string CreateSystemCodeCheckSQL(string kbn, string id)
        {
            return "SELECT 1 FROM Haishoku WHERE DelFlag = 0 " + "AND SystemKbn = N'" + SQLHelper.GetSanitaizingSqlString(kbn) + "' "
                                                               + "AND SystemId = N'" + SQLHelper.GetSanitaizingSqlString(id) + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="tablekey"></param>
        /// <param name="functionKey"></param>
        /// <param name="targetKey"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQLEx(int tablekey, int functionKey, decimal targetKey)
        {
            return "SELECT 1 FROM HaishokuEx WHERE DelFlag = 0 " + "AND TableKey = " + tablekey.ToString() + " "
                                                               + "AND FunctionKey = " + functionKey.ToString() + " "
                                                               + "AND TargetKey = " + targetKey.ToString() + " HAVING Count(*) > 1 ";
        }

        #endregion
    }
}
