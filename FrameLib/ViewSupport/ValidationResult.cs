using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Jpsys.SagyoManage.FrameLib.ViewSupport
{
    /// <summary>
    /// 検証結果を表します。
    /// </summary>
    public class ValidationResult
    {
        private ValidationResultTypeId _type;
        private string _message;

        /// <summary>
        /// </summary>
        /// <param name="message"></param>
        /// <param name="type"></param>
        public ValidationResult(ValidationResultTypeId type, string message)
        {
            _type = type;
            _message = message;
        }

        /// <summary>
        /// </summary>
        public ValidationResultTypeId Type { get { return _type; } }
        /// <summary>
        /// </summary>
        public string Message { get { return _message; } }
    }

    /// <summary>
    /// 検索結果タイプIdを表します。
    /// </summary>
    public enum ValidationResultTypeId
    {
        /// <summary>
        /// </summary>
        Warning,
        /// <summary>
        /// </summary>
        Error,
    }

    /// <summary>
    /// 検索結果タイプを表します。
    /// </summary>
    internal class ValidationResultType
    {
        internal ValidationResultType(ValidationResultTypeId typeId, string caption, MessageBoxIcon icon)
        {
            this.TypeId = typeId;
            this.Caption = caption;
            this.Icon = icon;
        }

        internal ValidationResultTypeId TypeId { get; private set; }
        internal string Caption { get; private set; }
        internal MessageBoxIcon Icon { get; private set; }

        internal static readonly List<ValidationResultType> Types =
            new List<ValidationResultType>() 
            { 
                new ValidationResultType(ValidationResultTypeId.Warning, "警告", MessageBoxIcon.Warning),
                new ValidationResultType(ValidationResultTypeId.Error, "エラー", MessageBoxIcon.Error),            
            };
    }
}
