using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class Employee : BaseEntity
    {
        public int EmployeeID { get; set; }
        public string EmployeeName { get; set; }
        public string CardNo { get; set; }
        public int GenderID { get; set; }
        public int DesignationID { get; set; }
        public int DepartmentID { get; set; }
        public string MobileNo { get; set; }
        public short MaritalStatus { get; set; }
        public string SpouseName { get; set; }
        public int? ProductionFloorLineID { get; set; }
        [ForeignKey("ProductionFloorLineID")]
        public virtual ProductionFloorLine ProductionFloorLines { get; set; }
        public int ShiftId { get; set; }
        //[ForeignKey("ShiftId")]
        //public virtual WorkingShift WorkingShifts { get; set; }

        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string Email { get; set; }
        public string NID { get; set; }
        public string BloodGroup { get; set; }
        public string GrandFatherName { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string NomineeName { get; set; }
        public int? ReferenceID { get; set; }
        public int? ReportToID { get; set; }
        public short? SalaryGrade { get; set; }
        public DateTime? DateOfBirth { get; set; }
        
        public DateTime JoinDate { get; set; }

        public int BranchID { get; set; }        
        public bool IsActive { get; set; }
        public bool? OTApplicable { get; set; }
        public bool? AttendanceBonusApplicable { get; set; }
        public int? TicketNo { get; set; }
        public int? EmployeeType { get; set; }
        public int? SectionID { get; set; }
    }


    public class CardNoMapping : BaseEntity
    {
        public int Id { get; set; }
        public string OriginalCardNo { get; set; }
        public string GeneratedCardNo { get; set; }
        public string Gender { get; set; }
    }
}
