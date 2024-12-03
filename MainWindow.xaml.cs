using KrskaKnihovna.ViewModels;
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

namespace KrskaKnihovna
{
    /// <summary>
    /// Interakční logika pro Window1.xaml
    /// </summary>
    public partial class BookLoaningWindow : Window
    {
        private MainViewModel viewModel;
        public BookLoaningWindow()
        {
            // InitializeComponent();
            viewModel = new MainViewModel();
            DataContext = viewModel;
        }

        //private void ViewModel_ItemFoundEvent(object sender, int index)
        //{
        //    listBoxInfo.SelectedIndex = index;
        //}
    }
}
