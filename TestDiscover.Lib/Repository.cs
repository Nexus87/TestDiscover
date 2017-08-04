using System;
using System.IO;
using System.Linq;
using LibGit2Sharp;

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

        public string Clone()
        {            
            _cloneDir = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
            var repoPath = LibGit2Sharp.Repository.Clone(_repo, _cloneDir);
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