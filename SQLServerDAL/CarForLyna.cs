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
using System.Data.OleDb;
using System.Data;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// LYNADBの車両テーブルのデータアクセスレイヤです。
    /// </summary>
    public class CarForLyna
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
        /// LYNADBの車両クラスのデフォルトコンストラクタです。
        /// </summary>
        public CarForLyna()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、発着地テーブルの
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public CarForLyna(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// Id指定でLYNADBの車両情報を取得します。
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CarForLynaInfo GetInfo(string id, SqlTransaction transaction = null)
        {
            return GetListInternal(transaction, new CarForLynaSearchParameter()
            {
                CarId = id,
            }).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、LYNADBの車両情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>発着地情報のリスト</returns>
        public IList<CarForLynaInfo> GetList(CarForLynaSearchParameter para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 登録情報を指定して、LYNADBの車両情報を登録・更新・削除します。
        /// </summary>
        /// <param name="saveInfo">登録情報</param>
        public void Save(CarForLynaSaveInfo saveInfo)
        {
            //コネクション取得
            using (OleDbConnection con =
                       AccessHelper.GetOleDbConn())
            {
                OleDbCommand command = new OleDbCommand();
                OleDbTransaction transaction = null;

                //DBコネクション接続
                command.Connection = con;

                try
                {
                    //オープン
                    con.Open();

                    //トランザクション開始
                    transaction = con.BeginTransaction(IsolationLevel.ReadCommitted);

                    //接続オブジェクト生成
                    command.Connection = con;
                    command.Transaction = transaction;

                    //初期化の場合
                    if (saveInfo.InitFlag)
                    {
                        //全件削除
                        command.CommandText = "DELETE * FROM 車両";
                        command.ExecuteNonQuery();
                    }

                    //登録データが存在する場合
                    if (saveInfo.AddList != null && 0 < saveInfo.AddList.Count)
                    {
                        //登録
                        foreach (CarForLynaInfo info in saveInfo.AddList)
                        {
                            command.CommandText = this.GetInsertSql(info);
                            command.ExecuteNonQuery();
                        }
                    }

                    //削除データが存在する場合
                    if (saveInfo.DelList != null && 0 < saveInfo.DelList.Count)
                    {
                        //削除
                        foreach (CarForLynaInfo info in saveInfo.DelList)
                        {
                            command.CommandText = "DELETE * FROM 車両 WHERE コード = " 
                                + @" """ + AccessHelper.GetSanitaizingSqlString(info.CarId.Trim()) + @""" ";
                            command.ExecuteNonQuery();
                        }
                    }

                    //更新データが存在する場合
                    if (saveInfo.UpdList != null && 0 < saveInfo.UpdList.Count)
                    {
                        //更新
                        foreach (CarForLynaInfo info in saveInfo.UpdList)
                        {
                            command.CommandText = this.GetUpdateSql(info);
                            command.ExecuteNonQuery();
                        }
                    }

                    //コミット
                    transaction.Commit();
                    Console.WriteLine("Both records are written to database.");
                }
                catch (Exception ex)
                {
                    try
                    {
                        //ロールバック
                        transaction.Rollback();

                        throw ex;
                    }
                    catch
                    {
                        //何もしない
                    }
                }
            }
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<CarForLynaInfo> GetListInternal(SqlTransaction transaction, CarForLynaSearchParameter para)
        {
            //返却用のリスト
            List<CarForLynaInfo> rt_list = new List<CarForLynaInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	車両.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	車両 ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	車型 ");
            sb.AppendLine("	ON 車型.コード = 車両.車型 ");

            sb.AppendLine("WHERE ");
            sb.AppendLine("	1 = 1");

            if (para != null)
            {
                if (string.IsNullOrWhiteSpace(para.CarId))
                {
                    sb.AppendLine(@"AND 車両.コード = """ + AccessHelper.GetSanitaizingSqlString(para.CarId.Trim()) + @""" ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine(" 車両.コード ");

            String sql = sb.ToString();

            return AccessHelper.SimpleRead(sql, rdr =>
            {
                CarForLynaInfo info = new CarForLynaInfo();
                info.CarId = rdr["コード"].ToString().Trim();
                info.CarName = rdr["名称"].ToString().Trim();
                info.LicPlateCarNo = rdr["車番"].ToString().Trim();
                info.CarKindName = rdr["車種名"].ToString().Trim();
                info.ShagataId = rdr["車型"].ToString().Trim();
                info.CarStartPointId = rdr["出発地"].ToString().Trim();
                info.CarEndPointId = rdr["終了地"].ToString().Trim();
                info.KaishiKanouHMS = rdr["開始可能時刻"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["開始可能時刻"].ToString());
                info.KaishiGenkaiHMS = rdr["開始限界時刻"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["開始限界時刻"].ToString());
                info.ShuryoGenkaiHMS = rdr["終了限界時刻"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["終了限界時刻"].ToString());
                info.MaxHMS = rdr["最大時間"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["最大時間"].ToString());
                info.MaxChokinHMS = rdr["最大超勤時間"].ToString() == string.Empty ? 0 : Convert.ToInt32(rdr["最大超勤時間"].ToString()); ;
                info.MaxKyori = rdr["最大距離"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["最大距離"].ToString());
                info.MaxChumonSu = rdr["最大注文数"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["最大注文数"].ToString());
                info.IchikaitenAtariMaxChumonSu = rdr["1回転当最大注文数"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["1回転当最大注文数"].ToString());
                info.MaxKaitenSu = rdr["最大回転数"].ToString() == string.Empty ? 0 : Convert.ToDecimal(rdr["最大回転数"].ToString());
                info.DisableFlag = rdr["使用"].ToString().Equals("-1") ? false : true;
                info.StaffId = rdr["ドライバー"].ToString().Trim();
                info.Biko = rdr["備考"].ToString().Trim();
                info.LynaGroupId01 = rdr["グループ1"].ToString();
                info.LynaGroupId02 = rdr["グループ2"].ToString();
                info.LynaGroupId03 = rdr["グループ3"].ToString();
                info.LynaGroupId04 = rdr["グループ4"].ToString();
                info.LynaGroupId05 = rdr["グループ5"].ToString();
                //info.LynaGroupId06 = rdr["グループ6"].ToString();
                //info.LynaGroupId07 = rdr["グループ7"].ToString();
                //info.LynaGroupId08 = rdr["グループ8"].ToString();
                //info.LynaGroupId09 = rdr["グループ9"].ToString();
                //info.LynaGroupId10 = rdr["グループ10"].ToString();
                //info.LynaGroupId11 = rdr["グループ11"].ToString();
                //info.LynaGroupId12 = rdr["グループ12"].ToString();
                //info.LynaGroupId13 = rdr["グループ13"].ToString();
                //info.LynaGroupId14 = rdr["グループ14"].ToString();
                //info.LynaGroupId15 = rdr["グループ15"].ToString();
                //info.LynaGroupId16 = rdr["グループ16"].ToString();
                //info.LynaGroupId17 = rdr["グループ17"].ToString();
                //info.LynaGroupId18 = rdr["グループ18"].ToString();
                //info.LynaGroupId19 = rdr["グループ19"].ToString();
                //info.LynaGroupId20 = rdr["グループ20"].ToString();
                info.ShagataName = rdr["車種名"].ToString().Trim();

                return info;
            });
        }

        /// <summary>
        /// LYNADBの車両情報をもとに登録SQLを取得します。
        /// </summary>
        /// <param name="info">LYNADBの車両情報</param>
        /// <returns>登録SQL</returns>
        public string GetInsertSql(CarForLynaInfo info)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("INSERT INTO ");
            sb.AppendLine(" 車両 ");
            sb.AppendLine(" ( ");
            sb.AppendLine("	 コード ");
            sb.AppendLine("	,名称 ");
            sb.AppendLine("	,車番 ");
            sb.AppendLine("	,車種名 ");
            sb.AppendLine("	,車型 ");
            sb.AppendLine("	,出発地 ");
            sb.AppendLine("	,終了地 ");
            sb.AppendLine("	,開始可能時刻 ");
            sb.AppendLine("	,開始限界時刻 ");
            sb.AppendLine("	,終了限界時刻 ");
            sb.AppendLine("	,最大時間 ");
            sb.AppendLine("	,最大超勤時間 ");
            sb.AppendLine("	,最大距離 ");
            sb.AppendLine("	,最大注文数 ");
            sb.AppendLine("	,1回転当最大注文数 ");
            sb.AppendLine("	,最大回転数 ");
            sb.AppendLine("	,使用 ");
            sb.AppendLine("	,ドライバー ");
            sb.AppendLine("	,備考 ");
            sb.AppendLine("	,グループ1 ");
            sb.AppendLine("	,グループ2 ");
            sb.AppendLine("	,グループ3 ");
            sb.AppendLine("	,グループ4 ");
            sb.AppendLine("	,グループ5 ");

            sb.AppendLine(" ) ");
            sb.AppendLine("VALUES ");
            sb.AppendLine("( ");
            sb.AppendLine(@" """ + AccessHelper.GetSanitaizingSqlString(info.CarId.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.CarName.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LicPlateCarNo.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.CarKindName.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.ShagataId.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.CarStartPointId.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.CarEndPointId.Trim()) + @""" ");
            sb.AppendLine(@"," + info.KaishiKanouHMS.ToString() + @" ");
            sb.AppendLine(@"," + info.KaishiGenkaiHMS.ToString() + @" ");
            sb.AppendLine(@"," + info.ShuryoGenkaiHMS.ToString() + @" ");
            sb.AppendLine(@"," + info.MaxHMS.ToString() + @" ");
            sb.AppendLine(@"," + info.MaxChokinHMS.ToString() + @" ");
            sb.AppendLine(@"," + info.MaxKyori.ToString() + @" ");
            sb.AppendLine(@"," + info.MaxChumonSu.ToString() + @" ");
            sb.AppendLine(@"," + info.IchikaitenAtariMaxChumonSu.ToString() + @" ");
            sb.AppendLine(@"," + info.MaxKaitenSu.ToString() + @" ");
            sb.AppendLine(@"," + (info.DisableFlag ? "0" : "-1") + @" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.StaffId.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.Biko.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId01.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId02.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId03.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId04.Trim()) + @""" ");
            sb.AppendLine(@",""" + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId05.Trim()) + @""" ");

            sb.AppendLine(") ");

            return sb.ToString();
        }

        /// <summary>
        /// LYNADBの車両情報をもとに更新SQLを取得します。
        /// </summary>
        /// <param name="info">LYNADBの車両情報</param>
        /// <returns>更新SQL</returns>
        public string GetUpdateSql(CarForLynaInfo info)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE  ");
            sb.AppendLine(" 車両 ");
            sb.AppendLine("SET ");
            sb.AppendLine(@" 名称  = """ + AccessHelper.GetSanitaizingSqlString(info.CarName.Trim()) + @""" ");
            sb.AppendLine(@",車番 = """ + AccessHelper.GetSanitaizingSqlString(info.LicPlateCarNo.Trim()) + @""" ");
            sb.AppendLine(@",車種名  = """ + AccessHelper.GetSanitaizingSqlString(info.CarKindName.Trim()) + @""" ");
            sb.AppendLine(@",車型  = """ + AccessHelper.GetSanitaizingSqlString(info.ShagataId.Trim()) + @""" ");
            sb.AppendLine(@",使用  = " + (info.DisableFlag ? "-1" : "0") + @" ");
            sb.AppendLine(@",ドライバー  = """ + AccessHelper.GetSanitaizingSqlString(info.ShagataId.Trim()) + @""" ");
            sb.AppendLine(@",グループ1  = """ + AccessHelper.GetSanitaizingSqlString(info.LynaGroupId01.Trim()) + @""" ");

            sb.AppendLine("WHERE ");
            sb.AppendLine(@"CarId = """ + AccessHelper.GetSanitaizingSqlString(info.CarId.Trim()) + @""" ");

            return sb.ToString();
        }

        #endregion
    }
}
