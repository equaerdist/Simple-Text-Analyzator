using Analyzer;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using WindowsFormsApp2.Models;
using WindowsFormsApp2.Presenters;
using WindowsFormsApp2.Properties;
using WindowsFormsApp2.Views;

namespace WindowsFormsApp2
{
    public partial class Stats : Form, IStats
    {
        private Form _notifier;
        private StatsPresenter _presenter;
        private Action<AnalyzerInfo> _setInfo;
        public Stats(FileStream fs, AnalyzerInfo info, Action<AnalyzerInfo> setInfo)
        {
            InitializeComponent();
            _presenter = new StatsPresenter(this);
            ErrorMessage = "Start analyze text...";
            _setInfo = setInfo;
            if (fs != null)
                Initialize(fs);
           
            if (info != null)
                ApplyExistingData(info);
            if (fs == null && info == null)
                throw new ArgumentNullException();
        }
        private void BindData(IAnalyzeInfoContainer analyzer)
        {
            MostLongWord = analyzer.MostLongWord;
            MostShortWord = analyzer.MostShortWord;
            MostShortSentence = analyzer.MostShortSentence;
            MostLongSentence = analyzer.MostLongSentence;
            SentenceAmount = analyzer.SentencesAmount;
            LettersBySentences = analyzer.LettersBySentences.OrderBy(t => t.Y).ToList();
            WordsBySentences = analyzer.WordsBySentences.OrderBy(t => t.Y).ToList();
            chart1.DataSource = WordsBySentences;
            chart1.Series[0].XValueMember = "Y";
            chart1.Series[0].YValueMembers = "X";
            chart1.DataBind();
            chart3.DataSource = LettersBySentences;
            chart3.Series[0].XValueMember = "Y";
            chart3.Series[0].YValueMembers = "X";
            chart3.DataBind();
        }
        private void ApplyExistingData(IAnalyzeInfoContainer analyzer)
        {
            BindData(analyzer);
        }
        private async void SaveProject(AnalyzerInfo analyzer)
        {
            var fileName = AppDomain.CurrentDomain.BaseDirectory + DateTime.Now.ToString("dd-MM-yyyy-hh-mm-ss") + ".json";
            analyzer.Filename = fileName;
            var fs = new FileStream(Path.Combine(fileName), FileMode.Create);
            var bytedInfo = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(analyzer, Formatting.Indented));
            try
            {
                await fs.WriteAsync(bytedInfo, 0, bytedInfo.Length);
            }
            catch(Exception ex)
            {
                ErrorMessage = $"Ошибка ${ex.Message}";
            }
            finally
            { 
                fs.Close(); 
            }
        }
        private void Initialize(FileStream fs)
        {
            try
            {
                var analyzer = new Analyzer.Analyzer(fs);
                analyzer.Analyze();
                BindData(analyzer);
                var cachedInfo = new AnalyzerInfo()
                {
                    Filename = $"Project {DateTime.Now.ToString("dd-MM-yyyy:hh:mm:ss")}",
                    MostLongSentence = analyzer.MostLongSentence,
                    MostShortSentence = analyzer.MostShortSentence,
                    MostLongWord = analyzer.MostLongWord,
                    MostShortWord = analyzer.MostShortWord,
                    WordsBySentences = analyzer.WordsBySentences,
                    LettersBySentences = analyzer.LettersBySentences,
                    SentencesAmount = analyzer.SentencesAmount
                };
                _setInfo(cachedInfo);
                SaveProject(cachedInfo);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка ${ex.Message}";
            }
            finally
            {
                fs?.Close();
            }
        }
        private string _errorMessage = string.Empty;
        public string MostLongWord { get => mlw_stat.Text; set { mlw_stat.Text = value; GlobalStateChanged?.Invoke(); } }
        public string MostShortWord { get => msw_stat.Text; set { msw_stat.Text = value; GlobalStateChanged?.Invoke(); } }
        public string MostLongSentence { get => mls_stat.Text; set { mls_stat.Text = value; GlobalStateChanged?.Invoke(); } }
        public string MostShortSentence { get => mss_stat.Text; set { mss_stat.Text = value;
                GlobalStateChanged?.Invoke();
            } }
        public int SentenceAmount { 
            get 
            { 
                if(int.TryParse(aos_stat.Text, out int value)) return value;
                return int.MaxValue;
            } 
            set 
            { 
                aos_stat.Text = value.ToString();
                GlobalStateChanged?.Invoke();
            } 
        }

        public string ErrorMessage { get => _errorMessage; 
            set 
            { _errorMessage = value; GlobalStateChanged?.Invoke(); 
                _notifier?.Close();
                if(ErrorMessage.Contains("Ошибка"))
                    _notifier = new Notification(ErrorMessage, false,0, Resources.Delete_80_icon_icons_com_57340);
                if (ErrorMessage.Contains("..."))
                    _notifier = new Notification(ErrorMessage);
            } 
        }

        public List<AnalyzePoint> LettersBySentences { get; set; }
        public List<AnalyzePoint> WordsBySentences { get ; set ; }

        public event Action GlobalStateChanged;
    }
}
