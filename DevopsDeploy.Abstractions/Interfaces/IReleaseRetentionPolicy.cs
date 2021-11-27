using System.Collections.Generic;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IReleaseRetentionPolicy
    {
        List<Release> ApplyPolicy(Dictionary<(string, string), List<(Release r,Deployment d)>> releases, int numReleases);
    }
}