using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Reflection;
using System.Linq.Expressions;

namespace Jpsys.SagyoManage.ComLib
{
    public static class ReflectionUtil
    {

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T">対象のプロパティの</typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T>> e)
        {
            var member = (MemberExpression)e.Body;

            var propinfo = member.Member as PropertyInfo;

            if (propinfo == null)
            {
                throw new Exception(member.Member.Name + "のPropertyInfoの取得に失敗しました。");
            }

            return propinfo;

            //一行で書くと
            //return ((MemberExpression)e.Body).Member as PropertyInfo;
            //使い方
            //X ins = new X()
            //PropertyInfo pi = GetProperty(() => ins.MyProperty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <returns></returns>
        public static PropertyInfo GetPropertyInfo<T>(Expression<Func<T, object>> e)
        {
            var member = (MemberExpression)e.Body;

            var propinfo = member.Member as PropertyInfo;

            if (propinfo == null)
            {
                throw new Exception(member.Member.Name + "のPropertyInfoの取得に失敗しました。");
            }

            return propinfo;

            //一行で書くと
            //return ((MemberExpression)e.Body).Member as PropertyInfo;
            //使い方
            //PropertyInfo pi = GetProperty<X>(c => c.MyProperty);
        }

        /// <summary>
        /// PropertyInfoを指定して対象の属性を取得くします。
        /// </summary>
        /// <typeparam name="T">取得したい属性型</typeparam>
        /// <param name="propInfo">PropertyInfo</param>
        /// <returns>
        /// 指定した型の属性
        /// 対象の属性が存在しない場合はnullを返却します。
        /// 対象の属性が複数ある場合は先頭の1件目を返却します。
        /// </returns>
        public static T GetAttributeByPropertyInfo<T>(PropertyInfo propInfo)
            where T : Attribute
        {
            T rt_attr = null;

            var attr = propInfo.GetCustomAttributes(typeof(T), true);
            if (attr != null && attr.Any())
            {
                rt_attr = (T)attr[0];
            }

            return rt_attr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T1"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <returns></returns>
        public static T1 GetModelClassDatabaseAttribute<T1, T2>()
            where T1 : Attribute
        {

            T1 MyAttribute =
                (T1)Attribute.GetCustomAttribute(typeof(T2), typeof(T1));


            if (MyAttribute == null)
            {
                throw new InvalidOperationException(typeof(T1).ToString() + "属性を持たないクラスは使用できません");
            }

            return MyAttribute;

        }

    }
}
