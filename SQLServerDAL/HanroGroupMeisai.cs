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
    /// 販路グループ明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class HanroGroupMeisai
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
        /// 販路グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public HanroGroupMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HanroGroupMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// 検索条件情報を指定して、販路グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路グループ情報一覧</returns>
        public IList<HanroGroupMeisaiInfo> GetList(HanroGroupMeisaiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 販路グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>販路グループ情報一覧</returns>
        private IList<HanroGroupMeisaiInfo> GetListInternal(SqlTransaction transaction, HanroGroupMeisaiSearchParameter para)
        {
            //返却用の一覧
            List<HanroGroupMeisaiInfo> rt_list = new List<HanroGroupMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 HanroGroupMeisai.* ");
            sb.AppendLine("	,Hanro.HanroCode HanroCode ");
            sb.AppendLine("	,Hanro.HanroName HanroName ");
            sb.AppendLine("	,Hanro.DisableFlag DisableFlag ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" HanroGroupMeisai ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" HanroGroup ");
            sb.AppendLine(" ON HanroGroup.HanroGroupId = HanroGroupMeisai.HanroGroupId ");
            sb.AppendLine(" AND HanroGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT JOIN ");
            sb.AppendLine(" Hanro ");
            sb.AppendLine(" ON Hanro.HanroId = HanroGroupMeisai.HanroId ");
            sb.AppendLine(" AND Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	(HanroGroupMeisai.HanroId = 0 OR Hanro.HanroId IS NOT NULL) ");

            if (para != null)
            {
                if (para.HanroGroupId.HasValue)
                {
                    sb.AppendLine(" AND HanroGroupMeisai.HanroGroupId = " + para.HanroGroupId.ToString() + " ");
                }
                if (para.Gyo.HasValue)
                {
                    sb.AppendLine(" AND HanroGroupMeisai.Gyo = " + para.Gyo.ToString() + " ");
                }
                if (para.HanroId.HasValue)
                {
                    sb.AppendLine(" AND HanroGroupMeisai.HanroId = " + para.HanroId.ToString() + " ");
                }
                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine(" AND Hanro.DisableFlag = " + (NSKUtil.BoolToInt(para.DisableFlag.Value)).ToString() + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	HanroGroupMeisai.HanroGroupId, HanroGroupMeisai.Gyo ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HanroGroupMeisaiInfo rt_info = new HanroGroupMeisaiInfo
                {
                    HanroGroupId = SQLServerUtil.dbDecimal(rdr["HanroGroupId"]),
                    Gyo = SQLServerUtil.dbInt(rdr["Gyo"]),
                    HanroId = SQLServerUtil.dbDecimal(rdr["HanroId"]),

                    HanroCode = SQLServerUtil.dbInt(rdr["HanroCode"]),
                    HanroName = rdr["HanroName"].ToString(),

                    DisableFlag = SQLHelper.dbBit(rdr["DisableFlag"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
