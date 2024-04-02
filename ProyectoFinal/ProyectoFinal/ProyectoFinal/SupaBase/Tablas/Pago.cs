using Postgrest.Attributes;
using Postgrest.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProyectoFinal.SupaBase.Tablas
{
    [Table("Pago")]
    public class Pago : BaseModel
    {
        [PrimaryKey("id_pago")]
        public long Id { get; set; } // Assuming int8 in the database corresponds to long in C#

        [Column("id_orden")]
        public int Id_Orden { get; set; }

        [Column("metodo_pago")]
        public string Metodo_Pago { get; set; }

        [Column("monto")]
        public float Monto { get; set; }

        [Column("fecha_pago")]
        public string Fecha_Pago { get; set; }
    }
}
