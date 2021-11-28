using System.Collections.Generic;
using System.Threading.Tasks;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IArtifactIdentification
    {
        Task<IEnumerable<ReleaseIdentification>> Identify();
    }

    public interface IArtifactGrouping
    {
        IEnumerable<ReleaseIdentification> GroupArtifacts(IEnumerable<Release> releases,
            IEnumerable<Deployment> deployments);
    }
}