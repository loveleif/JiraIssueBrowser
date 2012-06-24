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

namespace JiraIssueBrowser.Controllers
{
    public class JiraController : Controller
    {
        //
        // GET: /Jira/
        

        public ActionResult Issues(string[] priority, string[] status)
        {
            string jql = "project=TES";
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
            
            var client = Util.GetJiraClient(HttpContext, Server);

            var issues = new IssuesViewModel();
            /*
            issues.Issues = client.GetIssuesByProject("TES", new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                AnotherJiraRestClient.Issue.FIELD_STATUS, 
                AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                AnotherJiraRestClient.Issue.FIELD_ASSIGNEE });
            */
            issues.Issues = client.GetIssuesByJql(jql, new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                AnotherJiraRestClient.Issue.FIELD_STATUS, 
                AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                AnotherJiraRestClient.Issue.FIELD_ASSIGNEE });

            issues.PriorityFilter = new MultiSelectList(client.GetPriorities(), "id", "name");
            issues.StatusFilter = new MultiSelectList(client.GetStatuses(), "id", "name");
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
            var client = Util.GetJiraClient(HttpContext, Server);
            var issue = client.GetIssue(key, new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
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

            return View(issue);
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
            var client = Util.GetJiraClient(HttpContext, Server);
            var newIssue = new NewIssueViewModel();
            newIssue.PrioritySelectList = new SelectList(client.GetPriorities(), "id", "name");
            
            return View(newIssue);
        }

        [HttpPost]
        public ActionResult Create(NewIssueViewModel issue)
        {
            if (ModelState.IsValid)
            {
                var client = Util.GetJiraClient(HttpContext, Server);
                // TODO: project id!!!
                client.CreateIssue("10000", issue.summary, issue.issueTypeId, issue.priorityId, new string[] {});
                return RedirectToAction("Issues");
            }
            return View(issue);
        }
    }
}
