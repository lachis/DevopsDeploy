using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Domain.Models;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.BusinessLogic
{
    public class BusinessUseCase1TestsNoInput : IAsyncLifetime
    {
        private IEnumerable<(Release, Deployment)> _objectUnderTest;

        [Fact]
        public void DeployedReleasesCount_IsCorrect()
        {
            Assert.Single(_objectUnderTest);
        }
        
        [Fact]
        public void ReturnedRelease_ReleaseId_IsCorrect()
        {
            Assert.Equal("Release-1", _objectUnderTest.First().Item1.Id);
        }
        
        [Fact]
        public async Task ReturnedDeployment_DeploymentId_IsCorrect()
        {
            Assert.Equal("Deployment-1", _objectUnderTest.First().Item2.Id);
        }

        public async Task InitializeAsync()
        {
            LocalDiskArtifactRepository strategy = new(new TestFileConfiguration());
            var releases = await strategy.Get<Release>("TestReleases-1.json");
            var deployments = await strategy.Get<Deployment>("TestDeployments-1.json");

            _objectUnderTest = (from r in releases 
                join d in deployments on r.Id equals d.ReleaseId
                select (r, d )).ToList();
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}