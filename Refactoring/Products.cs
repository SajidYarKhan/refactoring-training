using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Refactoring
{
    public class Products
    {
        private const int EXIT_CODE = 8;
        private static double _balance;
        private static string _userName;
        private static string _password;
        private static int _quantity;

        public Products()
        {
            _balance = 0;
        }

        public static void Purchase(List<Product> products, string userName, string password)
        {
            var response = GetUserInput(products);
            _userName = userName;
            _password = password;
            GetBalance(userName, password);

            if (response == EXIT_CODE)
            {
                Exit(products,userName,password);
                return;
            }

            var quantity = GetUserProduct(products,response);

            CheckBalance(products, quantity, response);
            CheckQuantity(products, quantity, response);
            FinalizePruchase(products, quantity, response);

        }

        private static void FinalizePruchase(List<Product> products, int quantity, int selection)
        {
            // Check if quantity is greater than zero
            if (quantity > 0)
            {
                // Balance = Balance - Price * Quantity
                _balance = _balance - products[selection-1].Price * quantity;

                // Quanity = Quantity - Quantity
                products[selection - 1].Quantity = products[selection-1].Quantity - quantity;

                Console.Clear();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("You bought " + quantity + " " + products[selection-1].ProductName);
                Console.WriteLine("Your new balance is " + _balance.ToString("C"));
                Console.ResetColor();
                Exit(products, _userName, _password);
            }
            else
            {
                ShowMessage("Purchase cancelled");
            }
        }

        private static int GetUserProduct(List<Product> products, int selection)
        {
            Console.WriteLine();
            Console.WriteLine("You want to buy: " + products[selection-1].ProductName);
            Console.WriteLine("Your balance is " + _balance.ToString("C"));

            // Prompt for user input
            Console.WriteLine("Enter quantity to purchase:");
            var response = Console.ReadLine();
            return Convert.ToInt32(response);
        }

        private static int GetUserInput(IReadOnlyList<Product> products)
        {
            // Prompt for user input
            Console.WriteLine();
            Console.WriteLine("What would you like to buy?");
            for (var i = 0; i < 7; i++)
            {
                var product = products[i];
                Console.WriteLine(i + 1 + ": " + product.ProductName + " (" + product.Price.ToString("C") + ")");
            }

            Console.WriteLine(products.Count + 1 + ": Exit");

            // Prompt for user input
            Console.WriteLine("Enter a number:");
            var response = Console.ReadLine();
            return Convert.ToInt32(response);
        }

        private static void CheckQuantity(List<Product> products, int quantity, int response)
        {
            if (products[response].Quantity < quantity)
            {
                ShowMessage("Sorry, " + products[response].ProductName + " is out of stock");
                Purchase(products, _userName, _password);
            }
        }

        private static void CheckBalance(List<Product> products, int quantity, int response)
        {
            if (_balance - products[response-1].Price * quantity < 0)
            {
                ShowMessage("You do not have enough money to buy that.");
                Purchase(products, _userName, _password);
            }
        }

        private static void ShowMessage(string message)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine(message);
            Console.ResetColor();
        }

        private static void GetBalance(string userName, string password)
        {
            var user = GetUser(userName, password);

            _balance = user.Balance;
        }
        
        private static UserInformation GetUser(string userName, string password)
        {
            var user = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data\Users.json")).ToList()
                        .FirstOrDefault(row => row.UserName.ToLower().Equals(userName.ToLower()) && row.Password == password);

            return new UserInformation
            {
                UserName = user.UserName,
                Password = user.Password,
                Balance = user.BalanceAmount,
                IsExist = true
            };
        }

        private static void Exit(List<Product> products, string userName, string password)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(File.ReadAllText(@"Data\Users.json")).ToList();

            foreach (var user in users.Where(user => user.UserName.ToLower() == userName.ToLower() && user.Password == password))
            {
                user.BalanceAmount = _balance;
            }

            // Write out new balance
            string json = JsonConvert.SerializeObject(users, Formatting.Indented);
            File.WriteAllText(@"Data\Users.json", json);

            // Write out new quantities
            string json2 = JsonConvert.SerializeObject(products, Formatting.Indented);
            File.WriteAllText(@"Data\Products.json", json2);


            // Prevent console from closing
            Console.WriteLine();
            Console.WriteLine("Press Enter key to exit");
            Console.ReadLine();
        }
    }
}
