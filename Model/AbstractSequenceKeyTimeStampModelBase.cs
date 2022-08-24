using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// タイムスタンプ列を持ちキーにシーケンシャルなIDフィールドを持つエンティティコンポーネント
    /// の基底となる抽象化クラスです。
    /// </summary>
    [Serializable]
    public abstract class AbstractSequenceKeyTimeStampModelBase : AbstractTimeStampModelBase
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

        /// <summary>
        /// バージョンカラムの値が設定されているかでオブジェクトがDBに保存されているかを
        /// 判断するためのショートカットプロパティです。
        /// </summary>
        public new bool IsPersisted
        {
            get
            {
                return this.VersionColumn != null;
            }
        }
    }
}
