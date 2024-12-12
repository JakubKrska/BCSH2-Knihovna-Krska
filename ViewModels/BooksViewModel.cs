using System.Linq;
using KrskaKnihovna.ViewModel;
using System.Windows.Input;
using System.Windows;
using KrskaKnihovna.Models;

namespace KrskaKnihovna.ViewModels
{
    internal class BooksViewModel : ViewModelBasic
    {

        private Book book;
        private string title;
        private int pages;
        private int count;
        public ICommand OkCommand { get; private set; }
        public ICommand CancelCommand { get; private set; }

        public BooksViewModel()
        {
            InitializeCommands();
        }
        public BooksViewModel(Book book)
        {
            BookTitle = book.Title;
            BookPages = book.PageCount;
            BookCount = book.BookCount;
            Book = book;
            InitializeCommands();
        }
        public string BookTitle
        {
            get { return title; }
            set { SetProperty(ref title, value); }
        }

        public int BookPages
        {
            get { return pages; }
            set { SetProperty(ref pages, value); }
        }
        public int BookCount
        {
            get { return count; }
            set { SetProperty(ref count, value); }
        }
        private bool? dialogResult;
        public bool? DialogResult
        {
            get { return dialogResult; }
            set { SetProperty(ref dialogResult, value); }
        }
        private bool addBook;
        public bool AddBook
        {
            get { return addBook; }
            set { SetProperty(ref addBook, value); }
        }
        public Book Book
        {
            get { return book; }
            set { SetProperty(ref book, value); }
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
                if (!string.IsNullOrEmpty(BookTitle) && BookPages > 0 && BookCount > 0)
                {
                    Book = new Book(BookTitle, BookPages, BookCount);
                    DialogResult = true;
                    AddBook = true;
                    var window = Application.Current.Windows.OfType<Window>().SingleOrDefault(w => w.IsActive);
                    window?.Close();
                }
                else
                {
                    MessageBox.Show("Wrong input!!");
                }
            }
            catch
            {
                MessageBox.Show("Wrong input!");

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