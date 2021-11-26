namespace DevopsDeploy.Configuration
{
    public class FileConfiguration : ILocationConfiguration
    {
        public string Location { get; }

        public FileConfiguration(string location)
        {
            Location = location;
        }
    }

    public interface ILocationConfiguration
    {
        public string Location { get;  }
    }
}