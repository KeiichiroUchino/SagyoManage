using System;
using System.Collections.Generic;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配送指示メール送信情報のエンティティコンポーネント
    /// </summary>
    [Serializable]
    public class HaisoShijiMailSendInfo
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
        /// 単価を取得・設定します。
        /// </summary>
        public Decimal AtPrice { get; set; }
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
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal ItemId { get; set; }
        /// <summary>
        /// 荷主IDを取得・設定します。
        /// </summary>
        public Decimal OwnerId { get; set; }
        /// <summary>
        /// 荷主コードを取得・設定します。
        /// </summary>
        public Decimal OwnerCode { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerName { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime StartYMD { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
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
        /// 営業所略名を取得・設定します。
        /// </summary>
        public String BranchOfficeShortName { get; set; }

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
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }

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
        /// <summary>
        /// メールアドレスを取得・設定します。
        /// </summary>
        public String Email { get; set; }

        /***************
         * 積着地
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
    /// 配送指示メール送信条件_乗務員情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SendMailStaffInfo
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
    /// 配送指示メール送信条件のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijiMailSendSearchParameter
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
        /// メールタイトル接頭語を取得・設定します。
        /// </summary>
        public String TitlePrefix { get; set; }
        /// <summary>
        /// 共通伝達事項を取得・設定します。
        /// </summary>
        public String Attentions { get; set; }
        /// <summary>
        /// 配車指示メール送信条件_乗務員一覧情報
        /// </summary>
        public IList<SendMailStaffInfo> SendMailStaffList { get; set; }

        #region 関連項目

        /// <summary>
        /// チェックされた乗務員Idをカンマ区切りで取得します。
        /// </summary>
        public String StaffCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (SendMailStaffInfo info in this.SendMailStaffList)
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

        #endregion

        /// <summary>
        /// 条件指定を取得・設定します。
        /// </summary>
        public String Jokenshitei { get; set; }
    }
}
