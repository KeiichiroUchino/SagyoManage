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
    /// 所在地（LYNA）テーブルのデータアクセスレイヤです。
    /// </summary>
    public class PointForLyna
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
        /// 所在地（LYNA）クラスのデフォルトコンストラクタです。
        /// </summary>
        public PointForLyna()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public PointForLyna(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定で所在地（LYNA）情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PointForLynaInfo GetInfo(string id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new PointForLynaSearchParameter()
            {
                PointId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、所在地（LYNA）情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地情報のリスト</returns>
        public IList<PointForLynaInfo> GetList(PointForLynaSearchParameter para = null)
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
        public IList<PointForLynaInfo> GetListInternal(SqlTransaction transaction, PointForLynaSearchParameter para)
        {
            //返却用のリスト
            List<PointForLynaInfo> rt_list = new List<PointForLynaInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	所在地.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	所在地 ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1");

            if (para != null)
            {
                if (string.IsNullOrWhiteSpace(para.PointId))
                {
                    sb.AppendLine(@"AND 所在地.コード = """ + AccessHelper.GetSanitaizingSqlString(para.PointId.Trim()) + @""" ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" 所在地.コード ");

            String sql = sb.ToString();

            return AccessHelper.SimpleRead(sql, rdr =>
            {
                PointForLynaInfo info = new PointForLynaInfo();
                info.PointId = rdr["コード"].ToString().Trim();
                info.PointCode = rdr["発着地コード"].ToString().Trim();
                info.PointName = rdr["名称"].ToString().Trim();
                info.PointKbn = SQLServerUtil.dbInt(rdr["種別"]);
                info.Address = rdr["住所"].ToString().Trim();
                info.Tel = rdr["電話番号"].ToString().Trim();
                info.Keido = rdr["経度"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["経度"].ToString());
                info.Ido = rdr["緯度"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["緯度"].ToString());
                info.TsuikaNisabakiHMS = rdr["追加荷捌き時間"].ToString()==string.Empty?0:Convert.ToDecimal(rdr["追加荷捌き時間"].ToString());
                info.IchiSedo = rdr["位置精度"].ToString() == string.Empty ? 0 : Convert.ToInt32(rdr["位置精度"].ToString()); ;
                info.KaishiHMS = rdr["開始時刻"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["開始時刻"].ToString());
                info.ShuryoHMS = rdr["終了時刻"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["終了時刻"].ToString());
                info.Group01 = rdr["グループ1"].ToString().Trim();
                info.Group02 = rdr["グループ2"].ToString().Trim();
                info.Group03 = rdr["グループ3"].ToString().Trim();
                //info.Group04 = rdr["グループ4"].ToString().Trim();
                //info.Group05 = rdr["グループ5"].ToString().Trim();
                //info.Group06 = rdr["グループ6"].ToString().Trim();
                //info.Group07 = rdr["グループ7"].ToString().Trim();
                //info.Group08 = rdr["グループ8"].ToString().Trim();
                //info.Group09 = rdr["グループ9"].ToString().Trim();
                //info.Group10 = rdr["グループ10"].ToString().Trim();
                //info.Group11 = rdr["グループ11"].ToString().Trim();
                //info.Group12 = rdr["グループ12"].ToString().Trim();
                //info.Group13 = rdr["グループ13"].ToString().Trim();
                //info.Group14 = rdr["グループ14"].ToString().Trim();
                //info.Group15 = rdr["グループ15"].ToString().Trim();
                //info.Group16 = rdr["グループ16"].ToString().Trim();
                //info.Group17 = rdr["グループ17"].ToString().Trim();
                //info.Group18 = rdr["グループ18"].ToString().Trim();
                //info.Group19 = rdr["グループ19"].ToString().Trim();
                //info.Group20 = rdr["グループ20"].ToString().Trim();
                info.ShiteiShagata01 = rdr["指定車型1"].ToString().Trim();
                info.ShiteiShagata02 = rdr["指定車型2"].ToString().Trim();
                info.ShiteiShagata03 = rdr["指定車型3"].ToString().Trim();
                //info.ShiteiShagata04 = rdr["指定車型4"].ToString().Trim();
                //info.ShiteiShagata05 = rdr["指定車型5"].ToString().Trim();
                //info.ShiteiShagata06 = rdr["指定車型6"].ToString().Trim();
                //info.ShiteiShagata07 = rdr["指定車型7"].ToString().Trim();
                //info.ShiteiShagata08 = rdr["指定車型8"].ToString().Trim();
                //info.ShiteiShagata09 = rdr["指定車型9"].ToString().Trim();
                //info.ShiteiShagata10 = rdr["指定車型10"].ToString().Trim();
                //info.ShiteiShagata11 = rdr["指定車型11"].ToString().Trim();
                //info.ShiteiShagata12 = rdr["指定車型12"].ToString().Trim();
                //info.ShiteiShagata13 = rdr["指定車型13"].ToString().Trim();
                //info.ShiteiShagata14 = rdr["指定車型14"].ToString().Trim();
                //info.ShiteiShagata15 = rdr["指定車型15"].ToString().Trim();
                //info.ShiteiShagata16 = rdr["指定車型16"].ToString().Trim();
                //info.ShiteiShagata17 = rdr["指定車型17"].ToString().Trim();
                //info.ShiteiShagata18 = rdr["指定車型18"].ToString().Trim();
                //info.ShiteiShagata19 = rdr["指定車型19"].ToString().Trim();
                //info.ShiteiShagata20 = rdr["指定車型20"].ToString().Trim();
                info.ShiteiCarCd01 = rdr["指定車両1"].ToString().Trim();
                info.ShiteiCarCd02 = rdr["指定車両2"].ToString().Trim();
                info.ShiteiCarCd03 = rdr["指定車両3"].ToString().Trim();
                //info.ShiteiCarCd04 = rdr["指定車両4"].ToString().Trim();
                //info.ShiteiCarCd05 = rdr["指定車両5"].ToString().Trim();
                //info.ShiteiCarCd06 = rdr["指定車両6"].ToString().Trim();
                //info.ShiteiCarCd07 = rdr["指定車両7"].ToString().Trim();
                //info.ShiteiCarCd08 = rdr["指定車両8"].ToString().Trim();
                //info.ShiteiCarCd09 = rdr["指定車両9"].ToString().Trim();
                //info.ShiteiCarCd10 = rdr["指定車両10"].ToString().Trim();
                //info.ShiteiCarCd11 = rdr["指定車両11"].ToString().Trim();
                //info.ShiteiCarCd12 = rdr["指定車両12"].ToString().Trim();
                //info.ShiteiCarCd13 = rdr["指定車両13"].ToString().Trim();
                //info.ShiteiCarCd14 = rdr["指定車両14"].ToString().Trim();
                //info.ShiteiCarCd15 = rdr["指定車両15"].ToString().Trim();
                //info.ShiteiCarCd16 = rdr["指定車両16"].ToString().Trim();
                //info.ShiteiCarCd17 = rdr["指定車両17"].ToString().Trim();
                //info.ShiteiCarCd18 = rdr["指定車両18"].ToString().Trim();
                //info.ShiteiCarCd19 = rdr["指定車両19"].ToString().Trim();
                //info.ShiteiCarCd20 = rdr["指定車両20"].ToString().Trim();

                return info;
            });
        }

        #endregion
    }
}
