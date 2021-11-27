using System.Collections.Generic;
using System.Linq;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.Equality;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class StandardReleaseRetentionPolicy : IReleaseRetentionPolicy
    {
        public List<Release> ApplyPolicy(Dictionary<(string, string), List<(Release r,Deployment d)>> releases, int numReleases)
        {
            Dictionary<(string, string), List<Release>> result = new();
            foreach (var (key, value) in releases)
            {
                var finalReleases = value
                    .Select(x => x.r)
                    .Distinct(new ReleaseComparer())
                    .Take(numReleases)
                    .ToList();

                result.TryAdd(key, finalReleases);
            }

           return result.SelectMany(x => x.Value)
                .ToList();
        }
    }
}