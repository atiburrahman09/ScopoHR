using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class BaseEntity
    {
        [StringLength(20)]
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }        
        public bool? IsDeleted { get; set; }
    }
}
