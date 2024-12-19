using LiteDB;
using System;
using System.Text.RegularExpressions;

namespace KrskaKnihovna.Models
{
    public class Customer
    {
        [BsonId] public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public int LoanCount { get; set; }

        public Customer(string firstName, string lastName, string phone)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            LoanCount = 0;
        }
        public override string? ToString()
        {
            return string.Format("{0,-15} {1,-15} Phone: {2,-15} Loan count: {3,-5} ID:{4}", FirstName, LastName, Phone, LoanCount, Id);
        }
    }
}
