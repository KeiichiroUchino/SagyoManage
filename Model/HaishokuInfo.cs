using System;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配色情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishokuInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 配色IDを取得・設定します。
        /// </summary>
        public Decimal HaishokuId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32 TableKey { get; set; }
        /// <summary>
        /// テーブルIDを取得・設定します。
        /// </summary>
        public Decimal TableId { get; set; }
        /// <summary>
        /// システム区分を取得・設定します。
        /// </summary>
        public Int32 SystemKbn { get; set; }
        /// <summary>
        /// システムIDを取得・設定します。
        /// </summary>
        public Int32 SystemId { get; set; }
        /// <summary>
        /// 文字色を取得・設定します。
        /// </summary>
        public Int32 ForeColor { get; set; }
        /// <summary>
        /// 背景色を取得・設定します。
        /// </summary>
        public Int32 BackColor { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

        #region

        /// <summary>
        /// テーブルコードを取得・設定します。
        /// </summary>
        public Int32 TableCode { get; set; }
        /// <summary>
        /// テーブル名称を取得・設定します。
        /// </summary>
        public String TableName { get; set; }
        /// <summary>
        /// システム区分名称を取得・設定します。
        /// </summary>
        public String SystemKbnName { get; set; }

        #endregion
    }

    /// <summary>
    /// 配色検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishokuSearchParameter
    {
        /// <summary>
        /// 配色IDを取得・設定します。
        /// </summary>
        public Decimal? HaishokuId { get; set; }
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32? TableKey { get; set; }
        /// <summary>
        /// テーブルIDを取得・設定します。
        /// </summary>
        public Decimal? TableId { get; set; }
        /// <summary>
        /// システム区分を取得・設定します。
        /// </summary>
        public Int32? SystemKbn { get; set; }
        /// <summary>
        /// システムIDを取得・設定します。
        /// </summary>
        public Int32? SystemId { get; set; }
    }

    /// <summary>
    /// 配色情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishokuExInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// 配色IDを取得・設定します。
        /// </summary>
        public Decimal HaishokuId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32 TableKey { get; set; }
        /// <summary>
        /// 機能キーを取得・設定します。
        /// </summary>
        public Int32 FunctionKey { get; set; }
        /// <summary>
        /// 対象キーを取得・設定します。
        /// </summary>
        public Decimal TargetKey { get; set; }
        /// <summary>
        /// 文字色を取得・設定します。
        /// </summary>
        public Int32? ForeColor { get; set; }
        /// <summary>
        /// 背景色を取得・設定します。
        /// </summary>
        public Int32? BackColor { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

        #region

        /// <summary>
        /// 対象キーコードを取得・設定します。
        /// </summary>
        public Int32 TargetKeyCode { get; set; }
        /// <summary>
        /// 対象キー名称を取得・設定します。
        /// </summary>
        public String TargetKeyName { get; set; }

        #endregion
    }

    /// <summary>
    /// 配色検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishokuExSearchParameter
    {
        /// <summary>
        /// 配色IDを取得・設定します。
        /// </summary>
        public Decimal? HaishokuId { get; set; }
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32? TableKey { get; set; }
        /// <summary>
        /// 機能キーを取得・設定します。
        /// </summary>
        public Int32? FunctionKey { get; set; }
        /// <summary>
        /// 対象キーを取得・設定します。
        /// </summary>
        public Decimal? TargetKey { get; set; }
        /// <summary>
        /// 非表示取得フラグを取得・設定します。
        /// </summary>
        public Boolean? GetDisableDataFlag { get; set; }
    }

    /// <summary>
    /// テーブル情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TableKeyInfo
    {
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32 TableKey { get; set; }
        /// <summary>
        /// テーブルキー名称を取得・設定します。
        /// </summary>
        public String TableKeyName { get; set; }
    }

    /// <summary>
    /// 機能情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class FunctionKeyInfo
    {
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32 TableKey { get; set; }
        /// <summary>
        /// 機能キーを取得・設定します。
        /// </summary>
        public Int32 FunctionKey { get; set; }
        /// <summary>
        /// 機能キー名称を取得・設定します。
        /// </summary>
        public String FunctionKeyName { get; set; }
    }

    /// <summary>
    /// 対象情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class TargetKeyInfo
    {
        /// <summary>
        /// テーブルキーを取得・設定します。
        /// </summary>
        public Int32 TableKey { get; set; }
        /// <summary>
        /// 機能キーを取得・設定します。
        /// </summary>
        public Int32 FunctionKey { get; set; }
        /// <summary>
        /// 対象キーを取得・設定します。
        /// </summary>
        public Decimal TargetKey { get; set; }
        /// <summary>
        /// 対象コードを取得・設定します。
        /// </summary>
        public Int32 TargetKeyCode { get; set; }
        /// <summary>
        /// 対象名を取得・設定します。
        /// </summary>
        public String TargetKeyName { get; set; }
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
    }
}
