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
using Jpsys.SagyoManage.BizProperty;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 配送指示メール送信処理のデータアクセスレイヤです。
    /// </summary>
    public class HaisoShijiMailSend
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
        /// 配送指示メール送信処理クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaisoShijiMailSend()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaisoShijiMailSend(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        ///// <summary>
        ///// 配車情報を取得します。
        ///// </summary>
        ///// <returns>配車データ</returns>
        //public List<HaisoShijiMailSendInfo> GetHaisha(HaisoShijiMailSendSearchParameter para = null)
        //{
        //    // クエリ
        //    string query = this.GetQueryHaishaSelect(para);
        //    List<HaisoShijiMailSendInfo> list = SQLHelper.SimpleRead(query, rdr =>
        //    {
        //        //返却用の値
        //        HaisoShijiMailSendInfo ret = new HaisoShijiMailSendInfo();

        //        //配車情報
        //        HaisoShijiMailSendInfo retHaisha = new HaisoShijiMailSendInfo();
        //        retHaisha.HaishaId = SQLServerUtil.dbDecimal(rdr["HaishaId"]);
        //        retHaisha.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
        //        retHaisha.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
        //        retHaisha.LicPlateCarNo = rdr["LicPlateCarNo"].ToString();
        //        retHaisha.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
        //        retHaisha.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
        //        retHaisha.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
        //        retHaisha.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
        //        retHaisha.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
        //        retHaisha.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
        //        retHaisha.Price = SQLServerUtil.dbDecimal(rdr["Price"]);
        //        retHaisha.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
        //        retHaisha.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
        //        retHaisha.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]);
        //        retHaisha.PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]);
        //        retHaisha.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]);
        //        retHaisha.PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]);
        //        retHaisha.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]);
        //        retHaisha.TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]);
        //        retHaisha.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
        //        retHaisha.CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]);
        //        retHaisha.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
        //        retHaisha.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
        //        retHaisha.CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]);
        //        retHaisha.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]);
        //        retHaisha.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]);
        //        retHaisha.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]);
        //        retHaisha.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]);
        //        retHaisha.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]);
        //        retHaisha.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]);
        //        retHaisha.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
        //        retHaisha.Fee = SQLServerUtil.dbDecimal(rdr["Fee"]);
        //        retHaisha.PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]);
        //        retHaisha.TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]);
        //        retHaisha.FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]);
        //        retHaisha.FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]);
        //        retHaisha.FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]);
        //        retHaisha.FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]);
        //        retHaisha.FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]);
        //        retHaisha.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
        //        retHaisha.StartPointName = rdr["StartPointNM"].ToString();
        //        retHaisha.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
        //        retHaisha.EndPointName = rdr["EndPointNM"].ToString();
        //        retHaisha.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
        //        retHaisha.ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]);
        //        retHaisha.ItemName = rdr["ItemNM"].ToString();
        //        retHaisha.OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]);
        //        retHaisha.OwnerCode = SQLServerUtil.dbInt(rdr["OwnerCd"]);
        //        retHaisha.OwnerName = rdr["OwnerNM"].ToString();
        //        retHaisha.StartYMD = SQLHelper.dbDate(rdr["StartYMD"]);
        //        retHaisha.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
        //        retHaisha.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
        //        retHaisha.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
        //        retHaisha.Biko = rdr["Biko"].ToString();

        //        retHaisha.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
        //        retHaisha.LicPlateDeptName = rdr["LicPlateDeptNM"].ToString();
        //        retHaisha.LicPlateCarKindKbn = rdr["LicPlateCarKindKbn"].ToString();
        //        retHaisha.LicPlateUsageKbn = rdr["LicPlateUsageKbn"].ToString();
        //        retHaisha.LicPlateCarNo = rdr["LicPlateCarNo"].ToString();
        //        retHaisha.StaffName = rdr["StaffNM"].ToString();
        //        retHaisha.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
        //        retHaisha.FigName = rdr["FigNM"].ToString();
        //        retHaisha.HanroName = rdr["HanroName"].ToString();
        //        retHaisha.OfukuKbn = SQLServerUtil.dbInt(rdr["OfukuKbn"]);
        //        retHaisha.StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]);
        //        retHaisha.EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]);
        //        retHaisha.StartPointAddress1 = rdr["StartPointAddress1"].ToString();
        //        retHaisha.StartPointAddress2 = rdr["StartPointAddress2"].ToString();
        //        retHaisha.EndPointAddress1 = rdr["EndPointAddress1"].ToString();
        //        retHaisha.EndPointAddress2 = rdr["EndPointAddress2"].ToString();
        //        retHaisha.CarKindName = rdr["CarKindNM"].ToString();
        //        retHaisha.Email = rdr["Email"].ToString();

        //        // 受注情報
        //        retHaisha.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]);
        //        retHaisha.BranchOfficeCode = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]);
        //        retHaisha.BranchOfficeShortName = rdr["BranchOfficeSNM"].ToString();
        //        retHaisha.TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]);
        //        retHaisha.TokuisakiName = rdr["TokuisakiNM"].ToString();

        //        //返却用の値を返します
        //        return retHaisha;
        //    });

        //    return list;
        //}

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 受注情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">受注条件情報</param>
        /// <returns></returns>
        private string GetQueryJuchuSelect()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   BranchOffice.BranchOfficeCd ");
            sb.AppendLine("   ,BranchOffice.BranchOfficeSNM ");
            sb.AppendLine("   ,Tokuisaki.TokuisakiCd ");
            sb.AppendLine("   ,StartPoint.PointCd As StartPointCd ");
            sb.AppendLine("   ,EndPoint.PointCd As EndPointCd ");
            sb.AppendLine("   ,Item.ItemCd ");
            sb.AppendLine("   ,Fig.FigCd ");
            sb.AppendLine("   ,Fig.FigNM ");
            sb.AppendLine("   ,Hanro.HanroName ");
            sb.AppendLine("   ,Hanro.KoteiNissu ");
            sb.AppendLine("   ,Hanro.KoteiJikan ");
            sb.AppendLine("   ,Staff.StaffCd ");
            sb.AppendLine("   ,Staff.StaffNM ");
            sb.AppendLine("   ,Car.CarKbn ");
            sb.AppendLine("   ,Torihikisaki.TorihikiCd ");
            sb.AppendLine("   ,Torihikisaki.TorihikiNM ");
            sb.AppendLine("   ,Owner.OwnerCd ");
            sb.AppendLine("   ,ClmClass.ClmClassCd ");
            sb.AppendLine("   ,ClmClass.ClmClassNM ");
            sb.AppendLine("   ,Contract.ContractCd ");
            sb.AppendLine("   ,Contract.ContractNM ");
            sb.AppendLine("   ,JuchuTantoStaff.StaffCd AS JuchuTantoCd ");
            sb.AppendLine("   ,JuchuTantoStaff.StaffNM AS JuchuTantoNM ");
            sb.AppendLine("   ,Juchu.JuchuId ");
            sb.AppendLine("   ,Juchu.HanroId ");
            sb.AppendLine("   ,Juchu.OfukuKbn ");
            sb.AppendLine("   ,Juchu.ItemNM ");
            sb.AppendLine("   ,Juchu.BranchOfficeId ");
            sb.AppendLine("   ,Juchu.TokuisakiId ");
            sb.AppendLine("   ,Juchu.TokuisakiNM ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   Juchu  ");
            sb.AppendLine("   LEFT JOIN TORADON_BranchOffice BranchOffice ");
            sb.AppendLine("     ON Juchu.BranchOfficeId = BranchOffice.BranchOfficeId ");
            sb.AppendLine("     AND BranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Tokuisaki Tokuisaki ");
            sb.AppendLine("     ON Juchu.TokuisakiId = Tokuisaki.TokuisakiId ");
            sb.AppendLine("     AND Tokuisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Point StartPoint ");
            sb.AppendLine("     ON Juchu.StartPointId = StartPoint.PointId ");
            sb.AppendLine("     AND StartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Point EndPoint ");
            sb.AppendLine("     ON Juchu.EndPointId = EndPoint.PointId ");
            sb.AppendLine("     AND EndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Item Item ");
            sb.AppendLine("     ON Juchu.ItemId = Item.ItemId ");
            sb.AppendLine("     AND Item.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Car Car ");
            sb.AppendLine("     ON Juchu.CarId = Car.CarId ");
            sb.AppendLine("     AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Torihikisaki Torihikisaki");
            sb.AppendLine("     ON Juchu.CarOfChartererId = Torihikisaki.TorihikiId ");
            sb.AppendLine("     AND Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Owner Owner");
            sb.AppendLine("     ON Juchu.OwnerId = Owner.OwnerId ");
            sb.AppendLine("     AND Owner.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_ClmClass ClmClass");
            sb.AppendLine("     ON Juchu.ClmClassId = ClmClass.ClmClassId ");
            sb.AppendLine("     AND ClmClass.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Contract Contract");
            sb.AppendLine("     ON Juchu.ContractId = Contract.ContractId ");
            sb.AppendLine("     AND Contract.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff JuchuTantoStaff");
            sb.AppendLine("     ON Juchu.JuchuTantoId = JuchuTantoStaff.StaffId ");
            sb.AppendLine("     AND JuchuTantoStaff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Fig Fig ");
            sb.AppendLine("     ON Juchu.FigId = Fig.FigId ");
            sb.AppendLine("     AND Fig.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Hanro ");
            sb.AppendLine("     ON Juchu.HanroId = Hanro.HanroId ");
            sb.AppendLine("     AND Hanro.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff Staff ");
            sb.AppendLine("     ON Juchu.DriverId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Point ");
            sb.AppendLine("     ON Juchu.EndPointId = Point.ToraDONPointId ");
            sb.AppendLine("     AND Point.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Homen ");
            sb.AppendLine("     ON Point.HomenId = Homen.HomenId ");
            sb.AppendLine("     AND Homen.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        ///// <summary>
        ///// 配車情報を取得するSqlQueryを返す。
        ///// </summary>
        ///// <param name="para">検索条件情報</param>
        ///// <returns></returns>
        //private string GetQueryHaishaSelect(HaisoShijiMailSendSearchParameter para)
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.AppendLine(" SELECT ");

        //    // 配車情報
        //    sb.AppendLine("   Haisha.HaishaId ");
        //    sb.AppendLine("   , Haisha.JuchuId ");
        //    sb.AppendLine("   , Haisha.CarId ");
        //    sb.AppendLine("   , Haisha.DriverId ");
        //    sb.AppendLine("   , Haisha.Number ");
        //    sb.AppendLine("   , Haisha.FigId ");
        //    sb.AppendLine("   , Haisha.AtPrice ");
        //    sb.AppendLine("   , Haisha.Weight ");
        //    sb.AppendLine("   , Haisha.Price ");
        //    sb.AppendLine("   , Haisha.PriceInPrice ");
        //    sb.AppendLine("   , Haisha.TollFeeInPrice ");
        //    sb.AppendLine("   , Haisha.PriceOutTaxCalc ");
        //    sb.AppendLine("   , Haisha.PriceOutTax ");
        //    sb.AppendLine("   , Haisha.PriceInTaxCalc ");
        //    sb.AppendLine("   , Haisha.PriceInTax ");
        //    sb.AppendLine("   , Haisha.PriceNoTaxCalc ");
        //    sb.AppendLine("   , Haisha.TaxDispKbn ");
        //    sb.AppendLine("   , Haisha.AddUpYMD ");
        //    sb.AppendLine("   , Haisha.CharterPrice ");
        //    sb.AppendLine("   , Haisha.PriceInCharterPrice ");
        //    sb.AppendLine("   , Haisha.TollFeeInCharterPrice ");
        //    sb.AppendLine("   , Haisha.CarOfChartererId ");
        //    sb.AppendLine("   , Haisha.CharterPriceOutTaxCalc ");
        //    sb.AppendLine("   , Haisha.CharterPriceOutTax ");
        //    sb.AppendLine("   , Haisha.CharterPriceInTaxCalc ");
        //    sb.AppendLine("   , Haisha.CharterPriceInTax ");
        //    sb.AppendLine("   , Haisha.CharterPriceNoTaxCalc ");
        //    sb.AppendLine("   , Haisha.CharterTaxDispKbn ");
        //    sb.AppendLine("   , Haisha.CharterAddUpYMD ");
        //    sb.AppendLine("   , Haisha.Fee ");
        //    sb.AppendLine("   , Haisha.PriceInFee ");
        //    sb.AppendLine("   , Haisha.TollFeeInFee ");
        //    sb.AppendLine("   , Haisha.FeeOutTaxCalc ");
        //    sb.AppendLine("   , Haisha.FeeOutTax ");
        //    sb.AppendLine("   , Haisha.FeeInTaxCalc ");
        //    sb.AppendLine("   , Haisha.FeeInTax ");
        //    sb.AppendLine("   , Haisha.FeeNoTaxCalc ");
        //    sb.AppendLine("   , Haisha.StartPointId ");
        //    sb.AppendLine("   , Haisha.StartPointNM ");
        //    sb.AppendLine("   , Haisha.EndPointId ");
        //    sb.AppendLine("   , Haisha.EndPointNM ");
        //    sb.AppendLine("   , Haisha.ItemId ");
        //    sb.AppendLine("   , Haisha.ItemNM ");
        //    sb.AppendLine("   , Haisha.OwnerId ");
        //    sb.AppendLine("   , Haisha.OwnerNM ");
        //    sb.AppendLine("   , Haisha.StartYMD ");
        //    sb.AppendLine("   , Haisha.ReuseYMD ");
        //    sb.AppendLine("   , Haisha.TaskStartDateTime ");
        //    sb.AppendLine("   , Haisha.TaskEndDateTime ");
        //    sb.AppendLine("   , Haisha.Biko ");
        //    sb.AppendLine("   , Car.CarCd ");
        //    sb.AppendLine("   , Car.LicPlateDeptNM ");
        //    sb.AppendLine("   , Car.LicPlateCarKindKbn ");
        //    sb.AppendLine("   , Car.LicPlateUsageKbn ");
        //    sb.AppendLine("   , Car.LicPlateCarNo ");
        //    sb.AppendLine("   , CarKind.CarKindId ");
        //    sb.AppendLine("   , Staff.StaffNM ");
        //    sb.AppendLine("   , Item.ItemCd ");
        //    sb.AppendLine("   , Owner.OwnerCd ");
        //    sb.AppendLine("   , Fig.FigCd ");
        //    sb.AppendLine("   , Fig.FigNM ");
        //    sb.AppendLine("   , StartPoint.PointCd As StartPointCd ");
        //    sb.AppendLine("   , StartPoint.Address1 As StartPointAddress1 ");
        //    sb.AppendLine("   , StartPoint.Address2 As StartPointAddress2 ");
        //    sb.AppendLine("   , EndPoint.PointCd As EndPointCd ");
        //    sb.AppendLine("   , EndPoint.Address1 As EndPointAddress1 ");
        //    sb.AppendLine("   , EndPoint.Address2 As EndPointAddress2 ");
        //    sb.AppendLine("   , CarKind.CarKindNM ");
        //    sb.AppendLine("   , HaisoShijiMailSendToList.Email ");

        //    // 受注情報
        //    sb.AppendLine("   , JuchuInfo.HanroName ");
        //    sb.AppendLine("   , JuchuInfo.OfukuKbn ");
        //    sb.AppendLine("   , JuchuInfo.BranchOfficeId ");
        //    sb.AppendLine("   , JuchuInfo.BranchOfficeCd ");
        //    sb.AppendLine("   , JuchuInfo.BranchOfficeSNM ");
        //    sb.AppendLine("   , JuchuInfo.TokuisakiId ");
        //    sb.AppendLine("   , JuchuInfo.TokuisakiNM ");

        //    sb.AppendLine(" FROM ");
        //    sb.AppendLine("   Haisha  ");
        //    sb.AppendLine("   INNER JOIN (  ");
        //    sb.AppendLine(GetQueryJuchuSelect()); // 受注情報
        //    sb.AppendLine("   ) JuchuInfo ");
        //    sb.AppendLine("     ON Haisha.JuchuId = JuchuInfo.JuchuId ");
        //    sb.AppendLine("   INNER JOIN TORADON_Car Car ");
        //    sb.AppendLine("     ON Haisha.CarId = Car.CarId ");
        //    sb.AppendLine("     AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("     AND Car.DisableFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_CarKind CarKind ");
        //    sb.AppendLine("     ON  Car.CarKindId = CarKind.CarKindId ");
        //    sb.AppendLine("     AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Staff Staff ");
        //    sb.AppendLine("     ON Haisha.DriverId = Staff.StaffId ");
        //    sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Item Item ");
        //    sb.AppendLine("     ON Haisha.ItemId = Item.ItemId ");
        //    sb.AppendLine("     AND Item.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Owner Owner ");
        //    sb.AppendLine("     ON Haisha.OwnerId = Owner.OwnerId ");
        //    sb.AppendLine("     AND Owner.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Fig Fig ");
        //    sb.AppendLine("     ON Haisha.FigId = Fig.FigId ");
        //    sb.AppendLine("     AND Fig.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Point StartPoint ");
        //    sb.AppendLine("     ON Haisha.StartPointId = StartPoint.PointId ");
        //    sb.AppendLine("     AND StartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN TORADON_Point EndPoint ");
        //    sb.AppendLine("     ON Haisha.EndPointId = EndPoint.PointId ");
        //    sb.AppendLine("     AND EndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   LEFT JOIN HaisoShijiMailSendToList ");
        //    sb.AppendLine("     ON Haisha.DriverId = HaisoShijiMailSendToList.ToraDONStaffId ");
        //    sb.AppendLine("     AND HaisoShijiMailSendToList.DelFlag = " + NSKUtil.BoolToInt(false) + "");

        //    sb.AppendLine(" WHERE ");
        //    sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
        //    sb.AppendLine("   AND Car.CarKbn <> " + (int)DefaultProperty.CarKbn.Yosha + "");
        //    sb.AppendLine("   AND 0 < LEN(TRIM(ISNULL(HaisoShijiMailSendToList.Email,''))) ");

        //    if (para != null)
        //    {
        //        sb.AppendLine("       AND Haisha.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDFrom));
        //        sb.AppendLine("       AND Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDTo));

        //        //日付は積日のみ見るように修正 その他の日付を条件に追加する場合は下記を復活する予定 START
        //        //// 出発日時と到着日時（指定期間に該当するデータはすべて抽出する。）
        //        //sb.AppendLine("   AND (  ");

        //        //// 出発日が指定期間に該当する場合
        //        //sb.AppendLine("     (  ");
        //        //sb.AppendLine("       Haisha.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDFrom));
        //        //sb.AppendLine("       AND Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDTo));
        //        //sb.AppendLine("     )  ");

        //        //// 到着日が指定期間に該当する場合
        //        //sb.AppendLine("     OR (  ");
        //        //sb.AppendLine("       Haisha.TaskEndDateTime > " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDFrom));
        //        //sb.AppendLine("       AND Haisha.TaskEndDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDTo));
        //        //sb.AppendLine("     ) ");

        //        //// 指定期間を配送期間がまたぐ場合
        //        //sb.AppendLine("     OR (  ");
        //        //sb.AppendLine("       Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDFrom));
        //        //sb.AppendLine("       AND Haisha.TaskEndDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDTo));
        //        //sb.AppendLine("     ) ");

        //        //// 積載日付が指定期間に該当する場合
        //        //sb.AppendLine("     OR (  ");
        //        //sb.AppendLine("       Haisha.StartYMD >= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDFrom));
        //        //sb.AppendLine("       AND Haisha.StartYMD <= " + SQLHelper.DateTimeToDbDateTime(para.HizukeYMDTo));
        //        //sb.AppendLine("     ) ");

        //        //sb.AppendLine("   ) ");
        //        //日付は積日のみ見るように修正 その他の日付を条件に追加する場合は下記を復活する予定 END

        //        // 乗務員（個別指定）
        //        sb.AppendLine("   AND Staff.StaffId IN ( " + para.StaffCheckList + ") ");
        //    }

        //    sb.AppendLine(" ORDER BY ");
        //    sb.AppendLine("   Car.CarCd ");
        //    sb.AppendLine("   ,Staff.StaffCd ");
        //    sb.AppendLine("   ,Haisha.StartYMD ");
        //    sb.AppendLine("   ,Haisha.TaskEndDateTime ");

        //    return sb.ToString();
        //}

        #endregion
    }
}
