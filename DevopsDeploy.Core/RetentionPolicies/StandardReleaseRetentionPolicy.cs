using System.Collections.Generic;
using System.Linq;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.Equality;
using DevopsDeploy.Domain.DTO;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class StandardReleaseRetentionPolicy : IReleaseRetentionPolicy
    {
        /// <summary>
        /// Applies the Standard Retention policy to identified releases grouped by Project/Environment
        /// The Policy will compare the release by Ids to ensure we keep unique releases and
        /// keep the specified numReleases
        /// </summary>
        /// <param name="releases">The releases to apply the policy to</param>
        /// <param name="numReleases">The number of releases to keep per group</param>
        /// <returns>The releases that meet the retention policy</returns>
        public List<ReleaseDTO> ApplyPolicy(
            Dictionary<(string ProjectId, string EnvironmentId), List<ReleaseDTO>> releases, int numReleases)
        {
            Dictionary<(string, string), List<ReleaseDTO>> result = new();
            foreach (var (key, value) in releases)
            {
                var finalReleases = value
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