using System;
using System.IO;
using FluentAssertions;
using NUnit.Framework;
using TestDiscover.Lib;

namespace TestDiscover.Tests
{
    [TestFixture]
    public class SolutionScannerTests
    {
        private Repository _repository;
        private string _solutionPath;

        [SetUp]
        public void Setup()
        {
            _repository = new Repository(Constants.RepoPath);
            _solutionPath = Path.Combine(_repository.Clone(), Constants.Solution);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        public void Scan_Should_Return_Expected_Functions(int idx)
        {
            var commit = Commit.All[idx];
            CheckoutRepository(commit.Sha1Hash);
            var scanner = new SolutionScanner(_solutionPath);

            var result = scanner.Scan();

            result.Count.Should().Be(commit.Functions.Count);
            result.Should().Contain(commit.Functions);

        }
        private void CheckoutRepository(string hash)
        {
            _repository.CheckoutHash(hash);
        }

        [TearDown]
        public void TearDown()
        {
            _repository.Dispose();
        }
    }
}