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
        
    }
}
