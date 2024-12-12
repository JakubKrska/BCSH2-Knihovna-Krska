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
    /// Interakční logika pro BooksView.xaml
    /// </summary>
    public partial class BooksView : Window
    {
        private BooksViewModel viewModel;

        public BooksView()
        {
            InitializeComponent();
        }
        public BooksView(Book book)
        {
            InitializeComponent();
            viewModel = new BooksViewModel();
            DataContext = viewModel;
        }
    }
}
