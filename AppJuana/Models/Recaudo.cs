using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJuana.Models
{
    public class Recaudo
    {
        public string Banco { get; set; } = string.Empty;
        public decimal Valor { get; set; }
        public decimal Comision { get; set; }
        public decimal Iva { get; set; }

        public decimal Retencion { get; set; }

        public decimal ConsignacionNeta { get; set; }
        public decimal consinaGlobal { get; set; } 

        public decimal Participacion { get; set; }
    }
}


