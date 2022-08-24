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
    /// 運転者配車実績明細書のデータアクセスレイヤです。
    /// </summary>
    public class UntenshaHaishaJissekiMeisaiShoRpt
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
        public UntenshaHaishaJissekiMeisaiShoRpt()
        {
        }

        /// <summary>
        /// アプリケーション認証情報を指定して、
        /// データアクセスレイヤのインスタンスを初期化します。
        /// </summary>
        /// <param name="authInfo">アプリケーション認証情報</param>
        public UntenshaHaishaJissekiMeisaiShoRpt(AppAuthInfo authInfo)
        {
            this.authInfo = authInfo;
        }

        /// <summary>
        /// 抽出条件情報を指定して、運転者配車実績明細書情報のリストを取得します。
        /// </summary>
        /// <param name="searchParameter">抽出条件情報</param>
        /// <returns>運転者配車実績明細書情報のリスト</returns>
        public List<UntenshaHaishaJissekiMeisaiShoRptInfo> GetReportData(UntenshaHaishaJissekiMeisaiShoConditionInfo para)
        {
            //返却用リストを返却します
            return GetListInternal(para);
        }

        #region プライベートメソッド

        /// <summary>
        /// 検索条件情報を指定して、運転者配車実績明細書情報リストを取得します。
        /// </summary>
        /// <param name="para">検索条件情報</param>
        /// <returns>運転者配車実績明細書情報リスト</returns>
        private List<UntenshaHaishaJissekiMeisaiShoRptInfo> GetListInternal(UntenshaHaishaJissekiMeisaiShoConditionInfo para)
        {
            //返却用のリスト
            List<UntenshaHaishaJissekiMeisaiShoRptInfo> rt_list = new List<UntenshaHaishaJissekiMeisaiShoRptInfo>();

            //SQL文を作成
            StringBuilder sb = new StringBuilder();

            #region SQl文

            sb.AppendLine("WITH base as( ");
            sb.AppendLine("	SELECT ");
            sb.AppendLine("		ShozokuGroup.ShozokuGroupCd --所属コード ");
            sb.AppendLine("		,MAX(ShozokuGroup.ShozokuGroupNM) ShozokuGroupNM --所属名 ");
            sb.AppendLine("		,TORADON_Staff.StaffCd --乗務員コード ");
            sb.AppendLine("		,MAX(TORADON_Staff.StaffNM) StaffNM --乗務員名 ");
            sb.AppendLine("		,TORADON_CarKind.CarKindCd --車種コード        ");
            sb.AppendLine("		,MAX(TORADON_CarKind.CarKindNM) CarKindNM --車種名 ");
            sb.AppendLine("		,Haisha.TsumikomiYMD --積込日 ");
            sb.AppendLine("		,Haisha.TsumikomiKbn --積込区分 ");
            sb.AppendLine("		,MAX(TsumikomiKbn.SystemNameNM) TsumikomiKbnNM --積込区分名 ");
            sb.AppendLine("		,TORADON_Car.CarCd --車両コード ");
            sb.AppendLine("		,Haisha.KyuyoShiharaiYMD --給与支給日 ");
            sb.AppendLine("		,Hatchi.PointCd HatchiCd --発地コード ");
            sb.AppendLine("		,MAX(Hatchi.PointNM) HatchiNM --発地名 ");
            sb.AppendLine("		,Chakuchi.PointCd ChakuchiCd --着地コード ");
            sb.AppendLine("		,MAX(Chakuchi.PointNM) ChakuchiNM --着地名 ");
            sb.AppendLine("		,Nojo.NojoCd --農場コード ");
            sb.AppendLine("		,MAX(Nojo.NojoNM) NojoNM --農場名 ");
            sb.AppendLine("		,MAX(Haisha.RenketsuKbn) RenketsuKbn --連結区分 ");
            sb.AppendLine("		,MAX(Shoteate.Naiyo) DaihyoTeate --代表手当 ");
            sb.AppendLine("		,SUM(Haisha.Seikyusu) Seikyusu --請求数 ");
            sb.AppendLine("		,MAX(Haisha.SeikyuTanka) SeikyuTanka --請求単価	 ");
            sb.AppendLine("		,SUM(Haisha.SeikyuKingaku) SeikyuKingaku  --請求金額 ");
            sb.AppendLine("		,SUM(HaishaUriageTeate.Uriagesu) HoshoGoSu --保証後数 ");
            sb.AppendLine("		,MAX(HaishaUriageTeate.UriageTanka) HoshoGoTanka --保証後単価	 ");
            sb.AppendLine("		,SUM(HaishaUriageTeate.UriageKingaku) HoshoGoKingaku --保証後金額 ");
            sb.AppendLine("		,MAX(Haisha.HoshoTaishoKbn) HoshoTaishoKbn --保証対象区分 ");
            sb.AppendLine("		,SUM(CASE Shoteate.ShoteateCd WHEN 0 THEN 0 ELSE CASE Haisha.TeateKingaku WHEN 0 THEN 0 ELSE Haisha.Teatesu END END) ShoTeateSu --諸手当数 ");
            sb.AppendLine("		,SUM(CASE Shoteate.ShoteateCd WHEN 0 THEN 0 ELSE Haisha.TeateKingaku END) ShoTeateKingaku --諸手当金額 ");
            sb.AppendLine("		,SUM(HaishaUriageTeate.MiniNojoSu) MiniNojoSu --ミニ農場数	 ");
            sb.AppendLine("		,MAX(HaishaUriageTeate.MiniNojoTanka) MiniNojoTanka --ミニ農場単価	 ");
            sb.AppendLine("		,SUM(HaishaUriageTeate.MiniNojoKingaku) MiniNojoKingaku --ミニ農場金額	 ");
            sb.AppendLine("	FROM ");
            sb.AppendLine("		Haisha --配車 ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		HaishaUriageTeate --配車売上手当 ");
            sb.AppendLine("	ON	Haisha.HaishaId = HaishaUriageTeate.HaishaId  ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		TORADON_Point Hatchi --トラＤＯＮシステム発着地 ");
            sb.AppendLine("	ON	Haisha.HatchiId = Hatchi.PointId  ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		TORADON_Point Chakuchi --トラＤＯＮシステム発着地 ");
            sb.AppendLine("	ON	Haisha.ChakuchiId = Chakuchi.PointId  ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		TORADON_Staff --トラＤＯＮシステム乗務員 ");
            sb.AppendLine("	ON	Haisha.StaffId = TORADON_Staff.StaffId  ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		Staff --社員（トラDON補） ");
            sb.AppendLine("	ON	Haisha.StaffId = Staff.ToraDONStaffId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		ShozokuGroup --所属グループ ");
            sb.AppendLine("	ON	Staff.ShozokuGroupId = ShozokuGroup.ShozokuGroupId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		TORADON_Car --トラＤＯＮシステム車両 ");
            sb.AppendLine("	ON	Haisha.SharyoId = TORADON_Car.CarId  ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		TORADON_CarKind --トラＤＯＮシステム車種 ");
            sb.AppendLine("	ON	TORADON_Car.CarKindId = TORADON_CarKind.CarKindId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		CarKind --車種 ");
            sb.AppendLine("	ON	TORADON_Car.CarKindId = CarKind.ToraDONCarKindId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		Nojo --農場 ");
            sb.AppendLine("	ON	Haisha.NojoId = Nojo.NojoId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		Shoteate --諸手当 ");
            sb.AppendLine("	ON	Haisha.TeateId = Shoteate.ShoteateId ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("		SystemName AS TsumikomiKbn --積込区分名称  ");
            sb.AppendLine("	ON	Haisha.TsumikomiKbn = TsumikomiKbn.SystemNameCd ");
            sb.AppendLine("	AND TsumikomiKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemGlobalNameKbn.TsumikomiKbn).ToString() + " ");
            sb.AppendLine("	WHERE ");
            sb.AppendLine("		--削除フラグ ");
            sb.AppendLine("		Haisha.DelFlag = " + NSKUtil.BoolToInt(false) + " ");

            sb.AppendLine("	--	--自傭区分 ");
            sb.AppendLine("	--AND	ISNULL(Haisha.JiyoKbn,0) = 0 "); //旧システムでは0で聞いていたが。。。要確認

            sb.AppendLine("		--乗務員ID	 ");
            sb.AppendLine("	AND	Haisha.StaffId <> 0 ");
            sb.AppendLine("		--車両ID ");
            sb.AppendLine("	AND	Haisha.SharyoId <> 0 ");

            if (para != null)
            {
                sb.AppendLine("		--給与支給日 ");
                sb.AppendLine("	AND	Haisha.KyuyoShiharaiYMD >= " + SQLHelper.DateTimeToDbDate(para.TaishoYMDFrom) + " ");
                sb.AppendLine("	AND	Haisha.KyuyoShiharaiYMD <= " + SQLHelper.DateTimeToDbDate(para.TaishoYMDTo) + " ");

                if (para.Staff_HaniShiteiChecked)
                {
                    sb.AppendLine("		--乗務員コード ");
                    sb.AppendLine("	AND ISNULL(TORADON_Staff.StaffCd,0) >= " + para.StaffCodeFrom.ToString() + " ");
                    sb.AppendLine("	AND ISNULL(TORADON_Staff.StaffCd,0) <= " + para.StaffCodeTo.ToString() + " ");
                }

                if (para.Staff_KobetsuShiteiChecked)
                {
                    //乗務員コードリスト
                    if (para.UntenshaHaishaJissekiMeisaiShoConditionStaffList != null && para.UntenshaHaishaJissekiMeisaiShoConditionStaffList.Count > 0)
                    {
                        sb.AppendLine("AND ISNULL(TORADON_Staff.StaffCd,0) IN(" + string.Join(",", para.UntenshaHaishaJissekiMeisaiShoConditionStaffList.Select(obj => obj.StaffCode)) + ") ");
                    }
                }
            }

            sb.AppendLine("	GROUP BY  ");
            sb.AppendLine("		ShozokuGroup.ShozokuGroupCd  --所属コード ");
            sb.AppendLine("		,TORADON_Staff.StaffCd --乗務員コード ");
            sb.AppendLine("		,TORADON_CarKind.CarKindCd   --車種コード ");
            sb.AppendLine("		,Haisha.TsumikomiYMD --積込日 ");
            sb.AppendLine("		,Haisha.TsumikomiKbn --積込区分 ");
            sb.AppendLine("		,TORADON_Car.CarCd  --車両コード ");
            sb.AppendLine("		,Haisha.KyuyoShiharaiYMD  --給与支給日 ");
            sb.AppendLine("		,Hatchi.PointCd  --発地コード ");
            sb.AppendLine("		,Chakuchi.PointCd  --着地 コード ");
            sb.AppendLine("		,Nojo.NojoCd --農場コード ");
            sb.AppendLine(") ");
            sb.AppendLine("SELECT ");
            sb.AppendLine("	base.* ");
            sb.AppendLine("	,(RenketsuKbn.SystemNameNM) RenketsuKbnNM	 ");
            sb.AppendLine("FROM ");
            sb.AppendLine("	base ");
            sb.AppendLine("	LEFT OUTER JOIN  ");
            sb.AppendLine("	SystemName AS RenketsuKbn --連結区分名称  ");
            sb.AppendLine("ON	base.RenketsuKbn = RenketsuKbn.SystemNameCd  ");
            sb.AppendLine("AND RenketsuKbn.SystemNameKbn = " + ((int)BizProperty.DefaultProperty.SystemGlobalNameKbn.RenketsuKbn).ToString() + " ");
            sb.AppendLine("ORDER BY  ");
            sb.AppendLine("	base.ShozokuGroupCd ");
            sb.AppendLine("	,base.StaffCd ");
            sb.AppendLine("	,base.CarKindCd ");
            sb.AppendLine("	,base.TsumikomiYMD ");
            sb.AppendLine("	,base.TsumikomiKbn ");
            sb.AppendLine("	,base.CarCd ");
            sb.AppendLine("	,base.KyuyoShiharaiYMD ");
            sb.AppendLine("	,base.HatchiCd ");
            sb.AppendLine("	,base.ChakuchiCd ");
            sb.AppendLine("	,base.NojoCd ");

            #endregion

            String mySql = sb.ToString();

            return SQLHelper.SimpleRead(mySql, rdr =>
            {
                //返却用の値
                UntenshaHaishaJissekiMeisaiShoRptInfo info = new UntenshaHaishaJissekiMeisaiShoRptInfo();

                info.TaishoNengetsu = para.TaishoNengetsu;
                info.ShozokuGroupCode = SQLServerUtil.dbInt(rdr["ShozokuGroupCd"]);
                info.ShozokuGroupName = rdr["ShozokuGroupNM"].ToString();
                info.StaffCode = SQLServerUtil.dbInt(rdr["StaffCd"]);
                info.StaffName = rdr["StaffNM"].ToString();
                info.KyuyoShiharaiYMD = SQLHelper.dbDate(rdr["KyuyoShiharaiYMD"]);
                info.TsumikomiYMD = SQLHelper.dbDate(rdr["TsumikomiYMD"]);
                info.TsumikomiKbn = SQLServerUtil.dbInt(rdr["TsumikomiKbn"]);
                info.TsumikomiKbnName = rdr["TsumikomiKbnNM"].ToString();
                info.CarKindCode = SQLServerUtil.dbInt(rdr["CarKindCd"]);
                info.CarKindName = rdr["CarKindNM"].ToString();
                info.CarCode = SQLServerUtil.dbInt(rdr["CarCd"]);
                info.HatchiCode = SQLServerUtil.dbInt(rdr["HatchiCd"]);
                info.HatchiName = rdr["HatchiNM"].ToString();
                info.ChakuchiCode = SQLServerUtil.dbInt(rdr["ChakuchiCd"]);
                info.ChakuchiName = rdr["ChakuchiNM"].ToString();
                info.NojoCode = SQLServerUtil.dbInt(rdr["NojoCd"]);
                info.NojoName = rdr["NojoNM"].ToString();
                info.SeikyuSu = SQLServerUtil.dbDecimal(rdr["Seikyusu"]);
                info.SeikyuTanka = SQLServerUtil.dbDecimal(rdr["SeikyuTanka"]);
                info.SeikyuKingaku = SQLServerUtil.dbDecimal(rdr["SeikyuKingaku"]);
                info.HoshoGoSu = SQLServerUtil.dbDecimal(rdr["HoshoGoSu"]);
                info.HoshoGoTanka = SQLServerUtil.dbDecimal(rdr["HoshoGoTanka"]);
                info.HoshoGoKingaku = SQLServerUtil.dbDecimal(rdr["HoshoGoKingaku"]);
                info.HoshoTaishoKbn = SQLServerUtil.dbInt(rdr["HoshoTaishoKbn"]);
                info.DaihyoTeate = rdr["DaihyoTeate"].ToString();
                info.ShoTeateSu = SQLServerUtil.dbDecimal(rdr["ShoTeateSu"]);
                info.ShoTeateKingaku = SQLServerUtil.dbDecimal(rdr["ShoTeateKingaku"]);
                info.RenketsuKbn = SQLServerUtil.dbInt(rdr["RenketsuKbn"]);
                info.RenketsuKbnName = rdr["RenketsuKbnNM"].ToString();
                info.MiniNojoSu = SQLServerUtil.dbInt(rdr["MiniNojoSu"]);
                info.MiniNojoTanka = SQLServerUtil.dbDecimal(rdr["MiniNojoTanka"]);
                info.MiniNojoKingaku = SQLServerUtil.dbDecimal(rdr["MiniNojoKingaku"]);

                //返却用の値を返します
                return info;
            }, null);
        }

        #endregion
    }
}
