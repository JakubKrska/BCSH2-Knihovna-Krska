﻿using LiteDB;
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

       
        }
    }

