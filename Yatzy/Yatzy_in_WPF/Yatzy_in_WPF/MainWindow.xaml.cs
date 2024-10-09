using System.Windows;
using Yatzy_in_WPF.Pages;

namespace Yatzy_in_WPF
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            // Initialize the Game
            InitializeComponent();
            YatzyLogic.GameManager.InitializeGame(5);
            MainFrame.NavigationService.Navigate(new MainGame());
        }
    }
}
