using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.Configuration;
using DevopsDeploy.Core.DataAccess;
using DevopsDeploy.Core.Retention;
using DevopsDeploy.Core.RetentionPolicies;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace DevopsDeploy
{
    class Program
    {
        static async Task Main(string[] args)
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
            
            containerBuilder.RegisterType<StandardReleaseIdentificationPolicy>()
                .As<IReleaseIdentificationPolicy>();
            
            containerBuilder.Register(ctx => 
                    new LocalDiskArtifactIdentification(ctx.Resolve<IRepository>(), ctx.Resolve<IReleaseIdentificationPolicy>(), "Releases.json", "Deployments.json"))
                .As<IArtifactIdentification>();

            containerBuilder.RegisterDecorator<RetentionLoggingDecorator, IReleaseRetentionPolicy>();

            containerBuilder.RegisterType<ReleaseRetention>();
            var container = containerBuilder.Build();

            await using ILifetimeScope lifetimeScope = container.BeginLifetimeScope();

            var releaseRetention = lifetimeScope.Resolve<ReleaseRetention>();
            var retain = await releaseRetention.Retain(2);
            
        }
    }
}