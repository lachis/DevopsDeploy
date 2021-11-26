using System;
using System.Collections.Generic;
using System.Linq;

namespace DevopsDeploy.Domain
{
    public class Environment
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Deployment
    {
        public string Id { get; set; }
        public string ReleaseId { get; set; }
        public string EnvironmentId { get; set; }
        public string DeployedAt { get; set; }
    }
    
    public class Project
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class Release
    {
        public string Id { get; set; }
        public string ProjectId { get; set; }
        public string Version { get; set; }
        public DateTimeOffset Created { get; set; }
    }

    public class ReleaseIdentification
    {
        private readonly int _numReleases;

        public ReleaseIdentification((string ProjectId, string EnvironmentId) key, List<(Release r, Deployment d)> grouping)
        {
            Key = key;
            Grouping = grouping; 
        }

        public IEnumerable<Release> Releases { get; }

        public (string ProjectId, string EnvironmentId) Key { get; }
        public List<(Release r, Deployment d)> Grouping { get; }
    }
}