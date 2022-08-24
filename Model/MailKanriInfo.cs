using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// メール管理マスタのエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class MailKanriInfo : AbstractTimeStampModelBase
    {
        /// <summary>
        /// メール管理IDを取得・設定します。
        /// </summary>
        public Decimal MailKanriId { get; set; }
        /// <summary>
        /// SMTPサーバを取得・設定します。
        /// </summary>
        public String SMTPServer { get; set; }
        /// <summary>
        /// 送信ポート番号を取得・設定します。
        /// </summary>
        public Int32 SoshinPortBango { get; set; }
        /// <summary>
        /// メールアカウントを取得・設定します。
        /// </summary>
        public String MailAccount { get; set; }
        /// <summary>
        /// メールパスワードを取得・設定します。
        /// </summary>
        public String MailPassword { get; set; }
        /// <summary>
        /// メールタイトルを取得・設定します。
        /// </summary>
        public String MailTitle { get; set; }
    }

    /// <summary>
    /// メール管理検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class MailKanriSearchParameter
    {
        /// <summary>
        /// メール管理IDを取得・設定します。
        /// </summary>
        public Decimal? MailKanriId { get; set; }
    }
}
