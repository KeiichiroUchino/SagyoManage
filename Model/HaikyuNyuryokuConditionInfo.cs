using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配給入力（抽出条件）のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaikyuNyuryokuConditionInfo
    {
        /// <summary>
        /// 日付（範囲開始）を取得・設定します。
        /// </summary>
        public DateTime HizukeFrom { get; set; }
        /// <summary>
        /// 日付（範囲終了）を取得・設定します。
        /// </summary>
        public DateTime HizukeTo { get; set; }
        /// <summary>
        /// グループ指定選択フラグを取得・設定します。
        /// </summary>
        public Boolean NojoShiteiGroupChecked { get; set; }
        /// <summary>
        /// 個別指定選択フラグを取得・設定します。
        /// </summary>
        public Boolean NojoShiteiKobetsuChecked { get; set; }
        /// <summary>
        /// 農場タンクグループIdを取得・設定します。
        /// </summary>
        public Decimal NojoTankGroupId { get; set; }

        /// <summary>
        /// 配給入力（抽出条件）農場一覧情報
        /// </summary>
        public IList<HaikyuNyuryokuConditionNojoInfo> HaikyuNyuryokuConditionNojoList { get; set; }

        #region 関連項目

        /// <summary>
        /// 農場タンクグループコードを取得・設定します。
        /// </summary>
        public Int32 NojoTankGroupCode { get; set; }
        /// <summary>
        /// 農場タンクグループ名を取得・設定します。
        /// </summary>
        public String NojoTankGroupName { get; set; }
        /// <summary>
        /// チェックされた農場Idをカンマ区切りで取得します。
        /// </summary>
        public String NojoCheckList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (HaikyuNyuryokuConditionNojoInfo info in this.HaikyuNyuryokuConditionNojoList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.NojoId.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        #endregion
    }

    /// <summary>
    /// 配給入力（抽出条件）農場情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaikyuNyuryokuConditionNojoInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 NojoCode { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String NojoName { get; set; }
        /// <summary>
        /// カナ名称を取得・設定します。
        /// </summary>
        public String NojoNameKana { get; set; }
        /// <summary>
        /// 地区を取得・設定します。
        /// </summary>
        public String HomenName { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// 農場IDを取得・設定します。
        /// </summary>
        public Decimal NojoId { get; set; }

        #endregion
    }
}
