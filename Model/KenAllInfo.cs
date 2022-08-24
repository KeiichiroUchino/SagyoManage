using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// KEN_ALL_情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KenAllInfo
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 郵便番号IDを取得・設定します。
        /// </summary>
        public Decimal ZipId { get; set; }
        /// <summary>
        /// 郵便番号文字列を取得・設定します。
        /// </summary>
        public string ZipTextCd
        {
            get
            {
                return Int32.Parse(this.ZipCd).ToString("D7");
            }
        }
        /// <summary>
        /// 郵便番号を取得・設定します。
        /// </summary>
        public string ZipCd { get; set; }
        /// <summary>
        /// 都道府県名を取得・設定します。
        /// </summary>
        public string PrefName { get; set; }
        /// <summary>
        /// 市区町村名を取得・設定します。
        /// </summary>
        public string CityName { get; set; }
        /// <summary>
        /// 町域名を取得・設定します。
        /// </summary>
        public string TownName { get; set; }
    }

    /// <summary>
    /// KEN_ALL_CSV情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KenAllCsvInfo
    {
        /// <summary>
        /// CSV明細の列番号を表します。
        /// </summary>
        public enum CsvCurumnIndices : int
        {
            JisCode = 0,
            Post5Code = 1,
            PostCode = 2,
            PrefectureKana = 3,
            CityKana = 4,
            TownAreaKana = 5,
            Prefecture = 6,
            City = 7,
            TownArea = 8,
            IsOneTownByMultiPostCode = 9,
            IsNeedSmallAreaAddress = 10,
            IsChome = 11,
            IsMultiTownByOnePostCode = 12,
            Updated = 13,
            UpdateReason = 14,
        }

        /// <summary>
        /// CSV項目数（必須数）
        /// </summary>
        public static int CSV_MCURUMN_COUNT = 15;

        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 全国地方公共団体コードを取得・設定します。
        /// </summary>
        public String JisCode { get; set; }
        /// <summary>
        /// （旧）郵便番号（5桁）を取得・設定します。
        /// </summary>
        public String Post5Code { get; set; }
        /// <summary>
        /// 郵便番号（7桁）を取得・設定します。
        /// </summary>
        public String PostCode { get; set; }
        /// <summary>
        /// 都道府県名カナを取得・設定します。
        /// </summary>
        public String PrefectureKana { get; set; }
        /// <summary>
        /// 市区町村名カナを取得・設定します。
        /// </summary>
        public String CityKana { get; set; }
        /// <summary>
        /// 町域名カナを取得・設定します。
        /// </summary>
        public String TownAreaKana { get; set; }
        /// <summary>
        /// 都道府県名を取得・設定します。
        /// </summary>
        public String Prefecture { get; set; }
        /// <summary>
        /// 市区町村名を取得・設定します。
        /// </summary>
        public String City { get; set; }
        /// <summary>
        /// 町域名を取得・設定します。
        /// </summary>
        public String TownArea { get; set; }
        /// <summary>
        /// 一町域が二以上の郵便番号で表される場合の表示を取得・設定します。
        /// </summary>
        public Int32 IsOneTownByMultiPostCode { get; set; }
        /// <summary>
        /// 小字毎に番地が起番されている町域の表示を取得・設定します。
        /// </summary>
        public Int32 IsNeedSmallAreaAddress { get; set; }
        /// <summary>
        /// 丁目を有する町域の場合の表示を取得・設定します。
        /// </summary>
        public Int32 IsChome { get; set; }
        /// <summary>
        /// 一つの郵便番号で二以上の町域を表す場合の表示を取得・設定します。
        /// </summary>
        public Int32 IsMultiTownByOnePostCode { get; set; }
        /// <summary>
        /// 更新の表示を取得・設定します。
        /// </summary>
        public Int32 Updated { get; set; }
        /// <summary>
        /// 表示抑制を取得・設定します。
        /// </summary>
        public Int32 UpdateReason { get; set; }
    }
}
