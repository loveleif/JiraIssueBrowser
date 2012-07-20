using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace JiraIssueBrowser.Models
{
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
        public int TotalPages { get; set; }

        public bool IsCurrentPage(int page)
        {
            return page == CurrentPage;
        }

        public bool IsFirstPage()
        {
            return CurrentPage == 1;
        }

        public bool IsLastPage()
        {
            return CurrentPage == TotalPages;
        }

        public string GenerateUrl(UrlHelper helper, int pageNumber)
        {
            return _generateUrl(helper, pageNumber);
        }
    }
}