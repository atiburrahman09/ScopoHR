using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Common
{
    /// <summary>
    /// Class holds information about subsequent column ordering
    /// </summary>
    /// <typeparam name="T"> ENTITY type to apply ordering on </typeparam>
    /// <typeparam name="TKey"> Property/Column datatype to apply the order on </typeparam>
    public class ThenOrderBy<T, TKey>
    {
        /// <summary>
        /// Sort direction 
        /// </summary>
        public SortDirection direction { get; set; }

        /// <summary>
        /// Lambda expression (stored as an expression tree) sets the column to order by
        /// </summary>
        public Expression<Func<T, TKey>> thenBy { get; set; }
    }
}
