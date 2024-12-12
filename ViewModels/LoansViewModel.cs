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
        private ObservableCollection<string> librariesItems;
        private ObservableCollection<string> booksItems;
        private ObservableCollection<string> customersItems;
        private string selectedLibrary;
        private string selectedBook;
        private string selectedCustomer;
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

        public ObservableCollection<string> LibrariesItems
        {
            get { return librariesItems; }
            set { SetProperty(ref librariesItems, value); }
        }

        public ObservableCollection<string> BooksItems
        {
            get { return booksItems; }
            set { SetProperty(ref booksItems, value); }
        }

        public ObservableCollection<string> CustomersItems
        {
            get { return customersItems; }
            set { SetProperty(ref customersItems, value); }
        }

        public string SelectedLibrary
        {
            get { return selectedLibrary; }
            set
            {
                SetProperty(ref selectedLibrary, value);
            }
        }

        public string SelectedBook
        {
            get { return selectedBook; }
            set
            {
                SetProperty(ref selectedBook, value);
            }
        }

        public string SelectedCustomer
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
            LibrariesItems = new ObservableCollection<string>(librariesList.GetAll().Select(l => l.Name));
            BooksItems = new ObservableCollection<string>();
            foreach (var book in booksList.GetAll())
            {
                BooksItems.Add(book.Title + " (" + book.BookCount + ")");
            }

            CustomersItems = new ObservableCollection<string>(customersList.GetAll().Select(c => c.LastName));
            selectedLibrary = LibrariesItems.FirstOrDefault();
            selectedBook = BooksItems.FirstOrDefault();
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
                string[] parts = SelectedBook.Split(" (");
                if (parts.Length > 0)
                {
                    SelectedBook = parts[0];
                }
                library = librariesList.GetAll().FirstOrDefault(l => l.Name.Equals(SelectedLibrary));
                book = booksList.GetAll().FirstOrDefault(b => b.Title.Equals(SelectedBook));
                customer = customersList.GetAll().FirstOrDefault(c => c.LastName.Equals(SelectedCustomer));

                if (library != null && book != null && customer != null)
                {
                    if (book.BookCount > 0)
                    {
                        Loan newLoan = new Loan(library, book, customer);
                        loanedBook = newLoan;
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
                    MessageBox.Show("Invalid input values!");
                }
            }
            catch
            {
                MessageBox.Show("Invalid input values!");
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
