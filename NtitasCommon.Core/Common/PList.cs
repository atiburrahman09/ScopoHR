using Newtonsoft.Json;
using NtitasCommon.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Common
{/// <summary>
    /// SortDirection enum is used by the PList extensions
    /// </summary>
    public enum SortDirection
    {
        /// <summary>
        /// Ascending order
        /// </summary>
        Asc,

        /// <summary>
        /// Descending order
        /// </summary>
        Desc
    }

    /// <summary>
    /// PList (paged list) is class that inherits from List&#60;T&#62; and provides paging functionality  
    /// </summary>
    /// <typeparam name="T"> generic type </typeparam>
    /// <remarks>
    /// There is a problem when using Json.Net to serialize the PList class: because it inherits from List
    /// it is seen by the serializer as an IEnumerable and it will serialize the items to a Json array but 
    /// it will not serialize any of the properties (specifically the Pager property). In order to serialize both 
    /// the list items and the Pager property we placed the [JsonObjectAttribute] attribute on the class which
    /// makes Json.Net treat this class as an object (ie serialize properties) and not a list. Then in order to also
    /// serialize the list we set up the private List property which is explicitly included in the serialization (using 
    /// [JsonPropertyAttribute]). 
    /// The result of the serialization to JSON is { "Pager": { ... }, "List": [ ... ], ... }
    /// </remarks>
    [JsonObjectAttribute]
    public class PList<T> : List<T>
    {
        /// <summary>
        /// Pager Settings
        /// </summary>
        public PagerSettings Pager { get; set; }


        /// <summary>
        /// This private property is needed by the Json.Net serializer
        /// </summary>
        /// <remarks>
        /// The [JsonPropertyAttribute] tells Json.Net to also serialize this property
        /// </remarks>
        [JsonPropertyAttribute]
        private List<T> List { get { return new List<T>(this); } }
    }

    /// <summary>
    /// PagerSettings class is used by the PList class
    /// </summary>
    public class PagerSettings
    {
        /// <summary>
        /// Total Records
        /// </summary>
        public int TotalRecords { get; set; }

        /// <summary>
        /// Page Size
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Current Page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// URL Format
        /// </summary>
        public string URLFormat { get; set; }

        /// <summary>
        /// Number of pages
        /// </summary>
        public int Pages { get { return TotalRecords == 0 ? 0 : (int)Math.Ceiling(TotalRecords / (double)PageSize); } }

        private List<int> _pageNumbersToDisplay;

        /// <summary>
        /// List of the pages numbers that should be displayed (does not list all pages)
        /// </summary>
        public List<int> PageNumbersToDisplay
        {
            get { return _pageNumbersToDisplay ?? PageNumberHelper.GetPageNumbersToDisplay(this.Pages, this.CurrentPage); }
            set { _pageNumbersToDisplay = value; }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public PagerSettings()
        {
            PageSize = 10;
            CurrentPage = 0;
            TotalRecords = 0;
        }

        /// <summary>
        /// Copy constructor
        /// </summary>
        /// <param name="copy">object to copy</param>
        public PagerSettings(PagerSettings copy)
        {
            TotalRecords = copy.TotalRecords;
            PageSize = copy.PageSize;
            CurrentPage = copy.CurrentPage;
            URLFormat = copy.URLFormat;
        }
    }

    /// <summary>
    /// PList extensions
    /// </summary>
    public static class PageSortExtensions
    {
        /// <summary>
        /// Extension method converts the IEnumerable to a PList
        /// </summary>
        /// <typeparam name="T"> generic type </typeparam>
        /// <param name="theEnumerable">the enumerable</param>
        /// <param name="pager"> optionally, pass in a PagerSettings object in order to persist the values</param>
        /// <returns> new PList&#60;T&#62; </returns>
        public static PList<T> ToPList<T>(this IEnumerable<T> theEnumerable, PagerSettings pager = null)
        {
            PList<T> temp_ = new PList<T>();
            temp_.AddRange(theEnumerable);

            // use the provided pager settings
            temp_.Pager = pager == null ? new PagerSettings() : new PagerSettings(pager);

            return temp_;
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="query">query list</param>
        /// <param name="pageNumber">Required page number.Starting page is 0</param>
        /// <param name="pageSize">Page size e.g. 5,10,20 </param>
        /// <param name="orderBy"> order by property </param>
        /// <param name="direction"> order direction </param>
        /// <param name="thenOrderBy"> subsequent column ordering </param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        public static PList<T> PageSort<T>(this IQueryable<T> query, int pageNumber, int pageSize, Expression<Func<T, object>> orderBy, SortDirection direction, ThenOrderBy<T, object>[] thenOrderBy = null)
        {
            query = query.Order(orderBy, direction)         // order
                         .ThenOrderBy(thenOrderBy);         // thenBy

            return query.Page(pageNumber, pageSize);        // page and return PList
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="query">Query source</param>
        /// <param name="pageNumber">Required page number.Starting page is 0</param>
        /// <param name="pageSize">Page size e.g. 5,10,20 </param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        private static PList<T> Page<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            // This will trigger a SELECT COUNT(1) based on the current query IF this 
            // IQueryable instance was never converted to IEnumerable
            int totalRecords = query.Count();

            PList<T> list = new PList<T>
            {
                Pager = new PagerSettings
                {
                    TotalRecords = totalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };

            if (totalRecords == 0 || pageNumber >= list.Pager.Pages)
                return list;

            // executes the query
            list.AddRange(query.Skip(pageNumber * pageSize).Take(pageSize));
            return list;
        }

        /// <summary>
        /// Extension method applies paging to the current IQueryable instance and returns it 
        /// without executing the query (it only executes a SELECT COUNT (1))
        /// </summary>
        /// <typeparam name="T"> ENTITY data type </typeparam>
        /// <param name="query"> query instance </param>
        /// <param name="pageNumber"> page index </param>
        /// <param name="pageSize"> page size </param>
        /// <param name="psettings"> out parameter will also contain the total number of records for the query </param>
        /// <remarks>
        /// This will trigger a proper SELECT COUNT(1) based on the current query IF this
        /// IQueryable instance was never converted to IEnumerable</remarks>
        /// <returns> query instance </returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> query, int pageNumber, int pageSize, out PagerSettings psettings)
        {
            // This will trigger a proper SELECT COUNT(1) based on the current query IF this 
            // IQueryable instance was never converted to IEnumerable
            int totalRecords = query.Count();

            psettings = new PagerSettings
            {
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
                PageSize = pageSize
            };

            return query.Skip(pageNumber * pageSize).Take(pageSize);
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="list">query list</param>
        /// <param name="pageNumber">Required page number.Starting page is 0</param>
        /// <param name="pageSize">Page size e.g. 5,10,20 </param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        public static PList<T> Page<T>(this IList<T> list, int pageNumber, int pageSize)
        {
            var totalRecords = list.Count;

            PList<T> pList = new PList<T>
            {
                Pager = new PagerSettings
                {
                    TotalRecords = totalRecords,
                    CurrentPage = pageNumber,
                    PageSize = pageSize
                }
            };

            if (totalRecords == 0 || pageNumber >= pList.Pager.Pages)
                return pList;

            pList.AddRange(list.Skip(pageNumber * pageSize).Take(pageSize));
            return pList;
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="list"> list of T </param>
        /// <param name="pageNumber">required page number.Starting page is 0</param>
        /// <param name="pageSize">page size e.g. 5,10,20 </param>
        /// <param name="orderBy">order by</param>
        /// <param name="direction">direction</param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        public static PList<T> PageSort<T>(this IList<T> list, int pageNumber, int pageSize, Func<T, object> orderBy, SortDirection direction)
        {
            switch (direction)
            {
                case SortDirection.Asc: list = list.OrderBy(orderBy).ToList(); break;
                case SortDirection.Desc: list = list.OrderByDescending(orderBy).ToList(); break;
            }

            return list.Page(pageNumber, pageSize);
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="list"> list of T </param>
        /// <param name="pageNumber">required page number.Starting page is 0</param>
        /// <param name="pageSize">page size e.g. 5,10,20 </param>
        /// <param name="orderBy">order by</param>
        /// <param name="direction">direction</param>
        /// <param name="filterBy"> filter by</param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        public static PList<T> PageSortFilter<T>(this IList<T> list, int pageNumber, int pageSize, Func<T, object> orderBy, SortDirection direction, Func<T, bool> filterBy)
        {
            list = list.Where(filterBy).ToList();

            switch (direction)
            {
                case SortDirection.Asc: list = list.OrderBy(orderBy).ToList(); break;
                case SortDirection.Desc: list = list.OrderByDescending(orderBy).ToList(); break;
            }

            return list.Page(pageNumber, pageSize);
        }

        /// <summary>
        /// Performs paging on the provider records
        /// </summary>
        /// <param name="list"> list of T </param>
        /// <param name="pageNumber">Required page number.Starting page is 0</param>
        /// <param name="pageSize">Page size e.g. 5,10,20 </param>
        /// <param name="filterBy"> </param>
        /// <returns>Returns an instance of PagedRecordSet</returns>
        public static PList<T> PageFilter<T>(this IList<T> list, int pageNumber, int pageSize, Func<T, bool> filterBy)
        {
            list = list.Where(filterBy).ToList();

            return list.Page(pageNumber, pageSize);
        }
    }
}
