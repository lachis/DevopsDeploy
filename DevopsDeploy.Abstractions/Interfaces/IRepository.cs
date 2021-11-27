using System.Collections.Generic;
using System.Threading.Tasks;

namespace DevopsDeploy.Abstractions.Interfaces
{
    public interface IRepository
    {
        Task<IEnumerable<T>> Get<T>(string path) where T : class;
    }
}