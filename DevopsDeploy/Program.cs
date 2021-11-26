using System.Linq;
using System.Threading.Tasks;
using DevopsDeploy.Configuration;
using DevopsDeploy.Domain;
using DevopsDeploy.Strategies;

namespace DevopsDeploy
{
    class Program
    {
        static async Task Main(string[] args)
        {
            // autofac
            // main flow is a loop or a scheduled task
            // while the task asks to read from disk an extension would be to make this a strategy
            
            /*
             * For each project/environment combination, keep n releases that have most recently been deployed, 
            where n is the number of releases to keep.
            note: A release is considered to have "been deployed" if the release has one or more deployments
             */
            
            SystemPathStrategy strategy = new(new FileConfiguration("Assets"));
            var projects = await strategy.Get<Project>("Releases.json");
            var environments = await strategy.Get<Environment>("Deployments.json");

            // projects.Join(environments, p => p.Id, e => e.Id)

            
        }
    }
}