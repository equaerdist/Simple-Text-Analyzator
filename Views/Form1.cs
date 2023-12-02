using Analyzer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Models;
using WindowsFormsApp2.Presenters;
using WindowsFormsApp2.Properties;
using WindowsFormsApp2.Views;

namespace WindowsFormsApp2
{
    public partial class Form1 : Form, IGlobal
    {
        private Form _notifier = null;
        private Form _activeForm = null;
        private GlobalPresenter _presenter;
        public Form1()
        {
            InitializeComponent();
            _presenter = new GlobalPresenter(this);
            Text = string.Empty;
           
            var promo = new Promo(text => ProjectName = text, info => AnalyzerInfo = info);
            _activeForm = promo;
            button2.Enabled = false;
            ActivateForm(promo);
            tableLayoutPanel3.Controls.Add(promo, 0, 1);
            promo.Show();
            button1.Click += GoToPromo;

        }

        private string myVar;

        public event Action StateChanged;
        

        public string  ProjectName
        {
            get { return myVar; }
            set { 
                myVar = value;
                StateChanged?.Invoke();
            }
        }

        public string Section { get => section.Text; 
            set 
            { 
                section.Text = value; StateChanged?.Invoke(); 
            } 
        }
        public bool StatsActive {
            get => button2.Enabled; 
            set 
            { button2.Enabled = value;
                
            } 
        }

        public AnalyzerInfo AnalyzerInfo { get; set; }

        private void ActivateForm(Form form)
        {
            form.Dock = DockStyle.Fill;
            form.FormBorderStyle = FormBorderStyle.None;
            form.ControlBox = false;
            form.TopLevel = false;
        }
        private void GoToPromo(object sender, EventArgs e)
        {
            if (_activeForm is Promo)
                return;
            _activeForm?.Close();
            var promo = new Promo(text => ProjectName = text, info => AnalyzerInfo = info);
            ActivateForm(promo);
            _activeForm = promo;
            tableLayoutPanel3.Controls.Add(_activeForm, 0, 1);
            _activeForm.Show();
            Section = "Welcome";
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if (_activeForm is Stats)
                return;
            _activeForm?.Close();
            try
            {
                FileStream fs = null;
                if(AnalyzerInfo == null)
                    fs = new FileStream(myVar, FileMode.Open, FileAccess.Read);
                var stats = new Stats(fs, AnalyzerInfo, info => AnalyzerInfo = info);
                ActivateForm(stats);
                _activeForm = stats;
                tableLayoutPanel3.Controls.Add(_activeForm, 0, 1);
                Section = "Stats";
                _activeForm.Show();
            }
            catch (Exception ex)
            {
                _notifier?.Close();
                _notifier = new Notification($"Произошла ошибка\n {ex.Message}", false, 0, Resources.Delete_80_icon_icons_com_57340);
                _notifier.Show();
            }

}
    }
}
