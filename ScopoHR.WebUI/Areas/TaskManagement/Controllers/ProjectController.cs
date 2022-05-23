using ScopoHR.Core.Common;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.TaskManagement.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Task)]
    public class ProjectController : Controller
    {
        private ProjectService projectService;
        private ClientService clientService;

        public ProjectController(ProjectService projectService, ClientService clientService )
        {
            this.projectService = projectService;
            this.clientService = clientService;
        }

        public ActionResult Index()
        {
            return View();
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
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllClientList()
        {
            try
            {
                return Json(clientService.GetAllClientList(), JsonRequestBehavior.AllowGet);
            }

            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult SaveProject(ProjectViewModel projectVM)
        {
            
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (projectVM.ProjectID > 0)
                {
                    if (!projectService.IsUnique(projectVM))
                    {
                        projectService.Update(projectVM);
                        return Json(new { Message = "Project Successfully Updated" });
                    }
                    return Json(new { Message = "Project already exists." });
                   
                }
                if (projectVM.ProjectID == 0)
                {
                    try
                    {
                        if(!projectService.IsUnique(projectVM))
                        {
                            projectService.Create(projectVM);
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Message = "Project Successfully Created" });
                        }
                        return Json(new { Message = "Project already exists." });
                    }
                    catch (Exception ex)
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

        public JsonResult GetProjectDetailByID(int projectID)
        {
            try
            {
                return Json(projectService.GetProjectDetailByID(projectID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }


    }
}