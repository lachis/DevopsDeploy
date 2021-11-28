using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Domain.DTO
{
    public class ReleaseDTO
    {
        public string Key => $"{ProjectId}:{EnvironmentId}";
        public string ProjectId => Release.ProjectId;
        public string EnvironmentId => Deployment.EnvironmentId;
        
        public string ReleaseId => Release.Id;
        public Release Release { get; }
        public Deployment Deployment { get; }

        public ReleaseDTO(Release release, Deployment deployment)
        {
            Release = release;
            Deployment = deployment;
        }
    }
}