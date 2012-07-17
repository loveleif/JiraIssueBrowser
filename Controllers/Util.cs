using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using AnotherJiraRestClient;

namespace JiraIssueBrowser.Controllers
{
    public class Util
    {
        public const string KEY_JIRA_ACCOUNT = "JiraAccount";
        public const string KEY_JIRA_CLIENT = "JiraClient";
        private const string VIRTUAL_PATH_JIRA_ACCOUNT_XML = "~/App_Data/jira_account.xml";
        private const string APP_SETTING_PROJECT_KEY = "JiraProjectKey";
        private const string APP_SETTING_CLIENT_REPORTER_FIELD = "JiraClientReporterFieldName";

        /// <summary>
        /// Returns the JiraClient for this application. The JiraClient
        /// is stored in cache. If the cache has not been set this method
        /// will load the JiraAccount by loading JiraAccount from xml and
        /// constructing a new JiraClient.
        /// </summary>
        /// <param name="context">Context that contains the cache</param>
        /// <param name="server">Used to get absolute file path of the xml file</param>
        /// <returns>the JiraClient for this application</returns>
        public static JiraClient GetJiraClient(HttpContextBase context, HttpServerUtilityBase server)
        {
            return GetFromCache<JiraClient>(
                KEY_JIRA_CLIENT,
                () => new JiraClient(
                    GetJiraAccountFromXml(
                    server.MapPath(VIRTUAL_PATH_JIRA_ACCOUNT_XML))),
                context);
        }

        public static T GetFromCache<T>(string key, Func<T> loadData, HttpContextBase context)
        {
            T obj = (T)context.Cache.Get(key);
            if (obj == null)
            {
                obj = loadData();
                context.Cache.Insert(key, obj);
            }
            return obj;
        }

        private static JiraAccount GetJiraAccountFromXml(string path)
        {
            var serializer = new XmlSerializer(typeof(JiraAccount));
            FileStream stream = new FileStream(path, FileMode.Open);
            var account = (JiraAccount) serializer.Deserialize(stream);
            stream.Close();
            return account;
        }

        /// <summary>
        /// Returns the jira project key for this application (application 
        /// settings are cached automatically).
        /// </summary>
        /// <returns>the jira project key for this application</returns>
        public static string GetProjectKey()
        {
            return ConfigurationManager.AppSettings[APP_SETTING_PROJECT_KEY];
        }

        /// <summary>
        /// Returns the jira field name for the custom field "Client Reporter".
        /// </summary>
        /// <returns>the jira field name for the custom field "Client Reporter"</returns>
        public static string GetClientReporterFieldName()
        {
            return ConfigurationManager.AppSettings[APP_SETTING_CLIENT_REPORTER_FIELD];
        }

        public static string GetTimePassed(DateTime from, DateTime to)
        {
            var span = to.Subtract(from);
            var minutes = span.TotalMinutes;

            if (minutes >= 60 * 24)
            {
                var days = Math.Round(span.TotalDays);
                return days == 1 ? "1 dag" : days + " dagar";
            }
            else  if (minutes >= 60)
            {
                var hours = Math.Round(span.TotalHours);
                return hours == 1 ? "1 timma" : hours + " timmar";
            }
            else
            {
                var minutesRounded = Math.Round(minutes);
                return minutesRounded == 1 ? "1 minut" : minutesRounded + " minuter";
            }
        }

        public static void foo<T>(IEnumerable<T> values)
        {
            List<T> list = values.ToList();
        }
    }
}