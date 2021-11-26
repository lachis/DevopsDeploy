using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using DevopsDeploy.Configuration;
using DevopsDeploy.Domain;

namespace DevopsDeploy.Strategies
{
    public class SystemPathStrategy : IAccessStrategy
    {
        private readonly ILocationConfiguration _locationConfiguration;

        public SystemPathStrategy(ILocationConfiguration locationConfiguration)
        {
            _locationConfiguration = locationConfiguration;
        }

        /// <summary>
        /// Attempts to retrieve a file from a specified system file location
        /// </summary>
        /// <param name="path">The file path</param>
        /// <typeparam name="T">The Serialized Output type from the json string</typeparam>
        /// <returns></returns>
        public async Task<IEnumerable<T>> Get<T>(string path) where T : class
        {
            await using FileStream openStream = File.OpenRead($"{_locationConfiguration.Location}/{path}");
            return await JsonSerializer.DeserializeAsync<IEnumerable<T>>(openStream);
        }
    }

    public interface IAccessStrategy
    {
        Task<IEnumerable<T>> Get<T>(string path) where T : class;
    }
}