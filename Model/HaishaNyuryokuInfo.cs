using System;
using System.Collections.Generic;
using System.Drawing;

namespace Jpsys.HaishaManageV10.Model
{

    /// <summary>
    /// 配車情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaNyuryokuInfo : AbstractSequenceKeyTimeStampModelBase
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
        /// 車番を取得・設定します。
        /// </summary>
        public string LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
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
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
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
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
        /// <summary>
        /// 運賃を取得・設定します。
        /// </summary>
        public Decimal? Fee { get; set; }
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
        public int OwnerCode { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerName { get; set; }
        /// <summary>
        /// 出発日付を取得・設定します。
        /// </summary>
        public DateTime StartYMD { get; set; }
        /// <summary>
        /// 出発日時を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 到着日時を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        ///// <summary>
        ///// 配送完了日を取得・設定します。
        ///// </summary>
        //public Decimal? HaisouEndYMD { get; set; }
        ///// <summary>
        ///// 配送キャンセル日を取得・設定します。
        ///// </summary>
        //public Decimal? HaisouCancelYMD { get; set; }
        ///// <summary>
        ///// 配送キャンセル理由を取得・設定します。
        ///// </summary>
        //public String HaisouCancelRiyu { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }
        /// <summary>
        /// 待機時間数量を取得・設定します。
        /// </summary>
        public Decimal TaikijikanNumber { get; set; }
        /// <summary>
        /// 待機時間単位IDを取得・設定します。
        /// </summary>
        public Decimal TaikijikanFigId { get; set; }
        /// <summary>
        /// 待機時間単価を取得・設定します。
        /// </summary>
        public Decimal TaikijikanAtPrice { get; set; }
        /// <summary>
        /// 金額_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInPrice { get; set; }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 運賃_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInFee { get; set; }
        /// <summary>
        /// 運賃_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInFee { get; set; }
        /// <summary>
        /// 荷積み時間数量を取得・設定します。
        /// </summary>
        public Decimal NizumijikanNumber { get; set; }
        /// <summary>
        /// 荷積み時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NizumijikanFigId { get; set; }
        /// <summary>
        /// 荷積み時間単価を取得・設定します。
        /// </summary>
        public Decimal NizumijikanAtPrice { get; set; }
        /// <summary>
        /// 荷卸し時間数量を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanNumber { get; set; }
        /// <summary>
        /// 荷卸し時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NioroshijikanFigId { get; set; }
        /// <summary>
        /// 荷卸し時間単価を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanAtPrice { get; set; }
        /// <summary>
        /// 附帯業務時間数量を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanNumber { get; set; }
        /// <summary>
        /// 附帯業務時間単位IDを取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanFigId { get; set; }
        /// <summary>
        /// 附帯業務時間単価を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanAtPrice { get; set; }
        /// <summary>
        /// 乗務員売上同額フラグを取得・設定します。
        /// </summary>
        public Boolean JomuinUriageDogakuFlag { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }

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

        /***************
         * 営業所
         ***************/

        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCode { get; set; }

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

        /***************
         * 車種
         ***************/

        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32? CarKindCode { get; set; }

        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }

        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32 CarKbn { get; set; }

        /// <summary>
        /// 車両車番を取得・設定します。
        /// </summary>
        public String CarLicPlateCarNo { get; set; }

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
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCode { get; set; }

        /***************
         * 品目
         ***************/

        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCode { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }

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

        /***************
         * 取引先（傭車先）
         ***************/

        /// <summary>
        /// 取引先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCd { get; set; }
        /// <summary>
        /// 取引先名を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }
        /// <summary>
        /// 取引先略称を取得・設定します。
        /// </summary>
        public String TorihikiShortName { get; set; }

        /***********************
         * チェック用受注情報
         ***********************/

        /// <summary>
        /// 作業開始日付を取得・設定します。
        /// </summary>
        public DateTime JuchuTaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日付を取得・設定します。
        /// </summary>
        public DateTime JuchuTaskEndDateTime { get; set; }
        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime JuchuReuseYMD { get; set; }

        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }
        //TODO 性能問題のためコメント化中 START
        ///// <summary>
        ///// トラDON請求締切日を取得・設定します。TODO 仮実装中
        ///// </summary>
        //public DateTime MinClmFixDate { get; set; }
        ///// <summary>
        ///// トラDON傭車支払締切日を取得・設定します。TODO 仮実装中
        ///// </summary>
        //public DateTime MinCharterPayFixDate { get; set; }
        ///// <summary>
        ///// トラDON計上日付を取得・設定します。
        ///// </summary>
        //public DateTime AddUpDate { get; set; }
        ///// <summary>
        ///// トラDON傭車計上日付を取得・設定します。
        ///// </summary>
        //public DateTime CharterAddUpDate { get; set; }
        ///// <summary>
        ///// トラDON確定フラグを取得・設定します。
        ///// </summary>
        //public Boolean ToraDonFixFlag { get; set; }
        ///// <summary>
        ///// トラDON傭車確定フラグを取得・設定します。
        ///// </summary>
        //public Boolean ToraDonCharterFixFlag { get; set; }
        //TODO 性能問題のためコメント化中 END

        // ディープコピー
        public HaishaNyuryokuInfo Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (HaishaNyuryokuInfo)MemberwiseClone();
        }
    }

    /// <summary>
    /// 配車入力画面のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaNyuryokuSearchParameter
    {
        /// <summary>
        /// 検索用開始日付を取得・設定します。
        /// </summary>
        public DateTime? JuchuStartYMD { get; set; }
        /// <summary>
        /// 検索用終了日付を取得・設定します。
        /// </summary>
        public DateTime? JuchuEndYMD { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public decimal? TokuisakiCd { get; set; }
        /// <summary>
        /// 方面コードを取得・設定します。
        /// </summary>
        public decimal? HomenCode { get; set; }
        /// <summary>
        /// 車種Id（受注）を取得・設定します。
        /// </summary>
        public decimal? JuchuCarKindId { get; set; }
        /// <summary>
        /// 受注担当者Idを取得・設定します。
        /// </summary>
        public decimal? JuchuTAntoshaId { get; set; }
        /// <summary>
        /// 表示開始日付を取得・設定します。
        /// </summary>
        public DateTime DispStratYMD { get; set; }
        /// <summary>
        /// 表示終了日付を取得・設定します。
        /// </summary>
        public DateTime DispEndYMD { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32? CarCode { get; set; }
        /// <summary>
        /// 車種コード（配車）を取得・設定します。
        /// </summary>
        public int? CarKindCode { get; set; }
        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32? StaffCd { get; set; }
        /// <summary>
        /// キー検索フラグを取得・設定します。
        /// </summary>
        public bool KeyFlg { get; set; }
        /// <summary>
        /// 受注情報検索用の配車情報一覧を取得・設定します。
        /// </summary>
        public Dictionary<decimal, HaishaNyuryokuInfo> HaishaNyuryokuInfo { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 排他用抽出条件を取得・設定します。
        /// </summary>
        public HaishaNyuryokuConditionInfo HaishaNyuryokuConditionInfo { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public int? CarKbn { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal? BranchOfficeId { get; set; }
        /// <summary>
        /// 配車用営業所IDを取得・設定します。
        /// </summary>
        public Decimal? HaishaBranchOfficeId { get; set; }
        /// <summary>
        /// 配車受注一覧ソート区分を取得・設定します。
        /// </summary>
        public Int32 HaishaJuchuListSortKbn { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 配車IDリストを取得・設定します。
        /// </summary>
        public List<Decimal> HaishaIdList { get; set; }
        /// <summary>
        /// 時間間隔の前回値を取得・設定します。
        /// </summary>
        public int PrevTimeInterval { get; set; }
        /// <summary>
        /// 時間間隔の前回値を取得・設定します。
        /// </summary>
        public int PrevDisplayRange { get; set; }
        /// <summary>
        /// 車両区分の前回値を取得・設定します。
        /// </summary>
        public int PrevCarType { get; set; }
        /// <summary>
        /// 車両営業所の前回値を取得・設定します。
        /// </summary>
        public Decimal PrevCarOfiice { get; set; }
        /// <summary>
        /// 配車入力未配車両のみフラグの前回値を取得・設定します。
        /// </summary>
        public Boolean PrevHaishaNyuryokuMihainomiFlag { get; set; }
    }

    /// <summary>
    /// 車両情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaCarInfo
    {
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32? CarKindCd { get; set; }
        /// <summary>
        /// 車種名を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCode { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public string LicPlateCarNo { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }
        /// <summary>
        /// 乗務員コードを取得・設定します。
        /// </summary>
        public Int32? StaffCd { get; set; }
        /// <summary>
        /// 乗務員名を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }
        /// <summary>
        /// 乗務員グループ番号を取得・設定します。
        /// </summary>
        public Int32 StaffGroupNo { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32 CarKbn { get; set; }

        /***************
         * 取引先（傭車先）
         ***************/

        /// <summary>
        /// 取引先IDを取得・設定します。
        /// </summary>
        public Decimal TorihikiId { get; set; }
        /// <summary>
        /// 取引先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCd { get; set; }
        /// <summary>
        /// 取引先名称を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }
        /// <summary>
        /// 取引先略称を取得・設定します。
        /// </summary>
        public String TorihikiShortName { get; set; }
        /// <summary>
        /// 配車入力車両カテゴリ背景色を取得・設定します。
        /// </summary>
        public int? HaishaNyuryokuCarBackColor { get; set; }
        /// <summary>
        /// 配車入力車両台数未カウントフラグを取得・設定します。
        /// </summary>
        public Boolean HaishaNyuryokuCarCountExclusionFlag { get; set; }
        /// <summary>
        /// 表示フラグを取得・設定します。
        /// </summary>
        public int DisableFlag { get; set; }
    }

    /// <summary>
    /// 受注情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class JuchuInfo
    {
        /***************
         * 受注
         ***************/

        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 受注伝票番号を取得・設定します。
        /// </summary>
        public Decimal JuchuSlipNo { get; set; }
        /// <summary>
        /// 請求部門IDを取得・設定します。
        /// </summary>
        public Decimal ClmClassId { get; set; }
        /// <summary>
        /// 請求部門コードを取得・設定します。
        /// </summary>
        public Int32 ClmClassCode { get; set; }
        /// <summary>
        /// 請求部門名称を取得・設定します。
        /// </summary>
        public String ClmClassName { get; set; }
        /// <summary>
        /// 請負IDを取得・設定します。
        /// </summary>
        public Decimal ContractId { get; set; }
        /// <summary>
        /// 請負番号を取得・設定します。
        /// </summary>
        public Int32 ContractCode { get; set; }
        /// <summary>
        /// 請負名を取得・設定します。
        /// </summary>
        public String ContractName { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 車両車番を取得・設定します。
        /// </summary>
        public string CarLicPlateCarNo { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public string LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId { get; set; }
        /// <summary>
        /// 車種コードを取得・設定します。
        /// </summary>
        public Int32 CarKindCode { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
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
        public Decimal Weight { get; set; }
        /// <summary>
        /// 金額を取得・設定します。
        /// </summary>
        public Decimal Price { get; set; }
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
        /// 確定フラグを取得・設定します。
        /// </summary>
        public Boolean FixFlag { get; set; }
        /// <summary>
        /// 計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpYMD { get; set; }
        /// <summary>
        /// 傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean CharterFixFlag { get; set; }
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
        public Int32 OwnerCode { get; set; }
        /// <summary>
        /// 荷主名を取得・設定します。
        /// </summary>
        public String OwnerName { get; set; }
        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal BranchOfficeId { get; set; }
        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32 BranchOfficeCode { get; set; }
        /// <summary>
        /// 営業所略称を取得・設定します。
        /// </summary>
        public String BranchOfficeShortName { get; set; }
        /// <summary>
        /// 得意先コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 発地コードを取得・設定します。
        /// </summary>
        public Int32 StartPointCode { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 EndPointCode { get; set; }
        /// <summary>
        /// 品目コードを取得・設定します。
        /// </summary>
        public Int32 ItemCode { get; set; }
        /// <summary>
        /// 品目名を取得・設定します。
        /// </summary>
        public String ItemName { get; set; }
        /// <summary>
        /// 作業開始日付を取得・設定します。
        /// </summary>
        public DateTime TaskStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日付を取得・設定します。
        /// </summary>
        public DateTime TaskEndDateTime { get; set; }
        /// <summary>
        /// 削除対象Flgを取得・設定します。
        /// </summary>
        public bool DelTargetFlg { get; set; }
        /// <summary>
        /// 待機時間数量を取得・設定します。
        /// </summary>
        public Decimal TaikijikanNumber { get; set; }
        /// <summary>
        /// 待機時間単位IDを取得・設定します。
        /// </summary>
        public Decimal TaikijikanFigId { get; set; }
        /// <summary>
        /// 待機時間単価を取得・設定します。
        /// </summary>
        public Decimal TaikijikanAtPrice { get; set; }
        /// <summary>
        /// 金額_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInPrice { get; set; }
        /// <summary>
        /// 金額_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInPrice { get; set; }
        /// <summary>
        /// 金額_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInPrice { get; set; }
        /// <summary>
        /// 運賃_待機時間料を取得・設定します。
        /// </summary>
        public Decimal TaikijikanryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷積み料を取得・設定します。
        /// </summary>
        public Decimal NizumiryoInFee { get; set; }
        /// <summary>
        /// 運賃_荷卸し料を取得・設定します。
        /// </summary>
        public Decimal NioroshiryoInFee { get; set; }
        /// <summary>
        /// 運賃_附帯業務料を取得・設定します。
        /// </summary>
        public Decimal FutaigyomuryoInFee { get; set; }
        /// <summary>
        /// 荷積み時間数量を取得・設定します。
        /// </summary>
        public Decimal NizumijikanNumber { get; set; }
        /// <summary>
        /// 荷積み時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NizumijikanFigId { get; set; }
        /// <summary>
        /// 荷積み時間単価を取得・設定します。
        /// </summary>
        public Decimal NizumijikanAtPrice { get; set; }
        /// <summary>
        /// 荷卸し時間数量を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanNumber { get; set; }
        /// <summary>
        /// 荷卸し時間単位IDを取得・設定します。
        /// </summary>
        public Decimal NioroshijikanFigId { get; set; }
        /// <summary>
        /// 荷卸し時間単価を取得・設定します。
        /// </summary>
        public Decimal NioroshijikanAtPrice { get; set; }
        /// <summary>
        /// 附帯業務時間数量を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanNumber { get; set; }
        /// <summary>
        /// 附帯業務時間単位IDを取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanFigId { get; set; }
        /// <summary>
        /// 附帯業務時間単価を取得・設定します。
        /// </summary>
        public Decimal FutaigyomujikanAtPrice { get; set; }
        /// <summary>
        /// 乗務員売上同額フラグを取得・設定します。
        /// </summary>
        public Boolean JomuinUriageDogakuFlag { get; set; }
        /// <summary>
        /// 乗務員売上金額を取得・設定します。
        /// </summary>
        public Decimal JomuinUriageKingaku { get; set; }
        /// <summary>
        /// 受注担当IDを取得・設定します。
        /// </summary>
        public Decimal JuchuTantoId { get; set; }
        /// <summary>
        /// 受注担当コードを取得・設定します。
        /// </summary>
        public Int32 JuchuTantoCode { get; set; }
        /// <summary>
        /// 受注担当名を取得・設定します。
        /// </summary>
        public String JuchuTantoName { get; set; }
        /// <summary>
        /// 請求締切日を取得・設定します。
        /// </summary>
        public DateTime ClmFixYMD { get; set; }
        /// <summary>
        /// 傭車支払締切日を取得・設定します。
        /// </summary>
        public DateTime CharterPayFixYMD { get; set; }

        /***************
         * 単位
         ***************/

        /// <summary>
        /// 単位コードを取得・設定します。
        /// </summary>
        public Int32 FigCode { get; set; }
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
        /// <summary>
        /// 往復区分ソート順を取得・設定します。
        /// </summary>
        public Decimal OfukuKbnSortOrder { get; set; }
        /// <summary>
        /// 行程日数を取得・設定します。
        /// </summary>
        public Int32 KoteiNissu { get; set; }
        /// <summary>
        /// 行程時間を取得・設定します。
        /// </summary>
        public Int32 KoteiJikan { get; set; }

        /***************
         * 社員
         ***************/

        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32 StaffCd { get; set; }
        /// <summary>
        /// 氏名を取得・設定します。
        /// </summary>
        public String StaffName { get; set; }

        /***************
         * 車両
         ***************/

        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCode { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32 CarKbn { get; set; }
        /// <summary>
        /// 営業所略称を取得・設定します。
        /// </summary>
        public String CarBranchOfficeShortName { get; set; }

        /***************
         * 取引先（傭車先）
         ***************/

        /// <summary>
        /// 取引先コードを取得・設定します。
        /// </summary>
        public Int32 TorihikiCd { get; set; }
        /// <summary>
        /// 取引先名を取得・設定します。
        /// </summary>
        public String TorihikiName { get; set; }
        /// <summary>
        /// 取引先略称を取得・設定します。
        /// </summary>
        public String TorihikiShortName { get; set; }

        /// <summary>
        /// 再使用可能日時を取得・設定します。
        /// </summary>
        public DateTime ReuseYMD { get; set; }
        /// <summary>
        /// 孫傭車先を取得・設定します。
        /// </summary>
        public String MagoYoshasaki { get; set; }

        /***************
         * チェック用
         ***************/

        /// <summary>
        /// トラDON計上日付を取得・設定します。
        /// </summary>
        public DateTime AddUpDate { get; set; }
        /// <summary>
        /// トラDON傭車計上日付を取得・設定します。
        /// </summary>
        public DateTime CharterAddUpDate { get; set; }
        /// <summary>
        /// トラDON確定フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDonFixFlag { get; set; }
        /// <summary>
        /// トラDON傭車確定フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDonCharterFixFlag { get; set; }

        // ディープコピー
        public JuchuInfo Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (JuchuInfo)MemberwiseClone();
        }

    }

    /// <summary>
    /// appoint情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class AppointInfo
    {
        /// <summary>
        /// 配車情報を取得・設定します。
        /// </summary>
        public HaishaNyuryokuInfo HaishaInfo { get; set; }
        /// <summary>
        /// 受注情報を取得・設定します。
        /// </summary>
        public JuchuInfo JuchuInfo { get; set; }
        /// <summary>
        /// 配車情報に追加された情報か判定するFLGを取得・設定します。
        /// </summary>
        public bool AddFlg { get; set; }
        /// <summary>
        /// 登録済みの配車情報か判定するFLGを取得・設定します。
        /// </summary>
        public bool RegFlg { get; set; }
        /// <summary>
        /// 削除配車IDを取得・設定します。
        /// </summary>
        public Decimal DelHaishaId { get; set; }
        /// <summary>
        /// 予定区分を取得・設定します。
        /// </summary>
        public Int32 AppointKbn { get; set; }
        /// <summary>
        /// 休日の背景色を取得・設定します。
        /// </summary>
        public Color KyujitsuBackColor { get; set; }
        /// <summary>
        /// 休日の文字色を取得・設定します。
        /// </summary>
        public Color KyujitsuFontColor { get; set; }

        // ディープコピー
        public AppointInfo Clone()
        {
            // Object型で返ってくるのでキャストが必要
            return (AppointInfo)MemberwiseClone();
        }
    }

    /// <summary>
    /// 配車情報検索結果のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaSearchResultInfo
    {
        /// <summary>
        /// 配車情報を取得・設定します。
        /// </summary>
        public HaishaNyuryokuInfo HaishaInfo { get; set; }
        /// <summary>
        /// 受注情報を取得・設定します。
        /// </summary>
        public JuchuInfo JuchuInfo { get; set; }
    }

    /// <summary>
    /// 休日情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaKyujitsuCalendarInfo
    {
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 CarCode { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal DriverId { get; set; }
        /// <summary>
        /// 日付を取得・設定します。
        /// </summary>
        public DateTime HizukeYMD { get; set; }
        /// <summary>
        /// 休日区分を取得・設定します。
        /// </summary>
        public Int32 KyujitsuKbn { get; set; }

    }

    /// <summary>
    /// 排他情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaitaInfo
    {
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal HaishaId { get; set; }
    }

    /// <summary>
    /// 登録用排他情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaitaInsertInfo
    {
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal HaishaId { get; set; }
    }

    /// <summary>
    /// 配車入力用利用者情報補のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaitaOperatorExInfo
    {
        /// <summary>
        /// 時間間隔の前回値を取得・設定します。
        /// </summary>
        public int PrevTimeInterval { get; set; }
        /// <summary>
        /// 時間間隔の前回値を取得・設定します。
        /// </summary>
        public int PrevDisplayRange { get; set; }
        /// <summary>
        /// 車両区分の前回値を取得・設定します。
        /// </summary>
        public int PrevCarType { get; set; }
        /// <summary>
        /// 車両営業所の前回値を取得・設定します。
        /// </summary>
        public Decimal PrevCarOfiice { get; set; }
        /// <summary>
        /// 配車入力未配車両のみフラグの前回値を取得・設定します。
        /// </summary>
        public Boolean PrevHaishaNyuryokuMihainomiFlag { get; set; }
    }
}
