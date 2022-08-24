using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model;
using Jpsys.HaishaManageV10.ReportModel;
using Jpsys.HaishaManageV10.SQLServerDAL;

namespace Jpsys.HaishaManageV10.ReportDAL
{
    /// <summary>
    /// 受注配車一覧表のデータアクセスレイヤです。
    /// </summary>
    public class JuchuHaishaIchiranHyoRpt
    {
        /// <summary>
        /// アプリケーション認証情報の初期化
        /// </summary>
        private AppAuthInfo authInfo =
            new AppAuthInfo
            {
                OperatorId = 0,
                TerminalId = "",
                UserProcessId = "",
                UserProcessName = ""
            };

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public JuchuHaishaIchiranHyoRpt()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public JuchuHaishaIchiranHyoRpt(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        /// <summary>
        /// 抽出条件情報を指定して、受注配車一覧表情報のリストを取得します。
        /// </summary>
        /// <param name="searchParameter">抽出条件情報</param>
        /// <returns>受注配車一覧表情報のリスト</returns>
        public List<JuchuHaishaIchiranHyoRptInfo> GetReportData(JuchuHaishaIchiranHyoSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetSelectJuchuSQL(para));

            String mySql = sb.ToString();

            List<JuchuHaishaIchiranHyoRptInfo> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                JuchuHaishaIchiranHyoRptInfo rt_info = new JuchuHaishaIchiranHyoRptInfo();

                //キー情報
                rt_info.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]);
                rt_info.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                rt_info.HaishaId = SQLServerUtil.dbDecimal(rdr["HaishaId"]);
                rt_info.HaishaRowIndex = (int)SQLServerUtil.dbBigint(rdr["HaishaRowIndex"]);

                //受注情報
                rt_info.BranchOfficeCd = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]);
                rt_info.BranchOfficeSNM = (rdr["BranchOfficeSNM"]).ToString();
                rt_info.TokuisakiCd = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.TokuisakiNM = (rdr["TokuisakiNM"]).ToString();
                rt_info.HanroCd = SQLServerUtil.dbInt(rdr["HanroCd"]);
                rt_info.HanroNm = rdr["HanroNm"].ToString();
                rt_info.OfukuKbnNm = rdr["OfukuKbnNm"].ToString();
                rt_info.StartPointCd = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                rt_info.StartPointNM = rdr["StartPointNM"].ToString();
                rt_info.EndPointCd = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                rt_info.EndPointNM = rdr["EndPointNM"].ToString();
                rt_info.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                rt_info.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                rt_info.StartYMD = SQLHelper.dbDate(rdr["StartYMD"]);
                rt_info.ItemCd = SQLServerUtil.dbInt(rdr["ItemCd"]);
                rt_info.ItemNM = rdr["ItemNM"].ToString();
                rt_info.CarCd = SQLServerUtil.dbInt(rdr["CarCd"]);
                rt_info.LicPlateCarNo = (rdr["LicPlateCarNo"]).ToString();
                rt_info.CarKindCd = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                rt_info.CarKindSNM = (rdr["CarKindSNM"]).ToString();
                rt_info.DriverCd = SQLServerUtil.dbInt(rdr["DriverCd"]);
                rt_info.DriverNm = (rdr["DriverNm"]).ToString();
                rt_info.TorihikiCd = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                rt_info.TorihikiSNm = rdr["TorihikiSNM"].ToString();
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.FigNm = (rdr["FigNm"]).ToString();
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                rt_info.TaxDispKbnShortNm = (rdr["TaxDispKbnShortNm"]).ToString();
                rt_info.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                rt_info.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                rt_info.CharterTaxDispKbnShortNm = (rdr["CharterTaxDispKbnShortNm"]).ToString();
                rt_info.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                rt_info.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                rt_info.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);
                rt_info.JuchuTantoCd = SQLServerUtil.dbInt(rdr["JuchuTantoCd"]);
                rt_info.JuchuTantoNm = (rdr["JuchuTantoNm"]).ToString();
                rt_info.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["JuchuSlipNo"]);
                rt_info.OwnerCd = SQLServerUtil.dbInt(rdr["OwnerCd"]);
                rt_info.OwnerNM = (rdr["OwnerNM"]).ToString();
                rt_info.Memo = (rdr["Memo"]).ToString();
                rt_info.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                rt_info.MagoYoshasaki = (rdr["MagoYoshasaki"]).ToString();
                rt_info.ReceivedFlag = SQLHelper.dbBit(rdr["ReceivedFlag"]);

                //配車情報
                rt_info.Haisha_StartPointCd = SQLServerUtil.dbInt(rdr["Haisha_StartPointCd"]);
                rt_info.Haisha_StartPointNM = rdr["Haisha_StartPointNM"].ToString();
                rt_info.Haisha_EndPointCd = SQLServerUtil.dbInt(rdr["Haisha_EndPointCd"]);
                rt_info.Haisha_EndPointNM = rdr["Haisha_EndPointNM"].ToString();
                rt_info.Haisha_TaskStartDateTime = SQLHelper.dbDate(rdr["Haisha_TaskStartDateTime"]);
                rt_info.Haisha_TaskEndDateTime = SQLHelper.dbDate(rdr["Haisha_TaskEndDateTime"]);
                rt_info.Haisha_StartYMD = SQLHelper.dbDate(rdr["Haisha_StartYMD"]);
                rt_info.Haisha_ItemCd = SQLServerUtil.dbInt(rdr["Haisha_ItemCd"]);
                rt_info.Haisha_ItemNM = rdr["Haisha_ItemNM"].ToString();
                rt_info.Haisha_CarCd = SQLServerUtil.dbInt(rdr["Haisha_CarCd"]);
                rt_info.Haisha_LicPlateCarNo = (rdr["Haisha_LicPlateCarNo"]).ToString();
                rt_info.Haisha_CarKindCd = SQLServerUtil.dbInt(rdr["Haisha_CarKindCd"]);
                rt_info.Haisha_CarKindSNM = (rdr["Haisha_CarKindSNM"]).ToString();
                rt_info.Haisha_DriverCd = SQLServerUtil.dbInt(rdr["Haisha_DriverCd"]);
                rt_info.Haisha_DriverNm = (rdr["Haisha_DriverNm"]).ToString();
                rt_info.Haisha_TorihikiCd = SQLServerUtil.dbInt(rdr["Haisha_TorihikiCd"]);
                rt_info.Haisha_TorihikiSNm = rdr["Haisha_TorihikiSNM"].ToString();
                rt_info.Haisha_AtPrice = SQLServerUtil.dbDecimal(rdr["Haisha_AtPrice"]);
                rt_info.Haisha_Weight = SQLServerUtil.dbDecimal(rdr["Haisha_Weight"]);
                rt_info.Haisha_Number = SQLServerUtil.dbDecimal(rdr["Haisha_Number"]);
                rt_info.Haisha_FigNm = (rdr["Haisha_FigNm"]).ToString();
                rt_info.Haisha_PriceInPrice = SQLServerUtil.dbDecimal(rdr["Haisha_PriceInPrice"]);
                rt_info.Haisha_TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["Haisha_TollFeeInPrice"]);
                rt_info.Haisha_TaxDispKbnShortNm = (rdr["Haisha_TaxDispKbnShortNm"]).ToString();
                rt_info.Haisha_AddUpYMD = SQLHelper.dbDate(rdr["Haisha_AddUpYMD"]);
                rt_info.Haisha_FixFlag = SQLHelper.dbBit(rdr["Haisha_FixFlag"]);
                rt_info.Haisha_PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["Haisha_PriceInCharterPrice"]);
                rt_info.Haisha_TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["Haisha_TollFeeInCharterPrice"]);
                rt_info.Haisha_CharterTaxDispKbnShortNm = (rdr["Haisha_CharterTaxDispKbnShortNm"]).ToString();
                rt_info.Haisha_CharterAddUpYMD = SQLHelper.dbDate(rdr["Haisha_CharterAddUpYMD"]);
                rt_info.Haisha_CharterFixFlag = SQLHelper.dbBit(rdr["Haisha_CharterFixFlag"]);
                rt_info.Haisha_FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["Haisha_FutaigyomuryoInPrice"]);
                rt_info.Haisha_JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["Haisha_JomuinUriageKingaku"]);
                rt_info.Haisha_OwnerCd = SQLServerUtil.dbInt(rdr["Haisha_OwnerCd"]);
                rt_info.Haisha_OwnerNM = (rdr["Haisha_OwnerNm"]).ToString();
                rt_info.Haisha_Biko = (rdr["Haisha_Biko"]).ToString();
                rt_info.Haisha_ReuseYMD = SQLHelper.dbDate(rdr["Haisha_ReuseYMD"]);
                rt_info.Haisha_MagoYoshasaki = (rdr["Haisha_MagoYoshasaki"]).ToString();
                rt_info.ToraDONVersionKbn = para.TraDonVersionKbn;

                //返却用の値を返します
                return rt_info;
            }, null);

            if (list == null || list.Count == 0)
            {
                return new List<JuchuHaishaIchiranHyoRptInfo>();
            }

            return list.ToList();
        }

        #region プライベートメソッド

        /// <summary>
        /// SELECT_SQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>SELECT_SQL</returns>
        private string GetSelectJuchuSQL(JuchuHaishaIchiranHyoSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            //キー情報
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeId          AS BranchOfficeId, ");
            sb.AppendLine("   Juchu.JuchuId                                AS JuchuId, ");
            sb.AppendLine("   Haisha.HaishaId                              AS HaishaId, ");
            sb.AppendLine("   ROW_NUMBER() OVER(PARTITION BY Juchu.JuchuId ORDER BY Haisha.StartYMD) AS HaishaRowIndex, ");
            //受注情報                                              
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeCd          AS BranchOfficeCd, ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeSNM         AS BranchOfficeSNM, ");
            sb.AppendLine("   TORADON_Tokuisaki.TokuisakiCd                AS TokuisakiCd, ");
            sb.AppendLine("   Juchu.TokuisakiNM                            AS TokuisakiNM, ");
            sb.AppendLine("   Hanro.HanroCode                              AS HanroCd, ");
            sb.AppendLine("   Hanro.HanroName                              AS HanroNm, ");
            sb.AppendLine("   OfukuKbn.SystemNameName                      AS OfukuKbnNm, ");
            sb.AppendLine("   JuchuStartPoint.PointCd                      AS StartPointCd, ");
            sb.AppendLine("   Juchu.StartPointNM                           AS StartPointNM, ");
            sb.AppendLine("   JuchuEndPoint.PointCd                        AS EndPointCd, ");
            sb.AppendLine("   Juchu.EndPointNM                             AS EndPointNM, ");
            sb.AppendLine("   Juchu.TaskStartDateTime                      AS TaskStartDateTime, ");
            sb.AppendLine("   Juchu.TaskEndDateTime                        AS TaskEndDateTime, ");
            sb.AppendLine("   Juchu.TaskStartDateTime                      AS StartYMD, ");
            sb.AppendLine("   TORADON_Item.ItemCd                          AS ItemCd, ");
            sb.AppendLine("   Juchu.ItemNM                                 AS ItemNM, ");
            sb.AppendLine("   TORADON_Car.CarCd                            AS CarCd, ");
            sb.AppendLine("   CASE WHEN ISNULL(TORADON_Car.CarKbn, 0) = "
                + ((int)DefaultProperty.CarKbn.Yosha).ToString()
                + " THEN Juchu.LicPlateCarNo ELSE ISNULL(TORADON_Car.LicPlateCarNo, '') END LicPlateCarNo, ");
            sb.AppendLine("   TORADON_CarKind.CarKindCd                    AS CarKindCd, ");
            sb.AppendLine("   TORADON_CarKind.CarKindSNM                   AS CarKindSNM, ");
            sb.AppendLine("   Driver.StaffCd                               AS DriverCd, ");
            sb.AppendLine("   Driver.StaffNm                               AS DriverNm, ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiCd              AS TorihikiCd, ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiSNm             AS TorihikiSNm, ");
            sb.AppendLine("   Juchu.AtPrice                                AS AtPrice, ");
            sb.AppendLine("   Juchu.Weight                                 AS Weight, ");
            sb.AppendLine("   Juchu.Number                                 AS Number, ");
            sb.AppendLine("   TORADON_Fig.FigNm                            AS FigNm, ");
            sb.AppendLine("   Juchu.PriceInPrice                           AS PriceInPrice, ");
            sb.AppendLine("   Juchu.TollFeeInPrice                         AS TollFeeInPrice, ");
            sb.AppendLine("   TaxDispKbnShort.SystemNameName               AS TaxDispKbnShortNm, ");
            sb.AppendLine("   Juchu.AddUpYMD                               AS AddUpYMD, ");
            sb.AppendLine("   Juchu.FixFlag                                AS FixFlag, ");
            sb.AppendLine("   Juchu.PriceInCharterPrice                    AS PriceInCharterPrice, ");
            sb.AppendLine("   Juchu.TollFeeInCharterPrice                  AS TollFeeInCharterPrice, ");
            sb.AppendLine("   CharterTaxDispKbnShort.SystemNameName        AS CharterTaxDispKbnShortNm, ");
            sb.AppendLine("   Juchu.CharterAddUpYMD                        AS CharterAddUpYMD, ");
            sb.AppendLine("   Juchu.CharterFixFlag                         AS CharterFixFlag, ");
            sb.AppendLine("   Juchu.FutaigyomuryoInPrice                   AS FutaigyomuryoInPrice, ");
            sb.AppendLine("   Juchu.JomuinUriageKingaku                    AS JomuinUriageKingaku, ");
            sb.AppendLine("   JuchuTanto.StaffCd                           AS JuchuTantoCd,");
            sb.AppendLine("   JuchuTanto.StaffNm                           AS JuchuTantoNm,");
            sb.AppendLine("   Juchu.JuchuSlipNo                            AS JuchuSlipNo, ");
            sb.AppendLine("   TORADON_Owner.OwnerCd                        AS OwnerCd, ");
            sb.AppendLine("   Juchu.OwnerNM                                AS OwnerNM, ");
            sb.AppendLine("   Juchu.Memo                                   AS Memo, ");
            sb.AppendLine("   Juchu.ReuseYMD                               AS ReuseYMD, ");
            sb.AppendLine("   Juchu.MagoYoshasaki                          AS MagoYoshasaki, ");
            sb.AppendLine("   Juchu.ReceivedFlag                           AS ReceivedFlag, ");
            //配車情報
            sb.AppendLine("   HaishaStartPoint.PointCd                     AS Haisha_StartPointCd, ");
            sb.AppendLine("   Haisha.StartPointNM                          AS Haisha_StartPointNM, ");
            sb.AppendLine("   HaishaEndPoint.PointCd                       AS Haisha_EndPointCd, ");
            sb.AppendLine("   Haisha.EndPointNM                            AS Haisha_EndPointNM, ");
            sb.AppendLine("   Haisha.TaskStartDateTime                     AS Haisha_TaskStartDateTime, ");
            sb.AppendLine("   Haisha.TaskEndDateTime                       AS Haisha_TaskEndDateTime, ");
            sb.AppendLine("   Haisha.StartYMD                              AS Haisha_StartYMD, ");
            sb.AppendLine("   HaishaTORADON_Item.ItemCd                    AS Haisha_ItemCd, ");
            sb.AppendLine("   Haisha.ItemNM                                AS Haisha_ItemNM, ");
            sb.AppendLine("   HaishaTORADON_Car.CarCd                      AS Haisha_CarCd, ");
            sb.AppendLine("   CASE WHEN ISNULL(HaishaTORADON_Car.CarKbn, 0) = "
                + ((int)DefaultProperty.CarKbn.Yosha).ToString()
                + " THEN Haisha.LicPlateCarNo ELSE ISNULL(HaishaTORADON_Car.LicPlateCarNo, '') END Haisha_LicPlateCarNo, ");
            sb.AppendLine("   HaishaTORADON_CarKind.CarKindCd              AS Haisha_CarKindCd, ");
            sb.AppendLine("   HaishaTORADON_CarKind.CarKindSNM             AS Haisha_CarKindSNM, ");
            sb.AppendLine("   HaishaDriver.StaffCd                         AS Haisha_DriverCd, ");
            sb.AppendLine("   HaishaDriver.StaffNm                         AS Haisha_DriverNm, ");
            sb.AppendLine("   HaishaOwner.OwnerCd                          AS Haisha_OwnerCd, ");
            sb.AppendLine("   Haisha.OwnerNM                               AS Haisha_OwnerNm, ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.TorihikiCd        AS Haisha_TorihikiCd, ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.TorihikiSNm       AS Haisha_TorihikiSNm, ");
            sb.AppendLine("   Haisha.AtPrice                               AS Haisha_AtPrice, ");
            sb.AppendLine("   Haisha.Weight                                AS Haisha_Weight, ");
            sb.AppendLine("   Haisha.Number                                AS Haisha_Number, ");
            sb.AppendLine("   HaishaTORADON_Fig.FigNm                      AS Haisha_FigNm, ");
            sb.AppendLine("   Haisha.PriceInPrice                          AS Haisha_PriceInPrice, ");
            sb.AppendLine("   Haisha.TollFeeInPrice                        AS Haisha_TollFeeInPrice, ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameName         AS Haisha_TaxDispKbnShortNm, ");
            sb.AppendLine("   Haisha.AddUpYMD                              AS Haisha_AddUpYMD, ");
            sb.AppendLine("   Haisha.FixFlag                               AS Haisha_FixFlag, ");
            sb.AppendLine("   Haisha.PriceInCharterPrice                   AS Haisha_PriceInCharterPrice, ");
            sb.AppendLine("   Haisha.TollFeeInCharterPrice                 AS Haisha_TollFeeInCharterPrice, ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameName  AS Haisha_CharterTaxDispKbnShortNm, ");
            sb.AppendLine("   Haisha.CharterAddUpYMD                       AS Haisha_CharterAddUpYMD, ");
            sb.AppendLine("   Haisha.CharterFixFlag                        AS Haisha_CharterFixFlag, ");
            sb.AppendLine("   Haisha.FutaigyomuryoInPrice                  AS Haisha_FutaigyomuryoInPrice, ");
            sb.AppendLine("   Haisha.JomuinUriageKingaku                   AS Haisha_JomuinUriageKingaku, ");
            sb.AppendLine("   Haisha.Biko                                  AS Haisha_Biko, ");
            sb.AppendLine("   Juchu.TaskStartDateTime                      AS Haisha_OrderByJuchuStartYMD, ");
            sb.AppendLine("   Haisha.StartYMD                              AS Haisha_OrderByHaishaStartYMD, ");
            sb.AppendLine("   Haisha.ReuseYMD                              AS Haisha_ReuseYMD, ");
            sb.AppendLine("   Haisha.MagoYoshasaki                         AS Haisha_MagoYoshasaki ");
            sb.AppendLine(" FROM Juchu ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" Haisha ON ");
            sb.AppendLine("   Haisha.JuchuId = Juchu.JuchuId AND ");
            sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            //受注名称解決
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_BranchOffice ON ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeId = Juchu.BranchOfficeId AND ");
            sb.AppendLine("   TORADON_BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki ON ");
            sb.AppendLine("   TORADON_Tokuisaki.TokuisakiId = Juchu.TokuisakiId AND ");
            sb.AppendLine("   TORADON_Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" Hanro ON ");
            sb.AppendLine("   Hanro.HanroId = Juchu.HanroId AND ");
            sb.AppendLine("   Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff JuchuTanto ON ");
            sb.AppendLine("   JuchuTanto.StaffId = Juchu.JuchuTantoId AND ");
            sb.AppendLine("   JuchuTanto.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName OfukuKbn ON ");
            sb.AppendLine("   OfukuKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.OfukuKbn).ToString() + " AND ");
            sb.AppendLine("   OfukuKbn.SystemNameCode = Juchu.OfukuKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Car ON ");
            sb.AppendLine("   TORADON_Car.CarId = Juchu.CarId AND ");
            sb.AppendLine("   TORADON_Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_CarKind ON ");
            sb.AppendLine("   TORADON_CarKind.CarKindId = Juchu.CarKindId AND ");
            sb.AppendLine("   TORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff Driver ON ");
            sb.AppendLine("   Driver.StaffId = Juchu.DriverId AND ");
            sb.AppendLine("   Driver.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki ON ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiId = Juchu.CarOfChartererId AND ");
            sb.AppendLine("   TORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Owner ON ");
            sb.AppendLine("   TORADON_Owner.OwnerId = Juchu.OwnerId AND ");
            sb.AppendLine("   TORADON_Owner.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Item ON ");
            sb.AppendLine("   TORADON_Item.ItemId = Juchu.ItemId AND ");
            sb.AppendLine("   TORADON_Item.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Fig ON ");
            sb.AppendLine("   TORADON_Fig.FigId = Juchu.FigId AND ");
            sb.AppendLine("   TORADON_Fig.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN TORADON_Point JuchuStartPoint ");
            sb.AppendLine("   ON Juchu.StartPointId = JuchuStartPoint.PointId ");
            sb.AppendLine("   AND JuchuStartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN TORADON_Point JuchuEndPoint ");
            sb.AppendLine("   ON Juchu.EndPointId = JuchuEndPoint.PointId ");
            sb.AppendLine("   AND JuchuEndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName TaxDispKbnShort ON ");
            sb.AppendLine("   TaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   TaxDispKbnShort.SystemNameCode = Juchu.TaxDispKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName CharterTaxDispKbnShort ON ");
            sb.AppendLine("   CharterTaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   CharterTaxDispKbnShort.SystemNameCode = Juchu.CharterTaxDispKbn ");

            //配車名称解決
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Car HaishaTORADON_Car ON ");
            sb.AppendLine("   HaishaTORADON_Car.CarId = Haisha.CarId AND ");
            sb.AppendLine("   HaishaTORADON_Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_CarKind HaishaTORADON_CarKind ON ");
            sb.AppendLine("   HaishaTORADON_CarKind.CarKindId = Haisha.CarKindId AND ");
            sb.AppendLine("   HaishaTORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff HaishaDriver ON ");
            sb.AppendLine("   HaishaDriver.StaffId = Haisha.DriverId AND ");
            sb.AppendLine("   HaishaDriver.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Owner HaishaOwner ON ");
            sb.AppendLine("   HaishaOwner.OwnerId = Haisha.OwnerId AND ");
            sb.AppendLine("   HaishaOwner.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki HaishaTORADON_Torihikisaki ON ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.TorihikiId = Haisha.CarOfChartererId AND ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Owner HaishaTORADON_Owner ON ");
            sb.AppendLine("   HaishaTORADON_Owner.OwnerId = Haisha.OwnerId AND ");
            sb.AppendLine("   HaishaTORADON_Owner.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Item HaishaTORADON_Item ON ");
            sb.AppendLine("   HaishaTORADON_Item.ItemId = Haisha.ItemId AND ");
            sb.AppendLine("   HaishaTORADON_Item.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Fig HaishaTORADON_Fig ON ");
            sb.AppendLine("   HaishaTORADON_Fig.FigId = Haisha.FigId AND ");
            sb.AppendLine("   HaishaTORADON_Fig.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN TORADON_Point HaishaStartPoint ");
            sb.AppendLine("   ON HaishaStartPoint.PointId = Haisha.StartPointId ");
            sb.AppendLine("   AND HaishaStartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN TORADON_Point HaishaEndPoint ");
            sb.AppendLine("   ON HaishaEndPoint.PointId = Haisha.EndPointId ");
            sb.AppendLine("   AND HaishaEndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName HaishaTaxDispKbnShort ON ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameCode = Haisha.TaxDispKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName HaishaCharterTaxDispKbnShort ON ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameCode = Haisha.CharterTaxDispKbn ");

            //抽出条件
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(this.GetSelectJuchuWhereSQL(para));

            return sb.ToString();
        }

        /// <summary>
        /// SELECT_SQLのWhere句を取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>SELECT_SQLのWhere句</returns>
        private string GetSelectJuchuWhereSQL(JuchuHaishaIchiranHyoSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            // 日付指定区分によって対象日付の比較項目を判断
            if ((int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate == para.FilterDateKbns)
            {
                // 作業開始日付
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.TaskStartDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.TaskStartDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            else if ((int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate == para.FilterDateKbns)
            {
                // 作業終了日付
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.TaskEndDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.TaskEndDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            else if ((int)BizProperty.DefaultProperty.FilterDateKbns.AddUpDate == para.FilterDateKbns)
            {
                // 計上日付
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.AddUpYMD BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.AddUpYMD BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            // 営業所ID
            if (0 < para.BranchOfficeId)
            {
                sb.AppendLine(" AND Juchu.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");
            }
            // 車両ID
            if (0 < para.CarId)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.CarId = " + para.CarId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.CarId = " + para.CarId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            // 車種ID
            if (0 < para.CarId)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.CarKindId = " + para.CarKindId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.CarKindId = " + para.CarKindId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            // 乗務員ID
            if (0 < para.DriverId)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.DriverId = " + para.DriverId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.DriverId = " + para.DriverId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            // 傭車先ID
            if (0 < para.CarOfChartererId)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine(" ( ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Juchu.CarOfChartererId = " + para.CarOfChartererId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine("  OR ");
                sb.AppendLine("  ( ");
                sb.AppendLine("   Haisha.CarOfChartererId = " + para.CarOfChartererId.ToString() + " ");
                sb.AppendLine("  ) ");
                sb.AppendLine(" ) ");
            }
            // 得意先ID
            if (0 < para.TokuisakiId)
            {
                sb.AppendLine(" AND Juchu.TokuisakiId = " + para.TokuisakiId.ToString() + " ");
            }
            // 請求部門ID
            if (0 < para.ClmClassId)
            {
                sb.AppendLine(" AND Juchu.ClmClassId = " + para.ClmClassId.ToString() + " ");
            }
            // 請負ID
            if (0 < para.ContractId)
            {
                sb.AppendLine(" AND Juchu.ContractId = " + para.ContractId.ToString() + " ");
            }
            // 販路ID
            if (0 < para.HanroId)
            {
                sb.AppendLine(" AND Juchu.HanroId = " + para.HanroId.ToString() + " ");
            }
            // 受注担当ID
            if (0 < para.JuchuTantoId)
            {
                sb.AppendLine(" AND Juchu.JuchuTantoId = " + para.JuchuTantoId.ToString() + " ");
            }
            // 伝票№（From）
            if (0 < para.JuchuSlipNoFrom)
            {
                sb.AppendLine(" AND Juchu.JuchuSlipNo >= " + para.JuchuSlipNoFrom.ToString() + " ");
            }
            // 伝票№（To）
            if (0 < para.JuchuSlipNoTo)
            {
                sb.AppendLine(" AND Juchu.JuchuSlipNo <= " + para.JuchuSlipNoTo.ToString() + " ");
            }

            return sb.ToString();
        }

        #endregion
    }
}
