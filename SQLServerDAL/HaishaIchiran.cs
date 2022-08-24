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
using Jpsys.HaishaManageV10.ComLib;
using Jpsys.HaishaManageV10.Model.DALExceptions;

namespace Jpsys.HaishaManageV10.SQLServerDAL
{
    /// <summary>
    /// 配車一覧のデータアクセスレイヤです。
    /// </summary>
    public class HaishaIchiran
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
        /// 一括登録件数
        /// </summary>
        private const int EXECUTE_COUNT = 1000;

        /// <summary>
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public HaishaIchiran()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaishaIchiran(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 配車情報を取得します。
        /// </summary>
        /// <returns>配車データ</returns>
        public List<HaishaSearchResultInfo> GetHaisha(HaishaIchiranSearchParameter para = null)
        {
            // クエリ
            string query = this.GetQueryHaishaSelect(para);
            List<HaishaSearchResultInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaishaSearchResultInfo ret = new HaishaSearchResultInfo();

                //配車情報
                HaishaNyuryokuInfo retHaisha = new HaishaNyuryokuInfo();
                retHaisha.HaishaId = SQLServerUtil.dbDecimal(rdr["HaishaId"]);
                retHaisha.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                retHaisha.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                retHaisha.LicPlateCarNo = rdr["LicPlateCarNo"].ToString();
                retHaisha.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                retHaisha.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
                retHaisha.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                retHaisha.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                retHaisha.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                retHaisha.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                retHaisha.Price = SQLServerUtil.dbDecimal(rdr["Price"]);
                retHaisha.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                retHaisha.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                retHaisha.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]);
                retHaisha.PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]);
                retHaisha.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]);
                retHaisha.PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]);
                retHaisha.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]);
                retHaisha.TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]);
                retHaisha.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                retHaisha.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                retHaisha.CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]);
                retHaisha.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                retHaisha.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                retHaisha.CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]);
                retHaisha.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]);
                retHaisha.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]);
                retHaisha.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]);
                retHaisha.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]);
                retHaisha.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]);
                retHaisha.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]);
                retHaisha.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                retHaisha.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                retHaisha.Fee = SQLServerUtil.dbDecimal(rdr["Fee"]);
                retHaisha.PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]);
                retHaisha.TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]);
                retHaisha.FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]);
                retHaisha.FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]);
                retHaisha.FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]);
                retHaisha.FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]);
                retHaisha.FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]);
                retHaisha.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
                retHaisha.StartPointName = rdr["StartPointNM"].ToString();
                retHaisha.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
                retHaisha.EndPointName = rdr["EndPointNM"].ToString();

                retHaisha.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
                retHaisha.ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]);
                retHaisha.ItemName = rdr["ItemNM"].ToString();
                retHaisha.OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]);
                retHaisha.OwnerCode = SQLServerUtil.dbInt(rdr["OwnerCd"]);
                retHaisha.OwnerName = rdr["OwnerNM"].ToString();

                retHaisha.StartYMD = SQLHelper.dbDate(rdr["StartYMD"]);
                retHaisha.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                retHaisha.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                //retHaisha.HaisouEndYMD = SQLServerUtil.dbDecimal(rdr["HaisouEndYMD"]);
                //retHaisha.HaisouCancelYMD = SQLServerUtil.dbDecimal(rdr["HaisouCancelYMD"]);
                //retHaisha.HaisouCancelRiyu = rdr["HaisouCancelRiyu"].ToString();
                retHaisha.Biko = rdr["Biko"].ToString();
                retHaisha.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["Juchu_BranchOfficeId"]);
                retHaisha.TokuisakiId = SQLServerUtil.dbDecimal(rdr["Juchu_TokuisakiId"]);
                retHaisha.TokuisakiName = rdr["Juchu_TokuisakiNM"].ToString();
                retHaisha.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
                retHaisha.CarLicPlateCarNo = rdr["CarLicPlateCarNo"].ToString();
                retHaisha.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                retHaisha.CarKindName = rdr["CarKindNM"].ToString();
                retHaisha.StaffCd = SQLServerUtil.dbInt(rdr["StaffCd"]);
                retHaisha.StaffName = rdr["StaffNM"].ToString();
                retHaisha.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
                retHaisha.FigName = rdr["FigNM"].ToString();
                retHaisha.HanroName = rdr["Juchu_HanroName"].ToString();
                retHaisha.OfukuKbn = SQLServerUtil.dbInt(rdr["Juchu_OfukuKbn"]);
                retHaisha.StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                retHaisha.EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                retHaisha.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                retHaisha.TorihikiCd = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                retHaisha.TorihikiName = rdr["TorihikiNM"].ToString();
                retHaisha.TorihikiShortName = rdr["TorihikiSNM"].ToString();

                retHaisha.TaikijikanNumber = SQLServerUtil.dbDecimal(rdr["TaikijikanNumber"]);
                retHaisha.TaikijikanFigId = SQLServerUtil.dbDecimal(rdr["TaikijikanFigId"]);
                retHaisha.TaikijikanAtPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanAtPrice"]);
                retHaisha.TaikijikanryoInPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInPrice"]);
                retHaisha.NizumiryoInPrice = SQLServerUtil.dbDecimal(rdr["NizumiryoInPrice"]);
                retHaisha.NioroshiryoInPrice = SQLServerUtil.dbDecimal(rdr["NioroshiryoInPrice"]);
                retHaisha.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                retHaisha.TaikijikanryoInFee = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInFee"]);
                retHaisha.NizumiryoInFee = SQLServerUtil.dbDecimal(rdr["NizumiryoInFee"]);
                retHaisha.NioroshiryoInFee = SQLServerUtil.dbDecimal(rdr["NioroshiryoInFee"]);
                retHaisha.FutaigyomuryoInFee = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInFee"]);
                retHaisha.NizumijikanNumber = SQLServerUtil.dbDecimal(rdr["NizumijikanNumber"]);
                retHaisha.NizumijikanFigId = SQLServerUtil.dbDecimal(rdr["NizumijikanFigId"]);
                retHaisha.NizumijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NizumijikanAtPrice"]);
                retHaisha.NioroshijikanNumber = SQLServerUtil.dbDecimal(rdr["NioroshijikanNumber"]);
                retHaisha.NioroshijikanFigId = SQLServerUtil.dbDecimal(rdr["NioroshijikanFigId"]);
                retHaisha.NioroshijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NioroshijikanAtPrice"]);
                retHaisha.FutaigyomujikanNumber = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanNumber"]);
                retHaisha.FutaigyomujikanFigId = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanFigId"]);
                retHaisha.FutaigyomujikanAtPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanAtPrice"]);
                retHaisha.JomuinUriageDogakuFlag = SQLHelper.dbBit(rdr["JomuinUriageDogakuFlag"]);
                retHaisha.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);

                retHaisha.JuchuTaskStartDateTime = SQLHelper.dbDate(rdr["Juchu_TaskStartDateTime"]);
                retHaisha.JuchuTaskEndDateTime = SQLHelper.dbDate(rdr["Juchu_TaskEndDateTime"]);

                retHaisha.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                retHaisha.MagoYoshasaki = rdr["MagoYoshasaki"].ToString();
                retHaisha.JuchuReuseYMD = SQLHelper.dbDate(rdr["Juchu_ReuseYMD"]);

                // 受注情報
                JuchuInfo retJuchu = new JuchuInfo();
                retJuchu.JuchuId = SQLServerUtil.dbDecimal(rdr["Juchu_JuchuId"]);
                retJuchu.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["Juchu_JuchuSlipNo"]);
                retJuchu.CarId = SQLServerUtil.dbDecimal(rdr["Juchu_CarId"]);
                retJuchu.LicPlateCarNo = rdr["Juchu_LicPlateCarNo"].ToString();
                retJuchu.CarKindId = SQLServerUtil.dbDecimal(rdr["Juchu_CarKindId"]);
                retJuchu.CarKindCode = SQLServerUtil.dbInt(rdr["Juchu_CarKindCd"]);
                retJuchu.CarKindName = rdr["Juchu_CarKindNM"].ToString();
                retJuchu.DriverId = SQLServerUtil.dbDecimal(rdr["Juchu_DriverId"]);
                retJuchu.Number = SQLServerUtil.dbDecimal(rdr["Juchu_Number"]);
                retJuchu.FigId = SQLServerUtil.dbDecimal(rdr["Juchu_FigId"]);
                retJuchu.AtPrice = SQLServerUtil.dbDecimal(rdr["Juchu_AtPrice"]);
                retJuchu.Weight = SQLServerUtil.dbDecimal(rdr["Juchu_Weight"]);
                retJuchu.Price = SQLServerUtil.dbDecimal(rdr["Juchu_Price"]);
                retJuchu.PriceInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_PriceInPrice"]);
                retJuchu.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_TollFeeInPrice"]);
                retJuchu.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_PriceOutTaxCalc"]);
                retJuchu.PriceOutTax = SQLServerUtil.dbDecimal(rdr["Juchu_PriceOutTax"]);
                retJuchu.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_PriceInTaxCalc"]);
                retJuchu.PriceInTax = SQLServerUtil.dbDecimal(rdr["Juchu_PriceInTax"]);
                retJuchu.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_PriceNoTaxCalc"]);
                retJuchu.TaxDispKbn = SQLServerUtil.dbInt(rdr["Juchu_TaxDispKbn"]);
                retJuchu.AddUpYMD = SQLHelper.dbDate(rdr["Juchu_AddUpYMD"]);
                retJuchu.FixFlag = SQLHelper.dbBit(rdr["Juchu_FixFlag"]);
                retJuchu.CharterPrice = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPrice"]);
                retJuchu.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["Juchu_PriceInCharterPrice"]);
                retJuchu.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["Juchu_TollFeeInCharterPrice"]);
                retJuchu.CarOfChartererId = SQLServerUtil.dbDecimal(rdr["Juchu_CarOfChartererId"]);
                retJuchu.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPriceOutTaxCalc"]);
                retJuchu.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPriceOutTax"]);
                retJuchu.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPriceInTaxCalc"]);
                retJuchu.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPriceInTax"]);
                retJuchu.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_CharterPriceNoTaxCalc"]);
                retJuchu.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["Juchu_CharterTaxDispKbn"]);
                retJuchu.CharterAddUpYMD = SQLHelper.dbDate(rdr["Juchu_CharterAddUpYMD"]);
                retJuchu.CharterFixFlag = SQLHelper.dbBit(rdr["Juchu_CharterFixFlag"]);
                retJuchu.Fee = SQLServerUtil.dbDecimal(rdr["Juchu_Fee"]);
                retJuchu.PriceInFee = SQLServerUtil.dbDecimal(rdr["Juchu_PriceInFee"]);
                retJuchu.TollFeeInFee = SQLServerUtil.dbDecimal(rdr["Juchu_TollFeeInFee"]);
                retJuchu.FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_FeeOutTaxCalc"]);
                retJuchu.FeeOutTax = SQLServerUtil.dbDecimal(rdr["Juchu_FeeOutTax"]);
                retJuchu.FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_FeeInTaxCalc"]);
                retJuchu.FeeInTax = SQLServerUtil.dbDecimal(rdr["Juchu_FeeInTax"]);
                retJuchu.FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["Juchu_FeeNoTaxCalc"]);
                retJuchu.StartPointId = SQLServerUtil.dbDecimal(rdr["Juchu_StartPointId"]);
                retJuchu.StartPointName = rdr["Juchu_StartPointNM"].ToString();
                retJuchu.EndPointId = SQLServerUtil.dbDecimal(rdr["Juchu_EndPointId"]);
                retJuchu.EndPointName = rdr["Juchu_EndPointNM"].ToString();
                retJuchu.ItemId = SQLServerUtil.dbDecimal(rdr["Juchu_ItemId"]);
                retJuchu.OwnerId = SQLServerUtil.dbDecimal(rdr["Juchu_OwnerId"]);
                retJuchu.OwnerCode = SQLServerUtil.dbInt(rdr["Juchu_OwnerCd"]);
                retJuchu.OwnerName = rdr["Juchu_OwnerNM"].ToString();
                retJuchu.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["Juchu_BranchOfficeId"]);
                retJuchu.BranchOfficeCode = SQLServerUtil.dbInt(rdr["Juchu_BranchOfficeCd"]);
                retJuchu.BranchOfficeShortName = rdr["Juchu_BranchOfficeSNM"].ToString();
                retJuchu.TokuisakiCode = SQLServerUtil.dbInt(rdr["Juchu_TokuisakiCd"]);
                retJuchu.TokuisakiName = rdr["Juchu_TokuisakiNM"].ToString();
                retJuchu.StartPointCode = SQLServerUtil.dbInt(rdr["Juchu_StartPointCd"]);
                retJuchu.EndPointCode = SQLServerUtil.dbInt(rdr["Juchu_EndPointCd"]);
                retJuchu.ItemCode = SQLServerUtil.dbInt(rdr["Juchu_ItemCd"]);
                retJuchu.ItemName = rdr["Juchu_ItemNM"].ToString();
                retJuchu.Weight = SQLServerUtil.dbDecimal(rdr["Juchu_Weight"]);
                retJuchu.FigCode = SQLServerUtil.dbInt(rdr["Juchu_FigCd"]);
                retJuchu.FigName = rdr["Juchu_FigNM"].ToString();
                retJuchu.HanroName = rdr["Juchu_HanroName"].ToString();
                retJuchu.StaffCd = SQLServerUtil.dbInt(rdr["Juchu_StaffCd"]);
                retJuchu.StaffName = rdr["Juchu_StaffNM"].ToString();
                retJuchu.OfukuKbn = SQLServerUtil.dbInt(rdr["Juchu_OfukuKbn"]);
                retJuchu.OfukuKbnSortOrder = SQLServerUtil.dbDecimal(rdr["Juchu_OfukuKbnSortOrder"]);
                retJuchu.KoteiNissu = SQLServerUtil.dbInt(rdr["Juchu_KoteiNissu"]);
                retJuchu.KoteiJikan = SQLServerUtil.dbInt(rdr["Juchu_KoteiJikan"]);
                retJuchu.CarKbn = SQLServerUtil.dbInt(rdr["Juchu_CarKbn"]);

                retJuchu.TaskStartDateTime = SQLHelper.dbDate(rdr["Juchu_TaskStartDateTime"]);
                retJuchu.TaskEndDateTime = SQLHelper.dbDate(rdr["Juchu_TaskEndDateTime"]);

                retJuchu.TorihikiCd = SQLServerUtil.dbInt(rdr["Juchu_TorihikiCd"]);
                retJuchu.TorihikiName = rdr["Juchu_TorihikiNM"].ToString();
                retJuchu.TorihikiShortName = rdr["Juchu_TorihikiSNM"].ToString();

                retJuchu.ClmClassId = SQLServerUtil.dbDecimal(rdr["Juchu_ClmClassId"]);
                retJuchu.ClmClassCode = SQLServerUtil.dbInt(rdr["Juchu_ClmClassCd"]);
                retJuchu.ClmClassName = rdr["Juchu_ClmClassNM"].ToString();
                retJuchu.ContractId = SQLServerUtil.dbDecimal(rdr["Juchu_ContractId"]);
                retJuchu.ContractCode = SQLServerUtil.dbInt(rdr["Juchu_ContractCd"]);
                retJuchu.ContractName = rdr["Juchu_ContractNM"].ToString();
                retJuchu.JuchuTantoId = SQLServerUtil.dbDecimal(rdr["Juchu_JuchuTantoId"]);
                retJuchu.JuchuTantoCode = SQLServerUtil.dbInt(rdr["Juchu_JuchuTantoCd"]);
                retJuchu.JuchuTantoName = rdr["Juchu_JuchuTantoNM"].ToString();

                retJuchu.TaikijikanNumber = SQLServerUtil.dbDecimal(rdr["Juchu_TaikijikanNumber"]);
                retJuchu.TaikijikanFigId = SQLServerUtil.dbDecimal(rdr["Juchu_TaikijikanFigId"]);
                retJuchu.TaikijikanAtPrice = SQLServerUtil.dbDecimal(rdr["Juchu_TaikijikanAtPrice"]);
                retJuchu.TaikijikanryoInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_TaikijikanryoInPrice"]);
                retJuchu.NizumiryoInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_NizumiryoInPrice"]);
                retJuchu.NioroshiryoInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_NioroshiryoInPrice"]);
                retJuchu.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["Juchu_FutaigyomuryoInPrice"]);
                retJuchu.TaikijikanryoInFee = SQLServerUtil.dbDecimal(rdr["Juchu_TaikijikanryoInFee"]);
                retJuchu.NizumiryoInFee = SQLServerUtil.dbDecimal(rdr["Juchu_NizumiryoInFee"]);
                retJuchu.NioroshiryoInFee = SQLServerUtil.dbDecimal(rdr["Juchu_NioroshiryoInFee"]);
                retJuchu.FutaigyomuryoInFee = SQLServerUtil.dbDecimal(rdr["Juchu_FutaigyomuryoInFee"]);
                retJuchu.NizumijikanNumber = SQLServerUtil.dbDecimal(rdr["Juchu_NizumijikanNumber"]);
                retJuchu.NizumijikanFigId = SQLServerUtil.dbDecimal(rdr["Juchu_NizumijikanFigId"]);
                retJuchu.NizumijikanAtPrice = SQLServerUtil.dbDecimal(rdr["Juchu_NizumijikanAtPrice"]);
                retJuchu.NioroshijikanNumber = SQLServerUtil.dbDecimal(rdr["Juchu_NioroshijikanNumber"]);
                retJuchu.NioroshijikanFigId = SQLServerUtil.dbDecimal(rdr["Juchu_NioroshijikanFigId"]);
                retJuchu.NioroshijikanAtPrice = SQLServerUtil.dbDecimal(rdr["Juchu_NioroshijikanAtPrice"]);
                retJuchu.FutaigyomujikanNumber = SQLServerUtil.dbDecimal(rdr["Juchu_FutaigyomujikanNumber"]);
                retJuchu.FutaigyomujikanFigId = SQLServerUtil.dbDecimal(rdr["Juchu_FutaigyomujikanFigId"]);
                retJuchu.FutaigyomujikanAtPrice = SQLServerUtil.dbDecimal(rdr["Juchu_FutaigyomujikanAtPrice"]);
                retJuchu.JomuinUriageDogakuFlag = SQLHelper.dbBit(rdr["Juchu_JomuinUriageDogakuFlag"]);
                retJuchu.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["Juchu_JomuinUriageKingaku"]);

                retJuchu.ReuseYMD = SQLHelper.dbDate(rdr["Juchu_ReuseYMD"]);
                retJuchu.MagoYoshasaki = rdr["Juchu_MagoYoshasaki"].ToString();

                // // キー検索の場合
                if (para != null && para.KeyFlg)
                {
                    retJuchu.DelTargetFlg = true;
                }
                else
                {
                    retJuchu.DelTargetFlg = false;
                }

                //入力者以下の常設フィールドをセットする
                retHaisha = SQLHelper.GetTimeStampModelBaseValues(retHaisha, rdr);

                ret.HaishaInfo = retHaisha;
                ret.JuchuInfo = retJuchu;

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        /// 車両一覧を取得します。
        /// </summary>
        /// <returns>車両データ</returns>
        public List<HaishaIchiranCarInfo> GetCar(HaishaIchiranSearchParameter para = null)
        {
            // クエリ
            string query = this.GetQueryCarSelect(para);
            List<HaishaIchiranCarInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaishaIchiranCarInfo ret = new HaishaIchiranCarInfo();
                ret.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                ret.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]); ;
                ret.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                ret.CarKindCd = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                ret.CarKindName = rdr["CarKindNM"].ToString();
                ret.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
                ret.StaffCd = SQLServerUtil.dbInt(rdr["StaffCd"]);
                ret.StaffName = rdr["StaffNM"].ToString();
                ret.StaffGroupNo = SQLServerUtil.dbInt(rdr["StaffGroupNo"]);
                ret.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                ret.TorihikiId = SQLServerUtil.dbDecimal(rdr["TorihikiId"]);
                ret.TorihikiCd = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                ret.TorihikiName = rdr["TorihikiNM"].ToString();
                ret.TorihikiShortName = rdr["TorihikiSNM"].ToString();
                ret.LicPlateCarNo = rdr["LicPlateCarNo"].ToString();
                ret.HaishaNyuryokuCarBackColor = SQLHelper.dbNullableInt(rdr["HaishaNyuryokuCarBackColor"]);
                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        ///休日一覧を取得します。
        /// </summary>
        /// <returns>休日データ</returns>
        public List<HaishaKyujitsuCalendarInfo> GetKyujitsuCalendar(HaishaIchiranSearchParameter para = null)
        {
            // クエリ
            string query = this.GetQueryKyujitsuCalendarSelect(para);
            List<HaishaKyujitsuCalendarInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaishaKyujitsuCalendarInfo ret = new HaishaKyujitsuCalendarInfo();
                ret.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                ret.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
                ret.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
                ret.HizukeYMD = SQLHelper.dbDate((rdr["HizukeYMD"]));
                ret.KyujitsuKbn = SQLServerUtil.dbInt(rdr["KyujitsuKbn"]);

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        #endregion

        #region プライベートメソッド


        /// <summary>
        /// 配車情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryHaishaSelect(HaishaIchiranSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");

            // 配車情報
            sb.AppendLine("   Haisha.* ");
            sb.AppendLine("   , Car.CarCd ");
            sb.AppendLine("   , Car.LicPlateCarNo AS CarLicPlateCarNo ");
            sb.AppendLine("   , CarKind.CarKindCd ");
            sb.AppendLine("   , CarKind.CarKindNM ");
            sb.AppendLine("   , Staff.StaffCd ");
            sb.AppendLine("   , Staff.StaffNM ");
            sb.AppendLine("   , Fig.FigCd ");
            sb.AppendLine("   , Fig.FigNM ");
            sb.AppendLine("   , StartPoint.PointCd As StartPointCd ");
            sb.AppendLine("   , EndPoint.PointCd As EndPointCd ");
            sb.AppendLine("   , Car.CarKbn ");
            sb.AppendLine("   , Torihikisaki.TorihikiCd ");
            sb.AppendLine("   , Torihikisaki.TorihikiNM ");
            sb.AppendLine("   , Torihikisaki.TorihikiSNM ");
            sb.AppendLine("   , Item.ItemCd ");
            sb.AppendLine("   , Owner.OwnerCd ");

            // 受注情報
            sb.AppendLine("   , JuchuInfo.JuchuId AS Juchu_JuchuId ");
            sb.AppendLine("   , JuchuInfo.DailyReportId AS Juchu_DailyReportId ");
            sb.AppendLine("   , JuchuInfo.JuchuSlipNo AS Juchu_JuchuSlipNo ");
            sb.AppendLine("   , JuchuInfo.BranchOfficeId AS Juchu_BranchOfficeId ");
            sb.AppendLine("   , JuchuInfo.CarId AS Juchu_CarId ");
            sb.AppendLine("   , JuchuInfo.LicPlateCarNo AS Juchu_LicPlateCarNo ");
            sb.AppendLine("   , JuchuInfo.CarKindId AS Juchu_CarKindId ");
            sb.AppendLine("   , JuchuInfo.CarKindCd AS Juchu_CarKindCd ");
            sb.AppendLine("   , JuchuInfo.CarKindNM AS Juchu_CarKindNM ");
            sb.AppendLine("   , JuchuInfo.DriverId AS Juchu_DriverId ");
            sb.AppendLine("   , JuchuInfo.TokuisakiId AS Juchu_TokuisakiId ");
            sb.AppendLine("   , JuchuInfo.TokuisakiNM AS Juchu_TokuisakiNM ");
            sb.AppendLine("   , JuchuInfo.ClmClassId AS Juchu_ClmClassId ");
            sb.AppendLine("   , JuchuInfo.ContractId AS Juchu_ContractId ");
            sb.AppendLine("   , JuchuInfo.StartPointId AS Juchu_StartPointId ");
            sb.AppendLine("   , JuchuInfo.StartPointNM AS Juchu_StartPointNM ");
            sb.AppendLine("   , JuchuInfo.EndPointId AS Juchu_EndPointId ");
            sb.AppendLine("   , JuchuInfo.EndPointNM AS Juchu_EndPointNM ");
            sb.AppendLine("   , JuchuInfo.ItemId AS Juchu_ItemId ");
            sb.AppendLine("   , JuchuInfo.ItemCd AS Juchu_ItemCd ");
            sb.AppendLine("   , JuchuInfo.ItemNM AS Juchu_ItemNM ");
            sb.AppendLine("   , JuchuInfo.OwnerId AS Juchu_OwnerId ");
            sb.AppendLine("   , JuchuInfo.OwnerCd AS Juchu_OwnerCd ");
            sb.AppendLine("   , JuchuInfo.OwnerNM AS Juchu_OwnerNM ");
            sb.AppendLine("   , JuchuInfo.TaskStartDateTime AS Juchu_TaskStartDateTime ");
            sb.AppendLine("   , JuchuInfo.TaskEndDateTime AS Juchu_TaskEndDateTime ");
            sb.AppendLine("   , JuchuInfo.Number AS Juchu_Number ");
            sb.AppendLine("   , JuchuInfo.FigId AS Juchu_FigId ");
            sb.AppendLine("   , JuchuInfo.AtPrice AS Juchu_AtPrice ");
            sb.AppendLine("   , JuchuInfo.Price AS Juchu_Price ");
            sb.AppendLine("   , JuchuInfo.PriceInPrice AS Juchu_PriceInPrice ");
            sb.AppendLine("   , JuchuInfo.TollFeeInPrice AS Juchu_TollFeeInPrice ");
            sb.AppendLine("   , JuchuInfo.PriceOutTaxCalc AS Juchu_PriceOutTaxCalc ");
            sb.AppendLine("   , JuchuInfo.PriceOutTax AS Juchu_PriceOutTax ");
            sb.AppendLine("   , JuchuInfo.PriceInTaxCalc AS Juchu_PriceInTaxCalc ");
            sb.AppendLine("   , JuchuInfo.PriceInTax AS Juchu_PriceInTax ");
            sb.AppendLine("   , JuchuInfo.PriceNoTaxCalc AS Juchu_PriceNoTaxCalc ");
            sb.AppendLine("   , JuchuInfo.TaxDispKbn AS Juchu_TaxDispKbn ");
            sb.AppendLine("   , JuchuInfo.AddUpYMD AS Juchu_AddUpYMD ");
            sb.AppendLine("   , JuchuInfo.FixFlag AS Juchu_FixFlag ");
            sb.AppendLine("   , JuchuInfo.Memo AS Juchu_Memo ");
            sb.AppendLine("   , JuchuInfo.ClmFixYMD AS Juchu_ClmFixYMD ");
            sb.AppendLine("   , JuchuInfo.CarOfChartererId AS Juchu_CarOfChartererId ");
            sb.AppendLine("   , JuchuInfo.CharterPrice AS Juchu_CharterPrice ");
            sb.AppendLine("   , JuchuInfo.PriceInCharterPrice AS Juchu_PriceInCharterPrice ");
            sb.AppendLine("   , JuchuInfo.TollFeeInCharterPrice AS Juchu_TollFeeInCharterPrice ");
            sb.AppendLine("   , JuchuInfo.CharterPriceOutTaxCalc AS Juchu_CharterPriceOutTaxCalc ");
            sb.AppendLine("   , JuchuInfo.CharterPriceOutTax AS Juchu_CharterPriceOutTax ");
            sb.AppendLine("   , JuchuInfo.CharterPriceInTaxCalc AS Juchu_CharterPriceInTaxCalc ");
            sb.AppendLine("   , JuchuInfo.CharterPriceInTax AS Juchu_CharterPriceInTax ");
            sb.AppendLine("   , JuchuInfo.CharterPriceNoTaxCalc AS Juchu_CharterPriceNoTaxCalc ");
            sb.AppendLine("   , JuchuInfo.CharterTaxDispKbn AS Juchu_CharterTaxDispKbn ");
            sb.AppendLine("   , JuchuInfo.CharterAddUpYMD AS Juchu_CharterAddUpYMD ");
            sb.AppendLine("   , JuchuInfo.CharterFixFlag AS Juchu_CharterFixFlag ");
            sb.AppendLine("   , JuchuInfo.CharterPayFixYMD AS Juchu_CharterPayFixYMD ");
            sb.AppendLine("   , JuchuInfo.Fee AS Juchu_Fee ");
            sb.AppendLine("   , JuchuInfo.PriceInFee AS Juchu_PriceInFee ");
            sb.AppendLine("   , JuchuInfo.TollFeeInFee AS Juchu_TollFeeInFee ");
            sb.AppendLine("   , JuchuInfo.FeeOutTaxCalc AS Juchu_FeeOutTaxCalc ");
            sb.AppendLine("   , JuchuInfo.FeeOutTax AS Juchu_FeeOutTax ");
            sb.AppendLine("   , JuchuInfo.FeeInTaxCalc AS Juchu_FeeInTaxCalc ");
            sb.AppendLine("   , JuchuInfo.FeeInTax AS Juchu_FeeInTax ");
            sb.AppendLine("   , JuchuInfo.FeeNoTaxCalc AS Juchu_FeeNoTaxCalc ");
            sb.AppendLine("   , JuchuInfo.Weight AS Juchu_Weight ");
            sb.AppendLine("   , JuchuInfo.HanroId AS Juchu_HanroId ");
            sb.AppendLine("   , JuchuInfo.OfukuKbn AS Juchu_OfukuKbn ");
            sb.AppendLine("   , JuchuInfo.OfukuKbnSortOrder AS Juchu_OfukuKbnSortOrder ");
            sb.AppendLine("   , JuchuInfo.JuchuTantoId AS Juchu_JuchuTantoId ");
            sb.AppendLine("   , JuchuInfo.JuchuTantoCd AS Juchu_JuchuTantoCd ");
            sb.AppendLine("   , JuchuInfo.JuchuTantoNM AS Juchu_JuchuTantoNM ");
            sb.AppendLine("   , JuchuInfo.BranchOfficeCd AS Juchu_BranchOfficeCd ");
            sb.AppendLine("   , JuchuInfo.BranchOfficeSNM AS Juchu_BranchOfficeSNM ");
            sb.AppendLine("   , JuchuInfo.TokuisakiCd AS Juchu_TokuisakiCd ");
            sb.AppendLine("   , JuchuInfo.StartPointCd AS Juchu_StartPointCd ");
            sb.AppendLine("   , JuchuInfo.EndPointCd AS Juchu_EndPointCd ");
            sb.AppendLine("   , JuchuInfo.FigCd AS Juchu_FigCd ");
            sb.AppendLine("   , JuchuInfo.FigNM AS Juchu_FigNM ");
            sb.AppendLine("   , JuchuInfo.HanroName AS Juchu_HanroName ");
            sb.AppendLine("   , JuchuInfo.KoteiNissu AS Juchu_KoteiNissu ");
            sb.AppendLine("   , JuchuInfo.KoteiJikan AS Juchu_KoteiJikan ");
            sb.AppendLine("   , JuchuInfo.StaffCd AS Juchu_StaffCd ");
            sb.AppendLine("   , JuchuInfo.StaffNM AS Juchu_StaffNM ");
            sb.AppendLine("   , JuchuInfo.CarKbn AS Juchu_CarKbn ");
            sb.AppendLine("   , JuchuInfo.TorihikiCd  AS Juchu_TorihikiCd ");
            sb.AppendLine("   , JuchuInfo.TorihikiNM  AS Juchu_TorihikiNM ");
            sb.AppendLine("   , JuchuInfo.TorihikiSNM  AS Juchu_TorihikiSNM ");
            sb.AppendLine("   , JuchuInfo.TaikijikanNumber AS Juchu_TaikijikanNumber ");
            sb.AppendLine("   , JuchuInfo.TaikijikanFigId AS Juchu_TaikijikanFigId ");
            sb.AppendLine("   , JuchuInfo.TaikijikanAtPrice AS Juchu_TaikijikanAtPrice ");
            sb.AppendLine("   , JuchuInfo.TaikijikanryoInPrice AS Juchu_TaikijikanryoInPrice ");
            sb.AppendLine("   , JuchuInfo.NizumiryoInPrice AS Juchu_NizumiryoInPrice ");
            sb.AppendLine("   , JuchuInfo.NioroshiryoInPrice AS Juchu_NioroshiryoInPrice ");
            sb.AppendLine("   , JuchuInfo.FutaigyomuryoInPrice AS Juchu_FutaigyomuryoInPrice ");
            sb.AppendLine("   , JuchuInfo.TaikijikanryoInFee AS Juchu_TaikijikanryoInFee ");
            sb.AppendLine("   , JuchuInfo.NizumiryoInFee AS Juchu_NizumiryoInFee ");
            sb.AppendLine("   , JuchuInfo.NioroshiryoInFee AS Juchu_NioroshiryoInFee ");
            sb.AppendLine("   , JuchuInfo.FutaigyomuryoInFee AS Juchu_FutaigyomuryoInFee ");
            sb.AppendLine("   , JuchuInfo.NizumijikanNumber AS Juchu_NizumijikanNumber ");
            sb.AppendLine("   , JuchuInfo.NizumijikanFigId AS Juchu_NizumijikanFigId ");
            sb.AppendLine("   , JuchuInfo.NizumijikanAtPrice AS Juchu_NizumijikanAtPrice ");
            sb.AppendLine("   , JuchuInfo.NioroshijikanNumber AS Juchu_NioroshijikanNumber ");
            sb.AppendLine("   , JuchuInfo.NioroshijikanFigId AS Juchu_NioroshijikanFigId ");
            sb.AppendLine("   , JuchuInfo.NioroshijikanAtPrice AS Juchu_NioroshijikanAtPrice ");
            sb.AppendLine("   , JuchuInfo.FutaigyomujikanNumber AS Juchu_FutaigyomujikanNumber ");
            sb.AppendLine("   , JuchuInfo.FutaigyomujikanFigId AS Juchu_FutaigyomujikanFigId ");
            sb.AppendLine("   , JuchuInfo.FutaigyomujikanAtPrice AS Juchu_FutaigyomujikanAtPrice ");
            sb.AppendLine("   , JuchuInfo.JomuinUriageDogakuFlag AS Juchu_JomuinUriageDogakuFlag ");
            sb.AppendLine("   , JuchuInfo.JomuinUriageKingaku AS Juchu_JomuinUriageKingaku ");
            sb.AppendLine("   , JuchuInfo.ClmClassId AS Juchu_ClmClassId");
            sb.AppendLine("   , JuchuInfo.ClmClassCd AS Juchu_ClmClassCd");
            sb.AppendLine("   , JuchuInfo.ClmClassNM AS Juchu_ClmClassNM");
            sb.AppendLine("   , JuchuInfo.ContractId AS Juchu_ContractId");
            sb.AppendLine("   , JuchuInfo.ContractCd AS Juchu_ContractCd");
            sb.AppendLine("   , JuchuInfo.ContractNM AS Juchu_ContractNM");
            sb.AppendLine("   , JuchuInfo.ReuseYMD AS Juchu_ReuseYMD");
            sb.AppendLine("   , JuchuInfo.MagoYoshasaki AS Juchu_MagoYoshasaki");

            sb.AppendLine(" FROM ");
            sb.AppendLine("   Haisha  ");
            sb.AppendLine("   INNER JOIN (  ");
            sb.AppendLine(GetQueryJuchuSelect()); // 受注情報
            sb.AppendLine("   ) JuchuInfo ");

            sb.AppendLine("     ON Haisha.JuchuId = JuchuInfo.JuchuId ");
            sb.AppendLine("     AND JuchuInfo.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   INNER JOIN TORADON_Car Car ");
            sb.AppendLine("     ON Haisha.CarId = Car.CarId ");
            sb.AppendLine("     AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_CarKind CarKind ");
            sb.AppendLine("     ON  Haisha.CarKindId = CarKind.CarKindId ");
            sb.AppendLine("     AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Torihikisaki Torihikisaki");
            sb.AppendLine("     ON Haisha.CarOfChartererId = Torihikisaki.TorihikiId ");
            sb.AppendLine("     AND Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff Staff ");
            sb.AppendLine("     ON Haisha.DriverId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Fig Fig ");
            sb.AppendLine("     ON Haisha.FigId = Fig.FigId ");
            sb.AppendLine("     AND Fig.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Point StartPoint ");
            sb.AppendLine("     ON Haisha.StartPointId = StartPoint.PointId ");
            sb.AppendLine("     AND StartPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Point EndPoint ");
            sb.AppendLine("     ON Haisha.EndPointId = EndPoint.PointId ");
            sb.AppendLine("     AND EndPoint.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Item Item ");
            sb.AppendLine("     ON Haisha.ItemId = Item.ItemId ");
            sb.AppendLine("     AND Item.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Owner Owner");
            sb.AppendLine("     ON Haisha.OwnerId = Owner.OwnerId ");
            sb.AppendLine("     AND Owner.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            if (para != null)
            {
                // 表示範囲の日付
                sb.AppendLine("   AND (  ");
                sb.AppendLine("     (  ");
                sb.AppendLine("       Haisha.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
                sb.AppendLine("       AND Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
                sb.AppendLine("     )  ");
                sb.AppendLine("     OR (  ");
                sb.AppendLine("       Haisha.TaskEndDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
                sb.AppendLine("       AND Haisha.TaskEndDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
                sb.AppendLine("     ) ");
                sb.AppendLine("     OR (  ");
                sb.AppendLine("       Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
                sb.AppendLine("       AND Haisha.TaskEndDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
                sb.AppendLine("     ) ");
                sb.AppendLine("   ) ");

                // 営業所管理区分
                if (para.HaishaBranchOfficeId != null
                 && para.HaishaBranchOfficeId.Value.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("   AND Car.BranchOfficeId = " + para.HaishaBranchOfficeId);
                }

                // 車両コード
                if (para.CarCode != null)
                {
                    sb.AppendLine("   AND Car.CarCd = " + para.CarCode);
                }

                // 車種コード
                if (para.CarKindCode != null)
                {
                    sb.AppendLine("   AND CarKind.CarKindCd = " + para.CarKindCode);
                }

                // 乗務員（社員）コード
                if (para.StaffCd != null)
                {
                    sb.AppendLine("   AND Staff.StaffCd = " + para.StaffCd);
                }

                // 車両区分
                if (para.CarKbn != null)
                {
                    if (para.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                    {
                        sb.AppendLine("   AND Car.CarKbn = " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                    else
                    {
                        sb.AppendLine("   AND Car.CarKbn <> " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                }

                // 非表示フラグ
                if (!para.DisableFlag)
                {
                    sb.AppendLine("   AND Car.DisableFlag = " + NSKUtil.BoolToInt(false) + "");
                }

            }

            return sb.ToString();
        }

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
            sb.AppendLine("   ,CarKind.CarKindCd ");
            sb.AppendLine("   ,CarKind.CarKindNM ");
            sb.AppendLine("   ,Torihikisaki.TorihikiCd ");
            sb.AppendLine("   ,Torihikisaki.TorihikiNM ");
            sb.AppendLine("   ,Torihikisaki.TorihikiSNM ");
            sb.AppendLine("   ,Owner.OwnerCd ");
            sb.AppendLine("   ,ClmClass.ClmClassCd ");
            sb.AppendLine("   ,ClmClass.ClmClassNM ");
            sb.AppendLine("   ,Contract.ContractCd ");
            sb.AppendLine("   ,Contract.ContractNM ");
            sb.AppendLine("   ,JuchuTantoStaff.StaffCd AS JuchuTantoCd ");
            sb.AppendLine("   ,JuchuTantoStaff.StaffNM AS JuchuTantoNM ");
            sb.AppendLine("   ,ISNULL(SystemName_OfukuKbn.DecimalValue01, " + DefaultProperty.OFUKUKBN_SORTORDER_MAXVALUE + ") AS OfukuKbnSortOrder ");

            sb.AppendLine("   ,Juchu.* ");
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
            sb.AppendLine("   LEFT JOIN TORADON_CarKind CarKind ");
            sb.AppendLine("     ON Juchu.CarKindId = CarKind.CarKindId ");
            sb.AppendLine("     AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
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
            sb.AppendLine("   LEFT OUTER JOIN SystemName SystemName_OfukuKbn ");
            sb.AppendLine("     ON  SystemName_OfukuKbn.SystemNameKbn =  " + (int)DefaultProperty.SystemNameKbn.OfukuKbn);
            sb.AppendLine("     AND SystemName_OfukuKbn.SystemNameCode = Juchu.OfukuKbn ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        /// <summary>
        /// 車両情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryCarSelect(HaishaIchiranSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   Car.CarId ");
            sb.AppendLine("   , Car.CarCd ");
            sb.AppendLine("   , Car.DriverId ");
            sb.AppendLine("   , Car.CarKindId ");
            sb.AppendLine("   , Car.LicPlateCarNo ");
            sb.AppendLine("   , Car.CarKbn ");
            sb.AppendLine("   , CarKind.CarKindCd ");
            sb.AppendLine("   , CarKind.CarKindNM ");
            sb.AppendLine("   , Torihikisaki.TorihikiId ");
            sb.AppendLine("   , Torihikisaki.TorihikiCd ");
            sb.AppendLine("   , Torihikisaki.TorihikiNM ");
            sb.AppendLine("   , Torihikisaki.TorihikiSNM ");
            sb.AppendLine("   , Staff.StaffCd ");
            sb.AppendLine("   , Staff.StaffNM ");
            sb.AppendLine("   , StaffHo.HaishaNyuryokuCarBackColor ");
            sb.AppendLine("   , CASE WHEN ISNULL(StaffHo.GroupNo, 0) = 0 THEN " + Int32.MaxValue.ToString() + " ELSE StaffHo.GroupNo END StaffGroupNo ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   TORADON_Car Car ");
            sb.AppendLine("   LEFT JOIN TORADON_CarKind CarKind ");
            sb.AppendLine("   ON  Car.CarKindId = CarKind.CarKindId ");
            sb.AppendLine("   AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Car CarHo ");
            sb.AppendLine("     ON Car.CarId = CarHo.ToraDONCarId ");
            sb.AppendLine("     AND CarHo.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Torihikisaki Torihikisaki");
            sb.AppendLine("     ON CarHo.YoshasakiId = Torihikisaki.TorihikiId ");
            sb.AppendLine("     AND Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff Staff ");
            sb.AppendLine("     ON Car.DriverId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Staff StaffHo ");
            sb.AppendLine("     ON Staff.StaffId = StaffHo.ToraDONStaffId ");
            sb.AppendLine("     AND StaffHo.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Car.DelFlag = " + NSKUtil.BoolToInt(false).ToString() + "");

            if (para != null) 
            {
                // 車両コード
                if (para.CarCode != null)
                {
                    sb.AppendLine("   AND Car.CarCd = " + para.CarCode);
                }

                // 車種コード
                if (para.CarKindCode != null)
                {
                    sb.AppendLine("   AND CarKind.CarKindCd = " + para.CarKindCode);
                }

                // 乗務員（社員）コード
                if (para.StaffCd != null)
                {
                    sb.AppendLine("   AND Staff.StaffCd = " + para.StaffCd);
                }

                // 車両区分
                if (para.CarKbn != null)
                {
                    if (para.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                    {
                        sb.AppendLine("   AND Car.CarKbn = " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                    else
                    {
                        sb.AppendLine("   AND Car.CarKbn <> " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                }

                // 非表示フラグ
                if (!para.DisableFlag)
                {
                    sb.AppendLine("   AND Car.DisableFlag = " + NSKUtil.BoolToInt(false).ToString() + "");
                }

                // 営業所管理区分
                if (para.HaishaBranchOfficeId != null
                    && para.HaishaBranchOfficeId.Value.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("   AND Car.BranchOfficeId = " + para.HaishaBranchOfficeId);
                }
            }

            sb.AppendLine(" ORDER BY ");
            sb.AppendLine(" Car.CarKbn ");
            sb.AppendLine(" ,CASE WHEN ISNULL(StaffHo.GroupNo, 0) = 0 THEN " + Int32.MaxValue.ToString() + " ELSE StaffHo.GroupNo END ");
            sb.AppendLine(" ,Car.CarCd ");

            return sb.ToString();
        }

        /// <summary>
        /// 休日情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryKyujitsuCalendarSelect(HaishaIchiranSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   TORADON_Car.CarId ");
            sb.AppendLine("   , TORADON_Car.CarCd ");
            sb.AppendLine("   , TORADON_Car.DriverId ");
            sb.AppendLine("   , KyujitsuCalendarMeisai.HizukeYMD ");
            sb.AppendLine("   , KyujitsuCalendarMeisai.KyujitsuKbn  ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   TORADON_Car  ");
            sb.AppendLine("   LEFT JOIN TORADON_CarKind ");
            sb.AppendLine("   ON  TORADON_Car.CarKindId = TORADON_CarKind.CarKindId ");
            sb.AppendLine("   AND TORADON_CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff ");
            sb.AppendLine("     ON TORADON_Car.DriverId = TORADON_Staff.StaffId ");
            sb.AppendLine("     AND TORADON_Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   INNER JOIN KyujitsuCalendar  ");
            sb.AppendLine("     ON TORADON_Car.DriverId = KyujitsuCalendar.ToraDONStaffId  ");
            sb.AppendLine("     AND KyujitsuCalendar.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   INNER JOIN KyujitsuCalendarMeisai  ");
            sb.AppendLine("     ON KyujitsuCalendar.KyujitsuCalendarId = KyujitsuCalendarMeisai.KyujitsuCalendarId  ");
            sb.AppendLine(" WHERE ");

            sb.AppendLine("   KyujitsuCalendarMeisai.HizukeYMD >=  " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
            sb.AppendLine("   AND KyujitsuCalendarMeisai.HizukeYMD <=  " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));

            sb.AppendLine("   AND TORADON_Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            if (para != null)
            {
                // 車両コード
                if (para.CarCode != null)
                {
                    sb.AppendLine("   AND TORADON_Car.CarCd = " + para.CarCode);
                }

                // 車種コード
                if (para.CarKindCode != null)
                {
                    sb.AppendLine("   AND TORADON_CarKind.CarKindCd = " + para.CarKindCode);
                }

                // 乗務員（社員）コード
                if (para.StaffCd != null)
                {
                    sb.AppendLine("   AND TORADON_Staff.StaffCd = " + para.StaffCd);
                }

                // 車両区分
                if (para.CarKbn != null)
                {
                    if (para.CarKbn == (int)DefaultProperty.CarKbn.Yosha)
                    {
                        sb.AppendLine("   AND TORADON_Car.CarKbn = " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                    else
                    {
                        sb.AppendLine("   AND TORADON_Car.CarKbn <> " + (int)DefaultProperty.CarKbn.Yosha);
                    }
                }

                // 非表示フラグ
                if (!para.DisableFlag)
                {
                    sb.AppendLine("   AND TORADON_Car.DisableFlag = " + NSKUtil.BoolToInt(false).ToString() + "");
                }

                // 営業所管理区分
                if (para.HaishaBranchOfficeId != null
                    && para.HaishaBranchOfficeId.Value.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("   AND TORADON_Car.BranchOfficeId = " + para.HaishaBranchOfficeId);
                }
            }

            return sb.ToString();
        }

        #endregion
    }
}
