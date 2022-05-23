using Microsoft.VisualBasic.ApplicationServices;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class AttendanceService
    {
        private UnitOfWork unitOfWork;
        private Attendance attendance;
        private BiometricDevice bimetricDevice;
        private WorkerBus workerBus;

        public AttendanceService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<AttendanceViewModel> GetAllAttendances()
        {
            List<AttendanceViewModel> attendanceList = (
                    from at in unitOfWork.DailyAttendanceRepository.Get()
                    join emp in unitOfWork.EmployeeRepository.Get() on at.CardNo equals emp.CardNo
                    join dep in unitOfWork.DepartmentRepository.Get() on emp.DepartmentID equals dep.DepartmentID

                    select new AttendanceViewModel
                    {
                        AttendanceID = at.Id,
                        EmployeeName = emp.EmployeeName,
                        InTime = at.InTime,
                        OutTime = at.OutTime,
                        Date = at.Date,
                        Status = at.Status,
                        OverTimeHour = at.OverTime,
                        Remarks = at.Remarks,
                        DepartmentName = dep.DepartmentName,
                        CardNo = emp.CardNo
                    }
                ).OrderBy(x => x.CardNo).ToList();
            return attendanceList;
        }

        public AttendanceViewModel GetByID(int id)
        {
            AttendanceViewModel attendanceList = (
                   from at in unitOfWork.DailyAttendanceRepository.Get()
                   join emp in unitOfWork.EmployeeRepository.Get() on at.CardNo equals emp.CardNo
                   join dep in unitOfWork.DepartmentRepository.Get() on emp.DepartmentID equals dep.DepartmentID
                   where at.Id == id
                   select new AttendanceViewModel
                   {
                       AttendanceID = at.Id,
                       EmployeeName = emp.EmployeeName,
                       InTime = at.InTime,
                       OutTime = at.OutTime,
                       Date = at.Date,
                       Status = at.Status,
                       OverTimeHour = at.OverTime,
                       Remarks = at.Remarks,
                       DepartmentName = dep.DepartmentName,
                       CardNo = emp.CardNo
                   }
               ).SingleOrDefault();
            return attendanceList;
        }

        //public void Update(AttendanceViewModel AtVM, string Name)
        //{
        //    attendance = new Attendance
        //    {
        //        Id = AtVM.AttendanceID,
        //        CardNo = AtVM.CardNo,
        //        InOutTime = AtVM.InOutTime,                
        //        ModifiedBy = Name,
        //        LastModified = DateTime.Now,
        //        IsDeleted = false
        //    };
        //    unitOfWork.attendanceRepository.Update(attendance);
        //    unitOfWork.Save();
        //}

        public void Save(AttendanceViewModel at, string userName)
        {
            var generatedCardNo = unitOfWork.CardNoMappingRepository.Get().Where(x => x.OriginalCardNo == at.CardNo).SingleOrDefault();

            if (at.OutTime == null)
            {
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(at.InTimeDate).Date + Convert.ToDateTime(at.InTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = at.Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);
                unitOfWork.Save();
            }
            else
            {
                //for intime entry
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(at.InTimeDate).Date + Convert.ToDateTime(at.InTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = at.Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);

                //for out time entry
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(at.OutTimeDate).Date + Convert.ToDateTime(at.OutTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = at.Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);
                unitOfWork.Save();

            }

        }

        public void SaveAttendanceFromCSV(List<Attendance> attList)
        {
            unitOfWork.attendanceRepository.InsertRange(attList);
            unitOfWork.Save();
        }

        public List<AttendanceViewModel> GetAttendanceListByDateAndFloor(DateTime Date, string floor, int branchID)
        {
            List<AttendanceViewModel> attendanceList = unitOfWork.attendanceRepository.SelectQuery<AttendanceViewModel>("EXEC GET_ATTENDANCE '" + "" + "','" + Date.ToString() + "','" + floor + "','" + branchID + "'");
            return attendanceList;
        }

        public AttendanceViewModel GetDailyAttendance(SearchAttendanceViewModel searchVM, int branchId)
        {


            var query = $"EXEC GetAttendanceData '{searchVM.FromDate.ToString()}', '{searchVM.FromDate.ToString()}','', '{branchId}','','{searchVM.CardNo}'";
            List< AttendanceDataViewModel >  data= unitOfWork.attendanceRepository.SelectQuery<AttendanceDataViewModel>(query).ToList();

            //var data = (from emp in unitOfWork.EmployeeRepository.Get()
            //            join cm in unitOfWork.CardNoMappingRepository.Get() on emp.CardNo equals cm.OriginalCardNo
            //            join atd in unitOfWork.attendanceRepository.Get() on cm.GeneratedCardNo equals atd.CardNo into atg
            //            from att in atg.Where(x => DbFunctions.TruncateTime(x.InOutTime) == DbFunctions.TruncateTime(searchVM.FromDate)  && x.IsDeleted != true).DefaultIfEmpty()
            //            where emp.CardNo == searchVM.CardNo
            //            select new
            //            {
            //                CardNo = emp.CardNo,
            //                EmployeeName = emp.EmployeeName,
            //                InOutTime = att == null ? (DateTime?)null : att.InOutTime,
            //                Remarks = att.Remarks
            //            }).ToList();
            if(data.Count > 0)
            {
                var result = new AttendanceViewModel
                {
                    CardNo = data.FirstOrDefault().CardNo,
                    EmployeeName = data[0].EmployeeName,
                    InTime = Convert.ToDateTime(data[0].InTime),
                    InTimeDate = data[0].InTimeDate == null ? searchVM.FromDate.Date : Convert.ToDateTime(data[0].InTimeDate),
                    OutTime = data[0].OutTime == null ? (DateTime?)null : Convert.ToDateTime(data[0].OutTime),
                    OutTimeDate = data[0].OutTimeDate == null ? searchVM.FromDate.Date : Convert.ToDateTime(data[0].OutTimeDate),
                    Remarks = data[0].Remarks,
                };

                return result;
            }
            else
            {
                var empInfo = (from emp in unitOfWork.EmployeeRepository.Get()
                            join cm in unitOfWork.CardNoMappingRepository.Get() on emp.CardNo equals cm.OriginalCardNo
                            where emp.CardNo == searchVM.CardNo
                            select new AttendanceViewModel
                            {
                                CardNo = emp.CardNo,
                                EmployeeName = emp.EmployeeName,
                                InTime=(DateTime?)null,
                                InTimeDate = searchVM.FromDate.Date,
                                OutTime = (DateTime?)null,
                                OutTimeDate = searchVM.FromDate.Date,
                                HasAttendance=0
                            }).SingleOrDefault();
                return empInfo;
            }
          
        }

        public List<AttendanceViewModel> GetDailyAttendance(DateTime date, string floor, int branchId, bool absentOnly, int size, int skip)
        {
            var query = (
                from a in unitOfWork.DailyAttendanceRepository.Get()
                join emp in unitOfWork.EmployeeRepository.Get()
                on a.CardNo equals emp.CardNo


                join f in unitOfWork.productionFloorLineRepository.Get()
                on emp.ProductionFloorLineID equals f.ProductionFloorLineID into floor_group

                from fl in floor_group.DefaultIfEmpty()
                join d in unitOfWork.DepartmentRepository.Get()
                on emp.DepartmentID equals d.DepartmentID
                select new { a, emp, fl, d }
                ).AsQueryable();

            if (floor != null)
            {
                query = query.Where(x => x.fl.Floor == floor);
            }

            if (absentOnly)
            {
                query = query.Where(x => x.a.Status == "A");
            }
            else
            {
                query = query.Where(x => x.a.Status != "A");
            }

            var result = (from att in query
                          where
                          DbFunctions.TruncateTime(att.a.Date) == DbFunctions.TruncateTime(date)
                          && att.emp.BranchID == branchId
                          orderby att.a.Date
                          select new AttendanceViewModel
                          {
                              AttendanceID = att.a.Id,
                              CardNo = att.a.CardNo,
                              EmployeeName = att.emp.EmployeeName,
                              Date = att.a.Date,
                              Status = att.a.Status,
                              InTime = att.a.InTime,
                              OutTime = att.a.OutTime,
                              Remarks = att.a.Remarks,
                              OverTimeHour = att.a.OverTime,
                              DepartmentName = att.d.DepartmentName
                          }).Skip(skip).Take(size).ToList();
            return result;
        }

        public List<DropDownViewModel> GetAllBusLocation()
        {
            var res = (from b in unitOfWork.LocationRepository.Get()
                       select new DropDownViewModel
                       {
                           Text = b.LocationName,
                           Value = b.LocationID
                       }).ToList();
            return res;
        }

        public void SaveInformation(WorkerBusViewModel wbViewModel)
        {
            var query = $"EXEC UpdateAttendanceForLocation @locationID={wbViewModel.LocationID}, @inTimeDate='{wbViewModel.InTimeDate}', @inTime='{wbViewModel.InTime}', @modifiedBy='{wbViewModel.ModifiedBy}'";
            unitOfWork.attendanceRepository.RawQuery(query);
        }

        public void DeleteAttendance(AttendanceViewModel attendance, string userName)
        {
            var generatedCardNo = unitOfWork.CardNoMappingRepository.Get().Where(x => x.OriginalCardNo == attendance.CardNo).SingleOrDefault();

            //var inTime = Convert.ToDateTime(attendance.InTimeDate).Date + Convert.ToDateTime(attendance.InTime).TimeOfDay;
            //var outTime = Convert.ToDateTime(attendance.OutTimeDate).Date + Convert.ToDateTime(attendance.OutTime).TimeOfDay;

            //var attendanceIn = unitOfWork.attendanceRepository.Get().Where(y => y.InOutTime == inTime && y.CardNo == generatedCardNo.GeneratedCardNo).ToList();
            //foreach (var att in attendanceIn)
            //{
            //    att.IsDeleted = true;
            //    att.ModifiedBy = userName;
            //    att.LastModified = DateTime.Now;
            //    unitOfWork.attendanceRepository.Update(att);
            //}

            //var attendanceOut = unitOfWork.attendanceRepository.Get().Where(y => y.InOutTime == outTime && y.CardNo == generatedCardNo.GeneratedCardNo).ToList();
            //foreach (var att in attendanceOut)
            //{
            //    att.IsDeleted = true;
            //    att.ModifiedBy = userName;
            //    att.LastModified = DateTime.Now;
            //    unitOfWork.attendanceRepository.Update(att);
            //}


            var attendanceList = unitOfWork.attendanceRepository.Get().Where(y => (DbFunctions.TruncateTime(y.InOutTime)) == DbFunctions.TruncateTime(attendance.InTimeDate) && y.CardNo == generatedCardNo.GeneratedCardNo).ToList();
            if (attendanceList.Count() > 0)
            {
                foreach (var att in attendanceList)
                {
                    att.IsDeleted = true;
                    att.ModifiedBy = userName;
                    att.LastModified = DateTime.Now;
                    unitOfWork.attendanceRepository.Update(att);
                }
            }

            unitOfWork.Save();
        }

        public void SaveList(WorkerBusViewModel list)
        {
            unitOfWork.WorkerBusRepository.RawQuery("DELETE A FROM WorkerBus A WHERE A.LocationID = '"+ list.LocationID +"'");

            for (int i = 0; i < list.empList.Count(); i++)
            {
                workerBus = new WorkerBus
                {
                    CardNo=list.empList[i],
                    LocationID=list.LocationID                    
                };
                unitOfWork.WorkerBusRepository.Insert(workerBus);

            }

            unitOfWork.Save();
        }

        public void UpdateDailyAttendance(List<AttendanceViewModel> attList, string userName)
        {
            var cardNo = attList[0].CardNo;
            var generatedCardNo = unitOfWork.CardNoMappingRepository.Get().Where(x => x.OriginalCardNo == cardNo).SingleOrDefault();

            if (attList[0].OutTime == null)
            {
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(attList[0].InTimeDate).Date + Convert.ToDateTime(attList[0].InTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = attList[0].Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);
                unitOfWork.Save();
            }
            else
            {
                //for intime entry
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(attList[0].InTimeDate).Date + Convert.ToDateTime(attList[0].InTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = attList[0].Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);

                //for out time entry
                attendance = new Attendance
                {
                    CardNo = generatedCardNo.GeneratedCardNo,
                    InOutTime = Convert.ToDateTime(attList[0].OutTimeDate).Date + Convert.ToDateTime(attList[0].OutTime).TimeOfDay,
                    ModifiedBy = userName,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    Remarks = attList[0].Remarks
                };
                unitOfWork.attendanceRepository.Insert(attendance);
                unitOfWork.Save();
            }
        }

        public void UpdateDailyAttendance(List<LeaveApplicationViewModel> leaveApps)
        {
            foreach (var item in leaveApps)
            {
                List<DailyAttendance> attList = unitOfWork.DailyAttendanceRepository.Get()
                    .Where(
                        x => x.CardNo == item.CardNo
                        && DbFunctions.TruncateTime(x.Date) >= DbFunctions.TruncateTime(item.FromDate)
                        && DbFunctions.TruncateTime(x.Date) <= DbFunctions.TruncateTime(item.ToDate)
                    )
                    .Select(x => x)
                    .ToList();
                if (attList.Count > 0)
                {
                    foreach (var att in attList)
                    {
                        if (IsPayableLeave(item))
                        {
                            att.Status = AttendanceStatus.PaidLeave;
                        }
                        else
                        {
                            att.Status = AttendanceStatus.UnpaidLeave;
                        }
                        unitOfWork.DailyAttendanceRepository.Update(att);
                        unitOfWork.Save();
                    }
                }
            }
        }

        private bool IsPayableLeave(LeaveApplicationViewModel leave)
        {
            var res = unitOfWork.LeaveTypeRepository.Get()
                      .Where(x => x.LeaveTypeID == leave.LeaveTypeID)
                      .Select(x => x.IsPayable)
                      .SingleOrDefault();
            return res;
        }

        public void SyncDailyAttendance(int branchId, DateTime date, string userName)
        {
            var query = $"EXEC SyncDailyAttendance @branchId={branchId}, @date='{date}', @modifiedBy='{userName}'";
            unitOfWork.attendanceRepository.RawQuery(query);
        }


        // NEW Sync

        public void SyncInTime(int branchId, DateTime date, string shiftId, string userName)
        {
            var query = $"EXEC SyncInTime @branchId={branchId}, @date='{date}', @shiftId={shiftId}, @modifiedBy={userName}";
            unitOfWork.DailyAttendanceRepository.RawQuery(query);
        }

        public void SyncOutTime(int branchId, DateTime date, string shiftId, string userName)
        {
            var query = $"EXEC SyncOutTime @branchId={branchId}, @date='{date}', @shiftId={shiftId}, @modifiedBy={userName}";
            unitOfWork.DailyAttendanceRepository.RawQuery(query);
        }

        public void SaveFromAccessDB(List<AttendanceViewModel> attendanceViewModel, DateTime date, int deviceId)
        {
            foreach (var at in attendanceViewModel)
            {
                attendance = new Attendance
                {
                    CardNo = at.CardNo,
                    InOutTime = at.InOutTime,
                    ModifiedBy = at.ModifiedBy,
                    LastModified = DateTime.Now,
                    IsDeleted = false
                };
                unitOfWork.attendanceRepository.Insert(attendance);

            }
            var device = unitOfWork.BiometricDeviceRepository.GetById(deviceId);
            device.LastSync = date;
            device.LastModified = date;
            unitOfWork.BiometricDeviceRepository.Update(device);
            unitOfWork.Save();
        }

        public void SaveFromBiometricDevice(List<Attendance> records, DateTime date, int deviceId)
        {
            unitOfWork.attendanceRepository.InsertRange(records);

            var device = unitOfWork.BiometricDeviceRepository.GetById(deviceId);
            device.LastSync = date;
            device.LastModified = date;
            unitOfWork.BiometricDeviceRepository.Update(device);
            unitOfWork.Save();
        }

        public List<BiometricDeviceViewModel> GetBiometricDevices()
        {
            var res = (from b in unitOfWork.BiometricDeviceRepository.Get()
                       select new BiometricDeviceViewModel
                       {
                           Id = b.Id,
                           InServiceSince = b.InServiceSince,
                           IpAddress = b.IpAddress,
                           LastSync = b.LastSync,
                           Port = b.Port,
                           MachineNumber = b.MachineNumber
                       }).ToList();
            return res;
        }

        public DateTime? GetLastInOutTime(DateTime date)
        {
            var res = (from d in unitOfWork.attendanceRepository.Get()
                       where DbFunctions.TruncateTime(d.InOutTime) == DbFunctions.TruncateTime(date)
                       orderby d.InOutTime descending
                       select d.InOutTime).FirstOrDefault();
            if (DateTime.MinValue == res)
            {
                return null;
            }
            return res;
        }

        public void SaveMachinesFromAccessDB(List<BiometricDeviceViewModel> deviceList)
        {
            unitOfWork.BiometricDeviceRepository.RawQuery("DELETE D FROM BiometricDevices D");
            unitOfWork.Save();

            foreach (var device in deviceList)
            {
                bimetricDevice = new BiometricDevice
                {
                    InServiceSince = DateTime.Now,
                    IsDeleted = false,
                    LastModified = DateTime.Now,
                    LastSync = null,
                    MachineNumber = device.MachineNumber,
                    IpAddress = device.IpAddress,
                    Port = device.Port,
                    ModifiedBy = "Syste"
                };
                unitOfWork.BiometricDeviceRepository.Insert(bimetricDevice);
            }
            unitOfWork.Save();
        }

    }
}
