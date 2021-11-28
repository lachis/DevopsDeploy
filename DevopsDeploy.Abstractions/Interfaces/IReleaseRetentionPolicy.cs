﻿using System.Collections.Generic;
using DevopsDeploy.Domain.DTO;
using DevopsDeploy.Domain.Models;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IReleaseRetentionPolicy
    {
        List<ReleaseDTO> ApplyPolicy(Dictionary<(string ProjectId, string EnvironmentId), List<ReleaseDTO>> releases,
            int numReleases);
    }
}