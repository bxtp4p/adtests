using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace adtests
{
    class Program
    {
        static void Main(string[] args)
        {
            string exit = string.Empty;
            do
            {
                try
                {
                    var options = new Options();
                    if (CommandLine.Parser.Default.ParseArguments(args, options) && exit == string.Empty)   
                    {
                        GetUserGroups(options.Domain, options.Username);
                    } else
                    {
                        options = GetOptions();
                        GetUserGroups(options.Domain, options.Username);
                    }
                }
                catch (Exception ex)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Print(ex);
                    Console.ResetColor();
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Print("Press <x> to exit or <enter> to continue.");
                Console.ResetColor();
                exit = Console.ReadLine();
            }
            while (!exit.Equals("x", StringComparison.CurrentCultureIgnoreCase));
        }

        private static Options GetOptions()
        {
            var options = new Options();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Domain: ");
            Console.ResetColor();

            options.Domain = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.Write("Username: ");
            Console.ResetColor();

            options.Username = Console.ReadLine();

            return options;
        }

        private static void GetUserGroups(string domain, string username)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Print(string.Format("Checking {0}\\{1}", domain, username));
            Console.ResetColor();

            using (var context = new PrincipalContext(ContextType.Domain, domain))
            {
                using (var user = UserPrincipal.FindByIdentity(context, IdentityType.SamAccountName, username))
                {
                    if (user != null)
                    {
                        PrintHeader("Executing UserPrincipal.GetGroups()");
                        PrintRoles(user.GetGroups(context));

                        PrintHeader("Executing UserPrincipal.GetAuthorizationGroups()");

                        try
                        {
                            PrintRoles(user.GetAuthorizationGroups());
                        }
                        catch (PrincipalOperationException poe)
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Print(poe);
                            Console.ResetColor();
                        }
                    }
                }
            }
        }

        private static void PrintHeader(string s)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Print(s);
            Console.ResetColor();
        }

        private static void PrintRoles(PrincipalSearchResult<Principal> roles)
        {
            foreach (var s in roles)
            {
                Print(string.Format("> {0}", s));
            }
        }

        private static void Print(object msg)
        {
            Console.WriteLine(msg);
            Debug.WriteLine(msg);
        }
    }
}
