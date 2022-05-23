using NtitasCommon.Core.Common;
using ScopoHR.Core.Helpers;
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
    public class DocumentService
    {
        private Document document;
        private UnitOfWork unitOfWork;        
        public DocumentService(UnitOfWork _unitOfWork)
        {
            this.unitOfWork = _unitOfWork;            
        }

        public void Create(DocumentViewModel docVM, string userName)
        {                        
            document = new Document
            {
                EmployeeId = docVM.EmployeeId,
                Title = docVM.Title,
                UniqueIdentifier = docVM.UniqueIdentifier,
                Category = (short)docVM.Category,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = userName,
                Id=docVM.Id
            };
            unitOfWork.DocumentRepository.Insert(document);
            unitOfWork.Save();
        }

        public DocumentViewModel GetImage(int id, DocumentCategory category)
        {
            return (from d in unitOfWork.DocumentRepository.Get()
             where d.EmployeeId == id && d.Category == (int)category
             select new DocumentViewModel
             {
                 Category = (DocumentCategory)d.Category,
                 EmployeeId = d.EmployeeId,
                 Id = d.Id,
                 Title = d.Title,
                 UniqueIdentifier = d.UniqueIdentifier                 
             }).SingleOrDefault();
        }

        public DocumentViewModel FetchAndDelete(int id, DocumentCategory category)
        {
            var res =  (from d in unitOfWork.DocumentRepository.Get()
                    where d.EmployeeId == id && d.Category == (int)category
                    select new DocumentViewModel
                    {
                        Category = (DocumentCategory)d.Category,
                        EmployeeId = d.EmployeeId,
                        Id = d.Id,
                        Title = d.Title,
                        UniqueIdentifier = d.UniqueIdentifier
                    }).SingleOrDefault();

            if (res != null)
                DeleteImage(res.Id);
            return res;
        }

        public void DeleteImage(int id)
        {
            unitOfWork.DocumentRepository.Delete(unitOfWork.DocumentRepository.GetById(id));
            unitOfWork.Save();
        }

        public List<DocumentViewModel> GetEmployeeDocumentsById(int id)
        {
            var res = (from d in unitOfWork.DocumentRepository.Get()
                       where d.EmployeeId == id
                       select new DocumentViewModel
                       {
                           Category = (DocumentCategory)d.Category,
                           EmployeeId = d.EmployeeId,
                           Id = d.Id,
                           Title = d.Title,
                           UniqueIdentifier = d.UniqueIdentifier                           
                       }).ToList();
            return res;
        }

        public IEnumerable<DropDownViewModel> GetDocumentTypes()
        {
            //int[] notAllowed = new int[] { 1, 2, 3, 5, 6, 7 };
            
            var data = 
            unitOfWork.DocumentTypesRepository.Get()
                //.Where(x => !notAllowed.Contains(x.Id))
                .Select(x => new DropDownViewModel
                {
                    Text = x.Name,
                    Value = x.Id
                })
                .AsEnumerable(); 
            return data;
        }
    }
}
