using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AnotherJiraRestClient;
using System.Configuration;
using System.Xml.Serialization;
using JiraIssueBrowser.Models;
using System.Web.Mvc.Html;
using AnotherJiraRestClient.JiraModel;

namespace JiraIssueBrowser.Controllers
{
    public class JiraController : Controller
    {
        public const int ISSUES_PER_PAGE = 10;

        public JiraClient Client { get { return this.GetCachedJiraClient(); } }

        //
        // GET: /jira/issues
        public ActionResult Issues(int page = 1, int[] priority = null, int[] status = null)
        {
            string jql = "project=" + Util.GetProjectKey();
            if (priority != null)
                jql += " AND priority in (" + String.Join(",", priority) + ")";
            if (status != null)
                jql += " AND status in (" + String.Join(",", status) + ")";
            
            var model = new IssuesViewModel();

            try
            {
                model.Issues = Client.GetIssuesByJql(
                    jql, (page - 1) * ISSUES_PER_PAGE, ISSUES_PER_PAGE,
                    new string[] 
                { 
                    AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                    AnotherJiraRestClient.Issue.FIELD_STATUS, 
                    AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                    AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                    AnotherJiraRestClient.Issue.FIELD_ASSIGNEE,
                    AnotherJiraRestClient.Issue.FIELD_CREATED,
                    AnotherJiraRestClient.Issue.FIELD_REPORTER
                });

                model.PriorityFilter = new MultiSelectList(Client.GetCachedPriorities(HttpContext), "id", "name", priority);

                model.StatusFilter = new MultiSelectList(Client.GetCachedStatuses(HttpContext), "id", "name", status);
            }
            catch (JiraApiException ex)
            {
                throw new HttpException(503, "Tjänsten är inte tillgänglig, kunde inte nå Jira.");
            }

            model.Page = new Page(
                page,
                model.Issues.total / ISSUES_PER_PAGE + 1,
                (helper, pageNumber) => CreateIssuesUrl(helper, pageNumber, priority, status));
            
            return View(model);
        }

        /// <summary>
        /// Returns a string containing the url for accessing /jira/issues with the specified
        /// page and filters.
        /// </summary>
        /// <param name="helper">used to create the url</param>
        /// <param name="pageNumber">page number</param>
        /// <param name="priority">priority filter</param>
        /// <param name="status">status filter</param>
        /// <returns>string containing an url</returns>
        public static string CreateIssuesUrl(UrlHelper helper, int pageNumber, int[] priority, int[] status)
        {
            // Fulkod...
            var href = helper.Action("issues", "jira", new { page = pageNumber }, helper.RequestContext.HttpContext.Request.Url.Scheme);

            if (priority != null)
            {
                href += "?";
                href += string.Join("&", priority.Select(x => "priority=" + HttpUtility.UrlEncode(x.ToString())));
            }
            if (status != null)
            {
                href += "&";
                href += string.Join("&", status.Select(x => "status=" + HttpUtility.UrlEncode(x.ToString())));
            }
            
            return href;
        }

        //
        // GET: /jira/issues/{issue}
        public ActionResult Issue(string key)
        {
            var issue = Client.GetIssue(key, new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY,
                AnotherJiraRestClient.Issue.FIELD_PROJECT,
                AnotherJiraRestClient.Issue.FIELD_CREATED,
                AnotherJiraRestClient.Issue.FIELD_LABELS,
                AnotherJiraRestClient.Issue.FIELD_COMPONENTS,
                AnotherJiraRestClient.Issue.FIELD_COMMENT,
                AnotherJiraRestClient.Issue.FIELD_REPORTER,
                AnotherJiraRestClient.Issue.FIELD_PRIORITY, 
                AnotherJiraRestClient.Issue.FIELD_TIMEESTIMATE,
                AnotherJiraRestClient.Issue.FIELD_RESOLUTION, 
                AnotherJiraRestClient.Issue.FIELD_RESOLUTIONDATE,
                AnotherJiraRestClient.Issue.FIELD_STATUS, 
                AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                AnotherJiraRestClient.Issue.FIELD_ASSIGNEE });
            if (issue == null || issue.key != key || issue.fields.project.key != Util.GetProjectKey())
                throw new HttpException(404, "Issue not found.");
            return View(new IssueViewModel(issue));
        }

        //
        // GET: /jira/issues/create
        public ActionResult Create()
        {
            var newIssue = new NewIssueViewModel();
            newIssue.PrioritySelectList = new SelectList(Client.GetCachedPriorities(HttpContext), "id", "name");
            newIssue.IssueTypeSelectList = new SelectList(Client.GetProjectMeta(Util.GetProjectKey()).issuetypes, "id", "name");
            return View(newIssue);
        }

        //
        // POST: /jira/issues/create
        [HttpPost]
        public ActionResult Create(NewIssueViewModel issue)
        {
            if (!ModelState.IsValid)
                return View(issue);

            var createIssue = issue.ToCreateIssue();
               
            BasicIssue newIssue;
            try 
            {
                newIssue = Client.CreateIssue(createIssue);
	        }
	        catch (JiraApiException ex)
	        {
                return RedirectToAction("CreateStatus", new { message = ex.Message });
	        }

            return RedirectToAction("CreateStatus", new { message = "Skapade en ny förfrågan med nyckel: " + newIssue.key });
            
        }


        //
        // GET: /jira/issues/create
        public string CreateStatus(string message)
        {
            return message;
        }
    }
}
