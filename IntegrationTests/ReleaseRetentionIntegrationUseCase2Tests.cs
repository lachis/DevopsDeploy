using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.Configuration;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Core.Retention;
using DevopsDeploy.Core.RetentionPolicies;
using DevopsDeploy.Domain.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;

namespace IntegrationTests
{
    public class ReleaseRetentionIntegrationUseCase2Tests : IAsyncLifetime
    {
        private IContainer _container;
        private IReadOnlyList<ReleaseDTO> _objectUnderTest;

        [Fact]
        public void ReleasesRetainedIsNotNull()
        {
            Assert.NotNull(_objectUnderTest);
        }
        
        [Fact]
        public void ReleasesRetainedIsNotEmpty()
        {
            Assert.NotEmpty(_objectUnderTest);
        }
        
        [Fact]
        public void ReleasesRetainedIsLessOrEqualToInputParamForAllGroups()
        {
            var dictionary = _objectUnderTest.GroupBy(x=>x.Key, x=>x).ToDictionary(x=>x.Key, x=>x.ToList());

            foreach (var group in dictionary)
            {
                Assert.True(group.Value.Count <= 1);
            }
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
            Assert.Contains(_objectUnderTest, x => x.EnvironmentId == e && x.ProjectId == p);
        }
              
        [Theory]
        [InlineData("Project-1", "Environment-2")]
        [InlineData("Project-2", "Environment-2")]
        [InlineData("Project-5", "Environment-1")]
        [InlineData("Project-5", "Environment-2")]
        public void ResultDoesNotContainKey(string p, string e)
        {
            Assert.DoesNotContain(_objectUnderTest, x => x.EnvironmentId == e && x.ProjectId == p);
        }

        public async Task InitializeAsync()
        {
            ServiceCollection serviceCollection = new();
            serviceCollection.AddLogging(ctx => ctx.AddConsole());
            ContainerBuilder containerBuilder = new();
            containerBuilder.Populate(serviceCollection);
            containerBuilder.Register(_ => new FileConfiguration("Assets"))
                .As<ILocationConfiguration>();
            
            containerBuilder.RegisterType<LocalDiskArtifactRepository>()
                .As<IRepository>();

            containerBuilder.RegisterType<RetainedReleases>();
            containerBuilder.RegisterType<StandardReleaseRetentionPolicy>()
                .As<IReleaseRetentionPolicy>();
            
            containerBuilder.RegisterType<StandardArtifactGrouping>()
                .As<IArtifactGrouping>();
            
            containerBuilder.Register(ctx => 
                    new LocalDiskArtifactIdentification(ctx.Resolve<IRepository>(), ctx.Resolve<IArtifactGrouping>(), "TestReleases-3.json", "TestDeployments-3.json"))
                .As<IArtifactIdentification>();

            containerBuilder.RegisterDecorator<RetentionLoggingDecorator, IReleaseRetentionPolicy>();

            containerBuilder.RegisterType<ReleaseRetention>();
            _container = containerBuilder.Build();

            await using ILifetimeScope lifetimeScope = _container.BeginLifetimeScope();

            var releaseRetention = lifetimeScope.Resolve<ReleaseRetention>();
            _objectUnderTest = await releaseRetention.Retain(1);
        }

        public async Task DisposeAsync()
        {
            await _container.DisposeAsync();
        }
    }
}