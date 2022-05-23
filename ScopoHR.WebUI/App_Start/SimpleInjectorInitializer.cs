[assembly: WebActivator.PostApplicationStartMethod(typeof(ScopoHR.WebUI.App_Start.SimpleInjectorInitializer), "Initialize")]

namespace ScopoHR.WebUI.App_Start
{
    using System.Reflection;
    using System.Web.Mvc;

    using SimpleInjector;
    using SimpleInjector.Extensions;
    using SimpleInjector.Integration.Web;
    using SimpleInjector.Integration.Web.Mvc;
    using Core.Services;
    using Domain.Repositories;
    using Domain.Models;
    using Microsoft.AspNet.Identity;
    using Models;
    using NtitasCommon.Core.Interfaces;
    using Core.Helpers;
    using NtitasCommon.Core.Common;

    public static class SimpleInjectorInitializer
    {
        /// <summary>Initialize the container and register it as MVC3 Dependency Resolver.</summary>
        public static void Initialize()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new WebRequestLifestyle();
            
            InitializeContainer(container);

            //container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            
            container.Verify();
            
            DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(container));
        }
     
        private static void InitializeContainer(Container container)
        {
            container.Register<ScopoContext>(Lifestyle.Scoped);
            container.Register<UnitOfWork>();
            container.Register<LeaveApplicationService>();
            container.Register<DepartmentService>();
            container.Register<DesignationService>();
            container.Register<LeaveMappingService>();
            container.Register<NoticeService>();
            container.Register<ManpowerPlanningService>();
            container.Register<LeaveTypeService>();
            container.Register<SalaryMappingService>();
            container.Register<DocumentService>();
            container.Register<EmployeeService>();
            container.Register<SalaryTypeService>();
            container.Register<SecurityGuardRosterService>();
            container.Register<TaskService>();
            container.Register<IUserHelper, UserHelper>();
            container.Register<IUserLoginAuditService, UserLoginAuditService>();
            container.Register<ICookieAccessor, CookieAccessor>();
            container.Register<IConfig, Config>();            
            UserHelper.Instance = container.GetInstance<IUserHelper>();

        }
    }
}