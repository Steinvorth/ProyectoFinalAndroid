using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Carrito")]
    public class Carrito : BaseModel
    {
        [PrimaryKey("id_carrito")]
        public int Id { get; set; }

        [Column("fecha_creacion")]
        public string Fecha_Creacion { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("id_cliente")]
        public string Cliente { get; set; }
    }
}
