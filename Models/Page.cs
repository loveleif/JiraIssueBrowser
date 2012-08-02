using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace JiraIssueBrowser.Models
{
    /// <summary>
    /// Class representing a range of pages and a current page.
    /// </summary>
    public class Page
    {
        private int _currentPage;
        private Func<UrlHelper, int, string> _generateUrl;

        public Page(int currentPage, int totalPages, Func<UrlHelper, int, string> generateUrl)
        {
            TotalPages = totalPages;
            CurrentPage = currentPage;
            this._generateUrl = generateUrl;            
        }

        /// <summary>
        /// The current page.
        /// </summary>
        public int CurrentPage {
            get { return _currentPage; }
            set 
            {
                if (value > TotalPages)
                    _currentPage = TotalPages;
                else if (value < 0)
                    _currentPage = 0;
                else
                    _currentPage = value;
            }
        }

        /// <summary>
        /// The total number of pages.
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// Returns true if the specified page is the current page.
        /// </summary>
        /// <param name="page">page number</param>
        /// <returns>true if the specified page is the current page</returns>
        public bool IsCurrentPage(int page)
        {
            return page == CurrentPage;
        }

        /// <summary>
        /// Returns true if the current page is the first page.
        /// </summary>
        /// <returns>true if the current page is the first page</returns>
        public bool IsFirstPage()
        {
            return CurrentPage == 1;
        }

        /// <summary>
        /// Returns true if the current page is the last page.
        /// </summary>
        /// <returns>true if the current page is the last page</returns>
        public bool IsLastPage()
        {
            return CurrentPage == TotalPages;
        }

        /// <summary>
        /// Returns the url to the specified page number.
        /// </summary>
        /// <param name="helper">used to create the url</param>
        /// <param name="pageNumber">page number</param>
        /// <returns>the url to the specified page number</returns>
        public string GenerateUrl(UrlHelper helper, int pageNumber)
        {
            return _generateUrl(helper, pageNumber);
        }
    }
}