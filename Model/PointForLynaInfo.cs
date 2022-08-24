using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 所在地（LYNA）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointForLynaInfo
    {
        /// <summary>
        /// 所在地IDを取得・設定します。
        /// </summary>
        public String PointId { get; set; }
        /// <summary>
        /// 発着地コードを取得・設定します。
        /// </summary>
        public String PointCode { get; set; }
        /// <summary>
        /// 所在地名称を取得・設定します。
        /// </summary>
        public String PointName { get; set; }
        /// <summary>
        /// 所在地種別を取得・設定します。
        /// </summary>
        public Int32 PointKbn { get; set; }
        /// <summary>
        /// 住所を取得・設定します。
        /// </summary>
        public String Address { get; set; }
        /// <summary>
        /// 電話番号を取得・設定します。
        /// </summary>
        public String Tel { get; set; }
        /// <summary>
        /// 経度を取得・設定します。
        /// </summary>
        public Decimal Keido { get; set; }
        /// <summary>
        /// 緯度を取得・設定します。
        /// </summary>
        public Decimal Ido { get; set; }
        /// <summary>
        /// 追加荷捌き時間を取得・設定します。
        /// </summary>
        public Decimal TsuikaNisabakiHMS { get; set; }
        /// <summary>
        /// 位置精度を取得・設定します。
        /// </summary>
        public Int32 IchiSedo { get; set; }
        /// <summary>
        /// 備考を取得・設定します。
        /// </summary>
        public String Biko { get; set; }
        /// <summary>
        /// 開始時刻を取得・設定します。
        /// </summary>
        public Decimal KaishiHMS { get; set; }
        /// <summary>
        /// 終了時刻を取得・設定します。
        /// </summary>
        public Decimal ShuryoHMS { get; set; }
        /// <summary>
        /// グループ1を取得・設定します。
        /// </summary>
        public String Group01 { get; set; }
        /// <summary>
        /// グループ2を取得・設定します。
        /// </summary>
        public String Group02 { get; set; }
        /// <summary>
        /// グループ3を取得・設定します。
        /// </summary>
        public String Group03 { get; set; }
        ///// <summary>
        ///// グループ4を取得・設定します。
        ///// </summary>
        //public String Group04 { get; set; }
        ///// <summary>
        ///// グループ5を取得・設定します。
        ///// </summary>
        //public String Group05 { get; set; }
        ///// <summary>
        ///// グループ6を取得・設定します。
        ///// </summary>
        //public String Group06 { get; set; }
        ///// <summary>
        ///// グループ7を取得・設定します。
        ///// </summary>
        //public String Group07 { get; set; }
        ///// <summary>
        ///// グループ8を取得・設定します。
        ///// </summary>
        //public String Group08 { get; set; }
        ///// <summary>
        ///// グループ9を取得・設定します。
        ///// </summary>
        //public String Group09 { get; set; }
        ///// <summary>
        ///// グループ10を取得・設定します。
        ///// </summary>
        //public String Group10 { get; set; }
        ///// <summary>
        ///// グループ11を取得・設定します。
        ///// </summary>
        //public String Group11 { get; set; }
        ///// <summary>
        ///// グループ12を取得・設定します。
        ///// </summary>
        //public String Group12 { get; set; }
        ///// <summary>
        ///// グループ13を取得・設定します。
        ///// </summary>
        //public String Group13 { get; set; }
        ///// <summary>
        ///// グループ14を取得・設定します。
        ///// </summary>
        //public String Group14 { get; set; }
        ///// <summary>
        ///// グループ15を取得・設定します。
        ///// </summary>
        //public String Group15 { get; set; }
        ///// <summary>
        ///// グループ16を取得・設定します。
        ///// </summary>
        //public String Group16 { get; set; }
        ///// <summary>
        ///// グループ17を取得・設定します。
        ///// </summary>
        //public String Group17 { get; set; }
        ///// <summary>
        ///// グループ18を取得・設定します。
        ///// </summary>
        //public String Group18 { get; set; }
        ///// <summary>
        ///// グループ19を取得・設定します。
        ///// </summary>
        //public String Group19 { get; set; }
        ///// <summary>
        ///// グループ20を取得・設定します。
        ///// </summary>
        //public String Group20 { get; set; }
        /// <summary>
        /// 指定車型1を取得・設定します。
        /// </summary>
        public String ShiteiShagata01 { get; set; }
        /// <summary>
        /// 指定車型2を取得・設定します。
        /// </summary>
        public String ShiteiShagata02 { get; set; }
        /// <summary>
        /// 指定車型3を取得・設定します。
        /// </summary>
        public String ShiteiShagata03 { get; set; }
        ///// <summary>
        ///// 指定車型4を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata04 { get; set; }
        ///// <summary>
        ///// 指定車型5を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata05 { get; set; }
        ///// <summary>
        ///// 指定車型6を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata06 { get; set; }
        ///// <summary>
        ///// 指定車型7を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata07 { get; set; }
        ///// <summary>
        ///// 指定車型8を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata08 { get; set; }
        ///// <summary>
        ///// 指定車型9を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata09 { get; set; }
        ///// <summary>
        ///// 指定車型10を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata10 { get; set; }
        ///// <summary>
        ///// 指定車型11を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata11 { get; set; }
        ///// <summary>
        ///// 指定車型12を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata12 { get; set; }
        ///// <summary>
        ///// 指定車型13を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata13 { get; set; }
        ///// <summary>
        ///// 指定車型14を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata14 { get; set; }
        ///// <summary>
        ///// 指定車型15を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata15 { get; set; }
        ///// <summary>
        ///// 指定車型16を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata16 { get; set; }
        ///// <summary>
        ///// 指定車型17を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata17 { get; set; }
        ///// <summary>
        ///// 指定車型18を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata18 { get; set; }
        ///// <summary>
        ///// 指定車型19を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata19 { get; set; }
        ///// <summary>
        ///// 指定車型20を取得・設定します。
        ///// </summary>
        //public String ShiteiShagata20 { get; set; }
        /// <summary>
        /// 指定車両1を取得・設定します。
        /// </summary>
        public String ShiteiCarCd01 { get; set; }
        /// <summary>
        /// 指定車両2を取得・設定します。
        /// </summary>
        public String ShiteiCarCd02 { get; set; }
        /// <summary>
        /// 指定車両3を取得・設定します。
        /// </summary>
        public String ShiteiCarCd03 { get; set; }
        ///// <summary>
        ///// 指定車両4を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd04 { get; set; }
        ///// <summary>
        ///// 指定車両5を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd05 { get; set; }
        ///// <summary>
        ///// 指定車両6を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd06 { get; set; }
        ///// <summary>
        ///// 指定車両7を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd07 { get; set; }
        ///// <summary>
        ///// 指定車両8を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd08 { get; set; }
        ///// <summary>
        ///// 指定車両9を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd09 { get; set; }
        ///// <summary>
        ///// 指定車両10を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd10 { get; set; }
        ///// <summary>
        ///// 指定車両11を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd11 { get; set; }
        ///// <summary>
        ///// 指定車両12を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd12 { get; set; }
        ///// <summary>
        ///// 指定車両13を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd13 { get; set; }
        ///// <summary>
        ///// 指定車両14を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd14 { get; set; }
        ///// <summary>
        ///// 指定車両15を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd15 { get; set; }
        ///// <summary>
        ///// 指定車両16を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd16 { get; set; }
        ///// <summary>
        ///// 指定車両17を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd17 { get; set; }
        ///// <summary>
        ///// 指定車両18を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd18 { get; set; }
        ///// <summary>
        ///// 指定車両19を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd19 { get; set; }
        ///// <summary>
        ///// 指定車両20を取得・設定します。
        ///// </summary>
        //public String ShiteiCarCd20 { get; set; }

        #region 関連項目

        #endregion
    }

    /// <summary>
    /// 所在地（LYNA）検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointForLynaSearchParameter
    {
        /// <summary>
        /// 所在地IDを取得・設定します。
        /// </summary>
        public String PointId { get; set; }
    }

    /// <summary>
    /// 所在地（LYNA）連携情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class PointForLynaSaveInfo
    {
        /// <summary>
        /// 登録所在地リストを取得・設定します。
        /// </summary>
        public IList<PointForLynaInfo> AddList { get; set; }
        /// <summary>
        /// 削除所在地リストを取得・設定します。
        /// </summary>
        public IList<PointForLynaInfo> DelList { get; set; }
        /// <summary>
        /// 更新所在地リストを取得・設定します。
        /// </summary>
        public IList<PointForLynaInfo> UpdList { get; set; }
    }
}
