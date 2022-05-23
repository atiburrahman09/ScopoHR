using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class LeaveMappingService
    {
        private UnitOfWork unitOfWork;
        private LeaveMapping leaveMapping;
        private YearService yearService;
        private LeaveTypeService leaveTypeservice;

        public LeaveMappingService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            yearService = new YearService(unitOfWork);
            leaveTypeservice = new LeaveTypeService(unitOfWork);
        }

        public List<LeaveDaysViewModel> GetLeaveMapping(int employeeID)
        {

            var res = (from l in unitOfWork.LeaveTypeRepository.Get()
                       join e in unitOfWork.LeaveMappingRepository.Get().Where(x => x.EmployeeID == employeeID)
                       on l.LeaveTypeID equals e.LeaveTypeID into g1
                       from g in g1.DefaultIfEmpty()
                       select new LeaveDaysViewModel
                       {
                           LeaveDays = g != null ? g.LeaveDays : 0,
                           LeaveTaken = g != null ? g.LeaveTaken : 0,
                           LeaveTypeID = l.LeaveTypeID,
                           LeaveTypeName = l.LeaveTypeName
                       }
                       ).ToList();
            return res;
        }
        
        public List<LeaveApplicationViewModel> UpdateLeaveMapping(List<LeaveApplicationViewModel> list, string name)
        {
            var approaved = new List<LeaveApplicationViewModel>();

            try
            {
                foreach (var LeaveMVM in list)
                {
                    var leaveMapping = (from lM in unitOfWork.LeaveMappingRepository.Get()
                                        where lM.EmployeeID == LeaveMVM.EmployeeID && lM.LeaveTypeID == LeaveMVM.LeaveTypeID
                                        select lM).SingleOrDefault();
                    if (leaveMapping == null)
                        continue;
                    if (leaveMapping.LeaveDays - leaveMapping.LeaveTaken >= LeaveMVM.TotalDays)
                    {
                        leaveMapping.LeaveTaken += LeaveMVM.TotalDays;
                        leaveMapping.ModifiedBy = name;
                        leaveMapping.LastModified = DateTime.Now;
                        leaveMapping.IsDeleted = false;
                        LeaveMVM.Status = Helpers.LeaveApplicationStatus.Approaved;
                        approaved.Add(LeaveMVM);
                        unitOfWork.LeaveMappingRepository.Update(leaveMapping);
                    }
                }
                unitOfWork.Save();
                return approaved;
            }
            catch(Exception ex)
            {
                return new List<LeaveApplicationViewModel>();
            }            
        }

        public void SaveLeaveMapping(LeaveMappingViewModel leaveMappingVM)
        {

            var existingMapping = unitOfWork.LeaveMappingRepository.Get().Where(x => x.EmployeeID == leaveMappingVM.EmployeeID).ToList();

            if(existingMapping == null || existingMapping.Count == 0)
            {
                YearMappingViewModel yearVM = yearService.GetYear();
                if (yearVM == null)
                {
                    throw new Exception("Error: year not found.");
                }
                foreach (var item in leaveMappingVM.LeaveDaysList)
                {
                    leaveMapping = new LeaveMapping
                    {
                        EmployeeID = leaveMappingVM.EmployeeID,
                        LeaveTypeID = item.LeaveTypeID,
                        LeaveDays = item.LeaveDays,
                        YearMappingID = yearVM.YearMappingID
                    };
                    unitOfWork.LeaveMappingRepository.Insert(leaveMapping);

                }
                
            }
            else
            {
                //unitOfWork.LeaveMappingRepository.DeleteRange(existingMapping);
                
            }
            unitOfWork.Save();
        }

        public List<LeaveTypeViewModel> GetAll()
        {
            var data = (from l in unitOfWork.LeaveTypeRepository.Get()
                        select new LeaveTypeViewModel
                        {
                            LeaveTypeID=l.LeaveTypeID,
                            LeaveTypeName=l.LeaveTypeName
                        }).ToList();
            return data;
        }
        
        public int CreateLeaveMapping(int employeeId, int genderId, int yearMappingId)
        {
            List<LeaveTypeViewModel> leaveTypeList = leaveTypeservice.GetAll();
            YearMappingViewModel yearVM = yearService.GetYear();
            
            if (yearVM == null)
            {
                throw new Exception("Error: year not found.");
            }

            foreach (var item in leaveTypeList)
            {
                if (item.LeaveTypeName == "Maternity Leave")
                {
                    if (genderId == 1)
                    {
                        leaveMapping = new LeaveMapping
                        {
                            EmployeeID = employeeId,
                            LeaveTypeID = item.LeaveTypeID,
                            LeaveDays = 0,
                            YearMappingID = yearVM.YearMappingID
                        };

                        unitOfWork.LeaveMappingRepository.Insert(leaveMapping);
                        //unitOfWork.Save();
                    }
                    else {

                        leaveMapping = new LeaveMapping
                        {
                            EmployeeID = employeeId,
                            LeaveTypeID = item.LeaveTypeID,
                            LeaveDays = item.LeaveDays,
                            YearMappingID = yearVM.YearMappingID
                        };

                        unitOfWork.LeaveMappingRepository.Insert(leaveMapping);
                    }
                    //else
                    //{
                    //    leaveMapping = new LeaveMapping
                    //    {
                    //        EmployeeID = employeeId,
                    //        LeaveTypeID = item.LeaveTypeID,
                    //        LeaveDays = 0,
                    //        YearMappingID = yearVM.YearMappingID
                    //    };

                    //    unitOfWork.LeaveMappingRepository.Insert(leaveMapping);
                    //    unitOfWork.Save();
                    //}
                }
                //else if(item.LeaveTypeName == "paternal")
                //{
                //    if (genderId == 1)
                //    {
                       
                //    }
                //}
                else
                {
                    leaveMapping = new LeaveMapping
                    {
                        EmployeeID = employeeId,
                        LeaveTypeID = item.LeaveTypeID,
                        LeaveDays = item.LeaveDays,
                        YearMappingID = yearVM.YearMappingID
                    };

                    unitOfWork.LeaveMappingRepository.Insert(leaveMapping);
                   
                }
            }
            unitOfWork.Save();
            return 0;
        }

        public List<LeaveDaysViewModel> GetLeaveMappingByEmployeeId(int id)
        {
            var res = (from l in unitOfWork.LeaveMappingRepository.Get()
                       join lt in unitOfWork.LeaveTypeRepository.Get()
                       on l.LeaveTypeID equals lt.LeaveTypeID
                       where l.EmployeeID == id
                       select new LeaveDaysViewModel
                       {
                          LeaveMappingId = l.LeaveMappingID,
                          LeaveDays = l.LeaveDays,
                          LeaveTypeID = l.LeaveTypeID,                                                                          
                          LeaveTypeName = lt.LeaveTypeName                                 
                       }).ToList();
            return res;
        }

        public void UpdateLeaveAllocation(List<LeaveDaysViewModel> mapping)
        {
            foreach(var item in mapping)
            {
                leaveMapping = unitOfWork.LeaveMappingRepository.GetById(item.LeaveMappingId);
                leaveMapping.LeaveDays = item.LeaveDays;
                unitOfWork.LeaveMappingRepository.Update(leaveMapping);
                unitOfWork.Save();
            }
        }
    }
}
