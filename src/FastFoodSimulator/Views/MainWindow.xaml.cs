using System.Windows;
using System.Windows.Controls;
using FastFoodSimulator.Business;


namespace FastFoodSimulator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Simulator _simulator;

        public MainWindow()
        {
            InitializeComponent();
            _simulator = new Simulator(3000, 2000, 900, 1500, 2000);
            DataContext = _simulator;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _simulator.StartFastFoodSimulator();
            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _simulator.IsSimulatorWorking = false;
            btnStop.IsEnabled = false;
            btnStart.IsEnabled = true;
        }
    }
}
