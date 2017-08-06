using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using LibGit2Sharp.Handlers;

namespace TestDiscover.Lib
{
    public class Repository : IDisposable
    {
        private readonly string _repo;
        private LibGit2Sharp.Repository _repository;
        private string _cloneDir;

        public Repository(string repo)
        {
            _repo = repo;
        }

        public string Clone(string username, string password)
        {
            var co = new CloneOptions
            {
                CredentialsProvider = (url, user, cred) =>
                    new UsernamePasswordCredentials {Username = username, Password = password}
            };
            return Clone(co);
        }
        public string Clone(CloneOptions options = null)
        {            
            _cloneDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var repoPath = LibGit2Sharp.Repository.Clone(_repo, _cloneDir, options);
            _repository = new LibGit2Sharp.Repository(repoPath);

            return _cloneDir;
        }

        public void CheckoutTag(string tag)
        {
            var commit = _repository.Tags.SingleOrDefault(x => x.FriendlyName == tag)?.Reference.TargetIdentifier;
            Commands.Checkout(_repository, commit);
        }

        public void CheckoutTagFromEnd(int position)
        {
            var commit = _repository.Tags.Reverse().ElementAt(position).Reference.TargetIdentifier;
            Commands.Checkout(_repository, commit);
        }

        private void Remove()
        {
            if(!string.IsNullOrWhiteSpace(_cloneDir))
            {
                DirectoryHelper.DeleteDirectory(_cloneDir);
            }
        }

        public void Dispose()
        {
            Remove();
        }

        public void CheckoutHash(string hash)
        {
            Commands.Checkout(_repository, hash);
        }
    }
}