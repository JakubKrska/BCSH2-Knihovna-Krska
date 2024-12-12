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
using KrskaKnihovna.Models;
using KrskaKnihovna.ViewModels;

namespace KrskaKnihovna.Views
{
    /// <summary>
    /// Interakční logika pro LoansView.xaml
    /// </summary>
    public partial class LoansView : Window
    {
        private LoansViewModel viewModel;

        public LoansView(Records<Library> libraries, Records<Book> books, Records<Customer> customers )
        {
            InitializeComponent();
            viewModel = new LoansViewModel(libraries, books, customers);
            DataContext = viewModel;
        }
    }
}
