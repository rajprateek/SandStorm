using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace WpfApplication1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void button_MouseEnter(object sender, MouseEventArgs e)
        {
            Grid c = sender as Grid;
            var pop = c.Children.OfType<Label>();
            var pop2 = c.Children.OfType<Grid>();
            foreach (Label uie in pop)
            {
                uie.Visibility = Visibility.Hidden;
            }
            foreach (Grid uie in pop2)
            {
                uie.Visibility = Visibility.Visible;
            }
            Console.WriteLine("entered..");
        }

        private void button_MouseLeave(object sender, MouseEventArgs e)
        {
            Grid c = sender as Grid;
            var pop = c.Children.OfType<Label>();
            var pop2 = c.Children.OfType<Grid>();

            foreach (Label uie in pop)
            {
                uie.Visibility = Visibility.Visible;
            }
            foreach (Grid uie in pop2)
            {
                uie.Visibility = Visibility.Hidden;
            }
            Console.WriteLine("left..");
        }

        private void onCreate(object sender, RoutedEventArgs e)
        {
            Label t2 = new Label();
            t2.Name = "Label";

            Grid buttonOverlay = new Grid();
            buttonOverlay.Name = "ButtonOverlay";
            buttonOverlay.Width = 200;
            buttonOverlay.Height = 30;
            buttonOverlay.HorizontalAlignment = HorizontalAlignment.Left;
            buttonOverlay.VerticalAlignment = VerticalAlignment.Top;

            ColumnDefinition colDef1 = new ColumnDefinition();
            ColumnDefinition colDef2 = new ColumnDefinition();
            ColumnDefinition colDef3 = new ColumnDefinition();
            RowDefinition rowDef1 = new RowDefinition();
            buttonOverlay.ColumnDefinitions.Add(colDef1);
            buttonOverlay.ColumnDefinitions.Add(colDef2);
            buttonOverlay.ColumnDefinitions.Add(colDef3);
            buttonOverlay.RowDefinitions.Add(rowDef1);

            for (int i = 0; i < 3; i++)
            {
                Label t = new Label();

                t.Width = 26;
                t.Height = 26;
                t.Background = Brushes.Gray;
                t.HorizontalContentAlignment = HorizontalAlignment.Center;
                t.AddHandler(Label.MouseEnterEvent, new MouseEventHandler(highLight));
                t.AddHandler(Label.MouseLeaveEvent, new MouseEventHandler(flatten));
                if (i == 0)
                {
                    t.Content = "->";
                    t.AddHandler(Label.MouseLeftButtonDownEvent, new RoutedEventHandler(activate));
                }
                else if (i == 1)
                {
                    t.Content = "C";
                    t.AddHandler(Label.MouseLeftButtonDownEvent, new RoutedEventHandler(cloudSync));
                }
                else
                {
                    t.Content = "X";
                    t.AddHandler(Label.MouseLeftButtonUpEvent, new RoutedEventHandler(deleteSession));
                }
                Grid.SetRow(t, 0);
                Grid.SetColumn(t, i);
                buttonOverlay.Children.Add(t);
            }

            t2.Width = SessionList.Width;
            t2.Height = 30;
            t2.BorderThickness = new Thickness(4);
            t2.BorderBrush = Brushes.White;
            t2.Background = Brushes.Gray;

            buttonOverlay.Width = Width - 20;
            buttonOverlay.Height = 30;
            buttonOverlay.Background = Brushes.Gray;

            Grid g = new Grid();

            g.Children.Add(buttonOverlay);
            g.Children.Add(t2);
            SessionList.Items.Add(g);
            g.MouseEnter += new MouseEventHandler(button_MouseEnter);
            g.MouseLeave += new MouseEventHandler(button_MouseLeave);

            //GridListView.MaxHeight = this.Height - 53;
        }

        private void Sync_Click(object sender, RoutedEventArgs e)
        {
        }

        private void activate(object sender, RoutedEventArgs e)
        {
            Label l = sender as Label;
            l.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0099FF");
            Console.WriteLine("Session Activated!");
        }

        private void cloudSync(object sender, RoutedEventArgs e)
        {
            Label l = sender as Label;
            l.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0099FF");
            Console.WriteLine("Sync to cloud initiated!");
        }

        private void deleteSession(object sender, RoutedEventArgs e)
        {
            Label l = sender as Label;
            l.Background = (SolidColorBrush)new BrushConverter().ConvertFrom("#0099FF");
            Grid gd = (Grid)l.Parent;
            SessionList.Items.Remove(gd.Parent);
        }

        private void highLight(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            l.Background = Brushes.Silver;
        }

        private void flatten(object sender, MouseEventArgs e)
        {
            Label l = sender as Label;
            l.Background = Brushes.Gray;
        }
    }
}