using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeIdCardViewModel
    {
        public string CardNo { get; set; }
        public string IssedDate { get; set; }
        public string CompanyName { get; set; }
        public string EmployeeName { get; set; }
        public string Designation { get; set; }
        public string WorkingType { get; set; }
        public string Department { get; set; }
        public Nullable<DateTime> JoiningDate { get; set; }
        public int? Ticket { get; set; }
        public string Expiry { get; set; }
        public string Job { get; set; }
        public string BloodGroup { get; set; }
        public string CompanyAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Telephone { get; set; }
        public string EmergencyPhone { get; set; }
        public string NID { get; set; }
        public string Floor { get; set; }
        public string UniqueIdentifier { get; set; }

        public byte[] QRCode { get; set; }

    }
}
