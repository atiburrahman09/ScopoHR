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
    public class ManpowerPlanningService
    {
        private ManpowerPlanning manpowerPlanning;
        private UnitOfWork unitOfWork;

        public ManpowerPlanningService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        
    }
}
