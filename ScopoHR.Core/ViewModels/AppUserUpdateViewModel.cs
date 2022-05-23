using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class AppUserUpdateViewModel
    {
        [Required]
        public string UserName { get; set; }        
        public string Password { get; set; }                
        public string ConfirmPassword { get; set; }
        public List<string> Roles { get; set; }
        public List<int> BranchIDs { get; set; }
        public string EmployeeName { get; set; }
    }
}
