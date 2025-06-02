using DataAccess.Repositories.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Abstract
{
    public interface ITayinTalepDal:IGenericRepository<TayinTalep>
    {
        //ihtiyaç halinde sorgular eklenebilecek
        Task<List<TayinTalep>> GetAllWithIncludesAsync();
    }
}
