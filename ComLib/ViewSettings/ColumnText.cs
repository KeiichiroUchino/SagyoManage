using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.ViewSettings
{
    /// <summary>
    /// 列のテキストを表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnText : System.Attribute
    {
        private string _text;

        public ColumnText(string text)
        {
            _text = text;
        }

        public string Text { get { return this._text; } }
    }
}
