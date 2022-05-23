using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Branch : BaseEntity
    {
        [Key]
        public int BranchID { get; set; }
        public string BranchName { get; set; }
        public bool IsCompany { get; set; }
        public bool IsHeadOffice { get; set; }
        public string BranchAddress { get; set; }
        public string BranchEmail { get; set; }
        public string BranchPhone { get; set; }
        public string BranchFax { get; set; }
    }
}
