using System.Collections.Generic;
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
        private readonly IArtifactGrouping _standardArtifactGrouping;

        public LocalDiskArtifactIdentification(IRepository repository, IArtifactGrouping artifactGrouping, string releasesPath, string deploymentsPath)
        {
            _repository = repository;
            _releasesPath = releasesPath;
            _deploymentsPath = deploymentsPath;
            _standardArtifactGrouping = artifactGrouping;
        }

        public async Task<IEnumerable<ReleaseIdentification>> Identify()
        {
            var releases = await _repository.Get<Release>(_releasesPath);
            var deployments = await _repository.Get<Deployment>(_deploymentsPath);

            return _standardArtifactGrouping.GroupArtifacts(releases, deployments);
        }
    }
}