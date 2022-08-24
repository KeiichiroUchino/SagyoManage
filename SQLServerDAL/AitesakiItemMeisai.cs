using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using System.Windows.Forms;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.BizProperty;
using System.Configuration;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 相手先品目明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class AitesakiItemMeisai
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
        /// 相手先品目クラスのデフォルトコンストラクタです。
        /// </summary>
        public AitesakiItemMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public AitesakiItemMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// 検索条件情報を指定して、相手先品目情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>相手先品目情報一覧</returns>
        public IList<AitesakiItemMeisaiInfo> GetList(AitesakiItemMeisaiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 相手先品目情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>相手先品目情報一覧</returns>
        private IList<AitesakiItemMeisaiInfo> GetListInternal(SqlTransaction transaction, AitesakiItemMeisaiSearchParameter para)
        {
            //返却用の一覧
            List<AitesakiItemMeisaiInfo> rt_list = new List<AitesakiItemMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 AitesakiItemMeisai.* ");
            sb.AppendLine("	,ToraDONItem.ItemCd ToraDONItemCd ");
            sb.AppendLine("	,ToraDONItem.ItemNM ToraDONItemNM ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" AitesakiItemMeisai ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" HikitoriKojo HikitoriKojo ");
            sb.AppendLine("ON HikitoriKojo.HikitoriKojoId = AitesakiItemMeisai.HikitoriKojoId ");
            sb.AppendLine("AND HikitoriKojo.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Item ToraDONItem ");
            sb.AppendLine("ON ToraDONItem.ItemId = AitesakiItemMeisai.ToraDONItemId ");
            sb.AppendLine("AND ToraDONItem.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1 ");

            if (para != null)
            {
                if (para.HikitoriKojoId != null)
                {
                    sb.AppendLine("	--引数「引取工場Id」 ");
                    sb.AppendLine(" AND AitesakiItemMeisai.HikitoriKojoId = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.HikitoriKojoId)) + " ");
                }
                if (para.ToraDONItemId != null)
                {
                    sb.AppendLine("	--引数「品目Id」 ");
                    sb.AppendLine(" AND ToraDONItem.ItemId = " + SQLHelper.GetSanitaizingSqlString(Convert.ToString(para.ToraDONItemId)) + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	HikitoriKojo.HikitoriKojoCd, ToraDONItem.ItemCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                AitesakiItemMeisaiInfo rt_info = new AitesakiItemMeisaiInfo
                {
                    HikitoriKojoId = SQLServerUtil.dbDecimal(rdr["HikitoriKojoId"]),
                    ToraDONItemId = SQLServerUtil.dbDecimal(rdr["ToraDONItemId"]),
                    AitesakiItemCode = rdr["AitesakiItemCd"].ToString(),

                    ToraDONItemCode = SQLServerUtil.dbInt(rdr["ToraDONItemCd"]),
                    ToraDONItemName = rdr["ToraDONItemNM"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
