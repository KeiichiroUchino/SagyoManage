using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 契約情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KeiyakuInfo : AbstractSequenceKeyTimeStampModelBase
    {
		/// <summary>
		/// 契約IDを取得・設定します。
		/// </summary>
		public Decimal KeiyakuId { get; set; }
		/// <summary>
		/// 作業場所IDを取得・設定します。
		/// </summary>
		public Decimal SagyoBashoId { get; set; }
		/// <summary>
		/// 契約コードを取得・設定します。
		/// </summary>
		public String KeiyakuCode { get; set; }
		/// <summary>
		/// 契約名称を取得・設定します。
		/// </summary>
		public String KeiyakuName { get; set; }
		/// <summary>
		/// 作業大分類IDを取得・設定します。
		/// </summary>
		public Decimal SagyoDaiBunruiId { get; set; }
		/// <summary>
		/// 作業中分類IDを取得・設定します。
		/// </summary>
		public Decimal SagyoChuBunruiId { get; set; }
		/// <summary>
		/// 作業小分類IDを取得・設定します。
		/// </summary>
		public Decimal SagyoShoBunruiId { get; set; }
		/// <summary>
		/// 契約開始日を取得・設定します。
		/// </summary>
		public DateTime KeiyakuStartDate { get; set; }
		/// <summary>
		/// 契約終了日を取得・設定します。
		/// </summary>
		public DateTime KeiyakuEndDate { get; set; }
		/// <summary>
		/// 契約状況を取得・設定します。
		/// </summary>
		public Int32 KeiyakuJyokyo { get; set; }
		/// <summary>
		/// 作業人数を取得・設定します。
		/// </summary>
		public Int32 SagyoNinzu { get; set; }
		/// <summary>
		/// 作業日区分を取得・設定します。
		/// </summary>
		public Int32 SagyobiKbn { get; set; }
		/// <summary>
		/// 月曜日を取得・設定します。
		/// </summary>
		public Boolean Monday { get; set; }
		/// <summary>
		/// 火曜日を取得・設定します。
		/// </summary>
		public Boolean Tuesday { get; set; }
		/// <summary>
		/// 水曜日を取得・設定します。
		/// </summary>
		public Boolean Wednesday { get; set; }
		/// <summary>
		/// 木曜日を取得・設定します。
		/// </summary>
		public Boolean Thursday { get; set; }
		/// <summary>
		/// 金曜日を取得・設定します。
		/// </summary>
		public Boolean Friday { get; set; }
		/// <summary>
		/// 土曜日を取得・設定します。
		/// </summary>
		public Boolean Saturday { get; set; }
		/// <summary>
		/// 日曜日を取得・設定します。
		/// </summary>
		public Boolean Sunday { get; set; }
		/// <summary>
		/// 月指定日1を取得・設定します。
		/// </summary>
		public Decimal SagyoDay1 { get; set; }
		/// <summary>
		/// 月指定日2を取得・設定します。
		/// </summary>
		public Decimal SagyoDay2 { get; set; }
		/// <summary>
		/// 月指定日3を取得・設定します。
		/// </summary>
		public Decimal SagyoDay3 { get; set; }
		/// <summary>
		/// 月指定日4を取得・設定します。
		/// </summary>
		public Decimal SagyoDay4 { get; set; }
		/// <summary>
		/// 月指定日5を取得・設定します。
		/// </summary>
		public Decimal SagyoDay5 { get; set; }
		/// <summary>
		/// 指定年月日1を取得・設定します。
		/// </summary>
		public DateTime SagyoDate1 { get; set; }
		/// <summary>
		/// 指定年月日2を取得・設定します。
		/// </summary>
		public DateTime SagyoDate2 { get; set; }
		/// <summary>
		/// 指定年月日3を取得・設定します。
		/// </summary>
		public DateTime SagyoDate3 { get; set; }
		/// <summary>
		/// 指定年月日4を取得・設定します。
		/// </summary>
		public DateTime SagyoDate4 { get; set; }
		/// <summary>
		/// 指定年月日5を取得・設定します。
		/// </summary>
		public DateTime SagyoDate5 { get; set; }
		/// <summary>
		/// 指定年月日6を取得・設定します。
		/// </summary>
		public DateTime SagyoDate6 { get; set; }
		/// <summary>
		/// 指定年月日7を取得・設定します。
		/// </summary>
		public DateTime SagyoDate7 { get; set; }
		/// <summary>
		/// 指定年月日8を取得・設定します。
		/// </summary>
		public DateTime SagyoDate8 { get; set; }
		/// <summary>
		/// 指定年月日9を取得・設定します。
		/// </summary>
		public DateTime SagyoDate9 { get; set; }
		/// <summary>
		/// 指定年月日10を取得・設定します。
		/// </summary>
		public DateTime SagyoDate10 { get; set; }
		/// <summary>
		/// ShelterIDを取得・設定します。
		/// </summary>
		public Decimal ShelterId { get; set; }
		/// <summary>
		/// 非表示フラグを取得・設定します。
		/// </summary>
		public Boolean DisableFlag { get; set; }
		/// <summary>
		/// 削除フラグを取得・設定します。
		/// </summary>
		public Boolean DelFlag { get; set; }

		#region 関連項目

		/// <summary>
		/// 得意コードを取得・設定します。
		/// </summary>
		public String TokuisakiCode { get; set; }
		/// <summary>
		/// 得意先名称を取得・設定します。
		/// </summary>
		public String TokuisakiName { get; set; }
		/// <summary>
		/// 作業場所コードを取得・設定します。
		/// </summary>
		public String SagyoBashoCode { get; set; }
		/// <summary>
		/// 作業場所名称を取得・設定します。
		/// </summary>
		public String SagyoBashoName { get; set; }

		/// <summary>
		/// 契作業大分類名称を取得・設定します。
		/// </summary>
		public String SagyoDaiBunruiName { get; set; }

		/// <summary>
		/// 作業中分類名称を取得・設定します。
		/// </summary>
		public String SagyoChuBunruiName { get; set; }

		/// <summary>
		/// 作業小分類名称を取得・設定します。
		/// </summary>
		public String SagyoShoBunruiName { get; set; }

		#endregion

		public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 契約検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KeiyakuSearchParameter
    {
		/// <summary>
		/// 契約IDを取得・設定します。
		/// </summary>
		public Decimal? KeiyakuId { get; set; }
		/// <summary>
		/// 契約コードを取得・設定します。
		/// </summary>
		public String KeiyakuCode { get; set; }
		/// <summary>
		/// 契約コードを取得・設定します。
		/// </summary>
		public String KeiyakuCodeAmbiguous { get; set; }
		/// <summary>
		/// 契約名称を取得・設定します。
		/// </summary>
		public String KeiyakuName { get; set; }
		/// <summary>
		/// 得意先IDを取得・設定します。
		/// </summary>
		public Decimal? TokuisakiId { get; set; }
		/// <summary>
		/// 作業場所IDを取得・設定します。
		/// </summary>
		public Decimal? SagyoBashoId { get; set; }
		/// <summary>
		/// 作業大分類IDを取得・設定します。
		/// </summary>
		public Decimal? SagyoDaiBunruiId { get; set; }
		/// <summary>
		/// 作業中分類IDを取得・設定します。
		/// </summary>
		public Decimal? SagyoChuBunruiId { get; set; }
		/// <summary>
		/// 作業小分類IDを取得・設定します。
		/// </summary>
		public Decimal? SagyoShoBunruiId { get; set; }
		/// <summary>
		/// 日付指定区分を取得・設定します。
		/// </summary>
		public Int32 FilterDateKbns { get; set; }
		/// <summary>
		/// 開始日を取得・設定します。
		/// </summary>
		public DateTime StartYMD { get; set; }
		/// <summary>
		/// 終了日を取得・設定します。
		/// </summary>
		public DateTime EndYMD { get; set; }
		/// <summary>
		/// 非表示フラグを取得・設定します。
		/// </summary>
		public Boolean? DisableFlag { get; set; }
		/// <summary>
		/// ShelterIDを取得・設定します。
		/// </summary>
		public Decimal? ShelterId { get; set; }
		/// <summary>
		/// 重複フラグを取得・設定します。
		/// </summary>
		public bool DuplicateFlg { get; set; } = false;
	}
}
