using Jpsys.SagyoManage.BizProperty;
using Jpsys.SagyoManage.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ReportModel
{
    /// <summary>
    /// 配送指示書情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class HaisoShijiShoRptInfo
    {
        /***************
         * 配車
         ***************/

        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal HaishaId { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public Decimal Number { get; set; }
        /// <summary>
        /// 単位IDを取得・設定します。
        /// </summary>
        public Decimal? FigId { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public Decimal? Weight { get; set; }
        /// <summary>
        /// 金額を取得・設定します。
        /// </summary>
        public Decimal? Price { get; set; }
        /// <summary>
        /// 金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInPrice { get; set; }
        /// <summary>
        /// 金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInPrice { get; set; }
        /// <summary>
        /// 外税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceOutTaxCalc { get; set; }
        /// <summary>
        /// 外税額を取得・設定します。
        /// </summary>
        public Decimal PriceOutTax { get; set; }
        /// <summary>
        /// 内税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceInTaxCalc { get; set; }
        /// <summary>
        /// 内税額を取得・設定します。
        /// </summary>
        public Decimal PriceInTax { get; set; }
        /// <summary>
        /// 非課税対象額を取得・設定します。
        /// </summary>
        public Decimal PriceNoTaxCalc { get; set; }
        /// <summary>
        /// 税区分を取得・設定します。
        /// </summary>
        public Int32 TaxDispKbn { get; set; }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpYMD { get; set; }
        /// <summary>
        /// 傭車金額を取得・設定します。
        /// </summary>
        public Decimal CharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInCharterPrice { get; set; }
        /// <summary>
        /// 傭車金額_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInCharterPrice { get; set; }
        /// <summary>
        /// 傭車外税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceOutTaxCalc { get; set; }
        /// <summary>
        /// 傭車外税額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceOutTax { get; set; }
        /// <summary>
        /// 傭車内税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceInTaxCalc { get; set; }
        /// <summary>
        /// 傭車内税額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceInTax { get; set; }
        /// <summary>
        /// 傭車非課税対象額を取得・設定します。
        /// </summary>
        public Decimal CharterPriceNoTaxCalc { get; set; }
        /// <summary>
        /// 傭車税区分を取得・設定します。
        /// </summary>
        public Int32 CharterTaxDispKbn { get; set; }
        /// <summary>
        /// 傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpYMD { get; set; }
        /// <summary>
        /// 運賃を取得・設定します。
        /// </summary>
        public Decimal Fee { get; set; }
        /// <summary>
        /// 運賃_金額を取得・設定します。
        /// </summary>
        public Decimal PriceInFee { get; set; }
        /// <summary>
        /// 運賃_通行料を取得・設定します。
        /// </summary>
        public Decimal TollFeeInFee { get; set; }
        /// <summary>
        /// 運賃外税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeOutTaxCalc { get; set; }
        /// <summary>
        /// 運賃外税額を取得・設定します。
        /// </summary>
        public Decimal FeeOutTax { get; set; }
        /// <summary>
        /// 運賃内税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeInTaxCalc { get; set; }
        /// <summary>
        /// 運賃内税額を取得・設定します。
        /// </summary>
        public Decimal FeeInTax { get; set; }
        /// <summary>
        /// 運賃非課税対象額を取得・設定します。
        /// </summary>
        public Decimal FeeNoTaxCalc { get; set; }
        /// <summary>
        /// 発地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 発地名を取得・設定します。
        /// </summary>
        public String StartPointName { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public Decimal EndPointId { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public String EndPointName { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime StartYMD { get; set; }
        /// <summary>
        /// 積載日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 到着日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        /***************
         * 受注
         ***************/

        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }

        /***************
         * 営業所
         ***************/

        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCode { get; set; }
        /// <summary>
        /// 営業所名称を取得・設定します。
        /// </summary>
        public String BranchOfficeName { get; set; }

        /***************
         * 得意先
         ***************/

        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }

        /***************
         * 車両
         ***************/

        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCode { get; set; }
        /// <summary>
        /// 陸運名を取得・設定します。
        /// </summary>
        public String LicPlateDeptName { get; set; }
        /// <summary>
        /// 分類区分を取得・設定します。
        /// </summary>
        public String LicPlateCarKindKbn { get; set; }
        /// <summary>
        /// 業態区分を取得・設定します。
        /// </summary>
        public String LicPlateUsageKbn { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }

        /***************
         * 車種
         ***************/

        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }

        /***************
         * 社員
         ***************/

        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32? StaffCd { get; set; }
        /// <summary>
        /// 氏名を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }

        /***************
         * 発着地
         ***************/

        /// <summary>
        /// 発地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCode { get; set; }
        /// <summary>
        /// 発地住所１を取得・設定します。
        /// </summary>
        public string StartPointAddress1 { get; set; }
        /// <summary>
        /// 発地住所２を取得・設定します。
        /// </summary>
        public string StartPointAddress2 { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCode { get; set; }
        /// <summary>
        /// 着地住所１を取得・設定します。
        /// </summary>
        public string EndPointAddress1 { get; set; }
        /// <summary>
        /// 着地住所２を取得・設定します。
        /// </summary>
        public string EndPointAddress2 { get; set; }

        /***************
         * 品目
         ***************/

        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCode { get; set; }

        /***************
         * 単位
         ***************/

        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32? FigCode { get; set; }
        /// <summary>
        /// 単位名称を取得・設定します。
        /// </summary>
        public String FigName { get; set; }

        /***************
         * 販路
         ***************/

        /// <summary>
        /// 販路コードを取得・設定します。
        /// </summary>
        public Int32 HanroCode { get; set; }
        /// <summary>
        /// 販路名称を取得・設定します。
        /// </summary>
        public String HanroName { get; set; }
        /// <summary>
        /// 往復区分を取得・設定します。
        /// </summary>
        public Int32 OfukuKbn { get; set; }

        /***************
         * その他
         ***************/

        /// <summary>
        /// 削除配車IDを取得・設定します。
        /// </summary>
        public Decimal DelHaishaId { get; set; }
        /// <summary>
        /// 登録済みの配車情報か判定するFLGを取得・設定します。
        /// </summary>
        public bool RegFlg { get; set; }
        /// <summary>
        /// 編集FLGを取得・設定します。
        /// </summary>
        public bool UppdateFlg { get; set; }

    }

    /// <summary>
    /// 配送指示書（印刷条件）乗務員情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijishoConditionStaffInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 StaffCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
        /// <summary>
        /// カナ名称を取得・設定します。
        /// </summary>
        public String StaffNameKana { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal StaffId { get; set; }

        #endregion
    }

    /// <summary>
    /// 配車一覧表（印刷条件）車種情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijishoConditionCarKindInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 CarKindCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }

        #endregion
    }

    /// <summary>
    /// 配送指示書（印刷条件）のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijiShoRptSearchParameter
    {
        /// <summary>
        /// 日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDFrom { get; set; }
        /// <summary>
        /// 日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime HizukeYMDTo { get; set; }
        /// <summary>
        /// 共通伝達事項を取得・設定します。
        /// </summary>
        public String Attentions { get; set; }
        /// <summary>
        /// 乗務員_範囲指定フラグを取得・設定します。
        /// </summary>
        public Boolean Staff_HaniShiteiChecked { get; set; }
        /// <summary>
        /// 乗務員_個別指定フラグを取得・設定します。
        /// </summary>
        public Boolean Staff_KobetsuShiteiChecked { get; set; }
        /// <summary>
        /// 乗務員コード（範囲開始）を取得・設定します。
        /// </summary>
        public Int32 StaffCodeFrom { get; set; }
        /// <summary>
        /// 乗務員コード（範囲終了）を取得・設定します。
        /// </summary>
        public Int32 StaffCodeTo { get; set; }
        /// <summary>
        /// 配車指示書（印刷条件）乗務員一覧情報
        /// </summary>
        public IList<HaisoShijishoConditionStaffInfo> HaisoShijishoConditionStaffList { get; set; }

        /// <summary>
        /// 車種_範囲指定フラグを取得・設定します。
        /// </summary>
        public Boolean CarKind_HaniShiteiChecked { get; set; }
        /// <summary>
        /// 車種_個別指定フラグを取得・設定します。
        /// </summary>
        public Boolean CarKind_KobetsuShiteiChecked { get; set; }
        /// <summary>
        /// 車種コード（範囲開始）を取得・設定します。
        /// </summary>
        public Int32 CarKindCodeFrom { get; set; }
        /// <summary>
        /// 車種コード（範囲終了）を取得・設定します。
        /// </summary>
        public Int32 CarKindCodeTo { get; set; }
        /// <summary>
        /// 配車指示書（印刷条件）車種一覧情報
        /// </summary>
        public IList<HaisoShijishoConditionCarKindInfo> HaisoShijishoConditionCarKindList { get; set; }

        /// <summary>
        /// 開始前日数を取得・設定します。
        /// </summary>
        public Int32 KaishiMaeNissu { get; set; }

        #region 関連項目

        /// <summary>
        /// 乗務員名（範囲開始）を取得・設定します。
        /// </summary>
        public String StaffNameFrom { get; set; }
        /// <summary>
        /// 乗務員名（範囲終了）を取得・設定します。
        /// </summary>
        public String StaffNameTo { get; set; }
        /// <summary>
        /// チェックされた乗務員Idをカンマ区切りで取得します。
        /// </summary>
        public String StaffCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (HaisoShijishoConditionStaffInfo info in this.HaisoShijishoConditionStaffList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.StaffId.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 車種名（範囲開始）を取得・設定します。
        /// </summary>
        public String CarKindNameFrom { get; set; }
        /// <summary>
        /// 車種名（範囲終了）を取得・設定します。
        /// </summary>
        public String CarKindNameTo { get; set; }
        /// <summary>
        /// チェックされた車種Idをカンマ区切りで取得します。
        /// </summary>
        public String CarKindCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (HaisoShijishoConditionCarKindInfo info in this.HaisoShijishoConditionCarKindList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.CarKindId.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion

        /// <summary>
        /// 条件指定を取得・設定します。
        /// </summary>
        public String Jokenshitei { get; set; }
    }

    /// <summary>
    /// 配送指示書明細部のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijishoDetailsInfo
    {
        /// <summary>
        /// 積卸区分を取得・設定します。
        /// </summary>
        public string Kbn { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public string TokuisakiName { get; set; }
        /// <summary>
        /// 発着地名を取得・設定します。
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 品名を取得・設定します。
        /// </summary>
        public string ItemName { get; set; }
        /// <summary>
        /// 数量を取得・設定します。
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// 単位を取得・設定します。
        /// </summary>
        public string FigName { get; set; }
        /// <summary>
        /// 重量を取得・設定します。
        /// </summary>
        public string Weight { get; set; }
        /// <summary>
        /// 積載日を取得・設定します。
        /// </summary>
        public string SekisaiYMD { get; set; }
        /// <summary>
        /// 積載時間を取得・設定します。
        /// </summary>
        public string SekisaiHM { get; set; }
        /// <summary>
        /// 発着日を取得・設定します。
        /// </summary>
        public string TaskYMD { get; set; }
        /// <summary>
        /// 発着時間を取得・設定します。
        /// </summary>
        public string TaskHM { get; set; }
        /// <summary>
        /// 住所を取得・設定します。
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 受注ごとの最初の積載日＋積載時間区分値(ソート用)を取得・設定します。
        /// </summary>
        public decimal JuchuMinSekisaiYMDSort { get; set; }
        /// <summary>
        /// 受注ごとの最初の発日＋着日(ソート用)を取得・設定します。
        /// </summary>
        public decimal JuchuMinHatsuChakuYMDSort { get; set; }
        /// <summary>
        /// 日付(ソート用)を取得・設定します。
        /// </summary>
        public DateTime TaskYMDSort { get; set; }
        /// <summary>
        /// 時間帯(ソート用)を取得・設定します。
        /// </summary>
        public Int32 TaskHMSort { get; set; }
        /// <summary>
        /// 積卸区分(ソート用)を取得・設定します。
        /// </summary>
        public Int32 KbnInt { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
    }
}
