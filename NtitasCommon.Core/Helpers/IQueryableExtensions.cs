using NtitasCommon.Core.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Class holds extension methods for IQueryable and IOrderedQueryable
    /// </summary>
    public static class OrderExtensions
    {
        /// <summary>
        /// Extension method applies the <paramref name="orderBy"/> using the specified <paramref name="direction"/>
        /// </summary>
        /// <typeparam name="T"> ENTITY type </typeparam>
        /// <param name="query"> IOrderedQueryable reference - usually obtained as a result of calling .OrderBy() or OrderByDescending() </param>
        /// <param name="orderBy"> order by column </param>
        /// <param name="direction"> order direction </param>
        /// <returns> ordered queryable instance </returns>
        public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> query, Expression<Func<T, object>> orderBy, SortDirection direction)
        {
            return query.Order(orderBy, direction, thenBy: true);
        }

        /// <summary>
        /// Extension method applies subsequent ordering to the IOrderedQueryable reference
        /// </summary>
        /// <typeparam name="T"> ENTITY type </typeparam>
        /// <param name="query"> IOrderedQueryable reference - usually obtained as a result of calling .OrderBy() or OrderByDescending() </param>
        /// <param name="thenOrderBy"> array of subsequent ordering to be applied </param>
        /// <returns> ordered queryable instance </returns>
        public static IOrderedQueryable<T> ThenOrderBy<T>(this IOrderedQueryable<T> query, ThenOrderBy<T, object>[] thenOrderBy)
        {
            if (thenOrderBy != null)
                foreach (ThenOrderBy<T, object> @by in thenOrderBy)
                    query = query.Order(@by.thenBy, @by.direction, thenBy: true);

            return query;
        }

        /// <summary>
        /// Extension method converts the orderBy expression and changes the return type from object to a primary data type,
        /// then applies it to the query based on the sort direction.
        /// </summary>
        /// <typeparam name="T"> ENTITY type </typeparam>
        /// <param name="query"> IQueryable instance </param>
        /// <param name="orderBy"> order by predicate </param>
        /// <param name="direction"> sort direction </param>
        /// <param name="thenBy"> uses .ThenBy/.ThenByDescending if set to true, by default it uses .OrderBy/.OrderByDescending </param>
        /// <remarks>
        /// The cast from Expression[Func[T, object]] to Expression[Func[T, EntitySupportedDataType]] is first attempted as 
        /// a manual cast with all of the C# primary data types (including String and DateTime) and their nullable counterparts.
        /// Reflection is used as a fall back, in which case we make a log to ELMAH so we can further optimize.
        /// 
        /// Using Reflection to invoke Helper.MakeExpression[ENTITY, RETURNTYPE] is on average 4-5 times slower than using
        /// a hard-coded type, so we should add more cases above rather than rely on this default clause.
        /// </remarks>
        /// <returns> the IQueryable with the ordering applied </returns>
        public static IOrderedQueryable<T> Order<T>(this IQueryable<T> query, Expression<Func<T, object>> orderBy, SortDirection direction, bool thenBy = false)
        {
            if (orderBy == null)
                return query.OrderBy(q => false);

            Expression innerExpr = orderBy.Body;
            string returnType_s = orderBy.Body.Type.ToString();

            if (returnType_s == "System.Object")
            {
                UnaryExpression unaryExpr = orderBy.Body as UnaryExpression;
                MemberExpression operandExpr = unaryExpr.Operand as MemberExpression;

                if (operandExpr == null)
                {
                    MethodCallExpression methodOperandExpr = unaryExpr.Operand as MethodCallExpression;
                    innerExpr = methodOperandExpr;
                    returnType_s = methodOperandExpr.Type.ToString();
                }
                else
                {
                    innerExpr = operandExpr;
                    returnType_s = operandExpr.Type.ToString();
                }
            }

            dynamic validExpr = null;

            switch (returnType_s)
            {
                case "System.Byte": validExpr = Expression.Lambda<Func<T, byte>>(innerExpr, orderBy.Parameters); break;
                case "System.SByte": validExpr = Expression.Lambda<Func<T, sbyte>>(innerExpr, orderBy.Parameters); break;
                case "System.Int16": validExpr = Expression.Lambda<Func<T, short>>(innerExpr, orderBy.Parameters); break;
                case "System.UInt16": validExpr = Expression.Lambda<Func<T, ushort>>(innerExpr, orderBy.Parameters); break;
                case "System.Int32": validExpr = Expression.Lambda<Func<T, int>>(innerExpr, orderBy.Parameters); break;
                case "System.UInt32": validExpr = Expression.Lambda<Func<T, uint>>(innerExpr, orderBy.Parameters); break;
                case "System.Int64": validExpr = Expression.Lambda<Func<T, long>>(innerExpr, orderBy.Parameters); break;
                case "System.UInt64": validExpr = Expression.Lambda<Func<T, ulong>>(innerExpr, orderBy.Parameters); break;
                case "System.Single": validExpr = Expression.Lambda<Func<T, float>>(innerExpr, orderBy.Parameters); break;
                case "System.Double": validExpr = Expression.Lambda<Func<T, double>>(innerExpr, orderBy.Parameters); break;
                case "System.Decimal": validExpr = Expression.Lambda<Func<T, decimal>>(innerExpr, orderBy.Parameters); break;
                case "System.Char": validExpr = Expression.Lambda<Func<T, char>>(innerExpr, orderBy.Parameters); break;
                case "System.String": validExpr = Expression.Lambda<Func<T, string>>(innerExpr, orderBy.Parameters); break;
                case "System.Boolean": validExpr = Expression.Lambda<Func<T, bool>>(innerExpr, orderBy.Parameters); break;
                case "System.DateTime": validExpr = Expression.Lambda<Func<T, DateTime>>(innerExpr, orderBy.Parameters); break;
                case "System.Guid": validExpr = Expression.Lambda<Func<T, Guid>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Byte]": validExpr = Expression.Lambda<Func<T, byte?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.SByte]": validExpr = Expression.Lambda<Func<T, sbyte?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Int16]": validExpr = Expression.Lambda<Func<T, short?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.UInt16]": validExpr = Expression.Lambda<Func<T, ushort?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Int32]": validExpr = Expression.Lambda<Func<T, int?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.UInt32]": validExpr = Expression.Lambda<Func<T, uint?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Int64]": validExpr = Expression.Lambda<Func<T, long?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.UInt64]": validExpr = Expression.Lambda<Func<T, ulong?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Single]": validExpr = Expression.Lambda<Func<T, float?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Double]": validExpr = Expression.Lambda<Func<T, double?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Decimal]": validExpr = Expression.Lambda<Func<T, decimal?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Char]": validExpr = Expression.Lambda<Func<T, char?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Boolean]": validExpr = Expression.Lambda<Func<T, bool?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.DateTime]": validExpr = Expression.Lambda<Func<T, DateTime?>>(innerExpr, orderBy.Parameters); break;
                case "System.Nullable`1[System.Guid]": validExpr = Expression.Lambda<Func<T, Guid?>>(innerExpr, orderBy.Parameters); break;

                default:
                    // Using Reflection to invoke Helper.MakeExpression<ENTITY, RETURNTYPE> is on average 4-5 times slower than using
                    // a hard-coded type above, so we should add more cases above rather than rely on this clause. 
                    // I am using this clause so I don't throw an Exception (as a fall back).
                    System.Reflection.MethodInfo method = typeof(GenericHelper).GetMethod("MakeExpression");
                    System.Reflection.MethodInfo generic = method.MakeGenericMethod(typeof(T), innerExpr.Type);
                    validExpr = generic.Invoke(null, new object[] { innerExpr, orderBy });

                    // Log to Elmah so we can optimize cases that come up and are not listed above
                    Elmah.ErrorSignal.FromCurrentContext().Raise(new CoreHandledException(
                            "[OPTIMIZATION]",
                            new Exception(String.Format("[OPTIMIZATION NEEDED] PList.cs : Order<T> - Add a new case for Expression<Func<ENTITY, {0}>>", innerExpr.Type.ToString()))
                    ));
                    break;
            }

            switch (direction)
            {
                case SortDirection.Asc: query = thenBy ? Queryable.ThenBy(query as IOrderedQueryable<T>, validExpr) : Queryable.OrderBy(query, validExpr); break;
                case SortDirection.Desc: query = thenBy ? Queryable.ThenByDescending(query as IOrderedQueryable<T>, validExpr) : Queryable.OrderByDescending(query, validExpr); break;
            }

            return query as IOrderedQueryable<T>;
        }

        /// <summary>
        /// Extension method applies the <paramref name="whereClause"/> to this queryable only if <paramref name="applyWhereClause"/> is true
        /// </summary>
        /// <typeparam name="T"> parameter on which the queryable is typed on </typeparam>
        /// <param name="query"> query to apply the <paramref name="whereClause"/> to if <paramref name="applyWhereClause"/> is true </param>
        /// <param name="applyWhereClause"> set to true to apply the <paramref name="whereClause"/> to the query </param>
        /// <param name="whereClause"> where clause expression </param>
        /// <returns> query instance </returns>
        /// <remarks>
        /// This extension method allows you to change code like 
        /// .GetQueryable().Where(x => keyword == "" || x.title.Contains(keyword))
        /// to 
        /// .GetQueryable().WhereIf(keyword != "", x => x.title.Contains(keyword))
        /// In the first example the generated SQL will always contain both conditions.
        /// Using WhereIf the generated SQL will only contain the where clause when it should be there.
        /// That helps keep the size of the generated SQL down and might have even improve performance since in certain cases
        /// there will be less conditions to parse by Entity Framework.
        /// </remarks>
        public static IQueryable<T> WhereIf<T>(this IQueryable<T> query, bool applyWhereClause, Expression<Func<T, bool>> whereClause)
        {
            return applyWhereClause ? query.Where(whereClause) : query;
        }
    }
}
