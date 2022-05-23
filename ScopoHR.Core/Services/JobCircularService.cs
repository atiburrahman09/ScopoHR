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
    public class JobCircularService
    {
        JobCircular jobcircular;
        UnitOfWork unitOfWork;
        // Constructor 
        public JobCircularService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }



        public void Create(JobCircularViewModel JobCircularVM)
        {

            jobcircular = new JobCircular
            {
               JobCircularId=JobCircularVM.JobCircularId,
               JobTitle=JobCircularVM.JobTitle,
               JobDescription=JobCircularVM.JobDescription,
               DueDate=JobCircularVM.DueDate,
               CreatedBy=JobCircularVM.CreatedBy,
               CreatedDate=JobCircularVM.CreatedDate

            };

            unitOfWork.jobcircularRepository.Insert(jobcircular);
            unitOfWork.Save();
        }


        public void Update(JobCircularViewModel JobCircularVM)
        {
            jobcircular = new JobCircular
            {
              JobCircularId=JobCircularVM.JobCircularId,
              JobTitle=JobCircularVM.JobTitle,
              JobDescription=JobCircularVM.JobDescription,
              DueDate=JobCircularVM.DueDate,
              CreatedBy = JobCircularVM.CreatedBy,
              CreatedDate = JobCircularVM.CreatedDate

            };
            unitOfWork.jobcircularRepository.Update(jobcircular);
            unitOfWork.Save();
        }



       

        public void Delete(int id)
        {
            jobcircular = (
                from jc in unitOfWork.jobcircularRepository.Get()
                where jc.JobCircularId == id
                select jc
                ).SingleOrDefault();

            unitOfWork.jobcircularRepository.Delete(jobcircular);
            unitOfWork.Save();
        }


        public List<JobCircularViewModel> GetAll()
        {

            return (
                from jc in unitOfWork.jobcircularRepository.Get()
                orderby jc.JobCircularId ascending
                select new JobCircularViewModel
                {
                    JobCircularId = jc.JobCircularId,
                    JobTitle = jc.JobTitle,
                    JobDescription = jc.JobDescription,
                    DueDate = jc.DueDate,
                    CreatedBy = jc.CreatedBy,
                    CreatedDate = jc.CreatedDate
                }
                ).ToList();
        }


        public JobCircularViewModel GetByID(int id)
        {

            return (
                from jc in unitOfWork.jobcircularRepository.Get()
                where jc.JobCircularId == id
                select new JobCircularViewModel
                {
                    JobCircularId = jc.JobCircularId,
                    JobTitle=jc.JobTitle,
                    JobDescription = jc.JobDescription,
                    DueDate=jc.DueDate,
                    CreatedBy=jc.CreatedBy,
                    CreatedDate=jc.CreatedDate

                }
                ).SingleOrDefault();
        }
    }
}
