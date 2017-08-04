using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace TestDiscover
{
    internal class Program
    {
        private const string Solution = "TestProject.sln";
        private const string git = @"C:\Users\Kevin\Documents\Visual Studio 2017\Projects\TestProject\";
        private const string FirstTag = "initial";
        private const string SecondTag = "new";

        private static string CheckoutTag(string tag)
        {
            var cloneDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var repoPath = LibGit2Sharp.Repository.Clone(git, cloneDir);
            var rep = new LibGit2Sharp.Repository(repoPath);
            var commit = rep.Tags.SingleOrDefault(x => x.FriendlyName == tag)?.Reference.TargetIdentifier;
            Commands.Checkout(rep, commit);

            return cloneDir;
        }

        private static List<string> GetTestList(string solutionName, Action<Repository> checkoutMethod)
        {
            using (var repo = new Repository(git))
            {
                var path = repo.Clone();
                checkoutMethod(repo);

                var solution = Directory.GetFiles(path, solutionName, SearchOption.AllDirectories).First();

                var scanner = new SolutionScanner(solution);
                return scanner.Scan();
            }
        }

        private static async Task Compare(Action<Repository> checkoutMethodOldRepo, Action<Repository> checkoutMethodNewRepo)
        {
            var oldListTask = Task.Run(() => GetTestList(Solution, checkoutMethodOldRepo));
            var newListTask = Task.Run(() => GetTestList(Solution, checkoutMethodNewRepo));

            var oldList = await oldListTask;
            var newList = await newListTask;

            var removedTests = oldList.Except(newList).Select(x => "--- " + x);
            var newTests = newList.Except(oldList).Select(x => "+++ " + x);
            Console.WriteLine(string.Join("\n", removedTests));
            Console.WriteLine(string.Join("\n", newTests));
        }
        public static void Main(string[] args)
        {
            //Compare(repo => repo.CheckoutTag(FirstTag), repo => repo.CheckoutTag(SecondTag))
                //.Wait();
            Compare(repo => repo.CheckoutTagFromEnd(1), repo => repo.CheckoutTagFromEnd(0)).Wait();
        }
    }
}