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
using Jpsys.SagyoManage.BizProperty;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// トラDONマスタ同期のデータアクセスレイヤです。
    /// </summary>
    public class MergeToraDon
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
        /// 同期対象テーブル数
        /// </summary>
        public static int MARGETORADONTABLEKBN_TOTAL_COUNT = 18;

        /// <summary>
        /// MergeToraDonクラスのデフォルトコンストラクタです。
        /// </summary>
        public MergeToraDon()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、トラDON同期テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public MergeToraDon(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// テーブル指定でトラDON同期処理を行います。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public void MergeToraDonTable(int code, SqlTransaction transaction = null)
        {
            this.MergeToraDonTablesInternal(transaction, new List<int>() { code });
        }

        /// <summary>
        /// 複数テーブル指定でトラDON同期処理を行います。
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public void MergeToraDonTables(List<int> list, SqlTransaction transaction = null)
        {
            if (list == null || list.Count == 0)
            {
                return;
            }

            this.MergeToraDonTablesInternal(transaction, list);
        }

        /// <summary>
        /// すべてのテーブルのトラDON同期処理を行います。
        /// </summary>
        /// <returns></returns>
        public void MergeToraDonAllTables(SqlTransaction transaction = null)
        {
            List<int> list = new List<int>();

            for (int i = 0; i < MergeToraDon.MARGETORADONTABLEKBN_TOTAL_COUNT; i++)
            {
                list.Add(i + 1);
            }

            this.MergeToraDonTablesInternal(transaction, list);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、テーブル情報情報を指定して、トラDONのマスタ情報を同期します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="list">テーブル情報リスト</param>
        /// <returns>情報リスト</returns>
        public void MergeToraDonTablesInternal(SqlTransaction transaction, List<int> list)
        {
            List<string> mySqlList = new List<string>();

            foreach (int i in list)
            {
                try
                {
                    DefaultProperty.MergeToraDonTableKbn kbn = (DefaultProperty.MergeToraDonTableKbn)i;
                }
                //テーブル対象外の場合
                catch(Exception)
                {
                    //スキップ
                    continue;
                }

                switch ((DefaultProperty.MergeToraDonTableKbn)i)
                {
                    //トラDON_管理
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_SystemProperty:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeSystemPropertySql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeSystemPropertyUpdateSql());

                        break;

                    //トラDON_初期値
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_DefaultProperty:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeDefaultPropertySql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeDefaultPropertyUpdateSql());

                        break;

                    //トラDON_営業所
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_BranchOffice:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeBranchOfficeSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeBranchOfficeUpdateSql());

                        break;

                    //トラDON_得意先
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Tokuisaki:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeTokuisakiSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeTokuisakiUpdateSql());

                        break;

                    //トラDON_請求部門
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_ClmClass:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeClmClassSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeClmClassUpdateSql());

                        break;

                    //トラDON_取引先
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Torihikisaki:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeTorihikisakiSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeTorihikisakiUpdateSql());

                        break;

                    //トラDON_車両
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Car:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeCarSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeCarUpdateSql());

                        break;

                    //トラDON_車検証
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Inspect:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeInspectSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeInspectUpdateSql());

                        break;

                    //トラDON_車種
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_CarKind:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeCarKindSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeCarKindUpdateSql());

                        break;

                    //トラDON_社員
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Staff:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeStaffSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeStaffUpdateSql());

                        break;

                    //トラDON_発着地
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Point:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergePointSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergePointUpdateSql());

                        break;

                    //トラDON_品目
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Item:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeItemSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeItemUpdateSql());

                        break;

                    //トラDON_単価
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_AtPrice:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeAtPriceSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeAtPriceUpdateSql());

                        break;

                    //トラDON_単位
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Fig:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeFigSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeFigUpdateSql());

                        break;

                    //トラDON_備考
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Memo:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeMemoSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeMemoUpdateSql());

                        break;

                    //トラDON_経費科目
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_CostAccount:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeCostAccountSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeCostAccountUpdateSql());

                        break;

                    //トラDON_経費科目
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Contract:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeContractSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeContractUpdateSql());

                        break;

                    //トラDON_荷主
                    case DefaultProperty.MergeToraDonTableKbn.TORADON_Owner:

                        //マージSQL取得（INSERT、DELETE）
                        mySqlList.Add(this.GetMergeOwnerSql());

                        //マージSQL取得（UPDATE）
                        mySqlList.Add(this.GetMergeOwnerUpdateSql());

                        break;
                    default:
                        break;
                }
            }

            // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
            string query = SQLHelper.SQLQueryJoin(mySqlList);

            // 指定したトランザクション上でExecuteNonqueryを実行
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));
        }

        /// <summary>
        /// 管理とトラDON_管理のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeSystemPropertySql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_SystemProperty AS target ");
            sb.AppendLine(" USING VIEW_TORADON_SystemProperty AS source ");
            sb.AppendLine("    ON ( target.SystemPropertyId = source.SystemPropertyId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        SystemPropertyId ");
            sb.AppendLine("       ,CompanyNM ");
            sb.AppendLine("       ,ChiefNM ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,Url ");
            sb.AppendLine("       ,MailAddress ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,Account1 ");
            sb.AppendLine("       ,Account2 ");
            sb.AppendLine("       ,Account3 ");
            sb.AppendLine("       ,AccountPrintKbn ");
            sb.AppendLine("       ,BillComments ");
            sb.AppendLine("       ,PayDtlComments ");
            sb.AppendLine("       ,TaxRate ");
            sb.AppendLine("       ,NewTaxRate ");
            sb.AppendLine("       ,NewTaxRateStartDate ");
            sb.AppendLine("       ,AccountStartDate ");
            sb.AppendLine("       ,AccountSettlementDate ");
            sb.AppendLine("       ,InputMaxDate ");
            sb.AppendLine("       ,LogRemainDaySpan ");
            sb.AppendLine("       ,LastSUMOfMonthDate ");
            sb.AppendLine("       ,CancelAccClmEnableDate ");
            sb.AppendLine("       ,SalaryFixDay ");
            sb.AppendLine("       ,DefaultSearchKanaMode ");
            sb.AppendLine("       ,DefaultDailyDriveReportNumber ");
            sb.AppendLine("       ,DefaultDailyDriveReportStartDate ");
            sb.AppendLine("       ,ItemMustFlag ");
            sb.AppendLine("       ,DRBalanceCostAccountId1 ");
            sb.AppendLine("       ,DRBalanceCostAccountId2 ");
            sb.AppendLine("       ,DRBalanceCostAccountId3 ");
            sb.AppendLine("       ,DRBalanceCostAccountId4 ");
            sb.AppendLine("       ,DRBalanceCostAccountId5 ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.SystemPropertyId ");
            sb.AppendLine("       ,source.CompanyNM ");
            sb.AppendLine("       ,source.ChiefNM ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.Url ");
            sb.AppendLine("       ,source.MailAddress ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.Account1 ");
            sb.AppendLine("       ,source.Account2 ");
            sb.AppendLine("       ,source.Account3 ");
            sb.AppendLine("       ,source.AccountPrintKbn ");
            sb.AppendLine("       ,source.BillComments ");
            sb.AppendLine("       ,source.PayDtlComments ");
            sb.AppendLine("       ,source.TaxRate ");
            sb.AppendLine("       ,source.NewTaxRate ");
            sb.AppendLine("       ,source.NewTaxRateStartDate ");
            sb.AppendLine("       ,source.AccountStartDate ");
            sb.AppendLine("       ,source.AccountSettlementDate ");
            sb.AppendLine("       ,source.InputMaxDate ");
            sb.AppendLine("       ,source.LogRemainDaySpan ");
            sb.AppendLine("       ,source.LastSUMOfMonthDate ");
            sb.AppendLine("       ,source.CancelAccClmEnableDate ");
            sb.AppendLine("       ,source.SalaryFixDay ");
            sb.AppendLine("       ,source.DefaultSearchKanaMode ");
            sb.AppendLine("       ,source.DefaultDailyDriveReportNumber ");
            sb.AppendLine("       ,source.DefaultDailyDriveReportStartDate ");
            sb.AppendLine("       ,source.ItemMustFlag ");
            sb.AppendLine("       ,source.DRBalanceCostAccountId1 ");
            sb.AppendLine("       ,source.DRBalanceCostAccountId2 ");
            sb.AppendLine("       ,source.DRBalanceCostAccountId3 ");
            sb.AppendLine("       ,source.DRBalanceCostAccountId4 ");
            sb.AppendLine("       ,source.DRBalanceCostAccountId5 ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 管理とトラDON_管理をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeSystemPropertyUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_SystemProperty AS target ");
            sb.AppendLine(" USING VIEW_TORADON_SystemProperty AS source ");
            sb.AppendLine("    ON ( target.SystemPropertyId = source.SystemPropertyId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        SystemPropertyId = source.SystemPropertyId ");
            sb.AppendLine("       ,CompanyNM = source.CompanyNM ");
            sb.AppendLine("       ,ChiefNM = source.ChiefNM ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,Url = source.Url ");
            sb.AppendLine("       ,MailAddress = source.MailAddress ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,Account1 = source.Account1 ");
            sb.AppendLine("       ,Account2 = source.Account2 ");
            sb.AppendLine("       ,Account3 = source.Account3 ");
            sb.AppendLine("       ,AccountPrintKbn = source.AccountPrintKbn ");
            sb.AppendLine("       ,BillComments = source.BillComments ");
            sb.AppendLine("       ,PayDtlComments = source.PayDtlComments ");
            sb.AppendLine("       ,TaxRate = source.TaxRate ");
            sb.AppendLine("       ,NewTaxRate = source.NewTaxRate ");
            sb.AppendLine("       ,NewTaxRateStartDate = source.NewTaxRateStartDate ");
            sb.AppendLine("       ,AccountStartDate = source.AccountStartDate ");
            sb.AppendLine("       ,AccountSettlementDate = source.AccountSettlementDate ");
            sb.AppendLine("       ,InputMaxDate = source.InputMaxDate ");
            sb.AppendLine("       ,LogRemainDaySpan = source.LogRemainDaySpan ");
            sb.AppendLine("       ,LastSUMOfMonthDate = source.LastSUMOfMonthDate ");
            sb.AppendLine("       ,CancelAccClmEnableDate = source.CancelAccClmEnableDate ");
            sb.AppendLine("       ,SalaryFixDay = source.SalaryFixDay ");
            sb.AppendLine("       ,DefaultSearchKanaMode = source.DefaultSearchKanaMode ");
            sb.AppendLine("       ,DefaultDailyDriveReportNumber = source.DefaultDailyDriveReportNumber ");
            sb.AppendLine("       ,DefaultDailyDriveReportStartDate = source.DefaultDailyDriveReportStartDate ");
            sb.AppendLine("       ,ItemMustFlag = source.ItemMustFlag ");
            sb.AppendLine("       ,DRBalanceCostAccountId1 = source.DRBalanceCostAccountId1 ");
            sb.AppendLine("       ,DRBalanceCostAccountId2 = source.DRBalanceCostAccountId2 ");
            sb.AppendLine("       ,DRBalanceCostAccountId3 = source.DRBalanceCostAccountId3 ");
            sb.AppendLine("       ,DRBalanceCostAccountId4 = source.DRBalanceCostAccountId4 ");
            sb.AppendLine("       ,DRBalanceCostAccountId5 = source.DRBalanceCostAccountId5 ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 初期値とトラDON_初期値のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeDefaultPropertySql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_DefaultProperty AS target ");
            sb.AppendLine(" USING VIEW_TORADON_DefaultProperty AS source ");
            sb.AppendLine("    ON ( target.DefaultPropertyId = source.DefaultPropertyId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        DefaultPropertyId ");
            sb.AppendLine("       ,TokuiBillPRTKbn ");
            sb.AppendLine("       ,TokuiClmRemainPRTKbn ");
            sb.AppendLine("       ,TokuiTaxActKbn ");
            sb.AppendLine("       ,TokuiTaxCutKbn ");
            sb.AppendLine("       ,TokuiGakCutKbn ");
            sb.AppendLine("       ,TokuiClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixClmGakPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixPRTSpanKbn ");
            sb.AppendLine("       ,TokuiDepositDtlPRTKbn ");
            sb.AppendLine("       ,TokuiAccountPrintKbn1 ");
            sb.AppendLine("       ,TokuiAccountPrintKbn2 ");
            sb.AppendLine("       ,TokuiAccountPrintKbn3 ");
            sb.AppendLine("       ,TokuiClmClassUseKbn ");
            sb.AppendLine("       ,TokuiClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,TokuiClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiClmClassTaxPRTKbn ");
            sb.AppendLine("       ,TokuiAtPricePRTKbn ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn1 ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn2 ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn3 ");
            sb.AppendLine("       ,TokuiSaleSlipToClmDayKbn ");
            sb.AppendLine("       ,TokuiBillDatePRTKbn ");
            sb.AppendLine("       ,TokuiHonorificTitle ");
            sb.AppendLine("       ,TokuiRemainManageFlag ");
            sb.AppendLine("       ,ClmClassBillToContractorPRTKbn ");
            sb.AppendLine("       ,ClmClassBillToAddressPRTKbn ");
            sb.AppendLine("       ,TorihikiPayDtlStatePRTKbn ");
            sb.AppendLine("       ,TorihikiTaxActKbn ");
            sb.AppendLine("       ,TorihikiTaxCutKbn ");
            sb.AppendLine("       ,TorihikiGakCutKbn ");
            sb.AppendLine("       ,TorihikiPayDtlPRTKbn ");
            sb.AppendLine("       ,TorihikiSaleSlipToPayDayKbn ");
            sb.AppendLine("       ,TorihikiPayDtlDatePRTKbn ");
            sb.AppendLine("       ,TorihikiHonorificTitle ");
            sb.AppendLine("       ,TorihikiRemainManageFlag ");
            sb.AppendLine("       ,ItemTaxDispKbn ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.DefaultPropertyId ");
            sb.AppendLine("       ,source.TokuiBillPRTKbn ");
            sb.AppendLine("       ,source.TokuiClmRemainPRTKbn ");
            sb.AppendLine("       ,source.TokuiTaxActKbn ");
            sb.AppendLine("       ,source.TokuiTaxCutKbn ");
            sb.AppendLine("       ,source.TokuiGakCutKbn ");
            sb.AppendLine("       ,source.TokuiClmDtlPRTKbn ");
            sb.AppendLine("       ,source.TokuiUnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,source.TokuiUnFixClmGakPRTKbn ");
            sb.AppendLine("       ,source.TokuiUnFixPRTSpanKbn ");
            sb.AppendLine("       ,source.TokuiDepositDtlPRTKbn ");
            sb.AppendLine("       ,source.TokuiAccountPrintKbn1 ");
            sb.AppendLine("       ,source.TokuiAccountPrintKbn2 ");
            sb.AppendLine("       ,source.TokuiAccountPrintKbn3 ");
            sb.AppendLine("       ,source.TokuiClmClassUseKbn ");
            sb.AppendLine("       ,source.TokuiClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,source.TokuiClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,source.TokuiClmClassTaxPRTKbn ");
            sb.AppendLine("       ,source.TokuiAtPricePRTKbn ");
            sb.AppendLine("       ,source.TokuiAtPriceIDXKbn1 ");
            sb.AppendLine("       ,source.TokuiAtPriceIDXKbn2 ");
            sb.AppendLine("       ,source.TokuiAtPriceIDXKbn3 ");
            sb.AppendLine("       ,source.TokuiSaleSlipToClmDayKbn ");
            sb.AppendLine("       ,source.TokuiBillDatePRTKbn ");
            sb.AppendLine("       ,source.TokuiHonorificTitle ");
            sb.AppendLine("       ,source.TokuiRemainManageFlag ");
            sb.AppendLine("       ,source.ClmClassBillToContractorPRTKbn ");
            sb.AppendLine("       ,source.ClmClassBillToAddressPRTKbn ");
            sb.AppendLine("       ,source.TorihikiPayDtlStatePRTKbn ");
            sb.AppendLine("       ,source.TorihikiTaxActKbn ");
            sb.AppendLine("       ,source.TorihikiTaxCutKbn ");
            sb.AppendLine("       ,source.TorihikiGakCutKbn ");
            sb.AppendLine("       ,source.TorihikiPayDtlPRTKbn ");
            sb.AppendLine("       ,source.TorihikiSaleSlipToPayDayKbn ");
            sb.AppendLine("       ,source.TorihikiPayDtlDatePRTKbn ");
            sb.AppendLine("       ,source.TorihikiHonorificTitle ");
            sb.AppendLine("       ,source.TorihikiRemainManageFlag ");
            sb.AppendLine("       ,source.ItemTaxDispKbn ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 初期値とトラDON_初期値をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeDefaultPropertyUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_DefaultProperty AS target ");
            sb.AppendLine(" USING VIEW_TORADON_DefaultProperty AS source ");
            sb.AppendLine("    ON ( target.DefaultPropertyId = source.DefaultPropertyId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        DefaultPropertyId = source.DefaultPropertyId ");
            sb.AppendLine("       ,TokuiBillPRTKbn = source.TokuiBillPRTKbn ");
            sb.AppendLine("       ,TokuiClmRemainPRTKbn = source.TokuiClmRemainPRTKbn ");
            sb.AppendLine("       ,TokuiTaxActKbn = source.TokuiTaxActKbn ");
            sb.AppendLine("       ,TokuiTaxCutKbn = source.TokuiTaxCutKbn ");
            sb.AppendLine("       ,TokuiGakCutKbn = source.TokuiGakCutKbn ");
            sb.AppendLine("       ,TokuiClmDtlPRTKbn = source.TokuiClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixClmDtlPRTKbn = source.TokuiUnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixClmGakPRTKbn = source.TokuiUnFixClmGakPRTKbn ");
            sb.AppendLine("       ,TokuiUnFixPRTSpanKbn = source.TokuiUnFixPRTSpanKbn ");
            sb.AppendLine("       ,TokuiDepositDtlPRTKbn = source.TokuiDepositDtlPRTKbn ");
            sb.AppendLine("       ,TokuiAccountPrintKbn1 = source.TokuiAccountPrintKbn1 ");
            sb.AppendLine("       ,TokuiAccountPrintKbn2 = source.TokuiAccountPrintKbn2 ");
            sb.AppendLine("       ,TokuiAccountPrintKbn3 = source.TokuiAccountPrintKbn3 ");
            sb.AppendLine("       ,TokuiClmClassUseKbn = source.TokuiClmClassUseKbn ");
            sb.AppendLine("       ,TokuiClmClassClmRemainPRTKbn = source.TokuiClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,TokuiClmClassClmDtlPRTKbn = source.TokuiClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,TokuiClmClassTaxPRTKbn = source.TokuiClmClassTaxPRTKbn ");
            sb.AppendLine("       ,TokuiAtPricePRTKbn = source.TokuiAtPricePRTKbn ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn1 = source.TokuiAtPriceIDXKbn1 ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn2 = source.TokuiAtPriceIDXKbn2 ");
            sb.AppendLine("       ,TokuiAtPriceIDXKbn3 = source.TokuiAtPriceIDXKbn3 ");
            sb.AppendLine("       ,TokuiSaleSlipToClmDayKbn = source.TokuiSaleSlipToClmDayKbn ");
            sb.AppendLine("       ,TokuiBillDatePRTKbn = source.TokuiBillDatePRTKbn ");
            sb.AppendLine("       ,TokuiHonorificTitle = source.TokuiHonorificTitle ");
            sb.AppendLine("       ,TokuiRemainManageFlag = source.TokuiRemainManageFlag ");
            sb.AppendLine("       ,ClmClassBillToContractorPRTKbn = source.ClmClassBillToContractorPRTKbn ");
            sb.AppendLine("       ,ClmClassBillToAddressPRTKbn = source.ClmClassBillToAddressPRTKbn ");
            sb.AppendLine("       ,TorihikiPayDtlStatePRTKbn = source.TorihikiPayDtlStatePRTKbn ");
            sb.AppendLine("       ,TorihikiTaxActKbn = source.TorihikiTaxActKbn ");
            sb.AppendLine("       ,TorihikiTaxCutKbn = source.TorihikiTaxCutKbn ");
            sb.AppendLine("       ,TorihikiGakCutKbn = source.TorihikiGakCutKbn ");
            sb.AppendLine("       ,TorihikiPayDtlPRTKbn = source.TorihikiPayDtlPRTKbn ");
            sb.AppendLine("       ,TorihikiSaleSlipToPayDayKbn = source.TorihikiSaleSlipToPayDayKbn ");
            sb.AppendLine("       ,TorihikiPayDtlDatePRTKbn = source.TorihikiPayDtlDatePRTKbn ");
            sb.AppendLine("       ,TorihikiHonorificTitle = source.TorihikiHonorificTitle ");
            sb.AppendLine("       ,TorihikiRemainManageFlag = source.TorihikiRemainManageFlag ");
            sb.AppendLine("       ,ItemTaxDispKbn = source.ItemTaxDispKbn ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 営業所とトラDON_営業所のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeBranchOfficeSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_BranchOffice AS target ");
            sb.AppendLine(" USING VIEW_TORADON_BranchOffice AS source ");
            sb.AppendLine("    ON ( target.BranchOfficeId = source.BranchOfficeId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        BranchOfficeId ");
            sb.AppendLine("       ,BranchOfficeCd ");
            sb.AppendLine("       ,BranchOfficeNM ");
            sb.AppendLine("       ,BranchOfficeSNM ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,Url ");
            sb.AppendLine("       ,MailAddress ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,Account1 ");
            sb.AppendLine("       ,Account2 ");
            sb.AppendLine("       ,Account3 ");
            sb.AppendLine("       ,AccountSub1 ");
            sb.AppendLine("       ,AccountSub2 ");
            sb.AppendLine("       ,AccountSub3 ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("       ,ChihoKbn ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.BranchOfficeId ");
            sb.AppendLine("       ,source.BranchOfficeCd ");
            sb.AppendLine("       ,source.BranchOfficeNM ");
            sb.AppendLine("       ,source.BranchOfficeSNM ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.Url ");
            sb.AppendLine("       ,source.MailAddress ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.Account1 ");
            sb.AppendLine("       ,source.Account2 ");
            sb.AppendLine("       ,source.Account3 ");
            sb.AppendLine("       ,source.AccountSub1 ");
            sb.AppendLine("       ,source.AccountSub2 ");
            sb.AppendLine("       ,source.AccountSub3 ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("       ,source.ChihoKbn ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 営業所とトラDON_営業所をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeBranchOfficeUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_BranchOffice AS target ");
            sb.AppendLine(" USING VIEW_TORADON_BranchOffice AS source ");
            sb.AppendLine("    ON ( target.BranchOfficeId = source.BranchOfficeId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        BranchOfficeId = source.BranchOfficeId ");
            sb.AppendLine("       ,BranchOfficeCd = source.BranchOfficeCd ");
            sb.AppendLine("       ,BranchOfficeNM = source.BranchOfficeNM ");
            sb.AppendLine("       ,BranchOfficeSNM = source.BranchOfficeSNM ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,Url = source.Url ");
            sb.AppendLine("       ,MailAddress = source.MailAddress ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,Account1 = source.Account1 ");
            sb.AppendLine("       ,Account2 = source.Account2 ");
            sb.AppendLine("       ,Account3 = source.Account3 ");
            sb.AppendLine("       ,AccountSub1 = source.AccountSub1 ");
            sb.AppendLine("       ,AccountSub2 = source.AccountSub2 ");
            sb.AppendLine("       ,AccountSub3 = source.AccountSub3 ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("       ,ChihoKbn = source.ChihoKbn ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 得意先とトラDON_得意先のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeTokuisakiSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Tokuisaki AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Tokuisaki AS source ");
            sb.AppendLine("    ON ( target.TokuisakiId = source.TokuisakiId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        TokuisakiId ");
            sb.AppendLine("       ,TokuisakiCd ");
            sb.AppendLine("       ,BranchOfficeId ");
            sb.AppendLine("       ,TokuisakiNM ");
            sb.AppendLine("       ,TokuisakiSNM ");
            sb.AppendLine("       ,TokuisakiNMK ");
            sb.AppendLine("       ,HonorificTitle ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,TokuisakiStaffNM ");
            sb.AppendLine("       ,Url ");
            sb.AppendLine("       ,MailAddress ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,StaffId ");
            sb.AppendLine("       ,ClmId ");
            sb.AppendLine("       ,CorpId ");
            sb.AppendLine("       ,ClmAtTimeKbn ");
            sb.AppendLine("       ,FixDay1 ");
            sb.AppendLine("       ,DepositMonth1 ");
            sb.AppendLine("       ,DepositDay1 ");
            sb.AppendLine("       ,FixDay2 ");
            sb.AppendLine("       ,DepositMonth2 ");
            sb.AppendLine("       ,DepositDay2 ");
            sb.AppendLine("       ,FixDay3 ");
            sb.AppendLine("       ,DepositMonth3 ");
            sb.AppendLine("       ,DepositDay3 ");
            sb.AppendLine("       ,CollectGak1 ");
            sb.AppendLine("       ,CollectKbn11 ");
            sb.AppendLine("       ,CollectRate11 ");
            sb.AppendLine("       ,CollectKbn12 ");
            sb.AppendLine("       ,CollectRate12 ");
            sb.AppendLine("       ,CollectGak2 ");
            sb.AppendLine("       ,CollectKbn21 ");
            sb.AppendLine("       ,CollectRate21 ");
            sb.AppendLine("       ,CollectKbn22 ");
            sb.AppendLine("       ,CollectRate22 ");
            sb.AppendLine("       ,CollectGak3 ");
            sb.AppendLine("       ,CollectKbn31 ");
            sb.AppendLine("       ,CollectRate31 ");
            sb.AppendLine("       ,CollectKbn32 ");
            sb.AppendLine("       ,CollectRate32 ");
            sb.AppendLine("       ,BillPRTKbn ");
            sb.AppendLine("       ,ClmRemainPRTKbn ");
            sb.AppendLine("       ,TaxActKbn ");
            sb.AppendLine("       ,TaxCutKbn ");
            sb.AppendLine("       ,GakCutKbn ");
            sb.AppendLine("       ,ClmDtlPRTKbn ");
            sb.AppendLine("       ,UnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,UnFixClmGakPRTKbn ");
            sb.AppendLine("       ,UnFixPRTSpanKbn ");
            sb.AppendLine("       ,DepositDtlPRTKbn ");
            sb.AppendLine("       ,AccountPrintKbn1 ");
            sb.AppendLine("       ,AccountPrintKbn2 ");
            sb.AppendLine("       ,AccountPrintKbn3 ");
            sb.AppendLine("       ,ClmClassUseKbn ");
            sb.AppendLine("       ,ClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,ClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,ClmClassTaxPRTKbn ");
            sb.AppendLine("       ,AtPricePRTKbn ");
            sb.AppendLine("       ,AtPriceIDXKbn1 ");
            sb.AppendLine("       ,AtPriceIDXKbn2 ");
            sb.AppendLine("       ,AtPriceIDXKbn3 ");
            sb.AppendLine("       ,MemoAccount ");
            sb.AppendLine("       ,SaleSlipToClmDayKbn ");
            sb.AppendLine("       ,BillDatePRTKbn ");
            sb.AppendLine("       ,FirstClmDay ");
            sb.AppendLine("       ,LastClmFixDay ");
            sb.AppendLine("       ,RemainManageFlag ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.TokuisakiId ");
            sb.AppendLine("       ,source.TokuisakiCd ");
            sb.AppendLine("       ,source.BranchOfficeId ");
            sb.AppendLine("       ,source.TokuisakiNM ");
            sb.AppendLine("       ,source.TokuisakiSNM ");
            sb.AppendLine("       ,source.TokuisakiNMK ");
            sb.AppendLine("       ,source.HonorificTitle ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.TokuisakiStaffNM ");
            sb.AppendLine("       ,source.Url ");
            sb.AppendLine("       ,source.MailAddress ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.StaffId ");
            sb.AppendLine("       ,source.ClmId ");
            sb.AppendLine("       ,source.CorpId ");
            sb.AppendLine("       ,source.ClmAtTimeKbn ");
            sb.AppendLine("       ,source.FixDay1 ");
            sb.AppendLine("       ,source.DepositMonth1 ");
            sb.AppendLine("       ,source.DepositDay1 ");
            sb.AppendLine("       ,source.FixDay2 ");
            sb.AppendLine("       ,source.DepositMonth2 ");
            sb.AppendLine("       ,source.DepositDay2 ");
            sb.AppendLine("       ,source.FixDay3 ");
            sb.AppendLine("       ,source.DepositMonth3 ");
            sb.AppendLine("       ,source.DepositDay3 ");
            sb.AppendLine("       ,source.CollectGak1 ");
            sb.AppendLine("       ,source.CollectKbn11 ");
            sb.AppendLine("       ,source.CollectRate11 ");
            sb.AppendLine("       ,source.CollectKbn12 ");
            sb.AppendLine("       ,source.CollectRate12 ");
            sb.AppendLine("       ,source.CollectGak2 ");
            sb.AppendLine("       ,source.CollectKbn21 ");
            sb.AppendLine("       ,source.CollectRate21 ");
            sb.AppendLine("       ,source.CollectKbn22 ");
            sb.AppendLine("       ,source.CollectRate22 ");
            sb.AppendLine("       ,source.CollectGak3 ");
            sb.AppendLine("       ,source.CollectKbn31 ");
            sb.AppendLine("       ,source.CollectRate31 ");
            sb.AppendLine("       ,source.CollectKbn32 ");
            sb.AppendLine("       ,source.CollectRate32 ");
            sb.AppendLine("       ,source.BillPRTKbn ");
            sb.AppendLine("       ,source.ClmRemainPRTKbn ");
            sb.AppendLine("       ,source.TaxActKbn ");
            sb.AppendLine("       ,source.TaxCutKbn ");
            sb.AppendLine("       ,source.GakCutKbn ");
            sb.AppendLine("       ,source.ClmDtlPRTKbn ");
            sb.AppendLine("       ,source.UnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,source.UnFixClmGakPRTKbn ");
            sb.AppendLine("       ,source.UnFixPRTSpanKbn ");
            sb.AppendLine("       ,source.DepositDtlPRTKbn ");
            sb.AppendLine("       ,source.AccountPrintKbn1 ");
            sb.AppendLine("       ,source.AccountPrintKbn2 ");
            sb.AppendLine("       ,source.AccountPrintKbn3 ");
            sb.AppendLine("       ,source.ClmClassUseKbn ");
            sb.AppendLine("       ,source.ClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,source.ClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,source.ClmClassTaxPRTKbn ");
            sb.AppendLine("       ,source.AtPricePRTKbn ");
            sb.AppendLine("       ,source.AtPriceIDXKbn1 ");
            sb.AppendLine("       ,source.AtPriceIDXKbn2 ");
            sb.AppendLine("       ,source.AtPriceIDXKbn3 ");
            sb.AppendLine("       ,source.MemoAccount ");
            sb.AppendLine("       ,source.SaleSlipToClmDayKbn ");
            sb.AppendLine("       ,source.BillDatePRTKbn ");
            sb.AppendLine("       ,source.FirstClmDay ");
            sb.AppendLine("       ,source.LastClmFixDay ");
            sb.AppendLine("       ,source.RemainManageFlag ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 得意先とトラDON_得意先をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeTokuisakiUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Tokuisaki AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Tokuisaki AS source ");
            sb.AppendLine("    ON ( target.TokuisakiId = source.TokuisakiId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        TokuisakiId = source.TokuisakiId ");
            sb.AppendLine("       ,TokuisakiCd = source.TokuisakiCd ");
            sb.AppendLine("       ,BranchOfficeId = source.BranchOfficeId ");
            sb.AppendLine("       ,TokuisakiNM = source.TokuisakiNM ");
            sb.AppendLine("       ,TokuisakiSNM = source.TokuisakiSNM ");
            sb.AppendLine("       ,TokuisakiNMK = source.TokuisakiNMK ");
            sb.AppendLine("       ,HonorificTitle = source.HonorificTitle ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,TokuisakiStaffNM = source.TokuisakiStaffNM ");
            sb.AppendLine("       ,Url = source.Url ");
            sb.AppendLine("       ,MailAddress = source.MailAddress ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,StaffId = source.StaffId ");
            sb.AppendLine("       ,ClmId = source.ClmId ");
            sb.AppendLine("       ,CorpId = source.CorpId ");
            sb.AppendLine("       ,ClmAtTimeKbn = source.ClmAtTimeKbn ");
            sb.AppendLine("       ,FixDay1 = source.FixDay1 ");
            sb.AppendLine("       ,DepositMonth1 = source.DepositMonth1 ");
            sb.AppendLine("       ,DepositDay1 = source.DepositDay1 ");
            sb.AppendLine("       ,FixDay2 = source.FixDay2 ");
            sb.AppendLine("       ,DepositMonth2 = source.DepositMonth2 ");
            sb.AppendLine("       ,DepositDay2 = source.DepositDay2 ");
            sb.AppendLine("       ,FixDay3 = source.FixDay3 ");
            sb.AppendLine("       ,DepositMonth3 = source.DepositMonth3 ");
            sb.AppendLine("       ,DepositDay3 = source.DepositDay3 ");
            sb.AppendLine("       ,CollectGak1 = source.CollectGak1 ");
            sb.AppendLine("       ,CollectKbn11 = source.CollectKbn11 ");
            sb.AppendLine("       ,CollectRate11 = source.CollectRate11 ");
            sb.AppendLine("       ,CollectKbn12 = source.CollectKbn12 ");
            sb.AppendLine("       ,CollectRate12 = source.CollectRate12 ");
            sb.AppendLine("       ,CollectGak2 = source.CollectGak2 ");
            sb.AppendLine("       ,CollectKbn21 = source.CollectKbn21 ");
            sb.AppendLine("       ,CollectRate21 = source.CollectRate21 ");
            sb.AppendLine("       ,CollectKbn22 = source.CollectKbn22 ");
            sb.AppendLine("       ,CollectRate22 = source.CollectRate22 ");
            sb.AppendLine("       ,CollectGak3 = source.CollectGak3 ");
            sb.AppendLine("       ,CollectKbn31 = source.CollectKbn31 ");
            sb.AppendLine("       ,CollectRate31 = source.CollectRate31 ");
            sb.AppendLine("       ,CollectKbn32 = source.CollectKbn32 ");
            sb.AppendLine("       ,CollectRate32 = source.CollectRate32 ");
            sb.AppendLine("       ,BillPRTKbn = source.BillPRTKbn ");
            sb.AppendLine("       ,ClmRemainPRTKbn = source.ClmRemainPRTKbn ");
            sb.AppendLine("       ,TaxActKbn = source.TaxActKbn ");
            sb.AppendLine("       ,TaxCutKbn = source.TaxCutKbn ");
            sb.AppendLine("       ,GakCutKbn = source.GakCutKbn ");
            sb.AppendLine("       ,ClmDtlPRTKbn = source.ClmDtlPRTKbn ");
            sb.AppendLine("       ,UnFixClmDtlPRTKbn = source.UnFixClmDtlPRTKbn ");
            sb.AppendLine("       ,UnFixClmGakPRTKbn = source.UnFixClmGakPRTKbn ");
            sb.AppendLine("       ,UnFixPRTSpanKbn = source.UnFixPRTSpanKbn ");
            sb.AppendLine("       ,DepositDtlPRTKbn = source.DepositDtlPRTKbn ");
            sb.AppendLine("       ,AccountPrintKbn1 = source.AccountPrintKbn1 ");
            sb.AppendLine("       ,AccountPrintKbn2 = source.AccountPrintKbn2 ");
            sb.AppendLine("       ,AccountPrintKbn3 = source.AccountPrintKbn3 ");
            sb.AppendLine("       ,ClmClassUseKbn = source.ClmClassUseKbn ");
            sb.AppendLine("       ,ClmClassClmRemainPRTKbn = source.ClmClassClmRemainPRTKbn ");
            sb.AppendLine("       ,ClmClassClmDtlPRTKbn = source.ClmClassClmDtlPRTKbn ");
            sb.AppendLine("       ,ClmClassTaxPRTKbn = source.ClmClassTaxPRTKbn ");
            sb.AppendLine("       ,AtPricePRTKbn = source.AtPricePRTKbn ");
            sb.AppendLine("       ,AtPriceIDXKbn1 = source.AtPriceIDXKbn1 ");
            sb.AppendLine("       ,AtPriceIDXKbn2 = source.AtPriceIDXKbn2 ");
            sb.AppendLine("       ,AtPriceIDXKbn3 = source.AtPriceIDXKbn3 ");
            sb.AppendLine("       ,MemoAccount = source.MemoAccount ");
            sb.AppendLine("       ,SaleSlipToClmDayKbn = source.SaleSlipToClmDayKbn ");
            sb.AppendLine("       ,BillDatePRTKbn = source.BillDatePRTKbn ");
            sb.AppendLine("       ,FirstClmDay = source.FirstClmDay ");
            sb.AppendLine("       ,LastClmFixDay = source.LastClmFixDay ");
            sb.AppendLine("       ,RemainManageFlag = source.RemainManageFlag ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 請求部門とトラDON_請求部門のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeClmClassSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_ClmClass AS target ");
            sb.AppendLine(" USING VIEW_TORADON_ClmClass AS source ");
            sb.AppendLine("    ON ( target.ClmClassId = source.ClmClassId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        ClmClassId ");
            sb.AppendLine("       ,ClmClassCd ");
            sb.AppendLine("       ,TokuisakiId ");
            sb.AppendLine("       ,ClmClassNM ");
            sb.AppendLine("       ,ClmClassSNM ");
            sb.AppendLine("       ,ClmClassNMK ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,ClmPRTKbn ");
            sb.AppendLine("       ,ClmAddressPRTKbn ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.ClmClassId ");
            sb.AppendLine("       ,source.ClmClassCd ");
            sb.AppendLine("       ,source.TokuisakiId ");
            sb.AppendLine("       ,source.ClmClassNM ");
            sb.AppendLine("       ,source.ClmClassSNM ");
            sb.AppendLine("       ,source.ClmClassNMK ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.ClmPRTKbn ");
            sb.AppendLine("       ,source.ClmAddressPRTKbn ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 請求部門とトラDON_請求部門をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeClmClassUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_ClmClass AS target ");
            sb.AppendLine(" USING VIEW_TORADON_ClmClass AS source ");
            sb.AppendLine("    ON ( target.ClmClassId = source.ClmClassId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        ClmClassId = source.ClmClassId ");
            sb.AppendLine("       ,ClmClassCd = source.ClmClassCd ");
            sb.AppendLine("       ,TokuisakiId = source.TokuisakiId ");
            sb.AppendLine("       ,ClmClassNM = source.ClmClassNM ");
            sb.AppendLine("       ,ClmClassSNM = source.ClmClassSNM ");
            sb.AppendLine("       ,ClmClassNMK = source.ClmClassNMK ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,ClmPRTKbn = source.ClmPRTKbn ");
            sb.AppendLine("       ,ClmAddressPRTKbn = source.ClmAddressPRTKbn ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 取引先とトラDON_取引先のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeTorihikisakiSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Torihikisaki AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Torihikisaki AS source ");
            sb.AppendLine("    ON ( target.TorihikiId = source.TorihikiId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        TorihikiId ");
            sb.AppendLine("       ,TorihikiCd ");
            sb.AppendLine("       ,BranchOfficeId ");
            sb.AppendLine("       ,TorihikiNM ");
            sb.AppendLine("       ,TorihikiSNM ");
            sb.AppendLine("       ,TorihikiNMK ");
            sb.AppendLine("       ,HonorificTitle ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,TorihikiStaffNM ");
            sb.AppendLine("       ,Url ");
            sb.AppendLine("       ,MailAddress ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,CharterKbn ");
            sb.AppendLine("       ,PayeeId ");
            sb.AppendLine("       ,FixDay1 ");
            sb.AppendLine("       ,PaymentMonth1 ");
            sb.AppendLine("       ,PaymentDay1 ");
            sb.AppendLine("       ,FixDay2 ");
            sb.AppendLine("       ,PaymentMonth2 ");
            sb.AppendLine("       ,PaymentDay2 ");
            sb.AppendLine("       ,FixDay3 ");
            sb.AppendLine("       ,PaymentMonth3 ");
            sb.AppendLine("       ,PaymentDay3 ");
            sb.AppendLine("       ,PaymentGak1 ");
            sb.AppendLine("       ,PaymentKbn11 ");
            sb.AppendLine("       ,PaymentRate11 ");
            sb.AppendLine("       ,PaymentKbn12 ");
            sb.AppendLine("       ,PaymentRate12 ");
            sb.AppendLine("       ,PaymentGak2 ");
            sb.AppendLine("       ,PaymentKbn21 ");
            sb.AppendLine("       ,PaymentRate21 ");
            sb.AppendLine("       ,PaymentKbn22 ");
            sb.AppendLine("       ,PaymentRate22 ");
            sb.AppendLine("       ,PaymentGak3 ");
            sb.AppendLine("       ,PaymentKbn31 ");
            sb.AppendLine("       ,PaymentRate31 ");
            sb.AppendLine("       ,PaymentKbn32 ");
            sb.AppendLine("       ,PaymentRate32 ");
            sb.AppendLine("       ,PayDtlStatePRTKbn ");
            sb.AppendLine("       ,PayDtlPRTKbn ");
            sb.AppendLine("       ,TaxActKbn ");
            sb.AppendLine("       ,TaxCutKbn ");
            sb.AppendLine("       ,GakCutKbn ");
            sb.AppendLine("       ,CharterRate ");
            sb.AppendLine("       ,CompanyGSKbn ");
            sb.AppendLine("       ,SaleSlipToPayDayKbn ");
            sb.AppendLine("       ,PayDtlDatePRTKbn ");
            sb.AppendLine("       ,FirstClmDay ");
            sb.AppendLine("       ,LastPayFixDay ");
            sb.AppendLine("       ,RemainManageFlag ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.TorihikiId ");
            sb.AppendLine("       ,source.TorihikiCd ");
            sb.AppendLine("       ,source.BranchOfficeId ");
            sb.AppendLine("       ,source.TorihikiNM ");
            sb.AppendLine("       ,source.TorihikiSNM ");
            sb.AppendLine("       ,source.TorihikiNMK ");
            sb.AppendLine("       ,source.HonorificTitle ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.TorihikiStaffNM ");
            sb.AppendLine("       ,source.Url ");
            sb.AppendLine("       ,source.MailAddress ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.CharterKbn ");
            sb.AppendLine("       ,source.PayeeId ");
            sb.AppendLine("       ,source.FixDay1 ");
            sb.AppendLine("       ,source.PaymentMonth1 ");
            sb.AppendLine("       ,source.PaymentDay1 ");
            sb.AppendLine("       ,source.FixDay2 ");
            sb.AppendLine("       ,source.PaymentMonth2 ");
            sb.AppendLine("       ,source.PaymentDay2 ");
            sb.AppendLine("       ,source.FixDay3 ");
            sb.AppendLine("       ,source.PaymentMonth3 ");
            sb.AppendLine("       ,source.PaymentDay3 ");
            sb.AppendLine("       ,source.PaymentGak1 ");
            sb.AppendLine("       ,source.PaymentKbn11 ");
            sb.AppendLine("       ,source.PaymentRate11 ");
            sb.AppendLine("       ,source.PaymentKbn12 ");
            sb.AppendLine("       ,source.PaymentRate12 ");
            sb.AppendLine("       ,source.PaymentGak2 ");
            sb.AppendLine("       ,source.PaymentKbn21 ");
            sb.AppendLine("       ,source.PaymentRate21 ");
            sb.AppendLine("       ,source.PaymentKbn22 ");
            sb.AppendLine("       ,source.PaymentRate22 ");
            sb.AppendLine("       ,source.PaymentGak3 ");
            sb.AppendLine("       ,source.PaymentKbn31 ");
            sb.AppendLine("       ,source.PaymentRate31 ");
            sb.AppendLine("       ,source.PaymentKbn32 ");
            sb.AppendLine("       ,source.PaymentRate32 ");
            sb.AppendLine("       ,source.PayDtlStatePRTKbn ");
            sb.AppendLine("       ,source.PayDtlPRTKbn ");
            sb.AppendLine("       ,source.TaxActKbn ");
            sb.AppendLine("       ,source.TaxCutKbn ");
            sb.AppendLine("       ,source.GakCutKbn ");
            sb.AppendLine("       ,source.CharterRate ");
            sb.AppendLine("       ,source.CompanyGSKbn ");
            sb.AppendLine("       ,source.SaleSlipToPayDayKbn ");
            sb.AppendLine("       ,source.PayDtlDatePRTKbn ");
            sb.AppendLine("       ,source.FirstClmDay ");
            sb.AppendLine("       ,source.LastPayFixDay ");
            sb.AppendLine("       ,source.RemainManageFlag ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 取引先とトラDON_取引先をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeTorihikisakiUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Torihikisaki AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Torihikisaki AS source ");
            sb.AppendLine("    ON ( target.TorihikiId = source.TorihikiId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        TorihikiId = source.TorihikiId ");
            sb.AppendLine("       ,TorihikiCd = source.TorihikiCd ");
            sb.AppendLine("       ,BranchOfficeId = source.BranchOfficeId ");
            sb.AppendLine("       ,TorihikiNM = source.TorihikiNM ");
            sb.AppendLine("       ,TorihikiSNM = source.TorihikiSNM ");
            sb.AppendLine("       ,TorihikiNMK = source.TorihikiNMK ");
            sb.AppendLine("       ,HonorificTitle = source.HonorificTitle ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,TorihikiStaffNM = source.TorihikiStaffNM ");
            sb.AppendLine("       ,Url = source.Url ");
            sb.AppendLine("       ,MailAddress = source.MailAddress ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,CharterKbn = source.CharterKbn ");
            sb.AppendLine("       ,PayeeId = source.PayeeId ");
            sb.AppendLine("       ,FixDay1 = source.FixDay1 ");
            sb.AppendLine("       ,PaymentMonth1 = source.PaymentMonth1 ");
            sb.AppendLine("       ,PaymentDay1 = source.PaymentDay1 ");
            sb.AppendLine("       ,FixDay2 = source.FixDay2 ");
            sb.AppendLine("       ,PaymentMonth2 = source.PaymentMonth2 ");
            sb.AppendLine("       ,PaymentDay2 = source.PaymentDay2 ");
            sb.AppendLine("       ,FixDay3 = source.FixDay3 ");
            sb.AppendLine("       ,PaymentMonth3 = source.PaymentMonth3 ");
            sb.AppendLine("       ,PaymentDay3 = source.PaymentDay3 ");
            sb.AppendLine("       ,PaymentGak1 = source.PaymentGak1 ");
            sb.AppendLine("       ,PaymentKbn11 = source.PaymentKbn11 ");
            sb.AppendLine("       ,PaymentRate11 = source.PaymentRate11 ");
            sb.AppendLine("       ,PaymentKbn12 = source.PaymentKbn12 ");
            sb.AppendLine("       ,PaymentRate12 = source.PaymentRate12 ");
            sb.AppendLine("       ,PaymentGak2 = source.PaymentGak2 ");
            sb.AppendLine("       ,PaymentKbn21 = source.PaymentKbn21 ");
            sb.AppendLine("       ,PaymentRate21 = source.PaymentRate21 ");
            sb.AppendLine("       ,PaymentKbn22 = source.PaymentKbn22 ");
            sb.AppendLine("       ,PaymentRate22 = source.PaymentRate22 ");
            sb.AppendLine("       ,PaymentGak3 = source.PaymentGak3 ");
            sb.AppendLine("       ,PaymentKbn31 = source.PaymentKbn31 ");
            sb.AppendLine("       ,PaymentRate31 = source.PaymentRate31 ");
            sb.AppendLine("       ,PaymentKbn32 = source.PaymentKbn32 ");
            sb.AppendLine("       ,PaymentRate32 = source.PaymentRate32 ");
            sb.AppendLine("       ,PayDtlStatePRTKbn = source.PayDtlStatePRTKbn ");
            sb.AppendLine("       ,PayDtlPRTKbn = source.PayDtlPRTKbn ");
            sb.AppendLine("       ,TaxActKbn = source.TaxActKbn ");
            sb.AppendLine("       ,TaxCutKbn = source.TaxCutKbn ");
            sb.AppendLine("       ,GakCutKbn = source.GakCutKbn ");
            sb.AppendLine("       ,CharterRate = source.CharterRate ");
            sb.AppendLine("       ,CompanyGSKbn = source.CompanyGSKbn ");
            sb.AppendLine("       ,SaleSlipToPayDayKbn = source.SaleSlipToPayDayKbn ");
            sb.AppendLine("       ,PayDtlDatePRTKbn = source.PayDtlDatePRTKbn ");
            sb.AppendLine("       ,FirstClmDay = source.FirstClmDay ");
            sb.AppendLine("       ,LastPayFixDay = source.LastPayFixDay ");
            sb.AppendLine("       ,RemainManageFlag = source.RemainManageFlag ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車両とトラDON_車両のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCarSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Car AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Car AS source ");
            sb.AppendLine("    ON ( target.CarId = source.CarId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        CarId ");
            sb.AppendLine("       ,CarCd ");
            sb.AppendLine("       ,BranchOfficeId ");
            sb.AppendLine("       ,PartId ");
            sb.AppendLine("       ,LicPlateDeptNM ");
            sb.AppendLine("       ,LicPlateCarKindKbn ");
            sb.AppendLine("       ,LicPlateUsageKbn ");
            sb.AppendLine("       ,LicPlateCarNo ");
            sb.AppendLine("       ,DriverId ");
            sb.AppendLine("       ,CarKindId ");
            sb.AppendLine("       ,CarKbn ");
            sb.AppendLine("       ,InspectDate ");
            sb.AppendLine("       ,InspectCycle ");
            sb.AppendLine("       ,MakerId ");
            sb.AppendLine("       ,LeaseKbn ");
            sb.AppendLine("       ,LeaselimitedDate ");
            sb.AppendLine("       ,UnlimitedPersonKbn ");
            sb.AppendLine("       ,InsurePersonGak ");
            sb.AppendLine("       ,UnlimitedThingKbn ");
            sb.AppendLine("       ,InsureThingGak ");
            sb.AppendLine("       ,UnlimitedStaffKbn ");
            sb.AppendLine("       ,InsureStaffGak ");
            sb.AppendLine("       ,OptionalInsureExpirDate ");
            sb.AppendLine("       ,LiabilityInsureUpdateDate ");
            sb.AppendLine("       ,BuyDate ");
            sb.AppendLine("       ,BuyGak ");
            sb.AppendLine("       ,ResultUseKbn ");
            sb.AppendLine("       ,FixedCostUseKbn ");
            sb.AppendLine("       ,CardNo1 ");
            sb.AppendLine("       ,CardNo2 ");
            sb.AppendLine("       ,CardNo3 ");
            sb.AppendLine("       ,CardNo4 ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,FuelKbn ");
            sb.AppendLine("       ,ScrapDate ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.CarId ");
            sb.AppendLine("       ,source.CarCd ");
            sb.AppendLine("       ,source.BranchOfficeId ");
            sb.AppendLine("       ,source.PartId ");
            sb.AppendLine("       ,source.LicPlateDeptNM ");
            sb.AppendLine("       ,source.LicPlateCarKindKbn ");
            sb.AppendLine("       ,source.LicPlateUsageKbn ");
            sb.AppendLine("       ,source.LicPlateCarNo ");
            sb.AppendLine("       ,source.DriverId ");
            sb.AppendLine("       ,source.CarKindId ");
            sb.AppendLine("       ,source.CarKbn ");
            sb.AppendLine("       ,source.InspectDate ");
            sb.AppendLine("       ,source.InspectCycle ");
            sb.AppendLine("       ,source.MakerId ");
            sb.AppendLine("       ,source.LeaseKbn ");
            sb.AppendLine("       ,source.LeaselimitedDate ");
            sb.AppendLine("       ,source.UnlimitedPersonKbn ");
            sb.AppendLine("       ,source.InsurePersonGak ");
            sb.AppendLine("       ,source.UnlimitedThingKbn ");
            sb.AppendLine("       ,source.InsureThingGak ");
            sb.AppendLine("       ,source.UnlimitedStaffKbn ");
            sb.AppendLine("       ,source.InsureStaffGak ");
            sb.AppendLine("       ,source.OptionalInsureExpirDate ");
            sb.AppendLine("       ,source.LiabilityInsureUpdateDate ");
            sb.AppendLine("       ,source.BuyDate ");
            sb.AppendLine("       ,source.BuyGak ");
            sb.AppendLine("       ,source.ResultUseKbn ");
            sb.AppendLine("       ,source.FixedCostUseKbn ");
            sb.AppendLine("       ,source.CardNo1 ");
            sb.AppendLine("       ,source.CardNo2 ");
            sb.AppendLine("       ,source.CardNo3 ");
            sb.AppendLine("       ,source.CardNo4 ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.FuelKbn ");
            sb.AppendLine("       ,source.ScrapDate ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車両とトラDON_車両をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCarUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Car AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Car AS source ");
            sb.AppendLine("    ON ( target.CarId = source.CarId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        CarId = source.CarId ");
            sb.AppendLine("       ,CarCd = source.CarCd ");
            sb.AppendLine("       ,BranchOfficeId = source.BranchOfficeId ");
            sb.AppendLine("       ,PartId = source.PartId ");
            sb.AppendLine("       ,LicPlateDeptNM = source.LicPlateDeptNM ");
            sb.AppendLine("       ,LicPlateCarKindKbn = source.LicPlateCarKindKbn ");
            sb.AppendLine("       ,LicPlateUsageKbn = source.LicPlateUsageKbn ");
            sb.AppendLine("       ,LicPlateCarNo = source.LicPlateCarNo ");
            sb.AppendLine("       ,DriverId = source.DriverId ");
            sb.AppendLine("       ,CarKindId = source.CarKindId ");
            sb.AppendLine("       ,CarKbn = source.CarKbn ");
            sb.AppendLine("       ,InspectDate = source.InspectDate ");
            sb.AppendLine("       ,InspectCycle = source.InspectCycle ");
            sb.AppendLine("       ,MakerId = source.MakerId ");
            sb.AppendLine("       ,LeaseKbn = source.LeaseKbn ");
            sb.AppendLine("       ,LeaselimitedDate = source.LeaselimitedDate ");
            sb.AppendLine("       ,UnlimitedPersonKbn = source.UnlimitedPersonKbn ");
            sb.AppendLine("       ,InsurePersonGak = source.InsurePersonGak ");
            sb.AppendLine("       ,UnlimitedThingKbn = source.UnlimitedThingKbn ");
            sb.AppendLine("       ,InsureThingGak = source.InsureThingGak ");
            sb.AppendLine("       ,UnlimitedStaffKbn = source.UnlimitedStaffKbn ");
            sb.AppendLine("       ,InsureStaffGak = source.InsureStaffGak ");
            sb.AppendLine("       ,OptionalInsureExpirDate = source.OptionalInsureExpirDate ");
            sb.AppendLine("       ,LiabilityInsureUpdateDate = source.LiabilityInsureUpdateDate ");
            sb.AppendLine("       ,BuyDate = source.BuyDate ");
            sb.AppendLine("       ,BuyGak = source.BuyGak ");
            sb.AppendLine("       ,ResultUseKbn = source.ResultUseKbn ");
            sb.AppendLine("       ,FixedCostUseKbn = source.FixedCostUseKbn ");
            sb.AppendLine("       ,CardNo1 = source.CardNo1 ");
            sb.AppendLine("       ,CardNo2 = source.CardNo2 ");
            sb.AppendLine("       ,CardNo3 = source.CardNo3 ");
            sb.AppendLine("       ,CardNo4 = source.CardNo4 ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,FuelKbn = source.FuelKbn ");
            sb.AppendLine("       ,ScrapDate = source.ScrapDate ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車検証とトラDON_車検証のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeInspectSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Inspect AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Inspect AS source ");
            sb.AppendLine("    ON ( target.InspectId = source.InspectId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        InspectId ");
            sb.AppendLine("       ,CarId ");
            sb.AppendLine("       ,EntryDate ");
            sb.AppendLine("       ,FirstEntryDate ");
            sb.AppendLine("       ,CarTypeName ");
            sb.AppendLine("       ,Usage ");
            sb.AppendLine("       ,CarPrivateKind ");
            sb.AppendLine("       ,CarShape ");
            sb.AppendLine("       ,CarName ");
            sb.AppendLine("       ,CarCapacity ");
            sb.AppendLine("       ,MaxCarry ");
            sb.AppendLine("       ,CarWeight ");
            sb.AppendLine("       ,CarGrossWeight ");
            sb.AppendLine("       ,ChassisNo ");
            sb.AppendLine("       ,Length ");
            sb.AppendLine("       ,Width ");
            sb.AppendLine("       ,Height ");
            sb.AppendLine("       ,FrontFrontAxleWeight ");
            sb.AppendLine("       ,FrontRearAxleWeight ");
            sb.AppendLine("       ,RearFrontAxleWeight ");
            sb.AppendLine("       ,RearRearAxleWeight ");
            sb.AppendLine("       ,ModelName ");
            sb.AppendLine("       ,EngineModelName ");
            sb.AppendLine("       ,GrossDisplacement ");
            sb.AppendLine("       ,FuelKind ");
            sb.AppendLine("       ,ModelNameNo ");
            sb.AppendLine("       ,RuibetsuKbn ");
            sb.AppendLine("       ,ExpirDate ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.InspectId ");
            sb.AppendLine("       ,source.CarId ");
            sb.AppendLine("       ,source.EntryDate ");
            sb.AppendLine("       ,source.FirstEntryDate ");
            sb.AppendLine("       ,source.CarTypeName ");
            sb.AppendLine("       ,source.Usage ");
            sb.AppendLine("       ,source.CarPrivateKind ");
            sb.AppendLine("       ,source.CarShape ");
            sb.AppendLine("       ,source.CarName ");
            sb.AppendLine("       ,source.CarCapacity ");
            sb.AppendLine("       ,source.MaxCarry ");
            sb.AppendLine("       ,source.CarWeight ");
            sb.AppendLine("       ,source.CarGrossWeight ");
            sb.AppendLine("       ,source.ChassisNo ");
            sb.AppendLine("       ,source.Length ");
            sb.AppendLine("       ,source.Width ");
            sb.AppendLine("       ,source.Height ");
            sb.AppendLine("       ,source.FrontFrontAxleWeight ");
            sb.AppendLine("       ,source.FrontRearAxleWeight ");
            sb.AppendLine("       ,source.RearFrontAxleWeight ");
            sb.AppendLine("       ,source.RearRearAxleWeight ");
            sb.AppendLine("       ,source.ModelName ");
            sb.AppendLine("       ,source.EngineModelName ");
            sb.AppendLine("       ,source.GrossDisplacement ");
            sb.AppendLine("       ,source.FuelKind ");
            sb.AppendLine("       ,source.ModelNameNo ");
            sb.AppendLine("       ,source.RuibetsuKbn ");
            sb.AppendLine("       ,source.ExpirDate ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車検証とトラDON_車検証をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeInspectUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Inspect AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Inspect AS source ");
            sb.AppendLine("    ON ( target.InspectId = source.InspectId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        InspectId = source.InspectId ");
            sb.AppendLine("       ,CarId = source.CarId ");
            sb.AppendLine("       ,EntryDate = source.EntryDate ");
            sb.AppendLine("       ,FirstEntryDate = source.FirstEntryDate ");
            sb.AppendLine("       ,CarTypeName = source.CarTypeName ");
            sb.AppendLine("       ,Usage = source.Usage ");
            sb.AppendLine("       ,CarPrivateKind = source.CarPrivateKind ");
            sb.AppendLine("       ,CarShape = source.CarShape ");
            sb.AppendLine("       ,CarName = source.CarName ");
            sb.AppendLine("       ,CarCapacity = source.CarCapacity ");
            sb.AppendLine("       ,MaxCarry = source.MaxCarry ");
            sb.AppendLine("       ,CarWeight = source.CarWeight ");
            sb.AppendLine("       ,CarGrossWeight = source.CarGrossWeight ");
            sb.AppendLine("       ,ChassisNo = source.ChassisNo ");
            sb.AppendLine("       ,Length = source.Length ");
            sb.AppendLine("       ,Width = source.Width ");
            sb.AppendLine("       ,Height = source.Height ");
            sb.AppendLine("       ,FrontFrontAxleWeight = source.FrontFrontAxleWeight ");
            sb.AppendLine("       ,FrontRearAxleWeight = source.FrontRearAxleWeight ");
            sb.AppendLine("       ,RearFrontAxleWeight = source.RearFrontAxleWeight ");
            sb.AppendLine("       ,RearRearAxleWeight = source.RearRearAxleWeight ");
            sb.AppendLine("       ,ModelName = source.ModelName ");
            sb.AppendLine("       ,EngineModelName = source.EngineModelName ");
            sb.AppendLine("       ,GrossDisplacement = source.GrossDisplacement ");
            sb.AppendLine("       ,FuelKind = source.FuelKind ");
            sb.AppendLine("       ,ModelNameNo = source.ModelNameNo ");
            sb.AppendLine("       ,RuibetsuKbn = source.RuibetsuKbn ");
            sb.AppendLine("       ,ExpirDate = source.ExpirDate ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車種とトラDON_車種のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCarKindSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_CarKind AS target ");
            sb.AppendLine(" USING VIEW_TORADON_CarKind AS source ");
            sb.AppendLine("    ON ( target.CarKindId = source.CarKindId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        CarKindId ");
            sb.AppendLine("       ,CarKindCd ");
            sb.AppendLine("       ,CarKindNM ");
            sb.AppendLine("       ,CarKindSNM ");
            sb.AppendLine("       ,CarKindNMK ");
            sb.AppendLine("       ,CarKindKbn ");
            sb.AppendLine("       ,TransportWeight ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.CarKindId ");
            sb.AppendLine("       ,source.CarKindCd ");
            sb.AppendLine("       ,source.CarKindNM ");
            sb.AppendLine("       ,source.CarKindSNM ");
            sb.AppendLine("       ,source.CarKindNMK ");
            sb.AppendLine("       ,source.CarKindKbn ");
            sb.AppendLine("       ,source.TransportWeight ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 車種とトラDON_車種をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCarKindUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_CarKind AS target ");
            sb.AppendLine(" USING VIEW_TORADON_CarKind AS source ");
            sb.AppendLine("    ON ( target.CarKindId = source.CarKindId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        CarKindId = source.CarKindId ");
            sb.AppendLine("       ,CarKindCd = source.CarKindCd ");
            sb.AppendLine("       ,CarKindNM = source.CarKindNM ");
            sb.AppendLine("       ,CarKindSNM = source.CarKindSNM ");
            sb.AppendLine("       ,CarKindNMK = source.CarKindNMK ");
            sb.AppendLine("       ,CarKindKbn = source.CarKindKbn ");
            sb.AppendLine("       ,TransportWeight = source.TransportWeight ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 社員とトラDON_社員のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeStaffSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Staff AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Staff AS source ");
            sb.AppendLine("    ON ( target.StaffId = source.StaffId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        StaffId ");
            sb.AppendLine("       ,StaffCd ");
            sb.AppendLine("       ,BranchOfficeId ");
            sb.AppendLine("       ,PartId ");
            sb.AppendLine("       ,StaffNM ");
            sb.AppendLine("       ,StaffSNM ");
            sb.AppendLine("       ,StaffNMK ");
            sb.AppendLine("       ,StaffKbn ");
            sb.AppendLine("       ,RoleKbn ");
            sb.AppendLine("       ,Birthday ");
            sb.AppendLine("       ,JoinDate ");
            sb.AppendLine("       ,HierarchalName ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,RegisteredAddress ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,MobilePhone ");
            sb.AppendLine("       ,MailAddress ");
            sb.AppendLine("       ,EmergencyAddress ");
            sb.AppendLine("       ,Sex ");
            sb.AppendLine("       ,BloodType ");
            sb.AppendLine("       ,WelfareAnnuityNo ");
            sb.AppendLine("       ,WelfareAnnuityJoinDate ");
            sb.AppendLine("       ,HealthInsuranceNo ");
            sb.AppendLine("       ,HealthInsuranceJoinDate ");
            sb.AppendLine("       ,EmploymentInsuranceNo ");
            sb.AppendLine("       ,EmploymentInsuranceJoinDate ");
            sb.AppendLine("       ,EmploymentInsuranceEx ");
            sb.AppendLine("       ,WorkersAccidentInsuranceNo ");
            sb.AppendLine("       ,WorkersAccidentInsuranceJoinDate ");
            sb.AppendLine("       ,CommutingMinute1 ");
            sb.AppendLine("       ,CommutingMinute2 ");
            sb.AppendLine("       ,CommutingMinute3 ");
            sb.AppendLine("       ,CommutingMinute4 ");
            sb.AppendLine("       ,HouseKbn ");
            sb.AppendLine("       ,DriverElectionDate ");
            sb.AppendLine("       ,DriverRetireDate ");
            sb.AppendLine("       ,DriverRetireReason ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,RetireDate ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.StaffId ");
            sb.AppendLine("       ,source.StaffCd ");
            sb.AppendLine("       ,source.BranchOfficeId ");
            sb.AppendLine("       ,source.PartId ");
            sb.AppendLine("       ,source.StaffNM ");
            sb.AppendLine("       ,source.StaffSNM ");
            sb.AppendLine("       ,source.StaffNMK ");
            sb.AppendLine("       ,source.StaffKbn ");
            sb.AppendLine("       ,source.RoleKbn ");
            sb.AppendLine("       ,source.Birthday ");
            sb.AppendLine("       ,source.JoinDate ");
            sb.AppendLine("       ,source.HierarchalName ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.RegisteredAddress ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.MobilePhone ");
            sb.AppendLine("       ,source.MailAddress ");
            sb.AppendLine("       ,source.EmergencyAddress ");
            sb.AppendLine("       ,source.Sex ");
            sb.AppendLine("       ,source.BloodType ");
            sb.AppendLine("       ,source.WelfareAnnuityNo ");
            sb.AppendLine("       ,source.WelfareAnnuityJoinDate ");
            sb.AppendLine("       ,source.HealthInsuranceNo ");
            sb.AppendLine("       ,source.HealthInsuranceJoinDate ");
            sb.AppendLine("       ,source.EmploymentInsuranceNo ");
            sb.AppendLine("       ,source.EmploymentInsuranceJoinDate ");
            sb.AppendLine("       ,source.EmploymentInsuranceEx ");
            sb.AppendLine("       ,source.WorkersAccidentInsuranceNo ");
            sb.AppendLine("       ,source.WorkersAccidentInsuranceJoinDate ");
            sb.AppendLine("       ,source.CommutingMinute1 ");
            sb.AppendLine("       ,source.CommutingMinute2 ");
            sb.AppendLine("       ,source.CommutingMinute3 ");
            sb.AppendLine("       ,source.CommutingMinute4 ");
            sb.AppendLine("       ,source.HouseKbn ");
            sb.AppendLine("       ,source.DriverElectionDate ");
            sb.AppendLine("       ,source.DriverRetireDate ");
            sb.AppendLine("       ,source.DriverRetireReason ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.RetireDate ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 社員とトラDON_社員をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeStaffUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Staff AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Staff AS source ");
            sb.AppendLine("    ON ( target.StaffId = source.StaffId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        StaffId = source.StaffId ");
            sb.AppendLine("       ,StaffCd = source.StaffCd ");
            sb.AppendLine("       ,BranchOfficeId = source.BranchOfficeId ");
            sb.AppendLine("       ,PartId = source.PartId ");
            sb.AppendLine("       ,StaffNM = source.StaffNM ");
            sb.AppendLine("       ,StaffSNM = source.StaffSNM ");
            sb.AppendLine("       ,StaffNMK = source.StaffNMK ");
            sb.AppendLine("       ,StaffKbn = source.StaffKbn ");
            sb.AppendLine("       ,RoleKbn = source.RoleKbn ");
            sb.AppendLine("       ,Birthday = source.Birthday ");
            sb.AppendLine("       ,JoinDate = source.JoinDate ");
            sb.AppendLine("       ,HierarchalName = source.HierarchalName ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,RegisteredAddress = source.RegisteredAddress ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,MobilePhone = source.MobilePhone ");
            sb.AppendLine("       ,MailAddress = source.MailAddress ");
            sb.AppendLine("       ,EmergencyAddress = source.EmergencyAddress ");
            sb.AppendLine("       ,Sex = source.Sex ");
            sb.AppendLine("       ,BloodType = source.BloodType ");
            sb.AppendLine("       ,WelfareAnnuityNo = source.WelfareAnnuityNo ");
            sb.AppendLine("       ,WelfareAnnuityJoinDate = source.WelfareAnnuityJoinDate ");
            sb.AppendLine("       ,HealthInsuranceNo = source.HealthInsuranceNo ");
            sb.AppendLine("       ,HealthInsuranceJoinDate = source.HealthInsuranceJoinDate ");
            sb.AppendLine("       ,EmploymentInsuranceNo = source.EmploymentInsuranceNo ");
            sb.AppendLine("       ,EmploymentInsuranceJoinDate = source.EmploymentInsuranceJoinDate ");
            sb.AppendLine("       ,EmploymentInsuranceEx = source.EmploymentInsuranceEx ");
            sb.AppendLine("       ,WorkersAccidentInsuranceNo = source.WorkersAccidentInsuranceNo ");
            sb.AppendLine("       ,WorkersAccidentInsuranceJoinDate = source.WorkersAccidentInsuranceJoinDate ");
            sb.AppendLine("       ,CommutingMinute1 = source.CommutingMinute1 ");
            sb.AppendLine("       ,CommutingMinute2 = source.CommutingMinute2 ");
            sb.AppendLine("       ,CommutingMinute3 = source.CommutingMinute3 ");
            sb.AppendLine("       ,CommutingMinute4 = source.CommutingMinute4 ");
            sb.AppendLine("       ,HouseKbn = source.HouseKbn ");
            sb.AppendLine("       ,DriverElectionDate = source.DriverElectionDate ");
            sb.AppendLine("       ,DriverRetireDate = source.DriverRetireDate ");
            sb.AppendLine("       ,DriverRetireReason = source.DriverRetireReason ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,RetireDate = source.RetireDate ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 発着地とトラDON_発着地のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergePointSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Point AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Point AS source ");
            sb.AppendLine("    ON ( target.PointId = source.PointId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        PointId ");
            sb.AppendLine("       ,PointCd ");
            sb.AppendLine("       ,PointNM ");
            sb.AppendLine("       ,PointNMK ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.PointId ");
            sb.AppendLine("       ,source.PointCd ");
            sb.AppendLine("       ,source.PointNM ");
            sb.AppendLine("       ,source.PointNMK ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 発着地とトラDON_発着地をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergePointUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Point AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Point AS source ");
            sb.AppendLine("    ON ( target.PointId = source.PointId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        PointId = source.PointId ");
            sb.AppendLine("       ,PointCd = source.PointCd ");
            sb.AppendLine("       ,PointNM = source.PointNM ");
            sb.AppendLine("       ,PointNMK = source.PointNMK ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 品目とトラDON_品目のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeItemSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Item AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Item AS source ");
            sb.AppendLine("    ON ( target.ItemId = source.ItemId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        ItemId ");
            sb.AppendLine("       ,ItemCd ");
            sb.AppendLine("       ,ItemNM ");
            sb.AppendLine("       ,ItemNMK ");
            sb.AppendLine("       ,Weight ");
            sb.AppendLine("       ,FigId ");
            sb.AppendLine("       ,ItemTaxKbn ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.ItemId ");
            sb.AppendLine("       ,source.ItemCd ");
            sb.AppendLine("       ,source.ItemNM ");
            sb.AppendLine("       ,source.ItemNMK ");
            sb.AppendLine("       ,source.Weight ");
            sb.AppendLine("       ,source.FigId ");
            sb.AppendLine("       ,source.ItemTaxKbn ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 品目とトラDON_品目をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeItemUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Item AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Item AS source ");
            sb.AppendLine("    ON ( target.ItemId = source.ItemId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        ItemId = source.ItemId ");
            sb.AppendLine("       ,ItemCd = source.ItemCd ");
            sb.AppendLine("       ,ItemNM = source.ItemNM ");
            sb.AppendLine("       ,ItemNMK = source.ItemNMK ");
            sb.AppendLine("       ,Weight = source.Weight ");
            sb.AppendLine("       ,FigId = source.FigId ");
            sb.AppendLine("       ,ItemTaxKbn = source.ItemTaxKbn ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 単価とトラDON_単価のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeAtPriceSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_AtPrice AS target ");
            sb.AppendLine(" USING VIEW_TORADON_AtPrice AS source ");
            sb.AppendLine("    ON ( target.AtPriceId = source.AtPriceId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        AtPriceId ");
            sb.AppendLine("       ,TokuisakiId ");
            sb.AppendLine("       ,ItemId ");
            sb.AppendLine("       ,StartPointId ");
            sb.AppendLine("       ,EndPointId ");
            sb.AppendLine("       ,FigId ");
            sb.AppendLine("       ,CarKindId ");
            sb.AppendLine("       ,AtPrice ");
            sb.AppendLine("       ,EstablishmentFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.AtPriceId ");
            sb.AppendLine("       ,source.TokuisakiId ");
            sb.AppendLine("       ,source.ItemId ");
            sb.AppendLine("       ,source.StartPointId ");
            sb.AppendLine("       ,source.EndPointId ");
            sb.AppendLine("       ,source.FigId ");
            sb.AppendLine("       ,source.CarKindId ");
            sb.AppendLine("       ,source.AtPrice ");
            sb.AppendLine("       ,source.EstablishmentFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 単価とトラDON_単価をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeAtPriceUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_AtPrice AS target ");
            sb.AppendLine(" USING VIEW_TORADON_AtPrice AS source ");
            sb.AppendLine("    ON ( target.AtPriceId = source.AtPriceId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        AtPriceId = source.AtPriceId ");
            sb.AppendLine("       ,TokuisakiId = source.TokuisakiId ");
            sb.AppendLine("       ,ItemId = source.ItemId ");
            sb.AppendLine("       ,StartPointId = source.StartPointId ");
            sb.AppendLine("       ,EndPointId = source.EndPointId ");
            sb.AppendLine("       ,FigId = source.FigId ");
            sb.AppendLine("       ,CarKindId = source.CarKindId ");
            sb.AppendLine("       ,AtPrice = source.AtPrice ");
            sb.AppendLine("       ,EstablishmentFlag = source.EstablishmentFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 単位とトラDON_単位のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeFigSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Fig AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Fig AS source ");
            sb.AppendLine("    ON ( target.FigId = source.FigId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        FigId ");
            sb.AppendLine("       ,FigCd ");
            sb.AppendLine("       ,FigNM ");
            sb.AppendLine("       ,TimeKbn ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.FigId ");
            sb.AppendLine("       ,source.FigCd ");
            sb.AppendLine("       ,source.FigNM ");
            sb.AppendLine("       ,source.TimeKbn ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 単位とトラDON_単位をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeFigUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Fig AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Fig AS source ");
            sb.AppendLine("    ON ( target.FigId = source.FigId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        FigId = source.FigId ");
            sb.AppendLine("       ,FigCd = source.FigCd ");
            sb.AppendLine("       ,FigNM = source.FigNM ");
            sb.AppendLine("       ,TimeKbn = source.TimeKbn ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 備考とトラDON_備考のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeMemoSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Memo AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Memo AS source ");
            sb.AppendLine("    ON ( target.MemoId = source.MemoId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        MemoId ");
            sb.AppendLine("       ,MemoCd ");
            sb.AppendLine("       ,MemoNM ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.MemoId ");
            sb.AppendLine("       ,source.MemoCd ");
            sb.AppendLine("       ,source.MemoNM ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 備考とトラDON_備考をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeMemoUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Memo AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Memo AS source ");
            sb.AppendLine("    ON ( target.MemoId = source.MemoId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        MemoId = source.MemoId ");
            sb.AppendLine("       ,MemoCd = source.MemoCd ");
            sb.AppendLine("       ,MemoNM = source.MemoNM ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 経費科目とトラDON_経費科目のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCostAccountSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_CostAccount AS target ");
            sb.AppendLine(" USING VIEW_TORADON_CostAccount AS source ");
            sb.AppendLine("    ON ( target.CostAccountId = source.CostAccountId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        CostAccountId ");
            sb.AppendLine("       ,CostAccountCd ");
            sb.AppendLine("       ,CostAccountNM ");
            sb.AppendLine("       ,CostKbn ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.CostAccountId ");
            sb.AppendLine("       ,source.CostAccountCd ");
            sb.AppendLine("       ,source.CostAccountNM ");
            sb.AppendLine("       ,source.CostKbn ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 経費科目とトラDON_経費科目をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeCostAccountUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_CostAccount AS target ");
            sb.AppendLine(" USING VIEW_TORADON_CostAccount AS source ");
            sb.AppendLine("    ON ( target.CostAccountId = source.CostAccountId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        CostAccountId = source.CostAccountId ");
            sb.AppendLine("       ,CostAccountCd = source.CostAccountCd ");
            sb.AppendLine("       ,CostAccountNM = source.CostAccountNM ");
            sb.AppendLine("       ,CostKbn = source.CostKbn ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 請負とトラDON_請負のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeContractSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Contract AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Contract AS source ");
            sb.AppendLine("    ON ( target.ContractId = source.ContractId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        ContractId ");
            sb.AppendLine("       ,TokuisakiId ");
            sb.AppendLine("       ,ClmClassId ");
            sb.AppendLine("       ,ContractCd ");
            sb.AppendLine("       ,ContractNM ");
            sb.AppendLine("       ,ContractStartDate ");
            sb.AppendLine("       ,ContractEndDate ");
            sb.AppendLine("       ,AddUpDate ");
            sb.AppendLine("       ,ContractPrice ");
            sb.AppendLine("       ,PriceInContractPrice ");
            sb.AppendLine("       ,TollFeeInContractPrice ");
            sb.AppendLine("       ,TaxDispKbn ");
            sb.AppendLine("       ,EndKbn ");
            sb.AppendLine("       ,Memo ");
            sb.AppendLine("       ,PriceOutTaxCalc ");
            sb.AppendLine("       ,PriceOutTax ");
            sb.AppendLine("       ,PriceInTaxCalc ");
            sb.AppendLine("       ,PriceInTax ");
            sb.AppendLine("       ,PriceNoTaxCalc ");
            sb.AppendLine("       ,ClmFixDate ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.ContractId ");
            sb.AppendLine("       ,source.TokuisakiId ");
            sb.AppendLine("       ,source.ClmClassId ");
            sb.AppendLine("       ,source.ContractCd ");
            sb.AppendLine("       ,source.ContractNM ");
            sb.AppendLine("       ,source.ContractStartDate ");
            sb.AppendLine("       ,source.ContractEndDate ");
            sb.AppendLine("       ,source.AddUpDate ");
            sb.AppendLine("       ,source.ContractPrice ");
            sb.AppendLine("       ,source.PriceInContractPrice ");
            sb.AppendLine("       ,source.TollFeeInContractPrice ");
            sb.AppendLine("       ,source.TaxDispKbn ");
            sb.AppendLine("       ,source.EndKbn ");
            sb.AppendLine("       ,source.Memo ");
            sb.AppendLine("       ,source.PriceOutTaxCalc ");
            sb.AppendLine("       ,source.PriceOutTax ");
            sb.AppendLine("       ,source.PriceInTaxCalc ");
            sb.AppendLine("       ,source.PriceInTax ");
            sb.AppendLine("       ,source.PriceNoTaxCalc ");
            sb.AppendLine("       ,source.ClmFixDate ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 請負とトラDON_請負をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeContractUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Contract AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Contract AS source ");
            sb.AppendLine("    ON ( target.ContractId = source.ContractId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        ContractId = source.ContractId ");
            sb.AppendLine("       ,TokuisakiId = source.TokuisakiId ");
            sb.AppendLine("       ,ClmClassId = source.ClmClassId ");
            sb.AppendLine("       ,ContractCd = source.ContractCd ");
            sb.AppendLine("       ,ContractNM = source.ContractNM ");
            sb.AppendLine("       ,ContractStartDate = source.ContractStartDate ");
            sb.AppendLine("       ,ContractEndDate = source.ContractEndDate ");
            sb.AppendLine("       ,AddUpDate = source.AddUpDate ");
            sb.AppendLine("       ,ContractPrice = source.ContractPrice ");
            sb.AppendLine("       ,PriceInContractPrice = source.PriceInContractPrice ");
            sb.AppendLine("       ,TollFeeInContractPrice = source.TollFeeInContractPrice ");
            sb.AppendLine("       ,TaxDispKbn = source.TaxDispKbn ");
            sb.AppendLine("       ,EndKbn = source.EndKbn ");
            sb.AppendLine("       ,Memo = source.Memo ");
            sb.AppendLine("       ,PriceOutTaxCalc = source.PriceOutTaxCalc ");
            sb.AppendLine("       ,PriceOutTax = source.PriceOutTax ");
            sb.AppendLine("       ,PriceInTaxCalc = source.PriceInTaxCalc ");
            sb.AppendLine("       ,PriceInTax = source.PriceInTax ");
            sb.AppendLine("       ,PriceNoTaxCalc = source.PriceNoTaxCalc ");
            sb.AppendLine("       ,ClmFixDate = source.ClmFixDate ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        /// <summary>
        /// 荷主とトラDON_荷主のマージSQL（INSERT、DELETE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeOwnerSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Owner AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Owner AS source ");
            sb.AppendLine("    ON ( target.OwnerId = source.OwnerId) ");
            sb.AppendLine("  WHEN NOT MATCHED THEN ");
            sb.AppendLine("INSERT  ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        OwnerId ");
            sb.AppendLine("       ,OwnerCd ");
            sb.AppendLine("       ,OwnerNM ");
            sb.AppendLine("       ,OwnerSNM ");
            sb.AppendLine("       ,OwnerNMK ");
            sb.AppendLine("       ,Postal ");
            sb.AppendLine("       ,Address1 ");
            sb.AppendLine("       ,Address2 ");
            sb.AppendLine("       ,Tel ");
            sb.AppendLine("       ,Fax ");
            sb.AppendLine("       ,DisableFlag ");
            sb.AppendLine("       ,DelFlag ");
            sb.AppendLine("       ,EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("      ( ");
            sb.AppendLine("        source.OwnerId ");
            sb.AppendLine("       ,source.OwnerCd ");
            sb.AppendLine("       ,source.OwnerNM ");
            sb.AppendLine("       ,source.OwnerSNM ");
            sb.AppendLine("       ,source.OwnerNMK ");
            sb.AppendLine("       ,source.Postal ");
            sb.AppendLine("       ,source.Address1 ");
            sb.AppendLine("       ,source.Address2 ");
            sb.AppendLine("       ,source.Tel ");
            sb.AppendLine("       ,source.Fax ");
            sb.AppendLine("       ,source.DisableFlag ");
            sb.AppendLine("       ,source.DelFlag ");
            sb.AppendLine("       ,source.EntryDateTime ");
            sb.AppendLine("      ) ");
            sb.AppendLine("  WHEN NOT MATCHED BY SOURCE THEN ");
            sb.AppendLine("    DELETE; ");

            return sb.ToString();
        }

        /// <summary>
        /// 荷主とトラDON_荷主をマージSQL（UPDATE）を取得します。
        /// </summary>
        /// <returns>マージSQL</returns>
        private string GetMergeOwnerUpdateSql()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE ");
            sb.AppendLine("  INTO TORADON_Owner AS target ");
            sb.AppendLine(" USING VIEW_TORADON_Owner AS source ");
            sb.AppendLine("    ON ( target.OwnerId = source.OwnerId AND target.EntryDateTime <> source.EntryDateTime) ");
            sb.AppendLine("  WHEN MATCHED THEN ");
            sb.AppendLine("UPDATE ");
            sb.AppendLine("   SET ");
            sb.AppendLine("        OwnerId = source.OwnerId ");
            sb.AppendLine("       ,OwnerCd = source.OwnerCd ");
            sb.AppendLine("       ,OwnerNM = source.OwnerNM ");
            sb.AppendLine("       ,OwnerSNM = source.OwnerSNM ");
            sb.AppendLine("       ,OwnerNMK = source.OwnerNMK ");
            sb.AppendLine("       ,Postal = source.Postal ");
            sb.AppendLine("       ,Address1 = source.Address1 ");
            sb.AppendLine("       ,Address2 = source.Address2 ");
            sb.AppendLine("       ,Tel = source.Tel ");
            sb.AppendLine("       ,Fax = source.Fax ");
            sb.AppendLine("       ,DisableFlag = source.DisableFlag ");
            sb.AppendLine("       ,DelFlag = source.DelFlag ");
            sb.AppendLine("       ,EntryDateTime = source.EntryDateTime ");
            sb.AppendLine("    ; ");

            return sb.ToString();
        }

        #endregion
    }
}
