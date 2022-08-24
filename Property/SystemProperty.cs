using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using jp.co.jpsys.util;
using System.Configuration;

namespace Jpsys.SagyoManage.Property
{
    /// <summary>
    /// システムの動作に必要な情報を保持します。データベースの管理マスタ
    /// テーブルからの情報と、プログラムコード内に保持する定数値で構成
    /// されます。
    /// </summary>
    public class SystemProperty
    {
        /// <summary>
        /// インスタンスの保持用
        /// </summary>
        private static SystemProperty myInstance;

        /// <summary>
        /// プライベートコンストラクタです。SingleTongにする為に
        /// プライベートで宣言します。
        /// </summary>
        private SystemProperty()
        {
        }

        /// <summary>
        /// 本クラスのインスタンスを作成します。既にインスタンスが作成されて
        /// 居る場合は、新しく作成しません。
        /// </summary>
        private static void CreateInstance()
        {
            if (SystemProperty.myInstance == null)
            {
                SystemProperty.myInstance = new SystemProperty();
                SystemProperty.myInstance.SetProperty();
            }
        }

        /// <summary>
        /// 各プロパティに必要な値を初期値として設定します。
        /// </summary>
        private void SetProperty()
        {
            //NSKFrameworkより取得（起動シーケンスで設定されていることが前提）
            this.ProductName = NSKUtilSetting.ProductName;

            //デバッグログは、非ローミングユーザのApplication Dataフォルダに
            //落とす。
            //その直下に、jpsys.co.jp\DEBUG_ + NSKUtilSetting.SeihinId+ "_" 
            // + NSKUtilSetting.EditionName で生成
            //された名前を使用してフォルダを作成して、その配下にデバッグログを
            //作成すること。NSKUtilSetting.SeihinIdはFramework.Boot.Bootinitiarizerで
            //設定している
            this.DebugLoggingPath =
                Path.Combine(
                    System.Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                        @"jpsys.co.jp\DEBUG_" + NSKUtilSetting.SeihinId + "_" +
                        NSKUtilSetting.EditionName);
            //ディレクトリの存在チェック
            if (!Directory.Exists(this.DebugLoggingPath))
            {
                //存在しないときは作る
                Directory.CreateDirectory(this.DebugLoggingPath);
            }


            //データファイルは、非ローミングユーザのApplication Dataフォルダに
            //落とす。
            //その直下に、jpsys.co.jp\[SeihinId]_[EditionName]　というフォルダを作成して、その配下に
            //データを作成すること。
            //プリンタ設定を保持するフォルダ作成
            this.ReportPrinterSettingPath =
                Path.Combine(
                    System.Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData),
                    @"jpsys.co.jp\" + NSKUtilSetting.SeihinId + "_"
                    + NSKUtilSetting.EditionName);
            //ディレクトリの存在チェック
            if (!Directory.Exists(this.ReportPrinterSettingPath))
            {
                //存在しないときは作る
                Directory.CreateDirectory(this.ReportPrinterSettingPath);
            }

            //ユーザ別のレジストリパスの作成
            this.SystemDefaultRegistryPath =
                @"Software\jpsys.co.jp\"
                    + NSKUtilSetting.SeihinId + @"\"
                    + NSKUtilSetting.EditionName + @"\";


            //抽出対象指定最大数を取得する
            //--デフォルトで7000にしておく
            int choose_max_count = 7000;
            //--アプリケーション構成ファイルから値を取得してみる
            if (ConfigurationManager.AppSettings["ChooseMultiSelectableMaxCount"] != null)
            {
                choose_max_count =
                    Convert.ToInt32(
                        ConfigurationManager.AppSettings["ChooseMultiSelectableMaxCount"]);
            }
            this.ChooseMultiSelectableMaxCount = choose_max_count;
        }

        /// <summary>
        /// SystemPropertyクラスの単一インスタンスを取得します。
        /// </summary>
        /// <returns>本クラスの単一インスタンス</returns>
        public static SystemProperty GetInstance()
        {
            SystemProperty.CreateInstance();

            return SystemProperty.myInstance;
        }

        /// <summary>
        /// 本クラス内の静的プロパティを最新の値にセットしなおします。
        /// </summary>
        public void Refresh()
        {
            if (SystemProperty.myInstance == null)
            {
                SystemProperty.myInstance = new SystemProperty();
            }
            this.SetProperty();

        }

        #region 自動実装プロパティ

        /// <summary>
        /// 本システムの製品名称をNSKFrameworkより取得します。
        /// </summary>
        public string ProductName { get; private set; }


        //--プログラム内定数値

        /// <summary>
        /// 本システムで使用するデバッグログを保存するパスを取得します。
        /// </summary>
        public string DebugLoggingPath { get; private set; }

        /// <summary>
        /// 本システムで使用する帳票のプリンタ及び用紙の設定情報を
        /// 保存するパスを取得します。
        /// </summary>
        public string ReportPrinterSettingPath { get; private set; }

        /// <summary>
        /// 本システムで使用するレジストリキーのパス文字列を
        /// 取得します。キーの最上位ノードは返しません。値の
        /// 参照有効範囲により、HKEY_CURRENT_USER もしくは、
        /// HKEY_LOCAL_MACHINE 以下で使用してください。
        /// </summary>
        public string SystemDefaultRegistryPath { get; private set; }

        /// <summary>
        /// 共通検索より抽出対象などを複数選択可能な場合に、その選択可能
        /// な数の最大値を取得します。共通検索から複数指定した場合の最大
        /// 値ではなく、選択後に呼び出し元の画面にて保持可能な最大件数
        /// のことです。
        /// </summary>
        public int ChooseMultiSelectableMaxCount { get; private set; }

        #endregion
    }
}
