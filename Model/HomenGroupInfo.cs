using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 方面グループマスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenGroupInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 方面グループIDを取得・設定します。
        /// </summary>
        public Decimal HomenGroupId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 HomenGroupCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String HomenGroupName { get; set; }
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
        /// 方面グループ明細情報を取得・設定します。
        /// </summary>
        public IList<HomenGroupMeisaiInfo> HomenGroupMeisaiList { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 方面グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HomenGroupSearchParameter
    {
        /// <summary>
        /// 方面グループIDを取得・設定します。
        /// </summary>
        public Decimal? HomenGroupId { get; set; }
        /// <summary>
        /// 方面グループコードを取得・設定します。
        /// </summary>
        public Int32? HomenGroupCode { get; set; }
    }
}
