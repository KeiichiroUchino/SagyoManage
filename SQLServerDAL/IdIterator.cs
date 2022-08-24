using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.SQLServerDAL
{
    /// <summary>
    /// 複数IDの列挙を提供します。
    /// </summary>
    public class IdIterator
    {
        /// <summary>
        /// 元になるId
        /// </summary>
        private IList<decimal> _source;
        /// <summary>
        /// Idのイテレーター
        /// </summary>
        private IEnumerator<decimal> _idIterator;

        public IdIterator(IList<decimal> ids)
        {
            _source = ids;
            _idIterator = _source.GetEnumerator();
        }

        /// <summary>
        /// 値を持つかどうか
        /// </summary>
        /// <returns></returns>
        public bool HasValue { get { return _source.Count > 0; } }
        /// <summary>
        /// Idの件数を返します。
        /// </summary>
        public int Count { get { return _source.Count; } }

        /// <summary>
        /// 最初の要素を返します。
        /// </summary>
        /// <returns></returns>
        public decimal First()
        {
            return _source.First();
        }
        /// <summary>
        /// 最初の要素を返します。なければ空を表す値を返します。
        /// </summary>
        /// <returns></returns>
        public decimal FirstOrEmpty()
        {
            return _source.FirstOrDefault();
        }

        /// <summary>
        /// 次のIdを取得します。
        /// </summary>
        /// <returns></returns>
        public decimal Next()
        {
            bool passed = _idIterator.MoveNext();

            if (!passed)
            {
                throw new InvalidOperationException("次の値が存在しません。");
            }

            return _idIterator.Current;
        }

        /// <summary>
        /// 元になったIdを取得します。
        /// </summary>
        /// <returns></returns>
        public IList<decimal> GetSource()
        {
            //ToList()して、Listの新しいインスタンスを作成して返す。
            return _source.ToList();
        }
    }
}
