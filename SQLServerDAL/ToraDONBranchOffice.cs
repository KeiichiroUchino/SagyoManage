using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.ComLib;
using System.Configuration;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// トラDON営業所テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDONBranchOffice
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
        /// トラDON営業所クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDONBranchOffice()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、トラDON営業所テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDONBranchOffice(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定でトラDON営業所情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONBranchOfficeInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONBranchOfficeSearchParameter()
            {
                BranchOfficeCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定でトラDON営業所情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONBranchOfficeInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONBranchOfficeSearchParameter()
            {
                BranchOfficeId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、トラDON営業所情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON営業所情報のリスト</returns>
        public IList<ToraDONBranchOfficeInfo> GetList(ToraDONBranchOfficeSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// トラDON営業所情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON営業所情報のコンボボックス用リスト</returns>
        public IList<ToraDONBranchOfficeInfo> GetComboList()
        {
            return GetComboListInternal(null);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<ToraDONBranchOfficeInfo> GetListInternal(SqlTransaction transaction, ToraDONBranchOfficeSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONBranchOfficeInfo> rt_list = new List<ToraDONBranchOfficeInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 BranchOffice.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_BranchOffice BranchOffice ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.BranchOfficeId.HasValue)
                {
                    sb.AppendLine("AND BranchOffice.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");
                }
                if (para.BranchOfficeCode.HasValue)
                {
                    sb.AppendLine("AND BranchOffice.BranchOfficeCd = " + para.BranchOfficeCode.ToString() + " ");
                }

                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND BranchOffice.DisableFlag = " + NSKUtil.BoolToInt((bool)para.DisableFlag) + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" BranchOffice.BranchOfficeCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONBranchOfficeInfo rt_info = new ToraDONBranchOfficeInfo()
                {
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                    BranchOfficeCode = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]),
                    BranchOfficeName = rdr["BranchOfficeNM"].ToString(),
                    BranchOfficeShortName = rdr["BranchOfficeSNM"].ToString(),
                    Postal = rdr["Postal"].ToString(),
                    Address1 = rdr["Address1"].ToString(),
                    Address2 = rdr["Address2"].ToString(),
                    Tel = rdr["Tel"].ToString(),
                    Fax = rdr["Fax"].ToString(),
                    Url = rdr["Url"].ToString(),
                    MailAddress = rdr["MailAddress"].ToString(),
                    Memo = rdr["Memo"].ToString(),
                    Account1 = rdr["Account1"].ToString(),
                    Account2 = rdr["Account2"].ToString(),
                    Account3 = rdr["Account3"].ToString(),
                    AccountSub1 = rdr["AccountSub1"].ToString(),
                    AccountSub2 = rdr["AccountSub2"].ToString(),
                    AccountSub3 = rdr["AccountSub3"].ToString(),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// トラDON営業所情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON営業所情報のコンボボックス用リスト</returns>
        private IList<ToraDONBranchOfficeInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ToraDONBranchOfficeInfo> rt_list = new List<ToraDONBranchOfficeInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	BranchOffice.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_BranchOffice BranchOffice ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	BranchOffice.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" BranchOffice.BranchOfficeCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONBranchOfficeInfo rt_info = new ToraDONBranchOfficeInfo
                {
                    BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]),
                    BranchOfficeCode = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]),
                    BranchOfficeName = rdr["BranchOfficeNM"].ToString(),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
