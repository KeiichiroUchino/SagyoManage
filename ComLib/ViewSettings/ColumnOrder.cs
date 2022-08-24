using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib.ViewSettings
{
    /// <summary>
    /// 列の並び順を表します。
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnOrder : System.Attribute
    {
        private int _index;

        public ColumnOrder(int index)
        {
            this.Index = index;
        }

        public int Index
        {
            get { return _index; }
            set 
            {
                if (value < 0)
                    throw new ArgumentException("負数は設定できません。", "Index");

                _index = value; 
            }
        }
    }
}
