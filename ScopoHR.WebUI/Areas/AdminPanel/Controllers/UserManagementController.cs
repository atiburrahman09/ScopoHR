using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using ScopoHR.Core.Common;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ScopoHR.WebUI.Areas.AdminPanel.Controllers
{
    [Authorize(Roles = AppRoles.SuperUser + ", " + AppRoles.Admin + "," + AppRoles.ManageUsers)]

    public class UserManagementController : Controller
    {
        private EmployeeService employeeService;
        private BranchService _branchService;
        private UserBranchService _userBranchService;
        private ApplicationUserService _applicationUserService;
        private ApplicationRoleService _applicationRoleService;

        private UserManager<ApplicationUser> userManager;
        private ApplicationDbContext _context;

        private RoleStore<IdentityRole> roleStore;
        private RoleManager<IdentityRole> roleManager;

        public UserManagementController(EmployeeService employeeService,
            BranchService branchService, UserBranchService userBranchService,
            ApplicationUserService applicationUserService, ApplicationRoleService applicationRoleService)
        {
            this.employeeService = employeeService;
            this._branchService = branchService;
            this._context = new ApplicationDbContext();
            this.userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(_context));
            this.roleStore =  new RoleStore<IdentityRole>(_context);
            this.roleManager = new RoleManager<IdentityRole>(roleStore);

            this._userBranchService = userBranchService;
            this._applicationUserService = applicationUserService;
            this._applicationRoleService = applicationRoleService;

        }

        // GET: AdminPanel/UserManagement        
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetUserBranches(string userName)
        {
            if (string.IsNullOrEmpty(userName.Trim()))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid User selected!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                return Json(_userBranchService.GetUserBranches(userName), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetUserRoles(string userName)
        {
            if (string.IsNullOrEmpty(userName.Trim()))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid User selected!", JsonRequestBehavior.AllowGet);
            }
            try
            {                
                return Json(_applicationUserService.GetUserRoles(userName), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }
        }

        public JsonResult GetBranchDropDown()
        {
            try
            {
                return Json(_branchService.GetBranchDropDown(), JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }
        
        public JsonResult GetAllApplicationUsers()
        {
            try
            {
                return Json(_applicationUserService.GetAllApplicationUsers(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult GetAllRoles()
        {
            try
            {               
                return Json(_applicationRoleService.GetAllRoles(), JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> CreateUser(AppUserUpdateViewModel appUserVM)
        {

            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json(false);
            }

            if (!employeeService.IsValidCardNo(appUserVM.UserName))
            {
                Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                return Json("Invalid Card No Provided!");
            }

            var user = new ApplicationUser
            {
                UserName = appUserVM.UserName
            };
            
            try
            {
                var result = await userManager.CreateAsync(user, appUserVM.Password);
                if (result.Succeeded)
                {
                    userManager.AddToRoles(user.Id, appUserVM.Roles.ToArray());
                    // add to branch
                    _userBranchService.AddToBranch(user.UserName, appUserVM.BranchIDs);
                }
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message);
            }

            return Json(true);            
        }


        [HttpPost]
        public JsonResult UpdateUser(AppUserUpdateViewModel appUserVM)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid Data Submitted!");
            }

            try
            {
                string netUserId = _applicationUserService.GetAspNetUserID(appUserVM.UserName);
                _applicationUserService.UnmapRoles(netUserId);
                _applicationUserService.UnmapBranches(appUserVM.UserName);

                if (appUserVM.Roles != null && appUserVM.Roles.Count != 0)
                {
                    userManager.AddToRoles(netUserId, appUserVM.Roles.ToArray());
                }
                if (appUserVM.BranchIDs != null && appUserVM.BranchIDs.Count != 0)
                {
                    _userBranchService.AddToBranch(appUserVM.UserName, appUserVM.BranchIDs);
                }
                return Json("User Update Successfully!");
            }
            catch(Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult CheckUserExits(string userName)
        {
            if (string.IsNullOrEmpty(userName.Trim()))
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return Json("Invalid User selected!", JsonRequestBehavior.AllowGet);
            }
            try
            {
                string status = _applicationUserService.CheckUserExists(userName);
                return Json(status, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Response.StatusCode = (int)HttpStatusCode.ExpectationFailed;
                return Json(ex.Message, JsonRequestBehavior.AllowGet);
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
    }
}