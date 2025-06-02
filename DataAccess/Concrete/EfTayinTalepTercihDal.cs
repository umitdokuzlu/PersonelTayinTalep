using DataAccess.Abstract;
using DataAccess.Context;
using DataAccess.Repositories.Concrete;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Concrete
{
    public class EfTayinTalepTercihDal : GenericRepository<TayinTalepTercih>, ITayinTalepTercihDal
    {
        public EfTayinTalepTercihDal(AppDbContext context) : base(context)
        {
        }

        //ihtiyaç halinde sorgular eklenebilecek
    }
}
