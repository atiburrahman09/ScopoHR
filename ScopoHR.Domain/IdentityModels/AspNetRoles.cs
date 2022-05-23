using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.IdentityModels
{
    public class AspNetRoles
    {
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string Name { get; set; }

        public List<AspNetUserRoles> AspNetUserRoles { get; set; }
    }
}
