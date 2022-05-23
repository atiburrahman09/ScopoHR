using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.Services
{
    public class OverTimeReportService
    {
        UnitOfWork unitOfWork;
        // Constructor 
        public OverTimeReportService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
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
        
        public List<DailyReportViewModel> GetOverTimeReport(ReportFilteringViewModel dReport, int branch)
        {
            List<DailyReportViewModel> overTimeList = unitOfWork.attendanceRepository.SelectQuery<DailyReportViewModel>("EXEC GetOvertimeReport '" + dReport.FromDate.ToString() + "','"+ dReport.ToDate.ToString() +"','" + dReport.Floor + "','" + branch + "','"+dReport.ShiftId+"'");
            return overTimeList;
        }
    }
}
