using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApp2.Models;
using WindowsFormsApp2.Properties;

namespace WindowsFormsApp2.Views
{
    public partial class Notification : Form, INotifier
    {
        public Notification(string Message, bool progressVisible = false, int progressInitValue = 0, Bitmap icon = null)
        {
            InitializeComponent();
            this.Message = Message;
            this.Progress = progressInitValue;
            this.ProgressVisible = progressVisible;
            this.ButtonEnabled = true;
            this.Image = icon ?? Resources.icons8_information_32;
            button1.Click += (sender, e) => this.Close(); 
            StateChanged?.Invoke();
        }
        private int _progress;

        public int Progress
        {
            get { return _progress; }
            set { if (value >= 0 && value <= 100) { _progress = value; StateChanged?.Invoke(); } }
        }

        public bool ProgressVisible { get => progressBar1.Visible; set { progressBar1.Visible = value;
                StateChanged?.Invoke();
            } }
        public Image Image { get => pictureBox1.Image; set { pictureBox1.Image = value;
                StateChanged?.Invoke();
            } }
        public string Message { get => label1.Text; set { label1.Text = value;
                StateChanged?.Invoke();
            } }


        public bool ButtonEnabled { get => button1.Enabled; set { button1.Enabled = value; StateChanged?.Invoke(); } }

        public event Action StateChanged;
    }
}
