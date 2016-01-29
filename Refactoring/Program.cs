using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Refactoring
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Load users from data file
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data\Users.json")).ToList();

            // Load products from data file
            var products = JsonConvert.DeserializeObject<List<Product>>(File.ReadAllText(@"Data\Products.json")).ToList();

            Tusc.Start(users, products);
        }
    }
}
