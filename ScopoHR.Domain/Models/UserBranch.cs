﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScopoHR.Domain.Models
{
    public class UserBranch : BaseEntity
    {
        public int UserBranchID { get; set; }
        public string UserID { get; set; }
        public int BranchID { get; set; }
    }
}
