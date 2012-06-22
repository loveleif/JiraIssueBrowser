using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnotherJiraRestClient;

namespace JiraIssueBrowser.Models
{
    public class IssuesViewModel
    {
        /// <summary>
        /// The Issues to display.
        /// </summary>
        public Issues Issues { get; set; }

        public MultiSelectList PriorityFilter { get; set; }
        public MultiSelectList StatusFilter { get; set; }
        public SelectList OrderBy { get; set; }
        public bool orderAscending { get; set; }
    }
}