using ScopoHR.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
    public class DocumentViewModel
    {        
        public int Id { get; set; }
        public string Title { get; set; }
        public DocumentCategory Category { get; set; }
        public int EmployeeId { get; set; }        
        public string UniqueIdentifier { get; set; }
        public string Url { get; set; }
    }
}
