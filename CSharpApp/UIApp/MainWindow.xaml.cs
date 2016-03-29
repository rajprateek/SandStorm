using System;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Autofac;
using Chrome;
using Newtonsoft.Json;
using Sessions;
using WebSocketSharp;
using WebSocketSharp.Server;
using Application = System.Windows.Application;
using IContainer = Autofac.IContainer;
using UI = System.Windows.Controls;

namespace UIApp
{
    public partial class MainWindow : Window
    {
        private bool firstRun = true;
        private NotifyIcon m_notifyIcon;
        //private ChromeManager chromeManager;
        private WebSocketServer webSocket;

        private static IContainer Container { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            //chromeManager = new ChromeManager();
            initSocket();

            var builder = new ContainerBuilder();
            builder.RegisterType<SessionManager>();

            string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            builder.RegisterAssemblyTypes(Directory.GetFiles(path, "*.dll").Select(Assembly.LoadFile).ToArray()).AsImplementedInterfaces().As<ISessionHandler>();
            //builder.RegisterType<Adobe.Acrobat>().As<ISessionHandler>();
            //builder.RegisterType<ChromeManager>().As<ISessionHandler>();
            //builder.RegisterType<Office.Office>().As<ISessionHandler>();
            //builder.RegisterType<Windows.Windows>().As<ISessionHandler>();
            Container = builder.Build();
            
            WindowStartupLocation = WindowStartupLocation.Manual;
            Left = SystemParameters.WorkArea.Width - Width - 10;
            Top = SystemParameters.WorkArea.Height - Height - 10;

            m_notifyIcon = new NotifyIcon();
            m_notifyIcon.Text = "OneSync";
            Icon icon = Properties.Resources.icon;
            m_notifyIcon.Icon = new Icon(icon, 0, 0);
            m_notifyIcon.Click += new EventHandler(m_notifyIcon_Click);

            ContextMenuStrip m_contextMenu = new ContextMenuStrip();
            ToolStripMenuItem mI1 = new ToolStripMenuItem();
            mI1.Text = "Exit";
            mI1.Click += new EventHandler(ExitApp);
            m_contextMenu.Items.Add(mI1);

            m_notifyIcon.ContextMenuStrip = m_contextMenu;

            if (firstRun) Show();

            foreach (string s in SessionManager.GetAllSessionNames())
                if (!ListViewSessions.Items.Contains(s))
                    ListViewSessions.Items.Add(s);
        }

        private void initSocket()
        {
            webSocket = new WebSocketServer("ws://localhost:36000");
            webSocket.AddWebSocketService("/extension", () =>  new ChromeExtension() { Protocol = "soap" });
            webSocket.Start();
            ChromeManager.socket = webSocket;
        }

        private void ExitApp(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
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
            MouseEventArgs me = (MouseEventArgs)e;
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

        public void CreateSession(object sender, EventArgs e)
        {
            CreateSessionDialog dialog = new CreateSessionDialog();
            dialog.ShowDialog();

            if (!dialog.ok) return;

            using (var scope = Container.BeginLifetimeScope())
            {
                var sessionManager = scope.Resolve<SessionManager>();
                sessionManager.NewSession(dialog.sessionName, dialog.close);
                ListViewSessions.Items.Add(dialog.sessionName);
                ChromeManager.tabs = null;
            }

            //string sessionName = "Session - " + new Random().Next(20);
            //await Task.Run(() => SessionManager.NewSession(sessionName, true, chromeManager));
            //ListViewSessions.Items.Add(sessionName);
        }

        public void OnListHover(object sender, EventArgs e)
        {
            UI.Grid grid = (UI.Grid)sender;
            UI.UIElementCollection collection = grid.Children;

            UI.Label label = (UI.Label)collection[0];
            label.Visibility = Visibility.Hidden;

            UI.Grid grid2 = (UI.Grid)collection[1];
            grid2.Visibility = Visibility.Visible;
        }

        public void OffListHover(object sender, EventArgs e)
        {
            UI.Grid grid = (UI.Grid)sender;
            UI.UIElementCollection collection = grid.Children;

            UI.Label label = (UI.Label)collection[0];
            label.Visibility = Visibility.Visible;

            UI.Grid grid2 = (UI.Grid)collection[1];
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
            using (var scope = Container.BeginLifetimeScope())
            {
                var sessionManager = scope.Resolve<SessionManager>();
                sessionManager.RestoreSession(name);
            }
        }

        private void SyncSession_Click(object sender, RoutedEventArgs e)
        {
        }
    }

    public class ChromeExtension : WebSocketBehavior
    {
        public ChromeExtension()
        {
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            ChromeManager.tabs = JsonConvert.DeserializeObject<Tab[]>(e.Data);
            /*Debug.WriteLine("Received Data: " + e.Data);
            
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                string data = e.Data;
                Debug.WriteLine(e.Data);
                Debug.WriteLine("********");
                ChromeManager.tabs = JsonConvert.DeserializeObject<Tab[]>(data);
            });*/
        }
    }
}