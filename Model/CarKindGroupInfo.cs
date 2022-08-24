using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 車種グループマスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindGroupInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 車種グループIDを取得・設定します。
        /// </summary>
        public Decimal CarKindGroupId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 CarKindGroupCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String CarKindGroupName { get; set; }
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
        /// 車種グループ明細情報を取得・設定します。
        /// </summary>
        public IList<CarKindGroupMeisaiInfo> CarKindGroupMeisaiList { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 車種グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindGroupSearchParameter
    {
        /// <summary>
        /// 車種グループIDを取得・設定します。
        /// </summary>
        public Decimal? CarKindGroupId { get; set; }
        /// <summary>
        /// 車種グループコードを取得・設定します。
        /// </summary>
        public Int32? CarKindGroupCode { get; set; }
    }
}
