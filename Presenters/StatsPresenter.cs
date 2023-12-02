using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApp2.Models;

namespace WindowsFormsApp2.Presenters
{
    public class StatsPresenter
    {
        private readonly IStats _model;

        public StatsPresenter(IStats model) { _model = model; _model.GlobalStateChanged += Present; }
        private string CutInformation(string info, int neeededLength)
        {
            return info.Substring(0, neeededLength - 3) + "...";
        }
        private void Present()
        {
            const int neededLength = 10;
            if(_model.MostLongSentence?.Length > neededLength)
            _model.MostLongSentence = CutInformation(_model.MostLongSentence, neededLength);
            if(_model.MostShortSentence?.Length > neededLength)
            _model.MostShortSentence = CutInformation(_model.MostShortSentence, neededLength);
            if(_model.MostShortWord?.Length > neededLength)
            _model.MostShortWord = CutInformation(_model.MostShortWord, neededLength);
            if(_model.MostLongWord?.Length > neededLength)
            _model.MostLongWord = CutInformation(_model.MostLongWord, neededLength);
        }
    }
}
