using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Fclp;
using TestDiscover.Lib;

namespace TestDiscover
{
    internal static class Program
    {
        private static string _solution;
        private static string _gitRepo;
        private static string _passwd;
        private static string _username;


        private static List<string> GetTestList(Action<Repository> checkoutMethod)
        {
            using (var repo = new Repository(_gitRepo))
            {
                
                var path =!string.IsNullOrWhiteSpace(_username) ? repo.Clone(_username, _passwd) : repo.Clone();
                checkoutMethod(repo);

                var solution = Directory.GetFiles(path, _solution, SearchOption.AllDirectories).First();

                var scanner = new SolutionScanner(solution);
                return scanner.Scan();
            }
        }

        private static async Task Compare(Action<Repository> checkoutMethodOldRepo, Action<Repository> checkoutMethodNewRepo)
        {
            var oldListTask = Task.Run(() => GetTestList(checkoutMethodOldRepo));
            var newListTask = Task.Run(() => GetTestList(checkoutMethodNewRepo));

            var oldList = await oldListTask;
            var newList = await newListTask;

            var removedTests = oldList.Except(newList).Select(x => "--- " + x);
            var newTests = newList.Except(oldList).Select(x => "+++ " + x);
            Console.WriteLine(string.Join("\n", removedTests));
            Console.WriteLine(string.Join("\n", newTests));
        }
        public static void Main(string[] args)
        {
            Action<Repository> checkoutOld = null;
            Action<Repository> checkoutNew = null;
            
            var p = new FluentCommandLineParser();
            p.Setup<List<string>>('c', "commit")
                .Callback(items =>
                {
                    checkoutOld = r => r.CheckoutHash(items[0]);
                    checkoutNew = r => r.CheckoutHash(items[1]);
                })
                .WithDescription("[old] [new]\n\t Checkout by commit hash");

            p.Setup<bool>('l', "latest")
                .Callback(x =>
                {
                    checkoutOld = r => r.CheckoutTagFromEnd(1);
                    checkoutNew = r => r.CheckoutTagFromEnd(0);
                })
                .WithDescription("\n\t Checkout the two latest tags");

            p.Setup<List<string>>('t', "tag")
                .Callback(items =>
                {
                    checkoutOld = r => r.CheckoutTag(items[0]);
                    checkoutNew = r => r.CheckoutTag(items[1]);
                })
                .WithDescription("\n\t Checkout by tag");

            p.SetupHelp("?", "help")
                .Callback(text => Console.WriteLine(text));

            var result = p.Parse(args);
            _gitRepo =  ConfigurationManager.AppSettings["git"];
            _solution = ConfigurationManager.AppSettings["solution"];
            _username = ConfigurationManager.AppSettings["user"];
            _passwd = ConfigurationManager.AppSettings["passwd"];
            if (string.IsNullOrWhiteSpace(_gitRepo) || string.IsNullOrWhiteSpace(_solution))
            {
                Console.WriteLine("Please configure the git and solution options in App.config");
                return;
            }

            if (result.HasErrors)
            {
                Console.WriteLine(result.ErrorText);
            }
            else if (result.EmptyArgs)
            {
                Console.Write("Please choose an option");
                p.Parse(new[] {"-?"});
            }
            else if (checkoutOld != null && checkoutNew != null)
            {
                Compare(checkoutOld, checkoutNew).Wait();
            }
            else
            {
                Console.Write("No known option found");
                p.Parse(new[] { "-?" });
            }
        }
    }
}