using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;

namespace ScopoHR.Core.Services
{
    public class OneTimePunchService
    {
        private UnitOfWork unitOfWork;
        private Attendance attendance;
        public OneTimePunchService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<OneTimeEntryReportViewModel> GetDailyAttendance(SearchAttendanceViewModel searchVM, int id)
        {
            var query = $"EXEC GetOneTimePunchReport '" + searchVM.FromDate.ToString() + "','" + searchVM.ShiftId + "','" + searchVM.FloorName + "','"+searchVM.ToDate+"'";
            //var query = $"EXEC GetOneTimeEntryReportByDate '{dReport.FromDate.ToString()}'";
            List<OneTimeEntryReportViewModel> result = unitOfWork.attendanceRepository.SelectQuery<OneTimeEntryReportViewModel>(query).ToList();
            foreach(var a in result)
            {
                if(a.OutTimeDate == null)
                {
                    a.OutTimeDate = searchVM.FromDate.Date;
                }
            }

            return result;
        }

        public void Update(List<AttendanceViewModel> attendanceList, string name)
        {
            foreach(var at in attendanceList)
            {
                var generatedCardNo = unitOfWork.CardNoMappingRepository.Get().Where(x => x.OriginalCardNo == at.CardNo).SingleOrDefault();

                if (at.OutTime!= null)
                {
                    attendance = new Attendance
                    {
                        CardNo = generatedCardNo.GeneratedCardNo,
                        InOutTime = Convert.ToDateTime(at.OutTimeDate).Date + Convert.ToDateTime(at.OutTime).TimeOfDay,
                        ModifiedBy = name,
                        IsDeleted = false,
                        LastModified = DateTime.Now,
                        Remarks = "One Time Entry Update"
                    };
                    unitOfWork.attendanceRepository.Insert(attendance);
                }
                //for out time entry
              
                
            }
            unitOfWork.Save();
        }
    }
}
