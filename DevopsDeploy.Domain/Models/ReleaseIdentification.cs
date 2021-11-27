using System.Collections.Generic;

namespace DevopsDeploy.Domain.Models
{
    public class ReleaseIdentification
    {
        public ReleaseIdentification((string ProjectId, string EnvironmentId) key, List<(Release r, Deployment d)> grouping)
        {
            Key = key;
            Grouping = grouping; 
        }

        public (string ProjectId, string EnvironmentId) Key { get; }
        public List<(Release r, Deployment d)> Grouping { get; }
    }
}