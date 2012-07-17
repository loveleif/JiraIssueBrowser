using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AnotherJiraRestClient.JiraModel;
using JiraIssueBrowser.Controllers;

namespace JiraIssueBrowser.Models
{
    // TODO: Valdiation
    public class NewIssueViewModel
    {
        [ScaffoldColumn(false)]
        public SelectList IssueTypeSelectList { get; set; }
        
        [ScaffoldColumn(false)]
        public SelectList PrioritySelectList { get; set; }

        [Display(Name = "summary_label", ResourceType = typeof(Resources.strings))]
        [Required]
        [StringLength(254, ErrorMessage = "The summary can't exceed 254 characters.")]
        public string summary { get; set; }

        [Display(Name = "description_label", ResourceType = typeof(Resources.strings))]
        [Required]
        [DataType(DataType.MultilineText)]
        public string description { get; set; } 

        [Display(Name = "client_reporter_label", ResourceType = typeof(Resources.strings))]
        [StringLength(254, ErrorMessage = "The name can't exceed 254 characters.")]
        public string ClientReporter { get; set; }


        [DisplayName("Issue type")]
        [Required(ErrorMessage = "You must select a issue type")]
        [UIHint("DropDownList")]
        public string issueTypeId { get; set; }

        [Display(Name = "priority_label", ResourceType = typeof(Resources.strings))]
        [Required(ErrorMessage = "You must select a priority")]
        [UIHint("DropDownList")]
        public string priorityId { get; set; }

        [DisplayName("Labels")]
        public IEnumerable<string> labels { get; set; }

        public CreateIssue ToCreateIssue()
        {
            var newIssue = new CreateIssue(Util.GetProjectKey(), summary, description, issueTypeId, priorityId, labels);
            newIssue.AddField(Util.GetClientReporterFieldName(), ClientReporter);
            return newIssue;
        }
    }
}