using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class RegisterOfWorkerViewModel
    {
        public int? TicketNo { get; set; }
        public string Name { get; set; }
        public string NID { get; set; }
        public string FatherName { get; set; }
        public string MotherName { get; set; }
        public string Gender { get; set; }
        public Nullable<DateTime> DateOfBirth { get; set; }
        public string Age { get; set; }
        public string PermanentAddress { get; set; }
        public Nullable<DateTime> JoinDate { get; set; }
        public string Designation { get; set; }
        public Int16? Grade { get; set; }
        public string CardNo { get; set; }
        public string WorkingTime { get; set; }
        public string TimeOfInterval { get; set; }
        public string WeeklyHoliday { get; set; }
        public string Department { get; set; }
        public string Shift { get; set; }
        public string DescriptionOfChageOfGroup { get; set; }
        public string Comment { get; set; }

    }
}
