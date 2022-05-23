using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Sys_DocumentTypes
    {        
        [Key]
        public int Id { get; set; }
        [Required]
        [MaxLength(256)]        
        public string Name { get; set; }
    }
}
