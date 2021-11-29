using System.Collections.Generic;
using DevopsDeploy.Abstractions.Interfaces;
using DevopsDeploy.Domain.DTO;
using Microsoft.Extensions.Logging;

namespace DevopsDeploy.Core.RetentionPolicies
{
    public class RetentionLoggingDecorator : IReleaseRetentionPolicy
    {
        private readonly IReleaseRetentionPolicy _decoratee;
        private readonly ILogger<RetentionLoggingDecorator> _logger;

        public RetentionLoggingDecorator(IReleaseRetentionPolicy decoratee, ILogger<RetentionLoggingDecorator> logger)
        {
            _decoratee = decoratee;
            _logger = logger;
        }

        public List<ReleaseDTO> ApplyPolicy(
            Dictionary<(string ProjectId, string EnvironmentId), List<ReleaseDTO>> releases, int numReleases)
        {
            var validReleases = _decoratee.ApplyPolicy(releases, numReleases);

            foreach (var logOutput in validReleases)
            {
                _logger.LogInformation("{key}: {release} has met criteria (Top {n}) deployed release in Project/Environment.",
                    logOutput.Key, logOutput.ReleaseId, numReleases.ToString());
            }

            return validReleases;
        }
    }
}