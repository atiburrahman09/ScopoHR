using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ScopoHR.WebUI.Areas.TaskManagement.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.Task)]
    public class MailController : Controller
    {
        private MailHelpers mailHelper;

        public MailController(MailHelpers mailHelper)
        {
            this.mailHelper = mailHelper;
        }
        // GET: TaskManagement/Mail
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult SendMail()
        {
            mailHelper.SendMail();
            return Json(true);
        }
    }
}