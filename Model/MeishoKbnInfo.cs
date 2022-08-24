using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.Model
{
    public class MeishoKbnInfo
    {
        public MeishoKbnInfo() : this(0, string.Empty) { }

        public MeishoKbnInfo(int kbn, string kbnMei)
        {
            this.MeishoKbn = kbn;
            this.MeishoKbnMei = kbnMei;
        }

        public int MeishoKbn { get; set; }
        public string MeishoKbnMei { get; set; }
    }
}
