using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2.Presenters
{
    public class GlobalPresenter
    {
        IGlobal _model;
        public GlobalPresenter(IGlobal globalModel) { _model = globalModel; _model.StateChanged += CheckState; }
        private void CheckState()
        {
            if(_model.ProjectName != string.Empty) _model.StatsActive = true;
        }
    }
}
