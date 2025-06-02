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
    public class EfTayinTalepDal : GenericRepository<TayinTalep>, ITayinTalepDal
    {
        private readonly AppDbContext _context;
        public EfTayinTalepDal(AppDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<TayinTalep>> GetAllWithIncludesAsync()
        {
            return await _context.TayinTalepleri
            .Include(x => x.Personel)
            .Include(x => x.Tercihler)
                .ThenInclude(t => t.Adliye)
            .ToListAsync();
        }

        //ihtiyaç halinde sorgular eklenebilecek
    }
}
