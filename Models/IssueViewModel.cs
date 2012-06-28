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
        private Issue Issue { get; set; }
        public string Summary
        {
            get
            {
                return Issue.fields.summary;
            }
        }
        public string ReportedBy
        {
            get
            {
                return Issue.fields.reporter.displayName == null ? string.Empty : Issue.fields.reporter.displayName;
            }
        }
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
        public List<string> Labels
        {
            get
            {
                return Issue.fields.labels;
            }
        }
        public List<Component> Components
        { 
            get 
            {
                return Issue.fields.components;
            }
        }
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
        public Uri UrlToJira
        {
            get
            {
                var jiraUrl = new Uri(Issue.self);
                return new Uri(jiraUrl.GetLeftPart(UriPartial.Authority) + "/browse/" + Issue.key);
            }
        }
        public string Description
        {
            get
            {
                if (Issue.fields.description == null)
                    return string.Empty;

                return Issue.fields.description.Replace(Environment.NewLine, "<br />");
            }
        }

        public List<Comment> Comments
        {
            get
            {
                return Issue.fields.comment.comments;
            }
        }

        public IssueViewModel(Issue issue)
        {
            Issue = issue;
        }


    }
}