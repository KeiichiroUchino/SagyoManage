using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    public class IdVersionPair
    {
        private decimal _id;
        private byte[] _version;
        public IdVersionPair(decimal id, byte[] version)
        {
            _id = id;
            _version = version;
        }

        public decimal Id { get { return this._id; } }
        public byte[] Version { get { return this._version; } }
    }
}
