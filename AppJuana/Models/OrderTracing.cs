using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJuana.Models
{
    public  class OrderTracing
    {
        public string Grupo { get; set; } = string.Empty;
        public string zona { get; set; } = string.Empty;
        public decimal Abiertos { get; set; }
        public decimal Facturados { get; set; }
        public decimal Libres { get; set; }
        public decimal Inactivas { get; set; }
        public decimal Pase { get; set; }
        public decimal Picking { get; set; } 
        public decimal Transportes { get; set; }
        public decimal total { get; set; }
    }
}
