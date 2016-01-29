using System;
using System.Collections.Generic;


namespace Refactoring
{
    public class Tusc
    {
        private static string _userName;
        private static string _password;

        public static void Start(List<User> listOfUsers, List<Product> products)
        {
            WelcomeMessge();

            if (Authenticate(listOfUsers))
                Purchase(products);
        }

        private static bool Authenticate(List<User> listOfUsers)
        {
            var userInformation = Refactoring.Authenticate.AuthenticateUser(listOfUsers);
            _userName = userInformation.UserName;
            _password = userInformation.Password;
            return userInformation.IsExist || Authenticate(listOfUsers);
        }

        private static void Purchase(List<Product> products)
        {
            Console.WriteLine();
            Console.WriteLine("Welcome to Product Page");
            Console.WriteLine();
            Products.Purchase(products, _userName, _password);
        }

        private static void WelcomeMessge()
        {
            // Write welcome message
            Console.WriteLine("Welcome to TUSC");
            Console.WriteLine("---------------");
        }
    }

}
