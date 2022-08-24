using System;
using System.Collections.Generic;
using System.Drawing;

namespace Jpsys.HaishaManageV10.Model
{

    /// <summary>
    /// 配車入力画面のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaIchiranSearchParameter
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
        /// 伝票No.を取得・設定します。
        /// </summary>
        public decimal? SlipNo { get; set; }
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
        /// 車種コードを取得・設定します。
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
        /// 配車入力情報の削除情報を取得・設定します。
        /// </summary>
        public Dictionary<decimal, HaishaNyuryokuInfo> HaishaIchiranDel { get; set; }
        /// <summary>
        /// 受注IDを取得・設定します。
        /// </summary>
        public Decimal JuchuId { get; set; }
        /// <summary>
        /// 車両区分を取得・設定します。
        /// </summary>
        public Int32? CarKbn { get; set; }
        /// <summary>
        /// 配車用営業所IDを取得・設定します。
        /// </summary>
        public Decimal? HaishaBranchOfficeId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }

    }

    /// <summary>
    /// 車両情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaIchiranCarInfo
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
        /// 車番を取得・設定します。
        /// </summary>
        public string LicPlateCarNo { get; set; }
        /// <summary>
        /// 配車入力車両カテゴリ背景色を取得・設定します。
        /// </summary>
        public int? HaishaNyuryokuCarBackColor { get; set; }
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
    }

    /// <summary>
    /// appoint情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaIchiranAppointInfo
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
        /// 休日フラグを取得・設定します。
        /// </summary>
        public bool KyujitsuFlg { get; set; }
        /// <summary>
        /// 休日の文字色を取得・設定します。
        /// </summary>
        public Color KyujitsuFontColor { get; set; }
    }
    

}
