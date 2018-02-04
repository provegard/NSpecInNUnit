using System;
using System.Linq.Expressions;
using System.Reflection;

namespace NSpecInNUnit
{
    internal static class Extensions
    {
        internal static void SetProtectedProperty<T, TProp>(this T instance, Expression<Func<T, TProp>> propExpr,
            TProp value)
        {
            if (propExpr.Body is MemberExpression memExp)
            {
                if (memExp.Member is PropertyInfo propInfo)
                {
                    propInfo.SetValue(instance, value);
                    return;
                }
            }

            throw new ArgumentException("Cannot identify the property to set from " + propExpr);
        }
    }
}