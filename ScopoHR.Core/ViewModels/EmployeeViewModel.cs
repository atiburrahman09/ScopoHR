using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class EmployeeViewModel
    {
        public int EmployeeID { get; set; }
        [Required]
        public string EmployeeName { get; set; }
        public string CardNo { get; set; }
        [Required]
        public int GenderID { get; set; }
        public int DesignationID { get; set; }
        public string DesignationName { get; set; }
        public int DepartmentID { get; set; }
        public string DepartmentName { get; set; }
        [Required]
        public string MobileNo { get; set; }
        public short MaritalStatus { get; set; }
        public string SpouseName { get; set; }
        public int? ProductionFloorLineID { get; set; }
        public string Floor { get; set; }
        public string Line { get; set; }        
        public int ShiftId { get; set; }


        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string GrandFathername { get; set; }
        public short? SalaryGrade { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress]
        public string Email { get; set; }
        public string NID { get; set; }
        public string BloodGroup { get; set; }

        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public int? ReferenceID { get; set; }
        public string ReferenceName { get; set; }
        public string NomineeName { get; set; }
        public int? ReportToID { get; set; }
        public string ReportToName { get; set; }
        public DateTime JoinDate { get; set; }

        public int BranchID { get; set; }
        public string ModifiedBy { get; set; }        
        public bool IsActive { get; set; }
        public bool? IsDeleted { get; set; }

        public bool? OTApplicable { get; set; }
        public bool? AttendanceBonusApplicable { get; set; }

        public int? TicketNo { get; set; }
        public int? EmployeeType { get; set; }

        public int? SectionID { get; set; }


    }
}
