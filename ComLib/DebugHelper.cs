using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Jpsys.SagyoManage.ComLib
{
    /// <summary>
    /// デバッグサポートするための機能を提供します。
    /// </summary>
    public static class DebugHelper
    {
        /// <summary>
        /// オブジェクトのプロパティ値を連結してテキストにします。
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="newLine">プロパティ毎に改行をいれるかどうか</param>
        /// <returns></returns>
        public static string PropertiesToString(this object obj, bool newLine = true)
        {
            List<string> values = new List<string>();

            foreach (var property in obj.GetType().GetProperties())
            {
                try
                {
                    var value = property.GetValue(obj, null);
                    values.Add(string.Format("{0}：{1}", property.Name, (value ?? "[null]")));
                }
                catch (Exception)
                { }
            }

            string newLineString = (newLine) ? Environment.NewLine : string.Empty;

            return string.Join(newLineString + ",", values);
        }
    }
}
