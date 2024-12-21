using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KrskaKnihovna.ViewModel;
using System.Windows.Input;
using System.Windows;
using KrskaKnihovna.Models;

namespace KrskaKnihovna.ViewModels
{
    internal class LoansViewModel : ViewModelBasic
    {
        private ObservableCollection<Library> librariesItems;
        private ObservableCollection<Book> booksItems;
        private ObservableCollection<Customer> customersItems;
        private Library selectedLibrary;
        private Book selectedBook;
        private Customer selectedCustomer;
        public Library library;
        public Book book;
        public Customer customer;
        private Records<Library> librariesList;
        private Records<Book> booksList;
        private Records<Customer> customersList;
        private Loan loanedBook;
        private bool? dialogResult;
        private bool addLoan;

        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public ObservableCollection<Library> LibrariesItems
        {
            get { return librariesItems; }
            set { SetProperty(ref librariesItems, value); }
        }

        public ObservableCollection<Book> BooksItems
        {
            get { return booksItems; }
            set { SetProperty(ref booksItems, value); }
        }

        public ObservableCollection<Customer> CustomersItems
        {
            get { return customersItems; }
            set { SetProperty(ref customersItems, value); }
        }

        public Library SelectedLibrary
        {
            get { return selectedLibrary; }
            set { SetProperty(ref selectedLibrary, value); }
        }

        public Book SelectedBook
        {
            get { return selectedBook; }
            set { SetProperty(ref selectedBook, value); }
        }

        public Customer SelectedCustomer
        {
            get { return selectedCustomer; }
            set { SetProperty(ref selectedCustomer, value); }
        }

        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }

        public bool AddLoan
        {
            get { return addLoan; }
            set { SetProperty(ref addLoan, value); }
        }

        public Loan LoanedBook
        {
            get { return loanedBook; }
            set { SetProperty(ref loanedBook, value); }
        }

        public LoansViewModel(Records<Library> librariesList, Records<Book> booksList, Records<Customer> customersList)
        {
            this.librariesList = librariesList;
            this.customersList = customersList;
            this.booksList = booksList;
            InitializeCommands();
            InitializeData();
        }

        private void InitializeData()
        {
            LibrariesItems = new ObservableCollection<Library>(librariesList.GetAll());
            BooksItems = new ObservableCollection<Book>(booksList.GetAll());
            CustomersItems = new ObservableCollection<Customer>(customersList.GetAll());

            // Nastavení výchozích hodnot
            SelectedLibrary = LibrariesItems.FirstOrDefault();
            SelectedBook = BooksItems.FirstOrDefault();
            SelectedCustomer = CustomersItems.FirstOrDefault();
        }

        private void InitializeCommands()
        {
            OkCommand = new RelayCommand(_ => Ok());
            CancelCommand = new RelayCommand(_ => Cancel());
        }

        private void Ok()
        {
            try
            {
                if (SelectedLibrary != null && SelectedBook != null && SelectedCustomer != null)
                {
                    if (SelectedBook.BookCount > 0)
                    {
                        Loan newLoan = new Loan(SelectedLibrary, SelectedBook, SelectedCustomer);
                        LoanedBook = newLoan;
                        DialogResult = true;
                        AddLoan = true;

                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                        window?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Unfortunately, the book is not available at the moment.");
                    }
                }
                else
                {
                    MessageBox.Show("Please select valid values!");
                }
            }
            catch
            {
                MessageBox.Show("An error occurred while processing the loan.");
            }
        }

        private void Cancel()
        {
            DialogResult = false;
            var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
            window?.Close();
        }
    }
}
