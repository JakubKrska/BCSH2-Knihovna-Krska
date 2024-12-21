using System;
using System.Linq;
using LiteDB;
using System.Windows;
using System.Windows.Input;
using KrskaKnihovna.ViewModel;
using System.Collections.ObjectModel;
using KrskaKnihovna.Models;
using KrskaKnihovna.Views;
using System.IO;


namespace KrskaKnihovna.ViewModels
{
    internal class MainViewModel : ViewModelBasic
    {
        private ObservableCollection<Library> comboBoxLibrariesItems;
        private ObservableCollection<Book> comboBoxBooksItems;
        private ObservableCollection<Customer> comboBoxCustomersItems;
        private Library selectedLibrary;
        private Book selectedBook;
        private Customer selectedCustomer;
        private LiteCollection<Library> librariesDB;
        private LiteCollection<Book> booksDB;
        private LiteCollection<Customer> customersDB;
        private LiteCollection<Loan> loansDB;
        private LiteDatabase database;
        private Records<Library> librariesList;
        private Records<Book> booksList;
        private Records<Customer> customersList;
        private Records<Loan> loansList;
        private EnumPossibilities option = EnumPossibilities.Libraries;
        private string labelTitle;
        private readonly string notSelected = "All";

        public MainViewModel()
        {
            InitializeCommands();
            ListBoxBooks = new ObservableCollection<Book>();
            ListBoxLibraries = new ObservableCollection<Library>();
            ListBoxLoans = new ObservableCollection<Loan>();
            ListBoxCustomers = new ObservableCollection<Customer>();

            database = DatabaseHelper.GetDatabase();
            if (database == null) { return; }
            InitializeDatabase();
            Refresh(EnumPossibilities.Libraries);
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




        private ObservableCollection<Book> listBoxBooks;
        public ObservableCollection<Book> ListBoxBooks
        {
            get { return listBoxBooks; }
            set { SetProperty(ref listBoxBooks, value); }
        }

        private ObservableCollection<Library> listBoxLibraries;
        public ObservableCollection<Library> ListBoxLibraries
        {
            get { return listBoxLibraries; }
            set { SetProperty(ref listBoxLibraries, value); }
        }
        private ObservableCollection<Customer> listBoxCustomers;
        public ObservableCollection<Customer> ListBoxCustomers
        {
            get { return listBoxCustomers; }
            set { SetProperty(ref listBoxCustomers, value); }
        }
        private ObservableCollection<Loan> listBoxLoans;
        public ObservableCollection<Loan> ListBoxLoans
        {
            get { return listBoxLoans; }
            set { SetProperty(ref listBoxLoans, value); }
        }


        private EnumPossibilities _option;

        public EnumPossibilities Option
        {
            get { return _option; }
            set
            {
                _option = value;
                OnPropertyChanged();

                // Pokud je sekce Loans, deaktivovat tlačítko Edit
                ButtonEditIsEnabled = _option != EnumPossibilities.Loans && SelectedListBoxItem != null;
            }
        }


        private object _selectedListBoxItem;
        public object SelectedListBoxItem
        {
            get { return _selectedListBoxItem; }
            set
            {
                _selectedListBoxItem = value;
                OnPropertyChanged();

                // Povolit tlačítko Edit pouze pokud je něco vybráno
                ButtonEditIsEnabled = _option != EnumPossibilities.Loans && _selectedListBoxItem != null;
                ButtonDeleteIsEnabled = _selectedListBoxItem != null;
            }
        }

        public ICommand ButtonLibrariesCommand { get; private set; }
        public ICommand ButtonBooksCommand { get; private set; }
        public ICommand ButtonCustomersCommand { get; private set; }
        public ICommand ButtonLoansCommand { get; private set; }
        public ICommand ButtonAddCommand { get; private set; }
        public ICommand ButtonEditCommand { get; private set; }
        public ICommand ButtonDeleteCommand { get; private set; }
        public ICommand ButtonExitCommand { get; private set; }
        public ICommand ButtonFilterCommand { get; private set; }

        public ObservableCollection<Library> ComboBoxLibrariesItems
        {
            get { return comboBoxLibrariesItems; }
            set { SetProperty(ref comboBoxLibrariesItems, value); }
        }

        public ObservableCollection<Book> ComboBoxBooksItems
        {
            get { return comboBoxBooksItems; }
            set { SetProperty(ref comboBoxBooksItems, value); }
        }

        public ObservableCollection<Customer> ComboBoxCustomersItems
        {
            get { return comboBoxCustomersItems; }
            set { SetProperty(ref comboBoxCustomersItems, value); }
        }

        private bool _buttonEditIsEnabled;
        public bool ButtonEditIsEnabled
        {
            get { return _buttonEditIsEnabled; }
            set
            {
                _buttonEditIsEnabled = value;
                OnPropertyChanged(); // Notifikace UI o změně
            }
        }
        private bool _buttonDeleteIsEnabled;
        public bool ButtonDeleteIsEnabled
        {
            get { return _buttonDeleteIsEnabled; }
            set
            {
                _buttonDeleteIsEnabled = value;
                OnPropertyChanged();
            }
        }

        private Visibility filteredVisibility;
        public Visibility FilteredVisibility
        {
            get { return filteredVisibility; }
            set { SetProperty(ref filteredVisibility, value); }
        }


        private void InitializeCommands()
        {
            ButtonLibrariesCommand = new RelayCommand(_ => Refresh(EnumPossibilities.Libraries));
            ButtonBooksCommand = new RelayCommand(_ => Refresh(EnumPossibilities.Books));
            ButtonCustomersCommand = new RelayCommand(_ => Refresh(EnumPossibilities.Customers));
            ButtonLoansCommand = new RelayCommand(_ => Refresh(EnumPossibilities.Loans));
            ButtonAddCommand = new RelayCommand(_ => ButtonNew_Click(option));
            ButtonEditCommand = new RelayCommand(_ => ButtonEditClick(option));
            ButtonDeleteCommand = new RelayCommand(_ => ButtonDeleteClick(option));
            ButtonExitCommand = new RelayCommand(_ => ButtonExitClick());
            ButtonFilterCommand = new RelayCommand(_ => ButtonFilterClick());
        }

        private void InitializeDatabase()
        {
            try
            {
                librariesDB = (LiteCollection<Library>)database.GetCollection<Library>("LibrariesDB");
                librariesList = new Records<Library>(librariesDB);

                booksDB = (LiteCollection<Book>)database.GetCollection<Book>("BooksDB");
                booksList = new Records<Book>(booksDB);

                customersDB = (LiteCollection<Customer>)database.GetCollection<Customer>("CustomersDB");
                customersList = new Records<Customer>(customersDB);

                loansDB = (LiteCollection<Loan>)database.GetCollection<Loan>("LoansDB");
                loansList = new Records<Loan>(loansDB);

                ComboBoxLibrariesItems = new ObservableCollection<Library>(librariesList.GetAll());
                ComboBoxBooksItems = new ObservableCollection<Book>(booksList.GetAll());
                ComboBoxCustomersItems = new ObservableCollection<Customer>(customersList.GetAll());

                // Nastavení výchozích hodnot
                SelectedLibrary = ComboBoxLibrariesItems.FirstOrDefault();
                SelectedBook = ComboBoxBooksItems.FirstOrDefault();
                SelectedCustomer = ComboBoxCustomersItems.FirstOrDefault();
            }
            catch (Exception e)
            {
                MessageBox.Show("Error initializing database: " + e.Message);
            }
        }

        private void ButtonNew_Click(EnumPossibilities option)
        {
            switch (option)
            {
                case EnumPossibilities.Libraries:
                    AddBranch();
                    break;
                case EnumPossibilities.Books:
                    AddBook();
                    break;
                case EnumPossibilities.Customers:
                    AddCustomer();
                    break;
                case EnumPossibilities.Loans:
                    AddLoan();
                    break;
            }
            Refresh(option);
        }


        private void ButtonDeleteClick(EnumPossibilities option)
        {
            switch (option)
            {
                case EnumPossibilities.Libraries:
                    MessageBox.Show("Libraries cannot be deleted!");
                    break;
                case EnumPossibilities.Books:
                    var bookId = FindElement();
                    var bookToDelete = booksList.GetAll().FirstOrDefault(b => b.Id == bookId);

                    if (bookToDelete != null)
                    {
                        // Smazat všechny výpůjčky spojené s touto knihou
                        var relatedLoans = loansList.GetAll().Where(l => l.SelectedBook.Id == bookId).ToList();
                        foreach (var loan in relatedLoans)
                        {
                            // Snížit počet výpůjček zákazníka
                            var customer = customersList.GetAll().FirstOrDefault(c => c.Id == loan.SelectedCustomer.Id);
                            if (customer != null)
                            {
                                customer.LoanCount--;
                                customersList.Edit(customer);
                            }

                            loansList.Remove(loan.Id);
                        }

                        // Smazat samotnou knihu
                        booksList.Remove(bookId);
                    }
                    break;

                case EnumPossibilities.Customers:
                    var customerId = FindElement();
                    var customerToDelete = customersList.GetAll().FirstOrDefault(c => c.Id == customerId);

                    if (customerToDelete != null)
                    {
                        // Smazat všechny výpůjčky spojené s tímto zákazníkem
                        var relatedLoans = loansList.GetAll().Where(l => l.SelectedCustomer.Id == customerId).ToList();
                        foreach (var loan in relatedLoans)
                        {
                            // Vrátit knihy spojené s výpůjčkami
                            var book = booksList.GetAll().FirstOrDefault(b => b.Id == loan.SelectedBook.Id);
                            if (book != null)
                            {
                                book.BookCount++;
                                booksList.Edit(book);
                            }

                            loansList.Remove(loan.Id);
                        }

                        // Smazat samotného zákazníka
                        customersList.Remove(customerId);
                    }
                    break;

                case EnumPossibilities.Loans:
                    var loanId = FindElement();
                    var loanToDelete = loansList.GetAll().FirstOrDefault(l => l.Id == loanId);

                    if (loanToDelete != null)
                    {
                        // Vrátit knihu
                        var book = booksList.GetAll().FirstOrDefault(b => b.Id == loanToDelete.SelectedBook.Id);
                        if (book != null)
                        {
                            book.BookCount++;
                            booksList.Edit(book);
                        }

                        // Snížit počet výpůjček zákazníka
                        var customer = customersList.GetAll().FirstOrDefault(c => c.Id == loanToDelete.SelectedCustomer.Id);
                        if (customer != null)
                        {
                            customer.LoanCount--;
                            customersList.Edit(customer);
                        }

                        // Smazat samotnou výpůjčku
                        loansList.Remove(loanId);
                    }
                    break;
            }

            // Obnov zobrazení dat
            Refresh(option);
        }

        private int FindElement()
        {
            if (SelectedListBoxItem != null)
            {
                var selectedElement = SelectedListBoxItem.ToString();
                string[] parts = selectedElement.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length > 0 && int.TryParse(parts[parts.Length - 1].Trim(), out int elementID))
                {
                    return elementID;
                }
            }
            return 0;
        }

        private void ButtonExitClick()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit the application?", "Exit Confirmation", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void ButtonEditClick(EnumPossibilities option)
        {
            switch (option)
            {
                case EnumPossibilities.Libraries:
                    var branchToEdit = librariesList.GetAll().FirstOrDefault(l => l.Id == FindElement());
                    if (branchToEdit != null)
                    {
                        EditBranch(branchToEdit.Id);
                    }
                    break;
                case EnumPossibilities.Books:
                    var bookToEdit = booksList.GetAll().FirstOrDefault(b => b.Id == FindElement());
                    if (bookToEdit != null)
                    {
                        EditBook(bookToEdit.Id);
                    }
                    break;
                case EnumPossibilities.Customers:
                    var customerToEdit = customersList.GetAll().FirstOrDefault(c => c.Id == FindElement());
                    if (customerToEdit != null)
                    {
                        EditCustomer(customerToEdit.Id);
                    }
                    break;
                case EnumPossibilities.Loans:
                    // Při pokusu o editaci výpůjček zobrazíme varování
                    MessageBox.Show("Loans cannot be edited!");
                    break;

            }
            Refresh(option);
        }

        private void AddBranch()
        {
            LibraryBranchViewModel branchVM = new LibraryBranchViewModel();
            LibraryBranchView dialogBranch = new LibraryBranchView { DataContext = branchVM };

            bool? dialogResultBranch = dialogBranch.ShowDialog();

            if (branchVM.BranchAddedSuccessfully)
            {
                librariesList.Add(branchVM.LibraryBranch);
            }
        }

        private void AddBook()
        {
            BooksViewModel bookVM = new BooksViewModel();
            BooksView dialogBook = new BooksView { DataContext = bookVM };

            bool? dialogResultBook = dialogBook.ShowDialog();

            if (bookVM.AddBook)
            {
                booksList.Add(bookVM.Book);
            }
        }

        private void AddCustomer()
        {
            CustomersViewModel customersVM = new CustomersViewModel();
            CustomersView dialogCustomer = new CustomersView { DataContext = customersVM };

            bool? dialogResultCustomer = dialogCustomer.ShowDialog();

            if (customersVM.AddCustomer)
            {
                customersList.Add(customersVM.Customer);
            }
        }

        private void AddLoan()
        {
            LoansViewModel loanVM = new LoansViewModel(librariesList, booksList, customersList);
            LoansView loans = new LoansView(librariesList, booksList, customersList) { DataContext = loanVM };

            bool? dialogResultLoan = loans.ShowDialog();

            if (loanVM.AddLoan)
            {


                // Update the database dynamically
                var book = booksList.GetAll().FirstOrDefault(b => b.Id == loanVM.LoanedBook.SelectedBook.Id);
                var customer = customersList.GetAll().FirstOrDefault(c => c.Id == loanVM.LoanedBook.SelectedCustomer.Id);

                if (book != null && customer != null && book.BookCount > 0)
                {
                    // Add the loan
                    loansList.Add(loanVM.LoanedBook);
                    // Recalculate book count
                    book.BookCount--;
                    booksList.Edit(book);

                    // Recalculate loan count for customer
                    customer.LoanCount = loansList.GetAll().Count(l => l.SelectedCustomer.Id == customer.Id);
                    customersList.Edit(customer);
                }
                else
                {
                    MessageBox.Show("No available books for loan.");
                }
            }
        }

        private void EditBranch(int branchId)
        {
            var editedBranch = librariesList.GetAll().FirstOrDefault(p => p.Id == branchId);

            if (editedBranch != null)
            {
                LibraryBranchViewModel editBranchVM = new LibraryBranchViewModel(editedBranch);
                LibraryBranchView updateBranch = new LibraryBranchView { DataContext = editBranchVM };
                bool? dialogResultBranch = updateBranch.ShowDialog();

                if (editBranchVM.BranchAddedSuccessfully)
                {
                    editedBranch = editBranchVM.LibraryBranch;
                    editedBranch.Id = branchId;
                    foreach (var loan in loansList.GetAll().Where(p => p.SelectedLibrary == editedBranch))
                    {
                        loan.SelectedLibrary.Name = editBranchVM.BranchName;
                        loansList.Edit(loan);
                    }
                    librariesList.Edit(editedBranch);
                }
            }
        }

        private void EditBook(int bookId)
        {
            var editedBook = booksList.GetAll().FirstOrDefault(p => p.Id == bookId);

            if (editedBook != null)
            {
                BooksViewModel editBookVM = new BooksViewModel(editedBook);
                BooksView editBook = new BooksView { DataContext = editBookVM };
                bool? dialogResultKniha = editBook.ShowDialog();

                if (editBookVM.AddBook)
                {
                    editedBook = editBookVM.Book;
                    editedBook.Id = bookId;
                    foreach (var loan in loansList.GetAll().Where(p => p.SelectedBook.Id == editedBook.Id))
                    {
                        loan.SelectedBook.Title = editBookVM.BookTitle;
                        loansList.Edit(loan);
                    }
                    booksList.Edit(editedBook);
                }

            }
        }

        private void EditCustomer(int customerId)
        {
            var editedCustomer = customersList.GetAll().FirstOrDefault(p => p.Id == customerId);

            if (editedCustomer != null)
            {
                CustomersViewModel editCustomerVM = new CustomersViewModel(editedCustomer);
                CustomersView editCustomer = new CustomersView { DataContext = editCustomerVM };
                bool? dialogResultBook = editCustomer.ShowDialog();

                if (editCustomerVM.AddCustomer)
                {
                    // Aktualizace zákazníka
                    editedCustomer = editCustomerVM.Customer;
                    editedCustomer.Id = customerId;

                    // Aktualizace všech půjček tohoto zákazníka
                    foreach (var loan in loansList.GetAll().Where(p => p.SelectedCustomer.Id == editedCustomer.Id))
                    {
                        loan.SelectedCustomer.LastName = editCustomerVM.LastName;
                        loansList.Edit(loan);
                    }

                    // Přepočítání počtu výpůjček zákazníka
                    int activeLoans = loansList.GetAll().Count(l => l.SelectedCustomer.Id == editedCustomer.Id);
                    editedCustomer.LoanCount = activeLoans;

                    // Uložení zákazníka do databáze
                    customersList.Edit(editedCustomer);
                }
            }
        }
        private void ButtonFilterClick()
        {
            listBoxLoans.Clear();
            var loans = loansList.GetAll();
            foreach (var loan in loans)
            {
                bool filterLibrary = SelectedLibrary.Equals(notSelected) || SelectedLibrary.Equals(loan.SelectedLibrary.Name);
                bool filterBook = SelectedBook.Equals(notSelected) || SelectedBook.Equals(loan.SelectedBook.Title);
                bool filterCustomer = SelectedCustomer.Equals(notSelected) || SelectedCustomer.Equals(loan.SelectedCustomer.LastName);

                if (filterLibrary && filterBook && filterCustomer)
                {
                    listBoxLoans.Add(loan);
                }
            }
        }
        private Visibility _filtersVisibility = Visibility.Collapsed; // Default: Hidden

        public string LabelTitle { get; private set; }

        public Visibility FiltersVisibility
        {
            get { return _filtersVisibility; }
            set
            {
                _filtersVisibility = value;
                OnPropertyChanged(); // Notify UI that property has changed
            }
        }

        private void Refresh(EnumPossibilities option)
        {
            ListBoxCustomers.Clear();
            ListBoxBooks.Clear();
            ListBoxLibraries.Clear();
            ListBoxLoans.Clear();

            this.option = option;

            // Změníme stav pro tlačítka
            Option = option;
            SelectedListBoxItem = null;

            switch (option)
            {
                case EnumPossibilities.Libraries:
                    LabelTitle = "Library Branches";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var library in librariesList.GetAll())
                    {
                        ListBoxLibraries.Add(library);
                    }
                    break;
                case EnumPossibilities.Books:
                    LabelTitle = "Books Database";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var book in booksList.GetAll())
                    {
                        ListBoxBooks.Add(book);
                    }
                    break;
                case EnumPossibilities.Customers:
                    LabelTitle = "Customers";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var customer in customersList.GetAll())
                    {
                        customer.LoanCount = loansList.GetAll().Count(l => l.SelectedCustomer.Id == customer.Id);
                        ListBoxCustomers.Add(customer);
                    }
                    break;
                case EnumPossibilities.Loans:
                    LabelTitle = "Loans";
                    FiltersVisibility = Visibility.Visible;
                    foreach (var loan in loansList.GetAll())
                    {
                        ListBoxLoans.Add(loan);
                    }
                    break;
            }
        }
    }
}
