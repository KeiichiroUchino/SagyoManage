using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 車両（トラDON補）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarInfo : AbstractSequenceKeyTimeStampModelBase
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
        /// 車番を取得・設定します。
        /// </summary>
        public String LicPlateCarNo { get; set; }
        /// <summary>
        /// 車両名称を取得・設定します。
        /// </summary>
        public String CarName { get; set; }
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

        /// <summary>
        /// 使用停止を取得します。
        /// </summary>
        public string ShiyoTeishi
        {
            get { return (this.DisableFlag) ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 車両検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarSearchParameter
    {
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal? CarId { get; set; }
        /// <summary>
        /// 車両コードを取得・設定します。
        /// </summary>
        public Int32? CarCode { get; set; }
    }
}
