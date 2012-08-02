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

        /// <summary>
        /// Used to filter on issue priority.
        /// </summary>
        public MultiSelectList PriorityFilter { get; set; }

        /// <summary>
        /// Used i filter on issue status.
        /// </summary>
        public MultiSelectList StatusFilter { get; set; }
        
        /// <summary>
        /// Used to sort.
        /// </summary>
        public SelectList OrderBy { get; set; }
        
        /// <summary>
        /// Used to determine if sort is ascending or descending.
        /// </summary>
        public bool OrderAscending { get; set; }

        /// <summary>
        /// Result page.
        /// </summary>
        public Page Page { get; set; }

        /// <summary>
        /// Returns the time passed (in Swedish) since creating the specified issue. (Temp solution)
        /// </summary>
        /// <param name="issue">issue to relate to</param>
        /// <returns>the time passed (in Swedish) since creating the specified issue</returns>
        public string TimePassedSinceCreated(Issue issue)
        {
            // TODO: Temp lösning
            if (issue == null || issue.fields.created == null)
                return string.Empty;
            return Util.GetTimePassed(DateTime.Parse(issue.fields.created), DateTime.Now);
        }
    }
}