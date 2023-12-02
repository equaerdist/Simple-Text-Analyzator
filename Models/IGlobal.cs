using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp2.Models
{
    public interface IGlobal
    {
        string ProjectName { get; set; }
        string Section { get; set; }
       AnalyzerInfo AnalyzerInfo { get; set; }
        bool StatsActive { get; set; }
        event Action StateChanged;
    }
}
