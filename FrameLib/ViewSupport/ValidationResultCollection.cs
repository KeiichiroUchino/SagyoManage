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
    /// ValidationResultオブジェクトのコレクションです。
    /// </summary>
    public class ValidationResultCollection : Collection<ValidationResult>
    {
        /// <summary>
        /// </summary>
        public bool HasError
        {
            get
            {
                return this.Items.Any(element => element.Type == ValidationResultTypeId.Error);
            }
        }
    }

    /// <summary>
    /// ValidationResultCollectionの拡張メソッドです。
    /// </summary>
    public static class ValidationResultCollectionExtensions
    {
        /// <summary>
        /// </summary>
        /// <param name="form"></param>
        /// <param name="source"></param>
        public static void ShowMessageBoxIfHasItems(this ValidationResultCollection source, Form form)
        {
            if (!source.Any())
	            return;

            ValidationResult firstOfResults = source.FirstOrDefault();
            ValidationResultType type = ValidationResultType.Types.Where(obj => obj.TypeId == firstOfResults.Type).FirstOrDefault();
            
            if (type == null)
                return;		 

            MessageBox.Show(form, firstOfResults.Message, type.Caption, MessageBoxButtons.OK, type.Icon);
        }
    }
}
