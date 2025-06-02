using Business.Abstract;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Concrete
{
    public class TayinTalepManager : ITayinTalepService
    {
        private readonly ITayinTalepDal _tayinTalepDal;

        public TayinTalepManager(ITayinTalepDal tayinTalepDal)
        {
            _tayinTalepDal = tayinTalepDal;
        }

        public async Task AddAsync(TayinTalep talep)
        {
            await _tayinTalepDal.AddAsync(talep);
            await _tayinTalepDal.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var talep = await _tayinTalepDal.GetByIdAsync(id);
            if (talep != null)
            {
                _tayinTalepDal.Remove(talep);
                await _tayinTalepDal.SaveChangesAsync();
            }
        }

        public async Task<List<TayinTalep>> GetAllAsync()
        {
            return await _tayinTalepDal.GetAllAsync();
        }

        public async Task<List<TayinTalep>> GetAllWithDetailsAsync()
        {
            return await _tayinTalepDal.GetAllWithIncludesAsync();
        }

        public async Task<TayinTalep> GetByIdAsync(int id)
        {
            return await _tayinTalepDal.GetByIdAsync(id);
        }

        public async Task<List<TayinTalep>> GetByPersonelIdAsync(int personelId)
        {
            return await _tayinTalepDal.FindAsync(t => t.PersonelId == personelId);
        }

        public async Task UpdateAsync(TayinTalep talep)
        {
            _tayinTalepDal.Update(talep);
            await _tayinTalepDal.SaveChangesAsync();
        }
    }
}
