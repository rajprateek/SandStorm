using Sessions;
using Chrome;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using WebSocketSharp;
using WebSocketSharp.Server;
using System.Diagnostics;



using UI = System.Windows.Controls;
using Newtonsoft.Json;

namespace UIApp
{
    public partial class MainWindow : Window
    {
        private bool firstRun = true;
        private System.Windows.Forms.NotifyIcon m_notifyIcon;
        private ChromeManager chromeManager;
        private WebSocketServer webSocket;

        public MainWindow()
        {
            InitializeComponent();
            chromeManager = new ChromeManager();
            initSocket();

            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.WorkArea.Width - Width - 10;
            Top = SystemParameters.WorkArea.Height - Height - 10;

            m_notifyIcon = new System.Windows.Forms.NotifyIcon();
            m_notifyIcon.Text = "OneSync";
            Icon icon = UIApp.Properties.Resources.icon;
            m_notifyIcon.Icon = new Icon(icon, 0, 0);
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);

            ContextMenuStrip m_contextMenu = new System.Windows.Forms.ContextMenuStrip();
            ToolStripMenuItem mI1 = new ToolStripMenuItem();
            mI1.Text = "Exit";
            mI1.Click += new EventHandler(ExitApp);
            m_contextMenu.Items.Add(mI1);

            m_notifyIcon.ContextMenuStrip = m_contextMenu;

            if (firstRun) Show();

            foreach (string s in SessionManager.GetAllSessions())
                if (!ListViewSessions.Items.Contains(s))
                    ListViewSessions.Items.Add(s);
        }

        private void initSocket()
        {
            webSocket = new WebSocketServer("ws://localhost:36000");
            webSocket.AddWebSocketService("/extension", () =>  new ChromeExtension(chromeManager) { Protocol = "soap" });
            webSocket.Start();
            chromeManager.socket = webSocket;
        }

        private void ExitApp(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }

        private void Exit(object sender, EventArgs e)
        {
            Deactivate(sender, e);
        }

        private void OnClose(object sender, CancelEventArgs args)
        {
            m_notifyIcon.Dispose();
            m_notifyIcon = null;

          //  webSocket.Stop();
        }

        private void OnIsVisibleChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            CheckTrayIcon();
        }

        private void Deactivate(object sender, EventArgs e)
        {
            if (firstRun)
            {
                m_notifyIcon.ShowBalloonTip(2500, "OneSync",
                    "OneSync is now running in the background", ToolTipIcon.Info);
                firstRun = false;
            }
            Hide();
        }

        private void m_notifyIcon_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.MouseEventArgs me = (System.Windows.Forms.MouseEventArgs)e;
            if (me.Button == MouseButtons.Right) return;
            Show();
            WindowState = WindowState.Normal;
            Activate();
        }

        private void CheckTrayIcon()
        {
            if (m_notifyIcon != null)
                m_notifyIcon.Visible = true;
        }

        public async void CreateSession(object sender, EventArgs e)
        {
            string sessionName = "Session - " + new Random().Next(20);
            await Task.Run(() => SessionManager.NewSession(sessionName, true, chromeManager));
            ListViewSessions.Items.Add(sessionName);
        }

        public void OnListHover(object sender, EventArgs e)
        {
            Grid grid = (Grid)sender;
            UIElementCollection collection = grid.Children;

            UI.Label label = (UI.Label)collection[0];
            label.Visibility = Visibility.Hidden;

            Grid grid2 = (Grid)collection[1];
            grid2.Visibility = Visibility.Visible;
        }

        public void OffListHover(object sender, EventArgs e)
        {
            Grid grid = (Grid)sender;
            UIElementCollection collection = grid.Children;

            UI.Label label = (UI.Label)collection[0];
            label.Visibility = Visibility.Visible;

            Grid grid2 = (Grid)collection[1];
            grid2.Visibility = Visibility.Hidden;
        }

        public void Remove(object sender, EventArgs e)
        {
            UI.Button button = sender as UI.Button;
            string name = button.Tag.ToString();
            ListViewSessions.Items.Remove(name);
            SessionManager.RemoveSession(name);
        }

        public void Sync(object sender, EventArgs e)
        {
        }

        public void Activate(object sender, EventArgs e)
        {
            UI.Button button = sender as UI.Button;
            string name = button.Tag.ToString();
            SessionManager.RestoreSession(name,chromeManager);
        }
    }

    public class ChromeExtension : WebSocketBehavior
    {

        private ChromeManager cM;

        public ChromeExtension(ChromeManager chromeManager)
        {
            this.cM = chromeManager;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            Debug.WriteLine("Received Data: " + e.Data);
            
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                string data = e.Data;
                Debug.WriteLine(e.Data);
                Debug.WriteLine("********");
                cM.tabs = JsonConvert.DeserializeObject<Tab[]>(data);
            });
        }
    }
}