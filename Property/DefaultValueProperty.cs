using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jpsys.SagyoManage.BizProperty;

namespace Jpsys.SagyoManage.Property
{
    /// <summary>
    /// システムで利用するデフォルト値を保持します。
    /// 管理マスタに定義された情報も含みます。
    /// </summary>
    public class DefaultValueProperty
    {
        #region テーブル列内項目既定値関連（DB設計書の項目説明にのみ値が定義されているもの）

        /// <summary>
        /// インスタンスの保持用
        /// </summary>
        private static DefaultValueProperty myInstance;

        /// <summary>
        /// プライベートコンストラクタです。SingleTongにする為に
        /// プライベートで宣言します。
        /// </summary>
        private DefaultValueProperty()
        {
        }

        /// <summary>
        /// 本クラスのインスタンスを作成します。既にインスタンスが作成されて
        /// 居る場合は、新しく作成しません。
        /// </summary>
        private static void CreateInstance()
        {
            if (DefaultValueProperty.myInstance == null)
            {
                DefaultValueProperty.myInstance = new DefaultValueProperty();
                DefaultValueProperty.myInstance.SetProperty();
            }
        }

        /// <summary>
        /// 各プロパティに必要な値を初期値として設定します。
        /// </summary>
        private void SetProperty()
        {
            //システム名称マスタを取得
            //ビジネスロジックのインスタンスを作る
            SQLServerDAL.SystemName sg_bll =
                new SQLServerDAL.SystemName();

            //システム名称マスタをセットするリストのインスタンスを作ってプロパティ
            //にセット
            this.SystemGlobalNameList =
                new SortedDictionary<int, List<Model.SystemNameInfo>>();

            //システム名称マスタの各区分ごとの値を格納する領域を初期化
            //DBに値がない場合でも、区分の領域は作っておくため
            foreach (int val in Enum.GetValues(typeof(DefaultProperty.SystemNameKbn)))
            {
                this.SystemGlobalNameList[val] = new List<Model.SystemNameInfo>();
            }
            //一覧取得
            foreach (Model.SystemNameInfo item in sg_bll.GetList())
            {
                //名称IDをキーにして、キーの存在チェック
                //実際は、定義済みのキーは作成してるのでチェックは不要だが
                //未定義の区分が存在している場合でも、処理が続行できるようにする。
                if (!this.SystemGlobalNameList.ContainsKey(item.SystemNameKbn))
                {
                    //キーが存在していなければ、新規に格納用のリストを作る
                    this.SystemGlobalNameList[item.SystemNameKbn] =
                        new List<Model.SystemNameInfo>();
                }
                //プロパティに設定
                this.SystemGlobalNameList[item.SystemNameKbn].Add(item);
            }
        }

        /// <summary>
        /// DefaultValuePropertyクラスの単一インスタンスを取得します。
        /// </summary>
        /// <returns>本クラスの単一インスタンス</returns>
        public static DefaultValueProperty GetInstance()
        {
            DefaultValueProperty.CreateInstance();

            return DefaultValueProperty.myInstance;
        }

        /// <summary>
        /// 本クラス内の静的プロパティを最新の値にセットしなおします。
        /// </summary>
        public void Refresh()
        {
            if (DefaultValueProperty.myInstance == null)
            {
                DefaultValueProperty.myInstance = new DefaultValueProperty();
            }
            this.SetProperty();

        }
        #endregion

        #region 自動実装プロパティ

        //--システム名称マスタより
        /// <summary>
        /// システム名称マスタの名称区分をキーとした、区分に該当するシステム名称情報の一覧
        /// をペアとするSortedDictionaryのインスタンスを取得します。
        /// </summary>
        public SortedDictionary<int, List<Model.SystemNameInfo>>
                                        SystemGlobalNameList { get; private set; }
        #endregion
    }
}
