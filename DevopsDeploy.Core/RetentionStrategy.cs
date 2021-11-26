using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Domain;
using DevopsDeploy.Strategies;

namespace DevopsDeploy.Core
{
    public class SomeRepoClass : ISomeRepo
    {
        private readonly IAccessStrategy _accessStrategy;
        private readonly string _tempReleasePath;
        private readonly string _tempDeploymentsPath;

        public SomeRepoClass(IAccessStrategy accessStrategy, string tempReleasePath, string tempDeploymentsPath)
        {
            _accessStrategy = accessStrategy;
            _tempReleasePath = tempReleasePath;
            _tempDeploymentsPath = tempDeploymentsPath;
        }

        public async Task<ReleaseRetention> Identify(int numReleases)
        {
            var releases = await _accessStrategy.Get<Release>(_tempReleasePath);
            var deployments = await _accessStrategy.Get<Deployment>(_tempDeploymentsPath);

            var identifiedReleases = from r in releases
                join d in deployments on r.Id equals d.ReleaseId
                orderby d.DeployedAt descending 
                group (r, d) by (r.ProjectId, d.EnvironmentId)
                into grouping
                select new ReleaseIdentification(grouping.Key, grouping.ToList());

            return new ReleaseRetention(identifiedReleases, numReleases);
        }
    }

    public class ReleaseRetention
    {
        private readonly IEnumerable<ReleaseIdentification> _identifiedReleases;
        private readonly int _numReleases;

        public ReleaseRetention(IEnumerable<ReleaseIdentification> identifiedReleases, int numReleases)
        {
            _identifiedReleases = identifiedReleases;
            _numReleases = numReleases;
        }
        
        public void ApplyRetentionPolicy()
        {
            // when applying retention logic
            // if we say take 2 releases
            // and there are 3 deployments, r1, r1, r3
            // only one of r1 would be taken (assumption to document)
        }
    }


    public interface ISomeRepo
    {
        Task<ReleaseRetention> Identify(int numReleases);
    }
}