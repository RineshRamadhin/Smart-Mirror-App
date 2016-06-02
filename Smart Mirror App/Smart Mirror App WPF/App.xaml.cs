using Smart_Mirror_App_WPF.View;
using Smart_Mirror_App_WPF.Controller;
using Smart_Mirror_App_WPF.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Smart_Mirror_App_WPF.ViewModel;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Provides application-specific behavior to supplement the default Application class.
    /// </summary>
    sealed partial class App : Application
    {
        /// <summary>
        /// Initializes the singleton application object.  This is the first line of authored code
        /// executed, and as such is the logical equivalent of main() or WinMain().
        /// </summary>
        public App()
        {
            this.InitializeComponent();
            var clockModel = new ClockModel();
            clockModel.Update();
            TimerController.RegisterModel(clockModel);
            (Resources["clock"] as ClockView).InitializeComponent();

        }

        /// <summary>
        /// Invoked when the application is launched normally by the end user.  Other entry points
        /// will be used such as when the application is launched to open a specific file.
        /// </summary>
        /// <param name="e">Details about the launch request and process.</param>
        /// 
    }
}