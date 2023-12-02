using Analyzer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Models
{
    public  interface IStats
    {
        string MostLongWord { get;set; }
        string MostShortWord { get; set; }
        string MostLongSentence { get;set; }
        string MostShortSentence { get;set;}
        List<AnalyzePoint> LettersBySentences { get; set; }
        List<AnalyzePoint> WordsBySentences { get; set; }
        int SentenceAmount { get;set; }
        string ErrorMessage { get;set; }
        event Action GlobalStateChanged;
    }
}
