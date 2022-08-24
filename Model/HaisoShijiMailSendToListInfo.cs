using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 社員（トラDON補）情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijiMailSendToListInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 配送指示メール送信先リストIDを取得・設定します。
        /// </summary>
        public Decimal HaisoShijiMailSendToListId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONStaffId { get; set; }
        /// <summary>
        /// メールアドレスを取得・設定します。
        /// </summary>
        public String Email { get; set; }
        /// <summary>
        /// 送信可否フラグを取得・設定します。
        /// </summary>
        public Boolean SendFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        #region 関連項目

        /// <summary>
        /// トラDON社員コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONStaffCode { get; set; }
        /// <summary>
        /// トラDON社員名称を取得・設定します。
        /// </summary>
        public String ToraDONStaffName { get; set; }
        /// <summary>
        /// トラDON社員カナを取得・設定します。
        /// </summary>
        public String ToraDONStaffNameKana { get; set; }
        /// <summary>
        /// トラDON非表示フラグを取得・設定します。
        /// </summary>
        public Boolean ToraDONDisableFlag { get; set; }
        /// <summary>
        /// トラDON営業所IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONBranchOfficeId { get; set; }
        /// <summary>
        /// トラDON営業所コードを取得・設定します。
        /// </summary>
        public Int32 ToraDONBranchOfficeCode { get; set; }
        /// <summary>
        /// トラDON営業所名称を取得・設定します。
        /// </summary>
        public String ToraDONBranchOfficeName { get; set; }
        /// <summary>
        /// トラDON退職年月日を取得・設定します。
        /// </summary>
        public DateTime ToraDONRetireDate { get; set; }

        #endregion

        public string ShiyoTeishi
        {
            get { return this.ToraDONDisableFlag ? "停止中" : string.Empty; }
        }
    }

    /// <summary>
    /// 配送指示メール送信先リスト検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaisoShijiMailSendToListSearchParameter
    {
        /// <summary>
        /// トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal? ToraDONStaffId { get; set; }
        /// <summary>
        /// 運転者フラグを取得・設定します。
        /// </summary>
        public Boolean? DriverFlag { get; set; }
    }
}
