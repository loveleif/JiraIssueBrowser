using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using AnotherJiraRestClient;
using System.Configuration;
using System.Xml.Serialization;

namespace JiraIssueBrowser.Controllers
{
    public class JiraController : Controller
    {
        //
        // GET: /Jira/

        public ActionResult Issues()
        {
            // Load JiraAccount from xml
            var serializer = new XmlSerializer(typeof(JiraAccount));
            // TODO: Put file name in variable?
            FileStream stream = new FileStream(
                Server.MapPath("~/App_Data/jira_account.xml"), FileMode.Open);
            var account = (JiraAccount) serializer.Deserialize(stream);
            stream.Close();

            var client = new JiraClient(account);
            var issues = client.GetIssuesByProject("TES", new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                AnotherJiraRestClient.Issue.FIELD_STATUS, 
                AnotherJiraRestClient.Issue.FIELD_DESCRIPTION, 
                AnotherJiraRestClient.Issue.FIELD_PRIORITY,
                AnotherJiraRestClient.Issue.FIELD_ASSIGNEE });

            return View(issues);
        }

        public ActionResult Issue(string key)
        {
            // TODO: Authorized to view this project?

            // Load JiraAccount from xml
            var serializer = new XmlSerializer(typeof(JiraAccount));
            // TODO: Put file name in variable?
            FileStream stream = new FileStream(
                Server.MapPath("~/App_Data/jira_account.xml"), FileMode.Open);
            var account = (JiraAccount)serializer.Deserialize(stream);
            stream.Close();

            var client = new JiraClient(account);
            var issue = client.GetIssue(key, new string[] { 
                AnotherJiraRestClient.Issue.FIELD_SUMMARY, 
                AnotherJiraRestClient.Issue.FIELD_CREATED,
                AnotherJiraRestClient.Issue.FIELD_LABELS,
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
    }
}
