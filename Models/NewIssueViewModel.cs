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
    public class NewIssueViewModel
    {
        [ScaffoldColumn(false)]
        public SelectList IssueTypeSelectList { get; set; }
        
        [ScaffoldColumn(false)]
        public SelectList PrioritySelectList { get; set; }

        [Display(Name = "Sammanfattning")]
        [Required]
        [StringLength(254, ErrorMessage = "Sammanfattningen får inte överstiga 254 tecken.")]
        public string Summary { get; set; }

        [Display(Name = "Beskrivning")]
        [Required]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; } 

        [Display(Name = "Rapporterat av")]
        [StringLength(254, ErrorMessage = "Namnet får inte överstiga 254 tecken.")]
        public string ClientReporter { get; set; }

        [DisplayName("Typ")]
        [Required(ErrorMessage = "Ange en typ.")]
        [UIHint("DropDownList")]
        public string issueTypeId { get; set; }

        [Display(Name = "Prioritet")]
        [Required(ErrorMessage = "Ange en prioritet.")]
        [UIHint("DropDownList")]
        public string priorityId { get; set; }

        [Display(Name = "Etiketter")]
        public IEnumerable<string> labels { get; set; }

        /// <summary>
        /// Returns a new CreateIssue based on this view model.
        /// </summary>
        /// <returns>a new CreateIssue based on this view model</returns>
        public CreateIssue ToCreateIssue()
        {
            var newIssue = new CreateIssue(Util.GetProjectKey(), Summary, Description, issueTypeId, priorityId, labels);
            newIssue.AddField(Util.GetClientReporterFieldName(), ClientReporter);
            return newIssue;
        }
    }
}