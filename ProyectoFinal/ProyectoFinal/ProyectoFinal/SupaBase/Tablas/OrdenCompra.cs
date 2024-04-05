using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("OrdenCompra")]
    public class OrdenCompra : BaseModel
    {
        [PrimaryKey("id_orden")]
        public int Id { get; set; }

        [Column("id_cliente")]
        public int Id_Cliente { get; set; }

        [Column("estado")]
        public string Estado { get; set; }

        [Column("total")]
        public float Total { get; set; }

        [Column("fecha_creacion")]
        public string Fecha_Creacion { get; set; }
    }
}
