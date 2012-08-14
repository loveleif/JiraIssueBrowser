JiraIssueBrowser
================

JiraIssueBrowser is a ASP.NET MVC 3 web application for browsing, viewing and creating Jira issues.

Configuration
-------------

###### jira_account.xml
The Jira account information is stored as xml. Once the file is loaded it is cached on the server.

1. Copy `jira_account.template.xml` to the App_Data folder.
2. Rename the copied file to `jira_account.xml`.
3. Edit `jira_account.xml`, replace comments with Jira account information.

###### AppSettings
Make sure to set the following AppSettings in Web.config.

* JiraProjectKey - The project key used for this application (the application is configured for one specific project)
* JiraClientReporterFieldName - The custom field id for "Customer Reporter". This is a custom field that is used to capture the reporter of users without Jira access.