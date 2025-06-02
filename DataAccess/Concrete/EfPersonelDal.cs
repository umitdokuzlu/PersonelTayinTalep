using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repositories.Concrete;
using Entities.Concrete;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfPersonelDal : GenericRepository<Personel>, IPersonelDal
    {
        private readonly AppDbContext _context;

        public EfPersonelDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Personel> GetBySicilNoAsync(string sicilNo)
        {
            return await _context.Personeller.FirstOrDefaultAsync(p=>p.SicilNo == sicilNo);
        }
    }
}
