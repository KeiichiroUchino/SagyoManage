using System;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 排他制御情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ExclusiveControlInfo : AbstractTimeStampModelBase
    {
        /// <summary>
        /// 排他制御区分を取得・設定します。
        /// </summary>
        public Int32 ExclusiveControlKbn { get; set; }
        /// <summary>
        /// 機能区分を取得・設定します。
        /// </summary>
        public Int32 KinoKbn { get; set; }
        /// <summary>
        /// 処理値を取得・設定します。
        /// </summary>
        public Decimal ShoriValue { get; set; }
        /// <summary>
        /// ステータス区分を取得・設定します。
        /// </summary>
        public Int32 StatusKbn { get; set; }
        /// <summary>
        /// 利用者IDを取得・設定します。
        /// </summary>
        public Decimal OperatorId { get; set; }

        #region 関連項目

        /// <summary>
        /// 利用者コードを取得・設定します。
        /// </summary>
        public string OperatorCode { get; set; }
        /// <summary>
        /// 利用者名称を取得・設定します。
        /// </summary>
        public String OperatorName { get; set; }

        #endregion
    }

    /// <summary>
    /// 排他制御検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class ExclusiveControlSearchParameter
    {
        /// <summary>
        /// 排他制御区分を取得・設定します。
        /// </summary>
        public Int32? ExclusiveControlKbn { get; set; }
        /// <summary>
        /// 機能区分を取得・設定します。
        /// </summary>
        public Int32? KinoKbn { get; set; }
        /// <summary>
        /// 処理値を取得・設定します。
        /// </summary>
        public Decimal? ShoriValue { get; set; }
        /// <summary>
        /// ステータス区分を取得・設定します。
        /// </summary>
        public Int32? StatusKbn { get; set; }
        /// <summary>
        /// 除外利用者IDを取得・設定します。
        /// </summary>
        public Decimal? ExclusionOperatorId { get; set; }
    }
}
