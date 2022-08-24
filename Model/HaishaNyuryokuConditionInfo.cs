using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jpsys.HaishaManageV10.BizProperty;
using Microsoft.VisualBasic;

namespace Jpsys.HaishaManageV10.Model
{
    /// <summary>
    /// 配車入力（抽出条件）のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaNyuryokuConditionInfo
    {
        /// <summary>
        /// 配車入力条件区分を取得・設定します。
        /// </summary>
        public Int32 HaishaNyuryokuJokenKbn { get; set; }
        /// <summary>
        /// 営業所管理区分を取得・設定します。
        /// </summary>
        public Int32 EigyoshoKanriKbn { get; set; }

        /// <summary>
        /// 営業所IDを取得・設定します。
        /// </summary>
        public Decimal? BranchOfficeId { get; set; }
        /// <summary>
        /// 営業所コードを取得・設定します。
        /// </summary>
        public Int32? BranchOfficeCode { get; set; }
        /// <summary>
        /// 営業所名を取得・設定します。
        /// </summary>
        public String BranchOfficeName { get; set; }
        /// <summary>
        /// グループ指定選択フラグを取得・設定します。
        /// </summary>
        public Boolean ShiteiGroupChecked { get; set; }
        /// <summary>
        /// 個別指定選択フラグを取得・設定します。
        /// </summary>
        public Boolean ShiteiKobetsuChecked { get; set; }
        /// <summary>
        /// グループIdを取得・設定します。
        /// </summary>
        public Decimal GroupId { get; set; }
        /// <summary>
        /// グループコードを取得・設定します。
        /// </summary>
        public Int32 GroupCode { get; set; }
        /// <summary>
        /// グループ名を取得・設定します。
        /// </summary>
        public String GroupName { get; set; }
        /// <summary>
        /// チェックされたIdをカンマ区切りで取得します。
        /// </summary>
        public String CheckIdList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (HaishaNyuryokuConditionListInfo info in this.HaishaNyuryokuConditionList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.Id.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// チェックされたコードをカンマ区切りで取得します。
        /// </summary>
        public String CheckCodeList
        {
            get
            {
                StringBuilder sb = new StringBuilder();
                bool f = false;
                foreach (HaishaNyuryokuConditionListInfo info in this.HaishaNyuryokuConditionList)
                {
                    if (info.CheckedFlag)
                    {
                        if (f) sb.AppendLine(",");
                        sb.AppendLine(info.Code.ToString());
                        f = true;
                    }
                }
                return sb.ToString();
            }
        }

        /// <summary>
        /// 排他用抽出条件一覧情報
        /// </summary>
        public IList<HaishaNyuryokuConditionListInfo> HaishaNyuryokuConditionList { get; set; }

        /// <summary>
        /// 得意先Id明細情報を取得・設定します。
        /// </summary>
        public IList<decimal> TokuisakiIdMeisaiList { get; set; }

        /// <summary>
        /// 発着地Id明細情報を取得・設定します。
        /// </summary>
        public IList<decimal> PointIdMeisaiList { get; set; }

        /// <summary>
        /// 車種Id明細情報を取得・設定します。
        /// </summary>
        public IList<decimal> CarKindIdMeisaiList { get; set; }

    }

    /// <summary>
    /// 登録排他情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaitaResultInfo
    {
        /// <summary>
        /// 配車入力条件区分を取得・設定します。
        /// </summary>
        public Int32 kbn { get; set; }
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 Code { get; set; }
        /// <summary>
        /// 営業所略称を取得・設定します。
        /// </summary>
        public String BranchOfficeShortName { get; set; }
        /// <summary>
        /// 利用者コードを取得・設定します。
        /// </summary>
        public String OperatorCode { get; set; }
        /// <summary>
        /// 利用者名称を取得・設定します。
        /// </summary>
        public String OperatorName { get; set; }
        /// <summary>
        /// 排他明細を取得・設定します。
        /// </summary>
        public String HaitaResultMeisai
        {
            get
            {
                string rt_str = string.Empty;

                if (!String.IsNullOrWhiteSpace(this.BranchOfficeShortName))
                {
                    rt_str = "営業所：" + this.BranchOfficeShortName;
                }

                switch (kbn)
                {
                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.TokuisakiGroup:
                        // 得意先の場合
                        rt_str +=
                            (!String.IsNullOrWhiteSpace(rt_str) ? "　" : string.Empty)
                            + "得意先コード：" + this.Code.ToString();

                        break;

                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.HomenGroup:
                        // 方面の場合
                        rt_str +=
                            (!String.IsNullOrWhiteSpace(rt_str) ? "　" : string.Empty)
                            + "方面コード：" + this.Code.ToString();

                        break;

                    case (int)DefaultProperty.HaishaNyuryokuJokenKbn.CarKindGroup:
                        // 車種の場合
                        rt_str +=
                            (!String.IsNullOrWhiteSpace(rt_str) ? "　" : string.Empty)
                            + "車種コード：" + this.Code.ToString();

                        break;

                    default:
                        break;
                }

                rt_str +=
                    (!String.IsNullOrWhiteSpace(rt_str) ? "　" : string.Empty)
                    + "利用者：" + this.OperatorName;

                return rt_str;
            }
        }
    }

    /// <summary>
    /// 配車入力（抽出条件）一覧情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class HaishaNyuryokuConditionListInfo
    {
        /// <summary>
        /// コードを取得・設定します。
        /// </summary>
        public Int32 Code { get; set; }
        /// <summary>
        /// 名称を取得・設定します。
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 選択フラグを取得・設定します。
        /// </summary>
        public Boolean CheckedFlag { get; set; }

        #region 非表示項目

        /// <summary>
        /// IDを取得・設定します。
        /// </summary>
        public Decimal Id { get; set; }

        #endregion
    }
}
