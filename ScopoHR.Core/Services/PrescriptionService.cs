using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ScopoHR.Core.ViewModels;

namespace ScopoHR.Core.Services
{
   public class PrescriptionService
    {
        private UnitOfWork unitOfWork;
        private Prescription prescription;

        public PrescriptionService(UnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public void Update(PrescriptionViewModel prescriptionVM)
        {
            prescription = new Prescription
            {
                PrescriptionID=prescriptionVM.PrescriptionID,
                EmployeeID = prescriptionVM.EmployeeID,
                Description = prescriptionVM.Description,
                PrescribedDate = prescriptionVM.PrescribedDate,
                Pulse=prescriptionVM.Pulse,
                Anaemia=prescriptionVM.Anaemia,
                BP=prescriptionVM.BP,
                Dehydretion=prescriptionVM.Dehydretion,
                Eyes=prescriptionVM.Eyes,
                Heart=prescriptionVM.Heart,
                Jaundice=prescriptionVM.Jaundice,
                Lungs=prescriptionVM.Lungs,
                Oeadema=prescriptionVM.Oeadema,
                Others=prescriptionVM.Others,
                PAbdomen=prescriptionVM.PAbdomen,
                PatientStatement=prescriptionVM.PatientStatement,
                Respiration=prescriptionVM.Respiration,
                Skin=prescriptionVM.Skin,
                Temperature=prescriptionVM.Skin,
                IsDeleted = false,
                LastModified = DateTime.Now,
                ModifiedBy = prescriptionVM.ModifiedBy
            };
            unitOfWork.PrescriptionRepository.Update(prescription);
            unitOfWork.Save();
        }

        public void Create(PrescriptionViewModel prescriptionVM)
        {
            prescription = new Prescription
            {
                EmployeeID= prescriptionVM.EmployeeID,
                Description=prescriptionVM.Description,
                PrescribedDate=prescriptionVM.PrescribedDate,
                Pulse = prescriptionVM.Pulse,
                Anaemia = prescriptionVM.Anaemia,
                BP = prescriptionVM.BP,
                Dehydretion = prescriptionVM.Dehydretion,
                Eyes = prescriptionVM.Eyes,
                Heart = prescriptionVM.Heart,
                Jaundice = prescriptionVM.Jaundice,
                Lungs = prescriptionVM.Lungs,
                Oeadema = prescriptionVM.Oeadema,
                Others = prescriptionVM.Others,
                PAbdomen = prescriptionVM.PAbdomen,
                PatientStatement = prescriptionVM.PatientStatement,
                Respiration = prescriptionVM.Respiration,
                Skin = prescriptionVM.Skin,
                Temperature = prescriptionVM.Skin,
                IsDeleted =false,
                LastModified=DateTime.Now,
                ModifiedBy=prescriptionVM.ModifiedBy
            };
            unitOfWork.PrescriptionRepository.Insert(prescription);
            unitOfWork.Save();
        }

        public List<PrescriptionViewModel> GetRecentPrescriptionList()
        {
            List < PrescriptionViewModel > list= (from p in unitOfWork.PrescriptionRepository.Get()
                                                  join e in unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                                              select new PrescriptionViewModel
                                              {
                                                  PrescriptionID=p.PrescriptionID,
                                                  EmployeeName=e.EmployeeName,
                                                  CardNo=e.CardNo,
                                                  EmployeeID=p.EmployeeID,
                                                  Description=p.Description,
                                                  PrescribedDate=p.PrescribedDate,
                                                  Pulse = p.Pulse,
                                                  Anaemia = p.Anaemia,
                                                  BP = p.BP,
                                                  Dehydretion = p.Dehydretion,
                                                  Eyes = p.Eyes,
                                                  Heart = p.Heart,
                                                  Jaundice = p.Jaundice,
                                                  Lungs = p.Lungs,
                                                  Oeadema = p.Oeadema,
                                                  Others = p.Others,
                                                  PAbdomen = p.PAbdomen,
                                                  PatientStatement = p.PatientStatement,
                                                  Respiration = p.Respiration,
                                                  Skin = p.Skin,
                                                  Temperature = p.Skin
                                              }).Take(50).ToList();
            return list;
        }

        public object GetPrescriptionDropDownByKeyword(string inputString, int id)
        {
            var result = (from p in unitOfWork.PrescriptionRepository.Get()
                          join e in unitOfWork.EmployeeRepository.Get() on p.EmployeeID equals e.EmployeeID
                          where (e.EmployeeName.ToLower()
                          .Contains(inputString.ToLower())
                          || e.CardNo.Contains(inputString.ToLower()))
                          //&& emp.IsActive == true
                          select new EmployeeDropDown
                          {
                              CardNo = e.CardNo,
                              EmployeeName = e.EmployeeName,
                              EmployeeID = e.EmployeeID
                          }).ToList();
            return result;
        }
    }
}
