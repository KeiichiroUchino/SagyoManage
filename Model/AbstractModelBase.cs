using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jpsys.SagyoManage.Model
{
    /// <summary>
    /// すべてのエンティティコンポーネントの基底となる抽象化クラスです。
    /// IModelBaseの抽象化実装となります。
    /// </summary>
    [Serializable]
    public abstract class AbstractModelBase : IModelBase
    {
        /// <summary>
        /// 入力者ＩＤ
        /// </summary>
        protected decimal _entryOperatorId;
        /// <summary>
        /// 入力処理ＩＤ
        /// </summary>
        protected string _entryProcessId;
        /// <summary>
        /// 入力端末ＩＤ
        /// </summary>
        protected string _entryTerminalId;
        /// <summary>
        /// 入力日時
        /// </summary>
        protected DateTime? _entryDateTime;

        /// <summary>
        /// 登録者ＩＤ
        /// </summary>
        protected decimal _addOperatorId;
        /// <summary>
        /// 登録処理ＩＤ
        /// </summary>
        protected string _addProcessId;
        /// <summary>
        /// 登録端末ＩＤ
        /// </summary>
        protected string _addTerminalId;
        /// <summary>
        /// 登録日時
        /// </summary>
        protected DateTime? _addDateTime;

        /// <summary>
        /// 更新者ＩＤ
        /// </summary>
        protected decimal _updateOperatorId;
        /// <summary>
        /// 更新処理ＩＤ
        /// </summary>
        protected string _updateProcessId;
        /// <summary>
        /// 更新端末ＩＤ
        /// </summary>
        protected string _updateTerminalId;
        /// <summary>
        /// 更新日時
        /// </summary>
        protected DateTime? _updateDateTime;


        /// <summary>
        /// 入力者ＩＤを取得・設定します。
        /// </summary>
        public virtual decimal EntryOperatorId
        {
            get
            {
                return this._entryOperatorId;
            }
            set
            {
                this._entryOperatorId = value;
            }
        }

        /// <summary>
        /// 入力処理ＩＤを取得・設定します。
        /// </summary>
        public virtual string EntryProcessId
        {
            get
            {
                return this._entryProcessId;
            }
            set
            {
                this._entryProcessId = value;
            }
        }

        /// <summary>
        /// 入力端末ＩＤを取得・設定します。
        /// </summary>
        public virtual string EntryTerminalId
        {
            get
            {
                return this._entryTerminalId;
            }
            set
            {
                this._entryTerminalId = value;
            }
        }

        /// <summary>
        /// 入力日時を取得・設定します。
        /// </summary>
        public virtual DateTime? EntryDateTime
        {
            get
            {
                return this._entryDateTime;
            }
            set
            {
                this._entryDateTime = value;
            }
        }

        /// <summary>
        /// 登録者ＩＤを取得・設定します。
        /// </summary>
        public virtual decimal AddOperatorId
        {
            get
            {
                return this._addOperatorId;
            }
            set
            {
                this._addOperatorId = value;
            }
        }

        /// <summary>
        /// 登録処理ＩＤを取得・設定します。
        /// </summary>
        public virtual string AddProcessId
        {
            get
            {
                return this._addProcessId;
            }
            set
            {
                this._addProcessId = value;
            }
        }

        /// <summary>
        /// 登録端末ＩＤを取得・設定します。
        /// </summary>
        public virtual string AddTerminalId
        {
            get
            {
                return this._addTerminalId;
            }
            set
            {
                this._addTerminalId = value;
            }
        }

        /// <summary>
        /// 登録日時を取得・設定します。
        /// </summary>
        public virtual DateTime? AddDateTime
        {
            get
            {
                return this._addDateTime;
            }
            set
            {
                this._addDateTime = value;
            }
        }

        /// <summary>
        /// 更新者ＩＤを取得・設定します。
        /// </summary>
        public virtual decimal UpdateOperatorId
        {
            get
            {
                return this._updateOperatorId;
            }
            set
            {
                this._updateOperatorId = value;
            }
        }

        /// <summary>
        /// 更新処理ＩＤを取得・設定します。
        /// </summary>
        public virtual string UpdateProcessId
        {
            get
            {
                return this._updateProcessId;
            }
            set
            {
                this._updateProcessId = value;
            }
        }

        /// <summary>
        /// 更新端末ＩＤを取得・設定します。
        /// </summary>
        public virtual string UpdateTerminalId
        {
            get
            {
                return this._updateTerminalId;
            }
            set
            {
                this._updateTerminalId = value;
            }
        }

        /// <summary>
        /// 更新日時を取得・設定します。
        /// </summary>
        public virtual DateTime? UpdateDateTime
        {
            get
            {
                return this._updateDateTime;
            }
            set
            {
                this._updateDateTime = value;
            }
        }
    }
}
