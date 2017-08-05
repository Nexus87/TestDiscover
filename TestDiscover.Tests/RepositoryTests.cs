using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Repository = TestDiscover.Lib.Repository;

namespace TestDiscover.Tests
{
    [TestFixture]
    public class RepositoryTests
    {
       
        private Repository _repository;
        private string _repoPath;

        [SetUp]
        public void Setup()
        {
            _repository = new Repository(Constants.RepoPath);
            _repoPath = _repository.Clone();
        }

        [TestCase(0, 2)]
        [TestCase(1, 1)]
        [TestCase(2, 0)]
        public void CheckoutTagFromEnd_Repository_Has_Expected_Hash(int index, int commitIdx)
        {
            var commit = Commit.All[commitIdx];
            _repository.CheckoutTagFromEnd(index);

            RepositoryHash().Should().Be(commit.Sha1Hash);
        }

        private string RepositoryHash()
        {
            var repository = new LibGit2Sharp.Repository(_repoPath);
            return repository.Commits.First().Sha;
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void CheckoutTag_Repository_Has_Expected_Has(int index)
        {
            var commit = Commit.All[index];
            _repository.CheckoutTag(commit.Tag);

            RepositoryHash().Should().Be(commit.Sha1Hash);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void CheckoutHash_Repository_Has_Expected_Has(int index)
        {
            var commit = Commit.All[index];
            _repository.CheckoutHash(commit.Sha1Hash);

            RepositoryHash().Should().Be(commit.Sha1Hash);
        }
        
        [TearDown]
        public void TearDown()
        {
            _repository.Dispose();
        }
    }
}
