using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnotherJiraRestClient;
using JiraIssueBrowser.Controllers;

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

        public Page Page { get; set; }

        public string TestTime(Issue issue)
        {
            // TODO: Temp lösning, måste nog kolla om parse funkar
            if (issue == null || issue.fields.created == null)
                return string.Empty;
            return Util.GetTimePassed(DateTime.Parse(issue.fields.created), DateTime.Now);
        }
    }
}