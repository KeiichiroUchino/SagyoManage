using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// ControlValidatingEventRaiserオブジェクトのコレクションです。
    /// </summary>
    public class ControlValidatingEventRaiserCollection : Collection<ControlValidatingEventRaiser>
    {
        /// <summary>
        /// 所有しているControlValidatingEventRaiserオブジェクトの前回値を更新します。
        /// </summary>
        public void UpdateOldValue()
        {
            foreach (var item in this.Items)
            {
                item.UpdateOldValue();
            }
        }

        /// <summary>
        /// 指定したコントロールを含むEventRaiserオブジェクトのPerformWhenEnterメソッドを呼び出します。
        /// （Form.Validatingなどで）編集操作以外から検証処理を呼び出した為に、Enterイベントが発生しない場合に呼び出してください。
        /// </summary>
        /// <param name="control"></param>
        public void PerformWhenEnter(Control control)
        {
            foreach (ControlValidatingEventRaiser item in this.Items.Where(element => element.Control == control))
            {
                item.PerformWhenEnter();       
            }
        }
    }
}
