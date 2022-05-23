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
    public class ClientController : Controller
    {
        private ClientService clientService;

        public ClientController(ClientService clientService)
        {
            this.clientService = clientService;
        }


        // GET: TaskManagement/Client
        public ActionResult Index()
        {
            return View();
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
                return Json(ex.Message);
            }
        }


        [HttpPost]
        public JsonResult SaveClient(ClientViewModel clientVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }
            try
            {
                if (clientVM.ClientID > 0)
                {
                    if (!clientService.IsUnique(clientVM))
                    {
                        clientService.Update(clientVM);
                        return Json(new { Message = "Client Successfully Updated" });
                    }
                    return Json(new { Message = "Client Already Exists" });
                   
                }
                if (clientVM.ClientID == 0)
                {
                    try
                    {
                        if(!clientService.IsUnique(clientVM))
                        {
                            clientService.Create(clientVM);
                            Response.StatusCode = (int)HttpStatusCode.Created;
                            return Json(new { Message = "Client Successfully Created" });
                        }
                        return Json(new { Message = "Client Already Exists" });

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

        public JsonResult GetClientDetailByID(int clientID)
        {
            try
            {
                return Json(clientService.GetClientDetailByID(clientID), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
    }
}