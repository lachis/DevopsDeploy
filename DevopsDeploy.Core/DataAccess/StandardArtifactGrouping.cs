using System.Collections.Generic;
using System.Linq;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.DataAccess
{
    public class StandardArtifactGrouping : IArtifactGrouping
    {
        public IEnumerable<ReleaseIdentification> GroupArtifacts(IEnumerable<Release> releases, IEnumerable<Deployment> deployments)
        {
            return from r in releases
                join d in deployments on r.Id equals d.ReleaseId
                orderby d.DeployedAt descending
                group (r, d) by (r.ProjectId, d.EnvironmentId)
                into g
                select new ReleaseIdentification(g.Key, g.Select(x => new ReleaseDTO(x.r, x.d)));
        }
    }
}