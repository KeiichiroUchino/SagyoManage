using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// LYNADBの車両情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarForLynaInfo
    {
        /// <summary>
        /// 車両Idを取得・設定します。
        /// </summary>
        public String CarId { get; set; }
        /// <summary>
        /// 車両名称を取得・設定します。
        /// </summary>
        public String CarName { get; set; }
        /// <summary>
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 車種名称を取得・設定します。
        /// </summary>
        public String CarKindName { get; set; }
        /// <summary>
        /// 車型Idを取得・設定します。
        /// </summary>
        public String ShagataId { get; set; }
        /// <summary>
        /// 出発地Idを取得・設定します。
        /// </summary>
        public String CarStartPointId { get; set; }
        /// <summary>
        /// 終了地Idを取得・設定します。
        /// </summary>
        public String CarEndPointId { get; set; }
        /// <summary>
        /// 開始可能時刻を取得・設定します。
        /// </summary>
        public Decimal KaishiKanouHMS { get; set; }
        /// <summary>
        /// 開始限界時刻を取得・設定します。
        /// </summary>
        public Decimal KaishiGenkaiHMS { get; set; }
        /// <summary>
        /// 終了限界時刻を取得・設定します。
        /// </summary>
        public Decimal ShuryoGenkaiHMS { get; set; }
        /// <summary>
        /// 最大時間を取得・設定します。
        /// </summary>
        public Decimal MaxHMS { get; set; }
        /// <summary>
        /// 最大超勤時間を取得・設定します。
        /// </summary>
        public Decimal MaxChokinHMS { get; set; }
        /// <summary>
        /// 最大距離を取得・設定します。
        /// </summary>
        public Decimal MaxKyori { get; set; }
        /// <summary>
        /// 最大注文数を取得・設定します。
        /// </summary>
        public Decimal MaxChumonSu { get; set; }
        /// <summary>
        /// 1回転当最大注文数を取得・設定します。
        /// </summary>
        public Decimal IchikaitenAtariMaxChumonSu { get; set; }
        /// <summary>
        /// 最大回転数を取得・設定します。
        /// </summary>
        public Decimal MaxKaitenSu { get; set; }
        /// <summary>
        /// 乗務員Idを取得・設定します。
        /// </summary>
        public String StaffId { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// グループId01を取得・設定します。
        /// </summary>
        public String LynaGroupId01 { get; set; }
        /// <summary>
        /// グループId02を取得・設定します。
        /// </summary>
        public String LynaGroupId02 { get; set; }
        /// <summary>
        /// グループId03を取得・設定します。
        /// </summary>
        public String LynaGroupId03 { get; set; }
        /// <summary>
        /// グループId04を取得・設定します。
        /// </summary>
        public String LynaGroupId04 { get; set; }
        /// <summary>
        /// グループId05を取得・設定します。
        /// </summary>
        public String LynaGroupId05 { get; set; }
        ///// <summary>
        ///// グループId06を取得・設定します。
        ///// </summary>
        //public String LynaGroupId06 { get; set; }
        ///// <summary>
        ///// グループId07を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId07 { get; set; }
        ///// <summary>
        ///// グループId08を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId08 { get; set; }
        ///// <summary>
        ///// グループId09を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId09 { get; set; }
        ///// <summary>
        ///// グループId10を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId10 { get; set; }
        ///// <summary>
        ///// グループId11を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId11 { get; set; }
        ///// <summary>
        ///// グループId12を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId12 { get; set; }
        ///// <summary>
        ///// グループId13を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId13 { get; set; }
        ///// <summary>
        ///// グループId14を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId14 { get; set; }
        ///// <summary>
        ///// グループId15を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId15 { get; set; }
        ///// <summary>
        ///// グループId16を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId16 { get; set; }
        ///// <summary>
        ///// グループId17を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId17 { get; set; }
        ///// <summary>
        ///// グループId18を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId18 { get; set; }
        ///// <summary>
        ///// グループId19を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId19 { get; set; }
        ///// <summary>
        ///// グループId20を取得・設定します。
        ///// </summary>
        //public Decimal LynaGroupId20 { get; set; }

        #region 関連項目

        /// <summary>
        /// 車型名を取得・設定します。
        /// </summary>
        public String ShagataName { get; set; }

        #endregion
    }

    /// <summary>
    /// LYNADBの車両検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarForLynaSearchParameter
    {
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public String CarId { get; set; }
    }

    /// <summary>
    /// LYNADBの車両連携情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarForLynaSaveInfo
    {
        /// <summary>
        /// 初期化フラグを取得・設定します。
        /// </summary>
        public Boolean InitFlag { get; set; }
        /// <summary>
        /// 登録車両リストを取得・設定します。
        /// </summary>
        public IList<CarForLynaInfo> AddList { get; set; }
        /// <summary>
        /// 削除車両リストを取得・設定します。
        /// </summary>
        public IList<CarForLynaInfo> DelList { get; set; }
        /// <summary>
        /// 更新車両リストを取得・設定します。
        /// </summary>
        public IList<CarForLynaInfo> UpdList { get; set; }
    }
}
