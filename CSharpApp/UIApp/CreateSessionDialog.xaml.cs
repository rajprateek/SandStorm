using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Sessions;

namespace UIApp
{
    /// <summary>
    /// Interaction logic for CreateSessionDialog.xaml
    /// </summary>
    public partial class CreateSessionDialog : Window
    {
        public bool ok = false;
        public string sessionName;
        public bool close;

        public CreateSessionDialog()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

        }

        void btnDialogOk_Click(object sender, EventArgs e)
        {
            sessionName = this.SessionName.Text;
            if (String.IsNullOrEmpty(sessionName))
            {
                SessionName.BorderBrush = Brushes.Red;
                SessionName.BorderThickness = new Thickness(1.0);
                SessionName.ToolTip = "Session Name cannot be empty!";
                return;
            }
            
            if (SessionManager.GetAllSessionNames().Contains(sessionName))
            {
                SessionName.BorderBrush = Brushes.Red;
                SessionName.BorderThickness = new Thickness(1.0);
                SessionName.ToolTip = "Session Name already exists!";
                return;
            }

            ok = true;
            close = this.CloseSession.IsChecked.Value;
            Close();
        }
    }
}
