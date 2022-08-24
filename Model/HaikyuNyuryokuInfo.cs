using Jpsys.HaishaManageV10.BizProperty;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配給入力キー情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaikyuNyuryokuKeyInfo
    {
        #region 農場情報

        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }
        /// <summary>
        /// 農場コードを取得・設定します。
        /// </summary>
        public Int32 NojoCode { get; set; }
        /// <summary>
        /// 農場名を取得・設定します。
        /// </summary>
        public String NojoName { get; set; }
        /// <summary>
        /// 警告日数を取得・設定します。
        /// </summary>
        public Int32 KeikokuNissu { get; set; }
        /// <summary>
        /// 請求日区分を取得・設定します。
        /// </summary>
        public Int32 SeikyubiKbn { get; set; }

        /// <summary>
        /// 表示用農場名を取得・設定します。
        /// </summary>
        public string DispNojoName
        {
            get { return this.NojoName; }
        }

        #endregion

        #region 農場タンク情報

        /// <summary>
        /// 農場タンクIDを取得・設定します。
        /// </summary>
        public Decimal NojoTankId { get; set; }
        /// <summary>
        /// 農場タンクナンバーを取得・設定します。
        /// </summary>
        public Int32 NojoTankNo { get; set; }
        /// <summary>
        /// 農場タンク名を取得・設定します。
        /// </summary>
        public String NojoTankName { get; set; }
        /// <summary>
        /// 引取工場IDを取得・設定します。
        /// </summary>
        public Decimal HikitoriKojoId { get; set; }
        /// <summary>
        /// 引取工場コードを取得・設定します。
        /// </summary>
        public Int32 HikitoriKojoCode { get; set; }
        /// <summary>
        /// 引取工場名を取得・設定します。
        /// </summary>
        public string HikitoriKojoName { get; set; }
        /// <summary>
        /// 請求先IDを取得・設定します。
        /// </summary>
        public Decimal SeikyusakiId { get; set; }
        /// <summary>
        /// 請求先コードを取得・設定します。
        /// </summary>
        public Int32 SeikyusakiCode { get; set; }
        /// <summary>
        /// 請求先名を取得・設定します。
        /// </summary>
        public string SeikyusakiName { get; set; }
        /// <summary>
        /// 発地IDを取得・設定します。
        /// </summary>
        public Decimal HatchiId { get; set; }
        /// <summary>
        /// 発地コードを取得・設定します。
        /// </summary>
        public Int32 HatchiCode { get; set; }
        /// <summary>
        /// 発地名を取得・設定します。
        /// </summary>
        public string HatchiName { get; set; }
        /// <summary>
        /// 着地IDを取得・設定します。
        /// </summary>
        public Decimal ChakuchiId { get; set; }
        /// <summary>
        /// 着地コードを取得・設定します。
        /// </summary>
        public Int32 ChakuchiCode { get; set; }
        /// <summary>
        /// 着地名を取得・設定します。
        /// </summary>
        public string ChakuchiName { get; set; }
        /// <summary>
        /// 配給開始時間を取得・設定します。
        /// </summary>
        public Int32 HaikyuStartHMS { get; set; }
        /// <summary>
        /// 配給終了時間を取得・設定します。
        /// </summary>
        public Int32 HaikyuEndHMS { get; set; }
        /// <summary>
        /// パターン区分を取得・設定します。
        /// </summary>
        public Int32 PatternKbn { get; set; }
        /// <summary>
        /// 食量計算フラグを取得・設定します。
        /// </summary>
        public Boolean KuiryoKeisanFlag { get; set; }
        /// <summary>
        /// 納入量パターンフラグを取得・設定します。
        /// </summary>
        public Boolean NonyuryoPatternFlag { get; set; }
        /// <summary>
        /// 背景色_配給入力を取得・設定します。
        /// </summary>
        public Int32 BackColor_HaikyuNyuryoku { get; set; }
        /// <summary>
        /// 前景色_配給入力を取得・設定します。
        /// </summary>
        public Int32 ForeColor_HaikyuNyuryoku { get; set; }

        /// <summary>
        /// 農場タンクコードを取得・設定します。
        /// </summary>
        public Int32 NojoTankCode
        {
            get { return Convert.ToInt32(Convert.ToInt32(this.NojoCode).ToString() + String.Format("{0:00}", this.NojoTankNo)); }
        }

        #endregion

        #region 排他情報

        /// <summary>
        /// 排他フラグを取得・設定します。
        /// （false：更新可、true：更新不可）
        /// </summary>
        public Boolean HaitaFlag { get; set; }

        #endregion

        #region 過去直近配給情報

        /// <summary>
        /// 配給IDを取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_HaikyuId { get; set; }
        /// <summary>
        /// 日付を取得・設定します。
        /// </summary>
        public DateTime OldHaikyu_HizukeYMD { get; set; }
        /// <summary>
        /// パターンID01を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_PatternId01 { get; set; }
        /// <summary>
        /// パターンID02を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_PatternId02 { get; set; }
        /// <summary>
        /// 日数01を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_Nissu01 { get; set; }
        /// <summary>
        /// 日数02を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_Nissu02 { get; set; }
        /// <summary>
        /// 餌ID01を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_EsaId01 { get; set; }
        /// <summary>
        /// 餌ID02を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_EsaId02 { get; set; }
        /// <summary>
        /// 羽数01を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_Hasu01 { get; set; }
        /// <summary>
        /// 羽数02を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_Hasu02 { get; set; }
        /// <summary>
        /// 食量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_Kuiryo { get; set; }
        /// <summary>
        /// 実食量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_JitsuKuiryo { get; set; }
        /// <summary>
        /// タンク容量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_TankYoryo { get; set; }
        /// <summary>
        /// 餌残量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_EsaZanryo { get; set; }
        /// <summary>
        /// 旧餌残量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_OldEsaZanryo { get; set; }

        /// <summary>
        /// 餌コードを取得・設定します。
        /// </summary>
        public Int32 OldHaikyu_ItemCode { get; set; }
        /// <summary>
        /// 餌略略名を取得・設定します。
        /// </summary>
        public String OldHaikyu_ItemSShortName { get; set; }
        /// <summary>
        /// パターン01明細食量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_PatternMeisai01_Kuiryo { get; set; }
        /// <summary>
        /// パターン02明細食量を取得・設定します。
        /// </summary>
        public Decimal OldHaikyu_PatternMeisai02_Kuiryo { get; set; }

        #endregion

        #region 過去直近配車情報

        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Decimal OldHaisha_HaishaId { get; set; }
        /// <summary>
        /// 納入日を取得・設定します。
        /// </summary>
        public DateTime OldHaisha_NonyuYMD { get; set; }
        /// <summary>
        /// 農場指定を取得・設定します。
        /// </summary>
        public Boolean OldHaisha_NojoShiteiFlag { get; set; }
        /// <summary>
        /// 餌確定フラグを取得・設定します。
        /// </summary>
        public Boolean OldHaisha_EsaKakuteiFlag { get; set; }

        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 OldHaisha_CarCode { get; set; }
        /// <summary>
        /// 社員名を取得・設定します。
        /// </summary>
        public String OldHaisha_StaffName { get; set; }
        /// <summary>
        /// 餌略略名を取得・設定します。
        /// </summary>
        public String OldHaisha_ItemSShortName { get; set; }

        #endregion
    }

    /// <summary>
    /// 配給入力情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaikyuNyuryokuInfo
    {
        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }
        /// <summary>
        /// 農場タンクIDを取得・設定します。
        /// </summary>
        public Decimal NojoTankId { get; set; }

        #region 農場、農場タンクの関連情報

        /// <summary>
        /// 農場コードを取得・設定します。
        /// </summary>
        public Int32 NojoCode { get; set; }
        /// <summary>
        /// 農場略称を取得・設定します。
        /// </summary>
        public String NojoShortName { get; set; }
        /// <summary>
        /// 警告日数を取得・設定します。
        /// </summary>
        public Int32 KeikokuNissu { get; set; }
        /// <summary>
        /// 農場タンクナンバーを取得・設定します。
        /// </summary>
        public Int32 NojoTankNo { get; set; }
        /// <summary>
        /// 農場タンク名を取得・設定します。
        /// </summary>
        public String NojoTankName { get; set; }
        /// <summary>
        /// 農場タンク略称を取得・設定します。
        /// </summary>
        public String NojoTankShortName { get; set; }
        /// <summary>
        /// 食量計算フラグを取得・設定します。
        /// </summary>
        public Boolean KuiryoKeisanFlag { get; set; }

        #endregion

        #region 配給ワーク情報

        /// <summary>
        /// 配給IDを取得・設定します。
        /// </summary>
        public Int64 WK_Haikyu_HaikyuId { get; set; }
        /// <summary>
        /// 日付を取得・設定します。
        /// </summary>
        public DateTime WK_Haikyu_HizukeYMD { get; set; }
        /// <summary>
        /// パターンID01を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_PatternId01 { get; set; }
        /// <summary>
        /// 日数01を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Nissu01 { get; set; }
        /// <summary>
        /// 餌ID01を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_EsaId01 { get; set; }
        /// <summary>
        /// 羽数01を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Hasu01 { get; set; }
        /// <summary>
        /// パターンID02を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_PatternId02 { get; set; }
        /// <summary>
        /// 日数02を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Nissu02 { get; set; }
        /// <summary>
        /// 餌ID02を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_EsaId02 { get; set; }
        /// <summary>
        /// 羽数02を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Hasu02 { get; set; }
        /// <summary>
        /// 食量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Kuiryo { get; set; }
        /// <summary>
        /// 実食量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_JitsuKuiryo { get; set; }
        /// <summary>
        /// タンク容量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_TankYoryo { get; set; }
        /// <summary>
        /// 餌残量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_EsaZanryo { get; set; }
        /// <summary>
        /// 旧餌残量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_OldEsaZanryo { get; set; }

        /// <summary>
        /// 餌01コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haikyu_Esa01_ItemCode { get; set; }
        /// <summary>
        /// 餌01名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Esa01_ItemName { get; set; }
        /// <summary>
        /// 餌01単位重量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Esa01_ItemWeight { get; set; }
        /// <summary>
        /// 餌01略略名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Esa01_ItemSShortName { get; set; }
        /// <summary>
        /// 餌01有薬フラグを取得・設定します。
        /// </summary>
        public Boolean WK_Haikyu_Esa01_ItemYuYakuFlag { get; set; }
        /// <summary>
        /// 餌02コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haikyu_Esa02_ItemCode { get; set; }
        /// <summary>
        /// 餌02名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Esa02_ItemName { get; set; }
        /// <summary>
        /// 餌02単位重量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_Esa02_ItemWeight { get; set; }
        /// <summary>
        /// 餌02略略名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Esa02_ItemSShortName { get; set; }
        /// <summary>
        /// 餌02有薬フラグを取得・設定します。
        /// </summary>
        public Boolean WK_Haikyu_Esa02_ItemYuYakuFlag { get; set; }
        /// <summary>
        /// パターン01コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haikyu_Pattern01_PatternCode { get; set; }
        /// <summary>
        /// パターン01名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Pattern01_PatternName { get; set; }
        /// <summary>
        /// パターン02コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haikyu_Pattern02_PatternCode { get; set; }
        /// <summary>
        /// パターン02名称を取得・設定します。
        /// </summary>
        public String WK_Haikyu_Pattern02_PatternName { get; set; }
        /// <summary>
        /// パターン01明細食量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_PatternMeisai01_Kuiryo { get; set; }
        /// <summary>
        /// パターン02明細食量を取得・設定します。
        /// </summary>
        public Decimal WK_Haikyu_PatternMeisai02_Kuiryo { get; set; }

        #endregion

        #region イベントワーク情報（タンク残以外：イベント区分の昇順第一位のデータ）

        /// <summary>
        /// イベントID（タンク残以外）を取得・設定します。
        /// </summary>
        public Int64 WK_Event_EventId { get; set; }
        /// <summary>
        /// イベント日付を取得・設定します。
        /// </summary>
        public DateTime WK_Event_EventYMD { get; set; }
        /// <summary>
        /// イベント区分を取得・設定します。
        /// </summary>
        public Int32 WK_Event_EventKbn { get; set; }
        /// <summary>
        /// 入荷数01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Nyukasu01 { get; set; }
        /// <summary>
        /// 入荷数02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Nyukasu02 { get; set; }
        /// <summary>
        /// パターンID01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_PatternId01 { get; set; }
        /// <summary>
        /// パターンID02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_PatternId02 { get; set; }
        /// <summary>
        /// 開始日数01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_StartDay01 { get; set; }
        /// <summary>
        /// 開始日数02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_StartDay02 { get; set; }
        /// <summary>
        /// 食割01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Kuiwari01 { get; set; }
        /// <summary>
        /// 食割02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Kuiwari02 { get; set; }
        /// <summary>
        /// 固定食量01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_KoteiKuiryo01 { get; set; }
        /// <summary>
        /// 固定食量02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_KoteiKuiryo02 { get; set; }
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal WK_Event_HinmokuId { get; set; }
        /// <summary>
        /// 食量を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Kuiryo { get; set; }
        /// <summary>
        /// 羽数01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Hasu01 { get; set; }
        /// <summary>
        /// 羽数02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Hasu02 { get; set; }
        /// <summary>
        /// 換羽開始日を取得・設定します。
        /// </summary>
        public DateTime WK_Event_KanuStartYMD { get; set; }
        /// <summary>
        /// 出荷数01を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Shukkasu01 { get; set; }
        /// <summary>
        /// 出荷数02を取得・設定します。
        /// </summary>
        public Decimal WK_Event_Shukkasu02 { get; set; }
        /// <summary>
        /// イベント区分略名を取得・設定します。
        /// </summary>
        public String WK_Event_EventKbnShortName { get; set; }

        /// <summary>
        /// 品目略略名を取得・設定します。
        /// </summary>
        public String WK_Event_ItemSShortName { get; set; }
        /// <summary>
        /// パターン01餌略略名を取得・設定します。
        /// </summary>
        public String WK_Event_PatternMeisai01_ItemSShortName { get; set; }
        /// <summary>
        /// パターン02餌略略名を取得・設定します。
        /// </summary>
        public String WK_Event_PatternMeisai02_ItemSShortName { get; set; }

        #endregion

        #region イベントワーク情報（タンク残）

        /// <summary>
        /// イベントID（タンク残）を取得・設定します。
        /// </summary>
        public Int64 WK_Event_TankZan_EventId { get; set; }
        /// <summary>
        /// イベント区分（タンク残）を取得・設定します。
        /// </summary>
        public Int32 WK_Event_TankZan_EventKbn { get; set; }
        /// <summary>
        /// タンク残を取得・設定します。
        /// </summary>
        public Decimal WK_Event_TankZan_TankZan { get; set; }
        /// <summary>
        /// 納入時刻を取得・設定します。
        /// </summary>
        public Decimal WK_Event_TankZan_NonyuHMS { get; set; }
        /// <summary>
        /// 摘要を取得・設定します。
        /// </summary>
        public String WK_Event_TankZan_Tekiyo { get; set; }

        #endregion

        #region 配車ワーク情報

        /// <summary>
        /// 配車ワーク_更新区分を取得・設定します。
        /// </summary>
        public Int32 WK_Haisha_KoshinKbn { get; set; }
        /// <summary>
        /// 配車IDを取得・設定します。
        /// </summary>
        public Int64 WK_Haisha_HaishaId { get; set; }
        /// <summary>
        /// タンク区分を取得・設定します。
        /// </summary>
        public Int32 WK_Haisha_TankKbn { get; set; }
        /// <summary>
        /// 納入日を取得・設定します。
        /// </summary>
        public DateTime WK_Haisha_NonyuYMD { get; set; }
        /// <summary>
        /// 積込日を取得・設定します。
        /// </summary>
        public DateTime WK_Haisha_TsumikomiYMD { get; set; }
        /// <summary>
        /// 自傭区分を取得・設定します。
        /// </summary>
        public Int32 WK_Haisha_JiyoKbn { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_SharyoId { get; set; }
        /// <summary>
        /// 傭車IDを取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_YoshaId { get; set; }
        /// <summary>
        /// 乗務員IDを取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_StaffId { get; set; }
        /// <summary>
        /// 餌ID01を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaId01 { get; set; }
        /// <summary>
        /// 餌容量01を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaYoryo01 { get; set; }
        /// <summary>
        /// 餌略略名を取得・設定します。
        /// </summary>
        public String WK_Haisha_ItemSShortName01 { get; set; }
        /// <summary>
        /// 有薬フラグ01を取得・設定します。
        /// </summary>
        public Boolean WK_Haisha_YuYakuFlag01 { get; set; }
        /// <summary>
        /// 餌ID02を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaId02 { get; set; }
        /// <summary>
        /// 餌容量02を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaYoryo02 { get; set; }
        /// <summary>
        /// 餌略略名02を取得・設定します。
        /// </summary>
        public String WK_Haisha_ItemSShortName02 { get; set; }
        /// <summary>
        /// 有薬フラグ02を取得・設定します。
        /// </summary>
        public Boolean WK_Haisha_YuYakuFlag02 { get; set; }
        /// <summary>
        /// 餌ID03を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaId03 { get; set; }
        /// <summary>
        /// 餌容量03を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaYoryo03 { get; set; }
        /// <summary>
        /// 餌略略名03を取得・設定します。
        /// </summary>
        public String WK_Haisha_ItemSShortName03 { get; set; }
        /// <summary>
        /// 有薬フラグ03を取得・設定します。
        /// </summary>
        public Boolean WK_Haisha_YuYakuFlag03 { get; set; }
        /// <summary>
        /// 農場指定を取得・設定します。
        /// </summary>
        public Boolean WK_Haisha_NojoShiteiFlag { get; set; }
        /// <summary>
        /// 餌確定フラグを取得・設定します。
        /// </summary>
        public Boolean WK_Haisha_EsaKakuteiFlag { get; set; }

        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haisha_CarCode { get; set; }
        /// <summary>
        /// 社員コードを取得・設定します。
        /// </summary>
        public Int32 WK_Haisha_StaffCode { get; set; }
        /// <summary>
        /// 社員名を取得・設定します。
        /// </summary>
        public String WK_Haisha_StaffName { get; set; }
        /// <summary>
        /// 餌名を取得・設定します。
        /// </summary>
        public String WK_Haisha_ItemName { get; set; }

        /// <summary>
        /// 餌全容量を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaZenYoryo
        {
            get
            {
                return this.WK_Haisha_EsaYoryo01 +
                    this.WK_Haisha_EsaYoryo02 +
                    this.WK_Haisha_EsaYoryo03;
            }
        }

        #endregion
    }

    /// <summary>
    /// 配給入力検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaikyuNyuryokuSearchParameter
    {
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal? OperatorId { get; set; }
        /// <summary>
        /// 配給入力（抽出条件）を取得・設定します。
        /// </summary>
        public HaikyuNyuryokuConditionInfo HaikyuNyuryokuConditionInfo { get; set; }
    }

    /// <summary>
    /// タンク残計算用配給ワーク情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class WK_HaikyuCalcInfo : AbstractTimeStampModelBase
    {
        #region 配給情報

        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }
        /// <summary>
        /// 更新区分を取得・設定します。
        /// </summary>
        public Int32 KoshinKbn { get; set; }
        /// <summary>
        /// 配給IDを取得・設定します。
        /// </summary>
        public Int64 HaikyuId { get; set; }
        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }
        /// <summary>
        /// 農場タンクIDを取得・設定します。
        /// </summary>
        public Decimal NojoTankId { get; set; }
        /// <summary>
        /// 日付を取得・設定します。
        /// </summary>
        public DateTime HizukeYMD { get; set; }
        /// <summary>
        /// パターンID01を取得・設定します。
        /// </summary>
        public Decimal PatternId01 { get; set; }
        /// <summary>
        /// 日数01を取得・設定します。
        /// </summary>
        public Decimal Nissu01 { get; set; }
        /// <summary>
        /// 餌ID01を取得・設定します。
        /// </summary>
        public Decimal EsaId01 { get; set; }
        /// <summary>
        /// 羽数01を取得・設定します。
        /// </summary>
        public Decimal Hasu01 { get; set; }
        /// <summary>
        /// パターンID02を取得・設定します。
        /// </summary>
        public Decimal PatternId02 { get; set; }
        /// <summary>
        /// 日数02を取得・設定します。
        /// </summary>
        public Decimal Nissu02 { get; set; }
        /// <summary>
        /// 餌ID02を取得・設定します。
        /// </summary>
        public Decimal EsaId02 { get; set; }
        /// <summary>
        /// 羽数02を取得・設定します。
        /// </summary>
        public Decimal Hasu02 { get; set; }
        /// <summary>
        /// 食量を取得・設定します。
        /// </summary>
        public Decimal Kuiryo { get; set; }
        /// <summary>
        /// 実食量を取得・設定します。
        /// </summary>
        public Decimal JitsuKuiryo { get; set; }
        /// <summary>
        /// タンク容量を取得・設定します。
        /// </summary>
        public Decimal TankYoryo { get; set; }
        /// <summary>
        /// 餌残量を取得・設定します。
        /// </summary>
        public Decimal EsaZanryo { get; set; }
        /// <summary>
        /// 旧餌残量を取得・設定します。
        /// </summary>
        public Decimal OldEsaZanryo { get; set; }

        #endregion

        #region 入荷情報

        /// <summary>
        /// 入荷イベント区分を取得・設定します。
        /// </summary>
        public Int32 Nyuka_EventKbn { get; set; }
        /// <summary>
        /// 入荷数01を取得・設定します。
        /// </summary>
        public Decimal Nyuka_Nyukasu01 { get; set; }
        /// <summary>
        /// 入荷数02を取得・設定します。

        public Decimal Nyuka_Nyukasu02 { get; set; }

        #endregion

        #region パターン設定変更情報

        /// <summary>
        /// パターン設定変更イベント区分を取得・設定します。
        /// </summary>
        public Int32 Pattern_EventKbn { get; set; }
        /// <summary>
        /// パターンID01を取得・設定します。
        /// </summary>
        public Decimal Pattern_PatternId01 { get; set; }
        /// <summary>
        /// パターンID02を取得・設定します。
        /// </summary>
        public Decimal Pattern_PatternId02 { get; set; }
        /// <summary>
        /// 開始日数01を取得・設定します。
        /// </summary>
        public Decimal Pattern_StartDay01 { get; set; }
        /// <summary>
        /// 開始日数02を取得・設定します。
        /// </summary>
        public Decimal Pattern_StartDay02 { get; set; }
        /// <summary>
        /// 食割01を取得・設定します。
        /// </summary>
        public Decimal Pattern_Kuiwari01 { get; set; }
        /// <summary>
        /// 食割02を取得・設定します。
        /// </summary>
        public Decimal Pattern_Kuiwari02 { get; set; }
        /// <summary>
        /// 固定食量01を取得・設定します。
        /// </summary>
        public Decimal Pattern_KoteiKuiryo01 { get; set; }
        /// <summary>
        /// 固定食量02を取得・設定します。
        /// </summary>
        public Decimal Pattern_KoteiKuiryo02 { get; set; }

        #endregion

        #region 餌設定変更情報

        /// <summary>
        /// 餌設定変更イベント区分を取得・設定します。
        /// </summary>
        public Int32 Item_EventKbn { get; set; }
        /// <summary>
        /// 品目IDを取得・設定します。
        /// </summary>
        public Decimal Item_ItemId { get; set; }

        #endregion

        #region 食量設定変更情報

        /// <summary>
        /// 食量設定変更イベント区分を取得・設定します。
        /// </summary>
        public Int32 Kuiryo_EventKbn { get; set; }
        /// <summary>
        /// 食量を取得・設定します。
        /// </summary>
        public Decimal Kuiryo_Kuiryo { get; set; }

        #endregion

        #region タンク容量設定変更情報

        /// <summary>
        /// タンク容量設定変更イベント区分を取得・設定します。
        /// </summary>
        public Int32 TankRyo_EventKbn { get; set; }
        /// <summary>
        /// タンク量を取得・設定します。
        /// </summary>
        public Decimal TankRyo_TankRyo { get; set; }

        #endregion

        #region 羽数変更情報

        /// <summary>
        /// 羽数変更イベント区分を取得・設定します。
        /// </summary>
        public Int32 Hasu_EventKbn { get; set; }
        /// <summary>
        /// 羽数01を取得・設定します。
        /// </summary>
        public Decimal Hasu_Hasu01 { get; set; }
        /// <summary>
        /// 羽数02を取得・設定します。
        /// </summary>
        public Decimal Hasu_Hasu02 { get; set; }

        #endregion

        #region 出荷情報

        /// <summary>
        /// 出荷イベント区分を取得・設定します。
        /// </summary>
        public Int32 Shukka_EventKbn { get; set; }
        /// <summary>
        /// 出荷数01を取得・設定します。
        /// </summary>
        public Decimal Shukka_Shukkasu01 { get; set; }
        /// <summary>
        /// 出荷数02を取得・設定します。
        /// </summary>
        public Decimal Shukka_Shukkasu02 { get; set; }

        #endregion

        #region 強制換羽情報

        /// <summary>
        /// 強制換羽イベント区分を取得・設定します。
        /// </summary>
        public Int32 KyoseiKanu_EventKbn { get; set; }
        /// <summary>
        /// 換羽開始日を取得・設定します。
        /// </summary>
        public DateTime KyoseiKanu_KanuStartYMD { get; set; }
        /// <summary>
        /// 換羽開始時刻を取得・設定します。
        /// </summary>
        public Int32 KyoseiKanu_KanuStartHMS { get; set; }
        /// <summary>
        /// 換羽終了日を取得・設定します。
        /// </summary>
        public DateTime KyoseiKanu_KanuEndYMD { get; set; }
        /// <summary>
        /// 換羽終了時刻を取得・設定します。
        /// </summary>
        public Int32 KyoseiKanu_KanuEndHMS { get; set; }

        #endregion

        #region 出荷換羽情報

        /// <summary>
        /// 出荷換羽イベント区分を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanu_EventKbn { get; set; }
        /// <summary>
        /// 換羽開始日を取得・設定します。
        /// </summary>
        public DateTime ShukkaKanu_KanuStartYMD { get; set; }
        /// <summary>
        /// 換羽開始時刻を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanu_KanuStartHMS { get; set; }
        /// <summary>
        /// 換羽終了日を取得・設定します。
        /// </summary>
        public DateTime ShukkaKanu_KanuEndYMD { get; set; }
        /// <summary>
        /// 換羽終了時刻を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanu_KanuEndHMS { get; set; }

        /// <summary>
        /// 出荷換羽イベント区分（開始日以外）を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanuAnother_EventKbn { get; set; }
        /// <summary>
        /// 換羽開始日（開始日以外）を取得・設定します。
        /// </summary>
        public DateTime ShukkaKanuAnother_KanuStartYMD { get; set; }
        /// <summary>
        /// 換羽開始時刻（開始日以外）を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanuAnother_KanuStartHMS { get; set; }
        /// <summary>
        /// 換羽終了日（開始日以外）を取得・設定します。
        /// </summary>
        public DateTime ShukkaKanuAnother_KanuEndYMD { get; set; }
        /// <summary>
        /// 換羽終了時刻（開始日以外）を取得・設定します。
        /// </summary>
        public Int32 ShukkaKanuAnother_KanuEndHMS { get; set; }

        #endregion

        #region 餌受入情報

        /// <summary>
        /// 餌受入イベント区分を取得・設定します。
        /// </summary>
        public Int32 EsaIdoUkeire_EventKbn { get; set; }
        /// <summary>
        /// 受入量を取得・設定します。
        /// </summary>
        public Decimal EsaIdoUkeire_UkeireRyo { get; set; }

        #endregion

        #region 餌払出情報

        /// <summary>
        /// 餌払出イベント区分を取得・設定します。
        /// </summary>
        public Int32 EsaIdoHaraidashi_EventKbn { get; set; }
        /// <summary>
        /// 払出量を取得・設定します。
        /// </summary>
        public Decimal EsaIdoHaraidashi_HaraidashiRyo { get; set; }

        #endregion

        #region タンク残情報

        /// <summary>
        /// タンク残イベント区分を取得・設定します。
        /// </summary>
        public Int32 TankZan_EventKbn { get; set; }
        /// <summary>
        /// タンク残を取得・設定します。
        /// </summary>
        public Decimal TankZan_TankZan { get; set; }
        /// <summary>
        /// 納入時刻を取得・設定します。
        /// </summary>
        public Decimal TankZan_NonyuHMS { get; set; }

        #endregion

        #region 配車情報

        /// <summary>
        /// 餌全容量を取得・設定します。
        /// </summary>
        public Decimal WK_Haisha_EsaZenYoryo { get; set; }

        #endregion
    }

    /// <summary>
    /// 配給入力更新キー情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class KoshinHaikyuNyuryokuKeyInfo
    {
        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }
        /// <summary>
        /// 農場タンクIDを取得・設定します。
        /// </summary>
        public Decimal NojoTankId { get; set; }
        /// <summary>
        /// 更新開始日付を取得・設定します。
        /// </summary>
        public DateTime KoshinStartYMD { get; set; }
    }
}
