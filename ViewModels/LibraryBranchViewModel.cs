using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using KrskaKnihovna.Models;
using KrskaKnihovna.ViewModel;

namespace KrskaKnihovna.ViewModels
{
    internal class LibraryBranchViewModel : ViewModelBasic
    {
        private Library libraryBranch;
        private string name;
        private string address;
        private string phone;
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public LibraryBranchViewModel()
        {
            InitializeCommands();
        }

        public LibraryBranchViewModel(Library libraryBranch)
        {
            BranchName = libraryBranch.Name;
            BranchAddress = libraryBranch.Address;
            BranchPhone = libraryBranch.Phone;
            LibraryBranch = libraryBranch;
            InitializeCommands();
        }

        public string BranchName
        {
            get { return name; }
            set { SetProperty(ref name, value); }
        }

        public string BranchAddress
        {
            get { return address; }
            set { SetProperty(ref address, value); }
        }

        public string BranchPhone
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

        private bool branchAddedSuccessfully;
        public bool BranchAddedSuccessfully
        {
            get { return branchAddedSuccessfully; }
            set { SetProperty(ref branchAddedSuccessfully, value); }
        }

        public Library LibraryBranch
        {
            get { return libraryBranch; }
            set { SetProperty(ref libraryBranch, value); }
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
                if (!string.IsNullOrEmpty(BranchName) && !string.IsNullOrEmpty(BranchAddress) && !string.IsNullOrEmpty(BranchPhone))
                {
                    // Ověření, že telefonní číslo obsahuje pouze číslice
                    Regex regex = new Regex(@"^\d+$");
                    bool isPhoneNumber = regex.IsMatch(BranchPhone);

                    // Ověření, že telefonní číslo má maximálně 9 číslic
                    if (isPhoneNumber && BranchPhone.Length <= 9)
                    {
                        // Naformátování telefonního čísla: po každých třech číslech vložíme mezeru
                        string formattedPhone = Regex.Replace(BranchPhone, @"(\d{3})(?=\d)", "$1 ");

                        // Vytvoření pobočky knihovny s naformátovaným telefonem
                        LibraryBranch = new Library(BranchName, BranchAddress, formattedPhone);

                        DialogResult = true;
                        BranchAddedSuccessfully = true;
                        var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                        window?.Close();
                    }
                    else
                    {
                        MessageBox.Show("Phone number must contain only up to 9 digits.");
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
