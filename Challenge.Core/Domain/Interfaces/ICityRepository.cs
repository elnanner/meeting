using Challenge.Core.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Challenge.Core.Domain.Interfaces
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetAll();// TODO
    }
}