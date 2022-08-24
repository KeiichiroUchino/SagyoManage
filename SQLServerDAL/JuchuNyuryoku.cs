using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using jp.co.jpsys.util;
using jp.co.jpsys.util.db;
using System.Data.SqlClient;
using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.ComLib;


namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 受注入力のデータアクセスレイヤです。
    /// </summary>
    public class JuchuNyuryoku
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
        /// 本クラスのデフォルトコンストラクタです。
        /// </summary>
        public JuchuNyuryoku()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public JuchuNyuryoku(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 受注IDを指定して、受注情報を取得します。
        /// </summary>
        /// <returns>受注情報</returns>
        public List<JuchuNyuryokuInfo> GetJuchuInfoList(JuchuNyuryokuSearchParameter para = null)
        {
            //返却用のリスト
            List<JuchuNyuryokuInfo> rt_list = new List<JuchuNyuryokuInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetSelectJuchuSQL(para));

            String mySql = sb.ToString();

            List<JuchuNyuryokuInfo> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                JuchuNyuryokuInfo rt_info = new JuchuNyuryokuInfo();

                rt_info.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                rt_info.DailyReportId = SQLServerUtil.dbDecimal(rdr["DailyReportId"]);
                rt_info.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["JuchuSlipNo"]);
                rt_info.BranchOfficeId = SQLServerUtil.dbDecimal(rdr["BranchOfficeId"]);
                rt_info.CarId = SQLServerUtil.dbDecimal(rdr["CarId"]);
                rt_info.LicPlateCarNo = (rdr["LicPlateCarNo"]).ToString();
                rt_info.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                rt_info.CarKindId = SQLServerUtil.dbDecimal(rdr["CarKindId"]);
                rt_info.DriverId = SQLServerUtil.dbDecimal(rdr["DriverId"]);
                rt_info.TokuisakiId = SQLServerUtil.dbDecimal(rdr["TokuisakiId"]);
                rt_info.TokuisakiNM = (rdr["TokuisakiNM"]).ToString();
                rt_info.MemoAccount = SQLServerUtil.dbInt(rdr["MemoAccount"]);
                rt_info.ClmClassUseKbn = SQLServerUtil.dbInt(rdr["ClmClassUseKbn"]);
                rt_info.ClmClassId = SQLServerUtil.dbDecimal(rdr["ClmClassId"]);
                rt_info.ContractId = SQLServerUtil.dbDecimal(rdr["ContractId"]);
                rt_info.StartPointId = SQLServerUtil.dbDecimal(rdr["StartPointId"]);
                rt_info.StartPointNM = (rdr["StartPointNM"]).ToString();
                rt_info.EndPointId = SQLServerUtil.dbDecimal(rdr["EndPointId"]);
                rt_info.EndPointNM = (rdr["EndPointNM"]).ToString();
                rt_info.ItemId = SQLServerUtil.dbDecimal(rdr["ItemId"]);
                rt_info.ItemNM = (rdr["ItemNM"]).ToString();
                rt_info.OwnerId = SQLServerUtil.dbDecimal(rdr["OwnerId"]);
                rt_info.OwnerNM = (rdr["OwnerNM"]).ToString();
                rt_info.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                rt_info.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.FigId = SQLServerUtil.dbDecimal(rdr["FigId"]);
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Price = SQLServerUtil.dbDecimal(rdr["Price"]);
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                rt_info.PriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceOutTaxCalc"]);
                rt_info.PriceOutTax = SQLServerUtil.dbDecimal(rdr["PriceOutTax"]);
                rt_info.PriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceInTaxCalc"]);
                rt_info.PriceInTax = SQLServerUtil.dbDecimal(rdr["PriceInTax"]);
                rt_info.PriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["PriceNoTaxCalc"]);
                rt_info.TaxDispKbn = SQLServerUtil.dbInt(rdr["TaxDispKbn"]);
                rt_info.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                rt_info.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                rt_info.Memo = (rdr["Memo"]).ToString();
                rt_info.ClmFixYMD = SQLHelper.dbDate(rdr["ClmFixYMD"]);
                rt_info.CarOfChartererId = SQLServerUtil.dbDecimal(rdr["CarOfChartererId"]);
                rt_info.CharterPrice = SQLServerUtil.dbDecimal(rdr["CharterPrice"]);
                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                rt_info.CharterPriceOutTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTaxCalc"]);
                rt_info.CharterPriceOutTax = SQLServerUtil.dbDecimal(rdr["CharterPriceOutTax"]);
                rt_info.CharterPriceInTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceInTaxCalc"]);
                rt_info.CharterPriceInTax = SQLServerUtil.dbDecimal(rdr["CharterPriceInTax"]);
                rt_info.CharterPriceNoTaxCalc = SQLServerUtil.dbDecimal(rdr["CharterPriceNoTaxCalc"]);
                rt_info.CharterTaxDispKbn = SQLServerUtil.dbInt(rdr["CharterTaxDispKbn"]);
                rt_info.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                rt_info.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                rt_info.CharterPayFixYMD = SQLHelper.dbDate(rdr["CharterPayFixYMD"]);
                rt_info.Fee = SQLServerUtil.dbDecimal(rdr["Fee"]);
                rt_info.PriceInFee = SQLServerUtil.dbDecimal(rdr["PriceInFee"]);
                rt_info.TollFeeInFee = SQLServerUtil.dbDecimal(rdr["TollFeeInFee"]);
                rt_info.FeeOutTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeOutTaxCalc"]);
                rt_info.FeeOutTax = SQLServerUtil.dbDecimal(rdr["FeeOutTax"]);
                rt_info.FeeInTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeInTaxCalc"]);
                rt_info.FeeInTax = SQLServerUtil.dbDecimal(rdr["FeeInTax"]);
                rt_info.FeeNoTaxCalc = SQLServerUtil.dbDecimal(rdr["FeeNoTaxCalc"]);
                rt_info.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                rt_info.HanroId = SQLServerUtil.dbDecimal(rdr["HanroId"]);
                rt_info.ReceivedFlag = SQLHelper.dbBit(rdr["ReceivedFlag"]);
                rt_info.DelFlag = SQLHelper.dbBit(rdr["DelFlag"]);
                rt_info.TaikijikanNumber = SQLServerUtil.dbDecimal(rdr["TaikijikanNumber"]);
                rt_info.TaikijikanFigId = SQLServerUtil.dbDecimal(rdr["TaikijikanFigId"]);
                rt_info.TaikijikanAtPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanAtPrice"]);
                rt_info.TaikijikanryoInPrice = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInPrice"]);
                rt_info.NizumiryoInPrice = SQLServerUtil.dbDecimal(rdr["NizumiryoInPrice"]);
                rt_info.NioroshiryoInPrice = SQLServerUtil.dbDecimal(rdr["NioroshiryoInPrice"]);
                rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                rt_info.TaikijikanryoInFee = SQLServerUtil.dbDecimal(rdr["TaikijikanryoInFee"]);
                rt_info.NizumiryoInFee = SQLServerUtil.dbDecimal(rdr["NizumiryoInFee"]);
                rt_info.NioroshiryoInFee = SQLServerUtil.dbDecimal(rdr["NioroshiryoInFee"]);
                rt_info.FutaigyomuryoInFee = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInFee"]);
                rt_info.NizumijikanNumber = SQLServerUtil.dbDecimal(rdr["NizumijikanNumber"]);
                rt_info.NizumijikanFigId = SQLServerUtil.dbDecimal(rdr["NizumijikanFigId"]);
                rt_info.NizumijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NizumijikanAtPrice"]);
                rt_info.NioroshijikanNumber = SQLServerUtil.dbDecimal(rdr["NioroshijikanNumber"]);
                rt_info.NioroshijikanFigId = SQLServerUtil.dbDecimal(rdr["NioroshijikanFigId"]);
                rt_info.NioroshijikanAtPrice = SQLServerUtil.dbDecimal(rdr["NioroshijikanAtPrice"]);
                rt_info.FutaigyomujikanNumber = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanNumber"]);
                rt_info.FutaigyomujikanFigId = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanFigId"]);
                rt_info.FutaigyomujikanAtPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomujikanAtPrice"]);
                rt_info.JomuinUriageDogakuFlag = SQLHelper.dbBit(rdr["JomuinUriageDogakuFlag"]);
                rt_info.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);
                rt_info.BranchOfficeCd = SQLServerUtil.dbInt(rdr["BranchOfficeCd"]);
                rt_info.TokuisakiCd = SQLServerUtil.dbInt(rdr["TokuisakiCd"]);
                rt_info.ClmClassCd = SQLServerUtil.dbInt(rdr["ClmClassCd"]);
                rt_info.ClmClassSNM = (rdr["ClmClassSNM"]).ToString();
                rt_info.ContractCd = SQLServerUtil.dbInt(rdr["ContractCd"]);
                rt_info.ContractNm = (rdr["ContractNm"]).ToString();
                rt_info.ItemCd = SQLServerUtil.dbInt(rdr["ItemCd"]);
                rt_info.StartPointCd = SQLServerUtil.dbInt(rdr["StartPointCd"]);
                rt_info.EndPointCd = SQLServerUtil.dbInt(rdr["EndPointCd"]);
                rt_info.FigCd = SQLServerUtil.dbInt(rdr["FigCd"]);
                rt_info.FigNm = (rdr["FigNm"]).ToString();
                rt_info.CarCd = SQLServerUtil.dbInt(rdr["CarCd"]);
                rt_info.CarBranchOfficeSNM = (rdr["CarBranchOfficeSNM"]).ToString();
                rt_info.CarKbn = SQLServerUtil.dbInt(rdr["CarKbn"]);
                rt_info.CarKindCd = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                rt_info.CarKindSNM = (rdr["CarKindSNM"]).ToString();
                rt_info.DriverCd = SQLServerUtil.dbInt(rdr["DriverCd"]);
                rt_info.DriverNm = (rdr["DriverNm"]).ToString();
                rt_info.CarOfChartererCd = SQLServerUtil.dbInt(rdr["CarOfChartererCd"]);
                rt_info.CarOfChartererShortNm = (rdr["CarOfChartererShortNm"]).ToString();
                rt_info.OwnerCd = SQLServerUtil.dbInt(rdr["OwnerCd"]);
                rt_info.HanroCd = SQLServerUtil.dbInt(rdr["HanroCd"]);
                rt_info.HanroNm = rdr["HanroNm"].ToString();
                rt_info.JuchuTantoId = SQLServerUtil.dbDecimal(rdr["JuchuTantoId"]);
                rt_info.JuchuTantoCd = SQLServerUtil.dbInt(rdr["JuchTantoCd"]);
                rt_info.JuchuTantoNm = rdr["JuchTantoNm"].ToString();
                rt_info.OfukuKbn = SQLServerUtil.dbInt(rdr["OfukuKbn"]);
                rt_info.OfukuKbnNm = rdr["OfukuKbnNm"].ToString();
                rt_info.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                rt_info.MagoYoshasaki = (rdr["MagoYoshasaki"]).ToString();
                rt_info.ReceivedFlag = SQLHelper.dbBit(rdr["ReceivedFlag"]);
                //TODO 性能問題のためコメント化中 START
                //rt_info.MinClmFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["MinClmFixDate"]));
                //rt_info.MinCharterPayFixDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["MinCharterPayFixDate"]));
                //rt_info.MinAddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["MinAddUpDate"]));
                //rt_info.MinCharterAddUpDate =
                //        NSKUtil.DecimalWithTimeToDateTime(
                //            SQLServerUtil.dbDecimal(rdr["MinCharterAddUpDate"]));
                //rt_info.MaxFixFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["MaxFixFlag"]));
                //rt_info.MaxCharterFixFlag = NSKUtil.IntToBool(SQLServerUtil.dbInt(rdr["MaxCharterFixFlag"]));
                //TODO 性能問題のためコメント化中 END

                //入力者以下の常設フィールドをセットする
                rt_info = SQLHelper.GetTimeStampModelBaseValues(rt_info, rdr);

                //返却用の値を返します
                return rt_info;
            }, null);

            if (list == null || list.Count == 0)
            {
                return rt_list;
            }

            var wk_list = list.ToList();

            foreach (JuchuNyuryokuInfo info in wk_list)
            {
                JuchuNyuryokuInfo prtInfo = new JuchuNyuryokuInfo();
                prtInfo = info;
                rt_list.Add(prtInfo);
            }

            return rt_list.ToList();
        }

        /// <summary>
        /// SqlTransaction情報、受注情報リストを指定して、
        /// 受注情報の更新を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">受注情報リスト</param>
        public void Save(SqlTransaction transaction, List<JuchuNyuryokuInfo> InfoList)
        {
            foreach (var info in InfoList)
            {
                //明細情報がデータベースに保存されているかどうか
                if (info.IsPersisted)
                {
                    //--常設列の取得オプションを作る
                    SQLHelper.PopulateColumnOptions popOption =
                        SQLHelper.PopulateColumnOptions.EntryColumns |
                            SQLHelper.PopulateColumnOptions.UpdateColumns;

                    //Update文を作成
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine("UPDATE Juchu ");
                    sb.AppendLine("SET ");

                    sb.AppendLine("  BranchOfficeId = " + this.dbNullableString(info.BranchOfficeId) + " , ");
                    sb.AppendLine("  CarId = " + this.dbNullableString(info.CarId) + " , ");
                    sb.AppendLine("  LicPlateCarNo = '" + info.LicPlateCarNo.ToString() + "' , ");
                    sb.AppendLine("  CarKindId = " + this.dbNullableString(info.CarKindId) + " , ");
                    sb.AppendLine("  DriverId = " + this.dbNullableString(info.DriverId) + " , ");
                    sb.AppendLine("  TokuisakiId = " + this.dbNullableString(info.TokuisakiId) + " , ");
                    sb.AppendLine("  TokuisakiNM = '" + info.TokuisakiNM + "' , ");
                    sb.AppendLine("  ClmClassId = " + this.dbNullableString(info.ClmClassId) + " , ");
                    sb.AppendLine("  ContractId = " + this.dbNullableString(info.ContractId) + " , ");
                    sb.AppendLine("  StartPointId = " + this.dbNullableString(info.StartPointId) + " , ");
                    sb.AppendLine("  StartPointNM = '" + info.StartPointNM + "' , ");
                    sb.AppendLine("  EndPointId = " + this.dbNullableString(info.EndPointId) + " , ");
                    sb.AppendLine("  EndPointNM = '" + info.EndPointNM + "' , ");
                    sb.AppendLine("  ItemId = " + this.dbNullableString(info.ItemId) + " , ");
                    sb.AppendLine("  ItemNM = '" + info.ItemNM + "' , ");
                    sb.AppendLine("  OwnerId = " + this.dbNullableString(info.OwnerId) + " , ");
                    sb.AppendLine("  OwnerNM = '" + info.OwnerNM + "' , ");
                    sb.AppendLine("  TaskStartDateTime = " + SQLHelper.DateTimeToDbDateTime(info.TaskStartDateTime) + " , ");
                    sb.AppendLine("  TaskEndDateTime = " + SQLHelper.DateTimeToDbDateTime(info.TaskEndDateTime) + " , ");
                    sb.AppendLine("  Number = " + this.dbNullableString(info.Number) + " , ");
                    sb.AppendLine("  FigId = " + this.dbNullableString(info.FigId) + " , ");
                    sb.AppendLine("  AtPrice = " + this.dbNullableString(info.AtPrice) + " , ");
                    sb.AppendLine("  Price = " + this.dbNullableString(info.Price) + " , ");
                    sb.AppendLine("  PriceInPrice = " + this.dbNullableString(info.PriceInPrice) + " , ");
                    sb.AppendLine("  TollFeeInPrice = " + this.dbNullableString(info.TollFeeInPrice) + " , ");
                    sb.AppendLine("  PriceOutTaxCalc = " + this.dbNullableString(info.PriceOutTaxCalc) + " , ");
                    sb.AppendLine("  PriceOutTax = " + this.dbNullableString(info.PriceOutTax) + " , ");
                    sb.AppendLine("  PriceInTaxCalc = " + this.dbNullableString(info.PriceInTaxCalc) + " , ");
                    sb.AppendLine("  PriceInTax = " + this.dbNullableString(info.PriceInTax) + " , ");
                    sb.AppendLine("  PriceNoTaxCalc = " + this.dbNullableString(info.PriceNoTaxCalc) + " , ");
                    sb.AppendLine("  TaxDispKbn = " + this.dbNullableString(info.TaxDispKbn) + " , ");
                    sb.AppendLine("  AddUpYMD = " + SQLHelper.DateTimeToDbDateTime(info.AddUpYMD) + " , ");
                    sb.AppendLine("  FixFlag = " + NSKUtil.BoolToInt(info.FixFlag) + " , ");
                    sb.AppendLine("  Memo = '" + info.Memo + "' , ");
                    sb.AppendLine("  ClmFixYMD = " + SQLHelper.DateTimeToDbDateTime(info.ClmFixYMD) + " , ");
                    sb.AppendLine("  CarOfChartererId = " + this.dbNullableString(info.CarOfChartererId) + " , ");
                    sb.AppendLine("  CharterPrice = " + this.dbNullableString(info.CharterPrice) + " , ");
                    sb.AppendLine("  PriceInCharterPrice = " + this.dbNullableString(info.PriceInCharterPrice) + " , ");
                    sb.AppendLine("  TollFeeInCharterPrice = " + this.dbNullableString(info.TollFeeInCharterPrice) + " , ");
                    sb.AppendLine("  CharterPriceOutTaxCalc = " + this.dbNullableString(info.CharterPriceOutTaxCalc) + " , ");
                    sb.AppendLine("  CharterPriceOutTax = " + this.dbNullableString(info.CharterPriceOutTax) + " , ");
                    sb.AppendLine("  CharterPriceInTaxCalc = " + this.dbNullableString(info.CharterPriceInTaxCalc) + " , ");
                    sb.AppendLine("  CharterPriceInTax = " + this.dbNullableString(info.CharterPriceInTax) + " , ");
                    sb.AppendLine("  CharterPriceNoTaxCalc = " + this.dbNullableString(info.CharterPriceNoTaxCalc) + " , ");
                    sb.AppendLine("  CharterTaxDispKbn = " + this.dbNullableString(info.CharterTaxDispKbn) + " , ");
                    sb.AppendLine("  CharterAddUpYMD = " + SQLHelper.DateTimeToDbDateTime(info.CharterAddUpYMD) + " , ");
                    sb.AppendLine("  CharterFixFlag = " + NSKUtil.BoolToInt(info.CharterFixFlag).ToString() + " , ");
                    sb.AppendLine("  CharterPayFixYMD = " + SQLHelper.DateTimeToDbDateTime(info.CharterPayFixYMD) + " , ");
                    sb.AppendLine("  Fee = " + this.dbNullableString(info.Fee) + " , ");
                    sb.AppendLine("  PriceInFee = " + this.dbNullableString(info.PriceInFee) + " , ");
                    sb.AppendLine("  TollFeeInFee = " + this.dbNullableString(info.TollFeeInFee) + " , ");
                    sb.AppendLine("  FeeOutTaxCalc = " + this.dbNullableString(info.FeeOutTaxCalc) + " , ");
                    sb.AppendLine("  FeeOutTax = " + this.dbNullableString(info.FeeOutTax) + " , ");
                    sb.AppendLine("  FeeInTaxCalc = " + this.dbNullableString(info.FeeInTaxCalc) + " , ");
                    sb.AppendLine("  FeeInTax = " + this.dbNullableString(info.FeeInTax) + " , ");
                    sb.AppendLine("  FeeNoTaxCalc = " + this.dbNullableString(info.FeeNoTaxCalc) + " , ");
                    sb.AppendLine("  Weight = " + this.dbNullableString(info.Weight) + " , ");
                    sb.AppendLine("  HanroId = " + this.dbNullableString(info.HanroId) + " , ");
                    sb.AppendLine("  JuchuTantoId = " + this.dbNullableString(info.JuchuTantoId) + " , ");
                    sb.AppendLine("  OfukuKbn = " + this.dbNullableString(info.OfukuKbn) + " , ");
                    sb.AppendLine("  ReuseYMD = " + SQLHelper.DateTimeToDbDateTime(info.ReuseYMD) + " , ");
                    sb.AppendLine("  MagoYoshasaki = '" + info.MagoYoshasaki + "' , ");
                    sb.AppendLine("  ReceivedFlag = " + NSKUtil.BoolToInt(info.ReceivedFlag) + " , ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(false).ToString() + ", ");
                    sb.AppendLine("  TaikijikanNumber = " + info.TaikijikanNumber.ToString() + " , ");
                    sb.AppendLine("  TaikijikanFigId = " + info.TaikijikanFigId.ToString() + " , ");
                    sb.AppendLine("  TaikijikanAtPrice = " + info.TaikijikanAtPrice.ToString() + " , ");
                    sb.AppendLine("  TaikijikanryoInPrice = " + info.TaikijikanryoInPrice.ToString() + " , ");
                    sb.AppendLine("  NizumiryoInPrice = " + info.NizumiryoInPrice.ToString() + " , ");
                    sb.AppendLine("  NioroshiryoInPrice = " + info.NioroshiryoInPrice.ToString() + " , ");
                    sb.AppendLine("  FutaigyomuryoInPrice = " + info.FutaigyomuryoInPrice.ToString() + " , ");
                    sb.AppendLine("  TaikijikanryoInFee = " + info.TaikijikanryoInFee.ToString() + " , ");
                    sb.AppendLine("  NizumiryoInFee = " + info.NizumiryoInFee.ToString() + " , ");
                    sb.AppendLine("  NioroshiryoInFee = " + info.NioroshiryoInFee.ToString() + " , ");
                    sb.AppendLine("  FutaigyomuryoInFee = " + info.FutaigyomuryoInFee.ToString() + " , ");
                    sb.AppendLine("  NizumijikanNumber = " + info.NizumijikanNumber.ToString() + " , ");
                    sb.AppendLine("  NizumijikanFigId = " + info.NizumijikanFigId.ToString() + " , ");
                    sb.AppendLine("  NizumijikanAtPrice = " + info.NizumijikanAtPrice.ToString() + " , ");
                    sb.AppendLine("  NioroshijikanNumber = " + info.NioroshijikanNumber.ToString() + " , ");
                    sb.AppendLine("  NioroshijikanFigId = " + info.NioroshijikanFigId.ToString() + " , ");
                    sb.AppendLine("  NioroshijikanAtPrice = " + info.NioroshijikanAtPrice.ToString() + " , ");
                    sb.AppendLine("  FutaigyomujikanNumber = " + info.FutaigyomujikanNumber.ToString() + " , ");
                    sb.AppendLine("  FutaigyomujikanFigId = " + info.FutaigyomujikanFigId.ToString() + " , ");
                    sb.AppendLine("  FutaigyomujikanAtPrice = " + info.FutaigyomujikanAtPrice.ToString() + " , ");
                    sb.AppendLine("  JomuinUriageDogakuFlag = " + NSKUtil.BoolToInt(info.JomuinUriageDogakuFlag) + " , ");
                    sb.AppendLine("  JomuinUriageKingaku = " + info.JomuinUriageKingaku.ToString() + " , ");
                    sb.AppendLine("	" + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                    sb.AppendLine("WHERE ");
                    sb.AppendLine("  JuchuId = " + info.JuchuId.ToString() + " AND ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                    sb.AppendLine("--排他のチェック ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("  VersionColumn = @versionColumn ");

                    string sql = sb.ToString();

                    SqlCommand command = new SqlCommand(sql);
                    command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                    //指定したトランザクション上でExecuteNonqueryを実行し
                    //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                    //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                    SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
                }
                else
                {
                    //--常設列の取得オプションを作る
                    SQLHelper.PopulateColumnOptions popOption =
                        SQLHelper.PopulateColumnOptions.EntryColumns |
                            SQLHelper.PopulateColumnOptions.AdditionColumns;

                    StringBuilder sb = new StringBuilder();

                    // 受注Idを採番
                    Decimal JuchuId = SQLHelper.GetSequenceId(SQLHelper.SequenceIdKind.Car);

                    sb.AppendLine("INSERT INTO Juchu ");
                    sb.AppendLine(" ( ");
                    sb.AppendLine("  JuchuId, ");
                    sb.AppendLine("  DailyReportId, ");
                    sb.AppendLine("  JuchuSlipNo, ");
                    sb.AppendLine("  BranchOfficeId, ");
                    sb.AppendLine("  CarId, ");
                    sb.AppendLine("  LicPlateCarNo, ");
                    sb.AppendLine("  CarKindId, ");
                    sb.AppendLine("  DriverId, ");
                    sb.AppendLine("  TokuisakiId, ");
                    sb.AppendLine("  TokuisakiNM, ");
                    sb.AppendLine("  ClmClassId, ");
                    sb.AppendLine("  ContractId, ");
                    sb.AppendLine("  StartPointId, ");
                    sb.AppendLine("  StartPointNM, ");
                    sb.AppendLine("  EndPointId, ");
                    sb.AppendLine("  EndPointNM, ");
                    sb.AppendLine("  ItemId, ");
                    sb.AppendLine("  ItemNM, ");
                    sb.AppendLine("  OwnerId, ");
                    sb.AppendLine("  OwnerNM, ");
                    sb.AppendLine("  TaskStartDateTime, ");
                    sb.AppendLine("  TaskEndDateTime, ");
                    sb.AppendLine("  Number, ");
                    sb.AppendLine("  FigId, ");
                    sb.AppendLine("  AtPrice, ");
                    sb.AppendLine("  Price, ");
                    sb.AppendLine("  PriceInPrice, ");
                    sb.AppendLine("  TollFeeInPrice, ");
                    sb.AppendLine("  PriceOutTaxCalc, ");
                    sb.AppendLine("  PriceOutTax, ");
                    sb.AppendLine("  PriceInTaxCalc, ");
                    sb.AppendLine("  PriceInTax, ");
                    sb.AppendLine("  PriceNoTaxCalc, ");
                    sb.AppendLine("  TaxDispKbn, ");
                    sb.AppendLine("  AddUpYMD, ");
                    sb.AppendLine("  FixFlag, ");
                    sb.AppendLine("  Memo, ");
                    sb.AppendLine("  ClmFixYMD, ");
                    sb.AppendLine("  CarOfChartererId, ");
                    sb.AppendLine("  CharterPrice, ");
                    sb.AppendLine("  PriceInCharterPrice, ");
                    sb.AppendLine("  TollFeeInCharterPrice, ");
                    sb.AppendLine("  CharterPriceOutTaxCalc, ");
                    sb.AppendLine("  CharterPriceOutTax, ");
                    sb.AppendLine("  CharterPriceInTaxCalc, ");
                    sb.AppendLine("  CharterPriceInTax, ");
                    sb.AppendLine("  CharterPriceNoTaxCalc, ");
                    sb.AppendLine("  CharterTaxDispKbn, ");
                    sb.AppendLine("  CharterAddUpYMD, ");
                    sb.AppendLine("  CharterFixFlag, ");
                    sb.AppendLine("  CharterPayFixYMD, ");
                    sb.AppendLine("  Fee, ");
                    sb.AppendLine("  PriceInFee, ");
                    sb.AppendLine("  TollFeeInFee, ");
                    sb.AppendLine("  FeeOutTaxCalc, ");
                    sb.AppendLine("  FeeOutTax, ");
                    sb.AppendLine("  FeeInTaxCalc, ");
                    sb.AppendLine("  FeeInTax, ");
                    sb.AppendLine("  FeeNoTaxCalc, ");
                    sb.AppendLine("  Weight, ");
                    sb.AppendLine("  HanroId, ");
                    sb.AppendLine("  JuchuTantoId, ");
                    sb.AppendLine("  OfukuKbn, ");
                    sb.AppendLine("  ReuseYMD, ");
                    sb.AppendLine("  MagoYoshasaki, ");
                    sb.AppendLine("  ReceivedFlag, ");
                    sb.AppendLine("  DelFlag,");
                    sb.AppendLine("  TaikijikanNumber, ");
                    sb.AppendLine("  TaikijikanFigId, ");
                    sb.AppendLine("  TaikijikanAtPrice, ");
                    sb.AppendLine("  TaikijikanryoInPrice, ");
                    sb.AppendLine("  NizumiryoInPrice, ");
                    sb.AppendLine("  NioroshiryoInPrice, ");
                    sb.AppendLine("  FutaigyomuryoInPrice, ");
                    sb.AppendLine("  TaikijikanryoInFee, ");
                    sb.AppendLine("  NizumiryoInFee, ");
                    sb.AppendLine("  NioroshiryoInFee, ");
                    sb.AppendLine("  FutaigyomuryoInFee, ");
                    sb.AppendLine("  NizumijikanNumber, ");
                    sb.AppendLine("  NizumijikanFigId, ");
                    sb.AppendLine("  NizumijikanAtPrice, ");
                    sb.AppendLine("  NioroshijikanNumber, ");
                    sb.AppendLine("  NioroshijikanFigId, ");
                    sb.AppendLine("  NioroshijikanAtPrice, ");
                    sb.AppendLine("  FutaigyomujikanNumber, ");
                    sb.AppendLine("  FutaigyomujikanFigId, ");
                    sb.AppendLine("  FutaigyomujikanAtPrice, ");
                    sb.AppendLine("  JomuinUriageDogakuFlag, ");
                    sb.AppendLine("  JomuinUriageKingaku, ");

                    sb.AppendLine(" " + SQLHelper.GetPopulateColumnSelectionString(popOption));
                    sb.AppendLine(" ) ");
                    sb.AppendLine("VALUES ");
                    sb.AppendLine("( ");
                    sb.AppendLine("  " + JuchuId.ToString() + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.DailyReportId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.JuchuSlipNo) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.BranchOfficeId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CarId) + ", ");
                    sb.AppendLine("  '" + info.LicPlateCarNo + "', ");
                    sb.AppendLine("  " + this.dbNullableString(info.CarKindId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.DriverId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TokuisakiId) + ", ");
                    sb.AppendLine(" '" + info.TokuisakiNM + "', ");
                    sb.AppendLine("  " + this.dbNullableString(info.ClmClassId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.ContractId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.StartPointId) + ", ");
                    sb.AppendLine(" '" + info.StartPointNM + "', ");
                    sb.AppendLine("  " + this.dbNullableString(info.EndPointId) + ", ");
                    sb.AppendLine(" '" + info.EndPointNM + "', ");
                    sb.AppendLine("  " + this.dbNullableString(info.ItemId) + ", ");
                    sb.AppendLine(" '" + info.ItemNM + "', ");
                    sb.AppendLine("  " + this.dbNullableString(info.OwnerId) + ", ");
                    sb.AppendLine(" '" + info.OwnerNM + "', ");
                    sb.AppendLine("  " + SQLHelper.DateTimeToDbDateTime(info.TaskStartDateTime) + ", ");
                    sb.AppendLine("  " + SQLHelper.DateTimeToDbDateTime(info.TaskEndDateTime) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.Number) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FigId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.AtPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.Price) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TollFeeInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceOutTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceOutTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceInTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceInTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceNoTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaxDispKbn) + ", ");
                    sb.AppendLine(" " + SQLHelper.DateTimeToDbDateTime(info.AddUpYMD) + ", ");
                    sb.AppendLine("  " + NSKUtil.BoolToInt(info.FixFlag) + ", ");
                    sb.AppendLine(" '" + info.Memo + "', ");
                    sb.AppendLine(" " + SQLHelper.DateTimeToDbDateTime(info.ClmFixYMD) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CarOfChartererId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceInCharterPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TollFeeInCharterPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPriceOutTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPriceOutTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPriceInTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPriceInTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterPriceNoTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.CharterTaxDispKbn) + ", ");
                    sb.AppendLine(" " + SQLHelper.DateTimeToDbDateTime(info.CharterAddUpYMD) + ", ");
                    sb.AppendLine("  " + NSKUtil.BoolToInt(info.CharterFixFlag) + ", ");
                    sb.AppendLine(" " + SQLHelper.DateTimeToDbDateTime(info.CharterPayFixYMD) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.Fee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.PriceInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TollFeeInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FeeOutTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FeeOutTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FeeInTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FeeInTax) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FeeNoTaxCalc) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.Weight) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.HanroId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.JuchuTantoId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.OfukuKbn) + ", ");
                    sb.AppendLine(" " + SQLHelper.DateTimeToDbDateTime(info.ReuseYMD) + ", ");
                    sb.AppendLine(" '" + info.MagoYoshasaki + "', ");
                    sb.AppendLine("  " + NSKUtil.BoolToInt(info.ReceivedFlag) + ", ");
                    sb.AppendLine("  " + NSKUtil.BoolToInt(false) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaikijikanNumber) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaikijikanFigId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaikijikanAtPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaikijikanryoInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NizumiryoInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NioroshiryoInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FutaigyomuryoInPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.TaikijikanryoInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NizumiryoInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NioroshiryoInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FutaigyomuryoInFee) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NizumijikanNumber) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NizumijikanFigId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NizumijikanAtPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NioroshijikanNumber) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NioroshijikanFigId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.NioroshijikanAtPrice) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FutaigyomujikanNumber) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FutaigyomujikanFigId) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.FutaigyomujikanAtPrice) + ", ");
                    sb.AppendLine("  " + NSKUtil.BoolToInt(info.JomuinUriageDogakuFlag) + ", ");
                    sb.AppendLine("  " + this.dbNullableString(info.JomuinUriageKingaku) + ", ");
                    sb.AppendLine("  " + SQLHelper.GetPopulateColumnInsertString(this._authInfo, popOption));
                    sb.AppendLine(" ) ");

                    string sql = sb.ToString();
                    //指定したトランザクション上でExecuteNonqueryを実行し
                    //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                    //影響を受けた行数が1以上場合は、致命的なエラーの例外。 
                    SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, new SqlCommand(sql));
                }
            }
        }

        /// <summary>
        /// SqlTransaction情報、受注情報リストを指定して、
        /// 受注情報の論理削除を行います。
        /// </summary>
        /// <param name="transaction">SqlTransaction情報</param>
        /// <param name="info">受注情報リスト</param>
        public void Delete(SqlTransaction transaction, List<JuchuNyuryokuInfo> InfoList)
        {
            foreach (var info in InfoList)
            {
                //明細情報がデータベースに保存されているかどうか
                if (info.IsPersisted)
                {
                    //--常設列の取得オプションを作る
                    SQLHelper.PopulateColumnOptions popOption =
                        SQLHelper.PopulateColumnOptions.EntryColumns |
                            SQLHelper.PopulateColumnOptions.UpdateColumns;

                    //配車の論理削除
                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("UPDATE Haisha ");
                    sb.AppendLine("SET ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(true).ToString() + ", ");
                    sb.AppendLine("	" + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("  JuchuId = " + info.JuchuId.ToString() + " AND ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                    string sql = sb.ToString();

                    SqlCommand command = new SqlCommand(sql);
                    SQLHelper.ExecuteNonQueryOnTransaction(transaction, command);

                    sb.AppendLine("WHERE ");
                    sb.AppendLine("  JuchuId = " + info.JuchuId.ToString() + " AND ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                    sb.AppendLine("--排他のチェック ");

                    //受注の論理削除
                    sb = new StringBuilder();
                    sb.AppendLine("UPDATE Juchu ");
                    sb.AppendLine("SET ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(true).ToString() + ", ");
                    sb.AppendLine("	" + SQLHelper.GetPopulateColumnUpdateString(_authInfo, popOption) + " ");

                    sb.AppendLine("WHERE ");
                    sb.AppendLine("  JuchuId = " + info.JuchuId.ToString() + " AND ");
                    sb.AppendLine("  DelFlag = " + NSKUtil.BoolToInt(false).ToString() + " ");
                    sb.AppendLine("--排他のチェック ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("  VersionColumn = @versionColumn ");

                    sql = sb.ToString();

                    command = new SqlCommand(sql);
                    command.Parameters.Add(new SqlParameter("@versionColumn", info.VersionColumn));

                    //指定したトランザクション上でExecuteNonqueryを実行し
                    //影響を受けた行数が0の場合は、楽観的排他エラーの例外 
                    //影響を受けた行数が1以上場合は、致命的なエラーの例外。
                    SQLHelper.ExecuteNonQueryForSingleRecordUpdate(transaction, command);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokuisakiCd"></param>
        /// <param name="lastClmFixDay"></param>
        /// <returns></returns>
        public bool GetLastClmFixDayOfClmByTokuisaki(int tokuisakiCd, out DateTime lastClmFixDay)
        {
            //返却値用
            bool rt_val = false;

            //引数のoutキーワードに格納する為の変数
            DateTime wk_date = DateTime.MinValue;

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" COALESCE(CLM.LastClmFixDay, TK.LastClmFixDay) AS LastClmFixDay ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Tokuisaki AS TK LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki AS CLM ");
            sb.AppendLine(" ON TK.ClmId = CLM.TokuisakiId ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" TK.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND TK.TokuisakiCd = " +
                tokuisakiCd.ToString() + " ");

            String mySql = sb.ToString();

            List<DateTime> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                wk_date =
                            NSKUtil.DecimalWithTimeToDateTime(
                                SQLServerUtil.dbDecimal(rdr["LastClmFixDay"]));
                rt_val = true;
                //返却用の値を返します
                return wk_date;
            }, null);

            //outの変数に格納
            lastClmFixDay = wk_date;

            return rt_val;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="torihikisakiCd"></param>
        /// <param name="lastPaymentFixDay"></param>
        /// <returns></returns>
        public bool GetLastPaymentFixDayByTorihikisaki(int torihikisakiCd, out DateTime lastPaymentFixDay)
        {
            //返却値用
            bool rt_val = false;

            //引数のoutキーワードに格納する為の変数
            DateTime wk_date = DateTime.MinValue;

            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine(" COALESCE(PAY.LastPayFixDay, TR.LastPayFixDay) AS LastPayFixDay ");
            sb.AppendLine("FROM ");
            sb.AppendLine(" TORADON_Torihikisaki AS TR ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki AS PAY ");
            sb.AppendLine(" ON TR.PayeeId = PAY.TorihikiId ");
            sb.AppendLine("WHERE ");
            sb.AppendLine(" TR.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND TR.TorihikiCd = " +
                torihikisakiCd.ToString() + " ");

            String mySql = sb.ToString();

            List<DateTime> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                wk_date =
                            NSKUtil.DecimalWithTimeToDateTime(
                                SQLServerUtil.dbDecimal(rdr["LastPayFixDay"]));
                rt_val = true;
                //返却用の値を返します
                return wk_date;
            }, null);

            //outの変数に格納
            lastPaymentFixDay = wk_date;

            return rt_val;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokuisakiCode"></param>
        /// <param name="itemCode"></param>
        /// <param name="startPointCode"></param>
        /// <param name="endPointCode"></param>
        /// <param name="figCode"></param>
        /// <param name="carKindCode"></param>
        /// <returns></returns>
        public AtPriceInfo GetAtPrice(int tokuisakiCode, int itemCode, int startPointCode,
            int endPointCode, int figCode, int carKindCode, TokuisakiInfo tok_info)
        {
            AtPriceInfo rt_info = null;

            //得意先の単価索引区分保持用配列
            string[] indexKbnArr = new string[3];

            if (tok_info != null)
            {
                //単価索引区分を取得しておく
                //indexKbnArr[0] = tok_info.ToraDONFirstAtPriceIndexKbn;
                //indexKbnArr[1] = tok_info.ToraDONSecondAtPriceIndexKbn;
                //indexKbnArr[2] = tok_info.ToraDONThirdAtPriceIndexKbn;

                for (int i = 0; i < 3; i++)
                {
                    int search_tokuisaki = 0;
                    int search_item = 0;
                    int search_startpoint = 0;
                    int search_endpoint = 0;
                    int search_fig = 0;
                    int search_carkind = 0;

                    //単価索引区分に応じて検索用変数に引数をセット
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.Tokuisaki, 1))))
                    {
                        search_tokuisaki = tokuisakiCode;
                    }
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.Item, 1))))
                    {
                        search_item = itemCode;
                    }
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.StartPoint, 1))))
                    {
                        search_startpoint = startPointCode;
                    }
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.EndPoint, 1))))
                    {
                        search_endpoint = endPointCode;
                    }
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.Fig, 1))))
                    {
                        search_fig = figCode;
                    }
                    if (NSKUtil.IntToBool(int.Parse(indexKbnArr[i].Substring(
                        (int)BizProperty.DefaultProperty.AtPriceIndexKbn.CarKind, 1))))
                    {
                        search_carkind = carKindCode;
                    }

                    //単価テーブルから条件に一致する単価を取得する
                    AtPriceInfo wk_info = null;

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("SELECT ");
                    sb.AppendLine("A.* ");
                    sb.AppendLine("FROM ");
                    sb.AppendLine("TORADON_AtPrice AS A LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_Tokuisaki AS B ON ");
                    sb.AppendLine("A.TokuisakiId = B.TokuisakiId LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_Item AS C ON ");
                    sb.AppendLine("A.ItemId = C.ItemId LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_Point AS D ON ");
                    sb.AppendLine("A.StartPointId = D.PointId LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_Point AS E ON ");
                    sb.AppendLine("A.EndPointId = E.PointId LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_Fig AS F ON ");
                    sb.AppendLine("A.FigId = F.FigId LEFT OUTER JOIN ");
                    sb.AppendLine("TORADON_CarKind AS G ON ");
                    sb.AppendLine("A.CarKindId = G.CarKindId ");
                    sb.AppendLine("WHERE ");
                    sb.AppendLine("A.DelFlag = " +
                        NSKUtil.BoolToInt(false).ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("A.EstablishmentFlag = " +
                        indexKbnArr[i] + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(B.TokuisakiCd, 0) = " +
                        search_tokuisaki.ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(C.ItemCd, 0) = " +
                        search_item.ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(D.PointCd, 0) = " +
                        search_startpoint.ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(E.PointCd, 0) = " +
                        search_endpoint.ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(F.FigCd, 0) = " +
                        search_fig.ToString() + " ");
                    sb.AppendLine("AND ");
                    sb.AppendLine("ISNULL(G.CarKindCd, 0) = " +
                        search_carkind.ToString() + " ");

                    string mySql = sb.ToString();

                    List<AtPriceInfo> list = SQLHelper.SimpleRead(mySql, rdr =>
                    {
                        //存在したら単価情報作成
                        wk_info = new AtPriceInfo()
                        {
                            AtPrice =
                                SQLServerUtil.dbDecimal(rdr["AtPrice"])
                        };
                        //返却用の値を返します
                        return wk_info;
                    }, null);

                    if (wk_info != null)
                    {
                        rt_info = wk_info;
                    }
                }

            }

            return rt_info;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="juchuSlipNo"></param>
        /// <param name="excludedList"></param>
        /// <returns></returns>
        public bool ContainsClmTotaledJuchu(decimal juchuSlipNo, IList<decimal> excludedList)
        {
            //TODO 性能問題のため未使用になる可能性あり

            //請求集計済みの存在チェック
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("A.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("Juchu AS A ");
            sb.AppendLine("LEFT JOIN ");
            sb.AppendLine("( ");
            sb.AppendLine(" SELECT ");
            sb.AppendLine("  Juchu.JuchuId, ");
            sb.AppendLine("  MIN(TORADON_Sale.ClmFixDate)        AS MinClmFixDate ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  Juchu ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  Haisha ON ");
            sb.AppendLine("  Haisha.JuchuId = Juchu.JuchuId AND ");
            sb.AppendLine("  Haisha.DelFlag = 0 ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  HaishaSeikyuRenkeiManage ON ");
            sb.AppendLine("  HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            sb.AppendLine("  Haisha.DelFlag = 0 ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  TORADON_Sale ON ");
            sb.AppendLine("  TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            sb.AppendLine("  TORADON_Sale.DelFlag = 0 ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  Juchu.JuchuSlipNo = " + juchuSlipNo.ToString() + " AND ");
            sb.AppendLine("  Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" GROUP BY ");
            sb.AppendLine("  Juchu.JuchuId ");
            sb.AppendLine(") ToraDONSale ON ");
            sb.AppendLine("  ToraDONSale.JuchuId = A.JuchuId ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("A.JuchuSlipNo = " +
                juchuSlipNo.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("ISNULL(ToraDONSale.MinClmFixDate, 0) > 0 ");

            //--読込み時に既に請求済みだった受注明細は除外する
            int listcnt = excludedList.Count;

            if (listcnt > 0)
            {
                sb.AppendLine("AND A.JuchuId NOT IN ( ");

                StringBuilder sbId = new StringBuilder();
                foreach (decimal id in excludedList)
                {
                    sbId.AppendLine("," + id.ToString());
                }

                sb.AppendLine(sbId.ToString().Substring(1) + ") ");
            }

            string mySql = sb.ToString();

            int rt_cnt = SQLHelper.GetCountByQuery(mySql, null);

            if (0 < rt_cnt) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyReportId"></param>
        /// <param name="excludedList"></param>
        /// <returns></returns>
        public bool ContainsPayTotaledJuchu(int dailyReportId, IList<decimal> excludedList)
        {
            //TODO 性能問題のため未使用になる可能性あり

            //請求集計済みの存在チェック
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("A.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("Juchu AS A ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("A.DailyReportId = " +
                dailyReportId.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.CharterPayFixDate <> 0 ");

            //--読込み時に既に請求済みだった受注明細は除外する
            int listcnt = excludedList.Count;

            if (listcnt > 0)
            {
                sb.AppendLine("AND A.JuchuId NOT IN ( ");

                StringBuilder sbId = new StringBuilder();
                foreach (decimal id in excludedList)
                {
                    sbId.AppendLine("," + id.ToString());
                }

                sb.AppendLine(sbId.ToString().Substring(1) + ") ");
            }

            string mySql = sb.ToString();


            int rt_cnt = SQLHelper.GetCountByQuery(mySql, null);

            if (0 < rt_cnt) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="juchuSlipNo"></param>
        /// <param name="excludedList"></param>
        /// <returns></returns>
        public bool ContainsMonthlyTotaledJuchu(decimal juchuSlipNo, IList<decimal> excludedList, DateTime last_sum_of_monthday)
        {

            //月次締処理済みの存在チェック
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("A.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("Juchu AS A ");

            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine(" LEFT JOIN ");
            //sb.AppendLine(" ( ");
            //sb.AppendLine("  SELECT ");
            //sb.AppendLine("   Juchu.JuchuId, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.ClmFixDate = 0 THEN 99999999999999 ELSE TORADON_Sale.ClmFixDate END)        AS MinClmFixDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.CharterPayFixDate = 0 THEN 99999999999999 ELSE TORADON_Sale.CharterPayFixDate END) AS MinCharterPayFixDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.AddUpDate = 0 THEN 99999999999999 ELSE TORADON_Sale.AddUpDate END) AS MinAddUpDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.CharterAddUpDate = 0 THEN 99999999999999 ELSE TORADON_Sale.CharterAddUpDate END) AS MinCharterAddUpDate, ");
            //sb.AppendLine("   MAX(TORADON_Sale.FixFlag)           AS MaxFixFlag, ");
            //sb.AppendLine("   MAX(TORADON_Sale.CharterFixFlag)    AS MaxCharterFixFlag ");
            //sb.AppendLine("  FROM ");
            //sb.AppendLine("   Juchu ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   Haisha ON ");
            //sb.AppendLine("   Haisha.JuchuId = Juchu.JuchuId AND ");
            //sb.AppendLine("   Haisha.DelFlag = 0 ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage ON ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            //sb.AppendLine("   Haisha.DelFlag = 0 ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   TORADON_Sale ON ");
            //sb.AppendLine("   TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            //sb.AppendLine("   TORADON_Sale.DelFlag = 0 ");
            //sb.AppendLine("  WHERE ");
            //sb.AppendLine("   Juchu.JuchuSlipNo = " + juchuSlipNo.ToString() + " AND ");
            //sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            //sb.AppendLine("  GROUP BY ");
            //sb.AppendLine("   Juchu.JuchuId ");
            //sb.AppendLine(" ) ToraDONSale ON ");
            //sb.AppendLine("   ToraDONSale.JuchuId = A.JuchuId ");
            //TODO 性能問題のためコメント化中 END

            sb.AppendLine("WHERE ");
            sb.AppendLine("A.JuchuSlipNo = " +
                juchuSlipNo.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");

            sb.AppendLine("AND ");
            sb.AppendLine("(( ");

            //配車Aceの計上日、確定フラグ（傭車計上日、傭車確定フラグ）
            sb.AppendLine("A.AddUpYMD <= '" +
                    last_sum_of_monthday.ToString() + "' ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.FixFlag = " +
                NSKUtil.BoolToInt(true).ToString() + " ");
            sb.AppendLine(") OR ");
            sb.AppendLine("( ");
            sb.AppendLine("A.CharterAddUpYMD <= '" +
                    last_sum_of_monthday.ToString() + "' ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.CharterFixFlag = " +
                NSKUtil.BoolToInt(true).ToString() + " ");

            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine(") OR ");
            sb.AppendLine(") ");

            ////トラDONの計上日、確定フラグ（傭車計上日、傭車確定フラグ）
            //sb.AppendLine("( ");
            //sb.AppendLine("ToraDONSale.MinAddUpDate <= " +
            //    NSKUtil.DateTimeToDecimalWithTime(
            //        last_sum_of_monthday).ToString() + " ");
            //sb.AppendLine("AND ");
            //sb.AppendLine("ToraDONSale.MaxFixFlag = " +
            //    NSKUtil.BoolToInt(true).ToString() + " ");
            //sb.AppendLine(") OR ");
            //sb.AppendLine("( ");
            //sb.AppendLine("ToraDONSale.MinCharterAddUpDate <= " +
            //    NSKUtil.DateTimeToDecimalWithTime(
            //        last_sum_of_monthday).ToString() + " ");
            //sb.AppendLine("AND ");
            //sb.AppendLine("ToraDONSale.MaxCharterFixFlag = " +
            //    NSKUtil.BoolToInt(true).ToString() + " ");

            //sb.AppendLine(")) ");
            sb.AppendLine(") ");
            //TODO 性能問題のためコメント化中 END

            //--読込み時に既に月次締処理済みだった受注明細は除外する
            int listcnt = excludedList.Count;

            if (listcnt > 0)
            {
                sb.AppendLine("AND A.JuchuId NOT IN ( ");

                StringBuilder sbId = new StringBuilder();
                foreach (decimal id in excludedList)
                {
                    sbId.AppendLine("," + id.ToString());
                }

                sb.AppendLine(sbId.ToString().Substring(1) + ") ");
            }

            string mySql = sb.ToString();

            int rt_cnt = SQLHelper.GetCountByQuery(mySql, null);

            if (0 < rt_cnt) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dailyReportId"></param>
        /// <param name="excludedList"></param>
        /// <returns></returns>
        public bool ContainsPayTotaledJuchu(decimal juchuSlipNo, IList<decimal> excludedList)
        {
            //TODO 性能問題のため未使用になる可能性あり

            //請求集計済みの存在チェック
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("SELECT ");
            sb.AppendLine("A.* ");
            sb.AppendLine("FROM ");
            sb.AppendLine("Juchu AS A ");
            sb.AppendLine("LEFT JOIN ");
            sb.AppendLine("( ");
            sb.AppendLine(" SELECT ");
            sb.AppendLine("  Juchu.JuchuId, ");
            sb.AppendLine("  MIN(TORADON_Sale.CharterPayFixDate) AS MinCharterPayFixDate ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("  Juchu ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  Haisha ON ");
            sb.AppendLine("  Haisha.JuchuId = Juchu.JuchuId AND ");
            sb.AppendLine("  Haisha.DelFlag = 0 ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  HaishaSeikyuRenkeiManage ON ");
            sb.AppendLine("  HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            sb.AppendLine("  Haisha.DelFlag = 0 ");
            sb.AppendLine(" LEFT OUTER JOIN ");
            sb.AppendLine("  TORADON_Sale ON ");
            sb.AppendLine("  TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            sb.AppendLine("  TORADON_Sale.DelFlag = 0 ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("  Juchu.JuchuSlipNo = " + juchuSlipNo.ToString() + " AND ");
            sb.AppendLine("  Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" GROUP BY ");
            sb.AppendLine("  Juchu.JuchuId ");
            sb.AppendLine(") ToraDONSale ON ");
            sb.AppendLine("  ToraDONSale.JuchuId = A.JuchuId ");
            sb.AppendLine("WHERE ");
            sb.AppendLine("A.JuchuSlipNo = " +
                juchuSlipNo.ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("A.DelFlag = " +
                NSKUtil.BoolToInt(false).ToString() + " ");
            sb.AppendLine("AND ");
            sb.AppendLine("ISNULL(ToraDONSale.MinCharterPayFixDate, 0) > 0 ");

            //--読込み時に既に請求済みだった受注明細は除外する
            int listcnt = excludedList.Count;

            if (listcnt > 0)
            {
                sb.AppendLine("AND A.JuchuId NOT IN ( ");

                StringBuilder sbId = new StringBuilder();
                foreach (decimal id in excludedList)
                {
                    sbId.AppendLine("," + id.ToString());
                }

                sb.AppendLine(sbId.ToString().Substring(1) + ") ");
            }

            string mySql = sb.ToString();

            int rt_cnt = SQLHelper.GetCountByQuery(mySql, null);

            if (0 < rt_cnt) return true;

            return false;
        }

        /// <summary>
        /// 受注情報に紐付く配車データの存在可否を取得します。
        /// （true：存在する、false：存在しない）
        /// </summary>
        /// <param name="juchuInfoList">受注リスト</param>
        /// <returns>受注データ</returns>
        public bool ExistsHaishaInfo(List<JuchuNyuryokuInfo> juchuInfoList)
        {
            bool rt_bool = false;

            if (juchuInfoList == null || juchuInfoList.Count == 0)
            {
                return false;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine(" SELECT 1 ");
            sb.AppendLine(" FROM Haisha ");
            sb.AppendLine(" WHERE DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" AND JuchuId IN (" + this.GetHaishaIdList(juchuInfoList) + ") ");
            
            SQLHelper.ActionWithTransaction(tx =>
            {
                //配車データの存在チェック
                if (SQLHelper.RecordExists(sb.ToString(), tx))
                {
                    rt_bool = true;
                }
            });

            return rt_bool;
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// 営業所マスタSELECT_SQLを取得します。
        /// </summary>
        /// <returns>SELECT_SQL</returns>
        private string GetSelectBranchOfficeSQL()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("   BranchOfficeId   AS BranchOfficeId, ");
            sb.AppendLine("   BranchOfficeCd   AS BranchOfficeCd, ");
            sb.AppendLine("   BranchOfficeNM   AS BranchOfficeNM, ");
            sb.AppendLine("   BranchOfficeSNM  AS BranchOfficeSNM, ");
            sb.AppendLine("   Postal           AS Postal, ");
            sb.AppendLine("   Address1         AS Address1, ");
            sb.AppendLine("   Address2         AS Address2, ");
            sb.AppendLine("   Tel              AS Tel, ");
            sb.AppendLine("   Fax              AS Fax, ");
            sb.AppendLine("   Url              AS Url, ");
            sb.AppendLine("   MailAddress      AS MailAddress, ");
            sb.AppendLine("   Memo             AS Memo, ");
            sb.AppendLine("   Account1         AS Account1, ");
            sb.AppendLine("   Account2         AS Account2, ");
            sb.AppendLine("   Account3         AS Account3, ");
            sb.AppendLine("   AccountSub1      AS AccountSub1, ");
            sb.AppendLine("   AccountSub2      AS AccountSub2, ");
            sb.AppendLine("   AccountSub3      AS AccountSub3, ");
            sb.AppendLine("   DisableFlag      AS DisableFlag, ");
            sb.AppendLine("   DelFlag          AS DelFlag, ");
            sb.AppendLine("   VersionColumn    AS VersionColumn, ");
            sb.AppendLine("   EntryOperatorId  AS EntryOperatorId, ");
            sb.AppendLine("   EntryProcessId   AS EntryProcessId, ");
            sb.AppendLine("   EntryTerminalId  AS EntryTerminalId, ");
            sb.AppendLine("   EntryDateTime    AS EntryDateTime, ");
            sb.AppendLine("   AddOperatorId    AS AddOperatorId, ");
            sb.AppendLine("   AddProcessId     AS AddProcessId, ");
            sb.AppendLine("   AddTerminalId    AS AddTerminalId, ");
            sb.AppendLine("   AddDateTime      AS AddDateTime, ");
            sb.AppendLine("   UpdateOperatorId AS UpdateOperatorId, ");
            sb.AppendLine("   UpdateProcessId  AS UpdateProcessId, ");
            sb.AppendLine("   UpdateTerminalId AS UpdateTerminalId, ");
            sb.AppendLine("   UpdateDateTime   AS UpdateDateTime, ");
            sb.AppendLine("   ChihoKbn         AS ChihoKbn ");
            sb.AppendLine(" FROM ");
            sb.AppendLine("   TORADON_BranchOffice ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   DisableFlag = " + NSKUtil.BoolToInt(false) + " AND ");
            sb.AppendLine("   DelFlag     = " + NSKUtil.BoolToInt(false) + " ");
            sb.AppendLine(" ORDER BY BranchOfficeCd ");

            return sb.ToString();
        }

        /// <summary>
        /// SELECT_SQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>SELECT_SQL</returns>
        private string GetSelectJuchuSQL(JuchuNyuryokuSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT  ");
            sb.AppendLine("   Juchu.*, ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeCd  AS BranchOfficeCd, ");
            sb.AppendLine("   TORADON_Tokuisaki.TokuisakiCd        AS TokuisakiCd, ");
            sb.AppendLine("   TORADON_Tokuisaki.MemoAccount        AS MemoAccount, ");
            sb.AppendLine("   TORADON_Tokuisaki.ClmClassUseKbn     AS ClmClassUseKbn, ");
            sb.AppendLine("   Hanro.HanroCode                      AS HanroCd, ");
            sb.AppendLine("   Hanro.HanroName                      AS HanroNm, ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassCd          AS ClmClassCd, ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassSNM         AS ClmClassSNM, ");
            sb.AppendLine("   TORADON_Contract.ContractCd          AS ContractCd, ");
            sb.AppendLine("   TORADON_Contract.ContractNm          AS ContractNm, ");
            sb.AppendLine("   TORADON_Item.ItemCd                  AS ItemCd, ");
            sb.AppendLine("   StartPoint.PointCd                   AS StartPointCd, ");
            sb.AppendLine("   EndPoint.PointCd                     AS EndPointCd, ");
            sb.AppendLine("   TORADON_Fig.FigCd                    AS FigCd, ");
            sb.AppendLine("   TORADON_Fig.FigNm                    AS FigNm, ");
            sb.AppendLine("   TORADON_Car.CarCd                    AS CarCd, ");
            sb.AppendLine("   TORADON_Car.CarKbn                   AS CarKbn, ");
            sb.AppendLine("   Car_BranchOffice.BranchOfficeSNM     AS CarBranchOfficeSNM, ");
            sb.AppendLine("   TORADON_Car.CarKbn                   AS CarKbn, ");
            sb.AppendLine("   TORADON_CarKind.CarKindCd            AS CarKindCd, ");
            sb.AppendLine("   TORADON_CarKind.CarKindSNM           AS CarKindSNM, ");
            sb.AppendLine("   Driver.StaffCd                       AS DriverCd, ");
            sb.AppendLine("   Driver.StaffNm                       AS DriverNm, ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiCd      AS CarOfChartererCd, ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiNm      AS CarOfChartererShortNm, ");
            sb.AppendLine("   TORADON_Owner.OwnerCd                AS OwnerCd, ");
            sb.AppendLine("   JuchTanto.StaffCd                    AS JuchTantoCd, ");
            sb.AppendLine("   JuchTanto.StaffNm                    AS JuchTantoNm, ");
            sb.AppendLine("   OfukuKbn.SystemNameName              AS OfukuKbnNm ");
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine("  ,CASE WHEN ToraDONSale.MinClmFixDate = 99999999999999 THEN 0 ELSE ToraDONSale.MinClmFixDate END AS MinClmFixDate, ");
            //sb.AppendLine("   CASE WHEN ToraDONSale.MinCharterPayFixDate = 99999999999999 THEN 0 ELSE ToraDONSale.MinCharterPayFixDate END AS MinCharterPayFixDate, ");
            //sb.AppendLine("   CASE WHEN ToraDONSale.MinAddUpDate = 99999999999999 THEN 0 ELSE ToraDONSale.MinAddUpDate END AS MinAddUpDate, ");
            //sb.AppendLine("   CASE WHEN ToraDONSale.MinCharterAddUpDate = 99999999999999 THEN 0 ELSE ToraDONSale.MinCharterAddUpDate END AS MinCharterAddUpDate, ");
            //sb.AppendLine("   ToraDONSale.MaxFixFlag               AS MaxFixFlag, ");
            //sb.AppendLine("   ToraDONSale.MaxCharterFixFlag        AS MaxCharterFixFlag ");
            //TODO 性能問題のためコメント化中 END

            sb.AppendLine(" FROM Juchu ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_BranchOffice ON ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeId = Juchu.BranchOfficeId AND ");
            sb.AppendLine("   TORADON_BranchOffice.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" Hanro ON ");
            sb.AppendLine("   Hanro.HanroId = Juchu.HanroId AND ");
            sb.AppendLine("   Hanro.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Tokuisaki ON ");
            sb.AppendLine("   TORADON_Tokuisaki.TokuisakiId = Juchu.TokuisakiId AND ");
            sb.AppendLine("   TORADON_Tokuisaki.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_ClmClass ON ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassId = Juchu.ClmClassId AND ");
            sb.AppendLine("   TORADON_ClmClass.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Contract ON ");
            sb.AppendLine("   TORADON_Contract.ContractId = Juchu.ContractId AND ");
            sb.AppendLine("   TORADON_Contract.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Item ON ");
            sb.AppendLine("   TORADON_Item.ItemId = Juchu.ItemId AND ");
            sb.AppendLine("   TORADON_Item.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Point StartPoint ON ");
            sb.AppendLine("   StartPoint.PointId = Juchu.StartPointId AND ");
            sb.AppendLine("   StartPoint.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Point EndPoint ON ");
            sb.AppendLine("   EndPoint.PointId = Juchu.EndPointId AND ");
            sb.AppendLine("   EndPoint.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Fig ON ");
            sb.AppendLine("   TORADON_Fig.FigId = Juchu.FigId AND ");
            sb.AppendLine("   TORADON_Fig.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Car ON ");
            sb.AppendLine("   TORADON_Car.CarId = Juchu.CarId AND ");
            sb.AppendLine("   TORADON_Car.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_BranchOffice Car_BranchOffice ON ");
            sb.AppendLine("   Car_BranchOffice.BranchOfficeId = TORADON_Car.BranchOfficeId AND ");
            sb.AppendLine("   Car_BranchOffice.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_CarKind ON ");
            sb.AppendLine("   TORADON_CarKind.CarKindId = Juchu.CarKindId AND ");
            sb.AppendLine("   TORADON_CarKind.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff Driver ON ");
            sb.AppendLine("   Driver.StaffId = Juchu.DriverId AND ");
            sb.AppendLine("   Driver.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki ON ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiId = Juchu.CarOfChartererId AND ");
            sb.AppendLine("   TORADON_Torihikisaki.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Owner ON ");
            sb.AppendLine("   TORADON_Owner.OwnerId = Juchu.OwnerId AND ");
            sb.AppendLine("   TORADON_Owner.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff JuchTanto ON ");
            sb.AppendLine("   JuchTanto.StaffId = Juchu.JuchuTantoId AND ");
            sb.AppendLine("   JuchTanto.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName OfukuKbn ON ");
            sb.AppendLine("   OfukuKbn.SystemNameKbn =  " + (int)DefaultProperty.SystemNameKbn.OfukuKbn + " AND ");
            sb.AppendLine("   OfukuKbn.SystemNameCode = Juchu.OfukuKbn ");
            //TODO 性能問題のためコメント化中 START
            //sb.AppendLine(" LEFT JOIN ");
            //sb.AppendLine(" ( ");
            //sb.AppendLine("  SELECT ");
            //sb.AppendLine("   Juchu.JuchuId, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.ClmFixDate = 0 THEN 99999999999999 ELSE TORADON_Sale.ClmFixDate END)        AS MinClmFixDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.CharterPayFixDate = 0 THEN 99999999999999 ELSE TORADON_Sale.CharterPayFixDate END) AS MinCharterPayFixDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.AddUpDate = 0 THEN 99999999999999 ELSE TORADON_Sale.AddUpDate END) AS MinAddUpDate, ");
            //sb.AppendLine("   MIN(CASE WHEN TORADON_Sale.CharterAddUpDate = 0 THEN 99999999999999 ELSE TORADON_Sale.CharterAddUpDate END) AS MinCharterAddUpDate, ");
            //sb.AppendLine("   MAX(TORADON_Sale.FixFlag)           AS MaxFixFlag, ");
            //sb.AppendLine("   MAX(TORADON_Sale.CharterFixFlag)    AS MaxCharterFixFlag ");
            //sb.AppendLine("  FROM ");
            //sb.AppendLine("   Juchu ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   Haisha ON ");
            //sb.AppendLine("   Haisha.JuchuId = Juchu.JuchuId AND ");
            //sb.AppendLine("   Haisha.DelFlag = 0 ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage ON ");
            //sb.AppendLine("   HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId AND ");
            //sb.AppendLine("   Haisha.DelFlag = 0 ");
            //sb.AppendLine("  LEFT OUTER JOIN ");
            //sb.AppendLine("   TORADON_Sale ON ");
            //sb.AppendLine("   TORADON_Sale.SaleId = HaishaSeikyuRenkeiManage.ToraDONSaleId AND ");
            //sb.AppendLine("   TORADON_Sale.DelFlag = 0 ");
            //sb.AppendLine("  WHERE ");
            //sb.AppendLine("   Juchu.JuchuSlipNo = " + para.JuchuSlipNo.ToString() + " AND ");
            //sb.AppendLine("   Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");
            //sb.AppendLine("  GROUP BY ");
            //sb.AppendLine("   Juchu.JuchuId ");
            //sb.AppendLine(" ) ToraDONSale ON ");
            //sb.AppendLine("   ToraDONSale.JuchuId = Juchu.JuchuId ");
            //TODO 性能問題のためコメント化中 END

            sb.AppendLine(" WHERE ");
            sb.AppendLine("  Juchu.JuchuSlipNo = " + para.JuchuSlipNo.ToString() + " AND ");
            sb.AppendLine("  Juchu.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            return sb.ToString();
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
        ///受注Idをカンマ区切りで取得します。
        /// </summary>
        /// <param name="juchuInfoList">受注リスト</param>
        /// <returns>カンマ区切りの受注Idリスト</returns>
        private string GetHaishaIdList(List<JuchuNyuryokuInfo> juchuInfoList)
        {
            StringBuilder sb = new StringBuilder();
            bool f = false;
            foreach (JuchuNyuryokuInfo info in juchuInfoList)
            {
                if (f) sb.Append(@",");
                sb.AppendLine("\'" + info.JuchuId.ToString() + "\'");
                f = true;
            }
            return sb.ToString();
        }

        #endregion
    }
}
