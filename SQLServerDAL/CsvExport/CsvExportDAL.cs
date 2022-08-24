using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Jpsys.SagyoManage.BizProperty;
using jp.co.jpsys.util;

namespace Jpsys.SagyoManage.SQLServerDAL.CsvExport
{
    public class ExportUtil
    {
        public static string sqlStringGenerator(
            string table_name,
            string where,
            IEnumerable<string> items,
            Dictionary<string, string> joiners,
            bool hideDisabledData, //非表示フラグ
            bool disableflag_aritable //非表示フラグがあるテーブル
            )
        {
            StringBuilder sb = new StringBuilder();

            //先頭にテーブル名を付ける
            var converted_items = items.Select(item => item.Contains(".") ?
                String.Format("{0}", item) :
                String.Format("{0}.{1}", table_name, item));

            var converted_joiner = (joiners == null) ? new List<string> { } :
                joiners.Select(joiner => String.Format("LEFT OUTER JOIN {0} ON {1}", joiner.Key, joiner.Value));

            sb.AppendLine("Select ");
            sb.AppendLine(converted_items.Aggregate((acc, item) => acc + ",\n" + item));
            sb.AppendLine(String.Format("FROM {0}", table_name));
            if (converted_joiner.Count() > 0)
            {
                sb.AppendLine(converted_joiner.Aggregate((acc, join) => acc + "\n" + join));
            }

            //削除フラグ
            sb.AppendLine(String.Format("WHERE {0}.DelFlag = ", table_name) + (NSKUtil.BoolToInt(false)).ToString() + " ");

            if (where != null)
            {
                sb.AppendLine(String.Format("AND {0}", where));
            }

            //非表示フラグ
            if (disableflag_aritable &&　hideDisabledData)
            {
                sb.AppendLine(String.Format("AND {0}.DisableFlag = ", table_name) + NSKUtil.BoolToInt(false) + " ");
            }

            var result = sb.ToString();
            Console.WriteLine(result);
            return result;
        }
    }

    public class Exporter : CsvExportDALBase
    {
        public string name;
        public string table_name;
        public string where;
        public IEnumerable<string> columns;
        public Dictionary<string, string> joiners;
        /// <summary>
        /// 非表示フラグがあるテーブルの場合trueを設定　初期値は、true。
        /// </summary>
        public bool disableflag_aritable = true;

        public override string GetName()
        {
            return name;
        }

        protected override string GetSelectoinDataSqlWithoutOrderBy(bool hideDisabledData)
        {
            return ExportUtil.sqlStringGenerator(table_name, where, columns, joiners, hideDisabledData, disableflag_aritable);
        }

        protected override string GetOrderByClause()
        {
            return String.Format("ORDER BY {0}.{1}コード", table_name, name);
        }
    }

    #region 各テーブルのパリックメソッド

    #region 配車Aceシステムのテーブル

    /// <summary>
    /// 得意先グループ
    /// </summary>
    public class TokuisakiGroupExporter : Exporter
    {
        public TokuisakiGroupExporter()
        {
            this.name = "得意先グループ";

            this.table_name = "TokuisakiGroup";

            this.columns = new List<string> {

                "TokuisakiGroup.TokuisakiGroupCode	得意先グループコード ",
                "TokuisakiGroup.TokuisakiGroupName	得意先グループ名称 ",
                "ISNULL(TokuisakiGroupMeisai.Gyo, 0)	明細行 ",
                "ISNULL(TORADON_Tokuisaki.TokuisakiCd, 0)	得意先コード ",
                "ISNULL(TORADON_Tokuisaki.TokuisakiNM, '')	得意先名称 ",
                "ISNULL(TORADON_Tokuisaki.TokuisakiSNM, '')	得意先名称の略称 ",
                String.Format("CASE {0}.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ" , table_name) + " ",

            };

            this.joiners = new Dictionary<string, string>{

                //得意先グループ明細
                {"TokuisakiGroupMeisai ",
                    "TokuisakiGroupMeisai.TokuisakiGroupId = TokuisakiGroup.TokuisakiGroupId " } ,
                //得意先
                {"TORADON_Tokuisaki ",
                    "TORADON_Tokuisaki.TokuisakiId = TokuisakiGroupMeisai.ToraDONTokuisakiId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TokuisakiGroupCode, TokuisakiGroupMeisai.Gyo ";
        }
    }

    /// <summary>
    /// 方面グループ
    /// </summary>
    public class HomenGroupExporter : Exporter
    {
        public HomenGroupExporter()
        {
            this.name = "方面グループ";

            this.table_name = "HomenGroup";

            this.columns = new List<string> {

                "HomenGroup.HomenGroupCode	方面グループコード ",
                "HomenGroup.HomenGroupName	方面グループ名称 ",
                "ISNULL(HomenGroupMeisai.Gyo, 0)	明細行 ",
                "ISNULL(Homen.HomenCode, 0)	方面コード ",
                "ISNULL(Homen.HomenName, '')	方面名称 ",
                "ISNULL(Homen.HomenNameKana, '')	方面フリガナ ",
                String.Format("CASE {0}.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ" , table_name) + " ",

            };

            this.joiners = new Dictionary<string, string>{

                //方面グループ明細
                {"HomenGroupMeisai ",
                    "HomenGroupMeisai.HomenGroupId = HomenGroup.HomenGroupId " } ,
                //方面
                {"Homen ",
                    "Homen.HomenId = HomenGroupMeisai.HomenId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY HomenGroupCode, ISNULL(HomenGroupMeisai.Gyo, 0) ";
        }
    }

    /// <summary>
    /// 車種グループ
    /// </summary>
    public class CarKindGroupExporter : Exporter
    {
        public CarKindGroupExporter()
        {
            this.name = "車種グループ";

            this.table_name = "CarKindGroup";

            this.columns = new List<string> {

                "CarKindGroup.CarKindGroupCode	車種グループコード ",
                "CarKindGroup.CarKindGroupName	車種グループ名称 ",
                "ISNULL(CarKindGroupMeisai.Gyo, 0)	明細行 ",
                "ISNULL(TORADON_CarKind.CarKindCd, 0)	車種コード ",
                "ISNULL(TORADON_CarKind.CarKindNM, '')	車種名称 ",
                "ISNULL(TORADON_CarKind.CarKindSNM, '')	車種名称の略称 ",
                String.Format("CASE {0}.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ" , table_name) + " ",

            };

            this.joiners = new Dictionary<string, string>{

                //車種グループ明細
                {"CarKindGroupMeisai ",
                    "CarKindGroupMeisai.CarKindGroupId = CarKindGroup.CarKindGroupId " } ,
                //車種
                {"TORADON_CarKind ",
                    "TORADON_CarKind.CarKindId = CarKindGroupMeisai.ToraDONCarKindId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY CarKindGroupCode, CarKindGroupMeisai.Gyo ";
        }
    }

    /// <summary>
    /// 販路
    /// </summary>
    public class HanroExporter : Exporter
    {
        public HanroExporter()
        {
            this.name = "販路";

            this.table_name = "Hanro";

            this.columns = new List<string> {

                "Hanro.HanroCode	販路コード ",
                "Hanro.HanroName	販路名称 ",
                "Hanro.HanroNameKana	フリガナ ",
                "Hanro.HanroSName	略名称 ",
                "Hanro.HanroSSName	略略称 ",
                "TORADON_Tokuisaki.TokuisakiCd	得意先コード ",
                "TORADON_Tokuisaki.TokuisakiNM	得意先名称 ",
                "Hatchi.PointCd	発地コード ",
                "Hatchi.PointNM	発地名称 ",
                "Chakuchi.PointCd	着地コード ",
                "Chakuchi.PointNM	着地名称 ",
                "TORADON_Item.ItemCd	品目コード ",
                "TORADON_Item.ItemNM	品目名称 ",
                "TORADON_CarKind.CarKindCd	車種コード ",
                "TORADON_CarKind.CarKindNM	車種名称 ",
                "TORADON_Car.CarCd	車両コード ",
                "TORADON_Car.LicPlateCarNo	車番 ",
                "TORADON_Torihikisaki.TorihikiCd	傭車先コード ",
                "TORADON_Torihikisaki.TorihikiNM	傭車先名称 ",
                "ISNULL(Hanro.OfukuKbn, 0)	往路区分 ",
                "ISNULL(SystemName.SystemNameName, '')	往路区分名称 ",
                "Hanro.KoteiNissu	行程日数 ",
                "Hanro.KoteiJikan	行程時間 ",
                String.Format("CASE {0}.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ" , table_name) + " ",

            };

            this.joiners = new Dictionary<string, string>{
                
                //得意先
                {"TORADON_Tokuisaki ",
                    "TORADON_Tokuisaki.TokuisakiId = Hanro.ToraDONTokuisakiId " } ,
                //発地
                {"TORADON_Point Hatchi ",
                    "Hatchi.PointId = Hanro.ToraDONHatchiId " } ,
                //発地
                {"TORADON_Point Chakuchi ",
                    "Chakuchi.PointId = Hanro.ToraDONChakuchiId " } ,
                //品目
                {"TORADON_Item ",
                    "TORADON_Item.ItemId = Hanro.ToraDONItemId " } ,
                //車種
                {"TORADON_CarKind ",
                    "TORADON_CarKind.CarKindId = Hanro.ToraDONCarKindId " } ,
                //車両
                {"TORADON_Car ",
                    "TORADON_Car.CarId = Hanro.ToraDONCarId " } ,
                //傭車先
                {"TORADON_Torihikisaki ",
                    "TORADON_Torihikisaki.TorihikiId = Hanro.ToraDONTorihikiId " } ,
                //往路区分
                {"SystemName ",
                    "SystemName.SystemNameKbn = " + ((int)DefaultProperty.SystemNameKbn.OfukuKbn).ToString() + " AND " +
                    "SystemName.SystemNameCode = Hanro.OfukuKbn "} ,
            };

        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY Hanro.HanroCode ";
        }
    }

    /// <summary>
    /// 方面
    /// </summary>
    public class HomenExporter : Exporter
    {
        public HomenExporter()
        {
            this.name = "方面";

            this.table_name = "Homen";

            this.columns = new List<string> {

                "Homen.HomenCode	方面コード ",
                "Homen.HomenName	方面名称 ",
                "Homen.HomenNameKana	フリガナ ",
                String.Format("CASE {0}.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ" , table_name) + " ",

            };

        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY Homen.HomenCode ";
        }
    }

    /// <summary>
    /// 休日カレンダ
    /// </summary>
    public class KyujitsuCalendarExporter : Exporter
    {
        public KyujitsuCalendarExporter()
        {
            this.name = "休日カレンダ";

            this.table_name = "KyujitsuCalendar";

            this.disableflag_aritable = false;

            this.columns = new List<string> {

                "KyujitsuCalendar.Nendo	年度 ",
                "TORADON_Staff.StaffCd	乗務員コード ",
                "TORADON_Staff.StaffNM	乗務員名称 ",
                "KyujitsuCalendarMeisai.HizukeYMD	日付 ",
                "ISNULL(KyujitsuCalendarMeisai.KyujitsuKbn, 0)	休日区分 ",
                "ISNULL(SystemName.SystemNameName, '')	休日区分名称 ",

            };

            this.joiners = new Dictionary<string, string>{
                
                //社員
                {"TORADON_Staff ",
                    "TORADON_Staff.StaffId = KyujitsuCalendar.ToraDONStaffId " } ,
                //休日カレンダ明細
                {"KyujitsuCalendarMeisai ",
                    "KyujitsuCalendarMeisai.KyujitsuCalendarId = KyujitsuCalendar.KyujitsuCalendarId " } ,
                //休日区分
                {"SystemName ",
                    "SystemName.SystemNameKbn = " + ((int)DefaultProperty.SystemNameKbn.KyujitsuKbn).ToString() + " AND " +
                    "SystemName.SystemNameCode = KyujitsuCalendarMeisai.KyujitsuKbn "} ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY KyujitsuCalendar.Nendo, TORADON_Staff.StaffCd, KyujitsuCalendarMeisai.HizukeYMD, ISNULL(KyujitsuCalendarMeisai.KyujitsuKbn, 0) ";
        }
    }

    /// <summary>
    /// 品目分類
    /// </summary>
    public class ItemBunruiExporter : Exporter
    {
        public ItemBunruiExporter()
        {
            this.name = "品目分類";

            this.table_name = "ItemLBunrui";

            this.columns = new List<string> {

                "RIGHT('00' + ItemLBunrui.ItemLBunruiCode, 2)	品目大分類コード ",
                "ItemLBunrui.ItemLBunruiName	品目大分類名称 ",
                "RIGHT('00' + ISNULL(ItemMBunrui.ItemMBunruiCode, ''), 3)	品目中分類コード ",
                "ISNULL(ItemMBunrui.ItemMBunruiName, 0)	品目中分類名称 ",
                "CASE ItemLBunrui.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 品目大分類_無効フラグ ",
                "CASE ItemMBunrui.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 品目中分類_無効フラグ ",

            };

            this.joiners = new Dictionary<string, string>{

                //品目中分類
                {"ItemMBunrui ",
                    "ItemMBunrui.ItemLBunruiId = ItemLBunrui.ItemLBunruiId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY ItemLBunruiCode, ISNULL(ItemMBunrui.ItemMBunruiCode, 0) ";
        }
    }

    /// <summary>
    /// 発着地分類
    /// </summary>
    public class PointBunruiExporter : Exporter
    {
        public PointBunruiExporter()
        {
            this.name = "発着地分類";

            this.table_name = "PointLBunrui";

            this.columns = new List<string> {

                "RIGHT('00' + PointLBunrui.PointLBunruiCode, 2)	発着地大分類コード ",
                "PointLBunrui.PointLBunruiName	発着地大分類名称 ",
                "RIGHT('00' + ISNULL(PointMBunrui.PointMBunruiCode, ''), 3)	発着地中分類コード ",
                "ISNULL(PointMBunrui.PointMBunruiName, 0)	発着地中分類名称 ",
                "CASE PointLBunrui.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 発着地大分類_無効フラグ ",
                "CASE PointMBunrui.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 発着地中分類_無効フラグ ",

            };

            this.joiners = new Dictionary<string, string>{

                //発着地中分類
                {"PointMBunrui ",
                    "PointMBunrui.PointLBunruiId = PointLBunrui.PointLBunruiId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY PointLBunruiCode, ISNULL(PointMBunrui.PointMBunruiCode, 0) ";
        }
    }

    #endregion

    #region トラDON補

    /// <summary>
    /// 車両（トラDON補）
    /// </summary>
    public class CarExporter : Exporter
    {
        public CarExporter()
        {
            this.name = "車両（トラDON補）";

            this.table_name = "TORADON_Car";

            this.columns = new List<string> {

                "TORADON_Car.CarCd	車両コード ",
                "TORADON_Car.LicPlateDeptNM	陸運名 ",
                "TORADON_Car.LicPlateCarKindKbn	分類区分 ",
                "TORADON_Car.LicPlateUsageKbn	業態区分 ",
                "TORADON_Car.LicPlateCarNo	車番 ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeCd, 0)	営業所コード ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeNM, '')	営業所名称 ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeSNM, '')	営業所名称の略称 ",
                "ISNULL(TORADON_Torihikisaki.TorihikiCd, 0)	傭車先コード ",
                "ISNULL(TORADON_Torihikisaki.TorihikiNM, '')	傭車先名称 ",
                "ISNULL(TORADON_Torihikisaki.TorihikiSNM, '')	傭車先名称の略称 ",
                "ISNULL(TORADON_Torihikisaki.TorihikiNMK, '')	傭車先名称のフリガナ ",
                "CASE ISNULL(Car.HaishaNyuryokuCarCountExclusionFlag, 0) WHEN 1 THEN 'レ' ELSE '' END	配車入力車両台数未カウントフラグ ",
                "CASE TORADON_Car.DisableFlag WHEN 1 THEN 'レ' ELSE '' END	無効フラグ_トラDON ",

            };

            this.joiners = new Dictionary<string, string>{
 
                //営業所
                {"TORADON_BranchOffice ",
                    "TORADON_BranchOffice.BranchOfficeId = TORADON_Car.BranchOfficeId " } ,
 
                //車両（トラDON補）
                {"Car ",
                    "TORADON_Car.CarId = Car.ToraDONCarId " } ,

                //傭車先
                {"TORADON_Torihikisaki ",
                    "TORADON_Torihikisaki.TorihikiId = Car.YoshasakiId " } ,

            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TORADON_Car.CarCd";
        }
    }

    /// <summary>
    /// 品目（トラDON補）
    /// </summary>
    public class ItemExporter : Exporter
    {
        public ItemExporter()
        {
            this.name = "品目（トラDON補）";

            this.table_name = "TORADON_Item";

            this.columns = new List<string> {

                "TORADON_Item.ItemCd	品目コード ",
                "TORADON_Item.ItemNM	品目名称 ",
                "Item.ItemSName	品目名称の略称 ",
                "Item.ItemSSName	品目名称の略略称 ",
                "TORADON_Item.ItemNMK フリガナ ",
                "CASE TORADON_Item.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ_トラDON ",

            };

            this.joiners = new Dictionary<string, string>{

                //品目（トラDON補）ID
                {"Item ",
                    "TORADON_Item.ItemId = Item.ToraDONItemId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TORADON_Item.ItemCd";
        }
    }

    /// <summary>
    /// 発着地（トラDON補）
    /// </summary>
    public class PointExporter : Exporter
    {
        public PointExporter()
        {
            this.name = "発着地（トラDON補）";

            this.table_name = "TORADON_Point";

            this.columns = new List<string> {

                "TORADON_Point.PointCd	発着地コード ",
                "TORADON_Point.PointNM	発着地名称 ",
                "Point.PointSName	発着地名称の略称 ",
                "Point.PointSSName	発着地名称の略略称 ",
                "TORADON_Point.PointNMK フリガナ ",
                "CASE TORADON_Point.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ_トラDON ",

            };

            this.joiners = new Dictionary<string, string>{

                //発着地（トラDON補）ID
                {"Point ",
                    "TORADON_Point.PointId = Point.ToraDONPointId " } ,
            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TORADON_Point.PointCd";
        }
    }

    /// <summary>
    /// 車種（トラDON補）
    /// </summary>
    public class CarKindExporter : Exporter
    {
        public CarKindExporter()
        {
            this.name = "車種（トラDON補）";

            this.table_name = "TORADON_CarKind";

            this.columns = new List<string> {

                "TORADON_CarKind.CarKindCd	車種コード ",
                "TORADON_CarKind.CarKindNM	車種名称 ",
                "TORADON_CarKind.CarKindSNM	車種名称の略称 ",
                "TORADON_CarKind.CarKindNMK	フリガナ ",
                "ISNULL(CarKind.JomuinKakeritsu, 0)	掛率 ",
                "ISNULL(RenkeiCarKind.CarKindCd, 0)	トラDON連携車種コード ",
                "ISNULL(RenkeiCarKind.CarKindNM, '')	トラDON連携車種名称 ",
                "ISNULL(RenkeiCarKind.CarKindSNM, '')	トラDON連携車種名称の略称 ",
                "ISNULL(RenkeiCarKind.CarKindNMK, '')	トラDON連携車種のフリガナ ",
                "CASE TORADON_CarKind.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ_トラDON ",

            };

            this.joiners = new Dictionary<string, string>{
 
                //車種（トラDON補）
                {"CarKind ",
                    "TORADON_CarKind.CarKindId = CarKind.ToraDONCarKindId " } ,

                //トラDON連携車種
                {"TORADON_CarKind RenkeiCarKind ",
                    "RenkeiCarKind.CarKindId = CarKind.ToraDONRenkeiCarKindId " } ,

            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TORADON_CarKind.CarKindCd";
        }
    }

    /// <summary>
    /// 社員（トラDON補）
    /// </summary>
    public class StaffExporter : Exporter
    {
        public StaffExporter()
        {
            this.name = "社員（トラDON補）";

            this.table_name = "TORADON_Staff";

            this.columns = new List<string> {

                "TORADON_Staff.StaffCd	社員コード ",
                "TORADON_Staff.StaffNM	社員名称 ",
                "TORADON_Staff.StaffSNM	社員名称の略称 ",
                "TORADON_Staff.StaffNMK	フリガナ ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeCd, 0)	営業所コード ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeNM, '')	営業所名称 ",
                "ISNULL(TORADON_BranchOffice.BranchOfficeSNM, '')	営業所名称の略称 ",
                "ISNULL(Staff.GroupNo, 0)	グループ番号 ",
                "CASE ISNULL(Staff.DriverFlag, 0) WHEN 1 THEN 'レ' ELSE '' END 乗務員フラグ ",
                "CASE ISNULL(Staff.JuchuTantoshaFlag, 0) WHEN 1 THEN 'レ' ELSE '' END 受注担当者フラグ ",
                "ISNULL(Staff.HaishaNyuryokuCarBackColor, 0)	配車入力車両背景色 ",
                "CASE TORADON_Staff.DisableFlag WHEN 1 THEN 'レ' ELSE '' END 無効フラグ_トラDON ",

            };

            this.joiners = new Dictionary<string, string>{
 
                //営業所
                {"TORADON_BranchOffice ",
                    "TORADON_BranchOffice.BranchOfficeId = TORADON_Staff.BranchOfficeId " } ,

                //社員（トラDON補）
                {"Staff ",
                    "TORADON_Staff.StaffId = Staff.ToraDONStaffId " } ,

            };
        }

        protected override string GetOrderByClause()
        {
            return "ORDER BY TORADON_Staff.StaffCd";
        }
    }

    #endregion

    #endregion

}
