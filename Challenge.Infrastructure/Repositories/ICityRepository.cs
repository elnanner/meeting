using Challenge.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Infrastructure.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAll();
    }
}