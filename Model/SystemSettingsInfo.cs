using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// システム設定情報のエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class SystemSettingsInfo
    {
        /// <summary>
        /// システム管理IDを取得・設定します。
        /// </summary>
        public Decimal SystemSettingsId { get; set; }
        /// <summary>
        /// タイトル背景色を取得・設定します。
        /// </summary>
        public String FrameTitleBackColor { get; set; }
        /// <summary>
        /// フッター背景色を取得・設定します。
        /// </summary>
        public String FrameFooterBackColor { get; set; }
        /// <summary>
        /// 表ヘッダー背景色を取得・設定します。
        /// </summary>
        public String FrameGridHeaderBackColor { get; set; }
        /// <summary>
        /// 表選択行背景色を取得・設定します。
        /// </summary>
        public String FrameGridSelectionBackColor { get; set; }
        /// <summary>
        /// 配車入力条件区分を取得・設定します。
        /// </summary>
        public Int32 HaishaNyuryokuJokenKbn { get; set; }
        /// <summary>
        /// 営業所管理区分を取得・設定します。
        /// </summary>
        public Int32 EigyoshoKanriKbn { get; set; }
        /// <summary>
        /// 受注発着日計算区分を取得・設定します。
        /// </summary>
        public Int32 JuchuYMDCalculationKbn { get; set; }
        /// <summary>
        /// 配車受注一覧ソート区分を取得・設定します。
        /// </summary>
        public Int32 HaishaJuchuListSortKbn { get; set; }
        /// <summary>
        /// 配車休日チェックフラグを取得・設定します。
        /// </summary>
        public Boolean HaishaKyujitsuCheckFlag { get; set; }
        /// <summary>
        /// 配車発着日チェックフラグを取得・設定します。
        /// </summary>
        public Boolean HaishaYMDCheckFlag { get; set; }
        /// <summary>
        /// 配車発着日初期値区分を取得・設定します。
        /// </summary>
        public Int32 HaishaYMDDefaultKbn { get; set; }
        /// <summary>
        /// 請求連携日報区分を取得・設定します。
        /// </summary>
        public Int32 SeikyuRenkeiDailyReportKbn { get; set; }
        /// <summary>
        /// 請求連携上書区分を取得・設定します。
        /// </summary>
        public Int32 SeikyuRenkeiUwagakiKbn { get; set; }
        /// <summary>
        /// SendGridのAPIキーを取得・設定します。
        /// </summary>
        public String SendGridApiKey { get; set; }
        /// <summary>
        /// トラDONバージョン区分を取得・設定します。
        /// </summary>
        public Int32 TraDonVersionKbn { get; set; }
    }
}
