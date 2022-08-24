using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 管理マスタのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class SystemPropertyInfo : AbstractTimeStampModelBase
    {
		/// <summary>
		/// 管理IDを取得・設定します。
		/// </summary>
		public Decimal SystemPropertyId { get; set; }
		/// <summary>
		/// メールアドレスを取得・設定します。
		/// </summary>
		public String Email { get; set; }
		/// <summary>
		/// メールアドレス送信者名を取得・設定します。
		/// </summary>
		public String SenderName { get; set; }
		/// <summary>
		/// メールタイトルを取得・設定します。
		/// </summary>
		public String MailTitle { get; set; }
		/// <summary>
		/// メール本文ひな形を取得・設定します。
		/// </summary>
		public String MailBody { get; set; }
		/// <summary>
		/// PDF一時フォルダパスを取得・設定します。
		/// </summary>
		public String PDFTempFolderPath { get; set; }
		/// <summary>
		/// SendGridのAPIキーを取得・設定します。
		/// </summary>
		public String SendGridApiKey { get; set; }
		/// <summary>
		/// 会社名を取得・設定します。
		/// </summary>
		public String CompanyName { get; set; }
		/// <summary>
		/// 日付切替時間を取得・設定します。
		/// </summary>
		public Int32 DateSwitchingTime { get; set; }
	}

    /// <summary>
    /// 管理検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class SystemPropertySearchParameter
    {
        /// <summary>
        /// 管理IDを取得・設定します。
        /// </summary>
        public Decimal? KanriId { get; set; }
    }
}
