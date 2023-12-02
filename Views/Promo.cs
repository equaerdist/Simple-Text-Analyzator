using Analyzer;
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
using Newtonsoft.Json;
using WindowsFormsApp2.Models;
using System.Drawing.Text;
using WindowsFormsApp2.Views;
using WindowsFormsApp2.Properties;
using System.Text.RegularExpressions;

namespace WindowsFormsApp2
{
    public partial class Promo : Form, IPromo
    {
        Action<string> _setFile;
        Action<AnalyzerInfo> _setInfo;
        Notification _notifier = null;
        public IEnumerable<AnalyzerInfo> RecentProjects { get; set; }

        public Promo(Action<string> setFile, Action<AnalyzerInfo> setInfo)
        {
            _setFile = setFile;
            InitializeComponent();
            button1.Click += CreateProject;
            button2.Click += LoadProject;
            _setInfo = setInfo;
            CheckRecentProjects();
        }
        private void CreateProject(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Выбери фаил для анализа";
            openFileDialog1.Filter = "Текстовые файлы (*.txt)|*.txt";
            var result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                _setFile(openFileDialog1.FileName);
            }
        }
        private void CheckRecentProjects()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            RecentProjects = Directory
               .GetFiles(path, $"*.json")
               .OrderByDescending(file => File.GetLastWriteTime(file))
               .Take(2)
               .Select(file => JsonConvert.DeserializeObject<AnalyzerInfo>(File.ReadAllText(file)));
            int count = 0;
            if (RecentProjects.Count() >= 1)
            {
                tableLayoutPanel2.Controls.Remove(panel4);
              
            }
            if(RecentProjects.Count() >= 2)
            {
                tableLayoutPanel2.Controls.Remove(panel3);
            }
            foreach(var file in RecentProjects)
            {
                var panelObject = new Panel();
                panelObject.Click += (s, e) => GetInfoFromFileAndSetup(file.Filename);
                panelObject.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(22)))), ((int)(((byte)(22)))), ((int)(((byte)(22)))));
                panelObject.Cursor = System.Windows.Forms.Cursors.Hand;
                panelObject.Dock = System.Windows.Forms.DockStyle.Fill;
                panelObject.Name = "panel4";
                var image = new PictureBox();
                image.Anchor = System.Windows.Forms.AnchorStyles.None;
                image.Image = global::WindowsFormsApp2.Properties.Resources.icons8_stacked_organizational_chart_96c;
                image.Name = "pictureBox2";
                image.Size = new System.Drawing.Size(102, 63);
                image.Location = new System.Drawing.Point(65, 3);
                image.TabIndex = 1;
                image.TabStop = false;
                var projectName = new Label();
                projectName.Anchor = System.Windows.Forms.AnchorStyles.None;
                projectName.AutoSize = true;
                projectName.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                projectName.Name = "label4";
                projectName.Size = new System.Drawing.Size(85, 17);
                projectName.TabIndex = 0;
                Regex regex = new Regex(@"[^\\]*$");
                if (file.Filename != null) {
                    Match match = regex.Match(file.Filename);
                    if (match.Success) { file.Filename = match.Value; }
                }
                projectName.Text = file.Filename?.Substring(0, 8) + "...";
                projectName.Location = new System.Drawing.Point(85, 66);
                panelObject.Controls.Add(image);
                panelObject.Controls.Add(projectName);
                tableLayoutPanel2.Controls.Add(panelObject, count, 0);
                count++;
            }
        }
        private void GetInfoFromFileAndSetup(string file)
        {
            try
            {
                var analyzerInfo = JsonConvert.DeserializeObject<AnalyzerInfo>(File.ReadAllText(file));
                _setFile(analyzerInfo.Filename);
                _setInfo(analyzerInfo);
            }
            catch (Exception ex)
            {
                _notifier?.Close();
                _notifier = new Notification($"Возникла ошибка ${ex.Message}", false, 0, Resources.Delete_80_icon_icons_com_57340);
                Console.WriteLine(ex.Message);
                _notifier.Show();
            }
        }
        private void LoadProject(object sender, EventArgs e)
        {
            openFileDialog1.Title = "Выбери проект";
            openFileDialog1.Filter = "(*.json)|*.json";
            openFileDialog1.InitialDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var result = openFileDialog1.ShowDialog();
            if(result == DialogResult.OK)
            {
                GetInfoFromFileAndSetup(openFileDialog1.FileName);
            }
        }
    }
}
