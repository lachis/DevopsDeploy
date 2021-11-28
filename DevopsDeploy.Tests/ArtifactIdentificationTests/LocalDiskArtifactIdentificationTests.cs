using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Domain.Models;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.ArtifactIdentificationTests
{
    public class LocalDiskArtifactIdentificationTests : IAsyncLifetime
    {
        private IEnumerable<ReleaseIdentification> _objectUnderTest;

        public async Task InitializeAsync()
        {
            LocalDiskArtifactRepository repository = new(new TestFileConfiguration());
            LocalDiskArtifactIdentification artifactIdentification =
                new(repository, "TestReleases-3.json", "TestDeployments-3.json");

            _objectUnderTest = await artifactIdentification.Identify();
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
            Assert.Equal(6, _objectUnderTest.Count());
        }
        
        [Theory]
        [InlineData("Project-1", "Environment-1")]
        [InlineData("Project-2", "Environment-1")]
        [InlineData("Project-3", "Environment-1")]
        [InlineData("Project-4", "Environment-1")]
        [InlineData("Project-3", "Environment-2")]
        [InlineData("Project-4", "Environment-2")]
        public void ResultContainsKey(string p, string e)
        {
            Assert.Contains(_objectUnderTest, x => x.Key.EnvironmentId == e && x.Key.ProjectId == p);
        }
        
           
        [Theory]
        [InlineData("Project-1", "Environment-2")]
        [InlineData("Project-2", "Environment-2")]
        [InlineData("Project-5", "Environment-1")]
        [InlineData("Project-5", "Environment-2")]
        public void ResultDoesNotContainKey(string p, string e)
        {
            Assert.DoesNotContain(_objectUnderTest, x => x.Key.EnvironmentId == e && x.Key.ProjectId == p);
        }
    }
}