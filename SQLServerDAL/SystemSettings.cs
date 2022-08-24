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
    /// システム設定のデータアクセスレイヤです。
    /// </summary>
    public class SystemSettings
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
        /// システム設定クラスのデフォルトコンストラクタです。
        /// </summary>
        public SystemSettings()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public SystemSettings(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// コードを指定して、システム設定情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>システム設定情報</returns>
        public SystemSettingsInfo GetInfo(SqlTransaction transaction = null)
        {
            SystemSettingsInfo info = this.GetListInternal(transaction)
                .FirstOrDefault();

            return info;
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// システム設定情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>システム設定情報一覧</returns>
        private IList<SystemSettingsInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用の一覧
            List<SystemSettingsInfo> rt_list = new List<SystemSettingsInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 SystemSettings.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	SystemSettings --システム設定 ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	SystemSettingsId = 1 ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                SystemSettingsInfo rt_info = new SystemSettingsInfo
                {
                    SystemSettingsId = SQLServerUtil.dbDecimal(rdr["SystemSettingsId"]),
                    FrameTitleBackColor = rdr["FrameTitleBackColor"].ToString(),
                    FrameFooterBackColor = rdr["FrameFooterBackColor"].ToString(),
                    FrameGridHeaderBackColor = rdr["FrameGridHeaderBackColor"].ToString(),
                    FrameGridSelectionBackColor = rdr["FrameGridSelectionBackColor"].ToString(),
                    HaishaNyuryokuJokenKbn = SQLServerUtil.dbInt(rdr["HaishaNyuryokuJokenKbn"]),
                    JuchuYMDCalculationKbn = SQLServerUtil.dbInt(rdr["JuchuYMDCalculationKbn"]),
                    EigyoshoKanriKbn = SQLServerUtil.dbInt(rdr["EigyoshoKanriKbn"]),
                    HaishaJuchuListSortKbn = SQLServerUtil.dbInt(rdr["HaishaJuchuListSortKbn"]),
                    HaishaKyujitsuCheckFlag = SQLHelper.dbBit(rdr["HaishaKyujitsuCheckFlag"]),
                    HaishaYMDCheckFlag = SQLHelper.dbBit(rdr["HaishaYMDCheckFlag"]),
                    HaishaYMDDefaultKbn = SQLServerUtil.dbInt(rdr["HaishaYMDDefaultKbn"]),
                    SeikyuRenkeiDailyReportKbn = SQLServerUtil.dbInt(rdr["SeikyuRenkeiDailyReportKbn"]),
                    SeikyuRenkeiUwagakiKbn = SQLServerUtil.dbInt(rdr["SeikyuRenkeiUwagakiKbn"]),
                    SendGridApiKey = rdr["SendGridApiKey"].ToString(),
                    TraDonVersionKbn = SQLServerUtil.dbInt(rdr["TraDonVersionKbn"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
