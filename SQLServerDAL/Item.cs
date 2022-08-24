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
    /// 品目（トラDON補）テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Item
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
        /// 品目クラスのデフォルトコンストラクタです。
        /// </summary>
        public Item()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、品目テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Item(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で品目情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemSearchParameter()
            {
                ToraDONItemCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で品目情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ItemInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ItemSearchParameter()
            {
                ToraDONItemId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、品目情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>品目情報のリスト</returns>
        public IList<ItemInfo> GetList(ItemSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// SqlTransaction情報、品目情報を指定して、
        /// 品目情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目情報</param>
        public void Save(SqlTransaction transaction, ItemInfo info)
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
                sb.AppendLine(" Item ");
                sb.AppendLine("SET ");
                sb.AppendLine(" ItemSName = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemShortName.Trim()) + "'");
                sb.AppendLine(",ItemSSName = N'" + SQLHelper.GetSanitaizingSqlString(info.ItemSShortName.Trim()) + "'");
                //sb.AppendLine(",ItemGroupId = " + info.ItemGroupId.ToString() + " ");
                sb.AppendLine(",ItemLBunruiId = " + info.ItemLBunruiId.ToString() + " ");
                sb.AppendLine(",ItemMBunruiId = " + info.ItemMBunruiId.ToString() + " ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("ItemId = " + info.ItemId.ToString() + " ");
                sb.AppendLine("AND EXISTS ( ");
                sb.AppendLine("SELECT 1 FROM ");
                sb.AppendLine(" TORADON_Item ");
                sb.AppendLine("WHERE ");
                sb.AppendLine("ItemId = " + info.ToraDONItemId.ToString() + " ");
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
                //トラDONの品目存在チェック
                if (!SQLHelper.RecordExists(CreateCheckSQL(info.ToraDONItemId),
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.ItemMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Item ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 ItemId ");
                sb.AppendLine("	,ToraDONItemId ");
                sb.AppendLine("	,ItemSName ");
                sb.AppendLine("	,ItemSSName ");
                //sb.AppendLine("	,ItemGroupId ");
                sb.AppendLine("	,ItemLBunruiId ");
                sb.AppendLine("	,ItemMBunruiId ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine("," + info.ToraDONItemId.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemShortName.Trim()) + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.ItemSShortName.Trim()) + "'");
                //sb.AppendLine("," + info.ItemGroupId.ToString() + " ");
                sb.AppendLine("," + info.ItemMBunruiId.ToString() + " ");
                sb.AppendLine("," + info.ItemLBunruiId.ToString() + " ");

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
        }

        /// <summary>
        ///  SqlTransaction情報、品目情報を指定して、
        ///  品目情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">品目情報</param>
        public void Delete(SqlTransaction transaction, ItemInfo info)
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
            sb.AppendLine(" Item ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" ItemId = " + info.ItemId.ToString() + " ");
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
        /// SqlTransaction情報、検索条件情報を指定して、品目（トラDON補）テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>品目（トラDON補）テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, ItemSearchParameter para)
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
        public IList<ItemInfo> GetListInternal(SqlTransaction transaction, ItemSearchParameter para)
        {
            //返却用のリスト
            List<ItemInfo> rt_list = new List<ItemInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ToraDONItem.ItemId ToraDONItemId_Main ");
            sb.AppendLine("	,ToraDONItem.ItemCd ToraDONItemCd ");
            sb.AppendLine("	,ToraDONItem.ItemNM ToraDONItemNM ");
            sb.AppendLine("	,ToraDONItem.ItemNMK ToraDONItemNMK ");
            sb.AppendLine("	,ToraDONItem.DisableFlag ToraDONDisableFlag ");
            sb.AppendLine("	,ToraDONItem.Weight ToraDONWeight ");
            sb.AppendLine("	,ToraDONItem.FigId ToraDONFigId ");
            sb.AppendLine("	,ToraDONItem.ItemTaxKbn ToraDONItemTaxKbn ");
            sb.AppendLine("	,Item.* ");
            //sb.AppendLine("	,ItemGroup.ItemGroupCode ");
            //sb.AppendLine("	,ItemGroup.ItemGroupName ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Item AS ToraDONItem ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Item ");
            sb.AppendLine("ON Item.ToraDONItemId = ToraDONItem.ItemId ");
            sb.AppendLine("AND Item.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            //sb.AppendLine("LEFT OUTER JOIN ");
            //sb.AppendLine("	ItemGroup ");
            //sb.AppendLine("ON ItemGroup.ItemGroupId = Item.ItemGroupId ");
            //sb.AppendLine("AND ItemGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONItem.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ItemId.HasValue)
                {
                    sb.AppendLine("AND Item.ItemId = " + para.ItemId.ToString() + " ");
                }
                if (para.ToraDONItemId.HasValue)
                {
                    sb.AppendLine("AND ToraDONItem.ItemId = " + para.ToraDONItemId.ToString() + " ");
                }
                if (para.ToraDONItemCode.HasValue)
                {
                    sb.AppendLine("AND ToraDONItem.ItemCd = " + para.ToraDONItemCode.ToString() + " ");
                }
                //if (para.ItemGroupId.HasValue)
                //{
                //    sb.AppendLine("AND Item.ItemGroupId = " + para.ItemGroupId.ToString() + " ");
                //}
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ToraDONItemCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ItemInfo rt_info = new ItemInfo
                {
                    ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]),
                    ToraDONItemId = SQLServerUtil.dbDecimal(rdr["ToraDONItemId_Main"]),
                    ToraDONItemCode = SQLServerUtil.dbInt(rdr["ToraDONItemCd"]),
                    ToraDONItemName = rdr["ToraDONItemNM"].ToString(),
                    ToraDONItemNameKana = rdr["ToraDONItemNMK"].ToString(),
                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),
                    ItemShortName = rdr["ItemSName"].ToString(),
                    ItemSShortName = rdr["ItemSSName"].ToString(),
                    //ItemGroupId = SQLServerUtil.dbDecimal(rdr["ItemGroupId"]),
                    //ItemGroupCode = SQLServerUtil.dbInt(rdr["ItemGroupCode"]),
                    //ItemGroupName = rdr["ItemGroupName"].ToString(),
                    ItemLBunruiId = SQLServerUtil.dbDecimal(rdr["ItemLBunruiId"]),
                    ItemMBunruiId = SQLServerUtil.dbDecimal(rdr["ItemMBunruiId"]),
                    ToraDONWeight = SQLServerUtil.dbDecimal(rdr["ToraDONWeight"]),
                    ToraDONFigId = SQLServerUtil.dbDecimal(rdr["ToraDONFigId"]),
                    ToraDONItemTaxKbn = SQLServerUtil.dbInt(rdr["ToraDONItemTaxKbn"]),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"])
                };

                if (0 < rt_info.ItemId)
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
            sb.AppendLine(" TORADON_Item ");
            sb.AppendLine("WHERE DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND ItemId = " + id.ToString() + " ");
            return sb.ToString();
        }

        #endregion
    }
}
