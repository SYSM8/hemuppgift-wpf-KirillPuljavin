using System.Windows;
using System.Windows.Controls;

namespace Yatzy_in_WPF.Pages
{
    public partial class StartMenu : Page
    {
        public StartMenu()
        {
            InitializeComponent();
        }

        private void StartGameButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainGame());
        }
        private void HighScoresButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new MainGame());
        }
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
