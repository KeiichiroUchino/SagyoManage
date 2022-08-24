using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FarPoint.Win.Spread;
using FarPoint.Win.Spread.CellType;

namespace Jpsys.SagyoManage.FrameLib.Spread
{
    /// <summary>
    /// セルの拡張メソッドを定義します。
    /// </summary>
    public static class CellExtensions
    {
        /// <summary>
        /// セルの値を取得します。
        /// セル値の型が値型で、セルの値が null の場合は既定値を返します。
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="cell"></param>
        /// <returns></returns>
        public static T GetValue<T>(this Cell cell)
        {
            return (T)GetValue(cell, typeof(T));
        }

        /// <summary>
        /// セルの値を取得します。
        /// </summary>
        /// <param name="cell"></param>
        /// <param name="conversionType">返すオブジェクトの型</param>
        /// <returns></returns>
        public static object GetValue(this Cell cell, Type conversionType)
        {
            //指定された型が値型でかつ、セルの値がnullの場合はその値型のデフォルト値を返すのでその判断
            if (conversionType.IsValueType && cell.Value == null)
            {
                return Activator.CreateInstance(conversionType);
            }

            return Convert.ChangeType(cell.Value, conversionType);
        }
    }
}
