using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Document : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string UniqueIdentifier { get; set;}
        public short Category { get; set; }        
        public int EmployeeId { get; set; }
        [ForeignKey("EmployeeId")]
        public virtual  Employee Employees { get; set; }
    }
}
