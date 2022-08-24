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
    /// 品目大分類テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ItemLBunrui
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
        private string _tableName = "品目大分類";

        /// <summary>
        /// 品目中分類クラス
        /// </summary>
        private ItemMBunrui _ItemMBunrui = null;

         /// <summary>
        /// 品目大分類クラスのデフォルトコンストラクタです。
        /// </summary>
        public ItemLBunrui()
        {
            this._ItemMBunrui = new ItemMBunrui();
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、品目大分類テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ItemLBunrui(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
            this._ItemMBunrui = new ItemMBunrui(authInfo);
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public ItemLBunruiInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemLBunruiSearchParameter()
            {
                ItemLBunruiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で品目大分類情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public ItemLBunruiInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemLBunruiSearchParameter()
            {
                ItemLBunruiId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、品目大分類情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目大分類情報のリスト</returns>
        public IList<ItemLBunruiInfo> GetList(ItemLBunruiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 品目大分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目大分類情報のコンボボックス用リスト</returns>
        public IList<ItemLBunruiInfo> GetComboList()
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、品目大分類情報を指定して、
        /// 品目大分類情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目大分類情報</param>
        public void Save(SqlTransaction transaction, ItemLBunruiInfo info)
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
                sb.AppendLine(" ItemLBunrui ");
                sb.AppendLine("SET ");
                sb.AppendLine(" ItemLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemLBunruiCode).Trim() + "' ");
                sb.AppendLine(",ItemLBunruiName = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemLBunruiName).Trim() + "' ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + " ");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + " ");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.ItemLBunruiMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" ItemLBunrui ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ItemLBunruiId ");
                sb.AppendLine(" ,ItemLBunruiCode ");
                sb.AppendLine(" ,ItemLBunruiName ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemLBunruiCode).Trim() + "' ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemLBunruiName).Trim() + "' ");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.ItemLBunruiCode),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                    , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、品目大分類情報を指定して、
        ///  品目大分類情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目大分類情報</param>
        public void Delete(SqlTransaction transaction, ItemLBunruiInfo info)
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
            sb.AppendLine(" ItemLBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
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
        /// SqlTransaction情報、品目大分類情報、検索条件情報を指定して、
        /// 品目大分類情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目大分類情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, ItemLBunruiInfo info, string newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new ItemLBunrui().GetKenSuu(transaction, new ItemLBunruiSearchParameter() { ItemLBunruiCode = newCode }) != 0)
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
            sb.AppendLine(" ItemLBunrui ");
            sb.AppendLine("SET ");
            sb.AppendLine("	ItemLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + "' ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
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
        private IList<ItemLBunruiInfo> GetListInternal(SqlTransaction transaction, ItemLBunruiSearchParameter para, bool getMFlag = true)
        {
            //返却用のリスト
            List<ItemLBunruiInfo> rt_list = new List<ItemLBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" ItemLBunrui ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ItemLBunruiId.HasValue)
                {
                    sb.AppendLine("AND ItemLBunruiId = " + para.ItemLBunruiId.ToString() + " ");
                }
                if (!String.IsNullOrWhiteSpace(para.ItemLBunruiCode))
                {
                    sb.AppendLine("AND ItemLBunruiCode = N'" + SQLHelper.GetSanitaizingSqlString(para.ItemLBunruiCode) + "' ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ItemLBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ItemLBunruiInfo rt_info = new ItemLBunruiInfo
                {
                    ItemLBunruiId = SQLServerUtil.dbDecimal(rdr["ItemLBunruiId"]),
                    ItemLBunruiCode = rdr["ItemLBunruiCode"].ToString(),
                    ItemLBunruiName = rdr["ItemLBunruiName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (getMFlag)
                {
                    rt_info.ItemMBunruiList = this._ItemMBunrui.GetList(
                        new ItemMBunruiSearchParameter()
                    {
                        ItemLBunruiId = rt_info.ItemLBunruiId
                    }, transaction);
                }

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 品目大分類情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目大分類情報のコンボボックス用リスト</returns>
        public IList<ItemLBunruiInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ItemLBunruiInfo> rt_list = new List<ItemLBunruiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	ItemLBunrui.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" ItemLBunrui ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	ItemLBunrui.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	ItemLBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ItemLBunrui.ItemLBunruiCode ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ItemLBunruiInfo rt_info = new ItemLBunruiInfo
                {
                    ItemLBunruiId = SQLServerUtil.dbDecimal(rdr["ItemLBunruiId"]),
                    ItemLBunruiCode = rdr["ItemLBunruiCode"].ToString(),
                    ItemLBunruiName = rdr["ItemLBunruiName"].ToString(),
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
        private string CreateCodeCheckSQL(string code)
        {
            return "SELECT 1 FROM ItemLBunrui WHERE DelFlag = 0 " + "AND ItemLBunruiCode = '" + code + "' HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、品目大分類テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目大分類テーブルの件数</returns>
        private int GetKenSuu(SqlTransaction transaction, ItemLBunruiSearchParameter para)
        {
            return this.GetListInternal(transaction, para, false).Count;
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, ItemLBunruiInfo info)
        {
            List<string> list = new List<string>();

            string ItemLBunruiIdToString = info.ItemLBunruiId.ToString();

            // 品目中分類
            if (SQLHelper.RecordExists("SELECT 1 FROM ItemMBunrui WHERE DelFlag = 0 AND ItemMBunrui.ItemLBunruiId = " + ItemLBunruiIdToString, transaction))
            {
                list.Add("品目中分類マスタ");
            }

            // 品目
            if (SQLHelper.RecordExists("SELECT 1 FROM Item WHERE DelFlag = 0 AND Item.ItemLBunruiId = " + ItemLBunruiIdToString, transaction))
            {
                list.Add("品目マスタ");
            }

            // 重複を除いて返却
            return list.Distinct().ToList();
        }

        #endregion
    }
}
