using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using KrskaKnihovna.Models;
using KrskaKnihovna.ViewModel;

namespace KrskaKnihovna.ViewModels
{
    internal class CustomersViewModel : ViewModelBasic
    {
        private Customer customer;
        private string firstName;
        private string lastName;
        private string phone;
        private int loanCount;
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public CustomersViewModel()
        {
            InitializeCommands();
        }

        public CustomersViewModel(Customer customer)
        {
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Phone = customer.Phone;
            loanCount = customer.LoanCount;
            Customer = customer;
            InitializeCommands();
        }

        public string FirstName
        {
            get { return firstName; }
            set { SetProperty(ref firstName, value); }
        }

        public string LastName
        {
            get { return lastName; }
            set { SetProperty(ref lastName, value); }
        }

        public string Phone
        {
            get { return phone; }
            set { SetProperty(ref phone, value); }
        }

        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }

        private bool addCustomer;
        public bool AddCustomer
        {
            get { return addCustomer; }
            set { SetProperty(ref addCustomer, value); }
        }

        public Customer Customer
        {
            get { return customer; }
            set { SetProperty(ref customer, value); }
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
                if (!string.IsNullOrEmpty(FirstName) && !string.IsNullOrEmpty(LastName) && !string.IsNullOrEmpty(Phone))
                {
                    Regex regex = new Regex("^[0-9]+$");
                    bool isNumber = regex.IsMatch(Phone);
                    if (isNumber)
                    {
                        Customer = new Customer(FirstName, LastName, Phone)
                        {
                            LoanCount = loanCount
                        };
                        DialogResult = true;
                        AddCustomer = true;
                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                        window?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Invalid input values!");
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
