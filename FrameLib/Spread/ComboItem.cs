using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// </summary>
    public class ComboItem
    {
        /// <summary>
        /// </summary>
        public ComboItem(string data, string displayText)
        {
            this.Data = data;
            this.DisplayText = displayText;
        }

        /// <summary>
        /// </summary>
        public string Data { get; private set; }
        /// <summary>
        /// </summary>
        public string DisplayText { get; private set; }
    }
}
