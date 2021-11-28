using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Core.RetentionPolicies;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.BusinessLogicValidationTests
{
    public class UseCase2Projects1EnvironmentKeep2ReleasesTests : IAsyncLifetime
    {
        private List<ReleaseDTO> _objectUnderTest;
        
        [Fact]
        public void ReleaseIdentificationCount_GroupedByProjectEnvironment_CountIsCorrect()
        {
            Assert.Equal(4, _objectUnderTest.Count);
        }
        
        [Fact]
        public void ReturnedRelease_ReleaseId_IsCorrect()
        {
            Assert.Equal("Release-4", _objectUnderTest.First().ReleaseId);
        }
        
        [Fact]
        public void RetainedReleases_ProjectIds_AreCorrect()
        {
            var uniqueProjectIds = _objectUnderTest.Select(x=>x.ProjectId).Distinct().ToList();
            Assert.Contains(uniqueProjectIds, (x) => x == "Project-1");
            Assert.Contains(uniqueProjectIds, (x) => x == "Project-2");
        }

        public async Task InitializeAsync()
        {
            LocalDiskArtifactIdentification strategy = new(new LocalDiskArtifactRepository(new TestFileConfiguration()),
                new StandardArtifactGrouping(),
                "TestReleases-2.json", "TestDeployments-2.json");
            var identifiedRleases = await strategy.Identify();
            
            var releases = identifiedRleases
                .ToDictionary(x => x.Key, x => x.Releases.ToList());
            var policy = new StandardReleaseRetentionPolicy();
            _objectUnderTest = policy.ApplyPolicy(releases, 2);
            

        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}