using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jpsys.SagyoManage.ComLib.ViewSettings;

namespace Jpsys.SagyoManage.Model
{
    // <summary>
    /// 排他チェック用のタイムスタンプもちのモデル
    /// </summary>
    [Serializable]
    public abstract class AbstractTimeStampModelBase : AbstractModelBase
    {
        /// <summary>
        /// バージョンカラム
        /// </summary>
        protected byte[] _versionColumn;

        /// <summary>
        /// バージョンカラムを取得・設定します。
        /// </summary>
        [HiddenColumn]
        public virtual byte[] VersionColumn
        {
            get
            {
                return this._versionColumn;
            }
            set
            {
                this._versionColumn = value;
            }
        }

        /// <summary>
        /// バージョンカラムの値が設定されているかでオブジェクトがDBに保存されているかを
        /// 判断するためのショートカットプロパティです。
        /// </summary>
        public bool IsPersisted
        {
            get
            {
                return this.VersionColumn != null;
            }
        }
    }
}
