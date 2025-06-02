using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITayinTalepTercihService
    {
        Task<List<TayinTalepTercih>> GetAllAsync();
        Task<TayinTalepTercih> GetByIdAsync(int id);
        Task<List<TayinTalepTercih>> GetByTayinTalepIdAsync(int tayinTalepId);
        Task AddAsync(TayinTalepTercih tercih);
        Task UpdateAsync(TayinTalepTercih tercih);
        Task DeleteAsync(int id);
        Task<bool> AddWithValidationAsync(TayinTalepTercih tercih);
    }
}
