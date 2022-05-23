using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NtitasCommon.Core.Helpers
{
    /// <summary>
    /// Helper class to get page numbers to display
    /// </summary>
    public static class PageNumberHelper
    {
        private static PageNumberHelperAccessor _accessor;

        /// <summary>
        /// Instance of PageNumberHelperAccessor. Will be used in unit test for mocking
        /// </summary>
        public static PageNumberHelperAccessor Accessor
        {
            get
            {
                if (_accessor == null)
                    _accessor = new PageNumberHelperAccessor();
                return _accessor;
            }
            set
            {
                _accessor = value;
            }
        }

        /// <summary>
        /// Get list of page numbers to display on the web-page
        /// </summary>
        /// <param name="pages">total number of pages</param>
        /// <param name="currentPage">current page</param>
        /// <param name="maskCount">Total number of page numbers to pick between 1st and last after applying the mask</param>
        /// <param name="range">The number of pages to choose before and after the current page.</param>
        /// <returns>list of page numbers. Example: { 0, -1, 12, -1, 21, 22, 23, 24, 25, 26, 27, -1, 37, -1, 50, -1, 99 }</returns>
        public static List<int> GetPageNumbersToDisplay(int pages, int currentPage, int? maskCount = null, int? range = null)
        {
            return Accessor.GetPageNumbersToDisplay(pages, currentPage, maskCount, range);
        }

        /// <summary>
        /// Gets surrounding page numbers
        /// </summary>
        /// <param name="currentPage">current page</param>
        /// <param name="pages">total number of pages</param>
        /// <param name="range">The number of pages to choose before and after the current page.</param>
        /// <returns>list of page numbers</returns>
        public static List<int> GetSurroundingPages(int currentPage, int pages, int range)
        {
            return Accessor.GetSurroundingPages(currentPage, pages, range);
        }

        /// <summary>
        /// Get page numbers after applying the mask
        /// </summary>
        /// <param name="pages">total number of pages</param>
        /// <param name="maskCount">Total number of page numbers to pick between 1st and last after applying the mask</param>
        /// <returns>list of page numbers</returns>
        public static List<int> GetMaskPages(int pages, int maskCount)
        {
            return Accessor.GetMaskPages(pages, maskCount);
        }

        /// <summary>
        /// Get the page numbers exactly at the middle of currntPage and closest page on the left and right
        /// </summary>
        /// <param name="maskedPages">masked pages(should be ASC ordered and unique)</param>
        /// <param name="currentPage">current page</param>
        /// <returns>list of page numbers</returns>
        public static List<int> GetMidPages(List<int> maskedPages, int currentPage)
        {
            return Accessor.GetMidPages(maskedPages, currentPage);
        }

        /// <summary>
        /// Replace missing pages with -1 so that they can be replaced with '...' later in the view
        /// </summary>
        /// <param name="pageNumbers">list of page numbers to display on the webpage(should be ASC ordered and unique)</param>
        /// <returns>list of page numbers</returns>
        public static List<int> FillTheVoid(List<int> pageNumbers)
        {
            return Accessor.FillTheVoid(pageNumbers);
        }
    }

    /// <summary>
    /// Helper class accessor to get page numbers to display
    /// </summary>
    public class PageNumberHelperAccessor
    {
        /// <summary>
        /// Get list of page numbers to display on the web-page
        /// </summary>
        /// <param name="pages">total number of pages</param>
        /// <param name="currentPage">current page</param>
        /// <param name="maskCount">Total number of page numbers to pick between 1st and last after applying the mask</param>
        /// <param name="range">The number of pages to choose before and after the current page.</param>
        /// <returns>list of page numbers. Example: { 0, -1, 12, -1, 21, 22, 23, 24, 25, 26, 27, -1, 37, -1, 50, -1, 99 }</returns>
        public virtual List<int> GetPageNumbersToDisplay(int pages, int currentPage, int? maskCount = null, int? range = null)
        {
            maskCount = maskCount ?? 1;
            range = range ?? 3;

            List<int> @return = new List<int>();

            List<int> maskedPages = GetMaskPages(pages, maskCount.Value);

            List<int> midPages = GetMidPages(maskedPages, currentPage);

            @return.Add(currentPage);                                              //Add Current Page
            @return.AddRange(GetSurroundingPages(currentPage, pages, range.Value));//Add Surrounding Pages
            @return.AddRange(maskedPages);                                         //Add Masked pages
            @return.AddRange(midPages);                                            //Add Mid pages

            @return = @return.Where(x => x >= 0 && x < pages).ToList();//Check for invalid page numbers

            @return = FillTheVoid(@return);

            return @return;
        }

        /// <summary>
        /// Gets surrounding page numbers
        /// </summary>
        /// <param name="currentPage">current page</param>
        /// <param name="pages">total number of pages</param>
        /// <param name="range">The number of pages to choose before and after the current page.</param>
        /// <returns>list of page numbers</returns>
        public virtual List<int> GetSurroundingPages(int currentPage, int pages, int range)
        {
            List<int> @return = new List<int>();

            if (currentPage < 0 || currentPage > pages - 1)
                return @return;

            for (int i = currentPage - range; i <= currentPage + range; i++)
                if (i >= 0 && i < pages && i != currentPage)
                    @return.Add(i);

            return @return;
        }

        /// <summary>
        /// Get page numbers after applying the mask
        /// </summary>
        /// <param name="pages">total number of pages</param>
        /// <param name="maskCount">Total number of page numbers to pick between 1st and last after applying the mask</param>
        /// <returns>list of page numbers</returns>
        public virtual List<int> GetMaskPages(int pages, int maskCount)
        {
            if (pages <= 0)
                return new List<int> { };
            if (pages == 1)
                return new List<int> { 0 };
            if (pages == 2)
                return new List<int> { 0, 1 };

            List<int> @return = new List<int>();

            maskCount = (maskCount <= pages - 2) ? maskCount : (pages - 2);

            @return.Add(0);//First page

            for (int i = 1; i <= maskCount; i++)
                @return.Add((pages * i) / (maskCount + 1));

            @return.Add(pages - 1);//Last page

            return @return;
        }

        /// <summary>
        /// Get the page numbers exactly at the middle of currntPage and closest page on the left and right
        /// </summary>
        /// <param name="maskedPages">masked pages(should be ASC ordered and unique)</param>
        /// <param name="currentPage">current page</param>
        /// <returns>list of page numbers</returns>
        public virtual List<int> GetMidPages(List<int> maskedPages, int currentPage)
        {
            List<int> @return = new List<int>();

            if (maskedPages.Count < 2 || currentPage < 0 || currentPage > maskedPages.Max())
                return @return;

            int lowerBound = maskedPages.Where(x => x < currentPage).DefaultIfEmpty().Max();
            int upperBound = maskedPages.Where(x => x > currentPage).DefaultIfEmpty(currentPage).Min();

            @return.Add(currentPage - ((currentPage - lowerBound) / 2));
            @return.Add(currentPage + ((upperBound - currentPage) / 2));

            return @return;
        }

        /// <summary>
        /// Replace missing pages with -1 so that they can be replaced with '...' later in the view
        /// </summary>
        /// <param name="pageNumbers">list of page numbers to display on the webpage(should be ASC ordered and unique)</param>
        /// <returns>list of page numbers</returns>
        public virtual List<int> FillTheVoid(List<int> pageNumbers)
        {
            List<int> @return = new List<int>();

            pageNumbers = pageNumbers.Distinct().OrderBy(x => x).ToList();

            for (int i = 0; i < pageNumbers.Count; i++)
            {
                if (i != (pageNumbers.Count - 1) && pageNumbers[i] != (pageNumbers[i + 1] - 1))
                {
                    @return.Add(pageNumbers[i]);
                    @return.Add(-1);
                }
                else
                    @return.Add(pageNumbers[i]);
            }

            return @return;
        }
    }
}
