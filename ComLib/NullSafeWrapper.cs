using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// NullSafeなアクセスを提供するためのラッパークラスです。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullSafeWrapper<T>
    {
        #region コンストラクタ

        public NullSafeWrapper(T obj)
        {
            _obj = obj;
        }

        #endregion

        private T _obj;

        /// <summary>
        /// 値を返すファンクションを指定して値を取得します。
        /// 指定したオブジェクトがnullの場合はデフォルト値を返します。
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public PT GetValueOrDefault<PT>(Func<T, PT> func)
        {
            return NullSafeValueHelper.GetValueOrDefault(_obj, func);
        }

        /// <summary>
        /// 値を返すファンクションを指定して値を取得します。
        /// 指定したオブジェクトがnullの場合はデフォルト値を返します。
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public string GetValueOrEmpty(Func<T, string> func)
        {
            return NullSafeValueHelper.GetValueOrEmpty(_obj, func);
        }

        /// <summary>
        /// 値を返すファンクションを指定して値をNullable型で取得します。戻り値の型は構造体のみ指定できます。
        /// 指定したオブジェクトがnullの場合はNullを返します。
        /// </summary>
        /// <typeparam name="PT"></typeparam>
        /// <param name="func"></param>
        /// <returns></returns>
        public PT? GetValueOrNull<PT>(Func<T, PT> func) where PT : struct
        {
            return NullSafeValueHelper.GetValueOrNull(_obj, func);
        }
    }
}
