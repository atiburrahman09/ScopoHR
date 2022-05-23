using ScopoHR.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class WorkingShiftViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime InTime { get; set; }
        public DateTime OutTime { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? LastModified { get; set; }
        public bool? IsDeleted { get; set; }


        public static WorkingShiftViewModel EntityToVM(WorkingShift model)
        {
            return new WorkingShiftViewModel
            {
                Id = model.Id,
                Name = model.Name,
                InTime = model.InTime,
                OutTime = model.OutTime
            };
        }

        public WorkingShift ToEntity()
        {
            return new WorkingShift
            {
                Id = this.Id,
                Name = this.Name,
                InTime = this.InTime,
                OutTime = this.OutTime,
                ModifiedBy = this.ModifiedBy,
                LastModified = this.LastModified,
                IsDeleted = this.IsDeleted
            };
        }
    }
}
