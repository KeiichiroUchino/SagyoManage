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
    /// トラDON荷主テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDONOwner
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
        /// トラDON荷主クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDONOwner()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、トラDON荷主テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDONOwner(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定でトラDON荷主情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONOwnerInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONOwnerSearchParameter()
            {
                OwnerCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// Id指定でトラDON荷主情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONOwnerInfo GetInfoById(decimal id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONOwnerSearchParameter()
            {
                OwnerId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、トラDON荷主情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON荷主情報のリスト</returns>
        public IList<ToraDONOwnerInfo> GetList(ToraDONOwnerSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// トラDON荷主情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON荷主情報のコンボボックス用リスト</returns>
        public IList<ToraDONOwnerInfo> GetComboList()
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
        private IList<ToraDONOwnerInfo> GetListInternal(SqlTransaction transaction, ToraDONOwnerSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONOwnerInfo> rt_list = new List<ToraDONOwnerInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 Owner.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Owner Owner ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Owner.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.OwnerId.HasValue)
                {
                    sb.AppendLine("AND Owner.OwnerId = " + para.OwnerId.ToString() + " ");
                }
                if (para.OwnerCode.HasValue)
                {
                    sb.AppendLine("AND Owner.OwnerCd = " + para.OwnerCode.ToString() + " ");
                }

                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND Owner.DisableFlag = " + NSKUtil.BoolToInt((bool)para.DisableFlag) + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Owner.OwnerCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONOwnerInfo rt_info = new ToraDONOwnerInfo()
                {
                    OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]),
                    OwnerCode = SQLServerUtil.dbInt(rdr["OwnerCd"]),
                    OwnerName = rdr["OwnerNM"].ToString(),
                    OwnerShortName = rdr["OwnerSNM"].ToString(),
                    OwnerNameKana = rdr["OwnerNMK"].ToString(),
                    Postal = rdr["Postal"].ToString(),
                    Address1 = rdr["Address1"].ToString(),
                    Address2 = rdr["Address2"].ToString(),
                    Tel = rdr["Tel"].ToString(),
                    Fax = rdr["Fax"].ToString(),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// トラDON荷主情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON荷主情報のコンボボックス用リスト</returns>
        private IList<ToraDONOwnerInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ToraDONOwnerInfo> rt_list = new List<ToraDONOwnerInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Owner.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Owner Owner ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Owner.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	Owner.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Owner.OwnerCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONOwnerInfo rt_info = new ToraDONOwnerInfo
                {
                    OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]),
                    OwnerCode = SQLServerUtil.dbInt(rdr["OwnerCd"]),
                    OwnerName = rdr["OwnerNM"].ToString(),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
