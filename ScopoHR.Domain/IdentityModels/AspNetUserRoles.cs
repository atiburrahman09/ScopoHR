using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.IdentityModels
{
    public class AspNetUserRoles
    {
        [StringLength(128)]
        [Key, Column(Order = 0, TypeName = "NVARCHAR")]
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }

        [StringLength(128)]
        [Key, Column(Order = 1, TypeName = "NVARCHAR")]
        [ForeignKey("AspNetRole")]
        public string RoleId { get; set; }

        public AspNetRoles AspNetRole { get; set; }
        public AspNetUsers AspNetUser { get; set; }
    }
}
