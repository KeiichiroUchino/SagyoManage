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

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 郵便番号辞書クラスのビジネスロジックです。
    /// </summary>
    public class PostalZip
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
        /// テーブル名
        /// </summary>
        private string _tableName = "郵便番号";


        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public PostalZip()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、オペレータマスタの
        /// ビジネスロジックレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public PostalZip(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、情報を取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>相場エリア情報</returns>
        public PostalZipInfo GetInfo(PostalZipSearchParameter para)
        {
            if (para == null || para.ZipCode == null)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("{0} １件取得に必要な条件が設定されていません。"
                                , this._tableName),
                    MessageBoxIcon.Warning);
            }

            //zipCodeを数値に変換するだめなら、例外を投げる
            if (!NSKUtil.IsNumeric(para.ZipCode))
            {
                throw new ApplicationException("郵便番号に数値以外の文字列が指定されています。");
            }


            PostalZipInfo info =
                this.GetListInternal(null, para).FirstOrDefault();

            if (info == null)
            {
                //リトライ可能な例外
                throw new
                    Model.DALExceptions.CanRetryException(
                    string.Format("該当する{0}がありません。"
                                , this._tableName),
                    MessageBoxIcon.Warning);
            }

            return info;
        }

        /// <summary>
        /// 検索条件情報を指定して、情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>相場エリア情報のリスト</returns>
        public IList<PostalZipInfo> GetInfoList(PostalZipSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }


        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private IList<PostalZipInfo> GetListInternal(
            SqlTransaction transaction, PostalZipSearchParameter para)
        {
            //返却用のリスト
            List<PostalZipInfo> rt_list = new List<PostalZipInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" PostalZipData.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" PostalZipData ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" 1= 1 ");

            if (para != null)
            {
                if (para.ZipCode != null)
                {
                    sb.AppendLine(" AND PostalZipData.ZipTextCode LIKE N'" + para.ZipCode + "%' ");
                }

                if (para.Adress != null)
                {
                    sb.AppendLine("	AND PostalZipData.PrefName + PostalZipData.CityName + PostalZipData.TownName LIKE N'%" + para.Adress + "%' ");
                }

            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" PostalZipData.ZipId ");
            string mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr => this.BuildInfoFromSqlDataReader(rdr), transaction);
        }

        /// <summary>
        /// SqlDataReaderを指定して、
        /// クラスのインスタンスを返却します。
        /// </summary>
        /// <param name="rdr">読み込み済みのSqlDataReader</param>
        /// <returns>クラスのインスタンス</returns>
        private PostalZipInfo BuildInfoFromSqlDataReader(SqlDataReader rdr)
        {
            PostalZipInfo rt_info = new PostalZipInfo()
            {
                ZipCode = rdr["ZipTextCode"].ToString().Trim(),
                PrefName = rdr["PrefName"].ToString().Trim(),
                CityName = rdr["CityName"].ToString().Trim(),
                TownName = rdr["TownName"].ToString().Trim()
            };

            return rt_info;
        }

        #endregion
    }
}
