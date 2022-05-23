using ScopoHR.Domain.Repositories;
using ScopoHR.Domain.Models;
using ScopoHR.Core.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ScopoHR.Core.Services;
using ScopoHR.Core.Common;

namespace ScopoHR.WebUI.Areas.ManpowerPlanning.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser)]
    public class ManpowerPlanningController : Controller
    {
        private UnitOfWork unitOfWork;
        private DepartmentService departmentService;
        private DesignationService designationService;
        private ManpowerPlanningService manpowerPlanningService;
        private int currentYear;

        public ManpowerPlanningController()
        {
            unitOfWork = new UnitOfWork(new ScopoContext());
            departmentService = new DepartmentService(unitOfWork);
            designationService = new DesignationService(unitOfWork);
            manpowerPlanningService = new ManpowerPlanningService(unitOfWork);
            currentYear = DateTime.Today.Year;
        }
        // GET: ManpowerPlanning/ManpowerPlanning
        public ActionResult Index()
        {
            return View();
        }


    }
}