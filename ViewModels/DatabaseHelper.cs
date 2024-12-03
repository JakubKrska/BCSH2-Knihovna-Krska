using KrskaKnihovna.Models;
using LiteDB;
using System;
using System.Windows;

namespace KrskaKnihovna.ViewModel
{
    internal class DatabaseHelper
    {
        private static LiteDatabase database;

        private DatabaseHelper() { }

        public static LiteDatabase GetDatabase()
        {
            if (database == null)
            {
                try
                {
                    string databasePath = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "LibraryData.db");
                    var mapper = BsonMapper.Global;
                    mapper.Entity<Library>().Id(h => h.Id);
                    mapper.Entity<Book>().Id(h => h.Id);
                    mapper.Entity<Customer>().Id(h => h.Id);
                    mapper.Entity<Loan>().Id(h => h.Id);

                    database = new LiteDatabase(databasePath);

                    return database;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Error creating database: " + e.Message);
                }
            }
            return database;
        }
    }
}
