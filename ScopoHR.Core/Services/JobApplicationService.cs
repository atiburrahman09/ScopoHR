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
    public class JobApplicationService
    {

        JobApplication jobapplication;
        UnitOfWork unitOfWork;
        // Constructor 
        public JobApplicationService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }
        
        public void Create(JobApplicationViewModel JobApplicationVM)
        {

            jobapplication = new JobApplication
            {
               JobApplicationId=JobApplicationVM.JobApplicationId,
               JobCircularId=JobApplicationVM.JobCircularId,
               CandidateName=JobApplicationVM.CandidateName,
               CandidateEmail=JobApplicationVM.CandidateEmail,
               CreatedDate= JobApplicationVM.CreatedDate,
               ExpectedSalary= JobApplicationVM.ExpectedSalary,
               CandidateResume= JobApplicationVM.CandidateResume

    };

            unitOfWork.jobapplicationRepository.Insert(jobapplication);
            unitOfWork.Save();
        }
        
        public void Update(JobApplicationViewModel JobApplicationVM)
        {
            jobapplication = new JobApplication
            {
                JobApplicationId = JobApplicationVM.JobApplicationId,
                JobCircularId = JobApplicationVM.JobCircularId,
                CandidateName = JobApplicationVM.CandidateName,
                CandidateEmail = JobApplicationVM.CandidateEmail,
                CreatedDate = JobApplicationVM.CreatedDate,
                ExpectedSalary = JobApplicationVM.ExpectedSalary,
                CandidateResume = JobApplicationVM.CandidateResume
            };
            unitOfWork.jobapplicationRepository.Update(jobapplication);
            unitOfWork.Save();
        }
        
        public void Delete(JobApplicationViewModel JobApplicationVM)
        {
            jobapplication = (
                from jobApplication in unitOfWork.jobapplicationRepository.Get()
                where jobApplication.JobApplicationId == JobApplicationVM.JobApplicationId
                select jobApplication
                ).SingleOrDefault();

            unitOfWork.jobapplicationRepository.Delete(jobapplication);
            unitOfWork.Save();
        }

        public List<JobApplicationViewModel> GetAll()
        {

            return (
                from jc in unitOfWork.jobapplicationRepository.Get()
                orderby jc.JobApplicationId ascending
                select new JobApplicationViewModel
                {
                    JobApplicationId = jc.JobApplicationId,
                    JobCircularId = jc.JobCircularId,
                    CandidateName = jc.CandidateName,
                    CandidateEmail = jc.CandidateEmail,
                    CreatedDate = jc.CreatedDate,
                    ExpectedSalary = jc.ExpectedSalary,
                    CandidateResume = jc.CandidateResume
                }
                ).ToList();
        }
        
        public JobApplicationViewModel GetByID(int id)
        {

            return (
                from jc in unitOfWork.jobapplicationRepository.Get()
                where jc.JobApplicationId == id
                select new JobApplicationViewModel
                {
                    JobApplicationId = jc.JobApplicationId,
                    JobCircularId = jc.JobCircularId,
                    CandidateName = jc.CandidateName,
                    CandidateEmail = jc.CandidateEmail,
                    CreatedDate = jc.CreatedDate,
                    ExpectedSalary = jc.ExpectedSalary,
                    CandidateResume = jc.CandidateResume

                }
                ).SingleOrDefault();
        }
        
    }
}
