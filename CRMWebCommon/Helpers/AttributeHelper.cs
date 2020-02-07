using CRMWebCommon.EntityWrappers;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.Serialization;

namespace CRMWebCommon.Helpers
{
    public static class AttributeHelper
    {
        public static string GetAttributeName<TSource, TProperty>(Expression<Func<TSource, TProperty>> lambda)
        {
            Type type = typeof(TSource);
            
            MemberExpression member = lambda.Body as MemberExpression ?? 
                ((UnaryExpression)(lambda as LambdaExpression).Body).Operand as MemberExpression;
            if (member == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a method, not a property.",
                    lambda.ToString()));

            PropertyInfo propInfo = member.Member as PropertyInfo;
            if (propInfo == null)
                throw new ArgumentException(string.Format(
                    "Expression '{0}' refers to a field, not a property.",
                    lambda.ToString()));

            if (type != propInfo.ReflectedType &&
                !type.IsSubclassOf(propInfo.ReflectedType))
                throw new ArgumentException(string.Format(
                    "Expression '{0}' reafers to a property tht is not from type {1}.",
                    lambda.ToString(),
                    type));

            propInfo = type.GetProperty(propInfo.Name);

            IEnumerable<DataMemberAttribute> attrs = propInfo.GetCustomAttributes<DataMemberAttribute>(true);
            foreach (DataMemberAttribute attr in attrs)
            {
                DataMemberAttribute dataAttribute = attr as DataMemberAttribute;
                if (dataAttribute != null)
                {
                    return dataAttribute.Name;
                }
            }

            return string.Empty;
        }

        public static IEnumerable<string> GetAllColumns<T>() where T : CrmEntity
        {
            Type type = typeof(T);
            PropertyInfo[] pinfos = type.GetProperties();
            IList<string> cols = new List<string>();

            foreach (PropertyInfo pi in pinfos)
            {
                IEnumerable<DataMemberAttribute> attrs = pi.GetCustomAttributes<DataMemberAttribute>();

                foreach (DataMemberAttribute attr in attrs)
                {
                    cols.Add(attr.Name);
                }
            }

            return cols;
        }

    }
}
