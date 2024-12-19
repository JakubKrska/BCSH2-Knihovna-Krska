using KrskaKnihovna.Models;
using LiteDB;
using System;

namespace KrskaKnihovna.Models
{
    public class Loan
    {
        [BsonId] public int Id { get; set; }
        public Library SelectedLibrary { get; set; }
        public Book SelectedBook { get; set; }
        public Customer SelectedCustomer { get; set; }
        public Loan() { }
        public Loan(Library selectedLibrary, Book selectedBook, Customer selectedCustomer)
        {
            SelectedLibrary = selectedLibrary;
            SelectedBook = selectedBook;
            SelectedCustomer = selectedCustomer;
            SelectedBook.BookCount++;
        }

        public override string? ToString()
        {
            return string.Format("{0,-30} Title: {1,-30} Customer: {2,-12} ID:{3}", SelectedLibrary.Name, SelectedBook.Title, SelectedCustomer.LastName, Id);
        }
    }
}
