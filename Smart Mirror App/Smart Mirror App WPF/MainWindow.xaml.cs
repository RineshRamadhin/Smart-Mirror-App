using System.Windows;
using Smart_Mirror_App_WPF.Input.Motion.LeapMotion;

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

            LeapMotion leapmotion = new LeapMotion();
            leapmotion.Connect();
        }
    }
}
