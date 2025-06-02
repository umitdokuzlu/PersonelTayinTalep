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
    public class AdliyeManager : IAdliyeService
    {
        private readonly IAdliyeDal _adliyeDal;

        public AdliyeManager(IAdliyeDal adliyeDal)
        {
            _adliyeDal = adliyeDal;
        }

        public async Task AddAsync(Adliye adliye)
        {
            await _adliyeDal.AddAsync(adliye);
            await _adliyeDal.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var adliye = await _adliyeDal.GetByIdAsync(id);
            if (adliye != null)
            {
                _adliyeDal.Remove(adliye);
                await _adliyeDal.SaveChangesAsync();
            }
        }

        public async Task<List<Adliye>> GetAllAsync()
        {
            return await _adliyeDal.GetAllAsync();
        }

        public async Task<Adliye> GetByIdAsync(int id)
        {
            return await _adliyeDal.GetByIdAsync(id);
        }

        public async Task UpdateAsync(Adliye adliye)
        {
            _adliyeDal.Update(adliye);
            await _adliyeDal.SaveChangesAsync();
        }
    }
}
