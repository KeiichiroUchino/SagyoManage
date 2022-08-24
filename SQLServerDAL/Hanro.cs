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
    /// 販路テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Hanro
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
        private string _tableName = "販路";

        /// <summary>
        /// Hanroクラスのデフォルトコンストラクタです。
        /// </summary>
        public Hanro()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、販路テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Hanro(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で販路情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public HanroInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HanroSearchParameter()
            {
                HanroCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定で販路情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public HanroInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new HanroSearchParameter()
            {
                HanroId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、販路情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路情報のリスト</returns>
        public IList<HanroInfo> GetList(HanroSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// SqlTransaction情報、販路情報を指定して、
        /// 販路情報を更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路情報</param>
        public void Save(SqlTransaction transaction, HanroInfo info)
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
                sb.AppendLine(" Hanro ");
                sb.AppendLine("SET ");
                sb.AppendLine("  HanroCode = " + info.HanroCode.ToString() + " ");
                sb.AppendLine(" ,HanroName = N'" + SQLHelper.GetSanitaizingSqlString(info.HanroName).Trim() + "'");
                sb.AppendLine(" ,HanroNameKana = N'" + SQLHelper.GetSanitaizingSqlString(info.HanroNameKana).Trim() + "'");
                sb.AppendLine(" ,HanroSName = N'" + SQLHelper.GetSanitaizingSqlString(info.HanroSName).Trim() + "'");
                sb.AppendLine(" ,HanroSSName = N'" + SQLHelper.GetSanitaizingSqlString(info.HanroSSName).Trim() + "'");
                sb.AppendLine(" ,ToraDONTokuisakiId = " + info.ToraDONTokuisakiId.ToString() + " ");
                sb.AppendLine(" ,ToraDONHatchiId = " + info.ToraDONHatchiId.ToString() + " ");
                sb.AppendLine(" ,ToraDONChakuchiId = " + info.ToraDONChakuchiId.ToString() + " ");
                sb.AppendLine(" ,ToraDONItemId = " + info.ToraDONItemId.ToString() + " ");
                sb.AppendLine(" ,ToraDONCarKindId = " + info.ToraDONCarKindId.ToString() + " ");
                sb.AppendLine(" ,ToraDONCarId = " + info.ToraDONCarId.ToString() + " ");
                sb.AppendLine(" ,ToraDONTorihikiId = " + info.ToraDONTorihikiId.ToString() + " ");
                sb.AppendLine(" ,OfukuKbn = " + info.OfukuKbn.ToString() + " ");
                sb.AppendLine(" ,KoteiNissu = " + info.KoteiNissu.ToString() + " ");
                sb.AppendLine(" ,KoteiJikan = " + info.KoteiJikan.ToString() + " ");
                sb.AppendLine(" ,ReuseNissu = " + info.ReuseNissu.ToString() + " ");
                sb.AppendLine(" ,ReuseJikan = " + info.ReuseJikan.ToString() + " ");
                sb.AppendLine(" ,SeikyuTanka = " + info.SeikyuTanka.ToString() + " ");
                sb.AppendLine(" ,YoshaKingaku = " + info.YoshaKingaku.ToString() + " ");
                sb.AppendLine(" ,Futaigyomuryo = " + info.Futaigyomuryo.ToString() + " ");

                sb.AppendLine(",DisableFlag = " + NSKUtil.BoolToInt(info.DisableFlag).ToString() + "");
                sb.AppendLine(",DelFlag = " + NSKUtil.BoolToInt(info.DelFlag).ToString() + "");
                sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                sb.AppendLine("WHERE ");
                sb.AppendLine("HanroId = " + info.HanroId.ToString() + " ");
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
                    SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.HanroMst);

                //Insert文を作成
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("INSERT INTO ");
                sb.AppendLine(" Hanro ");
                sb.AppendLine(" ( ");
                sb.AppendLine("	 HanroId ");
                sb.AppendLine(" ,HanroCode ");
                sb.AppendLine(" ,HanroName ");
                sb.AppendLine(" ,HanroNameKana ");
                sb.AppendLine(" ,HanroSName ");
                sb.AppendLine(" ,HanroSSName ");
                sb.AppendLine(" ,ToraDONTokuisakiId ");
                sb.AppendLine(" ,ToraDONHatchiId ");
                sb.AppendLine(" ,ToraDONChakuchiId ");
                sb.AppendLine(" ,ToraDONItemId ");
                sb.AppendLine(" ,ToraDONCarKindId ");
                sb.AppendLine(" ,ToraDONCarId ");
                sb.AppendLine(" ,ToraDONTorihikiId ");
                sb.AppendLine("	,OfukuKbn ");
                sb.AppendLine("	,KoteiNissu ");
                sb.AppendLine("	,KoteiJikan ");
                sb.AppendLine("	,ReuseNissu ");
                sb.AppendLine("	,ReuseJikan ");
                sb.AppendLine("	,SeikyuTanka ");
                sb.AppendLine("	,YoshaKingaku ");
                sb.AppendLine("	,Futaigyomuryo ");

                sb.AppendLine("	,DisableFlag ");
                sb.AppendLine("	,DelFlag ");
                sb.AppendLine(" ," + SQLHelper.GetPopulateColumnSelectionString(popOption));

                sb.AppendLine(" ) ");
                sb.AppendLine("VALUES ");
                sb.AppendLine("( ");
                sb.AppendLine("" + newId.ToString() + "");
                sb.AppendLine(", " + info.HanroCode.ToString() + " ");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HanroName).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HanroNameKana).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HanroSName).Trim() + "'");
                sb.AppendLine(",N'" + SQLHelper.GetSanitaizingSqlString(info.HanroSSName).Trim() + "'");
                sb.AppendLine(", " + info.ToraDONTokuisakiId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONHatchiId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONChakuchiId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONItemId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONCarKindId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONCarId.ToString() + " ");
                sb.AppendLine(", " + info.ToraDONTorihikiId.ToString() + " ");
                sb.AppendLine(", " + info.OfukuKbn.ToString() + " ");
                sb.AppendLine(", " + info.KoteiNissu.ToString() + " ");
                sb.AppendLine(", " + info.KoteiJikan.ToString() + " ");
                sb.AppendLine(", " + info.ReuseNissu.ToString() + " ");
                sb.AppendLine(", " + info.ReuseJikan.ToString() + " ");
                sb.AppendLine(", " + info.SeikyuTanka.ToString() + " ");
                sb.AppendLine(", " + info.YoshaKingaku.ToString() + " ");
                sb.AppendLine(", " + info.Futaigyomuryo.ToString() + " ");

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
                if (SQLHelper.RecordExists(CreateCodeCheckSQL(info.HanroCode.ToString()),
                    transaction))
                {
                    throw new Model.DALExceptions.UniqueConstraintException(
                        string.Format("同じ{0}コードが既に登録されています。"
                                , this._tableName));
                }
            }
        }

        /// <summary>
        ///  SqlTransaction情報、販路情報を指定して、
        ///  販路情報を削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路情報</param>
        public void Delete(SqlTransaction transaction, HanroInfo info)
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
            sb.AppendLine(" Hanro ");
            sb.AppendLine("SET ");
            sb.AppendLine("	DelFlag = " + NSKUtil.BoolToInt(true) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HanroId = " + info.HanroId.ToString() + " ");
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
        /// SqlTransaction情報、検索条件情報を指定して、販路テーブルの件数を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路テーブルの件数</returns>
        public int GetKenSuu(SqlTransaction transaction, HanroSearchParameter para)
        {
            return this.GetListInternal(transaction, para).Count;
        }

        /// <summary>
        /// SqlTransaction情報、販路情報、検索条件情報を指定して、
        /// 販路情報のコードを変更します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">販路情報</param>
        /// <param name="para">検索条件情報</param>
        public void ChangeCode(SqlTransaction transaction, HanroInfo info, int newCode)
        {
            //「新コード」を指定して、存在テーブルの件数取得
            if (new Hanro().GetKenSuu(transaction, new HanroSearchParameter() { HanroCode = newCode }) != 0)
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
            sb.AppendLine(" Hanro ");
            sb.AppendLine("SET ");
            sb.AppendLine("	HanroCode = " + SQLHelper.GetSanitaizingSqlString(newCode.ToString()) + " ");
            sb.AppendLine("	," + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" HanroId = " + info.HanroId.ToString() + " ");
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

        /// <summary>
        /// 販路コードの欠番を取得します。
        /// </summary>
        /// <returns></returns>
        public int GetNextCode()
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 MIN(HanroCode + 1) AS HanroCode ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	 (SELECT HanroCode FROM Hanro WHERE DelFlag = 0 UNION SELECT 0) Hanro ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	 (HanroCode + 1) NOT IN (SELECT HanroCode FROM Hanro WHERE DelFlag = 0 UNION SELECT 0)  ");

            return SQLHelper.SimpleReadSingle(sb.ToString(), rdr => SQLServerUtil.dbInt(rdr["HanroCode"]));
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<HanroInfo> GetListInternal(SqlTransaction transaction, HanroSearchParameter para)
        {
            //返却用のリスト
            List<HanroInfo> rt_list = new List<HanroInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Hanro.* ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiId, 0) JoinToraDONTokuisakiId ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiCd, 0) ToraDONTokuisakiCode ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiNM, '') ToraDONTokuisakiName ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiSNM, '') ToraDONTokuisakiShortName ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.MemoAccount, 0) ToraDONTokuisakiMemoAccount ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.ClmClassUseKbn, 0) ToraDONTokuisakiClmClassUseKbn ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.GakCutKbn, 0) ToraDONTokuisakiGakCutKbn ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.SaleSlipToClmDayKbn, 0) ToraDONTokuisakiSaleSlipToClmDayKbn ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.DisableFlag, 0) ToraDONTokuisakiDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointId, 0) JoinToraDONHatchiId ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointCd, 0) ToraDONHatchiCode ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointNM, '') ToraDONHatchiName ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.DisableFlag, 0) ToraDONHatchiDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointId, 0) JoinToraDONChakuchiId ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointCd, 0) ToraDONChakuchiCode ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointNM, '') ToraDONChakuchiName ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.DisableFlag, 0) ToraDONChakuchiDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemId, 0) JoinToraDONItemId ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemCd, 0) ToraDONItemCode ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemNM, '') ToraDONItemName ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemTaxKbn, 0) ToraDONItemItemTaxKbn ");
            sb.AppendLine("	,ISNULL(ToraDONItem.DisableFlag, 0) ToraDONItemDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigId, 0) JoinToraDONFigId ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigCd, 0) ToraDONFigCode ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigNM, '') ToraDONFigName ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindId, 0) JoinToraDONCarKindId ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindCd, 0) ToraDONCarKindCode ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindNM, '') ToraDONCarKindName ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.DisableFlag, 0) ToraDONCarKindDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONCar.CarId, 0) JoinToraDONCarId ");
            sb.AppendLine("	,ISNULL(ToraDONCar.CarCd, 0) ToraDONCarCode ");
            sb.AppendLine("	,ISNULL(ToraDONCar.LicPlateCarNo, '') ToraDONCarName ");
            sb.AppendLine("	,ISNULL(ToraDONCar.CarKbn, 0) ToraDONCarKbn ");
            sb.AppendLine("	,ISNULL(ToraDONCar.DisableFlag, 0) ToraDONCarDisableFlag ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiId, 0) JoinToraDONTorihikiId ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiCd, 0) ToraDONTorihikiCode ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiNM, '') ToraDONTorihikiName ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiSNM, '') ToraDONTorihikiShortName ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.DisableFlag, 0) ToraDONTorihikiDisableFlag ");
            sb.AppendLine("	,ISNULL(SystemName.SystemNameName, '') OfukuKbnName ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	Hanro ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Tokuisaki AS ToraDONTokuisaki ");
            sb.AppendLine("ON ToraDONTokuisaki.TokuisakiId = Hanro.ToraDONTokuisakiId ");
            sb.AppendLine("AND ToraDONTokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Point AS ToraDONHatchi ");
            sb.AppendLine("ON ToraDONHatchi.PointId = Hanro.ToraDONHatchiId ");
            sb.AppendLine("AND ToraDONHatchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Point AS ToraDONChakuchi ");
            sb.AppendLine("ON ToraDONChakuchi.PointId = Hanro.ToraDONChakuchiId ");
            sb.AppendLine("AND ToraDONChakuchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Item AS ToraDONItem ");
            sb.AppendLine("ON ToraDONItem.ItemId = Hanro.ToraDONItemId ");
            sb.AppendLine("AND ToraDONItem.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Fig AS ToraDONFig ");
            sb.AppendLine("ON ToraDONFig.FigId = ToraDONItem.FigId ");
            sb.AppendLine("AND ToraDONFig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_CarKind AS ToraDONCarKind ");
            sb.AppendLine("ON ToraDONCarKind.CarKindId = Hanro.ToraDONCarKindId ");
            sb.AppendLine("AND ToraDONCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Car AS ToraDONCar ");
            sb.AppendLine("ON ToraDONCar.CarId = Hanro.ToraDONCarId ");
            sb.AppendLine("AND ToraDONCar.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Torihikisaki AS ToraDONTorihikisaki ");
            sb.AppendLine("ON ToraDONTorihikisaki.TorihikiId = Hanro.ToraDONTorihikiId ");
            sb.AppendLine("AND ToraDONTorihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	SystemName ");
            sb.AppendLine("ON  SystemName.SystemNameKbn =  " + (int)DefaultProperty.SystemNameKbn.OfukuKbn);
            sb.AppendLine("AND SystemName.SystemNameCode = Hanro.OfukuKbn ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(" Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HanroId.HasValue)
                {
                    sb.AppendLine("AND Hanro.HanroId = " + para.HanroId.ToString() + " ");
                }
                if (para.HanroCode.HasValue)
                {
                    sb.AppendLine("AND Hanro.HanroCode = " + para.HanroCode.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("  Hanro.HanroCode  ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HanroInfo rt_info = new HanroInfo
                {
                    HanroId = SQLServerUtil.dbDecimal(rdr["HanroId"]),
                    HanroCode = SQLServerUtil.dbInt(rdr["HanroCode"]),
                    HanroName = rdr["HanroName"].ToString(),
                    HanroNameKana = rdr["HanroNameKana"].ToString(),
                    HanroSName = rdr["HanroSName"].ToString(),
                    HanroSSName = rdr["HanroSSName"].ToString(),
                    ToraDONTokuisakiId = SQLServerUtil.dbDecimal(rdr["ToraDONTokuisakiId"]),
                    ToraDONHatchiId = SQLServerUtil.dbDecimal(rdr["ToraDONHatchiId"]),
                    ToraDONChakuchiId = SQLServerUtil.dbDecimal(rdr["ToraDONChakuchiId"]),
                    ToraDONItemId = SQLServerUtil.dbDecimal(rdr["ToraDONItemId"]),
                    ToraDONCarKindId = SQLServerUtil.dbDecimal(rdr["ToraDONCarKindId"]),
                    ToraDONCarId = SQLServerUtil.dbDecimal(rdr["ToraDONCarId"]),
                    ToraDONTorihikiId = SQLServerUtil.dbDecimal(rdr["ToraDONTorihikiId"]),
                    OfukuKbn = SQLServerUtil.dbInt(rdr["OfukuKbn"]),
                    KoteiNissu = SQLServerUtil.dbInt(rdr["KoteiNissu"]),
                    KoteiJikan = SQLServerUtil.dbInt(rdr["KoteiJikan"]),
                    ReuseNissu = SQLServerUtil.dbInt(rdr["ReuseNissu"]),
                    ReuseJikan = SQLServerUtil.dbInt(rdr["ReuseJikan"]),
                    SeikyuTanka = SQLServerUtil.dbDecimal(rdr["SeikyuTanka"]),
                    YoshaKingaku = SQLServerUtil.dbDecimal(rdr["YoshaKingaku"]),
                    Futaigyomuryo = SQLServerUtil.dbDecimal(rdr["Futaigyomuryo"]),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                    DelFlag = SQLHelper.dbBit(rdr["DelFlag"]),

                    JoinToraDONTokuisakiId = SQLServerUtil.dbDecimal(rdr["JoinToraDONTokuisakiId"]),
                    ToraDONTokuisakiCode = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiCode"]),
                    ToraDONTokuisakiName = rdr["ToraDONTokuisakiName"].ToString(),
                    ToraDONTokuisakiShortName = rdr["ToraDONTokuisakiShortName"].ToString(),
                    ToraDONTokuisakiMemoAccount = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiMemoAccount"]),
                    ToraDONTokuisakiClmClassUseKbn = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiClmClassUseKbn"]),
                    ToraDONTokuisakiGakCutKbn = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiGakCutKbn"]),
                    ToraDONTokuisakiSaleSlipToClmDayKbn = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiSaleSlipToClmDayKbn"]),
                    ToraDONTokuisakiDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONTokuisakiDisableFlag"])),
                    JoinToraDONHatchiId = SQLServerUtil.dbDecimal(rdr["JoinToraDONHatchiId"]),
                    ToraDONHatchiCode = SQLServerUtil.dbInt(rdr["ToraDONHatchiCode"]),
                    ToraDONHatchiName = rdr["ToraDONHatchiName"].ToString(),
                    ToraDONHatchiDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONHatchiDisableFlag"])),
                    JoinToraDONChakuchiId = SQLServerUtil.dbDecimal(rdr["JoinToraDONChakuchiId"]),
                    ToraDONChakuchiCode = SQLServerUtil.dbInt(rdr["ToraDONChakuchiCode"]),
                    ToraDONChakuchiName = rdr["ToraDONChakuchiName"].ToString(),
                    ToraDONChakuchiDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONChakuchiDisableFlag"])),
                    JoinToraDONItemId = SQLServerUtil.dbDecimal(rdr["JoinToraDONItemId"]),
                    ToraDONItemCode = SQLServerUtil.dbInt(rdr["ToraDONItemCode"]),
                    ToraDONItemName = rdr["ToraDONItemName"].ToString(),
                    ToraDONItemItemTaxKbn = SQLServerUtil.dbInt(rdr["ToraDONItemItemTaxKbn"]),
                    ToraDONItemDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONItemDisableFlag"])),
                    JoinToraDONFigId = SQLServerUtil.dbDecimal(rdr["JoinToraDONFigId"]),
                    ToraDONFigCode = SQLServerUtil.dbInt(rdr["ToraDONFigCode"]),
                    ToraDONFigName = rdr["ToraDONFigName"].ToString(),
                    JoinToraDONCarKindId = SQLServerUtil.dbDecimal(rdr["JoinToraDONCarKindId"]),
                    ToraDONCarKindCode = SQLServerUtil.dbInt(rdr["ToraDONCarKindCode"]),
                    ToraDONCarKindName = rdr["ToraDONCarKindName"].ToString(),
                    ToraDONCarKindDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONCarKindDisableFlag"])),
                    JoinToraDONCarId = SQLServerUtil.dbDecimal(rdr["JoinToraDONCarId"]),
                    ToraDONCarCode = SQLServerUtil.dbInt(rdr["ToraDONCarCode"]),
                    ToraDONCarName = rdr["ToraDONCarName"].ToString(),
                    ToraDONCarKbn = SQLServerUtil.dbInt(rdr["ToraDONCarKbn"]),
                    ToraDONCarDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONCarDisableFlag"])),
                    JoinToraDONTorihikiId = SQLServerUtil.dbDecimal(rdr["JoinToraDONTorihikiId"]),
                    ToraDONTorihikiCode = SQLServerUtil.dbInt(rdr["ToraDONTorihikiCode"]),
                    ToraDONTorihikiName = rdr["ToraDONTorihikiName"].ToString(),
                    ToraDONTorihikiShortName = rdr["ToraDONTorihikiShortName"].ToString(),
                    ToraDONTorihikiDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONTorihikiDisableFlag"])),
                    OfukuKbnName = rdr["OfukuKbnName"].ToString(),
                };

                if (0 < rt_info.HanroCode)
                {
                    //入力者以下の常設フィールドをセットする
                    rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);
                }

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
            return "SELECT 1 FROM Hanro WHERE DelFlag = 0 " + "AND HanroCode = N'" + SQLHelper.GetSanitaizingSqlString(code.ToString().Trim()) + "'  HAVING Count(*) > 1 ";
        }

        /// <summary>
        /// 指定したコードを参照しているレコードが存在するテーブル名を返します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info"></param>
        /// <returns></returns>
        private IList<string> GetReferenceTables(SqlTransaction transaction, HanroInfo info)
        {
            List<string> list = new List<string>();

            string HanroIdToString = info.HanroId.ToString();

            // 受注トラン
            if (SQLHelper.RecordExists("SELECT 1 FROM Juchu WHERE DelFlag = 0 AND Juchu.HanroId = " + HanroIdToString, transaction))
            {
                list.Add("受注");
            }

            // 重複を除いて返却
            return list.Distinct().ToList();
        }

        #endregion
    }
}
