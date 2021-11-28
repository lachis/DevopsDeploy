using System.Collections.Generic;
using System.Threading.Tasks;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.RetentionPolicies;
using DevopsDeploy.Domain.DTO;

namespace DevopsDeploy.Core.Retention
{
    public class ReleaseRetention
    {
        private readonly IArtifactIdentification _artifactProvider;
        private readonly RetainedReleases.Factory _retentionPolicyFactory;

        public ReleaseRetention(IArtifactIdentification artifactProvider, RetainedReleases.Factory retentionPolicyFactory)
        {
            _artifactProvider = artifactProvider;
            _retentionPolicyFactory = retentionPolicyFactory;
        }

        public async Task<IReadOnlyList<ReleaseDTO>> Retain(int releasesToRetain)
        {
            var identifiedReleases = await _artifactProvider.Identify();
            var retentionPolicyFactory = _retentionPolicyFactory(identifiedReleases, releasesToRetain);
            retentionPolicyFactory.ApplyPolicy();

            return retentionPolicyFactory.Result();
        }
    }
}