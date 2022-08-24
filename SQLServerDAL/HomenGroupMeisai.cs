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
    /// 方面グループ明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class HomenGroupMeisai
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
        /// 方面グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public HomenGroupMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HomenGroupMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// 検索条件情報を指定して、方面グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>方面グループ情報一覧</returns>
        public IList<HomenGroupMeisaiInfo> GetList(HomenGroupMeisaiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 方面グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>方面グループ情報一覧</returns>
        private IList<HomenGroupMeisaiInfo> GetListInternal(SqlTransaction transaction, HomenGroupMeisaiSearchParameter para)
        {
            //返却用の一覧
            List<HomenGroupMeisaiInfo> rt_list = new List<HomenGroupMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 HomenGroupMeisai.* ");
            sb.AppendLine("	,Homen.HomenCode HomenCode ");
            sb.AppendLine("	,Homen.HomenName HomenName ");
            sb.AppendLine("	,Homen.DisableFlag DisableFlag ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" HomenGroupMeisai ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" HomenGroup ");
            sb.AppendLine(" ON HomenGroup.HomenGroupId = HomenGroupMeisai.HomenGroupId ");
            sb.AppendLine(" AND HomenGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT JOIN ");
            sb.AppendLine(" Homen ");
            sb.AppendLine(" ON Homen.HomenId = HomenGroupMeisai.HomenId ");
            sb.AppendLine(" AND Homen.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	(HomenGroupMeisai.HomenId = 0 OR Homen.HomenId IS NOT NULL) ");

            if (para != null)
            {
                if (para.HomenGroupId.HasValue)
                {
                    sb.AppendLine(" AND HomenGroupMeisai.HomenGroupId = " + para.HomenGroupId.ToString() + " ");
                }
                if (para.Gyo.HasValue)
                {
                    sb.AppendLine(" AND HomenGroupMeisai.Gyo = " + para.Gyo.ToString() + " ");
                }
                if (para.HomenId.HasValue)
                {
                    sb.AppendLine(" AND HomenGroupMeisai.HomenId = " + para.HomenId.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine(" AND Homen.DisableFlag = " + (NSKUtil.BoolToInt(para.DisableFlag.Value)).ToString() + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	HomenGroupMeisai.HomenGroupId, HomenGroupMeisai.Gyo ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HomenGroupMeisaiInfo rt_info = new HomenGroupMeisaiInfo
                {
                    HomenGroupId = SQLServerUtil.dbDecimal(rdr["HomenGroupId"]),
                    Gyo = SQLServerUtil.dbInt(rdr["Gyo"]),
                    HomenId = SQLServerUtil.dbDecimal(rdr["HomenId"]),

                    HomenCode = SQLServerUtil.dbInt(rdr["HomenCode"]),
                    HomenName = rdr["HomenName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
