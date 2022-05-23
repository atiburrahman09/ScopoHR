using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class ClientViewModel
    {
        public int ClientID { get; set; }

        [Required]
        public string ClientName { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Website { get; set; }
        public string MobileNo { get; set; }
    }
}
