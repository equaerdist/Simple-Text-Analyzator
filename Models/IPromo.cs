﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Models
{
     public interface IPromo
    {
        IEnumerable<Analyzer.AnalyzerInfo> RecentProjects { get; set; }
    }
}
