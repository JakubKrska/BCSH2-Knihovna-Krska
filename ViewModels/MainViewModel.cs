﻿using System;
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
        private ObservableCollection<string> comboBoxLibrariesItems;
        private ObservableCollection<string> comboBoxBooksItems;
        private ObservableCollection<string> comboBoxCustomersItems;
        private string selectedLibrary;
        private string selectedBook;
        private string selectedCustomer;
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
            ListBoxInformationItems = new ObservableCollection<string>();
            database = DatabaseHelper.GetDatabase();
            if (database == null) { return; }
            InitializeDatabase();
            Refresh(option);
        }

        public string LabelTitle
        {
            get { return labelTitle; }
            set { SetProperty(ref labelTitle, value); }
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
            set
            {
                SetProperty(ref selectedCustomer, value);
            }
        }

        private ObservableCollection<string> listBoxInformationItems;
        public ObservableCollection<string> ListBoxInformationItems
        {
            get { return listBoxInformationItems; }
            set { SetProperty(ref listBoxInformationItems, value); }
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

        public ObservableCollection<string> ComboBoxLibrariesItems
        {
            get { return comboBoxLibrariesItems; }
            set { SetProperty(ref comboBoxLibrariesItems, value); }
        }

        public ObservableCollection<string> ComboBoxBooksItems
        {
            get { return comboBoxBooksItems; }
            set { SetProperty(ref comboBoxBooksItems, value); }
        }

        public ObservableCollection<string> ComboBoxCustomersItems
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
            }
            catch (Exception e)
            {
                MessageBox.Show("Error while creating database: " + e.Message);
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

                case EnumPossibilities.Loans: // aktualizuje počet dostupnych knih a přepočítá počet vypujček
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

        private int FindElement() // Najde ID vybraného objektu z ListBoxu
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

        private void AddLoan() // Checkne dostupnost knihy, snizi pocet a aktualizuje vypujcky
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
        private void ButtonFilterClick() // filtruje podle hodnot v comboboxech
        {
            listBoxInformationItems.Clear();
            var loans = loansList.GetAll();
            foreach (var loan in loans)
            {
                bool filterLibrary = SelectedLibrary.Equals(notSelected) || SelectedLibrary.Equals(loan.SelectedLibrary.Name);
                bool filterBook = SelectedBook.Equals(notSelected) || SelectedBook.Equals(loan.SelectedBook.Title);
                bool filterCustomer = SelectedCustomer.Equals(notSelected) || SelectedCustomer.Equals(loan.SelectedCustomer.LastName);

                if (filterLibrary && filterBook && filterCustomer)
                {
                    listBoxInformationItems.Add(loan.ToString());
                }
            }
        }
        private Visibility _filtersVisibility = Visibility.Collapsed; // Default: Hidden

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
            ListBoxInformationItems.Clear();
            this.option = option;

            // Změníme stav pro tlačítko Edit a Delete na základě vybrané sekce
            Option = option;

            // Reset SelectedListBoxItem, což deaktivuje tlačítko Delete pokud není nic vybráno
            SelectedListBoxItem = null;

            switch (option)
            {
                case EnumPossibilities.Libraries:
                    LabelTitle = "Library Branches";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var library in librariesList.GetAll())
                    {
                        ListBoxInformationItems.Add(library.ToString());
                    }
                    break;
                case EnumPossibilities.Books:
                    LabelTitle = "Books database";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var book in booksList.GetAll())
                    {
                        ListBoxInformationItems.Add(book.ToString());
                    }
                    break;
                case EnumPossibilities.Customers:
                    LabelTitle = "Customers";
                    FiltersVisibility = Visibility.Collapsed;
                    foreach (var customer in customersList.GetAll())
                    {
                        customer.LoanCount = loansList.GetAll().Count(l => l.SelectedCustomer.Id == customer.Id);
                        ListBoxInformationItems.Add(customer.ToString());
                    }
                    break;
                case EnumPossibilities.Loans:
                    LabelTitle = "Loans";
                    FiltersVisibility = Visibility.Visible; 
                    foreach (var loan in loansList.GetAll())
                    {
                        ListBoxInformationItems.Add(loan.ToString());
                    }
                    break;
            }


            // Doplnění ComboBoxů
            ComboBoxLibrariesItems = new ObservableCollection<string>();
            ComboBoxBooksItems = new ObservableCollection<string>();
            ComboBoxCustomersItems = new ObservableCollection<string>();

            ComboBoxLibrariesItems.Add(notSelected);
            ComboBoxBooksItems.Add(notSelected);
            ComboBoxCustomersItems.Add(notSelected);

            foreach (var library in librariesList.GetAll())
            {
                ComboBoxLibrariesItems.Add(library.Name);
            }
            foreach (var book in booksList.GetAll())
            {
                ComboBoxBooksItems.Add(book.Title);
            }
            foreach (var customer in customersList.GetAll())
            {
                ComboBoxCustomersItems.Add(customer.LastName);
            }

            // Vybereme první hodnotu v ComboBoxech, pokud je k dispozici
            if (ComboBoxLibrariesItems.Count > 0)
            {
                SelectedLibrary = ComboBoxLibrariesItems.FirstOrDefault();
            }
            if (ComboBoxBooksItems.Count > 0)
            {
                SelectedBook = ComboBoxBooksItems.FirstOrDefault();
            }
            if (ComboBoxCustomersItems.Count > 0)
            {
                SelectedCustomer = ComboBoxCustomersItems.FirstOrDefault();
            }
        }
    }
}
