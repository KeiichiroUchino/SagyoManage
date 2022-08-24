using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 得意先情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 得意コードを取得・設定します。
        /// </summary>
        public String TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 住所1を取得・設定します。
        /// </summary>
        public String TokuisakiAddress1 { get; set; }
        /// <summary>
        /// 住所2を取得・設定します。
        /// </summary>
        public String TokuisakiAddress2 { get; set; }
        /// <summary>
        /// TELを取得・設定します。
        /// </summary>
        public String TokuisakiTel { get; set; }
        /// <summary>
        /// FAXを取得・設定します。
        /// </summary>
        public String TokuisakiFax { get; set; }
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

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 得意先検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiSearchParameter
    {
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiId { get; set; }
        /// <summary>
        /// 得意コードを取得・設定します。
        /// </summary>
        public String TokuisakiCode { get; set; }
        /// <summary>
        /// 得意コード(あいまい検索用)を取得・設定します。
        /// </summary>
        public String TokuisakiCodeAmbiguous { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }
        /// <summary>
        /// 住所1を取得・設定します。
        /// </summary>
        public String TokuisakiAddress1 { get; set; }
        /// <summary>
        /// 住所2を取得・設定します。
        /// </summary>
        public String TokuisakiAddress2 { get; set; }
        /// <summary>
        /// TELを取得・設定します。
        /// </summary>
        public String TokuisakiTel { get; set; }
        /// <summary>
        /// FAXを取得・設定します。
        /// </summary>
        public String TokuisakiFax { get; set; }
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
