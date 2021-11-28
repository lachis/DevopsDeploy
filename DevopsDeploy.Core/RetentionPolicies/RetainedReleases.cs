using System.Collections.Generic;
using System.Linq;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class RetainedReleases 
    {
        private readonly IEnumerable<ReleaseIdentification> _identifiedReleases;
        private readonly int _numReleases;
        private readonly IReleaseRetentionPolicy _policy;
        private List<ReleaseDTO> _result;

        public delegate RetainedReleases Factory(IEnumerable<ReleaseIdentification> identifiedReleases, int numReleases);
        
        public RetainedReleases(IEnumerable<ReleaseIdentification> identifiedReleases, int numReleases, IReleaseRetentionPolicy policy)
        {
            _identifiedReleases = identifiedReleases;
            _numReleases = numReleases;
            _policy = policy;
        }
        
        public void ApplyPolicy()
        {
            var releases = _identifiedReleases
                .ToDictionary(x => x.Key, x => x.Releases.ToList());

            _result = _policy.ApplyPolicy(releases, _numReleases);
        }

        public List<ReleaseDTO> Result()
        {
            return _result;
        }
    }
}