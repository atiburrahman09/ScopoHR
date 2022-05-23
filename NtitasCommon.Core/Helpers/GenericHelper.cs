using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Class holds static generic helpers
    /// </summary>
    public static class GenericHelper
    {

        /// <summary>
        /// Re-types the supplied expression with the specified ENTITY and RETURN types
        /// </summary>
        /// <typeparam name="ENTITY">      the type for which the Expression is applied </typeparam>
        /// <typeparam name="RETURN">      return type of the Expression </typeparam>
        /// <param     name="operandExpr"> Expression to encapsulate </param>
        /// <param     name="expr">        Expression from which to copy the Parameters from </param>
        /// <returns></returns>
        public static Expression<Func<ENTITY, RETURN>> MakeExpression<ENTITY, RETURN>(MemberExpression operandExpr, Expression<Func<ENTITY, object>> expr)
        {
            return Expression.Lambda<Func<ENTITY, RETURN>>(operandExpr, expr.Parameters);
        }


        /// <summary>
        /// Extension method allows us to map any object 
        /// </summary>
        /// <typeparam name="TSource">tsource</typeparam>
        /// <typeparam name="TResult">tresult</typeparam>
        /// <param name="source">source</param>
        /// <param name="selector">selector</param>
        /// <returns>  </returns>
        public static TResult SelectAsNew<TSource, TResult>(this TSource source, Func<TSource, TResult> selector)
        {
            return selector.Invoke(source);
        }
    }
}
