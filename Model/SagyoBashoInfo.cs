using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 作業場所情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoBashoInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 作業場所IDを取得・設定します。
        /// </summary>
        public Decimal SagyoBashoId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiId { get; set; }
        /// <summary>
        /// 作業場所コードを取得・設定します。
        /// </summary>
        public String SagyoBashoCode { get; set; }
        /// <summary>
        /// 作業場所名称を取得・設定します。
        /// </summary>
        public String SagyoBashoName { get; set; }
        /// <summary>
        /// 住所1を取得・設定します。
        /// </summary>
        public String SagyoBashoAddress1 { get; set; }
        /// <summary>
        /// 住所2を取得・設定します。
        /// </summary>
        public String SagyoBashoAddress2 { get; set; }
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
        /// 得意先コードを取得・設定します。
        /// </summary>
        public String TokuisakiCode { get; set; }
        /// <summary>
        /// 得意先名称を取得・設定します。
        /// </summary>
        public String TokuisakiName { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

    }

    /// <summary>
    /// 作業場所検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SagyoBashoSearchParameter
    {
        /// <summary>
        /// 作業場所IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoBashoId { get; set; }
        /// <summary>
        /// 得意先IDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiId { get; set; }
        /// <summary>
        /// 作業場所コードを取得・設定します。
        /// </summary>
        public String SagyoBashoCode { get; set; }
        /// <summary>
        /// 作業場所コード(あいまい検索)を取得・設定します。
        /// </summary>
        public String SagyoBashoCodeAmbiguous { get; set; }
        /// <summary>
        /// 作業場所名称を取得・設定します。
        /// </summary>
        public String SagyoBashoName { get; set; }
        /// <summary>
        /// ShelterIDを取得・設定します。
        /// </summary>
        public Decimal? ShelterId { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean? DisableFlag { get; set; }
        /// <summary>
        /// 重複フラグを取得・設定します。
        /// </summary>
        public bool DuplicateFlg { get; set; } = false;
    }
}
