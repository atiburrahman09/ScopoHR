using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using ScopoHR.Web.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.TaskManagement.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Task)]
    [LoginAudit]
    public class TasksController : Controller
    {
        private TaskService taskService;
        private EmployeeService employeeService;
        private ProjectService projectService;

        public TasksController(TaskService taskService, EmployeeService employeeService, ProjectService projectService)
        {
            this.taskService = taskService;
            this.employeeService = employeeService;
            this.projectService = projectService;  
        }

        // GET: TaskManagement/Tasks
        public ActionResult Index()
        {            
            return View();
        }       

        public JsonResult GetAllTasks()
        {

            if (!employeeService.IsValidCardNo(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Non-Employee User Cannot Create Task!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                var owner = employeeService.GetEmployeeID(User.Identity.Name);
                return Json(taskService.GetAllTask(owner, User.Identity.Name), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetTaskCounts()
        {

            if (!employeeService.IsValidCardNo(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Non-Employee User Cannot Create Task!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                var owner = employeeService.GetEmployeeID(User.Identity.Name);
                return Json(taskService.GetTaskCounts(owner), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveTask(TaskViewModel taskVM)
        {

            if (!employeeService.IsValidCardNo(User.Identity.Name))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Non-Employee User Cannot Create Task!");
            }

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }

            if(taskVM.ActualStartDate != null && taskVM.ActualEndDate != null && (taskVM.ActualStartDate > taskVM.ActualEndDate))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("You can't end the task before you even start!");
            }

            try
            {
                taskVM.ModifiedBy = User.Identity.Name;                
                if (taskVM.TaskID > 0)
                {                    
                    if(taskVM.ActualEndDate != null && taskVM.ActualStartDate != null && taskVM.ActualStartDate <= taskVM.ActualEndDate)
                    {
                        taskVM.Status = 2;
                    }
                    else if(taskVM.ActualEndDate == null && taskVM.ActualStartDate != null)
                    {
                        taskVM.Status = 1;
                    }
                    return Json(new { Message = "Task Successfully Updated", Data = taskService.Update(taskVM) });
                }
                if (taskVM.TaskID == 0)
                {
                    try
                    {
                        taskVM.Owner = employeeService.GetEmployeeID(User.Identity.Name);
                        var data = taskService.Create(taskVM);
                        Response.StatusCode = (int)HttpStatusCode.Created;
                        return Json(new { Message = "Task Successfully Created", Data = data });
                    }
                    catch(Exception ex)
                    {
                        Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                        return Json(ex.Message);
                    }
                }
                else
                {
                    Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                    return Json("Data has been violated");
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
        
        public JsonResult GetEmployeeDropDownByKeyword(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Please enter employee name or card no.", JsonRequestBehavior.AllowGet);
            }

            try
            {
                var id = UserHelper.Instance.Get().BranchId;
                return Json(employeeService.GetEmployeeDropDownByKeyword(inputString, id), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllProjectList()
        {
            try
            {
                return Json(projectService.GetAllProjectList(), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }
    }
}