using System.Collections.Generic;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IReleaseIdentificationPolicy
    {
        IEnumerable<ReleaseIdentification> ApplyPolicy(IEnumerable<Release> releases,
            IEnumerable<Deployment> deployments);
    }
}