using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// 初期値情報のエンティティコンポーネントです。
    /// </summary>
    [Serializable]
    public class DefaultSettingsInfo
    {
        /// <summary>
        /// 初期値IDを取得・設定します。
        /// </summary>
        public Decimal DefaultSettingsId { get; set; }
        /// <summary>
        /// 配車入力の営業所初期値区分を取得・設定します。
        /// </summary>
        public Int32 HN_EigyosyoShokichiKbn { get; set; }
        /// <summary>
        /// 配車入力の営業所車両初期値区分を取得・設定します。
        /// </summary>
        public Int32 HN_EigyosyoCarShokichiKbn { get; set; }
    }
}
