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
    public class ClientService
    {
        private UnitOfWork unitOfWork;
        private Client client;

        public ClientService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public List<ClientViewModel> GetAllClientList()
        {
            return (from c in unitOfWork.ClientRepository.Get()
                    select new ClientViewModel
                    {
                        ClientID = c.ClientID,
                        ClientName = c.ClientName
                    }).ToList();
        }

        public void Update(ClientViewModel clientVM)
        {
            client = new Client
            {
                ClientID=clientVM.ClientID,
                ClientName = clientVM.ClientName,
                MobileNo = clientVM.MobileNo,
                Email = clientVM.Email,
                Website = clientVM.Website
            };
            unitOfWork.ClientRepository.Update(client);
            unitOfWork.Save();
        }

        public void Create(ClientViewModel clientVM)
        {
            client = new Client
            {
                ClientName = clientVM.ClientName,
                MobileNo= clientVM.MobileNo,
                Email=clientVM.Email,
                Website=clientVM.Website
            };
            unitOfWork.ClientRepository.Insert(client);
            unitOfWork.Save();
        }

        public ClientViewModel GetClientDetailByID(int clientID)
        {
            return (from c in unitOfWork.ClientRepository.Get()
                    where c.ClientID == clientID
                    select new ClientViewModel
                    {
                        ClientID = c.ClientID,
                        ClientName = c.ClientName,
                        MobileNo = c.MobileNo,
                        Email = c.Email,
                        Website = c.Website
                    }).SingleOrDefault();
        }

        public bool IsUnique(ClientViewModel clientVM)
        {
            IQueryable<int> result;

            if (clientVM.ClientID == 0)
            {
                result = (from c in unitOfWork.ClientRepository.Get()
                          where c.ClientName.ToLower().Trim() == clientVM.ClientName.Trim().ToLower()
                          select c.ClientID);
            }
            else
            {
                result = (from c in unitOfWork.ClientRepository.Get()
                          where c.ClientName.ToLower().Trim() == clientVM.ClientName.ToLower().Trim() && c.ClientID != clientVM.ClientID
                          select c.ClientID);
            }

            if (result.Count() > 0)
            {
                return true;
            }
            return false;
        }
    }
}
