using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.BizProperty
{
    /// <summary>
    /// システムで利用する固定値を保持します。データベース接続を必要としない
    /// システム固有の値を保持します。
    /// </summary>
    public class DefaultProperty
    {
        #region システム名称関連

        /// <summary> 
        /// システム名称マスタの名称区分を表す列挙体です。
        /// </summary> 
        /// <remarks></remarks> 
        public enum SystemNameKbn : int
        {
            /// <summary>
            /// 0:指定なしを表します。
            /// </summary>
            None = 0,
            /// <summary>
            /// 1:日付指定区分
            /// </summary>
            FilterDateKbns = 1,
            /// <summary>
            /// 2:契約状況区分
            /// </summary>
            KeiyakuJyokyoKbns = 2,
            /// <summary>
            /// 3:作業日区分
            /// </summary>
            SagyobiKbn = 3,
        }

        /// <summary>
        /// 1:日付指定区分を表す列挙体です。
        /// </summary>
        public enum FilterDateKbns : int
        {
            /// <summary>
            /// 0:指定なしを表します。
            /// </summary>
            None = 0,
            /// <summary>
            /// 開始日
            /// </summary>
            StartDate = 1,
            /// <summary>
            /// 終了日
            /// </summary>
            EndDate = 2,
        }

        /// <summary>
        /// 2:契約状況区分を表す列挙体です。
        /// </summary>
        public enum KeiyakuJyokyoKbns : int
        {
            /// <summary>
            /// 0:指定なしを表します。
            /// </summary>
            None = 0,
            /// <summary>
            /// 1:契約中
            /// </summary>
            Keiyakuch = 100,
            /// <summary>
            /// 2:契約終了
            /// </summary>
            KeiyakuShuryo = 900,
        }

        /// <summary>
        /// 3:作業日区分を表す列挙体です。
        /// </summary>
        public enum SagyobiKbns : int
        {
            /// <summary>
            /// 0:指定なしを表します。
            /// </summary>
            None = 0,
            /// <summary>
            /// 1:毎週
            /// </summary>
            Weekly = 1,
            /// <summary>
            /// 2:毎月
            /// </summary>
            Monthly = 2,
            /// <summary>
            /// 3:指定日
            /// </summary>
            Day = 3,
        }

        /// <summary> 
        /// 名称の名称区分を表す列挙体です。 
        /// </summary> 
        /// <remarks></remarks> 
        public enum MeishoKbn : int
        {
            /// <summary>
            /// 0:指定なしを表します。
            /// </summary>
            None = 0,
            /// <summary>
            /// 10:社員
            /// </summary>
            Staff = 10,
        }


 

        #endregion

        #region システム名称関連以外の固定値

        /// <summary>
        /// マスターのコード種類を表す列挙体です。
        /// </summary>
        public enum MasterCodeKbn : int
        {
            /// <summary>
            /// 0:未設定又はすべて
            /// </summary>
            None = 0,
            /// <summary>
            /// 1:品目大分類
            /// </summary>
            ItemLBunrui = 1,
            /// <summary>
            /// 2:品目中分類
            /// </summary>
            ItemMBunrui = 2,
            /// <summary>
            /// 3:得意先グループ
            /// </summary> 
            TokuisakiGroup = 3,
            /// <summary>
            /// 4:発着地大分類
            /// </summary>
            PointLBunrui = 4,
            /// <summary>
            /// 5:発着地中分類
            /// </summary>
            PointMBunrui = 5,
            /// <summary>
            /// 6:発着地グループ
            /// </summary> 
            PointGroup = 6,
            /// <summary>
            /// 7:方面
            /// </summary> 
            Homen = 7,
            /// <summary>
            /// 8:販路
            /// </summary> 
            Hanro = 8,
        }

        /// <summary>
        /// 名称区分を表す列挙体を指定して、名称区分の区分(種類)名を取得します。
        /// </summary>
        /// <param name="kbn">名称区分を表す列挙体</param>
        /// <returns>名称区分名</returns>
        public static string GetMeishoKbnString(DefaultProperty.MeishoKbn kbn)
        {
            string rt_val = string.Empty;

            switch (kbn)
            {
                case DefaultProperty.MeishoKbn.None:
                    rt_val = "";
                    break;
                case DefaultProperty.MeishoKbn.Staff:
                    rt_val = "社員区分";
                    break;
                default:
                    break;
            }

            return rt_val;

        }


        /// <summary>
        /// SAP Crystal Reportsの利用可能バージョン
        /// </summary>
        public const String SAP_VERSION = "13.0.29.";

        #endregion

        #region テーブル列内項目既定値関連（DB設計書の項目説明にのみ値が定義されているもの）

        /// <summary>
        /// インスタンスの保持用
        /// </summary>
        private static DefaultProperty myInstance;

        /// <summary>
        /// プライベートコンストラクタです。SingleTongにする為に
        /// プライベートで宣言します。
        /// </summary>
        private DefaultProperty()
        {
        }

        /// <summary>
        /// 本クラスのインスタンスを作成します。既にインスタンスが作成されて
        /// 居る場合は、新しく作成しません。
        /// </summary>
        private static void CreateInstance()
        {
            if (DefaultProperty.myInstance == null)
            {
                DefaultProperty.myInstance = new DefaultProperty();
                DefaultProperty.myInstance.SetProperty();
            }
        }

        /// <summary>
        /// 各プロパティに必要な値を初期値として設定します。
        /// </summary>
        private void SetProperty()
        {
        }

        /// <summary>
        /// DefaultPropertyクラスの単一インスタンスを取得します。
        /// </summary>
        /// <returns>本クラスの単一インスタンス</returns>
        public static DefaultProperty GetInstance()
        {
            DefaultProperty.CreateInstance();

            return DefaultProperty.myInstance;
        }

        /// <summary>
        /// 本クラス内の静的プロパティを最新の値にセットしなおします。
        /// </summary>
        public void Refresh()
        {
            if (DefaultProperty.myInstance == null)
            {
                DefaultProperty.myInstance = new DefaultProperty();
            }
            this.SetProperty();

        }
        #endregion

        #region メニュー使用不可情報

        /// <summary>
        /// 権限のMAX
        /// </summary>
        public const int AUTH_MAX = 9;

        /// <summary>
        /// 権限1
        /// </summary>
        public const string AUTH_1 = "権限１";
        /// <summary>
        /// 権限2
        /// </summary>
        public const string AUTH_2 = "権限２";
        /// <summary>
        /// 権限3
        /// </summary>
        public const string AUTH_3 = "権限３";
        /// <summary>
        /// 権限4
        /// </summary>
        public const string AUTH_4 = "権限４";
        /// <summary>
        /// 権限5
        /// </summary>
        public const string AUTH_5 = "権限５";
        /// <summary>
        /// 権限6
        /// </summary>
        public const string AUTH_6 = "権限６";
        /// <summary>
        /// 権限7
        /// </summary>
        public const string AUTH_7 = "権限７";
        /// <summary>
        /// 権限8
        /// </summary>
        public const string AUTH_8 = "権限８";
        /// <summary>
        /// 権限9
        /// </summary>
        public const string AUTH_9 = "権限９";

        #endregion

        #region 自動実装プロパティ

        #endregion

        #region 配色関連

        /// <summary>
        /// タイトル背景色初期値
        /// </summary>
        public const String FRAME_TITLE_DEFAULT_BACKCOLOR = "186, 226, 188";
        /// <summary>
        /// フッター背景色初期値
        /// </summary>
        public const String FRAME_FOOTER_DEFAULT_BACKCOLOR = "DarkCyan";
        /// <summary>
        /// 表ヘッダー背景色初期値
        /// </summary>
        public const String FRAME_GRIDHEADER_DEFAULT_BACKCOLOR = "186, 226, 188";
        /// <summary>
        /// 表選択行背景色初期値
        /// </summary>
        public const String FRAME_GRIDSELECTION_DEFAULT_BACKCOLOR = "255, 255, 192";
        /// <summary>
        /// 交互行背景色初期値
        /// </summary>
        public const String FRAME_GRIDEVEN_DEFAULT_BACKCOLOR = "212, 242, 242";
        /// <summary>
        /// 休日区分休日背景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_1_DEFAULT_BACKCOLOR = "Red";
        /// <summary>
        /// 休日区分午前休背景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_2_DEFAULT_BACKCOLOR = "Blue";
        /// <summary>
        /// 休日区分午後休背景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_3_DEFAULT_BACKCOLOR = "Green";
        /// <summary>
        /// 休日区分午後休背景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_4_DEFAULT_BACKCOLOR = "Yellow";
        /// <summary>
        /// 休日区分午後休背景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_5_DEFAULT_BACKCOLOR = "Pink";
        /// <summary>
        /// 休日区分休日文字色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_1_DEFAULT_FORECOLOR = "0, 0, 0";
        /// <summary>
        /// 休日区分午前文字景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_2_DEFAULT_FORECOLOR = "0, 0, 0";
        /// <summary>
        /// 休日区分午後文字景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_3_DEFAULT_FORECOLOR = "0, 0, 0";
        /// <summary>
        /// 休日区分午後文字景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_4_DEFAULT_FORECOLOR = "0, 0, 0";
        /// <summary>
        /// 休日区分午後文字景色初期値
        /// </summary>
        public const String FRAME_KYUJITSUKBN_5_DEFAULT_FORECOLOR = "0, 0, 0";

        #endregion


        #region 排他制御関連

        /// <summary>
        /// 排他制御区分を表す列挙体です。
        /// </summary>
        public enum ExclusiveControlKbn : int
        {
            /// <summary>
            /// 1:郵便辞書設定（KEN_ALL）
            /// </summary>
            KenAll = 1,
        }

        #endregion

        #region 正規表現文字列

        /// <summary>
        /// メールアドレス正規表現
        /// </summary>
        public static string EMAIL_REGEX = "^[a-zA-Z0-9.!#$%&*+\\/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\\.[a-zA-Z0-9-]+)+$";

        #endregion

        #region 共通定数

        /// <summary>
        /// 月指定月末日
        /// </summary>
        public static decimal SAGYODAY_MONTH_END = 99m;

        #endregion
    }
}
