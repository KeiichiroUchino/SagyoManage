using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using System.Configuration;
using Jpsys.SagyoManage.SQLServerDAL;
using Jpsys.SagyoManage.ReportModel;
using System.Globalization;

namespace Jpsys.SagyoManage.ReportDAL
{
    /// <summary>
    /// 月間作業計画表情報のデータアクセスレイヤです。
    /// </summary>
    public class GekkanSagyoKeikakuhyouRpt
    {

        private Dictionary<int, string> EraTable = new Dictionary<int, string>();

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
        /// 月間作業計画表クラスのデフォルトコンストラクタです。
        /// </summary>
        public GekkanSagyoKeikakuhyouRpt()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、月間作業計画表情報の
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public GekkanSagyoKeikakuhyouRpt(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、月間作業計画表情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>月間作業計画表情報のリスト</returns>
        public IList<GekkanSagyoKeikakuhyouRptInfo> GetList(GekkanSagyoKeikakuhyouRptInfoSearchParameter para = null)
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
        public IList<GekkanSagyoKeikakuhyouRptInfo> GetListInternal(SqlTransaction transaction, GekkanSagyoKeikakuhyouRptInfoSearchParameter para)
        {
            // クエリ
            String mySql = this.GetQuerySelect(para);

            this.GetEraTable();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                GekkanSagyoKeikakuhyouRptInfo rt_info = new GekkanSagyoKeikakuhyouRptInfo
                {
                    SagyoYM = this.ConvFormat(SQLHelper.dbDate(rdr["SagyoStartDateTime"]), false),
                    SagyoYMD = this.ConvFormat(SQLHelper.dbDate(rdr["SagyoStartDateTime"]), true),
                    YoteiJinin = "",
                    SagyoCode = "",
                    KeiyakuCode = rdr["KeiyakuCode"].ToString(),
                    SagyoBashoName = rdr["SagyoBashoName"].ToString(),
                    SagyoName = "",
                    YoteiKaishiDate = SQLHelper.dbDate(rdr["SagyoStartDateTime"]).ToString("yyyy/MM/dd HH:mm:ss"),
                    YoteiShuryoDate = SQLHelper.dbDate(rdr["SagyoEndDateTime"]).ToString("yyyy/MM/dd HH:mm:ss"),
                    CarCode = 0,
                    SekininshaName = rdr["StaffName"].ToString(),
                    StaffName = rdr["StaffNameLinkResult"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// 月間作業計画表情報を取得するSQLを作成します。
        /// </summary>
        /// <param name="info">月間作業計画表情報</param>
        /// <returns></returns>
        private string GetQuerySelect(GekkanSagyoKeikakuhyouRptInfoSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   SagyoAnken.* ");
            sb.AppendLine("   , Keiyaku.KeiyakuId AS K_KeiyakuId ");
            sb.AppendLine("   , Keiyaku.KeiyakuCode ");
            sb.AppendLine("   , Keiyaku.KeiyakuName ");
            sb.AppendLine("   , SagyoBasho.SagyoBashoCode ");
            sb.AppendLine("   , Tokuisaki.TokuisakiCode ");
            sb.AppendLine("   , SagyoBasho.SagyoBashoName ");
            sb.AppendLine("   , Tokuisaki.TokuisakiName ");
            sb.AppendLine("   , SagyoDaiBunrui.SagyoDaiBunruiName ");
            sb.AppendLine("   , SagyoChuBunrui.SagyoChuBunruiName ");
            sb.AppendLine("   , SagyoShoBunrui.SagyoShoBunruiName ");
            sb.AppendLine("   , Staff.StaffCode ");
            sb.AppendLine("   , Staff.StaffName  ");
            sb.AppendLine("   , TRIM(',' FROM SW1.StaffNameLinkResult) AS StaffNameLinkResult ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   SagyoAnken  ");
            sb.AppendLine("   LEFT JOIN Keiyaku  ");
            sb.AppendLine("     ON SagyoAnken.KeiyakuId = Keiyaku.KeiyakuId  ");
            sb.AppendLine("     AND Keiyaku.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoBasho  ");
            sb.AppendLine("     ON Keiyaku.SagyoBashoId = SagyoBasho.SagyoBashoId  ");
            sb.AppendLine("     AND SagyoBasho.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN Tokuisaki  ");
            sb.AppendLine("     ON SagyoBasho.TokuisakiId = Tokuisaki.TokuisakiId  ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN Staff  ");
            sb.AppendLine("     ON SagyoAnken.SekininshaId = Staff.StaffId  ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN (  ");
            sb.AppendLine("     select ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("       , (  ");
            sb.AppendLine("         SELECT ");
            sb.AppendLine("           T2.StaffName + ','  -- 文字列を連結する ");
            sb.AppendLine("         FROM ");
            sb.AppendLine("           (  ");
            sb.AppendLine("             SELECT ");
            sb.AppendLine("               SagyoinWariateId ");
            sb.AppendLine("               , SagyoAnkenId ");
            sb.AppendLine("               , StaffName  ");
            sb.AppendLine("             FROM ");
            sb.AppendLine("               SagyoinWariate  ");
            sb.AppendLine("               LEFT JOIN Staff  ");
            sb.AppendLine("                 ON SagyoinWariate.StaffId = Staff.StaffId  ");
            sb.AppendLine("                 AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" 			WHERE ");
            sb.AppendLine(" 			  SagyoinWariate.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("           ) T2  ");
            sb.AppendLine("         WHERE ");
            sb.AppendLine("           T2.SagyoAnkenId = T1.SagyoAnkenId  -- 対象のセクションを選択する ");
            sb.AppendLine("         ORDER BY ");
            sb.AppendLine("           T2.SagyoinWariateId FOR XML PATH ('')  -- FOR XML PATHで副問い合わせの結果を1行1列に連結する ");
            sb.AppendLine("       ) AS StaffNameLinkResult  ");
            sb.AppendLine("     FROM ");
            sb.AppendLine("       SagyoinWariate AS T1  ");
            sb.AppendLine("     GROUP BY ");
            sb.AppendLine("       T1.SagyoAnkenId ");
            sb.AppendLine("   ) SW1  ");
            sb.AppendLine("     ON SagyoAnken.SagyoAnkenId = SW1.SagyoAnkenId  ");
            sb.AppendLine("   LEFT JOIN SagyoDaiBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoDaiBunruiId = SagyoDaiBunrui.SagyoDaiBunruiId  ");
            sb.AppendLine("     AND SagyoDaiBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoChuBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoChuBunruiId = SagyoChuBunrui.SagyoChuBunruiId  ");
            sb.AppendLine("     AND SagyoChuBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT JOIN SagyoShoBunrui  ");
            sb.AppendLine("     ON Keiyaku.SagyoShoBunruiId = SagyoShoBunrui.SagyoShoBunruiId  ");
            sb.AppendLine("     AND SagyoShoBunrui.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   SagyoAnken.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                sb.AppendLine("       AND SagyoAnken.SagyoStartDateTime >= " + "'"
                    + para.SagyoYM.ToString("yyyy/MM/01 00:00:00", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");

                int days = DateTime.DaysInMonth(para.SagyoYM.Year, para.SagyoYM.Month); // その月の日数
                // DateTime構造体の場合
                var lastDayOfMonth1 = new DateTime(para.SagyoYM.Year, para.SagyoYM.Month, days);

                sb.AppendLine("       AND SagyoAnken.SagyoEndDateTime <= " + "'"
                    + lastDayOfMonth1.ToString("yyyy/MM/dd 23:59:59", System.Globalization.CultureInfo.InvariantCulture) + "'" + " ");
            }

            sb.AppendLine(" ORDER BY ");
            sb.AppendLine(" SagyoAnken.SagyoStartDateTime DESC ");

            return sb.ToString();
        }

        /// <summary>
        /// 月間作業計画表情報を取得するSQLを作成します。
        /// </summary>
        /// <param name="para">日付</param>
        /// <returns></returns>
        private string ConvFormat(DateTime para, bool deyFlg)
        {
            string result = string.Empty;

            if (deyFlg)
            {
                //和暦の使用準備
                JapaneseCalendar jc = new JapaneseCalendar();
                int era = jc.GetEra(para);

                result = this.EraTable[era] + " " + para.ToString("yy/MM/dd (ddd)");
            }
            else 
            {
                //和暦の使用準備
                JapaneseCalendar jc = new JapaneseCalendar();
                int era = jc.GetEra(para);

                result = this.EraTable[era] + " " + para.ToString("yy/MM");

            }
            return result;
        }

        /// <summary>
        /// 年号テーブルを作成します。
        /// </summary>
        /// <returns></returns>
        private void GetEraTable()
        {

            //DateTime日付を生成
            //和暦の使用準備
            JapaneseCalendar jc = new JapaneseCalendar();
            CultureInfo ci = new CultureInfo("Ja-JP", true);
            ci.DateTimeFormat.Calendar = jc;

            this.EraTable = new Dictionary<int, string>();
            for (char e = 'A'; e <= 'Z'; e++)
            {
                int eraIndex = ci.DateTimeFormat.GetEra(e.ToString());
                if (eraIndex > 0)
                    this.EraTable.Add(eraIndex, e.ToString());
            }

        }

        #endregion
    }
}
