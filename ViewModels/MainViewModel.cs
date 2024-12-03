using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiteDB;
using System.Windows;
using System.Windows.Input;
using KrskaKnihovna.ViewModel;
using System.Collections.ObjectModel;
using KrskaKnihovna.Models;
using System.ComponentModel;

namespace KrskaKnihovna.ViewModels
{
    internal class MainViewModel : ViewModelBasic
    {
        private ObservableCollection<string> comboBoxLibrariesItems;
        private ObservableCollection<string> comboBoxBooksItems;
        private ObservableCollection<string> comboBoxCustomersItems;
        private string chosenBook;
        private string chosenLibrary;
        private string chosenCustomer;
        private LiteCollection<Library> librariesDB;
        private LiteCollection<Book> booksDB;
        private LiteCollection<Customer> customersDB;
        private LiteCollection<Loan> loansDB;
        private LiteDatabase database;
        private Records<Library> fieldLibraries;
        private Records<Book> fieldBooks;
        private Records<Customer> fieldCustomers;
        private Records<Loan> fieldLoans;
        private EnumPossibilities possibility = EnumPossibilities.Libraries;
        private readonly string unchosen = "All";
        public MainViewModel()
        {
            InitializeCommands();
            ListBoxInfoItems = new ObservableCollection<string>();
            database = DatabaseHelper.GetDatabase();
            if (database != null) { return; }
            InitializeDatabase();
            Refresh(possibility);
        }
        public string ChosenLibrary
        {
            get { return chosenLibrary; }
            set
            {
                SetProperty(ref chosenLibrary, value);
            }
        }
        public string ChosenCustoker
        {
            get { return chosenCustomer; }
            set
            {
                SetProperty(ref chosenCustomer, value);
            }
        }
        public string ChosenBook
        {
            get { return chosenBook; }
            set
            {
                SetProperty(ref chosenBook, value);
            }
        }

        private ObservableCollection<string> listBoxInfoItems;
        public ObservableCollection<string> ListBoxInfoItems
        {
            get { return listBoxInfoItems; }
            set { SetProperty(ref listBoxInfoItems, value); }
        }
        private object selectedListBoxItem;
        public object SelectedListBoxItem
        {
            get { return selectedListBoxItem; }
            set { SetProperty(ref selectedListBoxItem, value); }
        }

        public ICommand ButtonLibrariesCommand { get; private set; }
        public ICommand ButtonBooksCommand { get; private set; }
        public ICommand ButtonCustomersCommand { get; private set; }
        public ICommand ButtonLoansCommand { get; private set; }
        public ICommand ButtonNewCommand { get; private set; }
        public ICommand ButtonEditCommand { get; private set; }
        public ICommand ButtonDeleteCommand { get; private set; }
        public ICommand ButtonEndCommand { get; private set; }
        public ICommand ButtonFilterCommand { get; private set; }

        public ObservableCollection<string> ComboBoxLibrariesItems
        {
            get { return comboBoxLibrariesItems; }
            set { SetProperty(ref comboBoxLibrariesItems, value); }
        }

        private void InitializeDatabase()
        {
            try
            {
                librariesDB = (LiteCollection<Library>)database.GetCollection<Library>("LibraryDB");
                fieldLibraries = new Records<Library>(librariesDB);

                booksDB = (LiteCollection<Book>)database.GetCollection<Book>("BookDB");
                fieldBooks = new Records<Book>(booksDB);

                customersDB = (LiteCollection<Customer>)database.GetCollection<Customer>("CustomersDB");
                fieldCustomers = new Records<Customer>(customersDB);

                loansDB = (LiteCollection<Loan>)database.GetCollection<Loan>("LoansDB");
                fieldLoans = new Records<Loan>(loansDB);

            }
            catch (Exception e)
            {
                MessageBox.Show("Error when creating database: " + e.Message);
            }
        }

            private void Refresh(EnumPossibilities possibility)
        {
            throw new NotImplementedException();
        }

        private void InitializeCommands()
        {
          
        }
    }
}
