using Microsoft.VisualBasic.FileIO;
using ScopoHR.Core.Helpers;
using ScopoHR.Core.Services;
using ScopoHR.Core.ViewModels;
using ScopoHR.Domain.Models;
using ScopoHR.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace ScopoHR.WebUI.Helpers
{
    public static class DocumentProcessorFactory
    {
        private static string getLocation(string category)
        {
            string path = ConfigurationManager.AppSettings.Get(category);
            return HostingEnvironment
                    .MapPath(path);
        }
        public static DocumentProcessor GetProcessor(DocumentCategory category, DocumentService docService)
        {
            switch (category)
            {               
                case DocumentCategory.ProfilePhoto:
                    var location = getLocation(DocumentCategory.ProfilePhoto.ToString());
                    return new ImageProcessor(location);

                case DocumentCategory.NomineePhoto:
                    var nomineeLocation = getLocation(DocumentCategory.NomineePhoto.ToString());                    
                    return new ImageProcessor(nomineeLocation);

                case DocumentCategory.FingerPrint:
                    var fingerPringLocation = getLocation(DocumentCategory.FingerPrint.ToString());
                    return new ImageProcessor(fingerPringLocation);

                case DocumentCategory.AttendanceCsvFile:
                    var csvLocation = getLocation(DocumentCategory.AttendanceCsvFile.ToString());
                    return new AttendanceCsvProcessor(csvLocation);

                case DocumentCategory.EmployeeCsvFile:
                    csvLocation = getLocation(DocumentCategory.EmployeeCsvFile.ToString());
                    return new EmployeeCsvProcessor(csvLocation);

                case DocumentCategory.CardNoMappingFile:
                    var txtLocation = getLocation(DocumentCategory.CardNoMappingFile.ToString());
                    return new CardNoMappingProcessor(txtLocation);

                case DocumentCategory.Medical:
                    var medicalLoc = getLocation(DocumentCategory.Medical.ToString());
                    return new PersistentFileProcessor(medicalLoc);

                case DocumentCategory.NationalId:
                    var nid = getLocation(DocumentCategory.NationalId.ToString());
                    return new PersistentFileProcessor(nid);

                case DocumentCategory.LeaveApplication:
                    var lapp = getLocation(DocumentCategory.LeaveApplication.ToString());
                    return new PersistentFileProcessor(lapp);


                default:
                    throw new Exception("No appropriate document processor class found!");
            }
        }
    }

    public abstract class DocumentProcessor
    {
        public abstract string Location { get; }
        
        public abstract void Process(DocumentViewModel document, string userName);
        
    }

    public class ImageProcessor : DocumentProcessor
    {        
        private DocumentService documentService;
        private UnitOfWork unitOfWork;
        private string _location;
        public ImageProcessor(string location)
        {   
            _location = location;
            unitOfWork = new UnitOfWork(new ScopoContext());
            documentService = new DocumentService(unitOfWork);
        }

        public override string Location
        {
            get { return _location; }

        }        
        public override void Process(DocumentViewModel document, string userName)
        {
            var existing = documentService.FetchAndDelete(document.EmployeeId, document.Category);
            if(existing != null)
            {
                var path = Path.Combine(Location + existing.UniqueIdentifier);
                File.Delete(path);
            }                
            documentService.Create(document, userName);
        }
    }

    public class PersistentFileProcessor : DocumentProcessor
    {
        private DocumentService documentService;
        private UnitOfWork unitOfWork;
        private string _location;
        public PersistentFileProcessor(string location)
        {
            _location = location;
            unitOfWork = new UnitOfWork(new ScopoContext());
            documentService = new DocumentService(unitOfWork);
        }

        public override string Location
        {
            get { return _location; }

        }
        public override void Process(DocumentViewModel document, string userName)
        {           
            documentService.Create(document, userName);
        }
    }
    
    public class AttendanceCsvProcessor : DocumentProcessor
    {
        private UnitOfWork unitOfWork;
        private DocumentService documentService;
        private AttendanceService attendanceService;
        private EmployeeService employeeService;
        private string _location;

        public AttendanceCsvProcessor(string location)
        {
            unitOfWork = new UnitOfWork(new ScopoContext());
            documentService = new DocumentService(unitOfWork);
            attendanceService = new AttendanceService(unitOfWork);
            employeeService = new EmployeeService(unitOfWork);
            _location = location;
        }
        public override string Location
        {
            get{ return _location; }
        }

        public override void Process(DocumentViewModel document, string userName)
        {
            var attendanceViewModel = new List<Attendance>();
            string path = Path.Combine(Location + document.UniqueIdentifier);
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;

                // Skip the row with the column names
                csvParser.ReadLine();

                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    var aVM = new Attendance();
                    string[] fields = csvParser.ReadFields();
                    //aVM.EmployeeId = Convert.ToInt32(fields[1]);
                    aVM.CardNo = fields[1];
                    bool status = true; // employeeService.IsValidCardNo(aVM.CardNo); //For Temporary Purpose
                    if (status)
                    {
                        aVM.InOutTime = Convert.ToDateTime(fields[2]);
                        aVM.ModifiedBy = userName;
                        aVM.IsDeleted = false;
                        aVM.Remarks = "CSV Upload";
                        attendanceViewModel.Add(aVM);
                    }
                    else
                    {
                        string msg = "Card No" + aVM.CardNo + "Does Not Exits";                        
                        throw new Exception(msg);
                    }
                }
            }
            attendanceService.SaveAttendanceFromCSV(attendanceViewModel);
        }
    }

    public class EmployeeCsvProcessor : DocumentProcessor
    {
        private string _location;
        private UnitOfWork unitOfWork;
        private DocumentService documentService;
        private AttendanceService attendanceService;
        private EmployeeService employeeService;
        DesignationService designationService;

        public EmployeeCsvProcessor(string location)
        {
            unitOfWork = new UnitOfWork(new ScopoContext());
            documentService = new DocumentService(unitOfWork);
            attendanceService = new AttendanceService(unitOfWork);
            employeeService = new EmployeeService(unitOfWork);
            designationService = new DesignationService(unitOfWork);
            _location = location;
        }

        public override string Location
        {
            get { return _location; }
        }

        public override void Process(DocumentViewModel document, string userName)
        {
            string path = Path.Combine(Location + document.UniqueIdentifier);
            var employeeViewModelList = new List<EmployeeViewModel>();            
            var designationViewModel = new DesignationViewModel();    
                    
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                // Skip the row with the column names
                csvParser.ReadLine();
                while (!csvParser.EndOfData)
                {
                    // Read current line fields, pointer moves to the next line.
                    EmployeeViewModel empVM = new EmployeeViewModel();
                    string[] fields = csvParser.ReadFields();
                    //aVM.EmployeeId = Convert.ToInt32(fields[1]);
                    empVM.CardNo = fields[0];
                    empVM.EmployeeName = fields[1];
                    if ((designationService.IfExitsByName(fields[2])) == 0)
                    {
                        designationViewModel.DesignationName = fields[2];
                        Designation desgObj = designationService.Create(designationViewModel);
                        empVM.DesignationID = desgObj.DesignationID;
                    }
                    else
                    {
                        empVM.DesignationID = designationService.IfExitsByName(fields[2]);
                    }
                    empVM.DepartmentID = 3;
                    empVM.BranchID = 1;
                    empVM.JoinDate = Convert.ToDateTime(fields[3]);
                    employeeViewModelList.Add(empVM);
                }
            }
            employeeService.SaveEmployeeFromCSV(employeeViewModelList);
        }
    }

    public class CardNoMappingProcessor : DocumentProcessor
    {
        private string _location;
        private UnitOfWork unitOfWork;
        private CardNoMappingViewModel cardNoMappingVM;
        private CardNoMappingService cardNoMappingService;
        public CardNoMappingProcessor(string location)
        {
            _location = location;
            unitOfWork = new UnitOfWork(new ScopoContext());
            cardNoMappingService = new CardNoMappingService(unitOfWork);
        }
        
        public override string Location
        {
            get
            {
                return _location;
            }
        }

        public override void Process(DocumentViewModel document, string userName)
        {
            List<CardNoMappingViewModel> mappingList = new List<CardNoMappingViewModel>();
            var file = File.OpenText(Path.Combine(Location + document.UniqueIdentifier));
            string line;
            while(!string.IsNullOrEmpty((line = file.ReadLine())))
            {
                var elems = line.Split('-');
                cardNoMappingVM = new CardNoMappingViewModel
                {
                    Gender = elems[elems.Length - 1],
                    GeneratedCardNo = elems[0],
                    OriginalCardNo = elems[1]
                };
                mappingList.Add(cardNoMappingVM);
            }

            file.Close();
            cardNoMappingService.CreateMapping(mappingList, userName);
        }
    }

    
};