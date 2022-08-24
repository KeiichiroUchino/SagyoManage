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
    /// 配車入力のデータアクセスレイヤです。
    /// </summary>
    public class HaishaNyuryoku
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
        public HaishaNyuryoku()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public HaishaNyuryoku(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 受注情報を取得します。
        /// </summary>
        /// <returns>受注データ</returns>
        public List<JuchuInfo> GetJuchu(HaishaNyuryokuSearchParameter para = null)
        {
            string query = string.Empty;

            // クエリ
            if (para != null && para.KeyFlg)
            {
                // 該当データがない場合は終了
                if (para.HaishaNyuryokuInfo.Count() == 0) return new List<JuchuInfo>();

                // キー検索の場合
                int i = 0;
                foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> info in para.HaishaNyuryokuInfo)
                {
                    // 2回目以降はUNION ALLで連結
                    if (i > 0) query = query + Environment.NewLine + " UNION " + Environment.NewLine;

                    para.JuchuId = info.Value.JuchuId;
                    query = query + this.GetQueryJuchuSelect(para);
                    i++;
                }
            }
            else
            {
                query = this.GetQueryJuchuSelect(para);
            }

            List<JuchuInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                JuchuInfo ret = new JuchuInfo();
                ret.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                ret.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["JuchuSlipNo"]);
                ret.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                ret.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
                ret.CarLicPlateCarNo = rdr["CarLicPlateCarNo"].ToString();
                ret.LicPlateCarNo = rdr["LicPlateCarNo"].ToString();
                ret.CarBranchOfficeShortName = rdr["CarBranchOfficeSNM"].ToString();
                ret.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                ret.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                ret.CarKindName = rdr["CarKindNM"].ToString();
                ret.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
                ret.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                ret.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                ret.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                ret.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                ret.Price = SQLServerUtil.dbDecimal(rdr["Price"]);
                ret.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                ret.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                ret.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]);
                ret.PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]);
                ret.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]);
                ret.PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]);
                ret.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]);
                ret.TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]);
                ret.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                ret.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                ret.CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]);
                ret.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                ret.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                ret.CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]);
                ret.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]);
                ret.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]);
                ret.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]);
                ret.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]);
                ret.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]);
                ret.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]);
                ret.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                ret.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                ret.Fee = SQLServerUtil.dbDecimal(rdr["Fee"]);
                ret.PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]);
                ret.TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]);
                ret.FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]);
                ret.FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]);
                ret.FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]);
                ret.FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]);
                ret.FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]);
                ret.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
                ret.StartPointName = rdr["StartPointNM"].ToString();
                ret.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
                ret.EndPointName = rdr["EndPointNM"].ToString();
                ret.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]);
                ret.BranchOfficeCode = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]);
                ret.BranchOfficeShortName = rdr["BranchOfficeSNM"].ToString();
                ret.TokuisakiCode = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                ret.TokuisakiName = rdr["TokuisakiNM"].ToString();
                ret.StartPointCode = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                ret.EndPointCode = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                ret.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
                ret.ItemCode = SQLServerUtil.dbInt(rdr["ItemCd"]);
                ret.ItemName = rdr["ItemNM"].ToString();
                ret.OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]);
                ret.OwnerCode = SQLServerUtil.dbInt(rdr["OwnerCd"]);
                ret.OwnerName = rdr["OwnerNM"].ToString();
                ret.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                ret.FigCode = SQLServerUtil.dbInt(rdr["FigCd"]);
                ret.FigName = rdr["FigNM"].ToString();
                ret.HanroCode = SQLServerUtil.dbInt(rdr["HanroCode"]);
                ret.HanroName = rdr["HanroName"].ToString();
                ret.StaffCd = SQLServerUtil.dbInt(rdr["StaffCd"]);
                ret.StaffName = rdr["StaffNM"].ToString();
                ret.OfukuKbn = SQLServerUtil.dbInt(rdr["OfukuKbn"]);
                ret.OfukuKbnSortOrder = SQLServerUtil.dbDecimal(rdr["OfukuKbnSortOrder"]);
                ret.KoteiNissu = SQLServerUtil.dbInt(rdr["KoteiNissu"]);
                ret.KoteiJikan = SQLServerUtil.dbInt(rdr["KoteiJikan"]);
                ret.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                ret.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                ret.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                ret.TorihikiCd = SQLServerUtil.dbInt(rdr["TorihikiCd"]);
                ret.TorihikiName = rdr["TorihikiNM"].ToString();
                ret.TorihikiShortName = rdr["TorihikiSNM"].ToString();

                ret.ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]);
                ret.ClmClassCode = SQLServerUtil.dbInt(rdr["ClmClassCd"]);
                ret.ClmClassName = rdr["ClmClassNM"].ToString();
                ret.ContractId = SQLServerUtil.dbDecimal(rdr["ContractId"]);
                ret.ContractCode = SQLServerUtil.dbInt(rdr["ContractCd"]);
                ret.ContractName = rdr["ContractNM"].ToString();
                ret.JuchuTantoId = SQLServerUtil.dbDecimal(rdr["JuchuTantoId"]);
                ret.JuchuTantoCode = SQLServerUtil.dbInt(rdr["JuchuTantoCd"]);
                ret.JuchuTantoName = rdr["JuchuTantoNM"].ToString();

                ret.TaikijikanNumber = SQLServerUtil.dbDecimal(rdr["TaikijikanNumber"]);
                ret.TaikijikanFigId = SQLServerUtil.dbDecimal(rdr["TaikijikanFigId"]);
                ret.TaikijikanAtPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanAtPrice"]);
                ret.TaikijikanryoInPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInPrice"]);
                ret.NizumiryoInPrice = SQLServerUtil.dbDecimal(rdr["NizumiryoInPrice"]);
                ret.NioroshiryoInPrice = SQLServerUtil.dbDecimal(rdr["NioroshiryoInPrice"]);
                ret.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                ret.TaikijikanryoInFee = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInFee"]);
                ret.NizumiryoInFee = SQLServerUtil.dbDecimal(rdr["NizumiryoInFee"]);
                ret.NioroshiryoInFee = SQLServerUtil.dbDecimal(rdr["NioroshiryoInFee"]);
                ret.FutaigyomuryoInFee = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInFee"]);
                ret.NizumijikanNumber = SQLServerUtil.dbDecimal(rdr["NizumijikanNumber"]);
                ret.NizumijikanFigId = SQLServerUtil.dbDecimal(rdr["NizumijikanFigId"]);
                ret.NizumijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NizumijikanAtPrice"]);
                ret.NioroshijikanNumber = SQLServerUtil.dbDecimal(rdr["NioroshijikanNumber"]);
                ret.NioroshijikanFigId = SQLServerUtil.dbDecimal(rdr["NioroshijikanFigId"]);
                ret.NioroshijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NioroshijikanAtPrice"]);
                ret.FutaigyomujikanNumber = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanNumber"]);
                ret.FutaigyomujikanFigId = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanFigId"]);
                ret.FutaigyomujikanAtPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanAtPrice"]);
                ret.JomuinUriageDogakuFlag = SQLHelper.dbBit(rdr["JomuinUriageDogakuFlag"]);
                ret.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);

                ret.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                ret.MagoYoshasaki = rdr["MagoYoshasaki"].ToString();

                // 受注情報一覧の検索時のみ結合
                if (para != null && !para.KeyFlg)
                {
                    //TODO 性能問題のためコメント化中
                    //ret.AddUpDate =
                    //    NSKUtil.DecimalWithTimeToDateTime(
                    //        SQLServerUtil.dbDecimal(rdr["AddUpDate"]));
                    //ret.CharterAddUpDate =
                    //        NSKUtil.DecimalWithTimeToDateTime(
                    //            SQLServerUtil.dbDecimal(rdr["CharterAddUpDate"]));
                    //ret.ToraDonFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["ToraDon_FixFlag"]));
                    //ret.ToraDonCharterFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["ToraDon_CharterFixFlag"]));
                }

                // キー検索の場合
                if (para != null && para.KeyFlg)
                {
                    ret.DelTargetFlg = true;
                }
                else
                {
                    ret.DelTargetFlg = false;
                }

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        /// 配車情報を取得します。
        /// </summary>
        /// <returns>配車データ</returns>
        public List<HaishaSearchResultInfo> GetHaisha(HaishaNyuryokuSearchParameter para = null)
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
                retHaisha.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                retHaisha.CarKindName = rdr["CarKindNM"].ToString();
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
                retHaisha.HanroCode = SQLServerUtil.dbInt(rdr["Juchu_HanroCode"]);
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

                //TODO 性能問題のためコメント化中 START
                //retHaisha.MinClmFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["ClmFixDate"]));
                //retHaisha.MinCharterPayFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["CharterPayFixDate"]));

                //retHaisha.AddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["AddUpDate"]));
                //retHaisha.CharterAddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["CharterAddUpDate"]));
                //retHaisha.ToraDonFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["ToraDon_FixFlag"]));
                //retHaisha.ToraDonCharterFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["ToraDon_CharterFixFlag"]));
                //TODO 性能問題のためコメント化中 END

                // 受注情報
                JuchuInfo retJuchu = new JuchuInfo();
                retJuchu.JuchuId = SQLServerUtil.dbDecimal(rdr["Juchu_JuchuId"]);
                retJuchu.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["Juchu_JuchuSlipNo"]);
                retJuchu.CarId = SQLServerUtil.dbDecimal(rdr["Juchu_CarId"]);
                retJuchu.CarLicPlateCarNo = rdr["Juchu_CarLicPlateCarNo"].ToString();
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
                retJuchu.ClmFixYMD = SQLHelper.dbDate(rdr["Juchu_ClmFixYMD"]);
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
                retJuchu.CharterPayFixYMD = SQLHelper.dbDate(rdr["Juchu_CharterPayFixYMD"]);
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
                retJuchu.HanroCode = SQLServerUtil.dbInt(rdr["Juchu_HanroCode"]);
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
        public List<HaishaCarInfo> GetCar(HaishaNyuryokuSearchParameter para = null)
        {
            // クエリ
            string query = this.GetQueryCarSelect(para);
            List<HaishaCarInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaishaCarInfo ret = new HaishaCarInfo();
                ret.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                ret.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
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
                ret.HaishaNyuryokuCarCountExclusionFlag = SQLHelper.dbBit(rdr["HaishaNyuryokuCarCountExclusionFlag"]);
                ret.DisableFlag = SQLServerUtil.dbInt(rdr["DisableFlag"]);

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        ///休日一覧を取得します。
        /// </summary>
        /// <returns>休日データ</returns>
        public List<HaishaKyujitsuCalendarInfo> GetKyujitsuCalendar(HaishaNyuryokuSearchParameter para = null)
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

        /// <summary>
        ///排他対象一覧を取得します。
        /// </summary>
        /// <returns>排他対象一覧データ</returns>
        public List<HaitaInfo> GetExclusiveInfoSelect(HaishaNyuryokuSearchParameter para = null)
        {
            // クエリ
            string query = this.GeQuerytExclusiveInfoSelect(para);
            List<HaitaInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaitaInfo ret = new HaitaInfo();
                ret.HaishaId = SQLServerUtil.dbDecimal(rdr["HaishaId"]);

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        ///チェック用休日一覧を取得します。
        /// </summary>
        /// <returns>休日データ</returns>
        public List<HaishaKyujitsuCalendarInfo> GetCheckKyujitsuCalendar(DateTime stratYMD, DateTime endYMD, Decimal? driverId)
        {
            // クエリ
            string query = this.GetQueryCheckKyujitsuCalendarSelect(stratYMD, endYMD, driverId);
            List<HaishaKyujitsuCalendarInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaishaKyujitsuCalendarInfo ret = new HaishaKyujitsuCalendarInfo();
                ret.DriverId = SQLServerUtil.dbDecimal(rdr["ToraDONStaffId"]);
                ret.HizukeYMD = SQLHelper.dbDate((rdr["HizukeYMD"]));

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        /// 受注IDに紐づく配車の件数を取得します。
        /// </summary>
        /// <param name="juchuId">受注ID</param>
        /// <returns>配車件数</returns>
        public int GetCheckHsishaSelectCount(decimal juchuId)
        {
            // クエリ
            string query = this.GetQueryCheckHsishaSelect(juchuId);
            List<decimal> list = SQLHelper.SimpleRead(query, rdr => SQLServerUtil.dbDecimal(rdr["HaishaId"]));

            return list.Count;
        }

        /// <summary>
        /// 車両IDを指定し、車両が表示可能か判定します。
        /// </summary>
        /// <param name="juchuId">受注ID</param>
        /// <returns>表示不可：true、表示可：false</returns>
        public bool GetCheckDisable(decimal carId)
        {
            // クエリ
            string query = this.GetQueryCheckDisableSelectCount(carId);
            List<decimal> list = SQLHelper.SimpleRead(query, rdr => SQLServerUtil.dbDecimal(rdr["carId"]));

            if (list.Count > 0) return false;
            return true;
        }

        /// <summary>
        /// 請求・支払日付を取得します。
        /// </summary>
        /// <returns>配車データ</returns>
        public List<HaishaNyuryokuInfo> GetFixDat(HaishaNyuryokuSearchParameter para = null)
        {
            // クエリ
            string query = this.GetQueryFixDatSelect(para);
            List<HaishaNyuryokuInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {

                //配車情報
                HaishaNyuryokuInfo retHaisha = new HaishaNyuryokuInfo();
                retHaisha.HaishaId = SQLServerUtil.dbDecimal(rdr["HaishaId"]);
                //TODO 性能問題のためコメント化中 START
                //retHaisha.MinClmFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["ClmFixDate"]));
                //retHaisha.MinCharterPayFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["CharterPayFixDate"]));
                //retHaisha.AddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["AddUpDate"]));
                //retHaisha.CharterAddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["CharterAddUpDate"]));
                //retHaisha.ToraDonFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["FixFlag"]));
                //retHaisha.ToraDonCharterFixFlag = this.intFromBool(SQLServerUtil.dbInt(rdr["CharterFixFlag"]));
                //TODO 性能問題のためコメント化中 END

                //返却用の値を返します
                return retHaisha;
            });

            return list;
        }

        /// <summary>
        /// 車両IDを指定し、車両が表示可能か判定します。
        /// </summary>
        /// <returns>利用者補情報</returns>
        public List<HaitaOperatorExInfo> GetOperatorEx()
        {
            // クエリ
            string query = this.GetQueryOperatorExSelect();
            List<HaitaOperatorExInfo> list = SQLHelper.SimpleRead(query, rdr =>
            {
                //返却用の値
                HaitaOperatorExInfo ret = new HaitaOperatorExInfo();
                ret.PrevTimeInterval = SQLServerUtil.dbInt(rdr["PrevTimeInterval"]);
                ret.PrevDisplayRange = SQLServerUtil.dbInt(rdr["PrevDisplayRange"]);
                ret.PrevCarType = SQLServerUtil.dbInt(rdr["PrevCarType"]);
                ret.PrevCarOfiice = SQLServerUtil.dbDecimal(rdr["PrevCarOfiice"]);
                ret.PrevHaishaNyuryokuMihainomiFlag = SQLHelper.dbBit(rdr["PrevHaishaNyuryokuMihainomiFlag"]);

                //返却用の値を返します
                return ret;
            });

            return list;
        }

        /// <summary>
        /// 配車テーブルの追加を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">追加配車情報</param>
        public void AddHaisha(SqlTransaction transaction, Dictionary<decimal, HaishaNyuryokuInfo> info)
        {

            var i = 0;
            List<string> mySqlList = new List<string>();
            List<decimal> idList = SQLHelper.GetSequenceIds(SQLHelper.SequenceIdKind.HaishaIdx, info.Count);

            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> item in info)
            {
                // 配車IDを設定
                item.Value.HaishaId = idList[i];

                // 顧客の未収詳細を登録
                mySqlList.Add(this.GetCommandHaishaInsert(item.Value));
                i++;

                if (i % EXECUTE_COUNT == 0 || info.Count() == i)
                {
                    int createCnt = mySqlList.Count();

                    // パフォーマンス向上のためにクエリを結合して一つのSQLにして実行する
                    string query = SQLHelper.SQLQueryJoin(mySqlList);

                    //指定したトランザクション上でExecuteNonqueryを実行し
                    int execCnt = SQLHelper.ExecuteNonQueryOnTransaction(transaction, new SqlCommand(query));

                    if (createCnt != execCnt)
                    {
                        //リトライ可能な例外
                        throw new
                            Model.DALExceptions.CanRetryException(
                            "配車情報作成に失敗しました。\r\n再度処理を実行してください。"
                            , MessageBoxIcon.Warning);
                    }

                    mySqlList.Clear();
                }

            }

        }

        /// <summary>
        /// 配車テーブルの更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">更新配車情報</param>
        public void UpdateHaisha(SqlTransaction transaction, Dictionary<decimal, HaishaNyuryokuInfo> info)
        {
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> item in info)
            {
                // 更新
                SqlCommand command = this.GetCommandHaishaUpdate(item.Value);

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
            }
        }

        /// <summary>
        /// 配車テーブルのレコードをDELETE
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="info">削除配車情報</param>
        public void DeleteHaisha(SqlTransaction transaction, Dictionary<decimal, HaishaNyuryokuInfo> info)
        {
            foreach (KeyValuePair<decimal, HaishaNyuryokuInfo> item in info)
            {
                // 更新
                SqlCommand command = this.GetCommandHaishaDelete(item.Value);

                //指定したトランザクション上でExecuteNonqueryを実行し
                //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
            }
        }

        /// <summary>
        /// 配車排他管理テーブルのレコードをDELETE
        /// </summary>
        /// <param name="transaction"></param>
        /// <param name="info">削除配車情報</param>
        public void DeleteHaishaExclusiveManage(SqlTransaction transaction)
        {
            SqlCommand command = this.GetCommandHaishaaExclusiveManageDelete();

            // 削除実行（該当データがある場合は必ず削除し、ない場合はそのまま続行）
            SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);
        }

        /// <summary>
        /// 利用者補テーブルの更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="para">表示条件情報</param>
        public void UpdateOperatorE(SqlTransaction transaction, HaishaNyuryokuSearchParameter para = null)
        {
            // 更新
            SqlCommand command = this.GetMergeOperatorExUpdate(para);

            //指定したトランザクション上でExecuteNonqueryを実行し
            //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
            //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
            SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 受注情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">受注条件情報</param>
        /// <returns></returns>
        private string GetQueryJuchuSelect(HaishaNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("    BranchOffice.BranchOfficeCd ");
            sb.AppendLine("   ,BranchOffice.BranchOfficeSNM ");
            sb.AppendLine("   ,Tokuisaki.TokuisakiCd ");
            sb.AppendLine("   ,StartPoint.PointCd As StartPointCd ");
            sb.AppendLine("   ,EndPoint.PointCd As EndPointCd ");
            sb.AppendLine("   ,Item.ItemCd ");
            sb.AppendLine("   ,Fig.FigCd ");
            sb.AppendLine("   ,Fig.FigNM ");
            sb.AppendLine("   ,Hanro.HanroCode ");
            sb.AppendLine("   ,Hanro.HanroName ");
            sb.AppendLine("   ,Hanro.KoteiNissu ");
            sb.AppendLine("   ,Hanro.KoteiJikan ");
            sb.AppendLine("   ,Staff.StaffCd ");
            sb.AppendLine("   ,Staff.StaffNM ");
            sb.AppendLine("   ,Car.CarCd ");
            sb.AppendLine("   ,Car.CarKbn ");
            sb.AppendLine("   ,Car.LicPlateCarNo AS CarLicPlateCarNo ");
            sb.AppendLine("   ,CarBranchOffice.BranchOfficeSNM AS CarBranchOfficeSNM ");
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

            // 受注情報一覧の検索時のみ結合
            if (para != null && !para.KeyFlg)
            {
                //TODO 性能問題のためコメント化中
                //sb.AppendLine("   ,TORADON_Sale.AddUpDate ");
                //sb.AppendLine("   ,TORADON_Sale.CharterAddUpDate  ");
                //sb.AppendLine("   ,TORADON_Sale.FixFlag  AS ToraDon_FixFlag");
                //sb.AppendLine("   ,TORADON_Sale.CharterFixFlag  AS ToraDon_CharterFixFlag");
            }

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
            sb.AppendLine("   LEFT JOIN TORADON_BranchOffice CarBranchOffice ");
            sb.AppendLine("     ON Car.BranchOfficeId = CarBranchOffice.BranchOfficeId ");
            sb.AppendLine("     AND CarBranchOffice.DelFlag = " + NSKUtil.BoolToInt(false) + "");
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
            sb.AppendLine("   LEFT OUTER JOIN SystemName SystemName_OfukuKbn ");
            sb.AppendLine("     ON  SystemName_OfukuKbn.SystemNameKbn =  " + (int)DefaultProperty.SystemNameKbn.OfukuKbn);
            sb.AppendLine("     AND SystemName_OfukuKbn.SystemNameCode = Juchu.OfukuKbn ");

            // 受注情報一覧の検索時のみ結合
            if (para != null && !para.KeyFlg)
            {
                sb.AppendLine("   LEFT JOIN Haisha ");
                sb.AppendLine("     ON Juchu.JuchuId = Haisha.JuchuId ");
                sb.AppendLine("     AND Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                //TODO 性能問題のためコメント化中 START
                //sb.AppendLine("  LEFT OUTER JOIN ");
                //sb.AppendLine("   HaishaSeikyuRenkeiManage ON ");
                //sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
                //sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                //sb.AppendLine("  LEFT OUTER JOIN ");
                //sb.AppendLine("   TORADON_Sale ON ");
                //sb.AppendLine("   TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
                //sb.AppendLine("   TORADON_Sale.DelFlag = " + NSKUtil.BoolToInt(false) + "");
                //TODO 性能問題のためコメント化中 END
            }

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

            if (para != null)
            {
                if (para.KeyFlg)
                {
                    sb.AppendLine("   AND Juchu.JuchuId = " + para.JuchuId.ToString());
                }
                else
                {
                    sb.AppendLine("   AND Haisha.HaishaId is null");

                    // 営業所管理区分
                    if (para.BranchOfficeId != null
                        && para.BranchOfficeId.Value.CompareTo(decimal.Zero) != 0)
                    {
                        sb.AppendLine("   AND Juchu.BranchOfficeId = " + para.BranchOfficeId);
                    }

                    // 開始日付
                    if (para.JuchuStartYMD != null)
                    {
                        sb.AppendLine("   AND Juchu.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.JuchuStartYMD.Value));
                    }

                    // 終了日付
                    if (para.JuchuEndYMD != null)
                    {
                        sb.AppendLine("   AND Juchu.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.JuchuEndYMD.Value));
                    }

                    // 得意先
                    if (para.TokuisakiCd != null)
                    {
                        sb.AppendLine("   AND Tokuisaki.TokuisakiCd = " + para.TokuisakiCd);
                    }

                    // 方面
                    if (para.HomenCode != null)
                    {
                        sb.AppendLine("   AND Homen.HomenCode = " + para.HomenCode);
                    }

                    // 車種
                    if (para.JuchuCarKindId != null && para.JuchuCarKindId.Value.CompareTo(decimal.Zero) != 0)
                    {
                        sb.AppendLine("   AND Juchu.CarKindId = " + para.JuchuCarKindId);
                    }

                    // 受注担当
                    if (para.JuchuTAntoshaId != null && para.JuchuTAntoshaId.Value.CompareTo(decimal.Zero) != 0)
                    {
                        sb.AppendLine("   AND Juchu.JuchuTantoId = " + para.JuchuTAntoshaId);
                    }

                    // 排他用抽出条件
                    if (para.HaishaNyuryokuConditionInfo != null)
                    {

                        // 配車入力条件区分
                        switch (para.HaishaNyuryokuConditionInfo.HaishaNyuryokuJokenKbn)
                        {
                            case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                                // 得意先（範囲指定）
                                sb.AppendLine("   AND Juchu.TokuisakiId IN ( ");
                                sb.AppendLine(" SELECT ");
                                sb.AppendLine("   WorkId  ");
                                sb.AppendLine(" FROM ");
                                sb.AppendLine("   HaishaExclusiveManage  ");
                                sb.AppendLine(" WHERE ");
                                sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                                sb.AppendLine("   ) ");

                                break;
                            case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                                // 方面(着地IDで指定)
                                sb.AppendLine("   AND Juchu.EndPointId IN ( ");
                                sb.AppendLine(" SELECT ");
                                sb.AppendLine("   WorkId  ");
                                sb.AppendLine(" FROM ");
                                sb.AppendLine("   HaishaExclusiveManage  ");
                                sb.AppendLine(" WHERE ");
                                sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                                sb.AppendLine("   ) ");

                                break;
                            case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                                // 車種
                                sb.AppendLine("   AND Juchu.CarKindId IN ( ");
                                sb.AppendLine(" SELECT ");
                                sb.AppendLine("   WorkId  ");
                                sb.AppendLine(" FROM ");
                                sb.AppendLine("   HaishaExclusiveManage  ");
                                sb.AppendLine(" WHERE ");
                                sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                                sb.AppendLine("   ) ");

                                break;
                            default:
                                break;
                        }
                    }
                }
            }

            return sb.ToString();
        }

        /// <summary>
        /// 配車情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryHaishaSelect(HaishaNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");

            // 配車情報
            sb.AppendLine("   Haisha.* ");
            sb.AppendLine("   , Car.CarCd ");
            sb.AppendLine("   , Car.LicPlateCarNo AS CarLicPlateCarNo");
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
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine("   , TORADON_Sale.ClmFixDate ");
            //sb.AppendLine("   , TORADON_Sale.CharterPayFixDate  ");
            //sb.AppendLine("   , TORADON_Sale.AddUpDate ");
            //sb.AppendLine("   , TORADON_Sale.CharterAddUpDate  ");
            //sb.AppendLine("   , TORADON_Sale.FixFlag AS ToraDon_FixFlag");
            //sb.AppendLine("   , TORADON_Sale.CharterFixFlag AS ToraDon_CharterFixFlag");
            //TODO 性能問題のためコメント化中 END

            // 受注情報
            sb.AppendLine("   , JuchuInfo.JuchuId AS Juchu_JuchuId ");
            sb.AppendLine("   , JuchuInfo.DailyReportId AS Juchu_DailyReportId ");
            sb.AppendLine("   , JuchuInfo.JuchuSlipNo AS Juchu_JuchuSlipNo ");
            sb.AppendLine("   , JuchuInfo.BranchOfficeId AS Juchu_BranchOfficeId ");
            sb.AppendLine("   , JuchuInfo.CarId AS Juchu_CarId ");
            sb.AppendLine("   , JuchuInfo.CarLicPlateCarNo AS Juchu_CarLicPlateCarNo ");
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
            sb.AppendLine("   , JuchuInfo.HanroCode AS Juchu_HanroCode ");
            sb.AppendLine("   , JuchuInfo.HanroName AS Juchu_HanroName ");
            sb.AppendLine("   , JuchuInfo.KoteiNissu AS Juchu_KoteiNissu ");
            sb.AppendLine("   , JuchuInfo.KoteiJikan AS Juchu_KoteiJikan ");
            sb.AppendLine("   , JuchuInfo.StaffCd AS Juchu_StaffCd ");
            sb.AppendLine("   , JuchuInfo.StaffNM AS Juchu_StaffNM ");
            sb.AppendLine("   , JuchuInfo.CarKbn AS Juchu_CarKbn ");
            sb.AppendLine("   , JuchuInfo.TorihikiCd  AS Juchu_TorihikiCd ");
            sb.AppendLine("   , JuchuInfo.TorihikiNM  AS Juchu_TorihikiNM ");
            sb.AppendLine("   , JuchuInfo.TorihikiSNM  AS Juchu_TorihikiSNM ");
            sb.AppendLine("   , JuchuInfo.CarKindId AS Juchu_CarKindId ");
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
            sb.AppendLine(GetQueryJuchuSelect(null)); // 受注情報
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
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage ON ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            //sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   TORADON_Sale ON ");
            //sb.AppendLine("   TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            //sb.AppendLine("   TORADON_Sale.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            //TODO 性能問題のためコメント化中 END

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
                sb.AppendLine("       Haisha.ReuseYMD >= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
                sb.AppendLine("       AND Haisha.ReuseYMD <= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
                sb.AppendLine("     ) ");
                sb.AppendLine("     OR (  ");
                sb.AppendLine("       Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
                sb.AppendLine("       AND Haisha.ReuseYMD >= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
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
                    sb.AppendLine("   AND Car.DisableFlag = " + NSKUtil.BoolToInt(false).ToString() + "");
                }

            }

            return sb.ToString();
        }

        /// <summary>
        /// 車両情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryCarSelect(HaishaNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("   TORADON_Car.CarId ");
            sb.AppendLine("   , TORADON_Car.CarCd ");
            sb.AppendLine("   , TORADON_Car.DriverId ");
            sb.AppendLine("   , TORADON_Car.CarKbn ");
            sb.AppendLine("   , TORADON_Car.CarKindId ");
            sb.AppendLine("   , TORADON_Car.LicPlateCarNo ");
            sb.AppendLine("   , TORADON_Car.DisableFlag ");
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
            sb.AppendLine("   , Car.HaishaNyuryokuCarCountExclusionFlag ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   TORADON_Car ");
            sb.AppendLine("   LEFT JOIN Car ");
            sb.AppendLine("     ON TORADON_Car.CarId = Car.ToraDONCarId ");
            sb.AppendLine("     AND Car.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_CarKind CarKind ");
            sb.AppendLine("   ON  TORADON_Car.CarKindId = CarKind.CarKindId ");
            sb.AppendLine("   AND CarKind.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Torihikisaki Torihikisaki");
            sb.AppendLine("     ON Car.YoshasakiId = Torihikisaki.TorihikiId ");
            sb.AppendLine("     AND Torihikisaki.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN TORADON_Staff Staff ");
            sb.AppendLine("     ON TORADON_Car.DriverId = Staff.StaffId ");
            sb.AppendLine("     AND Staff.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   LEFT JOIN Staff StaffHo ");
            sb.AppendLine("     ON Staff.StaffId = StaffHo.ToraDONStaffId ");
            sb.AppendLine("     AND StaffHo.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   TORADON_Car.DelFlag = " + NSKUtil.BoolToInt(false).ToString() + "");

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

            sb.AppendLine(" ORDER BY ");
            sb.AppendLine(" TORADON_Car.CarKbn ");
            sb.AppendLine(" ,CASE WHEN ISNULL(StaffHo.GroupNo, 0) = 0 THEN " + Int32.MaxValue.ToString() + " ELSE StaffHo.GroupNo END ");
            sb.AppendLine(" ,TORADON_Car.CarCd ");

            return sb.ToString();
        }

        /// <summary>
        /// 休日情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryKyujitsuCalendarSelect(HaishaNyuryokuSearchParameter para)
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

        /// <summary>
        /// チェック用休日情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="stratYMD">開始年月日</param>
        /// <param name="endYMD">終了年月日</param>
        /// <param name="driverId">乗務員ID</param>
        /// <returns></returns>
        private string GetQueryCheckKyujitsuCalendarSelect(DateTime stratYMD, DateTime endYMD, Decimal? driverId)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine(" KyujitsuCalendar.ToraDONStaffId ");
            sb.AppendLine(" ,KyujitsuCalendarMeisai.HizukeYMD ");
            sb.AppendLine(" FROM ");
            sb.AppendLine(" KyujitsuCalendar   ");
            sb.AppendLine(" INNER JOIN KyujitsuCalendarMeisai   ");
            sb.AppendLine("   ON KyujitsuCalendar.KyujitsuCalendarId = KyujitsuCalendarMeisai.KyujitsuCalendarId  ");
            sb.AppendLine(" WHERE ");

            sb.AppendLine("   KyujitsuCalendarMeisai.HizukeYMD >=  " + SQLHelper.DateTimeToDbDateTime(stratYMD));
            sb.AppendLine("   AND KyujitsuCalendarMeisai.HizukeYMD <=  " + SQLHelper.DateTimeToDbDateTime(endYMD));

            if (driverId != null && driverId.Value.CompareTo(decimal.Zero) != 0)
            {
                sb.AppendLine("   AND KyujitsuCalendar.ToraDONStaffId = " + driverId);
            }

            sb.AppendLine("   AND KyujitsuCalendar.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        /// <summary>
        /// 排他情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GeQuerytExclusiveInfoSelect(HaishaNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");
            sb.AppendLine("  Haisha.HaishaId ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  Haisha ");
            sb.AppendLine("  INNER JOIN Juchu ");
            sb.AppendLine("  ON Haisha.JuchuId = Juchu.JuchuId ");
            sb.AppendLine("  AND Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" LEFT JOIN Point  ");
            sb.AppendLine("  ON Haisha.EndPointId = Point.ToraDONPointId  ");
            sb.AppendLine("  AND Point.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");

            // 表示範囲の日付
            sb.AppendLine("   AND (  ");
            sb.AppendLine("     (  ");
            sb.AppendLine("       Haisha.TaskStartDateTime >= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
            sb.AppendLine("       AND Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
            sb.AppendLine("     )  ");
            sb.AppendLine("     OR (  ");
            sb.AppendLine("       Haisha.ReuseYMD >= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
            sb.AppendLine("       AND Haisha.ReuseYMD <= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
            sb.AppendLine("     ) ");
            sb.AppendLine("     OR (  ");
            sb.AppendLine("       Haisha.TaskStartDateTime <= " + SQLHelper.DateTimeToDbDateTime(para.DispStratYMD));
            sb.AppendLine("       AND Haisha.ReuseYMD >= " + SQLHelper.DateTimeToDbDateTime(para.DispEndYMD));
            sb.AppendLine("     ) ");
            sb.AppendLine("   ) ");

            // 排他用抽出条件
            if (para.HaishaNyuryokuConditionInfo != null)
            {

                // 営業所管理区分
                if (para.HaishaNyuryokuConditionInfo.BranchOfficeId != null
                    && para.HaishaNyuryokuConditionInfo.BranchOfficeId.Value.CompareTo(decimal.Zero) != 0)
                {
                    sb.AppendLine("   AND Juchu.BranchOfficeId = " + para.HaishaNyuryokuConditionInfo.BranchOfficeId);
                }

                // 配車入力条件区分
                switch (para.HaishaNyuryokuConditionInfo.HaishaNyuryokuJokenKbn)
                {
                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:

                        // 得意先
                        sb.AppendLine("   AND Juchu.TokuisakiId IN ( ");
                        sb.AppendLine(" SELECT ");
                        sb.AppendLine("   WorkId  ");
                        sb.AppendLine(" FROM ");
                        sb.AppendLine("   HaishaExclusiveManage  ");
                        sb.AppendLine(" WHERE ");
                        sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                        sb.AppendLine("   ) ");

                        break;
                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:

                        // 方面
                        sb.AppendLine("   AND Juchu.EndPointId IN ( ");
                        sb.AppendLine(" SELECT ");
                        sb.AppendLine("   WorkId  ");
                        sb.AppendLine(" FROM ");
                        sb.AppendLine("   HaishaExclusiveManage  ");
                        sb.AppendLine(" WHERE ");
                        sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                        sb.AppendLine("   ) ");

                        break;

                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:

                        // 車種
                        sb.AppendLine("   AND Haisha.CarKindId IN ( ");
                        sb.AppendLine(" SELECT ");
                        sb.AppendLine("   WorkId  ");
                        sb.AppendLine(" FROM ");
                        sb.AppendLine("   HaishaExclusiveManage  ");
                        sb.AppendLine(" WHERE ");
                        sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

                        sb.AppendLine("   ) ");

                        break;
                    default:
                        break;
                }
            }
            return sb.ToString();
        }

        /// <summary>
        /// チェック用配車情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="juchuId">受注ID</param>
        /// <returns></returns>
        private string GetQueryCheckHsishaSelect(decimal juchuId)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("  HaishaId ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  Haisha ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  JuchuId = " + juchuId);
            sb.AppendLine("  AND DelFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        /// <summary>
        /// チェック用配車情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="juchuId">車両ID</param>
        /// <returns></returns>
        private string GetQueryCheckDisableSelectCount(decimal carId)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("  CarId ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  TORADON_Car ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  CarId = " + carId);
            sb.AppendLine("  AND DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("  AND DisableFlag = " + NSKUtil.BoolToInt(false) + "");

            return sb.ToString();
        }

        /// <summary>
        /// 請求日付を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns></returns>
        private string GetQueryFixDatSelect(HaishaNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT ");

            // 配車情報
            sb.AppendLine("   Haisha.HaishaId ");
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine("   ,TORADON_Sale.ClmFixDate ");
            //sb.AppendLine("   ,TORADON_Sale.CharterPayFixDate  ");
            //sb.AppendLine("   ,TORADON_Sale.AddUpDate ");
            //sb.AppendLine("   ,TORADON_Sale.CharterAddUpDate  ");
            //sb.AppendLine("   ,TORADON_Sale.FixFlag ");
            //sb.AppendLine("   ,TORADON_Sale.CharterFixFlag  ");
            //TODO 性能問題のためコメント化中 END

            sb.AppendLine(" FROM ");
            sb.AppendLine("   Haisha  ");
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage ON ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            //sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   TORADON_Sale ON ");
            //sb.AppendLine("   TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            //sb.AppendLine("   TORADON_Sale.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            //TODO 性能問題のためコメント化中 END

            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine(" AND Haisha.HaishaId IN ("
                 + string.Join(",", para.HaishaIdList.Select(obj => obj.ToString())) + ") ");

            return sb.ToString();
        }

        /// <summary>
        /// 利用者補情報を取得するSqlQueryを返す。
        /// </summary>
        /// <param name="juchuId">車両ID</param>
        /// <returns></returns>
        private string GetQueryOperatorExSelect()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("   PrevTimeInterval ");
            sb.AppendLine("   , PrevDisplayRange ");
            sb.AppendLine("   , PrevCarType ");
            sb.AppendLine("   , PrevCarOfiice ");
            sb.AppendLine("   , PrevHaishaNyuryokuMihainomiFlag  ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   OperatorEx  ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   OperatorId = " + authInfo.OperatorId);

            return sb.ToString();
        }

        /// <summary>
        /// 配車情報を登録するSqlCommandを返す。
        /// </summary>
        /// <returns></returns>
        private string GetCommandHaishaInsert(HaishaNyuryokuInfo info)
        {
            //常設列の取得オプション
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.AdditionColumns;

            // クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" INSERT  ");
            sb.AppendLine(" INTO Haisha (  ");
            sb.AppendLine("   HaishaId ");
            sb.AppendLine("   , JuchuId ");
            sb.AppendLine("   , CarId ");
            sb.AppendLine("   , LicPlateCarNo ");
            sb.AppendLine("   , CarKindId ");
            sb.AppendLine("   , DriverId ");
            sb.AppendLine("   , Number ");
            sb.AppendLine("   , FigId ");
            sb.AppendLine("   , AtPrice ");
            sb.AppendLine("   , Weight ");
            sb.AppendLine("   , Price ");
            sb.AppendLine("   , PriceInPrice ");
            sb.AppendLine("   , TollFeeInPrice ");
            sb.AppendLine("   , PriceOutTaxCalc ");
            sb.AppendLine("   , PriceOutTax ");
            sb.AppendLine("   , PriceInTaxCalc ");
            sb.AppendLine("   , PriceInTax ");
            sb.AppendLine("   , PriceNoTaxCalc ");
            sb.AppendLine("   , TaxDispKbn ");
            sb.AppendLine("   , AddUpYMD ");
            sb.AppendLine("   , FixFlag ");
            sb.AppendLine("   , CarOfChartererId ");
            sb.AppendLine("   , CharterPrice ");
            sb.AppendLine("   , PriceInCharterPrice ");
            sb.AppendLine("   , TollFeeInCharterPrice ");
            sb.AppendLine("   , CharterPriceOutTaxCalc ");
            sb.AppendLine("   , CharterPriceOutTax ");
            sb.AppendLine("   , CharterPriceInTaxCalc ");
            sb.AppendLine("   , CharterPriceInTax ");
            sb.AppendLine("   , CharterPriceNoTaxCalc ");
            sb.AppendLine("   , CharterTaxDispKbn ");
            sb.AppendLine("   , CharterAddUpYMD ");
            sb.AppendLine("   , CharterFixFlag ");
            sb.AppendLine("   , Fee ");
            sb.AppendLine("   , PriceInFee ");
            sb.AppendLine("   , TollFeeInFee ");
            sb.AppendLine("   , FeeOutTaxCalc ");
            sb.AppendLine("   , FeeOutTax ");
            sb.AppendLine("   , FeeInTaxCalc ");
            sb.AppendLine("   , FeeInTax ");
            sb.AppendLine("   , FeeNoTaxCalc ");
            sb.AppendLine("   , StartPointId ");
            sb.AppendLine("   , StartPointNM ");
            sb.AppendLine("   , EndPointId ");
            sb.AppendLine("   , EndPointNM ");
            sb.AppendLine("   , ItemId ");
            sb.AppendLine("   , ItemNM ");
            sb.AppendLine("   , OwnerId ");
            sb.AppendLine("   , OwnerNM ");
            sb.AppendLine("   , StartYMD ");
            sb.AppendLine("   , TaskStartDateTime ");
            sb.AppendLine("   , TaskEndDateTime ");
            sb.AppendLine("   , ReuseYMD ");
            sb.AppendLine("   , MagoYoshasaki ");
            sb.AppendLine("   , Biko ");
            sb.AppendLine("   , DelFlag");
            sb.AppendLine("   , TaikijikanNumber ");
            sb.AppendLine("   , TaikijikanFigId ");
            sb.AppendLine("   , TaikijikanAtPrice ");
            sb.AppendLine("   , TaikijikanryoInPrice ");
            sb.AppendLine("   , NizumiryoInPrice ");
            sb.AppendLine("   , NioroshiryoInPrice ");
            sb.AppendLine("   , FutaigyomuryoInPrice ");
            sb.AppendLine("   , TaikijikanryoInFee ");
            sb.AppendLine("   , NizumiryoInFee ");
            sb.AppendLine("   , NioroshiryoInFee ");
            sb.AppendLine("   , FutaigyomuryoInFee ");
            sb.AppendLine("   , NizumijikanNumber ");
            sb.AppendLine("   , NizumijikanFigId ");
            sb.AppendLine("   , NizumijikanAtPrice ");
            sb.AppendLine("   , NioroshijikanNumber ");
            sb.AppendLine("   , NioroshijikanFigId ");
            sb.AppendLine("   , NioroshijikanAtPrice ");
            sb.AppendLine("   , FutaigyomujikanNumber ");
            sb.AppendLine("   , FutaigyomujikanFigId ");
            sb.AppendLine("   , FutaigyomujikanAtPrice ");
            sb.AppendLine("   , JomuinUriageDogakuFlag ");
            sb.AppendLine("   , JomuinUriageKingaku ");
            sb.AppendLine("   , " + SQLHelper.GetPopulateColumnSelectionString(popOption));
            sb.AppendLine(" )  ");
            sb.AppendLine(" VALUES (  ");
            sb.AppendLine("   " + this.dbNullableString(info.HaishaId));
            sb.AppendLine("   , " + this.dbNullableString(info.JuchuId));
            sb.AppendLine("   , " + this.dbNullableString(info.CarId));
            sb.AppendLine("   , '" + info.LicPlateCarNo + "'");
            sb.AppendLine("   , " + this.dbNullableString(info.CarKindId));
            sb.AppendLine("   , " + this.dbNullableString(info.DriverId));
            sb.AppendLine("   , " + this.dbNullableString(info.Number));
            sb.AppendLine("   , " + this.dbNullableString(info.FigId));
            sb.AppendLine("   , " + this.dbNullableString(info.AtPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.Weight));
            sb.AppendLine("   , " + this.dbNullableString(info.Price));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceInPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.TollFeeInPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceOutTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceOutTax));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceInTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceInTax));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceNoTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.TaxDispKbn));
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.AddUpYMD));
            sb.AppendLine("   , " + NSKUtil.BoolToInt(info.FixFlag) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.CarOfChartererId));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceInCharterPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.TollFeeInCharterPrice));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPriceOutTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPriceOutTax));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPriceInTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPriceInTax));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterPriceNoTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.CharterTaxDispKbn));
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.CharterAddUpYMD));
            sb.AppendLine("   , " + NSKUtil.BoolToInt(info.CharterFixFlag) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.Fee));
            sb.AppendLine("   , " + this.dbNullableString(info.PriceInFee));
            sb.AppendLine("   , " + this.dbNullableString(info.TollFeeInFee));
            sb.AppendLine("   , " + this.dbNullableString(info.FeeOutTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.FeeOutTax));
            sb.AppendLine("   , " + this.dbNullableString(info.FeeInTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.FeeInTax));
            sb.AppendLine("   , " + this.dbNullableString(info.FeeNoTaxCalc));
            sb.AppendLine("   , " + this.dbNullableString(info.StartPointId));
            sb.AppendLine("   , '" + info.StartPointName + "'");
            sb.AppendLine("   , " + this.dbNullableString(info.EndPointId));
            sb.AppendLine("   , '" + info.EndPointName + "'");
            sb.AppendLine("   , " + this.dbNullableString(info.ItemId));
            sb.AppendLine("   , '" + info.ItemName + "'");
            sb.AppendLine("   , " + this.dbNullableString(info.OwnerId));
            sb.AppendLine("   , '" + info.OwnerName + "'");
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.StartYMD));
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.TaskStartDateTime));
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.TaskEndDateTime));
            sb.AppendLine("   , " + SQLHelper.DateTimeToDbDateTime(info.ReuseYMD));
            sb.AppendLine("   , '" + info.MagoYoshasaki + "'");
            sb.AppendLine("   , '" + info.Biko + "'");
            sb.AppendLine("   , " + NSKUtil.BoolToInt(false) + "");
            sb.AppendLine("   , " + this.dbNullableString(info.TaikijikanNumber) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.TaikijikanFigId) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.TaikijikanAtPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.TaikijikanryoInPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NizumiryoInPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NioroshiryoInPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.FutaigyomuryoInPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.TaikijikanryoInFee) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NizumiryoInFee) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NioroshiryoInFee) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.FutaigyomuryoInFee) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NizumijikanNumber) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NizumijikanFigId) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NizumijikanAtPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NioroshijikanNumber) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NioroshijikanFigId) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.NioroshijikanAtPrice) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.FutaigyomujikanNumber) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.FutaigyomujikanFigId) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.FutaigyomujikanAtPrice) + " ");
            sb.AppendLine("   , " + NSKUtil.BoolToInt(info.JomuinUriageDogakuFlag) + " ");
            sb.AppendLine("   , " + this.dbNullableString(info.JomuinUriageKingaku) + " ");
            sb.AppendLine("   , " + SQLHelper.GetPopulateColumnInsertString(this.authInfo, popOption));
            sb.AppendLine(" )");

            return sb.ToString();
        }

        /// <summary>
        /// 配車情報を更新するSqlCommandを返す。
        /// </summary>
        /// <returns></returns>
        private SqlCommand GetCommandHaishaUpdate(HaishaNyuryokuInfo info)
        {
            //常設列の取得オプション
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            // クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" UPDATE Haisha  ");
            sb.AppendLine(" SET ");
            sb.AppendLine("   JuchuId = " + this.dbNullableString(info.JuchuId));
            sb.AppendLine("   , CarId = " + this.dbNullableString(info.CarId));
            sb.AppendLine("   , LicPlateCarNo = '" + this.dbNullableString(info.LicPlateCarNo) + "'");
            sb.AppendLine("   , CarKindId = " + this.dbNullableString(info.CarKindId));
            sb.AppendLine("   , DriverId = " + this.dbNullableString(info.DriverId));
            sb.AppendLine("   , Number = " + this.dbNullableString(info.Number));
            sb.AppendLine("   , FigId = " + this.dbNullableString(info.FigId));
            sb.AppendLine("   , AtPrice = " + this.dbNullableString(info.AtPrice));
            sb.AppendLine("   , Weight = " + this.dbNullableString(info.Weight));
            sb.AppendLine("   , Price = " + this.dbNullableString(info.Price));
            sb.AppendLine("   , PriceInPrice = " + this.dbNullableString(info.PriceInPrice));
            sb.AppendLine("   , TollFeeInPrice = " + this.dbNullableString(info.TollFeeInPrice));
            sb.AppendLine("   , PriceOutTaxCalc = " + this.dbNullableString(info.PriceOutTaxCalc));
            sb.AppendLine("   , PriceOutTax = " + this.dbNullableString(info.PriceOutTax));
            sb.AppendLine("   , PriceInTaxCalc = " + this.dbNullableString(info.PriceInTaxCalc));
            sb.AppendLine("   , PriceInTax = " + this.dbNullableString(info.PriceInTax));
            sb.AppendLine("   , PriceNoTaxCalc = " + this.dbNullableString(info.PriceNoTaxCalc));
            sb.AppendLine("   , TaxDispKbn = " + this.dbNullableString(info.TaxDispKbn));
            sb.AppendLine("   , AddUpYMD = " + SQLHelper.DateTimeToDbDateTime(info.AddUpYMD));
            sb.AppendLine("   , FixFlag = " + NSKUtil.BoolToInt(info.FixFlag));
            sb.AppendLine("   , CarOfChartererId = " + this.dbNullableString(info.CarOfChartererId));
            sb.AppendLine("   , CharterPrice = " + this.dbNullableString(info.CharterPrice));
            sb.AppendLine("   , PriceInCharterPrice = " + this.dbNullableString(info.PriceInCharterPrice));
            sb.AppendLine("   , TollFeeInCharterPrice = " + this.dbNullableString(info.TollFeeInCharterPrice));
            sb.AppendLine("   , CharterPriceOutTaxCalc = " + this.dbNullableString(info.CharterPriceOutTaxCalc));
            sb.AppendLine("   , CharterPriceOutTax = " + this.dbNullableString(info.CharterPriceOutTax));
            sb.AppendLine("   , CharterPriceInTaxCalc = " + this.dbNullableString(info.CharterPriceInTaxCalc));
            sb.AppendLine("   , CharterPriceInTax = " + this.dbNullableString(info.CharterPriceInTax));
            sb.AppendLine("   , CharterPriceNoTaxCalc = " + this.dbNullableString(info.CharterPriceNoTaxCalc));
            sb.AppendLine("   , CharterTaxDispKbn = " + this.dbNullableString(info.CharterTaxDispKbn));
            sb.AppendLine("   , CharterAddUpYMD = " + SQLHelper.DateTimeToDbDateTime(info.CharterAddUpYMD));
            sb.AppendLine("   , CharterFixFlag = " + NSKUtil.BoolToInt(info.CharterFixFlag));
            sb.AppendLine("   , Fee = " + this.dbNullableString(info.Fee));
            sb.AppendLine("   , PriceInFee = " + this.dbNullableString(info.PriceInFee));
            sb.AppendLine("   , TollFeeInFee = " + this.dbNullableString(info.TollFeeInFee));
            sb.AppendLine("   , FeeOutTaxCalc = " + this.dbNullableString(info.FeeOutTaxCalc));
            sb.AppendLine("   , FeeOutTax = " + this.dbNullableString(info.FeeOutTax));
            sb.AppendLine("   , FeeInTaxCalc = " + this.dbNullableString(info.FeeInTaxCalc));
            sb.AppendLine("   , FeeInTax = " + this.dbNullableString(info.FeeInTax));
            sb.AppendLine("   , FeeNoTaxCalc = " + this.dbNullableString(info.FeeNoTaxCalc));
            sb.AppendLine("   , StartPointId = " + this.dbNullableString(info.StartPointId));
            sb.AppendLine("   , StartPointNM = '" + info.StartPointName + "'");
            sb.AppendLine("   , EndPointId = " + this.dbNullableString(info.EndPointId));
            sb.AppendLine("   , EndPointNM = '" + info.EndPointName + "'");
            sb.AppendLine("   , ItemId = " + this.dbNullableString(info.ItemId));
            sb.AppendLine("   , ItemNM = '" + info.ItemName + "'");
            sb.AppendLine("   , OwnerId = " + this.dbNullableString(info.OwnerId));
            sb.AppendLine("   , OwnerNM = '" + info.OwnerName + "'");
            sb.AppendLine("   , StartYMD = " + SQLHelper.DateTimeToDbDateTime(info.StartYMD));
            sb.AppendLine("   , TaskStartDateTime = " + SQLHelper.DateTimeToDbDateTime(info.TaskStartDateTime));
            sb.AppendLine("   , TaskEndDateTime = " + SQLHelper.DateTimeToDbDateTime(info.TaskEndDateTime));
            sb.AppendLine("   , ReuseYMD = " + SQLHelper.DateTimeToDbDateTime(info.ReuseYMD));
            sb.AppendLine("   , MagoYoshasaki = '" + info.MagoYoshasaki + "'");
            sb.AppendLine("   , Biko = '" + info.Biko + "'");
            sb.AppendLine("   , TaikijikanNumber = " + info.TaikijikanNumber.ToString());
            sb.AppendLine("   , TaikijikanFigId = " + info.TaikijikanFigId.ToString());
            sb.AppendLine("   , TaikijikanAtPrice = " + info.TaikijikanAtPrice.ToString());
            sb.AppendLine("   , TaikijikanryoInPrice = " + info.TaikijikanryoInPrice.ToString());
            sb.AppendLine("   , NizumiryoInPrice = " + info.NizumiryoInPrice.ToString());
            sb.AppendLine("   , NioroshiryoInPrice = " + info.NioroshiryoInPrice.ToString());
            sb.AppendLine("   , FutaigyomuryoInPrice = " + info.FutaigyomuryoInPrice.ToString());
            sb.AppendLine("   , TaikijikanryoInFee = " + info.TaikijikanryoInFee.ToString());
            sb.AppendLine("   , NizumiryoInFee = " + info.NizumiryoInFee.ToString());
            sb.AppendLine("   , NioroshiryoInFee = " + info.NioroshiryoInFee.ToString());
            sb.AppendLine("   , FutaigyomuryoInFee = " + info.FutaigyomuryoInFee.ToString());
            sb.AppendLine("   , NizumijikanNumber = " + info.NizumijikanNumber.ToString());
            sb.AppendLine("   , NizumijikanFigId = " + info.NizumijikanFigId.ToString());
            sb.AppendLine("   , NizumijikanAtPrice = " + info.NizumijikanAtPrice.ToString());
            sb.AppendLine("   , NioroshijikanNumber = " + info.NioroshijikanNumber.ToString());
            sb.AppendLine("   , NioroshijikanFigId = " + info.NioroshijikanFigId.ToString());
            sb.AppendLine("   , NioroshijikanAtPrice = " + info.NioroshijikanAtPrice.ToString());
            sb.AppendLine("   , FutaigyomujikanNumber = " + info.FutaigyomujikanNumber.ToString());
            sb.AppendLine("   , FutaigyomujikanFigId = " + info.FutaigyomujikanFigId.ToString());
            sb.AppendLine("   , FutaigyomujikanAtPrice = " + info.FutaigyomujikanAtPrice.ToString());
            sb.AppendLine("   , JomuinUriageDogakuFlag = " + NSKUtil.BoolToInt(info.JomuinUriageDogakuFlag));
            sb.AppendLine("   , JomuinUriageKingaku = " + info.JomuinUriageKingaku.ToString());
            sb.AppendLine("   , " + SQLHelper.GetPopulateColumnUpdateString(authInfo, popOption));
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   HaishaId = " + info.HaishaId.ToString());
            sb.AppendLine("  AND DelFlag = " + NSKUtil.BoolToInt(false).ToString() + "");
            sb.AppendLine("  AND VersionColumn = @versionColumn");

            // パラメータ設定
            SqlCommand command = new SqlCommand(sb.ToString());
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            return command;
        }

        /// <summary>
        /// 配車情報を削除するSqlCommandを返す。
        /// </summary>
        /// <returns></returns>
        private SqlCommand GetCommandHaishaDelete(HaishaNyuryokuInfo info)
        {
            //常設列の取得オプション
            SQLHelper.PopulateColumnOptions popOption =
                SQLHelper.PopulateColumnOptions.EntryColumns |
                    SQLHelper.PopulateColumnOptions.UpdateColumns;

            // クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("UPDATE");
            sb.AppendLine("  Haisha");
            sb.AppendLine("SET");
            sb.AppendLine("	   DelFlag = " + NSKUtil.BoolToInt(true) + "");
            sb.AppendLine("	 , " + SQLHelper.GetPopulateColumnUpdateString(authInfo, popOption) + "");
            sb.AppendLine("WHERE");
            sb.AppendLine("      HaishaId = " + info.HaishaId.ToString() + "");
            sb.AppendLine("  AND VersionColumn = @versionColumn");

            // パラメータ設定
            SqlCommand command = new SqlCommand(sb.ToString());
            command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

            return command;
        }

        /// <summary>
        /// 配車情報を削除するSqlCommandを返す。
        /// </summary>
        /// <returns></returns>
        private SqlCommand GetCommandHaishaaExclusiveManageDelete()
        {
            // クエリ作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" DELETE FROM HaishaExclusiveManage WHERE OperatorId = " + authInfo.OperatorId);

            // パラメータ設定
            SqlCommand command = new SqlCommand(sb.ToString());

            return command;
        }

        /// <summary>
        /// 利用者補をマージするSqlCommandを返す。
        /// </summary>
        /// <returns></returns>
        private SqlCommand GetMergeOperatorExUpdate(HaishaNyuryokuSearchParameter para = null)
        {

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" MERGE  ");
            sb.AppendLine(" INTO OperatorEx  ");
            sb.AppendLine("   USING (SELECT 1 Id) source  ");
            sb.AppendLine("     ON (OperatorEx.OperatorId = " + authInfo.OperatorId + ") WHEN MATCHED THEN UPDATE  ");
            sb.AppendLine(" SET ");
            sb.AppendLine("   PrevTimeInterval = " + this.dbNullableString(para.PrevTimeInterval));
            sb.AppendLine("   , PrevDisplayRange = " + this.dbNullableString(para.PrevDisplayRange));
            sb.AppendLine("   , PrevCarType = " + this.dbNullableString(para.PrevCarType));
            sb.AppendLine("   , PrevCarOfiice = " + this.dbNullableString(para.PrevCarOfiice));
            sb.AppendLine("   , PrevHaishaNyuryokuMihainomiFlag = " + NSKUtil.BoolToInt(para.PrevHaishaNyuryokuMihainomiFlag));
            sb.AppendLine("   , EntryOperatorId = " + authInfo.OperatorId);
            sb.AppendLine("   , EntryProcessId = '" + authInfo.UserProcessId + "'");
            sb.AppendLine("   , EntryTerminalId = '" + authInfo.TerminalId + "'");
            sb.AppendLine("   , EntryDateTime = GETDATE() ");
            sb.AppendLine("   , UpdateOperatorId = " + authInfo.OperatorId);
            sb.AppendLine("   , UpdateProcessId = '" + authInfo.UserProcessId + "'");
            sb.AppendLine("   , UpdateTerminalId = '" + authInfo.TerminalId + "'");
            sb.AppendLine("   , UpdateDateTime = GETDATE() WHEN NOT MATCHED THEN  ");
            sb.AppendLine(" INSERT (  ");
            sb.AppendLine("   OperatorId ");
            sb.AppendLine("   , PrevTimeInterval ");
            sb.AppendLine("   , PrevDisplayRange ");
            sb.AppendLine("   , PrevCarType ");
            sb.AppendLine("   , PrevCarOfiice ");
            sb.AppendLine("   , PrevHaishaNyuryokuMihainomiFlag ");
            sb.AppendLine("   , EntryOperatorId ");
            sb.AppendLine("   , EntryProcessId ");
            sb.AppendLine("   , EntryTerminalId ");
            sb.AppendLine("   , EntryDateTime ");
            sb.AppendLine("   , AddOperatorId ");
            sb.AppendLine("   , AddProcessId ");
            sb.AppendLine("   , AddTerminalId ");
            sb.AppendLine("   , AddDateTime ");
            sb.AppendLine("   , UpdateOperatorId ");
            sb.AppendLine("   , UpdateProcessId ");
            sb.AppendLine("   , UpdateTerminalId ");
            sb.AppendLine("   , UpdateDateTime ");
            sb.AppendLine(" )  ");
            sb.AppendLine(" VALUES (  ");
            sb.AppendLine("   " + authInfo.OperatorId);
            sb.AppendLine("   , " + this.dbNullableString(para.PrevTimeInterval));
            sb.AppendLine("   , " + this.dbNullableString(para.PrevDisplayRange));
            sb.AppendLine("   , " + this.dbNullableString(para.PrevCarType));
            sb.AppendLine("   , " + this.dbNullableString(para.PrevCarOfiice));
            sb.AppendLine("   , " + NSKUtil.BoolToInt(para.PrevHaishaNyuryokuMihainomiFlag));
            sb.AppendLine("   , " + authInfo.OperatorId);
            sb.AppendLine("   , '" + authInfo.UserProcessId + "'");
            sb.AppendLine("   , '" + authInfo.TerminalId + "'");
            sb.AppendLine("   , GETDATE() ");
            sb.AppendLine("   , " + authInfo.OperatorId);
            sb.AppendLine("   , '" + authInfo.UserProcessId + "'");
            sb.AppendLine("   , '" + authInfo.TerminalId + "'");
            sb.AppendLine("   , GETDATE() ");
            sb.AppendLine("   , NULL ");
            sb.AppendLine("   , NULL ");
            sb.AppendLine("   , NULL ");
            sb.AppendLine("   , NULL ");
            sb.AppendLine(" ); ");

            // パラメータ設定
            SqlCommand command = new SqlCommand(sb.ToString());

            return command;
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
        /// intからBool型に変換します。
        /// </summary>
        /// <param name="val"></param>
        /// <returns></returns>
        private bool intFromBool(int val)
        {
            if (val == 0) return false;

            return true;
        }
        #endregion
    }
}
