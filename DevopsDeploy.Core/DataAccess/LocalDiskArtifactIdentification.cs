using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.DataAccess
{
    public class LocalDiskArtifactIdentification : IArtifactIdentification
    {
        private readonly IRepository _repository;
        private readonly string _releasesPath;
        private readonly string _deploymentsPath;

        public LocalDiskArtifactIdentification(IRepository repository, string releasesPath, string deploymentsPath)
        {
            _repository = repository;
            _releasesPath = releasesPath;
            _deploymentsPath = deploymentsPath;
        }

        public async Task<IEnumerable<ReleaseIdentification>> Identify()
        {
            var releases = await _repository.Get<Release>(_releasesPath);
            var deployments = await _repository.Get<Deployment>(_deploymentsPath);

            return from r in releases
                join d in deployments on r.Id equals d.ReleaseId
                orderby d.DeployedAt descending 
                group (r, d) by (r.ProjectId, d.EnvironmentId)
                into grouping
                select new ReleaseIdentification(grouping.Key, grouping.ToList());

            // return new StandardReleaseRetentionPolicy(identifiedReleases, numReleases);
        }
    }
}