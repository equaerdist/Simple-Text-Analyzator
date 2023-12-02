using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Models;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Presenters
{
    public class NotifierPresenter
    {
        private readonly INotifier _model;

        public NotifierPresenter(INotifier model)
        {
            _model = model;
            _model.StateChanged += Present;
        }
        private void Present()
        {
            if(_model.ProgressVisible == true && _model.Progress != 100) { _model.ButtonEnabled = false; }
            if(_model.Message?.Length > 80) _model.Message = _model.Message.Substring(0, _model.Message.Length - 3) + "...";
            
        }
    }
}
