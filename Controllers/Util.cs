using System;
using System.Collections.Generic;
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

        public static JiraClient GetJiraClient(HttpContextBase context, HttpServerUtilityBase server)
        {
            var client = (JiraClient)context.Cache.Get(KEY_JIRA_CLIENT);
            if (client == null)
            {
                var xmlPath = server.MapPath(VIRTUAL_PATH_JIRA_ACCOUNT_XML);
                client = new JiraClient(GetJiraAccountFromXml(xmlPath));
                context.Cache.Insert(
                    KEY_JIRA_CLIENT,
                    client);
            }
            return client;
        }

        public static T GetFromCache<T>(string key, Func<T> loadData, HttpContextBase context, HttpServerUtilityBase server)
        {
            T obj = (T)context.Cache.Get(key);
            // TODO: Check type?
            if (obj == null)
            {
                obj = loadData();
                context.Cache.Insert(key, obj);
            }
            return obj;
        }

        /// <summary>
        /// Returns the JiraAccount for this application. The JiraAccount
        /// is stored in cache. If the cache has not been set this method
        /// will load the JiraAccount from XML and set the cache.
        /// </summary>
        /// <param name="context">Context that contains the cache</param>
        /// <param name="server">Used to get absolute file path of the xml 
        /// file</param>
        /// <returns>the JiraAccount for this application</returns>
        public static JiraAccount GetJiraAccount(HttpContextBase context, HttpServerUtilityBase server)
        {

            // Try to get JiraAccount from cache
            JiraAccount account = (JiraAccount) context.Cache.Get(KEY_JIRA_ACCOUNT);
            if (account == null)
            {
                // Load JiraAccount from XML
                account = GetJiraAccountFromXml(server.MapPath(VIRTUAL_PATH_JIRA_ACCOUNT_XML));
                // TODO: Expiration?
                context.Cache.Insert(KEY_JIRA_ACCOUNT, account);
            }
            return account;
        }

        private static JiraAccount GetJiraAccountFromXml(string path)
        {
            var serializer = new XmlSerializer(typeof(JiraAccount));
            FileStream stream = new FileStream(path, FileMode.Open);
            var account = (JiraAccount) serializer.Deserialize(stream);
            stream.Close();
            return account;
        }
    }
}