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
    public class TayinTalepTercihManager : ITayinTalepTercihService
    {
        private readonly ITayinTalepTercihDal _tercihDal;

        public TayinTalepTercihManager(ITayinTalepTercihDal tercihDal)
        {
            _tercihDal = tercihDal;
        }

        public async Task AddAsync(TayinTalepTercih tercih)
        {
            await _tercihDal.AddAsync(tercih);
            await _tercihDal.SaveChangesAsync();
        }

        //bir kişi aynı adliyeyi birden fazlakez tercih edemez!
        public async Task<bool> AddWithValidationAsync(TayinTalepTercih yeniTercih)
        {
            var mevcutTercihler = await _tercihDal.FindAsync(x => x.TayinTalepId == yeniTercih.TayinTalepId);

            bool ayniAdliyeVarMi = mevcutTercihler.Any(x => x.AdliyeId == yeniTercih.AdliyeId);

            if (ayniAdliyeVarMi)
                return false; 

            await _tercihDal.AddAsync(yeniTercih);
            await _tercihDal.SaveChangesAsync();
            return true;
        }

        public async Task DeleteAsync(int id)
        {
            var tercih = await _tercihDal.GetByIdAsync(id);
            if (tercih != null)
            {
                _tercihDal.Remove(tercih);
                await _tercihDal.SaveChangesAsync();
            }
        }

        public async Task<List<TayinTalepTercih>> GetAllAsync()
        {
            return await _tercihDal.GetAllAsync();
        }

        public async Task<TayinTalepTercih> GetByIdAsync(int id)
        {
            return await _tercihDal.GetByIdAsync(id);
        }

        public async Task<List<TayinTalepTercih>> GetByTayinTalepIdAsync(int tayinTalepId)
        {
            return await _tercihDal.FindAsync(t => t.TayinTalepId == tayinTalepId);
        }

        public async Task UpdateAsync(TayinTalepTercih tercih)
        {
            _tercihDal.Update(tercih);
            await _tercihDal.SaveChangesAsync();
        }
    }
}
