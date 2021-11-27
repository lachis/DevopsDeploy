using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DevopsDeploy.Abstractions.Interfaces;

namespace DevopsDeploy.Core.DataAccess
{
    public class LocalDiskArtifactRepository : IRepository
    {
        private readonly ILocationConfiguration _locationConfiguration;

        public LocalDiskArtifactRepository(ILocationConfiguration locationConfiguration)
        {
            _locationConfiguration = locationConfiguration;
        }

        /// <summary>
        /// Attempts to retrieve a file from a specified system file location
        /// </summary>
        /// <param name="path">The file path</param>
        /// <typeparam name="T">The Serialized Output type from the json string</typeparam>
        /// <returns>A collection of T</returns>
        public async Task<IEnumerable<T>> Get<T>(string path) where T : class
        {
            await using FileStream openStream = File.OpenRead($"{_locationConfiguration.Location}/{path}");
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(openStream);
        }
    }
}