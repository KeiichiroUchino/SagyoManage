using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;
using System.Configuration;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// トラDON管理テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDonSystemProperty
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
        /// トラDON管理クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDonSystemProperty()
        {

        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDonSystemProperty(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// コードを指定して、トラDON管理情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <returns>トラDON管理情報</returns>
        public ToraDonSystemPropertyInfo GetInfo()
        {
            ToraDonSystemPropertyInfo info = this.GetListInternal(null)
                .FirstOrDefault();

            return info;
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 名称情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>名称情報一覧</returns>
        private IList<ToraDonSystemPropertyInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用の一覧
            List<ToraDonSystemPropertyInfo> rt_list = new List<ToraDonSystemPropertyInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("select ");
            sb.AppendLine("	ToraDONSystemProperty.* ");
            sb.AppendLine("	,DRBalanceCostAccountId1.CostAccountCd as CostAccountCd1 --経費科目1「コード」 ");
            sb.AppendLine("	,DRBalanceCostAccountId1.CostAccountNM as CostAccountNM1 --経費科目1「名称」 ");
            sb.AppendLine("	,DRBalanceCostAccountId2.CostAccountCd as CostAccountCd2 --経費科目2「コード」 ");
            sb.AppendLine("	,DRBalanceCostAccountId2.CostAccountNM as CostAccountNM2 --経費科目2「名称」 ");
            sb.AppendLine("	,DRBalanceCostAccountId3.CostAccountCd as CostAccountCd3 --経費科目3「コード」 ");
            sb.AppendLine("	,DRBalanceCostAccountId3.CostAccountNM as CostAccountNM3 --経費科目3「名称」 ");
            sb.AppendLine("	,DRBalanceCostAccountId4.CostAccountCd as CostAccountCd4 --経費科目4「コード」 ");
            sb.AppendLine("	,DRBalanceCostAccountId4.CostAccountNM as CostAccountNM4 --経費科目4「名称」 ");
            sb.AppendLine("	,DRBalanceCostAccountId5.CostAccountCd as CostAccountCd5 --経費科目5「コード」 ");
            sb.AppendLine("	,DRBalanceCostAccountId5.CostAccountNM as CostAccountNM5 --経費科目5「名称」 ");
            sb.AppendLine("from ");
            sb.AppendLine("	TORADON_SystemProperty ToraDONSystemProperty --管理 ");
            sb.AppendLine("left outer join ");
            sb.AppendLine("	TORADON_CostAccount as DRBalanceCostAccountId1 --経費科目1 ");
            sb.AppendLine("on	ToraDONSystemProperty.DRBalanceCostAccountId1 = DRBalanceCostAccountId1.CostAccountId ");
            sb.AppendLine("left outer join ");
            sb.AppendLine("	TORADON_CostAccount as DRBalanceCostAccountId2 --経費科目2 ");
            sb.AppendLine("on	ToraDONSystemProperty.DRBalanceCostAccountId2 = DRBalanceCostAccountId2.CostAccountId ");
            sb.AppendLine("left outer join ");
            sb.AppendLine("	TORADON_CostAccount as DRBalanceCostAccountId3 --経費科目3 ");
            sb.AppendLine("on	ToraDONSystemProperty.DRBalanceCostAccountId3 = DRBalanceCostAccountId3.CostAccountId ");
            sb.AppendLine("left outer join ");
            sb.AppendLine("	TORADON_CostAccount as DRBalanceCostAccountId4 --経費科目4 ");
            sb.AppendLine("on	ToraDONSystemProperty.DRBalanceCostAccountId4 = DRBalanceCostAccountId4.CostAccountId ");
            sb.AppendLine("left outer join ");
            sb.AppendLine("	TORADON_CostAccount as DRBalanceCostAccountId5 --経費科目5 ");
            sb.AppendLine("on	ToraDONSystemProperty.DRBalanceCostAccountId5 = DRBalanceCostAccountId5.CostAccountId ");
            sb.AppendLine("order by ");
            sb.AppendLine("	ToraDONSystemProperty.SystemPropertyId ");

            string mySql = sb.ToString();


            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDonSystemPropertyInfo rt_info = new ToraDonSystemPropertyInfo
                {
                    SystemPropertyId = SQLServerUtil.dbDecimal(rdr["SystemPropertyId"]),
                    CompanyName = rdr["CompanyNM"].ToString(),
                    ChiefName = rdr["ChiefNM"].ToString(),
                    Postal = rdr["Postal"].ToString(),
                    FirstAddress = rdr["Address1"].ToString(),
                    SecondAddress = rdr["Address2"].ToString(),
                    Tel = rdr["Tel"].ToString(),
                    Fax = rdr["Fax"].ToString(),
                    Url = rdr["Url"].ToString(),
                    MailAddress = rdr["MailAddress"].ToString(),
                    Memo = rdr["Memo"].ToString(),
                    FirstAccount = rdr["Account1"].ToString(),
                    SecondAccount = rdr["Account2"].ToString(),
                    ThirdAccount = rdr["Account3"].ToString(),
                    AccountPrintKbn = SQLServerUtil.dbInt(rdr["AccountPrintKbn"]),
                    TaxRate = SQLServerUtil.dbDecimal(rdr["TaxRate"]),
                    NewTaxRate = SQLServerUtil.dbDecimal(rdr["NewTaxRate"]),
                    NewTaxRateStartDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["NewTaxRateStartDate"])),
                    AccountStartDate =
                        NSKUtil.DecimalYYYYMMToDateTime(
                            SQLServerUtil.dbDecimal(rdr["AccountStartDate"])),
                    AccountSettlementDate =
                        NSKUtil.DecimalYYYYMMToDateTime(
                            SQLServerUtil.dbDecimal(rdr["AccountSettlementDate"])),
                    InputMaxDate =
                        NSKUtil.DecimalYYYYMMToDateTime(
                            SQLServerUtil.dbDecimal(rdr["InputMaxDate"])),
                    LogRemainDaySpan =
                        SQLServerUtil.dbInt(rdr["logRemainDaySpan"]),
                    LastSummaryOfMonthDate =
                        NSKUtil.DecimalYYYYMMToDateTime(
                            SQLServerUtil.dbDecimal(rdr["LastSUMOfMonthDate"])),

                    //集計取消可能日
                    CancelAccClmEnableDate =
                        NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["CancelAccClmEnableDate"])),
                    //給与締日
                    SalaryFixDay = SQLServerUtil.dbDecimal(rdr["SalaryFixDay"]),
                    BillComments = rdr["BillComments"].ToString(),
                    PayDtlComments = rdr["PayDtlComments"].ToString(),
                    DefaultSearchKanaMode =
                        (ToraDonSystemPropertyInfo.DefaultSearchKanaModeKbnItem)SQLServerUtil.dbInt(rdr["DefaultSearchKanaMode"]),
                    DefaultDailyDriveReportNumber =
                        (ToraDonSystemPropertyInfo.DefaultDailyDriveReportNumberIem)SQLServerUtil.dbInt(rdr["DefaultDailyDriveReportNumber"]),
                    DefaultDailyDriveReportStartDate =
                        (ToraDonSystemPropertyInfo.DefaultDailyDriveReportStartDateItem)SQLServerUtil.dbInt(rdr["DefaultDailyDriveReportStartDate"]),
                    ItemMustFlag =
                        NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ItemMustFlag"])),

                    DRBalanceCostAccountId1 = SQLServerUtil.dbDecimal(rdr["DRBalanceCostAccountId1"]),
                    DRBalanceCostAccountId2 = SQLServerUtil.dbDecimal(rdr["DRBalanceCostAccountId2"]),
                    DRBalanceCostAccountId3 = SQLServerUtil.dbDecimal(rdr["DRBalanceCostAccountId3"]),
                    DRBalanceCostAccountId4 = SQLServerUtil.dbDecimal(rdr["DRBalanceCostAccountId4"]),
                    DRBalanceCostAccountId5 = SQLServerUtil.dbDecimal(rdr["DRBalanceCostAccountId5"]),

                    DRBalanceCostAccountCd1 = SQLServerUtil.dbInt(rdr["CostAccountCd1"]),
                    DRBalanceCostAccountCd2 = SQLServerUtil.dbInt(rdr["CostAccountCd2"]),
                    DRBalanceCostAccountCd3 = SQLServerUtil.dbInt(rdr["CostAccountCd3"]),
                    DRBalanceCostAccountCd4 = SQLServerUtil.dbInt(rdr["CostAccountCd4"]),
                    DRBalanceCostAccountCd5 = SQLServerUtil.dbInt(rdr["CostAccountCd5"]),

                    DRBalanceCostAccountName1 = rdr["CostAccountNM1"].ToString(),
                    DRBalanceCostAccountName2 = rdr["CostAccountNM2"].ToString(),
                    DRBalanceCostAccountName3 = rdr["CostAccountNM3"].ToString(),
                    DRBalanceCostAccountName4 = rdr["CostAccountNM4"].ToString(),
                    DRBalanceCostAccountName5 = rdr["CostAccountNM5"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
