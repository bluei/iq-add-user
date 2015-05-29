using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace iq_add_user.Models
{
    public class IQMSUser
    {
        // This class is used as the model to add new users, 
        // and also to display a list of users.
        
        [Key, Required]
        public string Username { get; set; }

        [DataType(DataType.EmailAddress), Required]
        public string Email { get; set; }

        [Display(Name = "Copy From")]
        public string CopyFrom { get; set; }

        [Required, Display(Name = "Eplant")]
        public int EplantId { get; set; }

        public string EplantName { get; set; }

        // From PR_EMP...
        [Display(Name = "First Name"), Required]
        public string FirstName { get; set; }

        [Display(Name = "Last Name"), Required]
        public string LastName { get; set; }

        [Required, Display(Name = "Employee/Badge #")]
        public string EmpNo { get; set; }

        public string JobTitle { get; set; }

        [Display(Name = "Copy Settings")]
        public bool CopyPermissions { get; set; }   //Allows you to add an expense user but leave permissions alone

        [Display(Name = "Team Member")]
        public bool AddTeamMember { get; set; }

        [Display(Name = "Expense User")]
        public bool AddExpenseUser { get; set; }

        [Display(Name = "Expense Approver")]
        public string ApproverUsername { get; set; }

        public IQMSUser()
        {
            // Defaults on creation
            EmpNo = String.Format("{0:yyyyMMdd.HHmm}", DateTime.Now);
            Email = "@gill-industries.com";
            CopyPermissions = true;
        }
     

    }

}