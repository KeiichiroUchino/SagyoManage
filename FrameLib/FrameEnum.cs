using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 編集可能画面の現在の画面状態を表す列挙体です。
    /// </summary>
    public enum FrameEditMode : int
    {
        /// <summary>
        /// デフォルト（初期状態）をあらわします。
        /// </summary>
        Default,
        /// <summary>
        /// 新規入力可能状態を表します。
        /// </summary>
        New,
        /// <summary>
        /// 既存データ読み込み後の編集可能状態を表します。
        /// </summary>
        Editable,
        /// <summary>
        /// 既存データ読み込み後に参照のみが可能の状態を表します。
        /// </summary>
        ViewOnly,
    }

    /// <summary>
    /// 編集可能画面の現在の画面状態を表す列挙体です。
    /// </summary>
    public enum FrameEditModeEx : int
    {
        /// <summary>
        /// デフォルト（初期状態）をあらわします。
        /// </summary>
        Default,
        /// <summary>
        /// 新規入力可能状態を表します。
        /// </summary>
        New,
        /// <summary>
        /// 新規入力可能状態を表します。
        /// </summary>
        New2,
        /// <summary>
        /// 既存データ読み込み後の編集可能状態を表します。
        /// </summary>
        Editable,
        /// <summary>
        /// 既存データ読み込み後の編集可能状態を表します。
        /// </summary>
        Editable2,
        /// <summary>
        /// 既存データ読み込み後の編集可能状態を表します。
        /// </summary>
        Editable3,
    }

    /// <summary>
    /// 帳票印刷指示での印刷先を表す列挙体です。
    /// </summary>
    public enum ReportPrintDestType : int
    {
        /// <summary>
        /// （直接）印刷
        /// </summary>
        PrintToPrinter,
        /// <summary>
        /// プレビュー
        /// </summary>
        PrintToScreen,
        /// <summary>
        /// FAX
        /// </summary>
        PrintToFax,
        /// <summary>
        /// エクセル出力
        /// </summary>
        ExportExcel,
    }

    /// <summary>
    /// 一致方法を選択可能な文字列一致判定メソッドにおいて、その
    /// 一致とみなす方法を表す列挙体です。
    /// </summary>
    public enum SpecialStringContaintType : int
    {
        /// <summary>
        /// 前方一致
        /// </summary>
        StartsWith,
        /// <summary>
        /// 包含一致（いわゆるあいまい検索）
        /// </summary>
        Contains,
        /// <summary>
        /// 後方一致
        /// </summary>
        EndsWith

    }

    /// <summary>
    /// データの指定モード
    /// </summary>
    public enum DataSpecifyMode : int
    {
        /// <summary>
        /// すべて
        /// </summary>
        All,
        /// <summary>
        /// 範囲
        /// </summary>
        Range,
        /// <summary>
        /// 個別指定
        /// </summary>
        Separated,
    }

    /// <summary>
    /// 画面の表示モードを表す列挙体です。
    /// </summary>
    public enum FrameShowMode : int
    {
        /// <summary>
        /// 一覧表示
        /// </summary>
        ListView,
        /// <summary>
        /// 検索
        /// </summary>
        Search,
        /// <summary>
        /// サブ検索
        /// </summary>
        SubSearch
    }

    /// <summary>
    /// 処理モード
    /// </summary>
    public enum ProcessMode : int
    {
        /// <summary>
        /// 新規
        /// </summary>
        Shinki = 0,
        /// <summary>
        /// 修正
        /// </summary>
        Syuusei = 1,

    }
}
