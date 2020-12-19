using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace HW_ATS
{
    public class Clients
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int Age { get; set; }

        public ConnectingASimCard<AutomaticTelephoneStation> Port { get; set; }

        
        public static Clients ParseFile(string line) // парсим каждую линию и записываем значение в свойства
        {

            string[] parts = line.Split('|');
            return new Clients()
            {
                FirstName = parts[0],
                LastName = parts[1],
                PhoneName = parts[2],
                DateOfBirth = DateTime.Parse(parts[3]),
                Age = int.Parse(parts[4]),
                Port = default
            };
        }


        
    }
}