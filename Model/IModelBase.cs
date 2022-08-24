using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 常設項目の実装を必要とするエンティティコンポーネントの
    /// インタフェースです。入力者ID～更新日時までのフィールドを
    /// 実装する必要があるエンティティコンポーネントは本インタフェース
    /// を継承してください。
    /// </summary>
    public interface IModelBase
    {
        /// <summary>
        /// 入力者IDを取得・設定するプロパティのインタフェースです
        /// </summary>
        decimal EntryOperatorId { get; set; }

        /// <summary>
        /// 入力処理IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string EntryProcessId { get; set; }
        /// <summary>
        /// 入力端末IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string EntryTerminalId { get; set; }
        /// <summary>
        /// 入力日時を取得・設定するプロパティのインタフェースです。
        /// </summary>
        DateTime? EntryDateTime { get; set; }
        /// <summary>
        /// 登録者IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        decimal AddOperatorId { get; set; }
        /// <summary>
        /// 登録処理IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string AddProcessId { get; set; }
        /// <summary>
        /// 登録端末IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string AddTerminalId { get; set; }
        /// <summary>
        /// 登録日時を取得・設定するプロパティのインタフェースです。
        /// </summary>
        DateTime? AddDateTime { get; set; }
        /// <summary>
        /// 更新者IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        decimal UpdateOperatorId { get; set; }
        /// <summary>
        /// 更新処理IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string UpdateProcessId { get; set; }
        /// <summary>
        /// 更新端末IDを取得・設定するプロパティのインタフェースです。
        /// </summary>
        string UpdateTerminalId { get; set; }
        /// <summary>
        /// 更新日時を取得・設定するプロパティのインタフェースです。
        /// </summary>
        DateTime? UpdateDateTime { get; set; }
    }
}
