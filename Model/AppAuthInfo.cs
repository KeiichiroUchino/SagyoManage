using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// ビジネスロジックを利用するアプリケーション側の認証済みの
    /// 利用者情報及び利用する呼びだし元の情報を格納するクラスです
    /// </summary>
    /// <remarks>
    /// ビジネスロジック利用時に、利用元の情報としてビジネスロジックに
    /// 設定します。ビジネスロジック側は必要に応じてデータアクセスレイヤ
    /// にこの情報を引き渡します。
    /// </remarks>
    [Serializable]
    public class AppAuthInfo
    {
        /// <summary>
        /// オペレータIDを取得・設定します。
        /// </summary>
        public decimal OperatorId { get; set; }

        /// <summary>
        /// オペレータ名を取得・設定します。
        /// </summary>
        public string OperetorName { get; set; }

        /// <summary>
        /// 端末IDを取得・設定します。
        /// </summary>
        public string TerminalId { get; set; }

        /// <summary>
        /// 処理IDを取得・設定します。
        /// </summary>
        public string UserProcessId { get; set; }

        /// <summary>
        /// 処理名を取得・設定します。おもに画面タイトルです。
        /// </summary>
        public string UserProcessName { get; set; }

        /// <summary>
        /// アプリケーション認証情報の内容を連結した文字列で取得します。
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            //return base.ToString();

            StringBuilder sb = new StringBuilder();
            sb.Append(this.OperatorId.ToString());
            sb.Append("_");
            sb.Append(this.TerminalId.Trim());
            sb.Append("_");
            sb.Append(this.UserProcessId.Trim());
            sb.Append("_");
            sb.Append(this.UserProcessName.Trim());

            return sb.ToString();
        }
    }
}
