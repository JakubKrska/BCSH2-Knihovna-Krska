using LiteDB;
using System;
using System.Text.RegularExpressions;

namespace KrskaKnihovna.Models
{
    public class Library
    {
        [BsonId] public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public Library(string name, string address, string phone)
        {
            Name = name;
            Address = address;
            Phone = phone;
        }

        public override string? ToString()
        {
            return string.Format("{0,-25}|Address: {1,-20}|phone: {2,10} | Id: {3}", Name, Address, Phone, Id);
        }
    }
}
