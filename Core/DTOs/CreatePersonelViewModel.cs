using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class CreatePersonelViewModel
    {
        [Required(ErrorMessage ="Sicil numarası boş geçilemez")]
        [MaxLength(20)]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "Adı boş geçilemez")]
        [MaxLength(50)]
        public string Adi { get; set; }

        [Required(ErrorMessage = "Soyadı boş geçilemez")]
        [MaxLength(50)]
        public string Soyadi { get; set; }

        [Required(ErrorMessage = "unvanı boş geçilemez")]
        [MaxLength(50)]
        public string Unvan { get; set; }

        [Required(ErrorMessage = "şifre boş geçilemez")]
        [DataType(DataType.Password)]
        public string Sifre { get; set; }

        public bool IsAdmin { get; set; } = false;
    }
}
