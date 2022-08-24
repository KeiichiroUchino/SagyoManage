using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using Jpsys.HaishaManageV10.BizProperty;
using Jpsys.HaishaManageV10.Model;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 配車情報CSV出力のデータアクセスレイヤです。
    /// </summary>
    public class HaishaCsvExport
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
        /// HaishaCsvExportクラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaCsvExport()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、配車情報CSV出力の
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaishaCsvExport(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 検索条件情報を指定して、配車情報CSV出力のリストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>配車情報CSV出力のリスト</returns>
        public IList<HaishaCsvExportInfo> GetList(HaishaCsvExportSearchParameter para = null)
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
        public IList<HaishaCsvExportInfo> GetListInternal(SqlTransaction transaction, HaishaCsvExportSearchParameter para)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetSelectJuchuSQL(para));

            String mySql = sb.ToString();

            List<HaishaCsvExportInfo> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                HaishaCsvExportInfo rt_info = new HaishaCsvExportInfo();

                rt_info.BranchOfficeCd = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]);
                rt_info.BranchOfficeSNM = rdr["BranchOfficeSNM"].ToString();
                rt_info.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                rt_info.CarKbnName = rdr["CarKbnName"].ToString();
                rt_info.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                rt_info.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                rt_info.TokuisakiCd = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.TokuisakiNM = rdr["TokuisakiNM"].ToString();
                rt_info.ClmClassCd = SQLServerUtil.dbInt(rdr["ClmClassCd"]);
                rt_info.ClmClassNM = rdr["ClmClassNM"].ToString();
                rt_info.ContractCd = SQLServerUtil.dbInt(rdr["ContractCd"]);
                rt_info.ContractNM = rdr["ContractNM"].ToString();
                rt_info.StartPointCd = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                rt_info.StartPointNM = rdr["StartPointNM"].ToString();
                rt_info.EndPointCd = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                rt_info.EndPointNM = rdr["EndPointNM"].ToString();
                rt_info.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                rt_info.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                rt_info.StartYMD = SQLHelper.dbDate(rdr["StartYMD"]);
                rt_info.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                rt_info.ItemCd = SQLServerUtil.dbInt(rdr["ItemCd"]);
                rt_info.ItemNM = rdr["ItemNM"].ToString();
                rt_info.OwnerCd = SQLServerUtil.dbInt(rdr["OwnerCd"]);
                rt_info.OwnerNM = rdr["OwnerNM"].ToString();
                rt_info.CarCd = SQLServerUtil.dbInt(rdr["CarCd"]);
                rt_info.LicPlateCarNo = (rdr["LicPlateCarNo"]).ToString();
                rt_info.CarKindCd = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                rt_info.CarKindNM = (rdr["CarKindNM"]).ToString();
                rt_info.DriverCd = SQLServerUtil.dbInt(rdr["DriverCd"]);
                rt_info.DriverNm = (rdr["DriverNm"]).ToString();
                rt_info.TorihikiCd = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                rt_info.TorihikiNm = rdr["TorihikiNM"].ToString();
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.FigNm = (rdr["FigNm"]).ToString();
                rt_info.Price = SQLServerUtil.dbDecimal(rdr["Price"]);
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                rt_info.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]);
                rt_info.PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]);
                rt_info.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]);
                rt_info.PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]);
                rt_info.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]);
                rt_info.TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]);
                rt_info.TaxDispKbnShortNm = (rdr["TaxDispKbnShortNm"]).ToString();
                rt_info.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                rt_info.FixFlagInt = NSKUtil.BoolToInt(SQLHelper.dbBit(rdr["FixFlag"]));
                rt_info.CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]);
                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                rt_info.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]);
                rt_info.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]);
                rt_info.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]);
                rt_info.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]);
                rt_info.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]);
                rt_info.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]);
                rt_info.CharterTaxDispKbnShortNm = (rdr["CharterTaxDispKbnShortNm"]).ToString();
                rt_info.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                rt_info.CharterFixFlagInt = NSKUtil.BoolToInt(SQLHelper.dbBit(rdr["CharterFixFlag"]));
                rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                rt_info.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);
                rt_info.MagoYoshasaki = (rdr["MagoYoshasaki"]).ToString();
                rt_info.ReceivedFlag = SQLHelper.dbBit(rdr["ReceivedFlag"]);
                rt_info.ReceivedFlagInt = NSKUtil.BoolToInt(SQLHelper.dbBit(rdr["ReceivedFlag"]));
                rt_info.Memo = (rdr["Memo"]).ToString();
                rt_info.Comment = (rdr["Biko"]).ToString();

                //返却用の値を返します
                return rt_info;
            }, null);

            if (list == null || list.Count == 0)
            {
                return new List<HaishaCsvExportInfo>();
            }

            return list.ToList();
        }

        /// <summary>
        /// SELECT_SQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>SELECT_SQL</returns>
        private string GetSelectJuchuSQL(HaishaCsvExportSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");

            sb.AppendLine("   ISNULL(TORADON_BranchOffice.BranchOfficeCd, 0)     AS BranchOfficeCd, ");
            sb.AppendLine("   ISNULL(TORADON_BranchOffice.BranchOfficeSNM, '')   AS BranchOfficeSNM, ");
            sb.AppendLine("   ISNULL(HaishaTORADON_Car.CarKbn, 0)                AS CarKbn, ");
            sb.AppendLine("   ISNULL(HaishaCarKbn.SystemNameName, 0)             AS CarKbnName, ");
            sb.AppendLine("   Haisha.AddUpYMD                                    AS AddUpYMD, ");
            sb.AppendLine("   Haisha.CharterAddUpYMD                             AS CharterAddUpYMD, ");
            sb.AppendLine("   ISNULL(TORADON_Tokuisaki.TokuisakiCd, 0)           AS TokuisakiCd, ");
            sb.AppendLine("   ISNULL(TORADON_Tokuisaki.TokuisakiNM, '')          AS TokuisakiNM, ");
            sb.AppendLine("   ISNULL(TORADON_ClmClass.ClmClassCd, 0)             AS ClmClassCd, ");
            sb.AppendLine("   ISNULL(TORADON_ClmClass.ClmClassNM, '')            AS ClmClassNM, ");
            sb.AppendLine("   ISNULL(TORADON_Contract.ContractCd, 0)             AS ContractCd, ");
            sb.AppendLine("   ISNULL(TORADON_Contract.ContractNM, '')            AS ContractNM, ");
            sb.AppendLine("   ISNULL(Hanro.HanroCode, 0)                         AS HanroCode, ");
            sb.AppendLine("   ISNULL(Hanro.HanroName, '')                        AS HanroName, ");
            sb.AppendLine("   Haisha.TaskStartDateTime                           AS TaskStartDateTime, ");
            sb.AppendLine("   Haisha.StartYMD                                    AS StartYMD, ");
            sb.AppendLine("   Haisha.TaskEndDateTime                             AS TaskEndDateTime, ");
            sb.AppendLine("   Haisha.ReuseYMD                                    AS ReuseYMD, ");
            sb.AppendLine("   HaishaStartPoint.PointCd                           AS StartPointCd, ");
            sb.AppendLine("   Haisha.StartPointNM                                AS StartPointNM, ");
            sb.AppendLine("   HaishaEndPoint.PointCd                             AS EndPointCd, ");
            sb.AppendLine("   Haisha.EndPointNM                                  AS EndPointNM, ");
            sb.AppendLine("   HaishaTORADON_Item.ItemCd                          AS ItemCd, ");
            sb.AppendLine("   Haisha.ItemNM                                      AS ItemNM, ");
            sb.AppendLine("   HaishaTORADON_Owner.OwnerCd                        AS OwnerCd, ");
            sb.AppendLine("   Haisha.OwnerNM                                     AS OwnerNM, ");
            sb.AppendLine("   HaishaTORADON_Car.CarCd                            AS CarCd, ");
            sb.AppendLine("   CASE WHEN ISNULL(HaishaTORADON_Car.CarKbn, 0) = "
                + ((int)DefaultProperty.CarKbn.Yosha).ToString()
                + " THEN Haisha.LicPlateCarNo ELSE ISNULL(HaishaTORADON_Car.LicPlateCarNo, '') END AS LicPlateCarNo, ");
            sb.AppendLine("   HaishaTORADON_CarKind.CarKindCd                    AS CarKindCd, ");
            sb.AppendLine("   HaishaTORADON_CarKind.CarKindNM                    AS CarKindNM, ");
            sb.AppendLine("   HaishaDriver.StaffCd                               AS DriverCd, ");
            sb.AppendLine("   HaishaDriver.StaffNm                               AS DriverNm, ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.TorihikiCd              AS TorihikiCd, ");
            sb.AppendLine("   HaishaTORADON_Torihikisaki.TorihikiNm              AS TorihikiNm, ");
            sb.AppendLine("   Haisha.AtPrice                                     AS AtPrice, ");
            sb.AppendLine("   Haisha.Weight                                      AS Weight, ");
            sb.AppendLine("   Haisha.Number                                      AS Number, ");
            sb.AppendLine("   HaishaTORADON_Fig.FigNm                            AS FigNm, ");
            sb.AppendLine("   Haisha.Price                                       AS Price, ");
            sb.AppendLine("   Haisha.PriceInPrice                                AS PriceInPrice, ");
            sb.AppendLine("   Haisha.TollFeeInPrice                              AS TollFeeInPrice, ");
            sb.AppendLine("   Haisha.PriceOutTaxCalc                             AS PriceOutTaxCalc, ");
            sb.AppendLine("   Haisha.PriceOutTax                                 AS PriceOutTax, ");
            sb.AppendLine("   Haisha.PriceInTaxCalc                              AS PriceInTaxCalc, ");
            sb.AppendLine("   Haisha.PriceInTax                                  AS PriceInTax, ");
            sb.AppendLine("   Haisha.PriceNoTaxCalc                              AS PriceNoTaxCalc, ");
            sb.AppendLine("   Haisha.TaxDispKbn                                  AS TaxDispKbn, ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameName               AS TaxDispKbnShortNm, ");
            sb.AppendLine("   Haisha.FixFlag                                     AS FixFlag, ");
            sb.AppendLine("   Haisha.CharterPrice                                AS CharterPrice, ");
            sb.AppendLine("   Haisha.PriceInCharterPrice                         AS PriceInCharterPrice, ");
            sb.AppendLine("   Haisha.TollFeeInCharterPrice                       AS TollFeeInCharterPrice, ");
            sb.AppendLine("   Haisha.CharterPriceOutTaxCalc                      AS CharterPriceOutTaxCalc, ");
            sb.AppendLine("   Haisha.CharterPriceOutTax                          AS CharterPriceOutTax, ");
            sb.AppendLine("   Haisha.CharterPriceInTaxCalc                       AS CharterPriceInTaxCalc, ");
            sb.AppendLine("   Haisha.CharterPriceInTax                           AS CharterPriceInTax, ");
            sb.AppendLine("   Haisha.CharterPriceNoTaxCalc                       AS CharterPriceNoTaxCalc, ");
            sb.AppendLine("   Haisha.CharterTaxDispKbn                           AS CharterTaxDispKbn, ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameName        AS CharterTaxDispKbnShortNm, ");
            sb.AppendLine("   Haisha.CharterFixFlag                              AS CharterFixFlag, ");
            sb.AppendLine("   Haisha.FutaigyomuryoInPrice                        AS FutaigyomuryoInPrice, ");
            sb.AppendLine("   Haisha.JomuinUriageKingaku                         AS JomuinUriageKingaku, ");
            sb.AppendLine("   Haisha.MagoYoshasaki                               AS MagoYoshasaki, ");
            sb.AppendLine("   Juchu.ReceivedFlag                                 AS ReceivedFlag, ");
            sb.AppendLine("   Juchu.Memo                                         AS Memo, ");
            sb.AppendLine("   Haisha.Biko                                        AS Biko ");
            sb.AppendLine(" FROM Juchu ");
            sb.AppendLine(" INNER JOIN ");
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
            sb.AppendLine(" TORADON_ClmClass ON ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassId = Juchu.ClmClassId AND ");
            sb.AppendLine("   TORADON_ClmClass.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Contract ON ");
            sb.AppendLine("   TORADON_Contract.ContractId = Juchu.ContractId AND ");
            sb.AppendLine("   TORADON_Contract.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" Hanro ON ");
            sb.AppendLine("   Hanro.HanroId = Juchu.HanroId AND ");
            sb.AppendLine("   Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + "");

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
            sb.AppendLine(" SystemName HaishaCarKbn ON ");
            sb.AppendLine("   HaishaCarKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.CarKbn).ToString() + " AND ");
            sb.AppendLine("   HaishaCarKbn.SystemNameCode = HaishaTORADON_Car.CarKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName HaishaTaxDispKbnShort ON ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   HaishaTaxDispKbnShort.SystemNameCode = Haisha.TaxDispKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName HaishaCharterTaxDispKbnShort ON ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   HaishaCharterTaxDispKbnShort.SystemNameCode = Haisha.CharterTaxDispKbn ");

            //抽出条件
            sb.AppendLine("WHERE ");
            sb.AppendLine(" Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("	--引数「日付（範囲開始）」 ");
            sb.AppendLine(" AND Haisha.AddUpYMD >= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDFrom) + " ");
            sb.AppendLine("	--引数「日付（範囲終了）」 ");
            sb.AppendLine(" AND Haisha.AddUpYMD <= " + SQLHelper.DateTimeToDbDate(para.HizukeYMDTo) + " ");

            if (para.SeikyusakiShiteiKbn == HaishaCsvExportSearchParameter.SeikyusakiShiteiKbnItem.Shimebi)
            {
                sb.AppendLine("	--引数「締日」 ");
                sb.AppendLine(" AND TORADON_Tokuisaki.FixDay1 = " + ((int)para.Shimebi).ToString() + " ");
            }
            else
            {
                sb.AppendLine("	--引数「選択した請求先」 ");
                sb.AppendLine(" AND TORADON_Tokuisaki.TokuisakiId IN ("
                        + string.Join(",", para.SeikyusakiIdList.Select(obj => obj.ToString())) + ") ");
            }

            //並び順
            sb.AppendLine("ORDER BY ");
            sb.AppendLine("  ISNULL(TORADON_BranchOffice.BranchOfficeCd, 0)  ");
            sb.AppendLine(" ,ISNULL(HaishaTORADON_Car.CarKbn, 0)  ");
            sb.AppendLine(" ,Haisha.AddUpYMD  ");
            sb.AppendLine(" ,Haisha.CharterAddUpYMD  ");
            sb.AppendLine(" ,ISNULL(TORADON_Tokuisaki.TokuisakiCd, 0)  ");
            sb.AppendLine(" ,ISNULL(TORADON_ClmClass.ClmClassCd, 0)  ");
            sb.AppendLine(" ,ISNULL(TORADON_Contract.ContractCd, 0)  ");
            sb.AppendLine(" ,ISNULL(Hanro.HanroCode, 0)  ");
            sb.AppendLine(" ,Haisha.TaskStartDateTime  ");
            sb.AppendLine(" ,Haisha.TaskEndDateTime  ");
            sb.AppendLine(" ,HaishaStartPoint.PointCd  ");
            sb.AppendLine(" ,HaishaEndPoint.PointCd  ");
            sb.AppendLine(" ,HaishaTORADON_Item.ItemCd  ");
            sb.AppendLine(" ,HaishaTORADON_Car.CarCd  ");
            sb.AppendLine(" ,HaishaTORADON_CarKind.CarKindCd  ");
            sb.AppendLine(" ,HaishaDriver.StaffCd  ");
            sb.AppendLine(" ,HaishaTORADON_Torihikisaki.TorihikiCd  ");

            return sb.ToString();
        }

        #endregion
    }
}
