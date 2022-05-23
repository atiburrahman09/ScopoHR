using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System.Data.SqlClient;

namespace ScopoHR.Core.Services
{
    public class DailyReportService
    {
        UnitOfWork unitOfWork;
        // Constructor 
        public DailyReportService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<DepartmentViewModel> GetAll()
        {

            return (
                    from dep in unitOfWork.DepartmentRepository.Get()
                    orderby dep.DepartmentName ascending
                    select new DepartmentViewModel
                    { DepartmentID = dep.DepartmentID, DepartmentName = dep.DepartmentName }
                    ).ToList();
        }

        public List<ProductionFloorLineViewModel> GetProductionFloorList()
        {
            return (
                from pfl in unitOfWork.productionFloorLineRepository.Get()
                where pfl.IsDeleted==false
                orderby pfl.ProductionFloorLineID ascending
                select new ProductionFloorLineViewModel
                {
                    Floor = pfl.Floor
                }
                ).Distinct().ToList();
        }

        // move this one to floorline service or something like that
        public List<ProductionFloorLineViewModel> GetProductionLineList(string floor)
        {
            return (
              from pfl in unitOfWork.productionFloorLineRepository.Get()
              orderby pfl.ProductionFloorLineID ascending
              where pfl.Floor == floor
              select new ProductionFloorLineViewModel
              {
                  Line = pfl.Line
              }
              ).ToList();

        }

        //Shift Id will add in the proccedure
        public List<DailyReportViewModel> GetDailyReportByDateFloor(DailyReportFilteringViewModel dReport,int branch)
        {
            List<DailyReportViewModel> attendanceList = unitOfWork.attendanceRepository.SelectQuery<DailyReportViewModel>("EXEC GetAttendanceData '"+dReport.FromDate.ToString()+"','"+dReport.FromDate.ToString()+"','"+dReport.FloorID+"','"+branch+"','"+dReport.ShiftId+"','"+"'");
            return attendanceList;
        }

        // move this one to floorline service or something like that
        public List<ProductionFloorLineViewModel> GetProductionLine()
        {
            return (
             from pfl in unitOfWork.productionFloorLineRepository.Get()
             orderby pfl.ProductionFloorLineID ascending
             select new ProductionFloorLineViewModel
             {
                 Line = pfl.Line
             }
             ).ToList();

        }       
    }
}
