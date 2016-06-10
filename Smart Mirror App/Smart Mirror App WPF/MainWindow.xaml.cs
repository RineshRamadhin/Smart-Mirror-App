using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;

using Smart_Mirror_App_WPF.Util;
using System;

namespace Smart_Mirror_App_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ICommand _navigateToAuthenticationPage;

        public MainWindow()
        {
            InitializeComponent();
            NavigateToPage();
        }

        public ICommand NavigateToAuthenticationPage
        {
            get
            {
                if (_navigateToAuthenticationPage == null)
                {
                    _navigateToAuthenticationPage = new RelayCommand(
                        param => this.NavigateToPage(),
                        param => this.CanNavigate()
                    );
                }
                return _navigateToAuthenticationPage;
            }
        }

        private void NavigateToPage()
        {
            Authentication.Google.AuthenticationPage nextPage = new Authentication.Google.AuthenticationPage();
            MainFrame.Navigate(nextPage);
        }

        private bool CanNavigate()
        {
            return true;
        }
    }
}
