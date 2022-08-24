using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.Windows.Forms;
using System.Configuration;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.ComLib;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// システム名称クラスのビジネスロジック
    /// </summary>
    public class SystemName
    {
        /// <summary>
        /// 本クラスのインスタンスを取得します。
        /// </summary>
        public SystemName()
        {
        }

        #region ISystemNameDAL メンバ

        /// <summary>
        /// 区分とコードを指定して、システム名称情報を取得します。
        /// </summary>
        /// <param name="systemNameKbn">区分</param>
        /// <param name="systemNameCode">コード</param>
        /// <returns>システム名称情報</returns>
        public SystemNameInfo GetInfo(int systemNameKbn, int systemNameCode)
        {
            return
                this.GetListInternal(null, new SystemNameSearchParameter
                {
                    SystemNameKbn = systemNameKbn,
                    SystemNameCode = systemNameCode
                }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、システム名称情報のリストを取得します。
        /// </summary>
        /// <param name="systemNameKbn">区分/param>
        /// <returns>システム名称情報のリスト</returns>
        public IList<SystemNameInfo> GetList(int systemNameKbn = 0)
        {
            int? para_systemNameKbn = null;

            if (systemNameKbn != 0)
            {
                para_systemNameKbn = systemNameKbn;
            }

            return GetListInternal(null, new SystemNameSearchParameter { SystemNameKbn = para_systemNameKbn });
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        private IList<SystemNameInfo> GetListInternal(SqlTransaction transaction, SystemNameSearchParameter para = null)
        {
            //返却用のリスト
            List<SystemNameInfo> rt_list = new List<SystemNameInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("A.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("dbo.SystemName AS A ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("1 =1 ");

            if (para != null)
            {
                if (para.SystemNameKbn.HasValue)
                {
                    sb.AppendLine("AND A.SystemNameKbn = " +
                        para.SystemNameKbn.ToString() + " ");
                }

                if (para.SystemNameCode.HasValue)
                {
                    sb.AppendLine("AND A.SystemNameCode = " +
                        para.SystemNameCode.ToString() + " ");
                }

            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("A.[SystemNameKbn],A.[SystemNameCode] ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr => this.BuildInfoFromSqlDataReader(rdr), transaction);
        }

        /// <summary>
        /// SqlDataReaderを指定して、
        /// クラスのインスタンスを返却します。
        /// </summary>
        /// <param name="rdr">読み込み済みのSqlDataReader</param>
        /// <returns>クラスのインスタンス</returns>
        private SystemNameInfo BuildInfoFromSqlDataReader(SqlDataReader rdr)
        {
            SystemNameInfo rt_info = null;

            rt_info = new SystemNameInfo
            {
                SystemNameKbn = SQLServerUtil.dbInt(rdr["SystemNameKbn"]),
                SystemNameCode = SQLServerUtil.dbInt(rdr["SystemNameCode"]),
                SystemName = rdr["SystemNameName"].ToString().Trim(),
                StringValue01 = rdr["StringValue01"].ToString().Trim(),
                IntegerValue01 = SQLServerUtil.dbInt(rdr["IntegerValue01"]),
                DecimalValue01 = SQLServerUtil.dbDecimal(rdr["DecimalValue01"]),
            };

            return rt_info;
        }

        #endregion
    }
}
