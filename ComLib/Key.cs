using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    public class Key
    {
        /// <summary>
        /// キーの構成要素
        /// </summary>
        private List<object> _KeyElementValues;

        /// <summary>
        /// キーの構成要素の値を指定してインスタンスを初期化します。
        /// </summary>
        /// <param name="values"></param>
        public Key(params object[] values)
            : this((IEnumerable<object>)values)
        { }

        public Key(IEnumerable<object> values)
        {
            _KeyElementValues = new List<object>();

            foreach (var value in values)
            {
                _KeyElementValues.Add(value);
            }
        }

        public override bool Equals(object obj)
        {
            var objAs = obj as Key;

            if (objAs == null)
            {
                return false;
            }

            return Enumerable.SequenceEqual(this._KeyElementValues, objAs._KeyElementValues);
        }

        public override int GetHashCode()
        {
            return
                //キーの各要素をXORしていった結果を返す
                _KeyElementValues.Aggregate(
                0,
                (accumulated, element) =>
                {
                    int hashCodeOfElement = (element != null) ? element.GetHashCode() : 0;
                    return accumulated ^ hashCodeOfElement;
                });
        }

        public override string ToString()
        {
            return
                string.Join(
                    ",",
                    _KeyElementValues.Select(element => element.ToString()));
        }
    }
}
