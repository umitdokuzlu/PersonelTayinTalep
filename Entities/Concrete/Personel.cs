using Entities.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Concrete
{
    public class Personel : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string SicilNo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Adi { get; set; }

        [Required]
        [MaxLength(50)]
        public string Soyadi { get; set; }

        [Required]
        [MaxLength(50)]
        public string Unvan { get; set; }

        [Required]
        public byte[] SifreHash { get; set; }

        [Required]
        public byte[] SifreSalt { get; set; }

        public bool IsAdmin { get; set; } = false;

        public ICollection<TayinTalep> TayinTalepleri { get; set; }
    }
}
