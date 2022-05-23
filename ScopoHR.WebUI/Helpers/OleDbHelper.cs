using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ScopoHR.WebUI.Helpers
{
    public class OleDbHelper
    {
        private string source;
        private string connectionString;
        private OleDbConnection dbConnection;
        private AttendanceService attendanceService;          
        public OleDbHelper()
        {            
            try
            {
                source = ConfigurationManager.AppSettings.Get("OLEDB_Path");
                connectionString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={source}";
                dbConnection = new OleDbConnection(connectionString);
                dbConnection.Open();
                attendanceService = new AttendanceService(new UnitOfWork(new ScopoContext()));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }            
        }       
        
        public void GetAllMachines()
        {
            string query = $@"SELECT MachineNumber, IP, Port FROM Machines";
            
            OleDbCommand cmd = new OleDbCommand(query, dbConnection);

            OleDbDataReader reader = cmd.ExecuteReader();
            List<BiometricDeviceViewModel> deviceList = new List<BiometricDeviceViewModel>();

            while (reader.Read())
            {
                deviceList.Add(new BiometricDeviceViewModel
                {
                    MachineNumber = int.Parse(reader[0].ToString()),
                    IpAddress = reader[1].ToString(),
                    Port = int.Parse(reader[2].ToString())
                });
            }
            attendanceService.SaveMachinesFromAccessDB(deviceList);
        }
        
        public void PullData(DateTime date)
        {
            var lastEntryTime = attendanceService.GetLastInOutTime(date);

            if (lastEntryTime != null)
            {
                date = (DateTime)lastEntryTime;
            }
            else
            {
                //date = DateTime.Today;
            }

            OleDbCommand cmd = new OleDbCommand($@"
                            SELECT B.Badgenumber, CHECKTIME AS InOutTime FROM CHECKINOUT A
                            INNER JOIN USERINFO B ON A.USERID = B.USERID
                            WHERE  FORMAT(CHECKTIME, 'DD-MM-YYYY') = FORMAT('{date}', 'DD-MM-YYYY') AND FORMAT(CHECKTIME, 'DD-MM-YYYY hh:mm:ss') >= FORMAT('{date}', 'DD-MM-YYYY hh:mm:ss')
                            ", dbConnection);

            OleDbDataReader reader = cmd.ExecuteReader();
            List<AttendanceViewModel> attList = new List<AttendanceViewModel>();

            while (reader.Read())
            {
                attList.Add(new AttendanceViewModel
                {
                    CardNo = reader[0].ToString(),
                    InOutTime = Convert.ToDateTime(reader[1].ToString()),
                    ModifiedBy = "System"
                });
            }
            attendanceService.SaveFromAccessDB(attList, date, 1);
        } 
    }
}