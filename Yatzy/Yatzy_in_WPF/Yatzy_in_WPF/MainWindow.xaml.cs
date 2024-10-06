using System.Windows;
using Yatzy_in_WPF.Pages;

namespace Yatzy_in_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.NavigationService.Navigate(new MainGame());
        }
    }
}
