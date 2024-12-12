using KrskaKnihovna.ViewModels;
using System.Windows;

namespace KrskaKnihovna.Views
{
    public partial class MainWindowView : Window
    {
        private MainViewModel viewModel;

        public MainWindowView()
        {
            InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }
    }
}