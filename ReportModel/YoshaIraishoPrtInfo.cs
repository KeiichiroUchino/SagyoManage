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
    /// 傭車依頼書情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class YoshaIraishoPrtInfo
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
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal CarOfChartererId { get; set; }
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
        /// 積地IDを取得・設定します。
        /// </summary>
        public Decimal StartPointId { get; set; }
        /// <summary>
        /// 積地名を取得・設定します。
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
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }
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
        /// 積地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCode { get; set; }
        /// <summary>
        /// 積地住所１を取得・設定します。
        /// </summary>
        public string StartPointAddress1 { get; set; }
        /// <summary>
        /// 積地住所２を取得・設定します。
        /// </summary>
        public string StartPointAddress2 { get; set; }
        /// <summary>
        /// 積地TELを取得・設定します。
        /// </summary>
        public string StartPointTel { get; set; }
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
        /// <summary>
        /// 着地TELを取得・設定します。
        /// </summary>
        public string EndPointTel { get; set; }

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
         * 取引先
         ***************/

        /// <summary>
        /// 取引先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCode { get; set; }
        /// <summary>
        /// 取引先名を取得・設定します。
        /// </summary>
        public string TorihikiName { get; set; }
        /// <summary>
        /// 取引先TELを取得・設定します。
        /// </summary>
        public string TorihikiTel { get; set; }
        /// <summary>
        /// 取引先FAXを取得・設定します。
        /// </summary>
        public string TorihikiFax { get; set; }

        /// <summary>
        /// 取引先継承を取得・設定します。
        /// </summary>
        public string TorihikiHonorificTitle { get; set; }

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
    /// 傭車依頼書（印刷条件）得意先情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class YoshaIraishoConditionTokuisakiInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// カナ名称を取得・設定します。
        /// </summary>
        public String TokuisakiNameKana { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }

        #endregion
    }

    /// <summary>
    /// 傭車一覧表（印刷条件）傭車情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class YoshaIraishoConditionCarInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 YoshasakiCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String YoshasakiName { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// 傭車先IDを取得・設定します。
        /// </summary>
        public Decimal YoshasakiId { get; set; }

        #endregion
    }

    /// <summary>
    /// 傭車依頼書（印刷条件）のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class YoshaIraishoPrtSearchParameter
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
        /// 得意先_範囲指定フラグを取得・設定します。
        /// </summary>
        public Boolean Tokuisaki_HaniShiteiChecked { get; set; }
        /// <summary>
        /// 得意先_個別指定フラグを取得・設定します。
        /// </summary>
        public Boolean Tokuisaki_KobetsuShiteiChecked { get; set; }
        /// <summary>
        /// 得意先コード（範囲開始）を取得・設定します。
        /// </summary>
        public Int32 TokuisakiCodeFrom { get; set; }
        /// <summary>
        /// 得意先コード（範囲終了）を取得・設定します。
        /// </summary>
        public Int32 TokuisakiCodeTo { get; set; }
        /// <summary>
        /// 配車指示書（印刷条件）得意先一覧情報
        /// </summary>
        public IList<YoshaIraishoConditionTokuisakiInfo> YoshaIraishoConditionTokuisakiList { get; set; }

        /// <summary>
        /// 傭車_範囲指定フラグを取得・設定します。
        /// </summary>
        public Boolean Car_HaniShiteiChecked { get; set; }
        /// <summary>
        /// 傭車_個別指定フラグを取得・設定します。
        /// </summary>
        public Boolean Car_KobetsuShiteiChecked { get; set; }
        /// <summary>
        /// 傭車コード（範囲開始）を取得・設定します。
        /// </summary>
        public Int32 CarCodeFrom { get; set; }
        /// <summary>
        /// 傭車コード（範囲終了）を取得・設定します。
        /// </summary>
        public Int32 CarCodeTo { get; set; }
        /// <summary>
        /// 配車指示書（印刷条件）傭車一覧情報
        /// </summary>
        public IList<YoshaIraishoConditionCarInfo> YoshaIraishoConditionCarList { get; set; }

        /// <summary>
        /// 備考01を取得・設定します。
        /// </summary>
        public String Biko01 { get; set; }

        /// <summary>
        /// 備考02を取得・設定します。
        /// </summary>
        public String Biko02 { get; set; }

        /// <summary>
        /// 備考03を取得・設定します。
        /// </summary>
        public String Biko03 { get; set; }

        /// <summary>
        /// 備考04を取得・設定します。
        /// </summary>
        public String Biko04 { get; set; }

        /// <summary>
        /// 備考05を取得・設定します。
        /// </summary>
        public String Biko05 { get; set; }

        #region 関連項目

        /// <summary>
        /// 得意先名（範囲開始）を取得・設定します。
        /// </summary>
        public String TokuisakiNameFrom { get; set; }
        /// <summary>
        /// 得意先名（範囲終了）を取得・設定します。
        /// </summary>
        public String TokuisakiNameTo { get; set; }
        /// <summary>
        /// チェックされた得意先Idをカンマ区切りで取得します。
        /// </summary>
        public String TokuisakiCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (YoshaIraishoConditionTokuisakiInfo info in this.YoshaIraishoConditionTokuisakiList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.TokuisakiId.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }
        /// <summary>
        /// 傭車名（範囲開始）を取得・設定します。
        /// </summary>
        public String CarNameFrom { get; set; }
        /// <summary>
        /// 傭車名（範囲終了）を取得・設定します。
        /// </summary>
        public String CarNameTo { get; set; }
        /// <summary>
        /// チェックされた傭車Idをカンマ区切りで取得します。
        /// </summary>
        public String CarCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (YoshaIraishoConditionCarInfo info in this.YoshaIraishoConditionCarList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.YoshasakiId.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// 傭車依頼書明細部のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class YoshaIraishoDetailsInfo
    {
        /// <summary>
        /// 積卸区分を取得・設定します。
        /// </summary>
        public string Kbn { get; set; }
        /// <summary>
        /// 積着日を取得・設定します。
        /// </summary>
        public string TaskYMD { get; set; }
        /// <summary>
        /// 積着時間を取得・設定します。
        /// </summary>
        public string TaskHM { get; set; }
        /// <summary>
        /// 地名を取得・設定します。
        /// </summary>
        public string PointName { get; set; }
        /// <summary>
        /// 住所を取得・設定します。
        /// </summary>
        public string Address { get; set; }
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
        /// 電話番号を取得・設定します。
        /// </summary>
        public string Tel { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public string TokuisakiName { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 受注ごとの最初の積載日＋積載時間区分値(ソート用)を取得・設定します。
        /// </summary>
        public decimal JuchuMinSekisaiYMDSort { get; set; }
        /// <summary>
        /// 受注ごとの最初の着日(ソート用)を取得・設定します。
        /// </summary>
        public decimal JuchuMinChakuYMDSort { get; set; }
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
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }
    }
}
