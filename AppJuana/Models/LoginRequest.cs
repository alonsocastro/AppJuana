using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppJuana.Models
{
    public class LoginRequest
    {
        //[Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
        //[StringLength(50, ErrorMessage = "El nombre de usuario no puede tener más de 50 caracteres.")]
        public string usuario { get; set; } = string.Empty;

        //[Required(ErrorMessage = "La contraseña es obligatoria.")]
        public string clave { get; set; } = string.Empty;

        
        public string tipo { get; set; } = string.Empty;
    }
}
