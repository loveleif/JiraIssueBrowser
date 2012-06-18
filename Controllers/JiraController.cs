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

        public ActionResult Index()
        {
            // Load JiraAccount from xml
            var serializer = new XmlSerializer(typeof(JiraAccount));
            // TODO: Put file name in variable?
            FileStream stream = new FileStream(
                Server.MapPath("~/jira_account.xml"), FileMode.Open);
            var account = (JiraAccount) serializer.Deserialize(stream);
            stream.Close();

            var client = new JiraClient(account);
            var issues = client.GetIssuesByProject("TES");
            
            var issue = client.GetIssue("TES-1", new string[] {Issue.FIELD_SUMMARY, Issue.FIELD_STATUS});

            ViewBag.Test = issue.fields.summary + " | " + issue.fields.status.name + " | " + issue.fields.timeestimate;

            return View(issues);
        }
    }
}
