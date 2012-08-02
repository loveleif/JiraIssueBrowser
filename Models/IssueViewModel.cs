using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AnotherJiraRestClient;
using AnotherJiraRestClient.JiraModel;

namespace JiraIssueBrowser.Models
{
    public class IssueViewModel
    {
        /// <summary>
        /// The issue of this view model.
        /// </summary>
        private Issue Issue { get; set; }

        /// <summary>
        /// The issue summary.
        /// </summary>
        public string Summary
        {
            get
            {
                return Issue.fields.summary;
            }
        }

        /// <summary>
        /// The issue reporter.
        /// </summary>
        public string ReportedBy
        {
            get
            {
                return Issue.fields.reporter.displayName == null ? string.Empty : Issue.fields.reporter.displayName;
            }
        }

        /// <summary>
        /// The issue creation date time.
        /// </summary>
        public string Created
        {
            get
            {
                if (Issue.fields.created == null)
                    return string.Empty;
                
                var created = DateTime.Parse(Issue.fields.created);
                return created.ToLongDateString() + " " + created.ToShortTimeString();
            }
        }

        /// <summary>
        /// The issue status.
        /// </summary>
        public Status Status 
        {
            get
            {
                if (Issue.fields.status == null)
                    return Status.UNKNOWN_STATUS;
                else
                    return Issue.fields.status;
            }
        }

        /// <summary>
        /// The issue priority.
        /// </summary>
        public Priority Priority
        {
            get
            {
                if (Issue.fields.priority == null)
                    return Priority.UNKNOWN_PRIORITY;
                else
                    return Issue.fields.priority;
            }
        }

        /// <summary>
        /// The issue labels. 
        /// </summary>
        public List<string> Labels
        {
            get
            {
                return Issue.fields.labels;
            }
        }

        /// <summary>
        /// The issue components.
        /// </summary>
        public List<Component> Components
        { 
            get 
            {
                return Issue.fields.components;
            }
        }

        /// <summary>
        /// The issue time estimate.
        /// </summary>
        public string TimeEstimate 
        {
            get
            {
                if (Issue.fields.timeestimate <= 0)
                    return string.Empty;
                else
                    return (Issue.fields.timeestimate / 3600).ToString() + " timmar";
            }
        }

        /// <summary>
        /// The issue solution name and time.
        /// </summary>
        public string Solution
        {
            get
            {
                if (Issue.fields.resolution == null)
                    return string.Empty;

                var resolutionDate = DateTime.Parse(Issue.fields.resolutiondate);
                return Issue.fields.resolution.name 
                    + " (" + resolutionDate.ToLongDateString()
                    + " "  + resolutionDate.ToShortTimeString() + ")";

            }
        }

        /// <summary>
        /// Uri to issue in jira.
        /// </summary>
        public Uri UrlToJira
        {
            get
            {
                var jiraUrl = new Uri(Issue.self);
                return new Uri(jiraUrl.GetLeftPart(UriPartial.Authority) + "/browse/" + Issue.key);
            }
        }

        /// <summary>
        /// The issue description.
        /// </summary>
        public string Description
        {
            get
            {
                if (Issue.fields.description == null)
                    return string.Empty;

                return Issue.fields.description.Replace(Environment.NewLine, "<br />");
            }
        }

        /// <summary>
        /// The issue comments.
        /// </summary>
        public List<Comment> Comments
        {
            get
            {
                return Issue.fields.comment.comments;
            }
        }

        /// <summary>
        /// Constructs a new IssueViewModel.
        /// </summary>
        /// <param name="issue">issue to view</param>
        public IssueViewModel(Issue issue)
        {
            Issue = issue;
        }


    }
}