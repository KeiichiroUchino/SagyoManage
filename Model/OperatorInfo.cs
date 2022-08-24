using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualBasic;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// オペレータマスタのエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class OperatorInfo : AbstractSequenceKeyTimeStampModelBase
    {
        /// <summary>
        /// オペレータIDを取得・設定します。
        /// </summary>
        public Decimal OperatorId
        {
            get { return this._baseKeyId; }
            set { this._baseKeyId = value; }
        }
        /// <summary>
        /// オペレータコードを取得・設定します。
        /// </summary>
        public String OperatorCode { get; set; }
        /// <summary>
        /// オペレータ名称を取得・設定します。
        /// </summary>
        public String OperatorName { get; set; }
        /// <summary>
        /// パスワードを取得・設定します。
        /// </summary>
        public String Password { get; set; }
        /// <summary>
        /// トラDON社員IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONStaffId { get; set; }
        /// <summary>
        /// 権限IDを取得・設定します。
        /// </summary>
        public Decimal AuthorityId { get; set; }
        /// <summary>
        /// 権限名を取得・設定します。
        /// </summary>
        public String KengenName { get; set; }
        /// <summary>
        /// 管理者区分を取得・設定します。
        /// </summary>
        public Int32 AdminKbn { get; set; }
        /// <summary>
        /// ログイン日時を取得・設定します。
        /// </summary>
        public DateTime LoginYMD { get; set; }
        /// <summary>
        /// 管理者であるかを取得します。
        /// </summary>
        public Boolean IsAdmin
        {
            get {
                if (AdminKbn == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
       
        /// <summary>
        /// 非表示フラグを取得・設定します。
        /// </summary>
        public Boolean DisableFlag { get; set; }
        /// <summary>
        /// 削除フラグを取得・設定します。
        /// </summary>
        public Boolean DelFlag { get; set; }

        public string ShiyoTeishi
        {
            get { return this.DisableFlag ? "停止中" : string.Empty; }
        }

        #region 関連項目

        /// <summary>
        /// トラDON社員コードを取得・設定します。
        /// </summary>
        public Decimal ToraDONStaffCode { get; set; }
        /// <summary>
        /// トラDON社員名を取得・設定します。
        /// </summary>
        public String ToraDONStaffName { get; set; }
        /// <summary>
        /// トラDON営業書IDを取得・設定します。
        /// </summary>
        public Decimal ToraDONBranchOfficeId { get; set; }
        /// <summary>
        /// トラDON営業所コードを取得・設定します。
        /// </summary>
        public Decimal ToraDONBranchOfficeCode { get; set; }
        /// <summary>
        /// トラDON営業所名を取得・設定します。
        /// </summary>
        public String ToraDONBranchOfficeName { get; set; }
        /// <summary>
        /// 配車最終作業日時を取得・設定します。
        /// </summary>
        public DateTime LastHaishaEntryDateTime { get; set; }

        #endregion
    }

    /// <summary>
    /// オペレータ検索条件情報のエンティティコンポーネントを作成します。
    /// </summary>
    [Serializable]
    public class OperatorSearchParameter
    {
        /// <summary>
        /// Idを取得・設定します。
        /// </summary>
        public decimal? OperatoId { get; set; }
        /// <summary>
        /// オペレータコードを取得・設定します。
        /// </summary>
        public String OperatorCode { get; set; }
    }
}
