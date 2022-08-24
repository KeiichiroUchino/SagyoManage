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
    /// 初期値のデータアクセスレイヤです。
    /// </summary>
    public class DefaultSettings
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
        /// 初期値クラスのデフォルトコンストラクタです。
        /// </summary>
        public DefaultSettings()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public DefaultSettings(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリック メソッド

        /// <summary>
        /// コードを指定して、初期値情報を取得します。
        /// 引数が指定されていない場合はnullを返却します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>初期値情報</returns>
        public DefaultSettingsInfo GetInfo(SqlTransaction transaction = null)
        {
            DefaultSettingsInfo info = this.GetListInternal(transaction)
                .FirstOrDefault();

            return info;
        }

        #endregion

        #region プライベート メソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// 初期値情報一覧を取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>初期値情報一覧</returns>
        private IList<DefaultSettingsInfo> GetListInternal(SqlTransaction transaction)
        {
            //返却用の一覧
            List<DefaultSettingsInfo> rt_list = new List<DefaultSettingsInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("SELECT ");
            sb.AppendLine("	 DefaultSettings.* ");

            sb.AppendLine("FROM ");
            sb.AppendLine("	DefaultSettings --初期値 ");

            #region 抽出条件

            sb.AppendLine("WHERE ");
            sb.AppendLine("	DefaultSettingsId = 1 ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                DefaultSettingsInfo rt_info = new DefaultSettingsInfo
                {
                    DefaultSettingsId = SQLServerUtil.dbDecimal(rdr["DefaultSettingsId"]),
                    HN_EigyosyoShokichiKbn = SQLServerUtil.dbInt(rdr["HN_EigyosyoShokichiKbn"]),
                    HN_EigyosyoCarShokichiKbn = SQLServerUtil.dbInt(rdr["HN_EigyosyoCarShokichiKbn"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
