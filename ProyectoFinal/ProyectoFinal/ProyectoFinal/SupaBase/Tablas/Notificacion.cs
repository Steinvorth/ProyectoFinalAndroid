using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Notificacion")]
    public class Notificacion : BaseModel
    {
        [PrimaryKey("id_notificacion")]
        public long Id { get; set; } // Assuming int8 in the database corresponds to long in C#

        [Column("titulo")]
        public string Titulo { get; set; }

        [Column("cuerpo_mensaje")]
        public string Cuerpo_Mensaje { get; set; }

        [Column("fecha_creacion")]
        public string Fecha_Creacion { get; set; }

    }
}
