using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 得意先グループマスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiGroupInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 得意先グループIDを取得・設定します。
        /// </summary>
        public Decimal TokuisakiGroupId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 TokuisakiGroupCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String TokuisakiGroupName { get; set; }
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
        /// 得意先グループ明細情報を取得・設定します。
        /// </summary>
        public IList<TokuisakiGroupMeisaiInfo> TokuisakiGroupMeisaiList { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 得意先グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TokuisakiGroupSearchParameter
    {
        /// <summary>
        /// 得意先グループIDを取得・設定します。
        /// </summary>
        public Decimal? TokuisakiGroupId { get; set; }
        /// <summary>
        /// 得意先グループコードを取得・設定します。
        /// </summary>
        public Int32? TokuisakiGroupCode { get; set; }
    }
}
