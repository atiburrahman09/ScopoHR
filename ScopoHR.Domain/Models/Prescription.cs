using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Prescription:BaseEntity
    {
        public int PrescriptionID { get; set; }
        public int EmployeeID { get; set; }
        public string Description { get; set; }
        public DateTime PrescribedDate { get; set; }
        public string PatientStatement { get; set; }
        public string Pulse { get; set; }
        public string BP { get; set; }
        public string Respiration { get; set; }
        public string Temperature { get; set; }
        public string Heart { get; set; }
        public string Lungs { get; set; }
        public string Anaemia { get; set; }
        public string Jaundice { get; set; }
        public string Oeadema { get; set; }
        public string Dehydretion { get; set; }
        public string Skin { get; set; }
        public string Eyes { get; set; }
        public string PAbdomen { get; set; }
        public string Others { get; set; }
    }
}
