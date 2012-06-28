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

namespace JiraIssueBrowser.Controllers
{
    public class JiraController : Controller
    {
        public const int ISSUES_PER_PAGE = 10;

        //
        // GET: /Jira/
        private JiraClient _client = null;
        private JiraClient Client
        { 
          get
          {
              if (_client == null)
                  _client = Util.GetJiraClient(HttpContext, Server);
              return _client;
          }
        }

        public ActionResult Issues(int page = 1, int[] priority = null, int[] status = null)
        {
            string jql = "project=" + Util.GetProjectKey();
            if (priority != null)
                jql += " AND priority in (" + String.Join(",", priority) + ")";
            if (status != null)
                jql += " AND status in (" + String.Join(",", status) + ")";
            // TODO: Where to put the project?
            
            /*
            // Load JiraAccount from xml
            var serializer = new XmlSerializer(typeof(JiraAccount));
            // TODO: Put file name in variable?
            // TODO: Cache JiraAccount

            FileStream stream = new FileStream(
                Server.MapPath("~/App_Data/jira_account.xml"), FileMode.Open);
            var account = (JiraAccount) serializer.Deserialize(stream);
            stream.Close(); */
            
            var issues = new IssuesViewModel();
            /*
            issues.Issues = client.GetIssuesByProject("TES", new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                AnotherJiraRestClient.Issue.FIELD_STATUS, 
                AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                AnotherJiraRestClient.Issue.FIELD_ASSIGNEE });
            */

            issues.Issues = Client.GetIssuesByJql(
                jql, (page - 1) * ISSUES_PER_PAGE, ISSUES_PER_PAGE, 
                new string[] 
                { 
                    AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                    AnotherJiraRestClient.Issue.FIELD_STATUS, 
                    AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                    AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                    AnotherJiraRestClient.Issue.FIELD_ASSIGNEE,
                    AnotherJiraRestClient.Issue.FIELD_CREATED
                });

            issues.PriorityFilter = new MultiSelectList(Client.GetPriorities(), "id", "name", priority);

            issues.StatusFilter = new MultiSelectList(Client.GetStatuses(), "id", "name");
            issues.Page = new Page(
                page,
                issues.Issues.total / ISSUES_PER_PAGE + 1,
                (helper, pageNumber) => LinkExtensions.ActionLink(
                    helper, pageNumber.ToString(), "Issues", new { page = pageNumber, priority = priority, status = status }));
            return View(issues);
        }

        public ActionResult Issue(string key)
        {
            // TODO: Authorized to view this project?
            /*
            // Load JiraAccount from xml
            var serializer = new XmlSerializer(typeof(JiraAccount));
            // TODO: Put file name in variable?
            FileStream stream = new FileStream(
                Server.MapPath("~/App_Data/jira_account.xml"), FileMode.Open);
            var account = (JiraAccount)serializer.Deserialize(stream);
            stream.Close();

            var client = new JiraClient(account);
             */
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

        public ActionResult Create()
        {
            /*
            var issue = new Issue();
            issue.fields = new Fields();
            issue.fields.project = new Project { id = "10000" };
            issue.fields.summary = "Some summary";
            issue.fields.priority = new Priority { id = "1" };
            issue.fields.labels = new List<string> { "label_a", "label_b" };
            issue.fields.issuetype = new Issuetype { id = "2" };

            var client = new JiraClient(Util.GetJiraAccount(HttpContext, Server));
            var response = client.CreateIssue("10000", "REST issue", "2", "1", new string[] { "label1", "label2" });

            ViewBag.Response = response;
            */
            var newIssue = new NewIssueViewModel();
            newIssue.PrioritySelectList = new SelectList(Client.GetPriorities(), "id", "name");
            newIssue.IssueTypeSelectList = new SelectList(Client.GetProjectMeta(Util.GetProjectKey()).issuetypes, "id", "name");
            return View(newIssue);
        }

        [HttpPost]
        public ActionResult Create(NewIssueViewModel issue)
        {
            if (ModelState.IsValid)
            {
                var client = Util.GetJiraClient(HttpContext, Server);
                client.CreateIssue(
                    Util.GetProjectKey(), 
                    issue.summary, 
                    issue.description, 
                    issue.issueTypeId, 
                    issue.priorityId, 
                    new string[] {});
                return RedirectToAction("Issues");
            }
            return View(issue);
        }

    }
}
