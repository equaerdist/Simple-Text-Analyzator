using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApp2.Models
{
    public interface INotifier
    {
        bool ProgressVisible { get; set; }
        Image Image { get; set; }
        string Message { get; set; }
        int Progress { get; set; }
        event Action StateChanged;
        bool ButtonEnabled { get; set; }
        
    }
}
