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
    /// トラDON備考テーブルのデータアクセスレイヤです。
    /// </summary>
    public class Memo
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
        /// 備考クラスのデフォルトコンストラクタです。
        /// </summary>
        public Memo()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、備考テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public Memo(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、備考情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <returns>備考情報のリスト</returns>
        public IList<ToraDONMemoInfo> GetList(MemoSearchParameter para = null, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, para);
        }

        ///// <summary>
        ///// トラDON備考情報のコンボボックス用リストを取得します。
        ///// </summary>
        ///// <param name="para">検索条件情報</param>
        ///// <returns>トラDON備考情報のコンボボックス用リスト</returns>
        //public IList<ToraDONMemoInfo> GetComboList()
        //{
        //    return GetComboListInternal(null);
        //}

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<ToraDONMemoInfo> GetListInternal(SqlTransaction transaction, MemoSearchParameter para)
        {
            //返却用のリスト
            List<ToraDONMemoInfo> rt_list = new List<ToraDONMemoInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	Memo.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Memo Memo ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("	Memo.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.ToraDONMemoId.HasValue)
                {
                    sb.AppendLine("AND Memo.MemoId = " + para.ToraDONMemoId.ToString() + " ");
                }
                if (para.ToraDONMemoCd.HasValue)
                {
                    sb.AppendLine("AND Memo.MemoCd = " + para.ToraDONMemoCd.ToString() + " ");
                }

                if (para.DisableFlag.HasValue)
                {
                    sb.AppendLine("AND Memo.DisableFlag = " + NSKUtil.BoolToInt((bool)para.DisableFlag) + " ");
                }
            }
            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" Memo.MemoCd ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                ToraDONMemoInfo rt_info = new ToraDONMemoInfo();
                rt_info.ToraDONMemoId = SQLServerUtil.dbDecimal(rdr["MemoId"]);
                rt_info.ToraDONMemoCode = SQLServerUtil.dbInt(rdr["MemoCd"]);
                rt_info.ToraDONMemoName = rdr["MemoNm"].ToString();
                rt_info.DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]));

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }
        #endregion

        #region プライベートメソッド

        ///// <summary>
        ///// SqlTransaction情報、検索条件情報を指定して、
        ///// トラDON備考情報のコンボボックス用リストを取得します。
        ///// </summary>
        ///// <param name="transaction">SqlTransaction情報</param>
        ///// <param name="para">検索条件情報</param>
        ///// <returns>トラDON備考情報のコンボボックス用リスト</returns>
        //public IList<ToraDONMemoInfo> GetComboListInternal(SqlTransaction transaction)
        //{
        //    //返却用のリスト
        //    List<ToraDONMemoInfo> rt_list = new List<ToraDONMemoInfo>();

        //    //SQL文を作成
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine("SELECT ");
        //    sb.AppendLine("	Memo.* ");
        //    sb.AppendLine("FROM ");
        //    sb.AppendLine(" TORADON_Memo Memo ");
        //    sb.AppendLine("WHERE ");
        //    sb.AppendLine("	Memo.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
        //    sb.AppendLine("AND ");
        //    sb.AppendLine("	Memo.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
        //    sb.AppendLine("ORDER BY ");
        //    sb.AppendLine(" Memo.MemoCd ");

        //    String mySql = sb.ToString();

        //    return SQLHelper.SimpleRead(mySql, rdr =>
        //    {
        //        //返却用の値
        //        ToraDONMemoInfo rt_info = new ToraDONMemoInfo
        //        {
        //            ToraDONMemoId = SQLServerUtil.dbDecimal(rdr["MemoId"]),
        //            ToraDONMemoCode = SQLServerUtil.dbInt(rdr["MemoCd"]),
        //            ToraDONMemoName = rdr["MemoNM"].ToString(),
        //            DisableFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["DisableFlag"]))
        //        };

        //        //返却用の値を返します
        //        return rt_info;
        //    }, transaction);
        //}

        #endregion
    }
}
