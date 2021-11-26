using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Core;
using DevopsDeploy.Domain;
using DevopsDeploy.Strategies;
using DevopsDeploy.Tests.Configuration;
using Xunit;

namespace DevopsDeploy.Tests.BusinessLogicValidationTests
{
    public class UseCase2Projects1EnvironmentKeep2ReleasesTests : IAsyncLifetime
    {
        private IEnumerable<ReleaseIdentification> _objectUnderTest;

        [Fact]
        public void ReleaseIdentificationCount_GroupedByProjectEnvironment_CountIsCorrect()
        {
            Assert.Equal(2, _objectUnderTest.Count());
        }
        //
        // [Fact]
        // public void ReturnedRelease_ReleaseId_IsCorrect()
        // {
        //     Assert.Equal("Release-1", _objectUnderTest.First().Item1.Id);
        // }
        

        public async Task InitializeAsync()
        {
            SomeRepoClass strategy = new(new SystemPathStrategy(new TestFileConfiguration()),
                "TestReleases-2.json", "TestDeployments-2.json");
            _objectUnderTest = await strategy.Identify(2);
        }

        public Task DisposeAsync()
        {
            return Task.CompletedTask;
        }
    }
}