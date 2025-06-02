using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities.Abstract;

namespace Entities.Concrete
{
    public class TayinTalep : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Personel")]
        public int PersonelId { get; set; }

        
        [Required(ErrorMessage = "Talep Türü alanı zorunludur.")]
        [MaxLength(50)]
        public string TalepTuru { get; set; }

        [MaxLength(500)]
        public string Aciklama { get; set; }

        [Required]
        public DateTime BasvuruTarihi { get; set; }

        public Personel Personel { get; set; }
        public ICollection<TayinTalepTercih> Tercihler { get; set; }
    }
}
