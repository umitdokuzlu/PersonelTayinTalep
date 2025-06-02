using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DTOs
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "Sicil numarası zorunludur.")]
        public string SicilNo { get; set; }

        [Required(ErrorMessage = "Şifre zorunludur.")]
        public string Sifre { get; set; }
    }
}
