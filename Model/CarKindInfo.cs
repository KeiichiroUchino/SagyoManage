using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 車種情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 車種IDを取得・設定します。
        /// </summary>
        public Decimal CarKindId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// トラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONCarKindId { get; set; }
        /// <summary>
        /// 乗務員掛率を取得・設定します。
        /// </summary>
        public Decimal JomuinKakeritsu { get; set; }
        /// <summary>
        /// トラDON連携車種IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONRenkeiCarKindId { get; set; }
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
        /// トラDON車種コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONCarKindCode { get; set; }
        /// <summary>
        /// トラDON車種名称を取得・設定します。
        /// </summary>
        public String ToraDONCarKindName { get; set; }
        /// <summary>
        /// トラDON車種カナを取得・設定します。
        /// </summary>
        public String ToraDONCarKindNameKana { get; set; }
        /// <summary>
        /// トラDON車種略称を取得・設定します。
        /// </summary>
        public String ToraDONCarKindSNM { get; set; }
        /// <summary>
        /// トラDON非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }
        /// <summary>
        /// トラDON削除フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDelFlag { get; set; }

        /// <summary>
        /// トラDON連携車種コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONRenkeiCarKindCode { get; set; }
        /// <summary>
        /// トラDON連携車種名称を取得・設定します。
        /// </summary>
        public String ToraDONRenkeiCarKindName { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return (this.DisableFlag || this.ToraDONDisableFlag) ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 車種検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class CarKindSearchParameter
    {
        /// <summary>
        /// トラDON車種IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONCarKindId { get; set; }
        /// <summary>
        /// トラDON車種コードを取得・設定します。
        /// </summary>
        public Int32? ToraDONCarKindCode { get; set; }
    }
}
