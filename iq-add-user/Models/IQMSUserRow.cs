using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iq_add_user.Models
{
    public class IQMSUserRow
    {
        [Key]
        public string Username { get; set; }

        [Display(Name = "Status")]
        public string AccountStatus { get; set; }

        [Display(Name = "User Email")]
        [DataType(DataType.EmailAddress)]
        public string UserEmail { get; set; }

        [Display(Name="First")]
        public string FirstName { get; set; }

        [Display(Name = "Last")]
        public string LastName { get; set; }

        [Display(Name = "Emp Email")]
        [DataType(DataType.EmailAddress)]
        public string EmployeeEmail { get; set; }

        [Required, Display(Name = "EplantId")]
        public long? EplantId { get; set; }

        public string EplantName { get; set; }

        [Required, Display(Name = "Emp No")]
        public string EmpNo { get; set; }

        [Display(Name = "TM Id")]
        public long? TeamMemberId { get; set; }

        [Display(Name = "EU Id")]
        public long? ExpenseUserId { get; set; }

        [Display(Name = "Exp Approver")]
        public string ExpenseApprover { get; set; }
     

    }

}