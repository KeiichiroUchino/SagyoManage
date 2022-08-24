using System;
using System.Collections.Generic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 販路グループマスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroGroupInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 販路グループIDを取得・設定します。
        /// </summary>
        public Decimal HanroGroupId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 HanroGroupCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String HanroGroupName { get; set; }
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
        /// 販路グループ明細情報を取得・設定します。
        /// </summary>
        public IList<HanroGroupMeisaiInfo> HanroGroupMeisaiList { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 販路グループ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HanroGroupSearchParameter
    {
        /// <summary>
        /// 販路グループIDを取得・設定します。
        /// </summary>
        public Decimal? HanroGroupId { get; set; }
        /// <summary>
        /// 販路グループコードを取得・設定します。
        /// </summary>
        public Int32? HanroGroupCode { get; set; }
    }
}
