using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Core.ViewModels
{
   public  class PublishedNoticeViewModel
    {
        public int PublishID { get; set; }
        public int NoticeID { get; set; }
        public int EmployeeID { get; set; }
        public string NoticeTitle { get; set; }
        public string NoticeDetails { get; set; }
    }
}
