using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ScopoHR.WebUI.Helpers
{
    public class BiometricSyncService
    {
        private AttendanceService attendanceService;
        string dwEnrollNumber1 = "";
        int dwVerifyMode = 0;
        int dwInOutMode = 0;
        int dwYear = 0;
        int dwMonth = 0;
        int dwDay = 0;
        int dwHour = 0;
        int dwMinute = 0;
        int dwSecond = 0;
        int dwWorkCode = 0;
        int machineNumber = 1;
        private BiometricDeviceService deviceService;
        private List<AttendanceViewModel> attendanceList;        
        public BiometricSyncService()
        {
            attendanceService = new AttendanceService(new UnitOfWork(new ScopoContext()));
            deviceService = BiometricDeviceService.Instance();
            attendanceList = new List<AttendanceViewModel>();            
        }
        public void Sync()
        {
            var deviceList = attendanceService.GetBiometricDevices();
            foreach(var device in deviceList)
            {
                try
                {
                    if (deviceService.Connect_Net(device.IpAddress, device.Port))
                    {
                        deviceService.EnableDevice(device.MachineNumber, true);
                        deviceService.EnableDevice(device.MachineNumber, false);
                        deviceService.ReadAllGLogData(device.MachineNumber);

                        while (deviceService.SSR_GetGeneralLogData(machineNumber, out dwEnrollNumber1, out dwVerifyMode, out dwInOutMode, out dwYear, out dwMonth, out dwDay, out dwHour, out dwMinute, out dwSecond, ref dwWorkCode))
                        {
                            DateTime inputDateTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond);

                            if (device.LastSync != null)
                            {
                                if (inputDateTime >= device.LastSync)
                                {
                                    attendanceList.Add
                                    (
                                        new AttendanceViewModel
                                        {
                                            InOutTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond),
                                            CardNo = dwEnrollNumber1
                                        }
                                    );
                                }
                            }
                            else
                            {
                                attendanceList.Add(new AttendanceViewModel
                                {
                                    InOutTime = new DateTime(dwYear, dwMonth, dwDay, dwHour, dwMinute, dwSecond),
                                    CardNo = dwEnrollNumber1
                                });
                            }
                        }

                        attendanceService.SaveFromAccessDB(attendanceList, DateTime.Now, device.Id);
                        deviceService.EnableDevice(device.MachineNumber, true);
                        deviceService.Disconnect();
                    }                    
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message, ex.InnerException);
                }
            }   
            
        }
    }
}