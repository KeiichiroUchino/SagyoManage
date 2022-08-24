using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.FrameLib
{
    /// <summary>
    /// 本システムで使用するフォームの必要なインタフェースを定義します。
    /// </summary>
    public interface IFrameBase
    {
        /// <summary>
        /// フォームを初期化します。主にフォームのインスタンス化後の
        /// ShowまたはShowDialogメソッドの実行前に初期化する必要がある
        /// 処理を行います。
        /// </summary>
        void InitFrame();

        /// <summary>
        /// フォーム名（Form.Nameと等価の値）を取得・設定します。
        /// </summary>
        string FrameName { get; set; }

        /// <summary>
        /// フォーム表示文字列（Form.Textと等価の値）を取得・設定します。
        /// </summary>
        string FrameText { get; set; }

    }
}
