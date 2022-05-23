using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Medicine:BaseEntity
    {
        public int MedicineID { get; set; }
        public string MedicineName { get; set; }
        public decimal? UnitPrice { get; set; }
    }
}
