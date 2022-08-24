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
    /// LYNADBの車型テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ShagataForLyna
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
        /// LYNADBの車型クラスのデフォルトコンストラクタです。
        /// </summary>
        public ShagataForLyna()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ShagataForLyna(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// コード指定でLYNADBの車型情報を取得します。
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public ShagataForLynaInfo GetInfo(string code, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new ShagataForLynaSearchParameter()
            {
                ShagataCode = code,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、LYNADBの車型情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地情報のリスト</returns>
        public IList<ShagataForLynaInfo> GetList(ShagataForLynaSearchParameter para = null)
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
        public IList<ShagataForLynaInfo> GetListInternal(SqlTransaction transaction, ShagataForLynaSearchParameter para)
        {
            //返却用のリスト
            List<ShagataForLynaInfo> rt_list = new List<ShagataForLynaInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	車型.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	車型 ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1");

            if (para != null)
            {
                if (string.IsNullOrWhiteSpace(para.ShagataCode))
                {
                    sb.AppendLine(@"AND 車型.コード = """ + AccessHelper.GetSanitaizingSqlString(para.ShagataCode.Trim()) + @""" ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" 車型.コード ");

            String sql = sb.ToString();

            return AccessHelper.SimpleRead(sql, rdr =>
            {
                ShagataForLynaInfo info = new ShagataForLynaInfo();
                info.ShagataCode = rdr["コード"].ToString().Trim();
                info.ShagataName = rdr["名称"].ToString().Trim();
                info.KoteiCost = rdr["固定コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["固定コスト"].ToString());
                info.KyoriCost = rdr["距離コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["距離コスト"].ToString());
                info.JikanCost = rdr["時間コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["時間コスト"].ToString());
                info.ChokinJikanCost = rdr["超勤時間コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["超勤時間コスト"].ToString());
                info.Shoyu = rdr["所有"].ToString() == string.Empty ? 0 : Convert.ToInt32(rdr["所有"].ToString());
                info.NisabakiJikanHyo = rdr["荷捌き時間表"].ToString().Trim();
                info.Volume = rdr["体積"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["体積"].ToString());
                info.VolumeCost = rdr["体積コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["体積コスト"].ToString());
                info.Weight = rdr["重量"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["重量"].ToString());
                info.WeightCost = rdr["重量コスト"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["重量コスト"].ToString());
                info.DisableFlag = rdr["使用"].ToString().Equals("-1") ? false : true;

                return info;
            });
        }

        #endregion
    }
}
