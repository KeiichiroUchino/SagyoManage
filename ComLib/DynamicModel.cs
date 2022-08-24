using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Dynamic;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// プロパティを動的に追加できるモデル用のクラスです。
    /// </summary>
    public sealed class DynamicModel : DynamicObject
    {
        public readonly Dictionary<string, object> _properties;

        public DynamicModel(Dictionary<string, object> properties) {
            _properties = properties;
        }

        public override IEnumerable<string> GetDynamicMemberNames() {
            return _properties.Keys;
        }

        public override bool TryGetMember(GetMemberBinder binder, out object result) {
            if (_properties.ContainsKey(binder.Name)) {
                result = _properties[binder.Name];
                return true;
            } else {
                result = null;
                return false;
            }
        }

        public override bool TrySetMember(SetMemberBinder binder, object value) {
            if (_properties.ContainsKey(binder.Name)) {
                _properties[binder.Name] = value;
                return true;
            } else {
                return false;
            }
        }
    }
}
