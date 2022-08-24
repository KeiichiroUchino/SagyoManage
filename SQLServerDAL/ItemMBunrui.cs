using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 品目中分類テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ItemMBunrui
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
        private string _tableName = "品目中分類";

         /// <summary>
        /// 品目中分類クラスのデフォルトコンストラクタです。
        /// </summary>
        public ItemMBunrui()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、品目中分類テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ItemMBunrui(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="code"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public ItemMBunruiInfo GetInfo(decimal lid, string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemMBunruiSearchParameter()
            {
                ItemLBunruiId = lid,
                ItemMBunruiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で品目中分類情報を取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public ItemMBunruiInfo GetInfoById(decimal lid, decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemMBunruiSearchParameter()
            {
                ItemLBunruiId = lid,
                ItemMBunruiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、品目中分類情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目中分類情報のリスト</returns>
        public IList<ItemMBunruiInfo> GetList(ItemMBunruiSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// 品目中分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="lid"></param>
        /// <returns>品目中分類情報のコンボボックス用リスト</returns>
        public IList<ItemMBunruiInfo> GetComboList(decimal lid)
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、品目中分類情報を指定して、
        /// 品目中分類情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目中分類情報</param>
        public void Save(SqlTransaction transaction, ItemMBunruiInfo info)
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
                sb.AppendLine(" ItemMBunrui ");
                sb.AppendLine("SET ");
                sb.AppendLine(" ItemMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemMBunruiCode).Trim() + "' ");
                sb.AppendLine(",ItemMBunruiName = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemMBunruiName).Trim() + "' ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + " ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
                sb.AppendLine("AND ");
                sb.AppendLine("ItemMBunruiId = " + info.ItemMBunruiId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.ItemMBunruiMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" ItemMBunrui ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ItemLBunruiId ");
                sb.AppendLine(" ,ItemMBunruiId ");
                sb.AppendLine(" ,ItemMBunruiCode ");
                sb.AppendLine(" ,ItemMBunruiName ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("  " + info.ItemLBunruiId.ToString() + " ");
                sb.AppendLine(", " + newId.ToString() + "");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemMBunruiCode).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemMBunruiName).Trim() + "' ");

                sb.AppendLine("," + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine("," + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));

                sb.AppendLine(") ");

                string sql = sb.ToString();
                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));

                //コードの存在チェック
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.ItemLBunruiId, info.ItemMBunruiCode),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                    , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、品目中分類情報を指定して、
        ///  品目中分類情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目中分類情報</param>
        public void Delete(SqlTransaction transaction, ItemMBunruiInfo info)
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
            sb.AppendLine(" ItemMBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine(" ItemMBunruiId = " + info.ItemMBunruiId.ToString() + " ");
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

            // 他のテーブルに使用されていないか？
            IList<string> list = GetReferenceTables(transaction, info);

            if (list.Count > 0)
            {
                StringBuilder sb_table = new StringBuilder();

                foreach (string table in list)
                {
                    sb_table.AppendLine(table);
                }

                // リトライ可能な例外
                SQLHelper.ThrowCanNotDeleteException(sb_table.ToString());
            }

            #endregion
        }

        /// <summary>
        /// SqlTransaction情報、品目中分類情報、検索条件情報を指定して、
        /// 品目中分類情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目中分類情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, ItemMBunruiInfo info, string newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new ItemMBunrui().GetKenSuu(transaction, new ItemMBunruiSearchParameter() { ItemLBunruiId = info.ItemLBunruiId, ItemMBunruiCode = newCode }) != 0)
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
            sb.AppendLine(" ItemMBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	ItemMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + "' ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine(" ItemMBunruiId = " + info.ItemMBunruiId.ToString() + " ");
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
        private IList<ItemMBunruiInfo> GetListInternal(SqlTransaction transaction, ItemMBunruiSearchParameter para)
        {
            //返却用のリスト
            List<ItemMBunruiInfo> rt_list = new List<ItemMBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("	,ItemLBunrui.ItemLBunruiCode ItemLBunruiCode ");
            sb.AppendLine("	,ItemLBunrui.ItemLBunruiName ItemLBunruiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" ItemMBunrui ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" ItemLBunrui ");
            sb.AppendLine(" ON  ItemLBunrui.ItemLBunruiId = ItemMBunrui.ItemLBunruiId ");
            sb.AppendLine(" AND ItemLBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ItemMBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ItemLBunruiId.HasValue)
                {
                    sb.AppendLine("AND ItemMBunrui.ItemLBunruiId = " + para.ItemLBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.ItemLBunruiCode))
                {
                    sb.AppendLine("AND ItemLBunrui.ItemLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.ItemLBunruiCode) + "' ");
                }
                if (para.ItemMBunruiId.HasValue)
                {
                    sb.AppendLine("AND ItemMBunrui.ItemMBunruiId = " + para.ItemMBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.ItemMBunruiCode))
                {
                    sb.AppendLine("AND ItemMBunrui.ItemMBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.ItemMBunruiCode) + "' ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ItemMBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ItemMBunruiInfo rt_info = new ItemMBunruiInfo
                {
                    ItemLBunruiId = SQLServerUtil.dbDecimal(rdr["ItemLBunruiId"]),
                    ItemMBunruiId = SQLServerUtil.dbDecimal(rdr["ItemMBunruiId"]),
                    ItemMBunruiCode = rdr["ItemMBunruiCode"].ToString(),
                    ItemMBunruiName = rdr["ItemMBunruiName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    ItemLBunruiCode = rdr["ItemLBunruiCode"].ToString(),
                    ItemLBunruiName = rdr["ItemLBunruiName"].ToString(),
                };

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 品目中分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目中分類情報のコンボボックス用リスト</returns>
        public IList<ItemMBunruiInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ItemMBunruiInfo> rt_list = new List<ItemMBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	ItemMBunrui.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" ItemMBunrui ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	ItemMBunrui.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	ItemMBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ItemMBunrui.ItemMBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ItemMBunruiInfo rt_info = new ItemMBunruiInfo
                {
                    ItemLBunruiId = SQLServerUtil.dbDecimal(rdr["ItemLBunruiId"]),
                    ItemMBunruiId = SQLServerUtil.dbDecimal(rdr["ItemMBunruiId"]),
                    ItemMBunruiCode = rdr["ItemMBunruiCode"].ToString(),
                    ItemMBunruiName = rdr["ItemMBunruiName"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 指定したコードチェック用のSQLを作成します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private string CreateCodeCheckSQL(decimal lid, string code)
        {
            return "SELECT 1 FROM ItemMBunrui WHERE DelFlag = 0 " + "AND ItemLBunruiId = " + lid.ToString() + " AND ItemMBunruiCode = '" + code + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、品目中分類テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目中分類テーブルの件数</returns>
        private int GetKenSuu(SqlTransaction transaction, ItemMBunruiSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, ItemMBunruiInfo info)
        {
            List<string> list = new List<string>();

            string ItemMBunruiIdToString = info.ItemMBunruiId.ToString();

            // 品目マスタ
            if (SQLHelper.RecordExists("SELECT 1 FROM Item WHERE DelFlag = 0 AND Item.ItemMBunruiId = " + ItemMBunruiIdToString, transaction))
            {
                list.Add("品目マスタ");
            }

            // 重複を除いて返却
            return list.Distinct().ToList();
        }

        #endregion
    }
}
