using System.Collections.Generic;
using System.Threading.Tasks;
using DevopsDeploy.Domain.DTO;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IArtifactIdentification
    {
        Task<IEnumerable<ReleaseIdentification>> Identify();
    }
}