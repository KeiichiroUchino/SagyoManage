using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 取引先（トラDON）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TorihikisakiInfo
    {
        /// <summary>
        /// トラDON取引先IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONTorihikiId { get; set; }
        /// <summary>
        /// トラDON取引先コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONTorihikiCode { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// トラDON取引先名称を取得・設定します。
        /// </summary>
        public String ToraDONTorihikiName { get; set; }
        /// <summary>
        /// トラDON取引先略称を取得・設定します。
        /// </summary>
        public String ToraDONTorihikiShortName { get; set; }
        /// <summary>
        /// トラDON取引先カナを取得・設定します。
        /// </summary>
        public String ToraDONTorihikiNameKana { get; set; }
        /// <summary>
        /// 敬称を取得・設定します。
        /// </summary>
        public String HonorificTitle { get; set; }
        /// <summary>
        /// 郵便番号を取得・設定します。
        /// </summary>
        public String Postal { get; set; }
        /// <summary>
        /// 住所1を取得・設定します。
        /// </summary>
        public String Address1 { get; set; }
        /// <summary>
        /// 住所2を取得・設定します。
        /// </summary>
        public String Address2 { get; set; }
        /// <summary>
        /// TELを取得・設定します。
        /// </summary>
        public String Tel { get; set; }
        /// <summary>
        /// FAXを取得・設定します。
        /// </summary>
        public String Fax { get; set; }
        /// <summary>
        /// 相手担当者名を取得・設定します。
        /// </summary>
        public String TorihikiStaffName { get; set; }
        /// <summary>
        /// URLを取得・設定します。
        /// </summary>
        public String Url { get; set; }
        /// <summary>
        /// メールアドレスを取得・設定します。
        /// </summary>
        public String MailAddress { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Memo { get; set; }
        /// <summary>
        /// 傭車区分を取得・設定します。
        /// </summary>
        public Int32 CharterKbn { get; set; }
        /// <summary>
        /// 支払先IDを取得・設定します。
        /// </summary>
        public Decimal PayeeId { get; set; }
        /// <summary>
        /// 締日1を取得・設定します。
        /// </summary>
        public Int32 FixDay1 { get; set; }
        /// <summary>
        /// 支払月1を取得・設定します。
        /// </summary>
        public Decimal PaymentMonth1 { get; set; }
        /// <summary>
        /// 支払日1を取得・設定します。
        /// </summary>
        public Decimal PaymentDay1 { get; set; }
        /// <summary>
        /// 締日2を取得・設定します。
        /// </summary>
        public Int32 FixDay2 { get; set; }
        /// <summary>
        /// 支払月2を取得・設定します。
        /// </summary>
        public Decimal PaymentMonth2 { get; set; }
        /// <summary>
        /// 支払日2を取得・設定します。
        /// </summary>
        public Decimal PaymentDay2 { get; set; }
        /// <summary>
        /// 締日3を取得・設定します。
        /// </summary>
        public Int32 FixDay3 { get; set; }
        /// <summary>
        /// 支払月3を取得・設定します。
        /// </summary>
        public Decimal PaymentMonth3 { get; set; }
        /// <summary>
        /// 支払日3を取得・設定します。
        /// </summary>
        public Decimal PaymentDay3 { get; set; }
        /// <summary>
        /// 支払金額1を取得・設定します。
        /// </summary>
        public Decimal PaymentGak1 { get; set; }
        /// <summary>
        /// 支払区分11を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn11 { get; set; }
        /// <summary>
        /// 支払割合11を取得・設定します。
        /// </summary>
        public Decimal PaymentRate11 { get; set; }
        /// <summary>
        /// 支払区分12を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn12 { get; set; }
        /// <summary>
        /// 支払割合12を取得・設定します。
        /// </summary>
        public Decimal PaymentRate12 { get; set; }
        /// <summary>
        /// 支払金額2を取得・設定します。
        /// </summary>
        public Decimal PaymentGak2 { get; set; }
        /// <summary>
        /// 支払区分21を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn21 { get; set; }
        /// <summary>
        /// 支払割合21を取得・設定します。
        /// </summary>
        public Decimal PaymentRate21 { get; set; }
        /// <summary>
        /// 支払区分22を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn22 { get; set; }
        /// <summary>
        /// 支払割合22を取得・設定します。
        /// </summary>
        public Decimal PaymentRate22 { get; set; }
        /// <summary>
        /// 支払金額3を取得・設定します。
        /// </summary>
        public Decimal PaymentGak3 { get; set; }
        /// <summary>
        /// 支払区分31を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn31 { get; set; }
        /// <summary>
        /// 支払割合31を取得・設定します。
        /// </summary>
        public Decimal PaymentRate31 { get; set; }
        /// <summary>
        /// 支払区分32を取得・設定します。
        /// </summary>
        public Int32 PaymentKbn32 { get; set; }
        /// <summary>
        /// 支払割合32を取得・設定します。
        /// </summary>
        public Decimal PaymentRate32 { get; set; }
        /// <summary>
        /// 支払明細書発行区分を取得・設定します。
        /// </summary>
        public Int32 PayDtlStatePRTKbn { get; set; }
        /// <summary>
        /// 支払明細印字区分を取得・設定します。
        /// </summary>
        public Int32 PayDtlPRTKbn { get; set; }
        /// <summary>
        /// 消費税計算区分を取得・設定します。
        /// </summary>
        public Int32 TaxActKbn { get; set; }
        /// <summary>
        /// 消費税丸め区分を取得・設定します。
        /// </summary>
        public Int32 TaxCutKbn { get; set; }
        /// <summary>
        /// 金額丸め区分を取得・設定します。
        /// </summary>
        public Int32 GakCutKbn { get; set; }
        /// <summary>
        /// 傭車掛率を取得・設定します。
        /// </summary>
        public Decimal CharterRate { get; set; }
        /// <summary>
        /// 自社スタンド区分を取得・設定します。
        /// </summary>
        public Int32 CompanyGSKbn { get; set; }
        /// <summary>
        /// 傭車計上日区分を取得・設定します。
        /// </summary>
        public Int32 SaleSlipToPayDayKbn { get; set; }
        /// <summary>
        /// 支払明細書日付印字区分を取得・設定します。
        /// </summary>
        public Int32 PayDtlDatePRTKbn { get; set; }
        /// <summary>
        /// 取引開始日を取得・設定します。
        /// </summary>
        public DateTime FirstClmDay { get; set; }
        /// <summary>
        /// 最終支払締切日を取得・設定します。
        /// </summary>
        public DateTime LastPayFixDay { get; set; }
        /// <summary>
        /// 残高管理フラグを取得・設定します。
        /// </summary>
        public Boolean RemainManageFlag { get; set; }
        /// <summary>
        /// トラDON非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }
        /// <summary>
        /// トラDON削除フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.ToraDONDisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 取引先検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TorihikisakiSearchParameter
    {
        /// <summary>
        /// トラDON取引先IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONTorihikiId { get; set; }
        /// <summary>
        /// トラDON取引先コードを取得・設定します。
        /// </summary>
        public Int32? ToraDONTorihikiCode { get; set; }
        /// <summary>
        /// 傭車区分を取得・設定します。
        /// </summary>
        public Int32? CharterKbn { get; set; }
    }
}
