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
using Jpsys.HaishaManageV10.BizProperty;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 過去受注履歴のデータアクセスレイヤです。
    /// </summary>
    public class PastPrice
    {
        /// <summary>
        /// PastPriceクラスのデフォルトコンストラクタです。
        /// </summary>
        public PastPrice()
        {
        }

        #region パブリックメソッド

        /// <summary>
        /// 条件指定で過去受注履歴検索情報を取得します。
        /// </summary>
        /// <param name="para"></param>
        /// <returns></returns>
        public PastPriceDefaultSearchParametersInfo GetSearchInfo(PastPriceDefaultParameters para, SqlTransaction transaction = null)
        {
            return GetSearchListInternal(transaction, para).FirstOrDefault();
        }

        /// <summary>
        /// 検索条件情報を指定して、過去受注履歴情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>過去受注履歴情報のリスト</returns>
        public IList<PastPriceInfo> GetList(PastPriceSearchParametersInfo para = null)
        {
            return GetListInternal(null, para);
        }

        /// <summary>
        /// 検索条件情報を指定して、トラDonの過去受注履歴情報のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>過去受注履歴情報のリスト</returns>
        public IList<PastPriceInfo> GetListToraDon(PastPriceSearchParametersInfo para = null)
        {
            return GetListInternalToraDon(null, para);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<PastPriceInfo> GetListInternal(SqlTransaction transaction, PastPriceSearchParametersInfo para)
        {
            //返却用のリスト
            List<PastPriceInfo> rt_list = new List<PastPriceInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT TOP 2000 ");
            sb.AppendLine("   Juchu.JuchuId ");
            sb.AppendLine("   , Hanro.HanroCode ");
            sb.AppendLine("   , Hanro.HanroName ");
            sb.AppendLine("   , Hanro.DisableFlag AS HanroDisableFlag ");
            sb.AppendLine("   , Juchu.TokuisakiId ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiCd ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiNM ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiSNM ");
            sb.AppendLine("   , TORADON_Tokuisaki.DisableFlag AS TokuisakiDisableFlag ");
            sb.AppendLine("   , Juchu.CarKindId ");
            sb.AppendLine("   , TORADON_CarKind.CarKindCd ");
            sb.AppendLine("   , TORADON_CarKind.CarKindNM ");
            sb.AppendLine("   , TORADON_CarKind.CarKindSNM ");
            sb.AppendLine("   , TORADON_CarKind.DisableFlag AS CarKindDisableFlag ");
            sb.AppendLine("   , Juchu.StartPointId ");
            sb.AppendLine("   , StartPoint.PointCd AS StartPointCd ");
            sb.AppendLine("   , StartPoint.PointNM AS StartPointNM ");
            sb.AppendLine("   , StartPoint.DisableFlag AS StartPointDisableFlag ");
            sb.AppendLine("   , Juchu.EndPointId ");
            sb.AppendLine("   , EndPoint.PointCd AS EndPointCd ");
            sb.AppendLine("   , EndPoint.PointNM AS EndPointNM ");
            sb.AppendLine("   , EndPoint.DisableFlag AS EndPointDisableFlag ");
            sb.AppendLine("   , Juchu.ItemId ");
            sb.AppendLine("   , TORADON_Item.ItemCd ");
            sb.AppendLine("   , TORADON_Item.ItemNM ");
            sb.AppendLine("   , TORADON_Item.DisableFlag AS ItemDisableFlag ");
            sb.AppendLine("   , Juchu.FigId ");
            sb.AppendLine("   , TORADON_Fig.FigCd ");
            sb.AppendLine("   , TORADON_Fig.FigNM ");
            sb.AppendLine("   , TORADON_Fig.DisableFlag AS FigDisableFlag ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiId ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiCd ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiNM ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiSNM ");
            sb.AppendLine("   , TORADON_Torihikisaki.DisableFlag  AS TorihikiDisableFlag ");
            sb.AppendLine("   , Juchu.AtPrice ");
            sb.AppendLine("   , Juchu.Number ");
            sb.AppendLine("   , Juchu.FigId ");
            sb.AppendLine("   , Juchu.PriceInPrice ");
            sb.AppendLine("   , Juchu.TollFeeInPrice ");
            sb.AppendLine("   , Juchu.FutaigyomuryoInPrice ");
            sb.AppendLine("   , Juchu.PriceInCharterPrice ");
            sb.AppendLine("   , Juchu.TollFeeInCharterPrice  ");
            sb.AppendLine("   , Juchu.TaskStartDateTime  ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   Juchu  ");
            sb.AppendLine("   LEFT OUTER JOIN Hanro  ");
            sb.AppendLine("     ON Juchu.CarId = Hanro.ToraDONCarKindId  ");
            sb.AppendLine("     AND Juchu.CarKindId = Hanro.ToraDONCarKindId  ");
            sb.AppendLine("     AND Juchu.TokuisakiId = Hanro.ToraDONTokuisakiId  ");
            sb.AppendLine("     AND Juchu.StartPointId = Hanro.ToraDONHatchiId  ");
            sb.AppendLine("     AND Juchu.EndPointId = Hanro.ToraDONChakuchiId  ");
            sb.AppendLine("     AND Juchu.ItemId = Hanro.ToraDONItemId  ");
            sb.AppendLine("     AND Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Tokuisaki  ");
            sb.AppendLine("     ON Juchu.TokuisakiId = TORADON_Tokuisaki.TokuisakiId  ");
            sb.AppendLine("     AND TORADON_Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_CarKind  ");
            sb.AppendLine("     ON Juchu.CarKindId = TORADON_CarKind.CarKindId  ");
            sb.AppendLine("     AND TORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Point AS StartPoint  ");
            sb.AppendLine("     ON Juchu.StartPointId = StartPoint.PointId  ");
            sb.AppendLine("     AND StartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Point AS EndPoint  ");
            sb.AppendLine("     ON Juchu.EndPointId = EndPoint.PointId  ");
            sb.AppendLine("     AND EndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Item  ");
            sb.AppendLine("     ON Juchu.ItemId = TORADON_Item.ItemId  ");
            sb.AppendLine("     AND TORADON_Item.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Fig  ");
            sb.AppendLine("     ON Juchu.FigId = TORADON_Fig.FigId  ");
            sb.AppendLine("     AND TORADON_Fig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Torihikisaki  ");
            sb.AppendLine("     ON Juchu.CarOfChartererId = TORADON_Torihikisaki.TorihikiId  ");
            sb.AppendLine("     AND TORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                if (para.HanroId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Hanro.HanroId = " + para.HanroId.ToString() + " ");
                }
                if (para.TokuisakiId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (para.CarKindId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.CarKindId = " + para.CarKindId.ToString() + " ");
                }
                if (para.StartPointId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.StartPointId = " + para.StartPointId.ToString() + " ");
                }
                if (para.EndPointId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.EndPointId = " + para.EndPointId.ToString() + " ");
                }
                if (para.ItemId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.ItemId = " + para.ItemId.ToString() + " ");
                }
                if (para.FigId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.FigId = " + para.FigId.ToString() + " ");
                }
                if (para.TorihikiId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND Juchu.CarOfChartererId = " + para.TorihikiId.ToString() + " ");
                }
                if (para.TaishoYMDFrom.CompareTo(DateTime.MinValue) != 0)
                {
                    sb.AppendLine("AND Juchu.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " ");
                }
                if (para.TaishoYMDTo.CompareTo(DateTime.MinValue) != 0)
                {
                    sb.AppendLine("AND Juchu.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("  Hanro.HanroCode  ");
            sb.AppendLine("  ,TORADON_Tokuisaki.TokuisakiCd  ");
            sb.AppendLine("  ,StartPoint.PointCd  ");
            sb.AppendLine("  ,EndPoint.PointCd  ");
            sb.AppendLine("  ,TORADON_Item.ItemCd  ");
            sb.AppendLine("  ,Juchu.TaskStartDateTime DESC ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PastPriceInfo rt_info = new PastPriceInfo();

                rt_info.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                rt_info.HanroCode = SQLServerUtil.dbInt(rdr["HanroCode"]);
                rt_info.HanroName = rdr["HanroName"].ToString();
                rt_info.HanroDisableFlag = SQLHelper.dbBit(rdr["HanroDisableFlag"]);
                rt_info.TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]);
                rt_info.TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.TokuisakiName = rdr["TokuisakiNM"].ToString();
                rt_info.TokuisakiShortName = rdr["TokuisakiSNM"].ToString();
                rt_info.TokuisakiDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["TokuisakiDisableFlag"]));
                rt_info.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                rt_info.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                rt_info.CarKindName = rdr["CarKindNM"].ToString();
                rt_info.CarKindShortName = rdr["CarKindSNM"].ToString();
                rt_info.CarKindDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["CarKindDisableFlag"]));
                rt_info.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
                rt_info.StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                rt_info.StartPointName = rdr["StartPointNM"].ToString();
                rt_info.StartPointDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["StartPointDisableFlag"]));
                rt_info.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
                rt_info.EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                rt_info.EndPointName = rdr["EndPointNM"].ToString();
                rt_info.EndPointDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["EndPointDisableFlag"]));
                rt_info.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
                rt_info.ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]);
                rt_info.ItemName = rdr["ItemNM"].ToString();
                rt_info.ItemDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["ItemDisableFlag"]));
                rt_info.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                rt_info.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
                rt_info.FigName = rdr["FigNM"].ToString();
                rt_info.FigDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["FigDisableFlag"]));
                rt_info.TorihikiId = SQLServerUtil.dbDecimal(rdr["TorihikiId"]);
                rt_info.TorihikiCode = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                rt_info.TorihikiName = rdr["TorihikiNM"].ToString();
                rt_info.TorihikiShortName = rdr["TorihikiSNM"].ToString();
                rt_info.TorihikiDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["TorihikiDisableFlag"]));
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                rt_info.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);

                // 非表示フラグのいづれかがtrueの場合は、非表示
                if (rt_info.HanroDisableFlag
                 || rt_info.TokuisakiDisableFlag
                 || rt_info.CarKindDisableFlag
                 || rt_info.StartPointDisableFlag
                 || rt_info.EndPointDisableFlag
                 || rt_info.ItemDisableFlag
                 || rt_info.FigDisableFlag
                 || rt_info.TorihikiDisableFlag)
                {
                    rt_info.DisableFlag = true;
                }
                else
                {
                    rt_info.DisableFlag = false;
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、トラDONの情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<PastPriceInfo> GetListInternalToraDon(SqlTransaction transaction, PastPriceSearchParametersInfo para)
        {
            //返却用のリスト
            List<PastPriceInfo> rt_list = new List<PastPriceInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT TOP 2000 ");
            sb.AppendLine("   TORADON_Sale.SaleId ");
            //sb.AppendLine("   , Hanro.HanroCode ");
            //sb.AppendLine("   , Hanro.HanroName ");
            //sb.AppendLine("   , Hanro.DisableFlag AS HanroDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.TokuisakiId ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiCd ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiNM ");
            sb.AppendLine("   , TORADON_Tokuisaki.TokuisakiSNM ");
            sb.AppendLine("   , TORADON_Tokuisaki.DisableFlag AS TokuisakiDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.CarKindId ");
            sb.AppendLine("   , TORADON_CarKind.CarKindCd ");
            sb.AppendLine("   , TORADON_CarKind.CarKindNM ");
            sb.AppendLine("   , TORADON_CarKind.CarKindSNM ");
            sb.AppendLine("   , TORADON_CarKind.DisableFlag AS CarKindDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.StartPointId ");
            sb.AppendLine("   , StartPoint.PointCd AS StartPointCd ");
            sb.AppendLine("   , StartPoint.PointNM AS StartPointNM ");
            sb.AppendLine("   , StartPoint.DisableFlag AS StartPointDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.EndPointId ");
            sb.AppendLine("   , EndPoint.PointCd AS EndPointCd ");
            sb.AppendLine("   , EndPoint.PointNM AS EndPointNM ");
            sb.AppendLine("   , EndPoint.DisableFlag AS EndPointDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.ItemId ");
            sb.AppendLine("   , TORADON_Item.ItemCd ");
            sb.AppendLine("   , TORADON_Item.ItemNM ");
            sb.AppendLine("   , TORADON_Item.DisableFlag AS ItemDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.FigId ");
            sb.AppendLine("   , TORADON_Fig.FigCd ");
            sb.AppendLine("   , TORADON_Fig.FigNM ");
            sb.AppendLine("   , TORADON_Fig.DisableFlag AS FigDisableFlag ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiId ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiCd ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiNM ");
            sb.AppendLine("   , TORADON_Torihikisaki.TorihikiSNM ");
            sb.AppendLine("   , TORADON_Torihikisaki.DisableFlag  AS TorihikiDisableFlag ");
            sb.AppendLine("   , TORADON_Sale.AtPrice ");
            sb.AppendLine("   , TORADON_Sale.Number ");
            sb.AppendLine("   , TORADON_Sale.FigId ");
            sb.AppendLine("   , TORADON_Sale.PriceInPrice ");
            sb.AppendLine("   , TORADON_Sale.TollFeeInPrice ");

            // トラDON_V50の場合
            if (para.TraDonVersionKbn == (int)DefaultProperty.TraDonVersionKbn.V50)
            {
                sb.AppendLine("   , TORADON_Sale.FutaigyomuryoInPrice ");
            }

            sb.AppendLine("   , TORADON_Sale.PriceInCharterPrice ");
            sb.AppendLine("   , TORADON_Sale.TollFeeInCharterPrice  ");
            sb.AppendLine("   , TORADON_Sale.TaskStartDate  ");
            sb.AppendLine("   , TORADON_Sale.TaskStartTime  ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   TORADON_Sale  ");
            //sb.AppendLine("   LEFT OUTER JOIN Hanro  ");
            //sb.AppendLine("     ON TORADON_Sale.CarId = Hanro.ToraDONCarKindId  ");
            //sb.AppendLine("     AND TORADON_Sale.CarKindId = Hanro.ToraDONCarKindId  ");
            //sb.AppendLine("     AND TORADON_Sale.TokuisakiId = Hanro.ToraDONTokuisakiId  ");
            //sb.AppendLine("     AND TORADON_Sale.StartPointId = Hanro.ToraDONHatchiId  ");
            //sb.AppendLine("     AND TORADON_Sale.EndPointId = Hanro.ToraDONChakuchiId  ");
            //sb.AppendLine("     AND TORADON_Sale.ItemId = Hanro.ToraDONItemId  ");
            //sb.AppendLine("     AND Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Tokuisaki  ");
            sb.AppendLine("     ON TORADON_Sale.TokuisakiId = TORADON_Tokuisaki.TokuisakiId  ");
            sb.AppendLine("     AND TORADON_Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_CarKind  ");
            sb.AppendLine("     ON TORADON_Sale.CarKindId = TORADON_CarKind.CarKindId  ");
            sb.AppendLine("     AND TORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Point AS StartPoint  ");
            sb.AppendLine("     ON TORADON_Sale.StartPointId = StartPoint.PointId  ");
            sb.AppendLine("     AND StartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Point AS EndPoint  ");
            sb.AppendLine("     ON TORADON_Sale.EndPointId = EndPoint.PointId  ");
            sb.AppendLine("     AND EndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Item  ");
            sb.AppendLine("     ON TORADON_Sale.ItemId = TORADON_Item.ItemId  ");
            sb.AppendLine("     AND TORADON_Item.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Fig  ");
            sb.AppendLine("     ON TORADON_Sale.FigId = TORADON_Fig.FigId  ");
            sb.AppendLine("     AND TORADON_Fig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("   LEFT OUTER JOIN TORADON_Torihikisaki  ");
            sb.AppendLine("     ON TORADON_Sale.CarOfChartererId = TORADON_Torihikisaki.TorihikiId  ");
            sb.AppendLine("     AND TORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine(" WHERE ");
            sb.AppendLine("   TORADON_Sale.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            if (para != null)
            {
                //if (para.HanroId.CompareTo(decimal.Zero) != 0)
                //{
                //    sb.AppendLine("AND Hanro.HanroId = " + para.HanroId.ToString() + " ");
                //}
                if (para.TokuisakiId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
                }
                if (para.CarKindId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.CarKindId = " + para.CarKindId.ToString() + " ");
                }
                if (para.StartPointId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.StartPointId = " + para.StartPointId.ToString() + " ");
                }
                if (para.EndPointId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.EndPointId = " + para.EndPointId.ToString() + " ");
                }
                if (para.ItemId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.ItemId = " + para.ItemId.ToString() + " ");
                }
                if (para.FigId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.FigId = " + para.FigId.ToString() + " ");
                }
                if (para.TorihikiId.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.CarOfChartererId = " + para.TorihikiId.ToString() + " ");
                }
                if (para.TaishoYMDFrom.CompareTo(DateTime.MinValue) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.TaskStartDate >= "
                        + NSKUtil.DateTimeToDecimalWithTime(para.TaishoYMDFrom).ToString() + " ");
                }
                if (para.TaishoYMDTo.CompareTo(DateTime.MinValue) != 0)
                {
                    sb.AppendLine("AND TORADON_Sale.TaskStartDate <= "
                        + NSKUtil.DateTimeToDecimalWithTime(para.TaishoYMDTo).ToString() + " ");
                }
            }

            sb.AppendLine("ORDER BY ");
            sb.AppendLine("  TORADON_Tokuisaki.TokuisakiCd  ");
            sb.AppendLine("  ,StartPoint.PointCd  ");
            sb.AppendLine("  ,EndPoint.PointCd  ");
            sb.AppendLine("  ,TORADON_Item.ItemCd  ");
            sb.AppendLine("  ,TORADON_Sale.TaskStartDate DESC ");
            sb.AppendLine("  ,TORADON_Sale.TaskStartTime DESC ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PastPriceInfo rt_info = new PastPriceInfo();

                //rt_info.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                rt_info.HanroCode = SQLServerUtil.dbInt(null);
                rt_info.HanroName = string.Empty;
                rt_info.HanroDisableFlag = SQLHelper.dbBit(null);
                rt_info.TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]);
                rt_info.TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.TokuisakiName = rdr["TokuisakiNM"].ToString();
                rt_info.TokuisakiShortName = rdr["TokuisakiSNM"].ToString();
                rt_info.TokuisakiDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["TokuisakiDisableFlag"]));
                rt_info.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                rt_info.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                rt_info.CarKindName = rdr["CarKindNM"].ToString();
                rt_info.CarKindShortName = rdr["CarKindSNM"].ToString();
                rt_info.CarKindDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["CarKindDisableFlag"]));
                rt_info.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
                rt_info.StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                rt_info.StartPointName = rdr["StartPointNM"].ToString();
                rt_info.StartPointDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["StartPointDisableFlag"]));
                rt_info.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
                rt_info.EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                rt_info.EndPointName = rdr["EndPointNM"].ToString();
                rt_info.EndPointDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["EndPointDisableFlag"]));
                rt_info.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
                rt_info.ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]);
                rt_info.ItemName = rdr["ItemNM"].ToString();
                rt_info.ItemDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["ItemDisableFlag"]));
                rt_info.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                rt_info.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
                rt_info.FigName = rdr["FigNM"].ToString();
                rt_info.FigDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["FigDisableFlag"]));
                rt_info.TorihikiId = SQLServerUtil.dbDecimal(rdr["TorihikiId"]);
                rt_info.TorihikiCode = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                rt_info.TorihikiName = rdr["TorihikiNM"].ToString();
                rt_info.TorihikiShortName = rdr["TorihikiSNM"].ToString();
                rt_info.TorihikiDisableFlag = this.getIntToBit(SQLServerUtil.dbInt(rdr["TorihikiDisableFlag"]));
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);

                // トラDON_V50の場合
                if (para.TraDonVersionKbn == (int)DefaultProperty.TraDonVersionKbn.V50) 
                {
                    rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                }

                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);

                var ymd = NSKUtil.DecimalWithTimeToDateTime(
                            SQLServerUtil.dbDecimal(rdr["TaskStartDate"]));

                var hm = SQLHelper.dbCustomDecimalToTimeSpan(
                        rdr["TaskStartTime"]);


                if (hm != TimeSpan.MinValue)
                {
                    rt_info.TaskStartDateTime = new DateTime(
                            ymd.Year
                            , ymd.Month
                            , ymd.Day
                            , hm.Hours
                            , hm.Minutes
                            , 0);
                }
                else 
                {
                    rt_info.TaskStartDateTime = new DateTime(
                            ymd.Year
                            , ymd.Month
                            , ymd.Day
                            , 0
                            , 0
                            , 0);
                }


                // 非表示フラグのいづれかがtrueの場合は、非表示
                if (rt_info.TokuisakiDisableFlag
                 || rt_info.CarKindDisableFlag
                 || rt_info.StartPointDisableFlag
                 || rt_info.EndPointDisableFlag
                 || rt_info.ItemDisableFlag
                 || rt_info.FigDisableFlag
                 || rt_info.TorihikiDisableFlag)
                {
                    rt_info.DisableFlag = true;
                }
                else
                {
                    rt_info.DisableFlag = false;
                }

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// SqlTransaction情報、検索条件情報を指定して、情報リストを取得します。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">検索条件情報</param>
        /// <returns>情報リスト</returns>
        public IList<PastPriceDefaultSearchParametersInfo> GetSearchListInternal(SqlTransaction transaction, PastPriceDefaultParameters para)
        {
            //返却用のリスト
            List<PastPriceDefaultSearchParametersInfo> rt_list = new List<PastPriceDefaultSearchParametersInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("	 ISNULL(Hanro.HanroId, 0) HanroId ");
            sb.AppendLine("	,ISNULL(Hanro.HanroCode, 0) HanroCode ");
            sb.AppendLine("	,ISNULL(Hanro.HanroName, '') HanroName ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiId, 0) ToraDONTokuisakiId ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiCd, 0) ToraDONTokuisakiCode ");
            sb.AppendLine("	,ISNULL(ToraDONTokuisaki.TokuisakiNM, '') ToraDONTokuisakiName ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointId, 0) ToraDONHatchiId ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointCd, 0) ToraDONHatchiCode ");
            sb.AppendLine("	,ISNULL(ToraDONHatchi.PointNM, '') ToraDONHatchiName ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointId, 0) ToraDONChakuchiId ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointCd, 0) ToraDONChakuchiCode ");
            sb.AppendLine("	,ISNULL(ToraDONChakuchi.PointNM, '') ToraDONChakuchiName ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemId, 0) ToraDONItemId ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemCd, 0) ToraDONItemCode ");
            sb.AppendLine("	,ISNULL(ToraDONItem.ItemNM, '') ToraDONItemName ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigId, 0) ToraDONFigId ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigCd, 0) ToraDONFigCode ");
            sb.AppendLine("	,ISNULL(ToraDONFig.FigNM, '') ToraDONFigName ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindId, 0) ToraDONCarKindId ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindCd, 0) ToraDONCarKindCode ");
            sb.AppendLine("	,ISNULL(ToraDONCarKind.CarKindNM, '') ToraDONCarKindName ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiId, 0) ToraDONTorihikiId ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiCd, 0) ToraDONTorihikiCode ");
            sb.AppendLine("	,ISNULL(ToraDONTorihikisaki.TorihikiNM, '') ToraDONTorihikiName ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	(SELECT 1 AS DUMMY) DUMMY ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	Hanro ");
            sb.AppendLine("ON Hanro.HanroCode = " + this.dbNullableString(para.HanroCode) + " ");
            sb.AppendLine("AND Hanro.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Tokuisaki AS ToraDONTokuisaki ");
            sb.AppendLine("ON ToraDONTokuisaki.TokuisakiCd = " + this.dbNullableString(para.TokuisakiCode) + " ");
            sb.AppendLine("AND ToraDONTokuisaki.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONTokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Point AS ToraDONHatchi ");
            sb.AppendLine("ON ToraDONHatchi.PointCd = " + this.dbNullableString(para.StartPointCode) + " ");
            sb.AppendLine("AND ToraDONHatchi.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONHatchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Point AS ToraDONChakuchi ");
            sb.AppendLine("ON ToraDONChakuchi.PointCd = " + this.dbNullableString(para.EndPointCode) + " ");
            sb.AppendLine("AND ToraDONChakuchi.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONChakuchi.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Item AS ToraDONItem ");
            sb.AppendLine("ON ToraDONItem.ItemCd = " + this.dbNullableString(para.ItemCode) + " ");
            sb.AppendLine("AND ToraDONItem.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONItem.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Fig AS ToraDONFig ");
            sb.AppendLine("ON ToraDONFig.FigCd = " + this.dbNullableString(para.FigCode) + " ");
            sb.AppendLine("AND ToraDONFig.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONFig.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_CarKind AS ToraDONCarKind ");
            sb.AppendLine("ON ToraDONCarKind.CarKindCd = " + this.dbNullableString(para.CarKindCode) + " ");
            sb.AppendLine("AND ToraDONCarKind.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONCarKind.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("LEFT OUTER JOIN ");
            sb.AppendLine("	TORADON_Torihikisaki AS ToraDONTorihikisaki ");
            sb.AppendLine("ON ToraDONTorihikisaki.TorihikiCd = " + this.dbNullableString(para.TorihikiCode) + " ");
            sb.AppendLine("AND ToraDONTorihikisaki.DisableFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine("AND ToraDONTorihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                PastPriceDefaultSearchParametersInfo rt_info = new PastPriceDefaultSearchParametersInfo
                {
                    HanroId = SQLServerUtil.dbDecimal(rdr["HanroId"]),
                    HanroCode = SQLServerUtil.dbInt(rdr["HanroCode"]),
                    HanroName = rdr["HanroName"].ToString(),
                    TokuisakiId = SQLServerUtil.dbDecimal(rdr["ToraDONTokuisakiId"]),
                    TokuisakiCode = SQLServerUtil.dbInt(rdr["ToraDONTokuisakiCode"]),
                    TokuisakiName = rdr["ToraDONTokuisakiName"].ToString(),
                    StartPointId = SQLServerUtil.dbDecimal(rdr["ToraDONHatchiId"]),
                    StartPointCode = SQLServerUtil.dbInt(rdr["ToraDONHatchiCode"]),
                    StartPointName = rdr["ToraDONHatchiName"].ToString(),
                    EndPointId = SQLServerUtil.dbDecimal(rdr["ToraDONChakuchiId"]),
                    EndPointCode = SQLServerUtil.dbInt(rdr["ToraDONChakuchiCode"]),
                    EndPointName = rdr["ToraDONChakuchiName"].ToString(),
                    ItemId = SQLServerUtil.dbDecimal(rdr["ToraDONItemId"]),
                    ItemCode = SQLServerUtil.dbInt(rdr["ToraDONItemCode"]),
                    ItemName = rdr["ToraDONItemName"].ToString(),
                    CarKindId = SQLServerUtil.dbDecimal(rdr["ToraDONCarKindId"]),
                    CarKindCode = SQLServerUtil.dbInt(rdr["ToraDONCarKindCode"]),
                    CarKindName = rdr["ToraDONCarKindName"].ToString(),
                    FigId = SQLServerUtil.dbDecimal(rdr["ToraDONFigId"]),
                    FigCode = SQLServerUtil.dbInt(rdr["ToraDONFigCode"]),
                    FigName = rdr["ToraDONFigName"].ToString(),
                    TorihikiId = SQLServerUtil.dbDecimal(rdr["ToraDONTorihikiId"]),
                    TorihikiCode = SQLServerUtil.dbInt(rdr["ToraDONTorihikiCode"]),
                    TorihikiName = rdr["ToraDONTorihikiName"].ToString(),
                };

                //返却用の値を返します
                return rt_info;
            }, transaction);
        }

        /// <summary>
        /// DataReaderのNull値の場合、Stringのnull値に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private string dbNullableString(object val)
        {
            if (val == null) return "null";

            return SQLHelper.dbNullableString(val);
        }

        /// <summary>
        /// int型の値をbool型に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool getIntToBit(int val)
        {
            if (val == 0) return false;

            return true;
        }

        #endregion
    }
}
