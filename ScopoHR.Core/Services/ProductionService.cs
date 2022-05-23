using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Repositories;
using ScopoHR.Domain.Models;

namespace ScopoHR.Core.Services
{
    public class ProductionService
    {
        private UnitOfWork unitOfWork;
        private ProductionFloorLine production;

        public ProductionService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public ProductionFloorLine Create(ProductionFloorLineViewModel productionVM)
        {
            production = new ProductionFloorLine
            {
                Floor = productionVM.Floor,
                Line = productionVM.Line,
                ModifiedBy = productionVM.ModifiedBy,
                IsDeleted = false
            };
            unitOfWork.productionFloorLineRepository.Insert(production);
            unitOfWork.Save();
            return production;
        }

        public object GetAll()
        {
            return (
               from pfl in unitOfWork.productionFloorLineRepository.Get()
               where pfl.IsDeleted==false
               orderby pfl.ProductionFloorLineID ascending
               select new ProductionFloorLineViewModel
               {
                   ProductionFloorLineID = pfl.ProductionFloorLineID,
                   Floor = pfl.Floor,
                   Line = pfl.Line
               }
               ).ToList();
        }

        public void Update(ProductionFloorLineViewModel productionVM)
        {
            production = new ProductionFloorLine
            {
                ProductionFloorLineID = productionVM.ProductionFloorLineID,
                Floor = productionVM.Floor,
                Line = productionVM.Line,
                ModifiedBy = productionVM.ModifiedBy,
                IsDeleted = false
            };
            unitOfWork.productionFloorLineRepository.Update(production);
            unitOfWork.Save();
        }

        public bool IsProductionExists(ProductionFloorLineViewModel productionVM)
        {

            IQueryable<int> result;

            if (productionVM.ProductionFloorLineID == 0)
            {
                result = from p in unitOfWork.productionFloorLineRepository.Get()
                         where p.Floor == productionVM.Floor &&
                                p.Line == productionVM.Line
                         select p.ProductionFloorLineID;
            }
            else
            {
                result = from p in unitOfWork.productionFloorLineRepository.Get()
                         where p.Floor == productionVM.Floor &&
                        p.Line == productionVM.Line && p.ProductionFloorLineID != productionVM.ProductionFloorLineID
                         select p.ProductionFloorLineID;
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }

        public List<ProductionFloorLineViewModel> GetProductionFloorList()
        {
            return (
                from pfl in unitOfWork.productionFloorLineRepository.Get()
                where pfl.IsDeleted==false
                orderby pfl.ProductionFloorLineID ascending
                select new ProductionFloorLineViewModel
                {
                    Floor = pfl.Floor,
                    ProductionFloorLineID = pfl.ProductionFloorLineID
                }
                ).GroupBy(x => x.Floor).Select(x => x.FirstOrDefault()).ToList();
        }

        public List<ProductionFloorLineViewModel> GetProductionLineList(string floor)
        {
            return (
              from pfl in unitOfWork.productionFloorLineRepository.Get()
              orderby pfl.ProductionFloorLineID ascending
              where pfl.Floor == floor
              select new ProductionFloorLineViewModel
              {
                  Line = pfl.Line
              }
              ).ToList();
        }

        public List<ProductionFloorLineViewModel> GetProductionLine()
        {
            return (
             from pfl in unitOfWork.productionFloorLineRepository.Get()
             orderby pfl.ProductionFloorLineID ascending
             select new ProductionFloorLineViewModel
             {
                 Line = pfl.Line
             }
             ).ToList();

        }
    }
}
