
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using ScopoHR.WebUI.Helpers;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace ScopoHR.WebUI
{
    public class HangFireJob
    {
        private AttendanceService attendanceService;
        //private BiometricSyncService biometricSyncService;
        private ScopoContext db;
        private UnitOfWork unitOfWork;
        private int branchId = 1;
        private string userName = "System";

        private string dateStr;

        private DateTime date;

        public HangFireJob()
        {
            db = new ScopoContext();
            unitOfWork = new UnitOfWork(db);
            attendanceService = new AttendanceService(unitOfWork);
            //oleDbHelper = new OleDbHelper();
            dateStr = ConfigurationManager.AppSettings.Get("SyncDate");
            if (!string.IsNullOrEmpty(dateStr))
            {
                date = Convert.ToDateTime(dateStr).AddDays(-1);
            }
            else
            {
                date = DateTime.Now.AddDays(-1);
            }
        }



        public void SyncDailyAttendance()
        {            
            attendanceService.SyncDailyAttendance(branchId, DateTime.Now, userName);
        }
        
        // New Sync

        private void setDate()
        {
            if (!string.IsNullOrEmpty(dateStr))
            {
                date = Convert.ToDateTime(dateStr).AddDays(-1);
            }
            else
            {
                date = DateTime.Now.AddDays(-1);
            }
        }

        // Shift: Day   (08:30 - 17:00)
        public void SyncInDay()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.Day, userName);
        }

        public void SyncOutDay()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.Day, userName);
        }

        // Shift: Night (20:30 - 05:00)
        public void SyncInNight()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.Night, userName);
        }

        public void SyncOutNight()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.Night, userName);
        }
        // Shift: SG-A  (06:00 - 14:00)
        public void SyncInSG_A()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.SG_A, userName);
        }

        public void SyncOutSG_A()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.SG_A, userName);
        }
        // Shift: SG-B  (14:00 - 22:00)
        public void SyncInSG_B()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.SG_B, userName);
        }

        public void SyncOutSG_B()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.SG_B, userName);
        }
        // Shift: Sg-C  (22:00 - 06:00)
        public void SyncInSG_C()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.SG_C, userName);
        }

        public void SyncOutSG_C()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.SG_C, userName);
        }
        // Shift: SG-D  (08:00 - 17:00)
        public void SyncInSG_D()
        {
            setDate();
            attendanceService.SyncInTime(branchId, date, ShiftId.SG_D, userName);
        }

        public void SyncOutSG_D()
        {
            setDate();
            attendanceService.SyncOutTime(branchId, date, ShiftId.SG_D, userName);
        }

    }
}