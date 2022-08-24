using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// キーにシーケンシャルなIDフィールドを持つエンティティコンポーネント
    /// の基底となる抽象化クラスです。
    /// </summary>
    [Serializable]
    public abstract class AbstractSequenceKeyModelBase : AbstractModelBase
    {
        /// <summary>
        /// シーケンシャルなＩＤフィールド
        /// </summary>
        protected decimal _baseKeyId;


        /// <summary>
        /// シーケンシャルなＩＤフィールドの値を取得・設定します。
        /// </summary>
        protected virtual decimal BaseKeyId
        {
            get
            {
                return this._baseKeyId;
            }
            set
            {
                this._baseKeyId = value;
            }
        }
    }
}
