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
    /// 得意先グループ明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class TokuisakiGroupMeisai
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
        /// 得意先グループクラスのデフォルトコンストラクタです。
        /// </summary>
        public TokuisakiGroupMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public TokuisakiGroupMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// 検索条件情報を指定して、得意先グループ情報一覧を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>得意先グループ情報一覧</returns>
        public IList<TokuisakiGroupMeisaiInfo> GetList(TokuisakiGroupMeisaiSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }
        
        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 得意先グループ情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>得意先グループ情報一覧</returns>
        private IList<TokuisakiGroupMeisaiInfo> GetListInternal(SqlTransaction transaction, TokuisakiGroupMeisaiSearchParameter para)
        {
            //返却用の一覧
            List<TokuisakiGroupMeisaiInfo> rt_list = new List<TokuisakiGroupMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 TokuisakiGroupMeisai.* ");
            sb.AppendLine("	,ToraDONTokuisaki.TokuisakiCd ToraDONTokuisakiCode ");
            sb.AppendLine("	,ToraDONTokuisaki.TokuisakiNM ToraDONTokuisakiName ");
            sb.AppendLine("	,ToraDONTokuisaki.DisableFlag ToraDONDisableFlag ");

            sb.AppendLine("FROM ");
            sb.AppendLine(" TokuisakiGroupMeisai ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" TokuisakiGroup ");
            sb.AppendLine(" ON TokuisakiGroup.TokuisakiGroupId = TokuisakiGroupMeisai.TokuisakiGroupId ");
            sb.AppendLine(" AND TokuisakiGroup.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki ToraDONTokuisaki ");
            sb.AppendLine(" ON ToraDONTokuisaki.TokuisakiId = TokuisakiGroupMeisai.ToraDONTokuisakiId ");
            sb.AppendLine(" AND ToraDONTokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1 ");

            if (para != null)
            {
                if (para.TokuisakiGroupId.HasValue)
                {
                    sb.AppendLine(" AND TokuisakiGroupMeisai.TokuisakiGroupId = " + para.TokuisakiGroupId.ToString() + " ");
                }
                if (para.Gyo.HasValue)
                {
                    sb.AppendLine(" AND TokuisakiGroupMeisai.Gyo = " + para.Gyo.ToString() + " ");
                }
                if (para.ToraDONTokuisakiId.HasValue)
                {
                    sb.AppendLine(" AND TokuisakiGroupMeisai.ToraDONTokuisakiId = " + para.ToraDONTokuisakiId.ToString() + " ");
                }
                if (para.ToraDONDisableFlag.HasValue)
                {
                    sb.AppendLine(" AND ToraDONTokuisaki.DisableFlag = " + (NSKUtil.BoolToInt(para.ToraDONDisableFlag.Value)).ToString() + " ");
                }
            }

            #endregion

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("	TokuisakiGroupMeisai.TokuisakiGroupId, TokuisakiGroupMeisai.Gyo ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                TokuisakiGroupMeisaiInfo rt_info = new TokuisakiGroupMeisaiInfo
                {
                    TokuisakiGroupId = SQLServerUtil.dbDecimal(rdr["TokuisakiGroupId"]),
                    Gyo = SQLServerUtil.dbInt(rdr["Gyo"]),
                    ToraDONTokuisakiId = SQLServerUtil.dbDecimal(rdr["ToraDONTokuisakiId"]),

                    ToraDONTokuisakiCode = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiCode"]),
                    ToraDONTokuisakiName = rdr["ToraDONTokuisakiName"].ToString(),

                    ToraDONDisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["ToraDONDisableFlag"])),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
