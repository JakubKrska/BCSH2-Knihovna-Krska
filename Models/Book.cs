using LiteDB;
using System;

namespace KrskaKnihovna.Models
{
    public class Book
    {
        [BsonId] public int Id { get; set; }
        public string Title { get; set; }
        public int PageCount { get; set; }
        public int BookCount { get; set; }

        public Book(string title, int pageCount, int bookCount)
        {
            Title = title;
            PageCount = pageCount;
            BookCount = bookCount;
        }
        public override string? ToString()
        {
            return string.Format("{0,-30} | pages: {1,-8} | number of books: {2,10} | ID: {3}", Title, PageCount, BookCount, Id);
        }
    }
}
