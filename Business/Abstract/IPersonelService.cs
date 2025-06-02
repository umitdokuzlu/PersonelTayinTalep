using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Abstract
{
    public interface IPersonelService
    {
        Task<List<Personel>> GetAllAsync();
        Task<Personel> GetByIdAsync(int id);
        Task<Personel> GetBySicilNoAsync(string sicilNo);
        Task AddAsync(Personel personel);
        Task UpdateAsync(Personel personel);
        Task DeleteAsync(int id);
    }
}
