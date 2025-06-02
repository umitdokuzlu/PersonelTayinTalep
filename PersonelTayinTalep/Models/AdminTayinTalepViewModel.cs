using System.Collections.Generic;
using System;

namespace PersonelTayinTalep.Models
{
    public class AdminTayinTalepViewModel
    {
        public int TalepId { get; set; }
        public string AdSoyad { get; set; }
        public string SicilNo { get; set; }
        public string Unvan { get; set; }
        public string TalepTuru { get; set; }
        public string Aciklama { get; set; }
        public DateTime BasvuruTarihi { get; set; }
        public List<string> Tercihler { get; set; }
    }
}
