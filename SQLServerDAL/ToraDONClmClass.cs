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
    /// トラDON請求部門テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDONClmClass
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
        /// 請求部門クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDONClmClass()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、請求部門テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDONClmClass(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定でトラDON営業所情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ToraDONClmClassInfo GetInfo(int code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ToraDONClmClassSearchParameter()
            {
                ClmClassCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、トラDON営業所情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON営業所情報のリスト</returns>
        public IList<ToraDONClmClassInfo> GetList(ToraDONClmClassSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// トラDON請求部門情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON請求部門情報のコンボボックス用リスト</returns>
        public IList<ToraDONClmClassInfo> GetComboList()
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<ToraDONClmClassInfo> GetListInternal(SqlTransaction transaction, ToraDONClmClassSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONClmClassInfo> rt_list = new List<ToraDONClmClassInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ClmClass.* ");
            sb.AppendLine("	,Tokuisaki.TokuisakiCd ");
            sb.AppendLine("	,Tokuisaki.TokuisakiNM ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_ClmClass ClmClass ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki Tokuisaki ");
            sb.AppendLine(" ON  Tokuisaki.TokuisakiId = ClmClass.TokuisakiId ");
            sb.AppendLine(" AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	ClmClass.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ClmClassId.HasValue)
                {
                    sb.AppendLine("AND ClmClass.ClmClassId = " + para.ClmClassId.ToString() + " ");
                }
                if (para.ClmClassCode.HasValue)
                {
                    sb.AppendLine("AND ClmClass.ClmClassCd = " + para.ClmClassCode.ToString() + " ");
                }
                if (para.TokuisakiId.HasValue)
                {
                    sb.AppendLine("AND ClmClass.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }

                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND ClmClass.DisableFlag = " + NSKUtil.BoolToInt((bool)para.DisableFlag) + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ClmClass.ClmClassCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONClmClassInfo rt_info = new ToraDONClmClassInfo();
                rt_info.ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]);
                rt_info.ClmClassCode = SQLServerUtil.dbInt(rdr["ClmClassCd"]);
                rt_info.ClmClassName = rdr["ClmClassNM"].ToString();
                rt_info.ClmClassShortName = rdr["ClmClassSNM"].ToString();
                rt_info.ClmClassNameKana = rdr["ClmClassNMK"].ToString();
                rt_info.Postal = rdr["Postal"].ToString();
                rt_info.Address1 = rdr["Address1"].ToString();
                rt_info.Address2 = rdr["Address2"].ToString();
                rt_info.Tel = rdr["Tel"].ToString();
                rt_info.Fax = rdr["Fax"].ToString();
                rt_info.ClmPRTKbn = SQLServerUtil.dbInt(rdr["ClmPRTKbn"]);
                rt_info.ClmAddressPRTKbn = SQLServerUtil.dbInt(rdr["ClmAddressPRTKbn"]);
                rt_info.DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]));
                rt_info.TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]);
                rt_info.TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.TokuisakiName = rdr["TokuisakiNM"].ToString();

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }
        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// トラDON請求部門情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON請求部門情報のコンボボックス用リスト</returns>
        public IList<ToraDONClmClassInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ToraDONClmClassInfo> rt_list = new List<ToraDONClmClassInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	ClmClass.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_ClmClass ClmClass ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	ClmClass.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	ClmClass.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" ClmClass.ClmClassCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONClmClassInfo rt_info = new ToraDONClmClassInfo
                {
                    ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]),
                    ClmClassCode = SQLServerUtil.dbInt(rdr["ClmClassCd"]),
                    ClmClassName = rdr["ClmClassNM"].ToString(),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
