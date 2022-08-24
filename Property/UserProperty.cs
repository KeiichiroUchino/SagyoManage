using Jpsys.SagyoManage.Model;
using Jpsys.SagyoManage.SQLServerDAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Property
{
    public class UserProperty
    {
        /// <summary>
        /// インスタンスの保持用
        /// </summary>
        private static UserProperty myInstance;

        /// <summary>
        /// プライベートコンストラクターです。シングルトンに
        /// するために、プライベートで宣言します。
        /// </summary>
        private UserProperty()
        {
        }

        /// <summary>
        /// 本クラスのインスタンスを作成します。既にインスタンスが作成されて
        /// 居る場合は、新しく作成しません。
        /// </summary>
        private static void CreateInstance()
        {
            if (UserProperty.myInstance == null)
            {
                UserProperty.myInstance = new UserProperty();
            }
            UserProperty.myInstance.SetProperty();

        }

        /// <summary>
        /// 各プロパティに必要な値を初期値として設定します。
        /// </summary>
        private void SetProperty()
        {
            //NetBIOS名を取得して設定
            this.LoginTerminalId = Environment.MachineName;

        }

        /// <summary>
        /// UserPropertyクラスの単一インスタンスを取得します。
        /// </summary>
        /// <returns>本クラスの単一インスタンス</returns>
        public static UserProperty GetInstance()
        {
            UserProperty.CreateInstance();

            return UserProperty.myInstance;

        }

        /// <summary>
        /// 本クラス内の静的プロパティを最新の値にセットしなおします。
        /// </summary>
        public void Refresh()
        {
            if (UserProperty.myInstance == null)
            {
                UserProperty.myInstance = new UserProperty();
            }
            this.SetProperty();

        }

        #region 自動実装プロパティ

        /// <summary>
        /// ログインしているOperatorIdを取得・設定します。
        /// </summary>
        public decimal LoginOperetorId { get; set; }
        /// <summary>
        /// ログインしているOperatorのLoginCodeを取得・設定します。
        /// </summary>
        public string LoginCode { get; set; }
        /// <summary>
        /// ログインしているOperatorの社員名を取得・設定します。
        /// </summary>
        public string LoginOperatorName { get; set; }
        /// <summary>
        /// ログインしているOperatorの権限（AuthorityId）を取得・設定します。
        /// </summary>
        public decimal AuthorityId { get; set; }
        /// <summary>
        /// ログインしている端末のID（NetBIOS名）を取得します。
        /// </summary>
        public string LoginTerminalId { get; private set; }
        /// <summary>
        /// ログインしているユーザがNSKの管理用アカウントかどうかを取得・設定します。
        /// </summary>
        public bool NSKAdminLoginFlag { get; set; }
        /// <summary>
        /// ログインしているユーザーの管理者区分を取得・設定します。
        /// </summary>
        public Int32 AdminKbn { get; set; }
        /// <summary>
        /// ログインしているユーザーの事業所コードを取得・設定します。
        /// </summary>
        public string JigyoshoCode { get; set; }
        /// <summary>
        /// ログインしているユーザーの事業所名を取得・設定します。
        /// </summary>
        public string JigyoshoMei { get; set; }
        /// <summary>
        /// システム設定情報を取得
        /// </summary>
        public SystemSettingsInfo SystemSettingsInfo { get; set; }
        /// <summary>
        /// システム名称リストを取得
        /// </summary>
        public IList<SystemNameInfo> SystemNameList { get; set; }
        /// <summary>
        /// 初期値情報を取得
        /// </summary>
        public DefaultSettingsInfo DefaultSettingsInfo { get; set; }
        /// <summary>
        /// 受注テーブルの数量の整数桁数を取得・設定します。
        /// </summary>
        public Int32 JuchuNumberIntDigits { get; set; }
        /// <summary>
        /// 受注テーブルの数量の少数桁数を取得・設定します。
        /// </summary>
        public Int32 JuchuNumberDecimalDigits { get; set; }
        /// <summary>
        /// 受注テーブルの単価の整数桁数を取得・設定します。
        /// </summary>
        public Int32 JuchuAtPriceIntDigits { get; set; }
        /// <summary>
        /// 受注テーブルの単価の少数桁数を取得・設定します。
        /// </summary>
        public Int32 JuchuAtPriceDecimalDigits { get; set; }

        #endregion
    }
}
