using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.IdentityModels
{
    public class AspNetUsers
    {
        [Column(TypeName = "NVARCHAR")]
        [StringLength(128)]
        public string Id { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        public string PasswordHash { get; set; }
        public string SecurityStamp { get; set; }
        public string PhoneNumber { get; set; }
        public bool PhoneNumberConfirmed { get; set; }
        public bool TwoFactorEnabled { get; set; }
        public DateTime? LockoutEndDateUtc { get; set; }
        public bool LockoutEnabled { get; set; }
        public int AccessFailedCount { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(256)]
        public string UserName { get; set; }

        [Column(TypeName = "DATETIME2")]
        public DateTime? LastPasswordChangedDate { get; set; }

        [Column(TypeName = "NVARCHAR")]
        [StringLength(255)]
        public string PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpireDate { get; set; }

        public List<AspNetUserRoles> AspNetUserRoles { get; set; }

        public List<AspNetUserLogins> AspNetUserLogins { get; set; }
        public List<AspNetUserClaims> AspNetUserClaims { get; set; }

    }
}
