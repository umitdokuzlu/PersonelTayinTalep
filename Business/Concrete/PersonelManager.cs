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
    public class PersonelManager : IPersonelService
    {
        private readonly IPersonelDal _personelDal;

        public PersonelManager(IPersonelDal personelDal)
        {
            _personelDal = personelDal;
        }

        public async Task AddAsync(Personel personel)
        {
            await _personelDal.AddAsync(personel);
            await _personelDal.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var personel = await _personelDal.GetByIdAsync(id);
            if (personel != null)
            {
                _personelDal.Remove(personel);
                await _personelDal.SaveChangesAsync();
            }
        }

        public async Task<List<Personel>> GetAllAsync()
        {
            return await _personelDal.GetAllAsync();
        }

        public async Task<Personel> GetByIdAsync(int id)
        {
            return await _personelDal.GetByIdAsync(id);
        }

        public async Task<Personel> GetBySicilNoAsync(string sicilNo)
        {
            return await _personelDal.GetBySicilNoAsync(sicilNo);
        }

        public async Task UpdateAsync(Personel personel)
        {
            _personelDal.Update(personel);
            await _personelDal.SaveChangesAsync();
        }
    }
}
