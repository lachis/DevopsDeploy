using System.Collections.Generic;
using System.Linq;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Core.Equality;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class StandardReleaseRetentionPolicy : IReleaseRetentionPolicy
    {
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