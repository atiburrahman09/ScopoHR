using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ChangeCardNoViewModel : BaseEntity
    {
        [Required]
        public string OldCardNo { get; set; }
        [Required]
        public string NewCardNo { get; set; }
    }
}
