using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.IdentityModels
{
    public class AspNetUserLogins
    {
        [StringLength(128)]
        [Key, Column(Order = 0, TypeName = "NVARCHAR")]
        public string LoginProvider { get; set; }

        [StringLength(128)]
        [Key, Column(Order = 1, TypeName = "NVARCHAR")]
        public string ProviderKey { get; set; }

        [StringLength(128)]
        [Key, Column(Order = 2, TypeName = "NVARCHAR")]
        [ForeignKey("AspNetUser")]
        public string UserId { get; set; }

        public AspNetUsers AspNetUser { get; set; }
    }
}
