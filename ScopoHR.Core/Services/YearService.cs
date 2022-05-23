using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;

namespace ScopoHR.Core.Services
{
    public class YearService
    {
        private UnitOfWork unitOfWork;
        private YearMapping yearVM;
        private YearMappingViewModel previousYear;
        private YearMappingViewModel res;
        private LeaveMapping leaveVM;


        public YearService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void CreateYear(int year, string Name)
        {
            try
            {
                if (!IsPreviousYearExists(year))
                {
                    yearVM = new YearMapping
                    {
                        Year = year,
                        IsOpen = false,
                        ModifiedBy=Name,
                        LastModified=DateTime.Now,
                        IsDeleted=false
                    };
                    unitOfWork.YearRepository.Insert(yearVM);
                    yearVM = new YearMapping
                    {
                        Year = year + 1,
                        IsOpen = true,
                        ModifiedBy = Name,
                        LastModified = DateTime.Now,
                        IsDeleted = false
                    };
                    unitOfWork.YearRepository.Insert(yearVM);

                }
                else
                {

                    yearVM = new YearMapping
                    {
                        YearMappingID = previousYear.YearMappingID,
                        Year = year,
                        IsOpen = false
                    };
                    unitOfWork.YearRepository.Update(yearVM);
                    yearVM = new YearMapping
                    {
                        Year = year + 1,
                        IsOpen = true
                    };
                    unitOfWork.YearRepository.Insert(yearVM);
                }
                unitOfWork.Save();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ForwardLeave(string year, string Name)
        {
            res = GetYear();
            List<LeaveViewModel> leaveModel = (from l in unitOfWork.LeaveTypeRepository.Get()
                                               join lM in unitOfWork.LeaveMappingRepository.Get()
                                               on l.LeaveTypeID equals lM.LeaveTypeID into leaveGroup
                                               from leave in leaveGroup.DefaultIfEmpty()
                                               where l.IsForward == true && leave.YearMappingID == (res.YearMappingID - 1)
                                               orderby l.LeaveTypeID ascending
                                               select new LeaveViewModel
                                               {
                                                   EmployeeID = leave.EmployeeID,
                                                   LeaveMappingID=leave.LeaveMappingID,
                                                   LeaveTypeID = l.LeaveTypeID,
                                                   LeaveDays = leave != null ? leave.LeaveDays : 0,
                                                   LeaveDaysTotalByType = l.LeaveDays,
                                                   LeaveTaken = leave != null ? leave.LeaveTaken : 0,
                                                   YearMappingID = leave.YearMappingID,
                                                   TotalLeaveByType = l.LeaveDays

                                               }).ToList();

          

            foreach (var leave in leaveModel)
            {
                leaveVM = new LeaveMapping
                {
                    EmployeeID = leave.EmployeeID,
                    LeaveTypeID = leave.LeaveTypeID,
                    LeaveDays = (leave.LeaveDays - leave.LeaveTaken) + leave.LeaveDaysTotalByType,
                    LeaveTaken = 0,
                    YearMappingID = res.YearMappingID,
                    ModifiedBy = Name,
                    LastModified = DateTime.Now,
                    IsDeleted = false

                };
                unitOfWork.LeaveMappingRepository.Insert(leaveVM);
            }
            unitOfWork.Save();

        }

        public YearMappingViewModel GetYear()
        {
            previousYear = (from y in unitOfWork.YearRepository.Get()
                            orderby y.YearMappingID descending
                            select new YearMappingViewModel
                            {
                                Year = y.Year,
                                YearMappingID = y.YearMappingID,
                                IsOpen = y.IsOpen
                            }
                         ).Take(1).FirstOrDefault();

            return previousYear;
        }

        public bool IsPreviousYearExists(int year)
        {
            previousYear = (from y in unitOfWork.YearRepository.Get()
                            orderby y.YearMappingID descending
                            select new YearMappingViewModel
                            {
                                Year = y.Year,
                                YearMappingID = y.YearMappingID,
                                IsOpen = y.IsOpen
                            }
                           ).Take(1).SingleOrDefault();
            if (previousYear == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
