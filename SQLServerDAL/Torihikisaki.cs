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
    /// 取引先（トラDON）テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Torihikisaki
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
        /// 取引先クラスのデフォルトコンストラクタです。
        /// </summary>
        public Torihikisaki()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、取引先テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Torihikisaki(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定で取引先情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns></returns>
        public TorihikisakiInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new TorihikisakiSearchParameter()
            {
                ToraDONTorihikiCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、取引先情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>取引先情報のリスト</returns>
        public IList<TorihikisakiInfo> GetList(TorihikisakiSearchParameter para = null, SqlTransaction transaction = null)
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
        public IList<TorihikisakiInfo> GetListInternal(SqlTransaction transaction, TorihikisakiSearchParameter para)
        {
            //返却用のリスト
            List<TorihikisakiInfo> rt_list = new List<TorihikisakiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 * ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Torihikisaki AS ToraDONTorihikisaki ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	ToraDONTorihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ToraDONTorihikiId.HasValue)
                {
                    sb.AppendLine("AND ToraDONTorihikisaki.TorihikiId = " + para.ToraDONTorihikiId.ToString() + " ");
                }
                if (para.ToraDONTorihikiCode.HasValue)
                {
                    sb.AppendLine("AND ToraDONTorihikisaki.TorihikiCd = " + para.ToraDONTorihikiCode.ToString() + " ");
                }
                if (para.CharterKbn.HasValue)
                {
                    sb.AppendLine("AND ToraDONTorihikisaki.CharterKbn = " + para.CharterKbn.ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ToraDONTorihikisaki.TorihikiCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                TorihikisakiInfo rt_info = new TorihikisakiInfo
                {
                    ToraDONTorihikiId = SQLServerUtil.dbDecimal(rdr["TorihikiId"]),
                    ToraDONTorihikiCode = SQLServerUtil.dbInt(rdr["TorihikiCd"]),
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                    ToraDONTorihikiName = rdr["TorihikiNM"].ToString(),
                    ToraDONTorihikiShortName = rdr["TorihikiSNM"].ToString(),
                    ToraDONTorihikiNameKana = rdr["TorihikiNMK"].ToString(),
                    HonorificTitle = rdr["HonorificTitle"].ToString(),
                    Postal = rdr["Postal"].ToString(),
                    Address1 = rdr["Address1"].ToString(),
                    Address2 = rdr["Address2"].ToString(),
                    Tel = rdr["Tel"].ToString(),
                    Fax = rdr["Fax"].ToString(),
                    TorihikiStaffName = rdr["TorihikiStaffNM"].ToString(),
                    Url = rdr["Url"].ToString(),
                    MailAddress = rdr["MailAddress"].ToString(),
                    Memo = rdr["Memo"].ToString(),
                    CharterKbn = SQLServerUtil.dbInt(rdr["CharterKbn"]),
                    PayeeId = SQLServerUtil.dbDecimal(rdr["PayeeId"]),
                    FixDay1 = SQLServerUtil.dbInt(rdr["FixDay1"]),
                    PaymentMonth1 = SQLServerUtil.dbDecimal(rdr["PaymentMonth1"]),
                    PaymentDay1 = SQLServerUtil.dbDecimal(rdr["PaymentDay1"]),
                    FixDay2 = SQLServerUtil.dbInt(rdr["FixDay2"]),
                    PaymentMonth2 = SQLServerUtil.dbDecimal(rdr["PaymentMonth2"]),
                    PaymentDay2 = SQLServerUtil.dbDecimal(rdr["PaymentDay2"]),
                    FixDay3 = SQLServerUtil.dbInt(rdr["FixDay3"]),
                    PaymentMonth3 = SQLServerUtil.dbDecimal(rdr["PaymentMonth3"]),
                    PaymentDay3 = SQLServerUtil.dbDecimal(rdr["PaymentDay3"]),
                    PaymentGak1 = SQLServerUtil.dbDecimal(rdr["PaymentGak1"]),
                    PaymentKbn11 = SQLServerUtil.dbInt(rdr["PaymentKbn11"]),
                    PaymentRate11 = SQLServerUtil.dbDecimal(rdr["PaymentRate11"]),
                    PaymentKbn12 = SQLServerUtil.dbInt(rdr["PaymentKbn12"]),
                    PaymentRate12 = SQLServerUtil.dbDecimal(rdr["PaymentRate12"]),
                    PaymentGak2 = SQLServerUtil.dbDecimal(rdr["PaymentGak2"]),
                    PaymentKbn21 = SQLServerUtil.dbInt(rdr["PaymentKbn21"]),
                    PaymentRate21 = SQLServerUtil.dbDecimal(rdr["PaymentRate21"]),
                    PaymentKbn22 = SQLServerUtil.dbInt(rdr["PaymentKbn22"]),
                    PaymentRate22 = SQLServerUtil.dbDecimal(rdr["PaymentRate22"]),
                    PaymentGak3 = SQLServerUtil.dbDecimal(rdr["PaymentGak3"]),
                    PaymentKbn31 = SQLServerUtil.dbInt(rdr["PaymentKbn31"]),
                    PaymentRate31 = SQLServerUtil.dbDecimal(rdr["PaymentRate31"]),
                    PaymentKbn32 = SQLServerUtil.dbInt(rdr["PaymentKbn32"]),
                    PaymentRate32 = SQLServerUtil.dbDecimal(rdr["PaymentRate32"]),
                    PayDtlStatePRTKbn = SQLServerUtil.dbInt(rdr["PayDtlStatePRTKbn"]),
                    PayDtlPRTKbn = SQLServerUtil.dbInt(rdr["PayDtlPRTKbn"]),
                    TaxActKbn = SQLServerUtil.dbInt(rdr["TaxActKbn"]),
                    TaxCutKbn = SQLServerUtil.dbInt(rdr["TaxCutKbn"]),
                    GakCutKbn = SQLServerUtil.dbInt(rdr["GakCutKbn"]),
                    CharterRate = SQLServerUtil.dbDecimal(rdr["CharterRate"]),
                    CompanyGSKbn = SQLServerUtil.dbInt(rdr["CompanyGSKbn"]),
                    SaleSlipToPayDayKbn = SQLServerUtil.dbInt(rdr["SaleSlipToPayDayKbn"]),
                    PayDtlDatePRTKbn = SQLServerUtil.dbInt(rdr["PayDtlDatePRTKbn"]),
                    FirstClmDay = NSKUtil.NumberToDateTime((long)SQLServerUtil.dbDecimal(rdr["FirstClmDay"])),
                    LastPayFixDay = NSKUtil.NumberToDateTime((long)SQLServerUtil.dbDecimal(rdr["LastPayFixDay"])),
                    RemainManageFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["RemainManageFlag"])),

                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"])),
                    ToraDONDelFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DelFlag"])),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
