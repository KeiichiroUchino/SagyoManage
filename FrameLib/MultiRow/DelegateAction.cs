using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrapeCity.Win.MultiRow;

namespace Jpsys.SagyoManage.FrameLib.MultiRow
{
    /// <summary>
    /// 任意のデリゲートを実行するGcMultiRowのアクションを提供します。
    /// </summary>
    public class DelegateAction : GrapeCity.Win.MultiRow.Action
    {
        private Action<GcMultiRow> _actionDelegate;
            
        /// <summary>
        /// 実行するデリゲートを指定してインスタンスを初期化します。
        /// </summary>
        /// <param name="actionDelegate"></param>
        public DelegateAction(Action<GcMultiRow> actionDelegate)
        {
            _actionDelegate = actionDelegate;
        }


        /// <summary>
        /// 実行可否を取得します。
        /// </summary>
        /// <param name="target"></param>
        public override bool CanExecute(GcMultiRow target)
        {
            return true;
        }

        /// <summary>
        /// 実行するデリゲートを取得します。
        /// </summary>
        /// <param name="target"></param>
        protected override void OnExecute(GcMultiRow target)
        {
            _actionDelegate(target);
        }
    }
}
