using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// メニュー項目を表すビジネスエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class MenuItemInfo
    {
        /// <summary>
        /// メニューIDを取得・設定します。
        /// </summary>
        public decimal MenuId { get; set; }
        /// <summary>
        /// 大分類メニューのコードを取得・設定します。
        /// </summary>
        public int TopMenuCode { get; set; }
        /// <summary>
        /// 大分類メニューの表示名称を取得・設定します。
        /// </summary>
        public string TopMenuName { get; set; }
        /// <summary>
        /// 中分類メニューのコードを取得・設定します。
        /// </summary>
        public int MiddleMenuCode { get; set; }
        /// <summary>
        /// 中分類メニューの名称を取得・設定します。
        /// </summary>
        public string MiddleMenuName { get; set; }

        /// <summary>
        /// クラス名を補完し完全なアセンブリ名とするための名前空間名を取得・設定します。
        /// </summary>
        public string PartOfNameSpace { get; set; }

        /// <summary>
        /// 機能を呼び出すためのクラス名を取得・設定します。
        /// </summary>
        public string ClassName { get; set; }

        /// <summary>
        /// メニューを表示した上で有効無効を切り替えるための値を取得・設定します。
        /// </summary>
        public bool EnableFlag { get; set; }

        /// <summary>
        /// メニューから表示されるフォームをDialog表示で行うかどうかの値を取得・設定します。
        /// (0:最大化表示 1:Dialog表示で行う 2:通常表示）
        /// </summary>
        public int ShowingMode { get; set; }

        /// <summary>
        /// ソート重みの値を取得・設定します。
        /// </summary>
        public int SortWeight { get; set; }

        /// <summary>
        /// 表示フラグの値を取得・設定します。
        /// </summary>
        public bool ShowFlag { get; set; }

        /// <summary>
        /// 説明文を取得・設定します。
        /// </summary>
        public string MenuDescription { get; set; }
    }
}
