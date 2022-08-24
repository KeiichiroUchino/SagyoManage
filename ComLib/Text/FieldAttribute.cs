using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.Text
{
    /// <summary>
    /// テキスト出力処理時の列タイトルを表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class FieldAttribute : Attribute
    {
        private string _text;
        private int _order;

        public FieldAttribute() : this(null) { }

        public FieldAttribute(string text)
        {
            _text = text;
            _order = int.MaxValue;
        }

        public string Text { get { return this._text; } }
        public bool Disable { get; set; }
        public string Format { get; set; }
        public int Order
        {
            get { return _order; }
            set { _order = value; }
        }
    }
}
