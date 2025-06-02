using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IAdliyeService
    {
        Task<List<Adliye>> GetAllAsync();
        Task<Adliye> GetByIdAsync(int id);
        Task AddAsync(Adliye adliye);
        Task UpdateAsync(Adliye adliye);
        Task DeleteAsync(int id);
    }
}
