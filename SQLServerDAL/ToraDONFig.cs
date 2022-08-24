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
    /// トラDON単位テーブルのデータアクセスレイヤです。
    /// </summary>
    public class ToraDONFig
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
        /// 単位クラスのデフォルトコンストラクタです。
        /// </summary>
        public ToraDONFig()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、単位テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public ToraDONFig(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、単位情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>単位情報のリスト</returns>
        public IList<ToraDONFigInfo> GetList(FigSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        /// <summary>
        /// トラDON単位情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON単位情報のコンボボックス用リスト</returns>
        public IList<ToraDONFigInfo> GetComboList()
        {
            return GetComboListInternal(null);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<ToraDONFigInfo> GetListInternal(SqlTransaction transaction, FigSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONFigInfo> rt_list = new List<ToraDONFigInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Fig.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Fig Fig ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Fig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.FigId.HasValue)
                {
                    sb.AppendLine("AND Fig.FigId = " + para.FigId.ToString() + " ");
                }
                if (para.FigCd.HasValue)
                {
                    sb.AppendLine("AND Fig.FigCd = " + para.FigCd.ToString() + " ");
                }

                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND Fig.DisableFlag = " + NSKUtil.BoolToInt((bool)para.DisableFlag) + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Fig.FigCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONFigInfo rt_info = new ToraDONFigInfo();
                rt_info.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                rt_info.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
                rt_info.FigName = rdr["FigNm"].ToString();
                rt_info.TimeKbn = SQLServerUtil.dbInt(rdr["TimeKbn"]);
                rt_info.DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]));

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }
        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、
        /// トラDON単位情報のコンボボックス用リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>トラDON単位情報のコンボボックス用リスト</returns>
        public IList<ToraDONFigInfo> GetComboListInternal(SqlTransaction transaction)
        {
            //返却用のリスト
            List<ToraDONFigInfo> rt_list = new List<ToraDONFigInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Fig.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Fig Fig ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Fig.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("	Fig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Fig.FigCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONFigInfo rt_info = new ToraDONFigInfo
                {
                    FigId = SQLServerUtil.dbDecimal(rdr["FigId"]),
                    FigCode = SQLServerUtil.dbInt(rdr["FigCd"]),
                    FigName = rdr["FigNM"].ToString(),
                    TimeKbn = SQLServerUtil.dbInt(rdr["TimeKbn"]),
                    DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
