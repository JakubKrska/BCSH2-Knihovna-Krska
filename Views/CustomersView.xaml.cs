using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using KrskaKnihovna.ViewModels;

namespace KrskaKnihovna.Views
{
    /// <summary>
    /// Interakční logika pro CustomersView.xaml
    /// </summary>
    public partial class CustomersView : Window
    {
        private CustomersViewModel viewModel;
        public CustomersView()
        {
            InitializeComponent();
            viewModel = new CustomersViewModel();
            DataContext = viewModel;
        }
    }
}
