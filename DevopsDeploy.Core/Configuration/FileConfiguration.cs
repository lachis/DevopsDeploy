using DevopsDeploy.Abstractions.Interfaces;

namespace DevopsDeploy.Core.Configuration
{
    public class FileConfiguration : ILocationConfiguration
    {
        public string Location { get; }

        public FileConfiguration(string location)
        {
            Location = location;
        }
    }
}