﻿using Smart_Mirror_App_WPF.ViewModel;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            startclock();
           
        }

        private void startclock()
        {
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += tickevent;
            timer.Start(); 
        }

        private void tickevent(object sender, EventArgs e)
        {
            //  throw new NotImplementedException();
            time.Text = DateTime.Now.ToString("H:mm");
            date.Text = DateTime.Today.ToString("M", CultureInfo.CreateSpecificCulture("nl-BE"));
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new SettingsWindow().Show();

        }
    }
}
