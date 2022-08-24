using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 休刊カレンダ明細テーブルのデータアクセスレイヤです。
    /// </summary>
    public class KyujitsuCalendarMeisai
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
        /// 休刊カレンダ明細クラスのデフォルトコンストラクタです。
        /// </summary>
        public KyujitsuCalendarMeisai()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、休刊カレンダ明細テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public KyujitsuCalendarMeisai(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、休刊カレンダ明細情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>休刊カレンダ明細情報のリスト</returns>
        public IList<KyujitsuCalendarMeisaiInfo> GetList(KyujitsuCalendarMeisaiSearchParameter para = null)
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
        public IList<KyujitsuCalendarMeisaiInfo> GetListInternal(SqlTransaction transaction, KyujitsuCalendarMeisaiSearchParameter para)
        {
            //返却用のリスト
            List<KyujitsuCalendarMeisaiInfo> rt_list = new List<KyujitsuCalendarMeisaiInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("  KyujitsuCalendarMeisai.* ");
            sb.AppendLine(" ,KyujitsuKbn.DecimalValue01 KyujitsuNissu ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	KyujitsuCalendarMeisai ");
            sb.AppendLine("INNER JOIN ");
            sb.AppendLine("	KyujitsuCalendar ");
            sb.AppendLine("ON KyujitsuCalendar.KyujitsuCalendarId = KyujitsuCalendarMeisai.KyujitsuCalendarId ");
            sb.AppendLine("AND KyujitsuCalendar.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	SystemName KyujitsuKbn --休日区分 ");
            sb.AppendLine(" ON KyujitsuKbn.SystemNameKbn = " + ((int)DefaultProperty.SystemNameKbn.KyujitsuKbn).ToString() + " ");
            sb.AppendLine(" AND KyujitsuKbn.SystemNameCode = KyujitsuCalendarMeisai.KyujitsuKbn ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1 ");

            if (para != null)
            {
                if (para.KyujitsuCalendarId.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.KyujitsuCalendarId = " + para.KyujitsuCalendarId.ToString() + " ");
                    sb.AppendLine("AND KyujitsuCalendarMeisai.KyujitsuCalendarId = " + para.KyujitsuCalendarId.ToString() + " ");
                }
                if (para.Nendo.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.Nendo = " + para.Nendo.ToString() + " ");
                }
                if (para.ExcludeToraDONStaffId.HasValue)
                {
                    sb.AppendLine("AND KyujitsuCalendar.ToraDONStaffId <> " + para.ExcludeToraDONStaffId.ToString() + " ");
                }
            }

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                KyujitsuCalendarMeisaiInfo rt_info = new KyujitsuCalendarMeisaiInfo
                {
                    KyujitsuCalendarId = SQLServerUtil.dbDecimal(rdr["KyujitsuCalendarId"]),
                    HizukeYMD = SQLHelper.dbDate(rdr["HizukeYMD"]),
                    KyujitsuKbn = SQLServerUtil.dbInt(rdr["KyujitsuKbn"]),
                    Memo = rdr["Memo"].ToString(),

                    KyujitsuNissu = SQLServerUtil.dbDecimal(rdr["KyujitsuNissu"]),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        #endregion
    }
}
