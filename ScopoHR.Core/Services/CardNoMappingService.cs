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
    public class CardNoMappingService
    {
        private UnitOfWork unitOfWork;
        private CardNoMapping mapping;        
        
        public CardNoMappingService(UnitOfWork u)
        {
            unitOfWork = u;
        }

        public void CreateMapping(List<CardNoMappingViewModel> mappingList, string userName)
        {
            foreach(var item in mappingList)
            {
                mapping = new CardNoMapping
                {
                    Gender = item.Gender,
                    GeneratedCardNo = item.GeneratedCardNo,
                    OriginalCardNo = item.OriginalCardNo,
                    LastModified = DateTime.Now,
                    ModifiedBy = userName,
                    IsDeleted = false
                };
                unitOfWork.CardNoMappingRepository.Insert(mapping);
            }
            unitOfWork.Save();
        }
    }
}
