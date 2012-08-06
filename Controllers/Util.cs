using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Serialization;
using AnotherJiraRestClient;
using AnotherJiraRestClient.JiraModel;

namespace JiraIssueBrowser.Controllers
{
    public static class Util
    {
        public const string KEY_JIRA_ACCOUNT = "JiraAccount";
        public const string KEY_JIRA_CLIENT = "JiraClient";
        public const string KEY_PRIORITY_FILTER = "PriorityFilter";
        public const string KEY_STATUS_FILTER = "StatusFilter";
        private const double PRIORITY_FILTER_EXPIRATION_HOURS = 6.0;
        private const double STATUS_FILTER_EXPIRATION_HOURS = PRIORITY_FILTER_EXPIRATION_HOURS;
        private const string VIRTUAL_PATH_JIRA_ACCOUNT_XML = "~/App_Data/jira_account.xml";
        private const string APP_SETTING_PROJECT_KEY = "JiraProjectKey";
        private const string APP_SETTING_CLIENT_REPORTER_FIELD = "JiraClientReporterFieldName";


        /// <summary>
        /// Returns the JiraClient for this application. The JiraClient
        /// is stored in cache. If the cache has not been set this method
        /// will load the JiraAccount by loading JiraAccount from xml and
        /// constructing a new JiraClient.
        /// </summary>
        /// <param name="controller">controller used to get Context and Server</param>
        /// <returns>the JiraClient for this application</returns>
        public static JiraClient GetCachedJiraClient(this Controller controller)
        {
            return GetFromCache<JiraClient>(
                KEY_JIRA_CLIENT,
                () => new JiraClient(
                    GetJiraAccountFromXml(
                    controller.Server.MapPath(VIRTUAL_PATH_JIRA_ACCOUNT_XML))),
                controller.HttpContext);
        }

        /// <summary>
        /// Returns the possible priorities from cache. If the cache has not been set 
        /// this method will load the values using the specified JiraClient.
        /// </summary>
        /// <param name="client">used to load priories if not already in cache</param>
        /// <param name="context">Context that contains the cache</param>
        /// <returns>cached priority values</returns>
        public static List<Priority> GetCachedPriorities(this JiraClient client, HttpContextBase context)
        {
            return GetFromCache<List<Priority>>(
                KEY_PRIORITY_FILTER,
                () => client.GetPriorities(),
                PRIORITY_FILTER_EXPIRATION_HOURS,
                context);
        }

        /// <summary>
        /// Returns the possible statuses from cache. If the cache has not been set 
        /// this method will load the values using the specified JiraClient.
        /// </summary>
        /// <param name="client">used to load statuses if not already in cache</param>
        /// <param name="context">Context that contains the cache</param>
        /// <returns>cached status values</returns>
        public static List<Status> GetCachedStatuses(this JiraClient client, HttpContextBase context)
        //TODO: Convert to extension method?
        {
            return GetFromCache<List<Status>>(
                KEY_STATUS_FILTER,
                () => client.GetStatuses(),
                STATUS_FILTER_EXPIRATION_HOURS,
                context);
        }

        /// <summary>
        /// Returns the value with the specified key from the cache contained in the specified context. If
        /// the cache is not set a value will be inserted to the cache using loadData.
        /// </summary>
        /// <typeparam name="T">type of object to load</typeparam>
        /// <param name="key">cache key</param>
        /// <param name="loadData">method to load data if not in cache</param>
        /// <param name="context">context within wich the cache is contained</param>
        /// <returns>value from cache</returns>
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

        /// <summary>
        /// Returns the value with the specified key from the cache contained in the specified context. If
        /// the cache is not set a value will be inserted to the cache using loadData and the specified number
        /// of hours until expiration.
        /// </summary>
        /// <typeparam name="T">type of object to load</typeparam>
        /// <param name="key">cache key</param>
        /// <param name="loadData">method to load data if not in cache</param>
        /// <param name="hoursToExpiration">hours until value expires in cache</param>
        /// <param name="context">context within wich the cache is contained</param>
        /// <returns>value from cache</returns>
        public static T GetFromCache<T>(string key, Func<T> loadData, double hoursToExpiration, HttpContextBase context)
        {
            T obj = (T)context.Cache.Get(key);
            if (obj == null)
            {
                obj = loadData();
                context.Cache.Insert(key, obj, null, System.DateTime.UtcNow.AddHours(hoursToExpiration), System.Web.Caching.Cache.NoSlidingExpiration);
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