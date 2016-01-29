using System;
using System.Collections.Generic;
using System.Linq;


namespace Refactoring
{
    public static class Authenticate
    {
        public static UserInformation AuthenticateUser(List<User> listOfUsers)
        {
             return CheckIfUserExist(listOfUsers, GetUserInformation());
        }

        private static UserInformation GetUserInformation()
        {
            var userInformation = new UserInformation();
            Console.WriteLine();
            Console.WriteLine("Enter Username:");
            userInformation.UserName = Console.ReadLine();
            Console.WriteLine("Enter Password:");
            userInformation.Password = Console.ReadLine();
            return userInformation;
        }

        private static UserInformation CheckIfUserExist(IEnumerable<User> listOfUsers, UserInformation userInformation)
        {
            var userExists = listOfUsers.FirstOrDefault(user => String.Equals(user.UserName.ToLower(), userInformation.UserName.ToLower()) && user.Password == userInformation.Password);

            if (userExists != null && !String.IsNullOrWhiteSpace(userExists.UserName))
            {
                ShowWelcomeMessage(userExists);
                userInformation.IsExist = true;
                return userInformation;
            }
            
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine();
            Console.WriteLine("You entered an invalid user or password.");
            Console.ResetColor();
            userInformation.IsExist = false;
            return userInformation;
        }

        private static void ShowWelcomeMessage(User userExists)
        {
            // Show balance 
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("Your balance is " + userExists.BalanceAmount.ToString("C"));
            
            // Show welcome message
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine();
            Console.WriteLine("Login successful! Welcome " + userExists.UserName + "!");
            Console.ResetColor();
        }
    }
}
