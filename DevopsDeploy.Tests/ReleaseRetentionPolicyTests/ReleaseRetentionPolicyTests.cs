using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Core.RetentionPolicies;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.ReleaseRetentionPolicyTests
{
    public class ReleaseRetentionPolicyTests : IAsyncLifetime
    {
        private List<ReleaseDTO> _result;
        private List<string> _releaseIds;

        public async Task InitializeAsync()
        {
            var tempReleasePath = "TestReleases-2.json";
            var tempDeploymentsPath = "TestDeployments-2.json";
            LocalDiskArtifactRepository accessStrategy = new(new TestFileConfiguration());
            var releases = await accessStrategy.Get<Release>(tempReleasePath);
            var deployments = await accessStrategy.Get<Deployment>(tempDeploymentsPath);

            var identifiedReleases = from r in releases
                join d in deployments on r.Id equals d.ReleaseId
                orderby d.DeployedAt descending 
                group (r, d) by (r.ProjectId, d.EnvironmentId)
                into grouping
                select new ReleaseIdentification(grouping.Key, grouping.Select(x=>new ReleaseDTO(x.r, x.d)));

            var input = identifiedReleases
                .ToDictionary(x => x.Key, x => x.Releases.ToList());
            StandardReleaseRetentionPolicy policy = new();
            _result = policy.ApplyPolicy(input, 2);
             _releaseIds = _result.Select(x=>x.ReleaseId).ToList();

        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
        
        [Fact]
        public void ReleaseRetention_Count_IsCorrect()
        {
            Assert.Equal(4, _result.Count);
        }
        
        [Fact]
        public void RetainedReleases_For2Projects()
        {
            var uniqueProjectIds = _result.Select(x=>x.ProjectId).Distinct().ToList();
            Assert.Equal(2, uniqueProjectIds.Count);
        }
        
        [Fact]
        public void RetainedReleases_ProjectIds_AreCorrect()
        {
            var uniqueProjectIds = _result.Select(x=>x.ProjectId).Distinct().ToList();
            Assert.Contains(uniqueProjectIds, (x) => x == "Project-1");
            Assert.Contains(uniqueProjectIds, (x) => x == "Project-2");
        }
        
        [Fact]
        public void RetainedReleases_DoesNotContain_Project3()
        {
            var uniqueProjectIds = _result.Select(x=>x.ProjectId).Distinct().ToList();
            Assert.DoesNotContain(uniqueProjectIds, (x) => x == "Project-3");
        }
        
        [Fact]
        public void RetainedReleases_Contains_Release1()
        {
            Assert.Contains(_releaseIds, (x) => x == "Release-1");
        }
        
        [Fact]
        public void RetainedReleases_Contains_Release2()
        {
            Assert.Contains(_releaseIds, (x) => x == "Release-2");
        }
        
        [Fact]
        public void RetainedReleases_Contains_Release3()
        {
            Assert.Contains(_releaseIds, (x) => x == "Release-3");
        }
        
        [Fact]
        public void RetainedReleases_Contains_Release4()
        {
            Assert.Contains(_releaseIds, (x) => x == "Release-4");
        }
    }
}