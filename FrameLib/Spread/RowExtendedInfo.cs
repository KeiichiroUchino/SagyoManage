using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    //*** この行拡張情報は、SpreadのRowオブジェクトのTagに格納します。
    //*** ユーザー側ではTagに情報を設定しないようにしてください。
    /// <summary>
    /// 行の拡張情報
    /// </summary>
    public class RowExtendedInfo
    {
        /// <summary>
        /// </summary>
        public RowExtendedInfo()
        {
            this.ItemIsEdited = false;
            this.ItemIsDeleted = false;
            this.SourceObject = null;
        }

        /// <summary>
        /// 項目が編集されたかどうか。
        /// 値を一度でも書き換えた場合を編集済みとみなします。
        /// </summary>
        public bool ItemIsEdited { get; set; }
        /// <summary>
        /// 項目が削除されたかどうか
        /// </summary>
        public bool ItemIsDeleted { get; set; }
        /// <summary>
        /// 元オブジェクト
        /// </summary>
        public object SourceObject { get; set; }
    }
}
