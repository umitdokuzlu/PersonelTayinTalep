using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface ITayinTalepService
    {
        Task<List<TayinTalep>> GetAllAsync();
        Task<TayinTalep> GetByIdAsync(int id);
        Task<List<TayinTalep>> GetByPersonelIdAsync(int personelId);
        Task AddAsync(TayinTalep talep);
        Task UpdateAsync(TayinTalep talep);
        Task DeleteAsync(int id);
        Task<List<TayinTalep>> GetAllWithDetailsAsync();
    }
}
