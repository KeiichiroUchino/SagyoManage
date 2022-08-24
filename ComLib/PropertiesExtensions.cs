using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using Jpsys.SagyoManage.ComLib.ViewSettings;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// PropertyInfo配列の拡張メソッドを定義します。
    /// </summary>
    public static class PropertiesExtensions
    {
        /// <summary>
        /// プロパティの並び替えに必要な情報
        /// </summary>
        private struct PropertySortInfo
        {
            /// <summary>
            /// PropertyInfo配列上のインデックス
            /// </summary>
            public int PropertyInfoIndex { get; set; }
            /// <summary>
            /// ソートインデックス
            /// </summary>
            public int SortIndex { get; set; }
            /// <summary>
            /// プロパティ情報
            /// </summary>
            public PropertyInfo PropertyInfo { get; set; }
        }

        /// <summary>
        /// ColumnOrderの設定を使用して並び替えを行います。
        /// </summary>
        /// <param name="properties"></param>
        /// <returns></returns>
        public static IEnumerable<PropertyInfo> OrderByColumnOrder(this PropertyInfo[] properties)
        {
            List<PropertySortInfo> propertySortInfos = new List<PropertySortInfo>();

            for (int i = 0; i < properties.Length; i++)
            {
                var property = properties[i];
                var sortIndexAttribute = property.GetCustomAttribute<ColumnOrder>();

                //ソートIndexを取得。属性が設定されていない場合は-1
                int sortIndex = (sortIndexAttribute != null) ? sortIndexAttribute.Index : -1;

                propertySortInfos.Add(
                    new PropertySortInfo()
                    {
                        PropertyInfoIndex = i,
                        SortIndex = sortIndex,
                        PropertyInfo = property,
                    });
            }

            return
                propertySortInfos
                    .OrderBy(element => element.SortIndex < 0)
                    .ThenBy(element => element.SortIndex)
                    .ThenBy(element => element.PropertyInfoIndex)
                    .Select(element => element.PropertyInfo);
        }
    }
}
