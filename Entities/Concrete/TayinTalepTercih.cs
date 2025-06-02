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
    public class TayinTalepTercih : IEntity
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("TayinTalep")]
        public int TayinTalepId { get; set; }

        [Required]
        [ForeignKey("Adliye")]
        public int AdliyeId { get; set; }

        [Required]
        [Range(1, 10)]
        public int Sira { get; set; }

        public TayinTalep TayinTalep { get; set; }
        public Adliye Adliye { get; set; }
    }
}
