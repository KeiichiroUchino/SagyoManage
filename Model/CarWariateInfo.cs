using System;
using System.Collections.Generic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 作業員割当情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarWariateInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 車両員割当IDを取得・設定します。
        /// </summary>
        public Decimal CarWariateId { get; set; }
        /// <summary>
        /// 作業案件IDを取得・設定します。
        /// </summary>
        public Decimal SagyoAnkenId { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal CarId { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

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

        #endregion

    }

    /// <summary>
    /// 作業員割当情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarWariateSearchParameter
    {
        /// <summary>
        /// 検索区分を取得・設定します。
        /// </summary>
        public int SearchKbn { get; set; }
        /// <summary>
        /// 車両員割当IDを取得・設定します。
        /// </summary>
        public Decimal? CarWariateId { get; set; }
        /// <summary>
        /// 作業案件IDを取得・設定します。
        /// </summary>
        public Decimal? SagyoAnkenId { get; set; }
        /// <summary>
        /// 作業開始日時を取得・設定します。
        /// </summary>
        public DateTime? SagyoStartDateTime { get; set; }
        /// <summary>
        /// 作業終了日時を取得・設定します。
        /// </summary>
        public DateTime? SagyoEndDateTime { get; set; }
        /// <summary>
        /// 車両IDを取得・設定します。
        /// </summary>
        public Decimal? CarId { get; set; }
        /// <summary>
        /// 車両IDリストを取得・設定します。
        /// </summary>
        public List<Decimal> CarIdList { get; set; }

    }
}
