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
    /// 受注一覧のデータアクセスレイヤです。
    /// </summary>
    public class JuchuIchiran
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
        public JuchuIchiran()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public JuchuIchiran(AppAuthInfo authInfo)
        {
            this._authInfo = authInfo;
        }

        #region パブリックメソッド

        /// <summary>
        /// 未確定マーク
        /// </summary>
        private const string UNFIX_MARK = "●";

        /// <summary>
        /// 乗務員売上同額マーク
        /// </summary>
        private const string DOGAKU_MARK = "✓";

        /// <summary>
        /// 配車済マーク
        /// </summary>
        private const string HAISHAZUMI_MARK = "●";

        /// <summary>
        /// 連携済マーク
        /// </summary>
        private const string RENKEIZUMI_MARK = "●";

        /// <summary>
        /// 受注IDを指定して、受注情報を取得します。
        /// </summary>
        /// <returns>受注情報</returns>
        public List<JuchuIchiranInfo> GetJuchuInfoList(JuchuIchiranSearchParameter para = null)
        {
            //SQL文を作成
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(this.GetSelectJuchuSQL(para));

            String mySql = sb.ToString();

            List<JuchuIchiranInfo> list = SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                JuchuIchiranInfo rt_info = new JuchuIchiranInfo();

                rt_info.JuchuId = SQLServerUtil.dbDecimal(rdr["JuchuId"]);
                rt_info.JuchuSlipNo = SQLServerUtil.dbDecimal(rdr["JuchuSlipNo"]);
                rt_info.BranchOfficeSNM = (rdr["BranchOfficeSNM"]).ToString();
                rt_info.CarCd = SQLServerUtil.dbInt(rdr["CarCd"]);
                rt_info.LicPlateCarNo = (rdr["LicPlateCarNo"]).ToString();
                rt_info.CarKindSNM = (rdr["CarKindSNM"]).ToString();
                rt_info.DriverNm = (rdr["DriverNm"]).ToString();
                rt_info.TokuisakiNM = (rdr["TokuisakiNM"]).ToString();
                rt_info.ClmClassNM = (rdr["ClmClassNM"]).ToString();
                rt_info.ContractNM = (rdr["ContractNM"]).ToString();
                rt_info.TaskStartDateTime = SQLHelper.dbDate(rdr["TaskStartDateTime"]);
                rt_info.TaskEndDateTime = SQLHelper.dbDate(rdr["TaskEndDateTime"]);
                rt_info.ReuseYMD = SQLHelper.dbDate(rdr["ReuseYMD"]);
                rt_info.StartPointNM = (rdr["StartPointNM"]).ToString();
                rt_info.EndPointNM = (rdr["EndPointNM"]).ToString();
                rt_info.ItemNM = (rdr["ItemNM"]).ToString();
                rt_info.OwnerNM = (rdr["OwnerNM"]).ToString();
                rt_info.JuchuTantoNm = rdr["JuchTantoNm"].ToString();
                rt_info.HanroNm = rdr["HanroNm"].ToString();
                rt_info.OfukuKbnNm = rdr["OfukuKbnNm"].ToString();
                rt_info.CarOfChartererShortNm = (rdr["CarOfChartererShortNm"]).ToString();
                rt_info.Number = SQLServerUtil.dbDecimal(rdr["Number"]);
                rt_info.FigNm = (rdr["FigNm"]).ToString();
                rt_info.AtPrice = SQLServerUtil.dbDecimal(rdr["AtPrice"]);
                rt_info.Weight = SQLServerUtil.dbDecimal(rdr["Weight"]);
                rt_info.PriceInPrice = SQLServerUtil.dbDecimal(rdr["PriceInPrice"]);
                if (SQLHelper.dbBit(rdr["FixFlag"]))
                {
                    rt_info.TaxDispKbnNm = (rdr["TaxDispKbnNm"]).ToString();
                }
                else
                {
                    //未確定の場合は"●"をセット
                    rt_info.TaxDispKbnNm = UNFIX_MARK;
                }
                rt_info.TollFeeInPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInPrice"]);
                rt_info.AddUpYMD = SQLHelper.dbDate(rdr["AddUpYMD"]);
                rt_info.PriceInCharterPrice = SQLServerUtil.dbDecimal(rdr["PriceInCharterPrice"]);
                if (SQLHelper.dbBit(rdr["CharterFixFlag"]))
                {
                    rt_info.CharterTaxDispKbnNm = (rdr["CharterTaxDispKbnNm"]).ToString();
                }
                else
                {
                    //未確定の場合は"●"をセット
                    rt_info.CharterTaxDispKbnNm = UNFIX_MARK;
                }
                rt_info.TollFeeInCharterPrice = SQLServerUtil.dbDecimal(rdr["TollFeeInCharterPrice"]);
                rt_info.CharterAddUpYMD = SQLHelper.dbDate(rdr["CharterAddUpYMD"]);
                rt_info.FixFlag = SQLHelper.dbBit(rdr["FixFlag"]);
                rt_info.CharterFixFlag = SQLHelper.dbBit(rdr["CharterFixFlag"]);
                rt_info.Memo = (rdr["Memo"]).ToString();
                rt_info.FutaigyomuryoInPrice = SQLServerUtil.dbDecimal(rdr["FutaigyomuryoInPrice"]);
                rt_info.JomuinUriageKingaku = SQLServerUtil.dbDecimal(rdr["JomuinUriageKingaku"]);
                if (SQLHelper.dbBit(rdr["JomuinUriageDogakuFlag"]))
                {
                    rt_info.JomuinUriageDogakuFlag = DOGAKU_MARK;
                }
                else
                {
                    rt_info.JomuinUriageDogakuFlag = string.Empty;
                }
                rt_info.MagoYoshasaki = (rdr["MagoYoshasaki"]).ToString();
                rt_info.ReceivedFlag = SQLHelper.dbBit(rdr["ReceivedFlag"]);
                rt_info.ExistsHaishaFlag = (rdr["ExistsHaishaFlag"]).ToString();
                rt_info.ExistsRenkeiFlag = (rdr["ExistsRenkeiFlag"]).ToString();

                //返却用の値を返します
                return rt_info;
            }, null);

            if (list == null || list.Count == 0)
            {
                return new List<JuchuIchiranInfo>();
            }

            return list.ToList();
        }

        #endregion

        #region プライベートメソッド

        /// <summary>
        /// SELECT_SQLを取得します。
        /// </summary>
        /// <param name="para">抽出条件</param>
        /// <returns>SELECT_SQL</returns>
        private string GetSelectJuchuSQL(JuchuIchiranSearchParameter para)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine(" SELECT ");
            sb.AppendLine("   Juchu.JuchuId                       AS JuchuId, ");
            sb.AppendLine("   Juchu.JuchuSlipNo                   AS JuchuSlipNo, ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeSNM AS BranchOfficeSNM, ");
            sb.AppendLine("   TORADON_Car.CarCd                   AS CarCd, ");
            sb.AppendLine("   Juchu.LicPlateCarNo                 AS LicPlateCarNo, ");
            sb.AppendLine("   TORADON_CarKind.CarKindSNM          AS CarKindSNM, ");
            sb.AppendLine("   Driver.StaffNm                      AS DriverNm, ");
            sb.AppendLine("   Juchu.TokuisakiNM                   AS TokuisakiNM, ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassNM         AS ClmClassNM, ");
            sb.AppendLine("   TORADON_Contract.ContractNM         AS ContractNM, ");
            sb.AppendLine("   Juchu.TaskStartDateTime             AS TaskStartDateTime, ");
            sb.AppendLine("   Juchu.TaskEndDateTime               AS TaskEndDateTime, ");
            sb.AppendLine("   Juchu.ReuseYMD                      AS ReuseYMD, ");
            sb.AppendLine("   Juchu.StartPointNM                  AS StartPointNM, ");
            sb.AppendLine("   Juchu.EndPointNM                    AS EndPointNM, ");
            sb.AppendLine("   Juchu.ItemNM                        AS ItemNM, ");
            sb.AppendLine("   Juchu.OwnerNM                       AS OwnerNM, ");
            sb.AppendLine("   JuchTanto.StaffNm                   AS JuchTantoNm,");
            sb.AppendLine("   Hanro.HanroName                     AS HanroNm, ");
            sb.AppendLine("   OfukuKbn.SystemNameName             AS OfukuKbnNm, ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiNm     AS CarOfChartererShortNm, ");
            sb.AppendLine("   Juchu.Number                        AS Number, ");
            sb.AppendLine("   TORADON_Fig.FigNm                   AS FigNm, ");
            sb.AppendLine("   Juchu.AtPrice                       AS AtPrice, ");
            sb.AppendLine("   Juchu.Weight                        AS Weight, ");
            sb.AppendLine("   Juchu.PriceInPrice                  AS PriceInPrice, ");
            sb.AppendLine("   Juchu.FixFlag                       AS FixFlag, ");
            sb.AppendLine("   TaxDispKbn.SystemNameName           AS TaxDispKbnNm, ");
            sb.AppendLine("   Juchu.TollFeeInPrice                AS TollFeeInPrice, ");
            sb.AppendLine("   Juchu.AddUpYMD                      AS AddUpYMD, ");
            sb.AppendLine("   Juchu.PriceInCharterPrice           AS PriceInCharterPrice, ");
            sb.AppendLine("   Juchu.CharterFixFlag                AS CharterFixFlag, ");
            sb.AppendLine("   CharterTaxDispKbn.SystemNameName    AS CharterTaxDispKbnNm, ");
            sb.AppendLine("   Juchu.TollFeeInCharterPrice         AS TollFeeInCharterPrice, ");
            sb.AppendLine("   Juchu.CharterAddUpYMD               AS CharterAddUpYMD, ");
            sb.AppendLine("   Juchu.FixFlag                       AS FixFlag, ");
            sb.AppendLine("   Juchu.CharterFixFlag                AS CharterFixFlag, ");
            sb.AppendLine("   Juchu.Memo                          AS Memo, ");
            sb.AppendLine("   Juchu.FutaigyomuryoInPrice          AS FutaigyomuryoInPrice, ");
            sb.AppendLine("   Juchu.JomuinUriageDogakuFlag        AS JomuinUriageDogakuFlag, ");
            sb.AppendLine("   Juchu.JomuinUriageKingaku           AS JomuinUriageKingaku, ");
            sb.AppendLine("   Juchu.MagoYoshasaki                 AS MagoYoshasaki, ");
            sb.AppendLine("   Juchu.ReceivedFlag                  AS ReceivedFlag, ");
            sb.AppendLine("   CASE WHEN EXISTS ( ");
            sb.AppendLine("    SELECT * FROM Haisha ");
            sb.AppendLine("    WHERE Haisha.JuchuId = Juchu.JuchuId AND Haisha.DelFlag = 0 ");
            sb.AppendLine("   ) THEN '" + HAISHAZUMI_MARK + "' ELSE '' END AS ExistsHaishaFlag, ");
            sb.AppendLine("   CASE WHEN EXISTS ( ");
            sb.AppendLine("    SELECT * FROM Haisha ");
            sb.AppendLine("    INNER JOIN HaishaSeikyuRenkeiManage ");
            sb.AppendLine("     ON HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId ");
            sb.AppendLine("    WHERE Haisha.JuchuId = Juchu.JuchuId AND Haisha.DelFlag = 0 ");
            sb.AppendLine("   ) THEN '" + RENKEIZUMI_MARK + "' ELSE '' END AS ExistsRenkeiFlag ");
            sb.AppendLine(" FROM Juchu ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_BranchOffice ON ");
            sb.AppendLine("   TORADON_BranchOffice.BranchOfficeId = Juchu.BranchOfficeId AND ");
            sb.AppendLine("   TORADON_BranchOffice.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Car ON ");
            sb.AppendLine("   TORADON_Car.CarId = Juchu.CarId AND ");
            sb.AppendLine("   TORADON_Car.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_CarKind ON ");
            sb.AppendLine("   TORADON_CarKind.CarKindId = Juchu.CarKindId AND ");
            sb.AppendLine("   TORADON_CarKind.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff Driver ON ");
            sb.AppendLine("   Driver.StaffId = Juchu.DriverId AND ");
            sb.AppendLine("   Driver.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_ClmClass ON ");
            sb.AppendLine("   TORADON_ClmClass.ClmClassId = Juchu.ClmClassId AND ");
            sb.AppendLine("   TORADON_ClmClass.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Contract ON ");
            sb.AppendLine("   TORADON_Contract.ContractId = Juchu.ContractId AND ");
            sb.AppendLine("   TORADON_Contract.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Staff JuchTanto ON ");
            sb.AppendLine("   JuchTanto.StaffId = Juchu.JuchuTantoId AND ");
            sb.AppendLine("   JuchTanto.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Torihikisaki ON ");
            sb.AppendLine("   TORADON_Torihikisaki.TorihikiId = Juchu.CarOfChartererId AND ");
            sb.AppendLine("   TORADON_Torihikisaki.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" Hanro ON ");
            sb.AppendLine("   Hanro.HanroId = Juchu.HanroId AND ");
            sb.AppendLine("   Hanro.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" TORADON_Fig ON ");
            sb.AppendLine("   TORADON_Fig.FigId = Juchu.FigId AND ");
            sb.AppendLine("   TORADON_Fig.DelFlag = 0 ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName TaxDispKbn ON ");
            sb.AppendLine("   TaxDispKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   TaxDispKbn.SystemNameCode = Juchu.TaxDispKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName CharterTaxDispKbn ON ");
            sb.AppendLine("   CharterTaxDispKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.TaxDispKbnShort).ToString() + " AND ");
            sb.AppendLine("   CharterTaxDispKbn.SystemNameCode = Juchu.CharterTaxDispKbn ");
            sb.AppendLine(" LEFT JOIN ");
            sb.AppendLine(" SystemName OfukuKbn ON ");
            sb.AppendLine("   OfukuKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemNameKbn.OfukuKbn).ToString() + " AND ");
            sb.AppendLine("   OfukuKbn.SystemNameCode = Juchu.OfukuKbn ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine("   Juchu.DelFlag = 0 ");

            // 日付指定区分によって対象日付の比較項目を判断
            if ((int)BizProperty.DefaultProperty.FilterDateKbns.TaskStartDate == para.FilterDateKbns)
            {
                // 作業開始日付
                sb.AppendLine(" AND ( Juchu.TaskStartDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
                sb.AppendLine(" OR Juchu.TaskStartDateTime IS NULL )");
            }
            else if ((int)BizProperty.DefaultProperty.FilterDateKbns.TaskEndDate == para.FilterDateKbns)
            {
                // 作業終了日付
                sb.AppendLine(" AND Juchu.TaskEndDateTime BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
            }
            else if ((int)BizProperty.DefaultProperty.FilterDateKbns.AddUpDate == para.FilterDateKbns)
            {
                // 計上日付
                sb.AppendLine(" AND Juchu.AddUpYMD BETWEEN " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDFrom) + " AND " + SQLHelper.DateTimeToDbDateTime(para.TaishoYMDTo) + " ");
            }
            // 車両区分
            if (0 < para.CarId)
            {
                sb.AppendLine(" AND Juchu.CarId = " + para.CarId.ToString() + " ");
            }
            // 乗務員ID
            if (0 < para.DriverId)
            {
                sb.AppendLine(" AND Juchu.DriverId = " + para.DriverId.ToString() + " ");
            }
            // 傭車先ID
            if (0 < para.CarOfChartererId)
            {
                sb.AppendLine(" AND Juchu.CarOfChartererId = " + para.CarOfChartererId.ToString() + " ");
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
            // 営業所ID
            if (0 < para.BranchOfficeId)
            {
                sb.AppendLine(" AND Juchu.BranchOfficeId = " + para.BranchOfficeId.ToString() + " ");
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
            // 未配車のみフラグ
            if (para.MiHaishaNomiFlag)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine("   NOT EXISTS ( ");
                sb.AppendLine("    SELECT * FROM Haisha ");
                sb.AppendLine("    WHERE Haisha.JuchuId = Juchu.JuchuId AND Haisha.DelFlag = 0 ");
                sb.AppendLine("   ) ");
            }
            // 未連携のみフラグ
            if (para.MiRenkeiNomiFlag)
            {
                sb.AppendLine(" AND ");
                sb.AppendLine("   NOT EXISTS ( ");
                sb.AppendLine("    SELECT * FROM Haisha ");
                sb.AppendLine("    INNER JOIN HaishaSeikyuRenkeiManage ");
                sb.AppendLine("     ON HaishaSeikyuRenkeiManage.HaishaId = Haisha.HaishaId ");
                sb.AppendLine("    WHERE Haisha.JuchuId = Juchu.JuchuId AND Haisha.DelFlag = 0 ");
                sb.AppendLine("   ) ");
            }

            sb.AppendLine(" ORDER BY ");
            sb.AppendLine("   Juchu.JuchuSlipNo, Juchu.JuchuId ");

            return sb.ToString();
        }

        #endregion
    }
}
