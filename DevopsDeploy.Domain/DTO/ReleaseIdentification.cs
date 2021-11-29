using System.Collections.Generic;

namespace DevopsDeploy.Domain.DTO
{
    public class ReleaseIdentification
    {
        public ReleaseIdentification((string ProjectId, string EnvironmentId) key, IEnumerable<ReleaseDTO> releases)
        {
            Key = key;
            Releases = releases; 
        }

        public (string ProjectId, string EnvironmentId) Key { get; }
        public IEnumerable<ReleaseDTO> Releases { get; }
    }
}