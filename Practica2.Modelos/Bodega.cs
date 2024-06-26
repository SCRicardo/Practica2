﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Practica2.Modelos
{
    public class Bodega
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage="El campo Nombre es requerido")]
        [MaxLength(60,ErrorMessage ="El campo Nombre no es mas de 60 caracteres")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El campo Descripcion es requerido")]
        [MaxLength(60, ErrorMessage = "El campo Descripcion no es mas de 60 caracteres")]
        public string Descripcion { get; set; }
        [Required(ErrorMessage = "El campo Nombre es requerido")]
        public bool Estado { get; set; }
    }
}
