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
    public class NoticeService
    {
        private UnitOfWork unitOfWork;
        private Notice notice;
        private PublishNotice publishNotice;
        public NoticeService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Create(NoticeViewModel NoticeVM)
        {
            notice = new Notice
            {
                NoticeTitle = NoticeVM.NoticeTitle,
                NoticeDetail = NoticeVM.NoticeDetail,
                NoticeDate = DateTime.Now,
                ModifiedBy=NoticeVM.ModifiedBy,
                LastModified=DateTime.Now,
                IsDeleted=false
            };

            unitOfWork.NoticeRepository.Insert(notice);
            unitOfWork.Save();
        }

        public void Update(NoticeViewModel NoticeVM)
        {
            notice = new Notice
            {
                NoticeID = NoticeVM.NoticeID,
                NoticeTitle = NoticeVM.NoticeTitle,
                NoticeDetail = NoticeVM.NoticeDetail,
                ModifiedBy = NoticeVM.ModifiedBy,
                LastModified = DateTime.Now,
                IsDeleted = false
            };

            unitOfWork.NoticeRepository.Update(notice);
            unitOfWork.Save();
        }

        public void Delete(int id)
        {
            notice = (
                from n in unitOfWork.NoticeRepository.Get()
                where n.NoticeID == id
                select n
                ).SingleOrDefault();

            unitOfWork.NoticeRepository.Delete(notice);
            unitOfWork.Save();
        }

        public List<NoticeViewModel> GetAll()
        {
            return (
                from n in unitOfWork.NoticeRepository.Get()
                orderby n.NoticeDate descending
                select new NoticeViewModel
                {
                    NoticeID = n.NoticeID,
                    NoticeTitle = n.NoticeTitle,
                    NoticeDetail = n.NoticeDetail                    
                }
                ).ToList();
        }

        public NoticeViewModel GetByID(int id)
        {
            return (
                from n in unitOfWork.NoticeRepository.Get()
                where n.NoticeID == id
                select new NoticeViewModel
                {
                    NoticeID = n.NoticeID,
                    NoticeTitle = n.NoticeTitle,
                    NoticeDetail = n.NoticeDetail
                }
                ).SingleOrDefault();
        }

        public List<NoticeViewModel> GetDropDownList()
        {
            return (
                from notice in unitOfWork.NoticeRepository.Get()
                orderby notice.NoticeID ascending
                select new NoticeViewModel
                {
                    NoticeID = notice.NoticeID,
                    NoticeTitle = notice.NoticeTitle
                  
                }
                ).ToList();
        }

        //public void NoticeAssign(PublishedNoticeViewModel publish)
        //{
        //    foreach(var x in publish.EmployeeList)
        //    {
        //        publishNotice = new PublishNotice
        //        {
        //            NoticeID = publish.NoticeID,
        //            EmployeeID = x
        //        };

        //        unitOfWork.publishNoticeRepository.Insert(publishNotice);
                
        //    }
        //    unitOfWork.Save();
        //}

        public object SendNotice(PublishedNoticeViewModel data, string name)
        {
            publishNotice = new PublishNotice
            {
                NoticeID = data.NoticeID,
                NoticeDetails=data.NoticeDetails,
                NoticeTitle=data.NoticeTitle,
                EmployeeID = data.EmployeeID,
                ModifiedBy = name,
                LastModified = DateTime.Now,
                IsDeleted = false

            };
            unitOfWork.publishNoticeRepository.Insert(publishNotice);
            unitOfWork.Save();

            return (true);
        }

        public List<DropOutReportViewModel> GetDropOutReport(ReportFilteringViewModel dReport, int branchId)
        {
            var query = $"EXEC GetDropOutList '" + dReport.FromDate.ToString() + "','" + dReport.Floor + "','" + dReport.ShiftId + "','" + dReport.Days + "'";

            var result = unitOfWork.attendanceRepository.SelectQuery<DropOutReportViewModel>(query).ToList();
            return result;
        }
    }
}
