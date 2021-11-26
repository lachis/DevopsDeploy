using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using DevopsDeploy.Configuration;
using DevopsDeploy.Domain;
using DevopsDeploy.Strategies;
using Xunit;

namespace DevopsDeploy.Tests.StrategyTests
{
    public class JsonFileReadTests
    {
        [Fact]
        public async Task Basic_ReadProjects_Success()
        {
            string fileName = "Assets/Projects.json";
            using FileStream openStream = File.OpenRead(fileName);
            var projects = 
                await JsonSerializer.DeserializeAsync<IEnumerable<Project>>(openStream);

            Assert.NotEmpty(projects);
        }
        
        [Fact]
        public async Task Basic_ReadProjects_CountIsCorrect()
        {
            string fileName = "Assets/Projects.json";
            using FileStream openStream = File.OpenRead(fileName);
            var projects = 
                await JsonSerializer.DeserializeAsync<IEnumerable<Project>>(openStream);

            Assert.Equal(2, projects.Count());
        }

        [Fact]
        public async Task SystemPathStrategy_ReadFile_NotEmpty()
        {
            SystemPathStrategy strategy = new(new FileConfiguration("Assets"));
            var projects = await strategy.Get<Project>("Projects.json");
            
            Assert.NotEmpty(projects);
        }
    }
}