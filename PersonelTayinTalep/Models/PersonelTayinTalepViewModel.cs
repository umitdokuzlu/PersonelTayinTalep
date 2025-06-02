using Entities.Concrete;
using System.Collections.Generic;

namespace PersonelTayinTalep.Models
{
    public class PersonelTayinTalepViewModel
    {
        public Personel Personel { get; set; }
        public List<TayinTalep> Talepler { get; set; }
    }
}
