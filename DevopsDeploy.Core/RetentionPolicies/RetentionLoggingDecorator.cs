using System.Collections.Generic;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class RetentionLoggingDecorator : IReleaseRetentionPolicy
    {
        private readonly IReleaseRetentionPolicy _decoratee;

        public RetentionLoggingDecorator(IReleaseRetentionPolicy decoratee )
        {
            _decoratee = decoratee;
        }

        public List<Release> ApplyPolicy(Dictionary<(string, string), List<(Release r, Deployment d)>> releases, int numReleases)
        {
            return _decoratee.ApplyPolicy(releases, numReleases);
        }
    }
}