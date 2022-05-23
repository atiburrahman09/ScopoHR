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
    public class TaskService
    {
        private UnitOfWork unitOfWork;
        private Tasks task;


        public TaskService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public TaskViewModel Create(TaskViewModel taskVM)
        {
            task = new Tasks
            {
                TaskTitle = taskVM.TaskTitle,
                IsDeleted = false,
                Status = taskVM.Status,
                EntryDate = DateTime.Now,
                AssignedTo = taskVM.AssigneeID.HasValue ? taskVM.AssigneeID : (int?)null,
                Description = taskVM.Description,
                AttachmentPath = taskVM.AttachmentPath,
                LastModified = DateTime.Now,
                ModifiedBy = taskVM.ModifiedBy,
                Owner = taskVM.Owner,
                PlannedEndDate = taskVM.PlannedEndDate,
                PlannedManHour = taskVM.PlannedManHour,
                PlannedStartDate = taskVM.PlannedStartDate,
                Priority = taskVM.Priority,
                ProjectID = taskVM.ProjectID

            };
            unitOfWork.TaskRepository.Insert(task);
            unitOfWork.Save();
            return GetTaskById(task.TaskID);
        }

        public TaskViewModel Update(TaskViewModel taskVM)
        {
            task = new Tasks
            {
                TaskID = taskVM.TaskID,
                ActualEndDate = taskVM.ActualEndDate.HasValue ? taskVM.ActualEndDate : (DateTime?)null,
                Description = taskVM.Description,
                TaskTitle = taskVM.TaskTitle,
                EntryDate = taskVM.EntryDate,
                ActualManHour = taskVM.ActualManHour.HasValue ? taskVM.ActualManHour : (int?)null,
                ActualStartDate = taskVM.ActualStartDate.HasValue ? taskVM.ActualStartDate : (DateTime?)null,
                AssignedTo = taskVM.AssigneeID.HasValue ? taskVM.AssigneeID : (int?)null,
                AttachmentPath = taskVM.AttachmentPath,
                Owner = taskVM.Owner,
                PlannedEndDate = taskVM.PlannedEndDate,
                PlannedManHour = taskVM.PlannedManHour,
                PlannedStartDate = taskVM.PlannedStartDate,
                Priority = taskVM.Priority,
                Status = taskVM.Status,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = taskVM.ModifiedBy,
                ProjectID = taskVM.ProjectID
            };
            unitOfWork.TaskRepository.Update(task);
            unitOfWork.Save();
            return taskVM;
        }

        public List<TaskViewModel> GetAllTask(int ownerID, string ownerCardNo)
        {
            return (from s in unitOfWork.TaskRepository.Get()
                    join employee in unitOfWork.EmployeeRepository.Get()
                    on s.AssignedTo equals employee.EmployeeID into employee_group
                    from emp in employee_group.DefaultIfEmpty()

                    join owner in unitOfWork.EmployeeRepository.Get()
                    on s.Owner equals owner.EmployeeID

                    where s.Owner == ownerID || s.AssignedTo == ownerID
                    select new TaskViewModel
                    {
                        TaskID = s.TaskID,
                        TaskTitle = s.TaskTitle,
                        Description = s.Description,
                        PlannedStartDate = s.PlannedStartDate,
                        PlannedEndDate = s.PlannedEndDate,
                        ActualStartDate = s.ActualStartDate,
                        ActualEndDate = s.ActualEndDate,
                        Status = s.Status,
                        AttachmentPath = s.AttachmentPath,
                        Owner = s.Owner,
                        OwnerCardNo = owner.CardNo,
                        OwnerName = owner.EmployeeName,
                        EntryDate = s.EntryDate,
                        PlannedManHour = s.PlannedManHour,
                        Priority = s.Priority,
                        ActualManHour = s.ActualManHour,
                        AssigneeID = s.AssignedTo,
                        AssigneeName = emp != null ? emp.EmployeeName : null,
                        AssigneeCardNo = emp != null ? emp.CardNo : null,
                        ProjectID=s.ProjectID
                    }).ToList();
        }

        public TaskViewModel GetTaskById(int taskId)
        {
            return (from s in unitOfWork.TaskRepository.Get()
                    join employee in unitOfWork.EmployeeRepository.Get()
                    on s.AssignedTo equals employee.EmployeeID into employee_group
                    from emp in employee_group.DefaultIfEmpty()

                    join owner in unitOfWork.EmployeeRepository.Get()
                    on s.Owner equals owner.EmployeeID
                    where s.TaskID == taskId
                    select new TaskViewModel
                    {
                        TaskID = s.TaskID,
                        TaskTitle = s.TaskTitle,
                        Description = s.Description,
                        PlannedStartDate = s.PlannedStartDate,
                        PlannedEndDate = s.PlannedEndDate,
                        ActualStartDate = s.ActualStartDate,
                        ActualEndDate = s.ActualEndDate,
                        Status = s.Status,
                        AttachmentPath = s.AttachmentPath,
                        Owner = s.Owner,
                        OwnerCardNo = owner.CardNo,
                        OwnerName = owner.EmployeeName,
                        EntryDate = s.EntryDate,
                        PlannedManHour = s.PlannedManHour,
                        Priority = s.Priority,
                        ActualManHour = s.ActualManHour,
                        AssigneeID = s.AssignedTo,
                        AssigneeName = emp != null ? emp.EmployeeName : null,
                        AssigneeCardNo = emp != null ? emp.CardNo : null
                    }).SingleOrDefault();
        }

        public Dictionary<string, int> GetTaskCounts(int taskOwner)
        {
            var date_today = DateTime.Now.Date;

            Dictionary<string, int> counts = new Dictionary<string, int>();

            // all without completed
            var all = (from t in unitOfWork.TaskRepository.Get()
                       where t.Status < 2 && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                       select t.TaskID).ToList();
            counts.Add("AllTasks", all.Count);


            // completed
            var completed = (from t in unitOfWork.TaskRepository.Get()
                             where t.Status == 2 && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                             select t.TaskID).ToList();

            counts.Add("Completed", completed.Count);

            // today 
            var today = (from t in unitOfWork.TaskRepository.Get()
                         where t.Status < 2 &&
                         (DbFunctions.TruncateTime(t.ActualStartDate) <= date_today && DbFunctions.TruncateTime(t.PlannedEndDate) >= date_today)
                         && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                         select t.TaskID
                         ).ToList();
            counts.Add("Today", today.Count);

            // overdue
            var overdue = (from t in unitOfWork.TaskRepository.Get()
                           where t.Status < 2 && DbFunctions.TruncateTime(t.PlannedEndDate) < date_today
                            && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                           select t.TaskID).ToList();

            counts.Add("Overdue", overdue.Count);

            // priority Low
            var priorityL = (from t in unitOfWork.TaskRepository.Get()
                             where t.Status < 2 && t.Priority == 0
                             && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                             select t.TaskID).ToList();

            counts.Add("PriorityL", priorityL.Count);

            // priority Medium
            var priorityM = (from t in unitOfWork.TaskRepository.Get()
                             where t.Status < 2 && t.Priority == 1
                             && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                             select t.TaskID).ToList();

            counts.Add("PriorityM", priorityM.Count);

            //priority High
            var priorityH = (from t in unitOfWork.TaskRepository.Get()
                             where t.Status < 2 && t.Priority == 2
                             && (t.Owner == taskOwner || t.AssignedTo == taskOwner)
                             select t.TaskID).ToList();
            counts.Add("PriorityH", priorityH.Count);

            return counts;
        }

    }
}
