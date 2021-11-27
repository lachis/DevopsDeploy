using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Domain.Models;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.RepositoryTests
{
    public class LocalDiskArtifactRepositoryTests : IAsyncLifetime
    {
        private IEnumerable<Project> _objectUnderTest;

        public async Task InitializeAsync()
        {
            LocalDiskArtifactRepository repository = new(new TestFileConfiguration());
            _objectUnderTest = await repository.Get<Project>("TestProjects-2.json");
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }

        [Fact]
        public void ResultIsNotNull()
        {
            Assert.NotNull(_objectUnderTest);
        }
        
        [Fact]
        public void ResultCountIsCorrect()
        {
            Assert.Equal(2, _objectUnderTest.Count());
        }
        
        [Fact]
        public void FirstReturnedIdIsCorrect()
        {
            Assert.Equal("Project-1", _objectUnderTest.First().Id);
        }
        
        [Fact]
        public void FirstReturnedNameIsCorrect()
        {
            Assert.Equal("Random Quotes", _objectUnderTest.First().Name);
        }
        
        [Fact]
        public void SecondReturnedIdIsCorrect()
        {
            Assert.Equal("Project-2", _objectUnderTest.Last().Id);
        }
        
        [Fact]
        public void SecondReturnedNameIsCorrect()
        {
            Assert.Equal("Pet Shop", _objectUnderTest.Last().Name);
        }
        
        [Fact]
        public void TypeIsCorrect()
        {
            Assert.Equal(typeof(Project), _objectUnderTest.First().GetType());
        }
    }
}