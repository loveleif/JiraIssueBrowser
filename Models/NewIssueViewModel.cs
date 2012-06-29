using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace JiraIssueBrowser.Models
{
    // TODO: Valdiation
    public class NewIssueViewModel
    {
        [ScaffoldColumn(false)]
        public SelectList IssueTypeSelectList { get; set; }
        
        [ScaffoldColumn(false)]
        public SelectList PrioritySelectList { get; set; }

        [ScaffoldColumn(false)]
        public string projectId { get; set; }

        [Display(Name = "summary_label", ResourceType = typeof(Resources.strings))]
        [Required]
        [StringLength(254, ErrorMessage = "The summary can't exceed 254 characters.")]
        public string summary { get; set; }

        [Display(Name = "description_label", ResourceType = typeof(Resources.strings))]
        [Required]
        [DataType(DataType.MultilineText)]
        public string description { get; set; }

        [DisplayName("Issue type")]
        [Required(ErrorMessage = "You must select a issue type")]
        [UIHint("DropDownList")]
        public string issueTypeId { get; set; }

        [DisplayName("Priority")]
        [Required(ErrorMessage = "You must select a priority")]
        [UIHint("DropDownList")]
        public string priorityId { get; set; }

        [DisplayName("Labels")]
        public IEnumerable<string> labels { get; set; }
    }
}